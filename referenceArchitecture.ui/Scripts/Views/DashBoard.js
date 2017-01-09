/// <reference path="../jquery-1.10.2.js" />
//Variables globales

(function ($) {
    // Function example
    function test() {
        return true;
    }

    // Eventos
    function main() {

        // Click in body: Showing messages from Resources (See RoutesAndMessages.cshtml)
        $('.js-show-resources').click(function (e) {
            e.preventDefault();
            var jumbotron = $('.jumbotron');
            
            jumbotron.append('<p class="lead">' + appResources.message1 + '</>');
            jumbotron.append('<p class="lead">' + appResources.message2 + '</>');

            jumbotron.append('<p class="lead">' + appResources.url1 + '</>');
            jumbotron.append('<p class="lead">' + appResources.url2 + '</>');
            
        });

        //// El evento de este botón se indica con un "Delegated Events"
        //$('body').on('click', '.js-guardar', function () {

        //    // Indicamos que SI se debe guardar
        //    accionGuardar = true;

        //    $('#lnkEditFecha-1').click();
        //});
    }


    window.thisApp = window.thisApp || {};

    // Sacamos "test" fuera para poderlo llamar desde otro archivo de javaScript
    window.thisApp.test = test;

    // This is call this way: constantesApp.onSuccessConstantesTest
    window.thisApp.onSuccessConstantesTest = function (paraemeter1, parameter2) {

    };

    $(main);

})(jQuery);