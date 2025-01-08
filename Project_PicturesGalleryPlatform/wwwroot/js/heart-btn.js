$(document).on('click', '.heart-btn', function () {
    var heartBtn = $(this);
    var imageId = heartBtn.data('id');
    var heartIcon = heartBtn.find('i');
    var isFavorited = heartIcon.hasClass('fas');

    $.ajax({
        url: '/Page/ToggleImageLikeStatus',
        type: 'POST',
        data: {
            id: imageId,
            isFavorited: isFavorited
        },
        success: function (response) {
            if (response.success) {
                if (isFavorited) {
                    heartBtn.find(".heart").removeClass('fas fa-heart').addClass('far fa-heart');
                    heartBtn.find("span").text("\u00A0加入我的最愛");


                } else {
                    heartBtn.find(".heart").removeClass('far fa-heart').addClass('fas fa-heart');
                    heartBtn.find("span").text("\u00A0從我的最愛移除");

                }
            } else {
                window.location.href = '/Login/Login'; // 未登入的處理
            }
        },
        error: function () {
            alert('An error occurred, please try again.');
        }
    });
});