(function () { 
    window.addEventListener("load", function () {
        setTimeout(function () {
            // Section 01 - Set url link 
            var logo = document.getElementsByClassName('link');
            logo[0].href = "http://www.sample.com.tr/";
            logo[0].target = "_blank"; 
 
            // Section 02 - Set logo
            logo[0].children[0].alt = "HepsiYemek";
            logo[0].children[0].src = "/swagger/sample_logo.png"; 

             // Section 03 - Set Favicon
            var link = document.querySelector("link[rel*='icon']") || document.createElement('link');
            link.type = 'image/x-icon';
            link.rel = 'shortcut icon';
            link.href = '/swagger/favicon.png';
            document.getElementsByTagName('head')[0].appendChild(link);

             // Section 04 - Set Title
            document.title = 'Order API';
		          
        });
    });
})();




