window.trackedClickElement = null;

window.attachExclusiveListener = function (target) {
    target.addEventListener("click", (event) => {
        window.trackedClickElement = event.target;
    });
}
window.checkIfTargetHasBeenClicked = function (target) {
    return window.trackedClickElement === target;
}

window.debugObject = function (obj) {
    console.log(obj);
}
window.scrollToTop = function () {
    window.scrollTo(0, 0);
}
