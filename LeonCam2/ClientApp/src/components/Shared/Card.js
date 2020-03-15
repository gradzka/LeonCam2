export function pushCard(event) {
    let card_alt = event.target.parentNode;
    card_alt.classList.remove('hidden');
    Array.from(card_alt.getElementsByClassName('input-container')).forEach(element => element.classList.remove('hidden'));

    event.target.innerHTML = "";
}

export function popCard(innerHTML, event) {
    let card_alt = event.target.parentNode.parentNode;
    card_alt.classList.add('hidden');
    Array.from(card_alt.getElementsByClassName('input-container')).forEach(element => element.classList.add('hidden'));

    card_alt.childNodes[0].innerHTML = innerHTML;
}