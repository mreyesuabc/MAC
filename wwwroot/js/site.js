// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
document.addEventListener("DOMContentLoaded", function () {
    window.openModal = function () {
        const modal = document.getElementById("myModal");
        if (modal) {
            modal.classList.add("show");
        } else {
            console.error("No se encontró el modal con id 'myModal'");
        }
    };

    window.closeModal = function () {
        const modal = document.getElementById("myModal");
        if (modal) {
            modal.classList.remove("show");
        }
    };
});