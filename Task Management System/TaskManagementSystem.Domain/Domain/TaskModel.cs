using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagementSystem.Domain.Domain
{
    public class TaskModel : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
    }
}
