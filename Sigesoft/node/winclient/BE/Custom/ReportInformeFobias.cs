using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sigesoft.Node.WinClient.BE
{
   public  class ReportInformeFobias
    {
       public string ServiceID { get; set; }
       public DateTime? FechaNacimiento { get; set; }
       public string APELLIDOS_NOMBRES {get;set;}
       public int? EDAD { get; set; }
       public string DNI { get; set; }

       public string PUESTO_TRABAJO { get; set; }
       public string EMPRESA_CLIENTE { get; set; }
       public byte[] LOGO_PROPIETARIA { get; set; }
       public string TIPO_EXAMEN { get; set; }
       public DateTime? FECHA_EXAMEN { get; set; }
       public string RESULTADOS { get; set; }
       public string RECOMENDACIONES { get; set; }
       public string CONCLUSION { get; set; }
       public string DX_EXAMEN { get; set; }
       public byte[] FIRMA { get; set; }
    }
}
