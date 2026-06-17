using BookService.Application.DTOs;
using BookService.Application.Interfaces;
using BookService.Application.Users.Commands;
using BookService.Domain.Entities;
using MapsterMapper;
using MediatR;

namespace BookService.Application.Users.Handlers
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserResponseDTO>
    {
        private readonly IUserRepository _repo;
        private readonly IMapper _mapper;

        public CreateUserCommandHandler(IUserRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<UserResponseDTO> Handle(CreateUserCommand request, CancellationToken ct)
        {
            // Map command to domain entity
            var user = _mapper.Map<User>(request);

            // Save
            await _repo.AddAsync(user);
            await _repo.SaveChangesAsync();

            // Map entity back to DTO
            return _mapper.Map<UserResponseDTO>(user);
        }
    }
}