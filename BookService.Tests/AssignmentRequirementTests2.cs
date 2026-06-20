using MediatR;
using Mapster;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.Testing;
using BookService.Application.DTOs;
using BookService.Domain.Entities;
using BookService.Application.Interfaces;

namespace BookService.Tests
{
    public class AssignmentRequirementTests2
    {
        private readonly WebApplicationFactory<Program> _factory;

        public AssignmentRequirementTests2()
        {
            _factory = new WebApplicationFactory<Program>();
        }

        // ------------------------------------------------------------
        // 1. MediatR must exist
        // ------------------------------------------------------------
        [Fact]
        public void MediatR_ShouldBeRegistered()
        {
            using var scope = _factory.Services.CreateScope();

            var mediator = scope.ServiceProvider.GetService<IMediator>();

            Assert.NotNull(mediator);
        }

        // ------------------------------------------------------------
        // 2. Command handlers must exist
        // ------------------------------------------------------------
        [Fact]
        public void CommandHandlers_ShouldExist()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            var handlerExists = assemblies
                .SelectMany(a => a.GetTypes())
                .Any(t => t.Name.Contains("Handler"));

            Assert.True(handlerExists, "No CQRS handlers found");
        }

        // ------------------------------------------------------------
        // 3. Query objects must exist
        // ------------------------------------------------------------
        [Fact]
        public void Queries_ShouldExist()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            var queryExists = assemblies
                .SelectMany(a => a.GetTypes())
                .Any(t => t.Name.EndsWith("Query"));

            Assert.True(queryExists, "No Query classes found");
        }

        // ------------------------------------------------------------
        // 4. Repository interfaces must exist
        // ------------------------------------------------------------
        [Fact]
        public void RepositoryInterfaces_ShouldExist()
        {
            var appAssembly = typeof(IMediaItemRepository).Assembly;

            var exists = appAssembly.GetTypes()
                .Any(t =>
                    t.IsInterface &&
                    t.Name.EndsWith("Repository"));

            Assert.True(exists, "Repository interfaces missing");
        }

        // ------------------------------------------------------------
        // 5. Repository must be registered in DI
        // ------------------------------------------------------------
        [Fact]
        public void Repositories_ShouldBeRegistered_InDI()
        {
            // IMPORTANT:
            // Repositories are SCOPED services.
            // Therefore CreateScope() is REQUIRED.

            using var scope = _factory.Services.CreateScope();

            var provider = scope.ServiceProvider;

            var mediaRepo = provider.GetService<IMediaItemRepository>();

            Assert.NotNull(mediaRepo);
        }

        // ------------------------------------------------------------
        // 6. Mapster must map Domain -> DTO
        // ------------------------------------------------------------
        [Fact]
        public void Mapster_ShouldMap_DomainToDTO()
        {
            var book = new PhysicalBookUnit
            {
                Title = "Test Book",
                PageCount = 123,
                MediaItemId = 1
            };

            var dto = book.Adapt<MediaUnitDTO>();

            Assert.NotNull(dto);
            Assert.Equal("Test Book", dto.Title);
            Assert.Equal(123, dto.PageCount);
        }

        // ------------------------------------------------------------
        // 7. Inheritance must exist
        // ------------------------------------------------------------
        [Fact]
        public void Domain_ShouldSupportInheritance()
        {
            var book = new PhysicalBookUnit();
            var audio = new AudiobookUnit();

            Assert.IsAssignableFrom<MediaUnit>(book);
            Assert.IsAssignableFrom<MediaUnit>(audio);
        }
    }
}