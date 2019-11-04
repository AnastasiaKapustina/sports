using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Sports.ViewModels
{
    public class TeamEditModel
    {
        /// <summary>
        /// Используется только при редактировании.
        /// </summary>
        public Guid Id { get; set; }

        [DisplayName("Название")]
        public String Name { get; set; }

        /// <summary>
        /// Используется только при создании.
        /// </summary>
        [DisplayName("Список игроков")]
        public String Players { get; set; }

        [DisplayName("Играет в")]
        public Guid SportId { get; set; }
    }
}
