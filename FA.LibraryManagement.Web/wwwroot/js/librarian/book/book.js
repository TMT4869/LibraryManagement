let bookTable = null;
let pondFile = null;

const BookObj = {
    init: function () {
        var global = this;

        //Init flatpickr
        flatpickr('.flatpickr-no-config', {
            enableTime: false,
            maxDate: new Date(),
        })

        //Init select2
        $('#CategoryId').select2({
            placeholder: 'Select an option',
            theme: 'bootstrap-5'
        });

        //Init select2
        $('#Authors').select2({
            placeholder: 'Select an option',
            theme: 'bootstrap-5'
        });

        global.InitTinymce();

        global.InitFilePond();

        $('body').on('submit', '#book-form', function (event) {
            return global.bookForm(this, event);
        });
    },

    bookForm: function (eThis, event) {
        event.preventDefault();
        const formdata = new FormData(eThis);

        let filePondInstance = $("#image-pond").filepond('getFiles');
        
        filePondInstance.forEach(file => {
            formdata.append('files', file.file);
        });
        

        $.ajax({
            url: $(eThis).attr('action'),
            type: 'POST',
            contentType: false,
            processData: false,
            data: formdata,
            success: function (data) {
                if (data.success) {
                    location.href = "/Librarian/Book/List";
                }
            },
            error: function (error) {
                console.log(error);
            }
        });
    },

    InitFilePond: function () {
        useEditorWithJQuery(jQuery, pintura);

        const {
            openEditor,
            createDefaultImageReader,
            createDefaultImageWriter,
            processImage,
            getEditorDefaults,
        } = $.fn.pintura;

        $.fn.filepond.registerPlugin(
            FilePondPluginImageEditor,
            FilePondPluginFilePoster
        );

        $("#image-pond").filepond({
            allowReorder: true,
            filePosterMaxHeight: 256,
            imageEditor: {
                createEditor: openEditor,
                imageReader: [createDefaultImageReader],
                imageWriter: [
                    createDefaultImageWriter,
                    {
                        targetSize: {
                            width: 384,
                        },
                    },
                ],
                imageProcessor: processImage,
                editorOptions: {
                    ...getEditorDefaults(),
                },
            }
        });

    },

    InitTinymce: function () {
        //Init tinymce
        const themeOptions = document.body.classList.contains("dark")
            ? {
                skin: "oxide-dark",
                content_css: "dark",
            }
            : {
                skin: "oxide",
                content_css: "default",
            }

        tinymce.init({selector: "#Description", ...themeOptions})
        tinymce.init({
            selector: "#dark",
            toolbar:
                "undo redo styleselect bold italic alignleft aligncenter alignright bullist numlist outdent indent code",
            plugins: "code",
            ...themeOptions,
        })
    }
}

$(document).ready(function ($) {
    const BookClass = Object.create(BookObj);
    BookClass.init();
});

