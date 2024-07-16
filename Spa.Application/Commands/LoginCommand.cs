using MediatR;
using Spa.Application.Authentication;
using Spa.Application.Models;
using Spa.Domain.IRepository;

namespace Spa.Application.Commands
{
    public class LoginCommand : IRequest<AuthenticationResult>
    {
        public UserSession Session { get; set; }
        public AuthenticationResult authDTO { get; set; }
        public LoginDTO loginDTO { get; set; }
        

    }
    public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthenticationResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly IBranchRepository _branchRepository;

        public LoginCommandHandler(IUserRepository userRepository, IBranchRepository branchRepository)
        {
            _userRepository = userRepository;
            _branchRepository = branchRepository;
        }

        public async Task<AuthenticationResult> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByEmail(request.loginDTO.Email);
            if (user == null)
            {
                return new AuthenticationResult(false,"Tài khoản không tồn tại!",null,null);
            }
            string token = await _userRepository.LoginAccount(request.loginDTO.Email, request.loginDTO.Password);
            if (token is null)
            {
                return new AuthenticationResult(false,"Mật khẩu không đúng!",null,null);
            }
            if (user.Role != "Admin")
            {
                var emp = await _userRepository.GetEmpByEmail(request.loginDTO.Email);
                string branch = await _branchRepository.GetBranchNameByID(emp.BranchID);
                var userSession = new UserSession(user.Email, user.LastName + " " + user.FirstName, user.Role, branch,emp.BranchID);
                return new AuthenticationResult(true, "Thành công!", userSession, token);
            }
            else
            {
                var userSession = new UserSession(user.Email, user.LastName + " " + user.FirstName, user.Role,null, 1);
                return new AuthenticationResult(true, "Thành công!", userSession, token);
            }
            
        }
    }
}
