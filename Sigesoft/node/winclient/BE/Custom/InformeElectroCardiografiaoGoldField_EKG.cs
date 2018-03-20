using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sigesoft.Node.WinClient.BE
{
    public class InformeElectroCardiografiaoGoldField_EKG
    {
        public string ApellidoNombre { get; set; }
        public string Eda { get; set; }
        public string Sexo { get; set; }
        public string Empresa { get; set; }
        public string Fecha { get; set; }
        public string FrecuenciaCardiaca { get; set; }
        public string RitmoCardiaco { get; set; }
        public string IntervaloPR { get; set; }
        public string ComplejoQRS { get; set; }
        public string IntervaloQTC { get; set; }
        public string EjeCardiaco { get; set; }
        public string Hallazgo { get; set; }
        public string Conclusiones { get; set; }
        public string Observacion { get; set; }
        public byte[] FirmaMedico { get; set; }
    }
}
