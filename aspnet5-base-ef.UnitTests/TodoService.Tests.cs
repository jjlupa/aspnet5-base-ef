using System;
using Xunit;
using aspnet5_base_ef.DTOs;
using aspnet5_base_ef.Models;
using AutoMapper;
using aspnet5_base_ef.Profiles;

namespace aspnet5_base_ef.UnitTests
{
    //
    // OK, it's worth taking some time and thinking about what is getting tested in the layered service.  Since we are separating
    // out repository, that makes the service layer largely a mapping layer between model and DTO. This is where we should be validating
    // that all hidden data is correctly used.
    //
    // Again, a good way is to inspect the methods and think about what we would want to test. As you look through this, you can
    // see that really the only thing we could test would be Automapper behavior.  So, there IS business logic in automapper, but
    // it's encapsulated in the Profile class, which isn't helpful to us.
    //
    // Why don't we test the service get and create, and inspect the DTO and models to make sure we are getting the transforms we expect.
    //
    // In a normal service layer there would be plenty of real business logic to test here, but in this case not so much. Notice that
    // I'm not validating data correctness on model mapping here, I'm doing that in the repository instead, so more tests there instead of here.
    // I could hear and argument that we would want to do that here.
    //
    public class TodoServiceTests
    {
        [Fact]
        public void AutoMapper_DTOtoModel_ForwardsWhatWeWant()
        {
            // Arrange
            TodoItemDTOv1 item = new TodoItemDTOv1()
            {
                Id = Guid.NewGuid(),
                Name = "Don't call it a comeback",
                IsComplete = false
            };

            var config = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new TodoProfile());
            });

            var mapper = config.CreateMapper();

            // Act
            var model = mapper.Map<TodoItem>(item);

            // Assert
            Assert.Equal(model.Id, item.Id);
            Assert.Equal(model.Name, item.Name);
            Assert.Equal(model.IsComplete, item.IsComplete);
            Assert.Equal(model.CreatedAt, new DateTime(0));

            Assert.Null(model.DeletedAt);
        }

        [Fact]
        public void AutoMapper_ModeltoDTO_ForwardsWhatWeWant()
        {
            // Arrange
            TodoItem item = new TodoItem()
            {
                Id = Guid.NewGuid(),
                Name = "Don't call it a comeback",
                IsComplete = false,
                CreatedAt = DateTime.UtcNow,
                DeletedAt = DateTime.UtcNow
            };

            var config = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new TodoProfile());
            });

            var mapper = config.CreateMapper();

            // Act
            var dto = mapper.Map<TodoItemDTOv1>(item);

            // Assert
            Assert.Equal(dto.Id, item.Id);
            Assert.Equal(dto.Name, item.Name);
            Assert.Equal(dto.IsComplete, item.IsComplete);
        }
    }
}