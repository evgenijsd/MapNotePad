using MapNotePad.Helpers.ProcessHelpers;
using System.Threading.Tasks;

namespace MapNotePad.Services.Interface
{
    public interface IAuthentication
    {
        Task<int> CheckAsync(string Login, string Password);
    }
}
