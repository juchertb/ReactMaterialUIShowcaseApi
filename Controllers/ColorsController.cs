using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using ReactMaterialUIShowcaseApi.Dtos;
using ReactMaterialUIShowcaseApi.Services.Query;
using ReactMaterialUIShowcaseApi.Interfaces;

namespace ReactMaterialUIShowcaseApi.Controllers
{
    [ApiController]
    //[Authorize]
    [Route("api/[controller]")]
    public class ColorsController : ControllerBase
    {
        private readonly IColorRepository _repository;
        private readonly IMapper _mapper;

        public ColorsController(IColorRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        // GET: api/colors?filter=...&sort=...&range=...
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ColorDto>>> GetAll([FromQuery] ListQueryParameters parameters)
        {
            var parsed = ListQueryParser.Parse(parameters);

            var (items, total) = await _repository.QueryAsync(parsed);

            Response.Headers.Append(
                "Content-Range",
                $"colors {parsed.RangeStart}-{parsed.RangeEnd}/{total}"
            );

            return Ok(_mapper.Map<IEnumerable<ColorDto>>(items));
        }

        // GET: api/colors/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ColorDto>> GetById(int id)
        {
            var entity = await _repository.GetByIdAsync(id);

            if (entity == null)
                return NotFound();

            return Ok(_mapper.Map<ColorDto>(entity));
        }

        // POST: api/colors
        [HttpPost]
        public async Task<ActionResult<ColorDto>> Create(ColorDto dto)
        {
            var entity = _mapper.Map<Entities.Color>(dto);

            await _repository.AddAsync(entity);
            await _repository.SaveChangesAsync();

            var resultDto = _mapper.Map<ColorDto>(entity);

            return CreatedAtAction(nameof(GetById), new { id = entity.Id }, resultDto);
        }

        // PUT: api/colors/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, ColorDto dto)
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

        // DELETE: api/colors/5
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