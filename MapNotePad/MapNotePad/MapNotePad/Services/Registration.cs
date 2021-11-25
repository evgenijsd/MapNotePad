using MapNotePad.Enum;
using MapNotePad.Helpers.ProcessHelpers;
using MapNotePad.Models;
using MapNotePad.Services.Interface;
using MapNotePad.Services.Repository;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MapNotePad.Services
{
    public class Registration : IRegistration
    {
        private IRepository _repository;
        const int MAX_LENGTH_EMAIL = 64;
        const int MIN_PASSWORD_LENGTH = 6;

        public Registration(IRepository repository)
        {
            _repository = repository;
        }

        #region -- Public helpers --
        public async Task<AOResult<int>> CheckTheCorrectEmailAsync(string name, string email)
        {
            var result = new AOResult<int>();
            try
            {
                var user = await _repository.FindAsync<Users>(x => x.Email == email);
                ECheckEnter check = ECheckEnter.ChecksArePassed;
                check = CheckCorrectEmail(email);
                if (user != null)
                {
                    check = ECheckEnter.LoginExist;
                }
                result.SetSuccess((int)check);
            }
            catch (Exception ex)
            {
                result.SetError($"Exception: {nameof(CheckTheCorrectEmailAsync)}", "Wrong result", ex);
            }
            return result;
        }

        public ECheckEnter CheckCorrectEmail(string email)
        {
            ECheckEnter result = ECheckEnter.ChecksArePassed;
            int s = email.IndexOf('@');
            if (s > MAX_LENGTH_EMAIL || (email.Length - s) > MAX_LENGTH_EMAIL
                || email.Length - 1 == s || s == 0) result = ECheckEnter.EmailLengthNotValid;
            if (s == -1) result = ECheckEnter.EmailANotVaid;
            return result;
        }

        public int CheckTheCorrectPassword(string password, string confirmPassword)
        {
            const string validPassword = @"^(?=.*[A-ZА-ЯЁҐЄЇІ])(?=.*\d)[\d\D]+$";
            ECheckEnter check = ECheckEnter.ChecksArePassed;

            if (password != confirmPassword) check = ECheckEnter.PasswordsNotEqual;
            if (!Regex.IsMatch(password, validPassword)) check = ECheckEnter.PasswordBigLetterAndDigit;
            if (password.Length < MIN_PASSWORD_LENGTH) check = ECheckEnter.PasswordLengthNotValid;
            return (int)check;
        }

        public async Task<int> UserAddAsync(Users user)
        {
            int result = 0;
            try
            {
                result = await _repository.AddAsync(user);
            }
            catch { }
            return result;
        }
        #endregion
    }
}
