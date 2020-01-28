// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
listen = (id) => {
    const socket = new WebSocket(`ws://localhost:64415/Home/${id}`);

    socket.onmessage = event => {
        const statusDiv = document.getElementById("status");
        statusDiv.innerHTML = JSON.parse(event.data);
    };

    document.getElementById("submit").addEventListener("click", e => {
        e.preventDefault();

        const product = document.getElementById("product").value;

        const size = document.getElementById("size").value;

        fetch("/Home/Coffe",
                {
                    method: "POST",
                    body: { product, size }
                }).then(response => response.text())
            .then(text => listen(text));

    });
}