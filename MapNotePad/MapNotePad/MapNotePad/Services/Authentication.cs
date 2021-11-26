using MapNotePad.Helpers.ProcessHelpers;
using MapNotePad.Models;
using MapNotePad.Services.Interface;
using MapNotePad.Services.Repository;
using System;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace MapNotePad.Services
{
    public class Authentication : IAuthentication
    {
        private IRepository _repository { get; }

        public Authentication(IRepository repository)
        {
            _repository = repository;
        }

        #region -- Public properties --
        public int UserId
        {
            get => Preferences.Get(nameof(UserId), 0);
            set => Preferences.Set(nameof(UserId), value);
        }
        #endregion
        #region -- Public helpers --
        public async Task<AOResult<int>> CheckUserAsync(string email, string password)
        {
            var result = new AOResult<int>();

            try
            {
                var user = await _repository.FindAsync<Users>(x => x.Email == email);
                if (user != null && user.Email == email && user.Password == password)
                {
                    result.SetSuccess(user.Id);
                }
                else
                    result.SetFailure();
            }
            catch (Exception ex)
            {
                result.SetError($"Exception: {nameof(CheckUserAsync)}", "Wrong result", ex);
            }

            return result;
        }
        #endregion

    }
}
