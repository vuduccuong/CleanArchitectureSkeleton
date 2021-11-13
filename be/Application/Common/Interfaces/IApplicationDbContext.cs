using Domain.Entities.UserManagements;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DatabaseFacade Database { get; }
        Task<IDbContextTransaction> BeginTransactionAsync();
        Task CommitAsync(IDbContextTransaction transaction);
        IDbContextTransaction GetCurrentTransaction();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        bool HasActiveTransaction { get; }

        DbSet<UserProfile> USR_UserProfiles { get; set; }
    }
}
