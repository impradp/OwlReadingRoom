using Microsoft.Extensions.Configuration;

namespace OwlReadingRoom.Configurations
{
    /// <summary>
    /// Handles all the app level configuration and maps to the proper DTO.
    /// </summary>
    public static class ConfigurationHandler
    {
        /// <summary>
        /// Retrieves all the relevant configuration for the action features provided in either room or packages or others.
        /// </summary>
        /// <param name="configuration">The configuration manager that holds the key and values of app variables</param>
        /// <param name="actionIdentifier">The key to identify whether the action features are for Room or Packages.</param>
        /// <returns></returns>
        public static ActionFeatures GetActionFeatures(IConfiguration configuration, string actionIdentifier)
        {
            var actionIdentifierFeatures = configuration.GetRequiredSection("Features").GetRequiredSection(actionIdentifier);
            return new ActionFeatures
            {
                Delete = Boolean.Parse(actionIdentifierFeatures.GetSection("Delete").Value),
                Edit = Boolean.Parse(actionIdentifierFeatures.GetSection("Edit").Value),
                View = Boolean.Parse(actionIdentifierFeatures.GetSection("View").Value)
            };
        }

        /// <summary>
        /// Fetches the company details from the app settings manager.
        /// </summary>
        /// <param name="configuration">The interface that handles the configuration related service invocation.</param>
        /// <returns>The company details.</returns>
        public static CompanyDetails GetCompanyInformation(IConfiguration configuration)
        {
            var companyInformation = configuration.GetRequiredSection("Company");
            return new CompanyDetails
            {
                Name = companyInformation.GetSection("Name").Value,
                Address = companyInformation.GetSection("Address").Value,
                City = companyInformation.GetSection("City").Value,
                MobileNo = companyInformation.GetSection("MobileNo").Value,
                AlternateMobileNo = companyInformation.GetSection("AlternateMobileNo").Value,
                EmailID = companyInformation.GetSection("EmailID").Value,
                AlternateEmailID = companyInformation.GetSection("AlternateEmailID").Value
            };
        }
    }
}
