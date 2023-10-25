﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NexTube.Application.CQRS.Files.Videos.Commands.UploadVideo;
using NexTube.Application.CQRS.Files.Videos.Queries.GetVideoUrl;
using NexTube.WebApi.DTO.Files.Video;

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

        [HttpPost]
        public async Task<ActionResult> UploadVideo([FromForm] UploadVideoDto dto)
        {
            var command = mapper.Map<UploadVideoCommand>(dto);
            var videoId = await Mediator.Send(command);

            return Ok(videoId);
        }
    }
}
