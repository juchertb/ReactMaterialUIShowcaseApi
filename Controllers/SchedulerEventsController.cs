using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using ReactMaterialUIShowcaseApi.Dtos;
using ReactMaterialUIShowcaseApi.Interfaces;
using ReactMaterialUIShowcaseApi.Services.Query;

namespace ReactMaterialUIShowcaseApi.Controllers
{
    [ApiController]
    //[Authorize]
    [Route("api/[controller]")]
    public class SchedulerEventsController : ControllerBase
    {
        private readonly ISchedulerEventRepository _repository;
        private readonly IMapper _mapper;

        public SchedulerEventsController(ISchedulerEventRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        // GET: api/schedulerevents?filter=...&sort=...&range=...
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SchedulerEventDto>>> GetAll([FromQuery] ListQueryParameters parameters)
        {
            var parsed = ListQueryParser.Parse(parameters);

            var (items, total) = await _repository.QueryAsync(parsed);
            //throw new InvalidDataException("Test exception for error handling");

            Response.Headers.Append(
                "Content-Range",
                $"schedulerevents {parsed.RangeStart}-{parsed.RangeEnd}/{total}"
            );

            return Ok(_mapper.Map<IEnumerable<SchedulerEventDto>>(items));
        }

        // GET: api/schedulerevents/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<SchedulerEventDto>> GetById(int id)
        {
            var entity = await _repository.GetByIdAsync(id);

            if (entity == null)
                return NotFound();

            return Ok(_mapper.Map<SchedulerEventDto>(entity));
        }

        // POST: api/schedulerevents
        [HttpPost]
        public async Task<ActionResult<SchedulerEventDto>> Create(SchedulerEventDto dto)
        {
            // Make sure we don't have an Event ID when inserting. And clear the category oterwise EF will try to insert a new category.
            dto.Id = 0;
            dto.CategoryId = dto.Category?.Id ?? dto.CategoryId;
            dto.Category = null;

            var entity = _mapper.Map<Entities.SchedulerEvent>(dto);

            await _repository.AddAsync(entity);
            await _repository.SaveChangesAsync();

            var resultDto = _mapper.Map<SchedulerEventDto>(entity);

            return CreatedAtAction(nameof(GetById), new { id = entity.Id }, resultDto);
        }

        // PUT: api/schedulerevents/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, SchedulerEventDto dto)
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

        // DELETE: api/schedulerevents/5
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