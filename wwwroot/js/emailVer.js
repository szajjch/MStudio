window.emailVerification = {
    focusInput: function (index) {
        var input = document.querySelectorAll('.code-inputs input')[index];
        if (input) {
            input.focus();
        }
    },
    setInputValue: (input, value) => {
        input.value = value;
    }
};