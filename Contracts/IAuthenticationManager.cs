using System.Threading.Tasks;
using CompanyEmployee.Entities.DataTransferObjects;

namespace CompanyEmployee.Contracts
{
    public interface IAuthenticationManager
    {
        Task<bool> ValidateUser(LoginDto loginDto);
        Task<string> CreateToken();
    }
}