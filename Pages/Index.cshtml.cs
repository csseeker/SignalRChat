using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;

namespace SignalRChat.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ILogger<IndexModel> logger)
    {
        Debug.WriteLine(" *** Inside Index.cshtml.cs constructor!");
        _logger = logger;
    }

    public void OnGet()
    {
        Debug.WriteLine(" *** Index.cshtml OnGet");

        // Generate a JWT token with connectionId in the token body
        var connectionId = "your_connection_id";
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes("your_secret_key");
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("connectionId", connectionId)
            }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var jwtToken = tokenHandler.WriteToken(token);

        // Use the JWT token as needed
        // For example, you can pass it to the client-side for authentication
        Debug.WriteLine($"JWT Token: {jwtToken}");
    }
}
