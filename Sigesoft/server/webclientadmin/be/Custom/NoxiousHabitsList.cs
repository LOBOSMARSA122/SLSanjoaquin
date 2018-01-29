using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sigesoft.Server.WebClientAdmin.BE
{
    public class NoxiousHabitsList
    {
        public string Habito { get; set; }
        public string v_PersonId { get; set; }
        public int? i_TypeHabitsId { get; set; }
        public int? i_FrequencyId { get; set; }

        public string v_Comment { get; set; }
        public string v_NoxiousHabitsId { get; set; }
    }
}
