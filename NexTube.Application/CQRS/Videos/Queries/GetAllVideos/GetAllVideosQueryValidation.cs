using FluentValidation;

namespace NexTube.Application.CQRS.Videos.Queries.GetAllVideos {
    public class GetAllVideosQueryValidation : AbstractValidator<GetAllVideosQuery> {
        public GetAllVideosQueryValidation() {
            RuleFor(c => c.Page)
                .NotEmpty()
                .GreaterThan(0);

            RuleFor(c => c.PageSize)
                .NotEmpty()
                .GreaterThan(0);
        }
    }
}
