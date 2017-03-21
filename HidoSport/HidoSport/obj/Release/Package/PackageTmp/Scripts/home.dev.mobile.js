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
        items: 1,
        loop: false,
        margin: 30,
        lazyLoad: true
    });
    $(".two").owlCarousel({
        items: 3,
        loop: false,
        margin: 30,
        lazyLoad: true
    });
	
	//$(".dropdown").hover(            
    //    function() {
    //        $('.dropdown-menu', this).not('.open').stop(true,true).slideDown("10");
    //        $(this).toggleClass('open');        
    //    },
    //    function() {
    //        $('.dropdown-menu', this).not('.open').stop(true, true).slideUp("10");
    //        $(this).toggleClass('open');       
    //    }
    //);
	//$(".dropdown").click(function () {
	//    debugger;
	//    var link = $(this).find('.dropdown-toggle').attr('href');
	//    window.location.href = link;
        
	//})
	$("img[data-src]").lazyload({
	    load: function () {
	        this.style.opacity = 1;
	    }, threshold: 200
	});
})


