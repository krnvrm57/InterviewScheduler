$(document).ready(function () {
    $('[data-toggle="tooltip"]').tooltip();
    $("#SkillsRequired").hide();

    $('.datepicker').each(function (key, value) {
        $(this).datetimepicker({
            format: 'd-M-Y',
            timepicker: false,
        });
    });
    $('.timepicker').each(function (key, value) {
        $(this).datetimepicker({
            datepicker: false,
            step: 5,
            format: 'g:i a',
        });
    });
});

$("body").on("click", "#AllowMultipleSchedule", function (e) {
    if ($("#AllowMultipleSchedule:checked")[0] != undefined) {
        $("#divInterviewSlot2").fadeIn("fast");
        $("#divInterviewSlot1").removeClass("col-sm-12 mb-3 animation");
        $("#divInterviewSlot1").addClass("col-sm-6 mb-3 animation");
    }
    else {
        $("#divInterviewSlot2").fadeOut("fast");
        $("#divInterviewSlot1").removeClass("col-sm-6 mb-3 animation");
        $("#divInterviewSlot1").addClass("col-sm-12 mb-3 animation");
    }
});
$("body").on("click", "#btnCreateTask", function (e) {
    var _form = $("#FormCreateTask");
    var formData = new FormData();
    formData.append('Title', $("#title").val());
    formData.append('Technology', $("#Technology").val());
    formData.append('Position', $("#Position").val());
    formData.append('TaskDescription', $("#TaskDescription").val());
    formData.append('AllowMultipleSchedule', $("#AllowMultipleSchedule").val());
    formData.append('InterviewSlot1Date', $("#InterviewSlot1Date").val());
    formData.append('InterviewSlot1StartTime', $("#InterviewSlot1StartTime").val());
    formData.append('InterviewSlot1EndTime', $("#InterviewSlot1EndTime").val());
    formData.append('InterviewSlot2Date', $("#InterviewSlot2Date").val());
    formData.append('InterviewSlot2StartTime', $("#InterviewSlot2StartTime").val());
    formData.append('InterviewSlot2EndTime', $("#InterviewSlot2EndTime").val());
    formData.append('CandidateName', $("#CandidateName").val());
    formData.append('CandidateGender', $("#CandidateGender:checked").val());
    formData.append('EmailId', $("#EmailId").val());
    formData.append('PhoneNumber', $("#PhoneNumber").val());
    formData.append('FileUpload', $("#FileUpload").get(0).files[0]);
    formData.append('AssignedToPanel', $("#AssignedToPanel").val());
    formData.append('InterviewerRequired', $("#InterviewerRequired").val());
    if (_form.valid()) {
        $.ajax({
            type: "POST",
            url: '/Task/CreateTask',
            data: formData,
            dataType: "json",
            contentType: false,
            processData: false,
            beforeSend: function (jqXHR, settings) {
                $("#spinner").fadeIn("slow");
            },
            success: function (response) {
                try {
                    var resultdata = JSON.parse(response)
                    if (resultdata.Status == Status.Unauthrizedcode) {
                        window.location.href = url.LoginPageUrl
                    }

                }
                catch (err) { }
                window.location.href = "/Task";

            },
            failure: function (response) {
                alert(response.responseText);
            },
            error: function (response) {
                alert(response.responseText);
            },
            complete: function (jqXHR, textStatus) {
               
                $("#spinner").fadeOut("slow");
            },
        });
    }
});



$("body").on("change", "#TaskAction", function (e) {
    $("#SuggestSection").hide();
    $("#CommentSection").hide();
    $("#SuggestedSlotDate1").val("");
    $("#SuggestedStartTime1").val("");
    $("#SuggestedEndTime1").val("");
    $("#SuggestedSlotDate2").val("");
    $("#SuggestedStartTime2").val("");
    $("#SuggestedEndTime2").val("");


    if ($(this).val() !== "") {
        if ($(this).val() == "300") {
            $("#SuggestSection").show();

        }
        $("#CommentSection").show();
    }
});
$("body").on("click", "#btnSaveTaskResponse", function (e) {
    if ($("#FormViewUpdateDetails").valid()) {

        var _data = {
            Id: $("#Id").val(),
            TaskAction: $("#TaskAction").val(),
            SuggestedSlotDate1: $("#SuggestedSlotDate1").val(),
            SuggestedStartTime1: $("#SuggestedStartTime1").val(),
            SuggestedEndTime1: $("#SuggestedEndTime1").val(),
            SuggestedSlotDate2: $("#SuggestedStartTime2").val(),
            SuggestedStartTime2: $("#SuggestedStartTime2").val(),
            SuggestedEndTime2: $("#SuggestedEndTime2").val(),
            Comments: $("#Comments").val(),
        }
        $.ajax({
            type: "POST",
            url: '/MyTasks/GetTaskResponse',
            data: _data,
            dataType: "json",
            beforeSend: function (jqXHR, settings) {
                $("#spinner").fadeIn("slow");
            },
            success: function (response) {
                try {
                    var resultdata = JSON.parse(response)
                    if (resultdata.Status == Status.Unauthrizedcode) {
                        window.location.href = url.LoginPageUrl
                    }

                }
                catch (err) { }
                window.location.href = "/MyTasks";

            },
            failure: function (response) {
                alert(response.responseText);
            },
            error: function (response) {
                alert(response.responseText);
            },
            complete: function (jqXHR, textStatus) {
                if (completeFunction && (typeof completeFunction == "function")) {

                };
                $("#spinner").fadeOut("slow");
            },
        });
    }

});


var _form = $("#FormViewUpdateDetails");
$(_form).validate({
    rules: {
        SSDate: {
            required: true,
        },
        TaskAction: {
            required: true,
        },
        RISlotDate: {
            required: true,
        },
        RIStartTime: {
            required: true,
        },
        RIEndTime: {
            required: true,
        },
    },
    messages: {
        SSDate: 'Required !',
        TaskAction: 'Required !',
        RISlotDate: 'Required !',
        RIStartTime: 'Required !',
        RIEndTime: 'Required !',
    },
    errorElement: 'div',
    errorClass: 'text-danger',
    errorPlacement: function (error, element) {
        if (element.parent().hasClass('input-group')) {
            error.insertAfter(element.parent());
        }
        else if (element[0].type == "radio") {
            debugger;
            error.closest("form-group").insertAfter(element.parent());
        }
        else {
            error.insertAfter(element);
        }

    },
});
