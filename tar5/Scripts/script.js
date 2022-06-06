// TODO: FIX PAGINATION PAGES OVERFLOW
// TODO: ONLY FETCH WITH FILTERS
/**
 * 
 Google Maps
 *
 */
// mainPageDist - distance of map from top, to make it sticky once you scroll to it
var map, apartmentMap, activeInfoWindow = "", markersArray = [], mainPageDist;

// main screen map
function initMap() {
    map = new google.maps.Map(document.getElementById("map"), {
        center: { lat: 52.377282, lng: 4.894553 },
        zoom: 12,
    });
}

// apartment page map
function initApartmentMap(data) {
    apartmentMap = new google.maps.Map(document.getElementById("apartmentMap"), {
        center: { lat: parseFloat(data.Latitude), lng: parseFloat(data.Longitude) },
        zoom: 15,
    });
    marker = new google.maps.Marker({
        position: new google.maps.LatLng(parseFloat(data.Latitude), parseFloat(data.Longitude)),
        title: data.Name,
        map: apartmentMap
    });
}

// adding marker to main screen map
function addMapMarker(mapInfo, data) {
    let infowindow = new google.maps.InfoWindow({
        content: mapInfo
    });

    let marker = new google.maps.Marker({
        position: { lat: parseFloat(data.Latitude), lng: parseFloat(data.Longitude) },
        map,
        title: data.Name,
        label: data.Price + "$",
        icon: "/Content/marker.png"
    });
    markersArray.push(marker);

    marker.addListener('click', function () {
        if (activeInfoWindow) { activeInfoWindow.close(); }
        infowindow.open(map, marker);
        activeInfoWindow = infowindow;
    });
}

// clearing all markers on map
function clearOverlays() {
    for (var i = 0; i < markersArray.length; i++) {
        markersArray[i].setMap(null);
    }
    markersArray.length = 0;
}

// make the map sticky on left
$(document).scroll(function () { 
    if ($(this).scrollTop() >= mainPageDist) {
        if (!isMobile()) {
            $(".mapBlock").css({ "position": "fixed", "top": "0" });
        }
    } else {
        if (!isMobile()) {
            $(".mapBlock").css({ "position": "absolute", "top": "auto" });
        }
    }
});


// -----------------------------------------------------------------------------------------

/**
 * 
 Main Page Apartments
 *
 */
var setHeight = true;

function loadPage() {
    showloading();
    readFiltered(filter, false, true);
}

function showloading() {
    $(".apartmentsList").html('<div class="d-flex justify-content-center"><div class="spinner-grow" role="status"></div> <strong>Loading...</strong></div>');
}

// data is a dictionary, with a key which is number of total rows of query, and the value is the data array
function readSuccess(data) {
    if (data == null) {
        $(".apartmentsList").html('<div class="alert alert-danger">Error Reading Apartments Data</div>');
        return;
    }

    var rows = Object.keys(data)[0]; // number of total rows of the query
    // if pagination is hidden, then create one
    if ($(".pagination").css("visibility") == "hidden") {
        initPagination(rows, 15);
    }
    $("#results b").text(rows);

    var readData = data[rows]; // apartments data
    var row = $('<div class="row"></div>');
    $(".apartmentsList").html("");

    if (rows == 0) {
        $(".apartmentsList").html('<div class="alert alert-info">No Results Found</div>');
    }

    for (var i = 0; i < readData.length; i++) {

        // 3 apartments in a row
        if (i % 3 == 0) { 
            row.addClass("rowSpace");
            $(".apartmentsList").append(row);
            row = $('<div class="row"></div>');
        }

        let apartment = '<div class="apartment col-12 col-lg-4 " data-apartmentId="' + readData[i].Id + '"><div class="card apartmentCard">';
        apartment += '<img class="card-img-top" src="' + readData[i].Picture_url + '" alt="' + readData[i].Name + '">';
        apartment += '<div class="card-body apartmentCardBody"><h5 class="card-title">' + readData[i].Name + '</h5>';
        apartment += '<div class="card-text">';
        apartment += '<p style="font-size:10pt;"><span style="color:blue;">' + readData[i].Host_name + '</span> &#183; ' + readData[i].Property_type + '</p>';
        apartment += '<p><i class="bi bi-people-fill"></i> ' + readData[i].Accommodates + ' Guests';
        apartment += ' &#183; ' + readData[i].Bedrooms + ' Bedrooms</p>';
        apartment += '<p><i class="bi bi-chat-left-text-fill"></i> ' + readData[i].Number_of_reviews + ' Reviews &#183; ' + readData[i].Review_scores_rating + ' <i class="bi bi-star-fill"></i></p>';
        apartment += '</div><div class="pricelabel"><span class="bigprice">' + readData[i].Price + '</span>$ /Night</div>';
        apartment += '<div class="btn btn-primary openApartment">View</div></div></div>';

        row.html(row.html() + apartment);

        // if last row doesn't have 3 items
        if (i + 1 == readData.length) {
            row.addClass("rowSpace");
            $(".apartmentsList").append(row);
        }

        let mapInfo = apartment.replace('col-12 col-lg-4 ', "");
        addMapMarker(mapInfo, readData[i]); // initializing map markers
    }

    // making a fixed height only one time, for smoother scrolling
    if (setHeight) {
        $(".apartmentsBlock").css("height", $(".apartmentsBlock").height());
        setHeight = false;
    }
}


