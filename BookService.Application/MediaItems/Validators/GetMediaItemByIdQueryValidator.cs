using BookService.Application.MediaItems.Queries;
using FluentValidation;

namespace BookService.Application.MediaItems.Validators
{
    public class GetMediaItemByIdQueryValidator : AbstractValidator<GetMediaItemByIdQuery>
    {
        public GetMediaItemByIdQueryValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0);
        }
    }
}