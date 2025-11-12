(function () {
  "use strict";

  let checkAll = document.querySelector('.check-all');
  checkAll.addEventListener('click', checkAllFn)

  let check = document.querySelectorAll('.main-mail-item .main-mail-checkbox input')
  check.forEach(function (e) {
    e.addEventListener("click", checkSelected)

  })
  function checkAllFn() {
    if (this.checked) {
      document.querySelectorAll('.main-mail-checkbox input').forEach(function (e) {
        document.querySelectorAll('.main-mail-options .btn:not(:first-child)').forEach(function (btn) {
          btn.classList.remove('disabled');
        });
        e.closest('.mail-list').classList.add('selected');
        e.checked = true;
      });
    }
    else {
      document.querySelectorAll('.main-mail-checkbox input').forEach(function (e) {
        document.querySelectorAll('.main-mail-options .btn:not(:first-child)').forEach(function (btn) {
          btn.classList.add('disabled');
        });
        e.closest('.mail-list').classList.remove('selected');
        e.checked = false;
      });
    }
  }
  function checkSelected() {
    let status = false;
    let checkedBoxs = 0;
    let checkBtns = document.querySelectorAll('.main-mail-checkbox input');
    checkBtns.forEach(el => {
      if (el.checked) {
        status = true;
        checkedBoxs++;
      } else {
        checkAll.checked = false;
      }
    })
    if (status) {
      document.querySelectorAll('.main-mail-options .btn:not(:first-child)').forEach(function (btn) {
        btn.classList.remove('disabled');
      });
    }
    else {
      document.querySelectorAll('.main-mail-options .btn:not(:first-child)').forEach(function (btn) {
        btn.classList.add('disabled');
        checkAll.checked = false;
      });
    }
    if (document.querySelectorAll('.main-mail-checkbox input')?.length == checkedBoxs) {
      checkAll.checked = true;
    }
  }
})();

// document.querySelector('.main-mail-item .main-mail-checkbox input').on('click', function() {
//   if (this.checked) {
//     document.querySelectorAll('.main-mail-options .btn:not(:first-child)').forEach(function(btn) {
//       btn.classList.remove('disabled');
//     });
//   } else {
//     this.checked = true;
//     document.querySelectorAll('.main-mail-options .btn:not(:first-child)').forEach(function(btn) {
//       btn.classList.add('disabled');
//     });
//   }
// });