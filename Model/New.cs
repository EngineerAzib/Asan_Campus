using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Asan_Campus.Model
{
    public class New
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        [Column(TypeName = "NVARCHAR(MAX)")]
        public string ImageUrl { get; set; }

      
        public DateOnly PublishedDate { get; set; }

        public TimeOnly PublishedTime { get; set; }



        public bool _isActive {  get; set; }

        public int? CategoryId { get; set; }  // Changed to CategoryId for clarity

        [ForeignKey("CategoryId")]
        public Category Category { get; set; }  // Navigation property


    }
}
