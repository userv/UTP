﻿using System.ComponentModel.DataAnnotations;

namespace ECommerce.WebApi.Models
{
    public abstract class BaseModel<TKey>
    {
        [Key]
        public  TKey Id { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public DateTime? ModifiedOn { get; set; }
    }
}