// Modern Layout JavaScript - Inspired by Reference Form Design

$(document).ready(function () {
    // Smooth scroll for anchor links
    $('a[href^="#"]').on('click', function (event) {
        var target = $(this.getAttribute('href'));
        if (target.length) {
            event.preventDefault();
            $('html, body').stop().animate({
                scrollTop: target.offset().top - 80
            }, 600, 'swing');
        }
    });

    // Add active class to current nav item
    var currentPath = window.location.pathname;
    $('.nav-link-custom').each(function () {
        var linkPath = $(this).attr('href');
        if (linkPath && currentPath.includes(linkPath) && linkPath !== '/') {
            $(this).addClass('active');
        } else if (linkPath === '/' && currentPath === '/') {
            $(this).addClass('active');
        }
    });

    // Navbar scroll effect
    var navbar = $('.navbar-custom');
    var lastScrollTop = 0;

    $(window).scroll(function () {
        var scrollTop = $(this).scrollTop();

        if (scrollTop > 50) {
            navbar.css({
                'box-shadow': '0 4px 15px rgba(0, 0, 0, 0.15)',
                'padding': '0.75rem 0'
            });
        } else {
            navbar.css({
                'box-shadow': '0 2px 10px rgba(0, 0, 0, 0.1)',
                'padding': '1rem 0'
            });
        }

        lastScrollTop = scrollTop;
    });

    // Navbar collapse animation enhancement
    $('#navbarNav').on('show.bs.collapse', function () {
        $(this).css('animation', 'fadeInUp 0.3s ease');
    });

    // Add hover effect to cards
    $('.card-modern').hover(
        function () {
            $(this).css('transform', 'translateY(-3px)');
        },
        function () {
            $(this).css('transform', 'translateY(0)');
        }
    );

    // Button ripple effect
    $('.btn-primary-custom').on('click', function (e) {
        var ripple = $('<span class="ripple"></span>');
        var x = e.pageX - $(this).offset().left;
        var y = e.pageY - $(this).offset().top;

        ripple.css({
            left: x + 'px',
            top: y + 'px'
        });

        $(this).append(ripple);

        setTimeout(function () {
            ripple.remove();
        }, 600);
    });

    // Add smooth transitions to dynamically loaded content
    $('.content-container').css('opacity', '0').animate({
        opacity: 1
    }, 600);

    // Form input focus effects (if forms are present)
    $('input, textarea, select').on('focus', function () {
        $(this).parent().addClass('input-focused');
    }).on('blur', function () {
        $(this).parent().removeClass('input-focused');
    });
});

// Add ripple effect styles dynamically
var rippleStyle = `
<style>
.ripple {
    position: absolute;
    border-radius: 50%;
    background: rgba(255, 255, 255, 0.6);
    width: 20px;
    height: 20px;
    animation: ripple-animation 0.6s ease-out;
    pointer-events: none;
}

@keyframes ripple-animation {
    from {
        transform: scale(0);
        opacity: 1;
    }
    to {
        transform: scale(20);
        opacity: 0;
    }
}

.input-focused {
    transform: translateY(-2px);
    transition: all 0.3s ease;
}
</style>
`;

$('head').append(rippleStyle);