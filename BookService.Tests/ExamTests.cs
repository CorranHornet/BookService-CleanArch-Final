using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using MediatR;
using BookService.Application.MediaUnits.Commands;
using BookService.Domain.Entities;
using BookService.Infrastructure.Persistence;

namespace BookService.Tests
{

    public class ExamTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly IMediator _mediator;
        private readonly ApplicationDbContext _db;

        public ExamTests(WebApplicationFactory<Program> factory)
        {
            var scope = factory.Services.CreateScope();

            _mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            _db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        }

        [Fact]
        public async Task System_Should_Handle_Full_CQRS_Flow()
        {
            // ARRANGE
            var genre = new Genre { Name = "Exam Genre" };
            _db.Genres.Add(genre);
            await _db.SaveChangesAsync();

            var mediaItem = new MediaItem
            {
                Title = "Exam Item",
                GenreId = genre.Id
            };

            _db.MediaItems.Add(mediaItem);
            await _db.SaveChangesAsync();

            // ACT
            var result = await _mediator.Send(new CreateMediaUnitCommand
            {
                Title = "Exam Book",
                MediaItemId = mediaItem.Id,
                PageCount = 250
            });

            Assert.NotNull(result);
            Assert.Equal("Book", result.UnitType);
            Assert.Equal(250, result.PageCount);

            var dbEntity = await _db.MediaUnits.FirstOrDefaultAsync(x => x.Id == result.Id);
            Assert.NotNull(dbEntity);
        }
    }
}
            