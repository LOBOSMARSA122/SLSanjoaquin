using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sigesoft.Server.WebClientAdmin.BE
{
   public  class PersonMedicalHistoryList
    {
        public string Enfermedad { get; set; }
        public string v_PersonId { get; set; }
        public string v_DiseasesId { get; set; }
        public string v_FechaInicio { get; set; }
        public bool AsociadoTrabajo { get; set; }
        public int? i_AsociadoTrabajo { get; set; }
        public string v_DiagnosticDetail { get; set; }
        public string v_TreatmentSite { get; set; }
        public string v_PersonMedicalHistoryId { get; set; }
    }
}
