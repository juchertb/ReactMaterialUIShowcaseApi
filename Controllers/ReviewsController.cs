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
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewRepository _repository;
        private readonly IMapper _mapper;

        public ReviewsController(IReviewRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        // GET: api/reviews?filter=...&sort=...&range=...
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReviewDto>>> GetAll([FromQuery] ListQueryParameters parameters)
        {
            var parsed = ListQueryParser.Parse(parameters);

            var (items, total) = await _repository.QueryAsync(parsed);

            Response.Headers.Append(
                "Content-Range",
                $"reviews {parsed.RangeStart}-{parsed.RangeEnd}/{total}"
            );

            return Ok(_mapper.Map<IEnumerable<ReviewDto>>(items));
        }

        // GET: api/reviews/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ReviewDto>> GetById(int id)
        {
            var entity = await _repository.GetReviewWithDetailsAsync(id);

            if (entity == null)
                return NotFound();

            return Ok(_mapper.Map<ReviewDto>(entity));
        }

        // POST: api/reviews
        [HttpPost]
        public async Task<ActionResult<ReviewDto>> Create(ReviewDto dto)
        {
            var entity = _mapper.Map<Entities.Review>(dto);

            await _repository.AddAsync(entity);
            await _repository.SaveChangesAsync();

            var resultDto = _mapper.Map<ReviewDto>(entity);

            return CreatedAtAction(nameof(GetById), new { id = entity.Id }, resultDto);
        }

        // PUT: api/reviews/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, ReviewDto dto)
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

        // DELETE: api/reviews/5
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