var console;
var buildStatus;

/*var videoHTML = '<div id="video-cover"></div><div id="video-container"><section><div id="video-header"><a id="close-video-link" href="#">close</a></div><iframe id="video" src="http://channel9.msdn.com/Shows/Web+Camps+TV/The-Glimpse-Team-on-Channel-9/player?w=512&h=288" frameBorder="0" scrolling="no" ></iframe><div id="video-details">This is a video we recently filmed for channel 9 talking about the release of v1 and how to get stuck in with glimpse.</div></section></div>';*/
var inlineVideoHTML = '<iframe width="443" height="285" src="//www.youtube.com/embed/ybIxZ6TTm_E" frameborder="0" allowfullscreen></iframe>';

function getBuildStatus() {
    $.ajax({
        url:'/home/buildstatus',
        dataType:'json',
        success:function(data, status) {
            $('#build-info, #build-arrow').css('display', 'block');
            if (status === 'success') {
                buildStatus = data.status.toLowerCase() === "success" ? true : false;
                $('#build-tower').addClass(data.status.toLowerCase());
                if (buildStatus) {
                    
                    $("#build-status").empty().html("was successful");
                } else {
                    $("#build-status").empty().html("failed");
                }
                $('#build-id').empty().html("<a href='" + data.link + "'>" + data.id + "</a>");
                $('#build-date').empty().html(data.date + ' at ' + data.time);
            } else {
                $("#build-info").hidden();
            }
        }
    });
}

function getBlogPosts() {
    //live address - /home/glimpseblogposts
    $.getJSON("/home/glimpseblogposts", function(data) {
      $.each(data, function(i,item){
          //console.log(item);
          $('#blog-section .column'+(i+1)+' .innerColumn')
              .empty()
              .append('<h2>' + item.title + '</h2>')
              .append('<p>' + item.summary + '</p>');
      });
     }).fail(function() {
         $('#blog-section .twoColumn').remove()
         $('#blog-section h1').after('<div class="blog-fail">Oops, it looks like we have some gremlins in the system and we can\'t contact the blog at the moment. Sorry about that.</div>');
     });
}

function getTweets() {
    $.getJSON("/home/glimpsetweets",
     function(data) {
     var limit = 3, count = 0;
      $.each(data.statuses, function(i,item){
      count++;
       ct = item.text;
      // include time tweeted - thanks to will
        mytime = item.created_at;
    	var strtime = mytime.replace(/(\+\S+) (.*)/, '$2 $1')
    	var mydate = new Date(Date.parse(strtime)).toLocaleDateString();
    	var mytime = new Date(Date.parse(strtime)).toLocaleTimeString();
    	ct = ct.replace(/http:\/\/\S+/g,  '<a href="$&" target="_blank">$&</a>');
        $("#tweets").append('<div class="tweet"><a href="https://twitter.com/'+item.user.screen_name + '">'+item.user.screen_name + "</a>: "+ct + " <br /><small>(" + mydate + " @ " + mytime + ")</small></div>");
        if(count === limit) return false;
      });
     }).fail(function() {
         $("#tweets").html('It looks like the fail whale has hit us. Head over to twitter and get in touch, we\'re <a href="https://twitter.com/nikmd23">@nikmd23</a> and <a href="https://twitter.com/anthony_vdh">@anthony_vdh</a>');
     });
    
}

/*function loadVideo() {
    $('body').append(videoHTML);
    $('#close-video-link, #video-cover, #video-container').unbind().click(function(e) {
        e.preventDefault();
        $('#video-cover, #video-container').remove();
    });
}*/

function loadInlineVideo() {
    $('.inner-monitor').empty().append(inlineVideoHTML);
}

$().ready(function () {
    //prep the page
    
    $('.hover-point').each(function() {
        //console.log($(this).attr('data-tipsy'));
        $(this).tipsy({gravity:$(this).attr('data-tipsy')});
    });
    
    $('.video-link').click(function(e) {
        e.preventDefault();
        loadInlineVideo();
        _gaq.push(['_trackEvent', 'Video', 'Play', 'Glimpse Introduction']);
    });
    
    $('a.download, #demo>section>a.button').click(function (e) {
        e.preventDefault();
        if ($('#install').css('display') === 'none') {
            //console.log('clicked');
            $(window).scrollTo(0, 1500);
        }
        $('#install').stop().slideToggle();
        _gaq.push(['_trackEvent', 'Download', 'Install', 'Call to action']);
    });
    //call the external stuff last
    if ($('body').hasClass('inVS') == false) {
        getBuildStatus();
        getTweets();
        getBlogPosts()
    }
    
});

