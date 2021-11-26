using MapNotePad.Helpers.ProcessHelpers;
using System.Threading.Tasks;

namespace MapNotePad.Services.Interface
{
    public interface IAuthentication
    {
        int UserId { get; set; }

        Task<AOResult<int>> CheckUserAsync(string Login, string Password);
    }
}
