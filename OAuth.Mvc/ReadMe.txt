OAuth Flows
Redirection Flows (Implicit and Authorization_code flows)
1. A request for the resource is made by a web application by the user by clicking a link say, the user is redirected to the authorization server.
i.e http://OAuthserver/connect/authorize end point is called which redirects the user to the login page
http://OAuthserver/connect/authorize?client_id=socialnetwork_implicit&scope=openid profile&response_type=id_token token&redirect_uri=http://localhost:26654/&state=abc&nonce=xyz
2. The user is asked to enter the userid and password
3. A consent screen is displayed to the user
4. the user is redirected back to the web application and if aujthorized is able to view the screen.
