using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.DTOs
{
    /// <summary>
    /// Class with the product information to be added or updated.
    /// </summary>
	[NotMapped]
	public class AddProduct
	{
        /// <summary>
        /// Product identifier
        /// </summary>
		public int IdProduct { get; set; }

        /// <summary>
        /// Product description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Product active status
        /// </summary>
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
        public int IdProvider { get; set; }
    }
}

