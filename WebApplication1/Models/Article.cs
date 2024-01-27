using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace WebApplication1.Models
{
    public class Article
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [MinLength(2, ErrorMessage = "To short name")]
        [MaxLength(20, ErrorMessage = " To long name, do not exceed {1}")]
        public string Name { get; set; }
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Only positive number allowed")]
        [Column(TypeName = "decimal(18,2)")]
        public double Price { get; set; }
        [AllowNull]
        [DisplayName ("Photo")]
        public string ImagePath { get; set; }
        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is Article article &&
                  Id == article.Id;
        }

        public override string ToString()
        {
            return $"Article ID: {Id}, Name: {Name}, Price: {Price}, Category: {Category.Name}";
        }

        public Article()
        {
            Category = new Category();
        }

        public Article(int id, string name, double price, string image, Category category)
        {
            Id = id;
            Name = name;
            Price = price;
            ImagePath = image;
            Category = category;
        }
    }
}
