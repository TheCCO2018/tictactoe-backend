using System;
using System.Collections.Generic;
using System.Text;
using TictactoeBackend.Entity.Concretes;

namespace TictactoeBackend.DataAccess.Abstractions
{
    public interface IUserRepository: IDataRepository<User>
    {
    }
}
