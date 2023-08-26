using DotNetAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace DotNetAPI.Controllers
{
    [Route("api/")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public UserController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: api/users
        [HttpGet("users")]
        [Authorize]
        public IActionResult Get()
        {
            // Get all users from the database
            var users = _context.Users;

            // create new objects without the password using LINQ
            var usersWithoutPasswords = new List<object>();
            foreach (var user in users)
            {
                var userWithoutPassword = new
                {
                    user.Id,
                    user.Name,
                    user.Email
                };
                usersWithoutPasswords.Add(userWithoutPassword);
            }

            // Return a list of users
            return Ok(usersWithoutPasswords);
        }

        // POST api/login
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginModel user)
        {
            var userFromDb = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
            if (userFromDb == null)
                return Unauthorized();

            // Verify the password
            bool isPasswordValid = VerifyPassword(user.Password, userFromDb.PasswordHash);
            if (!isPasswordValid)
                return Unauthorized();

            string token = CreateToken(userFromDb, _configuration);

            // Create a user object without the password
            var userWithoutPassword = new
            {
                userFromDb.Id,
                userFromDb.Name,
                userFromDb.Email,
                Token = token
            };


            return Ok(userWithoutPassword);
        }

        // POST api/register
        [HttpPost("register")]
        public IActionResult Register([FromBody] RegistrationModel model)
        {
            if (!ModelState.IsValid)
            {
                // If model validation fails, return a bad request with validation errors
                return BadRequest(ModelState);
            }

            // Check if the email is already registered
            if (_context.Users.Any(u => u.Email == model.Email))
            {
                // Return a conflict response if the email is not unique
                ModelState.AddModelError("Email", "Email is already registered.");
                return Conflict(ModelState);
            }

            // Hash the user's password
            string hashedPassword = HashPassword(model.Password);

            // Map RegistrationModel to User entity
            var user = new User
            {
                Name = model.Name,
                Email = model.Email,
                PasswordHash = hashedPassword
            };

            // Save the user to the database using Entity Framework Core
            try
            {
                _context.Users.Add(user);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                // Handle any exceptions that might occur during database save
                return StatusCode(500, new { message = "Registration failed. Please try again later.", error = ex.Message });
            }

            // If successful, return a 201 Created response
            var message = new { message = "User registered successfully" };
            return CreatedAtAction(nameof(Register), message);
        }

        private static bool VerifyPassword(string password, string hashedPassword)
        {
            // Extract the salt and iteration count from the hashed password
            string[] passwordParts = hashedPassword.Split('|');
            if (passwordParts.Length != 3)
                return false;

            byte[] saltBytes = Convert.FromBase64String(passwordParts[0]);
            int iterationCount = int.Parse(passwordParts[1]);

            // Generate the hash of the provided password using the same salt and iteration count
            string generatedHash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: saltBytes,
                prf: KeyDerivationPrf.HMACSHA512,
                iterationCount: iterationCount,
                numBytesRequested: 256 / 8
            ));

            // Compare the generated hash with the stored hashed password
            bool isPasswordValid = generatedHash.Equals(passwordParts[2]);

            return isPasswordValid;
        }

        private static string HashPassword(string password)
        {
            // Generate a random salt
            byte[] saltBytes = new byte[128 / 8];
            using (var rngCryptoServiceProvider = new RNGCryptoServiceProvider())
            {
                rngCryptoServiceProvider.GetBytes(saltBytes);
            }

            // Generate the hash of the password using PBKDF2
            string hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: saltBytes,
                prf: KeyDerivationPrf.HMACSHA512,
                iterationCount: 10000,
                numBytesRequested: 256 / 8
            ));

            // Combine the salt, iteration count, and hashed password into a single string
            string passwordWithMetadata = $"{Convert.ToBase64String(saltBytes)}|10000|{hashedPassword}";

            return passwordWithMetadata;
        }

        private static string CreateToken(User user, IConfiguration configuration)
        {
            var issuer = configuration["Jwt:Issuer"];
            var audience = configuration["Jwt:Audience"];
            var key = Encoding.ASCII.GetBytes(configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                            new Claim("Id", Guid.NewGuid().ToString()),
                            new Claim(JwtRegisteredClaimNames.Sub, user.Name),
                            new Claim(JwtRegisteredClaimNames.Email, user.Email),
                            new Claim(JwtRegisteredClaimNames.Jti,
                            Guid.NewGuid().ToString())
                         }),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials
                (new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha512Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = tokenHandler.WriteToken(token);
            var stringToken = tokenHandler.WriteToken(token);
            return stringToken;
        }

    }

}
