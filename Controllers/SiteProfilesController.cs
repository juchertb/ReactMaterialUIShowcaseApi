using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using ReactMaterialUIShowcaseApi.Dtos;
using ReactMaterialUIShowcaseApi.Services.Query;
using ReactMaterialUIShowcaseApi.Interfaces;
using ReactMaterialUIShowcaseApi.Entities;

namespace ReactMaterialUIShowcaseApi.Controllers
{
    [ApiController]
    //[Authorize]
    [Route("api/[controller]")]
    public class SiteProfileController : ControllerBase
    {
        private readonly ISiteProfileRepository _repository;
        private readonly IMapper _mapper;

        public SiteProfileController(ISiteProfileRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        // GET: api/siteprofiles?filter=...&sort=...&range=...
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SiteProfileDto>>> GetAll([FromQuery] ListQueryParameters parameters)
        {
            var parsed = ListQueryParser.Parse(parameters);

            var (items, total) = await _repository.QueryAsync(parsed);

            Response.Headers.Append(
                "Content-Range",
                $"siteprofiles {parsed.RangeStart}-{parsed.RangeEnd}/{total}"
            );

            return Ok(_mapper.Map<IEnumerable<SiteProfileDto>>(items));
        }

        // GET: api/siteprofiles/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<SiteProfileDto>> GetById(int id)
        {
            var entity = await _repository.GetByIdAsync(id);

            if (entity == null)
                return NotFound();

            return Ok(_mapper.Map<SiteProfileDto>(entity));
        }

        // POST: api/siteprofiles
        [HttpPost]
        public async Task<ActionResult<SiteProfileDto>> Create(SiteProfileDto dto)
        {
            var entity = _mapper.Map<Entities.SiteProfile>(dto);

            await _repository.AddAsync(entity);
            await _repository.SaveChangesAsync();

            var resultDto = _mapper.Map<SiteProfileDto>(entity);

            return CreatedAtAction(nameof(GetById), new { id = entity.Id }, resultDto);
        }

        // PUT: api/siteprofiles/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, SiteProfileDto dto)
        {
            if (id != dto.Id)
                return BadRequest("ID mismatch.");

            var entity = await _repository.GetSiteProfileWithDetailsAsync(id);

            if (entity == null)
                return NotFound();

             _repository.Update(entity, dto);
            await _repository.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/siteprofiles/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _repository.GetByIdAsync(id);

            if (entity == null)
                return NotFound();

            _repository.Delete(entity);
            await _repository.SaveChangesAsync();

            return NoContent();
        }
    }
}