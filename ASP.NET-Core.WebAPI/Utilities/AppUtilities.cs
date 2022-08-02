using System;
using AutoMapper;
using System.Linq;
using System.Reflection;
using System.Linq.Expressions;
using Microsoft.Extensions.Configuration;
using ASP.NET.Core.WebAPI.Models.UtilityModels;
using AutoMapper.Internal;

namespace ASP.NET.Core.WebAPI.Utilities
{
    internal static class AppUtilities
    {
        internal static T ReadConfigurationKeyValue<T>(IConfiguration configuration, string configurationKeyName, T defaultValueToBeReturnedInCaseOfFailure)
            where T : IConvertible
        {
            string unprocessedValue = configuration[configurationKeyName];
            if (unprocessedValue == null)
            {
                // Key absent in Configuration [AppSettings.json/AppSettings.{ENVIRONMENT}.json/EnvironmentVariables etc.]
                return defaultValueToBeReturnedInCaseOfFailure;
            }
            else
            {
                T convertedType;
                try
                {
                    switch (typeof(T).Name.ToLower())
                    {
                        case "boolean":
                            bool booleanVariable = default;
                            if (string.Equals(unprocessedValue, "true", StringComparison.OrdinalIgnoreCase))
                            {
                                booleanVariable = true;
                            }
                            convertedType = (T)Convert.ChangeType(booleanVariable, typeof(T));
                            break;

                        case "int32":
                            if (!int.TryParse(unprocessedValue, out int int32Variable))
                            {
                                int32Variable = 0;
                            }
                            convertedType = (T)Convert.ChangeType(int32Variable, typeof(T));
                            break;

                        default:
                            // string
                            convertedType = (T)Convert.ChangeType(unprocessedValue, typeof(T));
                            break;

                    }
                }
                catch (Exception)
                {
                    convertedType = defaultValueToBeReturnedInCaseOfFailure;
                }
                return convertedType;
            }
        }

        /// <summary>
        /// Checks if the supplied environment variable values for PostgreSQL database are proper or not
        /// </summary>
        /// <param name="environmentConfiguration">Application's environment variables (strongly typed)</param>
        /// <returns>Boolean status</returns>
        internal static bool InspectPostgreSQLDbParams(EnvironmentConfiguration environmentConfiguration)
            =>
                (!string.IsNullOrEmpty(environmentConfiguration.PGHOST) &&
                !string.IsNullOrEmpty(environmentConfiguration.PGPORT) &&
                !string.IsNullOrEmpty(environmentConfiguration.PGVERSION) &&
                !string.IsNullOrEmpty(environmentConfiguration.PGDATABASE) &&
                !string.IsNullOrEmpty(environmentConfiguration.PGUSER) &&
                !string.IsNullOrEmpty(environmentConfiguration.PGPASSWORD) &&
                int.TryParse(environmentConfiguration.PGPORT, out _));

        /// <summary>
        /// Checks if the supplied environment variable values for SQL Server database are proper or not
        /// </summary>
        /// <param name="environmentConfiguration">Application's environment variables (strongly typed)</param>
        /// <returns>Boolean status</returns>
        internal static bool InspectSqlServerDbParams(EnvironmentConfiguration environmentConfiguration)
            =>
                (!string.IsNullOrEmpty(environmentConfiguration.SQLHOST) &&
                !string.IsNullOrEmpty(environmentConfiguration.SQLPORT) &&
                !string.IsNullOrEmpty(environmentConfiguration.SQLDATABASE) &&
                !string.IsNullOrEmpty(environmentConfiguration.SQLUSER) &&
                !string.IsNullOrEmpty(environmentConfiguration.SQLPASSWORD) &&
                int.TryParse(environmentConfiguration.SQLPORT, out _));

