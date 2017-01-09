/// <reference path="../jquery-1.10.2.js" />
//Variables globales

(function ($) {
    var loadingIconHtml = '<i style="margin-left:5px;" class="fa fa-refresh fa-spin fa-fw margin-bottom loadingIcon"></i>';
    var blockingHtmlOverlay = '<div class="overlay hp-middle-z-index" style="width:wwwpx; height:hhhpx;"></div>';
    var maxZindex = 'hp-max-z-index';
    var visivilityHidddenClass = 'hp-visibilityHidden';
    var hiddenClass = 'hidden';
    window.commonJs = window.commonJs || {};

    // Format date to MMyyHHmmSS
    window.commonJs.formatDateAsddMMyyHHmmSS = function (dateToFormat) {
        var monthNames = [
          "January", "February", "March",
          "April", "May", "June", "July",
          "August", "September", "October",
          "November", "December"
        ];

        // Get parts of the date
        var day = dateToFormat.getDate();
        var monthIndex = dateToFormat.getMonth() + 1;
        var year = dateToFormat.getFullYear();
        var hours = dateToFormat.getHours();
        var minutes = dateToFormat.getMinutes();
        var seconds = dateToFormat.getSeconds();

        // Set time format
        var ampm = hours >= 12 ? 'PM' : 'AM';
        hours = hours % 12;
        hours = hours ? hours : 12; // the hour '0' should be '12'
        minutes = minutes < 10 ? '0' + minutes : minutes;
        seconds = seconds < 10 ? '0' + seconds : seconds;

        // Set date format
        monthIndex = monthIndex < 10 ? '0' + monthIndex : monthIndex;
        day = day < 10 ? '0' + day : day;


        // Put Format together
        var dateAsString =  day + '/' + monthIndex + '/' + year + ' ' +
                            hours + ':' + minutes + ':' + seconds + ' ' + ampm;

        return dateAsString;
    }

    // Return a json object from a string with * instead of "
    window.commonJs.getJson = function (stringWithAteriscos) {
        var ret = null;
        if (stringWithAteriscos) {
            var stringValue = stringWithAteriscos.replace(/\*/g, '\"');
            ret = JSON.parse(stringValue);
        }

        return ret;
    }
    // Get exclamation error icon after an element
    // !! WARNING The element to insert should be only one tag like this <></>, has hidden class, and unique (place in the RoutesAndMessages.cshtml)
    window.commonJs.insertHtmlElement = function(elementSelectorToInsert, $elementWhereToInsertAfter, classesToAdd, titleAttrValue){

        // Remove the .setOfIcons class
        var selectorOfAlreadyInsertedElement = elementSelectorToInsert.replace('.setOfIcons ', '');

        // Check if the element selector to insert is not already
        var elementToInsertAlreadyExist = $elementWhereToInsertAfter.siblings(selectorOfAlreadyInsertedElement).length > 0;

        if (!elementToInsertAlreadyExist) {
            // Clone element to insert, add classes, and make visible
            var $elementToInsert = $(elementSelectorToInsert).clone().addClass(classesToAdd);

            // Add title if exist
            if (titleAttrValue) {
                $elementToInsert.attr('title', titleAttrValue);
            }
        
            $elementWhereToInsertAfter.after($elementToInsert);
        }
        }

    // Save a hidden value next to an element
    window.commonJs.saveHiddenValueNextToElement = function ($element, value) {
        var htmlHiddenInput = '<input class="hiddenValue" type="hidden" value="xxx"/>';
        var htmlWithValue = htmlHiddenInput.replace('xxx', value);
        $($element).after(htmlWithValue);

    }

    // Get a hidden value next to an element. Return null if there is no hidden value
    window.commonJs.getHiddenValueNextToElement = function ($element) {
        // Get nextElement and its value
        var $nextElement = $element.next();
        var nextElementValue = $nextElement.val();

        // Return null if there is no a hidden value. Otherwise return the hidden value
        var ret = $nextElement.hasClass('hiddenValue') ? nextElementValue : null;
        return ret;

    }
    // Apply visivility hidden of an element
    window.commonJs.enableVisibilityHidden = function ($elements) {
        for (var i = 0; i < $elements.length; i++) {
            if (!$($elements[i]).hasClass(visivilityHidddenClass)) {
                $($elements[i]).addClass(visivilityHidddenClass);
            }
        }
    }

    // Disable Visivility hidden of an element
    window.commonJs.disableVisibilityHidden = function ($elements) {
        for (var i = 0; i < $elements.length; i++) {
            if ($($elements[i]).hasClass(visivilityHidddenClass)) {
                $($elements[i]).removeClass(visivilityHidddenClass);
            }
        }
    }

    // Hide element
    window.commonJs.hideElement = function ($elements) {
        for (var i = 0; i < $elements.length; i++) {
            if (!$($elements[i]).hasClass(hiddenClass)) {
                $($elements[i]).addClass(hiddenClass);
            }
        }
    }

    // Show element
    window.commonJs.showElement = function ($elements) {
        for (var i = 0; i < $elements.length; i++) {
            if ($($elements[i]).hasClass(hiddenClass)) {
                $($elements[i]).removeClass(hiddenClass);
            }

        }
    }

    // Remove elements from array
    window.commonJs.removeElementsFromArray = function (ArrayOfElements, IndexesToRemove) {
        for (var i = 0; i < IndexesToRemove.length; i++) {
            delete ArrayOfElements[IndexesToRemove[i]];
        }
    }

    // Get array of elements within a container
    window.commonJs.getArrayOfElements = function ($container, ArrayOfSelectors) {
        var arrayOfElements = [];

        for (var i = 0; i < ArrayOfSelectors.length; i++) {
            arrayOfElements[i] = $container.find(ArrayOfSelectors[i]);
        }

        return arrayOfElements;
    }

    // Diable element
    window.commonJs.disableElements = function ($arrayOfelements) {
        for (var i = 0; i < $arrayOfelements.length; i++) {
            if ($arrayOfelements[i]) {
                $($arrayOfelements[i]).attr('disabled', 'disabled');
            }
        }
    }

    // Enable elements
    window.commonJs.enableElements = function ($arrayOfelements) {
        for (var i = 0; i < $arrayOfelements.length; i++) {
            if ($arrayOfelements[i]) {
                $($arrayOfelements[i]).removeAttr('disabled');
            }
        }
    }

    // Block buttons, panels and other stuff when the a timer is running
    window.commonJs.blockPage = function ($elementNotToBlock) {
        // Get wrappe size
        var wrapper = $('#wrapper');
        var wrapperWidth = wrapper.width();
        var warpperHeight = wrapper.height();

        // Place a blocking html in front of the wrapper to block entire page
        blockingHtmlOverlay = blockingHtmlOverlay.replace('www', wrapperWidth).replace('hhh', warpperHeight);
        var overlay = $('#wrapper').append(blockingHtmlOverlay);
        
        // unblock the element to not block.
        $elementNotToBlock.addClass(maxZindex);
    }

    // Block buttons, panels and other stuff when the a timer is running
    window.commonJs.unBlockPage = function () {
        // Remove the max-z-index class of the non block element
        $('.' + maxZindex).removeClass(maxZindex);

        // Unblock page by removing the bloking html
        $('.overlay').remove();
    }

    // Block form inputs, buttons and show loading image
    window.commonJs.blockFormAndShowLoadingImage = function ($form, buttonSelectorWhereShowLoadingIcon) {
        // Show loading icon
        var button = $form.find(buttonSelectorWhereShowLoadingIcon);
        button.append(loadingIconHtml);

        // Block
        $form.find('.form-control').attr('disabled', 'disabled');
        $form.find('.btn').attr('disabled', 'disabled');
    }

    // UnBlock form inputs, buttons and hide loading image
    window.commonJs.unblockFormAndHideLoadingImage = function ($form) {

        // Unblock
        $form.find('.form-control').removeAttr('disabled');
        $form.find('.btn').removeAttr('disabled');

        // Hide loading icon
        $form.find('.loadingIcon').remove();

    }

    // Close the modal
    window.commonJs.closeModal = function () {
        $('.js-closeModal').click();
    }

    // Highlight input and add label error
    window.commonJs.setErrorInInput = function ($formGroup, $elementWherePutLabelAfter, errorText) {
        $formGroup.addClass('has-error');
        $elementWherePutLabelAfter.after('<label class="errorLabel control-label">' + errorText + '</label>');
    }

    // Remove all errors and input texts
    window.commonJs.removeAllErrorsForm = function ($form) {

        $form.find('.has-error').removeClass('has-error');
        $form.find('.errorLabel').remove();

    }

    // Reset all fields
    window.commonJs.resetAllFieldsInForm = function ($form) {
        $form.find('.form-control').val('');
        $form.find('.form-control').text('');
    }

    // Show all the errors from modelError in a form.
    window.commonJs.showErrorMessageInFields = function (modelError, $form) {
        // Clean all previous errors
        commonJs.removeAllErrorsForm($form);

        // Get all inputs of the form
        var $inputs = $form.find(':input');

        // For each input in the form
        $inputs.each(function () {
            if (modelError[this.name] && modelError[this.name].length > 0) {

                // Get input and parent input
                var $inputElement = $(this);
                var $parentInput = $inputElement.parent();

                // Get label and Add error in the label
                var $label = $parentInput.find('label');
                var labelTextWithError = '. ' + modelError[this.name];

                // Set errors
                commonJs.setErrorInInput($parentInput, $label, labelTextWithError)
            }
        });

    }

    // Submit a form by ajax and get a json response
    /* url: make the request to this url
     * $form: form to submit.
     * successCallback and errorCallback: functions with data, status argument for each callback.*/
    window.commonJs.submitFormAndGetJson = function (url, $form, successCallback, errorCallback) {
        var data = $form.serialize();
        $.ajax(
        {
            type: "POST",
            url: url,
            dataType: "json",
            data: data,
            success: successCallback,
            error: errorCallback
        });
    }

    // Request an html page from server
    /* url: make the request to this url
     * successCallback and errorCallback: functions with data, status argument for each callback.*/
    window.commonJs.getHtmlPage = function (url, successCallback, errorCallback) {
        $.ajax(
        {
            type: "GET",
            cache: false,
            url: url,
            dataType: "html",
            success: successCallback,
            error: errorCallback
        });
    }

    // Apply datatable configuration
    window.commonJs.orderColumnOfDataTableByField = function (fieldIndex, tableSelector) {
        var table = $(tableSelector).DataTable();

        table
            .order([1, "desc"])
            .draw();
    }

    // Action to do when the popup form was submited sucessfully
    window.commonJs.onSuccessSubmitingPopupForm = function ($form, refreshUrl, submitButtonSelector) {
        // Unblock form and hide loading image
        commonJs.unblockFormAndHideLoadingImage($form, submitButtonSelector);

        // Remove errors, reset fields and close modal
        commonJs.removeAllErrorsForm($form);
        commonJs.resetAllFieldsInForm($form);
        commonJs.closeModal();

        // Refresh index page
        window.open(refreshUrl, "_self");
    }

    // Action to do when an error occurs when submiting the popup form
    window.commonJs.onFailureSubmitingPopupForm = function ($form, submitButtonSelector, dataFromResponse) {
        var isJson = true;

        // Try parse
        try {
            var modelError = JSON.parse(dataFromResponse.statusText);
        }
        catch (e) {
            isJson = false;
        }

        // Show errors if json can be parsed
        if (isJson) commonJs.showErrorMessageInFields(modelError, $form);
        
        // unblock
        commonJs.unblockFormAndHideLoadingImage($form, '.js-submit');
    }


    // Eventos
    function main() {
        // tooltip
        $('body').tooltip({
            selector: "[rel=tooltip]",
            container: "body"
        })
    }

    $(main);

})(jQuery);