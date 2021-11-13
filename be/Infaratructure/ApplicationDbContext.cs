using Application.Common.Interfaces;
using Domain.Entities.UserManagements;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using System;
using System.Data;
using System.Reflection;
using System.Threading.Tasks;

namespace Infaratructure
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
    {
        private readonly ILogger<ApplicationDbContext> _logger;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ILogger<ApplicationDbContext> logger)
            : base(options)
        {
            _logger = logger;
        }

        #region DbSet

        public DbSet<UserProfile> USR_UserProfiles { get; set; }

        #endregion DbSet

        #region Transaction
        private IDbContextTransaction _currentTransaction;
        public bool HasActiveTransaction => _currentTransaction != null;

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return _currentTransaction ??= await Database.BeginTransactionAsync(IsolationLevel.ReadCommitted).ConfigureAwait(false);
        }

        public async Task CommitAsync(IDbContextTransaction transaction)
        {
            ValidateTransaction(transaction);
            try
            {
                await SaveChangesAsync().ConfigureAwait(false);
                transaction.Commit();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Commit Transaction {transaction.TransactionId} will be rollback. Err: {ex.Message}");
                await RollBackTransactionAsync().ConfigureAwait(false);
                throw;
            }
            finally
            {
                await DisposeTransactionAsync().ConfigureAwait(false);
            }
        }

        private async Task RollBackTransactionAsync()
        {
            try
            {
                await _currentTransaction.RollbackAsync().ConfigureAwait(false);
            }
            finally
            {
                await DisposeTransactionAsync().ConfigureAwait(false);
            }
        }

        private async Task DisposeTransactionAsync()
        {
            if (_currentTransaction != null)
            {
                await _currentTransaction.DisposeAsync().ConfigureAwait(false);
                _currentTransaction = null;
            }
        }

        private void ValidateTransaction(IDbContextTransaction transaction)
        {
            if (transaction == null)
                throw new ArgumentNullException(nameof(transaction));

            if (_currentTransaction != transaction)
                throw new InvalidOperationException($"Transaction {transaction.TransactionId} invalid");
        }

        public IDbContextTransaction GetCurrentTransaction() => _currentTransaction;
        #endregion

        #region Model Create
        protected override void OnModelCreating(ModelBuilder builder)
        {
            Assembly assem = typeof(ApplicationDbContext).Assembly;

            builder.ApplyConfigurationsFromAssembly(assem);
            builder.Entity<UserProfile>()
                .HasKey("UserId")
                .HasName("UserId");

            builder.Entity<ApplicationUser>()
                .HasOne<UserProfile>(u => u.UserProfile)
                .WithOne(up => up.ApplicationUser)
                .HasForeignKey<UserProfile>(k=>k.UserId);

            base.OnModelCreating(builder);
        }
        #endregion
    }
}
