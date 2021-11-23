using MapNotePad.Models;
using MapNotePad.Services.Interface;
using MapNotePad.Services.Repository;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace MapNotePad.Services
{
    public class Authentication : IAuthentication
    {
        public int UserId
        {
            get => Preferences.Get(nameof(UserId), 0);
            set => Preferences.Set(nameof(UserId), value);
        }


        private IRepository _repository { get; }
        public Authentication(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<int> CheckAsync(string email, string password)
        {
            var result = 0;
            try
            {
                var user = await _repository.FindAsync<Users>(x => x.Email == email);
                if (user != null && user.Email == email && user.Password == password)
                {
                    return user.Id;
                }
            }
            catch
            { }

            return result;
        }
    }
}
