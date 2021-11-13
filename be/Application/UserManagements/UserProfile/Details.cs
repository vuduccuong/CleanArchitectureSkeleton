using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Common.Security;
using Application.UserManagements.UserProfile.Dtos;
using Application.UserManagements.UserProfile.Mapper;
using AutoMapper.QueryableExtensions;
using Domain.Enums.Users;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UserManagements.UserProfile
{
    [Authorize(ERole.Leader, ERole.SuperAdmin)]
    public class Details : IRequest<ResultBase<DetailsDto>>
    {
        public string UserId { get; set; }
    }

    public class DetailHandler : IRequestHandler<Details, ResultBase<DetailsDto>>
    {
        private readonly IIdentityService _identityService;
        private readonly IApplicationDbContext _context;

        public DetailHandler(IIdentityService identityService, IApplicationDbContext context)
        {
            _identityService = identityService;
            _context = context;
        }

        public async Task<ResultBase<DetailsDto>> Handle(Details request, CancellationToken cancellationToken)
        {
            var mapper = new UserDetailMapper();
            var userProfile = await _context.USR_UserProfiles.Where(u => u.UserId == request.UserId)
                .Include(up => up.ApplicationUser)
                .ProjectTo<DetailsDto>(mapper.Config).FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);


            if (userProfile != null)
            {
                var roles = await _identityService.GetRoleAsync(request.UserId).ConfigureAwait(false);
                userProfile.RolePermission = roles;
            }

            return ResultBase<DetailsDto>.Success(userProfile);
        }
    }
}
