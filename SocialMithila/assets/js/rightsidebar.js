// $(function() {
//     'use strict';
    
//     // Date& time
//       var datetime = null,
//       datetime2 = null,
//       date = null;
//       var update = function () {
//           date = moment(new Date())
//           datetime.html(date.format('HH:mm'));
//           datetime2.html(date.format('dddd, MMMM Do YYYY'));
//       };
  
//       $(document).ready(function(){
//           datetime = $('.time h1');
//           datetime2 = $('.time p');
//           update();
//           setInterval(update, 1000);
//       });
      
  
//   });
  

var update = function() {
    let date = moment(new Date());
    datetime.innerHTML = date.format('HH:mm');
    datetime2.innerHTML = date.format('dddd, MMMM Do YYYY');
};
    let datetime = document.querySelector('.time h1');
    let datetime2 = document.querySelector('.time p');
    update();
    setInterval(update, 1000);
  


