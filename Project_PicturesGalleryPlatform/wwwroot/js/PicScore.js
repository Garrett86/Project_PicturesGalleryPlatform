document.addEventListener("DOMContentLoaded", () => {
    const ratingContainer = document.querySelector(".star-rating");
    const stars = document.querySelectorAll('.star-rating i');
    const submitButton = document.getElementById('submit-rating');
    let selectedRating = 0;  // 用來儲存選中的評分

    // 確認用戶是否登入
    function isLoggedIn() {
        const userAccount = getCookie("UserAccount");  // 從 Cookie 讀取 UserAccount
        return userAccount !== null && userAccount !== "";  // 檢查 UserAccount 是否存在且非空
    }

    // 讀取 Cookie 的輔助函數
    function getCookie(name) {
        return name;
    }

    // 綁定點擊事件給每顆星星
    stars.forEach(star => {
        star.addEventListener('click', function () {
            selectedRating = parseInt(star.getAttribute('data-value'));  // 獲取當前點擊星星的評分
            updateStars(selectedRating);  // 更新星星的顯示狀態
        });
    });

    // 更新星星顯示狀態
    function updateStars(rating) {
        stars.forEach((star, index) => {
            if (index < rating) {
                star.classList.remove("far");
                star.classList.add("fas");
            } else {
                star.classList.remove("fas");
                star.classList.add("far");
            }
        });
    }

    // 點擊提交評分按鈕的事件處理
    submitButton.addEventListener("click", async () => {
        // 確認用戶是否登入
        if (!isLoggedIn()) {
            const userConfirmed = confirm("您尚未登入，是否前往登入頁面？");
            if (userConfirmed) {
                window.location.href = '/Login/Login'; // 重定向到登入頁面
            }
            return;
        }

        const userAccount = getCookie("UserAccount");  // 再次讀取 UserAccount
        if (!userAccount) {
            console.log("UserAccount Cookie 不存在");
            return;
        }

        // 確保選擇了評分
        const selectedStar = Array.from(stars).find(star => star.classList.contains("fas"));
        const rating = selectedStar ? selectedStar.getAttribute("data-value") : null;
        const productId = ratingContainer.getAttribute("data-product-id");

        if (!rating || !productId) {
            alert("請選擇評分！");
            return;
        }

        const csrfToken = document.querySelector('input[name="__RequestVerificationToken"]').value;

        try {
            // 發送評分請求到後端
            const response = await fetch('/SinglePic/SubmitRating', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'X-CSRF-TOKEN': csrfToken
                },
                body: JSON.stringify({
                    userAccount: userAccount,  // 使用從 Cookie 獲取的 userAccount
                    productId: productId,
                    rating: selectedRating  // 使用選中的評分
                })
            });

            const data = await response.json();

            if (!response.ok || !data.success) {
                alert("評分失敗，請稍後再試！");
            } else {
                console.log("評分成功:", data.message);
                alert("評分成功！");
                // 可以禁用提交按鈕防止重複提交
                submitButton.disabled = true;
            }
        } catch (error) {
            console.error("評分過程中發生錯誤:", error);
            alert("評分過程中發生錯誤，請稍後再試！");
        }
    });
});
