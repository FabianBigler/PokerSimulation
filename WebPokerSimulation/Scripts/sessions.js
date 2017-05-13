$('.progress-bar').each(function (i, obj) {
    //test

});

var testimonialElements = $(".testimonial");
for (var i = 0; i < testimonialElements.length; i++) {
    var element = testimonialElements.eq(i);
    //do something with element
}

function doUpdateProgressbars() {
    var testimonialElements = $(".session");
    for (var i = 0; i < testimonialElements.length; i++) {
        var element = testimonialElements.eq(i);
        //do something with element
    }
    //$.get('ajax/test.html', function (data) {
    //    alert('TEST');        
        
    //});

    setTimeout(doUpdateProgressbars, 5000);
}