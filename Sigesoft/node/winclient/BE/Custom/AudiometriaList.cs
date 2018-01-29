using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sigesoft.Node.WinClient.BE
{
    public class AudiometriaList
    {      
        public string v_PersonId { get; set; }
        public string v_FullPersonName { get; set; }
        public string v_DocNumber { get; set; }
        public int? i_SexTypeId { get; set; }
        public string v_SexType { get; set; }
        public DateTime? d_BirthDate { get; set; }
        public DateTime? d_ServiceDate { get; set; }
        public string Puesto { get; set; }

        public byte[] FirmaTrabajador { get; set; }
        public byte[] HuellaTrabajador { get; set; }
        public byte[] FirmaMedico { get; set; }
        public byte[] FirmaTecnologo { get; set; }
        public int i_AgePacient { get; set; }

        // Requisitos para la audiometria

        public string CambiosAltitud { get; set; }
        public string ExpuestoRuido { get; set; }
        public string ProcesoInfeccioso { get; set; }
        public string DurmioNochePrevia { get; set; }
        public string ConsumioAlcoholDiaPrevio { get; set; }
      

        // Antecedentes Medicos de importancia

        public string RinitisSinusitis { get; set; }
        public string UsoMedicamentos { get; set; }
        public string Sarampion { get; set; }
        public string Tec { get; set; }
        public string OtitisMediaCronica { get; set; }
        public string DiabetesMellitus { get; set; }
        public string SorderaAntecedente { get; set; }
        public string SorderaFamiliar { get; set; }
        public string Meningitis { get; set; }
        public string Dislipidemia { get; set; }
        public string EnfTiroidea { get; set; }
        public string SustQuimicas { get; set; }
      

        // Hobbies

        public string UsoMP3 { get; set; }
        public string PracticaTiro { get; set; }
        public string Otros { get; set; }

        // Sintomas actuales

        public string Sordera { get; set; }
        public string Otalgia { get; set; }
        public string Acufenos { get; set; }
        public string SecrecionOtica { get; set; }
        public string Vertigos { get; set; }

        // Otoscopia

        public string OidoIzquierdo { get; set; }
        public string OidoDerecho { get; set; }

        // Dx automaticos
        public string DX_OIDO_DERECHO { get; set; }
        public string DX_OIDO_IZQUIERDO { get; set; }
        public string v_RecomendationName { get; set; }


        public byte[] b_Logo { get; set; }
        public string EmpresaPropietaria { get; set; }
        public string EmpresaPropietariaDireccion { get; set; }
        public string EmpresaPropietariaTelefono { get; set; }
        public string EmpresaPropietariaEmail { get; set; }

        public string v_ServiceComponentId { get; set; }
        public string v_EsoTypeName { get; set; }
        public string v_WorkingOrganizationName { get; set; }
        public string v_FullWorkingOrganizationName { get; set; }

        public string MarcaAudiometria { get; set; }
        public string ModeloAudiometria { get; set; }
        public string CalibracionAudiometria { get; set; }

        public string TiempoTrabajo { get; set; }
        public string NombreUsuarioGraba { get; set; }

        public string Dx { get; set; }
        public string Recomendaciones { get; set; }
    }
}
