using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    public class ContactCustomField
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey(nameof(Contact))]
        public int ContactId { get; set; }

        [Required]
        public string FieldName { get; set; } = null!;

        [Required]
        public string DataType { get; set; } = null!;

        [Required]
        public string Value { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Contact Contact { get; set; } = new();
    }
}
