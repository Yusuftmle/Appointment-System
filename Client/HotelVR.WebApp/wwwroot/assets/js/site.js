// Blog sayfası için JavaScript yardımcı fonksiyonları

// Sayfa meta bilgilerini ayarla
window.setPageMeta = function (title, description) {
    document.title = title;

    // Meta description
    let metaDescription = document.querySelector('meta[name="description"]');
    if (metaDescription) {
        metaDescription.setAttribute('content', description);
    } else {
        metaDescription = document.createElement('meta');
        metaDescription.name = 'description';
        metaDescription.content = description;
        document.getElementsByTagName('head')[0].appendChild(metaDescription);
    }

    // Open Graph meta tags
    setMetaProperty('og:title', title);
    setMetaProperty('og:description', description);
    setMetaProperty('og:url', window.location.href);
    setMetaProperty('og:type', 'article');
};

// Meta property ayarla
function setMetaProperty(property, content) {
    let meta = document.querySelector(`meta[property="${property}"]`);
    if (meta) {
        meta.setAttribute('content', content);
    } else {
        meta = document.createElement('meta');
        meta.setAttribute('property', property);
        meta.setAttribute('content', content);
        document.getElementsByTagName('head')[0].appendChild(meta);
    }
}

// Başlıklara ID ekle
window.addIdToHeading = function (index, id) {
    const headings = document.querySelectorAll('.blog-content h2, .blog-content h3, .blog-content h4');
    if (headings[index]) {
        headings[index].id = id;
        headings[index].style.scrollMarginTop = '100px';
    }
};

// Elemana scroll et
window.scrollToElement = function (elementId) {
    const element = document.getElementById(elementId);
    if (element) {
        element.scrollIntoView({
            behavior: 'smooth',
            block: 'start'
        });

        // TOC linklerini güncelle
        updateTocActiveLink(elementId);
    }
};

// TOC aktif link güncelle
function updateTocActiveLink(activeId) {
    // Tüm TOC linklerinden active class'ını kaldır
    document.querySelectorAll('.toc-link').forEach(link => {
        link.classList.remove('active');
    });

    // Aktif linke active class ekle
    const activeLink = document.querySelector(`a[href="#${activeId}"]`);
    if (activeLink) {
        activeLink.classList.add('active');
    }
}

// Scroll olayını dinle ve TOC'u güncelle
let ticking = false;

function updateTocOnScroll() {
    if (!ticking) {
        requestAnimationFrame(() => {
            const headings = document.querySelectorAll('.blog-content h2[id], .blog-content h3[id], .blog-content h4[id]');
            let activeHeading = null;

            headings.forEach(heading => {
                const rect = heading.getBoundingClientRect();
                if (rect.top <= 150) {
                    activeHeading = heading;
                }
            });

            if (activeHeading) {
                updateTocActiveLink(activeHeading.id);
            }

            ticking = false;
        });
        ticking = true;
    }
}

// Sayfa yüklendiğinde scroll listener ekle
document.addEventListener('DOMContentLoaded', function () {
    window.addEventListener('scroll', updateTocOnScroll);
});

// Smooth scroll polyfill for older browsers
if (!('scrollBehavior' in document.documentElement.style)) {
    window.scrollToElement = function (elementId) {
        const element = document.getElementById(elementId);
        if (element) {
            const offsetTop = element.offsetTop - 100;
            window.scrollTo({
                top: offsetTop,
                behavior: 'smooth'
            });
        }
    };
}

// Link kopyalama fonksiyonu (fallback)
window.copyToClipboard = async function (text) {
    if (navigator.clipboard && navigator.clipboard.writeText) {
        try {
            await navigator.clipboard.writeText(text);
            return true;
        } catch (err) {
            console.error('Clipboard API failed:', err);
        }
    }

    // Fallback method
    const textArea = document.createElement('textarea');
    textArea.value = text;
    textArea.style.position = 'fixed';
    textArea.style.left = '-999999px';
    textArea.style.top = '-999999px';
    document.body.appendChild(textArea);
    textArea.focus();
    textArea.select();

    try {
        document.execCommand('copy');
        textArea.remove();
        return true;
    } catch (err) {
        console.error('Fallback copy failed:', err);
        textArea.remove();
        return false;
    }
};

