function onClick() {
    let input = document.getElementById("files");
    const formData = new FormData();

    for (const file of input.files) {
        formData.append('files', file, file.name);
    }     

    fetch("/api/images/image", {
        method: "POST",
        body: formData
    }).then(response => {
        response.json().then(result => {
            console.log(result);
            result.ids.forEach(id => {
                const img = document.createElement('img');
                img.src = "/api/images/image?id=" + id;
                document.getElementById('images').appendChild(img);
            });
        })
    })
}