// -----------------------------------------------------------------------------------------

/**
 *
 Pagination
 *
 */
var currentPage = 1, currentReviewsPage = 1, maxPages = 0, maxMainPages = 0;
var scrollBackUp = false; // scroll back up after pressing on page button?

function initPagination(rows, maxrows) {
    maxPages = Math.ceil(rows / maxrows);
    if (apartmentData == null) { // if you are on main page
        maxMainPages = maxPages;
    }

    $(".pagination").html("");
    $(".pagination").append($('<li class="page-item" data-pageid="pre"><a class="page-link" href="javascript:void(0);">Previous</a></li>'));
    for (var i = 1; i <= maxPages; i++) {
        $(".pagination").append($('<li class="page-item ' + (i == 1 ? "active" : "") + '" data-pageid="' + i + '"><a class="page-link" href="javascript:void(0);">' + i + '</a></li>'));
    }
    $(".pagination").append($('<li class="page-item" data-pageid="next"><a class="page-link" href="javascript:void(0);">Next</a></li>'));
    $(".pagination").css("visibility", "visible");
}

// click on page button
$(document).on("click", ".page-item", function () {
    var page = $(this).attr("data-pageid");
    if (page == 0) {
        return;
    }

    // check if main page or apartment page
    let pageTemp = currentReviewsPage;
    if (apartmentData == null) {
        pageTemp = currentPage;
    }

    // click on previous
    if (page == "pre") {
        if (pageTemp - 1 >= 1) {
            pageTemp--;
            page = pageTemp;
        } else {
            return;
        }

      // click on next
    } else if (page == "next") {
        if (pageTemp + 1 <= maxPages) {
            pageTemp++;
            page = pageTemp;
        } else {
            return;
        }
    }

    filter["page"] = page;
    pageTemp = parseInt(page);

    $(".page-item").removeClass("active"); // removing current page active color

    // check if main page or apartment page
    if (apartmentData == null) {
        $(".page-item:eq(" + (pageTemp + maxPages + 2) + ")").addClass("active");
        currentPage = pageTemp;
        readFiltered(filter, true, false);
    } else {
        $(".page-item:eq(" + pageTemp + ")").addClass("active");
        currentReviewsPage = pageTemp;
        scrollBackUp = true;
        readReviews(page);
    }
});


// -----------------------------------------------------------------------------------------

/**
 *
 Filter & Searching
 *
 */
var filter;

// search text field submit
function searchSubmit() {
    clearFilter();
    filter["keywords"] = $("#keywords").val();
    readFiltered(filter, true, true);
    return false;
}

