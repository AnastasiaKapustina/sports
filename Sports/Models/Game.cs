using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Sports.Models
{
    public class Game
    {
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Вид спорта игры.
        /// </summary>
        public Guid SportId { get; set; }

        /// <summary>
        /// Вид спорта игры.
        /// </summary>
        [DisplayName("Вид спорта")]
        public Sport Sport { get; set; }

        /// <summary>
        /// События, произошедшие в игре.
        /// </summary>
        public virtual ICollection<Event> Events { get; set; }

        /// <summary>
        /// Команда 1.
        /// </summary>
        public Guid Team1Id { get; set; }

        /// <summary>
        /// Команда 1.
        /// </summary>
        [DisplayName("Команда 1")]
        public Team Team1 { get; set; }

        /// <summary>
        /// Команда 2.
        /// </summary>
        public Guid Team2Id { get; set; }

        /// <summary>
        /// Команда 2.
        /// </summary>
        [DisplayName("Команда 2")]
        public Team Team2 { get; set; }

        /// <summary>
        /// Дата матча.
        /// </summary>
        [DisplayName("Дата")]
        public DateTime Date { get; set; }
    }
}
