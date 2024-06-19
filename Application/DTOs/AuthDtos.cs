namespace Application.DTOs;


public record LoginDto(string EmailOrUsername, string Password);

public record class RegisterDto(string Email, string Username, string Password);