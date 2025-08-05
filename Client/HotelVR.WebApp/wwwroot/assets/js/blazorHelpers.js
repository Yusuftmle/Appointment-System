window.blazorHelpers = {
    scrollIntoView: function (id) {
        const element = document.getElementById(id);
        if (element) {
            element.scrollIntoView({
                behavior: 'smooth'
            });
        }
    }
};