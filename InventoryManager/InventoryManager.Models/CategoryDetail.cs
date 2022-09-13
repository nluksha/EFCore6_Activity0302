﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InventoryManager.Models.Interfaces;

namespace InventoryManager.Models
{
    public class CategoryDetail: IIdentityModel
    {
        [Key, ForeignKey("Category")]
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(InventoryModelsConstants.MAX_COLORLAVUE_LENGTH)]
        public string ColorValue { get; set; }

        [Required]
        [StringLength(InventoryModelsConstants.MAX_COLORNAME_LENGTH)]
        public string ColorName { get; set; }

        public virtual Category Category { get; set; }
    }
}
