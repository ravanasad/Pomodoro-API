namespace Application.DTOs.Errors;
public static class TokenErrors
{
    public static ErrorDesc TokenGenerationFailed => new("Token Generation Failed", "An error occurred while generating the token. Please try again.");
    public static ErrorDesc InvalidRole => new("Invalid Roles", "Roles cannot be null");
    public static ErrorDesc FailedToAddClaim => new("Failed to Add Claims", "Failed to add claims to the user");
    public static ErrorDesc InvalidToken => new("Invalid Token", "The provided token is invalid or expired.");
    public static ErrorDesc InvalidRefreshToken => new("Invalid Refresh Token", "The provided refresh token is invalid or expired");
    public static ErrorDesc RefreshTokenExpired => new("Refresh Token Expired", "The refresh token has expired. Please login again.");
}