        /// <summary>
        /// Performs sorting based on passed in attribute (property) value and sorting order.
        /// By default will do sorting in ascending order. This method checks if the sort by attribute sent by client is a valid (exists on type TItemDTO)
        /// or not / is a writeable property or not / has one of the supported data types or not - if any of the condition fails, it defaults to 'Id' property
        /// ; if all conditions satisfy, it uses Automapper's mapping configurations to get the corresponding model (TItem) property which will actually be the key to
        /// do sorting on the database level.
        /// </summary>
        /// <typeparam name="TItem">Generic type argument, representing the database model type.</typeparam>
        /// <typeparam name="TItemDTO">Generic type argument, representing the DTO of the database model type.</typeparam>
        /// <param name="sortingRequest">Sorting request object</param>
        /// <param name="mapper">Automapper abstraction</param>
        /// <param name="data">Unsorted data (IQueryable collection instance)</param>
        /// <returns>Sorted data (IQueryable collection instance)</returns>
        internal static IQueryable<TItem> Sort<TItem, TItemDTO>(this IQueryable<TItem> data, SortingRequest sortingRequest, IMapper mapper)
        {
            PropertyInfo propertyInfo = GetSortingKeyInfo<TItem, TItemDTO>(sortingRequest, mapper);

            // Reference: https://stackoverflow.com/a/55342672 (Accessed on 2021-12-21)
            ParameterExpression parameterExp = Expression.Parameter(typeof(TItem), "x");
            MemberExpression propertyExp = Expression.Property(parameterExp, propertyInfo.Name);
            LambdaExpression orderByExp = Expression.Lambda(propertyExp, parameterExp);

            MethodInfo orderingLinqExtensionMethodInfo = sortingRequest.SortByOrder switch
            {
                SortOrder.DESC => typeof(Queryable).GetMethods().Single(method => method.Name.Equals("OrderByDescending") && method.GetParameters().Length == 2),

                // For "ASC" or for default case (default behavior), the ordering will be in ASCENDING order
                _ => typeof(Queryable).GetMethods().Single(method => method.Name.Equals("OrderBy") && method.GetParameters().Length == 2),
            };

            var orderByMethod = orderingLinqExtensionMethodInfo.MakeGenericMethod(typeof(TItem), propertyExp.Type);

            // Get the result
            return (IQueryable<TItem>)orderByMethod.Invoke(null, new object[] { data, orderByExp });
        }

        private static PropertyInfo GetSortingKeyInfo<TItem, TItemDTO>(SortingRequest sortingRequest, IMapper mapper)
        {
            PropertyInfo propertyInfo = typeof(TItemDTO).GetProperties().FirstOrDefault(prop => prop.Name.Equals(sortingRequest.SortByAttribute.ToString(), StringComparison.OrdinalIgnoreCase));

            // Verify if the property to do sorting sent by client exists or not
            // Can't allow sorting on READONLY properties (no SETTER), example FullName of AuthorDTO, because for FullName, corresponding property/database column doesn't exist
            // Allow sorting only on native types, no user-defined types allowed
            if (propertyInfo == null || !propertyInfo.CanWrite || !CheckIfSortableProperty(propertyInfo.PropertyType))
            {
                propertyInfo = typeof(TItemDTO).GetProperty(sortingRequest.SortByAttribute.ToString());
            }
            var typeMap = mapper.ConfigurationProvider.Internal().GetAllTypeMaps().SingleOrDefault(mapping => mapping.SourceType == typeof(TItemDTO));
            string modelMemberName = string.Empty;
            if (typeMap is not null)
            {
                var propertyMap = typeMap.PropertyMaps.FirstOrDefault(pm => pm.SourceMember.Name.Equals(sortingRequest.SortByAttribute.ToString(), StringComparison.OrdinalIgnoreCase));
                modelMemberName = propertyMap != null ? propertyMap.DestinationMember.Name : string.Empty;
            }
            else
            {
                modelMemberName = sortingRequest.SortByAttribute.ToString();
            }

            return typeof(TItem).GetProperties().FirstOrDefault(prop => prop.Name.Equals(modelMemberName, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Checks if the type of property based on sorting is requested is of a native type or not.
        /// </summary>
        /// <param name="propertyType">Property type</param>
        /// <returns>If the passed in property type is native type or not</returns>
        private static bool CheckIfSortableProperty(Type propertyType)
        {
            return propertyType == typeof(string) || propertyType == typeof(int) || propertyType == typeof(long) ||
                propertyType == typeof(double) || propertyType == typeof(float) || propertyType == typeof(decimal) || propertyType == typeof(bool);
        }
    }
}