// Scroll to top fonksiyonu
function scrollToTop() {
    window.scrollTo({
        top: 0,
        behavior: 'smooth'
    });
}

// Scroll to top button'ının görünürlüğünü kontrol et
function initScrollTop() {
    const scrollTopBtn = document.getElementById('scroll-top');
    if (!scrollTopBtn) return;

    // Scroll event listener'ı ekle
    function handleScroll() {
        if (window.pageYOffset > 100) {
            scrollTopBtn.classList.add('active');
        } else {
            scrollTopBtn.classList.remove('active');
        }
    }

    // Event listener'ı ekle
    window.addEventListener('scroll', handleScroll);

    // Click event'i için scroll fonksiyonunu ekle
    scrollTopBtn.addEventListener('click', function (e) {
        e.preventDefault();
        scrollToTop();
    });
}

// Bootstrap dropdown'ları başlat
function initDropdowns() {
    // Bootstrap'in yüklü olup olmadığını kontrol et
    if (typeof bootstrap === 'undefined') {
        console.warn('Bootstrap JavaScript library is not loaded');
        return;
    }

    var dropdownElementList = [].slice.call(document.querySelectorAll('[data-bs-toggle="dropdown"]'));
    var dropdownList = dropdownElementList.map(function (dropdownToggleEl) {
        return new bootstrap.Dropdown(dropdownToggleEl);
    });
}

// Manuel dropdown toggle fonksiyonu (Bootstrap alternatifi)
function toggleDropdown(buttonId) {
    const button = document.getElementById(buttonId);
    if (!button) {
        console.error('Button not found:', buttonId);
        return;
    }

    const dropdown = button.nextElementSibling;
    if (!dropdown) {
        console.error('Dropdown menu not found');
        return;
    }

    const isOpen = dropdown.classList.contains('show');

    // Önce tüm açık dropdown'ları kapat
    closeAllDropdowns();

    if (!isOpen) {
        // Bu dropdown'ı aç
        dropdown.classList.add('show');
        button.setAttribute('aria-expanded', 'true');
    }
}

// Tüm dropdown'ları kapat
function closeAllDropdowns() {
    document.querySelectorAll('.dropdown-menu.show').forEach(el => {
        el.classList.remove('show');
        const button = el.previousElementSibling;
        if (button) {
            button.setAttribute('aria-expanded', 'false');
        }
    });
}

// Dropdown'ları dışarı tıklayınca kapat
function initDropdownClickOutside() {
    document.addEventListener('click', function (event) {
        // Eğer tıklanan element dropdown wrapper'ın içinde değilse
        if (!event.target.closest('.user-menu-wrapper')) {
            closeAllDropdowns();
        }
    });
}

// Mobile nav toggle
function initMobileNav() {
    const navToggle = document.querySelector('.mobile-nav-toggle');
    const navMenu = document.getElementById('navmenu');

    if (navToggle && navMenu) {
        navToggle.addEventListener('click', function () {
            navMenu.classList.toggle('navmenu-active');
            this.classList.toggle('bi-x');
        });
    }
}

// Preloader'ı gizle
function hidePreloader() {
    const preloader = document.getElementById('preloader');
    if (preloader) {
        preloader.style.display = 'none';
    }
}

// Tüm fonksiyonları başlat
function initializeAll() {
    initMobileNav();
    hidePreloader();
    initDropdowns();
    initScrollTop();
    initDropdownClickOutside();
}

// DOM yüklendiğinde çalıştır
if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', initializeAll);
} else {
    initializeAll();
}