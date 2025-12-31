using MarketPlace.Domain.Common;
using MarketPlace.Shared;
using MarketPlace.Shared.Result.Generic;
using System;
using System.Collections.Generic;

namespace MarketPlace.Domain
{
    public class User : BaseEntity
    {
        public string Name { get; private set; } = string.Empty;

        // Relaciones
        public ICollection<Product> Products { get; set; } = [];
        public ICollection<Order> Orders { get; set; } = [];
        public ICollection<Review> Reviews { get; set; } = [];
        public ICollection<CartItem> Cart { get; set; } = [];

        private User() { }

        public static Result<User> Create(Guid id, string name)
        {
            if(id == Guid.Empty)
                return Result<User>.Fail(ErrorMessages.INVALID_USER_ID);
            if (name == null || name.Trim() == string.Empty)
                return Result<User>.Fail(ErrorMessages.INVALID_USER_NAME);

            var user = new User();
            user.Name = name;
            user.Id = id;

            return Result<User>.Ok(user);
        }  
    }
}
