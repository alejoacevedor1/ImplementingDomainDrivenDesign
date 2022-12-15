using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.DTOs
{
    /// <summary>
    /// Pagination and filter information.
    /// </summary>
    [NotMapped]
    public class PaginationFilter
    {
        /// <summary>
        /// Maximum number of items to display per page.
        /// </summary>
        public int ItemsPerPage { get; set; }

        /// <summary>
        /// Actual page.
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// List of filters to apply.
        /// </summary>
        public List<FilterByProperty> FilterList {get;set;} = new();
	}
}