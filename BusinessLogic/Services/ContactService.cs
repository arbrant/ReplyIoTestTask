using BusinessLogic.Models;
using DataAccess.Constants;
using DataAccess.Entities;
using DataAccess.Repository;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Services
{
    public class ContactService
    {
        private readonly ContactRepository contactRepository;

        public ContactService(ContactRepository contactRepository)
        {
            this.contactRepository = contactRepository;
        }

        public async Task<ContactsPaginationResult> GetContactsAsync(
            int page,
            int pageSize,
            ContactSortBy sortBy,
            bool ascending,
            string? search)
        {
            var contactsQueriable = this.contactRepository.GetContactsQueriable();

            contactsQueriable = this.Search(contactsQueriable, search);
            contactsQueriable = this.Sort(contactsQueriable, sortBy, ascending);
            
            var total = await contactsQueriable.CountAsync();

            contactsQueriable = this.Paginate(contactsQueriable, page, pageSize);

            var contacts = await contactsQueriable.ToListAsync();

            return new ContactsPaginationResult
            {
                Total = total,
                TotalPages = (int)Math.Ceiling(total / (double)pageSize),
                CurrentPage = page,
                Contacts = contacts
            };
        }

        public async Task<Contact?> GetContactAsync(int id)
        {
            return await this.contactRepository.GetContactWithCustomFieldsAsync(id);
        }

        public async Task<Contact?> CreateContactAsync(CreateContactDto createContactDto)
        {
            var contact = new Contact()
            {
                FirstName = createContactDto.FirstName,
                LastName = createContactDto.LastName,
                Email = createContactDto.Email,
                Phone = createContactDto.Phone,
            };

            await this.contactRepository.AddContact(contact);
            await this.contactRepository.SaveChanges();

            return contact;
        }

        public async Task<bool> DeleteContactAsync(int contactId)
        {
            var contact = await this.GetContactAsync(contactId);

            if (contact == null) return false;

            await this.contactRepository.RemoveContact(contact);
            await this.contactRepository.SaveChanges();

            return true;
        }

        public async Task<bool> UpdateContractAsync(UpdateContactDto updateContractDto)
        {
            var contactToUpdate = await this.GetContactAsync(updateContractDto.Id);

            if (contactToUpdate == null) return false;

            contactToUpdate.FirstName = updateContractDto.FirstName;
            contactToUpdate.LastName = updateContractDto.LastName;
            contactToUpdate.Email = updateContractDto.Email;
            contactToUpdate.Phone = updateContractDto.Phone;
            contactToUpdate.UpdatedAt = DateTime.UtcNow;

            await this.contactRepository.SaveChanges();

            return true;
        }

        private IQueryable<Contact> Search(IQueryable<Contact> contacts, string? search)
        {
            if (string.IsNullOrWhiteSpace(search)) return contacts;

            return contacts.Where(c =>
                    c.FirstName.Contains(search) ||
                    c.LastName.Contains(search) ||
                    c.Email.Contains(search));
        }

        private IQueryable<Contact> Sort(IQueryable<Contact> contacts, ContactSortBy sortBy, bool ascending)
        {
            return sortBy switch
            {
                ContactSortBy.FirstName => ascending ? contacts.OrderBy(c => c.FirstName) : contacts.OrderByDescending(c => c.FirstName),
                ContactSortBy.LastName => ascending ? contacts.OrderBy(c => c.LastName) : contacts.OrderByDescending(c => c.LastName),
                ContactSortBy.Email => ascending ? contacts.OrderBy(c => c.Email) : contacts.OrderByDescending(c => c.Email),
                _ => ascending ? contacts.OrderBy(c => c.CreatedAt) : contacts.OrderByDescending(c => c.CreatedAt)
            };
        }

        private IQueryable<Contact> Paginate(IQueryable<Contact> contacts, int page, int pageSize)
        {
            return contacts.Skip((page - 1) * pageSize).Take(pageSize);
        } 
    }
}
