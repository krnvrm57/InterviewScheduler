
$(document).ready(function () {

    $(".EmployeeName").siblings(".bootstrap-tagsinput").find(".tt-input").wrap('<div class="input-group-append"></div>');
    $('<span class="input-group-text"><i class="fa fa-user" data-toggle="tooltip" data-placement="top" title="Please enter fullname or atleaset first three char to select User Name"></i></label></span>').insertAfter($(".EmployeeName").siblings(".bootstrap-tagsinput").find(".tt-input"));



});

function AddTagsAutoComplete(ctrlId, PreAddedTagdata) {
    console.log(PreAddedTagdata);
    var elt = $("#" + ctrlId);
    debugger;
    elt.tagComplete({
        PreAddedTag: (PreAddedTagdata != undefined && JSON.parse(PreAddedTagdata) != undefined) ? JSON.parse(PreAddedTagdata) : '',
        keyLimit: 3,
        freeInput: false,
        hide: false,
        autocomplete: {
            // ajax options
            ajaxOpts: {
                url: '/Common/GetEmployees',
                method: 'GET',
                dataType: 'json',
                data: { name: $(".tag_input").val() },
                success: function (_data) {
                    // not sure if i should return the data here on at end of the ajax call definition
                    this.ajaxPool = _data;
                    return _data;
                },
            },

        },
        itemKey: "EmailId",
        itemValue: "FullName",
        ShowTemplate: function (data) {
            debugger;
            var ThumbnailPhoto = "";

            if (data.ThumbnailPhoto != undefined && data.ThumbnailPhoto != "") {
                ThumbnailPhoto = "data:image/jpeg;base64," + data.ThumbnailPhoto;
            }
            else {
                ThumbnailPhoto = "/Content/img/UserIcon.jpg";
            }
            return '<div><div class="row"><div class="col-sm-9">' + data.FullName + '<br><i>' + data.EmailId + '</i></div>' +
                '<div class="col-sm-3 text-right" style="white-space: normal;"><img class="rounded-circle" src="' + ThumbnailPhoto + '" style="height: 40px; width: 40px;"></div></div></div>'

        },
        template: function (data) {
            debugger;
            var ThumbnailPhoto = "";

            if (data.ThumbnailPhoto != undefined && data.ThumbnailPhoto != "") {
                ThumbnailPhoto = "data:image/jpeg;base64," + data.ThumbnailPhoto;
            }
            else {
                ThumbnailPhoto = "/Content/img/UserIcon.jpg";
            }
            return '<div><div class="row"><div class="col-sm-9">' + data.FullName + '<br><i>' + data.EmailId + '</i></div>' +
                '<div class="col-sm-3 text-right" style="white-space: normal;"><img class="rounded-circle" src="' + ThumbnailPhoto + '" style="height: 40px; width: 40px;"></div></div></div>'

        }

    });

}
