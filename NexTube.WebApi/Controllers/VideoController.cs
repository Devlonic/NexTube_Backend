﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NexTube.Application.CQRS.Files.Videos.Commands.RemoveVideoByEntityId;
using NexTube.Application.CQRS.Files.Videos.Commands.UploadVideo;
using NexTube.Application.CQRS.Files.Videos.Queries.GetAllVideoEntities;
using NexTube.Application.CQRS.Files.Videos.Queries.GetVideoEntity;
using NexTube.Application.CQRS.Files.Videos.Queries.GetVideoUrl;
using NexTube.Application.CQRS.Videos.Commands.AddComment;
using NexTube.Application.CQRS.Videos.Queries.GetCommentsList;
using NexTube.WebApi.DTO.Files.Video;
using WebShop.Domain.Constants;
using NexTube.WebApi.DTO.Videos;
using NexTube.Application.CQRS.Videos.Commands.DeleteComment;

namespace NexTube.WebApi.Controllers
{
    public class VideoController : BaseController
    {
        private readonly IMapper mapper;

        public VideoController(IMapper mapper)
        {
            this.mapper = mapper;
        }

        [HttpGet("{videoId}")]
        public async Task<ActionResult> GetVideoUrl(string videoId)
        {
            var getVideoDto = new GetVideoUrlDto()
            {
                VideoUrl = videoId,
            };

            var query = mapper.Map<GetVideoUrlQuery>(getVideoDto);
            var getVideoUrlVm = await Mediator.Send(query);

            return Redirect(getVideoUrlVm.VideoUrl);
        }

        [HttpGet("{videoEntityId}")]
        public async Task<ActionResult> GetVideoEntity(int videoEntityId)
        {
            var getVideoEntityDto = new GetVideoEntityDto()
            {
                VideoEntityId = videoEntityId,
            };

            var query = mapper.Map<GetVideoEntityQuery>(getVideoEntityDto);
            var getVideoEntityVm = await Mediator.Send(query);

            return Ok(getVideoEntityVm);
        }

        [HttpGet]
        public async Task<ActionResult> GetAllVideoEntites()
        {
            var query = new GetAllVideoEntitiesQuery();
            var getVideoEntityVm = await Mediator.Send(query);

            return Ok(getVideoEntityVm);
        }

        [HttpPost]
        [Authorize(Roles = Roles.User)]
        public async Task<ActionResult> UploadVideo([FromForm] UploadVideoDto dto)
        {
            await EnsureCurrentUserAssignedAsync();

            var command = mapper.Map<UploadVideoCommand>(dto);
            command.Creator = CurrentUser;
            var videoId = await Mediator.Send(command);

            return Ok(videoId);
        }

        [HttpDelete("{videoEntityId}")]
        public async Task<ActionResult> RemoveVideoByEntityId(int videoEntityId)
        {
            var removeVideoByEntityIdDto = new RemoveVideoByEntityIdDto()
            {
                VideoEntityId = videoEntityId,
            };

            var command = mapper.Map<RemoveVideoByEntityIdCommand>(removeVideoByEntityIdDto);
            await Mediator.Send(command);
            
            return Ok();
        }

        [HttpPost]
        [Authorize(Roles = Roles.User)]
        public async Task<ActionResult> AddComment([FromForm] AddCommentDto dto) {
            await EnsureCurrentUserAssignedAsync();
            
            var command = mapper.Map<AddCommentCommand>(dto);
            command.Creator = CurrentUser;
            await Mediator.Send(command);

            return Ok();
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
