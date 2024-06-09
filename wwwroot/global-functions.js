window.clickResults = {};

window.attachExclusiveListener = function (id) {
    //console.log("*** attachExclusiveListener", id)
    const target = document.getElementById(id);
    //console.log("*** attach to target", target)
    target.addEventListener("click", (event) => {
        const isTarget = event.target === target;
        //console.log("**** isTarget", isTarget)
        window.clickResults[id] = isTarget;
    });
}

window.getClickResult = function (id) {
    //console.log("**** clickresults", window.clickResults)
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
