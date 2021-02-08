using System;
using System.Threading.Tasks;
using Moq;
using Xunit;
using aspnet5_base_ef.Controllers;
using aspnet5_base_ef.Services;
using Microsoft.AspNetCore.Mvc;
using aspnet5_base_ef.DTOs;

namespace aspnet5_base_ef.UnitTests
{
    // Let's think out our test plan here... reviewing all methods of the TodoController, lets look for testable logic...
    //
    // GetTodoItems() : This is a straight passthrough. We could test the service, or we could test the aspnet response, but neither add value and are redundant with other tests.
    // GetTodoItem() : We can validate that null service finds turn into a NotFound.
    // PutTodoItem() : A little input validation happens, we can test that. Output differentiation, we can test that.
    // PostTodoItem() : We can test return differentiation.
    // DeleteTodoItem() : We can test return differentiation.
    //
    // We'll use the UnitOfWork_StateUnderTest_ExpectedBehavior naming convention

    public class TodoControllerTests
    {
        [Fact]
        public async Task GetTodoItem_Null_ReturnsNotFound()
        {
            // Arrange
            var mockService = new Mock<ITodoItemsService>(); //literally doesn't matter for this
            var controller = new TodoItemsController(mockService.Object);

            Guid id = Guid.NewGuid();

            // Act
            var result = await controller.GetTodoItem(id);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetTodoItem_NotNull_Returns201()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            TodoItemDTOv1 todoItemDTOv1 = new TodoItemDTOv1()
            {
                Id = id,
                Name = "It's a fake TODO Brant",
                IsComplete = false
            };

            var mockService = new Mock<ITodoItemsService>();
            mockService.Setup(repo => repo.GetTodoItem(id))
                .ReturnsAsync(todoItemDTOv1);

            var controller = new TodoItemsController(mockService.Object);

            // Act
            var result = await controller.GetTodoItem(id);

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public async Task PutTodoItem_BadData_ReturnsBadRequest()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            TodoItemDTOv1 todoItemDTOv1 = new TodoItemDTOv1()
            {
                Id = Guid.NewGuid(),
                Name = "It's a fake TODO Brant",
                IsComplete = false
            };

            var mockService = new Mock<ITodoItemsService>();
            var controller = new TodoItemsController(mockService.Object);

            // Act
            var result = await controller.PutTodoItem(id, todoItemDTOv1);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task PutTodoItem_Normal_ReturnsSuccess()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            TodoItemDTOv1 item = new TodoItemDTOv1()
            {
                Id = id,
                Name = "It's a fake TODO Brant",
                IsComplete = false
            };

            var mockService = new Mock<ITodoItemsService>();
            mockService.Setup(repo => repo.ChangeTodoItem(item))
                .ReturnsAsync(true);

            var controller = new TodoItemsController(mockService.Object);

            // Act
            var result = await controller.PutTodoItem(id, item);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        // I literally caught a bug in the code where I forgot an await due to writing this test
        [Fact]
        public async Task PutTodoItem_BadGuid_ReturnsNotFound()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            TodoItemDTOv1 item = new TodoItemDTOv1()
            {
                Id = id,
                Name = "It's a fake TODO Brant",
                IsComplete = false
            };

            var mockService = new Mock<ITodoItemsService>();
            mockService.Setup(repo => repo.ChangeTodoItem(item))
                .ReturnsAsync(false);
            mockService.Setup(repo => repo.GetTodoItem(id))
                .ReturnsAsync((TodoItemDTOv1)null);

            var controller = new TodoItemsController(mockService.Object);

            // Act
            var result = await controller.PutTodoItem(id, item);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        // Just because I don't think this can happen doesn't mean I shouldn't put in some validation code.
        [Fact]
        public async Task PutTodoItem_GoodGuidBadCommit_ReturnsBadRequest()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            TodoItemDTOv1 item = new TodoItemDTOv1()
            {
                Id = id,
                Name = "It's a fake TODO Brant",
                IsComplete = false
            };

            var mockService = new Mock<ITodoItemsService>();
            mockService.Setup(repo => repo.ChangeTodoItem(item))
                .ReturnsAsync(false);
            mockService.Setup(repo => repo.GetTodoItem(id))
                .ReturnsAsync(item);

            var controller = new TodoItemsController(mockService.Object);

            // Act
            var result = await controller.PutTodoItem(id, item);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task PostTodoItem_Normal_ReturnsSuccess()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            TodoItemDTOv1 item = new TodoItemDTOv1()
            {
                Id = id,
                Name = "It's a fake TODO Brant",
                IsComplete = false
            };

            var mockService = new Mock<ITodoItemsService>();
            mockService.Setup(repo => repo.CreateTodoItem(item))
                .ReturnsAsync(item);

            var controller = new TodoItemsController(mockService.Object);

            // Act
            var result = await controller.PostTodoItem(item);

            // Assert
            Assert.IsType<CreatedAtActionResult>(result.Result);
        }

        [Fact]
        public async Task PostTodoItem_CreateFailure_ReturnsBadRequest()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            TodoItemDTOv1 item = new TodoItemDTOv1()
            {
                Id = id,
                Name = "It's a fake TODO Brant",
                IsComplete = false
            };

            var mockService = new Mock<ITodoItemsService>();
            mockService.Setup(repo => repo.CreateTodoItem(item))
                .ReturnsAsync((TodoItemDTOv1)null);

            var controller = new TodoItemsController(mockService.Object);

            // Act
            var result = await controller.PostTodoItem(item);

            // Assert
            Assert.IsType<BadRequestResult>(result.Result);
        }

        [Fact]
        public async Task DeleteTodoItem_Found_ReturnsNoContent()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            TodoItemDTOv1 item = new TodoItemDTOv1()
            {
                Id = id,
                Name = "It's a fake TODO Brant",
                IsComplete = false
            };

            var mockService = new Mock<ITodoItemsService>();
            mockService.Setup(repo => repo.DeleteTodoItem(id))
                .ReturnsAsync(true);
            mockService.Setup(repo => repo.GetTodoItem(id))
                .ReturnsAsync(item);

            var controller = new TodoItemsController(mockService.Object);

            // Act
            var result = await controller.DeleteTodoItem(id);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteTodoItem_NotFound_ReturnsNotFound()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            TodoItemDTOv1 item = new TodoItemDTOv1()
            {
                Id = id,
                Name = "It's a fake TODO Brant",
                IsComplete = false
            };

            var mockService = new Mock<ITodoItemsService>();
            mockService.Setup(repo => repo.DeleteTodoItem(id))
                .ReturnsAsync(true);
            mockService.Setup(repo => repo.GetTodoItem(id))
                .ReturnsAsync((TodoItemDTOv1)null);

            var controller = new TodoItemsController(mockService.Object);

            // Act
            var result = await controller.DeleteTodoItem(id);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

    }
}
