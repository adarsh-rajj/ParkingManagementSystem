$(function () {

    AllParkingData("All");

    $("#ParkingAllocationTable").on("click", ".js-delete", function () {
        const parkingId = $(this).data('parking-id');
        if (confirm('Are you sure you want to delete this record?')) {
            console.log("Confirming deletion for Parking:", parkingId);
            $.ajax({
                url: `/ParkingAllotment/Delete/${parkingId}`,
                type: "POST",
                data: JSON.stringify({ id: parkingId }),
                contentType: "application/json",
                success: function () {
                    AllParkingData();
                    toastr.error('Parking deleted successfully!', 'Success');
                },
                error: function () {
                    alert("Error while deleting data");
                }
            });
        }
    });
});

$.validator.addMethod("dateRange", function (value, element, params) {
    var fromDateValue = $(params[0]).val();
    var fromDate = fromDateValue ? new Date(fromDateValue) : new Date();
    var toDate = new Date(value);
    return this.optional(element) || toDate >= fromDate;
}, "Parking Date To must be greater than Parking Date From.");

function loadData() {
    var blockNo = $("#BlockViewModel").val();
    AllParkingData(blockNo)
}

function AllParkingData(blockNo) {
    $('#ParkingAllocationTable').DataTable({
        processing: true,
        serverSide: true,
        order: [0, "asc"],
        destroy: true,
        ajax: {
            url: "/ParkingAllotment/LoadParkingData",
            type: "POST",
            datatype: "json",
            contentType: "application/json",
            data: function (d) {
                return JSON.stringify({
                    draw: d.draw,
                    start: d.start,
                    length: d.length,
                    search: d.search,
                    order: d.order,
                    columns: d.columns,
                    blockNo: blockNo
                });
            }
        },
        columns: [
            { data: "BlockNo", name: "BlockNo", searchable: true, orderable: true },
            { data: "VehicleRcNoId", name: "VehicleRcNoId", searchable: true, orderable: true },
            { data: "Description", name: "Description", searchable: true, orderable: true },
            {
                data: "ParkingDateFrom",
                name: "ParkingDateFrom",
                searchable: true,
                orderable: true,
                render: function (data) {
                    return formatDate(data);
                }
            },
            {
                data: "ParkingDateTo",
                name: "ParkingDateTo",
                searchable: true,
                orderable: true,
                render: function (data) {
                    return formatDate(data);
                }
            },
            {
                data: "CreatedDate",
                name: "CreatedDate",
                searchable: true,
                orderable: true,
                render: function (data) {
                    return formatDate(data);
                }
            },
            {
                data: "ModifiedDate",
                name: "ModifiedDate",
                searchable: true,
                orderable: true,
                render: function (data) {
                    return formatDate(data);
                }
            },
            {
                data: "AllocationId",
                autoWidth: true,
                searchable: false,
                orderable: false,
                render: function (data) {
                    return `<div class='btn-group' role='group'>
                                <button class='btn js-edit' data-parking-id='${data}' onclick='Editmodel(${data})'>
                                    <i class="bi bi-pencil-fill"></i>
                                </button>
                                <button class='btn js-delete' data-parking-id='${data}'>
                                    <i class="bi bi-trash"></i>
                                </button>
                            </div>`;
                }
            },
        ],
    });
}

function formatDate(dateString) {
    const regex = /\/Date\((\d+)\)\//;
    const match = regex.exec(dateString);
    if (match) {
        const timestamp = parseInt(match[1], 10);
        const date = new Date(timestamp);
        const day = String(date.getDate()).padStart(2, '0');
        const month = String(date.getMonth() + 1).padStart(2, '0');
        const year = date.getFullYear();
        return `${day}/${month}/${year}`;
    }
    return dateString;
}

function formatdotNetDate(dotNetDate) {
    const milliseconds = parseInt(dotNetDate.substr(6));
    let currentDate = new Date(milliseconds);
    currentDate.setDate(currentDate.getDate() + 1);
    return currentDate.toISOString().split('T')[0];
}

function Editmodel(id) {
    $("#Description-error").hide();
    $("#ParkingDateFrom-error").hide();
    $("#ParkingDateTo-error").hide();
    $("#BlockNo").removeClass("is-valid");
    $("#Description").removeClass("is-invalid");
    $("#Description").removeClass("is-valid");
    $("#ParkingDateFrom").removeClass("is-invalid");
    $("#ParkingDateFrom").removeClass("is-valid");
    $("#ParkingDateTo").removeClass("is-invalid");
    $("#ParkingDateTo").removeClass("is-valid");

    $.ajax({
        url: `/ParkingAllotment/GetParkingDateById/${id}`,
        type: 'Post',
        success: function (response) {
            $("#AllocationId").val(response.AllocationId);
            $("#VehicleRcNoId").val(response.VehicleRcNoId);
            $("#BlockNo").val(response.BlockNo);
            $("#VehicleRCNo").val(response.VehicleRCNo);
            $("#Description").val(response.Description);
            $("#ParkingDateTo").val(formatdotNetDate(response.ParkingDateTo));
            $("#ParkingDateFrom").val(formatdotNetDate(response.ParkingDateFrom));
            $("#CreatedDate").val(formatdotNetDate(response.CreatedDate));
            $("#PreviousBlockNo").val(response.BlockNo);


            const blockDropdown = $("#BlockNo");
            blockDropdown.empty();
            $.each(response.Blocks, function (index, block) {
                blockDropdown.append($('<option></option>').val(block.BlockNo).html(block.BlockNo));
            });
            blockDropdown.val(response.BlockNo);

            $("#editParking").modal("show");
        },
        error: function () {
            alert("Something goes wrong!");
        }
    });
}

