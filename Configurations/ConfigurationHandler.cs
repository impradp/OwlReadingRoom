using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
