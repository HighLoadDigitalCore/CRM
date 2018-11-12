$().ready(function () {
    $('label[rel="worker"]').draggable({
        //containment: $('#workerList'),
        //cursorAt: { left: 0, top: 0 },
        revert: true
    });
    $("#workerList").droppable({
        drop: function (event, ui) {
            var elem = $(ui.draggable);
            $(this).append('<div class="worker-in" arg="' + elem.attr('arg') + '"><label>' + elem.text() + '</label><a href="#nogo" onclick="deleteParticipant(' + elem.attr('arg') + ')" title="Удалить">X</a></div>');
            var arr = $('#Participants').val().split(';');
            if ($.inArray(elem.attr('arg'), arr) < 0) {
                var c = $('#Participants').val();
                if (c.length) {
                    c += ";";
                }
                c += elem.attr('arg');
                $('#Participants').val(c);
            }
            console.log($('#Participants').val());
            elem.remove();
        }
    });
});
function deleteParticipant(id) {

    $('#availList').append('<label class="col-sm-12 control-label" rel="worker" style="cursor: move" arg="' + id + '">' + $('.worker-in[arg="' + id + '"]').find('label').text() + '</label>');
    $('.worker-in[arg="' + id + '"]').remove();
    $('#availList label[arg="' + id + '"]').draggable({
        //containment: $('#workerList'),
        //cursorAt: { left: 0 },
        revert: true
    });
    var arr = $('#Participants').val().split(';');
    var ind = $.inArray(id.toString(), arr);
    if (ind >= 0) {
        arr.splice(ind, 1);
    }
    $('#Participants').val(arr.join(';'));
    console.log($('#Participants').val());


}

function saveGroup() {
    if (!$('#Name').val().length) {
        alert('Необходимо указать название группы');
        return false;
    }
    if (!$('#Participants').val().length) {
        alert('Необходимо добавить в группу хотя бы одного сотрудника');
        return false;
    }
    $.post('/Delivery/SaveGroup', { ID: $('#ID').val(), Name: $('#Name').val(), Participants: $('#Participants').val() }, function (data) {
        if (data != '1') {
            alert(data);
            return false;
        } else {
            document.location.href = '/Delivery/WorkerGroups';
        }
    });
}
