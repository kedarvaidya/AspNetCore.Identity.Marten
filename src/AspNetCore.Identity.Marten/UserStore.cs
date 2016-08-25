using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AspNetCore.Identity.Marten.Internal;
using Marten;
using Microsoft.AspNetCore.Identity;

namespace AspNetCore.Identity.Marten
{
    public class UserStore<TUser, TKey>
        : IUserStore<TUser>
        where TUser : IdentityUser<TKey>
    {
        public UserStore(IDocumentSession session)
        {
            if (session == null)
                throw new ArgumentNullException(nameof(session));

            Session = session;
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
