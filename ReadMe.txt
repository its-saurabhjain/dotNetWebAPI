WebAPI.Demo
https://app.pluralsight.com/player?author=scott-allen&name=aspdotnet-mvc5-fundamentals-m5-webapi2&mode=live&clip=1&course=aspdotnet-mvc5-fundamentals
1. How to use API Help (Microsoft.AspNet.WebApi.Help)
   - Change Build properties to output xml
   -uncomment HelpPageConfig.cs config.SetDocumentationProvider(new XmlDocumentationProvider(HttpContext.Current.Server.MapPath("~/App_Data/PatientDataAPI.xml")));
2. Add Package for Mongodb
Official MongoDB c# driver
3. Attribute routing (nested resources)
4. IHttpActionResult
5. CORS: Cross Origin Resource Sharing (Microsoft.AspNet.WebApi.Cors.5.2.3)
https://app.pluralsight.com/player?author=scott-allen&name=aspdotnet-mvc5-fundamentals-m5-webapi2&mode=live&clip=0&course=aspdotnet-mvc5-fundamentals
****Mongo command*********************8
mongod --config "c:\Program Files\MongoDB\mongo.config"

6. Demo for external google authentication, googleOauth.html
This demonstrate the OpenID connect & OAuth flows.
Using WebAPI framework to call external login url for the resource server, getting the access token and using that access token to call web apis
This is an authorization code flow is used by  browser javascript to get access token for authorization of web apis using external login.
The very first call to the WebAPI (Acting as Resoursce owner) generates a code that is exchanged with OP(openid provider) google for an access token.

Using Javascript client
Uisng Javascript to invoke google oauth server api to gain access token to  authorize access of google apis, this is an implicit flow that doesn't
require client secret access.
Using Javascript to gain a code from Resource server (Google), exchanging that code with auth/op server (google) to get ID token and access token,
using that access token to get access to Google apis.
The Id_token has claims, an application can use tokeninfo service to get the user claims and then can authenticate the user.(not implemented)

****************************Gotchas************************************************************* 
-for registering use Password123@, 
-if html Form doesn't have a filed with name attribute jquery .serialize() will not work


OAuth.Api
This application demos use of Identity Server and token for authentication
The Private Method of the home controller is having Authorize attribute, so it needs to have a authorization token,
once authorized this method call a WebAPIRouting API's Profile-->GetAsync method.