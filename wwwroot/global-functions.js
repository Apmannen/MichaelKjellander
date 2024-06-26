window.debugObject2 = function (obj) {
    console.log(obj);
}
window.scrollToTop = function () {
    //window.scrollTo(0, 0);
}

window.attachDraggable = function (draggableParent) {
    console.log("**** ATTACH2", draggableParent);

    let mouseDown = false;
    let startX, scrollLeft;
    const slider = draggableParent; //document.querySelector('.sg-toggle-button-container');
    const debugContainer = document.getElementById("debugcontainer");

    
    const getMouseX = (e) => {
        if(e.type.indexOf("touch") === 0) {
            const touch = e.originalEvent.touches[0] || e.originalEvent.changedTouches[0];
            return touch.pageX;
        }
        return e.pageX;
    }
    
    const startDragging = (e) => {
        debugContainer.innerHTML = "START<br>";
        mouseDown = true;
        startX = getMouseX(e) - slider.offsetLeft;
        scrollLeft = slider.scrollLeft;
    }

    const stopDragging = (e) => {
        debugContainer.innerHTML = "STOP<br>";
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
        debugContainer.innerHTML = `MOVE ${mouseX} ${x} ${scroll}`;
        slider.scrollLeft = scrollLeft - scroll;
    }

    slider.addEventListener('mousemove', move, false);
    slider.addEventListener('mousedown', startDragging, false);
    slider.addEventListener('mouseup', stopDragging, false);
    slider.addEventListener('mouseleave', stopDragging, false);

    slider.addEventListener('touchmove', move, false);
    slider.addEventListener('touchstart', startDragging, false);
    slider.addEventListener('touchend', stopDragging, false);
}
