using System;
using ASP.NET.Core.WebAPI.Models.UtilityModels;
using Microsoft.Extensions.Configuration;

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

        internal static bool InspectPostgreSQLDbParams(EnvironmentConfiguration environmentConfiguration)
            =>
                (!string.IsNullOrEmpty(environmentConfiguration.PGHOST) &&
                !string.IsNullOrEmpty(environmentConfiguration.PGPORT) &&
                !string.IsNullOrEmpty(environmentConfiguration.PGVERSION) &&
                !string.IsNullOrEmpty(environmentConfiguration.PGDATABASE) &&
                !string.IsNullOrEmpty(environmentConfiguration.PGUSER) &&
                !string.IsNullOrEmpty(environmentConfiguration.PGPASSWORD) &&
                int.TryParse(environmentConfiguration.PGPORT, out _));

        internal static bool InspectSqlServerDbParams(EnvironmentConfiguration environmentConfiguration)
            =>
                (!string.IsNullOrEmpty(environmentConfiguration.SQLHOST) &&
                !string.IsNullOrEmpty(environmentConfiguration.SQLPORT) &&
                !string.IsNullOrEmpty(environmentConfiguration.SQLDATABASE) &&
                !string.IsNullOrEmpty(environmentConfiguration.SQLUSER) &&
                !string.IsNullOrEmpty(environmentConfiguration.SQLPASSWORD) &&
                int.TryParse(environmentConfiguration.SQLPORT, out _));
    }
}