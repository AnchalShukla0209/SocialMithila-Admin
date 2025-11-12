(function () {
    "use strict";

    var myElement1 = document.getElementById('ChatList');
    new SimpleBar(myElement1, { autoHide: true });

    var myElement4 = document.getElementById('ChatBody');
    new SimpleBar(myElement4, { autoHide: true });

    var myElement5 = document.getElementById('ChatMain');
    new SimpleBar(myElement5, { autoHide: true });

})();