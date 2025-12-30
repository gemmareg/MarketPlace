using System;

namespace MarketPlace.Domain.Common
{
    public class BaseEntity
    {
        public Guid Id { get; protected set; } = Guid.NewGuid();
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime LastModifiedDate { get; set; }
        public string LastModifiedBy { get; set; } = string.Empty;

        protected BaseEntity() 
        { 
            Id = Guid.NewGuid();
        }

    }
}
