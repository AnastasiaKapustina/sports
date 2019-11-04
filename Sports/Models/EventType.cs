using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Sports.Models
{
    public class EventType
    {
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Вид спорта игры.
        /// </summary>
        [DisplayName("Вид спорта")]
        public Guid SportId { get; set; }

        /// <summary>
        /// Вид спорта игры.
        /// </summary>
        [DisplayName("Вид спорта")]
        public Sport Sport { get; set; }

        /// <summary>
        /// Название типа события.
        /// </summary>
        [DisplayName("Название")]
        public String Name { get; set; }

        /// <summary>
        /// Показывает, задействованы ли в событии два игрока.
        /// </summary>
        [DisplayName("Два игрока?")]
        public Boolean IsDualPlayer { get; set; }

        /// <summary>
        /// Название и название вида спорта.
        /// </summary>
        [NotMapped]
        public String NameAndSport
        {
            get
            {
                return this.Sport != null ? this.Name + " (" + this.Sport.Name + ")" : this.Name;
            }
        }
    }
}
