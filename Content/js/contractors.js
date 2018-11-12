function searchContractors(obj) {
    var w = $(obj).parent().parent().find('#SearchWord').val();
    $.get('/Contractor/ContractorSearchContent', { Word: w }, function(d) {
        $('#SearchResults').replaceWith(d);
    });
}

function sendInvite(obj) {
    $(obj).parent().parent().find('.search-result-invite').toggle();
}

function sendInviteMessage(obj) {
    var cell = $(obj).parent().parent();
    $.post('/Contractor/ContractorInvite', { ShopID: $('#ShopID').val(), UserID: cell.attr('UserID'), Message: cell.find('textarea').val() }, function(d) {
        cell.find('.search-result-actions').css('color', 'green').html('Приглашение отправлено');
        cell.find('.search-result-invite').remove();
    });
}