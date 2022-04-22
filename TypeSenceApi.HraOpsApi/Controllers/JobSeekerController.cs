using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TypeSenceApi.HraOpsApi.Repository;

namespace TypeSenceApi.HraOpsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobSeekerController : ControllerBase
    {
        ITypeSenseRepository _iTypeSenseRepository;
        public JobSeekerController(ITypeSenseRepository iTypeSenseRepository)
        {
            _iTypeSenseRepository = iTypeSenseRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get(long rid)
        {
            return Ok(await _iTypeSenseRepository.Get(rid));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateInsert(long rid)
        {
            return Ok(await _iTypeSenseRepository.UpdateInsert(rid));
        }
    }
}
