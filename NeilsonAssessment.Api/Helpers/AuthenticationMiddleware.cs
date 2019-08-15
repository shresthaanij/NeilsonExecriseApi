using System;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using NeilsonAssessment.Api.Repositories;

namespace NeilsonAssessment.Api.Helpers
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthenticationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (!context.Request.Headers.ContainsKey("Authorization"))
            {
                context.Response.StatusCode = 401;
                return;
            }

            var authenticated = false;

            try
            {
                var authenticationHeader = AuthenticationHeaderValue.Parse(context.Request.Headers["Authorization"]);

                if (!authenticationHeader.Scheme.Equals("Basic")) throw new Exception("Invalid Basic auth authroization schema.");

                var credentialChunk = Convert.FromBase64String(authenticationHeader.Parameter);
                var credentials = Encoding.UTF8.GetString(credentialChunk).Split(":");
                var username = credentials[0];
                var password = credentials[1];

                authenticated = username.Equals(password);
            }
            catch
            {
                context.Response.StatusCode = 401; //Unauthorized
                return;
            }

            if (authenticated)
                await _next.Invoke(context);
            else
            {
                context.Response.StatusCode = 401; //Unauthorized
                return;
            }
        }
    }
}
