using FluentAssertions;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Enums;
using TaskManager.Domain.Events;
using Xunit;

namespace TaskManager.Domain.Tests.Entities
{
    public class UserTests
    {
        [Fact]
        public void Constructor_ShouldCreateUser_WithValidParameters()
        {
            // Arrange
            var id = Guid.NewGuid();
            var name = "John Doe";
            var email = "john@example.com";

            // Act
            var user = new User(id, name, email, UserRole.Manager);

            // Assert
            user.Id.Should().Be(id);
            user.Name.Should().Be(name);
            user.Email.Should().Be(email);
            user.Role.Should().Be(UserRole.Manager);
            user.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
            user.DomainEvents.Should().ContainSingle(e => e is UserCreatedEvent);
        }

        [Fact]
        public void UpdateProfile_ShouldUpdateNameAndEmail()
        {
            // Arrange
            var user = new User(Guid.NewGuid(), "Old Name", "old@example.com", UserRole.User);
            user.ClearDomainEvents();

            // Act
            user.UpdateProfile("New Name", "new@example.com");

            // Assert
            user.Name.Should().Be("New Name");
            user.Email.Should().Be("new@example.com");
            user.DomainEvents.Should().ContainSingle(e => e is UserProfileUpdatedEvent);
        }

        [Fact]
        public void ChangeRole_ShouldUpdateRole()
        {
            // Arrange
            var user = new User(Guid.NewGuid(), "John Doe", "john@example.com", UserRole.User);
            user.ClearDomainEvents();

            // Act
            user.ChangeRole(UserRole.Admin);

            // Assert
            user.Role.Should().Be(UserRole.Admin);
            user.DomainEvents.Should().ContainSingle(e => e is UserRoleChangedEvent);
        }
    }
}