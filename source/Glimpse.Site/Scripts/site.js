// Example Usage
$(function() {
    $('.call-action-trigger').click(function() {
        var element = $('.call-action').toggleClass('call-action-active');
        window.scroll(0, element.offset().top - 80);
    });
});
