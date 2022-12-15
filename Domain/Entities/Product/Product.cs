using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Product
{
    /// <summary>
    /// Entity Product
    /// </summary>
	[Table("Product")]
	public class Product
	{
        /// <summary>
        /// Product identifier
        /// </summary>
        [Key]
		public int IdProduct { get; set; }

        /// <summary>
        /// Product description
        /// </summary>
        [Required]
		[MaxLength(300)]
		public string Description { get; set; }

        /// <summary>
        /// Product active status
        /// </summary>
        [Required]
        public bool Active { get; set; }

        /// <summary>
        /// Product manufacturing date
        /// </summary>
		public DateTime ManufacturingDate { get; set; }

        /// <summary>
        /// Product expiration date
        /// </summary>
		public DateTime ValidityDate { get; set; }

        /// <summary>
        /// Provider identifier
        /// </summary>
        [Required]
		public int IdProvider { get; set; }

        /// <summary>
        /// Provider infi
        /// </summary>
        [ForeignKey("IdProvider")]
		public Provider Provider { get; set; }

    }
}