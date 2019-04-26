/* Theme scripts initialization */
"use strict";

/* = Sticky Menu */
jQuery(document).scroll(function () {
	var y = jQuery(document).scrollTop(),
	headerheight = jQuery('.header-main').outerHeight(),
	header = jQuery(".fixed-top");
	
	if (y >= headerheight) {
		header.addClass('fixed animation fadeInDown animation-active').delay(2000).fadeIn();
		;
	} else {
		header.removeClass('fixed animation fadeInDown animation-active');
	}
});

function buttonUp(){
	var inputVal = jQuery('.searchbox-input').val();
	inputVal = jQuery.trim(inputVal).length;
	if( inputVal !== 0){
		jQuery('.searchbox-icon').css('display','none');
	} else {
		jQuery('.searchbox-input').val('');
		jQuery('.searchbox-icon').css('display','block');
	}
}

jQuery('document').ready(function($){
	//Back to top link
	$('#back-to-top').on("click",function(e){
		e.preventDefault();
		$('body,html').animate({scrollTop : 0},800);
		return false;
	});
	
	// Fix Media element validation error 
	$('#mediaelement-css, #wp-mediaelement-css').attr('property', 'stylesheet');
	
	// Search box 
	var submitIcon = $('.searchbox-icon');
	var inputBox = $('.searchbox-input');
	var searchBox = $('.searchbox');
	var isOpen = false;
	submitIcon.click(function(){
		if(isOpen == false){
			searchBox.addClass('searchbox-open');
			inputBox.focus();
			isOpen = true;
		} else {
			searchBox.removeClass('searchbox-open');
			inputBox.focusout();
			isOpen = false;
		}
	}); 
	submitIcon.mouseup(function(){
		return false;
	});
	searchBox.mouseup(function(){
		return false;
	});
	
	$(document).mouseup(function(){
		if(isOpen == true){
			$('.searchbox-icon').css('display','block');
			submitIcon.click();
		}
	});
	
	/* - Responsive Caret */
	$('.ddl-switch').on('click', function() {

		var li = $(this).parent();

		if ( li.hasClass('ddl-active') || li.find('.ddl-active').length !== 0 || li.find('.dropdown-menu').is(':visible') ) {
			li.removeClass('ddl-active');
			li.children().find('.ddl-active').removeClass('ddl-active');
			li.children('.dropdown-menu').slideUp();	
		}
		else {
			li.addClass('ddl-active');
			li.children('.dropdown-menu').slideDown();
		}
	});
	
	/* - Author */
	if( $(".bloglist.alternative").length ){	
		setTimeout(function() {
			var $container = $(".bloglist.alternative .row");
			$container.isotope({
				itemSelector: 'article',
				gutter: 0,
				transitionDuration: "0.5s"
			});
		},3000);
	}
	
	if( $("#author-grid").length ){
		var $grid = $('#author-grid').imagesLoaded( function() {
			$grid.isotope({
				itemSelector: 'article.hentry',
				transitionDuration: "0.5s",
			});
		});
	}
	
	if( $("#grid-view,#list-view,#single-gallery").length ) {
		setTimeout(function() {
			var $container = $("#grid-view,#list-view,#single-gallery");
			$container.isotope({
				itemSelector: '.grid-box, .list-box, .gallery-box',
				gutter: 0,
				transitionDuration: "0.5s"
			});
		},3000);
	}
	
	/* - Special */
	$(".popular-latest-posts li").first().addClass('active');
	$(".popular-latest-posts .tab-pane").first().addClass('active in');
	
	
	$('.comments-area').each(function() {
		var $this = $(this);
		if($this.html().replace(/\s|&nbsp;/g, '').length == 0){
			$this.remove();
		}
	});
              
	
});
/* ## Window Load - Handler for .load() called */
jQuery(window).load(function() {

	/* - Site Loader */
	if ( !jQuery('html').is('.ie6, .ie7, .ie8') ) {
		jQuery("#site-loader").imagesLoaded().fadeOut("slow");
	}
	else {
		jQuery("#site-loader").css('display','none');
	}
});

// Infinite scrolling functionality
jQuery(function ($) {
   "use strict";
   if ($("#ajaxloop").length) {
      var page = 1,
         $loading = true,
         $finished = false,
         $window = $(window),
         $el = $('#ajaxloop'),
         $content = $('#ajaxloop div.blog-box'),
         $path = $content.attr('data-path'),
	 $load = $content.attr('data-load-text');
      if ($path === undefined) {
         $path = '/wp-content/themes/viral-news';
      }
      $el.append('<div class="col-md-12"><span class="loader-ajax">' + $load + '</span></div>');
      $('.loader-ajax').hide();
      //Load posts function
      var load_posts = function () {
	    $('.loader-ajax').addClass('loading');
            $('.loader-ajax').text("Loading...");
            $.ajax({
               type: "GET",
               data: {
                  postType: $content.attr('data-post-type'),
                  category: $content.attr('data-category'),
                  author: $content.attr('data-author'),
                  taxonomy: $content.attr('data-taxonomy'),
                  tag: $content.attr('data-tag'),
                  postNotIn: $content.attr('data-post-not-in'),
                  numPosts: $content.attr('data-display-posts'),
		  archivedate: $content.attr('data-date'),
		  searchterm: $content.attr('data-search'),
                  pageNumber: page,
               },
               dataType: "html",
               url: $path + "/ajaxloop.php",
	       beforeSend: function () {
                  if (page != 1) {
                     $('.loader-ajax').show();
                  }
               },
               success: function (data) {
			var $data = $(data); // Convert data to an object   
                  //alert(data);           
                 if (data.length > 1) {
                    $data.hide();
                    $content.append($data);
		    $data.fadeIn(500, function () {
			$('.loader-ajax').removeClass('loading');
				$('.loader-ajax').text('');
			$loading = false;
		    });
                  } else {
		      $('.loader-ajax').removeClass('loading').addClass('done');
                     $('.loader-ajax').text('All Posts Loaded').delay(2000).hide();
                     $loading = false;
                     $finished = true;
                  }
		
               },
               error: function (jqXHR, textStatus, errorThrown) {
                 //$("#temp_load").remove();
                  alert(jqXHR + " :: " + textStatus + " :: " + errorThrown);
               }
            });
         }
      $window.scroll(function() {
        var content_offset = $content.offset();
       // console.log(content_offset.top);
        if(!$loading && !$finished && ($window.scrollTop() +
            $window.height()) > ($content.scrollTop() +
            $content.height() + content_offset.top)) {
                $loading = true;
                page++;
                load_posts();
        }
	});
      load_posts();
   }
});