$(document).ready(function () {
    $('[data-toggle="tooltip"]').tooltip();

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




    var elt = $("#SkillsRequired");

    elt.tagComplete({
        itemKey: "Id",
        itemValue: "Name",
        keyLimit: 2,
        freeInput: false,
        hide: false,
        autocomplete: {
            // ajax options
            ajaxOpts: {
                url: '/Common/GetSkills?RequestString=' + $(".tag_input").val(),
                method: 'GET',
                dataType: 'json',
                // data: { RequestString: $(".tag_input").val() },
                success: function (_data) {
                    // not sure if i should return the data here on at end of the ajax call definition
                    return _data;
                },
            },

        },
        ShowTemplate: function (data) {
            return data.Name;
        },
        template: function (data) {
            return data.Name;
        },
    });


    $("#SkillsRequired").siblings(".tag_complete_main").find(".tag_input").wrap('<div class="input-group-append"></div>');
    $('<span class="input-group-text"><i class="fa fa-file" data-toggle="tooltip" data-placement="top" title="Please enter fullname or atleaset  two char to select Skill"></i></label></span>').insertAfter($("#SkillsRequired").siblings(".tag_complete_main").find(".tag_input"));
});

$("body").on("click", "#AllowMultipleSchedule", function (e) {
    if ($("#AllowMultipleSchedule:checked")[0] != undefined) {
        $("#divInterviewSlot2").fadeIn("slow");
        $("#divInterviewSlot1").removeClass("col-sm-12 mb-3 animation");
        $("#divInterviewSlot1").addClass("col-sm-6 mb-3 animation");
    }
    else {
        $("#divInterviewSlot2").fadeOut("slow");
        $("#divInterviewSlot1").removeClass("col-sm-6 mb-3 animation");
        $("#divInterviewSlot1").addClass("col-sm-12 mb-3 animation");
    }
});

$("body").on("click", "#btnCreateTask", function (e) {
    SaveToDB(false);
});
function SaveToDB(isDraft) {
    var _form = $("#FormCreateTask");
    var formData = new FormData();
    formData.append('Title', $("#title").val());
    formData.append('Technology', $("#Technology").val());
    formData.append('Position', $("#Position").val());
    formData.append('TaskDescription', $("#TaskDescription").val());
    formData.append('AllowMultipleSchedule', $("#AllowMultipleSchedule").val() != undefined ? $("#AllowMultipleSchedule").val() : "N");
    formData.append('InterviewSlot1Date', $("#InterviewSlot1Date").val() != undefined ? $("#InterviewSlot1Date").val() : null);
    formData.append('InterviewSlot1StartTime', $("#InterviewSlot1StartTime").val() != undefined ? $("#InterviewSlot1StartTime").val() : "");
    formData.append('InterviewSlot1EndTime', $("#InterviewSlot1EndTime").val() != undefined ? $("#InterviewSlot1EndTime").val() : "");
    formData.append('InterviewSlot2Date', $("#InterviewSlot2Date").val() != undefined ? $("#InterviewSlot2Date").val() : null);
    formData.append('InterviewSlot2StartTime', $("#InterviewSlot2StartTime").val() != undefined ? $("#InterviewSlot2StartTime").val() : "");
    formData.append('InterviewSlot2EndTime', $("#InterviewSlot2EndTime").val() != undefined ? $("#InterviewSlot2EndTime").val() : "");
    formData.append('SkillsRequired', $("#SkillsRequired").val());
    //formData.append('CandidateGender', $("[name='CandidateGender']:checked").val());
    //formData.append('EmailId', $("#EmailId").val());
    //formData.append('PhoneNumber', $("#PhoneNumber").val());
    formData.append('AssignedToPanel', $("#AssignedToPanel").val());
    formData.append('InterviewRound', $("#InterviewRound").val());
    formData.append('IsDraft', isDraft);
    formData.append('DraftID', $("#DraftID").val());


    var totalProfiles = $("[id*=CandidateName]").length;

    if (totalProfiles > 0) {
        for (var i = 0; i < totalProfiles; i++) {
            formData.append('CandidateName_' + i, $("#CandidateName_" + i).val());
            formData.append('FileUpload_' + i, $("#FileUpload_" + i).get(0).files[0]);
        }
    }
    else {
        return false;
    }
    formData.append('InterviewerRequired', $("#InterviewerRequired").val());
    if (isDraft) {
        if (_form.valid()) {
            $.ajax({
                type: "POST",
                url: '/Task/SaveDraft',
                data: formData,
                dataType: "json",
                contentType: false,
                processData: false,
                beforeSend: function (jqXHR, settings) {
                    $("#spinner").fadeIn("slow");
                },
                success: function (response) {
                    try {
                        debugger;
                        if (response.Status == Status.Unauthrizedcode) {
                            window.location.href = url.LoginPageUrl
                        }
                        $("#spinner").fadeOut("slow");
                        if (response.Status == Status.Success) {
                            alert("Draft Saved Successfully !")
                        }
                        debugger;
                        if (response.DraftId != "00000000-0000-0000-0000-000000000000") {
                            $("#DraftID").val(response.DraftId);

                        }
                    }
                    catch (err) {
                        return false;
                    }

                },
                failure: function (response) {
                    return false;
                },
                error: function (response) {
                    return false;
                },
                complete: function (jqXHR, textStatus) {
                    return true;

                    $("#spinner").fadeOut("slow");
                },
            });
        }
    }
    else {
        {
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
                        if (completeFunction && (typeof completeFunction == "function")) {

                        };
                        $("#spinner").fadeOut("slow");
                    },
                });
            }
        }
    }
}

