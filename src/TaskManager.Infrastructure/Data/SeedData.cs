using TaskManager.Domain.Entities;
using TaskManager.Domain.Enums;
using TaskStatus = TaskManager.Domain.Enums.TaskStatus;

namespace TaskManager.Infrastructure.Data
{
    public static class SeedData
    {
        public static async Task Initialize(AppDbContext context)
        {
            // Skip if data already exists
            if (context.Users.Any())
                return;

            var users = new[]
            {
                new User(Guid.NewGuid(), "John Doe", "john.doe@example.com", UserRole.User),
                new User(Guid.NewGuid(), "Jane Smith", "jane.smith@example.com", UserRole.Manager),
                new User(Guid.NewGuid(), "Bob Johnson", "bob.johnson@example.com", UserRole.User),
                new User(Guid.NewGuid(), "Alice Brown", "alice.brown@example.com", UserRole.Admin)
            };

            await context.Users.AddRangeAsync(users);

            // Create projects for users
            var projects = new List<Project>();
            var random = new Random();

            foreach (var user in users)
            {
                var projectCount = random.Next(1, 4);

                for (int i = 1; i <= projectCount; i++)
                {
                    var project = new Project(
                        Guid.NewGuid(),
                        $"Project {i} - {user.Name}",
                        $"Description for project {i} of {user.Name}",
                        user.Id
                    );

                    projects.Add(project);

                    // Add tasks to projects - use FUTURE dates only
                    var taskCount = random.Next(3, 8);
                    for (int j = 1; j <= taskCount; j++)
                    {
                        // Use ONLY future dates for due dates
                        var dueDate = DateTime.UtcNow.AddDays(random.Next(1, 30));
                        var priority = (TaskPriority)random.Next(0, 3);
                        var status = (TaskStatus)random.Next(0, 3);

                        var task = project.AddTask(
                            $"Task {j} for Project {i}",
                            $"Description for task {j}",
                            dueDate, // SEMPRE data futura
                            priority
                        );

                        // Set task status
                        if (status != TaskStatus.Pending)
                        {
                            task.ChangeStatus(status, user.Id);
                        }

                        // Add some comments to tasks
                        if (random.Next(0, 2) == 1)
                        {
                            task.AddComment($"This is a comment on task {j}", user.Id);
                        }

                        // Add some history entries
                        if (random.Next(0, 2) == 1)
                        {
                            if (random.Next(0, 2) == 1)
                            {
                                task.UpdateTitle($"Updated Task {j} for Project {i}");
                            }

                            if (random.Next(0, 2) == 1 && status != TaskStatus.Completed)
                            {
                                task.ChangeStatus(TaskStatus.InProgress, user.Id);
                            }
                        }
                    }
                }
            }

            await context.Projects.AddRangeAsync(projects);
            await context.SaveChangesAsync();

            // Add some completed tasks for reporting - use PAST dates for completed tasks
            var completedTasksUser = users.First();
            var projectWithCompletedTasks = new Project(
                Guid.NewGuid(),
                "Completed Tasks Project",
                "Project with completed tasks for reporting",
                completedTasksUser.Id
            );

            for (int i = 1; i <= 10; i++)
            {
                // Para tarefas completadas, use datas passadas
                var dueDate = DateTime.UtcNow.AddDays(-random.Next(1, 15));

                var task = projectWithCompletedTasks.AddTask(
                    $"Completed Task {i}",
                    $"Description for completed task {i}",
                    dueDate, // Data passada para tarefas completadas
                    TaskPriority.Medium
                );

                // Mark as completed
                task.ChangeStatus(TaskStatus.Completed, completedTasksUser.Id);
            }

            context.Projects.Add(projectWithCompletedTasks);
            await context.SaveChangesAsync();
        }
    }
}