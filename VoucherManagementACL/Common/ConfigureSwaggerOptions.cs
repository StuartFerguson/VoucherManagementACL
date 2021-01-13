﻿namespace VoucherManagementACL.Common
{
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.AspNetCore.Mvc.ApiExplorer;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;
    using Microsoft.OpenApi.Models;
    using Swashbuckle.AspNetCore.SwaggerGen;

    /// <summary>
    /// Configures the Swagger generation options.
    /// </summary>
    /// <remarks>This allows API versioning to define a Swagger document per API version after the
    /// <see cref="IApiVersionDescriptionProvider"/> service has been resolved from the service container.</remarks>
    [ExcludeFromCodeCoverage]
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        #region Fields

        /// <summary>
        /// The provider
        /// </summary>
        private readonly IApiVersionDescriptionProvider provider;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigureSwaggerOptions"/> class.
        /// </summary>
        /// <param name="provider">The <see cref="IApiVersionDescriptionProvider">provider</see> used to generate Swagger documents.</param>
        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) => this.provider = provider;

        #endregion

        #region Methods

        /// <inheritdoc />
        public void Configure(SwaggerGenOptions options)
        {
            // add a swagger document for each discovered API version
            // note: you might choose to skip or document deprecated API versions differently
            foreach (ApiVersionDescription description in this.provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(description.GroupName, ConfigureSwaggerOptions.CreateInfoForApiVersion(description));
            }
        }

        /// <summary>
        /// Creates the information for API version.
        /// </summary>
        /// <param name="description">The description.</param>
        /// <returns></returns>
        private static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
        {
            OpenApiInfo info = new OpenApiInfo
            {
                Title = "Golf Handicapping API",
                Version = description.ApiVersion.ToString(),
                Description = "A REST Api to manage the golf club handicapping system.",
                Contact = new OpenApiContact
                {
                    Name = "Stuart Ferguson",
                    Email = "golfhandicapping@btinternet.com"
                },
                License = new OpenApiLicense
                {
                    Name = "TODO"
                }
            };

            if (description.IsDeprecated)
            {
                info.Description += " This API version has been deprecated.";
            }

            return info;
        }

        #endregion
    }
}
