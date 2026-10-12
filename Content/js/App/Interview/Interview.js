$(document).ready(function () {
    LoadData();
    AddFilter();
    $('body').tooltip({ selector: '[data-toggle="tooltip"]' });

});
function LoadData() {
    var drawtable = $('#Tasks').DataTable({
        "ajax": {
            "url": "/Interview/GetAllTasks",
            "type": "POST",
            "dataType": "JSON",
            "data": function (d) {
                d.filterType = $("#filter_SearchFor").val();
            }
        },
        success: function (data) {
            // Request success
        },
        dom: 'Blfrtip',
        lengthMenu: [
            [10, 25, 50, 100, -1],
            ['10', '25', '50', '100', 'Show all']
        ], "bAutoWidth": false,
        "bServerSide": true,
        "bProcessing": true,
        "bSearchable": true,
        "order": [
            [1, 'asc']
        ],
        "language": {
            "emptyTable": "No record found.",
        },
        "columns": [{
            "data": function (nRow, Data, iDisplayIndex) {
                $("td:first", nRow).html(iDisplayIndex + 1);
                return nRow;
            },
        }, {
            "data": "TaskNumber"
        },
        {
            "data": "Title"
        },

        {
            "data": "AssignedToPanel"
        },
        {
            "data": "TechnologyTitle",

        },
        {
            "data": "PositionTitle"
        },
        {
            "data": "InterviewRoundName"
        },
        {
            "data": "StatusTitle"
        },
        {
            "data": function (data) {
                var dateObject = parseInt(data.CreatedDate.replaceAll("/", "").replace("Date(", "").replace(")", ""));
                var d = new Date(dateObject);
                var day = d.getDate();
                var month = d.getMonth() + 1;
                var year = d.getFullYear();
                if (day < 10) {
                    day = "0" + day;
                }
                if (month < 10) {
                    month = "0" + month;
                }
                var date = day + "/" + month + "/" + year;

                return date;
            }
        },
        {
            "data": function (data) {
                console.log(data);
                if (data.StatusTitle == "Assigned" &&  data.IsActive) {
                    return "<a href='javascript:void(0)' id='btnview_" + data.Id + "' class='btnStartOnboarding pr-2' data-toggle=\"tooltip\" data-placement=\"top\" title=\"Click here to view details\"><i class='fa fa-eye'></i></a> <a href='javascript:void(0)' id='btnEdit_" + data.Id + "' class='btnRegenarateToken'  data-toggle=\"tooltip\" data-placement=\"top\" title=\"Click here to edit details\"><i class='fa fa-edit'></i> </a>";
                }
                else if (data.StatusTitle == "Acknowledged" && data.IsActive) {
                    return "<a href='javascript:void(0)' id='btnview_" + data.Id + "' class='btnStartOnboarding pr-2' data-toggle=\"tooltip\" data-placement=\"top\" title=\"Click here to view details\"><i class='fa fa-eye'></i></a> <a href='javascript:void(0)' id='btnActionOnTask_" + data.Id + "' class='btnRegenarateToken'  data-toggle=\"tooltip\" data-placement=\"top\" title=\"Click here to Schedule Interview\"><i class='fa fa-clock'></i> </a>";
                }
                else if (data.StatusTitle == "Profile Selected" && data.IsActive){
                    return "<a href='javascript:void(0)' id='btnview_" + data.Id + "' class='btnStartOnboarding pr-2' data-toggle=\"tooltip\" data-placement=\"top\" title=\"Click here to view details\"><i class='fa fa-eye'></i></a> <a href='javascript:void(0)' id='btnActionOnTask_" + data.Id + "' class='btnRegenarateToken'  data-toggle=\"tooltip\" data-placement=\"top\" title=\"Click here to Schedule  or update status  for interview\"><i class='fa fa-clock'></i> </a>";
                }
                else if (data.StatusTitle == "Interview Scheduled" && data.IsActive) {
                    return "<a href='javascript:void(0)' id='btnview_" + data.Id + "' class='btnStartOnboarding pr-2' data-toggle=\"tooltip\" data-placement=\"top\" title=\"Click here to view details\"><i class='fa fa-eye'></i></a> <a href='javascript:void(0)' id='btnRedirectTask_" + data.Id + "' class='btnRegenarateToken'  data-toggle=\"tooltip\" data-placement=\"top\" title=\"Click here to Transfer or redirect Task to other person\"><span class='fa-stack'><i class='fa fa-exchange fa-stack-inverse'></i> <i class='fa fa-user-plus fa-stack-1x'></i> </span></a>";
                }
                else {
                    return "<a href='javascript:void(0)' id='btnview_" + data.Id + "' class='btnStartOnboarding pr-2' data-toggle=\"tooltip\" data-placement=\"top\" title=\"Click here to view details\"><i class='fa fa-eye'></i></a>";
                }
            }
        }
        ],
        "columnDefs": [{
            "searchable": false,
            "orderable": false,
            "width": "6%",
            "targets": 0,
        }, {
            "width": "12%",
            "targets": 1,
        },
        {
            "width": "15%",
            "targets": 1,
        },
        {
            "width": "17%",
            "targets": 2,
        },
        {
            "width": "15%",
            "targets": 3,

        },
        {
            "width": "10%",
            "targets": 4,
        },
        {
            "width": "10%",
            "targets": 5,
        },
        {
            "width": "10%",
            "targets": 6,

        }, {
            "width": "5%",
            "targets": 7,
            "searchable": false,
            "orderable": false,

        }],
    });
    $(drawtable.table().container()).removeClass('form-inline');

    drawtable.on('order.dt search.dt', function () {
        drawtable.column(0, {
            search: 'applied',
            order: 'applied'
        }).nodes().each(function (cell, i) {
            cell.innerHTML = i + 1;
        });
    }).draw();
}
function AddFilter() {

    var select = $('<select id="filter_SearchFor" class="form-control form-control-sm"><option value=""></option></select>')
    var counter = 0;
    $("#Tasks").DataTable().columns().every(function () {
        counter++;
        if ($(this.header())[0].className != "sorting_disabled") {
            var d = $(this.header())[0].innerText;
            if (d != '') {
                select.append('<option value="' + counter + '">' + d + '</option>')
            }
        }
    });
    var currentdata = $("#Tasks_filter label");
    currentdata.html().replace("Search:", "");
    currentdata.html = "";

    $(select).insertBefore("#Tasks_filter label input");
    $("#Tasks_filter label input").attr("class", "form-control form-control-sm");
    $("#filter_SearchFor").change(function (e) {

        if (($(this)[0].value) == '') {
            $("#" + $(this)[0].id + "+ input").attr("disabled", "disabled")
        } else {
            $("#" + $(this)[0].id + "+ input").removeAttr("disabled")
        }
    });

}
$("body").on("click", "[id*='btnview_']", function (e) {
    debugger;
    var selectedItem = $(this)[0].id;
    var _id = selectedItem.split('_')[1];
    $("#spinner").fadeIn("slow");
    $.ajax({
        type: "POST",
        url: "/Interview/ViewTask",
        data: { Id: _id },
        dataType: "html",
        success: function (response) {
            try {
                var resultdata = JSON.parse(response)
                if (resultdata.Status == Status.Unauthrizedcode) {
                    window.location.href = url.LoginPageUrl
                }
            }
            catch (err) { }
            $('#TaskModal').html(response);
            $('#dialog').modal('show');
        },
        failure: function (response) {
            alert(response.responseText);
        },
        error: function (response) {
            alert(response.responseText);
        }
    });
    $("#spinner").fadeOut("slow");
});
$("body").on("click", "[id*='btnEdit_'],[id*='btnSheduleInterview_']", function (e) {
    debugger;
    var selectedItem = $(this)[0].id;
    var _id = selectedItem.split('_')[1];
    $("#spinner").fadeIn("slow");
    $.ajax({
        type: "POST",
        url: "/Interview/EditTask",
        data: { Id: _id },
        dataType: "html",
        success: function (response) {
            try {
                var resultdata = JSON.parse(response)
                if (resultdata.Status == Status.Unauthrizedcode) {
                    window.location.href = url.LoginPageUrl
                }
            }
            catch (err) { }
            $('#TaskModal').html(response);
            $('#dialog').modal('show');
        },
        failure: function (response) {
            alert(response.responseText);
        },
        error: function (response) {
            alert(response.responseText);
        }
    });
    $("#spinner").fadeOut("slow");
});
$("body").on("click", "[id*='btnActionOnTask_']", function (e) {
    debugger;
    var selectedItem = $(this)[0].id;
    var _id = selectedItem.split('_')[1];
    $("#spinner").fadeIn("slow");
    $.ajax({
        type: "POST",
        url: "/Interview/ActionOnTask",
        data: { Id: _id },
        dataType: "html",
        success: function (response) {
            try {
                var resultdata = JSON.parse(response)
                if (resultdata.Status == Status.Unauthrizedcode) {
                    window.location.href = url.LoginPageUrl
                }
            }
            catch (err) { }
            $('#TaskModal').html(response);
            $('#dialog').modal('show');
        },
        failure: function (response) {
            alert(response.responseText);
        },
        error: function (response) {
            alert(response.responseText);
        }
    });
    $("#spinner").fadeOut("slow");
});
$("body").on("click", "[id*='btnRedirectTask_']", function (e) {
    debugger;
    var selectedItem = $(this)[0].id;
    var _id = selectedItem.split('_')[1];
    $("#spinner").fadeIn("slow");
    $.ajax({
        type: "POST",
        url: "/Interview/TransferTask",
        data: { Id: _id },
        dataType: "html",
        success: function (response) {
            try {
                var resultdata = JSON.parse(response)
                if (resultdata.Status == Status.Unauthrizedcode) {
                    window.location.href = url.LoginPageUrl
                }
            }
            catch (err) { }
            $('#TaskModal').html(response);
            $('#dialog').modal('show');
        },
        failure: function (response) {
            alert(response.responseText);
        },
        error: function (response) {
            alert(response.responseText);
        }
    });
    $("#spinner").fadeOut("slow");
});

$("body").on("click", "#btnSheduleNewInterview", function (e) {
    debugger;
    $("#spinner").fadeIn("slow");
    $.ajax({
        type: "GET",
        url: "/Interview/CreateTask",
        dataType: "html",
        success: function (response) {
            try {
                var resultdata = JSON.parse(response)
                if (resultdata.Status == Status.Unauthrizedcode) {
                    window.location.href = url.LoginPageUrl
                }
            }
            catch (err) { }
            $('#TaskModal').html(response);
            $('#dialog').modal('show');
        },
        failure: function (response) {
            alert(response.responseText);
        },
        error: function (response) {
            alert(response.responseText);
        }
    });
    $("#spinner").fadeOut("slow");
});