// main page filter submit
function filterSubmit() {
    let priceFrom = ($("#priceFrom").val() != "") ? parseInt($("#priceFrom").val()) : 0;
    let priceTo = ($("#priceTo").val() != "") ? parseInt($("#priceTo").val()) : 1000000;
    if (priceFrom > priceTo) {
        $("#priceFrom").focus();
        return false;
    }

    filter = {
        priceFrom: priceFrom,
        priceTo: priceTo,
        rating: parseFloat($('#rating').val()),
        numOfRooms: parseInt($("#rooms").val()),
        distFromCenter: parseFloat($('#distance').val()),
        fromDate: $("#filterFromDate").val(),
        toDate: $("#filterToDate").val(),
        page: 1,
        keywords: ""
    };

    readFiltered(filter, true, true);

    return false;
}

// read the apartments data filtered
// filter object, scrollto - to scroll back up? , rebuildPagination - rebuild pagination?
function readFiltered(filter, scrollto, rebuildPagination) {
    showloading();
    if (rebuildPagination) {
        currentPage = 1;
        $(".pagination").css("visibility", "hidden");
    }
    if (scrollto) {
        $([document.documentElement, document.body]).animate({
            scrollTop: $("#results").offset().top
        }, 300);
    }
    clearOverlays();
    ajaxCall("POST", "../api/Apartment", JSON.stringify(filter), readSuccess, readECB);
}

// default filter that doesn't filter results
function clearFilter() {
    filter = {
        priceFrom: 0,
        priceTo: 1000000,
        rating: 0,
        numOfRooms: 0,
        distFromCenter: 0,
        fromDate: "2000-01-01",
        toDate: "2000-01-02",
        page: currentPage,
        keywords: ""
    };
}


// -----------------------------------------------------------------------------------------

/**
 *
 Apartment Page
 *
 */

// currentScreenLocation - save the distance from top before entering apartment page, so when you exit you return to currect position you have been
var apartmentData, currentScreenLocation = 0, numOfReviews = 0;

$(document).on("click", ".apartment", function () {
    $("#searchBox,#filterBox,.mainPage").fadeOut(300);
    $("#backButton,#apartmentPage").fadeIn(300);
    scrollBackUp = false;

    viewApartment(parseInt($(this).attr("data-apartmentId")));

    currentScreenLocation = $(document).scrollTop();
    $([document.documentElement, document.body]).animate({
        scrollTop: 0
    }, 300);
});

function viewApartment(id) {
    ajaxCall("GET", "../api/Apartment/" + id, "", viewSuccess, readECB);
}

// building the apartment page
function viewSuccess(data) {
    $("#apartment-picture").css("background-image", 'url("' + data.Picture_url + '")');
    $("#modalPic").attr("src", data.Picture_url);
    $("#apartment-name").text(data.Name);
    $("#apartment-price").text(data.Price);
    $("#apartment-minimum").text(data.Minimum_nights);
    $("#apartment-hostname").text(data.Host_name);
    $("#apartment-rating").text(data.Review_scores_rating);
    $("#apartment-description").html(data.Description);

    $("#apartment-propertyType").text(data.Property_type);
    $("#apartment-roomType").text(data.Room_type);
    $("#apartment-accommodates").text(data.Accommodates);
    $("#apartment-bedrooms").text(data.Bedrooms);
    $("#apartment-beds").text(data.Beds);
    $("#apartment-bathrooms").text(data.Bathrooms_text);

    var row = "<tr>";
    for (var i = 0; i < data.Amenities.length; i++) {
        row += "<td>" + data.Amenities[i] + "</td>";
        if ((i + 1) % 3 == 0 || (i + 1) == data.Amenities.length) {
            row += "</tr>";
            $("#apartment-amenities").append(row);
            row = "<tr>";
        }
    }

    $("#apartment-neighborhood").html(data.Neighborhood_overview);
    $("#apartment-reviews").text(data.Number_of_reviews);

    $("#apartment-accuracy").text(data.Review_scores_accuracy);
    $("#apartment-cleanliness").text(data.Review_scores_cleanliness);
    $("#apartment-checkin").text(data.Review_scores_checkin);
    $("#apartment-communication").text(data.Review_scores_communication);
    $("#apartment-location").text(data.Review_scores_location);
    $("#apartment-value").text(data.Review_scores_value);

    initApartmentMap(data);

    currentPage = 1;
    apartmentData = data;
    numOfReviews = parseInt(data.Number_of_reviews);

    readReviews(currentPage); // loading the reviews for the page
    initPagination(numOfReviews, 30); 
}


