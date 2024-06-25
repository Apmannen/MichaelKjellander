window.debugObject2 = function (obj) {
    console.log(obj);
}
window.scrollToTop = function () {
    //window.scrollTo(0, 0);
}

window.attachDraggable = function(draggableParent) {
    console.log("**** ATTACH2", draggableParent);
    
    let mouseDown = false;
    let startX, scrollLeft;
    const slider = draggableParent; //document.querySelector('.sg-toggle-button-container');

    const startDragging = (e) => {
        mouseDown = true;
        startX = e.pageX - slider.offsetLeft;
        scrollLeft = slider.scrollLeft;
    }

    const stopDragging = (e) => {
        mouseDown = false;
    }

    const move = (e) => {
        e.preventDefault();
        if(!mouseDown) { return; }
        const x = e.pageX - slider.offsetLeft;
        const scroll = x - startX;
        slider.scrollLeft = scrollLeft - scroll;
    }

    slider.addEventListener('mousemove', move, false);
    slider.addEventListener('mousedown', startDragging, false);
    slider.addEventListener('mouseup', stopDragging, false);
    slider.addEventListener('mouseleave', stopDragging, false);
    
    slider.addEventListener('touchmove', move, false);
    slider.addEventListener('touchdown', startDragging, false);
    slider.addEventListener('touchend', stopDragging, false);
    slider.addEventListener('touchcancel', stopDragging, false);
}
