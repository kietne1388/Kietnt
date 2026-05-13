using Microsoft.AspNetCore.Mvc;
using FastFood.Application.Interfaces;
using FastFood.Domain.Entities;

namespace FastFood.API.Controllers
{
    [ApiController]
    [Route("api/contact")]
    public class ContactApiController : ControllerBase
    {
        private readonly IContactService _contactService;

        public ContactApiController(IContactService contactService)
        {
            _contactService = contactService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var contacts = await _contactService.GetAllContactsAsync();
            return Ok(contacts);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var contact = await _contactService.GetContactByIdAsync(id);
            if (contact == null) return NotFound();
            return Ok(contact);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateContactRequest request)
        {
            var contact = await _contactService.CreateContactAsync(
                request.Name,
                request.Email,
                request.Subject,
                request.Message
            );
            return Ok(contact);
        }

        [HttpPut("{id}/read")]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            var success = await _contactService.MarkAsReadAsync(id);
            if (!success) return NotFound();
            return Ok(new { message = "Contact marked as read" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _contactService.DeleteContactAsync(id);
            if (!success) return NotFound();
            return Ok(new { message = "Contact deleted successfully" });
        }
    }

    public record CreateContactRequest(string Name, string Email, string Subject, string Message);
}
