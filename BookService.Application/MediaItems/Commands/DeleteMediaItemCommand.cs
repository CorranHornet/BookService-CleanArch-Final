using MediatR;

namespace BookService.Application.MediaItems.Commands
{
    public class DeleteMediaItemCommand : IRequest<bool>
    {
        public int Id { get; set; }

        public DeleteMediaItemCommand(int id)
        {
            Id = id;
        }
    }
}