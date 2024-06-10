window.listenToClicksForActivatingTarget = function (targetElement, contentContainer) {
    console.log("*** cont", contentContainer);
    const rootElement = targetElement.parentElement;
    const images = rootElement.getElementsByTagName("img");
    for(let image of images) {
        image.addEventListener("click", (event) => {
            event.preventDefault();
            targetElement.setAttribute("data-active", true);
            targetElement.addEventListener("click", (event) => {
                if(event.target === targetElement) {
                    targetElement.removeAttribute("data-active");
                }
            });
        });
    }
}


window.debugObject = function (obj) {
    console.log(obj);
}
window.scrollToTop = function () {
    window.scrollTo(0, 0);
}
