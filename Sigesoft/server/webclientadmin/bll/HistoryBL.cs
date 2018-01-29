using Sigesoft.Server.WebClientAdmin.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sigesoft.Server.WebClientAdmin.BLL
{
    public class HistoryBL
    {
        public List<Sigesoft.Node.WinClient.BE.HistoryList> GetHistoryReport(string pstrPersonId)
        {
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
                var query = (from A in dbContext.history
                             //join B in dbContext.workstationdangers on A.v_HistoryId equals B.v_HistoryId
                             //join B1 in dbContext.systemparameter on new { a = B.i_DangerId.Value, b = 145 } equals new { a = B1.i_ParameterId, b = B1.i_GroupId }
                             //join C in dbContext.typeofeep on A.v_HistoryId equals C.v_HistoryId
                             //join C1 in dbContext.systemparameter on new { a = C.i_TypeofEEPId.Value, b = 146 } equals new { a = C1.i_ParameterId, b = C1.i_GroupId }
                             join D in dbContext.systemparameter on new { a = A.i_TypeOperationId.Value, b = 204 } equals new { a = D.i_ParameterId, b = D.i_GroupId }
                             where A.i_IsDeleted == 0 && A.v_PersonId == pstrPersonId
                             //&& B.i_IsDeleted == 0 && C.i_IsDeleted == 0

                             select new Sigesoft.Node.WinClient.BE.HistoryList
                             {
                                 v_HistoryId = A.v_HistoryId,
                                 d_StartDate = A.d_StartDate,
                                 d_EndDate = A.d_EndDate,
                                 v_Organization = A.v_Organization,
                                 v_TypeActivity = A.v_TypeActivity,
                                 i_GeografixcaHeight = A.i_GeografixcaHeight,
                                 v_workstation = A.v_workstation,
                                 d_CreationDate = A.d_InsertDate,
                                 d_UpdateDate = A.d_UpdateDate,
                                 b_FingerPrintImage = A.b_FingerPrintImage,
                                 b_RubricImage = A.b_RubricImage,
                                 t_RubricImageText = A.t_RubricImageText,
                                 v_TypeOperationName = D.v_Value1
                             }).ToList();
                var q = (from a in query
                         let xxx = new ServiceBL().GetYearsAndMonth(a.d_EndDate, a.d_StartDate)
                         select new Sigesoft.Node.WinClient.BE.HistoryList
                         {
                             v_HistoryId = a.v_HistoryId,
                             d_StartDate = a.d_StartDate,
                             d_EndDate = a.d_EndDate,
                             v_Organization = a.v_Organization,
                             v_TypeActivity = a.v_TypeActivity,
                             i_GeografixcaHeight = a.i_GeografixcaHeight,
                             v_workstation = a.v_workstation,
                             d_CreationDate = a.d_CreationDate,
                             d_UpdateDate = a.d_UpdateDate,
                             b_FingerPrintImage = a.b_FingerPrintImage,
                             b_RubricImage = a.b_RubricImage,
                             t_RubricImageText = a.t_RubricImageText,
                             Fecha = "Fecha Inicio: " + a.d_StartDate.ToString().Substring(4, 7) + "  Fecha Fin: " + a.d_EndDate.ToString().Substring(4, 7),
                             Exposicion = ConcatenateExposiciones(a.v_HistoryId),
                             Epps = ConcatenateEpps(a.v_HistoryId),
                             v_TypeOperationName = a.v_TypeOperationName,
                             TiempoLabor = xxx
                         }).ToList();
                List<Sigesoft.Node.WinClient.BE.HistoryList> objData = q.ToList();
                return objData;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private string ConcatenateExposiciones(string pstrHistoryId)
        {
            SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

            var qry = (from a in dbContext.workstationdangers
                       join B1 in dbContext.systemparameter on new { a = a.i_DangerId.Value, b = 145 } equals new { a = B1.i_ParameterId, b = B1.i_GroupId }
                       where a.v_HistoryId == pstrHistoryId &&
                       a.i_IsDeleted == 0
                       select new
                       {
                           v_Exposicion = B1.v_Value1
                       }).ToList();

            return string.Join(", ", qry.Select(p => p.v_Exposicion));
        }

        private string ConcatenateEpps(string pstrHistoryId)
        {
            SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

            var qry = (from a in dbContext.typeofeep
                       join C1 in dbContext.systemparameter on new { a = a.i_TypeofEEPId.Value, b = 146 } equals new { a = C1.i_ParameterId, b = C1.i_GroupId }
                       where a.v_HistoryId == pstrHistoryId &&
                       a.i_IsDeleted == 0
                       select new
                       {
                           v_Epps = C1.v_Value1
                       }).ToList();

            return string.Join(", ", qry.Select(p => p.v_Epps));
        }

        public List<Sigesoft.Node.WinClient.BE.FamilyMedicalAntecedentsList> GetFamilyMedicalAntecedentsReport(string pstrPersonId)
        {
            //mon.IsActive = true;
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
                var query = (from A in dbContext.familymedicalantecedents
                             join B in dbContext.systemparameter on new { a = A.i_TypeFamilyId.Value, b = 149 }
                                                            equals new { a = B.i_ParameterId, b = B.i_GroupId } into B_join
                             from B in B_join.DefaultIfEmpty()
                             join C in dbContext.systemparameter on new { a = B.i_ParentParameterId.Value, b = 149 }
                                                          equals new { a = C.i_ParameterId, b = C.i_GroupId } into C_join
                             from C in C_join.DefaultIfEmpty()
                             join D in dbContext.diseases on new { a = A.v_DiseasesId }
                                                    equals new { a = D.v_DiseasesId } into D_join
                             from D in D_join.DefaultIfEmpty()

                             where A.i_IsDeleted == 0 && A.v_PersonId == pstrPersonId

                             select new Sigesoft.Node.WinClient.BE.FamilyMedicalAntecedentsList
                             {
                                 v_FamilyMedicalAntecedentsId = A.v_FamilyMedicalAntecedentsId,
                                 v_PersonId = A.v_PersonId,
                                 v_DiseasesId = A.v_DiseasesId,
                                 v_DiseaseName = D.v_Name,
                                 //i_TypeFamilyId = A.i_TypeFamilyId.Value,
                                 i_TypeFamilyId = C.i_ParameterId,
                                 v_TypeFamilyName = C.v_Value1,
                                 v_Comment = A.v_Comment,
                                 v_FullAntecedentName = D.v_Name + " / " + C.v_Value1 + ", " + A.v_Comment,
                                 DxAndComment = D.v_Name + "," + A.v_Comment
                             }).ToList();

                // add the sequence number on the fly
                var query1 = query.Select((x, index) => new Sigesoft.Node.WinClient.BE.FamilyMedicalAntecedentsList
                {
                    i_Item = index + 1,
                    v_FamilyMedicalAntecedentsId = x.v_FamilyMedicalAntecedentsId,
                    v_PersonId = x.v_PersonId,
                    v_DiseasesId = x.v_DiseasesId,
                    v_DiseaseName = x.v_DiseaseName,
                    i_TypeFamilyId = x.i_TypeFamilyId,
                    v_TypeFamilyName = x.v_TypeFamilyName,
                    v_Comment = x.v_Comment,
                    v_FullAntecedentName = x.v_FullAntecedentName,
                    DxAndComment = x.DxAndComment
                }).ToList();

                return query1;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<Sigesoft.Node.WinClient.BE.PersonMedicalHistoryList> GetPersonMedicalHistoryReport(string pstrPersonId)
        {
            //mon.IsActive = true;
            try
            {


                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
                var query = (from A in dbContext.personmedicalhistory

                             join B in dbContext.systemparameter on new { a = A.v_DiseasesId, b = 147 }
                                                               equals new { a = B.v_Value1, b = B.i_GroupId } into B_join
                             from B in B_join.DefaultIfEmpty()

                             join C in dbContext.systemparameter on new { a = B.i_ParentParameterId.Value, b = 147 }
                                                               equals new { a = C.i_ParameterId, b = C.i_GroupId } into C_join
                             from C in C_join.DefaultIfEmpty()

                             join D in dbContext.diseases on A.v_DiseasesId equals D.v_DiseasesId

                             join E in dbContext.systemparameter on new { a = A.i_TypeDiagnosticId.Value, b = 139 }
                                                               equals new { a = E.i_ParameterId, b = E.i_GroupId } into E_join
                             from E in E_join.DefaultIfEmpty()

                             where (A.i_IsDeleted == 0) && (A.v_PersonId == pstrPersonId)

                             select new Sigesoft.Node.WinClient.BE.PersonMedicalHistoryList
                             {
                                 v_PersonMedicalHistoryId = A.v_PersonMedicalHistoryId,
                                 v_PersonId = A.v_PersonId,
                                 v_DiseasesId = A.v_DiseasesId,
                                 v_DiseasesName = D.v_Name,
                                 i_TypeDiagnosticId = A.i_TypeDiagnosticId,
                                 d_StartDate = A.d_StartDate.Value,
                                 v_TreatmentSite = A.v_TreatmentSite,
                                 v_GroupName = C.v_Value1,
                                 v_TypeDiagnosticName = E.v_Value1,
                                 v_DiagnosticDetail = A.v_DiagnosticDetail,
                                 i_Answer = A.i_AnswerId.Value,

                             }).ToList();

                // add the sequence number on the fly
                var query1 = new List<Sigesoft.Node.WinClient.BE.PersonMedicalHistoryList>();

                query1 = query.Select((x, index) => new Sigesoft.Node.WinClient.BE.PersonMedicalHistoryList
                {
                    i_Item = index + 1,
                    v_PersonMedicalHistoryId = x.v_PersonMedicalHistoryId,
                    v_PersonId = x.v_PersonId,
                    v_DiseasesId = x.v_DiseasesId,
                    v_DiseasesName = x.v_DiseasesName,
                    i_TypeDiagnosticId = x.i_TypeDiagnosticId,
                    d_StartDate = x.d_StartDate,
                    v_TreatmentSite = x.v_TreatmentSite,
                    v_GroupName = x.v_GroupName,
                    v_TypeDiagnosticName = x.v_TypeDiagnosticName,
                    v_DiagnosticDetail = x.v_DiagnosticDetail,
                    i_Answer = x.i_Answer,
                }).ToList();

                //List<PersonMedicalHistoryList> objData = query.ToList();

                return query1;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<Sigesoft.Node.WinClient.BE.NoxiousHabitsList> GetNoxiousHabitsReport(string pstrPersonId)
        {
            //mon.IsActive = true;
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
                var query = (from A in dbContext.noxioushabits
                             join B in dbContext.systemparameter on new { a = A.i_TypeHabitsId.Value, b = 148 }
                                                            equals new { a = B.i_ParameterId, b = B.i_GroupId } into B_join
                             from B in B_join.DefaultIfEmpty()

                             where A.i_IsDeleted == 0 && A.v_PersonId == pstrPersonId

                             select new Sigesoft.Node.WinClient.BE.NoxiousHabitsList
                             {
                                 v_NoxiousHabitsId = A.v_NoxiousHabitsId,
                                 v_NoxiousHabitsName = B.v_Value1,
                                 v_PersonId = A.v_PersonId,
                                 v_Frequency = A.v_Frequency + ", " + A.v_Comment,
                                 v_Comment = A.v_Comment,
                                 i_TypeHabitsId = B.i_ParameterId,
                                 v_TypeHabitsName = B.v_Value1,
                                 i_RecordStatus = 0,// grabado
                                 i_RecordType = 2,// no temporal
                                 v_DescriptionQuantity = A.v_DescriptionQuantity,
                                 v_DescriptionHabit = A.v_DescriptionHabit,
                                 v_FrecuenciaHabito = A.v_Frequency

                             }).ToList();


                // add the sequence number on the fly
                var query1 = query.Select((x, index) => new Sigesoft.Node.WinClient.BE.NoxiousHabitsList
                {
                    i_Item = index + 1,
                    v_NoxiousHabitsId = x.v_NoxiousHabitsId,
                    v_NoxiousHabitsName = x.v_NoxiousHabitsName,
                    v_PersonId = x.v_PersonId,
                    v_Frequency = x.v_Frequency,
                    v_Comment = x.v_Comment,
                    i_TypeHabitsId = x.i_TypeHabitsId,
                    v_TypeHabitsName = x.v_TypeHabitsName,
                    v_DescriptionQuantity = x.v_DescriptionQuantity,
                    v_DescriptionHabit = x.v_DescriptionHabit,
                    v_FrecuenciaHabito = x.v_FrecuenciaHabito

                }).ToList();


                return query1;

            }
            catch (Exception ex)
            {
                return null;
            }
        }
     
    }
}
