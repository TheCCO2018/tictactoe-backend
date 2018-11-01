using System;
using System.Collections.Generic;
using System.Text;
using TictactoeBackend.DataAccess.Abstractions;
using TictactoeBackend.Entity.Concretes;

namespace TictactoeBackend.DataAccess.Concretes
{
    public class UserRepository : DataRepository<User>, IUserRepository
    {
    }
}
