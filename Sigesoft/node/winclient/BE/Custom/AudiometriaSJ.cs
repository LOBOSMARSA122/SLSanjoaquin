using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sigesoft.Node.WinClient.BE
{
    public class AudiometriaSJ
   
    {
        public string v_PersonId { get; set; }
        public string NOMBRE_PACIENTE { get; set; }
        public int EDAD_PACIENTE { get; set; }
        public DateTime? FECHA_HORA_EXAMEN { get; set; }
        public string EMPRESA_CLIENTE { get; set; }
        public string PUESTO_TRABAJO { get; set; }
        public string NRO_SERVICE_ID { get; set; }
        public string NRO_SERVICECOMPONENT_ID { get; set; }
        public byte[] LOGO { get; set; }
        public byte[] FirmaMedico { get; set; }
        public DateTime? d_BirthDate { get; set; }

        public string HORAS_AL_DIA { get; set; }
        public string ANIOS { get; set; }

        public string PROTECTOR_AUDITIVO { get; set; }
        public string APRECIACION_DEL_RUIDO { get; set; }
        public string OTOSCOPIA_O_D { get; set; }
        public string OTOSCOPIA_O_I { get; set; }
        public string CONSUMO_DE_TABACO { get; set; }
        public string SERVICIO_MILITAR { get; set; }
        public string HOBBIES_CON_EXPOSICION { get; set; }
        public string EXPOSICION_LABORAL { get; set; }
        public string INFECCION_EN_OIDOS { get; set; }
        public string TRAUMA_ACUSTICO { get; set; }

        public string USO_DE_OTOTOXICOS { get; set; }
        public string SARAMPION_MENINGITIS { get; set; }
        public string DISMINUCION_AUDICION { get; set; }
        public string DOLOR_EN_OIDOS { get; set; }
        public string ZUMBIDO { get; set; }
        public string MAREOS_VERTIGOS { get; set; }
        public string INFECCION_EN_OIDO { get; set; }
        public string INFECCION_RESPIRATORIA_AGUDA { get; set; }
        public string EXP_ULT_HRS_RUIDOS { get; set; }
        public string OTROS { get; set; }
        public string DX { get; set; }
    }
}
