using BookService.Application.DTOs;
using BookService.Application.Users.Commands;
using BookService.Domain.Entities;
using Mapster;
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
            // Mapster creates the User entity. 
            // It runs the entity's defaults (Role = "User", PasswordHash = string.Empty)
            // and then copies Username and Email from the request.

            // 1. Mapster creates the entity and maps input data
            var user = request.Adapt<User>();
            
            await _repo.AddAsync(user);
            await _repo.SaveAsync();

            // 2. Mapster maps the tracking entity details back to the DTO
            // Map the tracking database ID, Username, and Email back to the presentation layer
            return user.Adapt<UserResponseDTO>();
        }
    }
}