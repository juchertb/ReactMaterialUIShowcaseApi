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
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;

        public ProductsController(IProductRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        // GET api/products?filter=...&sort=...&range=...
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetAll([FromQuery] ListQueryParameters parameters)
        {
            var parsed = ListQueryParser.Parse(parameters);

            var (items, total) = await _repository.QueryAsync(parsed);

            // Set MSW/React-Admin compatible Content-Range header
            Response.Headers.Append(
                "Content-Range",
                $"products {parsed.RangeStart}-{parsed.RangeEnd}/{total}"
            );

            return Ok(_mapper.Map<IEnumerable<ProductDto>>(items));
        }

        // GET api/products/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductDto>> GetById(int id)
        {
            var product = await _repository.GetProductWithDetailsAsync(id);

            if (product == null)
                return NotFound();

            return Ok(_mapper.Map<ProductDto>(product));
        }

        // POST api/products
        [HttpPost]
        public async Task<ActionResult<ProductDto>> Create(ProductDto dto)
        {
            var entity = _mapper.Map<Entities.Product>(dto);

            await _repository.AddAsync(entity);
            await _repository.SaveChangesAsync();

            var resultDto = _mapper.Map<ProductDto>(entity);

            return CreatedAtAction(nameof(GetById), new { id = entity.Id }, resultDto);
        }

        // PUT api/products/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, ProductDto dto)
        {
            if (id != dto.Id)
                return BadRequest();

            var entity = await _repository.GetProductWithDetailsAsync(id);

            if (entity == null)
                return NotFound();

            _mapper.Map(dto, entity);

            _repository.Update(entity);
            await _repository.SaveChangesAsync();

            return NoContent();
        }

        // DELETE api/products/5
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
