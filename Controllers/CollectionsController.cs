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
    public class CollectionsController : ControllerBase
    {
        private readonly ICollectionRepository _repository;
        private readonly IMapper _mapper;

        public CollectionsController(ICollectionRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        // GET: api/collections?filter=...&sort=...&range=...
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CollectionDto>>> GetAll([FromQuery] ListQueryParameters parameters)
        {
            var parsed = ListQueryParser.Parse(parameters);

            var (items, total) = await _repository.QueryAsync(parsed);

            Response.Headers.Append(
                "Content-Range",
                $"collections {parsed.RangeStart}-{parsed.RangeEnd}/{total}"
            );

            return Ok(_mapper.Map<IEnumerable<CollectionDto>>(items));
        }

        // GET: api/collections/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<CollectionDto>> GetById(int id)
        {
            var entity = await _repository.GetByIdAsync(id);

            if (entity == null)
                return NotFound();

            return Ok(_mapper.Map<CollectionDto>(entity));
        }

        // POST: api/collections
        [HttpPost]
        public async Task<ActionResult<CollectionDto>> Create(CollectionDto dto)
        {
            var entity = _mapper.Map<Entities.Collection>(dto);

            await _repository.AddAsync(entity);
            await _repository.SaveChangesAsync();

            var resultDto = _mapper.Map<CollectionDto>(entity);

            return CreatedAtAction(nameof(GetById), new { id = entity.Id }, resultDto);
        }

        // PUT: api/collections/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, CollectionDto dto)
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

        // DELETE: api/collections/5
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