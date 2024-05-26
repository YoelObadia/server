This is a server coded in C#. 

The serve is divided in 3 parts.

The first part is a restfull web API with ASP.NET 6. I did all the steps of the following site https://medium.com/net-core/build-a-restful-web-api-with-asp-net-core-6-30747197e229. 
I adapted for my project and I used Weapons rather than moovies.
However, the features are same and the use of the database too.
The controller WeaponController is like MOoovieController in the site. Just names changes.

The second part is the ImaggaController. Imagga is an Image Recognition Application.
I created an account to get an API key and API secret key to use the API of Imagga. 
Then, I need to share to Imagga an URL or an image to get results.

The third part is ChatGPTController. 
I send a prompt to chatgpt and i get the response from him.
The principle is the same, you have to create an account with OpenAI and depending on the subscription you can ue a model from OpenAI.

This is just the server part. The graphic part connected to this serevr is in the repository Graphic.
You can see there how to use him to use this server.
Before asking request from the graphic part, you have to run the server.

You can use Visual Studio to run the server.
