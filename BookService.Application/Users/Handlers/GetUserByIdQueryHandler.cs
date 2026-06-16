using BookService.Application.DTOs;
using BookService.Application.Users.Queries;
using MapsterMapper;
using MediatR;

namespace BookService.Application.Users.Handlers
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserResponseDTO?>
    {
        private readonly IUserRepository _repo;
        private readonly IMapper _mapper;

        public GetUserByIdQueryHandler(IUserRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<UserResponseDTO?> Handle(GetUserByIdQuery request, CancellationToken ct)
        {
            var user = await _repo.GetByIdAsync(request.Id);

            if (user == null) return null;

            return _mapper.Map<UserResponseDTO>(user);
        }
    }
}