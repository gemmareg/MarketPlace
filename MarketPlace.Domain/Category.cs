using MarketPlace.Shared;
using System;
using System.Collections.Generic;

namespace MarketPlace.Domain
{
    public class Category
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }

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
    }    
}
