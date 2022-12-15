using System;
using API.DTOs;
using Domain.Entities.Product;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Linq;
using System.Reflection;

namespace API.Services.Helper
{
    /// <summary>
    /// Build expression to filter by entity properties.
    /// </summary>
	public static class BuilderConditionHelper
	{
        /// <summary>
        /// Evaluate the properties of the entity you want to filter by and build the expression to filter.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filterList"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> EvaluateFilters<T>(List<FilterByProperty> filterList)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(Product));
            List<FilterByProperty> validFiltersList = new();
            foreach (FilterByProperty filter in filterList)
            {
                PropertyDescriptor property = properties.Find(filter.Property, true);

                if (property != null)
                    validFiltersList.Add(filter);
            }

            Expression<Func<T, bool>> predicate = True<T>();

            if (validFiltersList.Any())
            {
                foreach (FilterByProperty filter in validFiltersList)
                    predicate = And<T>(predicate, ExpressionField<T>(filter));
            }

            return predicate;
        }

        /// <summary>
        /// Expression when should be evaluate how true.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Expression<Func<T, bool>> True<T>() { return f => true; }

        /// <summary>
        /// Add and statement.
        /// </summary>
        /// <param name="expr1"></param>
        /// <param name="expr2"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expr1,
                                                          Expression<Func<T, bool>> expr2)
        {
            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, bool>>
                  (Expression.AndAlso(expr1.Body, invokedExpr), expr1.Parameters);
        }

        /// <summary>
        /// Return expression to field o=>o.filed.contains(value)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Filter"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> ExpressionField<T>(FilterByProperty Filter)
        {
            /*parameter lambda "p"*/
            var xParameter = Expression.Parameter(typeof(T), "p");

            /*property*/
            var mi = typeof(T).GetProperty(Filter.Property);
            Type miTYpe = mi.PropertyType;

            ConstantExpression someValue = BuildConstant(Filter.Value, miTYpe);

            /*entity property "p.Field1"*/
            MemberExpression xOriginal = Expression.Property(xParameter, mi);

            /*Method to use in lambda*/
            Expression containsMethodExp = SelectConditional(Filter.FilterType, xOriginal, someValue, mi);

            /* expression "o => new Data { Field1 = o.Field1, Field2 = o.Field2 }"*/
            var lambda = Expression.Lambda<Func<T, bool>>(containsMethodExp, xParameter);

            /*compile to Func<Data, Data>*/
            return lambda;
        }

        /// <summary>
        /// Get expresion to constant
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static ConstantExpression BuildConstant(object value, Type type = null)
        {
            if (type.Equals(typeof(Int32)))
                value = Convert.ToInt32(value.ToString());

            else if (type.Equals(typeof(bool)))
                value = Convert.ToBoolean(value.ToString());

            else if (type.Equals(typeof(DateTime)))
                value = Convert.ToDateTime(value.ToString());

            else
                value = value.ToString();

            
            ConstantExpression someValue = Expression.Constant(value, value.GetType());

            return someValue;
        }

        /// <summary>
        /// Search and build the expresion to conditional
        /// </summary>
        /// <param name="FilterType"></param>
        /// <param name="MemberExpression"></param>
        /// <param name="ConstantExpression"></param>
        /// <param name="PropertyInfo"></param>
        /// <returns></returns>
        public static Expression SelectConditional(FilterType FilterType, MemberExpression MemberExpression, ConstantExpression ConstantExpression, PropertyInfo PropertyInfo)
        {
            MethodInfo method = null;
            Expression expresion = null;
            Type Type = PropertyInfo.PropertyType;
            Expression convertExpr = ConstantExpression;
            Expression mExpr = MemberExpression;

            switch (FilterType)
            {
                case FilterType.Contains:
                    method = Type.GetMethod("Contains", new[] { Type });
                    expresion = Expression.Call(mExpr, method, convertExpr);
                    break;
                case FilterType.NotContains:
                    method = Type.GetMethod("Contains", new[] { Type });
                    expresion = Expression.Call(mExpr, method, convertExpr);
                    expresion = Expression.Not(expresion);
                    break;
                case FilterType.Equals:

                    expresion = Expression.Equal(mExpr, convertExpr);

                    break;
                case FilterType.GreaterThan:
                    expresion = Expression.GreaterThan(mExpr, convertExpr);
                    break;
                case FilterType.GreaterThanOrEqual:
                    expresion = Expression.GreaterThanOrEqual(mExpr, convertExpr);
                    break;
                case FilterType.LessThan:
                    expresion = Expression.LessThan(mExpr, convertExpr);
                    break;
                case FilterType.LessThanOrEqual:
                    expresion = Expression.LessThanOrEqual(mExpr, convertExpr);
                    break;

            }

            return expresion;
        }
    }
}