$(document).ready(function () {
    // Click event for scrolling left
    $('.scroll-left').on('click', function () {
        // Find the corresponding slider for this button
        var $slider = $(this).closest('.catalog-slider').find('.catalog-container');
        $slider.animate({ scrollLeft: '-=550' }, 'slow');
    });

    // Click event for scrolling right
    $('.scroll-right').on('click', function () {
        // Find the corresponding slider for this button
        var $slider = $(this).closest('.catalog-slider').find('.catalog-container');
        $slider.animate({ scrollLeft: '+=550' }, 'slow');
    });
});

