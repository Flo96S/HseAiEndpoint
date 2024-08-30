const eventTarget = new EventTarget();
eventTarget.addEventListener('data', (event) => {
    console.log('Received data:', event.detail);
});


async function send() {
    const loader = document.getElementById("loader");
    const el = document.getElementById("text").value;
    const output = document.getElementById("data");

    loader.classList.remove("hidden");

    output.value = "";

    const obj = {
        "text": el
    };

    const result = await fetch("https://localhost:7184/chat", {
        method: 'POST',
        body: JSON.stringify(obj),
        headers: {
            'Content-Type': 'application/json'
        }
    }).then(async (res) => {
        if (res.ok) {
            const result = await res.text();
            return result;
        }
    }).catch(err => {
        console.log(err);
        output.value = err;
    });
    typeWriterEffect(output, result)
    loader.classList.add("hidden");
}

function typeWriterEffect(element, text, speed = 10) {
    let index = 0;

    function type() {
        if (index < text.length) {
            element.value += text.charAt(index);
            index++;
            setTimeout(type, speed);
        }
    }

    type();
}