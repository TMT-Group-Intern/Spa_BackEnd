using Spa.Domain.Entities;
using System.Security.Claims;

namespace Spa.Domain.IRepository
{
    public interface IUserRepository
    {
        Task<User> GetUserByEmail(string email);
        Task<string> GetUserBoolByEmail(string email);
        Task<Admin> GetAdminByEmail(string email);
        Task<Employee> GetEmpByEmail(string email);
        Task<List<User>> GetAllUsers();
        Task<List<Employee>> GetAllAdminsAndEmployees();
        Task<IEnumerable<User>> GetByPages(int pageNumber, int pageSize);
        Task<List<Employee>> GetAllEmployee();
        Task<List<Employee>> GetEmployeeByBranchAndJob(long branchID, long jobTypeID);
        Task<List<Admin>> GetAllAdmin();
        Task<User> CreateUser(User userDTO);
        Task<User> CreateUserForEmployee(string Email);
        Task CreateAdmin(Admin adminDTO);
        Task CreateEmployee(Employee empDTO);
        Task<string> LoginAccount(string Email, string Password);
        Task<string> GenerateToken(string Id, string Name, string Email, long? jobTypeID, string Role);
        Task<string> GenerateRefreshToken();
        Task<(string, string)> RefreshToken(string refreshToken, string jwtToken);
        Task<bool> DeleteUser(string Email);
        Task<bool> UpdateUser(User UserDTO);
        Task<bool> UpdateAdmin(Admin AdminDTO);
        Task<bool> UpdateEmployee(Employee EmpDTO);
        Task<Admin> GetLastAdminAsync();
        Task<Employee> GetLastEmployeeAsync();
        Task<int> GetAllItemProduct();

    }
}
