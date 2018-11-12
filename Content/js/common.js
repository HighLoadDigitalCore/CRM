
function setBox(obj) {
    console.log($(obj).val());
    if ($(obj).val().length) {
        $(obj).parent().find('input[type="checkbox"]').prop('checked', 'checked');
    } else {
        $(obj).parent().find('input[type="checkbox"]').removeAttr('checked');
    }
}