const form = document.querySelector('form');

window.addEventListener("click", (ev) => {
    const target = ev.target;
    const action = target.dataset.action;

    if (!action) {
        return;
    }

    switch (action) {
        case "Submit":
            form.submit();
            break;
        case "RemoveRepository":
            const parent = target.parentElement;

            parent.remove();
            break;

        case "AddRepository":
            const repositoryList = document.querySelector("#repositories");

            const li = document.createElement("li");
            const template = document.getElementById("repository-template");
            let html = template.innerHTML;
            const count = repositoryList.querySelectorAll("li").length;

            console.log(count)

            html = html.replace(/Config\.Repositories\[(\d)]/gm, (match, p1) => {
                console.log(match)
                return `Config.Repositories[${count}]`;
            });


            console.log(html);
            const newItem = document.createElement("span");
            newItem.innerHTML = html;

            li.appendChild(newItem);

            repositoryList.appendChild(li)
            break;

    }
})
