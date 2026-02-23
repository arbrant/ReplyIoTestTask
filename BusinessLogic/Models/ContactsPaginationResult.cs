using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Models
{
    public class ContactsPaginationResult
    {
        public int Total { get; set; }

        public int TotalPages { get; set; }

        public int CurrentPage { get; set; }

        public List<Contact> Contacts { get; set; }
    }
}
