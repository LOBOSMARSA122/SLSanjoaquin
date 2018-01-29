using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sigesoft.Node.WinClient.BE
{
    public class ReportOsteoSJL
    {
        public string Nombre_Trabajador { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public int? Genero { get; set; }
        public byte[] FirmaTrabajador { get; set; }
        public byte[] HuellaTrabajador { get; set; }
        public int Edad { get; set; }
        public byte[] b_Logo { get; set; }
        public string EmpresaPropietaria { get; set; }
        public string EmpresaPropietariaDireccion { get; set; }
        public string EmpresaPropietariaTelefono { get; set; }
        public string EmpresaPropietariaEmail { get; set; }


        public string DNI { get; set; }
        public string ServiceId { get; set; }
        public DateTime? ServiceDate { get; set; }
        public string NombreEmpresaCliente { get; set; }
        public string PuestoTrabajo { get; set; }
        public int TipoESOId { get; set; }
        public string Dx { get; set; }
        public string NombreUsuarioGraba { get; set; }
        public string CMPUsuarioGraba { get; set; }
        public byte[] FirmaUsuarioGraba { get; set; }

        public string OSTEO_SJ_txtTIpoTrabajo {get; set;}// "N009-OSJ00000001";
        public string OSTEO_SJ_txtHorasTotales {get; set;}// "N009-OSJ00000002";
        public string OSTEO_SJ_txtHorasDia {get; set;}// "N009-OSJ00000003";
        public string OSTEO_SJ_ddlFrecuencia {get; set;}// "N009-OSJ00000004";
        public string OSTEO_SJ_ddlCiclo {get; set;}// "N009-OSJ00000005";
        public string OSTEO_SJ_ddlMovimientoDeAlcane {get; set;}// "N009-OSJ00000006";
        public string OSTEO_SJ_ddlElcuelloSeMantiene {get; set;}// "N009-OSJ00000007";
        public string OSTEO_SJ_ddlGirosDeColumna {get; set;}// "N009-OSJ00000008";
        public string OSTEO_SJ_ddlMovimientosDeFlexion {get; set;}// "N009-OSJ00000009";
        public string OSTEO_SJ_ddlCompresionDeNervio {get; set;}// "N009-OSJ00000010";
        public string OSTEO_SJ_ddlDesviacionesCubitales {get; set;}// "N009-OSJ00000011";
        public string OSTEO_SJ_ddlRotacionExternaCodo {get; set;}// "N009-OSJ00000012";
        public string OSTEO_SJ_ddlFlexionExternaCodo {get; set;}// "N009-OSJ00000013";
        public string OSTEO_SJ_ddlOtros {get; set;}// "N009-OSJ00000014";
        public string OSTEO_SJ_txtOtrosDescripcion {get; set;}// "N009-OSJ00000015";
        public string OSTEO_SJ_txtLevanta {get; set;}// "N009-OSJ00000016";
        public string OSTEO_SJ_txtColoca {get; set;}// "N009-OSJ00000017";
        public string OSTEO_SJ_txtEmpuja {get; set;}// "N009-OSJ00000018";
        public string OSTEO_SJ_txtTracciona {get; set;}// "N009-OSJ00000019";
        public string OSTEO_SJ_txtQueTiempoTiene {get; set;}// "N009-OSJ00000020";
        public string OSTEO_SJ_txtCuantasHorasDia {get; set;}// "N009-OSJ00000021";
        public string OSTEO_SJ_txtDesde {get; set;}// "N009-OSJ00000022";
        public string OSTEO_SJ_txtHasta {get; set;}// "N009-OSJ00000023";
        public string OSTEO_SJ_ddlEquilibrioInestable {get; set;}// "N009-OSJ00000024";
        public string OSTEO_SJ_ddlManipulacionDistancia {get; set;}// "N009-OSJ00000025";
        public string OSTEO_SJ_cboExigeTorsion {get; set;}// "N009-OSJ00000026";
        public string OSTEO_SJ_cboExistePosiBrusco {get; set;}// "N009-OSJ00000027";
        public string OSTEO_SJ_ddlCuerpoPosicion {get; set;}// "N009-OSJ00000028";
        public string OSTEO_SJ_ddlPosturaForzada {get; set;}// "N009-OSJ00000029";
        public string OSTEO_SJ_ddlDesnivelesSuelo {get; set;}// "N009-OSJ00000030";
        public string OSTEO_SJ_ddlTemperaturaHumedad {get; set;}// "N009-OSJ00000031";
        public string OSTEO_SJ_ddlVibraciones {get; set;}// "N009-OSJ00000032";
        public string OSTEO_SJ_ddlEsfuerzosFisicos {get; set;}// "N009-OSJ00000033";
        public string OSTEO_SJ_ddlDesnivelesSuelo02 {get; set;}// "N009-OSJ00000034";
        public string OSTEO_SJ_ddlTemperaturaHumedad02 {get; set;}// "N009-OSJ00000035";
        public string OSTEO_SJ_ddlVibraciones02 {get; set;}// "N009-OSJ00000036";
        public string OSTEO_SJ_ddlConoceCuales {get; set;}// "N009-OSJ00000037";
        public string OSTEO_SJ_ddlLasUsa {get; set;}// "N009-OSJ00000038";
        public string OSTEO_SJ_ddlHaSidoCapacitado {get; set;}// "N009-OSJ00000039";
        public string OSTEO_SJ_ddlNuca12Meses {get; set;}// "N009-OSJ00000040";
        public string OSTEO_SJ_ddlNucaUltMeses {get; set;}// "N009-OSJ00000041";
        public string OSTEO_SJ_ddlNuca7dias {get; set;}// "N009-OSJ00000042";
        public string OSTEO_SJ_ddlHombroDer12Meses {get; set;}// "N009-OSJ00000043";
        public string OSTEO_SJ_ddlHombroDerUltMeses {get; set;}// "N009-OSJ00000044";
        public string OSTEO_SJ_ddlHombroDer7dias {get; set;}// "N009-OSJ00000045";
        public string OSTEO_SJ_ddlHombroIzq12Meses {get; set;}// "N009-OSJ00000049";
        public string OSTEO_SJ_ddlHombroIzqUltMeses {get; set;}// "N009-OSJ00000050";
        public string OSTEO_SJ_ddlHombroIzq7dias {get; set;}// "N009-OSJ00000051";
        public string OSTEO_SJ_ddlAmbosHombros12Meses {get; set;}// "N009-OSJ00000052";
        public string OSTEO_SJ_ddlAmbosHombrosUltMeses {get; set;}// "N009-OSJ00000053";
        public string OSTEO_SJ_ddlAmbosHombros7dias {get; set;}// "N009-OSJ00000054";
        public string OSTEO_SJ_ddlCodoDer12Meses {get; set;}// "N009-OSJ00000055";
        public string OSTEO_SJ_ddlCodoDerUltMeses {get; set;}// "N009-OSJ00000056";
        public string OSTEO_SJ_ddlCodoDer7dias {get; set;}// "N009-OSJ00000057";
        public string OSTEO_SJ_ddlCodoIzq12Meses {get; set;}// "N009-OSJ00000058";
        public string OSTEO_SJ_ddlCodoIzqUltMeses {get; set;}// "N009-OSJ00000059";
        public string OSTEO_SJ_ddlCodoIzq7dias {get; set;}// "N009-OSJ00000060";
        public string OSTEO_SJ_ddlAmbosCodos12Meses {get; set;}// "N009-OSJ00000061";
        public string OSTEO_SJ_ddlAmbosCodosUltMeses {get; set;}// "N009-OSJ00000062";
        public string OSTEO_SJ_ddlAmbosCodos7dias {get; set;}// "N009-OSJ00000063";
        public string OSTEO_SJ_ddlManoDer12Meses {get; set;}// "N009-OSJ00000064";
        public string OSTEO_SJ_ddlManoDerUltMeses {get; set;}// "N009-OSJ00000065";
        public string OSTEO_SJ_ddlManoDer7dias {get; set;}// "N009-OSJ00000066";
        public string OSTEO_SJ_ddlManoIzq12Meses {get; set;}// "N009-OSJ00000067";
        public string OSTEO_SJ_ddlManoIzqUltMeses {get; set;}// "N009-OSJ00000068";
        public string OSTEO_SJ_ddlManoIzq7dias {get; set;}// "N009-OSJ00000069";
        public string OSTEO_SJ_ddlAmbasManos12Meses {get; set;}// "N009-OSJ00000070";
        public string OSTEO_SJ_ddlAmbasManosUltMeses {get; set;}// "N009-OSJ00000071";
        public string OSTEO_SJ_ddlAmbasManos7dias {get; set;}// "N009-OSJ00000072";
        public string OSTEO_SJ_ddlColumnaDorsal12Meses {get; set;}// "N009-OSJ00000073";
        public string OSTEO_SJ_ddlClomunaDorsalUltMeses {get; set;}// "N009-OSJ00000074";
        public string OSTEO_SJ_ddlColumnaDorsal7dias {get; set;}// "N009-OSJ00000075";
        public string OSTEO_SJ_ddlColumnaLumbar12Meses {get; set;}// "N009-OSJ00000076";
        public string OSTEO_SJ_ddlClñoumnaLumbarUltMeses {get; set;}// "N009-OSJ00000077";
        public string OSTEO_SJ_ddlColumnaLumbar7dias {get; set;}// "N009-OSJ00000078";
        public string OSTEO_SJ_ddlCaderaDer12Meses {get; set;}// "N009-OSJ00000079";
        public string OSTEO_SJ_ddlCaderaDerUltMeses {get; set;}// "N009-OSJ00000080";
        public string OSTEO_SJ_ddlCaderaDer7dias {get; set;}// "N009-OSJ00000081";
        public string OSTEO_SJ_ddlCaderaIzq12Meses {get; set;}// "N009-OSJ00000082";
        public string OSTEO_SJ_ddlCaderaIzqUltMeses {get; set;}// "N009-OSJ00000083";
        public string OSTEO_SJ_ddlCaderaIzq7dias {get; set;}// "N009-OSJ00000084";
        public string OSTEO_SJ_ddlRodillaDer12Meses {get; set;}// "N009-OSJ00000085";
        public string OSTEO_SJ_ddlRodillaDerUltMeses {get; set;}// "N009-OSJ00000086";
        public string OSTEO_SJ_ddlRodillaDer7dias {get; set;}// "N009-OSJ00000087";
        public string OSTEO_SJ_ddlRodillaIzq12Meses {get; set;}// "N009-OSJ00000088";
        public string OSTEO_SJ_ddlRodillaIzqUltMeses {get; set;}// "N009-OSJ00000089";
        public string OSTEO_SJ_ddlRodillaIzq7dias {get; set;}// "N009-OSJ00000090";
        public string OSTEO_SJ_ddlTobilloDer12Meses {get; set;}// "N009-OSJ00000091";
        public string OSTEO_SJ_dllTobilloDerUltMeses {get; set;}// "N009-OSJ00000092";
        public string OSTEO_SJ_ddlTobilloDer7dias {get; set;}// "N009-OSJ00000093";
        public string OSTEO_SJ_ddlTobilloIzq12Meses {get; set;}// "N009-OSJ00000094";
        public string OSTEO_SJ_ddlTobilloIzqUltMeses {get; set;}// "N009-OSJ00000095";
        public string OSTEO_SJ_ddlTobilloIzq7dias {get; set;}// "N009-OSJ00000096";
        public string OSTEO_SJ_txtTieneAntecedente {get; set;}// "N009-OSJ00000097";
        public string OSTEO_SJ_txtAPadecido {get; set;}// "N009-OSJ00000098";
        public string OSTEO_SJ_txtDeLasEnfermedades {get; set;}// "N009-OSJ00000099";
        public string OSTEO_SJ_txtQueDeportes {get; set;}// "N009-OSJ00000100";
        public string OSTEO_SJ_txtHobbyOAlguna {get; set;}// "N009-OSJ00000101";
        public string OSTEO_SJ_txtAnamnesis {get; set;}// "N009-OSJ00000102";
        public string OSTEO_SJ_ddlCervicalAP {get; set;}// "N009-OSJ00000103";
        public string OSTEO_SJ_ddlDorsalAP {get; set;}// "N009-OSJ00000104";
        public string OSTEO_SJ_ddlLumbarAP {get; set;}// "N009-OSJ00000105";
        public string OSTEO_SJ_ddlDOrsalLAT {get; set;}// "N009-OSJ00000106";
        public string OSTEO_SJ_ddlLumbarLAT {get; set;}// "N009-OSJ00000107";
        public string OSTEO_SJ_txtCervicalApofisis {get; set;}// "N009-OSJ00000108";
        public string OSTEO_SJ_txtCervicalContract {get; set;}// "N009-OSJ00000109";
        public string OSTEO_SJ_txtDorsalApofisis {get; set;}// "N009-OSJ00000110";
        public string OSTEO_SJ_txtDorsalContract {get; set;}// "N009-OSJ00000111";
        public string OSTEO_SJ_txtLumbarApofisis {get; set;}// "N009-OSJ00000112";
        public string OSTEO_SJ_txtLumbarContract {get; set;}// "N009-OSJ00000113";
        public string OSTEO_SJ_ddlLasegeDer {get; set;}// "N009-OSJ00000114";
        public string OSTEO_SJ_ddlLasegueIzq {get; set;}// "N009-OSJ00000115";
        public string OSTEO_SJ_ddlBragardDer {get; set;}// "N009-OSJ00000116";
        public string OSTEO_SJ_ddlBragardIzq {get; set;}// "N009-OSJ00000117";
        public string OSTEO_SJ_ddlPhalenDer {get; set;}// "N009-OSJ00000118";
        public string OSTEO_SJ_ddlPhalenIzq {get; set;}// "N009-OSJ00000119";
        public string OSTEO_SJ_ddlTinelDer {get; set;}// "N009-OSJ00000120";
        public string OSTEO_SJ_ddlTinelIzq {get; set;}// "N009-OSJ00000121";
        public string OSTEO_SJ_txtSchoberReposo {get; set;}// "N009-OSJ00000122";
        public string OSTEO_SJ_txtSchoberExtension {get; set;}// "N009-OSJ00000123";
        public string OSTEO_SJ_ddlRodillaDerVaro {get; set;}// "N009-OSJ00000124";
        public string OSTEO_SJ_ddlRodillaIzqVaro {get; set;}// "N009-OSJ00000125";
        public string OSTEO_SJ_ddlPieDerVaro {get; set;}// "N009-OSJ00000126";
        public string OSTEO_SJ_ddlPieIzqVaro {get; set;}// "N009-OSJ00000127";
        public string OSTEO_SJ_ddlCodoDerVaro {get; set;}// "N009-OSJ00000128";
        public string OSTEO_SJ_ddlCOdoIzqVaro {get; set;}// "N009-OSJ00000129";
        public string OSTEO_SJ_ddlHombroDerAdd {get; set;}// "N009-OSJ00000130";
        public string OSTEO_SJ_ddlHombroIzqAdd {get; set;}// "N009-OSJ00000131";
        public string OSTEO_SJ_ddlCodoDerAdd {get; set;}// "N009-OSJ00000132";
        public string OSTEO_SJ_ddlCodoIzqAdd {get; set;}// "N009-OSJ00000133";
        public string OSTEO_SJ_ddlMuñecaDerAdd {get; set;}// "N009-OSJ00000134";
        public string OSTEO_SJ_ddlMuñecaIzqAdd {get; set;}// "N009-OSJ00000135";
        public string OSTEO_SJ_ddlCaderaDerAdd {get; set;}// "N009-OSJ00000136";
        public string OSTEO_SJ_ddlCaderaIzqAdd {get; set;}// "N009-OSJ00000137";
        public string OSTEO_SJ_ddlRodillaDerAdd {get; set;}// "N009-OSJ00000138";
        public string OSTEO_SJ_ddlRodillaIzqAdd {get; set;}// "N009-OSJ00000139";
        public string OSTEO_SJ_ddlTobilloDerAdd {get; set;}// "N009-OSJ00000140";
        public string OSTEO_SJ_ddlTobilloIzqAdd {get; set;}// "N009-OSJ00000141";
        public string OSTEO_SJ_ddlHombroDerFlex {get; set;}// "N009-OSJ00000142";
        public string OSTEO_SJ_ddlHombroIzqFlex {get; set;}// "N009-OSJ00000143";
        public string OSTEO_SJ_ddlCodoDerFlex {get; set;}// "N009-OSJ00000144";
        public string OSTEO_SJ_ddlCodoIzqFlex {get; set;}// "N009-OSJ00000145";
        public string OSTEO_SJ_ddlMuñecaDerFlex {get; set;}// "N009-OSJ00000146";
        public string OSTEO_SJ_ddlMuñecaIzqFlex {get; set;}// "N009-OSJ00000147";
        public string OSTEO_SJ_ddlCaderaDerFLex {get; set;}// "N009-OSJ00000148";
        public string OSTEO_SJ_ddlCaderaIzqFLex {get; set;}// "N009-OSJ00000149";
        public string OSTEO_SJ_ddlRodillaDerFLex {get; set;}// "N009-OSJ00000150";
        public string OSTEO_SJ_ddlRodillaIzqFlex {get; set;}// "N009-OSJ00000151";
        public string OSTEO_SJ_ddlTobilloDerFlex {get; set;}// "N009-OSJ00000152";
        public string OSTEO_SJ_ddlTobilloIzqFlex {get; set;}// "N009-OSJ00000153";
        public string OSTEO_SJ_ddlHombroDerExt {get; set;}// "N009-OSJ00000154";
        public string OSTEO_SJ_ddlHombroIzqExt {get; set;}// "N009-OSJ00000155";
        public string OSTEO_SJ_ddlCodoDerExt {get; set;}// "N009-OSJ00000156";
        public string OSTEO_SJ_ddlCodoIzqExt {get; set;}// "N009-OSJ00000157";
        public string OSTEO_SJ_ddlMuñecaDerExt {get; set;}// "N009-OSJ00000158";
        public string OSTEO_SJ_ddlMuñecaIzqExt {get; set;}// "N009-OSJ00000159";
        public string OSTEO_SJ_ddlCaderaDerExt {get; set;}// "N009-OSJ00000160";
        public string OSTEO_SJ_ddlCaderaIzqExt {get; set;}// "N009-OSJ00000161";
        public string OSTEO_SJ_ddlRodillaDerExt {get; set;}// "N009-OSJ00000162";
        public string OSTEO_SJ_ddlRodillaIzqExt {get; set;}// "N009-OSJ00000163";
        public string OSTEO_SJ_ddlTobilloDerExt {get; set;}// "N009-OSJ00000164";
        public string OSTEO_SJ_ddlTobilloIzqExt {get; set;}// "N009-OSJ00000165";
        public string OSTEO_SJ_ddlHombroDerRotExt {get; set;}// "N009-OSJ00000166";
        public string OSTEO_SJ_ddlHombroIzqRotExt {get; set;}// "N009-OSJ00000167";
        public string OSTEO_SJ_ddlCodoDerRotExt {get; set;}// "N009-OSJ00000168";
        public string OSTEO_SJ_ddlCodoIzqRotExt {get; set;}// "N009-OSJ00000169";
        public string OSTEO_SJ_ddlMuñecaDerRotExt {get; set;}// "N009-OSJ00000170";
        public string OSTEO_SJ_ddlMuñecaIzqRotExt {get; set;}// "N009-OSJ00000171";
        public string OSTEO_SJ_ddlCaderaDerRotExt {get; set;}// "N009-OSJ00000172";
        public string OSTEO_SJ_ddlCaderaIzqRotExt {get; set;}// "N009-OSJ00000173";
        public string OSTEO_SJ_ddlRodillaDerRotExt {get; set;}// "N009-OSJ00000174";
        public string OSTEO_SJ_ddlRodillaIzqRotExt {get; set;}// "N009-OSJ00000175";
        public string OSTEO_SJ_ddlTobilloDerRotExt {get; set;}// "N009-OSJ00000176";
        public string OSTEO_SJ_ddlTobilloIzqRotExt {get; set;}// "N009-OSJ00000177";
        public string OSTEO_SJ_ddlHombroDerRotInt {get; set;}// "N009-OSJ00000178";
        public string OSTEO_SJ_ddlHombroIzqRotInt {get; set;}// "N009-OSJ00000179";
        public string OSTEO_SJ_ddlCodoDerRotInt {get; set;}// "N009-OSJ00000180";
        public string OSTEO_SJ_ddlCodoIzqRotInt {get; set;}// "N009-OSJ00000181";
        public string OSTEO_SJ_ddlMuñecaDerRotInt {get; set;}// "N009-OSJ00000182";
        public string OSTEO_SJ_ddlMuñecaIzqRotInt {get; set;}// "N009-OSJ00000183";
        public string OSTEO_SJ_ddlCaderaDerRotInt {get; set;}// "N009-OSJ00000184";
        public string OSTEO_SJ_ddlCaderaIzqRotInt {get; set;}// "N009-OSJ00000185";
        public string OSTEO_SJ_ddlRodillaDerRotInt {get; set;}// "N009-OSJ00000186";
        public string OSTEO_SJ_ddlRodillaIzqRotInt {get; set;}// "N009-OSJ00000187";
        public string OSTEO_SJ_ddlTobilloDerRotInt {get; set;}// "N009-OSJ00000188";
        public string OSTEO_SJ_ddlTobilloIzqRotInt {get; set;}// "N009-OSJ00000189";
        public string OSTEO_SJ_ddlHombroDerIrrad {get; set;}// "N009-OSJ00000190";
        public string OSTEO_SJ_ddlHombroIzqIrrad {get; set;}// "N009-OSJ00000191";
        public string OSTEO_SJ_ddlCodoDerIrrad {get; set;}// "N009-OSJ00000192";
        public string OSTEO_SJ_ddlCodoIzqIrrad {get; set;}// "N009-OSJ00000193";
        public string OSTEO_SJ_ddlMuñecaDerIrrad {get; set;}// "N009-OSJ00000194";
        public string OSTEO_SJ_ddlMuñecaIzqIrrad {get; set;}// "N009-OSJ00000195";
        public string OSTEO_SJ_ddlCaderaDerIrrad {get; set;}// "N009-OSJ00000196";
        public string OSTEO_SJ_ddlCaderaIzqIrrad {get; set;}// "N009-OSJ00000197";
        public string OSTEO_SJ_ddlRodillaDerIrrad {get; set;}// "N009-OSJ00000198";
        public string OSTEO_SJ_ddlRodillaIzqIrrad {get; set;}// "N009-OSJ00000199";
        public string OSTEO_SJ_ddlTobilloDerIrrad {get; set;}// "N009-OSJ00000200";
        public string OSTEO_SJ_ddlTobilloIzqIrrad {get; set;}// "N009-OSJ00000201";
        public string OSTEO_SJ_ddlHombroDerMasaMusc {get; set;}// "N009-OSJ00000202";
        public string OSTEO_SJ_ddlHombroIzqMasaMusc {get; set;}// "N009-OSJ00000203";
        public string OSTEO_SJ_ddlCodoDerMasaMusc {get; set;}// "N009-OSJ00000204";
        public string OSTEO_SJ_ddlCodoIzqMasaMusc {get; set;}// "N009-OSJ00000205";
        public string OSTEO_SJ_ddlMuñecaDerMasaMusc {get; set;}// "N009-OSJ00000206";
        public string OSTEO_SJ_ddlMuñecaIzqMasaMusc {get; set;}// "N009-OSJ00000207";
        public string OSTEO_SJ_ddlCaderaDerMasaMusc {get; set;}// "N009-OSJ00000208";
        public string OSTEO_SJ_ddlCaderaIzqMasaMusc {get; set;}// "N009-OSJ00000209";
        public string OSTEO_SJ_ddlRodillaDerMasaMusc {get; set;}// "N009-OSJ00000210";
        public string OSTEO_SJ_ddlRodillaIzqMasaMusc {get; set;}// "N009-OSJ00000211";
        public string OSTEO_SJ_ddlTobilloDerMasaMusc {get; set;}// "N009-OSJ00000212";
        public string OSTEO_SJ_ddlTobilloIzqMasaMusc {get; set;}// "N009-OSJ00000212";
        public string OSTEO_SJ_txtObservacion {get; set;}// "N009-OSJ00000212";
        public string OSTEO_SJ_ddlAptitud {get; set;}// "N009-OSJ00000212";
    }
}
