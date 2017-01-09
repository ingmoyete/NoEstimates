/// <reference path="../Vendor/_references.js" />

(function ($) {
    // Datatable selectors
    var creationDateFieldIndex = 0; // 0 base index
    var dataTableSelector = '#dataTables-example';

    // Selectors
    var insertFormSelector = '.modalForm-insertProjectModal';
    var editFormSelector = '.modalForm-editProjectModal';
    var submitButtonSelector = '.js-submit';

    // Objects
    function project(ProjectId, Name, Description){
        this.ProjectId = ProjectId;
        this.Name = Name;
        this.Description = Description;
    }

    // Methods =========================
    function getProjectFromInfoRow($tableButton){
        // Get project information
        var $projectInfoRow = $tableButton.parent().parent().find('.projectInfo');
        var projectId = $projectInfoRow.data('project-id');
        var projectName = $projectInfoRow.text();
        var projectDescription = $projectInfoRow.attr('title');

        // Return project object
        var retProject = new project(projectId, projectName, projectDescription);
        return retProject;
    }

    // Eventos =========================
    function main() {
        // Click on the delete icon: the project name is populated in the confirmation popup
        $('body').on('click', '.js-delete-project', function (e) {
            e.preventDefault();

            // Get project name from project info row
            var project = getProjectFromInfoRow($(this));
            
            // Set the id of the project in the hidden field of the delete form
            var $form = $('.modalForm-deleteProjectModal');
            $form.find('.Id').val(project.ProjectId);

            // Replace project name in the resource
            var body = $form.find('.modal-body');   
            var newText = body.data('text').replace('projectName', project.Name);
            body.text(newText);
            body.append('<label class="control-label"></label><input type="hidden" name="SummaryError" class="form-control">');
        });

        // Submit delete form: delete the project
        $('body').on('submit', '.modalForm-deleteProjectModal', function () {

            // Delete project
            var url = appResources.deleteProject;
            $form = $(this);
            commonJs.submitFormAndGetJson(url, $form,
                function (data, status) {
                    commonJs.onSuccessSubmitingPopupForm($form, appResources.allProjectsUrl, submitButtonSelector);
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

        // Click on edit project: open the edit popup and populate information
        $('body').on('click', '.js-edit-project', function () {
            // Get project information
            var $this = $(this);

            // Get project name from project info row
            var project = getProjectFromInfoRow($(this));

            // Set the project information in the form
            var $editForm = $(editFormSelector);
            $editForm.find('[name="Id"]').val(project.ProjectId);
            $editForm.find('[name="Name"]').val(project.Name);
            $editForm.find('[name="Description"]').val(project.Description);

        });

        // Submit edit form: edit a project
        $(editFormSelector).submit(function () {

            // Get form
            var $form = $(this);

            // Make request
            commonJs.submitFormAndGetJson(appResources.updateProjectUrl, $form,
                // Success
                function (data, status) {
                    commonJs.onSuccessSubmitingPopupForm($form, appResources.allProjectsUrl, submitButtonSelector);
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
            commonJs.submitFormAndGetJson(appResources.createProjectUrl, $form,
                // Success
                function (data, status) {
                    commonJs.onSuccessSubmitingPopupForm($form, appResources.allProjectsUrl, submitButtonSelector);
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

        // Click on cancel button: clean all erros of the modal form
        $('.js-closeModal').click(function () {
            var $insertform = $(insertFormSelector);
            commonJs.removeAllErrorsForm($insertform);
            commonJs.resetAllFieldsInForm($insertform);

            var $editform = $(editFormSelector);
            commonJs.removeAllErrorsForm($editform);
            commonJs.resetAllFieldsInForm($editform);
        });


        // Datatables colums template =========================================
        function nameColumn(data, type, oObj) {
            var htmlTemplate =
                '<span class="projectInfo" rel="tooltip" title="*description*" data-project-id="*projectId*" *data*>*name*</span>';

            // Url, classes, icon, and title
            var projectId = oObj.ProjectId;
            var data =  'data-placement="top"';
            var description = oObj.Description;
            var name = oObj.Name;

            // Set html
            var retHtml = htmlTemplate
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
            //var status = '30';
            var status = oObj.Status;

            // Set html
            var retHtml = htmlTemplate
                .replace(/\#/g, status);
            return retHtml;
        }

        function requirementsColumn(data, type, oObj) {
            var htmlTemplate =  
                '<a href="*url*" class="*classes*" rel="tooltip" title="*title*" *data*>' +
                    '<i class="fa *icon* fa-fw"></i>' +
                '</a>';

            // Url, classes, icon, and title
            var url = appResources.getRequirementsByProject.replace('projectId', oObj.ProjectId);
            var classes = 'tableIconLink';
            var icon = 'fa-sitemap';
            var title = projectResources.toRequirements;
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
            var url = appResources.projectStatistic
                .replace('projectId', oObj.ProjectId)
                .replace('returnUrl', window.location.href);

            var classes = 'tableIconLink';
            var icon = 'fa-bar-chart-o';
            var title = projectResources.projectStatistics;
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
        }

        function editColumn(data, type, oObj) {
            var htmlTemplate = 
            '<a href="#" class="*classes*" rel="tooltip" title="*title*" *data*>' +
                    '<i class="fa *icon* fa-fw"></i>'
            '</a>';

            // Url, Classes, icon, attributes, and title
            var classes = 'tableIconLink js-edit-project';
            var title = projectResources.editProject;
            var icon = 'fa-pencil-square-o';
            var data = 
                'data-placement="top"' +
                'data-toggle="modal"' +
                'data-target="#editProjectModal"'+
                'data-backdrop="static"' +
                'data-keyboard="false"';

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
            var disabled = oObj.ThereAreRequirements ? 'disabled="disabled"' : '';
            var buttonClasses = 'hp-button-no-style js-delete-project tableIconLink disabledDelete';
            var icon = 'fa-trash-o';
            var title = projectResources.deleteProject;
            var data =
                'data-toggle="modal"' +
                'data-target="#deleteProjectModal"' +
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
        $(dataTableSelector).DataTable({
            "bServerSide": true,
            "sAjaxSource": appResources.dataTableProjectsAjaxHandler,
            "bProcessing": true,
            "aoColumns": [
                            { "sName": "Name", "mRender": nameColumn, "width": "25%" },
                            { "sName": "CreationDate", "mRender": creationDateColumn, "width": "20%" },
                            { "sName": "Status", "mRender": statusColumn, "bSortable": false },
                            { "sName": "Requirements", "mRender": requirementsColumn, "sClass": "helper-center", "width": "5%" },
                            { "sName": "Statistics", "mRender": statisticColumn, "sClass": "helper-center", "width": "5%" },
                            { "sName": "Edit", "mRender": editColumn, "sClass": "helper-center", "width": "5%" },
                            { "sName": "Remove", "mRender": removeColumn, "sClass": "helper-center", "width": "5%" }
            ],
            responsive: true,
            "order": [[creationDateFieldIndex, "desc"]]
        });
    }

    $(main);

})(jQuery);