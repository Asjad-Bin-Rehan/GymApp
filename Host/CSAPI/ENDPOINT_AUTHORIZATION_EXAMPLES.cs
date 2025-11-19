using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace CSAPI.Examples
{
    /// <summary>
    /// Examples of how to use JWT authentication and authorization in your endpoints
    /// </summary>
    public static class EndpointAuthorizationExamples
    {
        public static void MapExampleEndpoints(this WebApplication app)
        {
            // Example 1: Public endpoint (no authentication required)
            app.MapGet("/api/public", () => 
            {
                return Results.Ok(new { message = "This is a public endpoint" });
            })
            .WithName("PublicEndpoint")
            .WithOpenApi();

            // Example 2: Protected endpoint (requires authentication)
            app.MapGet("/api/protected", () => 
            {
                return Results.Ok(new { message = "This endpoint requires authentication" });
            })
            .RequireAuthorization()
            .WithName("ProtectedEndpoint")
            .WithOpenApi();

            // Example 3: Endpoint with custom policy
            app.MapGet("/api/custom-policy", () => 
            {
                return Results.Ok(new { message = "This endpoint uses custom authorization policy" });
            })
            .RequireAuthorization("CustomPolicy")
            .WithName("CustomPolicyEndpoint")
            .WithOpenApi();

            // Example 4: Access user information from JWT claims
            app.MapGet("/api/user-info", (HttpContext context) => 
            {
                var user = context.User;
                
                return Results.Ok(new
                {
                    userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                    userName = user.FindFirst(ClaimTypes.Name)?.Value,
                    email = user.FindFirst(ClaimTypes.Email)?.Value,
                    roles = user.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList(),
                    isAuthenticated = user.Identity?.IsAuthenticated ?? false
                });
            })
            .RequireAuthorization()
            .WithName("UserInfo")
            .WithOpenApi();

            // Example 5: Endpoint with role-based authorization
            app.MapGet("/api/admin-only", [Authorize(Roles = "Admin")] () => 
            {
                return Results.Ok(new { message = "This endpoint is for admins only" });
            })
            .WithName("AdminOnly")
            .WithOpenApi();

            // Example 6: POST endpoint with authentication
            app.MapPost("/api/create-resource", [Authorize] (CreateResourceRequest request, HttpContext context) => 
            {
                var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                
                // Your business logic here
                return Results.Created($"/api/resource/{Guid.NewGuid()}", new 
                { 
                    message = "Resource created",
                    createdBy = userId,
                    data = request
                });
            })
            .WithName("CreateResource")
            .WithOpenApi();

            // Example 7: Conditional authorization based on user claims
            app.MapGet("/api/conditional", (HttpContext context) => 
            {
                var user = context.User;
                
                if (!user.Identity?.IsAuthenticated ?? true)
                {
                    return Results.Unauthorized();
                }

                var hasPermission = user.HasClaim("Permission", "SpecialAccess");
                
                if (!hasPermission)
                {
                    return Results.Forbid();
                }

                return Results.Ok(new { message = "You have special access" });
            })
            .RequireAuthorization()
            .WithName("ConditionalAccess")
            .WithOpenApi();

            // Example 8: Allow anonymous access on otherwise protected endpoint
            app.MapGet("/api/optional-auth", [AllowAnonymous] (HttpContext context) => 
            {
                var isAuthenticated = context.User.Identity?.IsAuthenticated ?? false;
                
                return Results.Ok(new 
                { 
                    message = "This endpoint works with or without authentication",
                    isAuthenticated,
                    userName = isAuthenticated ? context.User.Identity?.Name : "Anonymous"
                });
            })
            .WithName("OptionalAuth")
            .WithOpenApi();
        }

        // Example request model
        public record CreateResourceRequest(string Name, string Description);
    }
}
