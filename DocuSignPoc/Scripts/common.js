var common = function () {
    var init = function () {
        $('.modal').on('show.bs.modal', function () {
            $('.wrapper').removeClass('animated fadeInRight');
        });
    },
        changeButtonState = function ($this, buttonTxt, disable) {
            $($this).attr('disabled', disable);
            $($this).val(buttonTxt);
        },
        closeModal = function ($this) {
            $($this).modal('hide');
        },
        bindLoader = function ($this) {
            $($this).html(`
                <div class="loader">
                    <img src="/Images/loader.jpg" />
                </div>
            `);
        },
        bindTimePicker = function () {
            $('input.timePicker').timepicker({
                timeFormat: 'h:mm p',
                dropdown: true,
                scrollbar: true,
                defaultTime: '10',
                zindex: 9999 /*It is critical for making time picker work on modal*/
            });
        },
        bindDatePicker = function () {
            $('.datepicker').datepicker({
                //startDate: '-3d',
                //orientation: "bottom right",
                firstDay: 0,
                dateFormat: 'mm/dd/yy'
            });
        },
        bindError = function (data) {
            $.each(data.formErrors, function () {
                if (this.errors.length > 0) {
                    $('[data-valmsg-for=' + this.key.replace(/(:|\.|\[|\]|,)/g, '\\$1') + ']').html(this.errors.join());
                }
            });
        },

        /**
    * Show toaster message as per type.
    * @param {any} message
    * @param {any} headline
    * @param {'Allow only success, error, warning and info'} type
    */
        showMessage = function (message, type, headline = null) {
            switch (type) {
                case 'success':
                    toastr.success(
                        message,
                        headline,
                        { timeOut: 5000, extendedTimeOut: 1000, closeButton: true, closeDuration: 3000 }
                    );
                    break;

                case 'error':
                    toastr.error(
                        message,
                        headline,
                        { timeOut: 5000, extendedTimeOut: 1000, closeButton: true, closeDuration: 3000 }
                    );
                    break;

                case 'warning':
                    toastr.warning(
                        message,
                        headline,
                        { timeOut: 5000, extendedTimeOut: 1000, closeButton: true, closeDuration: 3000 }
                    );
                    break;

                case 'info':
                    toastr.info(
                        message,
                        headline,
                        { timeOut: 5000, extendedTimeOut: 1000, closeButton: true, closeDuration: 3000 }
                    );
                    break;
            }
        };
    return {
        init: init,
        changeButtonState: changeButtonState,
        closeModal: closeModal,
        bindLoader: bindLoader,
        bindTimePicker: bindTimePicker,
        bindDatePicker: bindDatePicker,
        bindError: bindError,
        showMessage: showMessage
    }
}();