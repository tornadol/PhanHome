$(function () {
    var data = JSON.parse(localStorage.getItem("lstitem"));
    if (data != null && data.length >0) {
        var dataHtml = "";
        var html = "";
        var totalPrice = 0;
        for (var i = 0; i < data.length; i++) {
            html = '<tr>';
            html += '<td data-th="Product">';
            html +=   '<div class="row">';
            html +=       '<div class="col-sm-2 hidden-xs">';
            html +=          '<img src='+ data[i].img+' class="img-responsive" /></div>';
            html +=             '<div class="col-sm-10">';
            html +=                 '<h4 class="nomargin">' + data[i].name+ '</h4>';
            html +=             '</div>';
            html +=       '</div>';
            html += '</div>';
            html += '</td>'
            html += '<td data-th="Price">'+data[i].pricevnd+'</td>';
            html += '<td data-th="Quantity">';
            html += '   <input type="number" class="form-control quantity" value=' + data[i].quantity + ' min="1">';
            html += '</td>';
            html += '<input type="hidden" class="price" value=' + data[i].price + ' />';
            html += '<input type="hidden" class="idItem" value=' + data[i].id + ' />';
            html += '<input type="hidden" class="idItemGen" value=' + data[i].idgen + ' />';
            html += '<td data-th="Size" class="text-center">' + data[i].size + '</td>';
            html += '<td data-th="Subtotal" class="text-center text-price">'+data[i].pricevnd+'</td>';
            html += '<td class="actions" data-th=""><button class="btn btn-danger btn-sm"><i class="fa fa-trash-o"></i></button></td>';
            html += '</tr>';
            dataHtml += html;
            totalPrice = parseInt(totalPrice) + parseInt(data[i].price);
        }
        //Add total Price
        var textPrice = totalPrice.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1.") + 'đ';
        $('#totalCurrency').html(textPrice);
        $('#totalPrice').val(totalPrice);
        $('tbody').html(dataHtml);
        $('.loading').fadeOut();
        $('table').fadeIn();
    } else {
        $('.loading').fadeOut();
        $('.no-card').fadeIn();
    }
    
    $('.quantity').click(function () {
        ChangeQuantity($(this));
    });
    $('.quantity').keyup(function () {
        ChangeQuantity($(this));
    });
    //Update cart
    //Delete
    $('.btn-sm').click(function () {
        var idProduct = $(this).closest('tr').find('.idItemGen').val();
        var data = JSON.parse(localStorage.getItem("lstitem"));
        remove('idgen', idProduct, data);
        $(this).closest('tr').fadeOut();
        localStorage.setItem("lstitem", JSON.stringify(data));
    });
})
function ChangeQuantity(e) {
    var quantity = e.val();
    //Price product
    var price = $(e).closest('tr').find('.price').val();
    //Price newg
    var newprice = quantity * price;
    var newtextprice = newprice.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1.") + 'đ';
    $(e).closest('tr').find('.text-price').html(newtextprice);
    //Update Cart
    var idgen = $(e).closest('tr').find('.idItemGen').val();
    var data = JSON.parse(localStorage.getItem("lstitem"));
    //UpdateJsonObject('price', idgen,);
    UpdateJsonObject('quantity', idgen, data, quantity);
    UpdateJsonObject('pricevnd', idgen, data, newtextprice);
    localStorage.setItem("lstitem", JSON.stringify(data));
    //Update price text
    var newpriceTotal = 0;

    $('#totalPrice').val(newpriceTotal);
    $('#totalCurrency').html(newpriceTotal.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1.") + 'đ');
}
function remove(property, num, data) {
    for (var i in data) {
        if (data[i][property] == num) {
            data.splice(i, 1);
        }
    }
}
function UpdateJsonObject(property, num, data, value) {
    for (var i = 0; i < data.length; i++) {
        if (data[i].idgen.toString() === num) {
            data[i][property] = value;
        }
    }
    console.log(data);
}