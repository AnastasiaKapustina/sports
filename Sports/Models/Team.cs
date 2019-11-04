using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Sports.Models
{
    public class Team
    {
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Название команды.
        /// </summary>
        [DisplayName("Название")]
        public String Name { get; set; }

        /// <summary>
        /// Вид спорта команды.
        /// </summary>
        [DisplayName("Играет в")]
        public Guid SportId { get; set; }

        /// <summary>
        /// Вид спорта команды.
        /// </summary>
        public Sport Sport { get; set; }

        /// <summary>
        /// Состав команды.
        /// </summary>
        public virtual ICollection<Player> Players { get; set; }
    }
}
