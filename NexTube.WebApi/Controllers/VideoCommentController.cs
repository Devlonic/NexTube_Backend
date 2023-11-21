﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NexTube.Application.CQRS.Files.Videos.Commands.RemoveVideoByEntityId;
using NexTube.Application.CQRS.Files.Videos.Commands.UploadVideo;
using NexTube.Application.CQRS.Files.Videos.Queries.GetAllVideoEntities;
using NexTube.Application.CQRS.Files.Videos.Queries.GetVideoEntity;
using NexTube.Application.CQRS.Files.Videos.Queries.GetVideoUrl;
using NexTube.WebApi.DTO.Files.Video;
using WebShop.Domain.Constants;
using NexTube.Application.CQRS.Comments.VideoComments.Queries.GetCommentsList;
using NexTube.Application.CQRS.Comments.VideoComments.Commands.AddComment;
using NexTube.Application.CQRS.Comments.VideoComments.Commands.DeleteComment;
using NexTube.WebApi.DTO.Comments.VideoComments;
using NexTube.Application.CQRS.Comments.VideoComments.Commands.AddCommentReply;

namespace NexTube.WebApi.Controllers {
    [Route("api/Video/Comment/[action]")]
    public class VideoCommentController : BaseController {
        private readonly IMapper mapper;

        public VideoCommentController(IMapper mapper) {
            this.mapper = mapper;
        }

        [HttpPost]
        [Authorize(Roles = Roles.User)]
        public async Task<ActionResult> AddComment([FromBody] AddCommentDto dto) {
            await EnsureCurrentUserAssignedAsync();

            var command = mapper.Map<AddCommentCommand>(dto);
            command.Creator = CurrentUser;
            var result = await Mediator.Send(command);

            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = Roles.User)]
        public async Task<ActionResult> AddCommentReply([FromBody] AddCommentReplyDto dto) {
            await EnsureCurrentUserAssignedAsync();

            var command = mapper.Map<AddCommentReplyCommand>(dto);
            command.Creator = CurrentUser;
            var result = await Mediator.Send(command);

            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult> GetCommentsList([FromQuery] GetCommentsListDto dto) {
            var query = mapper.Map<GetCommentsListQuery>(dto);
            var result = await Mediator.Send(query);
            return Ok(result);
        }

        [Authorize(Roles = Roles.User, Policy = Policies.CanDeleteOwnComment)]
        [HttpDelete]
        public async Task<ActionResult> DeleteComment([FromQuery] DeleteCommentDto dto) {
            var command = mapper.Map<DeleteCommentCommand>(dto);
            await Mediator.Send(command);

            return Ok();
        }
    }
}
