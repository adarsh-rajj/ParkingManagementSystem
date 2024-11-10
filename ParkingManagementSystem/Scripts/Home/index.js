$(function () {
    AllVehicleData("All");

    $("#VehicleRegistrationTable").on("click", ".js-delete", function () {
        const vehicleId = $(this).data('vehicle-id');
        if (confirm('Are you sure you want to delete this record?')) {
            $.ajax({
                url: `/Home/Delete/${vehicleId}`,
                type: "POST",
                data: JSON.stringify({ id: vehicleId }),
                contentType: "application/json",
                success: function () {
                    AllVehicleData();
                    toastr.error('Vehicle deleted successfully!', 'Deleted');
                },
                error: function () {
                    alert("Error while deleting data");
                }
            });
        }
    });
});

function loadData() {
    const selectedFilter = $('input[name="filter"]:checked').val();
    switch (selectedFilter) {
        case 'parked':
            var blockNo = $("#BlockViewModel").val();
            ParkedVehiclesByBlockNo(blockNo);
            break;
        case 'status':
            var status = $("#VehicleRegistrationViewModel").val();
            AllVehicleData(status);
            break;
        case 'registered':
            AllVehicleData("Not-Parked");
            break;
        case 'capacity':
            AvailableBlock();
            break;
        default:
            return;
    }
}

function ParkedVehiclesByBlockNo(blockNo) {
    $('#VehicleRegistrationTable').DataTable({
        processing: true,
        serverSide: true,
        order: [0, "asc"],
        destroy: true,
        ajax: {
            url: "/Home/GetParkedVehicles",
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
            { data: "VehicleRcNoId", name: "VehicleRcNoId", searchable: true, orderable: true },
            { data: "VehicleRCNo", name: "VehicleRCNo", searchable: true, orderable: true },
            { data: "OwnerName", name: "OwnerName", searchable: true, orderable: true },
            { data: "Model", name: "Model", searchable: true, orderable: true },
            {
                data: "DateOfRegistration",
                name: "DateOfRegistration",
                searchable: true,
                orderable: true,
                render: function (data) {
                    return formatDate(data);
                }
            },
            { data: "Status", name: "Status", searchable: true, orderable: true },
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
                data: "VehicleRcNoId",
                autoWidth: true,
                searchable: false,
                orderable: false,
                render: function (data) {
                    return `<div class='btn-group' role='group'>
                                <button class='btn js-edit' data-vehicle-id='${data}' onclick='Editmodel(${data})'>
                                    <i class="bi bi-pencil-fill"></i>
                                </button>
                                <button class='btn js-delete' data-vehicle-id='${data}'>
                                    <i class="bi bi-trash"></i>
                                </button>
                            </div>`;
                }
            },
        ],
    });
}

function AllVehicleData(filterVehicle) {
    $('#VehicleRegistrationTable').DataTable({
        processing: true,
        serverSide: true,
        order: [0, "asc"],
        destroy: true,
        ajax: {
            url: "/Home/LoadVehicleData",
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
                    filterVehicle: filterVehicle
                });
            }
        },
        columns: [
            { data: "VehicleRcNoId", name: "VehicleRcNoId", searchable: true, orderable: true },
            { data: "VehicleRCNo", name: "VehicleRCNo", searchable: true, orderable: true },
            { data: "OwnerName", name: "OwnerName", searchable: true, orderable: true },
            { data: "Model", name: "Model", searchable: true, orderable: true },
            {
                data: "DateOfRegistration",
                name: "DateOfRegistration",
                searchable: true,
                orderable: true,
                render: function (data) {
                    return formatDate(data);
                }
            },
            { data: "Status", name: "Status", searchable: true, orderable: true },
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
                data: "VehicleRcNoId",
                autoWidth: true,
                searchable: false,
                orderable: false,
                render: function (data) {
                    return `<div class='btn-group' role='group'>
                                <button class='btn js-edit' data-vehicle-id='${data}' onclick='Editmodel(${data})'>
                                    <i class="bi bi-pencil-fill"></i>
                                </button>
                                <button class='btn js-delete' data-vehicle-id='${data}'>
                                    <i class="bi bi-trash"></i>
                                </button>
                            </div>`;
                }
            },
        ],
    });
}

