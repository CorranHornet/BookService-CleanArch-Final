using BookService.Application.DTOs;
using BookService.Application.Users.Queries;
using Mapster;
using MediatR;

namespace BookService.Application.Users.Handlers
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserResponseDTO?>
    {
        private readonly IUserRepository _repo;

        public GetUserByIdQueryHandler(IUserRepository repo)
        {
            _repo = repo;
        }

        public async Task<UserResponseDTO?> Handle(GetUserByIdQuery request, CancellationToken ct)
        {
            var user = await _repo.GetByIdAsync(request.Id);

            if (user == null) return null;

            return user.Adapt<UserResponseDTO>();
        }
    }
}