// -----------------------------------------------------------------------------------------

/**
 *
 Reviews
 *
 */

function readReviews(page) {
    showReviewsLoading();
    ajaxCall("GET", "../api/ReadReviews/" + apartmentData.Id + "/" + page, "", reviewSuccess, readECB);
}

function showReviewsLoading() {
    $("#reviews").html('<div class="d-flex justify-content-center"><div class="spinner-grow" role="status"></div> <strong>Loading...</strong></div>');
}

function reviewSuccess(data) {
    var comments = "";
    $("#reviews").css("height", "auto");
    for (var i = 0; i < data.length; i++) {
        comments += '<div class="card reviewCard"><div class="card-header">' + data[i].Reviewer_name + '&emsp; &#183; &emsp;' + data[i].Date.split(" ")[0] + '</div><div class="card-body"><blockquote class="blockquote mb-0"><p>' + data[i].Comments + '</p></blockquote></div></div>';
    }

    $("#reviews").html(comments);
    $("#reviews").css("height", $("#reviews").height());

    if (scrollBackUp) {
        $([document.documentElement, document.body]).animate({
            scrollTop: $("#apartment-reviews").offset().top
        }, 300);
    }
}


// -----------------------------------------------------------------------------------------

/**
 *
 Login & Register
 *
 */
var myUserId = 0;
function signup() {
    let user = {
        id: 0,
        email: $("#signup-email").val(),
        username: $("#signup-username").val(),
        password: $("#signup-password").val()
    }
    ajaxCall("POST", "../api/user", JSON.stringify(user), signupSCB, readECB);
    return false;
}

function signupSCB(data) {
    if (data == "1") {
        alert("Registered Successfully!");
        $("#signup-email").val("");
        $("#signup-username").val("");
        $("#signup-password").val("");
        $('#registerModal').modal('hide');
    } else {
        alert("Couldn't Register");
    }
}

function login() {
    ajaxCall("GET", "../api/LoginUser/" + $("#login-email").val() + "/" + $("#login-password").val(), "", loginSCB, readECB);
    return false;
}

function loginSCB(data) {
    if (data != null) {
        localStorage.setItem('username', data.Username);
        localStorage.setItem('userId', data.Id);
        localStorage.setItem('email', data.Email);

        $('#loginModal').modal('hide');
        showAfterLogin();
    } else {
        alert("Email or Password Incorrect");
    }
}

// checking if user logged
function isLogged() {
    return (localStorage.getItem('userId') != null && localStorage.getItem('username') != null && localStorage.getItem('email') != null);
}

// recreating UI after login
function showAfterLogin() {
    $(".beforeLogin").hide();
    $(".afterLogin").show();
    $("#usernameLabel").text(localStorage.getItem('username'));
    myUserId = localStorage.getItem('userId');
    if (localStorage.getItem('email') == 'admin@gmail.com') {
        $("#adminPanelButton").show();
    }
}


// -----------------------------------------------------------------------------------------

/**
 *
  Orders Page
 *
 */
var fromDate = "", toDate = "", tbl;

// calculate price of apartment between dates
function calculatePrice() {
    let totalNights = Math.ceil((toDate.getTime() - fromDate.getTime()) / (1000 * 3600 * 24));
    return (totalNights * apartmentData.Price);
}

function order() {
    if (myUserId == 0) {
        alert("Please Login First");
    } else {
        if (fromDate != "" && toDate != "") {
            let order = {
                id: 0,
                userId: myUserId,
                apartmentId: apartmentData.Id,
                fromDate: fromDate,
                toDate: toDate,
                orderDate: "",
                totalPrice: parseFloat(calculatePrice()),
                cancelDate: ""
            };
            ajaxCall("POST", "../api/order", JSON.stringify(order), orderSCB, readECB);
        }

    }
    return false;
}

