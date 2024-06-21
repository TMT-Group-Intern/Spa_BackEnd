using MediatR;
using Spa.Application.Authentication;
using Spa.Application.Models;
using Spa.Domain.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spa.Application.Commands
{
    public class LoginCommand : IRequest<AuthenticationResult>
    {
        public AuthenticationResult authDTO { get; set; }
        public LoginDTO loginDTO { get; set; }
    }
    public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthenticationResult>
    {
        private readonly IUserRepository _userRepository;

        public LoginCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<AuthenticationResult> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            // Fetch the user from the repository based on the email
            var user = await _userRepository.GetUserByEmail(request.loginDTO.Email);

            // If the user is not found, return an authentication failure result
            if (user == null)
            {
                return new AuthenticationResult(null, "Invalid email or password.");
            }
            var token = await _userRepository.LoginAccount(request.loginDTO.Email, request.loginDTO.Password);

            // Verify the password
            if (token is null)
            {
                return new AuthenticationResult(null, "Invalid email or password.");
            }

            // Generate a JWT token for the authenticated user


            // Return the authentication result
            return new AuthenticationResult(user, token);
        }
    }
}