var _form = $("#FormCreateTask");
$(_form).validate({
    rules: {
        title: {
            required: true,
        },
        Technology: {
            required: true,
        },
        Position: {
            required: true,
        },
        TaskDescription: {
            required: true,
        },
        InterviewSlot1Date: {
            required: true,
        },
        InterviewSlot1StartTime: {
            required: true,
        },
        InterviewSlot1EndTime: {
            required: true,
        },
        InterviewSlot2Date: {
            required: true,
        },
        InterviewSlot2StartTime: {
            required: true,
        },
        InterviewSlot2EndTime: {
            required: true,
        },
        CandidateName: {
            required: true,
        },
        CandidateGender: {
            required: true,
        },
        AssignedToPanel: {
            required: true,
        },
        InterviewerRequired: {
            required: true,
        },
        EmailId: {
            required: true,
            // email: true,
            //accept: "[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,}"
        },
        PhoneNumber: {
            required: true,
            //PhoneNumber: true,
            // accept: "[0-9]+"
        },
        FileUpload: {
            required: true,
        }, InterviewRound: {
            required: true,
        },
    },
    messages: {
        title: 'Required !',
        Technology: 'Required !',
        Position: 'Required !',
        TaskDescription: 'Required !',
        InterviewSlot1Date: 'Required !',
        InterviewSlot1StartTime: 'Required !',
        InterviewSlot1EndTime: 'Required !',
        InterviewSlot2Date: 'Required !',
        InterviewSlot2StartTime: 'Required !',
        InterviewSlot2EndTime: 'Required !',
        CandidateName: 'Required !',
        CandidateGender: 'Required !',
        AssignedToPanel: 'Required !',
        InterviewerRequired: 'Required !',
        EmailId: 'Required !',
        PhoneNumber: 'Required !',
        FileUpload: 'Required !',
        InterviewRound: 'Required !'
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

$("#FileUpload").change(function () {

    var extension = $(this).val().split('.').pop().toLowerCase();
    var validFileExtensions = ['jpg', 'jpeg', 'pdf'];
    if ($.inArray(extension, validFileExtensions) == -1) {
        alert("Sorry!! Upload only 'jpg', 'jpeg', 'pdf' file");
        $(this).replaceWith($(this).val('').clone(true));
        $('#btnUploadDocument').attr('disabled', 'disabled');
    } else {
        if ($(this).get(0).files[0].size > (204800)) {
            alert("Sorry!! Max allowed file size is 128 kb");
            $(this).replaceWith($(this).val('').clone(true));
            $('#btnUploadDocument').attr('disabled', 'disabled');
        } else {
            $('#btnUploadDocument').removeAttr('disabled');
        }
    }
});


$("body").on("click", ".removeCandidateInfo", function (e) {
    $(this).closest(".Candidate").remove();
});


$("#AddMoreCandidate").click(function (e) {
    debugger;
    var count = $(".CandidateInfo .Candidate").length;

    var closediv = document.createElement("div");
    closediv.setAttribute("class", "col-sm-12");

    var CandidateDiv = document.createElement("div");
    CandidateDiv.setAttribute("class", "row g-0 Candidate");
    CandidateDiv.setAttribute("style", "border-bottom: 3px solid gray; margin-bottom: 15px;");

    var outerdiv = document.createElement("div");
    outerdiv.setAttribute("class", "col-sm-6 mb-1");

    var closeSpan = document.createElement("span");
    closeSpan.setAttribute("class", "removeCandidateInfo fa fa-close");
    closeSpan.setAttribute("id", "close_" + count);

    var InnerDiv = document.createElement("div");
    InnerDiv.setAttribute("class", "form-group");

    var Label = document.createElement("label");
    Label.setAttribute("class", "form-label required");
    Label.innerText = "Candidate Name";

    var Input = document.createElement("Input");
    Input.setAttribute("type", "text");
    Input.setAttribute("class", "form-control");
    Input.setAttribute("Id", "CandidateName_" + count);
    Input.setAttribute("name", "CandidateName_" + count);
    Input.setAttribute("placeholder", "Enter Title here");
    Input.innerText = "Candidate Name";

    $(InnerDiv).append(Label);
    $(InnerDiv).append(Input);
    $(outerdiv).append(InnerDiv);
    $(closediv).append(closeSpan);
    $(CandidateDiv).append(closediv);
    $(CandidateDiv).append(outerdiv);

    var outerdiv1 = document.createElement("div");
    outerdiv1.setAttribute("class", "col-sm-6 mb-1");

    var InnerDiv1 = document.createElement("div");
    InnerDiv1.setAttribute("class", "form-group");

    var Label1 = document.createElement("label");
    Label1.setAttribute("class", "form-label required");
    Label1.innerText = "Upload Resume";

    $(InnerDiv1).append(Label1);



    var dropZone = document.createElement("div");
    dropZone.setAttribute("class", "drop-zone");

    var dropZonePrompt = document.createElement("span");
    dropZonePrompt.setAttribute("class", "drop-zone__prompt");
    dropZonePrompt.innerHTML = "Drop file here or click to upload";

    var Input1 = document.createElement("Input");
    Input1.setAttribute("type", "file");
    Input1.setAttribute("Id", "FileUpload_" + count);
    Input1.setAttribute("name", "FileUpload_" + count);
    Input1.setAttribute("class", "drop-zone__input");

    $(dropZone).append(dropZonePrompt);
    $(dropZone).append(Input1);
    $(outerdiv1).append(InnerDiv1);
    $(outerdiv1).append(dropZone);
    $(CandidateDiv).append(outerdiv1);

    $(".CandidateInfo").append(CandidateDiv);

    AddFileUploadFunctionality();
});



