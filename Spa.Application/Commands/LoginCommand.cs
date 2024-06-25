﻿using MediatR;
using Spa.Application.Authentication;
using Spa.Application.Models;
using Spa.Domain.IRepository;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public LoginCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<AuthenticationResult> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByEmail(request.loginDTO.Email);
            if (user == null)
            {
                return new AuthenticationResult(null,"User not exit.");
            }
            string token = await _userRepository.LoginAccount(request.loginDTO.Email, request.loginDTO.Password);
            if (token is null)
            {
                return new AuthenticationResult(null,"Invalid email or password.");
            }
            var userSession = new UserSession(user.Code,user.Email,user.FirstName+" "+user.LastName,user.Role);
            return new AuthenticationResult(userSession,token);
        }
    }
}