function formatdotNetDate(dotNetDate) {
    const milliseconds = parseInt(dotNetDate.substr(6));
    let currentDate = new Date(milliseconds);
    currentDate.setDate(currentDate.getDate() + 1);
    return currentDate.toISOString().split('T')[0];
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

function Editmodel(id) {
    $("#VehicleRCNo-error").hide();
    $("#OwnerName-error").hide();
    $("#Model-error").hide();
    $("#DateOfRegistration").hide();
    $("#VehicleRCNo").removeClass("is-invalid");
    $("#VehicleRCNo").removeClass("is-valid");
    $("#OwnerName").removeClass("is-invalid");
    $("#OwnerName").removeClass("is-valid");
    $("#Model").removeClass("is-invalid");
    $("#Model").removeClass("is-valid");
    $.ajax({
        url: `/Home/GetRegistrationDataById/${id}`,
        type: 'Post',
        success: function (response) {
            $("#VehicleRcNoId").val(response.VehicleRcNoId);
            $("#VehicleRCNo").val(response.VehicleRCNo);
            $("#OwnerName").val(response.OwnerName);
            $("#Status").val(response.Status);
            $("#Model").val(response.Model);
            $("#DateOfRegistration").val(formatdotNetDate(response.DateOfRegistration));
            $("#CreatedDate").val(formatDate(response.CreatedDate));

            $("#Editvehicle").modal("show");
        },
        error: function () {
            alert("Something goes wrong!");
        }
    });
}

function ModelDismiss() {
    $("#Editvehicle").modal("hide");
    $("#createvehicle").modal("hide");
}

function UpdateRegistrationdata() {

    if (!$("#validateEditHome").valid()) {
        return false;
    }

    var Model =
    {
        VehicleRcNoId: $("#VehicleRcNoId").val(),
        VehicleRCNo: $("#VehicleRCNo").val(),
        OwnerName: $("#OwnerName").val(),
        VehicleRcNoId: $("#VehicleRcNoId").val(),
        Status: $("#Status").val(),
        Model: $("#Model").val(),
        DateOfRegistration: $("#DateOfRegistration").val(),
        CreatedDate: $("#CreatedDate").val(),
    }
    $.ajax({
        url: '/Home/EditVehicle',
        type: 'POST',
        data: { model: Model },
        success: function (response) {
            $("#Editvehicle").modal("hide");
            if (response == "Successful") {
                AllVehicleData();
                toastr.success('Vehicle updated successfully!', 'Success');
            }
        },
        error: function () {
            alert("Something went wrong!");
        }
    });
};

function AllVehicleParkedData(blockNo) {
    $.ajax({
        url: '/Home/GetParkedVehicles',
        type: 'GET',
        data: { blockNo: blockNo },
        success: function (data) {
            $('#VehicleRegistrationTable').html(data);
        },
        error: function (xhr, status, error) {
            console.error('AJAX error:', status, error);
        }
    });
}

function AvailableBlock() {
    $.ajax({
        url: '/Home/AvailableBlock',
        type: 'GET',
        data: JSON,
        success: function (data) {
            $('#VehicleRegistrationTable').html(data);
        },
        error: function (xhr, status, error) {
            console.error('AJAX error:', status, error);
        }
    });
}

function CreateVechile() {
    $("#VehicleRCNos").val(''),
    $("#OwnerNames").val(''),
    $("#Models").val(''),
    $("#DateOfRegistrations").val(''),
    $("#VehicleRCNos-error").hide();
    $("#OwnerNames-error").hide();
    $("#Models-error").hide();
    $("#DateOfRegistrations-error").hide();
    $("#VehicleRCNos").removeClass("is-invalid");
    $("#VehicleRCNos").removeClass("is-valid");
    $("#OwnerNames").removeClass("is-invalid");
    $("#OwnerNames").removeClass("is-valid");
    $("#Models").removeClass("is-invalid");
    $("#Models").removeClass("is-valid");
    $("#DateOfRegistrations").removeClass("is-invalid");
    $("#DateOfRegistrations").removeClass("is-valid");
    $("#createvehicle").modal("show");
}

function CreateRegistrationdata() {
    if (!$("#validateCreateHome").valid()) {
        return false;
    }
    var Model =
    {
        VehicleRCNo: $("#VehicleRCNos").val(),
        OwnerName: $("#OwnerNames").val(),
        Model: $("#Models").val(),
        DateOfRegistration: $("#DateOfRegistrations").val(),
        Status: $("#Statuss").val(),
    }
    $.ajax({
        url: '/Home/Create',
        type: 'POST',
        data: { registrationModel: Model },
        success: function (response) {
            $("#createvehicle").modal("hide");
            if (response == "Successful") {
                toastr.success('Vehicle created successfully!', 'Success');
                AllVehicleData();
            }
        },
        error: function () {
            alert("Something went wrong!");
        }
    });
};

$("#validateEditHome").validate({
    rules: {
        VehicleRCNo: {
            required: true,
            maxlength: 30
        },
        OwnerName: {
            required: true,
            maxlength: 30
        },
        Model: {
            required: true,
            maxlength: 30
        },
        DateOfRegistration: {
            required: true,
            date: true
        }
    },
    messages: {
        VehicleRCNo: {
            required: "Please enter the vehicle registration number",
            maxlength: "Vehicle registration number cannot exceed 30 characters"
        },
        OwnerName: {
            required: "Please enter the owner's name",
            maxlength: "Owner's name cannot exceed 30 characters"
        },
        Model: {
            required: "Please enter the vehicle model",
            maxlength: "Vehicle model cannot exceed 30 characters"
        },
        DateOfRegistration: {
            required: "Please enter the date of registration",
            date: "Please enter a valid date"
        }
    },
    highlight: function (element) {
        $(element).addClass('is-invalid').removeClass('is-valid');
    },
    unhighlight: function (element) {
        $(element).addClass('is-valid').removeClass('is-invalid');
    }
});

$("#validateCreateHome").validate({
    rules: {
        VehicleRCNo: {
            required: true,
            maxlength: 30
        },
        OwnerName: {
            required: true,
            maxlength: 30
        },
        Model: {
            required: true,
            maxlength: 30
        },
        DateOfRegistration: {
            required: true,
            date: true
        }
    },
    messages: {
        VehicleRCNo: {
            required: "Please enter the vehicle registration number",
            maxlength: "Vehicle registration number cannot exceed 30 characters"
        },
        OwnerName: {
            required: "Please enter the owner's name",
            maxlength: "Owner's name cannot exceed 30 characters"
        },
        Model: {
            required: "Please enter the vehicle model",
            maxlength: "Vehicle model cannot exceed 30 characters"
        },
        DateOfRegistration: {
            required: "Please enter the date of registration",
            date: "Please enter a valid date"
        }
    },
    highlight: function (element) {
        $(element).addClass('is-invalid').removeClass('is-valid');
    },
    unhighlight: function (element) {
        $(element).addClass('is-valid').removeClass('is-invalid');
    }
});

function ParkingSystem() {
    $("#parkedradio").prop("checked", true);
    AllVehicleData("All");
}