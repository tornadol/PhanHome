$(function(){
	$('.dropdown').hover(function(){
		$(this).find('.subcate').css({"display":"block"});
	}, function(){
		$(this).find('.subcate').css({"display":"none"});
	});
})