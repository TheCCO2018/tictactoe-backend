using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Runtime.Serialization;
using System.Text;
using TictactoeBackend.Entity.Abstractions;

namespace TictactoeBackend.Entity.Concretes
{
   public class User : IEntity
   {

       [Key, Index]
       public int UserId { get; set; }
       public string AspUserId { get; set; }
       public string Username { get; set; }

       [Required]
       public virtual UserStat Stats{ get; set; }

       [NotMapped, IgnoreDataMember]
       public EntityState EntityState { get; set; }
   }
}
