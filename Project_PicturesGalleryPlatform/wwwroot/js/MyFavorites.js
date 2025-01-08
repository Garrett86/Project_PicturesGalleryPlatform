$(document).ready(function () {
    var page = 0;
    var pageSize = 40;
    var isLoading = false;

    function loadItems() {
        if (isLoading) return;
        isLoading = true;

        var urlParams = new URLSearchParams(window.location.search);
        var userAccount = urlParams.get('userAccount');
        
        $.ajax({
            url: `/api/Images/Favorites?userAccount=${userAccount}`,
            data: { page: page, pageSize: pageSize },
            type: 'GET',
            success: function (data) {
                if (data.length > 0) {
                    data.forEach(function (item) {
                        $('#MyFavoritesContainer').append(`
                            <div class="u-effect-fade u-gallery-item">
                                <a href="javascript:void(0);" class="heart-btn" data-id="${item.id}">
                                      <i class="heart ${item.isFavorited ? 'fas fa-heart' : 'far fa-heart'}" style="color: red;"></i>
                                </a>
                                <img class="u-back-image u-expanded" src="/images2/${item.id}.webp">
                                <a href="../SinglePic/SinglePic?id=${item.id}" class="u-over-slide u-shading u-over-slide-1"></a>

                            </div>
                        `);
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
});
