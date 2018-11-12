$().ready(function() {
    autoCompleteProduct();
});
function autoCompleteProduct() {
    $("#ComponentName")
        .autocomplete({
            source: function (request, response) {
                $.getJSON("/Products/Search", {
                    query: extractLast(request.term), id: $('#ProductID').val()
                }, response);
            },
            search: function () {
                var term = extractLast(this.value);
                if (term.length < 1) {
                    return false;
                }
            },
            focus: function () {
                return false;
            },
            select: function (event, ui) {
                
                $('#NewComponentID').val(ui.item.value);
                $('#ComponentName').val(ui.item.label);
                return false;
            }
        });

   
}

function deleteComponent(id, productId, obj) {
    $.post('/Products/DeleteComponent', { ID: id, ProductID: productId }, function (data) {
        if (data == '1') {
            $(obj).parents('.form-group').remove();
            if (!$('#CompList .form-group').length) {
                $('#CompList').html('<b>Данный продукт не имеет составных частей</b>');
            }
        }
    })
}

function split(val) {
    return val.split(/,\s*/);
}

function extractLast(term) {
    return split(term).pop();
}
