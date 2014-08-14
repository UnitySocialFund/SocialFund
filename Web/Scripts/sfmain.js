(function () {
    $(document).ready(function () {
        $('#menu li').hover(function () {
            clearTimeout($.data(this, 'timer'));
            $('#groupList', this).stop(true, true).slideDown(200);
        }, function () {
            $.data(this, 'timer', setTimeout($.proxy(function () {
                $('#groupList', this).stop(true, true).slideUp(200);
            }, this), 100));
        });
    });
})(window);
