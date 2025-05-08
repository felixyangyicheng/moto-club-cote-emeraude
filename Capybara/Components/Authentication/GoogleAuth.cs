

namespace Capybara.Components.Authentication
{
    public partial class GoogleAuth
    {

        [Parameter, NotNull]
        public String ClientId { get; set; } = "";

        /// <summary>
        /// documentation: https://developers.google.com/identity/gsi/web/reference/js-reference?hl=zh-cn#CredentialResponse
        /// </summary>
        /// 
        //[Parameter, NotNull]

        //public string RedirectUri { get; set; } = "";
        [Parameter]
        public bool Hide { get; set; } = false;

        [Parameter, NotNull]
        public Credential UserCredential { get; set; } =new();


        private DotNetObjectReference<GoogleAuth> objRef=default!;




        protected async override Task OnAfterRenderAsync(bool firstRender)
        {


            if (firstRender)
            {
                objRef = DotNetObjectReference.Create(this);


                await JsRuntime.InvokeVoidAsync("gajs.initGoogle", new object[] { objRef, ClientId });
            }



        }

        [JSInvokable]
        public async Task SaveCredentials(string mycredential)
        {

            var handler = new JwtSecurityTokenHandler();
            JwtSecurityToken decodedValue = handler.ReadJwtToken(mycredential);

            UserCredential.Email = (string)decodedValue.Payload["email"];

            UserCredential.UserId = (string)decodedValue.Payload["sub"];

            UserCredential.Name = (string)decodedValue.Payload["given_name"];
            UserCredential.Avatar = (string)decodedValue.Payload["picture"];



            UserCredential.IsLogged = true;

            await UserCredentialChanged.InvokeAsync(UserCredential);

            StateHasChanged();
        }

        //Automatically binded by @bind
        //https://www.syncfusion.com/faq/blazor/components/how-do-you-pass-values-from-child-to-parent-using-eventcallback-in-blazor
        [Parameter]
        public EventCallback<Credential> UserCredentialChanged { get; set; }

        //private Task OnUserCredetialChanged(ChangeEventArgs e)
        //{

        //    return UserCredentialChanged.InvokeAsync(UserCredential);
        //}


        public void Dispose()
        {
            objRef?.Dispose();
        }
    }
}
