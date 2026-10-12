
const Status = {
    Success: "Success",
    Failure: "Failure",
    Unauthrizedcode: "401",
}
 
$(document).ready(function () {
    $(document).ajaxError(function (e, xhr, opts) {
        debugger;
        console.log(xhr);
        if (xhr.status == Status.Unauthrizedcode) {
            window.location.href = "/Error/InvalidAccess";
        }
    });
});

function addAntiForgeryToken(data) {
    data.__RequestVerificationToken = $("#__RequestVerificationToken").val();
    return data;
};