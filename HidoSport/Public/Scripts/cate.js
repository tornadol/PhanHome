$(function () {
    $('.add-active').removeClass('active');
    $('.add-active').eq('1').addClass('active');
    $(window).on('scroll', function () {
        debugger;
        $(".menu-left").stick_in_parent();
    });
})