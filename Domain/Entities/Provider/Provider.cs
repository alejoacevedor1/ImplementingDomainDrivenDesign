using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    /// <summary>
    /// Entity Provider
    /// </summary>
	[Table("Provider")]
    public class Provider
    {
        /// <summary>
        /// Provider identifier
        /// </summary>
        [Key]
        public int IdProvider { get; set; }

        /// <summary>
        /// Provider description
        /// </summary>
        [MaxLength(300)]
        public string Description { get; set; }

        /// <summary>
        /// Provider Phone
        /// </summary>
        [MaxLength(20)]
        public string Phone { get; set; }
	}
}

