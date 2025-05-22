using System.Text.Json.Serialization;

namespace Capybara.Models.GraphQL
{
    public class MediaAsset
    {
        public string Path { get; set; } = "";
        public DateTime LastModifiedUtc { get; set; }

        // 新增计算属性
        [JsonIgnore] // 避免序列化此字段
        public string FullUrl => $"{Path}";
    }
}
