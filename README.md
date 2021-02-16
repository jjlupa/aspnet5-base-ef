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

# Unit Tests

OK, let's talk about repository stack, or layered architecture. Breaking up your layers helps to drive unit tests. There are acouple
ways to go about this, but I'm going to go full repository pattern, so each controller would be backed by a service, and aservice would
be backed by one or more repositories.  You could push multiple services against a controller for hybrid behaviors, but I generally
feel that's a bit much to start with.  Anyways, we'll just go with that and see how it feels.  For this to work, you really want your unit tests to map to business logic, and not framework code.  Don't shoot for 100% coverage, shoot to cover what you need.

* Delete Weather controller/model.
* Add CreatedAt, DeletedAt to the ToDo item model, change Id to Guid, because we aren't monsters.
    * We are adding soft deletes as an idea because it allows us to add business logic to get/put/post which we can unit test.
* Annotate the model with the appropriate EF stuff.
* Add Repositories, Services, and DTOs folders, transform the controller methods into call chains through the layers.
* Add a DTO that maps the model, without the CreatedAt, so we can show differentiated DTO and entities.
* Add Automapper
    * `dotnet add package AutoMapper.Extensions.Microsoft.DependencyInjection`
    * Add Automapper Startup in Startup.cs
    * Add Profiles folder
    * Add TodoItemProfile.cs to Profiles folder.
* Add Serivce and Repository to DI in Startup.cs
* Unwind Controller code down to the repository.
* Dress up the swagger, make sure to fix the XML in the csproj
    * https://docs.microsoft.com/en-us/aspnet/core/tutorials/getting-started-with-swashbuckle?view=aspnetcore-5.0&tabs=visual-studio-code
* Unit Tests!
    * `dotnet new xunit -o aspnet5-base-ef.UnitTests`
    * `dotnet add ./aspnet5-base-ef.UnitTests/aspnet5-base-ef.UnitTests.csproj reference ./aspnet5-base-ef/aspnet5-base-ef.csproj `
    * cd unit test dir
    * `dotnet add package Microsoft.AspNetCore.Mvc.Testing`
    * About this time I added an SLN because VS is better than Code at handling tests imo. https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-sln if you live and die by command line.
    * Create TodoController.Tests.cs read comments in that file for detail
    * Create TodoService.Tests.cs read comments in that file for detail
    * Create TodoRepository.Tests.cs read comments in that file for detail
* Integration Tests
    * `dotnet new xunit -o aspnet5-base-ef.IntegrationTests`
    * `dotnet sln add aspnet5-base-ef.IntegrationTests/aspnet5-base-ef.IntegrationTests.csproj`
* Serilog

```dotnet add package Serilog
dotnet add package Serilog.AspNetCore
dotnet add package Serilog.Enrichers.Environment
dotnet add package Serilog.Exceptions
dotnet add package Serilog.Extensions.Logging
dotnet add package Serilog.Settings.Configuration
dotnet add package Serilog.Sinks.Async
dotnet add package Serilog.Sinks.Console```

