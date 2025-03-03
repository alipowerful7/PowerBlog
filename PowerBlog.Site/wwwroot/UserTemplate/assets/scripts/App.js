const nav = document.querySelector(".mobile-menu"),
    openNavBtn = document.querySelector(".mobile-menu__open-icon"),
    closeNavBtn = document.querySelector(".mobile-menu__close-icon"),
    openSubmenuBtn = document.querySelector(".open-submenu"),
    submenu = document.querySelector(".submenu"),
    arrowsubmenu = document.querySelector(".arrow-submenu"),
    overlay = document.querySelector(".overlay"),
    openShoppingCartButtons = document.querySelectorAll(
        ".open-shopping-cart__btn"
    ),
    shoppingCart = document.querySelector(".shopping-cart"),
    closeShoppingCartButtons = document.querySelectorAll(
        ".close-shopping-cart__btn"
    ),
    alertElem = document.querySelector(".top-alert"),
    alertBtnElem = document.querySelector(".close-alert-btn"),
    toggleThemeBtns = document.querySelectorAll(".toggle-theme");
const openNav = () => {
    overlay.classList.remove("hidden");
    overlay.classList.add("flex");
    nav.classList.remove("-right-64");
    nav.classList.add("right-0");
};
const openShoppingCart = () => {
    overlay.classList.remove("hidden");
    overlay.classList.add("flex");
    shoppingCart.classList.remove("-left-72");
    shoppingCart.classList.add("left-0");
};
const closeNav = () => {
    overlay.classList.add("hidden");
    overlay.classList.remove("flex");
    nav.classList.remove("right-0");
    nav.classList.add("-right-64");
};
const closeShoppingCart = () => {
    overlay.classList.add("hidden");
    overlay.classList.remove("flex");
    shoppingCart.classList.add("-left-72");
    shoppingCart.classList.remove("left-0");
};
const toggleSubmenu = () => {
    openSubmenuBtn.classList.toggle("text-green-500");
    submenu.classList.toggle("hidden");
    submenu.classList.toggle("flex");
    arrowsubmenu.classList.toggle("-rotate-90");
};
const closeAlert = () => {
    alertElem.classList.add("hidden");
};
const toggleTheme = () => {
    if (localStorage.theme === "dark") {
        document.documentElement.classList.remove("dark");
        localStorage.theme = "light";
    } else {
        document.documentElement.classList.add("dark");
        localStorage.setItem("theme", "dark");
    }
};
openNavBtn.addEventListener("click", openNav);
openShoppingCartButtons.forEach((button) => {
    button.addEventListener("click", openShoppingCart);
});
closeNavBtn.addEventListener("click", closeNav);
closeShoppingCartButtons.forEach((button) => {
    button.addEventListener("click", closeShoppingCart);
});
overlay.addEventListener("click", () => {
    closeNav();
    closeShoppingCart();
});
openSubmenuBtn.addEventListener("click", toggleSubmenu);
//alertBtnElem.addEventListener("click", closeAlert);
toggleThemeBtns.forEach((btn) => {
    btn.addEventListener("click", toggleTheme);
});
const accordionHeaders = document.querySelectorAll(".accordion-header");
accordionHeaders.forEach((header) => {
    header.addEventListener("click", function () {
        const content = this.nextElementSibling,
            icon = this.querySelector("svg");
        content.classList.contains("hidden")
            ? (content.classList.remove("hidden"),
                content.classList.add("block"),
                icon.classList.add("rotate-90"))
            : (content.classList.remove("block"),
                content.classList.add("hidden"),
                icon.classList.remove("rotate-90"));
    });
});
document.addEventListener("DOMContentLoaded", () => {
    const input = document.getElementById("customInput"),
        incrementButton = document.querySelector(".increment"),
        decrementButton = document.querySelector(".decrement");
    incrementButton?.addEventListener("click", () => {
        if (input.value < 20) {
            input.value = parseInt(input.value) + 1;
        }
    });
    decrementButton?.addEventListener("click", () => {
        if (input.value > 1) {
            input.value = parseInt(input.value) - 1;
        }
    });
});
const moreCommentBtn = document.querySelector(".more-comment-btn"),
    moreCommentText = document.querySelector(".more-comment-text"),
    moreCommentIcon = document.querySelector(".more-comment-icon"),
    hiddenCommentItems = document.querySelectorAll(".hidden-comment-item");
if (moreCommentBtn) {
    moreCommentBtn.addEventListener("click", () => {
        hiddenCommentItems.forEach((item) => {
            item.classList.toggle("hidden");
            item.classList.toggle("block");
        });
        moreCommentText.innerHTML =
            moreCommentText.innerHTML === "مشاهده بیشتر"
                ? "مشاهده کمتر"
                : "مشاهده بیشتر";
        moreCommentIcon.classList.toggle("rotate-180");
    });
}
const priceElements = document.querySelectorAll(".price-input p"),
    rangeInputs = document.querySelectorAll(".range-input input"),
    range = document.querySelector(".slider-bar .progress");
let priceGap = 1000;
function formatNumber(num) {
    return num.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
}
rangeInputs.forEach((input) => {
    input.addEventListener("input", (e) => {
        let minVal = parseInt(rangeInputs[0].value) * 10,
            maxVal = parseInt(rangeInputs[1].value) * 10;
        if (maxVal - minVal < priceGap) {
            e.target.className === "min-range"
                ? (rangeInputs[0].value = (maxVal - priceGap) / 10)
                : (rangeInputs[1].value = (minVal + priceGap) / 10);
        } else {
            priceElements[0].textContent = formatNumber(minVal);
            priceElements[1].textContent = formatNumber(maxVal);
            range.style.left =
                (rangeInputs[0].value / rangeInputs[0].max) * 100 + "%";
            range.style.right =
                100 - (rangeInputs[1].value / rangeInputs[1].max) * 100 + "%";
        }
    });
});
