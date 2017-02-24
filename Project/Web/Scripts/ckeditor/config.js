/**
 * @license Copyright (c) 2003-2016, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see LICENSE.md or http://ckeditor.com/license
 */

CKEDITOR.editorConfig = function (config) {
    // Define changes to default configuration here.
    // For complete reference see:
    // http://docs.ckeditor.com/#!/api/CKEDITOR.config

    // The toolbar groups arrangement, optimized for two toolbar rows.

    config.uiColor = '#ffffff';
    config.height = 300;
    config.filebrowserImageUploadUrl = "/Files/UploadFile?filetype=1&ckeditor=true";
    config.filebrowserUploadUrl = "/Files/UploadFile?filetype=0&ckeditor=true";

    config.pasteFromWordRemoveFontStyles = false;
    config.pasteFromWordRemoveStyles = false;

    config.toolbarGroups = [
    //{ name: 'document', groups: ['mode', 'document', 'doctools'] },
    { name: 'clipboard', groups: ['clipboard', 'undo'] },
    { name: 'editing', groups: ['find', 'selection', 'spellchecker'] }, { name: 'links' },
   // { name: 'forms' },
    '/',
    { name: 'basicstyles', groups: ['basicstyles', 'cleanup'] }, { name: 'insert' },
    { name: 'paragraph', groups: ['list', 'indent', 'blocks', 'align', 'bidi'] },
    { name: 'tools' },

    '/',
    { name: 'styles' },
    { name: 'colors' },

    { name: 'others' },
    //{ name: 'about' }
    ];
};
