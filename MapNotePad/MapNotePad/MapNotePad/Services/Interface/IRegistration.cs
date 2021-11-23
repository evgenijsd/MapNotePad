using MapNotePad.Helpers.ProcessHelpers;
using MapNotePad.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MapNotePad.Services.Interface
{
    public interface IRegistration
    {
        Task<int> UserAddAsync(Users user);
        Task<AOResult<int>> CheckTheCorrectEmailAsync(string name, string email);
        int CheckTheCorrectPassword(string password, string confirmPassword);
    }
}
