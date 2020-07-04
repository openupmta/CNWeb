//scoll menu bar and back to top
var position = $(window).scrollTop();
$(window).scroll(function() {
    var scroll = $(window).scrollTop();
    if(scroll > position && scroll >145 ) {
        $('nav.nav-horizontal').addClass('nav-horizontal-active-1');
        $('section.banner-slider').addClass('section.banner-slider-active');
        $('.nav-horizontal-sublogo').addClass('nav-horizontal-sublogo-active');
        $('.nav-horizontal-logo').addClass('nav-horizontal-logo-active');
        $('.nav-horizontal-container').addClass('nav-horizontal-container-active');
        $('.nav-horizontal-content').addClass('nav-horizontal-content-active');
    } else {
        $('nav.nav-horizontal').removeClass('nav-horizontal-active-1');
        $('nav.nav-horizontal').removeClass('nav-horizontal-active-2');
        $('section.banner-slider').removeClass('section.banner-slider-active');
        $('.nav-horizontal-sublogo').removeClass('nav-horizontal-sublogo-active');
        $('.nav-horizontal-logo').removeClass('nav-horizontal-logo-active');
        $('.nav-horizontal-container').removeClass('nav-horizontal-container-active');
        $('.nav-horizontal-content').removeClass('nav-horizontal-content-active');
    }
    if(scroll > position && scroll >160) {
        $('nav.nav-horizontal').addClass('nav-horizontal-active-2');
    }
    if (scroll>300) {
		$('section.back-to-top').fadeIn('400');
	}else {
		$('section.back-to-top').fadeOut('400');
	}
    position = scroll;
});
$('.back-to-top-button').click(function(event) {
	$('html, body').animate({scrollTop:0},1200);
});
