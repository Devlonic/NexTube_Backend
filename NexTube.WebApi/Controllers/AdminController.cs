﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NexTube.Application.CQRS.Files.Videos.Commands.RemoveVideoByEntityId;
using NexTube.Application.CQRS.Files.Videos.Queries.GetAllVideoEntities;
using NexTube.Application.CQRS.Identity.Reports.Commands;
using NexTube.Application.CQRS.Identity.Reports.Queries;
using NexTube.Application.CQRS.Identity.Users.Commands.BanUser;
using NexTube.Application.CQRS.Identity.Users.Queries;
using NexTube.WebApi.DTO.Admin;
using NexTube.WebApi.DTO.Auth.User;
using NexTube.WebApi.DTO.Files.Video;
using WebShop.Domain.Constants;

namespace NexTube.WebApi.Controllers
{
    public class AdminController : BaseController
    {
        private readonly IMapper mapper;

        public AdminController(IMapper mapper)
        {
            this.mapper = mapper;
        }

        [Authorize(Roles =  Roles.Administrator+"," + Roles.Moderator)]
        [HttpGet]
        public async Task<ActionResult> GetAllUsers(int page, int pageSize)
        {
            var query = new GetAllUsersQuery() { Page= page,PageSize = pageSize};
            var getAllUsersQueryResult = await Mediator.Send(query);

            return Ok(getAllUsersQueryResult);
        }
        [Authorize(Roles = Roles.Administrator + "," + Roles.Moderator)]
        [HttpPost]
        public async Task<ActionResult> BanUser([FromBody] BanUserDto dto)
        {
            var command = mapper.Map<BanUserCommand>(dto);
            var result = await Mediator.Send(command);
            if (result.Result.Succeeded == false)
                return UnprocessableEntity(result);

            return Ok(result);
        }

        [Authorize(Roles = Roles.User)]
        [HttpPost]
        public async Task<ActionResult> ReportUser([FromForm] ReportUserDto dto)
        {
            var command = mapper.Map<ReportUserCommand>(dto);
            var result = await Mediator.Send(command);
            if (result.Succeeded == false)
                return UnprocessableEntity(result);

            return Ok(result);
        }

        [Authorize(Roles = Roles.Administrator + "," + Roles.Moderator)]
        [HttpGet]
        public async Task<ActionResult> GetAllReports(int page,int pageSize)
        {
            var query = new GetAllReportsQuery() { Page = page,PageSize = pageSize};
            var GetAllReportsQuery = await Mediator.Send(query);

            return Ok(GetAllReportsQuery);
        }

        [Authorize(Roles = Roles.Administrator + "," + Roles.Moderator)]
        [HttpGet]
        public async Task<ActionResult> GetAllReportsFromUser(int userId,int page, int pageSize)
        {
            var query = new GetAllReportsFromUserQuery() { Page = page, PageSize = pageSize , UserId = userId };
            var GetAllReportsFromUserQuery = await Mediator.Send(query);

            return Ok(GetAllReportsFromUserQuery);
        }

        [HttpDelete("{reportEntityId}")]
        [Authorize(Roles = Roles.Administrator + "," + Roles.Moderator)]
        public async Task<ActionResult> RemoveReportByEntityId(int reportEntityId)
        {
            var command = new RemoveReportByEntityIdCommand() { ReportId = reportEntityId };
            await Mediator.Send(command);

            return Ok();
        }


    }
}
