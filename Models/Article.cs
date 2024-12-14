using System.ComponentModel.DataAnnotations;

namespace ProiectDAW.Models
{
	public class Article
	{
		[Key]
		public int Id { get; set; }
		public string ArticleTitle { get; set; }
		public string ArticleContent { get; set; }

		public virtual ICollection<Chapter>? Chapters { get; set; }


	}
}
