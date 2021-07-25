using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Activities;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistance;

namespace API.Controllers
{
    public class ActivitiesController : BaseApiController
    {
        private readonly IMediator _mediator;
        public ActivitiesController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpGet]
        public async Task<IActionResult> GetActivities(/*CancellationToken ct*/) // Yesli client delayet cancel request
        {
            return HandleResult(await Mediator.Send(new List.Query()));
            // return await Mediator.Send(new List.Query() /*ct*/);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetActivity(Guid id)
        {

            var result = await Mediator.Send(new Details.Query { Id = id });
        
            return HandleResult(result);
            
            // Kod nije priveden yesli bil ne bilo patterna CQRS
            // var activity =  await Mediator.Send(new Details.Query { Id = id });

            // if (activity == null)
            // {
            //     return NotFound();
            // }

            // return activity;
        }

        [HttpPost]
        public async Task<IActionResult> CreateActivity([FromBody] Activity activity)
        {
            return HandleResult(await Mediator.Send(new Create.Command { Activity = activity }));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditActivity(Guid id, [FromBody] Activity activity)
        {
            activity.Id = id;
            return  HandleResult(await Mediator.Send(new Edit.Command { Activity = activity }));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActivity(Guid id)
        {
            return HandleResult(await Mediator.Send(new Delete.Command { Id = id }));
        }
    }
}