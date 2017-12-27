using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using PAYG.Domain.Services;
using PAYG.Server.Validators.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PAYG.Server.Features.Authentication
{
    public class RegisterUser
    {
        /// <summary>
        /// 
        /// </summary>
        public class Command : IRequest<Login.Result>
        {
            /// <summary>
            /// User Name
            /// </summary>
            public string userName { get; set; }

            /// <summary>
            /// New password
            /// </summary>
            public string password { get; set; }

            /// <summary>
            /// Confirm new password
            /// </summary>
            public string ConfirmPassword { get; set; }
        }

        /// <summary>
        /// 
        /// </summary>
        public class Validator : AbstractValidator<Command>
        {
            private readonly IUserValidator _userValidator;
            /// <summary>
            /// 
            /// </summary>
            public Validator(IUserValidator userValidator)
            {
                _userValidator = userValidator;

                RuleFor(v => v.userName).NotEmpty().WithMessage("A user name must be supplied");
                RuleFor(v => v.password).NotEmpty().WithMessage("A new password must be supplied");
                RuleFor(v => v.ConfirmPassword).NotEmpty().WithMessage("A confirmation password must be supplied");
                RuleFor(v => v.password).Equal(c => c.ConfirmPassword).WithMessage("Password and confirm password must match");

                RuleFor(v => v.userName)
                    .Must((command, userName) => UserNameValidation(command, userName).Result)
                                    .WithMessage(x => string.Format("User name: {0} is already in use.", x.userName));


            }

            private async Task<bool> UserNameValidation(Command command, string userName)
            {
                return await _userValidator.ValidateUserExists(userName);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public class Handler : IAsyncRequestHandler<Command, Login.Result>
        {
            private readonly IMediator _mediator;
            private readonly IUserService _service;

            public Handler(IMediator mediator, IUserService service)
            {
                _mediator = mediator;
                _service = service;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="message"></param>
            /// <returns></returns>
            public async Task<Login.Result> Handle(Command message)
            {
                var userId = await _service.RegisterConsumerUser(message.userName, message.password, message.ConfirmPassword);

                var loginCommand = new Login.Command
                {
                    UserName = message.userName,
                    Password = message.password,
                };

                return await _mediator.Send(loginCommand);
            }
        }
    }
}
