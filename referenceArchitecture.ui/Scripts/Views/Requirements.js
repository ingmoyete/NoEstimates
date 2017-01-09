(function ($) {
    var insertFormSelector = '.modalForm-insertRequirementsModal';
    var editFormSelector = '.modalForm-editRequirementsModal';
    var submitButtonSelector = '.js-submit';
    var creationDateFieldIndex = 1;

    // Objects
    function requirement(Id, Name, Description, ProjectId) {
        this.Id = Id;
        this.Name = Name;
        this.Description = Description;
        this.ProjectId = ProjectId;
    }

    // Methods =====================================
    function getRequirementInfo($tableButton) {

        // Get info from table
        var $requirementInfo = $tableButton.parent().parent().find('.requirementInfo');
        var requirementId = $requirementInfo.data('requirement-id');
        var requirementName = $requirementInfo.text();
        var requirementDescription = $requirementInfo.attr('title');
        var projectId = $requirementInfo.data('project-id');

        // Return requirement object
        var requirementObj = new requirement(requirementId, requirementName, requirementDescription, projectId);
        return requirementObj;

    }

    // Eventos =====================================
    function main() {

        // Click on the delete icon: the project name is populated in the confirmation popup
        $('body').on('click', '.js-delete-requirement', function (e) {
            e.preventDefault();

            // Get project name from project info row
            var requirement = getRequirementInfo($(this));

            // Set the id of the project in the hidden field of the delete form
            var $form = $('.modalForm-deleteRequirementModal');
            $form.find('.Id').val(requirement.Id);

            // Replace project name in the resource
            var body = $form.find('.modal-body');
            var newText = body.data('text').replace('requirementName', requirement.Name);
            body.text(newText);
            body.append('<label class="control-label"></label><input type="hidden" name="SummaryError" class="form-control">');
        });

        // Submit delete form: delete the project
        $('body').on('submit', '.modalForm-deleteRequirementModal', function () {

            // Delete project
            var url = appResources.deleteRequirement;
            $form = $(this);
            commonJs.submitFormAndGetJson(url, $form,
                function (data, status) {
                    commonJs.onSuccessSubmitingPopupForm($form, appResources.allRequirementsUrl, submitButtonSelector);
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
        });

        // Click on edit requirement: open the edit popup and populate information
        $('body').on('click', '.js-edit-requirement', function (e) {
            e.preventDefault();

            // Get project information
            var requirement = getRequirementInfo($(this));

            // Set the project information in the form
            var $editForm = $(editFormSelector);
            $editForm.find('[name="Id"]').val(requirement.Id);
            $editForm.find('[name="Name"]').val(requirement.Name);
            $editForm.find('[name="Description"]').val(requirement.Description);
            $editForm.find('[name="ProjectId"]').val(requirement.ProjectId);

        });

        // Click on insert requirement: open the insert requirement popup and populate the projectID
        $('.js-insert-requirement').click(function (e) {
            e.preventDefault();

            // Get project information
            var $this = $(this);
            var projectId = $this.data('project-id');

            // Set the project id information in the form
            var $insertForm = $(insertFormSelector);
            $insertForm.find('[name="ProjectId"]').val(projectId);

        });

        // Submit edit form: edit a project
        $(editFormSelector).submit(function () {

            // Get form
            var $form = $(this);

            // Make request
            commonJs.submitFormAndGetJson(appResources.updateRequirementUrl, $form,
                // Success
                function (data, status) {
                    commonJs.onSuccessSubmitingPopupForm($form, appResources.allRequirementsUrl, submitButtonSelector);
                },
                // Error
                function (data, status) {
                    commonJs.onFailureSubmitingPopupForm($form, submitButtonSelector, data);
                });

            // Block form and show loading icon
            var $modal = $form.closest('.modal');
            commonJs.disableElements($modal.find('.close'));
            commonJs.blockFormAndShowLoadingImage($form, submitButtonSelector);

            // Prevent the form to be submited and the modal to close
            return false;
        });

        // Submit insert form: create a project
        $(insertFormSelector).submit(function () {

            // Get form
            var $form = $(this);

            // Make request
            commonJs.submitFormAndGetJson(appResources.createRequirementUrl, $form,
                // Success
                function (data, status) {
                    commonJs.onSuccessSubmitingPopupForm($form, appResources.allRequirementsUrl, submitButtonSelector);
                },
                // Error
                function (data, status) {
                    commonJs.onFailureSubmitingPopupForm($form, submitButtonSelector, data);
                });

            // Block form and show loading icon
            var $modal = $form.closest('.modal');
            commonJs.disableElements($modal.find('.close'));
            commonJs.blockFormAndShowLoadingImage($form, submitButtonSelector);

            // Prevent the form to be submited and the modal to close
            return false;
        });

        // Click on cancel button: clean all errors of the modal form
        $('.js-closeModal').click(function () {
            var $insertform = $(insertFormSelector);
            commonJs.removeAllErrorsForm($insertform);
            commonJs.resetAllFieldsInForm($insertform);

            var $editform = $(editFormSelector);
            commonJs.removeAllErrorsForm($editform);
            commonJs.resetAllFieldsInForm($editform);
        });

        // Datatables Columns =========================================
        function nameColumn(data, type, oObj) {
            var htmlTemplate =
                '<span class="requirementInfo" rel="tooltip" title="*description*" ' +
                'data-project-id="*projectId*" ' +
                'data-requirement-id="*requirementId*" ' +
                '*data*>*name*</span>';

            // Url, classes, icon, and title
            var requirementId = oObj.RequirementId;
            var projectId = oObj.ProjectId;
            var data = 'data-placement="top"';
            var description = oObj.Description;
            var name = oObj.Name;

            // Set html
            var retHtml = htmlTemplate
                .replace('*requirementId*', requirementId)
                .replace('*projectId*', projectId)
                .replace('*data*', data)
                .replace('*description*', description)
                .replace('*name*', name);
            return retHtml;
        }

        function creationDateColumn(data, type, oObj) {
            var date = new Date(parseInt(oObj.CreationDate.substr(6)));
            var formatDate = commonJs.formatDateAsddMMyyHHmmSS(date);
            return formatDate;
        }

        function statusColumn(data, type, oObj) {
            var htmlTemplate =
                '<div>' +
                    '<p>' +
                        '<span class="pull-right text-muted">#</span>' +
                    '</p>' +
                    '<div class="progress progress-striped active">' +
                        '<div class="progress-bar progress-bar-success" role="progressbar" aria-valuenow="#" aria-valuemin="0" aria-valuemax="100" style="width: #%;">' +
                            '<span class="sr-only">#% Complete (success)</span>' +
                        '</div>' +
                    '</div>' +
                '</div>';

            // Url, classes, icon, and title
            var status = oObj.Status;

            // Set html
            var retHtml = htmlTemplate
                .replace(/\#/g, status);
            return retHtml;
        }

        function taskColumn(data, type, oObj) {
            var htmlTemplate =
                '<a href="*url*" class="*classes*" rel="tooltip" title="*title*" *data*>' +
                    '<i class="fa *icon* fa-fw"></i>' +
                '</a>';

            // Url, classes, icon, and title
            var url = appResources.getTasksByRequirementId.replace('requirementId', oObj.RequirementId);
            var classes = 'tableIconLink';
            var icon = 'fa-tasks';
            var title = requirementResources.toTasks;
            var data = 'data-placement="top"';

            // Set html
            var retHtml = htmlTemplate
                .replace('*url*', url)
                .replace('*classes*', classes)
                .replace('*icon*', icon)
                .replace('*title*', title)
                .replace('*data*', data);
            return retHtml;
        }

        function statisticColumn(data, type, oObj) {
            var htmlTemplate =
            '<a href="*url*" class="*classes*" rel="tooltip" title="*title*" *data*>' +
                '<i class="fa *icon* fa-fw"></i>' +
            '</a>';

            // Url, Classes, icon, attributes, and title
            var url = appResources.requirementStatistic
                .replace('requirementId', oObj.RequirementId)
                .replace('returnUrl', window.location.href);

            var classes = 'tableIconLink';
            var icon = 'fa-bar-chart-o';
            var title = requirementResources.requirementStatistics;
            var data =
                'data-placement="top"';

            // Set html
            var retHtml = htmlTemplate
                .replace('*url*', url)
                .replace('*classes*', classes)
                .replace('*icon*', icon)
                .replace('*title*', title)
                .replace('*data*', data);
            return retHtml;

            var htmlTemplate =
                '<a href="#" class="*classes*" rel="tooltip" title="*title*" *data*>' +
                    '<i class="fa *icon* fa-fw"></i>' +
                '</a>';
        }

        function editColumn(data, type, oObj) {
            var htmlTemplate =
            '<a href="#" class="*classes*" rel="tooltip" title="*title*" *data*>' +
                    '<i class="fa *icon* fa-fw"></i>'
            '</a>';

            // Url, Classes, icon, attributes, and title
            var classes = 'tableIconLink js-edit-requirement';
            var title = requirementResources.editRequirement;
            var icon = 'fa-pencil-square-o';
            var data =
                'data-placement="top" ' +
                'data-toggle="modal" ' +
                'data-target="#editRequirementsModal" ' +
                'data-backdrop="static" ' +
                'data-keyboard="false" ';

            // Set html
            var retHtml = htmlTemplate
                .replace('*classes*', classes)
                .replace('*title*', title)
                .replace('*icon*', icon)
                .replace('*data*', data);
            return retHtml;
        }

        function removeColumn(data, type, oObj) {
            var htmlTemplate =
                '<button *disabled* class="*buttonClasses*" *data*>' +
                    '<a href="#" rel="tooltip" data-placement="top" title="*title*">' +
                        '<i class="fa *icon* fa-fw"></i>' +
                    '</a>' +
                '</button>';

            // Url, Classes, icon, attributes, and title 
            var disabled = oObj.ThereAreTasks ? 'disabled="disabled"' : '';
            var buttonClasses = 'hp-button-no-style js-delete-requirement tableIconLink disabledDelete';
            var icon = 'fa-trash-o';
            var title = requirementResources.deleteRequirement;
            var data =
                'data-toggle="modal"' +
                'data-target="#deleteRequirementModal"' +
                'data-backdrop="static"' +
                'data-keyboard="false"';

            // Set html
            var retHtml = htmlTemplate
                .replace('*disabled*', disabled)
                .replace('*buttonClasses*', buttonClasses)
                .replace('*icon*', icon)
                .replace('*title*', title)
                .replace('*data*', data);
            return retHtml;
        }

        // Datatables initialization
        $('#dataTables-requirements').DataTable({
            "bServerSide": true,
            "sAjaxSource": appResources.dataTableRequirementsAjaxHandler,
            "bProcessing": true,
            "aoColumns": [
                            { "sName": "Name", "mRender": nameColumn, "width": "25%" },
                            { "sName": "CreationDate", "mRender": creationDateColumn, "width": "20%" },
                            { "sName": "Status", "mRender": statusColumn, "bSortable": false },
                            { "sName": "Tasks", "mRender": taskColumn, "sClass": "helper-center", "width": "5%" },
                            { "sName": "Statistics", "mRender": statisticColumn, "sClass": "helper-center", "width": "5%" },
                            { "sName": "Edit", "mRender": editColumn, "sClass": "helper-center", "width": "5%" },
                            { "sName": "Remove", "mRender": removeColumn, "sClass": "helper-center", "width": "5%" }
            ],
            responsive: true,
            "order": [[creationDateFieldIndex, "desc"]],
        });
    }

    $(main);

})(jQuery);