using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace DataAccess.Repository
{
    public class ContactRepository
    {
        private ContactsDbContext _context;

        public ContactRepository(ContactsDbContext context)
        {
            _context = context;
        }

        public async Task<Contact?> GetContactWithCustomFieldsAsync(int contactId)
        {
            return await _context.Contacts
                .Include(c => c.CustomValues)
                .FirstOrDefaultAsync(c => c.Id == contactId);
        }

        public IQueryable<Contact> GetContactsQueriable() => _context.Contacts.Include(c => c.CustomValues).AsQueryable();

        public async Task AddContact(Contact contact)
        {
            await this._context.Contacts.AddAsync(contact);
        }

        public async Task RemoveContact(Contact contact)
        {
            this._context.Remove(contact);
        }

        public async Task SaveChanges()
        {
            await this._context.SaveChangesAsync();
        }
    }
}
