using System.ComponentModel.DataAnnotations;

namespace BloggingTool.Models {
    public class ArticleLike {
        [Key]
        public int LikeId { get; set; }

         [Required]
        public bool IsLiked { get; set; } = true;

        [Required]
        public int ArticleId { get; set; }

        [Required]
        public int EmailAccountId { get; set; }
    }
}
