using AutoMapper;
using CoreCodeCamp.Data;
using CoreCodeCamp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreCodeCamp.Controllers
{
    [Route("api/[controller]")]
    public class CampsController : ControllerBase
    {
        private readonly ICampRepository repository;
        private readonly IMapper mapper;

        public CampsController(ICampRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<CampModel[]>> Get(bool includeTalks = false)
        {
            try
            {
                var results = await repository.GetAllCampsAsync(includeTalks);

                // I want to create camp model from our results
                return mapper.Map<CampModel[]>(results);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure:"+ex.Message);
            }
        }

        [HttpGet("{moniker}")]
        public async Task<ActionResult<CampModel>> Get(string moniker)
        {
            try
            {
                var result = await repository.GetCampAsync(moniker);

                if(result == null)
                {
                    return NotFound();
                }

                return mapper.Map<CampModel>(result);
            }
            catch (Exception ex)
            {

                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure:" + ex.Message);
            }
        }

        [HttpGet("search")]
        public async Task<ActionResult<CampModel[]>> SearchByDate(DateTime theDate, bool includeTalks = false)
        {
            try
            {
                var results = await repository.GetAllCampsByEventDate(theDate, includeTalks);

                if (!results.Any()) return NotFound();

                return mapper.Map<CampModel[]>(results);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure:" + ex.Message);
            }
        }
    }
}
