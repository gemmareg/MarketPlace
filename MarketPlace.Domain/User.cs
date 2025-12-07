using System.Collections.Generic;
using System;
using MarketPlace.Shared;

namespace MarketPlace.Domain
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public UserRole Role { get; set; }
        public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;

        // Relaciones
        public ICollection<Product>? Products { get; set; }
        public ICollection<Order>? Orders { get; set; }
        public ICollection<Review>? Reviews { get; set; }
        public ICollection<CartItem>? Cart { get; set; }

        private User() { }

        public static Result<User> Create(string name, string email, string passwordHash)
        {
            if(name == null || name.Trim() == string.Empty)
                return Result<User>.Fail(ErrorMessages.INVALID_USER_NAME);
            if(email == null || email.Trim() == string.Empty)
                return Result<User>.Fail(ErrorMessages.INVALID_USER_EMAIL);
            if(passwordHash == null || passwordHash.Trim() == string.Empty)
                return Result<User>.Fail(ErrorMessages.INVALID_USER_PASSWORD);

            var user = new User();
            user.Name = name;
            user.Email = email;
            user.PasswordHash = passwordHash;
            user.Role = UserRole.Buyer;

            return Result<User>.Ok(user);
        }


        public enum UserRole
        {
            Buyer,
            Seller,
            Admin
        }   
    }
}
