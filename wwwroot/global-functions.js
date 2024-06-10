

window.listenToClicksForActivatingTarget = function (targetElement) {
    const rootElement = targetElement.parentElement;
    //console.log("*** listenToClicksForActivatingTarget", rootElement, targetElement);
    const images = rootElement.getElementsByTagName("img");
    console.log("*** images", images);
    for(let image of images) {
        image.addEventListener("click", (event) => {
            event.preventDefault();
            targetElement.setAttribute("data-active", true);
            
            console.log("*** target el", targetElement)
            targetElement.addEventListener("click", (event) => {
                if(event.target === targetElement) {
                    console.log("*** CLICK BG")
                    targetElement.removeAttribute("data-active");
                }
            });
        });
    }
}

/*window.trackClickAndActivateTarget = function (classes, targetElement) {
    console.log("**** classes", classes, "target", targetElement);
    for (let c of classes) {
        const anElement = document.getElementsByClassName(c)[0];
        anElement.addEventListener("click", (event) => {
            targetElement.setAttribute("data-active", true);
        });
    }
}*/

/*window.trackedClickElement = null;
window.attachExclusiveListener = function (target) {
    target.addEventListener("click", (event) => {
        window.trackedClickElement = event.target;
    });
}
window.checkIfTargetHasBeenClicked = function (target) {
    return window.trackedClickElement === target;
}*/

window.debugObject = function (obj) {
    console.log(obj);
}
window.scrollToTop = function () {
    window.scrollTo(0, 0);
}
