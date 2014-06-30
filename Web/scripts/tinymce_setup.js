tinymce.init({
    selector: "textarea",
    menubar: false,
    toolbar: "cut copy paste bold italic underline forecolor numlist bullist link unlink removeformat",
    plugins: "link textcolor",
    encoding: "xml",
    target_list: [{ title: 'New page', value: '_blank' }, { title: 'Same page', value: '_self'}],
    extended_valid_elements: 'a[href|target=_blank],span[!class]',
    paste_strip_class_attributes: 'all',
    paste_remove_styles: true,
    paste_auto_cleanup_on_paste: true,
    setup: function (editor) {
        editor.on('SaveContent', function (ed) {
            ed.content = ed.content.replace(/&#39/g, "&apos");
            ed.content = ed.content.replace(/&amp;/g, "&");
        });
    }
});