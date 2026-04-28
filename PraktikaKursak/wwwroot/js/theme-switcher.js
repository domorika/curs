// Переключение тем
(function () {
    // Функция применения темы
    function applyTheme(theme) {
        const themeLink = document.getElementById('theme-stylesheet');

        if (!themeLink) {
            console.log("Элемент #theme-stylesheet не найден");
            return;
        }

        if (theme === 'dark') {
            themeLink.href = '/css/dark-theme.css';
            localStorage.setItem('theme', 'dark');
            console.log("Тёмная тема включена");
        } else {
            themeLink.href = '/css/light-theme.css';
            localStorage.setItem('theme', 'light');
            console.log("Светлая тема включена");
        }

        // Обновляем активное состояние кнопок
        const lightBtn = document.getElementById('theme-light-btn');
        const darkBtn = document.getElementById('theme-dark-btn');

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

    // Загружаем сохранённую тему
    const savedTheme = localStorage.getItem('theme');

    if (savedTheme === 'dark') {
        applyTheme('dark');
    } else if (savedTheme === 'light') {
        applyTheme('light');
    } else {
        // По умолчанию светлая тема
        applyTheme('light');
    }

    // Делаем функцию глобальной
    window.switchTheme = function (theme) {
        applyTheme(theme);
    };
})();