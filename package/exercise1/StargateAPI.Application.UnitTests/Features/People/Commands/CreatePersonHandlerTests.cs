using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using StargateAPI.Application.Features.People.Commands;
using StargateAPI.Domain.Entities;
using System.Timers;
using Xunit;

namespace StargateAPI.Tests.Application.Features.People
{
    public class CreatePersonHandlerTests
    {
        [Fact]
        public async Task Handle_WhenDatabaseThrowsDuplicateKeyException_ShouldBubbleUpException()
        {
            var mockDbSet = new Mock<DbSet<Person>>();
            var mockContext = new Mock<StargateContext>(new DbContextOptionsBuilder<StargateContext>().Options);

            mockContext.Setup(c => c.People).Returns(mockDbSet.Object);

            mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()))
                       .ThrowsAsync(new DbUpdateException("UNIQUE constraint failed: Person.Name"));

            var handler = new CreatePersonHandler(mockContext.Object);

            var command = new CreatePerson { Name = "GEORGE PARKER" };
            var cancellationToken = CancellationToken.None;

            Func<Task> act = async () => await handler.Handle(command, cancellationToken);

            var exceptionAssertion = await act.Should().ThrowAsync<DbUpdateException>();

            exceptionAssertion.WithMessage("UNIQUE constraint failed: Person.Name");

            mockDbSet.Verify(
                dbSet => dbSet.AddAsync(It.IsAny<Person>(), It.IsAny<CancellationToken>()),
                Times.Once
            );
        }

        [Fact]
        public async Task Handle_WhenRequestIsNull_ShouldBubbleUpException()
        {
            var mockDbSet = new Mock<DbSet<Person>>();
            var mockContext = new Mock<StargateContext>(new DbContextOptionsBuilder<StargateContext>().Options);

            var handler = new CreatePersonHandler(mockContext.Object);

            Func<Task> act = async () => await handler.Handle(null, CancellationToken.None);

            var exceptionAssertion = await act.Should().ThrowAsync<ApplicationException>();

            exceptionAssertion.WithMessage("Bad Request");
        }
        [Fact]
        public async Task Handle_WhenRequestNameIsEmptyString_ShouldBubbleUpException()
        {
            var mockDbSet = new Mock<DbSet<Person>>();
            var mockContext = new Mock<StargateContext>(new DbContextOptionsBuilder<StargateContext>().Options);

            var handler = new CreatePersonHandler(mockContext.Object);
            var command = new CreatePerson { Name = String.Empty };
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            var exceptionAssertion = await act.Should().ThrowAsync<ApplicationException>();

            exceptionAssertion.WithMessage("Bad Request");
        }
        [Fact]
        public async Task Handle_WhenRequestNameIsNull_ShouldBubbleUpException()
        {
            var mockDbSet = new Mock<DbSet<Person>>();
            var mockContext = new Mock<StargateContext>(new DbContextOptionsBuilder<StargateContext>().Options);

            var handler = new CreatePersonHandler(mockContext.Object);
            var command = new CreatePerson { Name = null };
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            var exceptionAssertion = await act.Should().ThrowAsync<ApplicationException>();

            exceptionAssertion.WithMessage("Bad Request");
        }
    }
}