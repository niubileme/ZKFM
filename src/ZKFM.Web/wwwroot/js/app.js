﻿requirejs.config({
    baseUrl: 'js/lib',
    paths: {
        app: '../app'
    }
});

requirejs(['jquery', 'canvas', 'app/sub'], function ($, canvas, sub) {

});