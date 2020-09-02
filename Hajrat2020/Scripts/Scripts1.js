function ShowImagePreview(imageUploader, previewImage) {
    if (imageUploader.files && imageUploader.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            $(previewImage).attr('src', e.target.result);
        }
        reader.readAsDataURL(imageUploader.files[0]);
    }
}
$(document).ready(function () {
    var modal = document.getElementById("myModal");
    var img = document.getElementById("imagePreview");
    var modalImg = document.getElementById("img01");
    var captionText = document.getElementById("caption");
    if (img == null) img = 'text'
    img.onclick = function () {
        modal.style.display = "block";
        modalImg.src = this.src;
        captionText.innerHTML = this.alt;
    }
    var span = document.getElementById("myModal");
    //var span = document.getElementsByClassName("modal")[0];
    if (span == null) span = 'text'
    span.onclick = function () {
        modal.style.display = "none";
    }
});

$(document).ready(function () {
    var modal2 = document.getElementById("myModal2");
    var img2 = document.getElementById("donationImage");
    var modalImg2 = document.getElementById("img02");
    var captionText2 = document.getElementById("caption2");
    if (img2 == null) img2 = 'text'
    img2.onclick = function () {
        modal2.style.display = "block";
        modalImg2.src = this.src;
        captionText2.innerHTML = this.alt;
    }
    var span2 = document.getElementById("myModal2");
    //var span2 = document.getElementsByClassName("modal")[1];
    if (span2 == null) span2 = 'text'
    span2.onclick = function () {
        modal2.style.display = "none";
    }
});

$(document).ready(function () {
    $("#TypeOfHelpId").change(function () {
        if ($(this).val() === '1') {
            $('div[name=moneyDiv]').show();
        } else {
            $('div[name=moneyDiv]').hide();
        }
    });
});

$(document).ready(function () {
    if ($("#TypeOfHelpId").val() === '1') {
        $('div[name=moneyDiv]').show();
    } else {
        $('div[name=moneyDiv]').hide();
    }
});

$(document).ready(function () {
    $("#changePassword").click(function () {
        if ($('#showPasswordSection').css('display') == 'none') {
            $('div[name=showPasswordSection]').show();
        }
    });
    if ($('#changePasswordInput').val() != null) {
        if ($('#changePasswordInput').val().length && $('#changePasswordInput').length) {
            $('div[name=showPasswordSection]').show();
        }
    }
});

//$('#buttonDonationsTable').click(function () {
//    var printme = document.getElementById('donationsTable');
//    CallPrint(printme);
//});

//$('#buttonDonationUserTable').click(function () {
//    var printme = document.getElementById('donationUserTable');
//    CallPrint(printme);
//});

$("body").on("click", "#downloadDonationsTable", function () {
    smallerScreen();
    convertToPdf("#donationsTable", "pdfDonations.pdf")
    biggerScreen();
});

$("body").on("click", "#downloadDonationUserTable", function () {
    smallerScreen();
    convertToPdf("#donationUserTable","pdfUDonations.pdf")
    biggerScreen();
});

function convertToPdf(tableName,pdfName) {
    html2canvas($(tableName)[0], {
        onrendered: function (canvas) {
            var data = canvas.toDataURL();
            var docDefinition = {
                content: [{
                    image: data,
                    width: 500
                }]
            };
            pdfMake.createPdf(docDefinition).download(pdfName);
        }
    });
}

function smallerScreen() {
    if (screen.width < 1024) {
        document.getElementById("viewport").setAttribute("content", "width=1200px");
    }
}

function biggerScreen() {
    setTimeout(function () {
        if (screen.width < 1024) {
            document.getElementById("viewport").setAttribute("content", "width=device-width, initial-scale=1");
        }
    }, 1000);
}
//function CallPrint(printme) {
//    var htmlToPrint = '' +
//        '<style type="text/css">' +
//        'table, th, td {' +
//        'border:1px solid black;' +
//        'border-collapse:collapse;' +
//        '}' +
//        '.hideRow {' +
//        'display:none' +
//        '}' +
//        'td, th {' +
//        'text-align: center;' +
//        'vertical-align: middle;' +
//        '}' +
//        '</style>';
//    htmlToPrint += printme.outerHTML;
//    var wme = window.open("", "", "width=900,height=700");
//    wme.document.write(htmlToPrint);
//    wme.document.close();
//    wme.focus();
//    wme.print();
//    wme.close();
//}

$(function () {
    $('.userDetail').click(function () {
        var id = $(this).data('assigned-id');
        var route = '/User/GetUser?id=' + id;
        $('#partialUser').load(route);
    });
});

$(function () {
    $('.familyDetail').click(function () {
        var id = $(this).data('assigned-id');
        var route = '/FamilyInNeed/GetFamily?id=' + id;
        $('#partialFamily').load(route);
    });
});

$(function () {
    $('.donationDetail').click(function () {
        var id = $(this).data('assigned-id');
        var route = '/Donations/GetDonation?id=' + id;
        $('#partialDonation').load(route);
    });
});