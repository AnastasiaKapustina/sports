using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Sports.Models
{
    public class Player
    {
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Имя игрока.
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// Команда игрока.
        /// </summary>
        public Guid TeamId { get; set; }

        /// <summary>
        /// Команда игрока.
        /// </summary>
        public Team Team { get; set; }

        /// <summary>
        /// Имя игрока и название команды.
        /// </summary>
        [NotMapped]
        public String NameAndTeam {
            get {
                return this.Name + " (" + this.Team.Name + ")";
            }
        }
    }
}
