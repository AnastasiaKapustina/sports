using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Sports.ViewModels
{
    public class EventViewModel
    {
        public Guid Id { get; set; }

        [DisplayName("Тип события")]
        public Guid EventTypeId { get; set; }

        [DisplayName("Тип события")]
        public String EventTypeName { get; set; }

        [DisplayName("Игрок 1")]
        public Guid Player1Id { get; set; }

        [DisplayName("Игрок 1")]
        public String Player1Name { get; set; }

        [DisplayName("Игрок 2")]
        public Guid Player2Id { get; set; }

        [DisplayName("Игрок 2")]
        public String Player2Name { get; set; }

        [DisplayName("Событие для двух игроков")]
        public Boolean IsTwoPlayer { get; set; }

        [DisplayName("Произошло в игре")]
        public Guid GameId { get; set; }

        [DisplayName("Минута игры")]
        public Int32 Time { get; set; }
    }
}
