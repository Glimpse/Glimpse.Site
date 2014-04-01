$(function() {
    $('.data-name').on('focus', function() {
        var element = $(this),
            allElements = $('.data-name'); 
        if (element.val() == '' && allElements.index(element) == allElements.length - 1) {
            var row = element.closest('tr'),
                clone = row.clone(true, true);
            row.after(clone);
        } 
    });

    var getData = function() {
        var names = $('.data-name'),
            versions = $('.data-version'),
            data = { stamp: '' };
                
        for (var i = 0; i < names.length; i++) {
            var nameVal = $(names[i]).val(),
                versionVal = $(versions[i]).val();
            if (nameVal && versionVal)
                data[nameVal] = versionVal;
        }

        return data;
    };

    $('.data-close').on('click', function() {
        $(this).closest('tr').remove();
    });
             
    $('.data-status').text('In Progress');

    $('.action-page, .action-service').click(function() {
        var element = $(this),
            data = getData(),
            type = element.attr('data-type'),
            url = element.attr('data-destination'),
            detailsElement = $('.data-with-details:checked'),
            detailsUrl = detailsElement.attr('data-destination');
            
        if (detailsUrl)
            url += detailsUrl;

                
        $.ajax({
            url: url,
            type: 'GET',
            contentType: 'application/json; charset=utf-8',
            dataType: type,
            data: data,
            success: function (result) {
                $('.data-url').text(this.url);
                        
                $('.data-status').text('Finished');
                setTimeout(function () {
                    $('.data-status').text('');
                }, 3000);

                if (type == 'json')
                    $('.data-result').html('<pre>' + jsl.format.formatJson(JSON.stringify(result)) + '</pre>');
                else
                    $('.data-result').html(result);
            },
            complete: function (a, b, c) {
                console.log(a);
            }
        });
    });
});