function orderSCB(data) {
    if (data == 1) {
        $("#fromDate").val("");
        $("#toDate").val("");
        $("#orderComplete").slideDown(200);
    } else if (data == 0) {
        alert("Order details are not correct");
    } else {
        alert("The apartment is booked in these dates");
    }
}

function readMyOrders() {
    ajaxCall("GET", "../api/order/" + myUserId, "", ordersSuccess, readECB);
}


function ordersSuccess(data) {

    // if there are no order or problem loading them
    if (data == null) {
        $("#noOrdersAlert").removeClass("alert-dark").addClass("alert-danger").text("Error Reading Orders Data").show();
        $("#ordersTable").hide();
        return;
    }
    if (data.length == 0) {
        $("#noOrdersAlert").removeClass("alert-dark").removeClass("alert-danger").addClass("alert-dark").show();
        $("#ordersTable").hide();
        return;
    }

    $("#noOrdersAlert").hide();
    $("#ordersTable").show();

    let orders = data;
    try {
        tbl = $('#ordersTable').DataTable({
            "paging": false,
            data: orders,
            pageLength: 100,
            responsive: true,
            "searching": false,
            columns: [
                {
                    data: "Id"
                },
                {
                    data: "Picture",
                    render: function (data, type, row, meta) {
                        return '<img src="' + data + '" style="height:50px;">';
                    }
                },
                {
                    data: "ApartmentId",
                    visible: false
                },
                {
                    data: "Name",
                    render: function (data, type, row, meta) {
                        return '<span class="apartment apartmentLink" data-apartmentId="' + row.ApartmentId + '">' + data.substring(0, 20) + '...</span>';
                    }
                },
                {
                    data: "FromDate"
                },
                {
                    data: "ToDate"
                },
                {
                    data: "OrderDate"
                },
                {
                    data: "TotalPrice",
                    render: function (data, type, row, meta) {
                        return data + '$';
                    }
                },
                {
                    data: "CancelDate",
                    visible: false
                },
                {
                    render: function (data, type, row, meta) {
                        if (row.CancelDate != "") {
                            return "<span style='font-size:10pt;color:red;'>Order Cancelled</span>";
                        }
                        if (dateDiff(row.FromDate.split("/")) <= 2) { // if there are less than 2 days till checkin, disallow cancel
                            return "";
                        }
                        let dataOrder = "data-orderId='" + row.Id + "'";
                        deleteBtn = "<button type='button' class = 'cancelBtn btn btn-danger' " + dataOrder + "> Cancel </button>";
                        return deleteBtn;
                    }
                }
            ],
        });
    }

    catch (err) {
        alert(err);
    }
}

// calculating number of days from today to given date
function dateDiff(from) {
    let dt = new Date();
    const date1 = new Date(dt.getFullYear(), dt.getMonth() + 1, dt.getDate());
    const date2 = new Date(from[2], from[1], from[0]);
    const diffTime = Math.abs(date2 - date1);
    const diffDays = Math.ceil(diffTime / (1000 * 60 * 60 * 24));
    return diffDays;
}

// cancel order
$(document).on("click", ".cancelBtn", function () {
    markSelected(this);
    var orderId = this.getAttribute('data-orderId');
    swal({ // this will open a dialouge
        title: "Press OK to cancel the order",
        text: "",
        icon: "warning",
        buttons: true,
        dangerMode: true
    })
        .then(function (willCancel) {
            if (willCancel) cancelOrder(orderId);
            else swal("Abored!");
        });
});

function markSelected(btn) {
    $("#carsTable tr").removeClass("selected"); // remove seleced class from rows that were selected before
    row = (btn.parentNode).parentNode; // button is in TD which is in Row
    row.className = 'selected'; // mark as selected
}

function redrawTable(tbl, data) {
    tbl.clear();
    for (var i = 0; i < data.length; i++) {
        tbl.row.add(data[i]);
    }
    tbl.draw();
}

