using BookService.Application.DTOs;
using BookService.Application.Users.Queries;
using MapsterMapper;
using MediatR;

namespace BookService.Application.Users.Handlers
{
    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, List<UserResponseDTO>>
    {
        private readonly IUserRepository _repo;
        private readonly IMapper _mapper;

        public GetUsersQueryHandler(IUserRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<List<UserResponseDTO>> Handle(GetUsersQuery request, CancellationToken ct)
        {
            var users = await _repo.GetAllAsync();

            return _mapper.Map<List<UserResponseDTO>>(users);
        }
    }
}