if (userId) {
    fetch(`http://localhost:5055/api/History/get-return-history-by-user-id/${userId}`)
        .then(response => response.json())
        .then(history => {
            const today = new Date();
            today.setHours(0, 0, 0, 0); // Set the time to 00:00:00

            const dueTime = new Date(history.dueTime);
            dueTime.setHours(0, 0, 0, 0); // Set the time to 00:00:00

            const oneDayInMilliseconds = 24 * 60 * 60 * 1000;
            const tomorrow = new Date(today.getTime() + oneDayInMilliseconds);

            if (history.status === 'Borrowing') {
                if (dueTime.getTime() === today.getTime()) {
                    Toastify({
                        text: "Your borrowed book is due today!",
                        close: true,
                        duration: -1,
                        gravity: "bottom", // `top` or `bottom`
                        position: 'left', // `left`, `center` or `right`
                        style: {
                            background: "linear-gradient(to right, #00b09b, #96c93d)",
                        }
                    }).showToast();
                } else if (dueTime.getTime() === tomorrow.getTime()) {
                    Toastify({
                        text: "Your borrowed book is due tomorrow!",
                        close: true,
                        duration: -1,
                        gravity: "bottom", // `top` or `bottom`
                        position: 'left', // `left`, `center` or `right`
                        style: {
                            background: "linear-gradient(to right, #ff7733, #ffcc00)",
                        }
                    }).showToast();
                } else if (dueTime.getTime() < today.getTime()) {
                    Toastify({
                        text: "Your borrowed book is overdue!",
                        close: true,
                        duration: -1,
                        gravity: "bottom", // `top` or `bottom`
                        position: 'left', // `left`, `center` or `right`
                        style: {
                            background: "linear-gradient(to right, #660000, #ff0000)",
                        }
                    }).showToast();
                }
            }
        })
        .catch(error => console.error('Error:', error));
}
