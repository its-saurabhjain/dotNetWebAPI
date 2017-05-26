var googleUrl = "";
var local_access_token = '';
var access_token = '';
var id_token = '';
var local_code = '';
    //1. get the google url
    var loginListFunc = function () {
        var externalloginUrl = "/api/AccountAPI/ExternalLogins?returnUrl=%2F&generateState=true";
        $.get(externalloginUrl)
            .success(saveUrl)
            .always(showResponseFunc);
        return false;
    };
    var showResponseFunc = function (object) {
        $("#output").text(JSON.stringify(object, null, 4));
    };
    //this will also have the redirect url once authenticated
    var saveUrl = function (data) {
        var localgoogleUrl = data[0].Url
        var parameters = localgoogleUrl.split('&');
        //changing the url redirection to default page, also Providers-->ApplicationOAuthProvider.cs needs to be changed
        var redirection = "redirect_uri=http%3A%2F%2Flocalhost%3A7825%2Fdefault.html";
        googleUrl = parameters[0] + '&' + parameters[1] + '&' + parameters[2] + '&' + redirection + '&' + parameters[4];
    };
    // authenticate the user by redirecting to google sigin screen
    var googleAuthFunc = function () {
        //showResponseFunc(googleUrl);
        window.location.href = googleUrl;
    };
    //get the access token 
    var getAccessTokenFunc = function () {
        if (location.hash) {
            if (location.hash.split("access_token=")) {
                local_access_token = location.hash.split("access_token=")[1].split('&')[0];
            }
        }
        return false;
    };
    var getIdTokenFunc = function () {
        var href = window.location.href;
        var local_code = href.split("code=")[1].split('&')[0];
        $.ajax('https://www.googleapis.com/oauth2/v4/token',
            {
                type: "POST",
                ContentType: 'application/json',
                dataType: 'json',
                data: {
                    "code": local_code,
                    "client_id": 'To Be added',
                    "client_secret": 'To Be added',
                    "redirect_uri": 'http://localhost:7825/default.html',
                    "grant_type": "authorization_code"
                }
            }).success(saveIdToken)
            .always(showResponseFunc);
    };

    var validateToken = function () {
        var href = window.location.href;
        if (href.indexOf('access_token') >= 0) {
            var local_code = href.split("access_token=")[1].split('&')[0];
            $.get("https://www.googleapis.com/oauth2/v3/tokeninfo?access_token=" + local_code)
                .success(saveIdToken(local_code))
                .always(showResponseFunc);
        }
        if (id_token != '') {
            $.get("https://www.googleapis.com/oauth2/v3/tokeninfo?id_token=" + id_token)
                .always(showResponseFunc);
        }
    };
    var saveIdToken = function (data) {
        //save id_token and access token once code is exchanged for token
        var str = JSON.stringify(data);
        if (str.indexOf('access_token') !== -1) {
            id_token = data['id_token'];
            access_token = data['access_token'];
            local_access_token = access_token;
        }
        else {
            //save access token from google oAuth
            local_access_token = data;
        };
    };
    var getUserInfoFunc = function () {
        $.ajax('/api/accountapi/userinfo', {
            type: "GET",
            headers: getHeaders()
        }).always(showResponseFunc);
        return false;
    };
    var getHeaders = function () {
        getAccessTokenFunc();
        if (local_access_token) {
            return { "Authorization": "Bearer " + local_access_token };
        }
    };
    var registerExternalUserFunc = function () {
        var regUrl = "api/accountAPI/RegisterExternal";
        var data = $("#login").serialize();
        //showResponseFunc(data);
        //$.post(regUrl, data).always(showResponseFunc);

        $.ajax('api/accountAPI/RegisterExternal', {
            type: "POST",
            headers: getHeaders(),
            data: data
        }).always(showResponseFunc);
        return false;
    };
    var clearTokenFunc = function () {
        local_access_token = '';
        //location.replace('http://localhost:7825/default.html');
        window.location.hash = '';
        window.location.reload();
    };
    // Access to Google API
    // Get code, then get ID_Token use access_token
    var getGoogleDriveAPI = function () {
        $.get('https://www.googleapis.com/drive/v2/files?access_token=' + local_access_token)
            .always(showResponseFunc);
    };

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    /*
    * Create form to request access token from Google's OAuth 2.0 server.
    * Refer https://developers.google.com/identity/protocols/OpenIDConnect
    */
    function oauthAccessToken() {
        // Google's OAuth 2.0 endpoint for requesting an access token
        var oauth2Endpoint = 'https://accounts.google.com/o/oauth2/v2/auth';

        //var oauth2Endpoint = 'https://accounts.google.com/signin/oauth/oauthchooseaccount';
        var url2 = oauth2Endpoint + '?' +
            "client_id=384570901380-qo9736vnefaj1s0oai3daf82h348c2fg.apps.googleusercontent.com&" +
            "redirect_uri=http%3A%2F%2Flocalhost%3A7825%2Fdefault.html&" +
            "destination=http%3A%2F%2Flocalhost%3A7825%2Fdefault.html&" +
            "response_type=token&" +     //receive a access_token
            "scope=openid%20email&" +
            "include_granted_scopes=true&" +
            "prompt=select_account&" +   //none or select_account
            "state='pass-through value'";
        window.location.href = url2;
        return false;
    };

    function oauthIdTokenCode() {
        // Google's OAuth 2.0 endpoint for requesting an access token
        var oauth2Endpoint = 'https://accounts.google.com/o/oauth2/v2/auth';
        var url2 = oauth2Endpoint + '?' +
            "client_id=384570901380-qo9736vnefaj1s0oai3daf82h348c2fg.apps.googleusercontent.com&" +
            "redirect_uri=http%3A%2F%2Flocalhost%3A7825%2Fdefault.html&" +
            "response_type=code&" +     //receive a code id_token & access token
            //"response_type=client_credentials&" +
            "scope=openid%20email%20profile&" +
            "include_granted_scopes=true&" +
            "prompt=select_account&" +    //none or select_account
            "access_type=offline&" +
            "state='pass-through value'";
        window.location.href = url2;
        return false;
    };

    function getProductsFunc() {
        //alert(local_access_token);
        $.ajax('/api/products',
            {
                type: "GET",
                headers: getHeaders()
            }).always(showResponseFunc);
        return false;
    };


