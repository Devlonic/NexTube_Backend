using AutoMapper;
using FluentValidation;
using Google.Apis.Auth;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NexTube.Application.Common.Interfaces;
using NexTube.Application.CQRS.Identity.Users.Commands.ChangePassword;
using NexTube.Application.CQRS.Identity.Users.Commands.CreateUser;
using NexTube.Application.CQRS.Identity.Users.Commands.Recover;
using NexTube.Application.CQRS.Identity.Users.Commands.SignInUser;
using NexTube.Application.CQRS.Identity.Users.Commands.SignInWithProvider;
using NexTube.Application.CQRS.Identity.Users.Commands.VerifyMail;
using NexTube.WebApi.DTO.Auth.ChangePassword;
using NexTube.WebApi.DTO.Auth.User;
using WebShop.Application.Common.Exceptions;
using WebShop.Domain.Constants;

namespace NexTube.WebApi.Controllers {
    public class AuthController : BaseController {

        private readonly IMapper mapper;
        private readonly ICaptchaValidatorService captchaValidator;

        public AuthController(IMapper mapper, ICaptchaValidatorService captchaValidator) {
            this.mapper = mapper;
            this.captchaValidator = captchaValidator;
        }



        [HttpPost]
        public async Task<ActionResult<int>> SignUp([FromForm] SignUpDto dto) {
            var captcha_result = await captchaValidator.IsCaptchaPassedAsync(dto.CaptchaToken ?? "");
            if ( captcha_result == false )
                throw new ValidationException("Missing token or failed captcha validation");

            // map received from request dto to cqrs command
            var command = mapper.Map<CreateUserCommand>(dto);
            var userId = await Mediator.Send(command);

            return Ok(userId);
        }

        [HttpPost]
        public async Task<ActionResult> SignIn([FromBody] SignInUserDto dto) {
            var captcha_result = await captchaValidator.IsCaptchaPassedAsync(dto.CaptchaToken ?? "");
            if ( captcha_result == false )
                throw new ValidationException("Missing token or failed captcha validation");

            // map received from request dto to cqrs command
            var command = mapper.Map<SignInUserCommand>(dto);
            var result = await Mediator.Send(command);

            return Ok(result);
        }


        [HttpPost]
        public async Task<ActionResult> VerifyUser([FromBody] VerifyUserDto dto) {
            // map received from request dto to cqrs command
            var command = mapper.Map<VerifyMailCommand>(dto);
            command.UserId = UserId;
            var result = await Mediator.Send(command);

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult> SignInWithProviderToken([FromBody] SignInWithProviderDto dto) {
            var command = mapper.Map<SignInWithProviderCommand>(dto);
            var result = await Mediator.Send(command);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult> Recover([FromBody] RecoverDto dto) {
            // map received from request dto to cqrs command
            var command = mapper.Map<RecoverCommand>(dto);
            var result = await Mediator.Send(command);
            return Ok(result);
        }

        [Authorize(Roles = Roles.User)]
        [HttpPost]
        public async Task<ActionResult> ChangePassword([FromBody] ChangePasswordDto dto) {
            // map received from request dto to cqrs command
            var command = mapper.Map<ChangePasswordCommand>(dto);
            command.UserId = UserId;
            var result = await Mediator.Send(command);
            return Ok(result);
        }
    }
}
