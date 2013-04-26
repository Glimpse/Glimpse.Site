var console;
var buildStatus;

/*var videoHTML = '<div id="video-cover"></div><div id="video-container"><section><div id="video-header"><a id="close-video-link" href="#">close</a></div><iframe id="video" src="http://channel9.msdn.com/Shows/Web+Camps+TV/The-Glimpse-Team-on-Channel-9/player?w=512&h=288" frameBorder="0" scrolling="no" ></iframe><div id="video-details">This is a video we recently filmed for channel 9 talking about the release of v1 and how to get stuck in with glimpse.</div></section></div>';*/
var inlineVideoHTML = '<iframe id="video" src="http://channel9.msdn.com/Shows/Web+Camps+TV/The-Glimpse-Team-on-Channel-9/player?w=443&h=285" frameBorder="0" scrolling="no" ></iframe>';

function getBuildStatus() {
    $.ajax({
        url:'/tests/teamCity.json',
        dataType:'json',
        success:function(data, status) {
            console.log(data.status);
            if(status === 'success') {
                buildStatus = data.status.toLowerCase() === "success" ? true : false;
                //console.log($(data).find('build:first').attr('id'));
                $('#build-tower').addClass(data.status.toLowerCase());
                if(buildStatus) {
                    $("#build-status").empty().html("was successful");
                } else {
                    $("#build-status").empty().html("failed");
                }
                $('#build-id').empty().html(data.id);
                $('#build-date').empty().html(data.date + ' at ' + data.time);
            }
        }
    });
}

function getTweets() {
    // UPDATE 10-17-2012: change in Twitter API!
    $.getJSON("https://api.twitter.com/1/statuses/user_timeline.json?screen_name=headloose&count=3&callback=?",
     function(data){
      $.each(data, function(i,item){
       ct = item.text;
      // include time tweeted - thanks to will
    	mytime = item.created_at;
    	var strtime = mytime.replace(/(\+\S+) (.*)/, '$2 $1')
    	var mydate = new Date(Date.parse(strtime)).toLocaleDateString();
    	var mytime = new Date(Date.parse(strtime)).toLocaleTimeString();
    	ct = ct.replace(/http:\/\/\S+/g,  '<a href="$&" target="_blank">$&</a>');
        $("#tweets").append('<div class="tweet">'+ct + " <br /><small>(" + mydate + " @ " + mytime + ")</small></div>");
      });
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
    getBuildStatus();
    getTweets();
    $('.hover-point').tipsy();
    
    $('.video-link').click(function(e) {
        e.preventDefault();
        loadInlineVideo();
    });
    
    $('a.download, #demo>section>a.button').click(function (e) {
        e.preventDefault();
        if ($('#install').css('display') === 'none') {
            console.log('clicked');
            $(window).scrollTo(0, 1500);
        }
        $('#install').stop().slideToggle();
        
    });
});

