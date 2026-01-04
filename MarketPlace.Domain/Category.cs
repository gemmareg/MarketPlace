using MarketPlace.Domain.Common;
using MarketPlace.Shared;
using MarketPlace.Shared.Result.Generic;
using MarketPlace.Shared.Result.NonGeneric;
using System;
using System.Collections.Generic;

namespace MarketPlace.Domain
{
    public class Category : BaseEntity
    {
        public string Name { get; private set; } = string.Empty;
        public string? Description { get; private set; }

        // Relaciones
        public ICollection<Product>? Productos { get; set; }

        private Category() { }

        public static Result<Category> Create(string name, string description)
        {
            if (string.IsNullOrEmpty(name))
                return Result<Category>.Fail(ErrorMessages.INVALID_CATEGORY_NAME);
            
            var category = new Category();
            category.Name = name;
            category.Description = description;
            
            return Result<Category>.Ok(category);
        }

        public Result SetName(string name)
        {
            if (string.IsNullOrEmpty(name))
                return Result.Fail(ErrorMessages.INVALID_CATEGORY_NAME);
            Name = name;
            return Result.Ok();
        }

        public Result SetDescription(string description)
        {
            if(description == null)
                return Result.Fail(description);
            Description = description;
            return Result.Ok();
        }
    }    
}
