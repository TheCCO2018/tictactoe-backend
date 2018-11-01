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
    public class UserStat : IEntity
    {
        [Key, Index, ForeignKey("User"),IgnoreDataMember]
        public int UserId { get; set; }

        private int winStreak;

        public int WinStreak
        {
            get { return this.winStreak; }
            set { winStreak = value;  }
        }

        private int totalWins = 0;

        public int TotalWins
        {
            get
            {
                return this.totalWins;
            }
            set
            {
                if(totalWins+1 == value) winStreak++;
                Point += 2 + Convert.ToInt32(Math.Floor((double) (WinStreak / 10)));
                totalWins = value;
            }
        }

        private int totalLoses = 0;
        public int TotalLoses
        {
            get
            {
                return this.totalLoses;
            }
            set
            {
                if(totalLoses+1 == value) winStreak = 0;
                this.Point -= 2;
                totalLoses = value;
            }
        }

        public int Point { get; private set; }

        [NotMapped]
        public int Rank { get; set; }
        
        [Required]
        [IgnoreDataMember]
        public virtual User User { get; set; }

        [NotMapped]
        [IgnoreDataMember]
        public EntityState EntityState { get; set; }
    }
}
