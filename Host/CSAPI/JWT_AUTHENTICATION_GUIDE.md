# JWT Authentication & Authorization Configuration Guide

## Overview
This application uses JWT (JSON Web Token) Bearer authentication with full validation enabled.

## Configuration Files

### 1. appsettings.json
JWT settings are configured in `appsettings.json`:

```json
"Jwt": {
  "Key": "Y6p!Qx83@vM91aZ$7dP2#BwL*eH9cT0k",
  "Issuer": "GymApp",
  "Audience": "GymAppUsers",
  "AccessTokenExpirationInMinutes": 60,
  "RefreshTokenExpirationInDays": 7
}
```

**Important:** For production, use environment variables instead of hardcoded values:
- `JWT_KEY` - Secret key for signing tokens (minimum 32 characters)
- `JWT_ISSUER` - Token issuer identifier
- `JWT_AUDIENCE` - Token audience identifier
- `JWT_ACCESS_TOKEN_EXPIRATION_IN_MINUTES` - Access token lifetime
- `JWT_REFRESH_TOKEN_EXPIRATION_IN_DAYS` - Refresh token lifetime

### 2. Program.cs
The main entry point configures services and middleware:

```csharp
// Register all services including JWT Authentication
builder.Services.RegisterService(builder.Configuration);

// Middleware pipeline includes Authentication & Authorization
await app.Configure();
```

### 3. JWT Validation Configuration
Located in `Extensions/Validators/Validator.cs`:

**Enabled Validations:**
- ✅ **ValidateIssuerSigningKey** - Validates the token signature
- ✅ **ValidateIssuer** - Validates the token issuer
- ✅ **ValidateAudience** - Validates the token audience
- ✅ **ValidateLifetime** - Validates token expiration
- ✅ **ClockSkew = Zero** - No tolerance for expired tokens

## Middleware Pipeline Order

The middleware is configured in the correct order in `ConfigureApp.cs`:

1. **CORS** - Must be before authentication
2. **UseAuthentication()** - Validates JWT tokens
3. **UseAuthorization()** - Checks user permissions
4. **UserContextMiddleware** - Custom middleware after auth
5. **Endpoints** - API endpoints

## Using JWT Authentication in Endpoints

### Require Authentication
```csharp
app.MapGet("/api/protected", () => "Protected data")
   .RequireAuthorization();
```

### Use Custom Policy
```csharp
app.MapGet("/api/admin", () => "Admin data")
   .RequireAuthorization(KPolicyDescriptor.CustomPolicy);
```

### Access User Claims
```csharp
app.MapGet("/api/user-info", (HttpContext context) =>
{
    var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    var userName = context.User.Identity?.Name;
    return new { userId, userName };
})
.RequireAuthorization();
```

## Testing JWT Authentication

### 1. Generate a Token
Use your login endpoint to get a JWT token.

### 2. Include Token in Requests
Add the token to the Authorization header:

```
Authorization: Bearer <your-jwt-token>
```

### 3. Test with Swagger
Swagger is configured to support JWT. Click "Authorize" and enter:
```
Bearer <your-jwt-token>
```

### 4. Test with curl
```bash
curl -H "Authorization: Bearer <your-jwt-token>" http://localhost:5087/api/protected
```

## Token Expiration Handling

When a token expires:
- The API returns 401 Unauthorized
- Response includes header: `Token-Expired: true`
- Client should refresh the token or redirect to login

## Security Best Practices

1. **Never commit secrets** - Use environment variables or Azure Key Vault
2. **Use HTTPS** - Always use HTTPS in production
3. **Strong secret keys** - Minimum 32 characters, random and complex
4. **Short token lifetime** - Keep access tokens short-lived (15-60 minutes)
5. **Implement refresh tokens** - Use refresh tokens for extended sessions
6. **Validate all claims** - All validations are enabled by default

## Troubleshooting

### 401 Unauthorized
- Check if token is included in Authorization header
- Verify token hasn't expired
- Ensure issuer and audience match configuration

### Token validation fails
- Verify the secret key matches between token generation and validation
- Check issuer and audience values
- Ensure token hasn't been tampered with

### CORS issues with authentication
- CORS middleware must be before Authentication
- Ensure AllowCredentials is set if using cookies
