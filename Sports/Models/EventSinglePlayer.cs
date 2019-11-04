using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Sports.Models
{
    public class EventSinglePlayer : Event
    {
        /// <summary>
        /// Игрок.
        /// </summary>
        public Guid PlayerId { get; set; }

        /// <summary>
        /// Игрок.
        /// </summary>
        public Player Player { get; set; }
    }
}
