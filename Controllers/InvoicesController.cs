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
    public class InvoicesController : ControllerBase
    {
        private readonly IInvoiceRepository _repository;
        private readonly IMapper _mapper;

        public InvoicesController(IInvoiceRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        // GET: api/invoices?filter=...&sort=...&range=...
        [HttpGet]
        public async Task<ActionResult<IEnumerable<InvoiceDto>>> GetAll([FromQuery] ListQueryParameters parameters)
        {
            var parsed = ListQueryParser.Parse(parameters);

            var (items, total) = await _repository.QueryAsync(parsed);

            Response.Headers.Append(
                "Content-Range",
                $"invoices {parsed.RangeStart}-{parsed.RangeEnd}/{total}"
            );

            return Ok(_mapper.Map<IEnumerable<InvoiceDto>>(items));
        }

        // GET: api/invoices/ABC123
        [HttpGet("{id}")]
        public async Task<ActionResult<InvoiceDto>> GetById(string id)
        {
            var entity = await _repository.GetInvoiceWithDetailsAsync(id);

            if (entity == null)
                return NotFound();

            return Ok(_mapper.Map<InvoiceDto>(entity));
        }

        // POST: api/invoices
        [HttpPost]
        public async Task<ActionResult<InvoiceDto>> Create(InvoiceDto dto)
        {
            var entity = _mapper.Map<Entities.Invoice>(dto);

            await _repository.AddAsync(entity);
            await _repository.SaveChangesAsync();

            var resultDto = _mapper.Map<InvoiceDto>(entity);

            return CreatedAtAction(nameof(GetById), new { id = entity.Id }, resultDto);
        }

        // PUT: api/invoices/ABC123
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, InvoiceDto dto)
        {
            if (id != dto.Id)
                return BadRequest("ID mismatch.");

            var entity = await _repository.GetByIdAsync(int.Parse(id));

            if (entity == null)
                return NotFound();

            _mapper.Map(dto, entity);

            _repository.Update(entity);
            await _repository.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/invoices/ABC123
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var entity = await _repository.GetByIdAsync(int.Parse(id));

            if (entity == null)
                return NotFound();

            _repository.Delete(entity);
            await _repository.SaveChangesAsync();

            return NoContent();
        }
    }
}