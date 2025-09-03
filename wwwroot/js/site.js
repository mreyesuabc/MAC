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
// site.js

function mostrarMensajeResultado(tipo, mensaje) {
    const mensajeDiv = document.getElementById("mensajeResultado");

    if (!mensajeDiv) return;

    // Limpiar clases anteriores
    mensajeDiv.className = "alert"; // Clase base

    // Asignar clase según el tipo
    switch (tipo) {
        case "success":
            mensajeDiv.classList.add("alert-success");
            break;
        case "error":
        case "danger":
            mensajeDiv.classList.add("alert-danger");
            break;
        default:
            mensajeDiv.classList.add("alert-info");
            break;
    }

    // Mostrar el mensaje
    mensajeDiv.textContent = mensaje;
    mensajeDiv.style.display = "block";

    // Ocultar después de 2 segundos
    setTimeout(() => {
        mensajeDiv.style.display = "none";
    }, 2000);
}
