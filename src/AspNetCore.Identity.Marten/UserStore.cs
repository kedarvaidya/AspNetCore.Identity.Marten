using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AspNetCore.Identity.Marten.Internal;
using Marten;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;

namespace AspNetCore.Identity.Marten
{
    public class UserStore<TUser, TKey>
        : IUserStore<TUser>
        , IUserPasswordStore<TUser>
        , IUserSecurityStampStore<TUser>
        , IUserEmailStore<TUser>
        , IUserPhoneNumberStore<TUser>
        , IUserTwoFactorStore<TUser>
        , IUserLockoutStore<TUser>
        where TUser : IdentityUser<TKey>
    {
        public UserStore(IDocumentSession session, ISystemClock clock)
        {
            if (session == null)
                throw new ArgumentNullException(nameof(session));

            if (clock == null)
                throw new ArgumentNullException(nameof(clock));

            Session = session;
            Clock = clock;
        }

        /// <summary>
        /// Gets the database document session for this store.
        /// </summary>
        public IDocumentSession Session { get; private set; }

        /// <summary>
        /// Gets or sets a flag indicating if changes should be persisted after CreateAsync, UpdateAsync and DeleteAsync are called.
        /// </summary>
        /// <value>
        /// True if changes should be automatically persisted, otherwise false.
        /// </value>
        public bool AutoSaveChanges { get; set; } = true;

        public ISystemClock Clock { get; private set; }

        #region IUserStore<TUser> Support
        public async Task<IdentityResult> CreateAsync(TUser user, CancellationToken cancellationToken)
        {
            Guard(user, cancellationToken);

            Session.Store<TUser>(user);
            await SaveChanges(cancellationToken);
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> UpdateAsync(TUser user, CancellationToken cancellationToken)
        {
            Guard(user, cancellationToken);

            Session.Store<TUser>(user);
            await SaveChanges(cancellationToken);
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(TUser user, CancellationToken cancellationToken)
        {
            Guard(user, cancellationToken);

            Session.Delete<TUser>(user);
            await SaveChanges(cancellationToken);
            return IdentityResult.Success;
        }      

        public Task<string> GetUserIdAsync(TUser user, CancellationToken cancellationToken)
        {
            Guard(user, cancellationToken);

            return Task.FromResult(user.Id.ToString());
        }

        public Task<TUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            return Session.QueryAsync(new FindUserById<TUser, TKey>(userId), cancellationToken);
        }

        public Task<string> GetUserNameAsync(TUser user, CancellationToken cancellationToken)
        {
            Guard(user, cancellationToken);

            return Task.FromResult(user.UserName);
        }

        public Task SetUserNameAsync(TUser user, string userName, CancellationToken cancellationToken)
        {
            Guard(user, cancellationToken);

            user.UserName = userName;
            return Task.FromResult(0);
        }

        public Task<string> GetNormalizedUserNameAsync(TUser user, CancellationToken cancellationToken)
        {
            Guard(user, cancellationToken);

            return Task.FromResult(user.NormalizedUserName);
        }

        public Task SetNormalizedUserNameAsync(TUser user, string normalizedUserName, CancellationToken cancellationToken)
        {
            Guard(user, cancellationToken);

            user.NormalizedUserName = normalizedUserName;
            return Task.FromResult(0);
        }

        public Task<TUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            Guard(cancellationToken);

            return Session.QueryAsync(new FindUserByNormalizedUserName<TUser, TKey>(normalizedUserName), cancellationToken);
        }
        #endregion

        #region IUserPasswordStore<TUser> Support

        public Task<string> GetPasswordHashAsync(TUser user, CancellationToken cancellationToken)
        {
            Guard(user, cancellationToken);

            return Task.FromResult(user.PasswordHash);
        }

        public Task SetPasswordHashAsync(TUser user, string passwordHash, CancellationToken cancellationToken)
        {
            Guard(user, cancellationToken);

            user.PasswordHash = passwordHash;
            return Task.FromResult(0);
        }

        public Task<bool> HasPasswordAsync(TUser user, CancellationToken cancellationToken)
        {
            Guard(user, cancellationToken);

            return Task.FromResult(!String.IsNullOrWhiteSpace(user.PasswordHash));
        }

        #endregion

        #region ISecurityStampStore<TUser> Support

        public Task<string> GetSecurityStampAsync(TUser user, CancellationToken cancellationToken)
        {
            Guard(user, cancellationToken);

            return Task.FromResult(user.SecurityStamp);
        }

        public Task SetSecurityStampAsync(TUser user, string stamp, CancellationToken cancellationToken)
        {
            Guard(user, cancellationToken);

            user.SecurityStamp = stamp;
            return Task.FromResult(0);
        }

        #endregion

        #region IUserEmailStore<TUser> Support

        public Task<string> GetEmailAsync(TUser user, CancellationToken cancellationToken)
        {
            Guard(user, cancellationToken);

            return Task.FromResult(user.Email);
        }

        public Task SetEmailAsync(TUser user, string email, CancellationToken cancellationToken)
        {
            Guard(user, cancellationToken);

            user.Email = email;
            return Task.FromResult(0);
        }

        public Task<string> GetNormalizedEmailAsync(TUser user, CancellationToken cancellationToken)
        {
            Guard(user, cancellationToken);

            return Task.FromResult(user.NormalizedEmail);
        }

        public Task SetNormalizedEmailAsync(TUser user, string normalizedEmail, CancellationToken cancellationToken)
        {
            Guard(user, cancellationToken);

            user.NormalizedEmail = normalizedEmail;
            return Task.FromResult(0);
        }

        public Task<TUser> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            Guard(cancellationToken);

            return Session.QueryAsync(new FindUserByNormalizedEmail<TUser, TKey>(normalizedEmail));
        }

