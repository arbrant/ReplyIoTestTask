using BusinessLogic.Models;
using BusinessLogic.Services;
using DataAccess.Constants;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class ContactsController : ControllerBase
{
    private readonly ContactService contactService;

    public ContactsController(ContactService contactPaginationService)
    {
        this.contactService = contactPaginationService;
    }

    [HttpGet]
    public async Task<ActionResult<ContactsPaginationResult>> GetContacts(
        int page = 1,
        int pageSize = 10,
        string sortBy = "CreatedAt",
        bool ascending = true,
        string? search = null)
    {
        if (!Enum.TryParse<ContactSortBy>(sortBy, true, out var sortByEnum))
        {
            return BadRequest($"Invalid sortBy value. Valid values: {string.Join(", ", Enum.GetNames<ContactSortBy>())}");
        }

        return await this.contactService.GetContactsAsync(page, pageSize, sortByEnum, ascending, search);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetContact(int id)
    {
        var contact = await this.contactService.GetContactAsync(id);
        if (contact == null) return NotFound();
        return Ok(contact);
    }

    [HttpPost]
    public async Task<IActionResult> CreateContact([FromBody] CreateContactDto createContactDto)
    {
        var contact = await this.contactService.CreateContactAsync(createContactDto);

        return CreatedAtAction(nameof(GetContact), new { id = contact.Id }, contact);
    }
    [HttpPut]
    public async Task<IActionResult> UpdateContact([FromBody] UpdateContactDto updateContactDto)
    {
        var successfullyUpdated = await this.contactService.UpdateContractAsync(updateContactDto);

        return successfullyUpdated ? NoContent() : NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteContact(int id)
    {
        var successfullyDeleted = await this.contactService.DeleteContactAsync(id);
        return successfullyDeleted ? NoContent() : NotFound();
    }
}
