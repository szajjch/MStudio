$(document).ready(function () {
    const firstSectionVideo = $('#first-vid');
    const slider = $('#slider');
    const slideWidthPercentage = 100 / $('input[name="offer-control"]').length;
    const ofLabel = $('label');
    const offerControls = $('input[name="offer-control"]');
    let vidIndex = 1;
    let vidNow = 1;
    let firstVisible = true;

    offerControls.change(function () {
        const selectedLabel = $(`#l-of-${this.value}`);
        const unselectedLabels = ofLabel.not(selectedLabel);

        selectedLabel.removeClass('w-5').addClass('w-14');
        unselectedLabels.removeClass('w-14').addClass('w-5');

        $('video').each(function () {
            this.pause();
        });

        $(`#vid${this.value}`)[0].play();

        const n = parseInt(this.value);
        vidIndex = n % 8;

        let slidePosition = -slideWidthPercentage * (n - 1);
        $('#slider').css('transform', `translateX(${slidePosition}%)`);
        $('#text-slider').css('transform', `translateX(${slidePosition}%)`);
        timer.reset(10000);
    });

    let timer = new Timer(function () {
        if (!firstVisible) {
            offerControls.eq(vidIndex).click();
        }
    }, 10000);

    $(window).scroll(function () {
        const firstSectionTop = firstSectionVideo.offset().top;
        const firstSectionBottom = firstSectionTop + firstSectionVideo.height();
        const viewportTop = $(window).scrollTop();
        const viewportBottom = viewportTop + $(window).height();

        const isMoreThanHalfVisible = firstSectionTop < viewportBottom && firstSectionBottom > viewportTop + firstSectionVideo.height() / 2;

        if (isMoreThanHalfVisible) {
            firstVisible = true;
            $('video').each(function () {
                this.pause();
            });
            firstSectionVideo[0].play();
            $('#arrowdown').fadeIn(200);
            timer.stop();
        } else {
            firstVisible = false;
            firstSectionVideo[0].pause();
            $(`#vid${vidIndex}`)[0].play();
            $('#arrowdown').fadeOut(200);
            timer.start();
        }
    });

    function Timer(fn, t) {
        let timerObj = setInterval(fn, t);

        this.stop = function () {
            if (timerObj) {
                clearInterval(timerObj);
                timerObj = null;
            }
            return this;
        }

        this.start = function () {
            if (!timerObj) {
                this.stop();
                timerObj = setInterval(fn, t);
            }
            return this;
        }

        this.reset = function (newT = t) {
            t = newT;
            return this.stop().start();
        }
    }

    $('#offerbtn, #arrowdown').on('click', function () {
        $('html, body').animate({
            scrollTop: $(document).height()
        }, 500);
    });

    $('#mainbtn').on('click', function () {
        $('html, body').animate({
            scrollTop: 0
        }, 500);
    });

    $('#nav-booking-btn, #first-booking-btn, #second-booking-btn').on('click', function () {
        window.location.href = '/rezerwacja';
    });
});