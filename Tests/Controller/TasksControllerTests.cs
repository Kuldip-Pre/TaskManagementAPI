using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagementAPI.Controllers;
using TaskManagementAPI.Data;
using TaskManagementAPI.Dtos;
using TaskManagementAPI.Models;
using System.Threading.Tasks;


public class TasksControllerTests
{
    private AppDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;

        return new AppDbContext(options);
    }

    [Fact]
    public async Task CreateTask_ValidData_ReturnsCreatedResult()
    {
        var context = GetInMemoryDbContext();  // Get in-memory database context
        var user = new User { Id = 1, Username = "TestUser", Role = Convert.ToString(UserRole.User) };
        context.Users.Add(user);
        await context.SaveChangesAsync();

        var controller = new TasksController(context);

        var dto = new CreateTaskDto
        {
            Title = "Test Task",
            Description = "Test Description",
            UserId = user.Id
        };

      
        var result = await controller.CreateTask(dto);

       
        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        var taskDto = Assert.IsType<TaskDto>(createdResult.Value);
        Assert.Equal(dto.Title, taskDto.Title);
    }

    [Fact]
    public async Task CreateTask_UserNotFound_ReturnsNotFound()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var controller = new TasksController(context);

        var dto = new CreateTaskDto
        {
            Title = "Task without user",
            Description = "No user",
            UserId = 999 // Non-existent
        };

        // Act
        var result = await controller.CreateTask(dto);

        // Assert
        var notFound = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("User not found.", notFound.Value);
    }

    [Fact]
    public async Task CreateTask_WithNonExistentUser_ReturnsNotFound()
    {
      
        var context = GetInMemoryDbContext();  // Get in-memory database context
        var controller = new TasksController(context);

        // Create a DTO with a UserId that does not exist
        var dto = new CreateTaskDto
        {
            Title = "Task with Non-Existent User",
            Description = "This task should fail because the user doesn't exist",
            UserId = 999 // Non-existent user ID
        };

      
        var result = await controller.CreateTask(dto);

     
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("User not found.", notFoundResult.Value);
    }
    [Fact]
    public async Task CreateTask_WithInvalidData_ReturnsBadRequest()
    {
       
        var context = GetInMemoryDbContext();  // Get in-memory database context

       
        var user = new User { Id = 1, Username = "TestUser", Role = Convert.ToString(UserRole.User) };
        context.Users.Add(user);
        await context.SaveChangesAsync();

        var controller = new TasksController(context);

        // Create a DTO with missing Title (invalid data)
        var invalidDto = new CreateTaskDto
        {
            Title = "", 
            Description = "This task has no title.",
            UserId = 1  
        };

       
        var result = await controller.CreateTask(invalidDto);

      
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Contains("Title", badRequestResult.Value.ToString()); 
    }


}
