using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Sports.Models
{
    public class Sport
    {
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Название вида спорта.
        /// </summary>
        [DisplayName("Название")]
        public String Name { get; set; }

        /// <summary>
        /// Команды, играющие в этот вид спорта.
        /// </summary>
        public virtual ICollection<Team> Teams { get; set; }

        /// <summary>
        /// Игры, сыгранные в этот вид спорта.
        /// </summary>
        public virtual ICollection<Game> Games { get; set; }

        /// <summary>
        /// Типы событий, возможные в игре.
        /// </summary>
        public virtual ICollection<EventType> EventTypes { get; set; }
    }
}
