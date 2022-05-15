
function submit_json() {
    let content = document.getElementById('content').value;
    let title = document.getElementById('title').value;
    const xreq = new XMLHttpRequest();
    const payload = {title: title, content: content};

    xreq.onload = function (){
        if(xreq.status >= 200 && xreq.status < 300) {

        } else if(xreq.status == 301) {
            window.location.href = xreq.getResponseHeader("Location");
        } else {
            let error = JSON.parse(xreq.responseText).error
            document.getElementById("error").innerHTML = error;
            document.getElementById("error").style.display = 'block';
        }
    };

    xreq.open("POST", "/v1/post");
    xreq.setRequestHeader("Content-type", "application/json");
    xreq.send(JSON.stringify(payload));

}
