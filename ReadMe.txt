PatientDataAPI
https://app.pluralsight.com/player?author=scott-allen&name=aspdotnet-mvc5-fundamentals-m5-webapi2&mode=live&clip=1&course=aspdotnet-mvc5-fundamentals
1. How to use API Help (Microsoft.AspNet.WebApi.Help)
   - Change Build properties to output xml
   -uncomment HelpPageConfig.cs config.SetDocumentationProvider(new XmlDocumentationProvider(HttpContext.Current.Server.MapPath("~/App_Data/PatientDataAPI.xml")));
2. Add Package for Mongodb
Official MongoDB c# driver
3. Attribute routing (nested resources)
4. IHttpActionResult
5. CORS: Cross Origin Resource Sharing (Microsoft.AspNet.WebApi.Cors.5.2.3)
****Mongo command*********************8
mongod --config "c:\Program Files\MongoDB\mongo.config"



****************************Gotchas************************************************************* 
-for registering use Password123@, 
-if html Form doesn't have a filed with name attribute jquery .serialize() will not work



OAuth.Demo
This application demos use of Identity Server and token for authentication
The Private Method of the home controller is having Authorize attribute, so it needs to have a authorization token,
once authorized this method call a WebAPIRouting API's Profile-->GetAsync method.