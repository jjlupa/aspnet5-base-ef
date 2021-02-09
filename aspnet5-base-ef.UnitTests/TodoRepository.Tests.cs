using System;
using System.Threading.Tasks;
using Xunit;
using aspnet5_base_ef.Models;
using Microsoft.EntityFrameworkCore;
using aspnet5_base_ef.Repositories;

namespace aspnet5_base_ef.UnitTests
{
    //
    // Repositories are our data sources of models, and generally wrap some abstraction such as a data context or httpclient
    // object. We don't need to test THOSE things themselves in unit, since we trust that they are thuroughly tested, however
    // we can test any behavior which we are building, and assumptions about behavior from those objects we are using.
    //
    // I still like the method walk approach to figuring out what we want to test, so I'll do that
    //
    // GetTodoItems() : Literally a passthrough, testing isn't interesting.
    // GetTodoItem() : nothing to test. Could make this a 1line function, but I don't like that cuz breakpoints.
    // CreateTodoItem() : This one is a little more interesting. SaveChangesAsync... what the hell does that do? Can it throw?
    //   yes it can!  Maybe we could emulate some throws to see what it does?  But no... because our code doesn't attempt to catch
    //   it kinda doesn't matter what it throws.  Now, I just thought of a refactor point!  We could make this class exception proof
    //   since we are all returning bools and nullables, which means we can eat all those DB exceptions instead of letting them leak
    //   to the front end on DBConcurrencyException or DBException.  That's a great idea, and we should do that.  But for now, nothing
    //   to do here, as soon as we try/catch, we test that. NOTE: FAILURE TO MOCK EXCEPTION THROW.
    // ChangeTodoItem() : What a coincidence! Some try/catch to test! Writing these tests makes you think about your try/catch more. Why
    //   are we letting other things escape? Should we have better handling? Future refactor points!
    // DeleteTodoItem() : Test return paths
    // TodoItemExists() : Normally private workers like this are TOTALLY good targets for testing, but this one is 1 line of linq.  We could
    //   test our assumptions about how linq works, but I'm for just reading the documentation currently. Testing this is more useful in integration
    //   tests, and that will happen on a happy path route, so Imma skipping.
    //
    public class TodoRepositoryTests
    {
        [Fact]
        public async Task ChangeTodoItem_SyncReturns_True()
        {
            // Arrange
            var item = new TodoItem()
            {
                Id = Guid.NewGuid(),
                Name = "Trailers for sale or rent",
                IsComplete = false,
                CreatedAt = DateTime.UtcNow,
                DeletedAt = null
            };

            var dbOptions = new DbContextOptionsBuilder<TodoContext>()
                     .UseInMemoryDatabase(Guid.NewGuid().ToString())
                     .Options;

            using (var context = new TodoContext(dbOptions))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
                context.TodoItems.Add(item);
                context.SaveChanges();

                var repo = new TodoItemsRepository(context);

                // Act
                bool result = await repo.ChangeTodoItem(item);

                // Assert
                Assert.True(result);
            }
        }

        [Fact(Skip = "Moqing the DBContext didn't work")]
        public void ChangeTodoItem_SyncThrowsItemIsGone_False()
        {
            //// Arrange
            //var item = new TodoItem()
            //{
            //    Id = Guid.NewGuid(),
            //    Name = "Rooms to let, fifty cents",
            //    IsComplete = false,
            //    CreatedAt = DateTime.UtcNow,
            //    DeletedAt = null
            //};
            //var items = new List<TodoItem>();
            //// items.Add(item); -- for this test, list is empty on purpose, simulating empty list
            //var context = new Mock<TodoContext>();
            //var dbSet = new Mock<DbSet<TodoItem>>();
            //context.Setup(x => x.TodoItems).Returns(() => dbSet.Object);
            //var queryable = items.AsQueryable();
            //dbSet.As<IQueryable<TodoItem>>().Setup(m => m.Provider).Returns(queryable.Provider);
            //dbSet.As<IQueryable<TodoItem>>().Setup(m => m.Expression).Returns(queryable.Expression);
            //dbSet.As<IQueryable<TodoItem>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            //dbSet.As<IQueryable<TodoItem>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());
            //context.Setup(x => x.SaveChangesAsync(default(CancellationToken)))
            //            .ThrowsAsync(new DbUpdateConcurrencyException());
            //// DELIBERATELY NOT ADDING ITEM TO REPO
            //var repo = new TodoItemsRepository(context.Object);
            //// Act
            //var result = await repo.ChangeTodoItem(item);
            //// Assert
            //Assert.False(result);
        }

        [Fact(Skip = "Moqing the DBContext didn't work")]
        public void ChangeTodoItem_SyncThrowsItemIsThere_RethrowDbUpdateConcurrencyException()
        {
            // Exception thrown: 'System.NotSupportedException' in Moq.dll

            //// Arrange
            //var item = new TodoItem()
            //{
            //    Id = Guid.NewGuid(),
            //    Name = "Rooms to let, fifty cents",
            //    IsComplete = false,
            //    CreatedAt = DateTime.UtcNow,
            //    DeletedAt = null
            //};
            //// Normally we fake ef repository in memory, but I need to mock it to force the throw
            //var dbOptions = new DbContextOptionsBuilder<TodoContext>()
            //         .UseInMemoryDatabase(Guid.NewGuid().ToString())
            //         .Options;
            //var mockContext = new Mock<TodoContext>(dbOptions);
            //mockContext.Setup(x => x.SaveChangesAsync(default(CancellationToken)))
            //            .ThrowsAsync(new DbUpdateConcurrencyException());
            //mockContext.Object.TodoItems.Add(item); // Gonna fake this in rather than mock
            //var repo = new TodoItemsRepository(mockContext.Object);
            //// Act
            //await Assert.ThrowsAsync<DbUpdateConcurrencyException>(() => repo.ChangeTodoItem(item));
            //// Assert
        }

        [Fact]
        public async Task DeleteTodoItem_NotFound_False()
        {
            // Arrange
            var item = new TodoItem()
            {
                Id = Guid.NewGuid(),
                Name = "Trailers for sale or rent",
                IsComplete = false,
                CreatedAt = DateTime.UtcNow,
                DeletedAt = null
            };

            var dbOptions = new DbContextOptionsBuilder<TodoContext>()
                     .UseInMemoryDatabase(Guid.NewGuid().ToString())
                     .Options;

            using (var context = new TodoContext(dbOptions))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
                // context.TodoItems.Add(item); DELIBERATELY HAVE NOTHING
                // context.SaveChanges();

                var repo = new TodoItemsRepository(context);

                // Act
                bool result = await repo.DeleteTodoItem(item.Id);

                // Assert
                Assert.False(result);
            }
        }

        [Fact]
        public async Task DeleteTodoItem_Found_True()
        {
            // Arrange
            var item = new TodoItem()
            {
                Id = Guid.NewGuid(),
                Name = "Trailers for sale or rent",
                IsComplete = false,
                CreatedAt = DateTime.UtcNow,
                DeletedAt = null
            };

            var dbOptions = new DbContextOptionsBuilder<TodoContext>()
                     .UseInMemoryDatabase(Guid.NewGuid().ToString())
                     .Options;

            using (var context = new TodoContext(dbOptions))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
                context.TodoItems.Add(item);
                context.SaveChanges();

                var repo = new TodoItemsRepository(context);

                // Act
                bool result = await repo.DeleteTodoItem(item.Id);

                // Assert
                Assert.True(result);
            }
        }
    }
}