using MediatR;
using NexTube.Application.Common.Interfaces;
using NexTube.Application.Common.Models;
using NexTube.Application.CQRS.Files.Photos.Commands.UploadSquarePhoto;
using NexTube.Application.CQRS.Identity.Users.Commands.VerifyMail;

namespace NexTube.Application.CQRS.Identity.Users.Commands.CreateUser
{
    public class CreateUserCommnadHandler : IRequestHandler<CreateUserCommand, CreateUserCommandResult>
    {
        private readonly IIdentityService _identityService;
        private readonly IJwtService _jwtService;
        private readonly IMediator _mediator;
        private readonly IMailService _mailService;
        public CreateUserCommnadHandler(IIdentityService identityService, IJwtService jwtService, IPhotoService photoService, IMediator mediator, IMailService mailService)
        {
            _identityService = identityService;
            _jwtService = jwtService;
            _mediator = mediator;
            _mailService = mailService;
        }
        public async Task<CreateUserCommandResult> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var photo = await _mediator.Send(new UploadSquarePhotoCommand() { Source = request.ChannelPhotoStream });

            var result = await _identityService.CreateUserAsync(
                 request.Password, request.Email, request.FirstName, request.LastName, Guid.Parse(photo));

            var secretPhrase = _mailService.GeneratePassword(64);
            await _mailService.SendMailAsync("Verify your registration: " + secretPhrase, request.Email);
          

            return new CreateUserCommandResult() {
                Result = result.Result,
                UserId = result.User.UserId,
                Token = _jwtService.GenerateToken(result.User.UserId ?? -1, result.User),
                VerificationToken = _jwtService.GenerateTokenWithSecretPhrase(secretPhrase)
            };


        }
    }
}
