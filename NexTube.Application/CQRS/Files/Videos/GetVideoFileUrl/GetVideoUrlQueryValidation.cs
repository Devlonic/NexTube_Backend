using FluentValidation;
using NexTube.Application.Common.Interfaces;

namespace NexTube.Application.CQRS.Files.Videos.GetVideoFileUrl {
    public class GetVideoUrlQueryValidation : AbstractValidator<GetVideoUrlQuery> {
        public GetVideoUrlQueryValidation(IVideoService videoService) {
            RuleFor(q => q.VideoFileId)
                .NotEmpty()
                .WithMessage("Video doesn't exists");
        }
    }
}
