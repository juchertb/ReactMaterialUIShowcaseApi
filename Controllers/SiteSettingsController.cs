using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using ReactMaterialUIShowcaseApi.Dtos;
using ReactMaterialUIShowcaseApi.Interfaces;

namespace ReactMaterialUIShowcaseApi.Controllers
{
    [ApiController]
    //[Authorize]
    [Route("api/[controller]")]
    public class SiteSettingsController : ControllerBase
    {
        private readonly ISiteSettingsRepository _repository;
        private readonly IMapper _mapper;

        public SiteSettingsController(ISiteSettingsRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        // GET: api/sitesettings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SiteSettingsDto>>> Get()
        {
            var list = new List<SiteSettingsDto>();
            var entity = await _repository.GetSettingsAsync();

            if (entity == null)
                return NotFound();

            list.Add(_mapper.Map<SiteSettingsDto>(entity));
            return Ok(list);
        }

        // PUT: api/sitesettings/1
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, SiteSettingsDto dto)
        {
            if (id != dto.Id)
                return BadRequest("ID mismatch.");

            var entity = await _repository.GetByIdAsync(id);

            if (entity == null)
                return NotFound();

            _mapper.Map(dto, entity);

            _repository.Update(entity);
            await _repository.SaveChangesAsync();

            return NoContent();
        }
    }
}