using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Sports.ViewModels
{
    public class GameViewModel
    {
        public Guid Id { get; set; }

        [DisplayName("Вид сопрта")]
        public Guid SportId { get; set; }

        [DisplayName("Вид спорта")]
        public String SportName { get; set; }

        [DisplayName("Команда 1")]
        public Guid Team1Id { get; set; }

        [DisplayName("Команда 1")]
        public String Team1Name { get; set; }

        [DisplayName("Команда 2")]
        public Guid Team2Id { get; set; }

        [DisplayName("Команда 2")]
        public String Team2Name { get; set; }

        [DisplayName("Команды")]
        public String Teams {
            get {
                return Team1Name + " - " + Team2Name;
            }
        }

        [DisplayName("События в игре")]
        public Dictionary<Guid, String> Events { get; set; }

        [DisplayName("Дата")]
        public DateTime Date { get; set; }
    }
}
