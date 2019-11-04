using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Sports.ViewModels
{
    public class PlayerEditModel
    {
        /// <summary>
        /// Используется только при редактировании.
        /// </summary>
        public Guid Id { get; set; }

        [DisplayName("Имя")]
        public String Name { get; set; }

        [DisplayName("Играет за")]
        public Guid TeamId { get; set; }
    }
}
