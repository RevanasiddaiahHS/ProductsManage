function applicationlogout() {
    
    $.ajax({
        type: "GET",
        url: "/Home/Logout",
        dataType: "JSON",
        success: function (response) {
            //window.history.go(-window.history.length);

            window.location.href = '/Login/Index';
        }
    });
}

function clearallfields() {
    $("#productunitid").val('');
    $("#quantity").val('');
    $("#price").val('');
    $("#totalamount").val('');
    $("#productuniqueid").val('');
    var dropDown = document.getElementById("productdropdown");
    dropDown.selectedIndex = "";
}


function GetProducts() {
    $.ajax({
        type: "POST",
        url: "/Home/BindGrid",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: OngridSuccess,
        failure: function (response) {
            alert(response.d);
        },
        error: function (response) {
            alert(response.d);
        }
    });
}


function OngridSuccess(response) {
    
    var model = response;
    $("#producttable tbody").html('');

    $.each(model, function (index) {
        
        var newRowContent = "<tr><input type=\"hidden\" name=\"productuniqueid\" value=" + model[index].id + "><input type=\"hidden\" name=\"userid\" value=" + model[index].userid +"><td>" + model[index].productName + "</td><td>" + model[index].productUnit + "</td><td>" + model[index].productQuantity + "</td><td>" + model[index].productPrice + "</td><td>" + model[index].totalAmount+"</td></tr>";
        $("#producttable tbody").append(newRowContent);

    });   
};

$('#producttable tbody').on('click', 'tr', function () {
    
    var data = {};
    data.id = $(this).closest("tr").find('[name="productuniqueid"]').val();
    data.value = $(this).closest("tr").find('[name="userid"]').val();

    $.ajax({
        type: "get",
        url: "/Home/GetOnclickUserData?uniqueproductid=" + data.id,
        dataType: "JSON",
        success: function (response) {
            
            //window.history.go(-window.history.length);
            $("#productunitid").val(response.productUnit);
            $("#quantity").val(response.productQuantity);
            $("#price").val(response.productPrice);
            $("#totalamount").val(response.totalAmount);
            $("#productuniqueid").val(response.id);
            $('#productdropdown').val(response.productName).trigger('change');

        }
    });
});


function deletetheselectedproduct() {
    var id = $("#productuniqueid").val();
    $.ajax({
        type: "get",
        url: "/Home/DeleteProduct?uniqueproductid=" + id,
        dataType: "JSON",
        success: function (response) {
            
            //window.history.go(-window.history.length);
            $("#productunitid").val('');
            $("#quantity").val('');
            $("#price").val('');
            $("#totalamount").val('');
            $("#productuniqueid").val('');
            $('#productdropdown').val("").trigger('change');
            GetProducts();
        }
    });
};
