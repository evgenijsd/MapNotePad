using MapNotePad.Enum;
using MapNotePad.Models;
using MapNotePad.Services.Interface;
using MapNotePad.Services.Repository;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MapNotePad.Services
{
    public class Registration : CheckType, IRegistration
    {
        private IRepository _repository;
        const int MaxLengthEmail = 64;
        const int MinPasswordLength = 6;
        public Registration(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<int> CheckTheCorrectEmailAsync(string name, string email)
        {
            var user = await _repository.FindAsync<Users>(x => x.Email == email);
            CheckEnter check = CheckEnter.ChecksArePassed;

            int s = email.IndexOf('@');
            if (user != null && name != string.Empty) check = CheckEnter.LoginExist;
            if (s > MaxLengthEmail || (email.Length - s) > MaxLengthEmail || email.Length-1 == s || s == 0) check = CheckEnter.EmailLengthNotValid;
            if (s == -1) check = CheckEnter.EmailANotVaid;
            return (int)check;
        }

        public int CheckTheCorrectPassword(string password, string confirmPassword)
        {
            const string validPassword = @"^(?=.*[A-ZА-ЯЁҐЄЇІ])(?=.*\d)[\d\D]+$";
            CheckEnter check = CheckEnter.ChecksArePassed;

            if (!Regex.IsMatch(password, validPassword)) check = CheckEnter.PasswordBigLetterAndDigit;
            if (password != confirmPassword) check = CheckEnter.PasswordsNotEqual;
            if (password.Length < MinPasswordLength) check = CheckEnter.PasswordLengthNotValid;
            return (int)check;
        }

        public async  Task<int> UserAddAsync(Users user)
        {
            int result = 0;
            try
            {
                result = await _repository.AddAsync(user);
            }
            catch { }
            return result;
        }
    }
}
