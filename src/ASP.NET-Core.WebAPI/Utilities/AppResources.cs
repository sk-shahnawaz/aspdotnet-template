namespace ASP.NET.Core.WebAPI.Utilities;

internal static class AppResources
{
    internal const string CorsPolicyName = "CorsPolicy";
    internal const string MimeTypeApplicationJson = "application/json";
    internal const string MimeTypeApplicationJsonPatchJson = "application/json-patch+json";
    internal const string InternalServerErrorMessage = "Internal Server Error, please try again later.";
    internal const string BadRequestDueToAutoMapFailure = "Bad Request.";
    internal const string ForbidRequestDueToUniqueConstraintFailure = "Duplicate item! {0} with value '{1}' already exists in datastore.";
    internal const string ActionExceptionMessage = "Error occured in application, details : {@data}";
    internal const string InvalidValueValidationMessage = "Value of {0} is invalid.";
    internal const string EmptyValueValidationMessage = "Value of {0} can not be empty.";
    internal const string NoItemsFoundMessage = "No {0} found in Database.";
    internal const string NoItemFoundMessage = "{0} with {1}: {2} not found in datastore.";
    internal const string ModelValidationErrorMessage = "One or more validation errors occurred.";
    internal const string UnprocessableEntity = "Unprocessable entity.";
    internal const string InMemoryDbNameMissing = "Please provide a valid DB name is appsetting.json file.";
    internal const string PostgresParamsInvalid = "One or more configuration parameters required to make connection with PostgreSQL database is invalid.";
    internal const string SqlServerParamsInvalid = "One or more configuration parameters required to make connection with SQL Server database is invalid.";
    internal const string DbConfigurationInvalid = "No proper Database is configured for this application to use. Please check.";
    internal const string ApiDescriptor = "This API manages all operations on resource ";
}