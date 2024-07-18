using AutoMapper;
using FormsBackendCommon.Interface;
using FormsBackendCommon.Model;
using MediatR;

namespace FormsBackendBusiness.Users.Queries.GetUsers;

public class GetUsersQueryHandler(IGenericRepository<UserModel> userRepository, IMapper mapper)
    : IRequestHandler<GetUsersQuery, GetUsersQueryResult>
{
    public async Task<GetUsersQueryResult> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        return new GetUsersQueryResult() { Users = mapper.Map<List<UserGet>>(await userRepository.GetAsync()) };
    }
}
