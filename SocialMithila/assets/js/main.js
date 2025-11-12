(function () {
  "use strict";

  if (localStorage.getItem("aziradarktheme")) {
    document.querySelector("html").setAttribute("data-theme-mode", "dark");
    document.querySelector("html").setAttribute("data-menu-styles", "dark");
    document
      .querySelector("html")
      .setAttribute("data-header-styles", "gradient");
  }
  if (localStorage.azirartl) {
    let html = document.querySelector("html");
    html.setAttribute("dir", "rtl");
    document
      .querySelector("#style")
      ?.setAttribute(
        "href",
        "../assets/libs/bootstrap/css/bootstrap.rtl.min.css"
      );
  }
  if (localStorage.aziralayout) {
    let html = document.querySelector("html");
    html.setAttribute("data-nav-layout", "horizontal");
    document.querySelector("html").setAttribute("data-menu-styles", "light");
  }
  if (localStorage.getItem("aziralayout") == "horizontal") {
    document
      .querySelector("html")
      .setAttribute("data-nav-layout", "horizontal");
  }

  if (localStorage.loaderEnable == "true") {
    document.querySelector("html").setAttribute("loader", "enable");
  } else {
    if (!document.querySelector("html").getAttribute("loader")) {
      document.querySelector("html").setAttribute("loader", "disable");
    }
  }

  function localStorageBackup() {
    // if there is a value stored, update color picker and background color
    // Used to retrive the data from local storage
    if (localStorage.primaryRGB) {
      if (document.querySelector(".theme-container-primary")) {
        document.querySelector(".theme-container-primary").value =
          localStorage.primaryRGB;
      }
      document.querySelector("html").style.setProperty("--primary-rgb", localStorage.primaryRGB);
    }
    if (localStorage.bodyBgRGB && localStorage.bodylightRGB) {
      if (document.querySelector(".theme-container-background")) {
        document.querySelector(".theme-container-background").value =
          localStorage.bodyBgRGB;
      }
      document.querySelector("html").style.setProperty("--body-bg-rgb", localStorage.bodyBgRGB);
      document.querySelector("html").style.setProperty("--body-bg-rgb2", localStorage.bodyBgRGB2);
      document.querySelector("html").style.setProperty("--light-rgb", localStorage.bodylightRGB);
      document.querySelector("html").style.setProperty("--form-control-bg", `rgb(${localStorage.bodylightRGB})`);
      document.querySelector("html").style.setProperty("--input-border", "rgba(255,255,255,0.1)");
      let html = document.querySelector("html");
      html.setAttribute("data-theme-mode", "dark");
      html.setAttribute("data-menu-styles", "dark");
      html.setAttribute("data-header-styles", "gradient");
    }
    if (localStorage.aziradarktheme) {
      let html = document.querySelector("html");
      html.setAttribute("data-theme-mode", "dark");
    }
    if (localStorage.aziralayout) {
      let html = document.querySelector("html");
      let layoutValue = localStorage.getItem("aziralayout");
      html.setAttribute("data-nav-layout", "horizontal");
      setTimeout(() => {
        clearNavDropdown();
      }, 5000);
      html.setAttribute("data-nav-style", "menu-click");
      setTimeout(() => {
        checkHoriMenu();
      }, 5000);
    }
    if (localStorage.aziraverticalstyles) {
      let html = document.querySelector("html");
      let verticalStyles = localStorage.getItem("aziraverticalstyles");

      if (verticalStyles == "default") {
        html.setAttribute("data-vertical-style", "default");
        localStorage.removeItem("aziranavstyles");
      }

      if (verticalStyles == "closed") {
        html.setAttribute("data-vertical-style", "closed");
        localStorage.removeItem("aziranavstyles");
      }
      if (verticalStyles == "icontext") {
        html.setAttribute("data-vertical-style", "icontext");
        localStorage.removeItem("aziranavstyles");
      }
      if (verticalStyles == "overlay") {
        html.setAttribute("data-vertical-style", "overlay");
        localStorage.removeItem("aziranavstyles");
      }
      if (verticalStyles == "detached") {
        html.setAttribute("data-vertical-style", "detached");
        html.setAttribute("data-header-position", "fixed");
        localStorage.removeItem("aziranavstyles");
      } 
      
      if (verticalStyles == 'doublemenu') {
        html.setAttribute('data-vertical-style', 'doublemenu');
        localStorage.removeItem("aziranavstyles")
        setTimeout(() => {

            const menuSlideItem = document.querySelectorAll(".main-menu > li > .side-menu__item");

            // Create the tooltip element
            const tooltip = document.createElement("div");
            tooltip.className = "custome-tooltip";
            // tooltip.textContent = "This is a tooltip";

            // Set the CSS properties of the tooltip element
            tooltip.style.setProperty("position", "fixed");
            tooltip.style.setProperty("display", "none");
            tooltip.style.setProperty("padding", "0.5rem");
            tooltip.style.setProperty("white-space", "nowrap");
            tooltip.style.setProperty("font-weight", "500");
            tooltip.style.setProperty("font-size", "0.75rem");
            tooltip.style.setProperty("background-color", "rgb(15, 23 ,42)");
            tooltip.style.setProperty("color", "rgb(255, 255 ,255)");
            tooltip.style.setProperty("margin-inline-start", "135px");
            tooltip.style.setProperty("border-radius", "0.25rem");
            tooltip.style.setProperty("z-index", "99");
            console.log(menuSlideItem);
            menuSlideItem.forEach((e) => {
                // Add an event listener to the menu slide item to show the tooltip
                e.addEventListener("mouseenter", () => {
                    tooltip.style.setProperty("display", "block");
                    tooltip.textContent =
                        e.querySelector(".side-menu__label").textContent;
                    if ( document.querySelector("html").getAttribute("data-vertical-style") == "doublemenu") {
                        e.appendChild(tooltip);
                    }
                });

                // Add an event listener to hide the tooltip
                e.addEventListener("mouseleave", () => {
                    tooltip.style.setProperty("display", "none");
                    tooltip.textContent =e.querySelector(".side-menu__label").textContent;
                    if (document.querySelector("html").getAttribute("data-vertical-style") == "doublemenu") {
                        e.removeChild(tooltip);
                    }
                });
            });
        }, 1000);
    }

    }
    if (localStorage.aziranavstyles) {
      let html = document.querySelector("html");
      let navStyles = localStorage.getItem("aziranavstyles");
      if (navStyles == "menu-click") {
        html.setAttribute("data-nav-style", "menu-click");
        localStorage.removeItem("aziraverticalstyles");
        html.removeAttribute("data-vertical-style");
      }
      if (navStyles == "menu-hover") {
        html.setAttribute("data-nav-style", "menu-hover");
        localStorage.removeItem("aziraverticalstyles");
        html.removeAttribute("data-vertical-style");

      }
      if (navStyles == "icon-click") {
        html.setAttribute("data-nav-style", "icon-click");
        localStorage.removeItem("aziraverticalstyles");
        html.removeAttribute("data-vertical-style");
      }
      if (navStyles == "icon-hover") {
        html.setAttribute("data-nav-style", "icon-hover");
        localStorage.removeItem("aziraverticalstyles");
        html.removeAttribute("data-vertical-style");
      }
    }
    if (localStorage.aziraclassic) {
      let html = document.querySelector("html");
      html.setAttribute("data-page-style", "classic");
    }
    if (localStorage.aziraboxed) {
      let html = document.querySelector("html");
      html.setAttribute("data-width", "boxed");
    }
    if (localStorage.aziraheaderfixed) {
      let html = document.querySelector("html");
      html.setAttribute("data-header-position", "fixed");
    }
    if (localStorage.aziraheaderscrollable) {
      let html = document.querySelector("html");
      html.setAttribute("data-header-position", "scrollable");
    }
    if (localStorage.aziraheaderrounded) {
      let html = document.querySelector("html");
      html.setAttribute("data-header-position", "rounded");
    }
    if (localStorage.aziramenufixed) {
      let html = document.querySelector("html");
      html.setAttribute("data-menu-position", "fixed");
    }
    if (localStorage.aziramenuscrollable) {
      let html = document.querySelector("html");
      html.setAttribute("data-menu-position", "scrollable");
    }
    if (localStorage.aziraMenu) {
      let html = document.querySelector("html");
      let menuValue = localStorage.getItem("aziraMenu");
      switch (menuValue) {
        case "light":
          html.setAttribute("data-menu-styles", "light");
          break;
        case "dark":
          html.setAttribute("data-menu-styles", "dark");
          break;
        default:
          break;
      }
    }
    if (localStorage.aziraHeader) {
      let html = document.querySelector("html");
      let headerValue = localStorage.getItem("aziraHeader");
      switch (headerValue) {
        case "dark":
          html.setAttribute("data-header-styles", "dark");
          html.removeAttribute("data-default-header-styles");
          break;
        case "color":
          html.setAttribute("data-header-styles", "color");
          html.removeAttribute("data-default-header-styles");
          break;
        case "gradient":
          html.setAttribute("data-header-styles", "gradient");
          html.removeAttribute("data-default-header-styles");
          break;
        case "transparent":
          html.setAttribute("data-header-styles", "transparent");
          html.removeAttribute("data-default-header-styles");
          break;

        default:
          break;
      }
    }
    if (localStorage.aziraDefaultHeader) {
      let html = document.querySelector("html");
      let defaultHeaderValue = localStorage.getItem("aziraDefaultHeader");
      switch (defaultHeaderValue) {
        case "light":
          html.setAttribute("data-default-header-styles", "light");
          html.removeAttribute("data-header-styles");
          break;
        case "dark":
          html.setAttribute("data-default-header-styles", "dark");
          html.removeAttribute("data-header-styles");
          break;
        case "color":
          html.setAttribute("data-default-header-styles", "color");
          html.removeAttribute("data-header-styles");
          break;
        case "gradient":
          html.setAttribute("data-default-header-styles", "gradient");
          html.removeAttribute("data-header-styles");
          break;
        case "transparent":
          html.setAttribute("data-default-header-styles", "transparent");
          html.removeAttribute("data-header-styles");
          break;

        default:
          break;
      }
    }
    if (localStorage.bgimg) {
      let html = document.querySelector("html");
      let value = localStorage.getItem("bgimg");
      html.setAttribute("data-bg-img", value);
    }
  }
  localStorageBackup();
})();
