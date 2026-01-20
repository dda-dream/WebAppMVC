var savedRange;


$(document).ready(function () {
    $('.summernote').summernote({
        height: 150
        ,
        toolbar: [
            ['style', ['bold', 'italic', 'underline', 'clear']],
            ['para', ['ul', 'ol', 'paragraph']],
            ['insert', ['link', 'picture']],
            ['mybutton', ['emojiButton']]
        ],
        buttons: {
            emojiButton: function (context) {
                var ui = $.summernote.ui;

                var button = ui.button({
                    contents: '<span>😊</span>',
                    tooltip: 'Вставить эмодзи',
                    click: function () {

                        var pos = $('.note-editor .note-toolbar').offset();
                        var picker = $('#emojiPicker');

                        picker.css({
                            top: (pos.top + $('.note-toolbar').outerHeight() + 6) + 'px',
                            left: pos.left + 'px'
                        }).toggle();
                    }
                });
                return button.render();
            }
        },
        callbacks: {
            onBlur: function () {
                savedRange = $('#summernote').summernote('createRange');
            },
            onKeyup: function () {
                savedRange = $('#summernote').summernote('createRange');
            },
            onMouseup: function () {
                savedRange = $('#summernote').summernote('createRange');
            }
        }
    });
});



$('#emojiPicker').on('click', '.emoji', function (e) {

    if (savedRange) {
        $('#summernote').summernote('restoreRange', savedRange);
    }
    var emoji = $(this).text();

    $('#summernote').summernote('focus');
    $('#summernote').summernote('editor.insertText', emoji);

    $('#emojiPicker').hide();
});



