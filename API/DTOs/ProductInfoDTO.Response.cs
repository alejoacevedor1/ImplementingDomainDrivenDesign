using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.DTOs
{
    /// <summary>
    /// Class Response with relevant product information
    /// </summary>
    [NotMapped]
    public class ProductInfoDTO
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
        /// Provider description
        /// </summary>
        public string DesciptionProvider { get; set;}

        /// <summary>
        /// Provider Phone
        /// </summary>
        public string PhoneProvider { get; set; }

    }
}

