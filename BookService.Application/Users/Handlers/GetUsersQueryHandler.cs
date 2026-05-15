using BookService.Application.DTOs;
using BookService.Application.Users.Queries;
using Mapster;
using MediatR;

namespace BookService.Application.Users.Handlers
{
    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, List<UserResponseDTO>>
    {
        private readonly IUserRepository _repo;

        public GetUsersQueryHandler(IUserRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<UserResponseDTO>> Handle(GetUsersQuery request, CancellationToken ct)
        {
            var users = await _repo.GetAllAsync();

            return users.Adapt<List<UserResponseDTO>>();
        }
    }
}