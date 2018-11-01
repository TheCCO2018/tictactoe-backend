using System;
using System.Collections.Generic;
using System.Text;

namespace TictactoeBackend.Entity.Abstractions
{
    public interface IEntity
    {
        EntityState EntityState { get; set; }
    }

    public enum EntityState
    {
        Unchanged,
        Added,
        Modified,
        Deleted
    }
}
