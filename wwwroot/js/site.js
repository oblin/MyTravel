(function () {
    var ele = $("#username");
    ele.text("Hebe Tien (SHE)");

    var main = $("#main");
    main.on("mouseenter", function () {
        main.style.backgroundColor = "#888";
    });

    main.on("mouseleave", function () {
        main.style.backgroundColor = "";
    });

    var menuItems = $("ul.menu li a");
    menuItems.on("click", function(){
        var me = $(this);
        alert(me.text());
    })
})();