function UpdateParkingdata() {
    if (!$("#validateEditParking").valid()) {
        return false;
    }
    var Model =
    {
        AllocationId: $("#AllocationId").val(),
        VehicleRcNoId: $("#VehicleRcNoId").val(),
        BlockNo: $("#BlockNo").val(),
        VehicleRCNo: $("#VehicleRCNo").val(),
        Description: $("#Description").val(),
        ParkingDateTo: $("#ParkingDateTo").val(),
        ParkingDateFrom: $("#ParkingDateFrom").val(),
        CreatedDate: $("#CreatedDate").val(),
        PreviousBlock: $("#PreviousBlockNo").val(),
    }
    $.ajax({
        url: '/ParkingAllotment/EditParking',
        type: 'POST',
        data: { model: Model },
        success: function (response) {
            $("#editParking").modal("hide");
            if (response == "Successful") {
                AllParkingData();
                toastr.success('Parking updated successfully!', 'Success');
            }
        },
        error: function () {
            alert("Something went wrong!");
        }
    });
};

function ModelDismiss() {
    $("#editParking").modal("hide");
    $("#createParking").modal("hide");
}

function CreateParking() {
    $("#BlockNos").val(''),
    $("#VehicleRcNoIds").val(''),
    $("#Descriptions").val(''),
    $("#ParkingDateFroms").val(''),
    $("#ParkingDateTos").val(''),
    $("#Descriptions-error").hide();
    $("#ParkingDateFroms-error").hide();
    $("#ParkingDateTos-error").hide();
    $("#Descriptions").removeClass("is-invalid");
    $("#Descriptions").removeClass("is-valid");
    $("#ParkingDateFroms").removeClass("is-invalid");
    $("#ParkingDateFroms").removeClass("is-valid");
    $("#ParkingDateTos").removeClass("is-invalid");
    $("#ParkingDateTos").removeClass("is-valid");
    $("#createParking").modal("show");
}

function CreateParkingdata() {
    if (!$("#allotmentForm").valid()) {
        return false;
    }
    var Model = {
        BlockNo: $("#BlockNos").val(),
        VehicleRcNoId: $("#VehicleRcNoIds").val(),
        Description: $("#Descriptions").val(),
        ParkingDateFrom: $("#ParkingDateFroms").val(),
        ParkingDateTo: $("#ParkingDateTos").val(),
    };

    $.ajax({
        url: '/ParkingAllotment/Create',
        type: 'post',
        contentType: 'application/json',
        data: JSON.stringify(Model),
        success: function (response) {
            $("#createParking").modal("hide");
            if (response == "Successful") {
                AllParkingData();
                toastr.success('Parking alloted successfully!', 'Success');
            }
        },
        error: function () {
            alert("Something went wrong!");
        }
    });
};

$("#validateEditParking").validate({
    rules: {
        BlockNo: {
            required: true,
        },
        VehicleRcNoId: {
            required: true
        },
        Description: {
            required: true
        },
        ParkingDateFrom: {
            required: true,
            date: true
        },
        ParkingDateTo: {
            required: true,
            date: true,
            dateRange: ["#ParkingDateFrom"]
        }
    },
    messages: {
        BlockNo: {
            required: "Please select the block number",
        },
        VehicleRcNoId: {
            required: "Please select the vehicle rc no id.",
        },
        Description: {
            required: "Please enter the description",
        },
        ParkingDateFrom: {
            required: "Please enter the parking date from",
            date: "Please enter a valid date"
        },
        ParkingDateTo: {
            required: "Please enter the parking date to",
            date: "Please enter a valid date",
            dateRange: "Parking Date To must be greater than Parking Date From."
        },
    },
    highlight: function (element) {
        $(element).addClass('is-invalid').removeClass('is-valid');
    },
    unhighlight: function (element) {
        $(element).addClass('is-valid').removeClass('is-invalid');
    }
});

$("#allotmentForm").validate({
    rules: {
        BlockNo: {
            required: true,
        },
        VehicleRcNoId: {
            required: true
        },
        Description: {
            required: true
        },
        ParkingDateFrom: {
            required: true,
            date: true
        },
        ParkingDateTo: {
            required: true,
            date: true,
            dateRange: ["#ParkingDateFroms"]
        }
    },
    messages: {
        BlockNo: {
            required: "Please select the block number",
        },
        VehicleRcNoId: {
            required: "Please select the vehicle rc no id.",
        },
        Description: {
            required: "Please enter the description",
        },
        ParkingDateFrom: {
            required: "Please enter the parking date from",
            date: "Please enter a valid date"
        },
        ParkingDateTo: {
            required: "Please enter the parking date to",
            date: "Please enter a valid date",
            dateRange: "Parking Date To must be greater than or equal to Parking Date From."
        },
    },
    submitHandler: function (form) {
        form.submit();
    },
    highlight: function (element) {
        $(element).addClass('is-invalid').removeClass('is-valid');
    },
    unhighlight: function (element) {
        $(element).addClass('is-valid').removeClass('is-invalid');
    }
});

function ParkingSystem() {
    AllParkingData("All");
}