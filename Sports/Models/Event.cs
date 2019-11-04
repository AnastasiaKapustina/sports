using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Sports.Models
{
    abstract public class Event
    {
        [Key]
        public Guid Id { get; set; }
        
        /// <summary>
        /// Игра, в которой произошло событие.
        /// </summary>
        public Guid GameId { get; set; }

        /// <summary>
        /// Игра, в которой произошло событие.
        /// </summary>
        public Game Game { get; set; }

        /// <summary>
        /// Тип события.
        /// </summary>
        public Guid EventTypeId { get; set; }

        /// <summary>
        /// Тип события.
        /// </summary>
        public EventType EventType { get; set; }

        /// <summary>
        /// Время события.
        /// </summary>
        public Int32 Time { get; set; }
    }
}
