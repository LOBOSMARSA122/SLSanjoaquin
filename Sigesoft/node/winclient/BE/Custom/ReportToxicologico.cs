using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sigesoft.Node.WinClient.BE
{
    public class ReportToxicologico
    {
        public string ServiceId { get; set; }
        public DateTime? Fecha { get; set; }
        public string Trabajador { get; set; }
        public int Edad { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public string EmpresaTrabajador { get; set; }
        public byte[] FirmaMedico { get; set; }
        public byte[] FirmaTrabajador { get; set; }
        public byte[] HuellaTrabajador { get; set; }
        public string Puesto { get; set; }
        public string Empresa { get; set; }
        public string COCAINA { get; set; }
        public string MARIHUANA { get; set; }
        public byte[] LogoEmpresa { get; set; }
        public string Dni { get; set; }
        public string NombreUsuarioGraba { get; set; }
        public string ANFETAMINA { get; set; }
        public string ALCOHOLEMIA { get; set; }
        public string ALCOHOLEMIA_DESEABLE { get; set; }
        public string BENZODIACEPINA { get; set; }
        public string BENZODIACEPINA_DESEABLE { get; set; }
        public string COLINESTERASA { get; set; }
        public string COLINESTERASA_DESEABLE { get; set; }

        public string CARBOXIHEMOGLOBINA { get; set; }
        public string CARBOXIHEMOGLOBINA_DESEABLE { get; set; }
    }

}
