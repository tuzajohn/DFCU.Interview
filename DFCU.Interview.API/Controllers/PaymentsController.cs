using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Contracts;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DFCU.Interview.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {

            return Ok(new string[] { "value1", "value2" });
        }

        [HttpGet("{id:guid}")]
        public IActionResult Get(Guid? id)
        {
            if (id is null || id == Guid.Empty)
            {
                return NotFound();
            }

            return Ok("value");
        }

        [HttpPost]
        public IActionResult Post([FromBody] string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return BadRequest("Value cannot be null or empty.");
            }

            return CreatedAtAction(nameof(Get), new { id = 1 }, value);
        }

        [HttpPut("{id:guid}")]
        public IActionResult Put(Guid? id, [FromBody] string value)
        {
            Contract.Requires(id is not null, "Id cannot be null.");
            Contract.Requires(id != Guid.Empty, "Id cannot be empty.");
            Contract.Requires(!string.IsNullOrEmpty(value), "Value cannot be null or empty.");



            if (string.IsNullOrEmpty(value))
            {
                return BadRequest("Value cannot be null or empty.");
            }

            return NoContent();
        }

        // DELETE api/<PaymentsController>/5
        [HttpDelete("{id:guid}")]
        public IActionResult Delete(Guid? id)
        {
            if (id is null || id == Guid.Empty)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
