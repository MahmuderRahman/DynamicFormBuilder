var table;
var currentSelectForOption = null;

var optionArray = [];

$(document).ready(function () {

    Manager.LoadDataTable();

    $('#addMore').on("click", function () {
        var clone = $('.field-template .field-block').clone();
        $('#fieldsContainer').append(clone);
    });

    $('#fieldsContainer').on('click', '.remove-field', function () {
        $(this).closest('.field-block').remove();
    });

    $('#formCreate').on('submit', function (e) {
        e.preventDefault();
        Manager.SaveForm();
    });

    $('#searchBox').on('keyup', function () {
        table.ajax.reload();
    });

    $('#clearForm').on("click", function () {
        $('#formCreate')[0].reset();
        $('#fieldsContainer').empty();
    });

    $('#fieldsContainer').on('change', '.fieldType', function () {
        var fieldBlock = $(this).closest('.field-block');

        if ($(this).val() === 'Dropdown') {
            fieldBlock.find('.dropdown-options-container').show();
            var select = fieldBlock.find('.dropdownMasterId');
            select.empty().append('<option value="">Select Item</option>');

            $.get('/Dropdown/GetAll', function (data) {
                data.forEach(function (item) {
                    select.append('<option value="' + item.id + '">' + item.name + '</option>');
                });
            });
        } else {
            fieldBlock.find('.dropdown-options-container').hide();
        }
    });

    $('#fieldsContainer').on('click', '.add-dropdown-option', function () {
        currentSelectForOption = $(this).closest('.field-block').find('.dropdownMasterId');
        $('#newDropdownName').val('');
        var addModal = new bootstrap.Modal(document.getElementById('addOptionModal'));
        addModal.show();
    });

    $('#saveOptionBtn').on('click', function () {
        Manager.SaveOption();
    });

});

var Manager = {


    SaveForm: function () {
        if (!$('#Title').val().trim()) {
            alert("Title is required");
            return;
        }
        var totalFields = $('#fieldsContainer .field-block').length;

        if (totalFields === 0) {
            alert("Please add at least one field");
            return;
        }

        var formData = new FormData();

        formData.append("Title", $('#formCreate input[name="Title"]').val());
        let validationFailed = false;

        $('#fieldsContainer .field-block').each(function (i) {

            var label = $(this).find('.label').val();
            var isRequired = $(this).find('.required').is(':checked');
            var fieldType = $(this).find('.fieldType').val();
            var dropdownMasterId = $(this).find('.dropdownMasterId').val();

            if (!label) {
                alert("Field label is required");
                validationFailed = true;
                return false;
            }

            if (isRequired && (!dropdownMasterId || dropdownMasterId === "")) {
                alert("Please select a value for the required dropdown");
                validationFailed = true;
                return false;
            }

            formData.append(`Fields[${i}].Label`, label);
            formData.append(`Fields[${i}].IsRequired`, isRequired);
            formData.append(`Fields[${i}].FieldType`, fieldType);
            formData.append(`Fields[${i}].DropdownMasterId`, dropdownMasterId);
        });

        if (validationFailed) return;

        if (Message.Prompt()) {
            var serviceURL = "/Form/Create";
            AjaxManager.SendJson(serviceURL, formData, onSuccess, onFailed);
        }

        function onSuccess(res) {
            if (res && res.message) {
                Message.Success(res.message);

                $('#formCreate')[0].reset();
                $('#fieldsContainer').empty();

                if (typeof table !== "undefined") {
                    table.ajax.reload();
                }
            }
            else {
                Message.Error("Something went wrong");
            }
        }

        function onFailed(xhr) {
            var msg = "Error occurred while saving.";
            if (xhr.responseJSON && xhr.responseJSON.message) {
                msg = xhr.responseJSON.message;
            }
            Message.Error(msg);
        }
    },

    LoadDataTable: function () {

        table = $('#formsTable').DataTable({
            serverSide: true,
            processing: true,
            searching: false,
            ajax: {
                url: '/Form/GetForms',
                type: 'GET',
                data: function (d) {
                    d.search = $('#searchBox').val();
                }
            },
            columns: [
                {
                    data: 'formId',
                    title: '#SL',
                    width: '50px',
                    className: 'text-center',
                    render: function (data, type, row, meta) {
                        return meta.row + 1;
                    }
                },
                { data: 'title', title: 'Title' },
                {
                    data: 'createdAt',
                    title: 'Created At',
                    width: '150px',
                    className: 'text-center',
                },
                {
                    data: 'formId',
                    title: 'Actions',
                    orderable: false,
                    width: '150px',
                    className: 'text-center',
                    render: function (data) {
                        return `<button class="btn btn-sm btn-primary preview" data-id="${data}">Preview</button>`;
                    }
                }
            ],
            pageLength: 10,
            lengthChange: false
        });
    },

    ShowPreview: function (data) {
        var html = `<div class="mb-3">
                    <label>Title</label>
                    <input type="text" class="form-control" value="${data.title}" disabled />
                </div>
                <form>`;

        data.fields.forEach(f => {
            html += `<div class="mb-3">
                    <label>${f.label} ${f.isRequired ? '<span class="text-danger">*</span>' : ''}</label>`;

            if (f.type === 'dropdown') {
                html += `<select class="form-select" disabled>
                        <option selected>${f.selectedOption}</option>
                     </select>`;
            } else {
                html += `<input type="${f.type}" class="form-control" disabled value="${f.selectedOption || ''}" />`;
            }

            html += `</div>`;
        });

        html += `</form>`;

        $('#previewModal .modal-body').html(html);
        $('#previewModal').modal('show');
    },

    SaveOption: function () {
        var dropdownName = $('#newDropdownName').val().trim();

        if (!dropdownName) {
            alert("Dropdown name is required");
            return;
        }

        $.ajax({
            url: '/Dropdown/Create',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify({ Name: dropdownName }),
            success: function (res) {
                if (res.success) {
                    currentSelectForOption.append(`<option value="${res.dropdownId}">${dropdownName}</option>`);
                    currentSelectForOption.val(res.dropdownId);

                    var modalEl = document.getElementById('addOptionModal');
                    bootstrap.Modal.getInstance(modalEl).hide();

                    alert("Option added successfully!");
                }
            }
        });
    },
}

$(document).on('click', '.preview', function () {
    var tr = $(this).closest('tr');
    var rowData = table.row(tr).data();
    Manager.ShowPreview(rowData);
});