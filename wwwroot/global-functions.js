

window.trackClickAndActivateTarget = function (classes, targetElement) {
    console.log("**** classes", classes, "target", targetElement);
    for (let c of classes) {
        const anElement = document.getElementsByClassName(c)[0];
        anElement.addEventListener("click", (event) => {
            targetElement.setAttribute("data-active", true);
        });
    }
}

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
