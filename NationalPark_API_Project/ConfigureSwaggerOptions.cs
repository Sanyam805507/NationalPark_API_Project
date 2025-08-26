using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace NationalPark_API_Project
{
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        public void Configure(SwaggerGenOptions options)
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header Using the Bearer Scheme \r\n\r\n." +
                "Enter 'Bearer' [SPACE] and then Your token in the text Input below \r\n\r\n" +
                "Example :Bearer 12345abcder \r\n " +
                "Name :Authorization \r\n" +
                "In : header",
                                Name = "Authorization",
                                In = ParameterLocation.Header,
                                Type = SecuritySchemeType.ApiKey,
                                Scheme = "Bearer"
            });

            //******
            options.AddSecurityRequirement(new OpenApiSecurityRequirement() {
              {
                new OpenApiSecurityScheme {
                    Reference =new OpenApiReference
                    {
                     Type = ReferenceType.SecurityScheme,
                     Id = "Bearer"
                    },
                    Scheme ="Oauth2",
                    Name ="Bearer",
                    In = ParameterLocation.Header
                },
                new List<String>()
              }
            });
        }
    }
}
