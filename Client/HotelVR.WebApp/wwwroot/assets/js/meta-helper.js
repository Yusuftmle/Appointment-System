// meta-helper.js'e ekleyin
window.isSearchBot = function () {
    return /Googlebot|bingbot|facebookexternalhit|Twitterbot/i.test(navigator.userAgent);
};

// Global fonksiyonlar olarak tanımlayın
window.setTitle = function (title) {
    if (title) {
        document.title = title;
    }
};

window.setMetaTag = function (name, content, attribute = 'name') {
    if (!name || !content) return;
    let selector = `meta[${attribute}="${name}"]`;
    let element = document.querySelector(selector);
    if (element) {
        element.setAttribute("content", content);
    } else {
        element = document.createElement("meta");
        element.setAttribute(attribute, name);
        element.setAttribute("content", content);
        document.head.appendChild(element);
    }
};

// Missing function - add this to fix the error
window.setMetaTagHttpEquiv = function (httpEquiv, content) {
    if (!httpEquiv || !content) return;
    let selector = `meta[http-equiv="${httpEquiv}"]`;
    let element = document.querySelector(selector);
    if (element) {
        element.setAttribute("content", content);
    } else {
        element = document.createElement("meta");
        element.setAttribute("http-equiv", httpEquiv);
        element.setAttribute("content", content);
        document.head.appendChild(element);
    }
};

window.setCanonical = function (url) {
    if (!url) return;
    let element = document.querySelector('link[rel="canonical"]');
    if (element) {
        element.setAttribute("href", url);
    } else {
        element = document.createElement("link");
        element.setAttribute("rel", "canonical");
        element.setAttribute("href", url);
        document.head.appendChild(element);
    }
};

window.addPageSchema = function (schemaJson) {
    if (!schemaJson) return;
    let existingSchema = document.querySelector('script[type="application/ld+json"]');
    if (existingSchema) {
        existingSchema.remove();
    }
    let script = document.createElement('script');
    script.type = 'application/ld+json';
    script.textContent = schemaJson;
    document.head.appendChild(script);
};
// wwwroot/assets/js/meta-helper.js
window.setCriticalMeta = function () {
    const path = window.location.pathname;
    const criticalMeta = {
        "/randevu": {
            title: "Randevu Al | Meridyen Psikoloji",
            canonical: "https://www.meridyen-psikoloji.com/randevu"
        }
    };
    const meta = criticalMeta[path] || {};
    if (meta.title) document.title = meta.title;
    if (meta.canonical) {
        let link = document.querySelector('link[rel="canonical"]') || document.createElement('link');
        link.rel = 'canonical';
        link.href = meta.canonical;
        document.head.appendChild(link);
    }
};