function cancelOrder(orderId) {
    ajaxCall("DELETE", "../api/CancelOrder/" + orderId + "/" + myUserId, "", cancelSuccess, readECB);
}

function cancelSuccess(ordersData) {
    tbl.clear();
    redrawTable(tbl, ordersData);
    swal("Cancelled Successfully!", "", "success");
}

// method for adding days to Date object
Date.prototype.addDays = function (days) {
    var date = new Date(this.valueOf());
    date.setDate(date.getDate() + days);
    return date;
}



// -----------------------------------------------------------------------------------------



$(document).ready(function () {
    window.initMap = initMap;
    mainPageDist = $(".mainPage").offset().top;

    clearFilter();
    loadPage();

    $("#filterForm").submit(filterSubmit);
    $("#searchForm").submit(searchSubmit);
    $("#signupForm").submit(signup);
    $("#loginForm").submit(login);
    $("#orderForm").submit(order);

    // show value of rating scroller
    $('#rating').on("change mousemove", function () {
        let dist = $(this).val();
        if (dist == "0") {
            $("#ratingText").text("All");
        } else {
            $("#ratingText").text($(this).val());
        }
    });

    // show value of distance scroller
    $('#distance').on("change mousemove", function () {
        let dist = $(this).val();
        if (dist == "0") {
            $("#distanceText").text("Any");
        } else {
            $("#distanceText").text($(this).val() + "km");
        }
    });

    // back button of pages
    $("#backButton").click(function () {
        $("#backButton,#apartmentPage,#ordersPage").fadeOut(300);
        $("#searchBox,#filterBox,.mainPage").fadeIn(300);
        exitPage(true);
    });

    // checking if user logged
    if (isLogged()) {
        myUserId = parseInt(localStorage.getItem('userId'));
        showAfterLogin();
    } else {
        $(".beforeLogin").show();
    }

    // logout
    $("#logout").click(function () {
        $(".afterLogin").slideUp(200, function () {
            $(".beforeLogin").slideDown(200);
        });
        localStorage.removeItem("userId");
        localStorage.removeItem("username");
        localStorage.removeItem("email");
        myUserId = 0;
        $("#adminPanelButton").hide();
    });

    // order's check-out date event
    $("#toDate").change(function () {
        toDate = new Date($("#toDate").val());
        if ($("#toDate").val() == "") {
            $("#totalPrice").text("");
        } else {
            $("#totalPrice").text("(" + calculatePrice() + "$)");
        }

    });

    // order's check-in date event, allowing ordering only by given range of nights
    $("#fromDate").attr("min", new Date().toISOString().split('T')[0]);
    $("#fromDate").change(function () {
        fromDate = new Date($("#fromDate").val());
        $("#toDate").attr("min", fromDate.addDays(apartmentData.Minimum_nights).toISOString().split('T')[0]);
        $("#toDate").attr("max", fromDate.addDays(apartmentData.Maximum_nights).toISOString().split('T')[0]);
    });

    // order page open event
    $("#openMyOrders").click(function () {
        $("#searchBox,#filterBox,.mainPage,#apartmentPage").fadeOut(300);
        $("#backButton,#ordersPage").fadeIn(300);
        exitPage(false);
        if (tbl !== undefined) {
            tbl.destroy();
            tbl = undefined;
        }
        readMyOrders();
    });

});

// method for reseting variables and ui after exiting page
function exitPage(destroyTable) {
    initPagination(maxMainPages, 1);
    apartmentData = null;
    $("#fromDate").val("");
    $("#toDate").val("");
    $("#totalPrice").val("");
    $("#orderComplete").hide();
    fromDate = "";
    toDate = "";
    $("#totalPrice").text("");
    if (destroyTable) {
        $([document.documentElement, document.body]).animate({
            scrollTop: currentScreenLocation
        });
    }
}

function isMobile() {
    return ((window.screen.width < window.outerWidth ? window.screen.width : window.outerWidth) <= 1200);
}
// global error for ajax calls
// only for server corruptions, since cases like empty data is handled like success request
function readECB(data) {
    alert("ERROR");
    console.log(data);
}