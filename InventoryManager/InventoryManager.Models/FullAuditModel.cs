using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InventoryManager.Models.Interfaces;

namespace InventoryManager.Models
{
    public abstract class FullAuditModel: IIdentityModel, IAuditedModel, IActivatableModel
    {
        public int Id { get; set; }

        [StringLength(InventoryModelsConstans.MAX_USERID_LENGTH)]
        public string? CreatedByUserId { get; set; }
        public DateTime CreatedDate { get; set; }

        [StringLength(InventoryModelsConstans.MAX_USERID_LENGTH)]
        public string? LastModifiedUserId { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public bool IsActive { get; set; }
    }
}
