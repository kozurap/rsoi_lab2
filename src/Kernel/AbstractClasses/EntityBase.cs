using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Kernel.Interfaces;

namespace Kernel.AbstractClasses
{
    public abstract class EntityBase : IEntity
    {
        [Key] public int Id { get; set; }
    }
}
