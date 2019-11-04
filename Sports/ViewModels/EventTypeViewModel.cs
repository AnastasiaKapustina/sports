using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Sports.ViewModels
{
    public class EventTypeViewModel
    {
        public Guid Id { get; set; }

        [DisplayName("Название")]
        public String Name { get; set; }

        [DisplayName("Событие для двух игроков")]
        public Boolean IsDualPlayer { get; set; }

        [DisplayName("Вид спорта")]
        public Guid SportId { get; set; }

        [DisplayName("Вид спорта")]
        public String SportName { get; set; }

        [DisplayName("Число задействованных игроков")]
        public Int32 PlayerCount
        {
            get => IsDualPlayer ? 2 : 1;
        }
    }
}
