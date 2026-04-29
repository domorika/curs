$(document).ready(function () {

    console.log("✅ jQuery работает! Версия: " + $.fn.jquery);

    var backToTopBtn = $('#backToTop');

    $(window).scroll(function () {
        if ($(this).scrollTop() > 400) {
            backToTopBtn.fadeIn(300);
        } else {
            backToTopBtn.fadeOut(300);
        }
    });

    backToTopBtn.click(function (e) {
        e.preventDefault();
        $('html, body').animate({
            scrollTop: 0
        }, 800);
    });

    $('a[href^="#"]').on('click', function (e) {
        e.preventDefault();
        var target = $(this.getAttribute('href'));
        if (target.length) {
            $('html, body').animate({
                scrollTop: target.offset().top - 80
            }, 800);
        }
    });

    function animateFeatures() {
        $('.features-item').each(function () {
            var elementTop = $(this).offset().top;
            var viewportBottom = $(window).scrollTop() + $(window).height();

            if (elementTop < viewportBottom - 100) {
                $(this).addClass('visible');
            }
        });
    }

    $(window).on('scroll', animateFeatures);
    animateFeatures();
});