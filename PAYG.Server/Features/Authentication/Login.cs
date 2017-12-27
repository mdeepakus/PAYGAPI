using MediatR;
using FluentValidation;
using PAYG.Domain;
using PAYG.Domain.Common;
using PAYG.Domain.Entities;
using PAYG.Domain.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using PAYG.Infrastructure.Authentication;

namespace PAYG.Server.Features.Authentication
{
    /// <summary>
    /// 
    /// </summary>
    public class Login
    {
        /// <summary>
        /// 
        /// </summary>
        public class Command : IRequest<Result>
        {
            /// <summary>
            /// UserName
            /// </summary>
            public string UserName { get; set; }

            /// <summary>
            /// Password
            /// </summary>
            public string Password { get; set; }

        }

        /// <summary>
        /// 
        /// </summary>
        public class Validator : AbstractValidator<Command>
        {
            /// <summary>
            /// 
            /// </summary>
            public Validator()
            {
                RuleFor(v => v.UserName).NotEmpty().WithMessage("Username must be supplied");
                RuleFor(v => v.Password).NotEmpty().WithMessage("Password must be supplied");
            }
        }

        /// <summary>
        /// Resul;t
        /// </summary>
        public class Result
        {
            /// <summary>
            /// Authentication Token
            /// </summary>
            [Required]
            public string Token { get; set; }

            /// <summary>
            /// User Id
            /// </summary>
            [Required]
            public int UserId { get; set; }

            /// <summary>
            /// User name
            /// </summary>
            [Required]
            public string Username { get; set; }

            /// <summary>
            /// Date of last password change
            /// </summary>
            public string ActionMessage { get; set; }

        }

        /// <summary>
        /// 
        /// </summary>
        public class Handler : IAsyncRequestHandler<Command, Result>
        {
            private readonly IMediator _mediator;
            private IUserService _service;
            private ITokenProvider _tokenProvider;

            /// <summary>
            /// 
            /// </summary>
            /// <param name="mediator"></param>
            /// <param name="userService"></param>
            /// <param name="tokenProvider"></param>
            public Handler(IMediator mediator, IUserService userService, ITokenProvider tokenProvider)
            {
                _mediator = mediator;
                _service = userService;
                _tokenProvider = tokenProvider;
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="command"></param>
            /// <returns></returns>
            public async Task<Result> Handle(Command command)
            {
                var result = await _service.UserLogon(command.UserName, command.Password);

                var user = result.Object;

                if (result.Result.Equals(PAYG.Domain.Common.Result.Fail))
                {
                    return new Result()
                    {
                        Username = command.UserName,
                        ActionMessage = result.ErrorMessage
                    };
                }

                var token = _tokenProvider.GenerateToken(user);

                return new Result
                {
                    UserId = user.UserId,
                    Username = user.Username,
                    ActionMessage = "Successfull",
                    Token = token
                };
            }
        }
    }
}
