using BookService.Application.Interfaces;
using BookService.Application.Users.Commands;
using MapsterMapper;
using MediatR;

namespace BookService.Application.Users.Handlers
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, bool>
    {
        private readonly IUserRepository _repo;
        private readonly IMapper _mapper;

        public DeleteUserCommandHandler(IUserRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<bool> Handle(DeleteUserCommand request, CancellationToken ct)
        {
            var user = await _repo.GetByIdAsync(request.Id);

            if (user == null)
                return false;

            if (await _repo.HasActiveLoans(request.Id))
                throw new Exception("Cannot delete user with active loans.");

            await _repo.DeleteAsync(user);
            await _repo.SaveChangesAsync();

            return true;
        }
    }
}