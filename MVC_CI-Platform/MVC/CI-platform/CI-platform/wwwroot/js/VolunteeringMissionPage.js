const tabs = (id) => {
    let active_mission = document.getElementsByClassName("active-detail")
    active_mission[0].classList.add("detail")
    active_mission[0].classList.remove("active-detail")
    let user_mission = document.getElementsByClassName(id)

    user_mission[0].classList.remove("detail")
    user_mission[0].classList.add("active-detail")
    let element = document.getElementById(id)

    let remaining_tab = document.getElementsByClassName("tab")
    for (let i = 0; i < remaining_tab.length; i++) {
        if (!remaining_tab[i].classList.contains("inner-content")) {
            remaining_tab[i].classList.add("inner-content")
        }
    }
    element.classList.remove("inner-content")
}