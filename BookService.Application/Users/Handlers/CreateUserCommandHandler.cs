using BookService.Application.DTOs;
using BookService.Application.Users.Commands;
using BookService.Domain.Entities;
using MediatR;

namespace BookService.Application.Users.Handlers
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserResponseDTO>
    {
        private readonly IUserRepository _repo;

        public CreateUserCommandHandler(IUserRepository repo)
        {
            _repo = repo;
        }

        public async Task<UserResponseDTO> Handle(CreateUserCommand request, CancellationToken ct)
        {
            var user = new User
            {
                Username = request.Username,
                Email = request.Email,
                PasswordHash = "",
                Role = "User"
            };

            await _repo.AddAsync(user);
            await _repo.SaveAsync();

            return new UserResponseDTO
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email
            };
        }
    }
}