using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using PAYG.Domain.Entities;
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
            public string UserName { get; set; }

            /// <summary>
            /// New password
            /// </summary>
            public string Password { get; set; }

            /// <summary>
            /// Confirm new password
            /// </summary>
            public string ConfirmPassword { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public RegisterNewUser UserDetails { get; set; }
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

                RuleFor(v => v.UserName).NotEmpty().WithMessage("A user name must be supplied");
                RuleFor(v => v.Password).NotEmpty().WithMessage("A new password must be supplied");
                RuleFor(v => v.ConfirmPassword).NotEmpty().WithMessage("A confirmation password must be supplied");
                RuleFor(v => v.Password).Equal(c => c.ConfirmPassword).WithMessage("Password and confirm password must match");

                RuleFor(v => v.UserName)
                    .Must((command, userName) => UserNameValidation(command, userName).Result)
                                    .WithMessage(x => string.Format("User name: {0} is already in use.", x.UserName));
                RuleFor(v => v.UserDetails.EmailAddress).NotEmpty().WithMessage("Email Adrress must be supplied");
                RuleFor(v => v.UserDetails.FirstName).NotEmpty().WithMessage("First name must be supplied");
                RuleFor(v => v.UserDetails.LastName).NotEmpty().WithMessage("Last name must be supplied");
                RuleFor(v => v.UserDetails.City).NotEmpty().WithMessage("City must be supplied");
                RuleFor(v => v.UserDetails.Country).NotEmpty().WithMessage("Country must be supplied");
                RuleFor(v => v.UserDetails.AddressLine1).NotEmpty().WithMessage("Address Line 1 must be supplied");

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
                var userId = await _service.RegisterConsumerUser(message.UserName, message.Password, message.ConfirmPassword, message.UserDetails);

                var loginCommand = new Login.Command
                {
                    UserName = message.UserName,
                    Password = message.Password,
                };

                return await _mediator.Send(loginCommand);
            }
        }
    }
}
