using BookService.Application.MediaItems.Commands;
using FluentValidation;

namespace BookService.Application.MediaItems.Validators
{
    public class DeleteMediaItemCommandValidator : AbstractValidator<DeleteMediaItemCommand>
    {
        public DeleteMediaItemCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0);
        }
    }
}