        public Task<bool> GetEmailConfirmedAsync(TUser user, CancellationToken cancellationToken)
        {
            Guard(user, cancellationToken);

            return Task.FromResult(user.EmailConfirmedAt.HasValue);
        }

        public Task SetEmailConfirmedAsync(TUser user, bool confirmed, CancellationToken cancellationToken)
        {
            Guard(user, cancellationToken);

            user.EmailConfirmedAt = confirmed ? Clock.UtcNow : (DateTimeOffset?)null;
            return Task.FromResult(0);
        }

        #endregion

        #region IUserPhoneNumberStore<TUser> Support

        public Task<string> GetPhoneNumberAsync(TUser user, CancellationToken cancellationToken)
        {
            Guard(user, cancellationToken);

            return Task.FromResult(user.PhoneNumber);
        }

        public Task SetPhoneNumberAsync(TUser user, string phoneNumber, CancellationToken cancellationToken)
        {
            Guard(user, cancellationToken);

            user.PhoneNumber = phoneNumber;
            return Task.FromResult(0);
        }

        public Task<bool> GetPhoneNumberConfirmedAsync(TUser user, CancellationToken cancellationToken)
        {
            Guard(user, cancellationToken);

            return Task.FromResult(user.PhoneNumberConfirmedAt.HasValue);
        }

        public Task SetPhoneNumberConfirmedAsync(TUser user, bool confirmed, CancellationToken cancellationToken)
        {
            Guard(user, cancellationToken);

            user.PhoneNumberConfirmedAt = confirmed ? Clock.UtcNow : (DateTimeOffset?)null;
            return Task.FromResult(0);
        }

        #endregion

        #region IUserTwoFactorStore<TUser> Support

        public Task<bool> GetTwoFactorEnabledAsync(TUser user, CancellationToken cancellationToken)
        {
            Guard(user, cancellationToken);

            return Task.FromResult(user.TwoFactorEnabledAt.HasValue);
        }

        public Task SetTwoFactorEnabledAsync(TUser user, bool enabled, CancellationToken cancellationToken)
        {
            Guard(user, cancellationToken);

            user.TwoFactorEnabledAt = enabled ? Clock.UtcNow : (DateTimeOffset?)null;
            return Task.FromResult(0);
        }

        #endregion

        #region IUserLockoutStore<TUser> Support

        public Task<bool> GetLockoutEnabledAsync(TUser user, CancellationToken cancellationToken)
        {
            Guard(user, cancellationToken);

            return Task.FromResult(user.LockoutEnabledAt.HasValue);
        }

        public Task SetLockoutEnabledAsync(TUser user, bool enabled, CancellationToken cancellationToken)
        {
            Guard(user, cancellationToken);

            user.LockoutEnabledAt = enabled ? Clock.UtcNow : (DateTimeOffset?)null;
            return Task.FromResult(0);
        }

        public Task<DateTimeOffset?> GetLockoutEndDateAsync(TUser user, CancellationToken cancellationToken)
        {
            Guard(user, cancellationToken);

            return Task.FromResult(user.LockoutEndsAt);
        }

        public Task SetLockoutEndDateAsync(TUser user, DateTimeOffset? lockoutEnd, CancellationToken cancellationToken)
        {
            Guard(user, cancellationToken);

            user.LockoutEndsAt = lockoutEnd;
            return Task.FromResult(0);
        }

        public Task<int> GetAccessFailedCountAsync(TUser user, CancellationToken cancellationToken)
        {
            Guard(user, cancellationToken);

            return Task.FromResult(user.AccessFailedAt.Count);
        }

        public Task<int> IncrementAccessFailedCountAsync(TUser user, CancellationToken cancellationToken)
        {
            Guard(user, cancellationToken);

            user.AccessFailedAt.Add(Clock.UtcNow);
            return Task.FromResult(user.AccessFailedAt.Count);
        }

        public Task ResetAccessFailedCountAsync(TUser user, CancellationToken cancellationToken)
        {
            Guard(user, cancellationToken);

            user.AccessFailedAt.Clear();
            return Task.FromResult(0);
        }

        #endregion

        #region IDisposable Support
        private bool _disposed;

        /// <summary>
        /// Dispose the store
        /// </summary>
        public void Dispose()
        {
            _disposed = true;
        }
        #endregion

        #region Helpers

        protected void Guard(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (_disposed)
                throw new ObjectDisposedException(GetType().Name);
        }

        protected void Guard(TUser user, CancellationToken cancellationToken)
        {
            Guard(cancellationToken);

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
        }

        /// <summary>Saves the current store.</summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
        protected Task SaveChanges(CancellationToken cancellationToken)
        {
            return AutoSaveChanges ? Session.SaveChangesAsync(cancellationToken) : Task.FromResult(0);
        }

        #endregion
    }
}
