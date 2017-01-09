/// <reference path="../Vendor/_references.js" />
/// <reference path="../scriptTemplate/dataTable/jquery.dataTables.min.js" />
/// <reference path="Common.js" />

(function ($) {
    // Variables for timer
    var defaultValueTimer = '00:00:00';
    var timerSelector = '.timer';
    var continueButtonTimer = '.resume-timer-btn';
    var pauseButtonTimer = '.pause-timer-btn';
    var submitButtonSelector = '.js-submit';

    // Variables for elements of the panel
    var panelElements = {
        colorIcons: 0,
        buttonTag: 1,
        textArea: 2,
        timerInput: 3,
        playButton: 4,
        pauseButton: 5,
        finishButton: 6,
        openButton: 7,
        okTextButton: 8,
        cancelTextButton: 9,
        deleteTask: 10
    };
    var panelSelectors = 
    [
        'button.colorIcons', 
        '.btn.dropdown-toggle',
        'textarea',
        '.timer',
        continueButtonTimer,
        pauseButtonTimer,
        '.js-finish-task',
        '.js-open-task',
        '.js-ok-text',
        '.js-cancel-text',
        '.js-delete-task'
    ];
    
    // Variables for text editing
    var insertFormSelector = '.modalForm-insertTaskModal';
    var textIsBeingEditedClass = 'textIsBeingEdited';

    // Variables for highlight
    var colorPanelClassOfRunningTimer = 'panel-info';
    
    // Enumerados
    var filterDate = { newestTask: 1, oldestTask: 2 };

    // Filter  ================================================================

    // Filter date as ascending
    function filterDateBy(filterDateEnumValue) {
        // Get all the panels
        var panels = $('.col-lg-4');

        // Get the filtered array of elements
        var filteredPanels = panels.sort(function (a, b) {
            // Get dates as string
            var aStringDate = $(a).data('creation-date');
            var bStringDate = $(b).data('creation-date');

            // Convert them into dates
            var aDate = new Date(aStringDate);
            var bDate = new Date(bStringDate);

            // Get the difference
            var ret;
            if (filterDateEnumValue == filterDate.newestTask) {
                ret = bDate - aDate;
            }
            else if (filterDateEnumValue == filterDate.oldestTask) {
                ret = aDate - bDate;
            }

            return ret;
        })

        // Set the new array of filtered elements
        $('.panelContainer').prepend(filteredPanels);
    }

    // Filter by an element
    function filterByPanelColorClass(panelColorClass) {
        // Select all the panel with that color class
        var panels = $('.' + panelColorClass).parent();

        // Move panels the beginning
        $('.panelContainer').prepend(panels);
    }

    // Block whole pannel
    function blockWholePanel($panelContainer) {
        // Get all the elements in the panel
        var allPanelElements = commonJs.getArrayOfElements($panelContainer, panelSelectors);

        // Block all the elements and show loading icon
        commonJs.disableElements(allPanelElements);
    }

    // Unblock whole pannel
    function unblockWholePanel($panelContainer) {
        // Get all the elements in the panel
        var allPanelElements = commonJs.getArrayOfElements($panelContainer, panelSelectors);

        // Block all the elements and show loading icon
        commonJs.enableElements(allPanelElements);
    }

    // Highlight, Remove and Tags ================================================================

    // Change the color of a panel with balls
    function changePanelColorTo($panel, $newBallElement, $activeBallElement, isComplete) {

        // Get classes for active and new color
        var newPanelClassColor = $newBallElement.data('color');
        var activePanelClassColor = $activeBallElement.data('color');

        // If it is not a complete status, then temove old class color
        $panel.removeClass(activePanelClassColor);

        // Add new color by adding new class
        $panel.addClass(newPanelClassColor);

        // Remove the current active ball item and set another active ball item
        if (!isComplete) {
            $activeBallElement.removeClass('activeColor');
            $activeBallElement.removeClass('colorWhite');

            $newBallElement.addClass('activeColor');
            $newBallElement.addClass('colorWhite');
        
        }
    }

    // On success submitting highlight
    function onSuccessSubmittingHighlight($panelContainer, jsonData) {
        stopLoadingChangingHighlight($panelContainer);

        // Set the id of the inserted record
        var $form = $panelContainer.find('.panel-heading').find('form.colorForm');
        $form.find('[name="Id"]').val(jsonData);
    }

    // On failure submitting highlight
    function onFailureSubmittingHighlight($panelContainer) {
        stopLoadingChangingHighlight($panelContainer)

        // Show error icon
        var $elementToPlaceTheLoadingIconNext = $panelContainer.find('.panel-heading').find('.colorIcons.lastOne');
        commonJs.insertHtmlElement('.setOfIcons .exclamationError', $elementToPlaceTheLoadingIconNext, 'errorChangingColor', appResources.changingHighlightError);
    }

    // Show the loading icon when changing higlight and block all elements
    function startLoadingChangingHighlight($panelContainer) {
        // Block whole panel
        blockWholePanel($panelContainer);

        // Show loading icon
        var $elementToPlaceTheLoadingIconNext = $panelContainer.find('.panel-heading').find('.colorIcons.lastOne');
        commonJs.insertHtmlElement('.setOfIcons .loadingIcon', $elementToPlaceTheLoadingIconNext, 'changingHighlightColor', appResources.changingHighlightError);
    }

    // Hide the loading icon when changing higlight and unblock all elements
    function stopLoadingChangingHighlight($panelContainer) {
        // Unblock whole panel
        unblockWholePanel($panelContainer);

        // Remove loading icon
        $panelContainer.find('.panel-heading').find('.loadingIcon').remove();
    }

    // Text ====================================================

    // Show ok and cancel buttons on editing text and hide the other ones
    function showEditingTextButtons($footerParent) {

        // Get timer textbox and all the elements next to the timer textbox in the footer
        var timerTextBox = $footerParent.find('.timer');
        var timerElements = $footerParent.find('.editingNextTotimer'); // you will apply a different visibility to the timer text box

        // Hide the timer elements and apply visibility hidden to the timer textbox
        commonJs.hideElement(timerElements);
        commonJs.enableVisibilityHidden(timerTextBox);

        // Get the ok and cancel button and Show them
        var editingTextButtons = $footerParent.find('.editingTextButtons');
        commonJs.showElement(editingTextButtons);

        // Add the textIsBeingEdited class in the footerParent to not run this method again
        $footerParent.addClass(textIsBeingEditedClass);
    }

    // Show ok and cancel buttons on editing text and hide the other ones
    function hideEditingTextButtons($footerParent) {

        // Get timer textbox and all the elements next to the timer textbox in the footer
        var timerTextBox = $footerParent.find('.timer');
        var timerElements = $footerParent.find('.editingNextTotimer'); // you will apply a different visibility to the timer text box

        // Hide the timer elements and apply visibility hidden to the timer textbox
        commonJs.showElement(timerElements);
        commonJs.disableVisibilityHidden(timerTextBox);

        // Get the ok and cancel button and Show them
        var editingTextButtons = $footerParent.find('.editingTextButtons');
        commonJs.hideElement(editingTextButtons);

        // Add the textIsBeingEdited class in the footerParent to not run this method again
        $footerParent.removeClass(textIsBeingEditedClass);
    }

    // On success submiting the text
    function onSuccessSubmitingText($panelContainer) {
        // stop loading
        stopLoadingForEditingText($panelContainer);

        // Hide editing text buttons
        var $footerParent = $panelContainer.find('.panel-footer');
        hideEditingTextButtons($footerParent);

        // Set the last text value in the hidden 
        var $textForm = $panelContainer.find('.panel-body').find('.taskTextForm');
        var newValue = $textForm.find('.taskText').val();
        $textForm.find('.lastTextValueFromDb').val(newValue);
    }

    // On faialure submiting the text
    function onFailureSubmittingText($panelContainer) {
        // stop loading
        stopLoadingForEditingText($panelContainer);

        // Show error icon if it is not already there
        var $elementToPlaceTheLoadingIconNext = $panelContainer.find('.panel-footer').find('.js-cancel-text');
        commonJs.insertHtmlElement('.setOfIcons .exclamationError', $elementToPlaceTheLoadingIconNext, 'pull-right', appResources.submitingTextError);
    }

    // Disable elements and show the loading icon for editing text
    function startLoadingForEditingText($panelContainer) {
        // Block whole panel
        blockWholePanel($panelContainer);

        // Show loading icon
        var $elementToPlaceTheLoadingIconNext = $panelContainer.find('.panel-footer').find('.js-cancel-text');
        commonJs.insertHtmlElement('.setOfIcons .loadingIcon', $elementToPlaceTheLoadingIconNext, 'pull-right textSubmitingError');
    }

    // Enable panel elements and show the loading icon for editing text
    function stopLoadingForEditingText($panelContainer) {
        // Unblock whole panel
        unblockWholePanel($panelContainer);

        // Remove the loading icon
        $panelContainer.find('.panel-footer').find('.loadingIcon').remove();
    }

    // Finish/Open Tasks ====================================================

    // Start the loading icon in the footer of the panel
    function startLoadingInButtonsFooter($panelContainer) {
        // Show loading icon
        var $elementToPlaceTheLoadingIconNext = $panelContainer.find('.panel-footer').find('.js-cancel-text');
        commonJs.insertHtmlElement('.setOfIcons .loadingIcon', $elementToPlaceTheLoadingIconNext, 'pull-right loadingIconButtonsInFooter');
    }

    // Stop the loading icon in the footer of the panel
    function stopLoadingInButtonsFooter($panelContainer) {
        // Remove loading icon
        $panelContainer.find('.panel-footer').find('.loadingIcon').remove();
    }

    // On success saving status
    function onSucessSavingStatus($panelContainer, dataFromJson) {
        stopLoadingInButtonsFooter($panelContainer);

        // Set the id of the complete record
        var idIsCompleteHidden = $panelContainer.find('.saveStatusForm').find('.Id');
        idIsCompleteHidden.val(dataFromJson);
    }

    // On failure saving satatus
    function onFailureSavingStatus($panelContainer) {
        stopLoadingInButtonsFooter($panelContainer);

        // Show error icon
        var $elementToPlaceTheLoadingIconNext = $panelContainer.find('.panel-footer').find('.js-cancel-text');
        commonJs.insertHtmlElement('.setOfIcons .exclamationError', $elementToPlaceTheLoadingIconNext, 'pull-right ErrorIconButtonsInFooter', appResources.statusSaveError);
    }

    // Get elements to block when finish button is clicked
    function getElementsToBlockWhenFinishTask($container) {
        var allPanelElements = commonJs.getArrayOfElements($container, panelSelectors);
        commonJs.removeElementsFromArray(allPanelElements, [panelElements.openButton]);

        return allPanelElements;
    }

    // Block panel elements when a task is finished
    function blockPanelWhenTaskIsFinished($container) {
        // Elements to block
        var panelElementsToBlock = getElementsToBlockWhenFinishTask($container);

        // Block panel items
        commonJs.disableElements(panelElementsToBlock);
    }

    // unBlock panel elements when a task is finished
    function unblockPanelWhenTaskIsFinished($container) {
        // Elements to unblock
        var panelElementsToBlock = getElementsToBlockWhenFinishTask($container);

        // Block panel items
        commonJs.enableElements(panelElementsToBlock);
    }

    // Timer ================================================================

    // Save the timer value in db
    function saveTimerInDb($timer, $timerForm, $panelContainer, unblockPanel) {
        // Get number of seconds and set the hidden input with the value
        var numberOfSeconds = getSeconds($timer.val());

        // Get form and set the number of secons in a hidden input inside the form
        var hiddenValueInSeconds = $timerForm.find('.TimeInSeconds');
        hiddenValueInSeconds.val(numberOfSeconds);

        // Save number of secons in db
        var url = appResources.writeTimer;
        commonJs.submitFormAndGetJson(url, $timerForm,
            function (data, status) {
                if (unblockPanel) onSuccessSavingTime($panelContainer, data);
                else onSuccessSavingTimeWithoutBlock($panelContainer, data);
            },
            function (data, status) {
                if (unblockPanel) onFailureSavingTime($panelContainer);
                else onFailureSavingTimeWithoutBlock($panelContainer);
            });
    }

    // failure on saving time in db
    function onFailureSavingTime($panelContainer) {
        // Stop loading and unblock panel
        unblockWholePanel($panelContainer);

        onFailureSavingTimeWithoutBlock($panelContainer);

    }

    // On success saving time in db
    function onSuccessSavingTime($panelContainer, dataFromJson) {
        // Stop loading and unblock panel
        unblockWholePanel($panelContainer);

        onSuccessSavingTimeWithoutBlock($panelContainer, dataFromJson);
    }

    // failure on saving time in db when finishing task
    function onFailureSavingTimeWithoutBlock($panelContainer) {
        // Stop loading and unblock panel
        stopLoadingInButtonsFooter($panelContainer);

        // Show error icon
        var $elementToPlaceTheLoadingIconNext = $panelContainer.find('.panel-footer').find('.js-cancel-text');
        commonJs.insertHtmlElement('.setOfIcons .exclamationError', $elementToPlaceTheLoadingIconNext, 'pull-right ErrorIconButtonsInFooter', appResources.saveTimerError);

    }

    // On success saving time in db
    function onSuccessSavingTimeWithoutBlock($panelContainer, dataFromJson) {
        // Stop loading and unblock panel
        stopLoadingInButtonsFooter($panelContainer);

        // Set the hidden Id input inside the form with the id of the inserted record
        var hiddenIdInput = $panelContainer.find('.saveTimer').find('.Id');
        hiddenIdInput.val(dataFromJson);
    }

    // Get elements to block when timer is on
    function getElementsToblockwhenTimerOn($container) {
        var allPanelElements = commonJs.getArrayOfElements($container, panelSelectors);
        commonJs.removeElementsFromArray(allPanelElements, [panelElements.pauseButton]);

        return allPanelElements;
    }

    // Block the page and the items of the panel
    function blockPageAndContainerChildren($container) {
        // Block page
        commonJs.blockPage($container);
        
        // Elements to block
        var panelElementsToBlock = getElementsToblockwhenTimerOn($container);
        
        // Block panel items
        commonJs.disableElements(panelElementsToBlock);
    }

    // Block the page and the items of the panel
    function unblockPageAndContainerChildren($container) {
        // Block page
        commonJs.unBlockPage($container);

        // Elements to block
        var panelElementsToBlock = getElementsToblockwhenTimerOn($container);

        // Block panel items
        commonJs.enableElements(panelElementsToBlock);
    }

    // Show error in the timer and block button(footer)
    function showTimerErrorAndBlockButton(errorMessage, $arrayOfElementsToDisable, $timer) {

        // Show error if it is not already display
        if (!$timer.hasClass('hasError')) {
            // Block finish and continue button
            commonJs.disableElements($arrayOfElementsToDisable);

            // Add hasError class
            $timer.addClass('hasError');

            // Set error
            var $elementToPlaceTheErrorIconNext = $timer.siblings('.js-cancel-text');
            commonJs.insertHtmlElement('.setOfIcons .exclamationError', $elementToPlaceTheErrorIconNext, 'ErrorIconButtonsInFooter', errorMessage);
        }
    }

    // Hide error in the timer and unblock button (footer)
    function hideTimerErrorAndUnblockButton($arrayOfElementToEnable, $timer) {

        commonJs.enableElements($arrayOfElementToEnable);

        // Remove class error and element error
        $timer.removeClass('hasError');
        $timer.parent().find('.exclamationError').remove();
    }

    // Validate time input and return a valid one
    function timeIsValidandShowOrHideErrors($timeInput) {
        var ret = true;
        // Get elements to block
        var $continueButton = $timeInput.parent().find(continueButtonTimer);
        var $finishButton = $timeInput.parent().find('.js-finish-task');
        var $arrayOfElementsToBlock = [$continueButton, $finishButton];

        // Get hours, minutes, and seconds
        var time = $timeInput.val().split(':');
        var hours = parseInt(time[0]);
        var minutes = parseInt(time[1]);
        var seconds = parseInt(time[2]);
        
        // Seconds must be lowe than 60
        if (ret && hours >= 24) {
            hideTimerErrorAndUnblockButton($continueButton, $timeInput);
            showTimerErrorAndBlockButton(appResources.hourError, $arrayOfElementsToBlock, $timeInput);
            ret = false;
        }

        // Minutes must be lowe than 60
        if (ret && minutes >= 60) {
            hideTimerErrorAndUnblockButton($continueButton, $timeInput);
            showTimerErrorAndBlockButton(appResources.minuteError, $arrayOfElementsToBlock, $timeInput);
            ret = false;
        }

        // Seconds must be lowe than 60
        if (ret && seconds >= 60) {
            hideTimerErrorAndUnblockButton($continueButton, $timeInput);
            showTimerErrorAndBlockButton(appResources.secondError, $arrayOfElementsToBlock, $timeInput);
            ret = false;
        }


        // If all the validations are ok
        if (ret) {
            hideTimerErrorAndUnblockButton($arrayOfElementsToBlock, $timeInput);
        }

        return ret;
    }

    // Get the amount of seconds for a time in the format hh:mm:ss
    function getSeconds(timeValue) {
        var time = timeValue.split(':');

        // Get hours, minutes, and seconds
        var hours = parseInt(time[0]);
        var minutes = parseInt(time[1]);
        var seconds = parseInt(time[2]);

        return hours * 3600 + minutes * 60 + seconds;
    }

    // Set timer the first time when it contains the defaultValueTimer
    function setTimer($timer, secondsValue) {
        // Set timer for the first time
        $timer.timer({
            editable: true,
            format: '%H:%M:%S',
            seconds: secondsValue
        });
    }

    // Get pause button
    function getPauseButton($anySibling) {
        var pauseButton = $anySibling.parent().find(pauseButtonTimer);
        return pauseButton;
    }

    // Get continue button
    function getContinueButton($anySibling) {
        var continueButton = $anySibling.parent().find(continueButtonTimer);
        return continueButton;
    }

    // Get the timer element as an input text
    function getTimer($anyTimerSibling) {
        var timer = $anyTimerSibling.parent().find(timerSelector);
        return timer;
    }

    // Get the current panel
    function getCurrentPanel($anyPanelChild) {
        var $panel = $anyPanelChild.closest('.panel');
        return $panel;
    }

    // Pause timer and unblock content
    function pauseTimer($pauseButton) {
        // Unblock all the panels
        var $panel = getCurrentPanel($pauseButton).parent();
        unblockPageAndContainerChildren($panel);
        //commonJs.unBlockPage($panel);

        // Pause timer
        getTimer($pauseButton).timer('pause');

        // Hide pause button and show the continue button
        $pauseButton.addClass('hidden');
        $(getContinueButton($pauseButton)).removeClass('hidden');
    }

    // Eventos
    function main() {
        // Panel task ====================================================

        // Click on new task: get requirement id and put it in an input inside the insert form
        $('.js-insert-task').click(function (e) {
            e.preventDefault();

            // Get project, and requirement information
            var $this = $(this);
            var requirementId = $this.data('requirement-id');

            // Set previus info in modal insert form
            var $insertForm = $(insertFormSelector);
            $insertForm.find('[name="RequirementId"]').val(requirementId);
        });

        // On submit insert modal form: submit a task.
        $(insertFormSelector).submit(function () {
            // Get form and urls
            var $form = $(this);
            var allTasksUrl = appResources.allTasksUrl;
            var createTaskUrl = appResources.createTaskUrl;
            
            // Make request
            commonJs.submitFormAndGetJson(createTaskUrl, $form,
                // Success
                function (data, status) {
                    commonJs.onSuccessSubmitingPopupForm($form, allTasksUrl, '.js-submit');
                },
                // Error
                function (data, status) {
                    commonJs.onFailureSubmitingPopupForm($form, submitButtonSelector, data);
                });

            // Block form and show loading icon
            var $modal = $form.closest('.modal');
            commonJs.disableElements($modal.find('.close'));
            commonJs.blockFormAndShowLoadingImage($form, '.js-submit');

            // Prevent the form to be submited and the modal to close
            return false;
        });

        // On Submit delete popup form: delete a task
        $('body').on('submit', '.modalForm-deleteTaskModal', function (e) {

            // Delete Task
            var url = appResources.deleteTask;
            $form = $(this);
            commonJs.submitFormAndGetJson(url, $form,
                function (data, status) {
                    commonJs.onSuccessSubmitingPopupForm($form, appResources.allTasksUrl, submitButtonSelector);
                },
                function (data, status) {
                    commonJs.onFailureSubmitingPopupForm($form, submitButtonSelector, data);
                });

            // Block form and show loading icon
            var $modal = $form.closest('.modal');
            commonJs.disableElements($modal.find('.close'));
            commonJs.blockFormAndShowLoadingImage($form, submitButtonSelector);

            // Prevent the form to perform create conflicts
            return false;
        })

        // On click in delete task: populate the popup form with id and text of the task.
        $('body').on('click', '.js-delete-task', function (e) {
            e.preventDefault();

            // Get project name from project info row
            var taskId = $(this).data('task-id');
            var taskText = $(this).closest('.panel').find('.taskText').text();

            // Set the id of the project in the hidden field of the delete form
            var $form = $('.modalForm-deleteTaskModal');
            $form.find('.Id').val(taskId);

            // Replace project name in the resource
            var body = $form.find('.modal-body');
            var newText = body.data('text').replace('taskText', taskText);
            body.text(newText);
            body.append('<label class="control-label"></label><input type="hidden" name="SummaryError" class="form-control">');
        });

        // Click on any filter color link: filter by that element.
        $('.filterByColor').click(function () {
            // Get the element to link
            var colorClassToFilterBy = $(this).find('i').data('color');

            // Filter by the panel color class
            filterByPanelColorClass(colorClassToFilterBy);
        });

        // Click on ascending or descending: filter by ascending or descending
        $('.filterByDate').click(function () {
            // Get the type of filtering: ascending or descending
            var typeOfFiltering = $(this).data('filtering-type');

            // Filter date
            filterDateBy(typeOfFiltering);
        });

        // Text ====================================================

        // On key press inside of the task text: show ok and cancel button
        $('.taskText').keyup(function () {
            // Get footer parent
            var $this = $(this);
            var $footerParent = $this.closest('.panel-body').siblings('.panel-footer');

            // If the key has not been pressed yet
            if (!$footerParent.hasClass(textIsBeingEditedClass)) {
                showEditingTextButtons($footerParent)            // show editing text buttons
            }

        });

        // On cancel text click: show timer buttons and reset the text to the previous value
        $('.js-cancel-text').click(function () {

            // Get footer parent
            var $this = $(this);
            var $footerParent = $this.closest('.panel-footer');

            // Get hidden value and set the editing text box to the previous value
            var $taskText = $footerParent.siblings('.panel-body').find('.taskText');
            var previousValue = $taskText.siblings('.lastTextValueFromDb').val();
            $taskText.val(previousValue);

            // Hide editing text buttons
            hideEditingTextButtons($footerParent);

        });

        // On ok text click: ajax request to edit text and show timer buttons
        $('.js-ok-text').click(function () {
            // Get form and panel container
            var $footerParent = $(this).closest('.panel-footer');
            var $form = $footerParent.siblings('.panel-body').find('.taskTextForm');
            var $panelContainer = $footerParent.parent();

            // Url
            var url = appResources.editTaskUrl;

            // Make ajax request 
            commonJs.submitFormAndGetJson(url, $form,
                function (data, status) {
                    onSuccessSubmitingText($panelContainer);

                },
                function (data, status) {
                    onFailureSubmittingText($panelContainer);
                });

            // Start loading text. !! CAREFUL, this should be done after the form is submitted otherwise the disable causes problem
            startLoadingForEditingText($panelContainer);
        });

        // Highlight, Remove and Tags ==================================================

        // Click on ball color item : change color and submit that color (save it in db)
        $('.colorIcons').click(function () {

            // Get panel and active ball element
            var $panel = $(this).closest('.panel');
            var $activeBallElement = $panel.find('.activeColor');
            var $newBallElement = $(this).children('i');

            // Change color
            changePanelColorTo($panel, $newBallElement, $activeBallElement, false);

            // Set color input hidden with the selected color
            var $form = $(this).siblings('.colorForm');
            var selectedColor = $(this).data('enum-value');
            $form.find('[name="Color"]').val(selectedColor);

            // Submit color
            var url = appResources.writeHighlight;
            commonJs.submitFormAndGetJson(url, $form,
                function (data, status) {
                    onSuccessSubmittingHighlight($panel, data);
                },
                function (data, status) {
                    onFailureSubmittingHighlight($panel);
                });

            // Start loading and block panel
            startLoadingChangingHighlight($panel);

            // Prevent the form to be submited.
            return false;
        });

        // Finish/Open Tasks ====================================================

        // Click on finish task: finish the task...
        $('.js-finish-task').click(function () {

            // Get panel and parent panel
            var $panel = getCurrentPanel($(this));
            var $parentPanel = $panel.parent();

            // Color change : Get active ball and new ball
            var $activeBallElement = $panel.find('.activeColor');
            var $newBallElement = $panel.find('.complete');
            changePanelColorTo($panel, $newBallElement, $activeBallElement, true);

            // Show open button and hide finish button
            commonJs.hideElement($panel.find('.js-finish-task'));
            commonJs.showElement($panel.find('.js-open-task'))

            // Set the hidden input IsComplete to true
            var isComplete = $panel.find('.IsComplete');
            isComplete.val('true');

            // Save status of the task in db
            var $form = $(this).siblings('.saveStatusForm');
            var url = appResources.writeComplete;
            commonJs.submitFormAndGetJson(url, $form,
                function (data, status) {
                    onSucessSavingStatus($parentPanel, data);

                },
                function (data, status) {
                    onFailureSavingStatus($parentPanel);
                });

            // Get timer element and the form that saves the time in db
            var $timer = getTimer($(this));
            var $form = $(this).siblings('.saveTimer');
            var $panelContainer = $(this).closest('.panel');

            // Save time in db
            saveTimerInDb($timer, $form, $panelContainer);

            // Block panel and start loading
            startLoadingInButtonsFooter($parentPanel);
            blockPanelWhenTaskIsFinished($parentPanel);

            // Prevent weird behaviour like going to index action
            return false;
        });

        // Click on open the task: open the task which is finished and ....
        $('.js-open-task').click(function () {
            // Get panel and parent panel
            var $panel = getCurrentPanel($(this));
            var $parentPanel = $panel.parent();

            // Color change : Get active ball and new ball
            var $activeBallElement = $panel.find('[data-color="panel-green"]');
            var $newBallElement = $panel.find('.activeColor'); 
            changePanelColorTo($panel, $newBallElement, $activeBallElement, false);

            // Show open button and hide finish button
            commonJs.showElement($panel.find('.js-finish-task'));
            commonJs.hideElement($panel.find('.js-open-task'))

            // Set the hidden input IsComplete to false
            var isComplete = $panel.find('.IsComplete');
            isComplete.val('false');

            // Save status of the task in db
            var $form = $(this).siblings('.saveStatusForm');
            var url = appResources.writeComplete;
            commonJs.submitFormAndGetJson(url, $form,
                function (data, status) {
                    onSucessSavingStatus($parentPanel, data);
                    unblockPanelWhenTaskIsFinished($parentPanel);
                },
                function (data, status) {
                    onFailureSavingStatus($parentPanel);
                    unblockPanelWhenTaskIsFinished($parentPanel);

                });
            
            // Block panel and start loading
            blockPanelWhenTaskIsFinished($parentPanel);
            startLoadingInButtonsFooter($parentPanel);

            // Prevent weird behaviour like going to index action
            return false;

        });

        // Timer ================================================================

        // Click on continue : Init timer resume
        $(continueButtonTimer).on('click', function () {

            // Block all teh panels but the current panel
            var $containerPanel = getCurrentPanel($(this)).parent();
            blockPageAndContainerChildren($containerPanel);

            // Get timer element and value
            var $timer = getTimer($(this));
            var timeValue = $timer.val();

            // True if it has been already set. Otherwise false
            var hasNotBeenSet = $timer.hasClass('hasNotBeenSet');

            // Timer has not been set yet
            if (hasNotBeenSet)  {
                $timer.removeClass('hasNotBeenSet');
                var seconds = getSeconds($timer.val());
                setTimer($timer, seconds);

            }
            // Init timer if it is equal to default
            else if (timeValue == defaultValueTimer) {
                setTimer($timer, 0);
            }
            // Otherwise continue
            else {
                $timer.timer('resume');
            }

            // Hide the continue button and show the pause button
            $(this).addClass('hidden');
            $(getPauseButton($(this))).removeClass('hidden');

            return false;
        });
        
        // Click on pause : Init timer pause
        $(pauseButtonTimer).on('click', function () {
            // Pause timer
            pauseTimer($(this));

            // Get timer, timer form and panelContainer
            var $timer = getTimer($(this));
            var $form = $(this).siblings('.saveTimer');
            var $panelContainer = $(this).closest('.panel');

            // Save time in db
            saveTimerInDb($timer, $form, $panelContainer, true);

            // Block whole panel and startLoading
            startLoadingInButtonsFooter($panelContainer);
            blockWholePanel($panelContainer);

            // Return false to prevent conflicting behaviour
            return false;
        });

        // On key timer key up : validate input entered
        $(timerSelector).on('keyup', function () {
            timeIsValidandShowOrHideErrors($(this));
        });
       
        // Set mask
        $(timerSelector).mask("99:99:99",
        {
            placeholder: "00:00:00"
        });
    }

    $(main);

})(jQuery);