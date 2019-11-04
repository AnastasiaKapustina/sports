using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Sports.Models
{
    public class EventDualPlayer : Event
    {
        /// <summary>
        /// Игрок 1.
        /// </summary>
        public Guid Player1Id { get; set; }

        /// <summary>
        /// Игрок 1.
        /// </summary>
        public Player Player1 { get; set; }

        /// <summary>
        /// Игрок 2.
        /// </summary>
        public Guid Player2Id { get; set; }

        /// <summary>
        /// Игрок 2.
        /// </summary>
        public Player Player2 { get; set; }
    }
}
