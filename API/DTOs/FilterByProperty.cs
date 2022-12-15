using System;

namespace API.DTOs
{
    /// <summary>
    /// Filter information to apply
    /// </summary>
	public class FilterByProperty
	{
        /// <summary>
        /// Name of the property to filter on
        /// </summary>
		public string Property { get; set; }

        /// <summary>
        /// Value by which to filter the property
        /// </summary>
		public object Value { get; set; }

        /// <summary>
        /// Type of filter to apply
        /// </summary>
		public FilterType FilterType{ get; set; }
	}

    /// <summary>
    /// Types of filters that can be applied
    /// </summary>
	public enum FilterType
	{
        Contains,
        NotContains,
        Equals,
        GreaterThan,
        GreaterThanOrEqual,
        LessThan,
        LessThanOrEqual
    }
}