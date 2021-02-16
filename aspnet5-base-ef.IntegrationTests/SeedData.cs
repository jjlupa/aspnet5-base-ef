using System;
using aspnet5_base_ef.Models;

namespace aspnet5_base_ef.IntegrationTests
{
    //
    // Generic class that seeds some data in a database.
    //
    public static class SeedData
    {
        public static void PopulateTestData(TodoContext dbContext)
        {
            dbContext.TodoItems.Add(
                new TodoItem()
                {
                    Name = "Chicken in the henhouse pick'n at dough",
                    IsComplete = false,
                    CreatedAt = DateTime.UtcNow
                }
            );

            dbContext.TodoItems.Add(
                new TodoItem()
                {
                    Name = "Babe you hurt me, and you know I hurt you too",
                    IsComplete = false,
                    CreatedAt = DateTime.UtcNow
                }
            );

            dbContext.SaveChanges();
        }
    }
}
