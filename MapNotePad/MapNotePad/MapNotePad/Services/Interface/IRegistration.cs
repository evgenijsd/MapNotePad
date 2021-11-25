using System.Threading.Tasks;
using MapNotePad.Enum;
using MapNotePad.Helpers.ProcessHelpers;
using MapNotePad.Models;

namespace MapNotePad.Services.Interface
{
    public interface IRegistration
    {
        Task<int> UserAddAsync(Users user);
        Task<AOResult<int>> CheckTheCorrectEmailAsync(string name, string email);
        int CheckTheCorrectPassword(string password, string confirmPassword);
        ECheckEnter CheckCorrectEmail(string email);
    }
}
