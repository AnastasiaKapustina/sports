using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Sports.ViewModels
{
    public class TeamViewModel
    {
        public Guid Id { get; set; }

        [DisplayName("Название")]
        public String Name { get; set; }

        public Dictionary<Guid, String> Players { get; set; }

        [DisplayName("Играет в")]
        public Guid SportId { get; set; }

        [DisplayName("Играет в")]
        public String SportName { get; set; }
    }
}
