window.addTouchSupport = (dotNetRef) => {
    let startX = 0;
    let endX = 0;
    const container = document.querySelector('.testimonials-container');

    if (container) {
        container.addEventListener('touchstart', (e) => {
            startX = e.touches[0].clientX;
        });

        container.addEventListener('touchend', (e) => {
            endX = e.changedTouches[0].clientX;
            const diff = startX - endX;

            if (Math.abs(diff) > 50) {
                if (diff > 0) {
                    dotNetRef.invokeMethodAsync('OnSwipeLeft');
                } else {
                    dotNetRef.invokeMethodAsync('OnSwipeRight');
                }
            }
        });
    }
};