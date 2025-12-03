function previewImage(event) {
    const reader = new FileReader();
    const preview = document.getElementById("preview");

    reader.onload = function () {
        preview.src = reader.result;
        preview.style.display = "block";
    };

    reader.readAsDataURL(event.target.files[0]);
}