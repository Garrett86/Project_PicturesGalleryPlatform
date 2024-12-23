$(document).ready(function () {
    var page = 0;
    var pageSize = 40;
    var isLoading = false;

    function loadItems() {
        if (isLoading) return;
        isLoading = true;

        $.ajax({
            url: $('#imageResultsContainer').data('url'),
            data: { page: page, pageSize: pageSize },
            type: 'GET',
            success: function (data) {
                if (data.length > 0) {
                    data.forEach(function (item) {
                        $('#imageResultsContainer').append(`<a href="https://localhost:7128/Page/PictureInfo?id= ${item.id}" class="item-link">
                                                        <img src="${item.url}" alt="Item Image" class="item-image" />
                                                        <div class="preview">
                                                            <img src="${item.url}" alt="Preview Image" />
                                                            
                                                        </div>
                                                    </a>`);
                    });
                    page++;
                }
                isLoading = false;
            },
            error: function () {
                isLoading = false;
                alert('Failed to load items');
            }
        });
    }

    loadItems();

    $(window).scroll(function () {
        if ($(window).scrollTop() + $(window).height() >= $(document).height() - 100) {
            loadItems();
        }
    });

    $(document).on('mouseenter', '.item-link', function () {
        $(this).find('.preview').show();
    });

    $(document).on('mouseleave', '.item-link', function () {
        $(this).find('.preview').hide();
    });
});