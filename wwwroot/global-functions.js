window.clickResults = {};

window.attachExclusiveListener = function (id, callback) {
    const target = document.getElementById(id);
    target.addEventListener("click", (event) => {
        const isTarget = event.target === target;
        console.log("**** isTarget", isTarget)
        window.clickResults[id] = isTarget;
    });
}

window.getClickResult = function (id) {
    console.log("**** clickresults", window.clickResults)
    const wasTarget = window.clickResults[id] === true;
    window.clickResults[id] = undefined;
    return wasTarget;
}

window.debugObject = function (obj) {
    console.log(obj);
}
window.scrollToTop = function () {
    window.scrollTo(0, 0);
}
