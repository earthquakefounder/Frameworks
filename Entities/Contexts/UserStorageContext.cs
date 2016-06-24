using Entities.Models.Identity;
using Entities.Settings;
using Infrastructure.Encryption;
using Infrastructure.Password;
using Infrastructure.Results;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Contexts
{
    public class UserStorageContext : BaseStorageContext, IStorageContext<AppUser>
    {
        IPasswordComplexity _passwordComplexity;
        IEncryptor _encryptor;

        public UserStorageContext(
            DatabaseConnections settings,
            IPasswordComplexity passwordComplexity,
            IEncryptor encryptor
        ) : base(settings.Database)
        {
            _passwordComplexity = passwordComplexity;
            _encryptor = encryptor;
        }

        private DbSet<_AppUser> Users => Set<_AppUser>();

        public IQueryable<AppUser> Entities => 
            Users.Select(user => new AppUser()
                {
                    Email = user.Email,
                    ID = user.ID,
                    Name = user.Name,
                    UserName = user.UserName
                });

        public AppUser Add(AppUser user)
        {
            var _user = new _AppUser()
            {
                ID = Guid.NewGuid(),
                UserName = user.UserName,
                Email = user.Email,
                Name = user.Name
            };

            Users.Add(_user);

            return _user;
        }

        public void Delete(AppUser user) => Users.Remove(new _AppUser() { ID = user.ID });

        public async Task<ValidationResult<UpdatePasswordFailure>> UpdatePasswordAsync(AppUser user, string password) => await UpdatePasswordAsync(user.ID.Value, password);

        public async Task<ValidationResult<UpdatePasswordFailure>> UpdatePasswordAsync(Guid id, string password)
        {
            var user = Users.Local.FirstOrDefault(x => x.ID == id) ?? await Users.FirstOrDefaultAsync(x => x.ID == id);

            if (user == null)
                throw new ArgumentException($"User '{id}' does not exist");

            if (!_passwordComplexity.Validate(password))
                return new ValidationResult<UpdatePasswordFailure>(UpdatePasswordFailure.PasswordNotComplex, _passwordComplexity.ComplexityMessage);

            string passwordHash, salt;
            passwordHash = _encryptor.Encrypt(password, out salt);

            user.PasswordHash = passwordHash;
            user.Salt = salt;

            return new ValidationResult<UpdatePasswordFailure>();
        }

        public async Task<ValidationResult<ChangePasswordResult>> AuthenticateAsync(string username, string password)
        {
            var user = await Users.FirstOrDefaultAsync(x => x.Email == username);

            if (user == null)
                return new ValidationResult<ChangePasswordResult>(ChangePasswordResult.InvalidCredentials);

            bool matched = _encryptor.Compare(password, user.PasswordHash, user.Salt);

            return !matched
                ? new ValidationResult<ChangePasswordResult>(ChangePasswordResult.InvalidCredentials)
                : new ValidationResult<ChangePasswordResult>();
        }
    }

    internal class _AppUser : AppUser
    {
        public string PasswordHash { get; set; }
        public string Salt { get; set; }
    }

    public enum UpdatePasswordFailure
    {
        PasswordNotComplex
    }

    public enum ChangePasswordResult
    {
        InvalidCredentials
    }
}
