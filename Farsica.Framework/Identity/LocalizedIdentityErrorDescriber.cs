namespace Farsica.Framework.Identity
{
    using Farsica.Framework.Resources;
    using Microsoft.AspNetCore.Identity;

    public class LocalizedIdentityErrorDescriber : IdentityErrorDescriber
    {
        public override IdentityError DuplicateEmail(string? email)
        {
            return new IdentityError
            {
                Code = nameof(DuplicateEmail),
                Description = string.Format(GlobalResource.DuplicateEmail, email),
            };
        }

        public override IdentityError DuplicateUserName(string? userName)
        {
            return new IdentityError
            {
                Code = nameof(DuplicateUserName),
                Description = string.Format(GlobalResource.DuplicateUserName, userName),
            };
        }

        public override IdentityError InvalidEmail(string? email)
        {
            return new IdentityError
            {
                Code = nameof(InvalidEmail),
                Description = string.Format(GlobalResource.InvalidEmail, email),
            };
        }

        public override IdentityError DuplicateRoleName(string? role)
        {
            return new IdentityError
            {
                Code = nameof(DuplicateRoleName),
                Description = string.Format(GlobalResource.DuplicateRoleName, role),
            };
        }

        public override IdentityError InvalidRoleName(string? role)
        {
            return new IdentityError
            {
                Code = nameof(InvalidRoleName),
                Description = string.Format(GlobalResource.InvalidRoleName, role),
            };
        }

        public override IdentityError InvalidToken()
        {
            return new IdentityError
            {
                Code = nameof(InvalidToken),
                Description = GlobalResource.InvalidToken,
            };
        }

        public override IdentityError InvalidUserName(string? userName)
        {
            return new IdentityError
            {
                Code = nameof(InvalidUserName),
                Description = string.Format(GlobalResource.InvalidUserName, userName),
            };
        }

        public override IdentityError LoginAlreadyAssociated()
        {
            return new IdentityError
            {
                Code = nameof(LoginAlreadyAssociated),
                Description = GlobalResource.LoginAlreadyAssociated,
            };
        }

        public override IdentityError PasswordMismatch()
        {
            return new IdentityError
            {
                Code = nameof(PasswordMismatch),
                Description = GlobalResource.PasswordMismatch,
            };
        }

        public override IdentityError PasswordRequiresDigit()
        {
            return new IdentityError
            {
                Code = nameof(PasswordRequiresDigit),
                Description = GlobalResource.PasswordRequiresDigit,
            };
        }

        public override IdentityError PasswordRequiresLower()
        {
            return new IdentityError
            {
                Code = nameof(PasswordRequiresLower),
                Description = GlobalResource.PasswordRequiresLower,
            };
        }

        public override IdentityError PasswordRequiresNonAlphanumeric()
        {
            return new IdentityError
            {
                Code = nameof(PasswordRequiresNonAlphanumeric),
                Description = GlobalResource.PasswordRequiresNonAlphanumeric,
            };
        }

        public override IdentityError PasswordRequiresUniqueChars(int uniqueChars)
        {
            return new IdentityError
            {
                Code = nameof(PasswordRequiresUniqueChars),
                Description = string.Format(GlobalResource.PasswordRequiresUniqueChars, uniqueChars),
            };
        }

        public override IdentityError PasswordRequiresUpper()
        {
            return new IdentityError
            {
                Code = nameof(PasswordRequiresUpper),
                Description = GlobalResource.PasswordRequiresUpper,
            };
        }

        public override IdentityError PasswordTooShort(int length)
        {
            return new IdentityError
            {
                Code = nameof(PasswordTooShort),
                Description = string.Format(GlobalResource.PasswordTooShort, length),
            };
        }

        public override IdentityError UserAlreadyHasPassword()
        {
            return new IdentityError
            {
                Code = nameof(UserAlreadyHasPassword),
                Description = GlobalResource.UserAlreadyHasPassword,
            };
        }

        public override IdentityError UserAlreadyInRole(string? role)
        {
            return new IdentityError
            {
                Code = nameof(UserAlreadyInRole),
                Description = string.Format(GlobalResource.UserAlreadyInRole, role),
            };
        }

        public override IdentityError UserNotInRole(string? role)
        {
            return new IdentityError
            {
                Code = nameof(UserNotInRole),
                Description = string.Format(GlobalResource.UserNotInRole, role),
            };
        }

        public override IdentityError UserLockoutNotEnabled()
        {
            return new IdentityError
            {
                Code = nameof(UserLockoutNotEnabled),
                Description = GlobalResource.UserLockoutNotEnabled,
            };
        }

        public override IdentityError RecoveryCodeRedemptionFailed()
        {
            return new IdentityError
            {
                Code = nameof(RecoveryCodeRedemptionFailed),
                Description = GlobalResource.RecoveryCodeRedemptionFailed,
            };
        }

        public override IdentityError ConcurrencyFailure()
        {
            return new IdentityError
            {
                Code = nameof(ConcurrencyFailure),
                Description = GlobalResource.ConcurrencyFailure,
            };
        }

        public override IdentityError DefaultError()
        {
            return new IdentityError
            {
                Code = nameof(DefaultError),
                Description = GlobalResource.DefaultIdentityError,
            };
        }
    }
}
