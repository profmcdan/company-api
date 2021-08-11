using System.Threading.Tasks;

namespace CompanyEmployee.Contracts
{
    public interface IRepositoryManager
    {
        ICompanyRepository Company { get; }
        IEmployeeRepository  Employee { get; }
        Task SaveAsync();
    }
}