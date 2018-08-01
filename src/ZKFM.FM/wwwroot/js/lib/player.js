﻿define(['jquery', 'logger'], function ($, logger) {
    audio = document.getElementById("music");
    function Lyc() {
        console.log("lyc")
    }
    //歌词
    var lrcMove = function () {
        //if (lrc_index.length > 0) {
        //    var time = parseInt(audio.currentTime);
        //    //console.log(time);
        //    var i = 0;
        //    for (var j = 0; j < lrc_index.length; j++) {
        //        if (lrc_index[j].time == time) {
        //            i = lrc_index[j].index;
        //            break;
        //        }
        //    }
        //    if (i != 0) {
        //        var y = i * 30;
        //        // console.log("line:" + i);
        //        $("#lrc-lines p").removeClass("lrc-current").eq(i - 1).addClass("lrc-current");
        //        $("#lrc-lines").animate({ marginTop: (30 - y) + "px" });
        //    }
        //}
    };
    //进度条
    var updateProgress = function () {
        if (audio.currentTime == audio.duration) {
            clearInterval(timeout);
            ratio = 0;
            $('#wrap .progress .current').css({ 'width': ratio + '%' });
            loadMusic();
        }
        ratio = audio.currentTime / audio.duration * 100;
        $('#wrap .progress .current').css({ 'width': ratio + '%' });
    };
    return {
        Play: function (data) {
            audio.src = data.src;
            audio.play();
            $('.album').addClass('playing');
            $('.start i').addClass('playing').removeClass('fa-play').addClass('fa-pause');
            timeout = setInterval(updateProgress, 1000);
            lrctimeout = setInterval(lrcMove, 1000);
            isPlaying = true;
            $('#pic').css("animationPlayState", "running");
        },
        Pause: function () {
            audio.pause();
            $('.album').removeClass('playing');
            $('.start i').removeClass('playing').removeClass('fa-pause').addClass('fa-play');
            clearInterval(timeout);
            clearInterval(lrctimeout);
            isPlaying = false;
            $('#pic').css("animationPlayState", "paused");
        },
        GetLoop: function () {
            return audio.loop;
        },
        SetLoop: function (isloop) {
            audio.loop = isloop;
        }
    }
});