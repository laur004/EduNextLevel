using System.ComponentModel.DataAnnotations;

namespace ProiectDAW.Models
{
	public class Grade
	{
		public int Id { get; set; }
		[Required(ErrorMessage = "Incadrarea intr-o clasa este obliagtorie")]
		public string GradeName { get; set; }

	}
}
