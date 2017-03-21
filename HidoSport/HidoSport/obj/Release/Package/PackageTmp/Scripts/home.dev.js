$(document).on('ready', function() {
	$(".regular").slick({
	    infinite: true,
	    slidesToShow: 1,
	    slidesToScroll: 1
	});
    $(".regular1").slick({
        infinite: true,
        slidesToShow: 3,
        slidesToScroll: 3
    });
    $(".one").owlCarousel({
        items: 2,
        loop: false,
        margin: 30,
        lazyLoad: true
    });
    $(".two").owlCarousel({
        items: 6,
        loop: false,
        margin: 30,
        lazyLoad: true
    });
	$("img[data-src]").lazyload({
	    load: function () {
	        this.style.opacity = 1;
	    }, threshold: 200
	});
})


