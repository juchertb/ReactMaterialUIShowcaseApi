using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using ReactMaterialUIShowcaseApi.Dtos;
using ReactMaterialUIShowcaseApi.Services.Query;
using ReactMaterialUIShowcaseApi.Interfaces;
using Microsoft.AspNetCore.Authorization;
using ReactMaterialUIShowcaseApi.Helpers;
using ReactMaterialUIShowcaseApi.Data;

namespace ReactMaterialUIShowcaseApi.Controllers
{
    //[Authorize]
    [ApiController]
    //[SetUserCulture] // controls localization based on language selected at logon
    [Route("api/customerOrders")] // Now api/orders
    public class OrdersController : ControllerBase
    {
        private readonly IOrderRepository _repository;
        private readonly IMapper _mapper;
        private readonly ICustomerRepository _customerRepository;

        public OrdersController(IOrderRepository repository, IMapper mapper, ICustomerRepository customerRepository)
        {
            _repository = repository;
            _mapper = mapper;
            _customerRepository = customerRepository;
        }

        // GET: api/orders?filter=...&sort=...&range=...
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetAll([FromQuery] ListQueryParameters parameters)
        {
            var parsed = ListQueryParser.Parse(parameters);

            var (items, total) = await _repository.QueryAsync(parsed);

            Response.Headers.Append(
                "Content-Range",
                $"orders {parsed.RangeStart}-{parsed.RangeEnd}/{total}"
            );

            return Ok(_mapper.Map<IEnumerable<OrderDto>>(items));
        }

        // GET: api/orders/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<OrderDto>> GetById(int id)
        {
            var entity = await _repository.GetOrderWithDetailsAsync(id);

            if (entity == null)
                return NotFound();

            return Ok(_mapper.Map<OrderDto>(entity));
        }

        // POST: api/orders
        [HttpPost]
        public async Task<ActionResult<OrderDto>> Create(OrderDto dto)
        {
            //return NoContent();
            if (dto == null) return NoContent();
            int result = 0;
            if (!int.TryParse(dto.Id, out result)) return NoContent();
            dto.Id = "0";

            // Hack to get the last customer id inserted since the order browser has multiple inserts that rely on each other.
            // The order relies on the new customer that was just inserted.
            dto.customer_id = await _customerRepository.GetMaxId();
            // This ensures that entity framework doesn't try to insert a new customer
            dto.Customer = null!;

            var entity = _mapper.Map<Entities.Order>(dto);

            await _repository.AddAsync(entity);
            await _repository.SaveChangesAsync();

            var resultDto = _mapper.Map<OrderDto>(entity);

            return CreatedAtAction(nameof(GetById), new { id = entity.Id }, resultDto);
        }

        // PUT: api/orders/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, OrderDto dto)
        {
            if (id.ToString() != dto.Id)
                return BadRequest("ID mismatch.");

            var entity = await _repository.GetByIdAsync(id);

            if (entity == null)
                return NotFound();

            _mapper.Map(dto, entity);

            _repository.Update(entity);
            await _repository.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/orders/5
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