# aspnet5-base-ef

When spiking out random things, I find myself needing starting projects slightly more interesting than `dotnet new webapi`.  So I'll just make them and put them up here and I'll always know where they are. Great success. For background, I'm generally using vscode and sdk command line tools on a mac. Gitignore is the Visual Studio standard one.

# specifics

* https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-web-api?view=aspnetcore-5.0&tabs=visual-studio-code
* `dotnet new webapi -o aspnet5-base-ef` // cd aspnet5-base-ef
* `dotnet add package Microsoft.EntityFrameworkCore.InMemory`
* `dotnet add package Microsoft.EntityFrameworkCore.SqlServer`
* `dotnet dev-certs https --trust` // this is pretty redundand unless it's your first rodeo
* Create a Model
* Create a Context
* Register Context in Startup.cs
* Skip the crap about autoscaffolding as it won't work anyways
* Just Copy/Paste the stupid controller instead, we've already done the rest of the wiring.
* So far so good, you can ignore the rest of that tutorial since it just goes over postman which doesn't add material value over swagger, imo.

# Next Steps

* Logging
* Testing

