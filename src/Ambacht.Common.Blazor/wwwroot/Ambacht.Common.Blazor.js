(function () {
  window.blazorLocalStorage = {
    get: key => key in localStorage ? JSON.parse(localStorage[key]) : null,
    set: (key, value) => { localStorage[key] = JSON.stringify(value); },
    delete: key => { delete localStorage[key]; }
  };


  window.Ambacht = {
    saveAsFile: function (filename, bytesBase64, mimeType) {
      var link = document.createElement('a');
      link.download = filename;
      link.href = "data:" + mimeType + ";base64," + bytesBase64;
      document.body.appendChild(link); // Needed for Firefox
      link.click();
      document.body.removeChild(link);
    },

    setWindowLocation: function (address) {
      window.location.assign(address);
    },

    scrollToTop: function () {
      window.scrollTo(0, 120); // Leave space for top bar
    },

    getBounds: function (el) {
      var rect = el.getBoundingClientRect();
      return rect;
    },

    getSize: function (element) {
      var rect = element.getBoundingClientRect();
      return [rect.width, rect.height];
    },

    // This will be called from WindowEventLayer.razor
    registerViewportChangeCallback: function (dotnetHelper) {
      window.addEventListener('load', () => {
        dotnetHelper.invokeMethodAsync('OnResize', window.innerWidth, window.innerHeight);
      });
      window.addEventListener('resize', () => {
        dotnetHelper.invokeMethodAsync('OnResize', window.innerWidth, window.innerHeight);
      });
    },


    // Is called if the user clicks outside this element.
    registerOutsideClick: function (dotnetHelper, element) {
      document.addEventListener('click', e => {
        if (!element.contains(e.target)) {
          dotnetHelper.invokeMethodAsync('OnClickOutside');
        }
      });
    }

  };




})();

