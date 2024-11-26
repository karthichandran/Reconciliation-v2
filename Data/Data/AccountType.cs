using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data
{
    public class AccountType
    {
        [Key]
        public int AccountTypeId { get; set; }
        public string AccountTypeName { get; set; }
        [NotMapped]
        public string SearchTerms { get; set; }
        public string BuildSearchTerms() => $"{AccountTypeName}".ToLower();

    }
}
