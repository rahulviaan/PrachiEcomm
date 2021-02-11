
$(document).ready(function () {

    var oTable = $('#myDataTable').dataTable({
        "bServerSide": true,
        "sAjaxSource": "CompanyAddress/AjaxHandler",
        "bProcessing": true,
        "aoColumns": [
                        {
                            "sName": "StateName",
                            "bSearchable": false,
                            "bSortable": false,
                            "fnRender": function (oObj) {
                                return '<a href=\"Company/Details/' + oObj.aData[0] + '\">View</a>';
                            }
                        },
			            { "sName": "StateName" },
			            { "sName": "H_No" },
			            { "sName": "Address" },
                        { "sName": "NearBy" },
                        { "sName": "City" },
                        { "sName": "Pincode" },
                        { "sName": "mobileno" },
                        { "sName": "Email" }
        ]
    });
});
