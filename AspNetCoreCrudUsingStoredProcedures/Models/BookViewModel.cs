using System.ComponentModel.DataAnnotations;

namespace AspNetCoreCrudUsingStoredProcedures.Models
{
    public class BookViewModel
    {
        [Key]
        public int bookId { get; set; }
        public string title { get; set; }
        public string author { get; set; }
        public int price { get; set; }
    }
}
