// 获取元素文本内容的辅助函数
window.getElementText = function (elementId) {
    const element = document.getElementById(elementId);
    return element ? element.innerText || '0' : '0';
};