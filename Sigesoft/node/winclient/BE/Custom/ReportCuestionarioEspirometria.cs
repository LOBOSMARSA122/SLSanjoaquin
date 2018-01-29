using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sigesoft.Node.WinClient.BE
{
    public class ReportCuestionarioEspirometria
    {
        public string IdServicio { get; set; }
        public string IdComponent { get; set; }
        public DateTime? Fecha { get; set; }
        public string NombreTrabajador { get; set; }
        public DateTime? FechaNacimineto { get; set; }
        public int Edad { get; set; }
        public int Genero { get; set; }
        public string Pregunta1ASiNo { get; set; }
        public string Pregunta2ASiNo { get; set; }
        public string Pregunta3ASiNo { get; set; }
        public string Pregunta4ASiNo { get; set; }
        public string Pregunta5ASiNo { get; set; }


        public string HemoptisisSiNo { get; set; }
        public string PseumotoraxSiNo { get; set; }
        public string TraqueostomiaSiNo { get; set; }
        public string SondaPleuralSiNo { get; set; }
        public string AneurismaSiNo { get; set; }
        public string EmboliaSiNo { get; set; }
        public string InfartoSiNo { get; set; }
        public string InestabilidadSiNo { get; set; }
        public string FiebreSiNo { get; set; }
        public string EmbarazoAvanzadoSiNo { get; set; }
        public string EmbarazoComplicadoSiNo { get; set; }

        public string Pregunta1BSiNo { get; set; }
        public string Pregunta2BSiNo { get; set; }
        public string Pregunta3BSiNo { get; set; }
        public string Pregunta4BSiNo { get; set; }
        public string Pregunta5BSiNo { get; set; }
        public string Pregunta6BSiNo { get; set; }
        public string Pregunta7BSiNo { get; set; }

        public byte[] FirmaTrabajador { get; set; }
        public byte[] HuellaTrabajador { get; set; }

        public byte[] Logo { get; set; }

        public byte[] b_Logo { get; set; }
        public string EmpresaPropietaria { get; set; }
        public string EmpresaPropietariaDireccion { get; set; }
        public string EmpresaPropietariaTelefono { get; set; }
        public string EmpresaPropietariaEmail { get; set; }

        public byte[] b_File { get; set; }
        public string NombreUsuarioGraba { get; set; }
        public string EmpresaCliente { get; set; }
        public string Dni { get; set; }
        public string TipoExamen { get; set; }
        
    }
}
