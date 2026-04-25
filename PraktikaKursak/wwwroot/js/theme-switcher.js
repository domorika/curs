
(function () {
    function applyTheme(theme) {
        const themeLink = document.getElementById('theme-stylesheet');

        if (theme === 'dark') {
            themeLink.href = '/css/dark-theme.css';
            localStorage.setItem('theme', 'dark');
        } else {
            themeLink.href = '/css/light-theme.css';
            localStorage.setItem('theme', 'light');
        }

        const lightBtn = document.querySelector('.theme-btn[onclick*="light"]');
        const darkBtn = document.querySelector('.theme-btn[onclick*="dark"]');

        if (lightBtn && darkBtn) {
            if (theme === 'light') {
                lightBtn.style.opacity = '1';
                darkBtn.style.opacity = '0.5';
            } else {
                lightBtn.style.opacity = '0.5';
                darkBtn.style.opacity = '1';
            }
        }
    }

    const savedTheme = localStorage.getItem('theme');

    if (savedTheme === 'dark') {
        applyTheme('dark');
    } else if (savedTheme === 'light') {
        applyTheme('light');
    } else {
        applyTheme('light');
    }

    window.switchTheme = function (theme) {
        applyTheme(theme);
    };
})();