using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sigesoft.Node.WinClient.BE
{
  public  class FichaMedicaOcupacional_CHO
    {

        public string ApellidoNombre { get; set; }
        public string LugarProcedencia { get; set; }
        public string FechaNacimiento { get; set; }
        public string Sexo { get; set; }
        public string PuestoTrabajo { get; set; }
        public string NumeroResgistro { get; set; }
        public string Profesion { get; set; }
        public string LugarNacimiento { get; set; }
        public string FechaInicio { get; set; }
        public string Empresa { get; set; }
        public string Altitud { get; set; }
        public string ActividadEmpresa { get; set; }
        public string AreaTrabajo { get; set; }
        public string Ocupacional { get; set; }
        public string TiempoTrabajo { get; set; }
        public string AgenteOcupacional { get; set; }
        public string epp { get; set; }
        public string Dia { get; set; }
        public string Mes { get; set; }
        public string Anio { get; set; }
        public byte[] FirmaTrabajador { get; set; }
        public byte[] HuellaTrabajador { get; set; }
        public byte[] FirmaMedico { get; set; }
    }
}
