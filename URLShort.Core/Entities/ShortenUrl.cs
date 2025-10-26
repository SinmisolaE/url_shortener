using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace URLShort.Core
{

    [Index(nameof(LongURL), IsUnique = true)]
    [Index(nameof(ShortURL), IsUnique = true)]
    public class ShortenUrl
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "VARCHAR(768)")]
        [MaxLength(768)]
        public string LongURL { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "VARCHAR(100)")]
        [MaxLength(100)]
        public string ShortURL { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public int Count { get; set; }


        public ShortenUrl(string LongURL)
        {    
            this.LongURL = LongURL;
        }

    }
}