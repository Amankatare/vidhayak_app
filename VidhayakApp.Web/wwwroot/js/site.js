// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
//const sideLinks = document.querySelectorAll('.sidebar .side-menu li a:not(.logout)');

//sideLinks.forEach(item => {
//    const li = item.parentElement;
//    item.addEventListener('click', () => {
//        sideLinks.forEach(i => {
//            i.parentElement.classList.remove('active');
//        })
//        li.classList.add('active');
//    })
//});

//const menuBar = document.querySelector('.content nav .bx.bx-menu');
//const sideBar = document.querySelector('.sidebar');

//menuBar.addEventListener('click', () => {
//    sideBar.classList.toggle('close');
//});

//const searchBtn = document.querySelector('.content nav form .form-input button');
//const searchBtnIcon = document.querySelector('.content nav form .form-input button .bx');
//const searchForm = document.querySelector('.content nav form');

//searchBtn.addEventListener('click', function (e) {
//    if (window.innerWidth < 576) {
//        e.preventDefault();
//        searchForm.classList.toggle('show');
//        if (searchForm.classList.contains('show')) {
//            searchBtnIcon.classList.replace('bx-search', 'bx-x');
//        } else {
//            searchBtnIcon.classList.replace('bx-x', 'bx-search');
//        }
//    }
//});

//window.addEventListener('resize', () => {
//    if (window.innerWidth < 768) {
//        sideBar.classList.add('close');
//    } else {
//        sideBar.classList.remove('close');
//    }
//    if (window.innerWidth > 576) {
//        searchBtnIcon.classList.replace('bx-x', 'bx-search');
//        searchForm.classList.remove('show');
//    }
//});

//const toggler = document.getElementById('theme-toggle');

//toggler.addEventListener('change', function () {
//    if (this.checked) {
//        document.body.classList.add('dark');
//    } else {
//        document.body.classList.remove('dark');
//    }
//});


//        /*

//        AppUser.cshtml

//        */

//$(document).ready(function () {
//    // Activate tooltip
//    $('[data-toggle="tooltip"]').tooltip();

//    // Select/Deselect checkboxes
//    var checkbox = $('table tbody input[type="checkbox"]');
//    $("#selectAll").click(function () {
//        if (this.checked) {
//            checkbox.each(function () {
//                this.checked = true;
//            });
//        } else {
//            checkbox.each(function () {
//                this.checked = false;
//            });
//        }
//    });
//    checkbox.click(function () {
//        if (!this.checked) {
//            $("#selectAll").prop("checked", false);
//        }
//    });
//});


document.addEventListener('DOMContentLoaded', function () {
    const sideLinks = document.querySelectorAll('.sidebar .side-menu li a:not(.logout)');
    const sideBar = document.querySelector('.sidebar');
    const menuBar = document.querySelector('.content nav .bx.bx-menu');

    sideLinks.forEach(item => {
        item.addEventListener('click', () => {
            document.querySelectorAll('.sidebar .side-menu li').forEach(li => {
                li.classList.remove('active');
            });
            item.parentElement.classList.add('active');
        });
    });

    menuBar.addEventListener('click', () => {
        sideBar.classList.toggle('close');
    });

    const searchBtn = document.querySelector('.content nav form .form-input button');
    const searchBtnIcon = document.querySelector('.content nav form .form-input button .bx');
    const searchForm = document.querySelector('.content nav form');

    searchBtn.addEventListener('click', function (e) {
        e.preventDefault();
        if (window.innerWidth < 576) {
            searchForm.classList.toggle('show');
            searchBtnIcon.classList.toggle('bx-search');
            searchBtnIcon.classList.toggle('bx-x');
        }
    });

    window.addEventListener('resize', () => {
        if (window.innerWidth < 768) {
            sideBar.classList.add('close');
        } else {
            sideBar.classList.remove('close');
        }
        if (window.innerWidth > 576) {
            searchBtnIcon.classList.replace('bx-x', 'bx-search');
            searchForm.classList.remove('show');
        }
    });

    const toggler = document.getElementById('theme-toggle');

    if (toggler) {
        toggler.addEventListener('change', function () {
            document.body.classList.toggle('dark', this.checked);
        });
    }

    // Table Checkbox Select/Deselect
    const checkbox = $('table tbody input[type="checkbox"]');
    const selectAllCheckbox = $("#selectAll");

    selectAllCheckbox.click(function () {
        checkbox.prop("checked", this.checked);
    });

    checkbox.click(function () {
        selectAllCheckbox.prop("checked", checkbox.length === checkbox.filter(":checked").length);
    });
});

//Translator code :-

<script type="text/javascript">
    function googleTranslateElementInit() {
        new google.translate.TranslateElement(
            { pageLanguage: 'en', layout: google.translate.TranslateElement.InlineLayout.SIMPLE },
            'google_translate_element'
        );
    }

    function toggleLanguage() {
        // Get the current language code
        var currentLanguage = google.translate.TranslateElement.getInstance().getLang();

        // Toggle between English ('en') and Hindi ('hi')
        var targetLanguage = (currentLanguage === 'en') ? 'hi' : 'en';

        // Change the language
        google.translate.TranslateElement.getInstance().changeLanguage(targetLanguage);
    }
</script>

<script type="text/javascript" src="//translate.google.com/translate_a/element.js?cb=googleTranslateElementInit"></script>

