using System.ComponentModel.DataAnnotations;

namespace BloggingTool.Models {
    public class ArticleLike {
        [Key]
        public int LikeId { get; set; }

         [Required]
        public bool IsLiked { get; set; } = true; // Default value set to true (which corresponds to 1 in your previous setup)

        [Required]
        public int ArticleId { get; set; }

        [Required]
        public int EmailAccountId { get; set; }

        [Required]
        public required Article Article { get; set; }

        [Required]
        public required EmailAccount EmailAccount { get; set; }
    }
}
