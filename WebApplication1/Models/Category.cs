using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;


namespace WebApplication1.Models
{
    public class Category
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MinLength(2, ErrorMessage = "To short name")]
        [MaxLength(20, ErrorMessage = " To long name, do not exceed {1}")]
        public string Name { get; set; }

        [JsonIgnore]
        public ICollection<Article> Articles { get; set; }

        public Category() 
        {
            if (Articles == null)
                Articles = new List<Article>();
        }
    }

}
