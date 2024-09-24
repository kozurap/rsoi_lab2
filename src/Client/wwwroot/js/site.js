// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function () {
    $('.nav-link-collapse').on('click', function () {
        $('.nav-link-collapse').not(this).removeClass('nav-link-show');
        $(this).toggleClass('nav-link-show');
    });

    //Music List
    function getTrackTable(data, status) {
        let str;
        data.map(item => str += `<tr><td>${item.name}</td><td>${item.author}</td><td>${item.date}</td></tr>`);
        $("#MusData").append(str)
    };

    $.ajax({
        type: "GET",
        url: "https://" + document.location.host + "/Music/GetList",
        success: (data, status) => { getTrackTable(data, status) }
    });

    //Music Search by Author
    function getSRecTable(data, status) {
        var table = document.getElementById("SrcMusTable");
        while (table.rows.length > 0) {
            table.deleteRow(0);
        }
        let str;
        data.map(item => str += `<tr><td>${item.name}</td><td>${item.author}</td><td>${item.date}</td></tr>`);
        $("#MusSData").append(str)
    };
    $("#FindGroup").on("click", function () {
        var author = document.getElementById("SearchMus").value;
        console.log(JSON.stringify(author));
        console.log(author);
        $.ajax({
            type: "GET",
            data: {
                author: author
            },
            contentType: "application/json; charset=utf-8",
            url: "/Music/SearchByAuthor",
            success: function (data, status) { getSRecTable(data, status) }
        });
        return false;
    });

    //Expelled List

    function getExpTable(data, status) {
        let str;
        data.map(item => str += `<tr><td>${item.name}</td><td>${item.professorName}</td><td>${item.description}</td></tr>`);
        $("#ExpData").append(str)
    };

    $.ajax({
        type: "GET",
        url: "https://" + document.location.host + "/Expired/GetList",
        success: (data, status) => { getExpTable(data, status) }
    });

    //Expelled Search By Professor

    function getSExpTable(data, status) {
        var table = document.getElementById("SrcExpTable");
        while (table.rows.length > 0) {
            table.deleteRow(0);
        }
        let str;
        data.map(item => str += `<tr><td>${item.name}</td><td>${item.professorName}</td><td>${item.description}</td></tr>`);
        $("#ExpSData").append(str)
    };
    $("#FindExp").on("click", function () {
        var author = document.getElementById("SearchProf").value;
        console.log(JSON.stringify(author));
        console.log(author);
        $.ajax({
            type: "GET",
            data: {
                professor: author
            },
            contentType: "application/json; charset=utf-8",
            url: "https://" + document.location.host + "/Expired/SearchByProfessor",
            success: (data, status) => { getSExpTable(data, status) }
        });
        return false;
    });
});