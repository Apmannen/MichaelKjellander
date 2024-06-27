window.debugObject2 = function (obj) {
    console.log(obj);
}
window.scrollToTop = function () {
    //window.scrollTo(0, 0);
}

window.attachDraggable = function (draggableParent) {
    let mouseDown = false;
    let startX, scrollLeft;
    const slider = draggableParent;
    //const debugContainer = document.getElementById("debugcontainer");

    const getMouseX = (e) => {
        return e.pageX || e.touches[0].pageX;
    }

    const startDragging = (e) => {
        mouseDown = true;
        startX = getMouseX(e) - slider.offsetLeft;
        scrollLeft = slider.scrollLeft;
    }

    const stopDragging = (e) => {
        mouseDown = false;
    }

    const move = (e) => {
        e.preventDefault();
        if (!mouseDown) {
            return;
        }
        let mouseX = getMouseX(e);
        const x = mouseX - slider.offsetLeft;
        const scroll = x - startX;
        slider.scrollLeft = scrollLeft - scroll;
    }

    //Workaround to avoid activating click while dragging
    const buttonContainers = slider.getElementsByClassName("toggle-button-single-container");
    for (let buttonContainer of buttonContainers) {
        const button = buttonContainer.getElementsByTagName("button")[0];
        const checkBox = buttonContainer.getElementsByTagName("input")[0];
        button.addEventListener("click", function () {
            checkBox.click();
        });
    }

    slider.even

    slider.addEventListener('mousemove', move, false);
    slider.addEventListener('mousedown', startDragging, false);
    slider.addEventListener('mouseup', stopDragging, false);
    slider.addEventListener('mouseleave', stopDragging, false);

    slider.addEventListener('touchmove', move, false);
    slider.addEventListener('touchstart', startDragging, false);
    slider.addEventListener('touchend', stopDragging, false);
    slider.addEventListener('touchcancel', stopDragging, false);
}
