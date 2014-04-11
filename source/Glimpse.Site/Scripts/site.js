$.fn.scrolled = function(waitTime, fn) {
    var tag = "scrollTimer";
    this.scroll(function() {
        var self = $(this);
        var timer = self.data(tag);
        if (timer) 
            clearTimeout(timer); 
        timer = setTimeout(function() {
            self.data(tag, null);
            fn();
        }, waitTime);
        self.data(tag, timer);
    });
};

var getTweetsLatest = function () {
        $.getJSON("/api/twitter/latest", function(data) {
            var limit = 2,
                count = 0;

            $.each(data.statuses, function(i, item) {
                var ct = item.text,
                    strtime = item.created_at.replace(/(\+\S+) (.*)/, '$2 $1'),
                    mydate = new Date(Date.parse(strtime)).toLocaleDateString(),
                    mytime = new Date(Date.parse(strtime)).toLocaleTimeString();

                ct = ct.replace(/http:\/\/\S+/g, '<a href="$&" target="_blank">$&</a>');
                $("#tweets").append('<li class="media media_tweet"><a href="https://twitter.com/' + item.user.screen_name + '">@' + item.user.screen_name + "</a>: " + ct + " <br /><small>(" + mydate + " @ " + mytime + ")</small></li>");

                if (count++ === limit) {
                    return false;
                }
            });
        }).fail(function() {
            $("#tweets").append('<li class="media">It looks like the fail whale has hit us. Head over to twitter and get in touch, we\'re <a href="https://twitter.com/nikmd23">@nikmd23</a> and <a href="https://twitter.com/anthony_vdh">@anthony_vdh</a></li>');
        });
    },
    getBuildLatest = function () {
        $.ajax({
            url: '/api/build/status',
            dataType: 'json',
            success: function(data, status) {
                $('.build_details').show();
                if (status === 'success') {
                    var buildStatus = data.status.toLowerCase() === "success" ? true : false;
                    $('.footer_tower-build').addClass(data.status.toLowerCase());
                    
                    if (buildStatus) { 
                        $("#build-status").empty().html("was successful");
                    } else {
                        $("#build-status").empty().html("failed");
                    }
                    
                    $('#build-id').empty().html("<a href='" + data.link + "'>" + data.number + "</a>");
                    $('#build-date').empty().html(data.date + ' at ' + data.time);
                } 
            }
        });
    }, 
    getBlogLastest = function () {
        var format = function(data) {
            return '<div class="col-md-6"><h3>' + data.title + '</h3><p>' + data.summary.replace('[&#8230;]', '<a href="' + data.link + '">[&#8230;]</a>') + '</p></div>';
        };
        $.getJSON("/api/blog/latest", function (data) {
            var first = data[0],
                second = data[1];
            $('.blog_container').show();
            $('.blog_content').html(format(first) + format(second));
        });
    },
    setupCarousel = function() {
        $('#carousel-screenshot').on('slide.bs.carousel', function (test) {
            $('.carousel-screenshot-shots .target.active').removeClass('active');
        
            var target = $(test.relatedTarget).attr('data-target');
            if (target) 
                $('.' + target).addClass('active'); 
        });
         
        var hudCarouselCall = function() {
                setTimeout(function() { hudCarousel(); }, 3000);
            },
            hudCarousel = function() {
                var container = $('.is-container'),
                    current = container.find('.active').removeClass('active').attr('data-target') || '0';
                if (current < 3) {
                    setTimeout(function() {
                        container.find(".target[data-target='" + (parseInt(current) + 1) + "']").addClass('active');
                        hudCarouselCall();
                    }, 1000);
                }
                else
                    hudCarouselCall();
            };

        var carouselTriggered = false, 
            carouselScrollWatch = function() {
                if (!carouselTriggered) { 
                    var min = 403, max = 685,
                        position = ($('h1').offset().top - 100 - $(window).scrollTop()) * -1 * 2,
                        height = (position < 0 ? min : (position > (max - min) ? max : position + min));

                    $('.carousel-screenshot-shots').css('maxHeight', height);

                    if (height == max) {
                        carouselTriggered = true;
                        $('#carousel-screenshot').carousel('cycle').carousel('next');
                        hudCarousel();
                    }
                }
             
                var installNavbar = $('.navbar-install');
                if ($(window).scrollTop() > 300) {
                    navbarTriggered = true;
                    installNavbar.addClass('navbar-install-show');
                }
                else 
                    installNavbar.removeClass('navbar-install-show'); 
            };
    
        $(window).scroll(carouselScrollWatch);
    },
    videoSetup = function() { 
        $('.action-video').click(function() {
            $('body').prepend('<div class="overlay"></div><div class="overlay-modal"><div class="overlay-close">x</div><iframe width="560" height="315" src="//www.youtube.com/embed/ybIxZ6TTm_E" frameborder="0" allowfullscreen></iframe></div>');
            $('.overlay-close').click(function() {
                $('.overlay, .overlay-modal').remove();
            });            
        });
    };

$(function() {
    getTweetsLatest();
    getBuildLatest();
    
    if ($('.page_home').length > 0) {
        getBlogLastest();
        setupCarousel();
        videoSetup();
    }
});

var _gaq = _gaq || [];
_gaq.push(['_setAccount', 'UA-22715154-1']);
_gaq.push(['_trackPageview']);

(function () {
    var ga = document.createElement('script'); ga.type = 'text/javascript'; ga.async = true;
    ga.src = ('https:' == document.location.protocol ? 'https://ssl' : 'http://www') + '.google-analytics.com/ga.js';
    var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(ga, s);
})();