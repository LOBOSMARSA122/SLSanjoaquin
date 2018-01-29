using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sigesoft.Node.WinClient.BE
{
    public class ListaExamenes
    {
        public string ServiceId { get; set; }
        public string Paciente { get; set; }

        public string Documento { get; set; }
        public string Cargo { get; set; }
        public DateTime? FechaServicio { get; set; }
        public int? Edad { get; set; }

        public string  AcidoUrico_N009_ME000000086 { get; set; }
        public string AglutinacionesLamina_N009_ME000000025 { get; set; }
        public string AntigenoProstatico_N009_ME000000009 { get; set; }
        public string Bioquimica3_N009_ME000000088 { get; set; }
        public string Bioquimica2_N009_ME000000087 { get; set; }
        public string BkDirecto_N009_ME000000081 { get; set; }
        public string BkEsputo_N002_ME000000030 { get; set; }
        public string ColesterolHDL_N009_ME000000074 { get; set; }
        public string ColesterolLDL_N002_ME000000011 { get; set; }
        public string ColesterolLDL_N009_ME000000075 { get; set; }


        public string ColesterolTotal_N009_ME000000016 { get; set; }
        public string ColesterolVLDL_N009_ME000000076 { get; set; }
        public string CoprocultivoSalmonela_N002_ME000000018 { get; set; }
        public string Creatina_N009_ME000000028 { get; set; }
        public string ExamenCompletoOrina_N002_ME000000014 { get; set; }
        public string ExamenCompletoOrina_N009_ME000000046 { get; set; }
        public string VHI_N009_ME000000030 { get; set; }
        public string Fecatest_N009_ME000000097 { get; set; }
        public string Glucosa_N009_ME000000008 { get; set; }
        public string GrupoFactorSanguineo_N009_ME000000000 { get; set; }

        public string HAVIGM_N009_ME000000004 { get; set; }
        public string Hematocrito_N009_ME000000001 { get; set; }
        public string Hemoglobina_N009_ME000000006 { get; set; }
        public string HemogramaCompleto_N009_ME000000045 { get; set; }
        public string HisopadoFaringeo_N009_ME000000095 { get; set; }
        public string InmunoEnzima_N009_ME000000005 { get; set; }
        public string ParasitologicoSeriado_N009_ME000000049 { get; set; }
        public string ParasitologicoSimple_N009_ME000000010 { get; set; }
        public string PerfilHepatico_N009_ME000000096 { get; set; }
        public string PlomoSangre_N009_ME000000060 { get; set; }


        public string SeriologiaLues_N002_ME000000013 { get; set; }
        public string SubUniBetaCualitativo_N009_ME000000027 { get; set; }
        public string TGO_N009_ME000000054 { get; set; }
        public string TGP_N009_ME000000055 { get; set; }
        public string TIFICOH_N009_ME000000080 { get; set; }
        public string TIFICOO_N009_ME000000079 { get; set; }
        public string ToxicologicoAlcoholemia_N009_ME000000041 { get; set; }
        public string ToxicologicoAnfetaminas_N009_ME000000043 { get; set; }
        public string ToxicologicoBenzodiazepinas_N009_ME000000040 { get; set; }
        public string ToxicologicoCarboxihemoglobina_N002_ME000000042 { get; set; }


        public string ToxicologicoColinesterasa_N009_ME000000042 { get; set; }
        public string ToxicologicoCocainaMarihuana_N009_ME000000053 { get; set; }
        public string Trigliceridos_N002_ME000000012 { get; set; }
        public string Trigliceridos_N009_ME000000017 { get; set; }
        public string Urea_N009_ME000000073 { get; set; }
        public string VDRL_N009_ME000000003 { get; set; }
        public string Odontograma_N002_ME000000027 { get; set; }
        public string Electrocardiograma_N002_ME000000025 { get; set; }
        public string PruebaEsfuerzo_N002_ME000000029 { get; set; }
        public string EvaCardiologica_N009_ME000000092 { get; set; }


        public string RadiografiaTorax_N002_ME000000032 { get; set; }
        public string EcografiaRenal_N009_ME000000019 { get; set; }
        public string EcografiaProstata_N009_ME000000020 { get; set; }
        public string EcografiaAbdominal_N009_ME000000051 { get; set; }
        public string RadiografiaOIT_N009_ME000000062 { get; set; }
        public string InformePsicologico_N002_ME000000033 { get; set; }
        public string FuncionesVitales_N002_ME000000001 { get; set; }
        public string Antropometria_N002_ME000000002 { get; set; }
        public string ExamenFisico_N002_ME000000022 { get; set; }
        public string ExamenAlturaGeografica_N002_ME000000045 { get; set; }

        public string ExamenOsteomuscular_N002_ME000000046 { get; set; }
        public string ExamenAlturaEstructural_N009_ME000000015 { get; set; }
        public string CuestionarioActividadFisica_N009_ME000000018 { get; set; }
        public string TamizajeDermatologico_N009_ME000000044 { get; set; }
        public string ExamenFisico7C_N009_ME000000052 { get; set; }
        public string ExamenOsteomuscular2_N009_ME000000084 { get; set; }
        public string EvaluacionNeurologica_N009_ME000000085 { get; set; }
        public string CuestionarioNordicoOsteomuscular_N009_ME000000089 { get; set; }
        public string TestVertigo_N009_ME000000090 { get; set; }
        public string OsteoMuscular_N009_ME000000091 { get; set; }

        public string VacunaFiebreAmarilla_N009_ME000000063 { get; set; }
        public string VacunaInfluencia_N009_ME000000064 { get; set; }
        public string VacunaDifteria_N009_ME000000065 { get; set; }
        public string VacunaHepatitisA_N009_ME000000066 { get; set; }
        public string VacunaHepatitisB_N009_ME000000067 { get; set; }
        public string VacunaAntirrabica_N009_ME000000068 { get; set; }
        public string InfluenzaA1H1N1_N009_ME000000069 { get; set; }
        public string VacunaTriple_N009_ME000000070 { get; set; }
        public string VacunaVaricela_N009_ME000000071 { get; set; }
        public string Oftalmolgia_N002_ME000000028 { get; set; }

        public string TestOjoSeco_N009_ME000000083 { get; set; }
        public string Petrinovic_N009_ME000000098 { get; set; }
        public string Audiometria_N002_ME000000005 { get; set; }
        public string Espirometria_N002_ME000000031 { get; set; }
        public string Electroencefalograma_N009_ME000000099 { get; set; }


        public string Hemograma_N009_ME000000113 { get; set; }
      public string Perfil_Lipidico_N009_ME000000114 { get; set; }
    public string Microalbuminuria_N009_ME000000117 { get; set; }
    public string Hisopado_Nasofaringeo_N009_ME000000118 { get; set; }
    public string Reaccion_Inflamatoria_N009_ME000000119 { get; set; }
    public string HCV_N009_ME000000120 { get; set; }
    public string HBsAg_N009_ME000000121 { get; set; }
    public string ExamenKOH_N009_ME000000122 { get; set; }
    public string Tiempo_Sangria_N009_ME000000123 { get; set; }
    public string Tiempo_Coagulacion_N009_ME000000124 { get; set; }
    public string Insulina_Basal_N009_ME000000125 { get; set; }
    public string Toxicologico_N009_ME000000303 { get; set; }
    public string SubUnidad_BetaCualitativo_N009_ME000000027 { get; set; }


    public Decimal COSTO { get; set; }
    public Decimal IGV { get; set; }
    public Decimal TOTALSERVICIO { get; set; }

    }
}
