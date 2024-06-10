window.listenToClicksForActivatingTarget = function (targetElement, contentContainer) {
    const rootElement = targetElement.parentElement;
    const images = rootElement.getElementsByTagName("img");
    for(let image of images) {
        image.addEventListener("click", (event) => {
            event.preventDefault();
            targetElement.setAttribute("data-active", true);
            const src = image.getAttribute("src");
            const splits = src.split(/-[0-9]+x[0-9]+/);
            let fullSizeSrc = src;
            if(splits.length === 2) {
                fullSizeSrc = splits[0]+splits[1];
            }
            contentContainer.innerHTML = "<img src='"+fullSizeSrc+"'>";
            
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
