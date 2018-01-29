using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Dynamic;
using Sigesoft.Server.WebClientAdmin.BE;
using Sigesoft.Server.WebClientAdmin.DAL;
using Sigesoft.Common;


namespace Sigesoft.Server.WebClientAdmin.BLL
{
   public class ServiceBL
    {

       List<int> ListaDiente = new List<int>();

       public List<ServiceList> GetService(ref OperationResult pobjOperationResult, int? pintPageIndex, int? pintResultsPerPage, string pstrSortExpression, string pstrFilterExpression, DateTime? FechaInico, DateTime? FechaFin)
       {
           try
           {
               SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

               var query = from A in dbContext.protocolsystemuser

                           join D in dbContext.protocol on A.v_ProtocolId equals    D.v_ProtocolId
                           join E in dbContext.organization on D.v_CustomerOrganizationId equals    E.v_OrganizationId
                           join F in dbContext.service on A.v_ProtocolId equals F.v_ProtocolId

                           join H in dbContext.person on F.v_PersonId equals H.v_PersonId

                           join G in dbContext.calendar on F.v_ServiceId equals     G.v_ServiceId
                           join J4 in dbContext.systemparameter on new { ItemId = F.i_AptitudeStatusId.Value, groupId = 124 }
                                     equals new { ItemId = J4.i_ParameterId, groupId = J4.i_GroupId } into J4_join
                           from J4 in J4_join.DefaultIfEmpty()

                           where F.i_IsDeleted == 0 && F.i_ServiceStatusId == (int)Common.ServiceStatus.Culminado
                          
                           select new ServiceList
                           {
                               v_ServiceId = F.v_ServiceId,
                               v_IdTrabajador = H.v_PersonId,
                               v_Trabajador = H.v_FirstLastName + " " + H.v_SecondLastName + " " + H.v_FirstName,
                               d_ServiceDate = F.d_ServiceDate.Value,
                               i_AptitudeId = F.i_AptitudeStatusId.Value,
                               i_TypeEsoId = F.i_MasterServiceId.Value,
                               v_ProtocolId = F.v_ProtocolId,
                               v_HCL = F.v_ServiceId,
                               v_AptitudeStatusName = J4.v_Value1,
                               v_ProtocolName = D.v_Name,
                               EmpresaCliente = E.v_Name,
                               v_CustomerOrganizationId = E.v_OrganizationId,
                               Dni = H.v_DocNumber
                              
                           };

             

                if (!string.IsNullOrEmpty(pstrFilterExpression))
                {
                    query = query.Where(pstrFilterExpression);
                }
                if (FechaInico.HasValue && FechaFin.HasValue)
                {
                    query = query.Where("d_ServiceDate >= @0 && d_ServiceDate <= @1", FechaInico.Value, FechaFin.Value);
                }
                if (!string.IsNullOrEmpty(pstrSortExpression))
                {
                    query = query.OrderBy(pstrSortExpression);
                }

                var query1 = (from A in query.ToList()
                             //let x = GetRestricctionByServiceId(A.v_ServiceId)
                             select new ServiceList
                             {
                                 v_ServiceId = A.v_ServiceId,
                                 v_IdTrabajador = A.v_IdTrabajador,
                                 v_Trabajador = A.v_Trabajador,
                                 d_ServiceDate = A.d_ServiceDate,
                                 i_AptitudeId = A.i_AptitudeId,
                                 i_TypeEsoId =A.i_TypeEsoId,
                                 v_ProtocolId = A.v_ProtocolId,
                                 v_HCL =A.v_HCL,
                                 v_AptitudeStatusName = A.v_AptitudeStatusName,
                                 v_ProtocolName = A.v_ProtocolName,
                                 EmpresaCliente = A.EmpresaCliente,
                                 v_CustomerOrganizationId = A.v_CustomerOrganizationId,
                                 //v_Restricction = x,
                                 Dni = A.Dni
                             }).ToList();



                var objData = query1.AsEnumerable()
                         .GroupBy(x => x.v_ServiceId)
                         .Select(group => group.First());

                List<ServiceList> objData1 = objData.ToList();
                pobjOperationResult.Success = 1; 
                return objData1;
           }
           catch (Exception ex)
           {
               pobjOperationResult.Success = 0;
               pobjOperationResult.ExceptionMessage = ex.Message;
               return null;
           }
       }

       public List<CalendarList> GetCalendarsPagedAndFiltered(ref OperationResult pobjOperationResult, int? pintPageIndex, int? pintResultsPerPage, string pstrSortExpression, string pstrFilterExpression, DateTime? FechaInico, DateTime? FechaFin)
       {          
           try
           {
               SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
               var query = from A in dbContext.calendar
                           join B in dbContext.person on A.v_PersonId equals B.v_PersonId
                           join D in dbContext.service on A.v_ServiceId equals D.v_ServiceId

                           join J in dbContext.protocol on new { a = D.v_ProtocolId }
                                        equals new { a = J.v_ProtocolId } into J_join
                           from J in J_join.DefaultIfEmpty()

                           join K in dbContext.systemparameter on new { a = J.i_EsoTypeId.Value, b = 118 }
                                        equals new { a = K.i_ParameterId, b = K.i_GroupId } into K_join
                           from K in K_join.DefaultIfEmpty()

                           // Empresa / Sede Cliente **************
                           join oc in dbContext.organization on new { a = J.v_CustomerOrganizationId }
                                   equals new { a = oc.v_OrganizationId } into oc_join
                           from oc in oc_join.DefaultIfEmpty()

                           join lc in dbContext.location on new { a = J.v_CustomerOrganizationId, b = J.v_CustomerLocationId }
                                 equals new { a = lc.v_OrganizationId, b = lc.v_LocationId } into lc_join
                           from lc in lc_join.DefaultIfEmpty()

                           join J4 in dbContext.systemparameter on new { ItemId = D.i_MasterServiceId.Value, groupId = 124 }
                                   equals new { ItemId = J4.i_ParameterId, groupId = J4.i_GroupId } into J4_join
                           from J4 in J4_join.DefaultIfEmpty()


                           where A.i_IsDeleted == 0
                           select new CalendarList
                           {
                               v_ServiceId = A.v_ServiceId,
                               v_IdTrabajador = B.v_PersonId,
                               v_Trabajador = B.v_FirstName + " " + B.v_FirstLastName + " " + B.v_SecondLastName,
                               d_ServiceDate = D.d_ServiceDate.Value,
                               i_AptitudeId = D.i_AptitudeStatusId.Value,
                               i_TypeEsoId = D.i_MasterServiceId.Value,
                               v_ProtocolId = D.v_ProtocolId,
                               v_HCL = D.v_ServiceId,
                               v_AptitudeStatusName = J4.v_Value1,
                               v_ProtocolName = J.v_Name,
                               EmpresaCliente = oc.v_Name,
                               v_CalendarId = A.v_CalendarId,
                               d_DateTimeCalendar = A.d_DateTimeCalendar.Value,
                               v_Pacient = B.v_FirstLastName + " " + B.v_SecondLastName + " " + B.v_FirstName,
                               v_NumberDocument = B.v_DocNumber,
                               v_PersonId = A.v_PersonId,
                               i_ServiceStatusId = D.i_ServiceStatusId.Value,

                               i_ServiceId = A.i_ServiceId.Value,
                               i_ServiceTypeId = A.i_ServiceTypeId.Value,
                               i_CalendarStatusId = A.i_CalendarStatusId.Value,
                               i_MasterServiceId = A.i_ServiceId.Value,

                               i_NewContinuationId = A.i_NewContinuationId.Value,
                               i_LineStatusId = A.i_LineStatusId.Value,
                               i_IsVipId = A.i_IsVipId.Value,

                               i_EsoTypeId = J.i_EsoTypeId.Value,
                               v_EsoTypeName = K.v_Value1,

                               v_CustomerOrganizationId = oc.v_OrganizationId,
                               v_CustomerLocationId = lc.v_LocationId,

                               v_DocNumber = B.v_DocNumber,
                               i_DocTypeId = B.i_DocTypeId.Value,
                               d_EntryTimeCM = A.d_EntryTimeCM.Value,

                           };

               if (!string.IsNullOrEmpty(pstrFilterExpression))
               {
                   query = query.Where(pstrFilterExpression);
               }
               if (FechaInico.HasValue && FechaFin.HasValue)
               {
                   query = query.Where("d_ServiceDate >= @0 && d_ServiceDate <= @1", FechaInico.Value, FechaFin.Value);
               }
             
               List<CalendarList> objData = query.ToList();
               pobjOperationResult.Success = 1;
               return objData;

           }
           catch (Exception ex)
           {
               pobjOperationResult.Success = 0;
               pobjOperationResult.ExceptionMessage = Common.Utils.ExceptionFormatter(ex);
               return null;
           }
       }
       
       public List<DiagnosticRepositoryList> GetAptitudeCertificate(ref OperationResult pobjOperationResult, string pstrServiceId)
       {
           //mon.IsActive = true;
           var isDeleted = 0;
           try
           {
               SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

               var query = (from sss in dbContext.service
                            join ccc in dbContext.diagnosticrepository on sss.v_ServiceId equals ccc.v_ServiceId into ccc_join
                            from ccc in ccc_join.DefaultIfEmpty()  // ESO

                            join ddd in dbContext.diseases on ccc.v_DiseasesId equals ddd.v_DiseasesId into ddd_join
                            from ddd in ddd_join.DefaultIfEmpty()  // Diagnosticos

                            join D in dbContext.person on sss.v_PersonId equals D.v_PersonId

                            join J in dbContext.systemparameter on new { a = D.i_SexTypeId.Value, b = 100 }
                                               equals new { a = J.i_ParameterId, b = J.i_GroupId }  // GENERO

                            join E in dbContext.protocol on sss.v_ProtocolId equals E.v_ProtocolId

                            join F in dbContext.groupoccupation on E.v_GroupOccupationId equals F.v_GroupOccupationId

                            join ooo in dbContext.organization on E.v_CustomerOrganizationId equals ooo.v_OrganizationId

                            join lll in dbContext.location on E.v_EmployerLocationId equals lll.v_LocationId

                            join H in dbContext.systemparameter on new { a = E.i_EsoTypeId.Value, b = 118 }
                                                equals new { a = H.i_ParameterId, b = H.i_GroupId }  // TIPO ESO [ESOA,ESOR,ETC]

                            join G in dbContext.systemparameter on new { a = sss.i_AptitudeStatusId.Value, b = 124 }
                                     equals new { a = G.i_ParameterId, b = G.i_GroupId }  // ESTADO APTITUD ESO                    

                            join J3 in dbContext.systemparameter on new { a = 119, b = sss.i_MasterServiceId.Value }  // DESCRIPCION DEL SERVICIO
                                                       equals new { a = J3.i_GroupId, b = J3.i_ParameterId } into J3_join
                            from J3 in J3_join.DefaultIfEmpty()

                            join J1 in dbContext.systemuser on new { i_InsertUserId = ccc.i_InsertUserId.Value }
                                            equals new { i_InsertUserId = J1.i_SystemUserId } into J1_join
                            from J1 in J1_join.DefaultIfEmpty()

                            join J2 in dbContext.systemuser on new { i_UpdateUserId = ccc.i_UpdateUserId.Value }
                                                            equals new { i_UpdateUserId = J2.i_SystemUserId } into J2_join
                            from J2 in J2_join.DefaultIfEmpty()

                            join su in dbContext.systemuser on sss.i_UpdateUserOccupationalMedicaltId.Value equals su.i_SystemUserId into su_join
                            from su in su_join.DefaultIfEmpty()

                            join pr in dbContext.professional on su.v_PersonId equals pr.v_PersonId into pr_join
                            from pr in pr_join.DefaultIfEmpty()

                            where (ccc.v_ServiceId == pstrServiceId) &&
                                  (ccc.i_IsDeleted == isDeleted) &&
                                  (ccc.i_FinalQualificationId == (int)FinalQualification.Definitivo ||
                                  ccc.i_FinalQualificationId == (int)FinalQualification.Presuntivo)

                            select new DiagnosticRepositoryList
                            {
                                v_DiagnosticRepositoryId = ccc.v_DiagnosticRepositoryId,
                                v_ServiceId = ccc.v_ServiceId,
                                v_DiseasesId = ccc.v_DiseasesId,
                                i_AutoManualId = ccc.i_AutoManualId,
                                i_PreQualificationId = ccc.i_PreQualificationId,
                                i_FinalQualificationId = ccc.i_FinalQualificationId,
                                i_DiagnosticTypeId = ccc.i_DiagnosticTypeId,
                                d_ExpirationDateDiagnostic = ccc.d_ExpirationDateDiagnostic,
                                v_DiseasesName = ddd.v_Name,
                                v_CreationUser = J1.v_UserName,
                                v_UpdateUser = J2.v_UserName,
                                d_CreationDate = J1.d_InsertDate,
                                d_UpdateDate = J2.d_UpdateDate,
                                i_IsDeleted = ccc.i_IsDeleted.Value,
                                v_ProtocolId = E.v_ProtocolId,
                                v_ProtocolName = E.v_Name,
                                v_PersonId = D.v_PersonId,
                                d_BirthDate = D.d_Birthdate,
                                v_EsoTypeName = H.v_Value1,
                                //v_OrganizationPartialName = ooo.v_Name,
                                i_NumberQuotasMen = ooo.i_NumberQuotasMen, 
                                v_LocationName = lll.v_Name,
                                v_FirstName = D.v_FirstName,
                                v_FirstLastName = D.v_FirstLastName,
                                v_SecondLastName = D.v_SecondLastName,
                                v_DocNumber = D.v_DocNumber,
                                v_GenderName = J.v_Value1,
                                v_AptitudeStatusName = G.v_Value1,
                                v_OccupationName = D.v_CurrentOccupation,
                                g_Image = pr.b_SignatureImage,
                                d_ServiceDate = sss.d_ServiceDate,
                                i_AptitudeStatusId = sss.i_AptitudeStatusId,
                                i_EsoTypeId_Old = E.i_EsoTypeId.Value,
                            });

               var MedicalCenter = GetInfoMedicalCenter();

               var q = (from a in query.ToList()
                        select new DiagnosticRepositoryList
                        {
                            v_DiagnosticRepositoryId = a.v_DiagnosticRepositoryId,
                            v_ServiceId = a.v_ServiceId,
                            v_DiseasesId = a.v_DiseasesId,
                            i_DiagnosticTypeId = a.i_DiagnosticTypeId,
                            d_ExpirationDateDiagnostic = a.d_ExpirationDateDiagnostic,
                            v_CreationUser = a.v_CreationUser,
                            v_UpdateUser = a.v_UpdateUser,
                            d_CreationDate = a.d_CreationDate,
                            d_UpdateDate = a.d_UpdateDate,
                            i_IsDeleted = a.i_IsDeleted,
                            i_EsoTypeId = a.i_EsoTypeId_Old.ToString(),
                            v_EsoTypeName = a.v_EsoTypeName,
                            v_OrganizationName = string.Format("{0} / {1}", a.v_OrganizationPartialName, a.v_LocationName),
                            v_PersonName = string.Format("{0} {1}, {2}", a.v_FirstLastName, a.v_SecondLastName, a.v_FirstName),
                            v_DocNumber = a.v_DocNumber,
                            i_Age = a.d_BirthDate == null ? (int?)null : DateTime.Today.AddTicks(-a.d_BirthDate.Value.Ticks).Year - 1,
                            v_GenderName = a.v_GenderName,
                            v_DiseasesName = a.v_DiseasesName,
                            v_RecomendationsName = ConcatenateRecommendation(a.v_DiagnosticRepositoryId),
                            v_RestrictionsName = ConcatenateRestriction(a.v_DiagnosticRepositoryId),
                            v_AptitudeStatusName = a.v_AptitudeStatusName,
                            v_OccupationName = a.v_OccupationName,  // por ahora se muestra el GESO
                            g_Image = a.g_Image,
                            b_Logo = MedicalCenter.b_Image,
                            EmpresaPropietaria = MedicalCenter.v_Name,
                            EmpresaPropietariaDireccion = MedicalCenter.v_Address,
                            EmpresaPropietariaTelefono = MedicalCenter.v_PhoneNumber,
                            EmpresaPropietariaEmail = MedicalCenter.v_Mail,
                            v_ServiceDate = a.d_ServiceDate == null ? string.Empty : a.d_ServiceDate.Value.ToShortDateString(),
                            i_AptitudeStatusId = a.i_AptitudeStatusId
                        }).ToList();

               pobjOperationResult.Success = 1;
               return q;
           }
           catch (Exception ex)
           {
               pobjOperationResult.Success = 0;
               pobjOperationResult.ExceptionMessage = Common.Utils.ExceptionFormatter(ex);
               return null;
           }
       }

       public Sigesoft.Node.WinClient.BE.organizationDto GetInfoMedicalCenter()
       {
           using (SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel())
           {

               var sql = (from o in dbContext.organization
                          where o.v_OrganizationId == Constants.OWNER_ORGNIZATION_ID
                          select new Sigesoft.Node.WinClient.BE.organizationDto
                          {
                              v_Name = o.v_Name,
                              v_Address = o.v_Address,
                              b_Image = o.b_Image,
                              v_PhoneNumber = o.v_PhoneNumber,
                              v_Mail = o.v_Mail,
                              v_IdentificationNumber = o.v_IdentificationNumber

                          }).SingleOrDefault();


               return sql;
           }
       }

       private string ConcatenateRestriction(string pstrDiagnosticRepositoryId)
       {
           SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

           var qry = (from a in dbContext.restriction  // RESTRICCIONES POR Diagnosticos
                      join eee in dbContext.masterrecommendationrestricction on a.v_MasterRestrictionId equals eee.v_MasterRecommendationRestricctionId
                      where a.v_DiagnosticRepositoryId == pstrDiagnosticRepositoryId &&
                      a.i_IsDeleted == 0 && eee.i_TypifyingId == (int)Typifying.Restricciones
                      select new
                      {
                          v_RestrictionsName = eee.v_Name
                      }).ToList();

           return string.Join(", ", qry.Select(p => p.v_RestrictionsName));
       }

       private string ConcatenateRecommendation(string pstrDiagnosticRepositoryId)
       {
           SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

           var qry = (from a in dbContext.recommendation  // RECOMENDACIONES
                      join eee in dbContext.masterrecommendationrestricction on a.v_MasterRecommendationId equals eee.v_MasterRecommendationRestricctionId
                      where a.v_DiagnosticRepositoryId == pstrDiagnosticRepositoryId &&
                      a.i_IsDeleted == 0 && eee.i_TypifyingId == (int)Typifying.Recomendaciones
                      select new
                      {
                          v_RecommendationName = eee.v_Name
                      }).ToList();

           return string.Join(", ", qry.Select(p => p.v_RecommendationName));
       }

       public List<ServiceComponentList> GetServiceComponents(ref OperationResult pobjOperationResult, string pstrServiceId)
       {
          
           int isDeleted = (int)SiNo.NO;
           int isRequired = (int)SiNo.SI;

           try
           {
               SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

               var query = (from A in dbContext.servicecomponent
                            join B in dbContext.systemparameter on new { a = A.i_ServiceComponentStatusId.Value, b = 127 }
                                     equals new { a = B.i_ParameterId, b = B.i_GroupId }
                            join C in dbContext.component on A.v_ComponentId equals C.v_ComponentId
                            join D in dbContext.systemparameter on new { a = A.i_QueueStatusId.Value, b = 128 }
                                     equals new { a = D.i_ParameterId, b = D.i_GroupId }
                            join E in dbContext.service on A.v_ServiceId equals E.v_ServiceId
                            join F in dbContext.systemparameter on new { a = C.i_CategoryId.Value, b = 116 }
                                     equals new { a = F.i_ParameterId, b = F.i_GroupId } into F_join
                            from F in F_join.DefaultIfEmpty()

                            where A.v_ServiceId == pstrServiceId &&
                                  A.i_IsDeleted == isDeleted &&
                                  A.i_IsRequiredId == isRequired

                            select new ServiceComponentList
                            {
                                v_ComponentId = A.v_ComponentId,
                                v_ComponentName = C.v_Name,
                                i_ServiceComponentStatusId = A.i_ServiceComponentStatusId.Value,
                                v_ServiceComponentStatusName = B.v_Value1,
                                d_StartDate = A.d_StartDate.Value,
                                d_EndDate = A.d_EndDate.Value,
                                i_QueueStatusId = A.i_QueueStatusId.Value,
                                v_QueueStatusName = D.v_Value1,
                                ServiceStatusId = E.i_ServiceStatusId.Value,
                                v_Motive = E.v_Motive,
                                i_CategoryId = C.i_CategoryId.Value,
                                v_CategoryName = C.i_CategoryId.Value == -1 ? C.v_Name : F.v_Value1,
                                v_ServiceId = E.v_ServiceId
                            });

               var objData = query.AsEnumerable()
                            .Where(s => s.i_CategoryId != -1)
                            .GroupBy(x => x.i_CategoryId)
                            .Select(group => group.First());

               List<ServiceComponentList> obj = objData.ToList();

               obj.AddRange(query.Where(p => p.i_CategoryId == -1));

               pobjOperationResult.Success = 1;
               return obj;
           }
           catch (Exception ex)
           {
               pobjOperationResult.Success = 0;
               pobjOperationResult.ExceptionMessage = Common.Utils.ExceptionFormatter(ex);
               return null;
           }
       }

       public string GetYearsAndMonth(DateTime? EndDate, DateTime? StartDate)
       {
           if (EndDate == null || StartDate == null)
           {
               return "0 años y 0 meses";
           }
           DateTime startDate = StartDate.Value;
           DateTime endDate = EndDate.Value;
           var totalDays = (endDate - startDate).TotalDays;
           var totalYears = Math.Truncate(totalDays / 365);
           var totalMonths = Math.Truncate((totalDays % 365) / 30);
           var remainingDays = Math.Truncate((totalDays % 365) % 30);

           if (totalYears == 0)
           {
               return string.Format("{1} mes(es)", totalMonths, remainingDays);
           }
           else if (totalMonths == 0)
           {
               return string.Format("{0} año(s)", totalYears, remainingDays);
           }
           else
           {
               return string.Format("{0} año(s), {1} mes(es)", totalYears, totalMonths, remainingDays);
           }
       }

       public Sigesoft.Node.WinClient.BE.ServiceList GetAnamnesisReport(string pstrServiceId)
       {
           //mon.IsActive = true;

           var isDeleted = 0;

           try
           {
               SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

               var query = from A in dbContext.service

                           join K in dbContext.systemparameter on new { a = 135, b = A.i_HasSymptomId.Value }
                                                   equals new { a = K.i_GroupId, b = K.i_ParameterId } into K_join
                           from K in K_join.DefaultIfEmpty()

                           join L in dbContext.systemparameter on new { a = 135, b = A.i_UrineId.Value }
                                                   equals new { a = L.i_GroupId, b = L.i_ParameterId } into L_join
                           from L in L_join.DefaultIfEmpty()

                           join M in dbContext.systemparameter on new { a = 135, b = A.i_DepositionId.Value }
                                                 equals new { a = M.i_GroupId, b = M.i_ParameterId } into M_join
                           from M in M_join.DefaultIfEmpty()

                           join N in dbContext.systemparameter on new { a = 135, b = A.i_AppetiteId.Value }
                                       equals new { a = N.i_GroupId, b = N.i_ParameterId } into N_join
                           from N in N_join.DefaultIfEmpty()

                           join O in dbContext.systemparameter on new { a = 134, b = A.i_MacId.Value }
                                 equals new { a = O.i_GroupId, b = O.i_ParameterId } into O_join
                           from O in O_join.DefaultIfEmpty()

                           join P in dbContext.systemparameter on new { a = 135, b = A.i_AppetiteId.Value }
                                     equals new { a = P.i_GroupId, b = P.i_ParameterId } into P_join
                           from P in P_join.DefaultIfEmpty()

                           join su in dbContext.systemuser on A.i_UpdateUserMedicalAnalystId.Value equals su.i_SystemUserId into su_join
                           from su in su_join.DefaultIfEmpty()

                           join pr in dbContext.professional on su.v_PersonId equals pr.v_PersonId into pr_join
                           from pr in pr_join.DefaultIfEmpty()

                           where A.v_ServiceId == pstrServiceId && A.i_IsDeleted == isDeleted

                           select new Sigesoft.Node.WinClient.BE.ServiceList
                           {
                               v_ServiceId = A.v_ServiceId,
                               i_HasSymptomId = A.i_HasSymptomId,
                               v_MainSymptom = A.v_MainSymptom,
                               i_TimeOfDisease = A.i_TimeOfDisease,
                               i_TimeOfDiseaseTypeId = A.i_TimeOfDiseaseTypeId,
                               v_Story = A.v_Story,
                               v_PersonId = pr.v_PersonId,

                               i_DreamId = A.i_DreamId,
                               v_Dream = K.v_Value1,
                               i_UrineId = A.i_UrineId,
                               v_Urine = L.v_Value1,
                               i_DepositionId = A.i_DepositionId,
                               v_Deposition = M.v_Value1,
                               i_AppetiteId = A.i_AppetiteId,
                               v_Appetite = N.v_Value1,
                               i_ThirstId = A.i_ThirstId,
                               v_Thirst = P.v_Value1,
                               d_Fur = A.d_Fur.Value,
                               v_CatemenialRegime = A.v_CatemenialRegime,
                               i_MacId = A.i_MacId,
                               v_Mac = O.v_Value1,

                               // Antecedentes ginecologicos
                               d_PAP = A.d_PAP.Value,
                               d_Mamografia = A.d_Mamografia.Value,
                               v_CiruGine = A.v_CiruGine,
                               v_Gestapara = A.v_Gestapara,
                               v_Menarquia = A.v_Menarquia,
                               v_Findings = A.v_Findings,

                               // firma y sello del medico que analisa y califica los diagnosticos
                               FirmaDoctor = pr.b_SignatureImage

                           };

               Sigesoft.Node.WinClient.BE.ServiceList objData = query.FirstOrDefault();
               return objData;
           }
           catch (Exception ex)
           {
               return null;
           }
       }
       //public List<Sigesoft.Node.WinClient.BE.DiagnosticRepositoryList> GetDiagnosticByServiceIdAndCategoryId(string pstrServiceId, int pintCategoryId)
       //{
       //    SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
       //    var query = (from ccc in dbContext.diagnosticrepository
       //                 join ddd in dbContext.component on ccc.v_ComponentId equals ddd.v_ComponentId

       //                 join B in dbContext.servicecomponent on ccc.v_ServiceId equals B.v_ServiceId
       //                 join C in dbContext.servicecomponentfields on B.v_ServiceComponentId equals C.v_ServiceComponentId
       //                 join D in dbContext.servicecomponentfieldvalues on C.v_ServiceComponentFieldsId equals D.v_ServiceComponentFieldsId
       //                 join E in dbContext.component on B.v_ComponentId equals E.v_ComponentId
       //                 join F in dbContext.componentfields on C.v_ComponentFieldId equals F.v_ComponentFieldId
       //                 join G in dbContext.componentfield on C.v_ComponentFieldId equals G.v_ComponentFieldId
       //                 join H in dbContext.component on F.v_ComponentId equals H.v_ComponentId


       //                 where ccc.v_ServiceId == pstrServiceId && ddd.i_CategoryId == pintCategoryId &&
       //                       ccc.i_IsDeleted == 0
       //                 select new Sigesoft.Node.WinClient.BE.DiagnosticRepositoryList
       //                 {
       //                     v_DiseasesName = ddd.v_Name,
       //                     v_ComponentFielName = G.v_TextLabel,
       //                     v_Value1 = D.v_Value1

       //                 }).ToList();


       //    return query;
       //}
       public List<Sigesoft.Node.WinClient.BE.ServiceComponentFieldValuesList> ValoresComponenteAMC(string pstrServiceId, int pintCategory)
       {
           SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
           int rpta = 0;

           try
           {
               var serviceComponentFieldValues = (from A in dbContext.service
                                                  join B in dbContext.servicecomponent on A.v_ServiceId equals B.v_ServiceId
                                                  join C in dbContext.servicecomponentfields on B.v_ServiceComponentId equals C.v_ServiceComponentId
                                                  join D in dbContext.servicecomponentfieldvalues on C.v_ServiceComponentFieldsId equals D.v_ServiceComponentFieldsId
                                                  join E in dbContext.component on B.v_ComponentId equals E.v_ComponentId
                                                  join F in dbContext.componentfields on C.v_ComponentFieldId equals F.v_ComponentFieldId
                                                  join G in dbContext.componentfield on C.v_ComponentFieldId equals G.v_ComponentFieldId
                                                  join H in dbContext.component on F.v_ComponentId equals H.v_ComponentId
                                                  join I in dbContext.diagnosticrepository on new { a = A.v_ServiceId, b = H.v_ComponentId } equals new { a = I.v_ServiceId, b = I.v_ComponentId }

                                                  join D1 in dbContext.datahierarchy on new { a = G.i_MeasurementUnitId.Value, b = 105 }
                                                      equals new { a = D1.i_ItemId, b = D1.i_GroupId } into D1_join
                                                  from D1 in D1_join.DefaultIfEmpty()

                                                  where A.v_ServiceId == pstrServiceId
                                                          && H.i_CategoryId == pintCategory
                                                          && B.i_IsDeleted == 0
                                                          && C.i_IsDeleted == 0

                                                  select new Sigesoft.Node.WinClient.BE.ServiceComponentFieldValuesList
                                                  {
                                                      v_ComponentFieldId = G.v_ComponentFieldId,
                                                      v_ComponentFielName = G.v_TextLabel,
                                                      v_ServiceComponentFieldsId = C.v_ServiceComponentFieldsId,
                                                      v_Value1 = D.v_Value1,
                                                      i_GroupId = G.i_GroupId.Value,
                                                      i_CategoryId = H.i_CategoryId.Value,
                                                      v_UnidadMedida = D1.v_Value1,
                                                      v_ComponentId = H.v_ComponentId
                                                  });

               var finalQuery = (from a in serviceComponentFieldValues.ToList()

                                 let value1 = int.TryParse(a.v_Value1, out rpta)
                                 join sp in dbContext.systemparameter on new { a = a.i_GroupId, b = rpta }
                                                 equals new { a = sp.i_GroupId, b = sp.i_ParameterId } into sp_join
                                 from sp in sp_join.DefaultIfEmpty()

                                 select new Sigesoft.Node.WinClient.BE.ServiceComponentFieldValuesList
                                 {
                                     v_ComponentFieldId = a.v_ComponentFieldId,
                                     v_ComponentFielName = a.v_ComponentFielName,
                                     v_ServiceComponentFieldsId = a.v_ServiceComponentFieldsId,
                                     v_Value1 = a.v_Value1,
                                     v_Value1Name = sp == null ? "" : sp.v_Value1,
                                     i_CategoryId = a.i_CategoryId,
                                     v_UnidadMedida = a.v_UnidadMedida,
                                     v_ComponentId = a.v_ComponentId
                                 }).ToList();


               return finalQuery;
           }
           catch (Exception)
           {

               throw;
           }

       }

       public List<Sigesoft.Node.WinClient.BE.ServiceComponentList> GetServiceComponentsReport(string pstrServiceId)
       {
           //mon.IsActive = true;        
           int isDeleted = 0;

           try
           {
               SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

               #region serviceComponentFields

               var serviceComponentFields = (from A in dbContext.servicecomponent
                                             join B in dbContext.servicecomponentfields on A.v_ServiceComponentId equals B.v_ServiceComponentId
                                             join C in dbContext.servicecomponentfieldvalues on B.v_ServiceComponentFieldsId equals C.v_ServiceComponentFieldsId
                                             join cfs in dbContext.componentfields on B.v_ComponentFieldId equals cfs.v_ComponentFieldId
                                             join D in dbContext.componentfield on B.v_ComponentFieldId equals D.v_ComponentFieldId
                                             join cm in dbContext.component on cfs.v_ComponentId equals cm.v_ComponentId

                                             join dh in dbContext.datahierarchy on new { a = 105, b = D.i_MeasurementUnitId.Value }
                                                                equals new { a = dh.i_GroupId, b = dh.i_ItemId } into dh_join
                                             from dh in dh_join.DefaultIfEmpty()

                                             where (A.v_ServiceId == pstrServiceId) &&
                                                 //(cm.v_ComponentId == pstrComponentId) &&
                                                   (A.i_IsDeleted == isDeleted) &&
                                                   (B.i_IsDeleted == isDeleted) &&
                                                   (C.i_IsDeleted == isDeleted)

                                             select new Sigesoft.Node.WinClient.BE.ServiceComponentFieldsList
                                             {
                                                 v_ServiceComponentFieldsId = B.v_ServiceComponentFieldsId,
                                                 v_ComponentFieldsId = B.v_ComponentFieldId,
                                                 v_ComponentFielName = D.v_TextLabel,
                                                 v_Value1 = C.v_Value1 == "" ? null : C.v_Value1,
                                                 i_GroupId = D.i_GroupId.Value,
                                                 v_MeasurementUnitName = dh.v_Value1,
                                                 v_ComponentId = cm.v_ComponentId,
                                                 v_ServiceComponentId = A.v_ServiceComponentId
                                             }).ToList();

               int rpta = 0;

               var _finalQuery = (from a in serviceComponentFields
                                  let value1 = int.TryParse(a.v_Value1, out rpta)
                                  join sp in dbContext.systemparameter on new { a = a.i_GroupId, b = rpta }
                                                  equals new { a = sp.i_GroupId, b = sp.i_ParameterId } into sp_join
                                  from sp in sp_join.DefaultIfEmpty()

                                  select new Sigesoft.Node.WinClient.BE.ServiceComponentFieldsList
                                  {
                                      v_ServiceComponentFieldsId = a.v_ServiceComponentFieldsId,
                                      v_ComponentFieldsId = a.v_ComponentFieldsId,
                                      v_ComponentFielName = a.v_ComponentFielName,
                                      i_GroupId = a.i_GroupId,
                                      v_Value1 = a.v_Value1,
                                      v_Value1Name = sp == null ? "" : sp.v_Value1,
                                      v_MeasurementUnitName = a.v_MeasurementUnitName,
                                      v_ComponentId = a.v_ComponentId,
                                      v_ConclusionAndDiagnostic = a.v_Value1 + " / " + GetServiceComponentDiagnosticsReport(pstrServiceId, a.v_ComponentId),
                                      v_ServiceComponentId = a.v_ServiceComponentId
                                  }).ToList();


               #endregion

               var components = (from aaa in dbContext.servicecomponent
                                 join bbb in dbContext.component on aaa.v_ComponentId equals bbb.v_ComponentId
                                 join J1 in dbContext.systemuser on new { i_InsertUserId = aaa.i_InsertUserId.Value }
                                                 equals new { i_InsertUserId = J1.i_SystemUserId } into J1_join
                                 from J1 in J1_join.DefaultIfEmpty()

                                 join J2 in dbContext.systemuser on new { i_UpdateUserId = aaa.i_UpdateUserId.Value }
                                                                 equals new { i_UpdateUserId = J2.i_SystemUserId } into J2_join
                                 from J2 in J2_join.DefaultIfEmpty()

                                 join fff in dbContext.systemparameter on new { a = bbb.i_CategoryId.Value, b = 116 } // CATEGORIA DEL EXAMEN
                                                                              equals new { a = fff.i_ParameterId, b = fff.i_GroupId } into J5_join
                                 from fff in J5_join.DefaultIfEmpty()

                                 // Usuario Medico Evaluador / Medico Aprobador ****************************
                                 join me in dbContext.systemuser on aaa.i_ApprovedUpdateUserId equals me.i_SystemUserId into me_join
                                 from me in me_join.DefaultIfEmpty()

                                 join pme in dbContext.professional on me.v_PersonId equals pme.v_PersonId into pme_join
                                 from pme in pme_join.DefaultIfEmpty()

                                 //*********************************************************************

                                 where (aaa.v_ServiceId == pstrServiceId) &&
                                       (bbb.i_ComponentTypeId == (int?)ComponentType.Examen) &&
                                       (aaa.i_IsDeleted == 0) &&
                                       (aaa.i_IsRequiredId == (int?)SiNo.SI)

                                 //orderby bbb.i_CategoryId, bbb.v_Name

                                 select new
                                 {
                                     v_ComponentId = bbb.v_ComponentId,
                                     v_ComponentName = bbb.v_Name,
                                     v_ServiceComponentId = aaa.v_ServiceComponentId,
                                     v_CreationUser = J1.v_UserName,
                                     v_UpdateUser = J2.v_UserName,
                                     d_CreationDate = aaa.d_InsertDate,
                                     d_UpdateDate = aaa.d_UpdateDate,
                                     i_IsDeleted = aaa.i_IsDeleted.Value,
                                     i_CategoryId = bbb.i_CategoryId.Value,
                                     v_CategoryName = fff.v_Value1,
                                     DiagnosticRepository = (from dr in aaa.service.diagnosticrepository
                                                             where (dr.v_ServiceId == pstrServiceId) &&
                                                                   (dr.v_ComponentId == aaa.v_ComponentId)
                                                             select new Sigesoft.Node.WinClient.BE.DiagnosticRepositoryList
                                                             {
                                                                 v_DiseasesId = dr.diseases.v_DiseasesId,
                                                                 v_DiseasesName = dr.diseases.v_Name
                                                             }),
                                     FirmaMedico = pme.b_SignatureImage
                                 }).AsEnumerable().Select(p => new Sigesoft.Node.WinClient.BE.ServiceComponentList
                                 {
                                     v_ComponentId = p.v_ComponentId,
                                     v_ComponentName = p.v_ComponentName,
                                     v_ServiceComponentId = p.v_ServiceComponentId,
                                     v_CreationUser = p.v_CreationUser,
                                     v_UpdateUser = p.v_UpdateUser,
                                     d_CreationDate = p.d_CreationDate,
                                     d_UpdateDate = p.d_UpdateDate,
                                     i_IsDeleted = p.i_IsDeleted,
                                     i_CategoryId = p.i_CategoryId,
                                     v_CategoryName = p.v_CategoryName,
                                     DiagnosticRepository = p.DiagnosticRepository.ToList(),
                                     FirmaMedico = p.FirmaMedico
                                 }).ToList();

               //var ff = _finalQuery.FindAll(p => p.v_ComponentId == Constants.GLUCOSA_ID);
               //var ff_ = _finalQuery.FindAll(p => p.v_ComponentId == Constants.GRUPO_Y_FACTOR_SANGUINEO_ID);
               //var _ff_ = _finalQuery.FindAll(p => p.v_ComponentId == Constants.COLESTEROL_ID);

               components.Sort((x, y) => x.v_ComponentId.CompareTo(y.v_ComponentId));
               components.ForEach(a => a.ServiceComponentFields = _finalQuery.FindAll(p => p.v_ComponentId == a.v_ComponentId));

               return components;
           }
           catch (Exception)
           {
               throw;
           }
       }

       public string GetServiceComponentDiagnosticsReport(string pstrServiceId, string pstrComponentId)
       {
           //mon.IsActive = true;

           try
           {
               SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

               var query = (from ccc in dbContext.diagnosticrepository
                            join bbb in dbContext.component on ccc.v_ComponentId equals bbb.v_ComponentId
                            join ddd in dbContext.diseases on ccc.v_DiseasesId equals ddd.v_DiseasesId  // Diagnosticos 
                            where (ccc.v_ServiceId == pstrServiceId) &&
                                  (ccc.v_ComponentId == pstrComponentId) &&
                                  (ccc.i_IsDeleted == 0)
                            select new Sigesoft.Node.WinClient.BE.DiagnosticRepositoryList
                            {
                                v_DiagnosticRepositoryId = ccc.v_DiagnosticRepositoryId,
                                v_ServiceId = ccc.v_ServiceId,
                                v_ComponentId = ccc.v_ComponentId,
                                v_DiseasesId = ccc.v_DiseasesId,
                                v_DiseasesName = ddd.v_Name,

                            }).ToList();

               var concat = string.Join(", ", query.Select(p => p.v_DiseasesName));

               return concat;
           }
           catch (Exception ex)
           {
               return null;
           }
       }

       public List<Sigesoft.Node.WinClient.BE.DiagnosticRepositoryList> GetServiceComponentConclusionesDxServiceIdReport(string pstrServiceId)
       {
           //mon.IsActive = true;

           try
           {
               SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
               var query = (from ccc in dbContext.diagnosticrepository
                            join bbb in dbContext.component on ccc.v_ComponentId equals bbb.v_ComponentId into J7_join
                            from bbb in J7_join.DefaultIfEmpty()

                            join ddd in dbContext.diseases on ccc.v_DiseasesId equals ddd.v_DiseasesId  // Diagnosticos                       

                            join fff in dbContext.systemparameter on new { a = ccc.i_PreQualificationId.Value, b = 137 } // PRE-CALIFICACION
                                                                equals new { a = fff.i_ParameterId, b = fff.i_GroupId } into J5_join
                            from fff in J5_join.DefaultIfEmpty()

                            join ggg in dbContext.systemparameter on new { a = ccc.i_FinalQualificationId.Value, b = 138 } //CALIFICACION FINAL
                                                                equals new { a = ggg.i_ParameterId, b = ggg.i_GroupId } into J4_join
                            from ggg in J4_join.DefaultIfEmpty()

                            join hhh in dbContext.systemparameter on new { a = ccc.i_DiagnosticTypeId.Value, b = 139 } // TIPO DE DX [Enfermedad comun, etc]
                                                                    equals new { a = hhh.i_ParameterId, b = hhh.i_GroupId } into J3_join
                            from hhh in J3_join.DefaultIfEmpty()

                            where (ccc.v_ServiceId == pstrServiceId) &&
                            (ccc.i_IsDeleted == 0) &&
                            (ccc.i_FinalQualificationId == (int)FinalQualification.Definitivo ||
                            ccc.i_FinalQualificationId == (int)FinalQualification.Presuntivo)
                            orderby bbb.v_Name

                            select new Sigesoft.Node.WinClient.BE.DiagnosticRepositoryList
                            {
                                v_DiagnosticRepositoryId = ccc.v_DiagnosticRepositoryId,
                                v_ServiceId = ccc.v_ServiceId,
                                v_ComponentId = ccc.v_ComponentId,
                                v_DiseasesId = ccc.v_DiseasesId,
                                v_DiseasesName = ddd.v_Name,
                                v_ComponentName = bbb.v_Name,
                                v_PreQualificationName = fff.v_Value1,
                                v_FinalQualificationName = ggg.v_Value1,
                                v_DiagnosticTypeName = hhh.v_Value1,
                                v_ComponentFieldsId = ccc.v_ComponentFieldId
                            }).ToList();

               // add the sequence number on the fly
               var finalQuery = query.Select((a, index) => new Sigesoft.Node.WinClient.BE.DiagnosticRepositoryList
               {
                   i_Item = index + 1,
                   v_DiagnosticRepositoryId = a.v_DiagnosticRepositoryId,
                   v_ServiceId = a.v_ServiceId,
                   v_ComponentId = a.v_ComponentId,
                   v_DiseasesId = a.v_DiseasesId,
                   v_DiseasesName = a.v_DiseasesName,
                   v_ComponentName = a.v_ComponentName,
                   v_DiagnosticTypeName = a.v_DiagnosticTypeName,
                   Recomendations = GetServiceRecommendationByDiagnosticRepositoryIdReport(a.v_DiagnosticRepositoryId),
                   Restrictions = GetServiceRestrictionByDiagnosticRepositoryIdReport(a.v_DiagnosticRepositoryId),
                   v_ComponentFieldsId = a.v_ComponentFieldsId
               }).ToList();

               return finalQuery;
           }
           catch (Exception ex)
           {
               return null;
           }
       }

       public List<Sigesoft.Node.WinClient.BE.RecomendationList> GetServiceRecommendationByDiagnosticRepositoryIdReport(string pstrDiagnosticRepositoryId)
       {
           //mon.IsActive = true;
           try
           {
               SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

               List<Sigesoft.Node.WinClient.BE.RecomendationList> query = (from ddd in dbContext.recommendation
                                                join eee in dbContext.masterrecommendationrestricction on ddd.v_MasterRecommendationId
                                                                        equals eee.v_MasterRecommendationRestricctionId //                                                                                                                                                                              
                                                where (ddd.v_DiagnosticRepositoryId == pstrDiagnosticRepositoryId) &&
                                                      (ddd.i_IsDeleted == 0)
                                                                           select new Sigesoft.Node.WinClient.BE.RecomendationList
                                                {
                                                    v_RecommendationId = ddd.v_RecommendationId,
                                                    v_DiagnosticRepositoryId = ddd.v_DiagnosticRepositoryId,
                                                    v_ServiceId = ddd.v_ServiceId,
                                                    v_ComponentId = ddd.v_ComponentId,
                                                    v_MasterRecommendationId = ddd.v_MasterRecommendationId,
                                                    v_RecommendationName = eee.v_Name,

                                                }).ToList();

               // add the sequence number on the fly
               var finalQuery = query.Select((a, index) => new Sigesoft.Node.WinClient.BE.RecomendationList
               {
                   i_Item = index + 1,
                   v_RecommendationId = a.v_RecommendationId,
                   v_DiagnosticRepositoryId = a.v_DiagnosticRepositoryId,
                   v_ServiceId = a.v_ServiceId,
                   v_ComponentId = a.v_ComponentId,
                   v_MasterRecommendationId = a.v_MasterRecommendationId,
                   v_RecommendationName = a.v_RecommendationName,
               }).ToList();

               return finalQuery;
           }
           catch (Exception ex)
           {

               return null;
           }
       }

       public Sigesoft.Node.WinClient.BE.ServiceList GetServiceReport(string pstrServiceId)
       {
           //mon.IsActive = true;

           try
           {
               SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

               var objEntity = (from A in dbContext.service
                                join B in dbContext.protocol on A.v_ProtocolId equals B.v_ProtocolId into B_join
                                from B in B_join.DefaultIfEmpty()

                                join C in dbContext.organization on B.v_WorkingOrganizationId equals C.v_OrganizationId into C_join
                                from C in C_join.DefaultIfEmpty()

                                join D in dbContext.datahierarchy on new { a = C.i_SectorTypeId.Value, b = 104 }
                                                       equals new { a = D.i_ItemId, b = D.i_GroupId } into D_join
                                from D in D_join.DefaultIfEmpty()

                                join E in dbContext.datahierarchy on new { a = C.i_DepartmentId.Value, b = 113 }
                                                      equals new { a = E.i_ItemId, b = E.i_GroupId } into E_join
                                from E in E_join.DefaultIfEmpty()

                                join F in dbContext.datahierarchy on new { a = C.i_ProvinceId.Value, b = 113 }
                                                      equals new { a = F.i_ItemId, b = F.i_GroupId } into F_join
                                from F in F_join.DefaultIfEmpty()

                                join G in dbContext.datahierarchy on new { a = C.i_DistrictId.Value, b = 113 }
                                                      equals new { a = G.i_ItemId, b = G.i_GroupId } into G_join
                                from G in G_join.DefaultIfEmpty()

                                join H in dbContext.person on A.v_PersonId equals H.v_PersonId into H_join
                                from H in H_join.DefaultIfEmpty()

                                join I in dbContext.datahierarchy on new { a = H.i_DepartmentId.Value, b = 113 }
                                                      equals new { a = I.i_ItemId, b = I.i_GroupId } into I_join
                                from I in I_join.DefaultIfEmpty()

                                join J in dbContext.datahierarchy on new { a = H.i_ProvinceId.Value, b = 113 }
                                                      equals new { a = J.i_ItemId, b = J.i_GroupId } into J_join
                                from J in J_join.DefaultIfEmpty()

                                join K in dbContext.datahierarchy on new { a = H.i_DistrictId.Value, b = 113 }
                                                      equals new { a = K.i_ItemId, b = K.i_GroupId } into K_join
                                from K in K_join.DefaultIfEmpty()

                                join L in dbContext.systemparameter on new { a = H.i_TypeOfInsuranceId.Value, b = 188 }
                                                     equals new { a = L.i_ParameterId, b = L.i_GroupId } into L_join
                                from L in L_join.DefaultIfEmpty()

                                join M in dbContext.systemparameter on new { a = H.i_MaritalStatusId.Value, b = 101 }
                                             equals new { a = M.i_ParameterId, b = M.i_GroupId } into M_join
                                from M in M_join.DefaultIfEmpty()

                                join N in dbContext.datahierarchy on new { a = H.i_LevelOfId.Value, b = 108 }
                                                equals new { a = N.i_ItemId, b = N.i_GroupId } into N_join
                                from N in N_join.DefaultIfEmpty()


                                join C1 in dbContext.organization on B.v_EmployerOrganizationId equals C1.v_OrganizationId into C1_join
                                from C1 in C1_join.DefaultIfEmpty()


                                join su in dbContext.systemuser on A.i_UpdateUserMedicalAnalystId.Value equals su.i_SystemUserId into su_join
                                from su in su_join.DefaultIfEmpty()

                                join pr in dbContext.professional on su.v_PersonId equals pr.v_PersonId into pr_join
                                from pr in pr_join.DefaultIfEmpty()


                                join P1 in dbContext.person on new { a = pr.v_PersonId }
                                        equals new { a = P1.v_PersonId } into P1_join
                                from P1 in P1_join.DefaultIfEmpty()

                                join O in dbContext.systemparameter on new { a = 134, b = A.i_MacId.Value }
                                                       equals new { a = O.i_GroupId, b = O.i_ParameterId } into O_join
                                from O in O_join.DefaultIfEmpty()

                                where A.v_ServiceId == pstrServiceId

                                select new Sigesoft.Node.WinClient.BE.ServiceList
                                {
                                    //-----------------CABECERA---------------------------------
                                    v_PersonId = H.v_PersonId,
                                    v_ServiceId = A.v_ServiceId,
                                    d_ServiceDate = A.d_ServiceDate,
                                    i_DiaV = A.d_ServiceDate.Value.Day,
                                    i_MesV = A.d_ServiceDate.Value.Month,
                                    i_AnioV = A.d_ServiceDate.Value.Year,
                                    i_EsoTypeId = B.i_EsoTypeId.Value, // tipo de ESO : Pre-Ocupacional ,  Periodico, etc 
                                    //---------------DATOS DE LA EMPRESA--------------------------------
                                    EmpresaTrabajo = C.v_Name,
                                    EmpresaEmpleadora = C1.v_Name,
                                    RubroEmpresaTrabajo = D.v_Value1,
                                    DireccionEmpresaTrabajo = C.v_Address,
                                    DepartamentoEmpresaTrabajo = E.v_Value1,
                                    ProvinciaEmpresaTrabajo = F.v_Value1,
                                    DistritoEmpresaTrabajo = G.v_Value1,
                                    v_CurrentOccupation = H.v_CurrentOccupation,
                                    //---------------DATOS DE FILIACIÓN TRABAJADOR--------------------------------
                                    i_DocTypeId = H.i_DocTypeId.Value,
                                    v_Pacient = H.v_FirstName + " " + H.v_FirstLastName + " " + H.v_SecondLastName,
                                    d_BirthDate = H.d_Birthdate,
                                    i_DiaN = H.d_Birthdate.Value.Day,
                                    i_MesN = H.d_Birthdate.Value.Month,
                                    i_AnioN = H.d_Birthdate.Value.Year,
                                    v_DocNumber = H.v_DocNumber,
                                    v_AdressLocation = H.v_AdressLocation,

                                    DepartamentoPaciente = I.v_Value1,
                                    ProvinciaPaciente = J.v_Value1,
                                    DistritoPaciente = K.v_Value1,
                                    i_ResidenceInWorkplaceId = H.i_ResidenceInWorkplaceId.Value,
                                    v_ResidenceTimeInWorkplace = H.v_ResidenceTimeInWorkplace,
                                    i_TypeOfInsuranceId = H.i_TypeOfInsuranceId.Value,
                                    Email = H.v_Mail,
                                    Telefono = H.v_TelephoneNumber,
                                    EstadoCivil = M.v_Value1,
                                    GradoInstruccion = N.v_Value1,
                                    v_Story = A.v_Story,
                                    i_AptitudeStatusId = A.i_AptitudeStatusId,

                                    HijosVivos = H.i_NumberLivingChildren,
                                    HijosMuertos = H.i_NumberDeadChildren,
                                    HijosDependientes = H.i_NumberDependentChildren,

                                    v_BirthPlace = H.v_BirthPlace,
                                    i_PlaceWorkId = H.i_PlaceWorkId.Value,
                                    v_ExploitedMineral = H.v_ExploitedMineral,
                                    i_AltitudeWorkId = H.i_AltitudeWorkId.Value,
                                    v_EmergencyPhone = H.v_EmergencyPhone,
                                    i_SexTypeId = H.i_SexTypeId,
                                    i_MaritalStatusId = H.i_MaritalStatusId.Value,
                                    i_LevelOfId = H.i_LevelOfId.Value,
                                    FirmaTrabajador = H.b_RubricImage,
                                    HuellaTrabajador = H.b_FingerPrintImage,

                                    //Datos del Doctor
                                    FirmaDoctor = pr.b_SignatureImage,
                                    NombreDoctor = P1.v_FirstName + " " + P1.v_FirstLastName + " " + P1.v_SecondLastName,
                                    CMP = pr.v_ProfessionalCode,

                                    d_Fur = A.d_Fur,
                                    v_CatemenialRegime = A.v_CatemenialRegime,
                                    i_MacId = A.i_MacId,
                                    v_Mac = O.v_Value1,

                                    // Antecedentes ginecologicos
                                    d_PAP = A.d_PAP.Value,
                                    d_Mamografia = A.d_Mamografia.Value,
                                    v_CiruGine = A.v_CiruGine,
                                    v_Gestapara = A.v_Gestapara,
                                    v_Menarquia = A.v_Menarquia,
                                    v_Findings = A.v_Findings,


                                });


               var sql = (from a in objEntity.ToList()
                          select new Sigesoft.Node.WinClient.BE.ServiceList
                          {
                              //-----------------CABECERA---------------------------------
                              v_ServiceId = a.v_ServiceId,
                              d_ServiceDate = a.d_ServiceDate,
                              i_DiaV = a.d_ServiceDate.Value.Day,
                              i_MesV = a.d_ServiceDate.Value.Month,
                              i_AnioV = a.d_ServiceDate.Value.Year,
                              i_EsoTypeId = a.i_EsoTypeId, // tipo de ESO : Pre-Ocupacional ,  Periodico, etc 
                              //---------------DATOS DE LA EMPRESA--------------------------------
                              EmpresaTrabajo = a.EmpresaTrabajo,
                              EmpresaEmpleadora = a.EmpresaEmpleadora,
                              RubroEmpresaTrabajo = a.RubroEmpresaTrabajo,
                              DireccionEmpresaTrabajo = a.DireccionEmpresaTrabajo,
                              DepartamentoEmpresaTrabajo = a.DepartamentoEmpresaTrabajo,
                              ProvinciaEmpresaTrabajo = a.ProvinciaEmpresaTrabajo,
                              DistritoEmpresaTrabajo = a.DistritoEmpresaTrabajo,
                              v_CurrentOccupation = a.v_CurrentOccupation,
                              //---------------DATOS DE FILIACIÓN TRABAJADOR--------------------------------
                              i_DocTypeId = a.i_DocTypeId,
                              v_Pacient = a.v_Pacient,
                              d_BirthDate = a.d_BirthDate,
                              i_DiaN = a.i_DiaN,
                              i_MesN = a.i_MesN,
                              i_AnioN = a.i_AnioN,
                              i_Edad = GetAge(a.d_BirthDate.Value),
                              //i_Edad =30,
                              v_DocNumber = a.v_DocNumber,
                              v_AdressLocation = a.v_AdressLocation,
                              DepartamentoPaciente = a.DepartamentoPaciente,
                              ProvinciaPaciente = a.ProvinciaPaciente,
                              DistritoPaciente = a.DistritoPaciente,
                              i_ResidenceInWorkplaceId = a.i_ResidenceInWorkplaceId,
                              v_ResidenceTimeInWorkplace = a.v_ResidenceTimeInWorkplace,
                              i_TypeOfInsuranceId = a.i_TypeOfInsuranceId,
                              Email = a.Email,
                              Telefono = a.Telefono,
                              EstadoCivil = a.EstadoCivil,
                              GradoInstruccion = a.GradoInstruccion,
                              v_Story = a.v_Story,
                              i_AptitudeStatusId = a.i_AptitudeStatusId,
                              v_OwnerOrganizationName = (from n in dbContext.organization
                                                         where n.v_OrganizationId == Constants.OWNER_ORGNIZATION_ID
                                                         select n.v_Name + " " + n.v_Address).SingleOrDefault<string>(),

                              HijosVivos = a.HijosVivos,
                              HijosMuertos = a.HijosMuertos,
                              HijosDependientes = a.HijosDependientes,
                              v_BirthPlace = a.v_BirthPlace,
                              i_PlaceWorkId = a.i_PlaceWorkId,
                              v_ExploitedMineral = a.v_ExploitedMineral,
                              i_AltitudeWorkId = a.i_AltitudeWorkId,
                              v_EmergencyPhone = a.v_EmergencyPhone,
                              i_SexTypeId = a.i_SexTypeId,
                              i_MaritalStatusId = a.i_MaritalStatusId,
                              i_LevelOfId = a.i_LevelOfId,

                              FirmaTrabajador = a.FirmaTrabajador,
                              HuellaTrabajador = a.HuellaTrabajador,

                              //Datos del Doctor
                              FirmaDoctor = a.FirmaDoctor,
                              NombreDoctor = a.NombreDoctor,
                              CMP = a.CMP,

                              d_Fur = a.d_Fur,
                              v_CatemenialRegime = a.v_CatemenialRegime,
                              i_MacId = a.i_MacId,
                              v_Mac = a.v_Mac,

                              //// Antecedentes ginecologicos
                              d_PAP = a.d_PAP,
                              d_Mamografia = a.d_Mamografia,
                              v_CiruGine = a.v_CiruGine,
                              v_Gestapara = a.v_Gestapara,
                              v_Menarquia = a.v_Menarquia,
                              v_Findings = a.v_Findings,

                          }).FirstOrDefault();

               return sql;
           }
           catch (Exception ex)
           {
               return null;
           }
       }

       public int GetAge(DateTime FechaNacimiento)
       {
           return int.Parse((DateTime.Today.AddTicks(-FechaNacimiento.Ticks).Year - 1).ToString());

       }

       public string GetCantidadCaries(string pstrServiceId, string pstrComponentId, string pstrFieldId)
       {
           try
           {
               string Retornar = "0";
               string[] componentId = null;
               ServiceBL oServiceBL = new ServiceBL();
               List<Sigesoft.Node.WinClient.BE.ServiceComponentFieldValuesList> oServiceComponentFieldValuesList = new List<Sigesoft.Node.WinClient.BE.ServiceComponentFieldValuesList>();

               oServiceComponentFieldValuesList = oServiceBL.ValoresComponenteOdontograma1(pstrServiceId, pstrComponentId);
               var xx = oServiceComponentFieldValuesList.Count() == 0 || ((Sigesoft.Node.WinClient.BE.ServiceComponentFieldValuesList)oServiceComponentFieldValuesList.Find(p => p.v_ComponentFieldId == pstrFieldId)) == null ? string.Empty : ((Sigesoft.Node.WinClient.BE.ServiceComponentFieldValuesList)oServiceComponentFieldValuesList.Find(p => p.v_ComponentFieldId == pstrFieldId)).v_Value1;

               componentId = xx.Split(';');
               if (componentId[0] == "")
               {
                   Retornar = "0";
               }
               else
               {
                   Retornar = componentId.Count().ToString();
               }
               return Retornar;
           }
           catch (Exception)
           {

               throw;
           }

       }

        public List<Sigesoft.Node.WinClient.BE.ServiceComponentFieldValuesList> ValoresComponenteOdontograma1(string pstrServiceId, string pstrComponentId)
        {
            SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
            try
            {
                List<Sigesoft.Node.WinClient.BE.ServiceComponentFieldValuesList> serviceComponentFieldValues = (from A in dbContext.service
                                                                                     join B in dbContext.servicecomponent on A.v_ServiceId equals B.v_ServiceId
                                                                                     join C in dbContext.servicecomponentfields on B.v_ServiceComponentId equals C.v_ServiceComponentId
                                                                                     join D in dbContext.servicecomponentfieldvalues on C.v_ServiceComponentFieldsId equals D.v_ServiceComponentFieldsId

                                                                                     where (A.v_ServiceId == pstrServiceId)
                                                                                           && (B.v_ComponentId == pstrComponentId)
                                                                                           && (B.i_IsDeleted == 0)
                                                                                           && (C.i_IsDeleted == 0)

                                                                                     select new Sigesoft.Node.WinClient.BE.ServiceComponentFieldValuesList
                                                                                     {
                                                                                         //v_ComponentId = B.v_ComponentId,
                                                                                         v_ComponentFieldId = C.v_ComponentFieldId,
                                                                                         //v_ComponentFieldId = G.v_ComponentFieldId,
                                                                                         //v_ComponentFielName = G.v_TextLabel,
                                                                                         v_ServiceComponentFieldsId = C.v_ServiceComponentFieldsId,
                                                                                         v_Value1 = D.v_Value1
                                                                                     }).ToList();


                return serviceComponentFieldValues;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public string GetCantidadAusentes(string pstrServiceId, string pstrComponentId, string pstrFieldId)
        {
            try
            {
                string retornar = "0";
                string[] componentId = null;
                ServiceBL oServiceBL = new ServiceBL();
                List<Sigesoft.Node.WinClient.BE.ServiceComponentFieldValuesList> oServiceComponentFieldValuesList = new List<Sigesoft.Node.WinClient.BE.ServiceComponentFieldValuesList>();

                oServiceComponentFieldValuesList = oServiceBL.ValoresComponenteOdontograma1(pstrServiceId, pstrComponentId);
                var xx = oServiceComponentFieldValuesList.Count() == 0 || ((Sigesoft.Node.WinClient.BE.ServiceComponentFieldValuesList)oServiceComponentFieldValuesList.Find(p => p.v_ComponentFieldId == pstrFieldId)) == null ? string.Empty : ((Sigesoft.Node.WinClient.BE.ServiceComponentFieldValuesList)oServiceComponentFieldValuesList.Find(p => p.v_ComponentFieldId == pstrFieldId)).v_Value1;

                componentId = xx.Split(';');

                if (componentId[0] == "")
                {
                    retornar = "0";
                }
                else
                {
                    retornar = componentId.Count().ToString();
                }
                return retornar;
            }
            catch (Exception)
            {

                throw;
            }

        }
       
        public List<Sigesoft.Node.WinClient.BE.ServiceComponentFieldValuesList> ValoresComponenteOdontogramaValue1(string pstrServiceId, string pstrComponentId)
        {
            SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
            try
            {
                List<Sigesoft.Node.WinClient.BE.ServiceComponentFieldValuesList> serviceComponentFieldValues = (from A in dbContext.service
                                                                                     join B in dbContext.servicecomponent on A.v_ServiceId equals B.v_ServiceId
                                                                                     join C in dbContext.servicecomponentfields on B.v_ServiceComponentId equals C.v_ServiceComponentId
                                                                                     join D in dbContext.servicecomponentfieldvalues on C.v_ServiceComponentFieldsId equals D.v_ServiceComponentFieldsId

                                                                                     where (A.v_ServiceId == pstrServiceId)
                                                                                           && (B.v_ComponentId == pstrComponentId)
                                                                                           && (B.i_IsDeleted == 0)
                                                                                           && (C.i_IsDeleted == 0)

                                                                                     select new Sigesoft.Node.WinClient.BE.ServiceComponentFieldValuesList
                                                                                     {
                                                                                         //v_ComponentId = B.v_ComponentId,
                                                                                         v_ComponentFieldId = C.v_ComponentFieldId,
                                                                                         //v_ComponentFieldId = G.v_ComponentFieldId,
                                                                                         //v_ComponentFielName = G.v_TextLabel,
                                                                                         v_ServiceComponentFieldsId = C.v_ServiceComponentFieldsId,
                                                                                         v_Value1 = D.v_Value1
                                                                                     }).ToList();


                return serviceComponentFieldValues;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public List<Sigesoft.Node.WinClient.BE.ServiceComponentFieldValuesList> ValoresComponente(string pstrServiceId, string pstrComponentId)
        {
            SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
            int rpta = 0;

            try
            {
                var serviceComponentFieldValues = (from A in dbContext.service
                                                   join B in dbContext.servicecomponent on A.v_ServiceId equals B.v_ServiceId
                                                   join C in dbContext.servicecomponentfields on B.v_ServiceComponentId equals C.v_ServiceComponentId
                                                   join D in dbContext.servicecomponentfieldvalues on C.v_ServiceComponentFieldsId equals D.v_ServiceComponentFieldsId
                                                   join E in dbContext.component on B.v_ComponentId equals E.v_ComponentId
                                                   join F in dbContext.componentfields on C.v_ComponentFieldId equals F.v_ComponentFieldId
                                                   join G in dbContext.componentfield on C.v_ComponentFieldId equals G.v_ComponentFieldId
                                                   join H in dbContext.component on F.v_ComponentId equals H.v_ComponentId

                                                   where A.v_ServiceId == pstrServiceId
                                                           && H.v_ComponentId == pstrComponentId
                                                           && B.i_IsDeleted == 0
                                                           && C.i_IsDeleted == 0

                                                   select new Sigesoft.Node.WinClient.BE.ServiceComponentFieldValuesList
                                                   {
                                                       v_ComponentFieldId = G.v_ComponentFieldId,
                                                       v_ComponentFielName = G.v_TextLabel,
                                                       v_ServiceComponentFieldsId = C.v_ServiceComponentFieldsId,
                                                       v_Value1 = D.v_Value1,
                                                       i_GroupId = G.i_GroupId.Value
                                                   });

                var finalQuery = (from a in serviceComponentFieldValues.ToList()

                                  let value1 = int.TryParse(a.v_Value1, out rpta)
                                  join sp in dbContext.systemparameter on new { a = a.i_GroupId, b = rpta }
                                                  equals new { a = sp.i_GroupId, b = sp.i_ParameterId } into sp_join
                                  from sp in sp_join.DefaultIfEmpty()

                                  select new Sigesoft.Node.WinClient.BE.ServiceComponentFieldValuesList
                                  {
                                      v_ComponentFieldId = a.v_ComponentFieldId,
                                      v_ComponentFielName = a.v_ComponentFielName,
                                      v_ServiceComponentFieldsId = a.v_ServiceComponentFieldsId,
                                      v_Value1 = a.v_Value1,
                                      v_Value1Name = sp == null ? "" : sp.v_Value1
                                  }).ToList();


                return finalQuery;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public List<Sigesoft.Node.WinClient.BE.ServiceComponentFieldValuesList> ValoresExamenComponete(string pstrServiceId, string pstrComponentId, int pintParameter)
        {

            SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

            var systemParameters = (from a in dbContext.systemparameter
                                    where a.i_GroupId == pintParameter
                                    select a);


            List<Sigesoft.Node.WinClient.BE.ServiceComponentFieldValuesList> serviceComponentFieldValues = (from A in dbContext.service
                                                                                 join B in dbContext.servicecomponent on A.v_ServiceId equals B.v_ServiceId
                                                                                 join C in dbContext.servicecomponentfields on B.v_ServiceComponentId equals C.v_ServiceComponentId
                                                                                 join D in dbContext.servicecomponentfieldvalues on C.v_ServiceComponentFieldsId equals D.v_ServiceComponentFieldsId
                                                                                 join E in dbContext.component on B.v_ComponentId equals E.v_ComponentId
                                                                                 join F in dbContext.componentfields on C.v_ComponentFieldId equals F.v_ComponentFieldId
                                                                                 join G in dbContext.componentfield on C.v_ComponentFieldId equals G.v_ComponentFieldId
                                                                                 join H in dbContext.component on F.v_ComponentId equals H.v_ComponentId
                                                                                 where A.v_ServiceId == pstrServiceId
                                                                                         && H.v_ComponentId == pstrComponentId
                                                                                         && B.i_IsDeleted == 0
                                                                                         && C.i_IsDeleted == 0

                                                                                                            select new Sigesoft.Node.WinClient.BE.ServiceComponentFieldValuesList
                                                                                 {
                                                                                     v_ComponentFieldId = G.v_ComponentFieldId,
                                                                                     v_ComponentFielName = G.v_TextLabel,
                                                                                     v_ServiceComponentFieldsId = C.v_ServiceComponentFieldsId,
                                                                                     v_Value1 = D.v_Value1

                                                                                 }).ToList();


            var sql = (from A in serviceComponentFieldValues
                       join F in systemParameters on A.v_Value1 equals F.i_ParameterId.ToString() into F_join
                       from F in F_join.DefaultIfEmpty()
                       select new Sigesoft.Node.WinClient.BE.ServiceComponentFieldValuesList
                       {
                           v_ComponentFieldId = A.v_ComponentFieldId,
                           v_ComponentFielName = A.v_ComponentFielName,
                           v_ServiceComponentFieldValuesId = A.v_ServiceComponentFieldValuesId,
                           v_ComponentFieldValuesId = A.v_ComponentFieldValuesId,
                           v_ServiceComponentFieldsId = A.v_ServiceComponentFieldsId,
                           v_Value1 = A.v_Value1,
                           v_Value2 = A.v_Value2,
                           i_Index = A.i_Index,
                           i_Value1 = A.i_Value1,
                           v_Value1Name = F == null ? "" : F.v_Value1
                       }).ToList();

            return sql;
        }

        public List<Sigesoft.Node.WinClient.BE.DiagnosticRepositoryList> GetServiceDisgnosticsReports(string pstrServiceId)
        {
            //mon.IsActive = true;
            var isDeleted = 0;

            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
                List<Sigesoft.Node.WinClient.BE.DiagnosticRepositoryList> query = (from ccc in dbContext.diagnosticrepository
                                                        //join bbb in dbContext.servicecomponent on ccc.v_ServiceId equals bbb.v_ServiceId

                                                        join ddd in dbContext.diseases on ccc.v_DiseasesId equals ddd.v_DiseasesId  // Diagnosticos

                                                        join eee in dbContext.systemparameter on new { a = ccc.i_AutoManualId.Value, b = 136 } // Auto / Manual
                                                                                                equals new { a = eee.i_ParameterId, b = eee.i_GroupId }

                                                        join fff in dbContext.systemparameter on new { a = ccc.i_PreQualificationId.Value, b = 137 } // PRE-CALIFICACION
                                                                                            equals new { a = fff.i_ParameterId, b = fff.i_GroupId } into J5_join
                                                        from fff in J5_join.DefaultIfEmpty()

                                                        join ggg in dbContext.systemparameter on new { a = ccc.i_FinalQualificationId.Value, b = 138 } //CALIFICACION FINAL
                                                                                            equals new { a = ggg.i_ParameterId, b = ggg.i_GroupId } into J4_join
                                                        from ggg in J4_join.DefaultIfEmpty()

                                                        join hhh in dbContext.systemparameter on new { a = ccc.i_DiagnosticTypeId.Value, b = 139 } // TIPO DE DX [Enfermedad comun, etc]
                                                                                                equals new { a = hhh.i_ParameterId, b = hhh.i_GroupId } into J3_join
                                                        from hhh in J3_join.DefaultIfEmpty()

                                                        join iii in dbContext.systemparameter on new { a = ccc.i_IsSentToAntecedent.Value, b = 111 } // RESPUESTA SI/NO
                                                                                             equals new { a = iii.i_ParameterId, b = iii.i_GroupId } into J6_join
                                                        from iii in J6_join.DefaultIfEmpty()

                                                        join J1 in dbContext.systemuser on new { i_InsertUserId = ccc.i_InsertUserId.Value }
                                                                        equals new { i_InsertUserId = J1.i_SystemUserId } into J1_join
                                                        from J1 in J1_join.DefaultIfEmpty()

                                                        join J2 in dbContext.systemuser on new { i_UpdateUserId = ccc.i_UpdateUserId.Value }
                                                                                        equals new { i_UpdateUserId = J2.i_SystemUserId } into J2_join
                                                        from J2 in J2_join.DefaultIfEmpty()

                                                        where (ccc.v_ServiceId == pstrServiceId) &&
                                                              (ccc.i_IsDeleted == isDeleted)
                                                        select new Sigesoft.Node.WinClient.BE.DiagnosticRepositoryList
                                                        {
                                                            v_DiagnosticRepositoryId = ccc.v_DiagnosticRepositoryId,
                                                            v_ServiceId = ccc.v_ServiceId,
                                                            v_ComponentId = ccc.v_ComponentId,
                                                            //v_ComponentName = bbb.v_Name,
                                                            v_DiseasesId = ccc.v_DiseasesId,
                                                            i_AutoManualId = ccc.i_AutoManualId,
                                                            i_PreQualificationId = ccc.i_PreQualificationId,
                                                            i_FinalQualificationId = ccc.i_FinalQualificationId, //sirve
                                                            i_DiagnosticTypeId = ccc.i_DiagnosticTypeId,//sirve
                                                            i_IsSentToAntecedent = ccc.i_IsSentToAntecedent,
                                                            d_ExpirationDateDiagnostic = ccc.d_ExpirationDateDiagnostic,
                                                            i_GenerateMedicalBreak = ccc.i_GenerateMedicalBreak,
                                                            v_ComponentFieldsId = ccc.v_ComponentFieldId,
                                                            v_DiseasesName = ddd.v_Name,
                                                            v_Cie10 = ddd.v_CIE10Id,
                                                            v_AutoManualName = eee.v_Value1,

                                                            v_PreQualificationName = fff.v_Value1,
                                                            v_FinalQualificationName = ggg.v_Value1,
                                                            v_DiagnosticTypeName = hhh.v_Value1,
                                                            v_IsSentToAntecedentName = iii.v_Value1,
                                                            i_RecordStatus = (int)RecordStatus.Grabado,
                                                            i_RecordType = (int)RecordType.NoTemporal,

                                                            v_CreationUser = J1.v_UserName,
                                                            v_UpdateUser = J2.v_UserName,
                                                            d_CreationDate = J1.d_InsertDate,
                                                            d_UpdateDate = J2.d_UpdateDate,
                                                            i_IsDeleted = ccc.i_IsDeleted.Value
                                                        }).ToList();


                var q = new List<Sigesoft.Node.WinClient.BE.DiagnosticRepositoryList>();
                q = query.Select((a, index) => new Sigesoft.Node.WinClient.BE.DiagnosticRepositoryList
                {
                    i_Item = index + 1,
                    v_DiagnosticRepositoryId = a.v_DiagnosticRepositoryId,
                    v_ServiceId = a.v_ServiceId,
                    v_ComponentId = a.v_ComponentId,
                    v_ComponentName = a.v_ComponentName,
                    v_DiseasesId = a.v_DiseasesId,
                    i_AutoManualId = a.i_AutoManualId,
                    i_PreQualificationId = a.i_PreQualificationId,
                    i_FinalQualificationId = a.i_FinalQualificationId,
                    i_DiagnosticTypeId = a.i_DiagnosticTypeId,
                    i_IsSentToAntecedent = a.i_IsSentToAntecedent,
                    d_ExpirationDateDiagnostic = a.d_ExpirationDateDiagnostic,
                    i_GenerateMedicalBreak = a.i_GenerateMedicalBreak,
                    v_ComponentFieldsId = a.v_ComponentFieldsId,

                    v_RestrictionsName = ConcatenateRestriction(a.v_DiagnosticRepositoryId),
                    v_RecomendationsName = ConcatenateRecommendation(a.v_DiagnosticRepositoryId),
                    v_DiseasesName = a.v_DiseasesName,
                    v_Cie10 = a.v_Cie10,
                    v_AutoManualName = a.v_AutoManualName,

                    v_PreQualificationName = a.v_PreQualificationName,
                    v_FinalQualificationName = a.v_FinalQualificationName,
                    v_DiagnosticTypeName = a.v_DiagnosticTypeName,
                    v_IsSentToAntecedentName = a.v_IsSentToAntecedentName,
                    i_RecordStatus = a.i_RecordStatus,
                    i_RecordType = a.i_RecordType,

                    v_CreationUser = a.v_CreationUser,
                    v_UpdateUser = a.v_UpdateUser,
                    d_CreationDate = a.d_CreationDate,
                    d_UpdateDate = a.d_UpdateDate,
                    i_IsDeleted = a.i_IsDeleted

                }).ToList();

                // Agregamos Restricciones / Recomendaciones
                OperationResult objOperationResult = new OperationResult();

                foreach (Sigesoft.Node.WinClient.BE.DiagnosticRepositoryList dr in q)
                {
                    dr.Restrictions = GetServiceRestrictionsByDiagnosticRepositoryId(ref objOperationResult, dr.v_DiagnosticRepositoryId);
                    dr.Recomendations = GetServiceRecommendationByDiagnosticRepositoryId(ref objOperationResult, dr.v_DiagnosticRepositoryId);
                }

                return q;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<Sigesoft.Node.WinClient.BE.RestrictionList> GetServiceRestrictionsByDiagnosticRepositoryId(ref OperationResult pobjOperationResult, string pstrDiagnosticRepositoryId)
        {
            //mon.IsActive = true;

            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                List<Sigesoft.Node.WinClient.BE.RestrictionList> query = (from ddd in dbContext.restriction  // RESTRICCIONES 
                                               join eee in dbContext.masterrecommendationrestricction on ddd.v_MasterRestrictionId equals eee.v_MasterRecommendationRestricctionId // RESTRICIONES

                                               join J1 in dbContext.systemuser on new { i_InsertUserId = ddd.i_InsertUserId.Value }
                                                               equals new { i_InsertUserId = J1.i_SystemUserId } into J1_join
                                               from J1 in J1_join.DefaultIfEmpty()

                                               join J2 in dbContext.systemuser on new { i_UpdateUserId = ddd.i_UpdateUserId.Value }
                                                                               equals new { i_UpdateUserId = J2.i_SystemUserId } into J2_join
                                               from J2 in J2_join.DefaultIfEmpty()

                                               where ddd.v_DiagnosticRepositoryId == pstrDiagnosticRepositoryId &&
                                                     ddd.i_IsDeleted == 0
                                                                          select new Sigesoft.Node.WinClient.BE.RestrictionList
                                               {
                                                   //v_RestrictionByDiagnosticId = ddd.v_RestrictionByDiagnosticId,
                                                   v_RestrictionByDiagnosticId = ddd.v_RestrictionId,
                                                   v_DiagnosticRepositoryId = ddd.v_DiagnosticRepositoryId,
                                                   v_ServiceId = ddd.v_ServiceId,
                                                   v_ComponentId = ddd.v_ComponentId,
                                                   v_MasterRestrictionId = ddd.v_MasterRestrictionId,
                                                   v_RestrictionName = eee.v_Name,
                                                   i_RecordStatus = (int)RecordStatus.Grabado,
                                                   i_RecordType = (int)RecordType.NoTemporal,
                                                   v_CreationUser = J1.v_UserName,
                                                   v_UpdateUser = J2.v_UserName,
                                                   d_CreationDate = J1.d_InsertDate,
                                                   d_UpdateDate = J2.d_UpdateDate,
                                                   i_IsDeleted = ddd.i_IsDeleted.Value
                                               }).ToList();



                //List<DiagnosticRepositoryList> objData = query.ToList();
                pobjOperationResult.Success = 1;
                return query;
            }
            catch (Exception ex)
            {
                pobjOperationResult.Success = 0;
                pobjOperationResult.ExceptionMessage = Common.Utils.ExceptionFormatter(ex);
                return null;
            }
        }

        public List<Sigesoft.Node.WinClient.BE.RecomendationList> GetServiceRecommendationByDiagnosticRepositoryId(ref OperationResult pobjOperationResult, string pstrDiagnosticRepositoryId)
        {
            //mon.IsActive = true;

            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                List<Sigesoft.Node.WinClient.BE.RecomendationList> query = (from ddd in dbContext.recommendation
                                                 join eee in dbContext.masterrecommendationrestricction on ddd.v_MasterRecommendationId equals eee.v_MasterRecommendationRestricctionId //                                                                                                                                  

                                                 join J1 in dbContext.systemuser on new { i_InsertUserId = ddd.i_InsertUserId.Value }
                                                                 equals new { i_InsertUserId = J1.i_SystemUserId } into J1_join
                                                 from J1 in J1_join.DefaultIfEmpty()

                                                 join J2 in dbContext.systemuser on new { i_UpdateUserId = ddd.i_UpdateUserId.Value }
                                                                                 equals new { i_UpdateUserId = J2.i_SystemUserId } into J2_join
                                                 from J2 in J2_join.DefaultIfEmpty()

                                                 where ddd.v_DiagnosticRepositoryId == pstrDiagnosticRepositoryId &&
                                                       ddd.i_IsDeleted == 0
                                                select new Sigesoft.Node.WinClient.BE.RecomendationList
                                                 {
                                                     v_RecommendationId = ddd.v_RecommendationId,
                                                     v_DiagnosticRepositoryId = ddd.v_DiagnosticRepositoryId,
                                                     v_ServiceId = ddd.v_ServiceId,
                                                     v_ComponentId = ddd.v_ComponentId,
                                                     v_MasterRecommendationId = ddd.v_MasterRecommendationId,
                                                     v_RecommendationName = eee.v_Name,
                                                     i_RecordStatus = (int)RecordStatus.Grabado,
                                                     i_RecordType = (int)RecordType.NoTemporal,
                                                     v_CreationUser = J1.v_UserName,
                                                     v_UpdateUser = J2.v_UserName,
                                                     d_CreationDate = J1.d_InsertDate,
                                                     d_UpdateDate = J2.d_UpdateDate,
                                                     i_IsDeleted = ddd.i_IsDeleted.Value
                                                 }).ToList();



                //List<DiagnosticRepositoryList> objData = query.ToList();
                pobjOperationResult.Success = 1;
                return query;
            }
            catch (Exception ex)
            {
                pobjOperationResult.Success = 0;
                pobjOperationResult.ExceptionMessage = Common.Utils.ExceptionFormatter(ex);
                return null;
            }
        }

        public List<Sigesoft.Node.WinClient.BE.RecomendationList> GetServiceRecommendationByServiceId(string pstrServiceId)
        {
            //mon.IsActive = true;

            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                List<Sigesoft.Node.WinClient.BE.RecomendationList> query = (from ddd in dbContext.recommendation
                                                 join fff in dbContext.diagnosticrepository on ddd.v_DiagnosticRepositoryId
                                                                                 equals fff.v_DiagnosticRepositoryId into J7_join
                                                 from fff in J7_join.DefaultIfEmpty()

                                                 join eee in dbContext.masterrecommendationrestricction on ddd.v_MasterRecommendationId equals eee.v_MasterRecommendationRestricctionId  // RECOMENDACIONES                                                                                                                                                                                                                                                         

                                                 join J1 in dbContext.systemuser on new { i_InsertUserId = ddd.i_InsertUserId.Value }
                                                                 equals new { i_InsertUserId = J1.i_SystemUserId } into J1_join
                                                 from J1 in J1_join.DefaultIfEmpty()

                                                 join J2 in dbContext.systemuser on new { i_UpdateUserId = ddd.i_UpdateUserId.Value }
                                                                                 equals new { i_UpdateUserId = J2.i_SystemUserId } into J2_join
                                                 from J2 in J2_join.DefaultIfEmpty()
                                                 where (ddd.v_ServiceId == pstrServiceId) &&
                                                 (ddd.i_IsDeleted == 0) &&
                                                 (fff.i_FinalQualificationId == (int)FinalQualification.Definitivo ||
                                                 fff.i_FinalQualificationId == (int)FinalQualification.Presuntivo)
                                                                            select new Sigesoft.Node.WinClient.BE.RecomendationList
                                                 {
                                                     v_RecommendationId = ddd.v_RecommendationId,
                                                     v_DiagnosticRepositoryId = ddd.v_DiagnosticRepositoryId,
                                                     v_ServiceId = ddd.v_ServiceId,
                                                     v_ComponentId = ddd.v_ComponentId,
                                                     v_MasterRecommendationId = ddd.v_MasterRecommendationId,
                                                     v_RecommendationName = eee.v_Name,
                                                     i_RecordStatus = (int)RecordStatus.Grabado,
                                                     i_RecordType = (int)RecordType.NoTemporal
                                                 }).ToList();

                var query1 = new List<Sigesoft.Node.WinClient.BE.RecomendationList>();

                query1 = query.Select((x, index) => new Sigesoft.Node.WinClient.BE.RecomendationList
                {
                    i_Item = index + 1,
                    v_RecommendationId = x.v_RecommendationId,
                    v_DiagnosticRepositoryId = x.v_DiagnosticRepositoryId,
                    v_ServiceId = x.v_ServiceId,
                    v_ComponentId = x.v_ComponentId,
                    v_MasterRecommendationId = x.v_MasterRecommendationId,
                    v_RecommendationName = x.v_RecommendationName,
                    i_RecordStatus = x.i_RecordStatus,
                    i_RecordType = x.i_RecordType
                }).ToList();

                return query1;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
              
        public List<ServiceComponentList> GetServiceComponentsForManagementReport(string pstrServiceId)
        {
            //mon.IsActive = true;        
            var isDeleted = 0;
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
                var components = (from aaa in dbContext.servicecomponent
                                  join bbb in dbContext.component on aaa.v_ComponentId equals bbb.v_ComponentId
                                  join J1 in dbContext.systemuser on new { i_InsertUserId = aaa.i_InsertUserId.Value }
                                                  equals new { i_InsertUserId = J1.i_SystemUserId } into J1_join
                                  from J1 in J1_join.DefaultIfEmpty()

                                  join J2 in dbContext.systemuser on new { i_UpdateUserId = aaa.i_UpdateUserId.Value }
                                                                  equals new { i_UpdateUserId = J2.i_SystemUserId } into J2_join
                                  from J2 in J2_join.DefaultIfEmpty()

                                  join fff in dbContext.systemparameter on new { a = bbb.i_CategoryId.Value, b = 116 } // CATEGORIA DEL EXAMEN
                                                                               equals new { a = fff.i_ParameterId, b = fff.i_GroupId } into J5_join
                                  from fff in J5_join.DefaultIfEmpty()

                                  where (aaa.v_ServiceId == pstrServiceId) &&
                                        (bbb.i_ComponentTypeId == (int?)ComponentType.Examen) &&
                                        (aaa.i_IsDeleted == isDeleted) &&
                                        (aaa.i_IsRequiredId == (int?)SiNo.SI)
                                  orderby bbb.i_CategoryId, bbb.v_Name

                                  select new ServiceComponentList
                                  {
                                      v_ComponentId = bbb.v_ComponentId,
                                      v_ComponentName = bbb.v_Name,
                                      v_ServiceComponentId = aaa.v_ServiceComponentId,
                                      i_CategoryId = bbb.i_CategoryId.Value,
                                      v_CategoryName = fff.v_Value1

                                  }).ToList();

                return components;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Sigesoft.Node.WinClient.BE.ServiceList> ReportFuncionesVitales(string pstrserviceId, string pstrComponentId)
        {
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();


                var objEntity = (from A in dbContext.service
                                 join B in dbContext.person on A.v_PersonId equals B.v_PersonId
                                 join E in dbContext.servicecomponent on new { a = pstrserviceId, b = pstrComponentId }
                                                                        equals new { a = E.v_ServiceId, b = E.v_ComponentId }

                                 //join F in dbContext.systemuser on E.i_UpdateUserId equals F.i_SystemUserId

                                 //join G in dbContext.professional on new { a = F.v_PersonId }
                                 //                                     equals new { a = G.v_PersonId } into G_join
                                 //from G in G_join.DefaultIfEmpty()

                                 //join H in dbContext.person on F.v_PersonId equals H.v_PersonId

                                 //join I in dbContext.protocol on A.v_ProtocolId equals I.v_ProtocolId

                                 //join J in dbContext.organization on I.v_EmployerOrganizationId equals J.v_OrganizationId


                                 where A.v_ServiceId == pstrserviceId
                                 select new Sigesoft.Node.WinClient.BE.ServiceList
                                 {
                                     v_PersonId = A.v_PersonId,
                                     v_NamePacient = B.v_FirstName,
                                     v_Surnames = B.v_FirstLastName + " " + B.v_SecondLastName,
                                     DireccionPaciente = B.v_AdressLocation,
                                     d_BirthDate = B.d_Birthdate,
                                     d_ServiceDate = A.d_ServiceDate,
                                     v_ServiceId = A.v_ServiceId,
                                     v_DocNumber = B.v_DocNumber


                                 });

                var sql = (from a in objEntity.ToList()
                           select new Sigesoft.Node.WinClient.BE.ServiceList
                           {

                               FC = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.FUNCIONES_VITALES_FREC_CARDIACA_ID, "NOCOMBO", 0, "SI"),
                               PA = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.FUNCIONES_VITALES_PAS_ID, "NOCOMBO", 0, "SI"),
                               FR = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.FUNCIONES_VITALES_FREC_RESPIRATORIA_ID, "NOCOMBO", 0, "SI"),
                               //IMC = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.FUNCIONES_VITALES_im_ID, "NOCOMBO", 0, "SI"),
                               Sat = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.FUNCIONES_VITALES_SAT_O2_ID, "NOCOMBO", 0, "SI"),
                               PAD = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.FUNCIONES_VITALES_PAD_ID, "NOCOMBO", 0, "SI")

                           }).ToList();

                return sql;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<Sigesoft.Node.WinClient.BE.ServiceList> ReportAntropometria(string pstrserviceId, string pstrComponentId)
        {
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();


                var objEntity = (from A in dbContext.service
                                 join B in dbContext.person on A.v_PersonId equals B.v_PersonId
                                 join E in dbContext.servicecomponent on new { a = pstrserviceId, b = pstrComponentId }
                                                                        equals new { a = E.v_ServiceId, b = E.v_ComponentId }

                                 //join F in dbContext.systemuser on E.i_UpdateUserId equals F.i_SystemUserId

                                 //join G in dbContext.professional on new { a = F.v_PersonId }
                                 //                                     equals new { a = G.v_PersonId } into G_join
                                 //from G in G_join.DefaultIfEmpty()

                                 //join H in dbContext.person on F.v_PersonId equals H.v_PersonId

                                 //join I in dbContext.protocol on A.v_ProtocolId equals I.v_ProtocolId

                                 //join J in dbContext.organization on I.v_EmployerOrganizationId equals J.v_OrganizationId


                                 where A.v_ServiceId == pstrserviceId
                                 select new Sigesoft.Node.WinClient.BE.ServiceList
                                 {
                                     v_PersonId = A.v_PersonId,
                                     v_NamePacient = B.v_FirstName,
                                     v_Surnames = B.v_FirstLastName + " " + B.v_SecondLastName,
                                     DireccionPaciente = B.v_AdressLocation,
                                     d_BirthDate = B.d_Birthdate,
                                     d_ServiceDate = A.d_ServiceDate,
                                     v_ServiceId = A.v_ServiceId,
                                     v_DocNumber = B.v_DocNumber


                                 }).ToList();

                var sql = (from a in objEntity.ToList()
                           select new Sigesoft.Node.WinClient.BE.ServiceList
                           {
                               IMC = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.ANTROPOMETRIA_IMC_ID, "NOCOMBO", 0, "SI"),
                               Peso = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.ANTROPOMETRIA_PESO_ID, "NOCOMBO", 0, "SI"),
                               talla = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.ANTROPOMETRIA_TALLA_ID, "NOCOMBO", 0, "SI"),
                           }).ToList();

                return sql;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<Sigesoft.Node.WinClient.BE.RestrictionList> GetServiceRestrictionByDiagnosticRepositoryIdReport(string pstrDiagnosticRepositoryId)
        {
            //mon.IsActive = true;

            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                List<Sigesoft.Node.WinClient.BE.RestrictionList> query = (from ddd in dbContext.restriction
                                               join eee in dbContext.masterrecommendationrestricction on ddd.v_MasterRestrictionId
                                                                       equals eee.v_MasterRecommendationRestricctionId //                                                                                                                                                                              
                                               where (ddd.v_DiagnosticRepositoryId == pstrDiagnosticRepositoryId) &&
                                                     (ddd.i_IsDeleted == 0)
                                                select new Sigesoft.Node.WinClient.BE.RestrictionList
                                               {
                                                   v_RestrictionId = ddd.v_RestrictionId,
                                                   v_DiagnosticRepositoryId = ddd.v_DiagnosticRepositoryId,
                                                   v_ServiceId = ddd.v_ServiceId,
                                                   v_ComponentId = ddd.v_ComponentId,
                                                   v_MasterRestrictionId = ddd.v_MasterRestrictionId,
                                                   v_RestrictionName = eee.v_Name,

                                               }).ToList();

                // add the sequence number on the fly
                var finalQuery = query.Select((a, index) => new Sigesoft.Node.WinClient.BE.RestrictionList
                {
                    i_Item = index + 1,
                    v_RestrictionId = a.v_RestrictionId,
                    v_DiagnosticRepositoryId = a.v_DiagnosticRepositoryId,
                    v_ServiceId = a.v_ServiceId,
                    v_ComponentId = a.v_ComponentId,
                    v_MasterRestrictionId = a.v_MasterRestrictionId,
                    v_RestrictionName = a.v_RestrictionName,
                }).ToList();

                return finalQuery;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public List<Sigesoft.Node.WinClient.BE.ReportOsteoMuscular> ReportOsteoMuscular(string pstrserviceId, string pstrComponentId)
        {
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                var objEntity = (from A in dbContext.service
                                 join B in dbContext.person on A.v_PersonId equals B.v_PersonId
                                 join E in dbContext.servicecomponent on new { a = pstrserviceId, b = pstrComponentId }
                                                                        equals new { a = E.v_ServiceId, b = E.v_ComponentId }

                                 // Usuario Medico Evaluador / Medico Aprobador ****************************
                                 join me in dbContext.systemuser on E.i_ApprovedUpdateUserId equals me.i_SystemUserId into me_join
                                 from me in me_join.DefaultIfEmpty()

                                 join pme in dbContext.professional on me.v_PersonId equals pme.v_PersonId into pme_join
                                 from pme in pme_join.DefaultIfEmpty()
                                 //**********************************************************************************************

                                 join I in dbContext.protocol on A.v_ProtocolId equals I.v_ProtocolId into I_join
                                 from I in I_join.DefaultIfEmpty()

                                 join J in dbContext.organization on I.v_EmployerOrganizationId equals J.v_OrganizationId

                                 join L in dbContext.systemparameter on new { a = I.i_EsoTypeId.Value, b = 118 }
                                                 equals new { a = L.i_ParameterId, b = L.i_GroupId } into L_join
                                 from L in L_join.DefaultIfEmpty()

                                 where A.v_ServiceId == pstrserviceId

                                 select new Sigesoft.Node.WinClient.BE.ReportOsteoMuscular
                                 {
                                     IdServicio = A.v_ServiceId,
                                     IdSericioComponente = E.v_ServiceComponentId,
                                     Paciente = B.v_FirstName + " " + B.v_FirstLastName + " " + B.v_SecondLastName,
                                     Puesto = B.v_CurrentOccupation,
                                     Protocolo = I.v_Name,
                                     Empresa = J.v_Name,
                                     TipoExamen = L.v_Value1,
                                     FirmaTrabajador = B.b_RubricImage,
                                     FirmaMedico = pme.b_SignatureImage,
                                     d_BirthDate = B.d_Birthdate.Value,
                                     HuellaTrabajador = B.b_FingerPrintImage
                                 });

                var MedicalCenter = GetInfoMedicalCenter();

                var sql = (from a in objEntity.ToList()
                           let OsteoMuscular = new ServiceBL().ValoresComponente(pstrserviceId, pstrComponentId)
                           select new Sigesoft.Node.WinClient.BE.ReportOsteoMuscular
                           {
                               IdServicio = a.IdServicio,
                               IdSericioComponente = a.IdSericioComponente,
                               Paciente = a.Paciente,
                               Puesto = a.Puesto,
                               Protocolo = a.Protocolo,
                               Empresa = a.Empresa,
                               TipoExamen = a.TipoExamen,
                               FirmaTrabajador = a.FirmaTrabajador,
                               FirmaMedico = a.FirmaMedico,
                               d_BirthDate = a.d_BirthDate,
                               Edad = GetAge(a.d_BirthDate.Value),

                               MetodoCarga = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_METODO_CARGA).v_Value1Name,
                               AntecedentesSiNo = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_PRESENTA_ANTECEDENTES).v_Value1,
                               AntecedentesDescripcion = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_DESCRIPCION).v_Value1,
                               PosturaSentado = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_POSTURA_SENTADO).v_Value1,
                               PosturaPie = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_POSTURA_PIE).v_Value1,
                               PosturaForzadaSiNo = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_POSTURA_FORZADA).v_Value1,
                               MovCargaManualSiNo = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_MOVIMIENTO_MANUAL_CARGA).v_Value1,
                               PesoCarga = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_PESO_CARGA).v_Value1,
                               MovRepetitivosSiNo = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_MOVIMIENTOS_REPETITIVOS).v_Value1,
                               UsuPantallaPVDSiNo = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_USUARIO_PANTALLA_PVD).v_Value1,
                               HorasDia = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_HORAS_DIA).v_Value1,
                               LordisisCervical = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.POSTURA_FORZADA__).v_Value1Name,
                               CifosisDorsal = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.POSTURA_FORZADA_DESCRIPCION_).v_Value1Name,
                               LordosisLumbar = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.POSTURA_SEDENTARIA__).v_Value1Name,
                               EscoliosisLumbar = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_ESCOLIOSIS_LUMBAR).v_Value1Name,
                               EscofiosisDorsal = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_ESCOLIOSIS_DORSAL).v_Value1Name,
                               DolorEspalda = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.MOVIMIENTOS_REPETITIVOS__).v_Value1Name,
                               ContracturaMuscular = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.SINTOMAS).v_Value1Name,
                               Observaciones = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_OBSERVACIONES).v_Value1,
                               RodillaDerechaVaroSiNo = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_RODILLA_DERECHA_VARO).v_Value1,
                               RodillaDerechaValgoSiNo = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_RODILLA_DERECHA_VALGO).v_Value1,
                               RodillaIzquierdaVaroSiNo = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_RODILLA_IZQUIERDA_VARO).v_Value1,
                               RodillaIzquierdaValgoSiNo = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_RODILLA_IZQUIERDA_VALGO).v_Value1,
                               PieDerechoCavoSiNo = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_PIE_DERECHO_CAVO).v_Value1,
                               PieDerechoPlanoSiNo = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_PIE_DERECHO_PLANO).v_Value1,
                               PieIzquierdoCavoSiNo = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_PIE_IZQUIERDO_CAVO).v_Value1,
                               PieIzquierdoPlanoSiNo = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_PIE_IZQUIERDO_PLANO).v_Value1,
                               ReflejoTotulianoDerechoSiNo = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.CERVICAL_LATERALIZACION_IZQUIERDA).v_Value1Name,
                               //ReflejoTotulianoIzquierdoSiNo = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_REFLEJO_TOTULIANO_IZQUIERDO).v_Value1Name,
                               ReflejoAquileoDerechoSiNo = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.PIE_CAVO_IZQUIERDO).v_Value1Name,
                               ReflejoAquileoIzquierdoSiNo = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.PIE_PLANO_DERECHO).v_Value1Name,
                               TestPhalenDerechoSiNo = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.PHALEN_DERECHO).v_Value1,
                               TestPhalenIzquierdoSiNo = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.PIE_CAVO_DERECHO).v_Value1,
                               TestTinelDerechoSiNo = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.TINEL_DERECHO).v_Value1,
                               TestTinelIzquierdoSiNo = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.TINEL_IZQUIERDO).v_Value1,
                               SignoLasagueIzquierdoSiNo = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.FINKELSTEIN_DERECHO).v_Value1,
                               SignoLasagueDerechoSiNo = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.LASEGUE_DERECHO).v_Value1,
                               SignoBragardIzquierdoSiNo = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.ADAM_IZQUIERDO).v_Value1,
                               SignoBragardDerechoSiNo = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.FINKELSTEIN_IZQUIERDO).v_Value1,

                               TemporoMadibularNID = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_TEMPERO_MANDIBULAR).v_Value1,
                               TemporoMadibularObs = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_TEMPERO_MANDIBULAR_DESCRIPCION).v_Value1,
                               HombroNID = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_HOMBRO).v_Value1,
                               HombroObs = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_HOMBRO_DESCRIPCION).v_Value1,
                               CodoNID = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_CODO).v_Value1,
                               CodoObs = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_CODO_DESCRIPCION).v_Value1,
                               MunecaNID = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_MUNECA).v_Value1,
                               MunecaObs = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_MUNECA_DESCRIPCION).v_Value1,
                               InterfalangicaNID = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_INTERFALANGICAS).v_Value1,
                               InterfalangicaObs = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_INTERFALANGICAS_DESCRIPCION).v_Value1,
                               CoxoFermoralNID = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_COXOFEMORAL).v_Value1,
                               CoxoFermoralObs = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_COXOFEMORAL_DESCRIPCION).v_Value1,
                               RodillaNID = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_RODILLA).v_Value1,
                               RodillaObs = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_RODILLA_DESCRIPCION).v_Value1,
                               TobilloPieNID = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_TOBILLO_PIE).v_Value1,
                               TobilloPieObs = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_TOBILLO_PIE_DESCRIPCION).v_Value1,

                               ColumnaCervicalSiNo = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_COLUMNA_CERVICAL).v_Value1,
                               ColumnaCervicalObs = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_COLUMNA_CERVICAL_DESCRIPCION).v_Value1,
                               ColumnaDorsalSiNo = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_COLUMNA_DORSAL).v_Value1,
                               ColumnaDorsalObs = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_COLUMNA_DORSAL_DESCRIPCION).v_Value1,
                               DorsoLumbarSiNo = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_COLUMNA_DORSO_LUMBAR).v_Value1,
                               DorsoLumbarObs = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_COLUMNA_DORSO_LUMBAR_DESCRIPCION).v_Value1,
                               LumbroSacraSiNo = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_COLUMNA_LUMBOSACRA).v_Value1,
                               LumbroSacraObs = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_COLUMNA_LUMBOSACRA_DESCRIPCION).v_Value1,
                               CondralesSiNo = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_COSTO_CONDRALES).v_Value1,
                               CondralesObs = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_COSTO_CONDRALES_DESCRIPCION).v_Value1,
                               CostoEsternalesSiNo = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_COSTO_ESTERNALES).v_Value1,
                               CostoEsternalesObs = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_COSTO_ESTERNALES_DESCRIPCION).v_Value1,

                               Descripcion = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.DESCRIPCION).v_Value1,
                               Aptitud = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.APTITUD).v_Value1Name,
                              Hallazgos = GetDiagnosticByServiceIdAndComponent(a.IdServicio, Constants.OSTEO_MUSCULAR_ID_1),
                               Recomendacion = GetRecommendationByServiceIdAndComponent(a.IdServicio, Constants.OSTEO_MUSCULAR_ID_1),
                               HuellaTrabajador = a.HuellaTrabajador,

                               b_Logo = MedicalCenter.b_Image,
                               EmpresaPropietaria = MedicalCenter.v_Name,
                               EmpresaPropietariaDireccion = MedicalCenter.v_Address,
                               EmpresaPropietariaTelefono = MedicalCenter.v_PhoneNumber,
                               EmpresaPropietariaEmail = MedicalCenter.v_Mail,

                           }).ToList();

                return sql;
            }
            catch (Exception)
            {

                throw;
            }
        }
       
        public string GetDiagnosticByServiceIdAndComponent(string pstrServiceId, string pstrComponent)
        {
            SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
            var query = (from ccc in dbContext.diagnosticrepository
                         join ddd in dbContext.diseases on ccc.v_DiseasesId equals ddd.v_DiseasesId into ddd_join
                         from ddd in ddd_join.DefaultIfEmpty()

                         join eee in dbContext.recommendation on new { a = pstrServiceId, b = pstrComponent }
                                                                    equals new { a = eee.v_ServiceId, b = eee.v_ComponentId } into eee_join
                         from eee in eee_join.DefaultIfEmpty()

                         join fff in dbContext.masterrecommendationrestricction on eee.v_MasterRecommendationId
                                                                equals fff.v_MasterRecommendationRestricctionId into fff_join
                         from fff in fff_join.DefaultIfEmpty()

                         where ccc.v_ServiceId == pstrServiceId && ccc.v_ComponentId == pstrComponent &&
                               ccc.i_IsDeleted == 0
                         select new
                         {
                             v_DiseasesName = ddd.v_Name
                         }).ToList();


            return string.Join(", ", query.Select(p => p.v_DiseasesName));
        }
       
        private string GetRecommendationByServiceIdAndComponent(string pstrServiceId, string pstrComponent)
        {
            SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
            var query = (from ccc in dbContext.recommendation
                         join ddd in dbContext.masterrecommendationrestricction on ccc.v_MasterRecommendationId equals ddd.v_MasterRecommendationRestricctionId  // Diagnosticos      
                         where ccc.v_ServiceId == pstrServiceId && ccc.v_ComponentId == pstrComponent &&
                               ccc.i_IsDeleted == 0
                         select new
                         {
                             v_Recommendation = ddd.v_Name

                         }).ToList();


            return string.Join(", ", query.Select(p => p.v_Recommendation));
        }

        public List<Sigesoft.Node.WinClient.BE.ReportHistoriaOcupacionalList> ReportHistoriaOcupacionalAudiometria(string pstrserviceId)
        {
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                var isDeleted = 0;

                var ruido = (int)PeligrosEnElPuesto.Ruido;

                var objEntity = (from A in dbContext.service
                                 join B in dbContext.person on A.v_PersonId equals B.v_PersonId

                                 join D in dbContext.history on B.v_PersonId equals D.v_PersonId into D_join
                                 from D in D_join.DefaultIfEmpty()

                                 join wd in dbContext.workstationdangers on D.v_HistoryId equals wd.v_HistoryId

                                 //*****
                                 join ter in dbContext.systemparameter on new { a = wd.i_NoiseSource.Value, b = (int)SystemParameterGroups.TiempoExpsosicionRuido } // Tiempo exp. al ruido
                                               equals new { a = ter.i_ParameterId, b = ter.i_GroupId } into ter_join
                                 from ter in ter_join.DefaultIfEmpty()

                                 join nr in dbContext.systemparameter on new { a = wd.i_NoiseLevel.Value, b = (int)SystemParameterGroups.NivelRuido } // Nivel de ruido
                                              equals new { a = nr.i_ParameterId, b = nr.i_GroupId } into nr_join
                                 from nr in nr_join.DefaultIfEmpty()
                                 //************

                                 join pro in dbContext.protocol on A.v_ProtocolId equals pro.v_ProtocolId

                                 // Empresa / Sede Trabajo  ********************************************************
                                 join ow in dbContext.organization on new { a = pro.v_WorkingOrganizationId }
                                         equals new { a = ow.v_OrganizationId } into ow_join
                                 from ow in ow_join.DefaultIfEmpty()

                                 join lw in dbContext.location on new { a = pro.v_WorkingOrganizationId, b = pro.v_WorkingLocationId }
                                      equals new { a = lw.v_OrganizationId, b = lw.v_LocationId } into lw_join
                                 from lw in lw_join.DefaultIfEmpty()

                                 //************************************************************************************

                                 where (A.v_ServiceId == pstrserviceId) &&
                                       (D.i_IsDeleted == isDeleted) &&
                                       (wd.i_DangerId == ruido)

                                 select new Sigesoft.Node.WinClient.BE.ReportHistoriaOcupacionalList
                                 {
                                     IdHistory = D.v_HistoryId,
                                     //Trabajador = B.v_FirstLastName + " " + B.v_SecondLastName + " " + B.v_FirstName,
                                     IdServicio = A.v_ServiceId,
                                     FNacimiento = B.d_Birthdate,
                                     Genero = B.i_SexTypeId.Value,
                                     LugarNacimiento = B.v_BirthPlace,

                                     Puesto = B.v_CurrentOccupation,
                                     FechaInicio = D.d_StartDate,
                                     FechaFin = D.d_EndDate,
                                     Empresa = D.v_Organization,
                                     Altitud = D.i_GeografixcaHeight.Value,
                                     AreaTrabajo = D.v_TypeActivity,
                                     PuestoTrabajo = D.v_workstation,

                                     FuenteRuidoName = wd.v_TimeOfExposureToNoise,
                                     NivelRuidoName = nr.v_Value1,
                                     TiempoExpoRuidoName = ter.v_Value1,
                                     v_PersonId = B.v_PersonId,
                                     //
                                     v_FullPersonName = B.v_FirstLastName + " " + B.v_SecondLastName + " " + B.v_FirstName,
                                     v_WorkingOrganizationName = ow.v_Name,
                                     v_FullWorkingOrganizationName = ow.v_Name + " / " + lw.v_Name,
                                     NroDocumento = B.v_DocNumber,
                                     FirmaTrabajador = B.b_RubricImage,
                                     HuellaTrabajador = B.b_FingerPrintImage,

                                 });

                var MedicalCenter = GetInfoMedicalCenter();

                var sql = (from a in objEntity.ToList()
                           let date1 = a.FechaInicio == null ? "" : a.FechaInicio.Value.ToString("MMMM / yyyy")
                           let date2 = a.FechaFin == null ? "" : a.FechaFin.Value.ToString("MMMM / yyyy")

                           select new Sigesoft.Node.WinClient.BE.ReportHistoriaOcupacionalList
                           {
                               IdHistory = a.IdHistory,
                               //Trabajador = a.Trabajador,
                               IdServicio = a.IdServicio,
                               FechaNacimiento = a.FNacimiento == null ? "" : a.FNacimiento.Value.ToString("dd/MM/yyyy"),
                               Genero = a.Genero,
                               LugarNacimiento = a.LugarNacimiento,

                               Puesto = a.Puesto,
                               FechaInicio = a.FechaInicio,
                               FechaFin = a.FechaFin,
                               Fechas = "Fecha Ini. \n" + date1 + "\n" + "Fecha Fin. \n" + date2,
                               Empresa = a.Empresa,

                               AreaTrabajo = a.AreaTrabajo,
                               PuestoTrabajo = a.PuestoTrabajo,

                               //Peligros = ConcatenateExposiciones(a.IdHistory),
                               Epp = ConcatenateEppsAndPercentage(a.IdHistory),

                               FuenteRuidoName = a.FuenteRuidoName,
                               NivelRuidoName = a.NivelRuidoName,
                               TiempoExpoRuidoName = a.TiempoExpoRuidoName,
                               v_PersonId = a.v_PersonId,

                               b_Logo = MedicalCenter.b_Image,
                               EmpresaPropietaria = MedicalCenter.v_Name,
                               EmpresaPropietariaDireccion = MedicalCenter.v_Address,
                               EmpresaPropietariaTelefono = MedicalCenter.v_PhoneNumber,
                               EmpresaPropietariaEmail = MedicalCenter.v_Mail,

                               //
                               v_FullPersonName = a.v_FullPersonName,
                               v_WorkingOrganizationName = a.v_WorkingOrganizationName,
                               v_FullWorkingOrganizationName = a.v_FullWorkingOrganizationName,
                               NroDocumento = a.NroDocumento,
                               FirmaTrabajador = a.FirmaTrabajador,
                               HuellaTrabajador = a.HuellaTrabajador,

                           }).ToList();

                return sql;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private string ConcatenateEppsAndPercentage(string pstrHistoryId)
        {
            SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

            int[] tipoEPPRuido = { (int)TipoEPP.Orejeras, 
                                   (int)TipoEPP.TaponesAuditivosEspuma,
                                   (int)TipoEPP.TaponesAuditivosSilicona 
                                 };

            var qry = (from a in dbContext.typeofeep
                       join C1 in dbContext.systemparameter on new { a = a.i_TypeofEEPId.Value, b = 146 }
                                                                equals new { a = C1.i_ParameterId, b = C1.i_GroupId }
                       where a.v_HistoryId == pstrHistoryId &&
                             a.i_IsDeleted == 0 &&
                             tipoEPPRuido.Contains(a.i_TypeofEEPId.Value)
                       select new
                       {
                           v_Epps = C1.v_Value1,
                           r_Percentage = a.r_Percentage
                       }).ToList();

            return string.Join(", ", qry.Select(p => p.v_Epps + " " + p.r_Percentage + " % "));

            //return string.Join(", ", qry.Select(p => 
            //    new { v_Epps = p.v_Epps, 
            //          r_Percentage = p.r_Percentage 

            //        }));

        }

        public List<Sigesoft.Node.WinClient.BE.ReportHistoriaOcupacionalList> ReportHistoriaOcupacional(string pstrserviceId)
        {
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
                var groupUbigeo = 113;
                var isDeleted = 0;
                var exFisicoId = Constants.EXAMEN_FISICO_ID;
                var exFisico7C = Constants.EXAMEN_FISICO_7C_ID;

                var objEntity = (from A in dbContext.service
                                 join B in dbContext.person on A.v_PersonId equals B.v_PersonId

                                 join D in dbContext.history on B.v_PersonId equals D.v_PersonId into D_join
                                 from D in D_join.DefaultIfEmpty()

                                 join sc in dbContext.servicecomponent on new { a = pstrserviceId, b = exFisicoId }
                                                                      equals new { a = sc.v_ServiceId, b = sc.v_ComponentId } into sc_join
                                 from sc in sc_join.DefaultIfEmpty()

                                 join E in dbContext.systemuser on sc.i_ApprovedUpdateUserId equals E.i_SystemUserId into E_join
                                 from E in E_join.DefaultIfEmpty()

                                 join F in dbContext.professional on E.v_PersonId equals F.v_PersonId into F_join
                                 from F in F_join.DefaultIfEmpty()

                                 // Examen fisico 7C *******************************************************************
                                 join sc1 in dbContext.servicecomponent on new { a = pstrserviceId, b = exFisico7C }
                                                                    equals new { a = sc1.v_ServiceId, b = sc1.v_ComponentId } into sc1_join
                                 from sc1 in sc1_join.DefaultIfEmpty()

                                 join su7c in dbContext.systemuser on sc1.i_ApprovedUpdateUserId equals su7c.i_SystemUserId into su7c_join
                                 from su7c in su7c_join.DefaultIfEmpty()

                                 join p7c in dbContext.professional on su7c.v_PersonId equals p7c.v_PersonId into p7c_join
                                 from p7c in p7c_join.DefaultIfEmpty()

                                 //******************************************************************************

                                 // Ubigeo de la persona *******************************************************
                                 join dep in dbContext.datahierarchy on new { a = B.i_DepartmentId.Value, b = groupUbigeo }
                                                      equals new { a = dep.i_ItemId, b = dep.i_GroupId } into dep_join
                                 from dep in dep_join.DefaultIfEmpty()

                                 join prov in dbContext.datahierarchy on new { a = B.i_ProvinceId.Value, b = groupUbigeo }
                                                       equals new { a = prov.i_ItemId, b = prov.i_GroupId } into prov_join
                                 from prov in prov_join.DefaultIfEmpty()

                                 join distri in dbContext.datahierarchy on new { a = B.i_DistrictId.Value, b = groupUbigeo }
                                                       equals new { a = distri.i_ItemId, b = distri.i_GroupId } into distri_join
                                 from distri in distri_join.DefaultIfEmpty()
                                 //*********************************************************************************************

                                 let varDpto = dep.v_Value1 == null ? "" : dep.v_Value1
                                 let varProv = prov.v_Value1 == null ? "" : prov.v_Value1
                                 let varDistri = distri.v_Value1 == null ? "" : distri.v_Value1
                                 let del = D.i_IsDeleted == null ? 0 : D.i_IsDeleted

                                 where (A.v_ServiceId == pstrserviceId) &&
                                       (del == isDeleted)

                                 select new Sigesoft.Node.WinClient.BE.ReportHistoriaOcupacionalList
                                 {
                                     IdHistory = D.v_HistoryId,
                                     Trabajador = B.v_FirstLastName + " " + B.v_SecondLastName + " " + B.v_FirstName,
                                     IdServicio = A.v_ServiceId,
                                     FNacimiento = B.d_Birthdate,
                                     Genero = B.i_SexTypeId.Value,
                                     LugarNacimiento = B.v_BirthPlace,
                                     LugarProcedencia = varDistri + "-" + varProv + "-" + varDpto, // Santa Anita - Lima - Lima
                                     Puesto = B.v_CurrentOccupation,
                                     FechaInicio = D.d_StartDate,
                                     FechaFin = D.d_EndDate,
                                     Empresa = D.v_Organization,
                                     Altitud = D.i_GeografixcaHeight.Value,
                                     AreaTrabajo = D.v_TypeActivity,
                                     PuestoTrabajo = D.v_workstation,
                                     IdTipoOperacion = D.i_TypeOperationId.Value,
                                     Dia = A.d_ServiceDate.Value.Day,
                                     Mes = A.d_ServiceDate.Value.Month,
                                     Anio = A.d_ServiceDate.Value.Year,
                                     FirmaMedico = F.b_SignatureImage == null ? p7c.b_SignatureImage : F.b_SignatureImage,
                                     FirmaTrabajador = B.b_RubricImage,
                                     HuellaTrabajador = B.b_FingerPrintImage,

                                 });

                var MedicalCenter = GetInfoMedicalCenter();

                var sql = (from a in objEntity.ToList()
                           let date1 = a.FechaInicio == null ? "" : a.FechaInicio.Value.ToString("MMMM / yyyy")
                           let date2 = a.FechaFin == null ? "" : a.FechaFin.Value.ToString("MMMM / yyyy")
                           let xxx = GetYearsAndMonth(a.FechaFin, a.FechaInicio)
                           select new Sigesoft.Node.WinClient.BE.ReportHistoriaOcupacionalList
                           {
                               IdHistory = a.IdHistory,
                               Trabajador = a.Trabajador,
                               IdServicio = a.IdServicio,
                               FechaNacimiento = a.FNacimiento == null ? "" : a.FNacimiento.Value.ToString("dd/MM/yyyy"),
                               Genero = a.Genero,
                               LugarNacimiento = a.LugarNacimiento,
                               LugarProcedencia = a.LugarProcedencia,
                               Puesto = a.Puesto,
                               FechaInicio = a.FechaInicio,
                               FechaFin = a.FechaFin,
                               Fechas = "Fecha Ini. \n" + date1 + "\n" + "Fecha Fin. \n" + date2,
                               Empresa = a.Empresa,
                               Altitud = a.Altitud,
                               AreaTrabajo = a.AreaTrabajo,
                               PuestoTrabajo = a.PuestoTrabajo,
                               IdTipoOperacion = a.IdTipoOperacion,
                               TiempoLabor = xxx,
                               Dia = a.Dia,
                               Mes = a.Mes,
                               Anio = a.Anio,
                               FirmaMedico = a.FirmaMedico,
                               FirmaTrabajador = a.FirmaTrabajador,
                               HuellaTrabajador = a.HuellaTrabajador,
                               Peligros = ConcatenateExposiciones(a.IdHistory),
                               Epp = ConcatenateEpps(a.IdHistory),

                               b_Logo = MedicalCenter.b_Image,
                               EmpresaPropietaria = MedicalCenter.v_Name,
                               EmpresaPropietariaDireccion = MedicalCenter.v_Address,
                               EmpresaPropietariaTelefono = MedicalCenter.v_PhoneNumber,
                               EmpresaPropietariaEmail = MedicalCenter.v_Mail,

                           }).ToList();

                return sql;
            }
            catch (Exception)
            {

                throw;
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

        public string GetValueOdontograma1(string pstrServiceId, string pstrComponentId, string pstrFieldId)
        {
            try
            {
                ServiceBL oServiceBL = new ServiceBL();
                List<Sigesoft.Node.WinClient.BE.ServiceComponentFieldValuesList> oServiceComponentFieldValuesList = new List<Sigesoft.Node.WinClient.BE.ServiceComponentFieldValuesList>();

                oServiceComponentFieldValuesList = oServiceBL.ValoresComponenteOdontograma1(pstrServiceId, pstrComponentId);
                var xx = oServiceComponentFieldValuesList.Count() == 0 || ((Sigesoft.Node.WinClient.BE.ServiceComponentFieldValuesList)oServiceComponentFieldValuesList.Find(p => p.v_ComponentFieldId == pstrFieldId)) == null ? string.Empty : ((Sigesoft.Node.WinClient.BE.ServiceComponentFieldValuesList)oServiceComponentFieldValuesList.Find(p => p.v_ComponentFieldId == pstrFieldId)).v_Value1;

                return xx;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public List<Sigesoft.Node.WinClient.BE.ReportEstudioElectrocardiografico> GetReportEstudioElectrocardiografico(string pstrserviceId, string pstrComponentId)
        {
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                var objEntity = (from A in dbContext.service
                                 join B in dbContext.person on A.v_PersonId equals B.v_PersonId
                                 join C in dbContext.protocol on A.v_ProtocolId equals C.v_ProtocolId
                                 join D in dbContext.organization on C.v_WorkingOrganizationId equals D.v_OrganizationId
                                 join E in dbContext.servicecomponent on new { a = A.v_ServiceId, b = pstrComponentId }
                                                                        equals new { a = E.v_ServiceId, b = E.v_ComponentId }

                                 join J1 in dbContext.datahierarchy on new { a = B.i_LevelOfId.Value, b = 108 }
                                                                    equals new { a = J1.i_ItemId, b = J1.i_GroupId } into J1_join
                                 from J1 in J1_join.DefaultIfEmpty()
                                 join F in dbContext.systemuser on E.i_InsertUserId equals F.i_SystemUserId
                                 join G in dbContext.professional on F.v_PersonId equals G.v_PersonId

                                 join M in dbContext.systemparameter on new { a = B.i_SexTypeId.Value, b = 100 }
                                     equals new { a = M.i_ParameterId, b = M.i_GroupId } into M_join
                                 from M in M_join.DefaultIfEmpty()



                                 // Usuario Medico Evaluador / Medico Aprobador ****************************
                                 join me in dbContext.systemuser on E.i_ApprovedUpdateUserId equals me.i_SystemUserId into me_join
                                 from me in me_join.DefaultIfEmpty()

                                 join pme in dbContext.professional on me.v_PersonId equals pme.v_PersonId into pme_join
                                 from pme in pme_join.DefaultIfEmpty()

                                 // Usuario Tecnologo *************************************
                                 join tec in dbContext.systemuser on E.i_UpdateUserTechnicalDataRegisterId equals tec.i_SystemUserId into tec_join
                                 from tec in tec_join.DefaultIfEmpty()

                                 join ptec in dbContext.professional on tec.v_PersonId equals ptec.v_PersonId into ptec_join
                                 from ptec in ptec_join.DefaultIfEmpty()
                                 // *******************************************************       

                                 join X in dbContext.person on me.v_PersonId equals X.v_PersonId
                                 join Y in dbContext.person on tec.v_PersonId equals Y.v_PersonId into Y_join
                                 from Y in Y_join.DefaultIfEmpty()

                                 join F1 in dbContext.servicecomponentmultimedia on E.v_ServiceComponentId equals F1.v_ServiceComponentId into F1_join
                                 from F1 in F1_join.DefaultIfEmpty()

                                 join G1 in dbContext.multimediafile on F1.v_MultimediaFileId equals G1.v_MultimediaFileId into G1_join
                                 from G1 in G1_join.DefaultIfEmpty()


                                 where A.v_ServiceId == pstrserviceId
                                 select new Sigesoft.Node.WinClient.BE.ReportEstudioElectrocardiografico
                                 {
                                     NroFicha = E.v_ServiceComponentId,
                                     NroHistoria = A.v_ServiceId,
                                     DatosPaciente = B.v_FirstLastName + " " + B.v_SecondLastName + " " + B.v_FirstName,
                                     FechaNacimiento = B.d_Birthdate.Value,
                                     Genero = M.v_Value1,
                                     FirmaMedico = pme.b_SignatureImage,
                                     FirmaTecnico = ptec.b_SignatureImage,
                                     Fecha = A.d_ServiceDate.Value,
                                     Empresa = D.v_Name,
                                     Puesto = B.v_CurrentOccupation,
                                     NombreDoctor = X.v_FirstLastName + " " + X.v_SecondLastName + " " + X.v_FirstName,
                                     NombreTecnologo = Y.v_FirstLastName + " " + Y.v_SecondLastName + " " + Y.v_FirstName,
                                     NombreUsuarioGraba = X.v_FirstLastName + " " + X.v_SecondLastName + " " + X.v_FirstName,
                                     b_Imagen = G1.b_File
                                 });

                var MedicalCenter = GetInfoMedicalCenter();

                var sql = (from a in objEntity.ToList()
                           let Valores = ValoresComponente(pstrserviceId, Constants.ELECTROCARDIOGRAMA_ID)
                           select new Sigesoft.Node.WinClient.BE.ReportEstudioElectrocardiografico
                           {
                               b_Imagen = a.b_Imagen,
                               NroFicha = a.NroFicha,
                               NroHistoria = a.NroHistoria,
                               DatosPaciente = a.DatosPaciente,
                               FechaNacimiento = a.FechaNacimiento,
                               Genero = a.Genero,
                               FirmaMedico = a.FirmaMedico,
                               FirmaTecnico = a.FirmaTecnico,
                               Fecha = a.Fecha,
                               Empresa = a.Empresa,
                               Puesto = a.Puesto,
                               Edad = GetAge(a.FechaNacimiento),
                               NombreDoctor = a.NombreDoctor,
                               NombreTecnologo = a.NombreTecnologo,

                               SoploSiNo = Valores.Count == 0 || Valores.Find(p => p.v_ComponentFieldId == Constants.ELECTROCARDIOGRAMA_SOPLO_CARDIACO_ID) == null ? string.Empty : Valores.Find(p => p.v_ComponentFieldId == Constants.ELECTROCARDIOGRAMA_SOPLO_CARDIACO_ID).v_Value1,
                               CansancioSiNo = Valores.Count == 0 || Valores.Find(p => p.v_ComponentFieldId == Constants.ELECTROCARDIOGRAMA_CANSANCIO_RAPIDO_ID) == null ? string.Empty : Valores.Find(p => p.v_ComponentFieldId == Constants.ELECTROCARDIOGRAMA_CANSANCIO_RAPIDO_ID).v_Value1,
                               MareosSiNo = Valores.Count == 0 || Valores.Find(p => p.v_ComponentFieldId == Constants.ELECTROCARDIOGRAMA_MAREOS_ID) == null ? string.Empty : Valores.Find(p => p.v_ComponentFieldId == Constants.ELECTROCARDIOGRAMA_MAREOS_ID).v_Value1,
                               PresionAltaSiNo = Valores.Count == 0 || Valores.Find(p => p.v_ComponentFieldId == Constants.ELECTROCARDIOGRAMA_PRESION_ALTA_ID) == null ? string.Empty : Valores.Find(p => p.v_ComponentFieldId == Constants.ELECTROCARDIOGRAMA_PRESION_ALTA_ID).v_Value1,
                               DolorPrecordialSiNo = Valores.Count == 0 || Valores.Find(p => p.v_ComponentFieldId == Constants.ELECTROCARDIOGRAMA_DOLOR_PRECORDIAL_ID) == null ? string.Empty : Valores.Find(p => p.v_ComponentFieldId == Constants.ELECTROCARDIOGRAMA_DOLOR_PRECORDIAL_ID).v_Value1,
                               PalpitacionesSiNo = Valores.Count == 0 || Valores.Find(p => p.v_ComponentFieldId == Constants.ELECTROCARDIOGRAMA_PALPITACIONES_ID) == null ? string.Empty : Valores.Find(p => p.v_ComponentFieldId == Constants.ELECTROCARDIOGRAMA_PALPITACIONES_ID).v_Value1,
                               AtaquesCorazonSiNo = Valores.Count == 0 || Valores.Find(p => p.v_ComponentFieldId == Constants.ELECTROCARDIOGRAMA_ATAQUE_CORAZON_ID) == null ? string.Empty : Valores.Find(p => p.v_ComponentFieldId == Constants.ELECTROCARDIOGRAMA_ATAQUE_CORAZON_ID).v_Value1,
                               PerdidaConcienciaSiNo = Valores.Count == 0 || Valores.Find(p => p.v_ComponentFieldId == Constants.ELECTROCARDIOGRAMA_PERDIDA_CONCIENCIA_ID) == null ? string.Empty : Valores.Find(p => p.v_ComponentFieldId == Constants.ELECTROCARDIOGRAMA_PERDIDA_CONCIENCIA_ID).v_Value1,
                               ObesidadSiNo = Valores.Count == 0 || Valores.Find(p => p.v_ComponentFieldId == Constants.ELECTROCARDIOGRAMA_OBESIDAD_ID) == null ? string.Empty : Valores.Find(p => p.v_ComponentFieldId == Constants.ELECTROCARDIOGRAMA_OBESIDAD_ID).v_Value1,
                               TabaquismoSiNo = Valores.Count == 0 || Valores.Find(p => p.v_ComponentFieldId == Constants.ELECTROCARDIOGRAMA_TABAQUISMO_ID) == null ? string.Empty : Valores.Find(p => p.v_ComponentFieldId == Constants.ELECTROCARDIOGRAMA_TABAQUISMO_ID).v_Value1,
                               DisplidemiaSiNo = Valores.Count == 0 || Valores.Find(p => p.v_ComponentFieldId == Constants.ELECTROCARDIOGRAMA_DISLIPIDEMIA_ID) == null ? string.Empty : Valores.Find(p => p.v_ComponentFieldId == Constants.ELECTROCARDIOGRAMA_DISLIPIDEMIA_ID).v_Value1,
                               DiabetesSiNo = Valores.Count == 0 || Valores.Find(p => p.v_ComponentFieldId == Constants.ELECTROCARDIOGRAMA_DIABETES_ID) == null ? string.Empty : Valores.Find(p => p.v_ComponentFieldId == Constants.ELECTROCARDIOGRAMA_DIABETES_ID).v_Value1,
                               SedentarismoSiNo = Valores.Count == 0 || Valores.Find(p => p.v_ComponentFieldId == Constants.ELECTROCARDIOGRAMA_SEDENTARISMO_ID) == null ? string.Empty : Valores.Find(p => p.v_ComponentFieldId == Constants.ELECTROCARDIOGRAMA_SEDENTARISMO_ID).v_Value1,
                               Otros1 = Valores.Count == 0 || Valores.Find(p => p.v_ComponentFieldId == Constants.ELECTROCARDIOGRAMA_OTROS1_ESPECIFICAR_ID) == null ? string.Empty : Valores.Find(p => p.v_ComponentFieldId == Constants.ELECTROCARDIOGRAMA_OTROS1_ESPECIFICAR_ID).v_Value1,
                               DolorPrecordial2SiNo = Valores.Count == 0 || Valores.Find(p => p.v_ComponentFieldId == Constants.ELECTROCARDIOGRAMA_DOLOR_PRECORDIAL1_ID) == null ? string.Empty : Valores.Find(p => p.v_ComponentFieldId == Constants.ELECTROCARDIOGRAMA_DOLOR_PRECORDIAL1_ID).v_Value1,
                               DesmayosSiNo = Valores.Count == 0 || Valores.Find(p => p.v_ComponentFieldId == Constants.ELECTROCARDIOGRAMA_DESMAYOS_ID) == null ? string.Empty : Valores.Find(p => p.v_ComponentFieldId == Constants.ELECTROCARDIOGRAMA_DESMAYOS_ID).v_Value1,
                               Palpitaciones2SiNo = Valores.Count == 0 || Valores.Find(p => p.v_ComponentFieldId == Constants.ELECTROCARDIOGRAMA_PALPITACIONES2_ID) == null ? string.Empty : Valores.Find(p => p.v_ComponentFieldId == Constants.ELECTROCARDIOGRAMA_PALPITACIONES2_ID).v_Value1,
                               DisneaSiNo = Valores.Count == 0 || Valores.Find(p => p.v_ComponentFieldId == Constants.ELECTROCARDIOGRAMA_DISNEA_PAROXISTICA_ID) == null ? string.Empty : Valores.Find(p => p.v_ComponentFieldId == Constants.ELECTROCARDIOGRAMA_DISNEA_PAROXISTICA_ID).v_Value1,
                               Otros2 = Valores.Count == 0 || Valores.Find(p => p.v_ComponentFieldId == Constants.ELECTROCARDIOGRAMA_OTROS2_ID) == null ? string.Empty : Valores.Find(p => p.v_ComponentFieldId == Constants.ELECTROCARDIOGRAMA_OTROS2_ID).v_Value1,
                               Mareos2SiNo = Valores.Count == 0 || Valores.Find(p => p.v_ComponentFieldId == Constants.ELECTROCARDIOGRAMA_MAREOS1_ID) == null ? string.Empty : Valores.Find(p => p.v_ComponentFieldId == Constants.ELECTROCARDIOGRAMA_MAREOS1_ID).v_Value1,
                               VaricesSiNo = Valores.Count == 0 || Valores.Find(p => p.v_ComponentFieldId == Constants.ELECTROCARDIOGRAMA_VARICES_PIERNAS_ID) == null ? string.Empty : Valores.Find(p => p.v_ComponentFieldId == Constants.ELECTROCARDIOGRAMA_VARICES_PIERNAS_ID).v_Value1,
                               ClaudicacSiNo = Valores.Count == 0 || Valores.Find(p => p.v_ComponentFieldId == Constants.ELECTROCARDIOGRAMA_CLAUDICAC_INTERMITENTE_ID) == null ? string.Empty : Valores.Find(p => p.v_ComponentFieldId == Constants.ELECTROCARDIOGRAMA_CLAUDICAC_INTERMITENTE_ID).v_Value1,
                               Ritmo = Valores.Count == 0 || Valores.Find(p => p.v_ComponentFieldId == Constants.ELECTROCARDIOGRAMA_RITMO_SINUAL_ID) == null ? string.Empty : Valores.Find(p => p.v_ComponentFieldId == Constants.ELECTROCARDIOGRAMA_RITMO_SINUAL_ID).v_Value1,
                               IntervaloPR = Valores.Count == 0 || Valores.Find(p => p.v_ComponentFieldId == Constants.ELECTROCARDIOGRAMA_INTERVALO_PR_ID) == null ? string.Empty : Valores.Find(p => p.v_ComponentFieldId == Constants.ELECTROCARDIOGRAMA_INTERVALO_PR_ID).v_Value1,
                               IntervaloQRS = Valores.Count == 0 || Valores.Find(p => p.v_ComponentFieldId == Constants.ELECTROCARDIOGRAMA_INTERVALO_QRS_ID) == null ? string.Empty : Valores.Find(p => p.v_ComponentFieldId == Constants.ELECTROCARDIOGRAMA_INTERVALO_QRS_ID).v_Value1,
                               IntervaloQT = Valores.Count == 0 || Valores.Find(p => p.v_ComponentFieldId == Constants.ELECTROCARDIOGRAMA_INTERVALO_QT_ID) == null ? string.Empty : Valores.Find(p => p.v_ComponentFieldId == Constants.ELECTROCARDIOGRAMA_INTERVALO_QT_ID).v_Value1,
                               OndaPAnormal = Valores.Count == 0 || Valores.Find(p => p.v_ComponentFieldId == Constants.ELECTROCARDIOGRAMA_ONDA_P_ANORMAL_ID) == null ? string.Empty : Valores.Find(p => p.v_ComponentFieldId == Constants.ELECTROCARDIOGRAMA_ONDA_P_ANORMAL_ID).v_Value1,
                               OndaTAnormal = Valores.Count == 0 || Valores.Find(p => p.v_ComponentFieldId == Constants.ELECTROCARDIOGRAMA_ONDA_T_ANORMAL_ID) == null ? string.Empty : Valores.Find(p => p.v_ComponentFieldId == Constants.ELECTROCARDIOGRAMA_ONDA_T_ANORMAL_ID).v_Value1,
                               ComplejoQRSAnormal = Valores.Count == 0 || Valores.Find(p => p.v_ComponentFieldId == Constants.ELECTROCARDIOGRAMA_COMPLEJO_QRS_ANORMAL_ID) == null ? string.Empty : Valores.Find(p => p.v_ComponentFieldId == Constants.ELECTROCARDIOGRAMA_COMPLEJO_QRS_ANORMAL_ID).v_Value1,
                               SegementoSTAnormal = Valores.Count == 0 || Valores.Find(p => p.v_ComponentFieldId == Constants.ELECTROCARDIOGRAMA_SEGMENTO_ST_ANORMAL_ID) == null ? string.Empty : Valores.Find(p => p.v_ComponentFieldId == Constants.ELECTROCARDIOGRAMA_SEGMENTO_ST_ANORMAL_ID).v_Value1,
                               TrasntornoRitmo = Valores.Count == 0 || Valores.Find(p => p.v_ComponentFieldId == Constants.ELECTROCARDIOGRAMA_TRANSTORNOS_RITMO_ID) == null ? string.Empty : Valores.Find(p => p.v_ComponentFieldId == Constants.ELECTROCARDIOGRAMA_TRANSTORNOS_RITMO_ID).v_Value1,
                               TranstornoConduccion = Valores.Count == 0 || Valores.Find(p => p.v_ComponentFieldId == Constants.ELECTROCARDIOGRAMA_TRANSTORNOS_CONDUCCION_ID) == null ? string.Empty : Valores.Find(p => p.v_ComponentFieldId == Constants.ELECTROCARDIOGRAMA_TRANSTORNOS_CONDUCCION_ID).v_Value1,
                               Conclusiones = Valores.Count == 0 || Valores.Find(p => p.v_ComponentFieldId == Constants.ELECTROCARDIOGRAMA_DESCRIPCION_ID) == null ? string.Empty : Valores.Find(p => p.v_ComponentFieldId == Constants.ELECTROCARDIOGRAMA_DESCRIPCION_ID).v_Value1,
                               Hallazgos = GetDiagnosticByServiceIdAndComponent(a.NroHistoria, Constants.ELECTROCARDIOGRAMA_ID),
                               Recomendaciones = GetRecommendationByServiceIdAndComponent(a.NroHistoria, Constants.ELECTROCARDIOGRAMA_ID),
                               OndaP = Valores.Count == 0 || Valores.Find(p => p.v_ComponentFieldId == Constants.ELECTROCARDIOGRAMA_ONDA_P_ID) == null ? string.Empty : Valores.Find(p => p.v_ComponentFieldId == Constants.ELECTROCARDIOGRAMA_ONDA_P_ID).v_Value1Name,
                               SegmentoST = Valores.Count == 0 || Valores.Find(p => p.v_ComponentFieldId == Constants.ELECTROCARDIOGRAMA_SEGMENTO_ST_ID) == null ? string.Empty : Valores.Find(p => p.v_ComponentFieldId == Constants.ELECTROCARDIOGRAMA_SEGMENTO_ST_ID).v_Value1Name,
                               ComplejoQRS = Valores.Count == 0 || Valores.Find(p => p.v_ComponentFieldId == Constants.ELECTROCARDIOGRAMA_COMPLEJO_QRS_ID) == null ? string.Empty : Valores.Find(p => p.v_ComponentFieldId == Constants.ELECTROCARDIOGRAMA_COMPLEJO_QRS_ID).v_Value1Name,
                               OndaT = Valores.Count == 0 || Valores.Find(p => p.v_ComponentFieldId == Constants.ELECTROCARDIOGRAMA_ONDA_T_ID) == null ? string.Empty : Valores.Find(p => p.v_ComponentFieldId == Constants.ELECTROCARDIOGRAMA_ONDA_T_ID).v_Value1Name,

                               b_Logo = MedicalCenter.b_Image,
                               EmpresaPropietaria = MedicalCenter.v_Name,
                               EmpresaPropietariaDireccion = MedicalCenter.v_Address,
                               EmpresaPropietariaTelefono = MedicalCenter.v_PhoneNumber,
                               EmpresaPropietariaEmail = MedicalCenter.v_Mail,
                               NombreUsuarioGraba = a.NombreUsuarioGraba
                               //Hallazgos = GetDiagnosticByServiceId(a.IdServicio),
                               //Descripcion = GetServiceComponentFielValue(a.IdServicio, pstrComponentId, Constants.OSTEO_MUSCULAR_DESCRIPCION_ID, "NOCOMBO", 0, "SI"),
                               //Recomendacion = GetRecommendationByServiceId(a.IdServicio),



                           }).ToList();

                return sql;
            }
            catch (Exception)
            {

                throw;
            }
        }

        // Alberto
        public List<Sigesoft.Node.WinClient.BE.ReportEsfuerzoFisico> GetReportPruebaEsfuerzo(string pstrserviceId, string pstrComponentId)
        {
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                var objEntity = (from A in dbContext.service
                                 join B in dbContext.person on A.v_PersonId equals B.v_PersonId
                                 join C in dbContext.protocol on A.v_ProtocolId equals C.v_ProtocolId
                                 join D in dbContext.organization on C.v_WorkingOrganizationId equals D.v_OrganizationId
                                 join E in dbContext.servicecomponent on new { a = A.v_ServiceId, b = pstrComponentId }
                                                                        equals new { a = E.v_ServiceId, b = E.v_ComponentId }

                                 join J1 in dbContext.datahierarchy on new { a = B.i_LevelOfId.Value, b = 108 }
                                                                    equals new { a = J1.i_ItemId, b = J1.i_GroupId } into J1_join
                                 from J1 in J1_join.DefaultIfEmpty()
                                 join F in dbContext.systemuser on E.i_InsertUserId equals F.i_SystemUserId
                                 join G in dbContext.professional on F.v_PersonId equals G.v_PersonId

                                 join M in dbContext.systemparameter on new { a = B.i_SexTypeId.Value, b = 100 }
                                     equals new { a = M.i_ParameterId, b = M.i_GroupId } into M_join
                                 from M in M_join.DefaultIfEmpty()

                                 // Usuario Medico Evaluador / Medico Aprobador ****************************
                                 join me in dbContext.systemuser on E.i_ApprovedUpdateUserId equals me.i_SystemUserId into me_join
                                 from me in me_join.DefaultIfEmpty()

                                 join pme in dbContext.professional on me.v_PersonId equals pme.v_PersonId into pme_join
                                 from pme in pme_join.DefaultIfEmpty()

                                 // Usuario Tecnologo *************************************
                                 join tec in dbContext.systemuser on E.i_UpdateUserTechnicalDataRegisterId equals tec.i_SystemUserId into tec_join
                                 from tec in tec_join.DefaultIfEmpty()

                                 join ptec in dbContext.professional on tec.v_PersonId equals ptec.v_PersonId into ptec_join
                                 from ptec in ptec_join.DefaultIfEmpty()
                                 // *******************************************************       


                                 join X in dbContext.person on me.v_PersonId equals X.v_PersonId
                                 join Y in dbContext.person on tec.v_PersonId equals Y.v_PersonId into Y_join
                                 from Y in Y_join.DefaultIfEmpty()

                                 where A.v_ServiceId == pstrserviceId
                                 select new Sigesoft.Node.WinClient.BE.ReportEsfuerzoFisico
                                 {
                                     Ficha = E.v_ComponentId,
                                     HistoriaClinica = A.v_ServiceId,
                                     DatoPaciente = B.v_FirstLastName + " " + B.v_SecondLastName + " " + B.v_FirstName,
                                     FechaNacimiento = B.d_Birthdate.Value,
                                     Genero = M.v_Value1,
                                     FirmaTecnologo = ptec.b_SignatureImage,
                                     FirmaMedico = pme.b_SignatureImage,

                                     NombreDoctor = X.v_FirstLastName + " " + X.v_SecondLastName + " " + X.v_FirstName,
                                     NombreTecnico = Y.v_FirstLastName + " " + Y.v_SecondLastName + " " + Y.v_FirstName,

                                 });

                var MedicalCenter = GetInfoMedicalCenter();

                var sql = (from a in objEntity.ToList()
                           select new Sigesoft.Node.WinClient.BE.ReportEsfuerzoFisico
                           {
                               Ficha = a.Ficha,
                               HistoriaClinica = a.HistoriaClinica,
                               DatoPaciente = a.DatoPaciente,
                               FechaNacimiento = a.FechaNacimiento,
                               Edad = GetAge(a.FechaNacimiento.Value),
                               Genero = a.Genero,
                               FirmaTecnologo = a.FirmaTecnologo,
                               FirmaMedico = a.FirmaMedico,

                               NombreDoctor = a.NombreDoctor,
                               NombreTecnico = a.NombreTecnico,


                               FumadorSiNo = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_FUMADOR_ID, "NOCOMBO", 0, "SI"),
                               DiabeticoSiNo = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_DIABETICO_ID, "NOCOMBO", 0, "SI"),
                               InfartoSiNo = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_PRIOR_ID, "NOCOMBO", 0, "SI"),
                               FamiliarSiNo = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_ANGINA_ID, "NOCOMBO", 0, "SI"),

                               PriorSiNo = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_INFARTO_ID, "NOCOMBO", 0, "SI"),
                               AnginaSiNo = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_ANTECEDENTE_FAMILIAR_ID, "NOCOMBO", 0, "SI"),

                               PReposoInicio = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_P_REPOSO_INICIO_ID, "NOCOMBO", 0, "SI"),
                               PReposoDuracion = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_P_REPOSO_DURACION_ID, "NOCOMBO", 0, "SI"),
                               PReposoVelocidad = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_P_REPOSO_VELOCIDAD_ID, "NOCOMBO", 0, "SI"),
                               PReposoInclinacion = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_P_REPOSO_INCLINACION_ID, "NOCOMBO", 0, "SI"),
                               PReposoMTS = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_P_REPOSO_MTS_ID, "NOCOMBO", 0, "SI"),
                               PReposoFC = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_P_REPOSO_FC_ID, "NOCOMBO", 0, "SI"),
                               PReposoPAS = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_P_REPOSO_PAS_ID, "NOCOMBO", 0, "SI"),
                               PReposoProducto = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_P_REPOSO_PRODUCTO_ID, "NOCOMBO", 0, "SI"),
                               PReposoComentario = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_P_REPOSO_COMENTARIO_ID, "NOCOMBO", 0, "SI"),


                               PEsfuerzoInicio = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_P_ESFUERZO_INICIO_ID, "NOCOMBO", 0, "SI"),

                               PEsfuerzoDuracion = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_P_ESFUERZO_DURACION_ID, "NOCOMBO", 0, "SI"),

                               PEsfuerzoVelocidad = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_P_ESFUERZO_VELOCIDAD_ID, "NOCOMBO", 0, "SI"),

                               PEsfuerzoInclinacion = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_P_ESFUERZO_INCLINACION_ID, "NOCOMBO", 0, "SI"),

                               PEsfuerzoMTS = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_P_ESFUERZO_MTS_ID, "NOCOMBO", 0, "SI"),

                               PEsfuerzoFC = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_P_ESFUERZO_FC_ID, "NOCOMBO", 0, "SI"),

                               PEsfuerzoPAS = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_P_ESFUERZO_PAS_ID, "NOCOMBO", 0, "SI"),

                               PEsfuerzoProducto = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_P_ESFUERZO_PRODUCTO_ID, "NOCOMBO", 0, "SI"),

                               PEsfuerzoComentario = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_P_ESFUERZO_COMENTARIO_ID, "NOCOMBO", 0, "SI"),


                               SEsfuerzoInicio = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_S_ESFUERZO_INICIO_ID, "NOCOMBO", 0, "SI"),

                               SEsfuerzoDuracion = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_S_ESFUERZO_DURACION_ID, "NOCOMBO", 0, "SI"),

                               SEsfuerzoVelocidad = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_S_ESFUERZO_VELOCIDAD_ID, "NOCOMBO", 0, "SI"),

                               SEsfuerzoInclinacion = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_S_ESFUERZO_INCLINACION_ID, "NOCOMBO", 0, "SI"),

                               SEsfuerzoMTS = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_S_ESFUERZO_MTS_ID, "NOCOMBO", 0, "SI"),

                               SEsfuerzoFC = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_S_ESFUERZO_FC_ID, "NOCOMBO", 0, "SI"),

                               SEsfuerzoPAS = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_S_ESFUERZO_PAS_ID, "NOCOMBO", 0, "SI"),

                               SEsfuerzoProducto = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_S_ESFUERZO_PRODUCTO_ID, "NOCOMBO", 0, "SI"),

                               SEsfuerzoComentario = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_S_ESFUERZO_COMENTARIO_ID, "NOCOMBO", 0, "SI"),


                               TEsfuerzoInicio = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_T_ESFUERZO_INICIO_ID, "NOCOMBO", 0, "SI"),

                               TEsfuerzoDuracion = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_T_ESFUERZO_DURACION_ID, "NOCOMBO", 0, "SI"),

                               TEsfuerzoVelocidad = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_T_ESFUERZO_VELOCIDAD_ID, "NOCOMBO", 0, "SI"),

                               TEsfuerzoInclinacion = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_T_ESFUERZO_INCLINACION_ID, "NOCOMBO", 0, "SI"),

                               TEsfuerzoMTS = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_T_ESFUERZO_MTS_ID, "NOCOMBO", 0, "SI"),

                               TEsfuerzoFC = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_T_ESFUERZO_FC_ID, "NOCOMBO", 0, "SI"),

                               TEsfuerzoPAS = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_T_ESFUERZO_PAS_ID, "NOCOMBO", 0, "SI"),

                               TEsfuerzoProducto = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_T_ESFUERZO_PRODUCTO_ID, "NOCOMBO", 0, "SI"),

                               TEsfuerzoComentario = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_T_ESFUERZO_COMENTARIO_ID, "NOCOMBO", 0, "SI"),


                               CEsfuerzoInicio = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_C_ESFUERZO_INICIO_ID, "NOCOMBO", 0, "SI"),

                               CEsfuerzoDuracion = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_C_ESFUERZO_DURACION_ID, "NOCOMBO", 0, "SI"),

                               CEsfuerzoVelocidad = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_C_ESFUERZO_VELOCIDAD_ID, "NOCOMBO", 0, "SI"),

                               CEsfuerzoInclinacion = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_C_ESFUERZO_INCLINACION_ID, "NOCOMBO", 0, "SI"),

                               CEsfuerzoMTS = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_C_ESFUERZO_MTS_ID, "NOCOMBO", 0, "SI"),

                               CEsfuerzoFC = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_C_ESFUERZO_FC_ID, "NOCOMBO", 0, "SI"),

                               CEsfuerzoPAS = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_C_ESFUERZO_PAS_ID, "NOCOMBO", 0, "SI"),

                               CEsfuerzoProducto = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_C_ESFUERZO_PRODUCTO_ID, "NOCOMBO", 0, "SI"),

                               CEsfuerzoComentario = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_C_ESFUERZO_COMENTARIO_ID, "NOCOMBO", 0, "SI"),


                               PRecuperacionInicio = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_P_RECUPERACION_INICIO_ID, "NOCOMBO", 0, "SI"),

                               PRecuperacionDuracion = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_P_RECUPERACION_DURACION_ID, "NOCOMBO", 0, "SI"),

                               PRecuperacionVelocidad = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_P_RECUPERACION_VELOCIDAD_ID, "NOCOMBO", 0, "SI"),

                               PRecuperacionInclinacion = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_P_RECUPERACION_INCLINACION_ID, "NOCOMBO", 0, "SI"),

                               PRecuperacionMTS = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_P_RECUPERACION_MTS_ID, "NOCOMBO", 0, "SI"),

                               PRecuperacionFC = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_P_RECUPERACION_FC_ID, "NOCOMBO", 0, "SI"),

                               PRecuperacionPAS = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_P_RECUPERACION_PAS_ID, "NOCOMBO", 0, "SI"),

                               PRecuperacionProtocolo = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_P_RECUPERACION_PRODUCTO_ID, "NOCOMBO", 0, "SI"),

                               PRecuperacionComentario = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_P_RECUPERACION_COMENTARIO_ID, "NOCOMBO", 0, "SI"),


                               SRecuperacionInicio = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_S_RECUPERACION_INICIO_ID, "NOCOMBO", 0, "SI"),

                               SRecuperacionDuracion = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_S_RECUPERACION_DURACION_ID, "NOCOMBO", 0, "SI"),

                               SRecuperacionVelocidad = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_S_RECUPERACION_VELOCIDAD_ID, "NOCOMBO", 0, "SI"),

                               SRecuperacionInclinacion = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_S_RECUPERACION_INCLINACION_ID, "NOCOMBO", 0, "SI"),

                               SRecuperacionMTS = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_S_RECUPERACION_MTS_ID, "NOCOMBO", 0, "SI"),

                               SRecuperacionFC = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_S_RECUPERACION_FC_ID, "NOCOMBO", 0, "SI"),

                               SRecuperacionPAS = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_S_RECUPERACION_PAS_ID, "NOCOMBO", 0, "SI"),

                               SRecuperacionProducto = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_S_RECUPERACION_PRODUCTO_ID, "NOCOMBO", 0, "SI"),

                               SRecuperacionComentario = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_S_RECUPERACION_COMENTARIO_ID, "NOCOMBO", 0, "SI"),


                               SReposoInicio = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_S_REPOSO_INICIO_ID, "NOCOMBO", 0, "SI"),

                               SReposoDuracion = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_S_REPOSO_DURACION_ID, "NOCOMBO", 0, "SI"),

                               SReposoVelocidad = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_S_REPOSO_VELOCIDAD_ID, "NOCOMBO", 0, "SI"),

                               SReposoInclinacion = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_S_REPOSO_INCLINACION_ID, "NOCOMBO", 0, "SI"),

                               SReposoMTS = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_S_REPOSO_MTS_ID, "NOCOMBO", 0, "SI"),

                               SReposoFC = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_S_REPOSO_FC_ID, "NOCOMBO", 0, "SI"),

                               SReposoPAS = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_S_REPOSO_PAS_ID, "NOCOMBO", 0, "SI"),

                               SReposoProducto = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_S_REPOSO_PRODUCTO_ID, "NOCOMBO", 0, "SI"),

                               SReposoComentario = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_S_REPOSO_COMENTARIO_ID, "NOCOMBO", 0, "SI"),





                               //-----------------------------------------------------------------



                               TiempoEjercicio = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_TIEMPO_EJERCICIO_ID, "NOCOMBO", 0, "SI"),

                               CPV = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_CPVS_ID, "NOCOMBO", 0, "SI"),

                               Derv = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_DERIV_100_UVST_ID, "NOCOMBO", 0, "SI"),

                               Velocidad = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_VELOCIDAD_ID, "NOCOMBO", 0, "SI"),

                               Pendiente = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_PENDIENTE_ID, "NOCOMBO", 0, "SI"),

                               METS = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_METS_ID, "NOCOMBO", 0, "SI"),

                               FCardiaca = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_F_CARDIACA_ID, "NOCOMBO", 0, "SI"),

                               PSistolica = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_P_SISTOLICA_ID, "NOCOMBO", 0, "SI"),

                               PDiastolica = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_DIASTOLICA_ID, "NOCOMBO", 0, "SI"),

                               FCxTA = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_FCXTA_ID, "NOCOMBO", 0, "SI"),

                               IndiceSTFC = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_INDICE_STFC_ID, "NOCOMBO", 0, "SI"),

                               Objetivo = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_OBJETIVO_ID, "NOCOMBO", 0, "SI"),

                               ElevacionST = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_ELEVACION_ST_ID, "NOCOMBO", 0, "SI"),

                               ElevacionSTEn = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_ELEVACION_ST_EN_ID, "NOCOMBO", 0, "SI"),

                               ElevacionSTAlos = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_ELEVACION_ST_ALOS_ID, "NOCOMBO", 0, "SI"),

                               DepresionST = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_DEPRESION_ST_ID, "NOCOMBO", 0, "SI"),

                               DepresionSTEn = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_DEPRESION_ST_EN_ID, "NOCOMBO", 0, "SI"),

                               DepresionSTAlos = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_DEPRESION_ST_ALOS_ID, "NOCOMBO", 0, "SI"),

                               CambioElevacion = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_CAMBIO_ELEVACION_ST_ID, "NOCOMBO", 0, "SI"),

                               CambioElevacionEn = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_CAMBIO_ELEVACION_ST_EN_ID, "NOCOMBO", 0, "SI"),

                               CambioElevacionAlos = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_CAMBIO_ELEVACION_ST_ALOS_ID, "NOCOMBO", 0, "SI"),

                               CambioDepresion = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_CAMBIO_DEPRESION_ST_ID, "NOCOMBO", 0, "SI"),

                               CambioDepresionEn = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_CAMBIO_DEPRESION_ST_EN_ID, "NOCOMBO", 0, "SI"),

                               CambioDepresionAlos = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_CAMBIO_DEPRESION_ST_ALOS_ID, "NOCOMBO", 0, "SI"),

                               Razones = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_RAZONES_FINALIZAR_ID, "NOCOMBO", 0, "SI"),

                               Conclusiones = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_CONCLUSIONES_ID, "NOCOMBO", 0, "SI"),

                               Sintomas = GetServiceComponentFielValue(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID, Constants.PRUEBA_ESFUERZO_SINTOMAS_ID, "NOCOMBO", 0, "SI"),



                               Dx = GetDiagnosticByServiceIdAndComponent(a.HistoriaClinica, Constants.PRUEBA_ESFUERZO_ID),

                               b_Logo = MedicalCenter.b_Image,
                               EmpresaPropietaria = MedicalCenter.v_Name,
                               EmpresaPropietariaDireccion = MedicalCenter.v_Address,
                               EmpresaPropietariaTelefono = MedicalCenter.v_PhoneNumber,
                               EmpresaPropietariaEmail = MedicalCenter.v_Mail,

                           }).ToList();

                return sql;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public string GetServiceComponentFielValue(string pstrServiceId, string pstrComponentId, string pstrFieldId, string Type, int pintParameter, string pstrConX)
        {
            try
            {
                ServiceBL oServiceBL = new ServiceBL();
                List<Sigesoft.Node.WinClient.BE.ServiceComponentFieldValuesList> oServiceComponentFieldValuesList = new List<Sigesoft.Node.WinClient.BE.ServiceComponentFieldValuesList>();
                string xx = "";
                if (Type == "NOCOMBO")
                {
                    oServiceComponentFieldValuesList = oServiceBL.ValoresComponente(pstrServiceId, pstrComponentId);
                    xx = oServiceComponentFieldValuesList.Count() == 0 || ((Sigesoft.Node.WinClient.BE.ServiceComponentFieldValuesList)oServiceComponentFieldValuesList.Find(p => p.v_ComponentFieldId == pstrFieldId)) == null ? string.Empty : ((Sigesoft.Node.WinClient.BE.ServiceComponentFieldValuesList)oServiceComponentFieldValuesList.Find(p => p.v_ComponentFieldId == pstrFieldId)).v_Value1;
                }
                else
                {
                    oServiceComponentFieldValuesList = oServiceBL.ValoresExamenComponete(pstrServiceId, pstrComponentId, pintParameter);
                    if (pstrConX == "SI")
                    {
                        xx = oServiceComponentFieldValuesList.Count() == 0 ? string.Empty : ((Sigesoft.Node.WinClient.BE.ServiceComponentFieldValuesList)oServiceComponentFieldValuesList.Find(p => p.v_ComponentFieldId == pstrFieldId)).v_Value1;
                    }
                    else
                    {
                        xx = oServiceComponentFieldValuesList.Count() == 0 ? string.Empty : ((Sigesoft.Node.WinClient.BE.ServiceComponentFieldValuesList)oServiceComponentFieldValuesList.Find(p => p.v_ComponentFieldId == pstrFieldId)).v_Value1Name;
                    }

                }

                return xx;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<Sigesoft.Node.WinClient.BE.ReportOdontograma> ReportOdontograma(string pstrserviceId, string pstrComponentId, string Path)
        {
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                var objEntity = (from A in dbContext.service
                                 join B in dbContext.person on A.v_PersonId equals B.v_PersonId
                                 join E in dbContext.servicecomponent on new { a = pstrserviceId, b = pstrComponentId }
                                                                        equals new { a = E.v_ServiceId, b = E.v_ComponentId }

                                 join I in dbContext.protocol on A.v_ProtocolId equals I.v_ProtocolId

                                 join J in dbContext.organization on I.v_EmployerOrganizationId equals J.v_OrganizationId

                                 // Usuario Medico Evaluador / Medico Aprobador ****************************
                                 join me in dbContext.systemuser on E.i_ApprovedUpdateUserId equals me.i_SystemUserId into me_join
                                 from me in me_join.DefaultIfEmpty()

                                 join pme in dbContext.professional on me.v_PersonId equals pme.v_PersonId into pme_join
                                 from pme in pme_join.DefaultIfEmpty()
                                 //**************************************************************************************

                                 where A.v_ServiceId == pstrserviceId
                                 select new Sigesoft.Node.WinClient.BE.ReportOdontograma
                                 {
                                     IdServicio = A.v_ServiceId,
                                     Trabajador = B.v_FirstName + " " + B.v_FirstLastName + " " + B.v_SecondLastName,
                                     Fecha = A.d_ServiceDate.Value,
                                     Puesto = B.v_CurrentOccupation,
                                     Ficha = E.v_ServiceComponentId,
                                     FirmaMedico = pme.b_SignatureImage,
                                     Empresa = J.v_Name

                                 });

                var MedicalCenter = GetInfoMedicalCenter();

                var sql = (from a in objEntity.ToList()
                           select new Sigesoft.Node.WinClient.BE.ReportOdontograma
                           {
                               IdServicio = a.IdServicio,
                               Trabajador = a.Trabajador,
                               Fecha = a.Fecha,
                               Puesto = a.Puesto,
                               Ficha = a.Ficha,
                               FirmaMedico = a.FirmaMedico,
                               Empresa = a.Empresa,
                               Tabaco = GetServiceComponentFielValue(a.IdServicio, pstrComponentId, Constants.ODONTOGRAMA_TABACO_ID, "NOCOMBO", 0, "SI"),
                               Diabetes = GetServiceComponentFielValue(a.IdServicio, pstrComponentId, Constants.ODONTOGRAMA_DIABETES_ID, "NOCOMBO", 0, "SI"),
                               Tbc = GetServiceComponentFielValue(a.IdServicio, pstrComponentId, Constants.ODONTOGRAMA_TBC_ID, "NOCOMBO", 0, "SI"),
                               Ets = GetServiceComponentFielValue(a.IdServicio, pstrComponentId, Constants.ODONTOGRAMA_ETS_ID, "NOCOMBO", 0, "SI"),
                               Hematopatias = GetServiceComponentFielValue(a.IdServicio, pstrComponentId, Constants.ODONTOGRAMA_HEMATOPATIAS_ID, "NOCOMBO", 0, "SI"),
                               Obesidad = GetServiceComponentFielValue(a.IdServicio, pstrComponentId, Constants.ODONTOGRAMA_OBESIDAD_ID, "NOCOMBO", 0, "SI"),
                               Periodontitis = GetServiceComponentFielValue(a.IdServicio, pstrComponentId, Constants.ODONTOGRAMA_PERIODONTITIS_ID, "NOCOMBO", 0, "SI"),
                               Movilidad = GetServiceComponentFielValue(a.IdServicio, pstrComponentId, Constants.ODONTOGRAMA_MOVILIDAD_ID, "NOCOMBO", 0, "SI"),
                               Recesion = GetServiceComponentFielValue(a.IdServicio, pstrComponentId, Constants.ODONTOGRAMA_RECESION_ID, "NOCOMBO", 0, "SI"),
                               Exudacion = GetServiceComponentFielValue(a.IdServicio, pstrComponentId, Constants.ODONTOGRAMA_EXUDACION_ID, "NOCOMBO", 0, "SI"),
                               Gingivitis = GetServiceComponentFielValue(a.IdServicio, pstrComponentId, Constants.ODONTOGRAMA_GINGIVITIS_ID, "NOCOMBO", 0, "SI"),
                               BolsaPeriodontales = GetServiceComponentFielValue(a.IdServicio, pstrComponentId, Constants.ODONTOGRAMA_BOLSA_PERIODONTALES_ID, "NOCOMBO", 0, "SI"),
                               Diagnosticos = GetDiagnosticByServiceIdAndComponent(a.IdServicio, pstrComponentId),
                               PiezasCaries = GetCantidadCaries(a.IdServicio, pstrComponentId, Constants.ODONTOGRAMA_PIEZAS_CARIES_ID),
                               PiezasAusentes = GetCantidadAusentes(a.IdServicio, pstrComponentId, Constants.ODONTOGRAMA_PIEZAS_AUSENTES_ID),

                               //PiezasCuracion = GetCantidad(a.IdServicio,pstrComponentId),
                               Corona = GetCantidad(a.IdServicio, pstrComponentId)[0].ToString(),
                               Exodoncia = GetCantidad(a.IdServicio, pstrComponentId)[1].ToString(),
                               Implante = GetCantidad(a.IdServicio, pstrComponentId)[2].ToString(),
                               Ppr = GetCantidad(a.IdServicio, pstrComponentId)[3].ToString(),
                               ProtesisTotal = GetCantidad(a.IdServicio, pstrComponentId)[4].ToString(),

                               PlacaBacteriana = GetValueOdontograma1(a.IdServicio, pstrComponentId, Constants.ODONTOGRAMA_PLACA_BACTERIANA_ID),
                               RemanentesReticulares = GetValueOdontograma1(a.IdServicio, pstrComponentId, Constants.ODONTOGRAMA_REMANENTES_RETICULARES_ID),
                               OtrosExamen = GetValueOdontograma1(a.IdServicio, pstrComponentId, Constants.ODONTOGRAMA_OTROS_EXAMEN_ID),
                               Aptitud = GetValueOdontograma1(a.IdServicio, pstrComponentId, Constants.ODONTOGRAMA_APTITUD_ID),

                               Diente181 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D18_1, Path, "18"),
                               Diente182 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D18_2, Path, "18"),
                               Diente183 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D18_3, Path, "18"),
                               Diente184 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D18_4, Path, "18"),
                               Diente185 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D18_5, Path, "18"),
                               Diente186 = GetValueOdontogramaAusente(a.IdServicio, pstrComponentId, Constants.D18_6, Path),

                               Diente171 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D17_1, Path, "17"),
                               Diente172 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D17_2, Path, "17"),
                               Diente173 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D17_3, Path, "17"),
                               Diente174 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D17_4, Path, "17"),
                               Diente175 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D17_5, Path, "17"),
                               Diente176 = GetValueOdontogramaAusente(a.IdServicio, pstrComponentId, Constants.D17_6, Path),

                               Diente161 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D16_1, Path, "16"),
                               Diente162 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D16_2, Path, "16"),
                               Diente163 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D16_3, Path, "16"),
                               Diente164 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D16_4, Path, "16"),
                               Diente165 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D16_5, Path, "16"),
                               Diente166 = GetValueOdontogramaAusente(a.IdServicio, pstrComponentId, Constants.D16_6, Path),

                               Diente151 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D15_1, Path, "15"),
                               Diente152 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D15_2, Path, "15"),
                               Diente153 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D15_3, Path, "15"),
                               Diente154 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D15_4, Path, "15"),
                               Diente155 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D15_5, Path, "15"),
                               Diente156 = GetValueOdontogramaAusente(a.IdServicio, pstrComponentId, Constants.D15_6, Path),

                               Diente141 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D14_1, Path, "14"),
                               Diente142 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D14_2, Path, "14"),
                               Diente143 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D14_3, Path, "14"),
                               Diente144 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D14_4, Path, "14"),
                               Diente145 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D14_5, Path, "14"),
                               Diente146 = GetValueOdontogramaAusente(a.IdServicio, pstrComponentId, Constants.D14_6, Path),

                               Diente131 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D13_1, Path, "13"),
                               Diente132 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D13_2, Path, "13"),
                               Diente133 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D13_3, Path, "13"),
                               Diente134 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D13_4, Path, "13"),
                               Diente135 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D13_5, Path, "13"),
                               Diente136 = GetValueOdontogramaAusente(a.IdServicio, pstrComponentId, Constants.D13_6, Path),

                               Diente121 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D12_1, Path, "12"),
                               Diente122 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D12_2, Path, "12"),
                               Diente123 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D12_3, Path, "12"),
                               Diente124 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D12_4, Path, "12"),
                               Diente125 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D12_5, Path, "12"),
                               Diente126 = GetValueOdontogramaAusente(a.IdServicio, pstrComponentId, Constants.D12_6, Path),

                               Diente111 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D11_1, Path, "11"),
                               Diente112 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D11_2, Path, "11"),
                               Diente113 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D11_3, Path, "11"),
                               Diente114 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D11_4, Path, "11"),
                               Diente115 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D11_5, Path, "11"),
                               Diente116 = GetValueOdontogramaAusente(a.IdServicio, pstrComponentId, Constants.D11_6, Path),

                               //-------------------------------------------------------------------------------

                               Diente211 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D21_1, Path, "21"),
                               Diente212 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D21_2, Path, "21"),
                               Diente213 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D21_3, Path, "21"),
                               Diente214 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D21_4, Path, "21"),
                               Diente215 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D21_5, Path, "21"),
                               Diente216 = GetValueOdontogramaAusente(a.IdServicio, pstrComponentId, Constants.D21_6, Path),

                               Diente221 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D22_1, Path, "22"),
                               Diente222 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D22_2, Path, "22"),
                               Diente223 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D22_3, Path, "22"),
                               Diente224 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D22_4, Path, "22"),
                               Diente225 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D22_5, Path, "22"),
                               Diente226 = GetValueOdontogramaAusente(a.IdServicio, pstrComponentId, Constants.D22_6, Path),

                               Diente231 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D23_1, Path, "23"),
                               Diente232 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D23_2, Path, "23"),
                               Diente233 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D23_3, Path, "23"),
                               Diente234 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D23_4, Path, "23"),
                               Diente235 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D23_5, Path, "23"),
                               Diente236 = GetValueOdontogramaAusente(a.IdServicio, pstrComponentId, Constants.D23_6, Path),

                               Diente241 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D24_1, Path, "24"),
                               Diente242 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D24_2, Path, "24"),
                               Diente243 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D24_3, Path, "24"),
                               Diente244 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D24_4, Path, "24"),
                               Diente245 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D24_5, Path, "24"),
                               Diente246 = GetValueOdontogramaAusente(a.IdServicio, pstrComponentId, Constants.D24_6, Path),

                               Diente251 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D25_1, Path, "25"),
                               Diente252 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D25_2, Path, "25"),
                               Diente253 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D25_3, Path, "25"),
                               Diente254 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D25_4, Path, "25"),
                               Diente255 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D25_5, Path, "25"),
                               Diente256 = GetValueOdontogramaAusente(a.IdServicio, pstrComponentId, Constants.D25_6, Path),

                               Diente261 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D26_1, Path, "26"),
                               Diente262 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D26_2, Path, "26"),
                               Diente263 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D26_3, Path, "26"),
                               Diente264 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D26_4, Path, "26"),
                               Diente265 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D26_5, Path, "26"),
                               Diente266 = GetValueOdontogramaAusente(a.IdServicio, pstrComponentId, Constants.D26_6, Path),

                               Diente271 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D27_1, Path, "27"),
                               Diente272 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D27_2, Path, "27"),
                               Diente273 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D27_3, Path, "27"),
                               Diente274 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D27_4, Path, "27"),
                               Diente275 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D27_5, Path, "27"),
                               Diente276 = GetValueOdontogramaAusente(a.IdServicio, pstrComponentId, Constants.D27_6, Path),

                               Diente281 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D28_1, Path, "28"),
                               Diente282 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D28_2, Path, "28"),
                               Diente283 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D28_3, Path, "28"),
                               Diente284 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D28_4, Path, "28"),
                               Diente285 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D28_5, Path, "28"),
                               Diente286 = GetValueOdontogramaAusente(a.IdServicio, pstrComponentId, Constants.D28_6, Path),

                               //-------------------------------------------------------------------------------

                               Diente311 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D31_1, Path, "31"),
                               Diente312 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D31_2, Path, "31"),
                               Diente313 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D31_3, Path, "31"),
                               Diente314 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D31_4, Path, "31"),
                               Diente315 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D31_5, Path, "31"),
                               Diente316 = GetValueOdontogramaAusente(a.IdServicio, pstrComponentId, Constants.D31_6, Path),

                               Diente321 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D32_1, Path, "32"),
                               Diente322 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D32_2, Path, "32"),
                               Diente323 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D32_3, Path, "32"),
                               Diente324 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D32_4, Path, "32"),
                               Diente325 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D32_5, Path, "32"),
                               Diente326 = GetValueOdontogramaAusente(a.IdServicio, pstrComponentId, Constants.D32_6, Path),

                               Diente331 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D33_1, Path, "33"),
                               Diente332 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D33_2, Path, "33"),
                               Diente333 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D33_3, Path, "33"),
                               Diente334 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D33_4, Path, "33"),
                               Diente335 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D33_5, Path, "33"),
                               Diente336 = GetValueOdontogramaAusente(a.IdServicio, pstrComponentId, Constants.D33_6, Path),

                               Diente341 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D34_1, Path, "34"),
                               Diente342 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D34_2, Path, "34"),
                               Diente343 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D34_3, Path, "34"),
                               Diente344 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D34_4, Path, "34"),
                               Diente345 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D34_5, Path, "34"),
                               Diente346 = GetValueOdontogramaAusente(a.IdServicio, pstrComponentId, Constants.D34_6, Path),

                               Diente351 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D35_1, Path, "35"),
                               Diente352 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D35_2, Path, "35"),
                               Diente353 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D35_3, Path, "35"),
                               Diente354 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D35_4, Path, "35"),
                               Diente355 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D35_5, Path, "35"),
                               Diente356 = GetValueOdontogramaAusente(a.IdServicio, pstrComponentId, Constants.D35_6, Path),


                               Diente361 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D36_1, Path, "36"),
                               Diente362 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D36_2, Path, "36"),
                               Diente363 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D36_3, Path, "36"),
                               Diente364 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D36_4, Path, "36"),
                               Diente365 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D36_5, Path, "36"),
                               Diente366 = GetValueOdontogramaAusente(a.IdServicio, pstrComponentId, Constants.D36_6, Path),

                               Diente371 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D37_1, Path, "37"),
                               Diente372 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D37_2, Path, "37"),
                               Diente373 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D37_3, Path, "37"),
                               Diente374 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D37_4, Path, "37"),
                               Diente375 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D37_5, Path, "37"),
                               Diente376 = GetValueOdontogramaAusente(a.IdServicio, pstrComponentId, Constants.D37_6, Path),

                               Diente381 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D38_1, Path, "38"),
                               Diente382 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D38_2, Path, "38"),
                               Diente383 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D38_3, Path, "38"),
                               Diente384 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D38_4, Path, "38"),
                               Diente385 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D38_5, Path, "38"),
                               Diente386 = GetValueOdontogramaAusente(a.IdServicio, pstrComponentId, Constants.D38_6, Path),
                               //-------------------------------------------------------------------------------

                               Diente411 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D41_1, Path, "41"),
                               Diente412 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D41_2, Path, "41"),
                               Diente413 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D41_3, Path, "41"),
                               Diente414 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D41_4, Path, "41"),
                               Diente415 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D41_5, Path, "41"),
                               Diente416 = GetValueOdontogramaAusente(a.IdServicio, pstrComponentId, Constants.D41_6, Path),

                               Diente421 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D42_1, Path, "42"),
                               Diente422 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D42_2, Path, "42"),
                               Diente423 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D42_3, Path, "42"),
                               Diente424 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D42_4, Path, "42"),
                               Diente425 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D42_5, Path, "42"),
                               Diente426 = GetValueOdontogramaAusente(a.IdServicio, pstrComponentId, Constants.D42_6, Path),

                               Diente431 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D43_1, Path, "43"),
                               Diente432 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D43_2, Path, "43"),
                               Diente433 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D43_3, Path, "43"),
                               Diente434 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D43_4, Path, "43"),
                               Diente435 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D43_5, Path, "43"),
                               Diente436 = GetValueOdontogramaAusente(a.IdServicio, pstrComponentId, Constants.D43_6, Path),

                               Diente441 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D44_1, Path, "44"),
                               Diente442 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D44_2, Path, "44"),
                               Diente443 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D44_3, Path, "44"),
                               Diente444 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D44_4, Path, "44"),
                               Diente445 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D44_5, Path, "44"),
                               Diente446 = GetValueOdontogramaAusente(a.IdServicio, pstrComponentId, Constants.D44_6, Path),

                               Diente451 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D45_1, Path, "45"),
                               Diente452 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D45_2, Path, "45"),
                               Diente453 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D45_3, Path, "45"),
                               Diente454 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D45_4, Path, "45"),
                               Diente455 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D45_5, Path, "45"),
                               Diente456 = GetValueOdontogramaAusente(a.IdServicio, pstrComponentId, Constants.D45_6, Path),

                               Diente461 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D46_1, Path, "46"),
                               Diente462 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D46_2, Path, "46"),
                               Diente463 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D46_3, Path, "46"),
                               Diente464 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D46_4, Path, "46"),
                               Diente465 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D46_5, Path, "46"),
                               Diente466 = GetValueOdontogramaAusente(a.IdServicio, pstrComponentId, Constants.D46_6, Path),

                               Diente471 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D47_1, Path, "47"),
                               Diente472 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D47_2, Path, "47"),
                               Diente473 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D47_3, Path, "47"),
                               Diente474 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D47_4, Path, "47"),
                               Diente475 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D47_5, Path, "47"),
                               Diente476 = GetValueOdontogramaAusente(a.IdServicio, pstrComponentId, Constants.D47_6, Path),

                               Diente481 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D48_1, Path, "48"),
                               Diente482 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D48_2, Path, "48"),
                               Diente483 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D48_3, Path, "48"),
                               Diente484 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D48_4, Path, "48"),
                               Diente485 = GetValueOdontograma(a.IdServicio, pstrComponentId, Constants.D48_5, Path, "48"),
                               Diente486 = GetValueOdontogramaAusente(a.IdServicio, pstrComponentId, Constants.D48_6, Path),

                               PiezasCuracion = NroDientesCurados(ListaDiente),

                               b_Logo = MedicalCenter.b_Image,
                               EmpresaPropietaria = MedicalCenter.v_Name,
                               EmpresaPropietariaDireccion = MedicalCenter.v_Address,
                               EmpresaPropietariaTelefono = MedicalCenter.v_PhoneNumber,
                               EmpresaPropietariaEmail = MedicalCenter.v_Mail,

                           }).ToList();

                return sql;
            }
            catch (Exception)
            {

                throw;
            }
        }
        private int NroDientesCurados(List<int> ListaD)
        {

            var x = ListaD.Distinct();

            return x.Count();
        }
        public List<Sigesoft.Node.WinClient.BE.ServiceComponentFieldValuesList> ValoresComponenteOdontograma(string pstrServiceId, string pstrComponentId, string pstrPath)
        {
            SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
            try
            {
                List<Sigesoft.Node.WinClient.BE.ServiceComponentFieldValuesList> serviceComponentFieldValues = (from A in dbContext.service
                                                                                     join B in dbContext.servicecomponent on A.v_ServiceId equals B.v_ServiceId
                                                                                     join C in dbContext.servicecomponentfields on B.v_ServiceComponentId equals C.v_ServiceComponentId
                                                                                     join D in dbContext.servicecomponentfieldvalues on C.v_ServiceComponentFieldsId equals D.v_ServiceComponentFieldsId

                                                                                     where (A.v_ServiceId == pstrServiceId)
                                                                                           && (B.v_ComponentId == pstrComponentId)
                                                                                           && (B.i_IsDeleted == 0)
                                                                                           && (C.i_IsDeleted == 0)
                                                                                     let range = (
                                                                                                     D.v_Value1 == "2" ? pstrPath + "\\Resources\\caries.png" :
                                                                                                      D.v_Value1 == "3" ? pstrPath + "\\Resources\\curacion.png" :
                                                                                                      string.Empty
                                                                                          )
                                                                                    select new Sigesoft.Node.WinClient.BE.ServiceComponentFieldValuesList
                                                                                     {
                                                                                         //v_ComponentId = B.v_ComponentId,
                                                                                         v_ComponentFieldId = C.v_ComponentFieldId,
                                                                                         //v_ComponentFieldId = G.v_ComponentFieldId,
                                                                                         //v_ComponentFielName = G.v_TextLabel,
                                                                                         v_ServiceComponentFieldsId = C.v_ServiceComponentFieldsId,
                                                                                         v_Value1 = range,
                                                                                         v_Value2 = D.v_Value1
                                                                                     }).ToList();


                return serviceComponentFieldValues;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public string GetValueOdontograma(string pstrServiceId, string pstrComponentId, string pstrFieldId, string pstrpath, string NroDiente)
        {
            try
            {
                ServiceBL oServiceBL = new ServiceBL();
                List<Sigesoft.Node.WinClient.BE.ServiceComponentFieldValuesList> oServiceComponentFieldValuesList = new List<Sigesoft.Node.WinClient.BE.ServiceComponentFieldValuesList>();
                List<Sigesoft.Node.WinClient.BE.ServiceComponentFieldValuesList> oServiceComponentFieldValuesList1 = new List<Sigesoft.Node.WinClient.BE.ServiceComponentFieldValuesList>();
                oServiceComponentFieldValuesList1 = ValoresComponenteOdontogramaValue1(pstrServiceId, pstrComponentId);
                oServiceComponentFieldValuesList = oServiceBL.ValoresComponenteOdontograma(pstrServiceId, pstrComponentId, pstrpath);
                var xx = oServiceComponentFieldValuesList.Count() == 0 || ((Sigesoft.Node.WinClient.BE.ServiceComponentFieldValuesList)oServiceComponentFieldValuesList.Find(p => p.v_ComponentFieldId == pstrFieldId)) == null ? string.Empty : ((Sigesoft.Node.WinClient.BE.ServiceComponentFieldValuesList)oServiceComponentFieldValuesList.Find(p => p.v_ComponentFieldId == pstrFieldId)).v_Value1;
                var valorDiente = oServiceComponentFieldValuesList1.Count() == 0 || ((Sigesoft.Node.WinClient.BE.ServiceComponentFieldValuesList)oServiceComponentFieldValuesList1.Find(p => p.v_ComponentFieldId == pstrFieldId)) == null ? string.Empty : ((Sigesoft.Node.WinClient.BE.ServiceComponentFieldValuesList)oServiceComponentFieldValuesList1.Find(p => p.v_ComponentFieldId == pstrFieldId)).v_Value1;


                switch (NroDiente)
                {
                    case "18":
                        if (valorDiente == "3")
                        {
                            //ContadorD18 = 1;
                            ListaDiente.Add(18);
                        }
                        break;
                    case "17":
                        if (valorDiente == "3")
                        {
                            ListaDiente.Add(17);
                        }
                        break;
                    case "16":
                        if (valorDiente == "3")
                        {
                            ListaDiente.Add(16);
                        }
                        break;
                    case "15":
                        if (valorDiente == "3")
                        {
                            ListaDiente.Add(15);
                        }
                        break;
                    case "14":
                        if (valorDiente == "3")
                        {
                            ListaDiente.Add(14);
                        }
                        break;
                    case "13":
                        if (valorDiente == "3")
                        {
                            ListaDiente.Add(13);
                        }
                        break;
                    case "12":
                        if (valorDiente == "3")
                        {
                            ListaDiente.Add(12);
                        }
                        break;
                    case "11":
                        if (valorDiente == "3")
                        {
                            ListaDiente.Add(11);
                        }
                        break;

                    //--------------------------------------

                    case "21":
                        if (valorDiente == "3")
                        {
                            //ContadorD18 = 1;
                            ListaDiente.Add(21);
                        }
                        break;
                    case "22":
                        if (valorDiente == "3")
                        {
                            ListaDiente.Add(22);
                        }
                        break;
                    case "23":
                        if (valorDiente == "3")
                        {
                            ListaDiente.Add(23);
                        }
                        break;
                    case "24":
                        if (valorDiente == "3")
                        {
                            ListaDiente.Add(24);
                        }
                        break;
                    case "25":
                        if (valorDiente == "3")
                        {
                            ListaDiente.Add(25);
                        }
                        break;
                    case "26":
                        if (valorDiente == "3")
                        {
                            ListaDiente.Add(26);
                        }
                        break;
                    case "27":
                        if (valorDiente == "3")
                        {
                            ListaDiente.Add(27);
                        }
                        break;
                    case "28":
                        if (valorDiente == "3")
                        {
                            ListaDiente.Add(28);
                        }
                        break;

                    //------------------------------

                    case "31":
                        if (valorDiente == "3")
                        {
                            //ContadorD18 = 1;
                            ListaDiente.Add(31);
                        }
                        break;
                    case "32":
                        if (valorDiente == "3")
                        {
                            ListaDiente.Add(32);
                        }
                        break;
                    case "33":
                        if (valorDiente == "3")
                        {
                            ListaDiente.Add(33);
                        }
                        break;
                    case "34":
                        if (valorDiente == "3")
                        {
                            ListaDiente.Add(34);
                        }
                        break;
                    case "35":
                        if (valorDiente == "3")
                        {
                            ListaDiente.Add(35);
                        }
                        break;
                    case "36":
                        if (valorDiente == "3")
                        {
                            ListaDiente.Add(36);
                        }
                        break;
                    case "37":
                        if (valorDiente == "3")
                        {
                            ListaDiente.Add(37);
                        }
                        break;
                    case "38":
                        if (valorDiente == "3")
                        {
                            ListaDiente.Add(38);
                        }
                        break;

                    //------------------------------

                    case "41":
                        if (valorDiente == "3")
                        {
                            //ContadorD18 = 1;
                            ListaDiente.Add(41);
                        }
                        break;
                    case "42":
                        if (valorDiente == "3")
                        {
                            ListaDiente.Add(42);
                        }
                        break;
                    case "43":
                        if (valorDiente == "3")
                        {
                            ListaDiente.Add(43);
                        }
                        break;
                    case "44":
                        if (valorDiente == "3")
                        {
                            ListaDiente.Add(44);
                        }
                        break;
                    case "45":
                        if (valorDiente == "3")
                        {
                            ListaDiente.Add(45);
                        }
                        break;
                    case "46":
                        if (valorDiente == "3")
                        {
                            ListaDiente.Add(46);
                        }
                        break;
                    case "47":
                        if (valorDiente == "3")
                        {
                            ListaDiente.Add(47);
                        }
                        break;
                    case "48":
                        if (valorDiente == "3")
                        {
                            ListaDiente.Add(48);
                        }
                        break;
                    default:
                        break;
                }

                return xx;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public int[] GetCantidad(string pstrServiceId, string pstrComponentId)
        {
            try
            {
                int ContadorCorona = 0;
                int ContadorExodoncia = 0;
                int ContadorImplante = 0;
                int ContadorPPR = 0;
                int ContadorProtesisTotal = 0;

                int[] xxx = new int[5];

                ServiceBL oServiceBL = new ServiceBL();
                List<Sigesoft.Node.WinClient.BE.ServiceComponentFieldValuesList> oServiceComponentFieldValuesList = new List<Sigesoft.Node.WinClient.BE.ServiceComponentFieldValuesList>();

                oServiceComponentFieldValuesList = oServiceBL.ValoresComponenteOdontograma1(pstrServiceId, pstrComponentId);

                for (int i = 0; i < oServiceComponentFieldValuesList.Count(); i++)
                {

                    #region Region 1

                    if (oServiceComponentFieldValuesList[i].v_ComponentFieldId == Constants.D11_6)
                    {
                        if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Corona).ToString())
                        {
                            ContadorCorona = ContadorCorona + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Exodoncia).ToString())
                        {
                            ContadorExodoncia = ContadorExodoncia + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Implante).ToString())
                        {
                            ContadorImplante = ContadorImplante + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.PPR).ToString())
                        {
                            ContadorPPR = ContadorPPR + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.ProtesisTotal).ToString())
                        {
                            ContadorProtesisTotal = ContadorProtesisTotal + 1;
                        }
                    }

                    if (oServiceComponentFieldValuesList[i].v_ComponentFieldId == Constants.D12_6)
                    {
                        if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Corona).ToString())
                        {
                            ContadorCorona = ContadorCorona + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Exodoncia).ToString())
                        {
                            ContadorExodoncia = ContadorExodoncia + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Implante).ToString())
                        {
                            ContadorImplante = ContadorImplante + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.PPR).ToString())
                        {
                            ContadorPPR = ContadorPPR + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.ProtesisTotal).ToString())
                        {
                            ContadorProtesisTotal = ContadorProtesisTotal + 1;
                        }
                    }

                    if (oServiceComponentFieldValuesList[i].v_ComponentFieldId == Constants.D13_6)
                    {
                        if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Corona).ToString())
                        {
                            ContadorCorona = ContadorCorona + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Exodoncia).ToString())
                        {
                            ContadorExodoncia = ContadorExodoncia + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Implante).ToString())
                        {
                            ContadorImplante = ContadorImplante + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.PPR).ToString())
                        {
                            ContadorPPR = ContadorPPR + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.ProtesisTotal).ToString())
                        {
                            ContadorProtesisTotal = ContadorProtesisTotal + 1;
                        }
                    }

                    if (oServiceComponentFieldValuesList[i].v_ComponentFieldId == Constants.D14_6)
                    {
                        if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Corona).ToString())
                        {
                            ContadorCorona = ContadorCorona + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Exodoncia).ToString())
                        {
                            ContadorExodoncia = ContadorExodoncia + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Implante).ToString())
                        {
                            ContadorImplante = ContadorImplante + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.PPR).ToString())
                        {
                            ContadorPPR = ContadorPPR + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.ProtesisTotal).ToString())
                        {
                            ContadorProtesisTotal = ContadorProtesisTotal + 1;
                        }
                    }

                    if (oServiceComponentFieldValuesList[i].v_ComponentFieldId == Constants.D15_6)
                    {
                        if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Corona).ToString())
                        {
                            ContadorCorona = ContadorCorona + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Exodoncia).ToString())
                        {
                            ContadorExodoncia = ContadorExodoncia + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Implante).ToString())
                        {
                            ContadorImplante = ContadorImplante + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.PPR).ToString())
                        {
                            ContadorPPR = ContadorPPR + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.ProtesisTotal).ToString())
                        {
                            ContadorProtesisTotal = ContadorProtesisTotal + 1;
                        }
                    }

                    if (oServiceComponentFieldValuesList[i].v_ComponentFieldId == Constants.D16_6)
                    {
                        if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Corona).ToString())
                        {
                            ContadorCorona = ContadorCorona + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Exodoncia).ToString())
                        {
                            ContadorExodoncia = ContadorExodoncia + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Implante).ToString())
                        {
                            ContadorImplante = ContadorImplante + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.PPR).ToString())
                        {
                            ContadorPPR = ContadorPPR + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.ProtesisTotal).ToString())
                        {
                            ContadorProtesisTotal = ContadorProtesisTotal + 1;
                        }
                    }

                    if (oServiceComponentFieldValuesList[i].v_ComponentFieldId == Constants.D17_6)
                    {
                        if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Corona).ToString())
                        {
                            ContadorCorona = ContadorCorona + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Exodoncia).ToString())
                        {
                            ContadorExodoncia = ContadorExodoncia + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Implante).ToString())
                        {
                            ContadorImplante = ContadorImplante + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.PPR).ToString())
                        {
                            ContadorPPR = ContadorPPR + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.ProtesisTotal).ToString())
                        {
                            ContadorProtesisTotal = ContadorProtesisTotal + 1;
                        }
                    }

                    if (oServiceComponentFieldValuesList[i].v_ComponentFieldId == Constants.D18_6)
                    {
                        if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Corona).ToString())
                        {
                            ContadorCorona = ContadorCorona + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Exodoncia).ToString())
                        {
                            ContadorExodoncia = ContadorExodoncia + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Implante).ToString())
                        {
                            ContadorImplante = ContadorImplante + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.PPR).ToString())
                        {
                            ContadorPPR = ContadorPPR + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.ProtesisTotal).ToString())
                        {
                            ContadorProtesisTotal = ContadorProtesisTotal + 1;
                        }
                    }

                    #endregion

                    #region Region 2

                    if (oServiceComponentFieldValuesList[i].v_ComponentFieldId == Constants.D21_6)
                    {
                        if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Corona).ToString())
                        {
                            ContadorCorona = ContadorCorona + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Exodoncia).ToString())
                        {
                            ContadorExodoncia = ContadorExodoncia + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Implante).ToString())
                        {
                            ContadorImplante = ContadorImplante + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.PPR).ToString())
                        {
                            ContadorPPR = ContadorPPR + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.ProtesisTotal).ToString())
                        {
                            ContadorProtesisTotal = ContadorProtesisTotal + 1;
                        }
                    }

                    if (oServiceComponentFieldValuesList[i].v_ComponentFieldId == Constants.D22_6)
                    {
                        if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Corona).ToString())
                        {
                            ContadorCorona = ContadorCorona + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Exodoncia).ToString())
                        {
                            ContadorExodoncia = ContadorExodoncia + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Implante).ToString())
                        {
                            ContadorImplante = ContadorImplante + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.PPR).ToString())
                        {
                            ContadorPPR = ContadorPPR + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.ProtesisTotal).ToString())
                        {
                            ContadorProtesisTotal = ContadorProtesisTotal + 1;
                        }
                    }

                    if (oServiceComponentFieldValuesList[i].v_ComponentFieldId == Constants.D23_6)
                    {
                        if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Corona).ToString())
                        {
                            ContadorCorona = ContadorCorona + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Exodoncia).ToString())
                        {
                            ContadorExodoncia = ContadorExodoncia + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Implante).ToString())
                        {
                            ContadorImplante = ContadorImplante + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.PPR).ToString())
                        {
                            ContadorPPR = ContadorPPR + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.ProtesisTotal).ToString())
                        {
                            ContadorProtesisTotal = ContadorProtesisTotal + 1;
                        }
                    }

                    if (oServiceComponentFieldValuesList[i].v_ComponentFieldId == Constants.D24_6)
                    {
                        if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Corona).ToString())
                        {
                            ContadorCorona = ContadorCorona + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Exodoncia).ToString())
                        {
                            ContadorExodoncia = ContadorExodoncia + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Implante).ToString())
                        {
                            ContadorImplante = ContadorImplante + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.PPR).ToString())
                        {
                            ContadorPPR = ContadorPPR + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.ProtesisTotal).ToString())
                        {
                            ContadorProtesisTotal = ContadorProtesisTotal + 1;
                        }
                    }

                    if (oServiceComponentFieldValuesList[i].v_ComponentFieldId == Constants.D25_6)
                    {
                        if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Corona).ToString())
                        {
                            ContadorCorona = ContadorCorona + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Exodoncia).ToString())
                        {
                            ContadorExodoncia = ContadorExodoncia + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Implante).ToString())
                        {
                            ContadorImplante = ContadorImplante + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.PPR).ToString())
                        {
                            ContadorPPR = ContadorPPR + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.ProtesisTotal).ToString())
                        {
                            ContadorProtesisTotal = ContadorProtesisTotal + 1;
                        }
                    }

                    if (oServiceComponentFieldValuesList[i].v_ComponentFieldId == Constants.D26_6)
                    {
                        if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Corona).ToString())
                        {
                            ContadorCorona = ContadorCorona + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Exodoncia).ToString())
                        {
                            ContadorExodoncia = ContadorExodoncia + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Implante).ToString())
                        {
                            ContadorImplante = ContadorImplante + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.PPR).ToString())
                        {
                            ContadorPPR = ContadorPPR + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.ProtesisTotal).ToString())
                        {
                            ContadorProtesisTotal = ContadorProtesisTotal + 1;
                        }
                    }

                    if (oServiceComponentFieldValuesList[i].v_ComponentFieldId == Constants.D27_6)
                    {
                        if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Corona).ToString())
                        {
                            ContadorCorona = ContadorCorona + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Exodoncia).ToString())
                        {
                            ContadorExodoncia = ContadorExodoncia + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Implante).ToString())
                        {
                            ContadorImplante = ContadorImplante + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.PPR).ToString())
                        {
                            ContadorPPR = ContadorPPR + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.ProtesisTotal).ToString())
                        {
                            ContadorProtesisTotal = ContadorProtesisTotal + 1;
                        }
                    }

                    if (oServiceComponentFieldValuesList[i].v_ComponentFieldId == Constants.D28_6)
                    {
                        if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Corona).ToString())
                        {
                            ContadorCorona = ContadorCorona + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Exodoncia).ToString())
                        {
                            ContadorExodoncia = ContadorExodoncia + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Implante).ToString())
                        {
                            ContadorImplante = ContadorImplante + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.PPR).ToString())
                        {
                            ContadorPPR = ContadorPPR + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.ProtesisTotal).ToString())
                        {
                            ContadorProtesisTotal = ContadorProtesisTotal + 1;
                        }
                    }

                    #endregion

                    #region Region 3

                    if (oServiceComponentFieldValuesList[i].v_ComponentFieldId == Constants.D31_6)
                    {
                        if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Corona).ToString())
                        {
                            ContadorCorona = ContadorCorona + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Exodoncia).ToString())
                        {
                            ContadorExodoncia = ContadorExodoncia + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Implante).ToString())
                        {
                            ContadorImplante = ContadorImplante + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.PPR).ToString())
                        {
                            ContadorPPR = ContadorPPR + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.ProtesisTotal).ToString())
                        {
                            ContadorProtesisTotal = ContadorProtesisTotal + 1;
                        }
                    }

                    if (oServiceComponentFieldValuesList[i].v_ComponentFieldId == Constants.D32_6)
                    {
                        if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Corona).ToString())
                        {
                            ContadorCorona = ContadorCorona + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Exodoncia).ToString())
                        {
                            ContadorExodoncia = ContadorExodoncia + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Implante).ToString())
                        {
                            ContadorImplante = ContadorImplante + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.PPR).ToString())
                        {
                            ContadorPPR = ContadorPPR + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.ProtesisTotal).ToString())
                        {
                            ContadorProtesisTotal = ContadorProtesisTotal + 1;
                        }
                    }

                    if (oServiceComponentFieldValuesList[i].v_ComponentFieldId == Constants.D33_6)
                    {
                        if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Corona).ToString())
                        {
                            ContadorCorona = ContadorCorona + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Exodoncia).ToString())
                        {
                            ContadorExodoncia = ContadorExodoncia + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Implante).ToString())
                        {
                            ContadorImplante = ContadorImplante + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.PPR).ToString())
                        {
                            ContadorPPR = ContadorPPR + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.ProtesisTotal).ToString())
                        {
                            ContadorProtesisTotal = ContadorProtesisTotal + 1;
                        }
                    }

                    if (oServiceComponentFieldValuesList[i].v_ComponentFieldId == Constants.D34_6)
                    {
                        if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Corona).ToString())
                        {
                            ContadorCorona = ContadorCorona + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Exodoncia).ToString())
                        {
                            ContadorExodoncia = ContadorExodoncia + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Implante).ToString())
                        {
                            ContadorImplante = ContadorImplante + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.PPR).ToString())
                        {
                            ContadorPPR = ContadorPPR + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.ProtesisTotal).ToString())
                        {
                            ContadorProtesisTotal = ContadorProtesisTotal + 1;
                        }
                    }

                    if (oServiceComponentFieldValuesList[i].v_ComponentFieldId == Constants.D35_6)
                    {
                        if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Corona).ToString())
                        {
                            ContadorCorona = ContadorCorona + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Exodoncia).ToString())
                        {
                            ContadorExodoncia = ContadorExodoncia + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Implante).ToString())
                        {
                            ContadorImplante = ContadorImplante + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.PPR).ToString())
                        {
                            ContadorPPR = ContadorPPR + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.ProtesisTotal).ToString())
                        {
                            ContadorProtesisTotal = ContadorProtesisTotal + 1;
                        }
                    }

                    if (oServiceComponentFieldValuesList[i].v_ComponentFieldId == Constants.D36_6)
                    {
                        if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Corona).ToString())
                        {
                            ContadorCorona = ContadorCorona + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Exodoncia).ToString())
                        {
                            ContadorExodoncia = ContadorExodoncia + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Implante).ToString())
                        {
                            ContadorImplante = ContadorImplante + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.PPR).ToString())
                        {
                            ContadorPPR = ContadorPPR + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.ProtesisTotal).ToString())
                        {
                            ContadorProtesisTotal = ContadorProtesisTotal + 1;
                        }
                    }

                    if (oServiceComponentFieldValuesList[i].v_ComponentFieldId == Constants.D37_6)
                    {
                        if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Corona).ToString())
                        {
                            ContadorCorona = ContadorCorona + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Exodoncia).ToString())
                        {
                            ContadorExodoncia = ContadorExodoncia + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Implante).ToString())
                        {
                            ContadorImplante = ContadorImplante + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.PPR).ToString())
                        {
                            ContadorPPR = ContadorPPR + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.ProtesisTotal).ToString())
                        {
                            ContadorProtesisTotal = ContadorProtesisTotal + 1;
                        }
                    }

                    if (oServiceComponentFieldValuesList[i].v_ComponentFieldId == Constants.D38_6)
                    {
                        if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Corona).ToString())
                        {
                            ContadorCorona = ContadorCorona + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Exodoncia).ToString())
                        {
                            ContadorExodoncia = ContadorExodoncia + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Implante).ToString())
                        {
                            ContadorImplante = ContadorImplante + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.PPR).ToString())
                        {
                            ContadorPPR = ContadorPPR + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.ProtesisTotal).ToString())
                        {
                            ContadorProtesisTotal = ContadorProtesisTotal + 1;
                        }
                    }

                    #endregion

                    #region Region 4

                    if (oServiceComponentFieldValuesList[i].v_ComponentFieldId == Constants.D41_6)
                    {
                        if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Corona).ToString())
                        {
                            ContadorCorona = ContadorCorona + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Exodoncia).ToString())
                        {
                            ContadorExodoncia = ContadorExodoncia + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Implante).ToString())
                        {
                            ContadorImplante = ContadorImplante + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.PPR).ToString())
                        {
                            ContadorPPR = ContadorPPR + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.ProtesisTotal).ToString())
                        {
                            ContadorProtesisTotal = ContadorProtesisTotal + 1;
                        }
                    }

                    if (oServiceComponentFieldValuesList[i].v_ComponentFieldId == Constants.D42_6)
                    {
                        if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Corona).ToString())
                        {
                            ContadorCorona = ContadorCorona + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Exodoncia).ToString())
                        {
                            ContadorExodoncia = ContadorExodoncia + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Implante).ToString())
                        {
                            ContadorImplante = ContadorImplante + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.PPR).ToString())
                        {
                            ContadorPPR = ContadorPPR + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.ProtesisTotal).ToString())
                        {
                            ContadorProtesisTotal = ContadorProtesisTotal + 1;
                        }
                    }

                    if (oServiceComponentFieldValuesList[i].v_ComponentFieldId == Constants.D43_6)
                    {
                        if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Corona).ToString())
                        {
                            ContadorCorona = ContadorCorona + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Exodoncia).ToString())
                        {
                            ContadorExodoncia = ContadorExodoncia + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Implante).ToString())
                        {
                            ContadorImplante = ContadorImplante + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.PPR).ToString())
                        {
                            ContadorPPR = ContadorPPR + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.ProtesisTotal).ToString())
                        {
                            ContadorProtesisTotal = ContadorProtesisTotal + 1;
                        }
                    }

                    if (oServiceComponentFieldValuesList[i].v_ComponentFieldId == Constants.D44_6)
                    {
                        if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Corona).ToString())
                        {
                            ContadorCorona = ContadorCorona + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Exodoncia).ToString())
                        {
                            ContadorExodoncia = ContadorExodoncia + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Implante).ToString())
                        {
                            ContadorImplante = ContadorImplante + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.PPR).ToString())
                        {
                            ContadorPPR = ContadorPPR + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.ProtesisTotal).ToString())
                        {
                            ContadorProtesisTotal = ContadorProtesisTotal + 1;
                        }
                    }

                    if (oServiceComponentFieldValuesList[i].v_ComponentFieldId == Constants.D45_6)
                    {
                        if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Corona).ToString())
                        {
                            ContadorCorona = ContadorCorona + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Exodoncia).ToString())
                        {
                            ContadorExodoncia = ContadorExodoncia + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Implante).ToString())
                        {
                            ContadorImplante = ContadorImplante + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.PPR).ToString())
                        {
                            ContadorPPR = ContadorPPR + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.ProtesisTotal).ToString())
                        {
                            ContadorProtesisTotal = ContadorProtesisTotal + 1;
                        }
                    }

                    if (oServiceComponentFieldValuesList[i].v_ComponentFieldId == Constants.D46_6)
                    {
                        if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Corona).ToString())
                        {
                            ContadorCorona = ContadorCorona + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Exodoncia).ToString())
                        {
                            ContadorExodoncia = ContadorExodoncia + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Implante).ToString())
                        {
                            ContadorImplante = ContadorImplante + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.PPR).ToString())
                        {
                            ContadorPPR = ContadorPPR + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.ProtesisTotal).ToString())
                        {
                            ContadorProtesisTotal = ContadorProtesisTotal + 1;
                        }
                    }

                    if (oServiceComponentFieldValuesList[i].v_ComponentFieldId == Constants.D47_6)
                    {
                        if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Corona).ToString())
                        {
                            ContadorCorona = ContadorCorona + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Exodoncia).ToString())
                        {
                            ContadorExodoncia = ContadorExodoncia + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Implante).ToString())
                        {
                            ContadorImplante = ContadorImplante + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.PPR).ToString())
                        {
                            ContadorPPR = ContadorPPR + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.ProtesisTotal).ToString())
                        {
                            ContadorProtesisTotal = ContadorProtesisTotal + 1;
                        }
                    }

                    if (oServiceComponentFieldValuesList[i].v_ComponentFieldId == Constants.D48_6)
                    {
                        if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Corona).ToString())
                        {
                            ContadorCorona = ContadorCorona + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Exodoncia).ToString())
                        {
                            ContadorExodoncia = ContadorExodoncia + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.Implante).ToString())
                        {
                            ContadorImplante = ContadorImplante + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.PPR).ToString())
                        {
                            ContadorPPR = ContadorPPR + 1;
                        }
                        else if (oServiceComponentFieldValuesList[i].v_Value1 == ((int)LeyendaOdontograma.ProtesisTotal).ToString())
                        {
                            ContadorProtesisTotal = ContadorProtesisTotal + 1;
                        }
                    }

                    #endregion
                }

                xxx[0] = ContadorCorona;
                xxx[1] = ContadorExodoncia;
                xxx[2] = ContadorImplante;
                xxx[3] = ContadorPPR;
                xxx[4] = ContadorProtesisTotal;


                return xxx;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public List<Sigesoft.Node.WinClient.BE.ReportRadiologico> ReportRadiologico(string pstrserviceId, string pstrComponentId)
        {
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();


                var objEntity = (from A in dbContext.service
                                 join B in dbContext.person on A.v_PersonId equals B.v_PersonId
                                 join E in dbContext.servicecomponent on new { a = pstrserviceId, b = pstrComponentId }
                                                                        equals new { a = E.v_ServiceId, b = E.v_ComponentId }
                                 // Usuario Medico Evaluador / Medico Aprobador ****************************
                                 join me in dbContext.systemuser on E.i_ApprovedUpdateUserId equals me.i_SystemUserId into me_join
                                 from me in me_join.DefaultIfEmpty()

                                 join pme in dbContext.professional on me.v_PersonId equals pme.v_PersonId into pme_join
                                 from pme in pme_join.DefaultIfEmpty()

                                 // Usuario Tecnologo *************************************
                                 join tec in dbContext.systemuser on E.i_UpdateUserTechnicalDataRegisterId equals tec.i_SystemUserId into tec_join
                                 from tec in tec_join.DefaultIfEmpty()

                                 join ptec in dbContext.professional on tec.v_PersonId equals ptec.v_PersonId into ptec_join
                                 from ptec in ptec_join.DefaultIfEmpty()
                                 // *******************************************************                            

                                 join I in dbContext.protocol on A.v_ProtocolId equals I.v_ProtocolId

                                 join J in dbContext.organization on I.v_EmployerOrganizationId equals J.v_OrganizationId


                                 where A.v_ServiceId == pstrserviceId

                                 select new Sigesoft.Node.WinClient.BE.ReportRadiologico
                                 {
                                     v_ServiceId = A.v_ServiceId,
                                     Paciente = B.v_FirstName + " " + B.v_FirstLastName + " " + B.v_SecondLastName,
                                     ExamenSolicitado = "Radiografia de Torax (P-A)",
                                     Empresa = J.v_Name,
                                     Fecha = A.d_ServiceDate.Value,
                                     FirmaTecnologo = ptec.b_SignatureImage,
                                     FirmaMedicoEva = pme.b_SignatureImage,
                                     d_BirthDate = B.d_Birthdate.Value,

                                 });

                var MedicalCenter = GetInfoMedicalCenter();

                var sql = (from a in objEntity.ToList()
                           select new Sigesoft.Node.WinClient.BE.ReportRadiologico
                           {
                               v_ServiceId = a.v_ServiceId,
                               Paciente = a.Paciente,
                               ExamenSolicitado = a.ExamenSolicitado,
                               Empresa = a.Empresa,
                               Fecha = a.Fecha,
                               FirmaTecnologo = a.FirmaTecnologo,
                               FirmaMedicoEva = a.FirmaMedicoEva,
                               d_BirthDate = a.d_BirthDate,
                               Edad = GetAge(a.d_BirthDate.Value),

                               Vertices = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_VERTICES_ID, "NOCOMBO", 0, "SI"),
                               CamposPulmonares = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_CAMPOS_PULMONARES_ID, "NOCOMBO", 0, "SI"),
                               SenosCosto = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_COSTO_ODIAFRAGMATICO_ID, "NOCOMBO", 0, "SI"),
                               SenosCardio = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_SENOS_CARDIOFRENICOS_DESCRIPCION_ID, "NOCOMBO", 0, "SI"),
                               Mediastinos = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_MEDIASTINOS_DESCRIPCION_ID, "NOCOMBO", 0, "SI"),
                               Silueta = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_SILUETA_CARDIACA_DESCRIPCION_ID, "NOCOMBO", 0, "SI"),
                               Indice = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_INDICE_CARDIACO_DESCRIPCION_ID, "NOCOMBO", 0, "SI"),
                               PartesBlandas = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_PARTES_BLANDAS_OSEAS_ID, "NOCOMBO", 0, "SI"),
                               Conclusiones = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_CONCLUSIONES_RADIOGRAFICAS_DESCRIPCION_ID, "NOCOMBO", 0, "SI"),
                               Hilos = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_HILOS_ID, "NOCOMBO", 0, "SI"),
                               Hallazgos = GetDiagnosticByServiceIdAndComponent(a.v_ServiceId, pstrComponentId),


                               b_Logo = MedicalCenter.b_Image,
                               EmpresaPropietaria = MedicalCenter.v_Name,
                               EmpresaPropietariaDireccion = MedicalCenter.v_Address,
                               EmpresaPropietariaTelefono = MedicalCenter.v_PhoneNumber,
                               EmpresaPropietariaEmail = MedicalCenter.v_Mail,

                           }).ToList();

                return sql;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<Sigesoft.Node.WinClient.BE.ReportInformeRadiografico> ReportInformeRadiografico(string pstrserviceId, string pstrComponentId)
        {
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();


                var objEntity = (from A in dbContext.service
                                 join B in dbContext.person on A.v_PersonId equals B.v_PersonId
                                 join E in dbContext.servicecomponent on new { a = pstrserviceId, b = pstrComponentId }
                                                                        equals new { a = E.v_ServiceId, b = E.v_ComponentId }

                                 //join F in dbContext.systemuser on E.i_UpdateUserId equals F.i_SystemUserId into F_join
                                 //from F in F_join.DefaultIfEmpty()


                                 // Usuario Medico Evaluador / Medico Aprobador ****************************
                                 join me in dbContext.systemuser on E.i_ApprovedUpdateUserId equals me.i_SystemUserId into me_join
                                 from me in me_join.DefaultIfEmpty()

                                 join pme in dbContext.professional on me.v_PersonId equals pme.v_PersonId into pme_join
                                 from pme in pme_join.DefaultIfEmpty()


                                 // Usuario Tecnologo *************************************
                                 join tec in dbContext.systemuser on E.i_UpdateUserTechnicalDataRegisterId equals tec.i_SystemUserId into tec_join
                                 from tec in tec_join.DefaultIfEmpty()

                                 join ptec in dbContext.professional on tec.v_PersonId equals ptec.v_PersonId into ptec_join
                                 from ptec in ptec_join.DefaultIfEmpty()
                                 // *******************************************************      

                                 join me1 in dbContext.person on me.v_PersonId equals me1.v_PersonId


                                 join G in dbContext.professional on new { a = me.v_PersonId }
                                                                      equals new { a = G.v_PersonId } into G_join
                                 from G in G_join.DefaultIfEmpty()

                                 join H in dbContext.person on me.v_PersonId equals H.v_PersonId into H_join
                                 from H in H_join.DefaultIfEmpty()

                                 join I in dbContext.protocol on A.v_ProtocolId equals I.v_ProtocolId into I_join
                                 from I in I_join.DefaultIfEmpty()

                                 join J in dbContext.organization on I.v_EmployerOrganizationId equals J.v_OrganizationId into J_join
                                 from J in J_join.DefaultIfEmpty()

                                 join Z in dbContext.person on me.v_PersonId equals Z.v_PersonId

                                 where A.v_ServiceId == pstrserviceId
                                 select new Sigesoft.Node.WinClient.BE.ReportInformeRadiografico
                                 {
                                     Nombre = B.v_FirstName + " " + B.v_FirstLastName + " " + B.v_SecondLastName,
                                     FechaNacimiento = B.d_Birthdate,
                                     d_ServiceDate = A.d_ServiceDate,
                                     v_ServiceId = A.v_ServiceId,
                                     FirmaMedico = pme.b_SignatureImage,
                                     v_ServiceComponentId = E.v_ServiceComponentId,
                                     Lector = me1.v_FirstName + " " + me1.v_FirstLastName + " " + me1.v_SecondLastName,
                                     Hcl = A.v_ServiceId,
                                     FirmaTecnologo = ptec.b_SignatureImage,
                                     DNI = B.v_DocNumber,
                                     NombreUsuarioGraba = Z.v_FirstLastName + " " + Z.v_SecondLastName + " " + Z.v_FirstName,
                                 });

                var MedicalCenter = GetInfoMedicalCenter();

                var sql = (from a in objEntity.ToList()
                           let Valores = new ServiceBL().ValoresComponente(pstrserviceId, Constants.OIT_ID)
                           select new Sigesoft.Node.WinClient.BE.ReportInformeRadiografico
                           {

                               LOCALIZACION_PERFIL = Valores.Count == 0 || Valores.Find(p => p.v_ComponentFieldId == Constants.LOCALIZACION_PERFIL) == null ? string.Empty : Valores.Find(p => p.v_ComponentFieldId == Constants.LOCALIZACION_PERFIL).v_Value1,
                               LOCALIZACION_FRENTE = Valores.Count == 0 || Valores.Find(p => p.v_ComponentFieldId == Constants.LOCALIZACION_FRENTE) == null ? string.Empty : Valores.Find(p => p.v_ComponentFieldId == Constants.LOCALIZACION_FRENTE).v_Value1,
                               CLACIFICACION_PERFIL = Valores.Count == 0 || Valores.Find(p => p.v_ComponentFieldId == Constants.CLACIFICACION_PERFIL) == null ? string.Empty : Valores.Find(p => p.v_ComponentFieldId == Constants.CLACIFICACION_PERFIL).v_Value1,
                               CLACIFICACION_FRENTE = Valores.Count == 0 || Valores.Find(p => p.v_ComponentFieldId == Constants.CLACIFICACION_FRENTE) == null ? string.Empty : Valores.Find(p => p.v_ComponentFieldId == Constants.CLACIFICACION_FRENTE).v_Value1,
                               EXTENSION_DER_ENGROSAMIENTO = Valores.Count == 0 || Valores.Find(p => p.v_ComponentFieldId == Constants.EXTENSION_DER_ENGROSAMIENTO) == null ? string.Empty : Valores.Find(p => p.v_ComponentFieldId == Constants.EXTENSION_DER_ENGROSAMIENTO).v_Value1,
                               EXTENSION_IZQ_ENGROSAMIENTO = Valores.Count == 0 || Valores.Find(p => p.v_ComponentFieldId == Constants.EXTENSION_IZQ_ENGROSAMIENTO) == null ? string.Empty : Valores.Find(p => p.v_ComponentFieldId == Constants.EXTENSION_IZQ_ENGROSAMIENTO).v_Value1,
                               ANCHURA_DER_ENGROSAMIENTO = Valores.Count == 0 || Valores.Find(p => p.v_ComponentFieldId == Constants.ANCHURA_DER_ENGROSAMIENTO) == null ? string.Empty : Valores.Find(p => p.v_ComponentFieldId == Constants.ANCHURA_DER_ENGROSAMIENTO).v_Value1,
                               ANCHURA_IZQ_ENGROSAMIENTO = Valores.Count == 0 || Valores.Find(p => p.v_ComponentFieldId == Constants.ANCHURA_IZQ_ENGROSAMIENTO) == null ? string.Empty : Valores.Find(p => p.v_ComponentFieldId == Constants.ANCHURA_IZQ_ENGROSAMIENTO).v_Value1,
                               DE_PERFIL = Valores.Count == 0 || Valores.Find(p => p.v_ComponentFieldId == Constants.DE_PERFIL) == null ? string.Empty : Valores.Find(p => p.v_ComponentFieldId == Constants.DE_PERFIL).v_Value1,
                               DE_FRENTE = Valores.Count == 0 || Valores.Find(p => p.v_ComponentFieldId == Constants.DE_FRENTE) == null ? string.Empty : Valores.Find(p => p.v_ComponentFieldId == Constants.DE_FRENTE).v_Value1,
                               DIAFRAGMA = Valores.Count == 0 || Valores.Find(p => p.v_ComponentFieldId == Constants.DIAFRAGMA) == null ? string.Empty : Valores.Find(p => p.v_ComponentFieldId == Constants.DIAFRAGMA).v_Value1,
                               OTROS_SITIOS = Valores.Count == 0 || Valores.Find(p => p.v_ComponentFieldId == Constants.OTROS_SITIOS) == null ? string.Empty : Valores.Find(p => p.v_ComponentFieldId == Constants.OTROS_SITIOS).v_Value1,
                               DE_PERFIL_CLASF = Valores.Count == 0 || Valores.Find(p => p.v_ComponentFieldId == Constants.DE_PERFIL_CLASF) == null ? string.Empty : Valores.Find(p => p.v_ComponentFieldId == Constants.DE_PERFIL_CLASF).v_Value1,
                               DE_FRENTE_CLASF = Valores.Count == 0 || Valores.Find(p => p.v_ComponentFieldId == Constants.DE_FRENTE_CLASF) == null ? string.Empty : Valores.Find(p => p.v_ComponentFieldId == Constants.DE_FRENTE_CLASF).v_Value1,
                               DIAFRAGMA_CLASF = Valores.Count == 0 || Valores.Find(p => p.v_ComponentFieldId == Constants.DIAFRAGMA_CLASF) == null ? string.Empty : Valores.Find(p => p.v_ComponentFieldId == Constants.DIAFRAGMA_CLASF).v_Value1,
                               OTROS_SITIOS_CLASF = Valores.Count == 0 || Valores.Find(p => p.v_ComponentFieldId == Constants.OTROS_SITIOS_CLASF) == null ? string.Empty : Valores.Find(p => p.v_ComponentFieldId == Constants.OTROS_SITIOS_CLASF).v_Value1,
                               EXTENSION_DER_PLACA = Valores.Count == 0 || Valores.Find(p => p.v_ComponentFieldId == Constants.EXTENSION_DER_PLACA) == null ? string.Empty : Valores.Find(p => p.v_ComponentFieldId == Constants.EXTENSION_DER_PLACA).v_Value1,
                               EXTENSION_IZQ_PLACA = Valores.Count == 0 || Valores.Find(p => p.v_ComponentFieldId == Constants.EXTENSION_IZQ_PLACA) == null ? string.Empty : Valores.Find(p => p.v_ComponentFieldId == Constants.EXTENSION_IZQ_PLACA).v_Value1,
                               ANCHURA_DER_PLACA = Valores.Count == 0 || Valores.Find(p => p.v_ComponentFieldId == Constants.ANCHURA_DER_PLACA) == null ? string.Empty : Valores.Find(p => p.v_ComponentFieldId == Constants.ANCHURA_DER_PLACA).v_Value1,
                               ANCHURA_IZQ_PLACA = Valores.Count == 0 || Valores.Find(p => p.v_ComponentFieldId == Constants.ANCHURA_IZQ_PLACA) == null ? string.Empty : Valores.Find(p => p.v_ComponentFieldId == Constants.ANCHURA_IZQ_PLACA).v_Value1,
                               OBLITANG_DER_PLACA = Valores.Count == 0 || Valores.Find(p => p.v_ComponentFieldId == Constants.OBLITANG_DER_PLACA) == null ? string.Empty : Valores.Find(p => p.v_ComponentFieldId == Constants.OBLITANG_DER_PLACA).v_Value1,
                               OBLITANG_IZQ_PLACA = Valores.Count == 0 || Valores.Find(p => p.v_ComponentFieldId == Constants.OBLITANG_IZQ_PLACA) == null ? string.Empty : Valores.Find(p => p.v_ComponentFieldId == Constants.OBLITANG_IZQ_PLACA).v_Value1,


                               Nombre = a.Nombre,
                               DNI = a.DNI,
                               FechaNacimiento = a.FechaNacimiento,
                               d_ServiceDate = a.d_ServiceDate,
                               v_ServiceId = a.v_ServiceId,
                               FirmaMedico = a.FirmaMedico,
                               FirmaTecnologo = a.FirmaTecnologo,
                               v_ServiceComponentId = a.v_ServiceComponentId,
                               Lector = a.Lector,
                               Edad = GetAge(a.FechaNacimiento.Value),
                               Placa = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_NRO_PLACA_ID, "NOCOMBO", 0, "SI"),
                               CalidaRadio = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_CALIDAD_ID, "NOCOMBO", 0, "SI"),
                               Causas = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_CAUSAS_ID, "NOCOMBO", 0, "SI"),
                               Comentario = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_COMENTARIOS_ID, "NOCOMBO", 0, "SI"),
                               Hcl = a.Hcl,
                               FechaLectura = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_FECHA_LECTURA_ID, "NOCOMBO", 0, "SI"),
                               FechaRadiografia = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_FECHA_TOMA_ID, "NOCOMBO", 0, "SI"),

                               SuperiorDerecho = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_SUPERIOR_DERECHO_ID, "NOCOMBO", 0, "SI"),
                               SuperiorIzquierda = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_SUPERIOR_IZQUIERDO_ID, "NOCOMBO", 0, "SI"),
                               MedioDerecho = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_MEDIO_DERECHO_ID, "NOCOMBO", 0, "SI"),
                               MedioIzquierda = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_MEDIO_IZQUIERDO_ID, "NOCOMBO", 0, "SI"),
                               InferiorDerecho = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_INFERIOR_DERECHO_ID, "NOCOMBO", 0, "SI"),
                               InferiorIzquierdo = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_INFERIOR_IZQUIERDO_ID, "NOCOMBO", 0, "SI"),

                               SimboloSi = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_SIMBOLO_SI_ID, "NOCOMBO", 0, "SI"),
                               SimboloNo = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_SIMBOLO_NO_ID, "NOCOMBO", 0, "SI"),

                               CeroNada = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_0_NADA_ID, "NOCOMBO", 0, "SI"),
                               CeroCero = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_0_0_ID, "NOCOMBO", 0, "SI"),
                               CeroUno = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_0_1_ID, "NOCOMBO", 0, "SI"),

                               UnoCero = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_1_0_ID, "NOCOMBO", 0, "SI"),
                               UnoUno = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_1_1_ID, "NOCOMBO", 0, "SI"),
                               UnoDos = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_1_2_ID, "NOCOMBO", 0, "SI"),

                               DosUno = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_2_1_ID, "NOCOMBO", 0, "SI"),
                               DosDos = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_2_2_ID, "NOCOMBO", 0, "SI"),
                               DosTres = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_2_3_ID, "NOCOMBO", 0, "SI"),

                               TresDos = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_3_2_ID, "NOCOMBO", 0, "SI"),
                               TresTres = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_3_3_ID, "NOCOMBO", 0, "SI"),
                               TresMas = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_3_MAS_ID, "NOCOMBO", 0, "SI"),

                               p = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_P_ID, "NOCOMBO", 0, "SI"),
                               q = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_Q_ID, "NOCOMBO", 0, "SI"),
                               r = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_R_ID, "NOCOMBO", 0, "SI"),
                               s = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_S_ID, "NOCOMBO", 0, "SI"),
                               t = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_T_ID, "NOCOMBO", 0, "SI"),
                               u = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_U_ID, "NOCOMBO", 0, "SI"),
                               p1 = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_P1_ID, "NOCOMBO", 0, "SI"),
                               q1 = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_Q1_ID, "NOCOMBO", 0, "SI"),
                               r1 = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_R1_ID, "NOCOMBO", 0, "SI"),
                               s1 = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_S1_ID, "NOCOMBO", 0, "SI"),
                               t1 = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_T1_ID, "NOCOMBO", 0, "SI"),
                               u1 = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_U1_ID, "NOCOMBO", 0, "SI"),


                               O = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_D_ID, "NOCOMBO", 0, "SI"),
                               A = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_A_ID, "NOCOMBO", 0, "SI"),
                               B = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_B_ID, "NOCOMBO", 0, "SI"),
                               C = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_C_ID, "NOCOMBO", 0, "SI"),
                               //SimboloSiNo= GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_SIM, "NOCOMBO", 0, "SI"), 
                               aa = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_AA_ID, "NOCOMBO", 0, "SI"),
                               at = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_AT_ID, "NOCOMBO", 0, "SI"),
                               ax = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_AX_ID, "NOCOMBO", 0, "SI"),
                               bu = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_BU_ID, "NOCOMBO", 0, "SI"),
                               ca = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_CA_ID, "NOCOMBO", 0, "SI"),
                               cg = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_CG_ID, "NOCOMBO", 0, "SI"),
                               cn = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_CN_ID, "NOCOMBO", 0, "SI"),
                               co = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_CO_ID, "NOCOMBO", 0, "SI"),
                               cp = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_CP_ID, "NOCOMBO", 0, "SI"),
                               cv = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_CV_ID, "NOCOMBO", 0, "SI"),



                               di = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_DI_ID, "NOCOMBO", 0, "SI"),
                               ef = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_EF_ID, "NOCOMBO", 0, "SI"),
                               em = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_EM_ID, "NOCOMBO", 0, "SI"),
                               es = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_ES_ID, "NOCOMBO", 0, "SI"),
                               fr = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_FR_ID, "NOCOMBO", 0, "SI"),
                               hi = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_HI_ID, "NOCOMBO", 0, "SI"),
                               ho = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_HO_ID, "NOCOMBO", 0, "SI"),
                               id = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_ID_ID, "NOCOMBO", 0, "SI"),
                               ih = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_IH_ID, "NOCOMBO", 0, "SI"),
                               kl = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_KL_ID, "NOCOMBO", 0, "SI"),
                               me = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_ME_ID, "NOCOMBO", 0, "SI"),
                               od = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_OD_ID, "NOCOMBO", 0, "SI"),
                               pa = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_PA_ID, "NOCOMBO", 0, "SI"),
                               pb = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_PB_ID, "NOCOMBO", 0, "SI"),
                               pi = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_PI_ID, "NOCOMBO", 0, "SI"),
                               px = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_PX_ID, "NOCOMBO", 0, "SI"),
                               ra = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_RA_ID, "NOCOMBO", 0, "SI"),
                               rp = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_RP_ID, "NOCOMBO", 0, "SI"),
                               tb = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_TB_ID, "NOCOMBO", 0, "SI"),
                               Comentario_Od = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_COMENTARIO_OD_ID, "NOCOMBO", 0, "SI"),


                               Conclusiones = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.RX_CONCLUSIONES_OIT_DESCRIPCION_ID, "NOCOMBO", 0, "SI"),
                               Dx = GetDiagnosticByServiceIdAndComponent(a.v_ServiceId, pstrComponentId),

                               b_Logo = MedicalCenter.b_Image,
                               EmpresaPropietaria = MedicalCenter.v_Name,
                               EmpresaPropietariaDireccion = MedicalCenter.v_Address,
                               EmpresaPropietariaTelefono = MedicalCenter.v_PhoneNumber,
                               EmpresaPropietariaEmail = MedicalCenter.v_Mail,
                               NombreUsuarioGraba = a.NombreUsuarioGraba

                           }).ToList();

                return sql;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<Sigesoft.Node.WinClient.BE.ReportTamizajeDermatologico> ReportTamizajeDermatologico(string pstrserviceId, string pstrComponentId)
        {
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();


                var objEntity = (from A in dbContext.service
                                 join B in dbContext.person on A.v_PersonId equals B.v_PersonId
                                 join E in dbContext.servicecomponent on new { a = pstrserviceId, b = pstrComponentId }
                                                                        equals new { a = E.v_ServiceId, b = E.v_ComponentId }

                                 //join F in dbContext.systemuser on E.i_UpdateUserId equals F.i_SystemUserId into F_join
                                 //from F in F_join.DefaultIfEmpty()


                                 // Usuario Medico Evaluador / Medico Aprobador ****************************
                                 join me in dbContext.systemuser on E.i_ApprovedUpdateUserId equals me.i_SystemUserId into me_join
                                 from me in me_join.DefaultIfEmpty()

                                 join pme in dbContext.professional on me.v_PersonId equals pme.v_PersonId into pme_join
                                 from pme in pme_join.DefaultIfEmpty()

                                 join G in dbContext.professional on new { a = me.v_PersonId }
                                                                      equals new { a = G.v_PersonId } into G_join
                                 from G in G_join.DefaultIfEmpty()

                                 join H in dbContext.person on me.v_PersonId equals H.v_PersonId into H_join
                                 from H in H_join.DefaultIfEmpty()

                                 join I in dbContext.protocol on A.v_ProtocolId equals I.v_ProtocolId into I_join
                                 from I in I_join.DefaultIfEmpty()

                                 join J in dbContext.organization on I.v_EmployerOrganizationId equals J.v_OrganizationId into J_join
                                 from J in J_join.DefaultIfEmpty()



                                 where A.v_ServiceId == pstrserviceId
                                 select new Sigesoft.Node.WinClient.BE.ReportTamizajeDermatologico
                                 {
                                     Ficha = A.v_ServiceId,
                                     Hc = E.v_ServiceComponentId,
                                     NombreTrabajador = B.v_FirstName + " " + B.v_FirstLastName + " " + B.v_SecondLastName,
                                     Fecha = A.d_ServiceDate.Value,
                                     FirmaMedico = pme.b_SignatureImage,
                                     FirmaTrabajador = B.b_RubricImage,
                                     HuellaTrabajador = B.b_FingerPrintImage
                                 });


                var MedicalCenter = GetInfoMedicalCenter();

                var sql = (from a in objEntity.ToList()
                           select new Sigesoft.Node.WinClient.BE.ReportTamizajeDermatologico
                           {
                               Ficha = a.Ficha,
                               Hc = a.Hc,
                               NombreTrabajador = a.NombreTrabajador,
                               Fecha = a.Fecha,
                               FirmaMedico = a.FirmaMedico,
                               FirmaTrabajador = a.FirmaTrabajador,
                               HuellaTrabajador = a.HuellaTrabajador,
                               SufreEnfermedadPielSiNo = GetServiceComponentFielValue(pstrserviceId, pstrComponentId, Constants.TAMIZAJE_DERMATOLOGIO_SUFRE_UD_ENFERMEDAD_PIEL_ID, "NOCOMBO", 0, "SI"),
                               SiQueDxTiene = GetServiceComponentFielValue(a.Ficha, pstrComponentId, Constants.TAMIZAJE_DERMATOLOGIO_SI_QUE_DX_TIENE_ID, "NOCOMBO", 0, "SI"),
                               ActualmenteLesionSiNo = GetServiceComponentFielValue(a.Ficha, pstrComponentId, Constants.TAMIZAJE_DERMATOLOGIO_ACTUALMENTE_LESION_ID, "NOCOMBO", 0, "SI"),
                               DondeLocalizaLesion = GetServiceComponentFielValue(a.Ficha, pstrComponentId, Constants.TAMIZAJE_DERMATOLOGIO_SI_DONDE_LOCALIZA_ID, "NOCOMBO", 0, "SI"),
                               CuantoTieneLesion = GetServiceComponentFielValue(a.Ficha, pstrComponentId, Constants.TAMIZAJE_DERMATOLOGIO_CUANTO_TIENE_LESION_ID, "NOCOMBO", 0, "SI"),
                               PresentaColoracionPielSiNo = GetServiceComponentFielValue(a.Ficha, pstrComponentId, Constants.TAMIZAJE_DERMATOLOGIO_PRESENTA_COLORACION_PIEL_ID, "NOCOMBO", 0, "SI"),
                               LesionRepiteVariasAniosSiNo = GetServiceComponentFielValue(a.Ficha, pstrComponentId, Constants.TAMIZAJE_DERMATOLOGIO_LESIONES_REPITE_VARIAS_ANIOS_ID, "NOCOMBO", 0, "SI"),
                               EnrrojecimientoParteCuerpoSiNo = GetServiceComponentFielValue(a.Ficha, pstrComponentId, Constants.TAMIZAJE_DERMATOLOGIO_ENRROJECIMIENTO_PARTE_CUERPO_ID, "NOCOMBO", 0, "SI"),
                               EnrrojecimientoLocaliza = GetServiceComponentFielValue(a.Ficha, pstrComponentId, Constants.TAMIZAJE_DERMATOLOGIO_ENRROJECIMIENTO_SI_DONDE_LOCALIZA_ID, "NOCOMBO", 0, "SI"),
                               TieneComezonSiNo = GetServiceComponentFielValue(a.Ficha, pstrComponentId, Constants.TAMIZAJE_DERMATOLOGIO_TIENE_COMEZON_ID, "NOCOMBO", 0, "SI"),
                               ComezonLocaliza = GetServiceComponentFielValue(a.Ficha, pstrComponentId, Constants.TAMIZAJE_DERMATOLOGIO_COMEZON_LOCALIZA_ID, "NOCOMBO", 0, "SI"),
                               HinchazonParteCuerpoSiNo = GetServiceComponentFielValue(a.Ficha, pstrComponentId, Constants.TAMIZAJE_DERMATOLOGIO_HINCHAZON_PARTE_CUERPO_ID, "NOCOMBO", 0, "SI"),
                               HinchazonParteCuerpoLocaliza = GetServiceComponentFielValue(a.Ficha, pstrComponentId, Constants.TAMIZAJE_DERMATOLOGIO_HINCHAZON_PARTE_CUERPO_DONDE_LOCALIZA_ID, "NOCOMBO", 0, "SI"),
                               AlergiaAsmaSiNo = GetServiceComponentFielValue(a.Ficha, pstrComponentId, Constants.TAMIZAJE_DERMATOLOGIO_ALERGIA_ASMA_ID, "NOCOMBO", 0, "SI"),
                               EppSiNo = GetServiceComponentFielValue(a.Ficha, pstrComponentId, Constants.TAMIZAJE_DERMATOLOGIO_USA_EPP_ID, "NOCOMBO", 0, "SI"),
                               TipoProteccionUsa = GetServiceComponentFielValue(a.Ficha, pstrComponentId, Constants.TAMIZAJE_DERMATOLOGIO_SI_TIPO_PROTECCION_USA_ID, "NOCOMBO", 0, "SI"),
                               CambioUnasSiNo = GetServiceComponentFielValue(a.Ficha, pstrComponentId, Constants.TAMIZAJE_DERMATOLOGIO_PRESENTA_CAMBIO_UNAS_ID, "NOCOMBO", 0, "SI"),
                               TomandoMedicacionSiNo = GetServiceComponentFielValue(a.Ficha, pstrComponentId, Constants.TAMIZAJE_DERMATOLOGIO_TOMANDO_ALGUNA_MEDICACION_ID, "NOCOMBO", 0, "SI"),
                               ComoLlamaMedicacion = GetServiceComponentFielValue(a.Ficha, pstrComponentId, Constants.TAMIZAJE_DERMATOLOGIO_SI_COMO_SE_LLAMA_ID, "NOCOMBO", 0, "SI"),
                               DosisFrecuencia = GetServiceComponentFielValue(a.Ficha, pstrComponentId, Constants.TAMIZAJE_DERMATOLOGIO_DOSIS_FRECUENCIA_ID, "NOCOMBO", 0, "SI"),
                               Descripcion = GetServiceComponentFielValue(a.Ficha, pstrComponentId, Constants.TAMIZAJE_DERMATOLOGIO_DESCRIPCION1_ID, "NOCOMBO", 0, "SI"),
                               DermatopiaSiNo = GetServiceComponentFielValue(a.Ficha, pstrComponentId, Constants.TAMIZAJE_DERMATOLOGIO_DERMATOPIA_ID, "NOCOMBO", 0, "SI"),
                               NikolskySiNo = GetServiceComponentFielValue(a.Ficha, pstrComponentId, Constants.TAMIZAJE_DERMATOLOGIO_DERMATOPIA_ID, "NOCOMBO", 0, "SI"),
                               v_OwnerOrganizationName = (from n in dbContext.organization
                                                          where n.v_OrganizationId == Constants.OWNER_ORGNIZATION_ID
                                                          select n.v_Name).SingleOrDefault<string>(),


                               b_Logo = MedicalCenter.b_Image,
                               EmpresaPropietaria = MedicalCenter.v_Name,
                               EmpresaPropietariaDireccion = MedicalCenter.v_Address,
                               EmpresaPropietariaTelefono = MedicalCenter.v_PhoneNumber,
                               EmpresaPropietariaEmail = MedicalCenter.v_Mail,

                           }).ToList();

                return sql;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<Sigesoft.Node.WinClient.BE.ReportEvaGinecologico> GetReportEvaluacionGinecologico(string pstrserviceId, string pstrComponentId)
        {

            string[] excludeComponents = {   Sigesoft.Common.Constants.GINECOLOGIA_ID,
                                                 Sigesoft.Common.Constants.EXAMEN_MAMA_ID,
                                                 Sigesoft.Common.Constants.ECOGRAFIA_MAMA_ID ,
                                                 Sigesoft.Common.Constants.PAPANICOLAU_ID,
                                                 Sigesoft.Common.Constants.RESULTADOS_MAMOGRAFIA_ID
                                             };
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                var objEntity = (from A in dbContext.service
                                 join B in dbContext.person on A.v_PersonId equals B.v_PersonId

                                 join C in dbContext.protocol on A.v_ProtocolId equals C.v_ProtocolId into C_join
                                 from C in C_join.DefaultIfEmpty()

                                 join D in dbContext.organization on C.v_CustomerOrganizationId equals D.v_OrganizationId into D_join
                                 from D in D_join.DefaultIfEmpty()

                                 join C1 in dbContext.organization on C.v_EmployerOrganizationId equals C1.v_OrganizationId into C1_join
                                 from C1 in C1_join.DefaultIfEmpty()

                                 join M in dbContext.systemparameter on new { a = B.i_TypeOfInsuranceId.Value, b = 188 }
                                     equals new { a = M.i_ParameterId, b = M.i_GroupId } into M_join
                                 from M in M_join.DefaultIfEmpty()

                                 join E in dbContext.servicecomponent on new { a = A.v_ServiceId, b = pstrComponentId }
                                                                        equals new { a = E.v_ServiceId, b = E.v_ComponentId } into E_join
                                 from E in E_join.DefaultIfEmpty()


                                 //join F in dbContext.component on new { a = E.v_ComponentId, b = 12 }
                                 //                                       equals new { a = F.v_ComponentId, b = F.i_CategoryId.Value } into F_join
                                 //from F in F_join.DefaultIfEmpty()

                                 // Usuario Medico Evaluador / Medico Aprobador ****************************
                                 join me in dbContext.systemuser on E.i_ApprovedUpdateUserId equals me.i_SystemUserId into me_join
                                 from me in me_join.DefaultIfEmpty()

                                 join pme in dbContext.professional on me.v_PersonId equals pme.v_PersonId into pme_join
                                 from pme in pme_join.DefaultIfEmpty()

                                 join y in dbContext.person on me.v_PersonId equals y.v_PersonId


                                 join I in dbContext.systemparameter on new { a = A.i_MacId.Value, b = 134 }
                                   equals new { a = I.i_ParameterId, b = I.i_GroupId } into I_join
                                 from I in I_join.DefaultIfEmpty()

                                 join ccc in dbContext.diagnosticrepository on A.v_ServiceId equals ccc.v_ServiceId into ccc_join
                                 from ccc in ccc_join.DefaultIfEmpty()  // ESO

                                 join ddd in dbContext.diseases on ccc.v_DiseasesId equals ddd.v_DiseasesId into ddd_join
                                 from ddd in ddd_join.DefaultIfEmpty()  // Diagnosticos


                                 where A.v_ServiceId == pstrserviceId
                                 select new Sigesoft.Node.WinClient.BE.ReportEvaGinecologico
                                 {
                                     v_DiagnosticRepositoryId = ccc.v_DiagnosticRepositoryId,
                                     Ficha = E.v_ServiceComponentId,
                                     Historia = A.v_ServiceId,
                                     NombreTrabajador = B.v_FirstLastName + " " + B.v_SecondLastName + " " + B.v_FirstName,
                                     FechaNacimiento = B.d_Birthdate.Value,
                                     Seguro = M.v_Value1,
                                     EmpresaCliente = D.v_Name,
                                     EmpresaEmpleadora = C1.v_Name,
                                     Medico = y.v_FirstName + " " + y.v_FirstLastName + " " + y.v_SecondLastName,
                                     FechaAtencion = A.d_ServiceDate.Value,
                                     Fum = A.d_Fur.Value,
                                     Gestapara = A.v_Gestapara,
                                     MAC = I.v_Value1,
                                     Menarquia = A.v_Menarquia,
                                     RegimenCatamenial = A.v_CatemenialRegime,
                                     CirugiaGinecologica = A.v_CiruGine,
                                     FirmaDoctor = pme.b_SignatureImage,
                                     FotoPaciente = B.b_PersonImage,
                                     Diagnosticos = ddd.v_Name,
                                     v_ComponentId = ccc.v_ComponentId

                                 });

                var MedicalCenter = GetInfoMedicalCenter();

                var sql = (from a in objEntity.ToList()
                           let Gine = ValoresComponente(pstrserviceId, Constants.GINECOLOGIA_ID)


                           let ExamMama = ValoresComponente(pstrserviceId, Constants.EXAMEN_MAMA_ID)
                           let LogoEmpresa = GetLogoMedicalCenter()
                           //let ResultadoEcografiaMama = ValoresComponente(pstrserviceId, Constants.ECOGRAFIA_MAMA_ID)
                           let AntePersonales = ValoresComponente(pstrserviceId, Constants.ISTAS_21_ABREVIADA)

                           //let ResultadoPapanicolao = ValoresComponente(pstrserviceId, Constants.PAPANICOLAU_ID)
                           let GineAuxiliares = ValoresComponente(pstrserviceId, Constants.PAPANICOLAU_ID)
                           let ResultadoEcografia = ValoresComponente(pstrserviceId, Constants.RESULTADOS_MAMOGRAFIA_ID)
                           let ResultadoEcografiaMama = ValoresComponente(pstrserviceId, Constants.ECOGRAFIA_MAMA_ID)

                           select new Sigesoft.Node.WinClient.BE.ReportEvaGinecologico
                           {
                               v_ComponentId = a.v_ComponentId,
                               v_DiagnosticRepositoryId = a.v_DiagnosticRepositoryId,
                               Ficha = a.Ficha,
                               Historia = a.Historia,
                               Logo = LogoEmpresa,
                               NombreTrabajador = a.NombreTrabajador,
                               FechaNacimiento = a.FechaNacimiento,
                               Edad = GetAge(a.FechaNacimiento.Value),
                               Seguro = a.Seguro,
                               EmpresaCliente = a.EmpresaCliente,
                               EmpresaEmpleadora = a.EmpresaEmpleadora,
                               CentroMedico = (from n in dbContext.organization
                                               where n.v_OrganizationId == Constants.OWNER_ORGNIZATION_ID
                                               select n.v_Name + ", " + n.v_Address).SingleOrDefault<string>(),
                               Medico = a.Medico,
                               FechaAtencion = a.FechaAtencion,
                               Fum = a.Fum,
                               Gestapara = a.Gestapara,
                               FechaPAP = Gine.Count == 0 ? string.Empty : Gine.Find(p => p.v_ComponentFieldId == Constants.EVALUACION_GINECOLOGICA_ANTECEDENTES_FECHA_ULTIMO_PAP).v_Value1,
                               MAC = a.MAC,
                               Menarquia = a.Menarquia,
                               FechaMamografia = Gine.Count == 0 ? string.Empty : Gine.Find(p => p.v_ComponentFieldId == Constants.EVALUACION_GINECOLOGICA_ANTECEDENTES_FECHA_ULTIMA_MAMOGRAFIA).v_Value1,
                               RegimenCatamenial = a.RegimenCatamenial,
                               CirugiaGinecologica = a.CirugiaGinecologica,

                               Leucorrea = Gine.Count == 0 ? string.Empty : Gine.Find(p => p.v_ComponentFieldId == Constants.GINECOLOGIA_SINTOMAS_LEUCORREA).v_Value1,
                               LeucorreaDescripcion = Gine.Count == 0 ? string.Empty : Gine.Find(p => p.v_ComponentFieldId == Constants.GINECOLOGIA_SINTOMAS_LEUCORREA_COMENTARIO).v_Value1,

                               Dipareunia = Gine.Count == 0 ? string.Empty : Gine.Find(p => p.v_ComponentFieldId == Constants.GINECOLOGIA_SINTOMAS_DISPAREUNIA).v_Value1,
                               DipareuniaDescripcion = Gine.Count == 0 ? string.Empty : Gine.Find(p => p.v_ComponentFieldId == Constants.GINECOLOGIA_SINTOMAS_DISPAREUNIA_COMENTARIO).v_Value1,

                               Incontinencia = Gine.Count == 0 ? string.Empty : Gine.Find(p => p.v_ComponentFieldId == Constants.GINECOLOGIA_SINTOMAS_INCONTINENCIA_URINARIA).v_Value1,
                               IncontinenciaDescripcion = Gine.Count == 0 ? string.Empty : Gine.Find(p => p.v_ComponentFieldId == Constants.GINECOLOGIA_SINTOMAS_INCONTINENCIA_URINARIA_COMENTARIO).v_Value1,

                               Otros = Gine.Count == 0 ? string.Empty : Gine.Find(p => p.v_ComponentFieldId == Constants.GINECOLOGIA_SINTOMAS_OTROS).v_Value1,
                               OtrosDescripcion = Gine.Count == 0 ? string.Empty : Gine.Find(p => p.v_ComponentFieldId == Constants.GINECOLOGIA_SINTOMAS_OTROS_COMENTARIO).v_Value1,

                               EvaluacionGinecologica = Gine.Count == 0 ? string.Empty : Gine.Find(p => p.v_ComponentFieldId == Constants.GINECOLOGIA_HALLAZGOS_HALLAZGOS).v_Value1,

                               ExamenMama = ExamMama.Count == 0 ? string.Empty : ExamMama.Find(p => p.v_ComponentFieldId == Constants.GINECOLOGIA_EX_MAMA_HALLAZGOS_HALLAZGOS).v_Value1.Replace("\n", " "),




                               ResultadoPAP = GineAuxiliares.Count == 0 ? "N/A" : GineAuxiliares.Find(p => p.v_ComponentFieldId == Constants.PAPANICOLAU_HALLAZGOS).v_Value1 == "" ? "" : GineAuxiliares.Find(p => p.v_ComponentFieldId == Constants.PAPANICOLAU_HALLAZGOS).v_Value1,
                               ResultadoMamografia = ResultadoEcografia.Count == 0 ? "N/A" : ResultadoEcografia.Find(p => p.v_ComponentFieldId == Constants.PAPANICOLAU_RADIOGRAFIA_HALLAZGOS).v_Value1 == "" ? "" : ResultadoEcografia.Find(p => p.v_ComponentFieldId == Constants.PAPANICOLAU_RADIOGRAFIA_HALLAZGOS).v_Value1,
                               ResultadoMama = ResultadoEcografiaMama.Count == 0 ? "N/A" : ResultadoEcografiaMama.Find(p => p.v_ComponentFieldId == Constants.RESULTADOS_DE_ECOGRAFIA_HALLAZGOS).v_Value1 == "" ? "" : ResultadoEcografiaMama.Find(p => p.v_ComponentFieldId == Constants.RESULTADOS_DE_ECOGRAFIA_HALLAZGOS).v_Value1,






                               Diagnosticos = a.Diagnosticos,
                               Recomendaciones = ConcatenateRecommendation(a.v_DiagnosticRepositoryId),

                               FotoPaciente = a.FotoPaciente,
                               FirmaDoctor = a.FirmaDoctor,
                               AntecedentesPersonales = Gine.Count == 0 ? string.Empty : Gine.Find(p => p.v_ComponentFieldId == Constants.GINECOLOGIA_ANTECEDENTES_PERSONALES_ANTECEDENTES).v_Value1,

                               AntecendentesFamiliares = Gine.Count == 0 ? string.Empty : Gine.Find(p => p.v_ComponentFieldId == Constants.GINECOLOGIA_ANTECEDENTES_FAMILIARES).v_Value1,


                               b_Logo = MedicalCenter.b_Image,
                               EmpresaPropietaria = MedicalCenter.v_Name,
                               EmpresaPropietariaDireccion = MedicalCenter.v_Address,
                               EmpresaPropietariaTelefono = MedicalCenter.v_PhoneNumber,
                               EmpresaPropietariaEmail = MedicalCenter.v_Mail,

                           }).ToList();


                var otherExams = sql.FindAll(p => excludeComponents.Contains(p.v_ComponentId));


                return otherExams;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public byte[] GetLogoMedicalCenter()
        {
            using (SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel())
            {

                var nameMedicalCenter = (from n in dbContext.organization
                                         where n.v_OrganizationId == Constants.OWNER_ORGNIZATION_ID
                                         select n.b_Image).SingleOrDefault();

                return nameMedicalCenter;
            }
        }


        public List<Sigesoft.Node.WinClient.BE.ReportCuestionarioEspirometria> GetReportCuestionarioEspirometria(string pstrserviceId, string pstrComponentId)
        {
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                var objEntity = (from A in dbContext.service

                                 join B in dbContext.servicecomponent on new { a = A.v_ServiceId, b = pstrComponentId }
                                                                     equals new { a = B.v_ServiceId, b = B.v_ComponentId } into B_join
                                 from B in B_join.DefaultIfEmpty()


                                 join C in dbContext.protocol on A.v_ProtocolId equals C.v_ProtocolId into C_join
                                 from C in C_join.DefaultIfEmpty()

                                 join D in dbContext.organization on C.v_EmployerOrganizationId equals D.v_OrganizationId into D_join
                                 from D in D_join.DefaultIfEmpty()

                                 join E in dbContext.person on A.v_PersonId equals E.v_PersonId into E_join
                                 from E in E_join.DefaultIfEmpty()

                                 join SP1 in dbContext.systemparameter on new { a = D.i_SectorTypeId.Value, b = 104 }
                                        equals new { a = SP1.i_ParameterId, b = SP1.i_GroupId } into SP1_join
                                 from SP1 in SP1_join.DefaultIfEmpty()


                                 where A.v_ServiceId == pstrserviceId
                                 select new Sigesoft.Node.WinClient.BE.ReportCuestionarioEspirometria
                                 {
                                     IdServicio = A.v_ServiceId,
                                     IdComponent = B.v_ServiceComponentId,
                                     Fecha = A.d_ServiceDate.Value,
                                     NombreTrabajador = E.v_FirstName + " " + E.v_FirstLastName + " " + E.v_SecondLastName,
                                     FechaNacimineto = E.d_Birthdate,
                                     Genero = E.i_SexTypeId.Value,
                                     FirmaTrabajador = E.b_RubricImage,
                                     HuellaTrabajador = E.b_FingerPrintImage


                                 });

                var MedicalCenter = GetInfoMedicalCenter();

                var sql = (from a in objEntity.ToList()

                           let Espirometria = ValoresComponente(pstrserviceId, Constants.ESPIROMETRIA_ID)
                           let age = GetAge(a.FechaNacimineto.Value)
                           let LogoEmpresa = GetLogoMedicalCenter()
                           select new Sigesoft.Node.WinClient.BE.ReportCuestionarioEspirometria
                           {
                               IdServicio = a.IdServicio,
                               IdComponent = a.IdComponent,
                               Fecha = a.Fecha,
                               NombreTrabajador = a.NombreTrabajador,
                               FechaNacimineto = a.FechaNacimineto,
                               Edad = age,
                               Logo = LogoEmpresa,
                               Pregunta1ASiNo = Espirometria.Count == 0 ? string.Empty : Espirometria.Find(p => p.v_ComponentFieldId == Constants.ESPIROMETRIA_CUESTIONARIO_DE_EXCLUSION_1).v_Value1,
                               Pregunta2ASiNo = Espirometria.Count == 0 ? string.Empty : Espirometria.Find(p => p.v_ComponentFieldId == Constants.ESPIROMETRIA_CUESTIONARIO_DE_EXCLUSION_2).v_Value1,
                               Pregunta3ASiNo = Espirometria.Count == 0 ? string.Empty : Espirometria.Find(p => p.v_ComponentFieldId == Constants.ESPIROMETRIA_CUESTIONARIO_DE_EXCLUSION_3).v_Value1,
                               Pregunta4ASiNo = Espirometria.Count == 0 ? string.Empty : Espirometria.Find(p => p.v_ComponentFieldId == Constants.ESPIROMETRIA_CUESTIONARIO_DE_EXCLUSION_4).v_Value1,
                               Pregunta5ASiNo = Espirometria.Count == 0 ? string.Empty : Espirometria.Find(p => p.v_ComponentFieldId == Constants.ESPIROMETRIA_CUESTIONARIO_DE_EXCLUSION_5).v_Value1,

                               HemoptisisSiNo = Espirometria.Count == 0 ? string.Empty : Espirometria.Find(p => p.v_ComponentFieldId == Constants.PIROMETRIA_ANTECEDENTES_HEMOSTISIS).v_Value1,
                               PseumotoraxSiNo = Espirometria.Count == 0 ? string.Empty : Espirometria.Find(p => p.v_ComponentFieldId == Constants.ESPIROMETRIA_ANTECEDENTES_PNEUMOTORAX).v_Value1,
                               TraqueostomiaSiNo = Espirometria.Count == 0 ? string.Empty : Espirometria.Find(p => p.v_ComponentFieldId == Constants.ESPIROMETRIA_ANTECEDENTES_TRAQUEOSTOMIA).v_Value1,
                               SondaPleuralSiNo = Espirometria.Count == 0 ? string.Empty : Espirometria.Find(p => p.v_ComponentFieldId == Constants.ESPIROMETRIA_ANTECEDENTES_SONDA_PLEURAL).v_Value1,
                               AneurismaSiNo = Espirometria.Count == 0 ? string.Empty : Espirometria.Find(p => p.v_ComponentFieldId == Constants.ESPIROMETRIA_ANTECEDENTES_ANEURISMA_CEREBRAL).v_Value1,
                               EmboliaSiNo = Espirometria.Count == 0 ? string.Empty : Espirometria.Find(p => p.v_ComponentFieldId == Constants.ESPIROMETRIA_ANTECEDENTES_EMBOLIA_PULMONAR).v_Value1,
                               InfartoSiNo = Espirometria.Count == 0 ? string.Empty : Espirometria.Find(p => p.v_ComponentFieldId == Constants.ESPIROMETRIA_ANTECEDENTES_INFARTO_RECIENTE).v_Value1,
                               InestabilidadSiNo = Espirometria.Count == 0 ? string.Empty : Espirometria.Find(p => p.v_ComponentFieldId == Constants.ESPIROMETRIA_ANTECEDENTES_INESTABILIDAD_CV).v_Value1,
                               FiebreSiNo = Espirometria.Count == 0 ? string.Empty : Espirometria.Find(p => p.v_ComponentFieldId == Constants.ESPIROMETRIA_ANTECEDENTES_FIEBRE_NAUSEAS).v_Value1,
                               EmbarazoAvanzadoSiNo = Espirometria.Count == 0 ? string.Empty : Espirometria.Find(p => p.v_ComponentFieldId == Constants.ESPIROMETRIA_ANTECEDENTES_EMBARAZO_AVANZADO).v_Value1,
                               EmbarazoComplicadoSiNo = Espirometria.Count == 0 ? string.Empty : Espirometria.Find(p => p.v_ComponentFieldId == Constants.ESPIROMETRIA_ANTECEDENTES_EMBARAZO_COMPLICADO).v_Value1,

                               Pregunta1BSiNo = Espirometria.Count == 0 ? string.Empty : Espirometria.Find(p => p.v_ComponentFieldId == Constants.ESESPIROMETRIA_CUESTIONARIO_PARA_1).v_Value1,
                               Pregunta2BSiNo = Espirometria.Count == 0 ? string.Empty : Espirometria.Find(p => p.v_ComponentFieldId == Constants.ESPIROMETRIA_CUESTIONARIO_PARA_2).v_Value1,
                               Pregunta3BSiNo = Espirometria.Count == 0 ? string.Empty : Espirometria.Find(p => p.v_ComponentFieldId == Constants.ESPIROMETRIA_CUESTIONARIO_PARA_3).v_Value1,
                               Pregunta4BSiNo = Espirometria.Count == 0 ? string.Empty : Espirometria.Find(p => p.v_ComponentFieldId == Constants.ESPIROMETRIA_CUESTIONARIO_PARA_4).v_Value1,
                               Pregunta5BSiNo = Espirometria.Count == 0 ? string.Empty : Espirometria.Find(p => p.v_ComponentFieldId == Constants.ESPIROMETRIA_CUESTIONARIO_PARA_5).v_Value1,
                               Pregunta6BSiNo = Espirometria.Count == 0 ? string.Empty : Espirometria.Find(p => p.v_ComponentFieldId == Constants.ESPIROMETRIA_CUESTIONARIO_PARA_6).v_Value1,
                               Pregunta7BSiNo = Espirometria.Count == 0 ? string.Empty : Espirometria.Find(p => p.v_ComponentFieldId == Constants.ESPIROMETRIA_CUESTIONARIO_PARA_7).v_Value1,

                               Genero = a.Genero,
                               FirmaTrabajador = a.FirmaTrabajador,
                               HuellaTrabajador = a.HuellaTrabajador,

                               b_Logo = MedicalCenter.b_Image,
                               EmpresaPropietaria = MedicalCenter.v_Name,
                               EmpresaPropietariaDireccion = MedicalCenter.v_Address,
                               EmpresaPropietariaTelefono = MedicalCenter.v_PhoneNumber,
                               EmpresaPropietariaEmail = MedicalCenter.v_Mail,

                           }).ToList();

                return sql;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<Sigesoft.Node.WinClient.BE.ReportInformeEspirometria> GetReportInformeEspirometria(string pstrserviceId, string pstrComponentId)
        {
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                var objEntity = (from A in dbContext.service

                                 join B in dbContext.servicecomponent on new { a = A.v_ServiceId, b = pstrComponentId }
                                                                     equals new { a = B.v_ServiceId, b = B.v_ComponentId } into B_join
                                 from B in B_join.DefaultIfEmpty()


                                 join C in dbContext.protocol on A.v_ProtocolId equals C.v_ProtocolId into C_join
                                 from C in C_join.DefaultIfEmpty()

                                 join D in dbContext.organization on C.v_EmployerOrganizationId equals D.v_OrganizationId into D_join
                                 from D in D_join.DefaultIfEmpty()

                                 join E in dbContext.person on A.v_PersonId equals E.v_PersonId into E_join
                                 from E in E_join.DefaultIfEmpty()

                                 join SP1 in dbContext.datahierarchy on new { a = D.i_SectorTypeId.Value, b = 104 }
                                        equals new { a = SP1.i_ItemId, b = SP1.i_GroupId } into SP1_join
                                 from SP1 in SP1_join.DefaultIfEmpty()

                                 // Usuario Medico Evaluador / Medico Aprobador ****************************
                                 join me in dbContext.systemuser on B.i_ApprovedUpdateUserId equals me.i_SystemUserId into me_join
                                 from me in me_join.DefaultIfEmpty()

                                 join pme in dbContext.professional on me.v_PersonId equals pme.v_PersonId into pme_join
                                 from pme in pme_join.DefaultIfEmpty()

                                 // Usuario Tecnologo *************************************
                                 join tec in dbContext.systemuser on B.i_UpdateUserTechnicalDataRegisterId equals tec.i_SystemUserId into tec_join
                                 from tec in tec_join.DefaultIfEmpty()

                                 join ptec in dbContext.professional on tec.v_PersonId equals ptec.v_PersonId into ptec_join
                                 from ptec in ptec_join.DefaultIfEmpty()
                                 // *******************************************************  

                                 where A.v_ServiceId == pstrserviceId
                                 select new Sigesoft.Node.WinClient.BE.ReportInformeEspirometria
                                 {
                                     EspirometriaNro = B.v_ServiceComponentId,
                                     Fecha = A.d_ServiceDate.Value,
                                     HCL = A.v_ServiceId,
                                     i_TipoEvaluacion = C.i_MasterServiceTypeId.Value,
                                     RazonSocial = D.v_Name,
                                     ActividadEconomica = SP1.v_Value1,
                                     PuestoTrabajo = E.v_CurrentOccupation,
                                     NombreTrabajador = E.v_FirstName + " " + E.v_FirstLastName + " " + E.v_SecondLastName,
                                     FechaNacimiento = E.d_Birthdate.Value,
                                     i_Sexo = E.i_SexTypeId.Value,
                                     FirmaRealizaEspirometria = ptec.b_SignatureImage,
                                     FirmaMedicoInterpreta = pme.b_SignatureImage
                                 });

                var MedicalCenter = GetInfoMedicalCenter();

                var sql = (from a in objEntity.ToList()
                           let nameMedicalCenter = GetNameMedicalCenter()
                           let Espirometria = ValoresComponente(pstrserviceId, Constants.ESPIROMETRIA_ID)
                           let age = GetAge(a.FechaNacimiento.Value)
                           let Antropometria = ValoresComponente(pstrserviceId, Constants.ANTROPOMETRIA_ID)

                           let DxEspirometria = GetDiagnosticByServiceIdAndComponent(pstrserviceId, Constants.ESPIROMETRIA_ID)
                           select new Sigesoft.Node.WinClient.BE.ReportInformeEspirometria
                           {
                               Logo = MedicalCenter.b_Image,
                               EspirometriaNro = a.EspirometriaNro,
                               Fecha = a.Fecha,
                               HCL = a.HCL,
                               TipoEvaluacion = a.i_TipoEvaluacion.ToString(),
                               LugarExamen = nameMedicalCenter,
                               RazonSocial = a.RazonSocial,
                               ActividadEconomica = a.ActividadEconomica,
                               PuestoTrabajo = a.PuestoTrabajo,
                               TiempoTrabajo = Espirometria.Count == 0 ? string.Empty : Espirometria.Find(p => p.v_ComponentFieldId == Constants.ESPIROMETRIA_TIEMPO_TRABAJO_ID).v_Value1,
                               NombreTrabajador = a.NombreTrabajador,
                               Edad = age,
                               Sexo = a.i_Sexo.ToString(),
                               Talla = Antropometria.Count == 0 ? string.Empty : Antropometria.Find(p => p.v_ComponentFieldId == Constants.ANTROPOMETRIA_TALLA_ID).v_Value1,
                               Peso = Antropometria.Count == 0 ? string.Empty : Antropometria.Find(p => p.v_ComponentFieldId == Constants.ANTROPOMETRIA_PESO_ID).v_Value1,
                               OrigenEtnico = Espirometria.Count == 0 ? string.Empty : Espirometria.Find(p => p.v_ComponentFieldId == Constants.ESPIROMETRIA_ORIGEN_ETNICO).v_Value1,
                               FumadorSiNo = Espirometria.Count == 0 ? string.Empty : Espirometria.Find(p => p.v_ComponentFieldId == Constants.ESPIROMETRIA_TABAQUISMO).v_Value1,
                               CVF = Espirometria.Count == 0 ? string.Empty : Espirometria.Find(p => p.v_ComponentFieldId == Constants.ESPIROMETRIA_FUNCION_RESPIRATORIA_ABS_CVF).v_Value1,
                               VEF1 = Espirometria.Count == 0 ? string.Empty : Espirometria.Find(p => p.v_ComponentFieldId == Constants.ESPIROMETRIA_FUNCION_RESPIRATORIA_ABS_VEF_1).v_Value1,
                               VEF1CVF = Espirometria.Count == 0 ? string.Empty : Espirometria.Find(p => p.v_ComponentFieldId == Constants.ESPIROMETRIA_FUNCION_RESPIRATORIA_ABS_VEF_1_CVF).v_Value1,
                               FET = Espirometria.Count == 0 ? string.Empty : Espirometria.Find(p => p.v_ComponentFieldId == Constants.ESPIROMETRIA_FUNCION_RESPIRATORIA_FET).v_Value1,
                               FEV2575 = Espirometria.Count == 0 ? string.Empty : Espirometria.Find(p => p.v_ComponentFieldId == Constants.ESPIROMETRIA_FUNCION_RESPIRATORIA_ABS_FEF_25_75).v_Value1,
                               PEF = Espirometria.Count == 0 ? string.Empty : Espirometria.Find(p => p.v_ComponentFieldId == Constants.ESPIROMETRIA_FUNCION_RESPIRATORIA_PEF).v_Value1,
                               CVFDes = Espirometria.Count == 0 ? string.Empty : Espirometria.Find(p => p.v_ComponentFieldId == Constants.ESPIROMETRIA_FUNCION_RESPIRATORIA_DESCRIPCION_CVF).v_Value1,
                               VEF1Des = Espirometria.Count == 0 ? string.Empty : Espirometria.Find(p => p.v_ComponentFieldId == Constants.ESPIROMETRIA_FUNCION_RESPIRATORIA_DESCRIPCION_VEF_1).v_Value1,
                               VEF1CVFDes = Espirometria.Count == 0 ? string.Empty : Espirometria.Find(p => p.v_ComponentFieldId == Constants.ESPIROMETRIA_FUNCION_RESPIRATORIA_DESCRIPCION_VEF_1_CVF).v_Value1,
                               FETDes = Espirometria.Count == 0 ? string.Empty : Espirometria.Find(p => p.v_ComponentFieldId == Constants.ESPIROMETRIA_FUNCION_RESPIRATORIA_DESCRIPCION_FET).v_Value1,
                               FEV2575Des = Espirometria.Count == 0 ? string.Empty : Espirometria.Find(p => p.v_ComponentFieldId == Constants.ESPIROMETRIA_FUNCION_RESPIRATORIA_DESCRIPCION_F_25_75).v_Value1,
                               PEFDes = Espirometria.Count == 0 ? string.Empty : Espirometria.Find(p => p.v_ComponentFieldId == Constants.ESPIROMETRIA_FUNCION_RESPIRATORIA_DESCRIPCION_PEF).v_Value1,
                               EdadPulmonar = Espirometria.Count == 0 ? string.Empty : Espirometria.Find(p => p.v_ComponentFieldId == Constants.ESPIROMETRIA_FUNCION_RESPIRATORIA_EDAD_PULMONAR_ESTIMADA).v_Value1,
                               Resultado = Espirometria.Count == 0 ? string.Empty : Espirometria.Find(p => p.v_ComponentFieldId == Constants.ESPIROMETRIA_RESULTADO_ID).v_Value1,
                               Observacion = Espirometria.Count == 0 ? string.Empty : Espirometria.Find(p => p.v_ComponentFieldId == Constants.ESPIROMETRIA_OBSERVACION_ID).v_Value1,
                               FirmaRealizaEspirometria = a.FirmaRealizaEspirometria,
                               FirmaMedicoInterpreta = a.FirmaMedicoInterpreta,
                               Dx = DxEspirometria,

                               b_Logo = MedicalCenter.b_Image,
                               EmpresaPropietaria = MedicalCenter.v_Name,
                               EmpresaPropietariaDireccion = MedicalCenter.v_Address,
                               EmpresaPropietariaTelefono = MedicalCenter.v_PhoneNumber,
                               EmpresaPropietariaEmail = MedicalCenter.v_Mail,

                           }).ToList();

                return sql;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public List<Sigesoft.Node.WinClient.BE.ReportOsteo> GetReportOsteo(string pstrserviceId, string pstrComponentId)
        {
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                var objEntity = (from A in dbContext.service
                                 join B in dbContext.person on A.v_PersonId equals B.v_PersonId

                                 join C in dbContext.protocol on A.v_ProtocolId equals C.v_ProtocolId into C_join
                                 from C in C_join.DefaultIfEmpty()

                                 join D in dbContext.organization on C.v_CustomerOrganizationId equals D.v_OrganizationId into D_join
                                 from D in D_join.DefaultIfEmpty()

                                 join C1 in dbContext.organization on C.v_EmployerOrganizationId equals C1.v_OrganizationId into C1_join
                                 from C1 in C1_join.DefaultIfEmpty()

                                 join E in dbContext.servicecomponent on new { a = A.v_ServiceId, b = pstrComponentId }
                                                                        equals new { a = E.v_ServiceId, b = E.v_ComponentId } into E_join
                                 from E in E_join.DefaultIfEmpty()

                                 join J in dbContext.systemparameter on new { a = B.i_SexTypeId.Value, b = 100 }
                                                             equals new { a = J.i_ParameterId, b = J.i_GroupId } into J_join // GENERO
                                 from J in J_join.DefaultIfEmpty()


                                 // Usuario Medico Evaluador / Medico Aprobador ****************************
                                 join me in dbContext.systemuser on E.i_ApprovedUpdateUserId equals me.i_SystemUserId into me_join
                                 from me in me_join.DefaultIfEmpty()

                                 join pme in dbContext.professional on me.v_PersonId equals pme.v_PersonId into pme_join
                                 from pme in pme_join.DefaultIfEmpty()

                                 join y in dbContext.person on me.v_PersonId equals y.v_PersonId

                                 where A.v_ServiceId == pstrserviceId
                                 select new Sigesoft.Node.WinClient.BE.ReportOsteo
                                 {
                                     ServiceId = A.v_ServiceId,
                                     HuellaTrabajador = B.b_FingerPrintImage,
                                     FirmaTrabajador = B.b_RubricImage,
                                     FirmaMedico = pme.b_SignatureImage

                                 });

                var MedicalCenter = GetInfoMedicalCenter();

                var sql = (from a in objEntity.ToList()
                           let Osteo = ValoresComponente(pstrserviceId, Constants.OSTEO_MUSCULAR_ID_1)


                           select new Sigesoft.Node.WinClient.BE.ReportOsteo
                           {

                               ServiceId = a.ServiceId,
                               HuellaTrabajador = a.HuellaTrabajador,
                               FirmaTrabajador = a.FirmaTrabajador,
                               FirmaMedico = a.FirmaMedico,

                               TareasHorasDias = Osteo.Count == 0 ? string.Empty : Osteo.Find(p => p.v_ComponentFieldId == Constants.MANIPULACION_JALAR_CARGAS).v_Value1,
                               TareasFrecuencia = Osteo.Count == 0 ? string.Empty : Osteo.Find(p => p.v_ComponentFieldId == Constants.MANIPULACION_LEVANTAR_CARGAS_DESCRIPCION).v_Value1,
                               TareasHorasSemana = Osteo.Count == 0 ? string.Empty : Osteo.Find(p => p.v_ComponentFieldId == Constants.MANIPULACION_EMPUJAR_CARGA_DESCRIPCION).v_Value1,
                               TareasTipo = Osteo.Count == 0 ? string.Empty : Osteo.Find(p => p.v_ComponentFieldId == Constants.PESOS_SUPERIORES_A_25KG).v_Value1,
                               TareasCiclo = Osteo.Count == 0 ? string.Empty : Osteo.Find(p => p.v_ComponentFieldId == Constants.MANIPULACION_LEVANTAR_CARGAS).v_Value1,
                               TareasCarga = Osteo.Count == 0 ? string.Empty : Osteo.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_TAREAS_CARGA).v_Value1,
                               LateralCervical = Osteo.Count == 0 ? string.Empty : Osteo.Find(p => p.v_ComponentFieldId == Constants.LEVANTAMIENTO_POR_ENCIMA_DELHOMBRODESCRIPCION).v_Value1,
                               LateralLumbar = Osteo.Count == 0 ? string.Empty : Osteo.Find(p => p.v_ComponentFieldId == Constants.MANIPULACION_DE_VALVULAS_DESCRIPCION).v_Value1,
                               LateralDorsal = Osteo.Count == 0 ? string.Empty : Osteo.Find(p => p.v_ComponentFieldId == Constants.MANIPULACION_DE_VALVULAS_).v_Value1,
                               LordosisCervical = Osteo.Count == 0 ? string.Empty : Osteo.Find(p => p.v_ComponentFieldId == Constants.POSTURA_FORZADA__).v_Value1,
                               LordosisLumbar = Osteo.Count == 0 ? string.Empty : Osteo.Find(p => p.v_ComponentFieldId == Constants.POSTURA_SEDENTARIA__).v_Value1,
                               EscoliosisLumbar = Osteo.Count == 0 ? string.Empty : Osteo.Find(p => p.v_ComponentFieldId == Constants.POSTURA_FORZADA_DESCRIPCION_).v_Value1,
                               ContracturaMuscular = Osteo.Count == 0 ? string.Empty : Osteo.Find(p => p.v_ComponentFieldId == Constants.SINTOMAS).v_Value1,
                               DolorEspalda = Osteo.Count == 0 ? string.Empty : Osteo.Find(p => p.v_ComponentFieldId == Constants.MOVIMIENTOS_REPETITIVOS__).v_Value1,
                               ConclusionDescripcion = Osteo.Count == 0 ? string.Empty : Osteo.Find(p => p.v_ComponentFieldId == Constants.DESCRIPCION).v_Value1,
                               Dx = GetDiagnosticByServiceIdAndComponent(pstrserviceId, Constants.OSTEO_MUSCULAR_ID_1),
                               Aptitud = Osteo.Count == 0 ? string.Empty : Osteo.Find(p => p.v_ComponentFieldId == Constants.APTITUD).v_Value1,
                               Recomendaciones = GetRecommendationByServiceIdAndComponent(a.ServiceId, Constants.OSTEO_MUSCULAR_ID_1),

                               b_Logo = MedicalCenter.b_Image,
                               EmpresaPropietaria = MedicalCenter.v_Name,
                               EmpresaPropietariaDireccion = MedicalCenter.v_Address,
                               EmpresaPropietariaTelefono = MedicalCenter.v_PhoneNumber,
                               EmpresaPropietariaEmail = MedicalCenter.v_Mail,

                           }).ToList();


                return sql;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<Sigesoft.Node.WinClient.BE.ReportEvaluacionPsicolaboralPersonal> GetReportEvaluacionPsicolaborlaPersonal(string pstrserviceId, string pstrComponentId)
        {
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                var objEntity = (from A in dbContext.service
                                 join B in dbContext.person on A.v_PersonId equals B.v_PersonId

                                 join C in dbContext.protocol on A.v_ProtocolId equals C.v_ProtocolId into C_join
                                 from C in C_join.DefaultIfEmpty()

                                 join D in dbContext.organization on C.v_CustomerOrganizationId equals D.v_OrganizationId into D_join
                                 from D in D_join.DefaultIfEmpty()

                                 join C1 in dbContext.organization on C.v_EmployerOrganizationId equals C1.v_OrganizationId into C1_join
                                 from C1 in C1_join.DefaultIfEmpty()

                                 join E in dbContext.servicecomponent on new { a = A.v_ServiceId, b = pstrComponentId }
                                                                        equals new { a = E.v_ServiceId, b = E.v_ComponentId } into E_join
                                 from E in E_join.DefaultIfEmpty()

                                 join J in dbContext.systemparameter on new { a = B.i_SexTypeId.Value, b = 100 }
                                                             equals new { a = J.i_ParameterId, b = J.i_GroupId } into J_join // GENERO
                                 from J in J_join.DefaultIfEmpty()


                                 // Usuario Medico Evaluador / Medico Aprobador ****************************
                                 join me in dbContext.systemuser on E.i_ApprovedUpdateUserId equals me.i_SystemUserId into me_join
                                 from me in me_join.DefaultIfEmpty()

                                 join pme in dbContext.professional on me.v_PersonId equals pme.v_PersonId into pme_join
                                 from pme in pme_join.DefaultIfEmpty()

                                 join y in dbContext.person on me.v_PersonId equals y.v_PersonId



                                 where A.v_ServiceId == pstrserviceId
                                 select new Sigesoft.Node.WinClient.BE.ReportEvaluacionPsicolaboralPersonal
                                 {
                                     Trabajador = B.v_FirstLastName + " " + B.v_SecondLastName + " " + B.v_FirstName,
                                     Genero = J.v_Value1,
                                     FechaNacimiento = B.d_Birthdate.Value,
                                     PuestoPostula = B.v_CurrentOccupation,
                                     EmpresaCliente = D.v_Name,
                                     FechaEvaluacion = A.d_ServiceDate.Value,
                                     Evaluador = y.v_FirstName + " " + y.v_FirstLastName + " " + y.v_SecondLastName,
                                     Cpsp = pme.v_ProfessionalCode,
                                     FirmaTrabajador = B.b_RubricImage,
                                     FirmaProfesional = pme.b_SignatureImage


                                 });

                var MedicalCenter = GetInfoMedicalCenter();

                var sql = (from a in objEntity.ToList()
                           let EvaPsicolaborla = ValoresComponente(pstrserviceId, Constants.EVALUACION_PSICOLABORAL)
                           select new Sigesoft.Node.WinClient.BE.ReportEvaluacionPsicolaboralPersonal
                           {

                               Trabajador = a.Trabajador,
                               Genero = a.Genero,
                               FechaNacimiento = a.FechaNacimiento,
                               Edad = GetAge(a.FechaNacimiento),
                               PuestoPostula = a.PuestoPostula,
                               EmpresaCliente = a.EmpresaCliente,
                               FechaEvaluacion = a.FechaEvaluacion,
                               Cs = (from n in dbContext.organization
                                     where n.v_OrganizationId == Constants.OWNER_ORGNIZATION_ID
                                     select n.v_Name).SingleOrDefault<string>(),
                               Evaluador = a.Evaluador,
                               Cpsp = a.Cpsp,
                               FirmaTrabajador = a.FirmaTrabajador,
                               FirmaProfesional = a.FirmaProfesional,

                               _1 = EvaPsicolaborla.Count == 0 ? string.Empty : EvaPsicolaborla.Find(p => p.v_ComponentFieldId == Constants._1).v_Value1,
                               _2 = EvaPsicolaborla.Count == 0 ? string.Empty : EvaPsicolaborla.Find(p => p.v_ComponentFieldId == Constants._2).v_Value1,
                               _3 = EvaPsicolaborla.Count == 0 ? string.Empty : EvaPsicolaborla.Find(p => p.v_ComponentFieldId == Constants._3).v_Value1,
                               _4 = EvaPsicolaborla.Count == 0 ? string.Empty : EvaPsicolaborla.Find(p => p.v_ComponentFieldId == Constants._4).v_Value1,
                               _5 = EvaPsicolaborla.Count == 0 ? string.Empty : EvaPsicolaborla.Find(p => p.v_ComponentFieldId == Constants._5).v_Value1,
                               _6 = EvaPsicolaborla.Count == 0 ? string.Empty : EvaPsicolaborla.Find(p => p.v_ComponentFieldId == Constants._6).v_Value1,
                               _7 = EvaPsicolaborla.Count == 0 ? string.Empty : EvaPsicolaborla.Find(p => p.v_ComponentFieldId == Constants._7).v_Value1,
                               _8 = EvaPsicolaborla.Count == 0 ? string.Empty : EvaPsicolaborla.Find(p => p.v_ComponentFieldId == Constants._8).v_Value1,
                               _9 = EvaPsicolaborla.Count == 0 ? string.Empty : EvaPsicolaborla.Find(p => p.v_ComponentFieldId == Constants._9).v_Value1,
                               _10 = EvaPsicolaborla.Count == 0 ? string.Empty : EvaPsicolaborla.Find(p => p.v_ComponentFieldId == Constants._10).v_Value1,
                               _11 = EvaPsicolaborla.Count == 0 ? string.Empty : EvaPsicolaborla.Find(p => p.v_ComponentFieldId == Constants._11).v_Value1,
                               _12 = EvaPsicolaborla.Count == 0 ? string.Empty : EvaPsicolaborla.Find(p => p.v_ComponentFieldId == Constants._12).v_Value1,
                               _13 = EvaPsicolaborla.Count == 0 ? string.Empty : EvaPsicolaborla.Find(p => p.v_ComponentFieldId == Constants._13).v_Value1,
                               _14 = EvaPsicolaborla.Count == 0 ? string.Empty : EvaPsicolaborla.Find(p => p.v_ComponentFieldId == Constants._14).v_Value1,
                               _15 = EvaPsicolaborla.Count == 0 ? string.Empty : EvaPsicolaborla.Find(p => p.v_ComponentFieldId == Constants._15).v_Value1,
                               _16 = EvaPsicolaborla.Count == 0 ? string.Empty : EvaPsicolaborla.Find(p => p.v_ComponentFieldId == Constants._16).v_Value1,
                               _17 = EvaPsicolaborla.Count == 0 ? string.Empty : EvaPsicolaborla.Find(p => p.v_ComponentFieldId == Constants._17).v_Value1,
                               _18 = EvaPsicolaborla.Count == 0 ? string.Empty : EvaPsicolaborla.Find(p => p.v_ComponentFieldId == Constants._18).v_Value1,
                               _19 = EvaPsicolaborla.Count == 0 ? string.Empty : EvaPsicolaborla.Find(p => p.v_ComponentFieldId == Constants._19).v_Value1,
                               _20 = EvaPsicolaborla.Count == 0 ? string.Empty : EvaPsicolaborla.Find(p => p.v_ComponentFieldId == Constants._20).v_Value1,
                               _21 = EvaPsicolaborla.Count == 0 ? string.Empty : EvaPsicolaborla.Find(p => p.v_ComponentFieldId == Constants._21).v_Value1,
                               _22 = EvaPsicolaborla.Count == 0 ? string.Empty : EvaPsicolaborla.Find(p => p.v_ComponentFieldId == Constants._22).v_Value1,
                               _23 = EvaPsicolaborla.Count == 0 ? string.Empty : EvaPsicolaborla.Find(p => p.v_ComponentFieldId == Constants._23).v_Value1,
                               _24 = EvaPsicolaborla.Count == 0 ? string.Empty : EvaPsicolaborla.Find(p => p.v_ComponentFieldId == Constants._24).v_Value1,
                               _25 = EvaPsicolaborla.Count == 0 ? string.Empty : EvaPsicolaborla.Find(p => p.v_ComponentFieldId == Constants._25).v_Value1,
                               _26 = EvaPsicolaborla.Count == 0 ? string.Empty : EvaPsicolaborla.Find(p => p.v_ComponentFieldId == Constants._26).v_Value1,
                               _27 = EvaPsicolaborla.Count == 0 ? string.Empty : EvaPsicolaborla.Find(p => p.v_ComponentFieldId == Constants._27).v_Value1,
                               _28 = EvaPsicolaborla.Count == 0 ? string.Empty : EvaPsicolaborla.Find(p => p.v_ComponentFieldId == Constants._28).v_Value1,

                               Fatiga = EvaPsicolaborla.Count == 0 ? string.Empty : EvaPsicolaborla.Find(p => p.v_ComponentFieldId == Constants.EVALUACION_PSICOLABORAL_ESCALA_FATIGA_OBSERVACIONES).v_Value1,
                               Recomendacion = EvaPsicolaborla.Count == 0 ? string.Empty : EvaPsicolaborla.Find(p => p.v_ComponentFieldId == Constants.EVALUACION_PSICOLABORAL_ESCALA_FATIGA_RECOMENDACIONES).v_Value1,
                               Somnolencia = EvaPsicolaborla.Count == 0 ? string.Empty : EvaPsicolaborla.Find(p => p.v_ComponentFieldId == Constants.EVALUACION_PSICOLABORAL_ESCALA_SOMNOLENCIA).v_Value1,
                               ConclusionFinal = EvaPsicolaborla.Count == 0 ? string.Empty : EvaPsicolaborla.Find(p => p.v_ComponentFieldId == Constants.EVALUACION_PSICOLABORAL_CONCLUSION_FINAL_CONCLUSION).v_Value1,
                               Conclusion = EvaPsicolaborla.Count == 0 ? string.Empty : EvaPsicolaborla.Find(p => p.v_ComponentFieldId == Constants.EVALUACION_PSICOLABORAL_CONCLUSION_FINAL_APTITUD).v_Value1,
                               RiesgoEstres = EvaPsicolaborla.Count == 0 ? string.Empty : EvaPsicolaborla.Find(p => p.v_ComponentFieldId == Constants.EVALUACION_PSICOLABORAL_RIESGO_ESTRES).v_Value1,

                               b_Logo = MedicalCenter.b_Image,
                               EmpresaPropietaria = MedicalCenter.v_Name,
                               EmpresaPropietariaDireccion = MedicalCenter.v_Address,
                               EmpresaPropietariaTelefono = MedicalCenter.v_PhoneNumber,
                               EmpresaPropietariaEmail = MedicalCenter.v_Mail,

                           }).ToList();


                return sql;
            }
            catch (Exception)
            {

                throw;
            }
        }

        // Alejandro
        public List<Sigesoft.Node.WinClient.BE.DiagnosticRepositoryList> GetDiagnosticRepositoryByComponent(string pstrServiceId, string componentId)
        {
            //mon.IsActive = true;

            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();


                var isDeleted = 0;
                var recomId = (int)Typifying.Recomendaciones;

                var qryRecom = (from dr in dbContext.diagnosticrepository
                                join a in dbContext.recommendation on dr.v_DiagnosticRepositoryId equals a.v_DiagnosticRepositoryId  // RECOMENDACIONES
                                join eee in dbContext.masterrecommendationrestricction on a.v_MasterRecommendationId equals eee.v_MasterRecommendationRestricctionId

                                where dr.v_ServiceId == pstrServiceId &&
                                      dr.v_ComponentId == componentId &&
                                      a.i_IsDeleted == isDeleted &&
                                      eee.i_TypifyingId == recomId
                                select new Sigesoft.Node.WinClient.BE.RecomendationList
                                {
                                    v_DiagnosticRepositoryId = dr.v_DiagnosticRepositoryId,
                                    v_RecommendationName = eee.v_Name,
                                    v_ServiceId = dr.v_ServiceId
                                }).ToList();

                var restricId = (int)Typifying.Restricciones;

                var qryRestric = (from dr in dbContext.diagnosticrepository
                                  join a in dbContext.restriction on dr.v_DiagnosticRepositoryId equals a.v_DiagnosticRepositoryId  // RECOMENDACIONES
                                  join eee in dbContext.masterrecommendationrestricction on a.v_MasterRestrictionId equals eee.v_MasterRecommendationRestricctionId

                                  where dr.v_ServiceId == pstrServiceId &&
                                        dr.v_ComponentId == componentId &&
                                        a.i_IsDeleted == isDeleted &&
                                        eee.i_TypifyingId == restricId
                                  select new Sigesoft.Node.WinClient.BE.RestrictionList
                                  {
                                      v_DiagnosticRepositoryId = dr.v_DiagnosticRepositoryId,
                                      v_RestrictionName = eee.v_Name,
                                      v_ServiceId = dr.v_ServiceId
                                  }).ToList();


                var query = (from ccc in dbContext.diagnosticrepository

                             //join sss in dbContext.service on ccc.v_ServiceId equals sss.v_ServiceId  // ESO

                             join ddd in dbContext.diseases on ccc.v_DiseasesId equals ddd.v_DiseasesId  // Diagnosticos

                             where (ccc.v_ServiceId == pstrServiceId) &&
                                   (ccc.v_ComponentId == componentId) &&
                                   (ccc.i_IsDeleted == 0) &&
                                   (ccc.i_FinalQualificationId == (int)FinalQualification.Definitivo ||
                                   ccc.i_FinalQualificationId == (int)FinalQualification.Presuntivo)

                             select new Sigesoft.Node.WinClient.BE.DiagnosticRepositoryList
                             {
                                 v_DiagnosticRepositoryId = ccc.v_DiagnosticRepositoryId,
                                 v_ServiceId = ccc.v_ServiceId,
                                 v_DiseasesId = ccc.v_DiseasesId,
                                 i_AutoManualId = ccc.i_AutoManualId,
                                 i_PreQualificationId = ccc.i_PreQualificationId,
                                 i_FinalQualificationId = ccc.i_FinalQualificationId,
                                 i_DiagnosticTypeId = ccc.i_DiagnosticTypeId,

                                 d_ExpirationDateDiagnostic = ccc.d_ExpirationDateDiagnostic,

                                 v_DiseasesName = ddd.v_Name,
                                 v_ComponentFieldsId = ccc.v_ComponentFieldId

                             }).ToList();

                query.ForEach(a =>
                {
                    a.Recomendations = qryRecom.FindAll(p => p.v_DiagnosticRepositoryId == a.v_DiagnosticRepositoryId);
                    a.Restrictions = qryRestric.FindAll(p => p.v_DiagnosticRepositoryId == a.v_DiagnosticRepositoryId);
                });


                //var q = (from a in query
                //         select new DiagnosticRepositoryList
                //         {
                //             v_DiagnosticRepositoryId = a.v_DiagnosticRepositoryId,
                //             v_ServiceId = a.v_ServiceId,
                //             v_DiseasesId = a.v_DiseasesId,
                //             i_DiagnosticTypeId = a.i_DiagnosticTypeId,
                //             d_ExpirationDateDiagnostic = a.d_ExpirationDateDiagnostic,



                //             v_DiseasesName = a.v_DiseasesName,
                //             v_RecomendationsName = ConcatenateRecommendation(a.v_DiagnosticRepositoryId),
                //             v_RestrictionsName = ConcatenateRestriction(a.v_DiagnosticRepositoryId),
                //             v_AptitudeStatusName = a.v_AptitudeStatusName,
                //             v_OccupationName = a.v_OccupationName  // por ahora se muestra el GESO
                //         }).ToList();


                return query;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public List<Sigesoft.Node.WinClient.BE.ServiceList> ReportAscensoGrandesAlturas(string pstrserviceId, string pstrComponentId)
        {
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();


                var objEntity = (from A in dbContext.service
                                 join B in dbContext.person on A.v_PersonId equals B.v_PersonId
                                 join E in dbContext.servicecomponent on new { a = pstrserviceId, b = pstrComponentId }
                                                                        equals new { a = E.v_ServiceId, b = E.v_ComponentId }

                                 // Usuario Medico Evaluador / Medico Aprobador ****************************
                                 join me in dbContext.systemuser on E.i_ApprovedUpdateUserId equals me.i_SystemUserId into me_join
                                 from me in me_join.DefaultIfEmpty()

                                 join pme in dbContext.professional on me.v_PersonId equals pme.v_PersonId into pme_join
                                 from pme in pme_join.DefaultIfEmpty()

                                 join G in dbContext.professional on new { a = me.v_PersonId }
                                                                      equals new { a = G.v_PersonId } into G_join
                                 from G in G_join.DefaultIfEmpty()

                                 join H in dbContext.person on me.v_PersonId equals H.v_PersonId into H_join
                                 from H in H_join.DefaultIfEmpty()

                                 join I in dbContext.protocol on A.v_ProtocolId equals I.v_ProtocolId into I_join
                                 from I in I_join.DefaultIfEmpty()

                                 join J in dbContext.organization on I.v_EmployerOrganizationId equals J.v_OrganizationId into J_join
                                 from J in J_join.DefaultIfEmpty()

                                 where A.v_ServiceId == pstrserviceId
                                 select new Sigesoft.Node.WinClient.BE.ServiceList
                                 {
                                     v_PersonId = A.v_PersonId,
                                     v_NamePacient = B.v_FirstName,
                                     v_Surnames = B.v_FirstLastName + " " + B.v_SecondLastName,
                                     DireccionPaciente = B.v_AdressLocation,
                                     d_BirthDate = B.d_Birthdate,
                                     d_ServiceDate = A.d_ServiceDate,
                                     v_ServiceId = A.v_ServiceId,
                                     v_DocNumber = B.v_DocNumber,
                                     i_SexTypeId = B.i_SexTypeId.Value,
                                     FirmaMedico = pme.b_SignatureImage,
                                     ApellidosDoctor = H.v_FirstLastName + " " + H.v_SecondLastName,
                                     NombreDoctor = H.v_FirstName,
                                     CMP = pme.v_ProfessionalCode,
                                     DireccionDoctor = H.v_AdressLocation,
                                     EmpresaEmpleadora = J.v_Name,
                                     FirmaTrabajador = B.b_RubricImage,
                                     HuellaTrabajador = B.b_FingerPrintImage,
                                     v_ServiceComponentId = E.v_ServiceComponentId,
                                     NombreUsuarioGraba = H.v_FirstName + " " + H.v_FirstLastName + " " + H.v_SecondLastName,
                                 });

                var MedicalCenter = GetInfoMedicalCenter();

                var funcionesVitales = ReportFuncionesVitales(pstrserviceId, Constants.FUNCIONES_VITALES_ID);
                var antropometria = ReportAntropometria(pstrserviceId, Constants.ANTROPOMETRIA_ID);

                var sql = (from a in objEntity.ToList()
                           select new Sigesoft.Node.WinClient.BE.ServiceList
                           {
                               v_ServiceId = a.v_ServiceId,
                               v_ServiceComponentId = a.v_ServiceComponentId,
                               v_PersonId = a.v_PersonId,
                               v_NamePacient = a.v_NamePacient,
                               DireccionPaciente = a.DireccionPaciente,
                               v_Surnames = a.v_Surnames,
                               d_BirthDate = a.d_BirthDate,
                               i_AgePacient = GetAge(a.d_BirthDate.Value),
                               d_ServiceDate = a.d_ServiceDate,
                               v_DocNumber = a.v_DocNumber,
                               i_SexTypeId = a.i_SexTypeId,
                               FirmaMedico = a.FirmaMedico,
                               Anemia = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.ALTURA_7D_ANEMIA_ID, "NOCOMBO", 0, "SI"),
                               Cirugia = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.ASCENSO_GRANDES_ALTURAS_CIRUGIA_MAYOR_CRECIENTE_ID, "NOCOMBO", 0, "SI"),
                               Desordenes = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.ASCENSO_GRANDES_ALTURAS_DESORDENES_COAGULACION_ID, "NOCOMBO", 0, "SI"),
                               Diabetes = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.ASCENSO_GRANDES_ALTURAS_DIABETES_MELLITUS_ID, "NOCOMBO", 0, "SI"),
                               Hipertension = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.ASCENSO_GRANDES_ALTURAS_HIPERTENSION_ARTERIAL_ID, "NOCOMBO", 0, "SI"),
                               Embarazo = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.ASCENSO_GRANDES_ALTURAS_EMBARAZO_ID, "NOCOMBO", 0, "SI"),
                               ProbNeurologicos = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.ASCENSO_GRANDES_ALTURAS_PROBLEMAS_NEUROLOGICOS_ID, "NOCOMBO", 0, "SI"),
                               Infecciones = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.ASCENSO_GRANDES_ALTURAS_INFECCIONES_RECIENTES_ID, "NOCOMBO", 0, "SI"),
                               Obesidad = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.ASCENSO_GRANDES_ALTURAS_OBESIDAD_MORBIDA_ID, "NOCOMBO", 0, "SI"),
                               ProCardiacos = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.ASCENSO_GRANDES_ALTURAS_PROBLEMAS_CARDIACOS_ID, "NOCOMBO", 0, "SI"),
                               ProRespiratorios = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.ASCENSO_GRANDES_ALTURAS_PROBLEMAS_RESPIRATORIOS_ID, "NOCOMBO", 0, "SI"),
                               ProOftalmologico = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.ASCENSO_GRANDES_ALTURAS_PROBLEMAS_OFTALMOLOGICOS_ID, "NOCOMBO", 0, "SI"),
                               ProDigestivo = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.ASCENSO_GRANDES_ALTURAS_PROBLEMAS_DIGESTIVOS_ID, "NOCOMBO", 0, "SI"),
                               Apnea = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.ASCENSO_GRANDES_ALTURAS_APNEA_SUEÑO_ID, "NOCOMBO", 0, "SI"),
                               Otra = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.ASCENSO_GRANDES_ALTURAS_OTRA_CONDICON_ID, "NOCOMBO", 0, "SI"),
                               Alergia = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.ASCENSO_GRANDES_ALTURAS_ALERGIAS_ID, "NOCOMBO", 0, "SI"),
                               MedicacionActual = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.ASCENSO_GRANDES_ALTURAS_USO_MEDICACION_ACTUAL_ID, "NOCOMBO", 0, "SI"),
                               AptoAscenderAlturas = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.ASCENSO_GRANDES_ALTURAS_APTO_ASCENDER_GRANDES_ALTURAS_ID, "SICOMBO", 163, "NO"),
                               ActividadRealizar = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.ASCENSO_GRANDES_ALTURAS_ACTIVIDAD_REALIZAR_DESCRIPCION_ID, "NOCOMBO", 0, "SI"),
                               ApellidosDoctor = a.ApellidosDoctor,
                               NombreDoctor = a.NombreDoctor,
                               CMP = a.CMP,
                               DireccionDoctor = a.DireccionDoctor,
                               EmpresaEmpleadora = a.EmpresaEmpleadora,
                               FirmaTrabajador = a.FirmaTrabajador,
                               HuellaTrabajador = a.HuellaTrabajador,
                               Descripcion = GetServiceComponentFielValue(a.v_ServiceId, pstrComponentId, Constants.ALTURA_7D_DESCRIPCION_ID, "NOCOMBO", 0, "SI"),

                               b_Logo = MedicalCenter.b_Image,
                               EmpresaPropietaria = MedicalCenter.v_Name,
                               EmpresaPropietariaDireccion = MedicalCenter.v_Address,
                               EmpresaPropietariaTelefono = MedicalCenter.v_PhoneNumber,
                               EmpresaPropietariaEmail = MedicalCenter.v_Mail,
                               NombreUsuarioGraba = a.NombreUsuarioGraba

                           }).ToList();

                return sql;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public string GetValueOdontogramaAusente(string pstrServiceId, string pstrComponentId, string pstrFieldId, string pstrPath)
        {
            try
            {
                ServiceBL oServiceBL = new ServiceBL();
                List<Sigesoft.Node.WinClient.BE.ServiceComponentFieldValuesList> oServiceComponentFieldValuesList = new List<Sigesoft.Node.WinClient.BE.ServiceComponentFieldValuesList>();

                oServiceComponentFieldValuesList = oServiceBL.ValoresComponenteOdontogramaAusente(pstrServiceId, pstrComponentId, pstrPath);
                var xx = oServiceComponentFieldValuesList.Count() == 0 || ((Sigesoft.Node.WinClient.BE.ServiceComponentFieldValuesList)oServiceComponentFieldValuesList.Find(p => p.v_ComponentFieldId == pstrFieldId)) == null ? String.Empty : ((Sigesoft.Node.WinClient.BE.ServiceComponentFieldValuesList)oServiceComponentFieldValuesList.Find(p => p.v_ComponentFieldId == pstrFieldId)).v_Value1;

                return xx;

            }
            catch (Exception)
            {

                throw;
            }

        }

        public List<Sigesoft.Node.WinClient.BE.ServiceComponentFieldValuesList> ValoresComponenteOdontogramaAusente(string pstrServiceId, string pstrComponentId, string pstrPath)
        {
            SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();


            try
            {
                List<Sigesoft.Node.WinClient.BE.ServiceComponentFieldValuesList> serviceComponentFieldValues = (from A in dbContext.service
                                                                                     join B in dbContext.servicecomponent on A.v_ServiceId equals B.v_ServiceId
                                                                                     join C in dbContext.servicecomponentfields on B.v_ServiceComponentId equals C.v_ServiceComponentId
                                                                                     join D in dbContext.servicecomponentfieldvalues on C.v_ServiceComponentFieldsId equals D.v_ServiceComponentFieldsId

                                                                                     where (A.v_ServiceId == pstrServiceId)
                                                                                           && (B.v_ComponentId == pstrComponentId)
                                                                                           && (B.i_IsDeleted == 0)
                                                                                           && (C.i_IsDeleted == 0)
                                                                                     let range = (D.v_Value1 == "1" ? pstrPath + "\\Resources\\ausent.png" :
                                                                                                 string.Empty
                                                                                                 )
                                                                                    select new Sigesoft.Node.WinClient.BE.ServiceComponentFieldValuesList
                                                                                     {
                                                                                         v_ComponentFieldId = C.v_ComponentFieldId,
                                                                                         //v_ComponentFieldId = G.v_ComponentFieldId,
                                                                                         //v_ComponentFielName = G.v_TextLabel,
                                                                                         v_ServiceComponentFieldsId = C.v_ServiceComponentFieldsId,
                                                                                         //v_Value1 =  D.v_Value1 == "1" ? pstrPath + "\\caries.png" ? D.v_Value1 == "2" ? pstrPath + "\\curacion.png" ?D.v_Value1 == "3" ? pstrPath + "\\ausent.png" : D.v_Value1, 
                                                                                         //v_Value1 = D.v_Value1
                                                                                         v_Value1 = range
                                                                                     }).ToList();


                return serviceComponentFieldValues;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public string GetNameMedicalCenter()
        {
            using (SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel())
            {

                var nameMedicalCenter = (from n in dbContext.organization
                                         where n.v_OrganizationId == Constants.OWNER_ORGNIZATION_ID
                                         select n.v_Name + " " + n.v_Address).SingleOrDefault<string>();

                return nameMedicalCenter;
            }
        }


        // Alejandro
        public List<Sigesoft.Node.WinClient.BE.AudiometriaUserControlList> ReportAudiometriaUserControl(string pstrserviceId, string pstrComponentId)
        {
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
                var audiometriaList = new List<Sigesoft.Node.WinClient.BE.AudiometriaUserControlList>();

                var __sql = ValoresComponentesUserControl(pstrserviceId, pstrComponentId);

                if (__sql.Count == 0)
                    return audiometriaList;

                var multimediaFileId_OD = string.Empty;
                var multimediaFileId_OI = string.Empty;

                var xMultimediaFileId_OD = __sql.Find(p => p.v_ComponentFieldId == Constants.txt_MULTIMEDIA_FILE_OD);
                if (xMultimediaFileId_OD != null)
                    multimediaFileId_OD = xMultimediaFileId_OD.v_Value1;

                var xMultimediaFileId_OI = __sql.Find(p => p.v_ComponentFieldId == Constants.txt_MULTIMEDIA_FILE_OI);
                if (xMultimediaFileId_OI != null)
                    multimediaFileId_OI = xMultimediaFileId_OI.v_Value1;

                var img_OD = (from mf in dbContext.multimediafile where mf.v_MultimediaFileId == multimediaFileId_OD select mf.b_File).SingleOrDefault();
                var img_OI = (from mf in dbContext.multimediafile where mf.v_MultimediaFileId == multimediaFileId_OI select mf.b_File).SingleOrDefault();

                var ent = new Sigesoft.Node.WinClient.BE.AudiometriaUserControlList();
                // OD
                var xVA_OD_500 = __sql.Find(p => p.v_ComponentFieldId == Constants.txt_VA_OD_500);
                if (xVA_OD_500 != null)
                    ent.VA_OD_500 = xVA_OD_500.v_Value1;

                var xVA_OD_1000 = __sql.Find(p => p.v_ComponentFieldId == Constants.txt_VA_OD_1000);
                if (xVA_OD_1000 != null)
                    ent.VA_OD_1000 = xVA_OD_1000.v_Value1;

                var xVA_OD_2000 = __sql.Find(p => p.v_ComponentFieldId == Constants.txt_VA_OD_2000);
                if (xVA_OD_2000 != null)
                    ent.VA_OD_2000 = xVA_OD_2000.v_Value1;

                var xVA_OD_3000 = __sql.Find(p => p.v_ComponentFieldId == Constants.txt_VA_OD_3000);
                if (xVA_OD_3000 != null)
                    ent.VA_OD_3000 = xVA_OD_3000.v_Value1;

                var xVA_OD_4000 = __sql.Find(p => p.v_ComponentFieldId == Constants.txt_VA_OD_4000);
                if (xVA_OD_4000 != null)
                    ent.VA_OD_4000 = xVA_OD_4000.v_Value1;

                var xVA_OD_6000 = __sql.Find(p => p.v_ComponentFieldId == Constants.txt_VA_OD_6000);
                if (xVA_OD_6000 != null)
                    ent.VA_OD_6000 = xVA_OD_6000.v_Value1;

                var xVA_OD_8000 = __sql.Find(p => p.v_ComponentFieldId == Constants.txt_VA_OD_8000);
                if (xVA_OD_8000 != null)
                    ent.VA_OD_8000 = xVA_OD_8000.v_Value1;

                var xVO_OD_500 = __sql.Find(p => p.v_ComponentFieldId == Constants.txt_VO_OD_500);
                if (xVO_OD_500 != null)
                    ent.VO_OD_500 = xVO_OD_500.v_Value1;

                var xVO_OD_1000 = __sql.Find(p => p.v_ComponentFieldId == Constants.txt_VO_OD_1000);
                if (xVO_OD_1000 != null)
                    ent.VO_OD_1000 = xVO_OD_1000.v_Value1;

                var xVO_OD_2000 = __sql.Find(p => p.v_ComponentFieldId == Constants.txt_VO_OD_2000);
                if (xVO_OD_2000 != null)
                    ent.VO_OD_2000 = xVO_OD_2000.v_Value1;

                var xVO_OD_3000 = __sql.Find(p => p.v_ComponentFieldId == Constants.txt_VO_OD_3000);
                if (xVO_OD_3000 != null)
                    ent.VO_OD_3000 = xVO_OD_3000.v_Value1;

                var xVO_OD_4000 = __sql.Find(p => p.v_ComponentFieldId == Constants.txt_VO_OD_4000);
                if (xVO_OD_4000 != null)
                    ent.VO_OD_4000 = xVO_OD_4000.v_Value1;

                var xVO_OD_6000 = __sql.Find(p => p.v_ComponentFieldId == Constants.txt_VO_OD_6000);
                if (xVO_OD_6000 != null)
                    ent.VO_OD_6000 = xVO_OD_6000.v_Value1;

                var xVO_OD_8000 = __sql.Find(p => p.v_ComponentFieldId == Constants.txt_VO_OD_8000);
                if (xVO_OD_8000 != null)
                    ent.VO_OD_8000 = xVO_OD_8000.v_Value1;

                // OI
                var xVA_OI_500 = __sql.Find(p => p.v_ComponentFieldId == Constants.txt_VA_OI_500);
                if (xVA_OI_500 != null)
                    ent.VA_OI_500 = xVA_OI_500.v_Value1;

                var xVA_OI_1000 = __sql.Find(p => p.v_ComponentFieldId == Constants.txt_VA_OI_1000);
                if (xVA_OI_1000 != null)
                    ent.VA_OI_1000 = xVA_OI_1000.v_Value1;

                var xVA_OI_2000 = __sql.Find(p => p.v_ComponentFieldId == Constants.txt_VA_OI_2000);
                if (xVA_OI_2000 != null)
                    ent.VA_OI_2000 = xVA_OI_2000.v_Value1;

                var xVA_OI_3000 = __sql.Find(p => p.v_ComponentFieldId == Constants.txt_VA_OI_3000);
                if (xVA_OI_3000 != null)
                    ent.VA_OI_3000 = xVA_OI_3000.v_Value1;

                var xVA_OI_4000 = __sql.Find(p => p.v_ComponentFieldId == Constants.txt_VA_OI_4000);
                if (xVA_OI_4000 != null)
                    ent.VA_OI_4000 = xVA_OI_4000.v_Value1;

                var xVA_OI_6000 = __sql.Find(p => p.v_ComponentFieldId == Constants.txt_VA_OI_6000);
                if (xVA_OI_6000 != null)
                    ent.VA_OI_6000 = xVA_OI_6000.v_Value1;

                var xVA_OI_8000 = __sql.Find(p => p.v_ComponentFieldId == Constants.txt_VA_OI_8000);
                if (xVA_OI_8000 != null)
                    ent.VA_OI_8000 = xVA_OI_8000.v_Value1;

                var xVO_OI_500 = __sql.Find(p => p.v_ComponentFieldId == Constants.txt_VO_OI_500);
                if (xVO_OI_500 != null)
                    ent.VO_OI_500 = xVO_OI_500.v_Value1;

                var xVO_OI_1000 = __sql.Find(p => p.v_ComponentFieldId == Constants.txt_VO_OI_1000);
                if (xVO_OI_1000 != null)
                    ent.VO_OI_1000 = xVO_OI_1000.v_Value1;

                var xVO_OI_2000 = __sql.Find(p => p.v_ComponentFieldId == Constants.txt_VO_OI_2000);
                if (xVO_OI_2000 != null)
                    ent.VO_OI_2000 = xVO_OI_2000.v_Value1;

                var xVO_OI_3000 = __sql.Find(p => p.v_ComponentFieldId == Constants.txt_VO_OI_3000);
                if (xVO_OI_3000 != null)
                    ent.VO_OI_3000 = xVO_OI_3000.v_Value1;

                var xVO_OI_4000 = __sql.Find(p => p.v_ComponentFieldId == Constants.txt_VO_OI_4000);
                if (xVO_OI_4000 != null)
                    ent.VO_OI_4000 = xVO_OI_4000.v_Value1;

                var xVO_OI_6000 = __sql.Find(p => p.v_ComponentFieldId == Constants.txt_VO_OI_6000);
                if (xVO_OI_6000 != null)
                    ent.VO_OI_6000 = xVO_OI_6000.v_Value1;

                var xVO_OI_8000 = __sql.Find(p => p.v_ComponentFieldId == Constants.txt_VO_OI_8000);
                if (xVO_OI_8000 != null)
                    ent.VO_OI_8000 = xVO_OI_8000.v_Value1;


                ent.b_AudiogramaOD = img_OD;
                ent.b_AudiogramaOI = img_OI;

                audiometriaList.Add(ent);

                return audiometriaList;
            }
            catch (Exception)
            {

                throw;
            }
        }

        // Alejandro
        public List<Sigesoft.Node.WinClient.BE.AudiometriaList> ReportAudiometria(string pstrserviceId, string pstrComponentId)
        {
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                var objEntity = (from A in dbContext.service
                                 join B in dbContext.person on A.v_PersonId equals B.v_PersonId
                                 join E in dbContext.servicecomponent on new { a = pstrserviceId, b = pstrComponentId }
                                                                        equals new { a = E.v_ServiceId, b = E.v_ComponentId }

                                 // Usuario Medico Evaluador / Medico Aprobador ****************************
                                 join me in dbContext.systemuser on E.i_ApprovedUpdateUserId equals me.i_SystemUserId into me_join
                                 from me in me_join.DefaultIfEmpty()

                                 join pme in dbContext.professional on me.v_PersonId equals pme.v_PersonId into pme_join
                                 from pme in pme_join.DefaultIfEmpty()

                                 // Usuario Tecnologo *************************************
                                 join tec in dbContext.systemuser on E.i_UpdateUserTechnicalDataRegisterId equals tec.i_SystemUserId into tec_join
                                 from tec in tec_join.DefaultIfEmpty()

                                 join ptec in dbContext.professional on tec.v_PersonId equals ptec.v_PersonId into ptec_join
                                 from ptec in ptec_join.DefaultIfEmpty()
                                 // *******************************************************  

                                 join pro in dbContext.protocol on A.v_ProtocolId equals pro.v_ProtocolId

                                 join H in dbContext.systemparameter on new { a = pro.i_EsoTypeId.Value, b = 118 }
                                                 equals new { a = H.i_ParameterId, b = H.i_GroupId }  // TIPO ESO [ESOA,ESOR,ETC]

                                 // Empresa / Sede Trabajo  ********************************************************
                                 join ow in dbContext.organization on new { a = pro.v_WorkingOrganizationId }
                                         equals new { a = ow.v_OrganizationId } into ow_join
                                 from ow in ow_join.DefaultIfEmpty()

                                 join lw in dbContext.location on new { a = pro.v_WorkingOrganizationId, b = pro.v_WorkingLocationId }
                                      equals new { a = lw.v_OrganizationId, b = lw.v_LocationId } into lw_join
                                 from lw in lw_join.DefaultIfEmpty()

                                 //************************************************************************************


                                 where A.v_ServiceId == pstrserviceId

                                 select new Sigesoft.Node.WinClient.BE.AudiometriaList
                                 {
                                     v_PersonId = A.v_PersonId,

                                     v_FullPersonName = B.v_FirstLastName + " " + B.v_SecondLastName + " " + B.v_FirstName,

                                     d_BirthDate = B.d_Birthdate,
                                     d_ServiceDate = A.d_ServiceDate,

                                     v_DocNumber = B.v_DocNumber,
                                     i_SexTypeId = B.i_SexTypeId.Value,

                                     FirmaTecnologo = ptec.b_SignatureImage,
                                     FirmaMedico = pme.b_SignatureImage,

                                     Puesto = B.v_CurrentOccupation,
                                     v_SexType = B.i_SexTypeId == (int)Gender.MASCULINO ? "M" : "F",
                                     //
                                     v_EsoTypeName = H.v_Value1,
                                     v_ServiceComponentId = E.v_ServiceComponentId,
                                     v_WorkingOrganizationName = ow.v_Name,
                                     v_FullWorkingOrganizationName = ow.v_Name + " / " + lw.v_Name,
                                     FirmaTrabajador = B.b_RubricImage,
                                     HuellaTrabajador = B.b_FingerPrintImage,


                                 });

                var MedicalCenter = GetInfoMedicalCenter();

                var __sql = ValoresComponente(pstrserviceId, pstrComponentId);

                //// Requisitos para la Audiometria                         
                //          var CambiosAltitud = __sql.Count == 0 ? string.Empty : __sql.Find(p => p.v_ComponentFieldId == Constants.AUDIOMETRIA_REQUISITOS_CAMBIOS_ALTITUD).v_Value1;
                //           var ExpuestoRuido = __sql.Count == 0 ? string.Empty : __sql.Find(p => p.v_ComponentFieldId == Constants.AUDIOMETRIA_REQUISITOS_EXPUESTO_RUIDO).v_Value1;
                //           var ProcesoInfeccioso = __sql.Count == 0 ? string.Empty : __sql.Find(p => p.v_ComponentFieldId == Constants.AUDIOMETRIA_REQUISITOS_PROCESO_INFECCIOSO).v_Value1;
                //           var DurmioNochePrevia = __sql.Count == 0 ? string.Empty : __sql.Find(p => p.v_ComponentFieldId == Constants.AUDIOMETRIA_REQUISITOS_DURMIO_NOCHE_PREVIA).v_Value1;
                //           var ConsumioAlcoholDiaPrevio = __sql.Count == 0 ? string.Empty : __sql.Find(p => p.v_ComponentFieldId == Constants.AUDIOMETRIA_REQUISITOS_CONSUMIO_ALCOHOL_DIA_PREVIO).v_Value1;

                //           //// Antecedentes Medicos de importancia

                //           var RinitisSinusitis = __sql.Count == 0 ? string.Empty : __sql.Find(p => p.v_ComponentFieldId == Constants.AUDIOMETRIA_ANTECEDENTES_RINITIS_SINUSITIS).v_Value1;
                //           var UsoMedicamentos = __sql.Count == 0 ? string.Empty : __sql.Find(p => p.v_ComponentFieldId == Constants.AUDIOMETRIA_ANTECEDENTES_USO_MEDICAMENTOS).v_Value1;
                //           var Sarampion = __sql.Count == 0 ? string.Empty : __sql.Find(p => p.v_ComponentFieldId == Constants.AUDIOMETRIA_ANTECEDENTES_SARAMPION).v_Value1;
                //           var Tec = __sql.Count == 0 ? string.Empty : __sql.Find(p => p.v_ComponentFieldId == Constants.AUDIOMETRIA_ANTECEDENTES_TEC).v_Value1;
                //           var OtitisMediaCronica = __sql.Count == 0 ? string.Empty : __sql.Find(p => p.v_ComponentFieldId == Constants.AUDIOMETRIA_ANTECEDENTES_OTITIS_MEDIA_CRONICA).v_Value1;
                //          var  DiabetesMellitus = __sql.Count == 0 ? string.Empty : __sql.Find(p => p.v_ComponentFieldId == Constants.AUDIOMETRIA_ANTECEDENTES_DIABETES_MELLITUS).v_Value1;
                //           var SorderaAntecedente = __sql.Count == 0 ? string.Empty : __sql.Find(p => p.v_ComponentFieldId == Constants.AUDIOMETRIA_ANTECEDENTES_SORDERA).v_Value1;
                //           var SorderaFamiliar = __sql.Count == 0 ? string.Empty : __sql.Find(p => p.v_ComponentFieldId == Constants.AUDIOMETRIA_ANTECEDENTES_SORDERA_FAMILIAR).v_Value1;
                //          var  Meningitis = __sql.Count == 0 ? string.Empty : __sql.Find(p => p.v_ComponentFieldId == Constants.AUDIOMETRIA_ANTECEDENTES_MENINGITIS).v_Value1;
                //           var Dislipidemia = __sql.Count == 0 ? string.Empty : __sql.Find(p => p.v_ComponentFieldId == Constants.AUDIOMETRIA_ANTECEDENTES_DISLIPIDEMIA).v_Value1;
                //           var EnfTiroidea = __sql.Count == 0 ? string.Empty : __sql.Find(p => p.v_ComponentFieldId == Constants.AUDIOMETRIA_ANTECEDENTES_ENF_TIROIDEA).v_Value1;
                //           var SustQuimicas = __sql.Count == 0 ? string.Empty : __sql.Find(p => p.v_ComponentFieldId == Constants.AUDIOMETRIA_ANTECEDENTES_SUST_QUIMICAS).v_Value1;

                //           //// Hobbies

                //           var UsoMP3 = __sql.Count == 0 ? string.Empty : __sql.Find(p => p.v_ComponentFieldId == Constants.AUDIOMETRIA_HOBBIES_USO_MP3).v_Value1;
                //           var PracticaTiro = __sql.Count == 0 ? string.Empty : __sql.Find(p => p.v_ComponentFieldId == Constants.AUDIOMETRIA_HOBBIES_PRACTICA_TIRO).v_Value1;
                //           var Otros = __sql.Count == 0 ? string.Empty : __sql.Find(p => p.v_ComponentFieldId == Constants.AUDIOMETRIA_HOBBIES_OTROS).v_Value1;

                //           //// Sintomas actuales

                //           var Sordera = __sql.Count == 0 ? string.Empty : __sql.Find(p => p.v_ComponentFieldId == Constants.AUDIOMETRIA_SINTOMAS_ACTUALES_SORDERA).v_Value1;
                //           var Otalgia = __sql.Count == 0 ? string.Empty : __sql.Find(p => p.v_ComponentFieldId == Constants.AUDIOMETRIA_SINTOMAS_ACTUALES_OTALGIA).v_Value1;
                //           var Acufenos = __sql.Count == 0 ? string.Empty : __sql.Find(p => p.v_ComponentFieldId == Constants.AUDIOMETRIA_SINTOMAS_ACTUALES_ACUFENOS).v_Value1;
                //           var SecrecionOtica = __sql.Count == 0 ? string.Empty : __sql.Find(p => p.v_ComponentFieldId == Constants.AUDIOMETRIA_SINTOMAS_ACTUALES_SECRECION_OTICA).v_Value1;
                //           var Vertigos = __sql.Count == 0 ? string.Empty : __sql.Find(p => p.v_ComponentFieldId == Constants.AUDIOMETRIA_SINTOMAS_ACTUALES_VERTIGOS).v_Value1;

                //           //// Otoscopia

                //           var OidoIzquierdo = __sql.Count == 0 ? string.Empty : __sql.Find(p => p.v_ComponentFieldId == Constants.AUDIOMETRIA_OTOSCOPIA_OIDO_IZQUIERDO).v_Value1;
                //           var OidoDerecho = __sql.Count == 0 ? string.Empty : __sql.Find(p => p.v_ComponentFieldId == Constants.AUDIOMETRIA_OTOSCOPIA_OIDO_DERECHO).v_Value1;


                var sql = (from a in objEntity.ToList()

                           select new Sigesoft.Node.WinClient.BE.AudiometriaList
                           {

                               v_PersonId = a.v_PersonId,
                               v_FullPersonName = a.v_FullPersonName,

                               d_BirthDate = a.d_BirthDate,
                               i_AgePacient = GetAge(a.d_BirthDate.Value),
                               d_ServiceDate = a.d_ServiceDate,
                               v_DocNumber = a.v_DocNumber,
                               i_SexTypeId = a.i_SexTypeId,
                               FirmaMedico = a.FirmaMedico,
                               FirmaTecnologo = a.FirmaTecnologo,
                               Puesto = a.Puesto,
                               v_SexType = a.v_SexType,

                               // Requisitos para la Audiometria                         
                               CambiosAltitud = __sql.Count == 0 ? string.Empty : __sql.Find(p => p.v_ComponentFieldId == Constants.AUDIOMETRIA_REQUISITOS_CAMBIOS_ALTITUD).v_Value1,
                               ExpuestoRuido = __sql.Count == 0 ? string.Empty : __sql.Find(p => p.v_ComponentFieldId == Constants.AUDIOMETRIA_REQUISITOS_EXPUESTO_RUIDO).v_Value1,
                               ProcesoInfeccioso = __sql.Count == 0 ? string.Empty : __sql.Find(p => p.v_ComponentFieldId == Constants.AUDIOMETRIA_REQUISITOS_PROCESO_INFECCIOSO).v_Value1,
                               DurmioNochePrevia = __sql.Count == 0 ? string.Empty : __sql.Find(p => p.v_ComponentFieldId == Constants.AUDIOMETRIA_REQUISITOS_DURMIO_NOCHE_PREVIA).v_Value1,
                               ConsumioAlcoholDiaPrevio = __sql.Count == 0 ? string.Empty : __sql.Find(p => p.v_ComponentFieldId == Constants.AUDIOMETRIA_REQUISITOS_CONSUMIO_ALCOHOL_DIA_PREVIO).v_Value1,

                               //// Antecedentes Medicos de importancia

                               RinitisSinusitis = __sql.Count == 0 ? string.Empty : __sql.Find(p => p.v_ComponentFieldId == Constants.AUDIOMETRIA_ANTECEDENTES_RINITIS_SINUSITIS).v_Value1,
                               UsoMedicamentos = __sql.Count == 0 ? string.Empty : __sql.Find(p => p.v_ComponentFieldId == Constants.AUDIOMETRIA_ANTECEDENTES_USO_MEDICAMENTOS).v_Value1,
                               Sarampion = __sql.Count == 0 ? string.Empty : __sql.Find(p => p.v_ComponentFieldId == Constants.AUDIOMETRIA_ANTECEDENTES_SARAMPION).v_Value1,
                               Tec = __sql.Count == 0 ? string.Empty : __sql.Find(p => p.v_ComponentFieldId == Constants.AUDIOMETRIA_ANTECEDENTES_TEC).v_Value1,
                               OtitisMediaCronica = __sql.Count == 0 ? string.Empty : __sql.Find(p => p.v_ComponentFieldId == Constants.AUDIOMETRIA_ANTECEDENTES_OTITIS_MEDIA_CRONICA).v_Value1,
                               DiabetesMellitus = __sql.Count == 0 ? string.Empty : __sql.Find(p => p.v_ComponentFieldId == Constants.AUDIOMETRIA_ANTECEDENTES_DIABETES_MELLITUS).v_Value1,
                               SorderaAntecedente = __sql.Count == 0 ? string.Empty : __sql.Find(p => p.v_ComponentFieldId == Constants.AUDIOMETRIA_ANTECEDENTES_SORDERA).v_Value1,
                               SorderaFamiliar = __sql.Count == 0 ? string.Empty : __sql.Find(p => p.v_ComponentFieldId == Constants.AUDIOMETRIA_ANTECEDENTES_SORDERA_FAMILIAR).v_Value1,
                               Meningitis = __sql.Count == 0 ? string.Empty : __sql.Find(p => p.v_ComponentFieldId == Constants.AUDIOMETRIA_ANTECEDENTES_MENINGITIS).v_Value1,
                               Dislipidemia = __sql.Count == 0 ? string.Empty : __sql.Find(p => p.v_ComponentFieldId == Constants.AUDIOMETRIA_ANTECEDENTES_DISLIPIDEMIA).v_Value1,
                               EnfTiroidea = __sql.Count == 0 ? string.Empty : __sql.Find(p => p.v_ComponentFieldId == Constants.AUDIOMETRIA_ANTECEDENTES_ENF_TIROIDEA).v_Value1,
                               SustQuimicas = __sql.Count == 0 ? string.Empty : __sql.Find(p => p.v_ComponentFieldId == Constants.AUDIOMETRIA_ANTECEDENTES_SUST_QUIMICAS).v_Value1,

                               //// Hobbies

                               UsoMP3 = __sql.Count == 0 ? string.Empty : __sql.Find(p => p.v_ComponentFieldId == Constants.AUDIOMETRIA_HOBBIES_USO_MP3).v_Value1,
                               PracticaTiro = __sql.Count == 0 ? string.Empty : __sql.Find(p => p.v_ComponentFieldId == Constants.AUDIOMETRIA_HOBBIES_PRACTICA_TIRO).v_Value1,
                               Otros = __sql.Count == 0 ? string.Empty : __sql.Find(p => p.v_ComponentFieldId == Constants.AUDIOMETRIA_HOBBIES_OTROS).v_Value1,

                               //// Sintomas actuales

                               Sordera = __sql.Count == 0 ? string.Empty : __sql.Find(p => p.v_ComponentFieldId == Constants.AUDIOMETRIA_SINTOMAS_ACTUALES_SORDERA).v_Value1,
                               Otalgia = __sql.Count == 0 ? string.Empty : __sql.Find(p => p.v_ComponentFieldId == Constants.AUDIOMETRIA_SINTOMAS_ACTUALES_OTALGIA).v_Value1,
                               Acufenos = __sql.Count == 0 ? string.Empty : __sql.Find(p => p.v_ComponentFieldId == Constants.AUDIOMETRIA_SINTOMAS_ACTUALES_ACUFENOS).v_Value1,
                               SecrecionOtica = __sql.Count == 0 ? string.Empty : __sql.Find(p => p.v_ComponentFieldId == Constants.AUDIOMETRIA_SINTOMAS_ACTUALES_SECRECION_OTICA).v_Value1,
                               Vertigos = __sql.Count == 0 ? string.Empty : __sql.Find(p => p.v_ComponentFieldId == Constants.AUDIOMETRIA_SINTOMAS_ACTUALES_VERTIGOS).v_Value1,

                               //// Otoscopia

                               OidoIzquierdo = __sql.Count == 0 ? string.Empty : __sql.Find(p => p.v_ComponentFieldId == Constants.AUDIOMETRIA_OTOSCOPIA_OIDO_IZQUIERDO).v_Value1,
                               OidoDerecho = __sql.Count == 0 ? string.Empty : __sql.Find(p => p.v_ComponentFieldId == Constants.AUDIOMETRIA_OTOSCOPIA_OIDO_DERECHO).v_Value1,

                               b_Logo = MedicalCenter.b_Image,
                               EmpresaPropietaria = MedicalCenter.v_Name,
                               EmpresaPropietariaDireccion = MedicalCenter.v_Address,
                               EmpresaPropietariaTelefono = MedicalCenter.v_PhoneNumber,
                               EmpresaPropietariaEmail = MedicalCenter.v_Mail,

                               //
                               v_EsoTypeName = a.v_EsoTypeName,
                               v_ServiceComponentId = a.v_ServiceComponentId,
                               v_WorkingOrganizationName = a.v_WorkingOrganizationName,
                               v_FullWorkingOrganizationName = a.v_FullWorkingOrganizationName,
                               FirmaTrabajador = a.FirmaTrabajador,
                               HuellaTrabajador = a.HuellaTrabajador,
                               MarcaAudiometria = __sql.Count == 0 ? string.Empty : __sql.Find(p => p.v_ComponentFieldId == Constants.AUDIOMETRIA_DATOS_DEL_AUDIOMETRO_MARCA).v_Value1,
                               ModeloAudiometria = __sql.Count == 0 ? string.Empty : __sql.Find(p => p.v_ComponentFieldId == Constants.AUDIOMETRIA_DATOS_DEL_AUDIOMETRO_MODELO).v_Value1,
                               CalibracionAudiometria = __sql.Count == 0 ? string.Empty : __sql.Find(p => p.v_ComponentFieldId == Constants.AUDIOMETRIA_DATOS_DEL_AUDIOMETRO_CALIBRACION).v_Value1,
                               TiempoTrabajo = __sql.Count == 0 ? string.Empty : __sql.Find(p => p.v_ComponentFieldId == Constants.AUDIOMETRIA_REQUISITOS_TIEMPO_DE_TRABAJO).v_Value1,

                           }).ToList();

                return sql;
            }
            catch (Exception)
            {

                throw;
            }
        }

        // Alejandro
        public List<Sigesoft.Node.WinClient.BE.ServiceComponentFieldValuesList> ValoresComponentesUserControl(string pstrServiceId, string pstrComponentId)
        {
            SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
            int rpta = 0;

            try
            {
                var serviceComponentFieldValues = (from A in dbContext.service
                                                   join B in dbContext.servicecomponent on A.v_ServiceId equals B.v_ServiceId
                                                   join C in dbContext.servicecomponentfields on B.v_ServiceComponentId equals C.v_ServiceComponentId
                                                   join D in dbContext.servicecomponentfieldvalues on C.v_ServiceComponentFieldsId equals D.v_ServiceComponentFieldsId

                                                   where A.v_ServiceId == pstrServiceId
                                                           && B.v_ComponentId == pstrComponentId
                                                           && B.i_IsDeleted == 0
                                                           && C.i_IsDeleted == 0

                                                   select new Sigesoft.Node.WinClient.BE.ServiceComponentFieldValuesList
                                                   {
                                                       v_ComponentFieldId = C.v_ComponentFieldId,
                                                       //v_ComponentFielName = G.v_TextLabel,
                                                       v_ServiceComponentFieldsId = C.v_ServiceComponentFieldsId,
                                                       v_Value1 = D.v_Value1,
                                                       //i_GroupId = G.i_GroupId.Value
                                                   });


                return serviceComponentFieldValues.ToList();
            }
            catch (Exception)
            {

                throw;
            }

        }

        public Sigesoft.Node.WinClient.BE.ServiceList GetDoctoPhisicalExam(string serviceId)
        {
            using (SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel())
            {
                var sql = (from x in dbContext.servicecomponent
                           join me in dbContext.systemuser on x.i_ApprovedUpdateUserId equals me.i_SystemUserId into me_join
                           from me in me_join.DefaultIfEmpty()

                           join Pe in dbContext.person on me.v_PersonId equals Pe.v_PersonId

                           join pr in dbContext.professional on Pe.v_PersonId equals pr.v_PersonId into pr_join
                           from pr in pr_join.DefaultIfEmpty()

                           where x.v_ComponentId == Constants.EXAMEN_FISICO_ID &&
                                 x.v_ServiceId == serviceId

                           select new Sigesoft.Node.WinClient.BE.ServiceList
                           {
                               v_Pacient = Pe.v_FirstName + " " + Pe.v_FirstLastName + " " + Pe.v_SecondLastName,
                               FirmaDoctor = pr.b_SignatureImage

                           }).FirstOrDefault();

                return sql;
            }
        }

        private string GetRestricctionByServiceId(string pstrServiceId)
        {
            SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
            var query = (from ccc in dbContext.restriction
                         join ddd in dbContext.masterrecommendationrestricction on ccc.v_MasterRestrictionId equals ddd.v_MasterRecommendationRestricctionId  // Diagnosticos      
                         where ccc.v_ServiceId == pstrServiceId &&
                               ccc.i_IsDeleted == 0
                         select new
                         {
                             v_DiseasesName = ddd.v_Name

                         }).ToList();


            return string.Join("/ ", query.Select(p => p.v_DiseasesName));
        }

        public string GetDiagnosticForAudiometria(string pstrServiceId, string pstrComponent)
        {
            SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
            var query = (from ccc in dbContext.diagnosticrepository
                         join ddd in dbContext.diseases on ccc.v_DiseasesId equals ddd.v_DiseasesId into ddd_join
                         from ddd in ddd_join.DefaultIfEmpty()

                         where ccc.v_ServiceId == pstrServiceId && ccc.v_ComponentId == pstrComponent &&
                               ccc.i_IsDeleted == 0
                         select new
                         {
                             v_DiseasesName = ddd.v_Name
                         }).ToList();


            return string.Join(", ", query.Select(p => p.v_DiseasesName));
        }

        public void ActualizarAptitudServicio(ref OperationResult pobjOperationResult, string ServiceId, int AptitudeStatusId, List<string> ClientSession, string pstrComentario, string pstrComentarioMedico, string pstrRestricciones)
        {
            try
            {

                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                // Obtener la entidad fuente
                var objEntitySource = (from a in dbContext.service
                                       where a.v_ServiceId == ServiceId
                                       select a).FirstOrDefault();


                objEntitySource.i_AptitudeStatusId = AptitudeStatusId;
                objEntitySource.v_ObsStatusService = pstrComentario;
                //objEntitySource.v_ComentarioMedico = pstrComentarioMedico;
                //objEntitySource.v_ComentarioRestricciones = pstrRestricciones;

                objEntitySource.d_UpdateDate = DateTime.Now;
                objEntitySource.i_UpdateUserId = Int32.Parse(ClientSession[2]);
                objEntitySource.i_UpdateUserOccupationalMedicaltId = Int32.Parse(ClientSession[2]);
                // Guardar los cambios
                dbContext.SaveChanges();
                pobjOperationResult.Success = 1;
                return;

            }
            catch (Exception ex)
            {

                pobjOperationResult.Success = 0;
                pobjOperationResult.ExceptionMessage = ex.Message;
                return;
            }
        }
       
        public void ActualizarEstadoComponente(ref OperationResult pobjOperationResult, string pstrServiceComponentId, int pintStatusComponentId, List<string> ClientSession)
        {
            try
            {

                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                // Obtener la entidad fuente
                var objEntitySource = (from a in dbContext.servicecomponent
                                       where a.v_ServiceComponentId == pstrServiceComponentId
                                       select a).FirstOrDefault();


                objEntitySource.i_ServiceComponentStatusId = pintStatusComponentId;
                objEntitySource.i_ApprovedUpdateUserId = Int32.Parse(ClientSession[2]);
                objEntitySource.d_UpdateDate = DateTime.Now;
                objEntitySource.i_UpdateUserId = Int32.Parse(ClientSession[2]);
                // Guardar los cambios
                dbContext.SaveChanges();
                pobjOperationResult.Success = 1;
                return;

            }
            catch (Exception ex)
            {

                pobjOperationResult.Success = 0;
                pobjOperationResult.ExceptionMessage = ex.Message;
                return;
            }
        }

        public void ActualizarEstadoComponentesPorCategoria(ref OperationResult pobjOperationResult, int pintCategoriaI, string pstrServiceId, int pintStatusComponentId, List<string> ClientSession)
        {
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                // Obtener la entidad fuente
                var ListobjEntitySource = (from a in dbContext.servicecomponent
                                       join b in dbContext.component on a.v_ComponentId equals  b.v_ComponentId
                                       where a.v_ServiceId == pstrServiceId && b.i_CategoryId == pintCategoriaI
                                       select a).ToList();

                foreach (var item in ListobjEntitySource)
                {
                    item.i_ServiceComponentStatusId = pintStatusComponentId;
                    item.i_ApprovedUpdateUserId = Int32.Parse(ClientSession[2]);
                    item.d_UpdateDate = DateTime.Now;
                    item.i_UpdateUserId = Int32.Parse(ClientSession[2]);
                    // Guardar los cambios
                    dbContext.SaveChanges();
                    pobjOperationResult.Success = 1;
                }
              
                return;

            }
            catch (Exception ex)
            {

                pobjOperationResult.Success = 0;
                pobjOperationResult.ExceptionMessage = ex.Message;
                return;
            }
        }


        public List<masterrecommendationrestricctionList> DevolverTodasRecomendacionesRestricciones(int TypifyingId, string pBusqueda)
        {
            //mon.IsActive = true;

            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
                var query = from A in dbContext.masterrecommendationrestricction
                            where A.i_TypifyingId == TypifyingId && A.v_Name.Contains(pBusqueda)
                            select new masterrecommendationrestricctionList
                            {
                                v_MasterRecommendationRestricctionId = A.v_MasterRecommendationRestricctionId,
                                v_Name = A.v_Name,
                                i_TypifyingId = A.i_TypifyingId.Value,

                            };

                List<masterrecommendationrestricctionList> objData = query.ToList();
                return objData;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public void AgregarRestriccion(ref OperationResult pobjOperationResult, restrictionDto pobjDtoEntity, List<string> ClientSession)
        {
            //mon.IsActive = true;
            string NewId = "(No generado)";
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
                restriction objEntity = restrictionAssembler.ToEntity(pobjDtoEntity);

                objEntity.d_InsertDate = DateTime.Now;
                objEntity.i_InsertUserId = Int32.Parse(ClientSession[2]);
                objEntity.i_IsDeleted = 0;
                // Autogeneramos el Pk de la tabla                 
                int intNodeId = int.Parse(ClientSession[0]);
                NewId = Common.Utils.GetNewId(intNodeId, Utils.GetNextSecuentialId(intNodeId, 30), "RD");
                objEntity.v_RestrictionId = NewId;

                dbContext.AddTorestriction(objEntity);
                dbContext.SaveChanges();

                pobjOperationResult.Success = 1;
                // Llenar entidad Log
                return;
            }
            catch (Exception ex)
            {
                pobjOperationResult.Success = 0;
                pobjOperationResult.ExceptionMessage = Common.Utils.ExceptionFormatter(ex);
                // Llenar entidad Log
                return;
            }
        }

        public List<Sigesoft.Node.WinClient.BE.DxFrecuenteDetalleList> ObtenerListaRecomendacionesPorDxFrecuente(string pDxFrecuenteId, int ptipo)
        {
            SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
            var query = (from A in dbContext.dxfrecuentedetalle
                         join B in dbContext.masterrecommendationrestricction on A.v_MasterRecommendationRestricctionId equals B.v_MasterRecommendationRestricctionId
                         where A.v_DxFrecuenteId == pDxFrecuenteId && B.i_TypifyingId == ptipo
                         select new Sigesoft.Node.WinClient.BE.DxFrecuenteDetalleList
                         {
                             v_DxFrecuenteDetalleId = A.v_DxFrecuenteDetalleId,
                             v_DxFrecuenteId = A.v_DxFrecuenteId,
                             v_MasterRecommendationRestricctionId = B.v_MasterRecommendationRestricctionId
                         }).ToList();

            return query;
        }

        public string AddMasterRecommendationRestricction(ref OperationResult pobjOperationResult, masterrecommendationrestricctionDto pobjDtoEntity, List<string> ClientSession)
        {
            //mon.IsActive = true;
            string NewId = "(No generado)";
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
                masterrecommendationrestricction objEntity = masterrecommendationrestricctionAssembler.ToEntity(pobjDtoEntity);

                objEntity.d_InsertDate = DateTime.Now;
                objEntity.i_InsertUserId = Int32.Parse(ClientSession[2]);
                objEntity.i_IsDeleted = 0;
                // Autogeneramos el Pk de la tabla                 
                int intNodeId = int.Parse(ClientSession[0]);
                NewId = Common.Utils.GetNewId(intNodeId, Utils.GetNextSecuentialId(intNodeId, 43), "MR"); ;
                objEntity.v_MasterRecommendationRestricctionId = NewId;

                dbContext.AddTomasterrecommendationrestricction(objEntity);
                dbContext.SaveChanges();

                pobjOperationResult.Success = 1;
                // Llenar entidad Log
                LogBL.SaveLog(ClientSession[0], ClientSession[1], ClientSession[2], LogEventType.CREACION, "RECOMENDACIÓN / RESTRICCIÓN", "v_MasterRecommendationRestricctionId=" + NewId.ToString(), Success.Ok, null);
                return NewId;
            }
            catch (Exception ex)
            {
                pobjOperationResult.Success = 0;
                pobjOperationResult.ExceptionMessage = Common.Utils.ExceptionFormatter(ex);
                // Llenar entidad Log
                LogBL.SaveLog(ClientSession[0], ClientSession[1], ClientSession[2], LogEventType.CREACION, "RECOMENDACIÓN / RESTRICCIÓN", "v_MasterRecommendationRestricctionId=" + NewId.ToString(), Success.Failed, pobjOperationResult.ExceptionMessage);
                return null;
            }
        }

        public List<KeyValueDTO> DevolverExamenesPorCategoria(string pServideId, int pCategoriaID)
        {
            //mon.IsActive = true;
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
                var DataComponentList = (from a in dbContext.servicecomponent
                                         join b in dbContext.component on a.v_ComponentId equals b.v_ComponentId
                                         //join B in dbContext.systemparameter on new { a = a.i_CategoryId.Value, b = 116 } equals new { a = B.i_ParameterId, b = B.i_GroupId } into B_join
                                         //from B in B_join.DefaultIfEmpty()
                                         where a.i_IsDeleted == 0 && b.i_CategoryId == pCategoriaID && a.v_ServiceId == pServideId
                                         select new KeyValueDTO
                                         {
                                             Id = a.v_ComponentId,
                                             Value1 = b.v_Name
                                         }).ToList();


                List<KeyValueDTO> objData = DataComponentList.ToList();
                return objData;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public void EliminarRecomendacion(ref OperationResult pobjOperationResult, string pRecomendationId)
        {
            //mon.IsActive = true;

            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                var objEntity = (from a in dbContext.recommendation
                                 where a.v_RecommendationId == pRecomendationId
                                 select a).FirstOrDefault();

                if (objEntity != null)
                {
                    dbContext.DeleteObject(objEntity);
                    dbContext.SaveChanges();
                }

                pobjOperationResult.Success = 1;
                // Llenar entidad Log
                return;
            }
            catch (Exception ex)
            {
                pobjOperationResult.Success = 0;
                pobjOperationResult.ExceptionMessage = ex.Message;
                // Llenar entidad Log
                return;
            }
        }

        public void EliminarRestriccion(ref OperationResult pobjOperationResult, string pRestriccionId)
        {
            //mon.IsActive = true;

            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                var objEntity = (from a in dbContext.restriction
                                 where a.v_RestrictionId == pRestriccionId
                                 select a).FirstOrDefault();

                if (objEntity != null)
                {
                    dbContext.DeleteObject(objEntity);
                    dbContext.SaveChanges();
                }

                pobjOperationResult.Success = 1;
                // Llenar entidad Log
                return;
            }
            catch (Exception ex)
            {
                pobjOperationResult.Success = 0;
                pobjOperationResult.ExceptionMessage = ex.Message;
                // Llenar entidad Log
                return;
            }
        }

        public List<KeyValueDTO> GetAllComponents(ref OperationResult pobjOperationResult)
        {
            //mon.IsActive = true;
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
                var DataComponentList = (from a in dbContext.component
                                         join B in dbContext.systemparameter on new { a = a.i_CategoryId.Value, b = 116 } equals new { a = B.i_ParameterId, b = B.i_GroupId } into B_join
                                         from B in B_join.DefaultIfEmpty()
                                         where a.i_IsDeleted == 0 &&
                                               a.i_ComponentTypeId == 1
                                         select new KeyValueDTO
                                         {
                                             Value4 = a.i_CategoryId.Value,//i_CategoryId
                                             Value1 = a.i_CategoryId.Value == -1 ? a.v_Name : B.v_Value1, //CategoryName
                                             Value2 = a.v_ComponentId, // ComponentId
                                             Value3 = a.v_Name, // v_Name
                                             //Id = a.v_ComponentId
                                         }).ToList();


                List<KeyValueDTO> objData = DataComponentList.ToList();
                pobjOperationResult.Success = 1;
                return objData;
            }
            catch (Exception ex)
            {
                pobjOperationResult.Success = 0;
                pobjOperationResult.ExceptionMessage = ex.Message;
                return null;
            }
        }

        public List<Sigesoft.Node.WinClient.BE.DxFrecuenteList> GetDxFrecuentes(ref OperationResult pobjOperationResult, int? pintPageIndex, int? pintResultsPerPage, string pstrSortExpression, string pstrFilterExpression)
        {
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
                var query = from A in dbContext.dxfrecuente
                            join B in dbContext.dxfrecuentedetalle on A.v_DxFrecuenteId equals B.v_DxFrecuenteId
                            join C in dbContext.diseases on A.v_DiseasesId equals C.v_DiseasesId
                            join D in dbContext.masterrecommendationrestricction on B.v_MasterRecommendationRestricctionId equals D.v_MasterRecommendationRestricctionId
                            where A.i_IsDeleted == 0
                            select new Sigesoft.Node.WinClient.BE.DxFrecuenteList
                            {
                                v_DxFrecuenteId = A.v_DxFrecuenteId,
                                v_DiseasesId = A.v_DiseasesId,
                                v_DiseasesName = C.v_Name,
                                v_CIE10Id = A.v_CIE10Id,
                                v_DxFrecuenteDetalleId = B.v_DxFrecuenteDetalleId,
                                v_MasterRecommendationRestricctionId = B.v_MasterRecommendationRestricctionId,
                                v_MasterRecommendationRestricctionName = D.v_Name,
                                i_Tipo = D.i_TypifyingId.Value,
                                v_CIE10Idv_Name = A.v_CIE10Id + " " + C.v_Name
                            };

                if (!string.IsNullOrEmpty(pstrFilterExpression))
                {
                    query = query.Where(pstrFilterExpression);
                }

                List<Sigesoft.Node.WinClient.BE.DxFrecuenteList> objData = query.ToList();

                List<Sigesoft.Node.WinClient.BE.DxFrecuenteList> QueryFinal = new List<Sigesoft.Node.WinClient.BE.DxFrecuenteList>();


                //Transformar Data
                //Agrupar Data para DXS
                var DxFrecunetes = (objData.GroupBy(g => new { g.v_DiseasesId })
                               .Select(s => s.First()).ToList()).FindAll(p => p.v_DiseasesName != null);

                Sigesoft.Node.WinClient.BE.DxFrecuenteList oDxFrecuente;
                List<Sigesoft.Node.WinClient.BE.DxFrecuenteDetalleList> ListaDxFrecuenteDetalleList;

                foreach (var Dx in DxFrecunetes)
                {
                    oDxFrecuente = new Sigesoft.Node.WinClient.BE.DxFrecuenteList();
                    oDxFrecuente.v_DxFrecuenteId = Dx.v_DxFrecuenteId;
                    oDxFrecuente.v_DiseasesId = Dx.v_DiseasesId;
                    oDxFrecuente.v_DiseasesName = Dx.v_DiseasesName;
                    oDxFrecuente.v_CIE10Id = Dx.v_CIE10Id;

                    var ListaRecomendacionesRestricciones = objData.FindAll(p => p.v_DxFrecuenteId == Dx.v_DxFrecuenteId);
                    ListaDxFrecuenteDetalleList = new List<Sigesoft.Node.WinClient.BE.DxFrecuenteDetalleList>();
                    Sigesoft.Node.WinClient.BE.DxFrecuenteDetalleList oDxFrecuenteDetalleList;
                    foreach (var item in ListaRecomendacionesRestricciones)
                    {
                        oDxFrecuenteDetalleList = new Sigesoft.Node.WinClient.BE.DxFrecuenteDetalleList();
                        oDxFrecuenteDetalleList.v_DxFrecuenteDetalleId = item.v_DxFrecuenteDetalleId;
                        oDxFrecuenteDetalleList.v_DxFrecuenteId = item.v_DxFrecuenteId;
                        oDxFrecuenteDetalleList.v_MasterRecommendationRestricctionId = item.v_MasterRecommendationRestricctionId;
                        oDxFrecuenteDetalleList.i_Tipo = item.i_Tipo;
                        if (item.i_Tipo == 1)
                        {
                            oDxFrecuenteDetalleList.v_RecomendacionName = item.v_MasterRecommendationRestricctionName;
                        }
                        else
                        {
                            oDxFrecuenteDetalleList.v_RestriccionName = item.v_MasterRecommendationRestricctionName;
                        }

                        ListaDxFrecuenteDetalleList.Add(oDxFrecuenteDetalleList);
                    }
                    oDxFrecuente.DxFrecuenteDetalle = ListaDxFrecuenteDetalleList;

                    QueryFinal.Add(oDxFrecuente);
                }


                pobjOperationResult.Success = 1;
                return QueryFinal;
            }
            catch (Exception ex)
            {
                pobjOperationResult.Success = 0;
                pobjOperationResult.ExceptionMessage = Common.Utils.ExceptionFormatter(ex);
                return null;
            }
        }

        public List<DiagnosticRepositoryList> GetServiceComponentDisgnosticsByServiceId(ref OperationResult pobjOperationResult, string pstrServiceId)
        {

            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                var query = (from ccc in dbContext.diagnosticrepository

                             join bbb in dbContext.component on ccc.v_ComponentId equals bbb.v_ComponentId into J7_join
                             from bbb in J7_join.DefaultIfEmpty()

                             join sc in dbContext.servicecomponent on new { a = ccc.v_ComponentId, b = pstrServiceId }
                                                                    equals new { a = sc.v_ComponentId, b = sc.v_ServiceId } into sc_join
                             from sc in sc_join.DefaultIfEmpty()

                             join ddd in dbContext.diseases on ccc.v_DiseasesId equals ddd.v_DiseasesId  // Diagnosticos

                             join eee in dbContext.systemparameter on new { a = ccc.i_AutoManualId.Value, b = 136 } // Auto / Manual
                                                                     equals new { a = eee.i_ParameterId, b = eee.i_GroupId } into J8_join
                             from eee in J8_join.DefaultIfEmpty()

                             join zzz in dbContext.systemparameter on new { a = bbb.i_CategoryId.Value, b = 116 } // CATEGORIA DEL EXAMEN
                                                                 equals new { a = zzz.i_ParameterId, b = zzz.i_GroupId } into J9_join
                             from zzz in J9_join.DefaultIfEmpty()

                             join fff in dbContext.systemparameter on new { a = ccc.i_PreQualificationId.Value, b = 137 } // PRE-CALIFICACION
                                                                 equals new { a = fff.i_ParameterId, b = fff.i_GroupId } into J5_join
                             from fff in J5_join.DefaultIfEmpty()

                             join ggg in dbContext.systemparameter on new { a = ccc.i_FinalQualificationId.Value, b = 138 } //CALIFICACION FINAL
                                                                 equals new { a = ggg.i_ParameterId, b = ggg.i_GroupId } into J4_join
                             from ggg in J4_join.DefaultIfEmpty()

                             join hhh in dbContext.systemparameter on new { a = ccc.i_DiagnosticTypeId.Value, b = 139 } // TIPO DE DX [Enfermedad comun, etc]
                                                                     equals new { a = hhh.i_ParameterId, b = hhh.i_GroupId } into J3_join
                             from hhh in J3_join.DefaultIfEmpty()

                             join iii in dbContext.systemparameter on new { a = ccc.i_IsSentToAntecedent.Value, b = 111 } // RESPUESTA SI/NO
                                                                 equals new { a = iii.i_ParameterId, b = iii.i_GroupId } into J6_join
                             from iii in J6_join.DefaultIfEmpty()

                             //join J1 in dbContext.systemuser on new { i_InsertUserId = ccc.i_InsertUserId.Value }
                             //                equals new { i_InsertUserId = J1.i_SystemUserId } into J1_join
                             //from J1 in J1_join.DefaultIfEmpty()

                             join J2 in dbContext.systemuser on new { i_UpdateUserId = sc.i_ApprovedUpdateUserId.Value }
                                                             equals new { i_UpdateUserId = J2.i_SystemUserId } into J2_join
                             from J2 in J2_join.DefaultIfEmpty()

                             where ccc.v_ServiceId == pstrServiceId &&
                                     ccc.i_IsDeleted == 0
                             orderby bbb.v_Name

                             select new DiagnosticRepositoryList
                             {
                                 v_DiagnosticRepositoryId = ccc.v_DiagnosticRepositoryId,
                                 v_ServiceId = ccc.v_ServiceId,
                                 v_ComponentId = ccc.v_ComponentId,
                                 v_DiseasesId = ccc.v_DiseasesId,
                                 i_AutoManualId = ccc.i_AutoManualId,
                                 i_PreQualificationId = ccc.i_PreQualificationId,
                                 i_FinalQualificationId = ccc.i_FinalQualificationId,
                                 i_DiagnosticTypeId = ccc.i_DiagnosticTypeId,
                                 i_IsSentToAntecedent = ccc.i_IsSentToAntecedent,
                                 d_ExpirationDateDiagnostic = ccc.d_ExpirationDateDiagnostic,
                                 i_GenerateMedicalBreak = ccc.i_GenerateMedicalBreak,
                                 i_ServiceComponentStatusId = sc.i_ServiceComponentStatusId,
                                 v_DiseasesName = ddd.v_Name,
                                 v_AutoManualName = eee.v_Value1,
                                 v_ServiceComponentId = sc.v_ServiceComponentId,
                                 //v_ComponentName = bbb.v_Name,
                                 //v_ComponentName = (ccc.i_AutoManualId == (int)AutoManual.Manual && bbb.i_CategoryId != -1) ? zzz.v_Value1 : bbb.v_Name,
                                 v_ComponentName = bbb.i_CategoryId == -1 ? bbb.v_Name : zzz.v_Value1,
                                 v_PreQualificationName = fff.v_Value1,
                                 v_FinalQualificationName = ggg.v_Value1,
                                 v_DiagnosticTypeName = hhh.v_Value1,
                                 v_IsSentToAntecedentName = iii.v_Value1,
                                 i_RecordStatus = (int)RecordStatus.Grabado,
                                 i_RecordType = (int)RecordType.NoTemporal,

                                 v_UpdateUser = J2.v_UserName,

                                 d_UpdateDate = J2.d_UpdateDate,
                                 i_IsDeleted = ccc.i_IsDeleted.Value

                             }).ToList();


                var q = (from a in query
                         select new DiagnosticRepositoryList
                         {
                             v_DiagnosticRepositoryId = a.v_DiagnosticRepositoryId,
                             v_ServiceId = a.v_ServiceId,
                             v_ComponentId = a.v_ComponentId,
                             v_DiseasesId = a.v_DiseasesId,
                             i_AutoManualId = a.i_AutoManualId,
                             i_PreQualificationId = a.i_PreQualificationId,
                             i_FinalQualificationId = a.i_FinalQualificationId,
                             i_DiagnosticTypeId = a.i_DiagnosticTypeId,
                             i_IsSentToAntecedent = a.i_IsSentToAntecedent,
                             d_ExpirationDateDiagnostic = a.d_ExpirationDateDiagnostic,
                             i_GenerateMedicalBreak = a.i_GenerateMedicalBreak,
                             i_ServiceComponentStatusId =a.i_ServiceComponentStatusId,
                             v_RestrictionsName = ConcatenateRestriction(a.v_DiagnosticRepositoryId),
                             v_RecomendationsName = ConcatenateRecommendation(a.v_DiagnosticRepositoryId),
                             v_DiseasesName = a.v_DiseasesName,
                             v_AutoManualName = a.v_AutoManualName,
                             v_ComponentName = a.v_ComponentName,
                             v_ServiceComponentId = a.v_ServiceComponentId,
                             v_PreQualificationName = a.v_PreQualificationName,
                             v_FinalQualificationName = a.v_FinalQualificationName,
                             v_DiagnosticTypeName = a.v_DiagnosticTypeName,
                             v_IsSentToAntecedentName = a.v_IsSentToAntecedentName,
                             i_RecordStatus = a.i_RecordStatus,
                             i_RecordType = a.i_RecordType,

                             v_UpdateUser = a.v_UpdateUser,

                             d_UpdateDate = a.d_UpdateDate,
                             i_IsDeleted = a.i_IsDeleted

                         }).ToList();

                //List<DiagnosticRepositoryList> objData = query.ToList();
                pobjOperationResult.Success = 1;
                return q;
            }
            catch (Exception ex)
            {
                pobjOperationResult.Success = 0;
                pobjOperationResult.ExceptionMessage = Common.Utils.ExceptionFormatter(ex);
                return null;
            }
        }

        public List<ServiceList> GetServicesForAuditoriaEvaluador(ref OperationResult pobjOperationResult, int? pintPageIndex, int? pintResultsPerPage, string pstrSortExpression, string pstrFilterExpression, DateTime? pdatBeginDate, DateTime? pdatEndDate, List<string> ComponentesId)
        {
            //mon.IsActive = true;

            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
                var query = from A in dbContext.service

                            join I in dbContext.person on A.v_PersonId equals I.v_PersonId

                            join K in dbContext.systemparameter on new { a = A.i_AptitudeStatusId.Value, b = 124 } equals new { a = K.i_ParameterId, b = K.i_GroupId } into K_join
                            from K in K_join.DefaultIfEmpty()

                            join E in dbContext.protocol on A.v_ProtocolId equals E.v_ProtocolId

                            join F in dbContext.groupoccupation on E.v_GroupOccupationId equals F.v_GroupOccupationId

                            join J in dbContext.systemparameter on new { a = I.i_SexTypeId.Value, b = 100 }
                                            equals new { a = J.i_ParameterId, b = J.i_GroupId } into J_join // GENERO
                            from J in J_join.DefaultIfEmpty()

                            join H in dbContext.systemparameter on new { a = E.i_EsoTypeId.Value, b = 118 }
                                                equals new { a = H.i_ParameterId, b = H.i_GroupId }  // TIPO ESO [ESOA,ESOR,ETC]

                            join M in dbContext.servicecomponent on A.v_ServiceId equals M.v_ServiceId into M_join
                            from M in M_join.DefaultIfEmpty()

                            // Empresa / Sede Cliente ******************************************************
                            join oc in dbContext.organization on new { a = E.v_CustomerOrganizationId }
                                    equals new { a = oc.v_OrganizationId } into oc_join
                            from oc in oc_join.DefaultIfEmpty()

                            join lc in dbContext.location on new { a = E.v_CustomerOrganizationId, b = E.v_CustomerLocationId }
                                  equals new { a = lc.v_OrganizationId, b = lc.v_LocationId } into lc_join
                            from lc in lc_join.DefaultIfEmpty()

                            //**********************************************************************************

                            where A.i_IsDeleted == 0
                            && M.i_ServiceComponentStatusId == (int)ServiceComponentStatus.PorAprobacion || M.i_ServiceComponentStatusId == (int)ServiceComponentStatus.Especialista
                            select new ServiceList
                            {
                                v_IdTrabajador = A.v_PersonId,
                                v_ServiceId = A.v_ServiceId,
                                v_ProtocolId = A.v_ProtocolId,
                                v_PersonId = A.v_PersonId,
                                i_AptitudeStatusId = A.i_AptitudeStatusId.Value,
                                d_ServiceDate = (DateTime)A.d_ServiceDate,
                                v_Pacient = I.v_FirstLastName + " " + I.v_SecondLastName + " " + I.v_FirstName,
                                v_ProtocolName = E.v_Name,
                                v_CustomerOrganizationId = E.v_CustomerOrganizationId,
                                v_CustomerLocationId = lc.v_LocationId,
                                EmpresaCliente = oc.v_Name,
                                d_FechaNacimiento = I.d_Birthdate,
                                v_Genero = J.v_Value1,
                                v_TipoEso = H.v_Value1,
                                v_GrupoRiesgo = F.v_Name,
                                v_Puesto = I.v_CurrentOccupation,
                                v_ComponentId = M.v_ComponentId,
                                v_ObsStatusService = A.v_ObsStatusService,
                                Dni = I.v_DocNumber

                            };

                if (!string.IsNullOrEmpty(pstrFilterExpression))
                {
                    query = query.Where(pstrFilterExpression);
                }
                if (pdatBeginDate.HasValue && pdatEndDate.HasValue)
                {
                    query = query.Where("d_ServiceDate >= @0 && d_ServiceDate <= @1", pdatBeginDate.Value, pdatEndDate.Value);
                }
                if (!string.IsNullOrEmpty(pstrSortExpression))
                {
                    query = query.OrderBy(pstrSortExpression);
                }

                List<ServiceList> objData = query.ToList();
                pobjOperationResult.Success = 1;

                if (ComponentesId != null)
                {
                    var x = objData.FindAll(p => ComponentesId.Contains(p.v_ComponentId));

                    var y = x.GroupBy(g => g.v_ServiceId)
                                  .Select(s => s.First());
                    pobjOperationResult.Success = 1;
                    return y.ToList();
                }
                else
                {
                    var final = objData.GroupBy(g => g.v_ServiceId)
                               .Select(s => s.First());
                    pobjOperationResult.Success = 1;
                    return final.ToList();

                }

                



            }
            catch (Exception ex)
            {
                pobjOperationResult.Success = 0;
                pobjOperationResult.ExceptionMessage = Common.Utils.ExceptionFormatter(ex);
                return null;
            }
        }

        public List<ServiceList> GetServicesForAuditoriaAuditor(ref OperationResult pobjOperationResult, int? pintPageIndex, int? pintResultsPerPage, string pstrSortExpression, string pstrFilterExpression, DateTime? pdatBeginDate, DateTime? pdatEndDate, List<string> ComponentesId)
        {
            //mon.IsActive = true;

            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
                var query = from A in dbContext.service

                            join I in dbContext.person on A.v_PersonId equals I.v_PersonId

                            join K in dbContext.systemparameter on new { a = A.i_AptitudeStatusId.Value, b = 124 } equals new { a = K.i_ParameterId, b = K.i_GroupId } into K_join
                            from K in K_join.DefaultIfEmpty()

                            join E in dbContext.protocol on A.v_ProtocolId equals E.v_ProtocolId
                            join F in dbContext.groupoccupation on E.v_GroupOccupationId equals F.v_GroupOccupationId
                            join J in dbContext.systemparameter on new { a = I.i_SexTypeId.Value, b = 100 }
                                            equals new { a = J.i_ParameterId, b = J.i_GroupId } into J_join // GENERO
                            from J in J_join.DefaultIfEmpty()

                            join H in dbContext.systemparameter on new { a = E.i_EsoTypeId.Value, b = 118 }
                                                equals new { a = H.i_ParameterId, b = H.i_GroupId }  // TIPO ESO [ESOA,ESOR,ETC]

                            join M in dbContext.servicecomponent on A.v_ServiceId equals M.v_ServiceId into M_join
                            from M in M_join.DefaultIfEmpty()

                            join N in dbContext.component on M.v_ComponentId equals N.v_ComponentId into N_join
                            from N in N_join.DefaultIfEmpty()

                            // Empresa / Sede Cliente ******************************************************
                            join oc in dbContext.organization on new { a = E.v_CustomerOrganizationId }
                                    equals new { a = oc.v_OrganizationId } into oc_join
                            from oc in oc_join.DefaultIfEmpty()

                            join lc in dbContext.location on new { a = E.v_CustomerOrganizationId, b = E.v_CustomerLocationId }
                                  equals new { a = lc.v_OrganizationId, b = lc.v_LocationId } into lc_join
                            from lc in lc_join.DefaultIfEmpty()

                            //**********************************************************************************

                            where A.i_IsDeleted == 0
                            && A.i_ServiceStatusId == (int)ServiceStatus.EsperandoAptitud
                            select new ServiceList
                            {
                                v_IdTrabajador = A.v_PersonId,
                                v_ServiceId = A.v_ServiceId,
                                v_ProtocolId = A.v_ProtocolId,
                                v_PersonId = A.v_PersonId,
                                i_AptitudeStatusId = A.i_AptitudeStatusId.Value,
                                d_ServiceDate = (DateTime)A.d_ServiceDate,
                                v_Pacient = I.v_FirstLastName + " " + I.v_SecondLastName + " " + I.v_FirstName,
                                v_ProtocolName = E.v_Name,
                                v_CustomerOrganizationId = E.v_CustomerOrganizationId,
                                v_CustomerLocationId = lc.v_LocationId,
                                EmpresaCliente = oc.v_Name,
                                d_FechaNacimiento = I.d_Birthdate,
                                v_Genero = J.v_Value1,
                                v_TipoEso = H.v_Value1,
                                v_GrupoRiesgo = F.v_Name,
                                v_Puesto = I.v_CurrentOccupation,
                                v_ComponentId = M.v_ComponentId,
                                v_ObsStatusService = A.v_ObsStatusService,
                                Dni = I.v_DocNumber,
                                i_CategoryId = N.i_CategoryId.Value
                            };

                if (!string.IsNullOrEmpty(pstrFilterExpression))
                {
                    query = query.Where(pstrFilterExpression);
                }
                if (pdatBeginDate.HasValue && pdatEndDate.HasValue)
                {
                    query = query.Where("d_ServiceDate >= @0 && d_ServiceDate <= @1", pdatBeginDate.Value, pdatEndDate.Value);
                }
                if (!string.IsNullOrEmpty(pstrSortExpression))
                {
                    query = query.OrderBy(pstrSortExpression);
                }

                List<ServiceList> objData = query.ToList();
                pobjOperationResult.Success = 1;

                if (ComponentesId != null)
                {
                    var x = objData.FindAll(p => ComponentesId.Contains(p.v_ComponentId));

                    var y = x.GroupBy(g => g.v_ServiceId)
                                  .Select(s => s.First());
                    pobjOperationResult.Success = 1;
                    return y.ToList();
                }
                else
                {
                    var final = objData.GroupBy(g => g.v_ServiceId)
                               .Select(s => s.First());
                    pobjOperationResult.Success = 1;
                    return final.ToList();

                }





            }
            catch (Exception ex)
            {
                pobjOperationResult.Success = 0;
                pobjOperationResult.ExceptionMessage = Common.Utils.ExceptionFormatter(ex);
                return null;
            }
        }

        public string ObtenerRecomendacionesPorDxFrecuente(string pDxFrecuenteId, int ptipo)
        {
            SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
            var query = (from A in dbContext.dxfrecuentedetalle
                         join B in dbContext.masterrecommendationrestricction on A.v_MasterRecommendationRestricctionId equals B.v_MasterRecommendationRestricctionId
                         where A.v_DxFrecuenteId == pDxFrecuenteId && B.i_TypifyingId == ptipo
                         select new
                         {
                             Nombre = B.v_Name
                         }).ToList();


            return string.Join(", ", query.Select(p => p.Nombre));
        }

        public void UpdateDiseases(ref OperationResult pobjOperationResult, diseasesDto pobjDtoEntity, List<string> ClientSession)
        {
            //mon.IsActive = true;

            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                // Obtener la entidad fuente
                var objEntitySource = (from a in dbContext.diseases
                                       where a.v_DiseasesId == pobjDtoEntity.v_DiseasesId
                                       select a).FirstOrDefault();

                // Crear la entidad con los datos actualizados
                pobjDtoEntity.d_UpdateDate = DateTime.Now;
                pobjDtoEntity.i_UpdateUserId = Int32.Parse(ClientSession[2]);
                diseases objEntity = diseasesAssembler.ToEntity(pobjDtoEntity);

                // Copiar los valores desde la entidad actualizada a la Entidad Fuente
                dbContext.diseases.ApplyCurrentValues(objEntity);

                // Guardar los cambios
                dbContext.SaveChanges();

                pobjOperationResult.Success = 1;
                // Llenar entidad Log
                LogBL.SaveLog(ClientSession[0], ClientSession[1], ClientSession[2], LogEventType.ACTUALIZACION, "ENFERMEDAD", "v_Diseases=" + objEntity.v_DiseasesId.ToString(), Success.Ok, null);
                return;
            }
            catch (Exception ex)
            {
                pobjOperationResult.Success = 0;
                pobjOperationResult.ExceptionMessage = Common.Utils.ExceptionFormatter(ex);
                // Llenar entidad Log
                LogBL.SaveLog(ClientSession[0], ClientSession[1], ClientSession[2], LogEventType.ACTUALIZACION, "ENFERMEDAD", "v_Diseases=" + pobjDtoEntity.v_DiseasesId.ToString(), Success.Failed, pobjOperationResult.ExceptionMessage);
                return;
            }
        }

        public List<ServiceList> GetServicesForPrint(ref OperationResult pobjOperationResult, int? pintPageIndex, int? pintResultsPerPage, string pstrSortExpression, string pstrFilterExpression, DateTime? pdatBeginDate, DateTime? pdatEndDate)
        {
            //mon.IsActive = true;

            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
                var query = from A in dbContext.service

                            join I in dbContext.person on A.v_PersonId equals I.v_PersonId

                            join K in dbContext.systemparameter on new { a = A.i_AptitudeStatusId.Value, b = 124 } equals new { a = K.i_ParameterId, b = K.i_GroupId } into K_join
                            from K in K_join.DefaultIfEmpty()

                            join E in dbContext.protocol on A.v_ProtocolId equals E.v_ProtocolId

                            // Empresa / Sede Cliente ******************************************************
                            join oc in dbContext.organization on new { a = E.v_CustomerOrganizationId }
                                    equals new { a = oc.v_OrganizationId } into oc_join
                            from oc in oc_join.DefaultIfEmpty()

                            join lc in dbContext.location on new { a = E.v_CustomerOrganizationId, b = E.v_CustomerLocationId }
                                  equals new { a = lc.v_OrganizationId, b = lc.v_LocationId } into lc_join
                            from lc in lc_join.DefaultIfEmpty()

                            //**********************************************************************************

                            where A.i_IsDeleted == 0
                            && A.i_ServiceStatusId == (int)ServiceStatus.Culminado
                            select new ServiceList
                            {
                                v_IdTrabajador = A.v_PersonId,
                                v_ServiceId = A.v_ServiceId,
                                v_ProtocolId = A.v_ProtocolId,
                                v_PersonId = A.v_PersonId,
                                i_AptitudeStatusId = A.i_AptitudeStatusId.Value,
                                d_ServiceDate = (DateTime)A.d_ServiceDate,
                                v_Pacient = I.v_FirstLastName + " " + I.v_SecondLastName + " " + I.v_FirstName,
                                v_ProtocolName = E.v_Name,
                                v_CustomerOrganizationId = E.v_CustomerOrganizationId,
                                v_CustomerLocationId = lc.v_LocationId,
                                EmpresaCliente = oc.v_Name

                            };

                if (!string.IsNullOrEmpty(pstrFilterExpression))
                {
                    query = query.Where(pstrFilterExpression);
                }
                if (pdatBeginDate.HasValue && pdatEndDate.HasValue)
                {
                    query = query.Where("d_ServiceDate >= @0 && d_ServiceDate <= @1", pdatBeginDate.Value, pdatEndDate.Value);
                }
                if (!string.IsNullOrEmpty(pstrSortExpression))
                {
                    query = query.OrderBy(pstrSortExpression);
                }

                List<ServiceList> objData = query.ToList();
                pobjOperationResult.Success = 1;


                return objData;



            }
            catch (Exception ex)
            {
                pobjOperationResult.Success = 0;
                pobjOperationResult.ExceptionMessage = Common.Utils.ExceptionFormatter(ex);
                return null;
            }
        }

        public Sigesoft.Node.WinClient.BE.DiagnosticRepositoryList GetServiceComponentTotalDiagnostics(ref OperationResult pobjOperationResult, string pstrDiagnosticRepositoryId)
        {
            //mon.IsActive = true;

            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                Sigesoft.Node.WinClient.BE.DiagnosticRepositoryList query = (from ccc in dbContext.diagnosticrepository
                                                                             join bbb in dbContext.component on ccc.v_ComponentId equals bbb.v_ComponentId into J7_join
                                                                             from bbb in J7_join.DefaultIfEmpty()

                                                                             join ddd in dbContext.diseases on ccc.v_DiseasesId equals ddd.v_DiseasesId  // Diagnosticos

                                                                             join eee in dbContext.systemparameter on new { a = ccc.i_AutoManualId.Value, b = 136 } // Auto / Manual
                                                                                                                     equals new { a = eee.i_ParameterId, b = eee.i_GroupId } into J8_join
                                                                             from eee in J8_join.DefaultIfEmpty()

                                                                             join fff in dbContext.systemparameter on new { a = ccc.i_PreQualificationId.Value, b = 137 } // PRE-CALIFICACION
                                                                                                                 equals new { a = fff.i_ParameterId, b = fff.i_GroupId } into J5_join
                                                                             from fff in J5_join.DefaultIfEmpty()

                                                                             join ggg in dbContext.systemparameter on new { a = ccc.i_FinalQualificationId.Value, b = 138 } //CALIFICACION FINAL
                                                                                                                 equals new { a = ggg.i_ParameterId, b = ggg.i_GroupId } into J4_join
                                                                             from ggg in J4_join.DefaultIfEmpty()

                                                                             join hhh in dbContext.systemparameter on new { a = ccc.i_DiagnosticTypeId.Value, b = 139 } // TIPO DE DX [Enfermedad comun, etc]
                                                                                                                     equals new { a = hhh.i_ParameterId, b = hhh.i_GroupId } into J3_join
                                                                             from hhh in J3_join.DefaultIfEmpty()

                                                                             join iii in dbContext.systemparameter on new { a = ccc.i_IsSentToAntecedent.Value, b = 111 } // RESPUESTA SI/NO
                                                                                                                  equals new { a = iii.i_ParameterId, b = iii.i_GroupId } into J6_join
                                                                             from iii in J6_join.DefaultIfEmpty()

                                                                             join J1 in dbContext.systemuser on new { i_InsertUserId = ccc.i_InsertUserId.Value }
                                                                                             equals new { i_InsertUserId = J1.i_SystemUserId } into J1_join
                                                                             from J1 in J1_join.DefaultIfEmpty()

                                                                             join J2 in dbContext.systemuser on new { i_UpdateUserId = ccc.i_UpdateUserId.Value }
                                                                                                             equals new { i_UpdateUserId = J2.i_SystemUserId } into J2_join
                                                                             from J2 in J2_join.DefaultIfEmpty()

                                                                             where ccc.v_DiagnosticRepositoryId == pstrDiagnosticRepositoryId &&
                                                                                   ccc.i_IsDeleted == 0
                                                                             select new Sigesoft.Node.WinClient.BE.DiagnosticRepositoryList
                                                                             {
                                                                                 v_DiagnosticRepositoryId = ccc.v_DiagnosticRepositoryId,
                                                                                 v_ServiceId = ccc.v_ServiceId,
                                                                                 v_ComponentId = ccc.v_ComponentId,
                                                                                 v_DiseasesId = ccc.v_DiseasesId,
                                                                                 i_AutoManualId = ccc.i_AutoManualId,
                                                                                 i_PreQualificationId = ccc.i_PreQualificationId,
                                                                                 i_FinalQualificationId = ccc.i_FinalQualificationId,
                                                                                 i_DiagnosticTypeId = ccc.i_DiagnosticTypeId,
                                                                                 i_IsSentToAntecedent = ccc.i_IsSentToAntecedent,
                                                                                 d_ExpirationDateDiagnostic = ccc.d_ExpirationDateDiagnostic,
                                                                                 i_GenerateMedicalBreak = ccc.i_GenerateMedicalBreak,

                                                                                 v_DiseasesName = ddd.v_Name,
                                                                                 v_AutoManualName = eee.v_Value1,

                                                                                 v_PreQualificationName = fff.v_Value1,
                                                                                 v_FinalQualificationName = ggg.v_Value1,
                                                                                 v_DiagnosticTypeName = hhh.v_Value1,
                                                                                 v_IsSentToAntecedentName = iii.v_Value1,
                                                                                 i_RecordStatus = (int)RecordStatus.Grabado,
                                                                                 i_RecordType = (int)RecordType.NoTemporal,

                                                                                 v_CreationUser = J1.v_UserName,
                                                                                 v_UpdateUser = J2.v_UserName,
                                                                                 d_CreationDate = J1.d_InsertDate,
                                                                                 d_UpdateDate = J2.d_UpdateDate,
                                                                                 i_IsDeleted = ccc.i_IsDeleted.Value
                                                                             }).FirstOrDefault();



                // Agregamos Restricciones / Recomendaciones
                OperationResult objOperationResult = new OperationResult();

                query.Restrictions = GetServiceRestrictionsByDiagnosticRepositoryId(ref objOperationResult, query.v_DiagnosticRepositoryId);
                query.Recomendations = GetServiceRecommendationByDiagnosticRepositoryId(ref objOperationResult, query.v_DiagnosticRepositoryId);

                pobjOperationResult.Success = 1;
                return query;
            }
            catch (Exception ex)
            {
                pobjOperationResult.Success = 0;
                pobjOperationResult.ExceptionMessage = Common.Utils.ExceptionFormatter(ex);
                return null;
            }
        }

        public diseasesDto GetIsValidateDiseases(ref OperationResult pobjOperationResult, string pstrDiseasesName)
        {
            //mon.IsActive = true;

            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
                diseasesDto objDtoEntity = null;

                var objEntity = (from a in dbContext.diseases
                                 where a.v_Name == pstrDiseasesName && a.i_IsDeleted == 0
                                 select a).FirstOrDefault();

                if (objEntity != null)
                    objDtoEntity = diseasesAssembler.ToDTO(objEntity);

                pobjOperationResult.Success = 1;
                return objDtoEntity;
            }
            catch (Exception ex)
            {
                pobjOperationResult.Success = 0;
                pobjOperationResult.ExceptionMessage = Common.Utils.ExceptionFormatter(ex);
                return null;
            }
        }
       
        public diseasesDto GetDiseases(ref OperationResult pobjOperationResult, string pstrDiseasesId)
        {
            //mon.IsActive = true;

            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
                diseasesDto objDtoEntity = null;

                var objEntity = (from a in dbContext.diseases
                                 where a.v_DiseasesId == pstrDiseasesId && a.i_IsDeleted == 0
                                 select a).FirstOrDefault();

                if (objEntity != null)
                    objDtoEntity = diseasesAssembler.ToDTO(objEntity);

                pobjOperationResult.Success = 1;
                return objDtoEntity;
            }
            catch (Exception ex)
            {
                pobjOperationResult.Success = 0;
                pobjOperationResult.ExceptionMessage = Common.Utils.ExceptionFormatter(ex);
                return null;
            }
        }
       
        public void ActualizarRestricctionServicio(ref OperationResult pobjOperationResult, string RestricctionId, string MasterRecommendationId, List<string> ClientSession)
        {
            try
            {

                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                // Obtener la entidad fuente
                var objEntitySource = (from a in dbContext.restriction
                                       where a.v_RestrictionId == RestricctionId
                                       select a).FirstOrDefault();


                objEntitySource.v_MasterRestrictionId = MasterRecommendationId;
                objEntitySource.d_UpdateDate = DateTime.Now;
                objEntitySource.i_UpdateUserId = Int32.Parse(ClientSession[2]);
                // Guardar los cambios
                dbContext.SaveChanges();
                pobjOperationResult.Success = 1;
                return;

            }
            catch (Exception ex)
            {

                pobjOperationResult.Success = 0;
                pobjOperationResult.ExceptionMessage = ex.Message;
                return;
            }
        }
       
        public List<Sigesoft.Node.WinClient.BE.DiseasesList> GetDiseasesPagedAndFiltered(ref OperationResult pobjOperationResult, int? pintPageIndex, int? pintResultsPerPage, string pstrSortExpression, string pstrFilterExpression)
        {
            //mon.IsActive = true;

            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
                var query = (from B in dbContext.diseases
                             //join B in dbContext.diseases on new { v_CIE10Id = A.v_CIE10Id }
                             //                               equals new { v_CIE10Id = B.v_CIE10Id } into B_join
                             //from B in B_join.DefaultIfEmpty()

                             join J1 in dbContext.systemuser on new { i_InsertUserId = B.i_InsertUserId.Value }
                                                             equals new { i_InsertUserId = J1.i_SystemUserId } into J1_join
                             from J1 in J1_join.DefaultIfEmpty()

                             join J2 in dbContext.systemuser on new { i_UpdateUserId = B.i_UpdateUserId.Value }
                                                             equals new { i_UpdateUserId = J2.i_SystemUserId } into J2_join
                             from J2 in J2_join.DefaultIfEmpty()
                             select new Sigesoft.Node.WinClient.BE.DiseasesList
                             {
                                 v_DiseasesId = B.v_DiseasesId,
                                 v_CIE10Idv_Name = B.v_CIE10Id + " " + B.v_Name,
                                 v_CIE10Id = B.v_CIE10Id,
                                 v_Name = B.v_Name,
                                 v_CIE10Description1 = null,
                                 v_CreationUser = J1.v_UserName,
                                 v_UpdateUser = J2.v_UserName,
                                 d_CreationDate = B.d_InsertDate,
                                 d_UpdateDate = B.d_UpdateDate
                             }).Union(from A in dbContext.cie10
                                      join J1 in dbContext.systemuser on new { i_InsertUserId = A.i_InsertUserId.Value }
                                                                      equals new { i_InsertUserId = J1.i_SystemUserId } into J1_join
                                      from J1 in J1_join.DefaultIfEmpty()

                                      join J2 in dbContext.systemuser on new { i_UpdateUserId = A.i_UpdateUserId.Value }
                                                                      equals new { i_UpdateUserId = J2.i_SystemUserId } into J2_join
                                      from J2 in J2_join.DefaultIfEmpty()
                                      select new Sigesoft.Node.WinClient.BE.DiseasesList
                                      {
                                          v_DiseasesId = null,
                                          v_CIE10Idv_Name = A.v_CIE10Id,
                                          v_CIE10Id = A.v_CIE10Id,
                                          v_Name = A.v_CIE10Description1,
                                          v_CIE10Description1 = A.v_CIE10Description1,
                                          v_CreationUser = J1.v_UserName,
                                          v_UpdateUser = J2.v_UserName,
                                          d_CreationDate = A.d_InsertDate,
                                          d_UpdateDate = A.d_UpdateDate
                                      });


                if (!string.IsNullOrEmpty(pstrFilterExpression))
                {
                    query = query.Where(pstrFilterExpression);
                }
                if (!string.IsNullOrEmpty(pstrSortExpression))
                {
                    query = query.OrderBy(pstrSortExpression);
                }
                if (pintPageIndex.HasValue && pintResultsPerPage.HasValue)
                {
                    int intStartRowIndex = pintPageIndex.Value * pintResultsPerPage.Value;
                    query = query.Skip(intStartRowIndex);
                }
                if (pintResultsPerPage.HasValue)
                {
                    query = query.Take(pintResultsPerPage.Value);
                }

                List<Sigesoft.Node.WinClient.BE.DiseasesList> objData = query.ToList();
                pobjOperationResult.Success = 1;
                return objData;

            }
            catch (Exception ex)
            {
                pobjOperationResult.Success = 0;
                pobjOperationResult.ExceptionMessage = Common.Utils.ExceptionFormatter(ex);
                return null;
            }
        }

        private string ConcatenateTypeOfeep(string pstrHistoryId)
        {
            SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

            var qry = (from A in dbContext.typeofeep
                       join B in dbContext.systemparameter on new { a = A.i_TypeofEEPId.Value, b = 146 }  // TIPO DE EPP USADO
                                                            equals new { a = B.i_ParameterId, b = B.i_GroupId }
                       where A.v_HistoryId == pstrHistoryId &&
                       A.i_IsDeleted == 0
                       select new
                       {
                           v_DiseasesName = B.v_Value1
                       }).ToList();

            return qry.Count == 0 ? "No usa EPP" : string.Join(", ", qry.Select(p => p.v_DiseasesName));
        }

        private string ConcatenateWorkStationDangers(string pstrHistoryId)
        {
            SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

            var qry = (from A in dbContext.workstationdangers
                       join B in dbContext.systemparameter on new { a = A.i_DangerId.Value, b = 145 } // PELIGROS EN EL PUESTO
                                                              equals new { a = B.i_ParameterId, b = B.i_GroupId }
                       where A.v_HistoryId == pstrHistoryId &&
                       A.i_IsDeleted == 0
                       select new
                       {
                           v_DiseasesName = B.v_Value1
                       }).ToList();

            return qry.Count == 0 ? "No refiere peligros en el puesto" : string.Join(", ", qry.Select(p => p.v_DiseasesName));
        }

        private string ConcatenateFamilyMedicalAntecedents(string pstrPersonId, int pintTypeFamilyId)
        {
            SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

            var qry = (from A in dbContext.familymedicalantecedents
                       join B in dbContext.systemparameter on new { a = A.i_TypeFamilyId.Value, b = 149 }  // ANTECEDENTES FAMILIARES MÉDICOS
                                                    equals new { a = B.i_ParameterId, b = B.i_GroupId } into B_join
                       from B in B_join.DefaultIfEmpty()
                       join C in dbContext.systemparameter on new { a = B.i_ParentParameterId.Value, b = 149 }  // [PADRE,MADRE,HERMANOS]
                                                      equals new { a = C.i_ParameterId, b = C.i_GroupId } into C_join
                       from C in C_join.DefaultIfEmpty()
                       join D in dbContext.diseases on new { a = A.v_DiseasesId }
                                                               equals new { a = D.v_DiseasesId } into D_join
                       from D in D_join.DefaultIfEmpty()
                       where A.v_PersonId == pstrPersonId &&
                       A.i_IsDeleted == 0 && C.i_ParameterId == pintTypeFamilyId
                       select new
                       {
                           v_DiseasesName = D.v_Name
                       }).ToList();

            return string.Join(", ", qry.Select(p => p.v_DiseasesName));
        }

        public string AddDiseases(ref OperationResult pobjOperationResult, diseasesDto pobjDtoEntity, List<string> ClientSession)
        {
            //mon.IsActive = true;
            string NewId = "(No generado)";
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
                diseases objEntity = diseasesAssembler.ToEntity(pobjDtoEntity);

                objEntity.d_InsertDate = DateTime.Now;
                objEntity.i_InsertUserId = Int32.Parse(ClientSession[2]);
                objEntity.i_IsDeleted = 0;
                // Autogeneramos el Pk de la tabla
                int intNodeId = int.Parse(ClientSession[0]);
                NewId = Common.Utils.GetNewId(intNodeId, Utils.GetNextSecuentialId(intNodeId, 27), "DD");
                objEntity.v_DiseasesId = NewId;


                dbContext.AddTodiseases(objEntity);
                dbContext.SaveChanges();

                pobjOperationResult.Success = 1;
                // Llenar entidad Log
                LogBL.SaveLog(ClientSession[0], ClientSession[1], ClientSession[2], LogEventType.CREACION, "ENFERMEDAD", "v_Diseases=" + NewId.ToString(), Success.Ok, null);
                return NewId;
            }
            catch (Exception ex)
            {
                pobjOperationResult.Success = 0;
                pobjOperationResult.ExceptionMessage = Common.Utils.ExceptionFormatter(ex);
                // Llenar entidad Log
                LogBL.SaveLog(ClientSession[0], ClientSession[1], ClientSession[2], LogEventType.CREACION, "ENFERMEDAD", "v_Diseases=" + NewId.ToString(), Success.Failed, pobjOperationResult.ExceptionMessage);
                return null;
            }
        }

        public void AddDiagnosticRepository(ref OperationResult pobjOperationResult, List<Sigesoft.Node.WinClient.BE.DiagnosticRepositoryList> pobjDiagnosticRepository, servicecomponentDto pobjServiceComponent, List<string> ClientSession, bool? enabledchkApproved)
        {
            //mon.IsActive = true;
            string NewId0 = "(No generado)";
            int intNodeId = int.Parse(ClientSession[0]);
            string componentId = null;

            //try
            //{
            SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

            if (pobjDiagnosticRepository != null)
            {
                foreach (var dr in pobjDiagnosticRepository)
                {

                    #region DiagnosticRepository -> ADD / UPDATE / DELETE

                    // ADD
                    if (dr.i_RecordType == (int)RecordType.Temporal && (dr.i_RecordStatus == (int)RecordStatus.Agregado || dr.i_RecordStatus == (int)RecordStatus.Modificado))
                    {
                        diagnosticrepository objEntity = new diagnosticrepository();

                        // En caso de haber mas de un ComponentID quiere decir que lo datos provienen de un examen agrupador con una categoria (LAB,PSICOLOGIA) 
                        // entonces cojo el ID del hijo mayor (osea el primer ID)[0]
                        // Buscar un palote
                        if (dr.v_ComponentId != null)
                        {
                            if (dr.v_ComponentId.Contains('|'))
                                componentId = (dr.v_ComponentId.Split('|'))[0];
                            else
                                componentId = dr.v_ComponentId;
                        }

                        objEntity.v_DiagnosticRepositoryId = dr.v_DiagnosticRepositoryId;
                        objEntity.v_ServiceId = dr.v_ServiceId;
                        objEntity.v_ComponentId = componentId;
                        objEntity.v_DiseasesId = dr.v_DiseasesId;
                        // ID del Control que generó el DX automático [v_ComponentFieldsId]
                        objEntity.v_ComponentFieldId = dr.v_ComponentFieldsId;
                        objEntity.i_AutoManualId = dr.i_AutoManualId;
                        objEntity.i_PreQualificationId = dr.i_PreQualificationId;
                        // Total Diagnósticos
                        objEntity.i_FinalQualificationId = dr.i_FinalQualificationId;
                        objEntity.i_DiagnosticTypeId = dr.i_DiagnosticTypeId;
                        objEntity.i_IsSentToAntecedent = dr.i_IsSentToAntecedent;
                        objEntity.d_ExpirationDateDiagnostic = dr.d_ExpirationDateDiagnostic;

                        objEntity.d_InsertDate = DateTime.Now;
                        objEntity.i_InsertUserId = Int32.Parse(ClientSession[2]);
                        objEntity.i_IsDeleted = 0;

                        // Accidente laboral
                        objEntity.i_DiagnosticSourceId = dr.i_DiagnosticSourceId;
                        objEntity.i_ShapeAccidentId = dr.i_ShapeAccidentId;
                        objEntity.i_BodyPartId = dr.i_BodyPartId;
                        objEntity.i_ClassificationOfWorkAccidentId = dr.i_ClassificationOfWorkAccidentId;

                        // Enfermedad laboral
                        objEntity.i_RiskFactorId = dr.i_RiskFactorId;
                        objEntity.i_ClassificationOfWorkdiseaseId = dr.i_ClassificationOfWorkdiseaseId;

                        // Autogeneramos el Pk de la tabla                      
                        NewId0 = Common.Utils.GetNewId(intNodeId, Utils.GetNextSecuentialId(intNodeId, 29), "DR");
                        objEntity.v_DiagnosticRepositoryId = NewId0;

                        dbContext.AddTodiagnosticrepository(objEntity);

                    }                                                // UPDATE
                    else if (dr.i_RecordType == (int)RecordType.NoTemporal && dr.i_RecordStatus == (int)RecordStatus.Modificado)
                    {
                        // Obtener la entidad fuente
                        var objEntitySource = (from a in dbContext.diagnosticrepository
                                               where a.v_DiagnosticRepositoryId == dr.v_DiagnosticRepositoryId
                                               select a).FirstOrDefault();

                        // Crear la entidad con los datos actualizados   
                        objEntitySource.i_AutoManualId = dr.i_AutoManualId;
                        objEntitySource.i_PreQualificationId = dr.i_PreQualificationId;
                        objEntitySource.v_ComponentId = dr.v_ComponentId.Split('|')[0];
                        // ID del Control que generó el DX automático [v_ComponentFieldsId]
                        //objEntitySource.v_ComponentFieldsId = dr.v_ComponentFieldsId;
                        // Total Diagnósticos
                        if (objEntitySource.i_FinalQualificationId == null)
                            objEntitySource.i_FinalQualificationId = dr.i_FinalQualificationId;

                        objEntitySource.i_DiagnosticTypeId = dr.i_DiagnosticTypeId;
                        objEntitySource.i_IsSentToAntecedent = dr.i_IsSentToAntecedent;
                        objEntitySource.d_ExpirationDateDiagnostic = dr.d_ExpirationDateDiagnostic;

                        // Accidente laboral
                        objEntitySource.i_DiagnosticSourceId = dr.i_DiagnosticSourceId;
                        objEntitySource.i_ShapeAccidentId = dr.i_ShapeAccidentId;
                        objEntitySource.i_BodyPartId = dr.i_BodyPartId;
                        objEntitySource.i_ClassificationOfWorkAccidentId = dr.i_ClassificationOfWorkAccidentId;

                        // Enfermedad laboral
                        objEntitySource.i_RiskFactorId = dr.i_RiskFactorId;
                        objEntitySource.i_ClassificationOfWorkdiseaseId = dr.i_ClassificationOfWorkdiseaseId;

                        objEntitySource.d_UpdateDate = DateTime.Now;
                        objEntitySource.i_UpdateUserId = Int32.Parse(ClientSession[2]);

                    }                                                // DELETE
                    else if (dr.i_RecordType == (int)RecordType.NoTemporal && dr.i_RecordStatus == (int)RecordStatus.EliminadoLogico)
                    {
                        // Obtener la entidad fuente
                        var objEntitySource = (from a in dbContext.diagnosticrepository
                                               where a.v_DiagnosticRepositoryId == dr.v_DiagnosticRepositoryId
                                               select a).FirstOrDefault();

                        // Crear la entidad con los datos actualizados                                                           
                        objEntitySource.d_UpdateDate = DateTime.Now;
                        objEntitySource.i_UpdateUserId = Int32.Parse(ClientSession[2]);
                        objEntitySource.i_IsDeleted = 1;

                    }

                    #endregion

                    #region Restricciones -> ADD / DELETE

                    if (dr.Restrictions != null)
                    {
                        // Operaciones básicas [Add,Update,Delete] restricciones 
                        foreach (var r in dr.Restrictions)
                        {
                            if (r.i_RecordType == (int)RecordType.Temporal && r.i_RecordStatus == (int)RecordStatus.Agregado)
                            {
                                restriction objRestriction = new restriction();

                                var NewId1 = Common.Utils.GetNewId(intNodeId, Utils.GetNextSecuentialId(intNodeId, 30), "RD");
                                objRestriction.v_ServiceId = r.v_ServiceId;
                                objRestriction.v_ComponentId = r.v_ComponentId.Split('|')[0];
                                //objRestriction.v_RestrictionByDiagnosticId = NewId1;
                                objRestriction.v_RestrictionId = NewId1;
                                objRestriction.v_DiagnosticRepositoryId = NewId0 == "(No generado)" ? dr.v_DiagnosticRepositoryId : NewId0;

                                objRestriction.v_MasterRestrictionId = r.v_MasterRestrictionId.Length > 16 ? null : r.v_MasterRestrictionId;
                                objRestriction.d_InsertDate = DateTime.Now;
                                objRestriction.i_InsertUserId = Int32.Parse(ClientSession[2]);
                                objRestriction.i_IsDeleted = 0;

                                dbContext.AddTorestriction(objRestriction);

                            }
                            else if (r.i_RecordType == (int)RecordType.NoTemporal && r.i_RecordStatus == (int)RecordStatus.EliminadoLogico)
                            {
                                // Obtener la entidad fuente v_RestrictionByDiagnosticId
                                var objEntitySource = (from a in dbContext.restriction
                                                       where a.v_RestrictionId == r.v_RestrictionByDiagnosticId
                                                       select a).FirstOrDefault();

                                // Crear la entidad con los datos actualizados                                                           
                                objEntitySource.d_UpdateDate = DateTime.Now;
                                objEntitySource.i_UpdateUserId = Int32.Parse(ClientSession[2]);
                                objEntitySource.i_IsDeleted = 1;

                            }
                        }
                    }

                    #endregion

                    #region Recomendaciones -> ADD / DELETE

                    if (dr.Recomendations != null)
                    {
                        // Grabar recomendaciones 
                        foreach (var r in dr.Recomendations)
                        {
                            if (r.i_RecordType == (int)RecordType.Temporal && r.i_RecordStatus == (int)RecordStatus.Agregado)
                            {
                                recommendation objRecommendation = new recommendation();

                                var NewId1 = Common.Utils.GetNewId(intNodeId, Utils.GetNextSecuentialId(intNodeId, 32), "RR");
                                objRecommendation.v_ServiceId = r.v_ServiceId;
                                objRecommendation.v_ComponentId = r.v_ComponentId.Split('|')[0];
                                objRecommendation.v_RecommendationId = NewId1;
                                objRecommendation.v_DiagnosticRepositoryId = NewId0 == "(No generado)" ? dr.v_DiagnosticRepositoryId : NewId0;

                                //objRecommendation.v_MasterRecommendationId = r.v_RecommendationId.Length > 16 ? null : r.v_MasterRecommendationId;
                                objRecommendation.v_MasterRecommendationId = r.v_MasterRecommendationId;
                                objRecommendation.d_InsertDate = DateTime.Now;
                                objRecommendation.i_InsertUserId = Int32.Parse(ClientSession[2]);
                                objRecommendation.i_IsDeleted = 0;

                                dbContext.AddTorecommendation(objRecommendation);

                            }
                            else if (r.i_RecordType == (int)RecordType.NoTemporal && r.i_RecordStatus == (int)RecordStatus.EliminadoLogico)
                            {
                                // Obtener la entidad fuente
                                var objEntitySource = (from a in dbContext.recommendation
                                                       where a.v_RecommendationId == r.v_RecommendationId
                                                       select a).FirstOrDefault();

                                // Crear la entidad con los datos actualizados                                                           
                                objEntitySource.d_UpdateDate = DateTime.Now;
                                objEntitySource.i_UpdateUserId = Int32.Parse(ClientSession[2]);
                                objEntitySource.i_IsDeleted = 1;

                            }
                        }
                    }

                    #endregion

                }

                // Guardar los cambios
                dbContext.SaveChanges();

                pobjOperationResult.Success = 1;
                // Llenar entidad Log
                LogBL.SaveLog(ClientSession[0], ClientSession[1], ClientSession[2], LogEventType.CREACION, "DIAGNOSTICOS / RESTRICCIONES / RECOMENDACIONES POR EXAMEN COMPONENTE", "v_DiagnosticRepositoryId=" + NewId0.ToString(), Success.Ok, null);

                //}
                //catch (Exception ex)
                //{
                //    pobjOperationResult.Success = 0;
                //    pobjOperationResult.ExceptionMessage = Common.Utils.ExceptionFormatter(ex);
                //    // Llenar entidad Log
                //    LogBL.SaveLog(ClientSession[0], ClientSession[1], ClientSession[2], LogEventType.CREACION, "DIAGNOSTICOS / RESTRICCIONES / RECOMENDACIONES POR EXAMEN COMPONENTE", "v_DiagnosticRepositoryId=" + NewId0.ToString(), Success.Failed, pobjOperationResult.ExceptionMessage);
                //    return;
                //}
            }

            if (pobjServiceComponent != null)
            {
                // Actualizar algunos valores de ServiceComponent
                OperationResult objOperationResult = new OperationResult();
                //UpdateServiceComponentFromEso(ref objOperationResult, pobjServiceComponent, ClientSession, enabledchkApproved);
            }

        }
      
       public void AgregarRecomendacion(ref OperationResult pobjOperationResult, recommendationDto pobjDtoEntity, List<string> ClientSession)
        {
            //mon.IsActive = true;
            string NewId = "(No generado)";
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
                recommendation objEntity = recommendationAssembler.ToEntity(pobjDtoEntity);

                objEntity.d_InsertDate = DateTime.Now;
                objEntity.i_InsertUserId = Int32.Parse(ClientSession[2]);
                objEntity.i_IsDeleted = 0;
                // Autogeneramos el Pk de la tabla                 
                int intNodeId = int.Parse(ClientSession[0]);
                NewId = Common.Utils.GetNewId(intNodeId, Utils.GetNextSecuentialId(intNodeId, 32), "RR"); ;
                objEntity.v_RecommendationId = NewId;

                dbContext.AddTorecommendation(objEntity);
                dbContext.SaveChanges();

                pobjOperationResult.Success = 1;
                // Llenar entidad Log
                return;
            }
            catch (Exception ex)
            {
                pobjOperationResult.Success = 0;
                pobjOperationResult.ExceptionMessage = Common.Utils.ExceptionFormatter(ex);
                // Llenar entidad Log
                return;
            }
        }

        public void ActualizarRecomendacionServicio(ref OperationResult pobjOperationResult, string RecommendationId, string MasterRecommendationId, List<string> ClientSession)
        {
            try
            {

                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                // Obtener la entidad fuente
                var objEntitySource = (from a in dbContext.recommendation
                                       where a.v_RecommendationId == RecommendationId
                                       select a).FirstOrDefault();


                objEntitySource.v_MasterRecommendationId = MasterRecommendationId;
                objEntitySource.d_UpdateDate = DateTime.Now;
                objEntitySource.i_UpdateUserId = Int32.Parse(ClientSession[2]);
                // Guardar los cambios
                dbContext.SaveChanges();
                pobjOperationResult.Success = 1;
                return;

            }
            catch (Exception ex)
            {

                pobjOperationResult.Success = 0;
                pobjOperationResult.ExceptionMessage = ex.Message;
                return;
            }
        }

        public List<Sigesoft.Node.WinClient.BE.PersonMedicalHistoryList> GetAntecedentConsolidateForService(ref OperationResult pobjOperationResult, string pstrPersonId)
        {
            //mon.IsActive = true;

            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                List<Sigesoft.Node.WinClient.BE.PersonMedicalHistoryList> lis = new List<Sigesoft.Node.WinClient.BE.PersonMedicalHistoryList>();

                int isDeleted = (int)SiNo.NO;

                #region querys individuales

                // Obtener todos loa antecedentes de una persona (una o varias empresas)
                var historyId = (from a in dbContext.history
                                 where a.v_PersonId == pstrPersonId && a.i_IsDeleted == isDeleted
                                 select new Sigesoft.Node.WinClient.BE.PersonMedicalHistoryList
                                 {
                                     v_AntecedentTypeName = "Ocupacionales",
                                     v_DiseasesName = null,
                                     v_HistoryId = a.v_HistoryId,
                                     d_StartDate = a.d_StartDate,
                                     d_EndDate = a.d_EndDate,
                                     v_Occupation = a.v_workstation,
                                     v_GroupName = null
                                 }).ToList();

                // personmedicalhistory
                var q1tmp = (from A in dbContext.personmedicalhistory
                             //join F in dbContext.history on A.v_PersonId equals F.v_PersonId
                             //join B in dbContext.systemparameter on new { a = A.v_DiseasesId, b = 147 }      // MINI CIE10
                             //                                  equals new { a = B.v_Value1, b = B.i_GroupId } into B_join
                             //from B in B_join.DefaultIfEmpty()
                             //join C in dbContext.systemparameter on new { a = B.i_ParentParameterId.Value, b = 147 }
                             //                                  equals new { a = C.i_ParameterId, b = C.i_GroupId } into C_join
                             //from C in C_join.DefaultIfEmpty()
                             join D in dbContext.diseases on A.v_DiseasesId equals D.v_DiseasesId
                             //join E in dbContext.systemparameter on new { a = A.i_TypeDiagnosticId.Value, b = 139 }
                             //                                   equals new { a = E.i_ParameterId, b = E.i_GroupId }

                             where A.i_IsDeleted == isDeleted && A.v_PersonId == pstrPersonId

                             select new Sigesoft.Node.WinClient.BE.PersonMedicalHistoryList
                             {
                                 v_AntecedentTypeName = "Medicos-Personales",
                                 //v_PersonMedicalHistoryId = A.v_PersonMedicalHistoryId,
                                 //v_PersonId = A.v_PersonId,
                                 //v_DiseasesId = A.v_DiseasesId,
                                 v_DiseasesName = D.v_Name,
                                 //i_TypeDiagnosticId = A.i_TypeDiagnosticId,
                                 d_StartDate = A.d_StartDate,
                                 v_GroupName = null

                             }).ToList();

                var q1 = (from A in q1tmp
                          select new Sigesoft.Node.WinClient.BE.PersonMedicalHistoryList
                          {
                              v_AntecedentTypeName = "Medicos-Personales",
                              v_DiseasesName = A.v_DiseasesName,
                              v_DateOrGroup = A.d_StartDate.Value.ToShortDateString(),
                              d_StartDate = A.d_StartDate,
                          }).ToList();

                // typeofeep
                var q2 = (from A in historyId
                          select new Sigesoft.Node.WinClient.BE.PersonMedicalHistoryList
                          {
                              v_AntecedentTypeName = "Ocupacionales, " + A.v_Occupation,
                              v_DiseasesName = ConcatenateTypeOfeep(A.v_HistoryId),
                              v_DateOrGroup = A.d_StartDate.Value.ToString("MM/yyyy") + " - " + A.d_EndDate.Value.ToString("MM/yyyy"),
                              d_StartDate = A.d_StartDate,
                          }).ToList();

                // workstationdangers
                var q3 = (from A in historyId
                          select new Sigesoft.Node.WinClient.BE.PersonMedicalHistoryList
                          {
                              v_AntecedentTypeName = "Ocupacionales, " + A.v_Occupation,
                              v_DiseasesName = ConcatenateWorkStationDangers(A.v_HistoryId),
                              v_DateOrGroup = A.d_StartDate.Value.ToString("MM/yyyy") + " - " + A.d_EndDate.Value.ToString("MM/yyyy"),
                              d_StartDate = A.d_StartDate,
                          }).ToList();

                // noxioushabits
                var q4 = (from A in dbContext.noxioushabits
                          join B in dbContext.systemparameter on new { a = A.i_TypeHabitsId.Value, b = 148 }  // HÁBITOS NOCIVOS
                                                         equals new { a = B.i_ParameterId, b = B.i_GroupId } into B_join
                          from B in B_join.DefaultIfEmpty()
                          where A.i_IsDeleted == 0 && A.v_PersonId == pstrPersonId
                          select new Sigesoft.Node.WinClient.BE.PersonMedicalHistoryList
                          {
                              v_AntecedentTypeName = "Hábitos Nocivos",
                              v_DiseasesName = B.v_Value1 + ", " + A.v_Frequency,
                          }).ToList();

                // familymedicalantecedents

                var q5tmp = (from A in dbContext.familymedicalantecedents
                             join B in dbContext.systemparameter on new { a = A.i_TypeFamilyId.Value, b = 149 }  // ANTECEDENTES FAMILIARES MÉDICOS
                                                            equals new { a = B.i_ParameterId, b = B.i_GroupId } into B_join
                             from B in B_join.DefaultIfEmpty()
                             join C in dbContext.systemparameter on new { a = B.i_ParentParameterId.Value, b = 149 }
                                                          equals new { a = C.i_ParameterId, b = C.i_GroupId } into C_join
                             from C in C_join.DefaultIfEmpty()
                             where A.i_IsDeleted == 0 && A.v_PersonId == pstrPersonId
                             group C by new { C.i_ParameterId, C.v_Value1 } into g
                             select new Sigesoft.Node.WinClient.BE.PersonMedicalHistoryList
                             {
                                 v_AntecedentTypeName = "Familiares",
                                 i_TypeFamilyId = g.Key.i_ParameterId,
                                 v_TypeFamilyName = g.Key.v_Value1
                             }).ToList();

                var q5 = (from A in q5tmp
                          select new Sigesoft.Node.WinClient.BE.PersonMedicalHistoryList
                          {
                              v_AntecedentTypeName = A.v_AntecedentTypeName,
                              v_DiseasesName = ConcatenateFamilyMedicalAntecedents(pstrPersonId, A.i_TypeFamilyId),
                              v_DateOrGroup = A.v_TypeFamilyName
                          }).ToList();

                #endregion

                #region Fusion

                if (q1.Count > 0)
                    lis.AddRange(q1);
                if (q2.Count > 0)
                    lis.AddRange(q2);
                if (q3.Count > 0)
                    lis.AddRange(q3);
                if (q4.Count > 0)
                    lis.AddRange(q4);
                if (q5.Count > 0)
                    lis.AddRange(q5);

                #endregion

                //var ss = lis.OrderByDescending(x => x.v_AntecedentTypeName).ThenByDescending(x => x.d_StartDate).ToList();

                pobjOperationResult.Success = 1;
                //return lis.OrderByDescending(x => x.v_AntecedentTypeName).ThenByDescending(x => x.d_StartDate).ToList();
                return lis.OrderBy(x => x.v_AntecedentTypeName).ToList();


            }
            catch (Exception ex)
            {
                pobjOperationResult.Success = 0;
                pobjOperationResult.ExceptionMessage = Common.Utils.ExceptionFormatter(ex);
                return null;
            }
        }

        public List<rolenodecomponentprofileDto> GetRoleNodeComponentProfileByRoleNodeId(int pintNodeId, int pintRoleId)
        {
            //mon.IsActive = true;

            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                var query = (from a in dbContext.rolenodecomponentprofile
                             where (a.i_NodeId == pintNodeId) &&
                                   (a.i_RoleId == pintRoleId) &&
                                   (a.i_IsDeleted == (int)SiNo.NO)
                             select new rolenodecomponentprofileDto
                             {
                                 v_ComponentId = a.v_ComponentId,
                                 v_RoleNodeComponentId = a.v_RoleNodeComponentId,
                                 i_Read = a.i_Read.Value
                             }).ToList();

                return query;
            }
            catch (Exception)
            {
                throw;

            }

        }
       
        public bool AddServiceComponentValues(ref OperationResult pobjOperationResult, List<Sigesoft.Node.WinClient.BE.ServiceComponentFieldsList> pobjServicecomponentfields, List<string> ClientSession, string pstrPersonId, string pServiceComponentId)
        {

            bool result = false;

            string NewId = "(No generado)";
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                int intNodeId = int.Parse(ClientSession[0]);
                List<string> grupoFactorSanguineo = new List<string>();

                var serviceComponentfields = (from a in dbContext.servicecomponentfields
                                              where a.v_ServiceComponentId == pServiceComponentId
                                              select a).ToList();

                //var ENT = serviceComponentfields.SelectMany(P => P.servicecomponentfieldvalues.Where(p => p.v_ServiceComponentFieldsId == "N009-CF000517532")).ToList();

                serviceComponentfields.Sort((x, y) => x.v_ComponentFieldId.CompareTo(y.v_ComponentFieldId));

                foreach (var cf in pobjServicecomponentfields)
                {
                    var q = serviceComponentfields.Find(p => p.v_ComponentFieldId == cf.v_ComponentFieldsId);

                    if (q == null)   // ADD
                    {
                        #region GRABAR CAMPOS DE UN SERVICE COMPONENT

                        servicecomponentfields objEntity = new servicecomponentfields();

                        objEntity.v_ComponentFieldId = cf.v_ComponentFieldsId;
                        objEntity.v_ServiceComponentId = cf.v_ServiceComponentId;
                        objEntity.d_InsertDate = DateTime.Now;
                        objEntity.i_InsertUserId = Int32.Parse(ClientSession[2]);
                        objEntity.i_IsDeleted = 0;

                        // Autogeneramos el Pk de la tabla               
                        NewId = Common.Utils.GetNewId(intNodeId, Utils.GetNextSecuentialId(intNodeId, 35), "CF");
                        objEntity.v_ServiceComponentFieldsId = NewId;

                        dbContext.AddToservicecomponentfields(objEntity);
                        //dbContext.SaveChanges();

                        #endregion

                        foreach (var fv in cf.ServiceComponentFieldValues)
                        {
                            #region Buscar y almacenar GRUPO y FACTOR SANGUINEO en una lista temp

                            if (cf.v_ComponentFieldsId == Constants.GRUPO_SANGUINEO_ID)
                            {
                                grupoFactorSanguineo.Add(fv.v_Value1);
                            }

                            if (cf.v_ComponentFieldsId == Constants.FACTOR_SANGUINEO_ID)
                            {
                                grupoFactorSanguineo.Add(fv.v_Value1);
                            }

                            #endregion

                            #region GRABAR VALORES DE UN SERVICE COMPONENT

                            servicecomponentfieldvalues objEntity1 = new servicecomponentfieldvalues();

                            objEntity1.v_ComponentFieldValuesId = fv.v_ComponentFieldValuesId;
                            objEntity1.v_Value1 = fv.v_Value1;
                            objEntity1.d_InsertDate = DateTime.Now;
                            objEntity1.i_InsertUserId = Int32.Parse(ClientSession[2]);
                            objEntity1.i_IsDeleted = 0;

                            // Autogeneramos el Pk de la tabla               
                            var NewId1 = Common.Utils.GetNewId(intNodeId, Utils.GetNextSecuentialId(intNodeId, 36), "CV");
                            objEntity1.v_ServiceComponentFieldValuesId = NewId1;
                            objEntity1.v_ServiceComponentFieldsId = NewId;

                            dbContext.AddToservicecomponentfieldvalues(objEntity1);


                            #endregion
                        }
                    }
                    else         // UPDATE
                    {
                        #region ACTUALIZAR CAMPOS DE UN SERVICE COMPONENT

                        //q.v_ComponentFieldId = cf.v_ComponentFieldsId;
                        //q.v_ServiceComponentId = cf.v_ServiceComponentId;
                        q.d_UpdateDate = DateTime.Now;
                        q.i_UpdateUserId = Int32.Parse(ClientSession[2]);

                        // Guardar los cambios
                        //dbContext.SaveChanges();

                        #endregion

                        foreach (var fv in cf.ServiceComponentFieldValues)
                        {
                            #region Buscar y almacenar GRUPO y FACTOR SANGUINEO en una lista temp

                            if (cf.v_ComponentFieldsId == Constants.GRUPO_SANGUINEO_ID)
                            {
                                grupoFactorSanguineo.Add(fv.v_Value1);
                            }

                            if (cf.v_ComponentFieldsId == Constants.FACTOR_SANGUINEO_ID)
                            {
                                grupoFactorSanguineo.Add(fv.v_Value1);
                            }

                            #endregion

                            #region ACTUALIZAR VALORES DE UN SERVICE COMPONENT

                            var q1 = (from a in dbContext.servicecomponentfieldvalues
                                      where a.v_ServiceComponentFieldsId == q.v_ServiceComponentFieldsId
                                      select a).FirstOrDefault();

                            // problema k pasaba con examen fisico se grababa el campo pero no el valor si el valor no esta grabado
                            // se graba a la prepo
                            if (q1 == null)
                            {
                                servicecomponentfieldvalues objEntity1 = new servicecomponentfieldvalues();

                                objEntity1.v_ComponentFieldValuesId = fv.v_ComponentFieldValuesId;
                                objEntity1.v_Value1 = fv.v_Value1;
                                objEntity1.d_InsertDate = DateTime.Now;
                                objEntity1.i_InsertUserId = Int32.Parse(ClientSession[2]);
                                objEntity1.i_IsDeleted = 0;

                                // Autogeneramos el Pk de la tabla               
                                var NewId1 = Common.Utils.GetNewId(intNodeId, Utils.GetNextSecuentialId(intNodeId, 36), "CV");
                                objEntity1.v_ServiceComponentFieldValuesId = NewId1;
                                objEntity1.v_ServiceComponentFieldsId = q.v_ServiceComponentFieldsId;

                                dbContext.AddToservicecomponentfieldvalues(objEntity1);
                            }
                            else
                            {
                                //if (q.v_ServiceComponentFieldsId == "N009-CF000517532")
                                //{
                                //    var ff = cf.v_Value1;
                                //}

                                //q1.v_ComponentFieldValuesId = fv.v_ComponentFieldValuesId;
                                q1.v_Value1 = fv.v_Value1;
                                q1.d_UpdateDate = DateTime.Now;
                                q1.i_UpdateUserId = Int32.Parse(ClientSession[2]);
                            }

                            #endregion
                        }
                    }
                }

                if (grupoFactorSanguineo.Count != 0)
                {
                    #region GRABAR GRUPO y FACTOR SANGUINEO

                    var person = (from a in dbContext.person
                                  where a.v_PersonId == pstrPersonId
                                  select a).FirstOrDefault();

                    person.i_BloodGroupId = int.Parse(grupoFactorSanguineo[0]);
                    person.i_BloodFactorId = int.Parse(grupoFactorSanguineo[1]);
                    person.d_UpdateDate = DateTime.Now;
                    person.i_UpdateUserId = Int32.Parse(ClientSession[2]);
                    //dbContext.SaveChanges();

                    #endregion
                }

              

                result = (dbContext.SaveChanges() > 0);

                pobjOperationResult.Success = 1;
                // Llenar entidad Log
                LogBL.SaveLog(ClientSession[0], ClientSession[1], ClientSession[2], LogEventType.CREACION, "CAMPOS DE UN COMPONENTE DE SERVICIO", "v_ServiceComponentId=" + NewId.ToString(), Success.Ok, null);

            }
            catch (Exception ex)
            {
                pobjOperationResult.Success = 0;
                pobjOperationResult.ExceptionMessage = Common.Utils.ExceptionFormatter(ex);
                // Llenar entidad Log
                LogBL.SaveLog(ClientSession[0], ClientSession[1], ClientSession[2], LogEventType.CREACION, "CAMPOS DE UN COMPONENTE DE SERVICIO", "v_ServiceComponentId=" + NewId.ToString(), Success.Failed, pobjOperationResult.ExceptionMessage);

            }

            return result;
        }

        public bool AddServiceComponentValues_(ref OperationResult pobjOperationResult, List<Sigesoft.Node.WinClient.BE.ServiceComponentFieldsList> pobjServicecomponentfields, List<string> ClientSession, string pstrPersonId, string pServiceComponentId)
        {

            bool result = false;

            string NewId = "(No generado)";
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                int intNodeId = int.Parse(ClientSession[0]);
                List<string> grupoFactorSanguineo = new List<string>();

                var serviceComponentfields = (from a in dbContext.servicecomponentfields
                                              where a.v_ServiceComponentId == pServiceComponentId
                                              select a).ToList();

                List<string> Ids = new List<string>();
                foreach (var item in serviceComponentfields)
                {
                    Ids.Add(item.v_ServiceComponentFieldsId);
                }
                //&& ListaServicioIds.Contains(A.v_ServiceId)
                var ListaServiceComponentfieldsValues = (from a in dbContext.servicecomponentfieldvalues
                                                         where Ids.Contains(a.v_ServiceComponentFieldsId)
                                                         select a).ToList();

                //var ENT = serviceComponentfields.SelectMany(P => P.servicecomponentfieldvalues.Where(p => p.v_ServiceComponentFieldsId == "N009-CF000517532")).ToList();

                serviceComponentfields.Sort((x, y) => x.v_ComponentFieldId.CompareTo(y.v_ComponentFieldId));

                //Obtener Los ids de las tablas serviceComponentfields y servicecomponentfieldvalues***************************************************************************************
                int Ini_Secuential_serviceComponentfields = 0;
                int Ini_Secuential_servicecomponentfieldvalues = 0;
                if (pobjServicecomponentfields.Count != 0 && serviceComponentfields.Count == 0)
                {
                    Ini_Secuential_serviceComponentfields = Utils.GetNextSecuentialIdMejorado(intNodeId, 35, pobjServicecomponentfields.Count);
                    Ini_Secuential_servicecomponentfieldvalues = Utils.GetNextSecuentialIdMejorado(intNodeId, 36, pobjServicecomponentfields.Count);
                }
                //**************************************************************************************************************************************************************************


                foreach (var cf in pobjServicecomponentfields)
                {
                    var q = serviceComponentfields.Find(p => p.v_ComponentFieldId == cf.v_ComponentFieldsId);

                    if (q == null)   // ADD
                    {
                        #region GRABAR CAMPOS DE UN SERVICE COMPONENT

                        servicecomponentfields objEntity = new servicecomponentfields();

                        objEntity.v_ComponentFieldId = cf.v_ComponentFieldsId;
                        objEntity.v_ServiceComponentId = cf.v_ServiceComponentId;
                        objEntity.d_InsertDate = DateTime.Now;
                        objEntity.i_InsertUserId = Int32.Parse(ClientSession[2]);
                        objEntity.i_IsDeleted = 0;

                        // Autogeneramos el Pk de la tabla               
                        NewId = Common.Utils.GetNewId(intNodeId, Ini_Secuential_serviceComponentfields++, "CF");
                        objEntity.v_ServiceComponentFieldsId = NewId;

                        dbContext.AddToservicecomponentfields(objEntity);
                        //dbContext.SaveChanges();

                        #endregion

                        foreach (var fv in cf.ServiceComponentFieldValues)
                        {
                            #region Buscar y almacenar GRUPO y FACTOR SANGUINEO en una lista temp

                            if (cf.v_ComponentFieldsId == Constants.GRUPO_SANGUINEO_ID)
                            {
                                grupoFactorSanguineo.Add(fv.v_Value1);
                            }

                            if (cf.v_ComponentFieldsId == Constants.FACTOR_SANGUINEO_ID)
                            {
                                grupoFactorSanguineo.Add(fv.v_Value1);
                            }

                            #endregion

                            #region GRABAR VALORES DE UN SERVICE COMPONENT

                            servicecomponentfieldvalues objEntity1 = new servicecomponentfieldvalues();

                            objEntity1.v_ComponentFieldValuesId = fv.v_ComponentFieldValuesId;
                            objEntity1.v_Value1 = fv.v_Value1;
                            objEntity1.d_InsertDate = DateTime.Now;
                            objEntity1.i_InsertUserId = Int32.Parse(ClientSession[2]);
                            objEntity1.i_IsDeleted = 0;

                            // Autogeneramos el Pk de la tabla               
                            var NewId1 = Common.Utils.GetNewId(intNodeId, Ini_Secuential_servicecomponentfieldvalues++, "CV");
                            objEntity1.v_ServiceComponentFieldValuesId = NewId1;
                            objEntity1.v_ServiceComponentFieldsId = NewId;

                            dbContext.AddToservicecomponentfieldvalues(objEntity1);


                            #endregion
                        }
                    }
                    else         // UPDATE
                    {
                        #region ACTUALIZAR CAMPOS DE UN SERVICE COMPONENT

                        //q.v_ComponentFieldId = cf.v_ComponentFieldsId;
                        //q.v_ServiceComponentId = cf.v_ServiceComponentId;
                        q.d_UpdateDate = DateTime.Now;
                        q.i_UpdateUserId = Int32.Parse(ClientSession[2]);

                        // Guardar los cambios
                        //dbContext.SaveChanges();

                        #endregion

                        foreach (var fv in cf.ServiceComponentFieldValues)
                        {
                            #region Buscar y almacenar GRUPO y FACTOR SANGUINEO en una lista temp

                            if (cf.v_ComponentFieldsId == Constants.GRUPO_SANGUINEO_ID)
                            {
                                grupoFactorSanguineo.Add(fv.v_Value1);
                            }

                            if (cf.v_ComponentFieldsId == Constants.FACTOR_SANGUINEO_ID)
                            {
                                grupoFactorSanguineo.Add(fv.v_Value1);
                            }

                            #endregion

                            #region ACTUALIZAR VALORES DE UN SERVICE COMPONENT

                            //var q1 = (from a in dbContext.servicecomponentfieldvalues
                            //          where a.v_ServiceComponentFieldsId == q.v_ServiceComponentFieldsId
                            //          select a).FirstOrDefault();

                            var q1 = ListaServiceComponentfieldsValues.Find(p => p.v_ServiceComponentFieldsId == q.v_ServiceComponentFieldsId);

                            // problema k pasaba con examen fisico se grababa el campo pero no el valor si el valor no esta grabado
                            // se graba a la prepo
                            if (q1 == null)
                            {
                                servicecomponentfieldvalues objEntity1 = new servicecomponentfieldvalues();

                                objEntity1.v_ComponentFieldValuesId = fv.v_ComponentFieldValuesId;
                                objEntity1.v_Value1 = fv.v_Value1;
                                objEntity1.d_InsertDate = DateTime.Now;
                                objEntity1.i_InsertUserId = Int32.Parse(ClientSession[2]);
                                objEntity1.i_IsDeleted = 0;

                                // Autogeneramos el Pk de la tabla               
                                var NewId1 = Common.Utils.GetNewId(intNodeId, Ini_Secuential_servicecomponentfieldvalues++, "CV");
                                objEntity1.v_ServiceComponentFieldValuesId = NewId1;
                                objEntity1.v_ServiceComponentFieldsId = q.v_ServiceComponentFieldsId;

                                dbContext.AddToservicecomponentfieldvalues(objEntity1);
                            }
                            else
                            {
                                q1.v_Value1 = fv.v_Value1;
                                q1.d_UpdateDate = DateTime.Now;
                                q1.i_UpdateUserId = Int32.Parse(ClientSession[2]);
                            }

                            #endregion
                        }
                    }
                }

                if (grupoFactorSanguineo.Count != 0)
                {
                    #region GRABAR GRUPO y FACTOR SANGUINEO

                    var person = (from a in dbContext.person
                                  where a.v_PersonId == pstrPersonId
                                  select a).FirstOrDefault();

                    person.i_BloodGroupId = int.Parse(grupoFactorSanguineo[0]);
                    person.i_BloodFactorId = int.Parse(grupoFactorSanguineo[1]);
                    person.d_UpdateDate = DateTime.Now;
                    person.i_UpdateUserId = Int32.Parse(ClientSession[2]);
                    //dbContext.SaveChanges();

                    #endregion
                }

                #region MyRegion
                //var aaaa = pobjServicecomponentfields.SelectMany(p => p.ServiceComponentFieldValues).ToList();
                //var search = aaaa.FindAll(p => p.v_Value1 != "1" && p.v_Value1 != "-1");

                //pobjServicecomponentfields.Select(p => p.ServiceComponentFieldValues).ToList() = null;
                //foreach (var item in pobjServicecomponentfields)
                //{
                //    item.ServiceComponentFieldValues = search;
                //}

                //pobjServicecomponentfields.SelectMany(p => p.ServiceComponentFieldValues);

                //foreach (var cf in pobjServicecomponentfields)
                //{
                //    var q = (from a in dbContext.servicecomponentfields
                //             where a.v_ComponentFieldId == cf.v_ComponentFieldsId &&
                //             a.v_ServiceComponentId == cf.v_ServiceComponentId
                //             select a).FirstOrDefault();

                //    if (q == null)   // ADD
                //    {
                //        #region GRABAR CAMPOS DE UN SERVICE COMPONENT

                //        servicecomponentfields objEntity = new servicecomponentfields();

                //        objEntity.v_ComponentFieldId = cf.v_ComponentFieldsId;
                //        objEntity.v_ServiceComponentId = cf.v_ServiceComponentId;
                //        objEntity.d_InsertDate = DateTime.Now;
                //        objEntity.i_InsertUserId = Int32.Parse(ClientSession[2]);
                //        objEntity.i_IsDeleted = 0;

                //        // Autogeneramos el Pk de la tabla               
                //        NewId = Common.Utils.GetNewId(intNodeId, Utils.GetNextSecuentialId(intNodeId, 35), "CF");
                //        objEntity.v_ServiceComponentFieldsId = NewId;

                //        dbContext.AddToservicecomponentfields(objEntity);
                //        //dbContext.SaveChanges();

                //        #endregion

                //        foreach (var fv in cf.ServiceComponentFieldValues)
                //        {
                //            #region Buscar y almacenar GRUPO y FACTOR SANGUINEO en una lista temp

                //            if (cf.v_ComponentFieldsId == Constants.GRUPO_SANGUINEO_ID)
                //            {
                //                grupoFactorSanguineo.Add(fv.v_Value1);
                //            }

                //            if (cf.v_ComponentFieldsId == Constants.FACTOR_SANGUINEO_ID)
                //            {
                //                grupoFactorSanguineo.Add(fv.v_Value1);
                //            }

                //            #endregion

                //            #region GRABAR VALORES DE UN SERVICE COMPONENT

                //            servicecomponentfieldvalues objEntity1 = new servicecomponentfieldvalues();

                //            objEntity1.v_ComponentFieldValuesId = fv.v_ComponentFieldValuesId;
                //            objEntity1.v_Value1 = fv.v_Value1;
                //            objEntity1.d_InsertDate = DateTime.Now;
                //            objEntity1.i_InsertUserId = Int32.Parse(ClientSession[2]);
                //            objEntity1.i_IsDeleted = 0;

                //            // Autogeneramos el Pk de la tabla               
                //            var NewId1 = Common.Utils.GetNewId(intNodeId, Utils.GetNextSecuentialId(intNodeId, 36), "CV");
                //            objEntity1.v_ServiceComponentFieldValuesId = NewId1;
                //            objEntity1.v_ServiceComponentFieldsId = NewId;

                //            dbContext.AddToservicecomponentfieldvalues(objEntity1);
                //            //dbContext.SaveChanges();

                //            #endregion
                //        }
                //    }
                //    else         // UPDATE
                //    {
                //        #region ACTUALIZAR CAMPOS DE UN SERVICE COMPONENT

                //        //q.v_ComponentFieldId = cf.v_ComponentFieldsId;
                //        //q.v_ServiceComponentId = cf.v_ServiceComponentId;
                //        q.d_UpdateDate = DateTime.Now;
                //        q.i_UpdateUserId = Int32.Parse(ClientSession[2]);

                //        // Guardar los cambios
                //        //dbContext.SaveChanges();

                //        #endregion

                //        foreach (var fv in cf.ServiceComponentFieldValues)
                //        {
                //            #region Buscar y almacenar GRUPO y FACTOR SANGUINEO en una lista temp

                //            if (cf.v_ComponentFieldsId == Constants.GRUPO_SANGUINEO_ID)
                //            {
                //                grupoFactorSanguineo.Add(fv.v_Value1);
                //            }

                //            if (cf.v_ComponentFieldsId == Constants.FACTOR_SANGUINEO_ID)
                //            {
                //                grupoFactorSanguineo.Add(fv.v_Value1);
                //            }

                //            #endregion

                //            #region ACTUALIZAR VALORES DE UN SERVICE COMPONENT FIELD VALUES

                //            var q1 = (from a in dbContext.servicecomponentfieldvalues
                //                      where a.v_ServiceComponentFieldsId == q.v_ServiceComponentFieldsId
                //                      select a).FirstOrDefault();

                //            //q1.v_ComponentFieldValuesId = fv.v_ComponentFieldValuesId;
                //            q1.v_Value1 = fv.v_Value1;
                //            q1.d_UpdateDate = DateTime.Now;
                //            q1.i_UpdateUserId = Int32.Parse(ClientSession[2]);

                //            dbContext.SaveChanges();

                //            #endregion
                //        }
                //    }
                //}

                //if (grupoFactorSanguineo.Count != 0)
                //{
                //    #region GRABAR GRUPO y FACTOR SANGUINEO

                //    var person = (from a in dbContext.person
                //                  where a.v_PersonId == pstrPersonId
                //                  select a).FirstOrDefault();

                //    person.i_BloodGroupId = int.Parse(grupoFactorSanguineo[0]);
                //    person.i_BloodFactorId = int.Parse(grupoFactorSanguineo[1]);
                //    person.d_UpdateDate = DateTime.Now;
                //    person.i_UpdateUserId = Int32.Parse(ClientSession[2]);
                //    //dbContext.SaveChanges();

                //    #endregion
                //}
                #endregion


                result = (dbContext.SaveChanges() > 0);

                pobjOperationResult.Success = 1;
                // Llenar entidad Log
                LogBL.SaveLog(ClientSession[0], ClientSession[1], ClientSession[2], LogEventType.CREACION, "CAMPOS DE UN COMPONENTE DE SERVICIO", "v_ServiceComponentId=" + NewId.ToString(), Success.Ok, null);

            }
            catch (Exception ex)
            {
                pobjOperationResult.Success = 0;
                pobjOperationResult.ExceptionMessage = Common.Utils.ExceptionFormatter(ex);
                // Llenar entidad Log
                LogBL.SaveLog(ClientSession[0], ClientSession[1], ClientSession[2], LogEventType.CREACION, "CAMPOS DE UN COMPONENTE DE SERVICIO", "v_ServiceComponentId=" + NewId.ToString(), Success.Failed, pobjOperationResult.ExceptionMessage);

            }

            return result;
        }

        public string[] AddMultimediaFileComponent(ref OperationResult pobjOperationResult, FileInfoDto pobjDtoEntity, List<string> ClientSession)
        {
            //mon.IsActive = true;
            string[] IDs = new string[2];
            var multimediaFileId = string.Empty;
            var serviceComponentMultimediaId = string.Empty;

            try
            {
                multimediafileDto multimediaFile = new multimediafileDto();
                multimediaFile.v_PersonId = pobjDtoEntity.PersonId;
                multimediaFile.v_FileName = pobjDtoEntity.FileName;
                multimediaFile.b_File = pobjDtoEntity.ByteArrayFile;
                multimediaFile.b_ThumbnailFile = pobjDtoEntity.ThumbnailFile;

                // Grabar MultimediaFile
                multimediaFileId = AddMultimediaFile(ref pobjOperationResult, multimediaFile, ClientSession, SiNo.NO);
                if (pobjOperationResult.Success == 0) return null;

                servicecomponentmultimediaDto serviceComponentMultimedia = new servicecomponentmultimediaDto();
                serviceComponentMultimedia.v_ServiceComponentId = pobjDtoEntity.ServiceComponentId;
                serviceComponentMultimedia.v_MultimediaFileId = multimediaFileId;
                serviceComponentMultimedia.v_Comment = pobjDtoEntity.Description;

                // Grabar MultimediaFileComponent
                serviceComponentMultimediaId = AddServiceComponentMultimedia(ref pobjOperationResult, serviceComponentMultimedia, ClientSession, SiNo.NO);
                if (pobjOperationResult.Success == 0) return null;

                IDs[0] = multimediaFileId;
                IDs[1] = serviceComponentMultimediaId;

                // Llenar entidad Log
                LogBL.SaveLog(ClientSession[0], ClientSession[1], ClientSession[2], LogEventType.CREACION, "MULTIMEDIA FILE", null, Success.Ok, null);

            }
            catch (Exception ex)
            {
                pobjOperationResult.Success = 0;
                pobjOperationResult.ExceptionMessage = Common.Utils.ExceptionFormatter(ex);
                // Llenar entidad Log
                LogBL.SaveLog(ClientSession[0], ClientSession[1], ClientSession[2], LogEventType.CREACION, "MULTIMEDIA FILE", null, Success.Failed, pobjOperationResult.ExceptionMessage);
            }

            return IDs;
        }

        private string AddMultimediaFile(ref OperationResult pobjOperationResult, multimediafileDto pobjDtoEntity, List<string> ClientSession, SiNo ExecLog)
        {
            //mon.IsActive = true;
            string NewId = "(No generado)";
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
                multimediafile objEntity = multimediafileAssembler.ToEntity(pobjDtoEntity);

                objEntity.d_InsertDate = DateTime.Now;
                objEntity.i_InsertUserId = Int32.Parse(ClientSession[2]);
                objEntity.i_IsDeleted = 0;

                // Autogeneramos el Pk de la tabla
                int intNodeId = int.Parse(ClientSession[0]);
                NewId = Common.Utils.GetNewId(intNodeId, Utils.GetNextSecuentialId(intNodeId, 45), "FU");
                objEntity.v_MultimediaFileId = NewId;

                dbContext.AddTomultimediafile(objEntity);
                dbContext.SaveChanges();

                pobjOperationResult.Success = 1;

                if (ExecLog == SiNo.SI)
                {
                    // Llenar entidad Log
                    LogBL.SaveLog(ClientSession[0], ClientSession[1], ClientSession[2], LogEventType.CREACION, "MULTIMEDIA FILE", "v_MultimediaFileId=" + NewId, Success.Ok, null);
                }

            }
            catch (Exception ex)
            {
                pobjOperationResult.Success = 0;
                pobjOperationResult.ExceptionMessage = Common.Utils.ExceptionFormatter(ex);

                // Llenar entidad Log
                LogBL.SaveLog(ClientSession[0], ClientSession[1], ClientSession[2], LogEventType.CREACION, "MULTIMEDIA FILE", "v_MultimediaFileId=" + NewId, Success.Failed, pobjOperationResult.ExceptionMessage);

            }

            return NewId;

        }

        private string AddServiceComponentMultimedia(ref OperationResult pobjOperationResult, servicecomponentmultimediaDto pobjDtoEntity, List<string> ClientSession, SiNo ExecLog)
        {
            //mon.IsActive = true;
            string NewId = "(No generado)";

            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
                servicecomponentmultimedia objEntity = servicecomponentmultimediaAssembler.ToEntity(pobjDtoEntity);

                objEntity.d_InsertDate = DateTime.Now;
                objEntity.i_InsertUserId = Int32.Parse(ClientSession[2]);
                objEntity.i_IsDeleted = 0;

                // Autogeneramos el Pk de la tabla
                int intNodeId = int.Parse(ClientSession[0]);
                NewId = Common.Utils.GetNewId(intNodeId, Utils.GetNextSecuentialId(intNodeId, 46), "FC");
                objEntity.v_ServiceComponentMultimediaId = NewId;

                dbContext.AddToservicecomponentmultimedia(objEntity);
                dbContext.SaveChanges();

                pobjOperationResult.Success = 1;

                if (ExecLog == SiNo.SI)
                {
                    // Llenar entidad Log
                    LogBL.SaveLog(ClientSession[0], ClientSession[1], ClientSession[2], LogEventType.CREACION, "SERVICE COMPONENT MULTIMEDIA", "v_ServiceComponentMultimediaId=" + NewId.ToString(), Success.Ok, null);
                }

            }
            catch (Exception ex)
            {
                pobjOperationResult.Success = 0;
                pobjOperationResult.ExceptionMessage = Common.Utils.ExceptionFormatter(ex);

                // Llenar entidad Log
                LogBL.SaveLog(ClientSession[0], ClientSession[1], ClientSession[2], LogEventType.CREACION, "SERVICE COMPONENT MULTIMEDIA", "v_ServiceComponentMultimediaId=" + NewId.ToString(), Success.Failed, pobjOperationResult.ExceptionMessage);
            }

            return NewId;
        }

        public servicecomponentDto GetServiceComponentByServiceIdAndComponentId(string pstrServiceId, string pstrComponentId)
        {
            //mon.IsActive = true;

            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
                servicecomponentDto objDtoEntity = null;

                var objEntity = (from a in dbContext.servicecomponent
                                 where a.v_ComponentId == pstrComponentId && a.v_ServiceId == pstrServiceId
                                 select a).FirstOrDefault();

                if (objEntity != null)
                    objDtoEntity = servicecomponentAssembler.ToDTO(objEntity);

                return objDtoEntity;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public string GetDiagnosticByServiceIdAndCategoryId(string pstrServiceId, int pintCategoriaId)
        {
            SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
            var query = (from ccc in dbContext.diagnosticrepository
                         join aaa in dbContext.component on ccc.v_ComponentId equals aaa.v_ComponentId
                         join ddd in dbContext.diseases on ccc.v_DiseasesId equals ddd.v_DiseasesId into ddd_join
                         from ddd in ddd_join.DefaultIfEmpty()

                         join eee in dbContext.recommendation on new { a = pstrServiceId, b = ccc.v_ComponentId }
                                                                    equals new { a = eee.v_ServiceId, b = eee.v_ComponentId } into eee_join
                         from eee in eee_join.DefaultIfEmpty()

                         join fff in dbContext.masterrecommendationrestricction on eee.v_MasterRecommendationId
                                                                equals fff.v_MasterRecommendationRestricctionId into fff_join
                         from fff in fff_join.DefaultIfEmpty()

                         where ccc.v_ServiceId == pstrServiceId && aaa.i_CategoryId == pintCategoriaId &&
                               ccc.i_IsDeleted == 0
                         select new
                         {
                             v_DiseasesName = ddd.v_Name
                         }).Distinct().ToList();


            return string.Join(", ", query.Select(p => p.v_DiseasesName));
        }

        public string ConcatenateRecomendacionesByCategoria(int pintCategoryId, string pstrServiceId)
        {
            SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

            var qry = (from a in dbContext.recommendation  // RESTRICCIONES POR Diagnosticos
                       join c in dbContext.component on a.v_ComponentId equals c.v_ComponentId
                       join eee in dbContext.masterrecommendationrestricction on a.v_MasterRecommendationId equals eee.v_MasterRecommendationRestricctionId
                       where c.i_CategoryId == pintCategoryId && eee.i_IsDeleted == 0 && a.v_ServiceId == pstrServiceId &&
                       a.i_IsDeleted == 0 && eee.i_TypifyingId == (int)Typifying.Recomendaciones
                       select new
                       {
                           v_RestrictionsName = eee.v_Name
                       }).ToList();

            return string.Join(", ", qry.Select(p => p.v_RestrictionsName));
        }

        public List<Sigesoft.Node.WinClient.BE.ServiceComponentList> GetServiceComponentsCulminados(ref OperationResult pobjOperationResult, string pstrServiceId)
        {
            //mon.IsActive = true;
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
                var query = (from A in dbContext.servicecomponent
                             join B in dbContext.systemparameter on new { a = A.i_ServiceComponentStatusId.Value, b = 127 }
                                    equals new { a = B.i_ParameterId, b = B.i_GroupId }
                             join C in dbContext.component on A.v_ComponentId equals C.v_ComponentId
                             where (A.v_ServiceId == pstrServiceId) &&
                                   (A.i_IsDeleted == 0) &&
                                   (A.i_IsRequiredId == (int?)SiNo.SI) &&
                                   (A.i_ServiceComponentStatusId != (int)ServiceComponentStatus.Evaluado)

                             select new Sigesoft.Node.WinClient.BE.ServiceComponentList
                             {
                                 v_ComponentId = A.v_ComponentId,
                                 v_ComponentName = C.v_Name,
                                 i_ServiceComponentStatusId = A.i_ServiceComponentStatusId.Value,
                                 v_ServiceComponentStatusName = B.v_Value1
                             }).ToList();

                pobjOperationResult.Success = 1;
                return query;
            }
            catch (Exception ex)
            {
                pobjOperationResult.Success = 0;
                pobjOperationResult.ExceptionMessage = Common.Utils.ExceptionFormatter(ex);
                return null;
            }
        }

        public serviceDto GetService(ref OperationResult pobjOperationResult, string pstrServiceId)
        {
            //mon.IsActive = true;

            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
                serviceDto objDtoEntity = null;

                var objEntity = (from a in dbContext.service
                                 where a.v_ServiceId == pstrServiceId
                                 select a).FirstOrDefault();

                if (objEntity != null)
                    objDtoEntity = serviceAssembler.ToDTO(objEntity);

                pobjOperationResult.Success = 1;
                return objDtoEntity;
            }
            catch (Exception ex)
            {
                pobjOperationResult.Success = 0;
                pobjOperationResult.ExceptionMessage = Common.Utils.ExceptionFormatter(ex);
                return null;
            }
        }

        public void UpdateService(ref OperationResult pobjOperationResult, serviceDto pobjDtoEntity, List<string> ClientSession)
        {
            //mon.IsActive = true;

            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                // Obtener la entidad fuente
                var objEntitySource = (from a in dbContext.service
                                       where a.v_ServiceId == pobjDtoEntity.v_ServiceId
                                       select a).FirstOrDefault();

                // Crear la entidad con los datos actualizados
                pobjDtoEntity.d_UpdateDate = DateTime.Now;
                pobjDtoEntity.i_UpdateUserId = Int32.Parse(ClientSession[2]);
                service objEntity = serviceAssembler.ToEntity(pobjDtoEntity);

                // Copiar los valores desde la entidad actualizada a la Entidad Fuente
                dbContext.service.ApplyCurrentValues(objEntity);

                // Guardar los cambios
                dbContext.SaveChanges();

                pobjOperationResult.Success = 1;
                // Llenar entidad Log
                LogBL.SaveLog(ClientSession[0], ClientSession[1], ClientSession[2], LogEventType.ACTUALIZACION, "SERVICIO", "v_ServiceId=" + objEntity.v_ServiceId.ToString(), Success.Ok, null);
                return;
            }
            catch (Exception ex)
            {
                pobjOperationResult.Success = 0;
                pobjOperationResult.ExceptionMessage = Common.Utils.ExceptionFormatter(ex);
                // Llenar entidad Log
                LogBL.SaveLog(ClientSession[0], ClientSession[1], ClientSession[2], LogEventType.ACTUALIZACION, "SERVICIO", "v_ServiceId=" + pobjDtoEntity.v_ServiceId.ToString(), Success.Failed, pobjOperationResult.ExceptionMessage);
                return;
            }
        }

        public void EliminarDxByDiagnosticRepositoryId(string pstrDiagosticReporsitoryId, List<string> ClientSession)
        {

            SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

            // Obtener la entidad fuente
            var ListaEliminar = (from a in dbContext.diagnosticrepository
                                 where a.v_DiagnosticRepositoryId == pstrDiagosticReporsitoryId 
                                 select a).ToList();


            foreach (var item in ListaEliminar)
            {
                // Crear la entidad con los datos actualizados
                item.d_UpdateDate = DateTime.Now;
                item.i_UpdateUserId = Int32.Parse(ClientSession[2]);
                item.i_IsDeleted = 1;

            }
            // Guardar los cambios
            dbContext.SaveChanges();


        }

        public List<Sigesoft.Node.WinClient.BE.ReportConsentimiento> GetReportConsentimiento(string pstrServiceId)
        {
            //mon.IsActive = true;
            var groupUbigeo = 113;
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                var objEntity = (from A in dbContext.service
                                 join B in dbContext.protocol on A.v_ProtocolId equals B.v_ProtocolId into B_join
                                 from B in B_join.DefaultIfEmpty()

                                 join C in dbContext.organization on B.v_WorkingOrganizationId equals C.v_OrganizationId into C_join
                                 from C in C_join.DefaultIfEmpty()

                                 join P1 in dbContext.person on new { a = A.v_PersonId }
                                         equals new { a = P1.v_PersonId } into P1_join
                                 from P1 in P1_join.DefaultIfEmpty()

                                 join p in dbContext.person on A.v_PersonId equals p.v_PersonId

                                 // Ubigeo de la persona *******************************************************
                                 join dep in dbContext.datahierarchy on new { a = p.i_DepartmentId.Value, b = groupUbigeo }
                                                      equals new { a = dep.i_ItemId, b = dep.i_GroupId } into dep_join
                                 from dep in dep_join.DefaultIfEmpty()

                                 join prov in dbContext.datahierarchy on new { a = p.i_ProvinceId.Value, b = groupUbigeo }
                                                       equals new { a = prov.i_ItemId, b = prov.i_GroupId } into prov_join
                                 from prov in prov_join.DefaultIfEmpty()

                                 join distri in dbContext.datahierarchy on new { a = p.i_DistrictId.Value, b = groupUbigeo }
                                                       equals new { a = distri.i_ItemId, b = distri.i_GroupId } into distri_join
                                 from distri in distri_join.DefaultIfEmpty()
                                 //*********************************************************************************************

                                 let varDpto = dep.v_Value1 == null ? "" : dep.v_Value1
                                 let varProv = prov.v_Value1 == null ? "" : prov.v_Value1
                                 let varDistri = distri.v_Value1 == null ? "" : distri.v_Value1

                                 where A.v_ServiceId == pstrServiceId

                                 select new Sigesoft.Node.WinClient.BE.ReportConsentimiento
                                 {
                                     NombreTrabajador = P1.v_FirstName + " " + P1.v_FirstLastName + " " + P1.v_SecondLastName,
                                     NroDocumento = P1.v_DocNumber,
                                     Ocupacion = P1.v_CurrentOccupation,
                                     Empresa = C.v_Name,
                                     FirmaTrabajador = P1.b_RubricImage,
                                     HuellaTrabajador = P1.b_FingerPrintImage,
                                     LugarProcedencia = varDistri + "-" + varProv + "-" + varDpto, // Santa Anita - Lima - Lima
                                     v_AdressLocation = p.v_AdressLocation,
                                     d_ServiceDate = A.d_ServiceDate,

                                 });

                var serviceBL = new ServiceBL();
                var MedicalCenter = serviceBL.GetInfoMedicalCenter();
                var ComponetesConcatenados = DevolverComponentesConcatenados(pstrServiceId);
                var ComponentesLaboratorioConcatenados = DevolverComponentesLaboratorioConcatenados(pstrServiceId);
                var sql = (from a in objEntity.ToList()
                           select new Sigesoft.Node.WinClient.BE.ReportConsentimiento
                           {
                               Fecha = DateTime.Now.ToShortDateString(),
                               Logo = MedicalCenter.b_Image,
                               NombreTrabajador = a.NombreTrabajador,
                               NroDocumento = a.NroDocumento,
                               Ocupacion = a.Ocupacion,
                               Empresa = a.Empresa,
                               FirmaTrabajador = a.FirmaTrabajador,
                               HuellaTrabajador = a.HuellaTrabajador,

                               b_Logo = MedicalCenter.b_Image,
                               EmpresaPropietaria = MedicalCenter.v_Name,
                               EmpresaPropietariaDireccion = MedicalCenter.v_Address,
                               EmpresaPropietariaTelefono = MedicalCenter.v_PhoneNumber,
                               EmpresaPropietariaEmail = MedicalCenter.v_Mail,
                               LugarProcedencia = a.LugarProcedencia,
                               v_AdressLocation = a.v_AdressLocation,
                               v_ServiceDate = a.d_ServiceDate == null ? string.Empty : a.d_ServiceDate.Value.ToShortDateString(),
                               Componentes = ComponetesConcatenados,
                               ComponentesLaboratorio = ComponentesLaboratorioConcatenados
                           }).ToList();

                return sql;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private string DevolverComponentesConcatenados(string pstrServiceId)
        {

            SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
            int isRequired = (int)SiNo.SI;
            var objEntity = (from A in dbContext.service
                             join B in dbContext.servicecomponent on A.v_ServiceId equals B.v_ServiceId
                             join C in dbContext.component on B.v_ComponentId equals C.v_ComponentId

                             join E in dbContext.systemparameter on new { a = C.i_CategoryId.Value, b = 116 }
                                                               equals new { a = E.i_ParameterId, b = E.i_GroupId } into E_join
                             from E in E_join.DefaultIfEmpty()

                             where A.v_ServiceId == pstrServiceId && C.i_CategoryId != 1 && B.i_IsDeleted == 0 && B.i_IsRequiredId == isRequired
                             select new
                             {
                                 NombreComponente = E.v_Value1
                             }).ToList().Distinct();

            return string.Join(", ", objEntity.Select(p => p.NombreComponente));
        }

        private string DevolverComponentesLaboratorioConcatenados(string pstrServiceId)
        {
            int isRequired = (int)SiNo.SI;
            SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

            var objEntity = (from A in dbContext.service
                             join B in dbContext.servicecomponent on A.v_ServiceId equals B.v_ServiceId
                             join C in dbContext.component on B.v_ComponentId equals C.v_ComponentId
                             where A.v_ServiceId == pstrServiceId && C.i_CategoryId == 1 && B.i_IsDeleted == 0 && B.v_ComponentId != Constants.INFORME_LABORATORIO_ID && B.i_IsRequiredId == isRequired
                             select new
                             {
                                 NombreComponente = C.v_Name
                             }).ToList();

            return string.Join(", ", objEntity.Select(p => p.NombreComponente));
        }

        public List<Sigesoft.Node.WinClient.BE.OsteomuscularNuevo> ReportOsteoMuscularNuevo(string pstrserviceId, string pstrComponentId)
        {
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                var objEntity = (from A in dbContext.service
                                 join B in dbContext.person on A.v_PersonId equals B.v_PersonId
                                 join E in dbContext.servicecomponent on new { a = pstrserviceId, b = pstrComponentId }
                                                                        equals new { a = E.v_ServiceId, b = E.v_ComponentId }

                                 // Usuario Medico Evaluador / Medico Aprobador ****************************
                                 join me in dbContext.systemuser on E.i_ApprovedUpdateUserId equals me.i_SystemUserId into me_join
                                 from me in me_join.DefaultIfEmpty()

                                 join pme in dbContext.professional on me.v_PersonId equals pme.v_PersonId into pme_join
                                 from pme in pme_join.DefaultIfEmpty()
                                 //**********************************************************************************************

                                 join I in dbContext.protocol on A.v_ProtocolId equals I.v_ProtocolId into I_join
                                 from I in I_join.DefaultIfEmpty()

                                 join J in dbContext.organization on I.v_EmployerOrganizationId equals J.v_OrganizationId

                                 join L in dbContext.systemparameter on new { a = I.i_EsoTypeId.Value, b = 118 }
                                                 equals new { a = L.i_ParameterId, b = L.i_GroupId } into L_join
                                 from L in L_join.DefaultIfEmpty()

                                 join Z in dbContext.person on me.v_PersonId equals Z.v_PersonId

                                 where A.v_ServiceId == pstrserviceId

                                 select new Sigesoft.Node.WinClient.BE.OsteomuscularNuevo
                                 {
                                     IdServicio = A.v_ServiceId,
                                     NOMBRE_PACIENTE = B.v_FirstName + " " + B.v_FirstLastName + " " + B.v_SecondLastName,
                                     PUESTO_TRABAJO = B.v_CurrentOccupation,
                                     EMPRESA_CLIENTE = J.v_Name,
                                     FechaNacimiento = B.d_Birthdate.Value,
                                     i_SEXO = B.i_SexTypeId.Value,
                                     NroDNI = B.v_DocNumber,
                                     FirmaGraba = pme.b_SignatureImage,
                                     FirmaTrabajador = B.b_FingerPrintImage,
                                     HuellaTrabajadr = B.b_RubricImage,
                                     FECHA_SERVICIO = A.d_ServiceDate.Value
                                 });

                var MedicalCenter = GetInfoMedicalCenter();


                var sql = (from a in objEntity.ToList()
                           let OsteoMuscular = new ServiceBL().ValoresComponente(pstrserviceId, pstrComponentId)
                           select new Sigesoft.Node.WinClient.BE.OsteomuscularNuevo
                           {
                               IdServicio = a.IdServicio,
                               FECHA_SERVICIO = a.FECHA_SERVICIO,
                               NOMBRE_PACIENTE = a.NOMBRE_PACIENTE,
                               PUESTO_TRABAJO = a.PUESTO_TRABAJO,
                               EMPRESA_CLIENTE = a.EMPRESA_CLIENTE,
                               FechaNacimiento = a.FechaNacimiento,
                               EDAD = GetAge(a.FechaNacimiento.Value),
                               FirmaGraba = a.FirmaGraba,
                               FirmaTrabajador = a.FirmaTrabajador,
                               HuellaTrabajadr = a.HuellaTrabajadr,
                               i_SEXO = a.i_SEXO,
                               SEXO = a.i_SEXO.ToString(),

                               LEVANTAMIENTO_ENCIMA_DEL_HOMBRO = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.LEVANTAMIENTO_ENCIMA_DEL_HOMBRO) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.LEVANTAMIENTO_ENCIMA_DEL_HOMBRO).v_Value1,
                               POSTURA_SEDENTARIA_DESCRIPCION = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.POSTURA_SEDENTARIA_DESCRIPCION) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.POSTURA_SEDENTARIA_DESCRIPCION).v_Value1,
                               MANIPULACION_JALAR_CARGAS = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.MANIPULACION_JALAR_CARGAS) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.MANIPULACION_JALAR_CARGAS).v_Value1,
                               MANIPULACION_LEVANTAR_CARGAS_DESCRIPCION = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.MANIPULACION_LEVANTAR_CARGAS_DESCRIPCION) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.MANIPULACION_LEVANTAR_CARGAS_DESCRIPCION).v_Value1,
                               MANIPULACION_EMPUJAR_CARGA_DESCRIPCION = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.MANIPULACION_EMPUJAR_CARGA_DESCRIPCION) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.MANIPULACION_EMPUJAR_CARGA_DESCRIPCION).v_Value1,
                               PESOS_SUPERIORES_A_25KGDESCRIPCION = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.PESOS_SUPERIORES_A_25KGDESCRIPCION) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.PESOS_SUPERIORES_A_25KGDESCRIPCION).v_Value1,
                               PMANIPULACIÓN_JALAR_CARGAS_DESCRIPCION = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.PMANIPULACIÓN_JALAR_CARGAS_DESCRIPCION) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.PMANIPULACIÓN_JALAR_CARGAS_DESCRIPCION).v_Value1,
                               PESOS_SUPERIORES_A_25KG = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.PESOS_SUPERIORES_A_25KG) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.PESOS_SUPERIORES_A_25KG).v_Value1,
                               MANIPULACION_LEVANTAR_CARGAS = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.MANIPULACION_LEVANTAR_CARGAS) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.MANIPULACION_LEVANTAR_CARGAS).v_Value1,
                               OSTEO_MUSCULAR_TAREAS_CARGA = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_TAREAS_CARGA) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_TAREAS_CARGA).v_Value1,

                               LEVANTAMIENTO_POR_ENCIMA_DELHOMBRODESCRIPCION = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.LEVANTAMIENTO_POR_ENCIMA_DELHOMBRODESCRIPCION) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.LEVANTAMIENTO_POR_ENCIMA_DELHOMBRODESCRIPCION).v_Value1,
                               MANIPULACION_DE_VALVULAS_ = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.MANIPULACION_DE_VALVULAS_) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.MANIPULACION_DE_VALVULAS_).v_Value1,
                               MANIPULACION_DE_VALVULAS_DESCRIPCION = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.MANIPULACION_DE_VALVULAS_DESCRIPCION) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.MANIPULACION_DE_VALVULAS_DESCRIPCION).v_Value1,
                               POSTURA_FORZADA__ = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.POSTURA_FORZADA__) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.POSTURA_FORZADA__).v_Value1,
                               POSTURA_FORZADA_DESCRIPCION_ = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.POSTURA_FORZADA_DESCRIPCION_) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.POSTURA_FORZADA_DESCRIPCION_).v_Value1,
                               POSTURA_SEDENTARIA__ = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.POSTURA_SEDENTARIA__) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.POSTURA_SEDENTARIA__).v_Value1,
                               MOVIMIENTOS_REPETITIVOS__ = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.MOVIMIENTOS_REPETITIVOS__) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.MOVIMIENTOS_REPETITIVOS__).v_Value1,
                               MOVIMIENTOS_REPETITIVOS_DESCRIPCION_ = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.MOVIMIENTOS_REPETITIVOS_DESCRIPCION_) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.MOVIMIENTOS_REPETITIVOS_DESCRIPCION_).v_Value1,
                               SINTOMAS = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.SINTOMAS) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.SINTOMAS).v_Value1,
                               CADERA_POBRE = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.CADERA_POBRE) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.CADERA_POBRE).v_Value1,

                               CADERA_PUNTOS = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.CADERA_PUNTOS) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.CADERA_PUNTOS).v_Value1,
                               CADRA_OBSERVACIONES = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.CADRA_OBSERVACIONES) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.CADRA_OBSERVACIONES).v_Value1,
                               MUSLO = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.MUSLO) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.MUSLO).v_Value1Name,
                               MUSLO_EXCELENTE = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.MUSLO_EXCELENTE) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.MUSLO_EXCELENTE).v_Value1,
                               _MUSLO_PROMEDIO = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants._MUSLO_PROMEDIO) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants._MUSLO_PROMEDIO).v_Value1,
                               MUSLO_REGULAR = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.MUSLO_REGULAR) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.MUSLO_REGULAR).v_Value1,
                               MUSLO_POBRE = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.MUSLO_POBRE) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.MUSLO_POBRE).v_Value1,
                               MUSLO_PUNTOS = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.MUSLO_PUNTOS) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.MUSLO_PUNTOS).v_Value1,
                               MUSLO_OBSERVACIONES = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.MUSLO_OBSERVACIONES) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.MUSLO_OBSERVACIONES).v_Value1,
                               ABDOMEN_LAT = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.ABDOMEN_LAT) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.ABDOMEN_LAT).v_Value1,

                               ABDOMEN_LAT_EXCELENTE = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.ABDOMEN_LAT_EXCELENTE) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.ABDOMEN_LAT_EXCELENTE).v_Value1,
                               ABDOMEN_LAT_PROMEDIO = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.ABDOMEN_LAT_PROMEDIO) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.ABDOMEN_LAT_PROMEDIO).v_Value1,
                               ABDOMEN_LAT_REGULAR = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.ABDOMEN_LAT_REGULAR) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.ABDOMEN_LAT_REGULAR).v_Value1,
                               ABDOMEN_LAT_POBRE = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.ABDOMEN_LAT_POBRE) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.ABDOMEN_LAT_POBRE).v_Value1,
                               ABDOMEN_LAT_PUNTOS = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.ABDOMEN_LAT_PUNTOS) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.ABDOMEN_LAT_PUNTOS).v_Value1,
                               ABDOMEN_LAT_OBSERVACIONES = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.ABDOMEN_LAT_OBSERVACIONES) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.ABDOMEN_LAT_OBSERVACIONES).v_Value1,
                               TOTAL = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.TOTAL) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.TOTAL).v_Value1,
                               ABDOMEN = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.ABDOMEN) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.ABDOMEN).v_Value1,
                               CADERA_EXCELENTE = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.CADERA_EXCELENTE) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.CADERA_EXCELENTE).v_Value1,
                               PUNTOS_ABDOMEN = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.PUNTOS_ABDOMEN) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.PUNTOS_ABDOMEN).v_Value1,

                               CADERA_PROMEDIO = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.CADERA_PROMEDIO) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.CADERA_PROMEDIO).v_Value1,
                               ABDOMEN_EXCELENTE = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.ABDOMEN_EXCELENTE) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.ABDOMEN_EXCELENTE).v_Value1,
                               ABDOMEN_PROMEDIO = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.ABDOMEN_PROMEDIO) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.ABDOMEN_PROMEDIO).v_Value1,
                               ABDOMEN_REGULAR = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.ABDOMEN_REGULAR) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.ABDOMEN_REGULAR).v_Value1,
                               ABDOMEN_POBRE = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.ABDOMEN_POBRE) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.ABDOMEN_POBRE).v_Value1,
                               CADERA_REGULAR = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.CADERA_REGULAR) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.CADERA_REGULAR).v_Value1,
                               ABDOMEN_OBSERVACIONES = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.ABDOMEN_OBSERVACIONES) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.ABDOMEN_OBSERVACIONES).v_Value1,
                               CADERA = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.CADERA) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.CADERA).v_Value1Name,
                               FLEXION = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.FLEXION) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.FLEXION).v_Value1Name,
                               HOMBRO_IZQFLEXION = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.HOMBRO_IZQFLEXION) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.HOMBRO_IZQFLEXION).v_Value1Name,

                               HOMBRO_IZQEXTENSION = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.HOMBRO_IZQEXTENSION) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.HOMBRO_IZQEXTENSION).v_Value1Name,
                               HOMBRO_IZQROTDER = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.HOMBRO_IZQROTDER) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.HOMBRO_IZQROTDER).v_Value1Name,
                               HOMBRO_IZQROTIZQ = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.HOMBRO_IZQROTIZQ) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.HOMBRO_IZQROTIZQ).v_Value1Name,
                               HOMBRO_IZQFUERZA = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.HOMBRO_IZQFUERZA) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.HOMBRO_IZQFUERZA).v_Value1Name,
                               HOMBRO_IZQDOLOR = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.HOMBRO_IZQDOLOR) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.HOMBRO_IZQDOLOR).v_Value1Name,
                               CODO_DCH = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.CODO_DCH) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.CODO_DCH).v_Value1Name,
                               CODO_DCHABDUCC = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.CODO_DCHABDUCC) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.CODO_DCHABDUCC).v_Value1Name,
                               CODO_DCHFELXION = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.CODO_DCHFELXION) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.CODO_DCHFELXION).v_Value1Name,
                               CODO_DCHEXTENSION = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.CODO_DCHEXTENSION) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.CODO_DCHEXTENSION).v_Value1Name,
                               CODO_DCHROTDER = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.CODO_DCHROTDER) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.CODO_DCHROTDER).v_Value1Name,

                               CODO_DCHROTIZQ = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.CODO_DCHROTIZQ) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.CODO_DCHROTIZQ).v_Value1Name,
                               CODO_DCHDOLOR = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.CODO_DCHDOLOR) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.CODO_DCHDOLOR).v_Value1Name,
                               CODO_DCHFUERZA = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.CODO_DCHFUERZA) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.CODO_DCHFUERZA).v_Value1Name,
                               CODO_IZQ = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.CODO_IZQ) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.CODO_IZQ).v_Value1Name,
                               CODO_IZQABDUCC = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.CODO_IZQABDUCC) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.CODO_IZQABDUCC).v_Value1Name,
                               CODO_IZQFLEXION = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.CODO_IZQFLEXION) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.CODO_IZQFLEXION).v_Value1Name,
                               CODO_IZQEXTENSION = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.CODO_IZQEXTENSION) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.CODO_IZQEXTENSION).v_Value1Name,
                               CODO_IZQROTEXT = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.CODO_IZQROTEXT) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.CODO_IZQROTEXT).v_Value1Name,
                               CODO_IZQROT_INT = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.CODO_IZQROT_INT) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.CODO_IZQROT_INT).v_Value1Name,
                               CODO_IZQFUERZA = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.CODO_IZQFUERZA) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.CODO_IZQFUERZA).v_Value1Name,

                               CODO_IZQDOLOR = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.CODO_IZQDOLOR) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.CODO_IZQDOLOR).v_Value1Name,
                               MUNIECA_DCH = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.MUNIECA_DCH) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.MUNIECA_DCH).v_Value1Name,
                               MUNIEECA_DCHABDUCC = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.MUNIEECA_DCHABDUCC) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.MUNIEECA_DCHABDUCC).v_Value1Name,
                               MUNIEECA_DCHFLEXION = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.MUNIEECA_DCHFLEXION) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.MUNIEECA_DCHFLEXION).v_Value1Name,
                               MUNIEECA_DCHEXTENSION = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.MUNIEECA_DCHEXTENSION) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.MUNIEECA_DCHEXTENSION).v_Value1Name,
                               MUNIEECA_DCHROTEXT = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.MUNIEECA_DCHROTEXT) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.MUNIEECA_DCHROTEXT).v_Value1Name,
                               MUNIEECA_DCHROTINT = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.MUNIEECA_DCHROTINT) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.MUNIEECA_DCHROTINT).v_Value1Name,
                               MUNIEECA_DCHFUERZA = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.MUNIEECA_DCHFUERZA) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.MUNIEECA_DCHFUERZA).v_Value1Name,
                               MUNIEECA_DCHDOLOR = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.MUNIEECA_DCHDOLOR) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.MUNIEECA_DCHDOLOR).v_Value1Name,
                               MUNIEECA_IZQ = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.MUNIEECA_IZQ) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.MUNIEECA_IZQ).v_Value1Name,

                               MUNIEECA_IZQABDUCC = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.MUNIEECA_IZQABDUCC) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.MUNIEECA_IZQABDUCC).v_Value1Name,
                               MUNIEECA_IZQFLEXION = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.MUNIEECA_IZQFLEXION) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.MUNIEECA_IZQFLEXION).v_Value1Name,
                               MUNIEECA_IZQEXTENSION = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.MUNIEECA_IZQEXTENSION) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.MUNIEECA_IZQEXTENSION).v_Value1Name,
                               MUNIEECA_IZQROTEXT = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.MUNIEECA_IZQROTEXT) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.MUNIEECA_IZQROTEXT).v_Value1Name,
                               MUNIEECA_IZQROTINT = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.MUNIEECA_IZQROTINT) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.MUNIEECA_IZQROTINT).v_Value1Name,
                               MUNIEECA_IZQFUERZA = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.MUNIEECA_IZQFUERZA) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.MUNIEECA_IZQFUERZA).v_Value1Name,
                               MUNIEECA_IZQDOLOR = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.MUNIEECA_IZQDOLOR) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.MUNIEECA_IZQDOLOR).v_Value1Name,
                               CADERA_DCH = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.CADERA_DCH) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.CADERA_DCH).v_Value1Name,
                               CADERA_DCHABDUCC = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.CADERA_DCHABDUCC) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.CADERA_DCHABDUCC).v_Value1Name,
                               CADERA_DCHFLEXION = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.CADERA_DCHFLEXION) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.CADERA_DCHFLEXION).v_Value1Name,

                               CADERA_DCHEXTENSION = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.CADERA_DCHEXTENSION) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.CADERA_DCHEXTENSION).v_Value1Name,
                               CADERA_DCHROTEXT = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.CADERA_DCHROTEXT) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.CADERA_DCHROTEXT).v_Value1Name,
                               CADERA_DCHROTINT = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.CADERA_DCHROTINT) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.CADERA_DCHROTINT).v_Value1Name,
                               CADERA_DCHFUERZA = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.CADERA_DCHFUERZA) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.CADERA_DCHFUERZA).v_Value1Name,
                               CADERA_DCHDOLOR = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.CADERA_DCHDOLOR) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.CADERA_DCHDOLOR).v_Value1Name,
                               CADERA_IZQ = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.CADERA_IZQ) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.CADERA_IZQ).v_Value1Name,
                               CADERA_IZQABDUC = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.CADERA_IZQABDUC) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.CADERA_IZQABDUC).v_Value1Name,
                               CADERA_IZQFLEXION = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.CADERA_IZQFLEXION) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.CADERA_IZQFLEXION).v_Value1Name,
                               CADERA_IZQROTEXT = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.CADERA_IZQROTEXT) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.CADERA_IZQROTEXT).v_Value1Name,
                               CADERA_IZQROTINT = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.CADERA_IZQROTINT) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.CADERA_IZQROTINT).v_Value1Name,


                               CADERA_IZQEXTENSION = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.CADERA_IZQEXTENSION) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.CADERA_IZQEXTENSION).v_Value1Name,
                               CADERA_IZQFUERZA = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.CADERA_IZQFUERZA) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.CADERA_IZQFUERZA).v_Value1Name,
                               CADERA_IZQDOLOR = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.CADERA_IZQDOLOR) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.CADERA_IZQDOLOR).v_Value1Name,
                               RODILLA_DCH = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.RODILLA_DCH) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.RODILLA_DCH).v_Value1Name,
                               RODILLA_DCHABDUCC = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.RODILLA_DCHABDUCC) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.RODILLA_DCHABDUCC).v_Value1Name,
                               RODILLA_DCHFLEXION = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.RODILLA_DCHFLEXION) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.RODILLA_DCHFLEXION).v_Value1Name,
                               RODILLA_DCHEXTENSION = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.RODILLA_DCHEXTENSION) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.RODILLA_DCHEXTENSION).v_Value1Name,
                               RODILLA_DCHROTEXT = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.RODILLA_DCHROTEXT) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.RODILLA_DCHROTEXT).v_Value1Name,
                               RODILLA_DCHROTINT = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.RODILLA_DCHROTINT) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.RODILLA_DCHROTINT).v_Value1Name,
                               RODILLA_DCHFUERZA = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.RODILLA_DCHFUERZA) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.RODILLA_DCHFUERZA).v_Value1Name,

                               RODILLA_DCHDOLOR = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.RODILLA_DCHDOLOR) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.RODILLA_DCHDOLOR).v_Value1Name,
                               RODILLA_IZQ = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.RODILLA_IZQ) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.RODILLA_IZQ).v_Value1Name,
                               RODILLA_IZQABDUCC = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.RODILLA_IZQABDUCC) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.RODILLA_IZQABDUCC).v_Value1Name,
                               RODILLA_IZQFLEXION = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.RODILLA_IZQFLEXION) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.RODILLA_IZQFLEXION).v_Value1Name,
                               RODILLA_IZQEXTENSION = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.RODILLA_IZQEXTENSION) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.RODILLA_IZQEXTENSION).v_Value1Name,
                               RODILLA_IZQROTEXT = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.RODILLA_IZQROTEXT) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.RODILLA_IZQROTEXT).v_Value1Name,
                               RODILLA_IZQINT = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.RODILLA_IZQINT) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.RODILLA_IZQINT).v_Value1Name,
                               RODILLA_IZQFUERZA = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.RODILLA_IZQFUERZA) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.RODILLA_IZQFUERZA).v_Value1Name,
                               RODILLA_IZQDOLOR = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.RODILLA_IZQDOLOR) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.RODILLA_IZQDOLOR).v_Value1Name,
                               TOBILLO_DCH = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.TOBILLO_DCH) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.TOBILLO_DCH).v_Value1Name,

                               TOBILLO_DCHABDUCC = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.TOBILLO_DCHABDUCC) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.TOBILLO_DCHABDUCC).v_Value1Name,
                               TOBILLO_DCHFLEXION = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.TOBILLO_DCHFLEXION) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.TOBILLO_DCHFLEXION).v_Value1Name,
                               TOBILLO_DCHEXTENSION = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.TOBILLO_DCHEXTENSION) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.TOBILLO_DCHEXTENSION).v_Value1Name,
                               TOBILLO_DCHROTEXT = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.TOBILLO_DCHROTEXT) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.TOBILLO_DCHROTEXT).v_Value1Name,
                               TOBILLO_DCHROTINT = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.TOBILLO_DCHROTINT) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.TOBILLO_DCHROTINT).v_Value1Name,
                               TOBILLO_DCHFUERZA = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.TOBILLO_DCHFUERZA) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.TOBILLO_DCHFUERZA).v_Value1Name,
                               TOBILLO_DCHDOLOR = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.TOBILLO_DCHDOLOR) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.TOBILLO_DCHDOLOR).v_Value1Name,
                               TOBILLO_IZQ = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.TOBILLO_IZQ) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.TOBILLO_IZQ).v_Value1Name,
                               TOBILLO_IZQABDUCC = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.TOBILLO_IZQABDUCC) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.TOBILLO_IZQABDUCC).v_Value1Name,
                               TOBILLO_IZQFLEXION = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.TOBILLO_IZQFLEXION) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.TOBILLO_IZQFLEXION).v_Value1Name,

                               TOBILLO_IZQROTEXT = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.TOBILLO_IZQROTEXT) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.TOBILLO_IZQROTEXT).v_Value1Name,
                               TOBILLO_IZQEXTENSION = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.TOBILLO_IZQEXTENSION) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.TOBILLO_IZQEXTENSION).v_Value1Name,
                               TOBILLO_IZQROTINT = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.TOBILLO_IZQROTINT) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.TOBILLO_IZQROTINT).v_Value1Name,
                               TOBILLO_IZQFUERZA = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.TOBILLO_IZQFUERZA) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.TOBILLO_IZQFUERZA).v_Value1Name,
                               TOBILLO_IZQDOLOR = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.TOBILLO_IZQDOLOR) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.TOBILLO_IZQDOLOR).v_Value1Name,
                               HOMBRO_IZQABDUCC = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.HOMBRO_IZQABDUCC) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.HOMBRO_IZQABDUCC).v_Value1Name,
                               HOMBRO_DCHABDUCC = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.HOMBRO_DCHABDUCC) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.HOMBRO_DCHABDUCC).v_Value1Name,
                               HOMBRO_DCH = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.HOMBRO_DCH) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.HOMBRO_DCH).v_Value1Name,
                               HOMBRO_DCHFLEXION = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.HOMBRO_DCHFLEXION) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.HOMBRO_DCHFLEXION).v_Value1Name,
                               HOMBRO_DCHROTEXT = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.HOMBRO_DCHROTEXT) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.HOMBRO_DCHROTEXT).v_Value1Name,

                               HOMBRO_DCHDOLOR = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.HOMBRO_DCHDOLOR) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.HOMBRO_DCHDOLOR).v_Value1Name,
                               HOMBRO_DCHFUERZATONO = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.HOMBRO_DCHFUERZATONO) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.HOMBRO_DCHFUERZATONO).v_Value1Name,
                               HOMBRO_DCHEXTENSION = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.HOMBRO_DCHEXTENSION) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.HOMBRO_DCHEXTENSION).v_Value1Name,
                               HOMBRO_IZQ = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.HOMBRO_IZQ) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.HOMBRO_IZQ).v_Value1Name,
                               HOMBRO_DCHROTINT = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.HOMBRO_DCHROTINT) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.HOMBRO_DCHROTINT).v_Value1Name,
                               OBSERVACION = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OBSERVACION) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OBSERVACION).v_Value1,
                               TINEL_DERECHO = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.TINEL_DERECHO) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.TINEL_DERECHO).v_Value1,
                               TINEL_IZQUIERDO = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.TINEL_IZQUIERDO) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.TINEL_IZQUIERDO).v_Value1,
                               LASEGUE_DERECHO = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.LASEGUE_DERECHO) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.LASEGUE_DERECHO).v_Value1,
                               LASEGUE_IZQUIERDO = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.LASEGUE_IZQUIERDO) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.LASEGUE_IZQUIERDO).v_Value1,

                               FINKELSTEIN_DERECHO = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.FINKELSTEIN_DERECHO) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.FINKELSTEIN_DERECHO).v_Value1,
                               ADAM_DERECHO = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.ADAM_DERECHO) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.ADAM_DERECHO).v_Value1,
                               FINKELSTEIN_IZQUIERDO = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.FINKELSTEIN_IZQUIERDO) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.FINKELSTEIN_IZQUIERDO).v_Value1,
                               ADAM_IZQUIERDO = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.ADAM_IZQUIERDO) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.ADAM_IZQUIERDO).v_Value1,
                               PHALEN_DERECHO = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.PHALEN_DERECHO) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.PHALEN_DERECHO).v_Value1,
                               PHALEN_IZQUIERDO = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.PHALEN_IZQUIERDO) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.PHALEN_IZQUIERDO).v_Value1,
                               PIE_CAVO_DERECHO = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.PIE_CAVO_DERECHO) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.PIE_CAVO_DERECHO).v_Value1,
                               PIE_CAVO_IZQUIERDO = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.PIE_CAVO_IZQUIERDO) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.PIE_CAVO_IZQUIERDO).v_Value1,
                               PIE_PLANO_DERECHO = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.PIE_PLANO_DERECHO) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.PIE_PLANO_DERECHO).v_Value1,
                               PIE_PLANO_IZQUIERDO = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.PIE_PLANO_IZQUIERDO) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.PIE_PLANO_IZQUIERDO).v_Value1Name,

                               CERVICAL_AP = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.CERVICAL_AP) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.CERVICAL_AP).v_Value1,
                               DORSAL_AP = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.DORSAL_AP) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.DORSAL_AP).v_Value1,
                               LUMBAR_AP = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.LUMBAR_AP) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.LUMBAR_AP).v_Value1,
                               DORSAL_LATERAL = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.DORSAL_LATERAL) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.DORSAL_LATERAL).v_Value1,
                               LUMBAR_LATERAL = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.LUMBAR_LATERAL) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.LUMBAR_LATERAL).v_Value1,
                               CERVICAL_LATERALIZACION_DERECHA = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.CERVICAL_LATERALIZACION_DERECHA) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.CERVICAL_LATERALIZACION_DERECHA).v_Value1Name,
                               CERVICAL_LATERALIZACION_IZQUIERDA = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.CERVICAL_LATERALIZACION_IZQUIERDA) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.CERVICAL_LATERALIZACION_IZQUIERDA).v_Value1Name,
                               DORSAL_LUMBAR_LATERAL_IZQUIERDA = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.DORSAL_LUMBAR_LATERAL_IZQUIERDA) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.DORSAL_LUMBAR_LATERAL_IZQUIERDA).v_Value1Name,
                               DORSAL_LUMBAR_ROACION_DERECHA = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.DORSAL_LUMBAR_ROACION_DERECHA) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.DORSAL_LUMBAR_ROACION_DERECHA).v_Value1Name,
                               CERVICAL_EXTENXION__ = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.CERVICAL_EXTENXION__) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.CERVICAL_EXTENXION__).v_Value1Name,

                               DORSAL_LUMBAR = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.DORSAL_LUMBAR) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.DORSAL_LUMBAR).v_Value1Name,
                               DORSAL_LUMBAR_EXTENSION = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.DORSAL_LUMBAR_EXTENSION) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.DORSAL_LUMBAR_EXTENSION).v_Value1Name,
                               CERVICAL_ROTACION_DERECHA = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.CERVICAL_ROTACION_DERECHA) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.CERVICAL_ROTACION_DERECHA).v_Value1Name,
                               CERVICAL_ROTACION_IZQUIERDA = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.CERVICAL_ROTACION_IZQUIERDA) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.CERVICAL_ROTACION_IZQUIERDA).v_Value1Name,
                               CERVICAL_IRRADIACION = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.CERVICAL_IRRADIACION) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.CERVICAL_IRRADIACION).v_Value1Name,
                               DORSAL_LUMBAR_LATERAL_DERECHA = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.DORSAL_LUMBAR_LATERAL_DERECHA) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.DORSAL_LUMBAR_LATERAL_DERECHA).v_Value1Name,
                               CERVICAL_FLEXION = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.CERVICAL_FLEXION) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.CERVICAL_FLEXION).v_Value1Name,
                               DORSAL_LUMBAR_IRRADIACION = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.DORSAL_LUMBAR_IRRADIACION) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.DORSAL_LUMBAR_IRRADIACION).v_Value1Name,
                               DORSAL_LUMBAR_ROACION_IZQUIERDA = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.DORSAL_LUMBAR_ROACION_IZQUIERDA) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.DORSAL_LUMBAR_ROACION_IZQUIERDA).v_Value1Name,
                               COLUMNA_CERVICAL_CONTRACTURA = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.COLUMNA_CERVICAL_CONTRACTURA) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.COLUMNA_CERVICAL_CONTRACTURA).v_Value1Name,

                               COLUMNA_LUMBAR = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.COLUMNA_LUMBAR) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.COLUMNA_LUMBAR).v_Value1,
                               COLUMNA_DORSAL_CONTRACTURA = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.COLUMNA_DORSAL_CONTRACTURA) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.COLUMNA_DORSAL_CONTRACTURA).v_Value1,
                               COLUMNA_LUMBAR_CONTACTURA = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.COLUMNA_LUMBAR_CONTACTURA) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.COLUMNA_LUMBAR_CONTACTURA).v_Value1,
                               COLUMNA_DORSAL = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.COLUMNA_DORSAL) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.COLUMNA_DORSAL).v_Value1,
                               COLUMNA_CERVICAL = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.COLUMNA_CERVICAL) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.COLUMNA_CERVICAL).v_Value1,
                               DESCRIPCION = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.DESCRIPCION) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.DESCRIPCION).v_Value1,
                               b_Logo = MedicalCenter.b_Image,
                               EmpresaPropietaria = MedicalCenter.v_Name,
                               EmpresaPropietariaDireccion = MedicalCenter.v_Address,
                               EmpresaPropietariaTelefono = MedicalCenter.v_PhoneNumber,
                               EmpresaPropietariaEmail = MedicalCenter.v_Mail,
                               NroDNI = a.NroDNI,
                               Recomendaciones = GetRecomendationByServiceIdAndComponentConcatec(a.IdServicio, Constants.OSTEO_MUSCULAR_ID_1),
                               DxCIE10 = GetDisgnosticsCIE10ByServiceIdAndComponentConcatec(a.IdServicio, Constants.OSTEO_MUSCULAR_ID_1)
                           }).ToList();

                return sql;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public string GetDisgnosticsCIE10ByServiceIdAndComponentConcatec(string pstrServiceId, string pstrComponentId)
        {
            //mon.IsActive = true;

            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
                var query = (from ccc in dbContext.diagnosticrepository
                             join ddd in dbContext.diseases on ccc.v_DiseasesId equals ddd.v_DiseasesId  // Diagnosticos                                                  


                             where ccc.v_ServiceId == pstrServiceId && ccc.v_ComponentId == pstrComponentId &&
                                   ccc.i_IsDeleted == 0

                             select new
                             {
                                 v_DiseasesName = ddd.v_Name + "/" + ddd.v_CIE10Id,

                             }).ToList();


                return string.Join(", ", query.Select(p => p.v_DiseasesName));
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public string GetRecomendationByServiceIdAndComponentConcatec(string pstrServiceId, string pstrComponentId)
        {
            //mon.IsActive = true;

            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
                var query = (from ccc in dbContext.recommendation
                             join ddd in dbContext.masterrecommendationrestricction on ccc.v_MasterRecommendationId equals ddd.v_MasterRecommendationRestricctionId


                             where ccc.v_ServiceId == pstrServiceId && ccc.v_ComponentId == pstrComponentId &&
                                   ccc.i_IsDeleted == 0

                             select new
                             {

                                 Recomendations = ddd.v_Name,

                             }).ToList();


                return string.Join(", ", query.Select(p => p.Recomendations));
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public List<Sigesoft.Node.WinClient.BE.UcOsteo> ReporteOsteomuscular(string pstrserviceId, string pstrComponentId)
        {
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                var objEntity = (from A in dbContext.service
                                 join B in dbContext.person on A.v_PersonId equals B.v_PersonId
                                 join C in dbContext.servicecomponent on new { a = pstrserviceId, b = pstrComponentId }
                                                                       equals new { a = C.v_ServiceId, b = C.v_ComponentId }

                                 // Usuario Medico Evaluador / Medico Aprobador ****************************
                                 join me in dbContext.systemuser on C.i_ApprovedUpdateUserId equals me.i_SystemUserId into me_join
                                 from me in me_join.DefaultIfEmpty()

                                 join pme in dbContext.professional on me.v_PersonId equals pme.v_PersonId into pme_join
                                 from pme in pme_join.DefaultIfEmpty()

                                 join Z in dbContext.person on me.v_PersonId equals Z.v_PersonId

                                 join E1 in dbContext.protocol on A.v_ProtocolId equals E1.v_ProtocolId

                                 join et in dbContext.systemparameter on new { a = E1.i_EsoTypeId.Value, b = 118 }
                                               equals new { a = et.i_ParameterId, b = et.i_GroupId } into et_join  // TIPO ESO [ESOA,ESOR,ETC]
                                 from et in et_join.DefaultIfEmpty()
                                 where A.v_ServiceId == pstrserviceId
                                 select new Sigesoft.Node.WinClient.BE.UcOsteo
                                 {
                                     Trabajador = B.v_FirstLastName + " " + B.v_FirstLastName + " " + B.v_FirstName,
                                     FechaNacimiento = B.d_Birthdate,
                                     FechaEvaluacion = A.d_ServiceDate,
                                     Puesto = B.v_CurrentOccupation,
                                     ServicioId = A.v_ServiceId,
                                     FirmaUsuarioGraba = pme.b_SignatureImage,
                                     NombreUsuarioGraba = Z.v_FirstLastName + " " + Z.v_FirstLastName + " " + Z.v_FirstName,
                                     HuellaTrabajador = B.b_FingerPrintImage,
                                     FirmaTrabajador = B.b_RubricImage,
                                     TipoEso = et.v_Value1,
                                     Dni = B.v_DocNumber
                                 });

                var ValorUSer = ValoresComponenteOdontogramaValue1(pstrserviceId, Constants.EXAMEN_FISICO_ID).ToList();
                var MedicalCenter = GetInfoMedicalCenter();
                var sql = (from a in objEntity.ToList()
                           select new Sigesoft.Node.WinClient.BE.UcOsteo
                           {
                               TipoEso = a.TipoEso,
                               Dni = a.Dni,
                               Trabajador = a.Trabajador,
                               FechaNacimiento = a.FechaNacimiento,
                               FechaEvaluacion = a.FechaEvaluacion,
                               Puesto = a.Puesto,
                               ServicioId = a.ServicioId,
                               FirmaUsuarioGraba = a.FirmaUsuarioGraba,
                               NombreUsuarioGraba = a.NombreUsuarioGraba,
                               HuellaTrabajador = a.HuellaTrabajador,
                               FirmaTrabajador = a.FirmaTrabajador,
                               Edad = GetAge(a.FechaNacimiento.Value),


                               UC_OSTEO_FLEXIBILIDAD = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.UC_OSTEO_FLEXIBILIDAD) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.UC_OSTEO_FLEXIBILIDAD).v_Value1,
                               UC_OSTEO_FLEXIBILIDAD_PTJ = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.UC_OSTEO_FLEXIBILIDAD_PTJ) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.UC_OSTEO_FLEXIBILIDAD_PTJ).v_Value1,
                               UC_OSTEO_FLEXIBILIDAD_OBS = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.UC_OSTEO_FLEXIBILIDAD_OBS) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.UC_OSTEO_FLEXIBILIDAD_OBS).v_Value1,
                               UC_OSTEO_CADERA = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.UC_OSTEO_CADERA) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.UC_OSTEO_CADERA).v_Value1,
                               UC_OSTEO_CADERA_PTJ = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.UC_OSTEO_CADERA_PTJ) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.UC_OSTEO_CADERA_PTJ).v_Value1,
                               UC_OSTEO_CADERA_OBS = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.UC_OSTEO_CADERA_OBS) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.UC_OSTEO_CADERA_OBS).v_Value1,
                               UC_OSTEO_MUSLO = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.UC_OSTEO_MUSLO) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.UC_OSTEO_MUSLO).v_Value1,
                               UC_OSTEO_MUSLO_PTJ = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.UC_OSTEO_MUSLO_PTJ) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.UC_OSTEO_MUSLO_PTJ).v_Value1,
                               UC_OSTEO_MUSLO_OBS = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.UC_OSTEO_MUSLO_OBS) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.UC_OSTEO_MUSLO_OBS).v_Value1,
                               UC_OSTEO_ABDOMEN = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.UC_OSTEO_ABDOMEN) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.UC_OSTEO_ABDOMEN).v_Value1,


                               UC_OSTEO_ABDOMEN_PTJ = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.UC_OSTEO_ABDOMEN_PTJ) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.UC_OSTEO_ABDOMEN_PTJ).v_Value1,
                               UC_OSTEO_ABDOMEN_OBS = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.UC_OSTEO_ABDOMEN_OBS) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.UC_OSTEO_ABDOMEN_OBS).v_Value1,
                               UC_OSTEO_ABD_180 = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.UC_OSTEO_ABD_180) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.UC_OSTEO_ABD_180).v_Value1,
                               UC_OSTEO_ABD_180_PTJ = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.UC_OSTEO_ABD_180_PTJ) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.UC_OSTEO_ABD_180_PTJ).v_Value1,
                               UC_OSTEO_ABD_180_SINO = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.UC_OSTEO_ABD_180_SINO) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.UC_OSTEO_ABD_180_SINO).v_Value1,
                               UC_OSTEO_ABD_60 = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.UC_OSTEO_ABD_60) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.UC_OSTEO_ABD_60).v_Value1,
                               UC_OSTEO_ABD_60_PTJ = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.UC_OSTEO_ABD_60_PTJ) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.UC_OSTEO_ABD_60_PTJ).v_Value1,
                               UC_OSTEO_ABD_60_SINO = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.UC_OSTEO_ABD_60_SINO) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.UC_OSTEO_ABD_60_SINO).v_Value1,
                               UC_OSTEO_ABD_90 = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.UC_OSTEO_ABD_90) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.UC_OSTEO_ABD_90).v_Value1,
                               UC_OSTEO_ABD_90_PTJ = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.UC_OSTEO_ABD_90_PTJ) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.UC_OSTEO_ABD_90_PTJ).v_Value1,


                               UC_OSTEO_ABD_90_SINO = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.UC_OSTEO_ABD_90_SINO) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.UC_OSTEO_ABD_90_SINO).v_Value1,
                               UC_OSTEO_ROTACION = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.UC_OSTEO_ROTACION) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.UC_OSTEO_ROTACION).v_Value1,
                               UC_OSTEO_ROTACION_PTJ = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.UC_OSTEO_ROTACION_PTJ) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.UC_OSTEO_ROTACION_PTJ).v_Value1,
                               UC_OSTEO_ROTACION_SINO = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.UC_OSTEO_ROTACION_SINO) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.UC_OSTEO_ROTACION_SINO).v_Value1,
                               UC_OSTEO_OBS = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.UC_OSTEO_OBS) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.UC_OSTEO_OBS).v_Value1,
                               UC_OSTEO_TOTAL1 = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.UC_OSTEO_TOTAL1) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.UC_OSTEO_TOTAL1).v_Value1,
                               UC_OSTEO_TOTAL2 = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.UC_OSTEO_TOTAL2) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.UC_OSTEO_TOTAL2).v_Value1,


                               b_Logo = MedicalCenter.b_Image,
                               EmpresaPropietaria = MedicalCenter.v_Name,
                               EmpresaPropietariaDireccion = MedicalCenter.v_Address,
                               EmpresaPropietariaTelefono = MedicalCenter.v_PhoneNumber,
                               EmpresaPropietariaEmail = MedicalCenter.v_Mail,

                           }).ToList();


                return sql;

            }
            catch (Exception)
            {

                throw;
            }


        }

        public List<Sigesoft.Node.WinClient.BE.ServiceList> GetMusculoEsqueletico(string pstrserviceId, string pstrComponentId)
        {
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                var objEntity = (from A in dbContext.service
                                 join B in dbContext.person on A.v_PersonId equals B.v_PersonId into B_join
                                 from B in B_join.DefaultIfEmpty()
                                 join C in dbContext.protocol on A.v_ProtocolId equals C.v_ProtocolId into C_join
                                 from C in C_join.DefaultIfEmpty()
                                 join D in dbContext.organization on C.v_WorkingOrganizationId equals D.v_OrganizationId into D_join
                                 from D in D_join.DefaultIfEmpty()

                                 join E in dbContext.servicecomponent on new { a = A.v_ServiceId, b = pstrComponentId }
                                                                        equals new { a = E.v_ServiceId, b = E.v_ComponentId }

                                 join J1 in dbContext.datahierarchy on new { a = B.i_LevelOfId.Value, b = 108 }
                                                                    equals new { a = J1.i_ItemId, b = J1.i_GroupId } into J1_join
                                 from J1 in J1_join.DefaultIfEmpty()

                                 join F in dbContext.systemuser on E.i_InsertUserId equals F.i_SystemUserId
                                 join G in dbContext.professional on F.v_PersonId equals G.v_PersonId into G_join
                                 from G in G_join.DefaultIfEmpty()

                                 join F1 in dbContext.systemuser on E.i_ApprovedUpdateUserId equals F1.i_SystemUserId into F1_join
                                 from F1 in F1_join.DefaultIfEmpty()

                                 join Z in dbContext.person on F.v_PersonId equals Z.v_PersonId

                                 where A.v_ServiceId == pstrserviceId
                                 select new Sigesoft.Node.WinClient.BE.ServiceList
                                 {
                                     v_PersonId = A.v_PersonId,
                                     v_Pacient = B.v_FirstLastName + " " + B.v_SecondLastName + " " + B.v_FirstName,
                                     d_ServiceDate = A.d_ServiceDate,
                                     EmpresaTrabajo = D.v_Name,
                                     v_ServiceId = A.v_ServiceId,
                                     v_ComponentId = E.v_ServiceComponentId,
                                     NombreUsuarioGraba = Z.v_FirstLastName + " " + Z.v_SecondLastName + " " + Z.v_FirstName,
                                 });

                var serviceBL = new ServiceBL();
                var MedicalCenter = serviceBL.GetInfoMedicalCenter();

                var sql = (from a in objEntity.ToList()

                           let OsteoMuscular = new ServiceBL().ValoresComponente(pstrserviceId, Constants.OSTEO_MUSCULAR_ID_1)
                           select new Sigesoft.Node.WinClient.BE.ServiceList
                           {
                               v_PersonId = a.v_PersonId,
                               v_Pacient = a.v_Pacient,
                               d_ServiceDate = a.d_ServiceDate,
                               EmpresaTrabajo = a.EmpresaTrabajo,
                               v_ServiceId = a.v_ServiceId,
                               v_ComponentId = a.v_ComponentId,

                               AbdomenExcelente = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.ABDOMEN_EXCELENTE) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.ABDOMEN_EXCELENTE).v_Value1,
                               AbdomenPromedio = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.ABDOMEN_PROMEDIO) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.ABDOMEN_PROMEDIO).v_Value1,
                               AbdomenRegular = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.ABDOMEN_REGULAR) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.ABDOMEN_REGULAR).v_Value1,
                               AbdomenPobre = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.ABDOMEN_POBRE) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.ABDOMEN_POBRE).v_Value1,
                               AbdomenPuntos = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.PUNTOS_ABDOMEN) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.PUNTOS_ABDOMEN).v_Value1,
                               AbdomenObs = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.ABDOMEN_OBSERVACIONES) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.ABDOMEN_OBSERVACIONES).v_Value1,
                               CaderaExcelente = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.CADERA_EXCELENTE) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.CADERA_EXCELENTE).v_Value1,
                               CaderaPromedio = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.CADERA_PROMEDIO) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.CADERA_PROMEDIO).v_Value1,
                               CaderaRegular = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.CADERA_REGULAR) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.CADERA_REGULAR).v_Value1,
                               CaderaPobre = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.CADERA_POBRE) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.CADERA_POBRE).v_Value1,
                               CaderaPuntos = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.CADERA_PUNTOS) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.CADERA_PUNTOS).v_Value1,
                               CaderaObs = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.CADRA_OBSERVACIONES) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.CADRA_OBSERVACIONES).v_Value1,
                               MusloExcelente = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.MUSLO_EXCELENTE) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.MUSLO_EXCELENTE).v_Value1,
                               MusloPromedio = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants._MUSLO_PROMEDIO) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants._MUSLO_PROMEDIO).v_Value1,
                               MusloRegular = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.MUSLO_REGULAR) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.MUSLO_REGULAR).v_Value1,
                               MusloPobre = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.MUSLO_POBRE) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.MUSLO_POBRE).v_Value1,
                               MusloPuntos = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.MUSLO_PUNTOS) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.MUSLO_PUNTOS).v_Value1,
                               MusloObs = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.MUSLO_OBSERVACIONES) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.MUSLO_OBSERVACIONES).v_Value1,
                               AbdomenLateralExcelente = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.ABDOMEN_LAT_EXCELENTE) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.ABDOMEN_LAT_EXCELENTE).v_Value1,
                               AbdomenLateralPromedio = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.ABDOMEN_LAT_PROMEDIO) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.ABDOMEN_LAT_PROMEDIO).v_Value1,
                               AbdomenLateralRegular = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.ABDOMEN_LAT_REGULAR) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.ABDOMEN_LAT_REGULAR).v_Value1,
                               AbdomenLateralPobre = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.ABDOMEN_LAT_POBRE) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.ABDOMEN_LAT_POBRE).v_Value1,
                               AbdomenLateralPuntos = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.ABDOMEN_LAT_PUNTOS) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.ABDOMEN_LAT_PUNTOS).v_Value1,
                               AbdomenLateralObs = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.ABDOMEN_LAT_OBSERVACIONES) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.ABDOMEN_LAT_OBSERVACIONES).v_Value1,
                               AbduccionHombroNormalOptimo = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.COLUMNA_LUMBAR) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.COLUMNA_LUMBAR).v_Value1,
                               AbduccionHombroNormalLimitado = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_ADUCCION_HOMBRO_NORMAL_LIMITADO_ID) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_ADUCCION_HOMBRO_NORMAL_LIMITADO_ID).v_Value1,
                               AbduccionHombroNormalMuyLimitado = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.COLUMNA_DORSAL_CONTRACTURA) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.COLUMNA_DORSAL_CONTRACTURA).v_Value1,
                               AbduccionHombroNormalPuntos = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.COLUMNA_LUMBAR_CONTACTURA) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.COLUMNA_LUMBAR_CONTACTURA).v_Value1,
                               AbduccionHombroNormalObs = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.COLUMNA_DORSAL) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.COLUMNA_DORSAL).v_Value1Name,
                               AbduccionHombroOptimo = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_ADUCCION_HOMBRO_OPTIMO_ID) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_ADUCCION_HOMBRO_OPTIMO_ID).v_Value1,
                               AbduccionHombroLimitado = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.DORSAL_LUMBAR_IRRADIACION) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.DORSAL_LUMBAR_IRRADIACION).v_Value1,
                               AbduccionHombroMuyLimitado = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_ADUCCION_HOMBRO_MUY_LIMITADO_ID) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_ADUCCION_HOMBRO_MUY_LIMITADO_ID).v_Value1,
                               AbduccionHombroPuntos = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.COLUMNA_CERVICAL) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.COLUMNA_CERVICAL).v_Value1,
                               AbduccionHombroObs = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.DORSAL_LUMBAR_ROACION_IZQUIERDA) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.DORSAL_LUMBAR_ROACION_IZQUIERDA).v_Value1Name,
                               RotacionExternaOptimo = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.HOMBRO_DCHABDUCC) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.HOMBRO_DCHABDUCC).v_Value1,
                               RotacionExternaLimitado = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.HOMBRO_DCH) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.HOMBRO_DCH).v_Value1,
                               RotacionExternaMuyLimitado = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_ROTACION_EXTERNA_MUY_LIMITADO_ID) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_ROTACION_EXTERNA_MUY_LIMITADO_ID).v_Value1,
                               RotacionExternaPuntos = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.HOMBRO_DCHFLEXION) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.HOMBRO_DCHFLEXION).v_Value1,
                               RotacionExternaObs = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_ROTACION_EXTERNA_DOLOR_ID) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_ROTACION_EXTERNA_DOLOR_ID).v_Value1Name,
                               RotacionExternaHombroInternoOptimo = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.HOMBRO_DCHDOLOR) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.HOMBRO_DCHDOLOR).v_Value1,
                               RotacionExternaHombroInternoLimitado = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.HOMBRO_DCHFUERZATONO) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.HOMBRO_DCHFUERZATONO).v_Value1,
                               RotacionExternaHombroInternoMuyLimitado = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.HOMBRO_DCHEXTENSION) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.HOMBRO_DCHEXTENSION).v_Value1,
                               RotacionExternaHombroInternoPuntos = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.HOMBRO_IZQ) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.HOMBRO_IZQ).v_Value1,
                               RotacionExternaHombroInternoObs = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.HOMBRO_DCHROTINT) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.HOMBRO_DCHROTINT).v_Value1Name,
                               Total1 = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.TOTAL) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.TOTAL).v_Value1,
                               Total2 = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.HOMBRO_IZQABDUCC) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.HOMBRO_IZQABDUCC).v_Value1,
                               AptitudMusculoEsqueletico = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.APTITUD) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.APTITUD).v_Value1,
                               Conclusiones = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.DESCRIPCION) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.DESCRIPCION).v_Value1,

                               ReflejoTotulianoDerechoSiNo = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.PIE_PLANO_IZQUIERDO) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.PIE_PLANO_IZQUIERDO).v_Value1Name,
                               //ReflejoTotulianoIzquierdoSiNo = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_REFLEJO_TOTULIANO_IZQUIERDO) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_REFLEJO_TOTULIANO_IZQUIERDO).v_Value1Name,
                               ReflejoAquileoDerechoSiNo = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.PIE_CAVO_IZQUIERDO) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.PIE_CAVO_IZQUIERDO).v_Value1Name,
                               ReflejoAquileoIzquierdoSiNo = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.PIE_PLANO_DERECHO) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.PIE_PLANO_DERECHO).v_Value1Name,
                               TestPhalenDerechoSiNo = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.PHALEN_DERECHO) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.PHALEN_DERECHO).v_Value1,
                               TestPhalenIzquierdoSiNo = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.PHALEN_IZQUIERDO) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.PHALEN_IZQUIERDO).v_Value1,
                               TestTinelDerechoSiNo = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.TINEL_DERECHO) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.TINEL_DERECHO).v_Value1,
                               TestTinelIzquierdoSiNo = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.TINEL_IZQUIERDO) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.TINEL_IZQUIERDO).v_Value1,
                               SignoLasagueIzquierdoSiNo = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.LASEGUE_IZQUIERDO) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.LASEGUE_IZQUIERDO).v_Value1,
                               SignoLasagueDerechoSiNo = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.LASEGUE_DERECHO) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.LASEGUE_DERECHO).v_Value1,
                               SignoBragardIzquierdoSiNo = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.ADAM_IZQUIERDO) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.ADAM_IZQUIERDO).v_Value1,
                               SignoBragardDerechoSiNo = OsteoMuscular.Count == 0 || OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.ADAM_DERECHO) == null ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.ADAM_DERECHO).v_Value1,

                               b_Logo = MedicalCenter.b_Image,
                               EmpresaPropietaria = MedicalCenter.v_Name,
                               EmpresaPropietariaDireccion = MedicalCenter.v_Address,
                               EmpresaPropietariaTelefono = MedicalCenter.v_PhoneNumber,
                               EmpresaPropietariaEmail = MedicalCenter.v_Mail,
                               NombreUsuarioGraba = a.NombreUsuarioGraba
                           }).ToList();

                return sql;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<Sigesoft.Node.WinClient.BE.ReportCuestionarioNordico> GetReportCuestionarioNordico(string pstrserviceId, string pstrComponentId)
        {
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                var objEntity = (from A in dbContext.service
                                 join B in dbContext.person on A.v_PersonId equals B.v_PersonId
                                 join E in dbContext.servicecomponent on new { a = A.v_ServiceId, b = pstrComponentId }
                                                                   equals new { a = E.v_ServiceId, b = E.v_ComponentId }
                                 join F in dbContext.systemuser on E.i_ApprovedUpdateUserId equals F.i_SystemUserId
                                 join Z in dbContext.person on F.v_PersonId equals Z.v_PersonId
                                 where A.v_ServiceId == pstrserviceId
                                 select new Sigesoft.Node.WinClient.BE.ReportCuestionarioNordico
                                 {
                                     Nombre_Trabajador = B.v_FirstName + " " + B.v_FirstLastName + " " + B.v_SecondLastName,
                                     FechaNacimiento = B.d_Birthdate.Value,
                                     Genero = B.i_SexTypeId.Value,
                                     FirmaTrabajador = B.b_RubricImage,
                                     HuellaTrabajador = B.b_FingerPrintImage,
                                     NombreUsuarioGraba = Z.v_FirstLastName + " " + Z.v_SecondLastName + " " + Z.v_FirstName,

                                 });

                var MedicalCenter = GetInfoMedicalCenter();

                var sql = (from a in objEntity.ToList()
                           let CuestNordi = ValoresComponente(pstrserviceId, Constants.C_N_ID)

                           select new Sigesoft.Node.WinClient.BE.ReportCuestionarioNordico
                           {
                               FirmaTrabajador = a.FirmaTrabajador,
                               HuellaTrabajador = a.HuellaTrabajador,
                               Nombre_Trabajador = a.Nombre_Trabajador,
                               FechaNacimiento = a.FechaNacimiento,
                               Genero = a.Genero,
                               Edad = GetAge(a.FechaNacimiento),

                               C_N_CABECERA_TIPO_TRABAJO_REALIZA_ID = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_CABECERA_TIPO_TRABAJO_REALIZA_ID) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_CABECERA_TIPO_TRABAJO_REALIZA_ID).v_Value1,
                               C_N_CABECERA_TIEMPO_LABOR_ID = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_CABECERA_HORAS_TRABAJO_SEMANAL_ID) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_CABECERA_TIEMPO_LABOR_ID).v_Value1,
                               C_N_CABECERA_HORAS_TRABAJO_SEMANAL_ID = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.MANIPULACION_EMPUJAR_CARGA_DESCRIPCION) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_CABECERA_HORAS_TRABAJO_SEMANAL_ID).v_Value1,
                               C_N_CABECERA_DIESTRO_ID = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_CABECERA_DIESTRO_ID) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_CABECERA_DIESTRO_ID).v_Value1,
                               C_N_CABECERA_ZURDO_ID = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_CABECERA_ZURDO_ID) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_CABECERA_ZURDO_ID).v_Value1,

                               C_N_LOCOMOCION_TODOS_CUELLOS_ID = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_LOCOMOCION_TODOS_CUELLOS_ID) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_LOCOMOCION_TODOS_CUELLOS_ID).v_Value1,
                               C_N_LOCOMOCION_TODOS_HOMBROS_ID = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_LOCOMOCION_TODOS_HOMBROS_ID) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_LOCOMOCION_TODOS_HOMBROS_ID).v_Value1,
                               C_N_LOCOMOCION_TODOS_CODOS_ID = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_LOCOMOCION_TODOS_CODOS_ID) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_LOCOMOCION_TODOS_CODOS_ID).v_Value1,
                               C_N_LOCOMOCION_TODOS_MUÑECA_ID = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_LOCOMOCION_TODOS_MUÑECA_ID) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_LOCOMOCION_TODOS_MUÑECA_ID).v_Value1,
                               C_N_LOCOMOCION_TODOS_ESPALDA_ALTA_ID = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_LOCOMOCION_TODOS_ESPALDA_ALTA_ID) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_LOCOMOCION_TODOS_ESPALDA_ALTA_ID).v_Value1,
                               C_N_LOCOMOCION_TODOS_ESPALDA_BAJA_ID = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_LOCOMOCION_TODOS_ESPALDA_BAJA_ID) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_LOCOMOCION_TODOS_ESPALDA_BAJA_ID).v_Value1,
                               C_N_LOCOMOCION_TODOS_CADERAS_MUSLOS_ID = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_LOCOMOCION_TODOS_CADERAS_MUSLOS_ID) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_LOCOMOCION_TODOS_CADERAS_MUSLOS_ID).v_Value1,
                               C_N_LOCOMOCION_TODOS_RODILLAS_ID = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_LOCOMOCION_TODOS_RODILLAS_ID) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_LOCOMOCION_TODOS_RODILLAS_ID).v_Value1,
                               C_N_LOCOMOCION_TODOS_TOBILLOS_PIES_ID = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_LOCOMOCION_TODOS_TOBILLOS_PIES_ID) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_LOCOMOCION_TODOS_TOBILLOS_PIES_ID).v_Value1,

                               C_N_LOCOMOCION_12_MESES_CUELLO_ID = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_LOCOMOCION_12_MESES_CUELLO_ID) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_LOCOMOCION_12_MESES_CUELLO_ID).v_Value1,
                               C_N_LOCOMOCION_12_MESES_HOMBROS_ID = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_LOCOMOCION_12_MESES_HOMBROS_ID) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_LOCOMOCION_12_MESES_HOMBROS_ID).v_Value1,
                               C_N_LOCOMOCION_12_MESES_CODOS_ID = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_LOCOMOCION_12_MESES_CODOS_ID) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_LOCOMOCION_12_MESES_CODOS_ID).v_Value1,
                               C_N_LOCOMOCION_12_MESES_MUÑECA_ID = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_LOCOMOCION_12_MESES_MUÑECA_ID) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_LOCOMOCION_12_MESES_MUÑECA_ID).v_Value1,
                               C_N_LOCOMOCION_12_MESES_ESPALDA_ALTA_ID = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_LOCOMOCION_12_MESES_ESPALDA_ALTA_ID) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_LOCOMOCION_12_MESES_ESPALDA_ALTA_ID).v_Value1,
                               C_N_LOCOMOCION_12_MESES_ESPALDA_BAJA_ID = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_LOCOMOCION_12_MESES_ESPALDA_BAJA_ID) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_LOCOMOCION_12_MESES_ESPALDA_BAJA_ID).v_Value1,
                               C_N_LOCOMOCION_12_MESES_CADERAS_MUSLOS_ID = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_LOCOMOCION_12_MESES_CADERAS_MUSLOS_ID) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_LOCOMOCION_12_MESES_CADERAS_MUSLOS_ID).v_Value1,
                               C_N_LOCOMOCION_12_MESES_RODILLAS_ID = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_LOCOMOCION_12_MESES_RODILLAS_ID) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_LOCOMOCION_12_MESES_RODILLAS_ID).v_Value1,
                               C_N_LOCOMOCION_12_MESES_TOBILLOS_ID = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_LOCOMOCION_12_MESES_TOBILLOS_ID) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_LOCOMOCION_12_MESES_TOBILLOS_ID).v_Value1,

                               C_N_LOCOMOCION_7_DIAS_CUELLO_ID = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_LOCOMOCION_7_DIAS_CUELLO_ID) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_LOCOMOCION_7_DIAS_CUELLO_ID).v_Value1,
                               C_N_LOCOMOCION_7_DIAS_HOMBROS_ID = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_LOCOMOCION_7_DIAS_HOMBROS_ID) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_LOCOMOCION_7_DIAS_HOMBROS_ID).v_Value1,
                               C_N_LOCOMOCION_7_DIAS_CODOS_ID = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_LOCOMOCION_7_DIAS_CODOS_ID) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_LOCOMOCION_7_DIAS_CODOS_ID).v_Value1,
                               C_N_LOCOMOCION_7_DIAS_MUÑECA_ID = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_LOCOMOCION_7_DIAS_MUÑECA_ID) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_LOCOMOCION_7_DIAS_MUÑECA_ID).v_Value1,
                               C_N_LOCOMOCION_7_DIAS_ESPALDA_ALTA_ID = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_LOCOMOCION_7_DIAS_ESPALDA_ALTA_ID) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_LOCOMOCION_7_DIAS_ESPALDA_ALTA_ID).v_Value1,
                               C_N_LOCOMOCION_7_DIAS_ESPALDA_BAJA_ID = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_LOCOMOCION_7_DIAS_ESPALDA_BAJA_ID) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_LOCOMOCION_7_DIAS_ESPALDA_BAJA_ID).v_Value1,
                               C_N_LOCOMOCION_7_DIAS_CADERA_ID = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_LOCOMOCION_7_DIAS_CADERA_ID) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_LOCOMOCION_7_DIAS_CADERA_ID).v_Value1,
                               C_N_LOCOMOCION_7_DIAS_RODILLAS_ID = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_LOCOMOCION_7_DIAS_RODILLAS_ID) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_LOCOMOCION_7_DIAS_RODILLAS_ID).v_Value1,
                               C_N_LOCOMOCION_7_DIAS_TOBILLOS_ID = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_LOCOMOCION_7_DIAS_TOBILLOS_ID) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_LOCOMOCION_7_DIAS_TOBILLOS_ID).v_Value1,


                               C_N_ESPALDA_BAJA_PROBLEMAS_ESPALDA_BAJA_ID = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_ESPALDA_BAJA_PROBLEMAS_ESPALDA_BAJA_ID) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_ESPALDA_BAJA_PROBLEMAS_ESPALDA_BAJA_ID).v_Value1,
                               C_N_ESPALDA_BAJA_HOSPITALIZADO_PROBLEMA_ESPALDA_BAJA_ID = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_ESPALDA_BAJA_HOSPITALIZADO_PROBLEMA_ESPALDA_BAJA_ID) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_ESPALDA_BAJA_HOSPITALIZADO_PROBLEMA_ESPALDA_BAJA_ID).v_Value1,
                               C_N_ESPALDA_BAJA_CAMBIOS_TRABAJO_ACTIVIDAD_PROBLEMA_ESPALDA_BAJA_ID = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_ESPALDA_BAJA_CAMBIOS_TRABAJO_ACTIVIDAD_PROBLEMA_ESPALDA_BAJA_ID) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_ESPALDA_BAJA_CAMBIOS_TRABAJO_ACTIVIDAD_PROBLEMA_ESPALDA_BAJA_ID).v_Value1,
                               C_N_ESPALDA_BAJA_CURACION_TOTAL_ESPALDA_BAJA_ID = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_ESPALDA_BAJA_CURACION_TOTAL_ESPALDA_BAJA_ID) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_ESPALDA_BAJA_CURACION_TOTAL_ESPALDA_BAJA_ID).v_Value1,
                               C_N_ESPALDA_BAJA_ACTIVIDAD_TRABAJO_1_ID = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_ESPALDA_BAJA_ACTIVIDAD_TRABAJO_1_ID) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_ESPALDA_BAJA_ACTIVIDAD_TRABAJO_1_ID).v_Value1,
                               C_N_ESPALDA_BAJA_ACTIVIDAD_RECREATIVA_1_ID = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_ESPALDA_BAJA_ACTIVIDAD_RECREATIVA_1_ID) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_ESPALDA_BAJA_ACTIVIDAD_RECREATIVA_1_ID).v_Value1,

                               C_N_DURACION_PRBLEMAS_IMPEDIR_RUTINA_1_ID = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_DURACION_PRBLEMAS_IMPEDIR_RUTINA_1_ID) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_DURACION_PRBLEMAS_IMPEDIR_RUTINA_1_ID).v_Value1,
                               C_N_VISTO_PROFESIONAL_ESPALDA_BAJA_ID = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_VISTO_PROFESIONAL_ESPALDA_BAJA_ID) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_VISTO_PROFESIONAL_ESPALDA_BAJA_ID).v_Value1,
                               C_N_PROBLEMAS_ESPALDA_BAJA_7_DIAS_ID = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_PROBLEMAS_ESPALDA_BAJA_7_DIAS_ID) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_PROBLEMAS_ESPALDA_BAJA_7_DIAS_ID).v_Value1,


                               C_N_PROBLEMAS_HOMBROS_PROBLEMAS_HOMBROS_ID = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_PROBLEMAS_HOMBROS_PROBLEMAS_HOMBROS_ID) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_PROBLEMAS_HOMBROS_PROBLEMAS_HOMBROS_ID).v_Value1,
                               C_N_PROBLEMAS_HOMBROS_LESION_HOMBROS_ACCIDENTES_ID = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_PROBLEMAS_HOMBROS_LESION_HOMBROS_ACCIDENTES_ID) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_PROBLEMAS_HOMBROS_LESION_HOMBROS_ACCIDENTES_ID).v_Value1,
                               C_N_PROBLEMAS_HOMBROS_CAMBIO_TRABAJO_ACTIVIDAD_HOMBROS_ID = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_PROBLEMAS_HOMBROS_CAMBIO_TRABAJO_ACTIVIDAD_HOMBROS_ID) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_PROBLEMAS_HOMBROS_CAMBIO_TRABAJO_ACTIVIDAD_HOMBROS_ID).v_Value1,
                               C_N_PROBLEMAS_HOMBROS_PROBLEMAS_HOMBROS_ULTIMOS_12_MESES_ID = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_PROBLEMAS_HOMBROS_PROBLEMAS_HOMBROS_ULTIMOS_12_MESES_ID) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_PROBLEMAS_HOMBROS_PROBLEMAS_HOMBROS_ULTIMOS_12_MESES_ID).v_Value1,
                               C_N_PROBLEMAS_HOMBROS_TIEMPO_TOTAL_PROBLEMAS_HOMBROS_ID = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_PROBLEMAS_HOMBROS_TIEMPO_TOTAL_PROBLEMAS_HOMBROS_ID) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_PROBLEMAS_HOMBROS_TIEMPO_TOTAL_PROBLEMAS_HOMBROS_ID).v_Value1,


                               C_N_PROBLEMAS_HOMBROS_ACTIVIDAD_TRABAJO_2_ID = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_PROBLEMAS_HOMBROS_ACTIVIDAD_TRABAJO_2_ID) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_PROBLEMAS_HOMBROS_ACTIVIDAD_TRABAJO_2_ID).v_Value1,
                               C_N_PROBLEMAS_HOMBROS_ACTIVIDAD_RECREATIVA_2_ID = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_PROBLEMAS_HOMBROS_ACTIVIDAD_RECREATIVA_2_ID) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_PROBLEMAS_HOMBROS_ACTIVIDAD_RECREATIVA_2_ID).v_Value1,
                               C_N_PROBLEMAS_HOMBROS_DURACION_PRBLEMAS_IMPEDIR_RUTINA_2_ID = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_PROBLEMAS_HOMBROS_DURACION_PRBLEMAS_IMPEDIR_RUTINA_2_ID) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_PROBLEMAS_HOMBROS_DURACION_PRBLEMAS_IMPEDIR_RUTINA_2_ID).v_Value1,
                               C_N_PROBLEMAS_HOMBROS_VISTO_PROFESIONAL_HOMBROS_ID = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_PROBLEMAS_HOMBROS_VISTO_PROFESIONAL_HOMBROS_ID) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_PROBLEMAS_HOMBROS_VISTO_PROFESIONAL_HOMBROS_ID).v_Value1,
                               C_N_PROBLEMAS_HOMBROS_PROBLEMAS_HOMBROS_DURANTE_7_DIAS_ID = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_PROBLEMAS_HOMBROS_PROBLEMAS_HOMBROS_DURANTE_7_DIAS_ID) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_PROBLEMAS_HOMBROS_PROBLEMAS_HOMBROS_DURANTE_7_DIAS_ID).v_Value1,

                               C_N_PROBLEMA_CUELLO_PROBLEMA_CUELLO_ID = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_PROBLEMA_CUELLO_PROBLEMA_CUELLO_ID) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_PROBLEMA_CUELLO_PROBLEMA_CUELLO_ID).v_Value1,
                               C_N_PROBLEMA_CUELLO_LESIONADO_CUELLO_ACCIDENTE_ID = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_PROBLEMA_CUELLO_LESIONADO_CUELLO_ACCIDENTE_ID) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_PROBLEMA_CUELLO_LESIONADO_CUELLO_ACCIDENTE_ID).v_Value1,
                               C_N_PROBLEMA_CUELLO_CAMBIO_TRABAJO_PROBLEMA_CUELLO_ID = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_PROBLEMA_CUELLO_CAMBIO_TRABAJO_PROBLEMA_CUELLO_ID) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_PROBLEMA_CUELLO_CAMBIO_TRABAJO_PROBLEMA_CUELLO_ID).v_Value1,
                               C_N_PROBLEMA_CUELLO_DURACION_TOTAL_TIEMPO_PROBLEMA_CUELLO_ID = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_PROBLEMA_CUELLO_DURACION_TOTAL_TIEMPO_PROBLEMA_CUELLO_ID) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_PROBLEMA_CUELLO_DURACION_TOTAL_TIEMPO_PROBLEMA_CUELLO_ID).v_Value1,
                               C_N_PROBLEMA_CUELLO_DURACION_PRBLEMAS_IMPEDIR_RUTINA_3_ID = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_PROBLEMA_CUELLO_DURACION_PRBLEMAS_IMPEDIR_RUTINA_3_ID) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_PROBLEMA_CUELLO_DURACION_PRBLEMAS_IMPEDIR_RUTINA_3_ID).v_Value1,
                               C_N_PROBLEMA_CUELLO_VISTO_PROFESIONAL_CUELLO_ID = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_PROBLEMA_CUELLO_VISTO_PROFESIONAL_CUELLO_ID) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_PROBLEMA_CUELLO_VISTO_PROFESIONAL_CUELLO_ID).v_Value1,
                               C_N_PROBLEMA_CUELLO_PROBLEMAS_CUELLO_DURANTE_7_DIAS_ID = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_PROBLEMA_CUELLO_PROBLEMAS_CUELLO_DURANTE_7_DIAS_ID) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_PROBLEMA_CUELLO_PROBLEMAS_CUELLO_DURANTE_7_DIAS_ID).v_Value1,
                               C_N_PROBLEMA_CUELLO_ACTIVIDAD_TRABAJO_3_ID = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_PROBLEMA_CUELLO_ACTIVIDAD_TRABAJO_3_ID) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_PROBLEMA_CUELLO_ACTIVIDAD_TRABAJO_3_ID).v_Value1,
                               C_N_PROBLEMA_CUELLO_ACTIVIDAD_RECREATIVA_3_ID = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_PROBLEMA_CUELLO_ACTIVIDAD_RECREATIVA_3_ID) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.C_N_PROBLEMA_CUELLO_ACTIVIDAD_RECREATIVA_3_ID).v_Value1,
                               b_Logo = MedicalCenter.b_Image,
                               EmpresaPropietaria = MedicalCenter.v_Name,
                               EmpresaPropietariaDireccion = MedicalCenter.v_Address,
                               EmpresaPropietariaTelefono = MedicalCenter.v_PhoneNumber,
                               EmpresaPropietariaEmail = MedicalCenter.v_Mail,
                               NombreUsuarioGraba = a.NombreUsuarioGraba
                           }).ToList();


                return sql;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<Sigesoft.Node.WinClient.BE.OjoSeco> ReporteOjoSeco(string pstrserviceId, string pstrComponentId)
        {
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                var objEntity = (from A in dbContext.service
                                 join B in dbContext.person on A.v_PersonId equals B.v_PersonId
                                 join C in dbContext.servicecomponent on new { a = pstrserviceId, b = pstrComponentId }
                                                                       equals new { a = C.v_ServiceId, b = C.v_ComponentId }

                                 // Usuario Medico Evaluador / Medico Aprobador ****************************
                                 join me in dbContext.systemuser on C.i_ApprovedUpdateUserId equals me.i_SystemUserId into me_join
                                 from me in me_join.DefaultIfEmpty()

                                 join pme in dbContext.professional on me.v_PersonId equals pme.v_PersonId into pme_join
                                 from pme in pme_join.DefaultIfEmpty()

                                 join Z in dbContext.person on me.v_PersonId equals Z.v_PersonId
                                 join E1 in dbContext.protocol on A.v_ProtocolId equals E1.v_ProtocolId

                                 join et in dbContext.systemparameter on new { a = E1.i_EsoTypeId.Value, b = 118 }
                                               equals new { a = et.i_ParameterId, b = et.i_GroupId } into et_join  // TIPO ESO [ESOA,ESOR,ETC]
                                 from et in et_join.DefaultIfEmpty()
                                 join D1 in dbContext.organization on E1.v_CustomerOrganizationId equals D1.v_OrganizationId

                                 where A.v_ServiceId == pstrserviceId
                                 select new Sigesoft.Node.WinClient.BE.OjoSeco
                                 {
                                     EmpresaCliente = D1.v_Name,
                                     Trabajador = B.v_FirstLastName + " " + B.v_FirstLastName + " " + B.v_FirstName,
                                     FechaNacimiento = B.d_Birthdate,
                                     FechaEvaluacion = A.d_ServiceDate,
                                     Puesto = B.v_CurrentOccupation,
                                     ServicioId = A.v_ServiceId,
                                     FirmaUsuarioGraba = pme.b_SignatureImage,
                                     NombreUsuarioGraba = Z.v_FirstLastName + " " + Z.v_FirstLastName + " " + Z.v_FirstName,
                                     HuellaTrabajador = B.b_FingerPrintImage,
                                     FirmaTrabajador = B.b_RubricImage,
                                     TipoEso = et.v_Value1,
                                     Dni = B.v_DocNumber
                                 });

                var ValorUSer = ValoresComponentesUserControl(pstrserviceId, Constants.OFTALMOLOGIA_ID).ToList();
                var MedicalCenter = GetInfoMedicalCenter();
                var sql = (from a in objEntity.ToList()
                           select new Sigesoft.Node.WinClient.BE.OjoSeco
                           {
                               EmpresaCliente = a.EmpresaCliente,
                               TipoEso = a.TipoEso,
                               Dni = a.Dni,
                               Trabajador = a.Trabajador,
                               FechaNacimiento = a.FechaNacimiento,
                               FechaEvaluacion = a.FechaEvaluacion,
                               Puesto = a.Puesto,
                               ServicioId = a.ServicioId,
                               FirmaUsuarioGraba = a.FirmaUsuarioGraba,
                               NombreUsuarioGraba = a.NombreUsuarioGraba,
                               HuellaTrabajador = a.HuellaTrabajador,
                               FirmaTrabajador = a.FirmaTrabajador,
                               Edad = GetAge(a.FechaNacimiento.Value),


                               OJO_SECO_ENROJECIMIENTO = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.OJO_SECO_ENROJECIMIENTO) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.OJO_SECO_ENROJECIMIENTO).v_Value1,
                               OJO_SECO_ENROJECIMIENTO_PTJ_1 = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.OJO_SECO_ENROJECIMIENTO_PTJ_1) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.OJO_SECO_ENROJECIMIENTO_PTJ_1).v_Value1,
                               OJO_SECO_BORDE = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.OJO_SECO_BORDE) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.OJO_SECO_BORDE).v_Value1,
                               OJO_SECO_BORDE_PTJ_2 = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.OJO_SECO_BORDE_PTJ_2) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.OJO_SECO_BORDE_PTJ_2).v_Value1,
                               OJO_SECO_ESCAMAS = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.OJO_SECO_ESCAMAS) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.OJO_SECO_ESCAMAS).v_Value1,
                               OJO_SECO_ESCAMAS_PTJ_3 = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.OJO_SECO_ESCAMAS_PTJ_3) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.OJO_SECO_ESCAMAS_PTJ_3).v_Value1,
                               OJO_SECO_OJOS = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.OJO_SECO_OJOS) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.OJO_SECO_OJOS).v_Value1,
                               OJO_SECO_OJOS_PTJ_4 = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.OJO_SECO_OJOS_PTJ_4) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.OJO_SECO_OJOS_PTJ_4).v_Value1,
                               OJO_SECO_SECRE = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.OJO_SECO_SECRE) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.OJO_SECO_SECRE).v_Value1,
                               OJO_SECO_SECRE_PTJ_5 = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.OJO_SECO_SECRE_PTJ_5) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.OJO_SECO_SECRE_PTJ_5).v_Value1,
                               OJO_SECO_SEQUEDAD = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.OJO_SECO_SEQUEDAD) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.OJO_SECO_SEQUEDAD).v_Value1,

                               OJO_SECO_SEQUEDAD_PTJ_6 = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.OJO_SECO_SEQUEDAD_PTJ_6) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.OJO_SECO_SEQUEDAD_PTJ_6).v_Value1,
                               OJO_SECO_ARENILLA = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.OJO_SECO_ARENILLA) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.OJO_SECO_ARENILLA).v_Value1,
                               OJO_SECO_ARENILLA_PTJ_7 = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.OJO_SECO_ARENILLA_PTJ_7) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.OJO_SECO_ARENILLA_PTJ_7).v_Value1,
                               OJO_SECO_EXTRANO = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.OJO_SECO_EXTRANO) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.OJO_SECO_EXTRANO).v_Value1,
                               OJO_SECO_EXTRANO_PTJ_8 = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.OJO_SECO_EXTRANO_PTJ_8) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.OJO_SECO_EXTRANO_PTJ_8).v_Value1,
                               OJO_SECO_ARDOR = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.OJO_SECO_ARDOR) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.OJO_SECO_ARDOR).v_Value1,
                               OJO_SECO_ARDOR_PTJ_9 = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.OJO_SECO_ARDOR_PTJ_9) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.OJO_SECO_ARDOR_PTJ_9).v_Value1,
                               OJO_SECO_PICOR = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.OJO_SECO_PICOR) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.OJO_SECO_PICOR).v_Value1,
                               OJO_SECO_PICOR_PTJ_10 = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.OJO_SECO_PICOR_PTJ_10) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.OJO_SECO_PICOR_PTJ_10).v_Value1,
                               OJO_SECO_MALESTAR = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.OJO_SECO_MALESTAR) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.OJO_SECO_MALESTAR).v_Value1,
                               OJO_SECO_MALESTAR_PTJ_11 = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.OJO_SECO_MALESTAR_PTJ_11) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.OJO_SECO_MALESTAR_PTJ_11).v_Value1,

                               OJO_SECO_DOLOR = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.OJO_SECO_DOLOR) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.OJO_SECO_DOLOR).v_Value1,
                               OJO_SECO_DOLOR_PTJ_12 = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.OJO_SECO_DOLOR_PTJ_12) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.OJO_SECO_DOLOR_PTJ_12).v_Value1,
                               OJO_SECO_LAGRIMEO = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.OJO_SECO_LAGRIMEO) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.OJO_SECO_LAGRIMEO).v_Value1,
                               OJO_SECO_LAGRIMEO_PTJ_13 = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.OJO_SECO_LAGRIMEO_PTJ_13) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.OJO_SECO_LAGRIMEO_PTJ_13).v_Value1,
                               OJO_SECO_LLOROSOS = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.OJO_SECO_LLOROSOS) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.OJO_SECO_LLOROSOS).v_Value1,
                               OJO_SECO_LLOROSOS_PTJ_14 = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.OJO_SECO_LLOROSOS_PTJ_14) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.OJO_SECO_LLOROSOS_PTJ_14).v_Value1,
                               OJO_SECO_SENSIBILIDAD = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.OJO_SECO_SENSIBILIDAD) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.OJO_SECO_SENSIBILIDAD).v_Value1,
                               OJO_SECO_SENSIBILIDAD_PTJ_15 = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.OJO_SECO_SENSIBILIDAD_PTJ_15) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.OJO_SECO_SENSIBILIDAD_PTJ_15).v_Value1,

                               OJO_SECO_VISION = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.OJO_SECO_VISION) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.OJO_SECO_VISION).v_Value1,
                               OJO_SECO_VISION_PTJ_16 = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.OJO_SECO_VISION_PTJ_16) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.OJO_SECO_VISION_PTJ_16).v_Value1,
                               OJO_SECO_CANSANCION = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.OJO_SECO_CANSANCION) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.OJO_SECO_CANSANCION).v_Value1,
                               OJO_SECO_CANSANCION_PTJ_17 = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.OJO_SECO_CANSANCION_PTJ_17) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.OJO_SECO_CANSANCION_PTJ_17).v_Value1,
                               OJO_SECO_PESADEZ = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.OJO_SECO_PESADEZ) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.OJO_SECO_PESADEZ).v_Value1,
                               OJO_SECO_PESADEZ_PTJ_18 = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.OJO_SECO_PESADEZ_PTJ_18) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.OJO_SECO_PESADEZ_PTJ_18).v_Value1,
                               OJO_SECO_TOTAL = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.OJO_SECO_TOTAL) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.OJO_SECO_TOTAL).v_Value1,


                               b_Logo = MedicalCenter.b_Image,
                               EmpresaPropietaria = MedicalCenter.v_Name,
                               EmpresaPropietariaDireccion = MedicalCenter.v_Address,
                               EmpresaPropietariaTelefono = MedicalCenter.v_PhoneNumber,
                               EmpresaPropietariaEmail = MedicalCenter.v_Mail,

                               Dx = GetDiagnosticByServiceIdAndComponent(pstrserviceId, Constants.TESTOJOSECO_ID),
                           }).ToList();


                return sql;

            }
            catch (Exception)
            {

                throw;
            }


        }

        public List<Sigesoft.Node.WinClient.BE.ReportEvaNeurologica> GetReportEvaNeurologica(string pstrserviceId, string pstrComponentId)
        {
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                var objEntity = (from A in dbContext.service
                                 join B in dbContext.person on A.v_PersonId equals B.v_PersonId

                                 join E in dbContext.servicecomponent on new { a = A.v_ServiceId, b = pstrComponentId }
                                                                      equals new { a = E.v_ServiceId, b = E.v_ComponentId }

                                 join J in dbContext.systemparameter on new { a = B.i_SexTypeId.Value, b = 100 }
                                                                        equals new { a = J.i_ParameterId, b = J.i_GroupId } into J_join // GENERO
                                 from J in J_join.DefaultIfEmpty()

                                 join E1 in dbContext.protocol on A.v_ProtocolId equals E1.v_ProtocolId

                                 join oc in dbContext.organization on new { a = E1.v_CustomerOrganizationId }
                                   equals new { a = oc.v_OrganizationId } into oc_join
                                 from oc in oc_join.DefaultIfEmpty()


                                 // Usuario Medico Evaluador / Medico Aprobador ****************************
                                 join me in dbContext.systemuser on E.i_ApprovedUpdateUserId equals me.i_SystemUserId into me_join
                                 from me in me_join.DefaultIfEmpty()

                                 join pme in dbContext.professional on me.v_PersonId equals pme.v_PersonId into pme_join
                                 from pme in pme_join.DefaultIfEmpty()

                                 join Z in dbContext.person on me.v_PersonId equals Z.v_PersonId


                                 where A.v_ServiceId == pstrserviceId
                                 select new Sigesoft.Node.WinClient.BE.ReportEvaNeurologica
                                 {

                                     Paciente = B.v_FirstName + " " + B.v_FirstLastName + " " + B.v_SecondLastName,
                                     FechaNacimiento = B.d_Birthdate,
                                     Genero = J.v_Value1,
                                     FechaServicio = A.d_ServiceDate.Value,
                                     DNI = B.v_DocNumber,
                                     v_ServiceId = A.v_ServiceId,
                                     FirmaMedico = pme.b_SignatureImage,
                                     Empresa = oc.v_Name,
                                     NombreUsuarioGraba = Z.v_FirstLastName + " " + Z.v_SecondLastName + " " + Z.v_FirstName,

                                 });

                var MedicalCenter = GetInfoMedicalCenter();

                var sql = (from a in objEntity.ToList()
                           let EvaEnu = ValoresComponente(pstrserviceId, Constants.EVA_NEUROLOGICA_ID)

                           select new Sigesoft.Node.WinClient.BE.ReportEvaNeurologica
                           {
                               Paciente = a.Paciente,
                               FechaNacimiento = a.FechaNacimiento,
                               Genero = a.Genero,
                               FechaServicio = a.FechaServicio,
                               DNI = a.DNI,
                               v_ServiceId = a.v_ServiceId,
                               FirmaMedico = a.FirmaMedico,
                               Empresa = a.Empresa,
                               Edad = GetAge(a.FechaNacimiento.Value),
                               EVA_NEUROLOGICA_TEST_ROMBERG_ID = EvaEnu.Count == 0 || EvaEnu.Find(p => p.v_ComponentFieldId == Constants.EVA_NEUROLOGICA_TEST_ROMBERG_ID) == null ? string.Empty : EvaEnu.Find(p => p.v_ComponentFieldId == Constants.EVA_NEUROLOGICA_TEST_ROMBERG_ID).v_Value1,

                               EVA_NEUROLOGICA_PRUEBA_MARCHA_ID = EvaEnu.Count == 0 || EvaEnu.Find(p => p.v_ComponentFieldId == Constants.EVA_NEUROLOGICA_PRUEBA_MARCHA_ID) == null ? string.Empty : EvaEnu.Find(p => p.v_ComponentFieldId == Constants.EVA_NEUROLOGICA_PRUEBA_MARCHA_ID).v_Value1,

                               EVA_NEUROLOGICA_PRUEBA_INDICE_NARIZ_ID = EvaEnu.Count == 0 || EvaEnu.Find(p => p.v_ComponentFieldId == Constants.EVA_NEUROLOGICA_PRUEBA_INDICE_NARIZ_ID) == null ? string.Empty : EvaEnu.Find(p => p.v_ComponentFieldId == Constants.EVA_NEUROLOGICA_PRUEBA_INDICE_NARIZ_ID).v_Value1,

                               EVA_NEUROLOGICA_MIEMBRO_SUP_ID = EvaEnu.Count == 0 || EvaEnu.Find(p => p.v_ComponentFieldId == Constants.EVA_NEUROLOGICA_MIEMBRO_SUP_ID) == null ? string.Empty : EvaEnu.Find(p => p.v_ComponentFieldId == Constants.EVA_NEUROLOGICA_MIEMBRO_SUP_ID).v_Value1,

                               EVA_NEUROLOGICA_MIEMBRO_INF_ID = EvaEnu.Count == 0 || EvaEnu.Find(p => p.v_ComponentFieldId == Constants.EVA_NEUROLOGICA_MIEMBRO_INF_ID) == null ? string.Empty : EvaEnu.Find(p => p.v_ComponentFieldId == Constants.EVA_NEUROLOGICA_MIEMBRO_INF_ID).v_Value1,

                               EVA_NEUROLOGICA_POSITIVO_ID = EvaEnu.Count == 0 || EvaEnu.Find(p => p.v_ComponentFieldId == Constants.EVA_NEUROLOGICA_POSITIVO_ID) == null ? string.Empty : EvaEnu.Find(p => p.v_ComponentFieldId == Constants.EVA_NEUROLOGICA_POSITIVO_ID).v_Value1,

                               EVA_NEUROLOGICA_FLEXION_ID = EvaEnu.Count == 0 || EvaEnu.Find(p => p.v_ComponentFieldId == Constants.EVA_NEUROLOGICA_FLEXION_ID) == null ? string.Empty : EvaEnu.Find(p => p.v_ComponentFieldId == Constants.EVA_NEUROLOGICA_FLEXION_ID).v_Value1,

                               EVA_NEUROLOGICA_CONCLUSION_ID = EvaEnu.Count == 0 || EvaEnu.Find(p => p.v_ComponentFieldId == Constants.EVA_NEUROLOGICA_CONCLUSION_ID) == null ? string.Empty : EvaEnu.Find(p => p.v_ComponentFieldId == Constants.EVA_NEUROLOGICA_CONCLUSION_ID).v_Value1,


                               b_Logo = MedicalCenter.b_Image,
                               EmpresaPropietaria = MedicalCenter.v_Name,
                               EmpresaPropietariaDireccion = MedicalCenter.v_Address,
                               EmpresaPropietariaTelefono = MedicalCenter.v_PhoneNumber,
                               EmpresaPropietariaEmail = MedicalCenter.v_Mail,
                               NombreUsuarioGraba = a.NombreUsuarioGraba

                           }).ToList();

                return sql;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<Sigesoft.Node.WinClient.BE.UcAcumetria> ReporteAcumetria(string pstrserviceId, string pstrComponentId)
        {
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                var objEntity = (from A in dbContext.service
                                 join B in dbContext.person on A.v_PersonId equals B.v_PersonId
                                 join C in dbContext.servicecomponent on new { a = pstrserviceId, b = pstrComponentId }
                                                                       equals new { a = C.v_ServiceId, b = C.v_ComponentId }

                                 // Usuario Medico Evaluador / Medico Aprobador ****************************
                                 join me in dbContext.systemuser on C.i_ApprovedUpdateUserId equals me.i_SystemUserId into me_join
                                 from me in me_join.DefaultIfEmpty()

                                 join pme in dbContext.professional on me.v_PersonId equals pme.v_PersonId into pme_join
                                 from pme in pme_join.DefaultIfEmpty()

                                 join Z in dbContext.person on me.v_PersonId equals Z.v_PersonId

                                 join E1 in dbContext.protocol on A.v_ProtocolId equals E1.v_ProtocolId
                                 join D1 in dbContext.organization on E1.v_CustomerOrganizationId equals D1.v_OrganizationId

                                 join et in dbContext.systemparameter on new { a = E1.i_EsoTypeId.Value, b = 118 }
                                               equals new { a = et.i_ParameterId, b = et.i_GroupId } into et_join  // TIPO ESO [ESOA,ESOR,ETC]
                                 from et in et_join.DefaultIfEmpty()

                                 where A.v_ServiceId == pstrserviceId
                                 select new Sigesoft.Node.WinClient.BE.UcAcumetria
                                 {
                                     EmpresaCliente = D1.v_Name,
                                     Trabajador = B.v_FirstLastName + " " + B.v_FirstLastName + " " + B.v_FirstName,
                                     FechaNacimiento = B.d_Birthdate,
                                     FechaEvaluacion = A.d_ServiceDate,
                                     Puesto = B.v_CurrentOccupation,
                                     ServicioId = A.v_ServiceId,
                                     FirmaUsuarioGraba = pme.b_SignatureImage,
                                     NombreUsuarioGraba = Z.v_FirstLastName + " " + Z.v_FirstLastName + " " + Z.v_FirstName,
                                     HuellaTrabajador = B.b_FingerPrintImage,
                                     FirmaTrabajador = B.b_RubricImage,
                                     TipoEso = et.v_Value1,
                                     Dni = B.v_DocNumber
                                 });

                var ValorUSer = ValoresComponentesUserControl(pstrserviceId, Constants.AUDIOMETRIA_ID).ToList();
                var MedicalCenter = GetInfoMedicalCenter();
                var sql = (from a in objEntity.ToList()
                           select new Sigesoft.Node.WinClient.BE.UcAcumetria
                           {
                               EmpresaCliente = a.EmpresaCliente,
                               TipoEso = a.TipoEso,
                               Dni = a.Dni,
                               Trabajador = a.Trabajador,
                               FechaNacimiento = a.FechaNacimiento,
                               FechaEvaluacion = a.FechaEvaluacion,
                               Puesto = a.Puesto,
                               ServicioId = a.ServicioId,
                               FirmaUsuarioGraba = a.FirmaUsuarioGraba,
                               NombreUsuarioGraba = a.NombreUsuarioGraba,
                               HuellaTrabajador = a.HuellaTrabajador,
                               FirmaTrabajador = a.FirmaTrabajador,
                               Edad = GetAge(a.FechaNacimiento.Value),


                               ACUMETRIA_PRUEBA_WEBER = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.ACUMETRIA_PRUEBA_WEBER) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.ACUMETRIA_PRUEBA_WEBER).v_Value1,
                               ACUMETRIA_OD_RINNER = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.ACUMETRIA_OD_RINNER) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.ACUMETRIA_OD_RINNER).v_Value1,
                               ACUMETRIA_OI_RINNER = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.ACUMETRIA_OI_RINNER) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.ACUMETRIA_OI_RINNER).v_Value1,
                               ACUMETRIA_WEBER = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.ACUMETRIA_WEBER) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.ACUMETRIA_WEBER).v_Value1,
                               ACUMETRIA_OD_RINNE = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.ACUMETRIA_OD_RINNE) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.ACUMETRIA_OD_RINNE).v_Value1,
                               ACUMETRIA_OI_RINNE = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.ACUMETRIA_OI_RINNE) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.ACUMETRIA_OI_RINNE).v_Value1,
                               ACUMETRIA_CONCLUSIONES = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.ACUMETRIA_CONCLUSIONES) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.ACUMETRIA_CONCLUSIONES).v_Value1,
                               b_Logo = MedicalCenter.b_Image,
                               EmpresaPropietaria = MedicalCenter.v_Name,
                               EmpresaPropietariaDireccion = MedicalCenter.v_Address,
                               EmpresaPropietariaTelefono = MedicalCenter.v_PhoneNumber,
                               EmpresaPropietariaEmail = MedicalCenter.v_Mail,

                           }).ToList();


                return sql;

            }
            catch (Exception)
            {

                throw;
            }


        }

        public List<Sigesoft.Node.WinClient.BE.UcOtoscopiacs> ReporteOtoscopia(string pstrserviceId, string pstrComponentId)
        {
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                var objEntity = (from A in dbContext.service
                                 join B in dbContext.person on A.v_PersonId equals B.v_PersonId
                                 join C in dbContext.servicecomponent on new { a = pstrserviceId, b = pstrComponentId }
                                                                       equals new { a = C.v_ServiceId, b = C.v_ComponentId }

                                 // Usuario Medico Evaluador / Medico Aprobador ****************************
                                 join me in dbContext.systemuser on C.i_ApprovedUpdateUserId equals me.i_SystemUserId into me_join
                                 from me in me_join.DefaultIfEmpty()

                                 join pme in dbContext.professional on me.v_PersonId equals pme.v_PersonId into pme_join
                                 from pme in pme_join.DefaultIfEmpty()

                                 join Z in dbContext.person on me.v_PersonId equals Z.v_PersonId

                                 join E1 in dbContext.protocol on A.v_ProtocolId equals E1.v_ProtocolId
                                 join D1 in dbContext.organization on E1.v_CustomerOrganizationId equals D1.v_OrganizationId

                                 join et in dbContext.systemparameter on new { a = E1.i_EsoTypeId.Value, b = 118 }
                                               equals new { a = et.i_ParameterId, b = et.i_GroupId } into et_join  // TIPO ESO [ESOA,ESOR,ETC]
                                 from et in et_join.DefaultIfEmpty()

                                 where A.v_ServiceId == pstrserviceId
                                 select new Sigesoft.Node.WinClient.BE.UcOtoscopiacs
                                 {
                                     EmpresaCliente = D1.v_Name,
                                     Trabajador = B.v_FirstLastName + " " + B.v_FirstLastName + " " + B.v_FirstName,
                                     FechaNacimiento = B.d_Birthdate,
                                     FechaEvaluacion = A.d_ServiceDate,
                                     Puesto = B.v_CurrentOccupation,
                                     ServicioId = A.v_ServiceId,
                                     FirmaUsuarioGraba = pme.b_SignatureImage,
                                     NombreUsuarioGraba = Z.v_FirstLastName + " " + Z.v_FirstLastName + " " + Z.v_FirstName,
                                     HuellaTrabajador = B.b_FingerPrintImage,
                                     FirmaTrabajador = B.b_RubricImage,
                                     TipoEso = et.v_Value1,
                                     Dni = B.v_DocNumber
                                 });

                var ValorUSer = ValoresComponentesUserControl(pstrserviceId, Constants.AUDIOMETRIA_ID).ToList();
                var MedicalCenter = GetInfoMedicalCenter();
                var sql = (from a in objEntity.ToList()
                           select new Sigesoft.Node.WinClient.BE.UcOtoscopiacs
                           {
                               EmpresaCliente = a.EmpresaCliente,
                               TipoEso = a.TipoEso,
                               Dni = a.Dni,
                               Trabajador = a.Trabajador,
                               FechaNacimiento = a.FechaNacimiento,
                               FechaEvaluacion = a.FechaEvaluacion,
                               Puesto = a.Puesto,
                               ServicioId = a.ServicioId,
                               FirmaUsuarioGraba = a.FirmaUsuarioGraba,
                               NombreUsuarioGraba = a.NombreUsuarioGraba,
                               HuellaTrabajador = a.HuellaTrabajador,
                               FirmaTrabajador = a.FirmaTrabajador,
                               Edad = GetAge(a.FechaNacimiento.Value),


                               OTOSCOPIA_RUIDO = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.OTOSCOPIA_RUIDO) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.OTOSCOPIA_RUIDO).v_Value1,
                               OTOSCOPIA_QUIMICO = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.OTOSCOPIA_QUIMICO) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.OTOSCOPIA_QUIMICO).v_Value1,
                               OTOSCOPIA_DEPORTE = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.OTOSCOPIA_DEPORTE) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.OTOSCOPIA_DEPORTE).v_Value1,
                               OTOSCOPIA_RUIDO_EXCE = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.OTOSCOPIA_RUIDO_EXCE) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.OTOSCOPIA_RUIDO_EXCE).v_Value1,
                               OTOSCOPIA_MUSICA = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.OTOSCOPIA_MUSICA) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.OTOSCOPIA_MUSICA).v_Value1,
                               OTOSCOPIA_OTOXICOS = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.OTOSCOPIA_OTOXICOS) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.OTOSCOPIA_OTOXICOS).v_Value1,
                               OTOSCOPIA_MANIPULACION = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.OTOSCOPIA_MANIPULACION) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.OTOSCOPIA_MANIPULACION).v_Value1,

                               OTOSCOPIA_OTOLOGICOS = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.OTOSCOPIA_OTOLOGICOS) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.OTOSCOPIA_OTOLOGICOS).v_Value1,
                               OTOSCOPIA_ZUMBIDOS = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.OTOSCOPIA_ZUMBIDOS) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.OTOSCOPIA_ZUMBIDOS).v_Value1,
                               OTOSCOPIA_SECRECION = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.OTOSCOPIA_SECRECION) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.OTOSCOPIA_SECRECION).v_Value1,
                               OTOSCOPIA_MAREOS = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.OTOSCOPIA_MAREOS) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.OTOSCOPIA_MAREOS).v_Value1,
                               OTOSCOPIA_OTALGIA = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.OTOSCOPIA_OTALGIA) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.OTOSCOPIA_OTALGIA).v_Value1,
                               OTOSCOPIA_DISMINUCION = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.OTOSCOPIA_DISMINUCION) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.OTOSCOPIA_DISMINUCION).v_Value1,
                               OTOSCOPIA_TRACTO = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.OTOSCOPIA_TRACTO) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.OTOSCOPIA_TRACTO).v_Value1,
                               OTOSCOPIA_OTROS = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.OTOSCOPIA_OTROS) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.OTOSCOPIA_OTROS).v_Value1,
                               OTOSCOPIA_OD_1 = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.OTOSCOPIA_OD_1) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.OTOSCOPIA_OD_1).v_Value1,

                               OTOSCOPIA_OD_2 = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.OTOSCOPIA_OD_2) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.OTOSCOPIA_OD_2).v_Value1,
                               OTOSCOPIA_OD_3 = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.OTOSCOPIA_OD_3) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.OTOSCOPIA_OD_3).v_Value1,
                               OTOSCOPIA_OD_4 = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.OTOSCOPIA_OD_4) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.OTOSCOPIA_OD_4).v_Value1,
                               OTOSCOPIA_OI_1 = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.OTOSCOPIA_OI_1) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.OTOSCOPIA_OI_1).v_Value1,
                               OTOSCOPIA_OI_2 = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.OTOSCOPIA_OI_2) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.OTOSCOPIA_OI_2).v_Value1,
                               OTOSCOPIA_OI_3 = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.OTOSCOPIA_OI_3) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.OTOSCOPIA_OI_3).v_Value1,
                               OTOSCOPIA_OI_4 = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.OTOSCOPIA_OI_4) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.OTOSCOPIA_OI_4).v_Value1,
                               OTOSCOPIA_OD_DESC = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.OTOSCOPIA_OD_DESC) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.OTOSCOPIA_OD_DESC).v_Value1,
                               OTOSCOPIA_OI_DESC = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.OTOSCOPIA_OI_DESC) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.OTOSCOPIA_OI_DESC).v_Value1,




                               b_Logo = MedicalCenter.b_Image,
                               EmpresaPropietaria = MedicalCenter.v_Name,
                               EmpresaPropietariaDireccion = MedicalCenter.v_Address,
                               EmpresaPropietariaTelefono = MedicalCenter.v_PhoneNumber,
                               EmpresaPropietariaEmail = MedicalCenter.v_Mail,

                           }).ToList();


                return sql;

            }
            catch (Exception)
            {

                throw;
            }


        }

        public List<Sigesoft.Node.WinClient.BE.UcErgonomica> ReporteErgnomia(string pstrserviceId, string pstrComponentId)
        {
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                var objEntity = (from A in dbContext.service
                                 join B in dbContext.person on A.v_PersonId equals B.v_PersonId
                                 join C in dbContext.servicecomponent on new { a = pstrserviceId, b = pstrComponentId }
                                                                       equals new { a = C.v_ServiceId, b = C.v_ComponentId }

                                 // Usuario Medico Evaluador / Medico Aprobador ****************************
                                 join me in dbContext.systemuser on C.i_ApprovedUpdateUserId equals me.i_SystemUserId into me_join
                                 from me in me_join.DefaultIfEmpty()

                                 join pme in dbContext.professional on me.v_PersonId equals pme.v_PersonId into pme_join
                                 from pme in pme_join.DefaultIfEmpty()

                                 join Z in dbContext.person on me.v_PersonId equals Z.v_PersonId
                                 join E1 in dbContext.protocol on A.v_ProtocolId equals E1.v_ProtocolId
                                 join D1 in dbContext.organization on E1.v_CustomerOrganizationId equals D1.v_OrganizationId

                                 join et in dbContext.systemparameter on new { a = E1.i_EsoTypeId.Value, b = 118 }
                                               equals new { a = et.i_ParameterId, b = et.i_GroupId } into et_join  // TIPO ESO [ESOA,ESOR,ETC]
                                 from et in et_join.DefaultIfEmpty()

                                 where A.v_ServiceId == pstrserviceId
                                 select new Sigesoft.Node.WinClient.BE.UcErgonomica
                                 {
                                     EmpresaCliente = D1.v_Name,
                                     Trabajador = B.v_FirstLastName + " " + B.v_SecondLastName + " " + B.v_FirstName,
                                     FechaNacimiento = B.d_Birthdate,
                                     FechaEvaluacion = A.d_ServiceDate,
                                     Puesto = B.v_CurrentOccupation,
                                     ServicioId = A.v_ServiceId,
                                     FirmaUsuarioGraba = pme.b_SignatureImage,
                                     NombreUsuarioGraba = Z.v_FirstLastName + " " + Z.v_FirstLastName + " " + Z.v_FirstName,
                                     HuellaTrabajador = B.b_FingerPrintImage,
                                     FirmaTrabajador = B.b_RubricImage,
                                     Dni = B.v_DocNumber,
                                     TipoEso = et.v_Value1,

                                 });

                var ValorUSer = ValoresComponentesUserControl(pstrserviceId, Constants.EXAMEN_FISICO_ID).ToList();
                var MedicalCenter = GetInfoMedicalCenter();
                var sql = (from a in objEntity.ToList()
                           select new Sigesoft.Node.WinClient.BE.UcErgonomica
                           {
                               EmpresaCliente = a.EmpresaCliente,
                               TipoEso = a.TipoEso,
                               Trabajador = a.Trabajador,
                               FechaNacimiento = a.FechaNacimiento,
                               FechaEvaluacion = a.FechaEvaluacion,
                               Puesto = a.Puesto,
                               ServicioId = a.ServicioId,
                               FirmaUsuarioGraba = a.FirmaUsuarioGraba,
                               NombreUsuarioGraba = a.NombreUsuarioGraba,
                               HuellaTrabajador = a.HuellaTrabajador,
                               FirmaTrabajador = a.FirmaTrabajador,
                               Edad = GetAge(a.FechaNacimiento.Value),
                               Dni = a.Dni,

                               EVA_ERGONOMICA_HOMBORS = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.EVA_ERGONOMICA_HOMBORS) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.EVA_ERGONOMICA_HOMBORS).v_Value1,
                               EVA_ERGONOMICA_CUELLO = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.EVA_ERGONOMICA_CUELLO) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.EVA_ERGONOMICA_CUELLO).v_Value1,
                               EVA_ERGONOMICA_ESPALDA = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.EVA_ERGONOMICA_ESPALDA) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.EVA_ERGONOMICA_ESPALDA).v_Value1,
                               EVA_ERGONOMICA_RODILLAS = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.EVA_ERGONOMICA_RODILLAS) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.EVA_ERGONOMICA_RODILLAS).v_Value1,
                               EVA_ERGONOMICA_RODILLAS_2 = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.EVA_ERGONOMICA_RODILLAS_2) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.EVA_ERGONOMICA_RODILLAS_2).v_Value1,
                               EVA_ERGONOMICA_BRAZO_MUNE = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.EVA_ERGONOMICA_BRAZO_MUNE) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.EVA_ERGONOMICA_BRAZO_MUNE).v_Value1,
                               EVA_ERGONOMICA_BRAZO_MUNE_2 = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.EVA_ERGONOMICA_BRAZO_MUNE_2) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.EVA_ERGONOMICA_BRAZO_MUNE_2).v_Value1,

                               EVA_ERGONOMICA_MANOS = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.EVA_ERGONOMICA_MANOS) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.EVA_ERGONOMICA_MANOS).v_Value1,
                               EVA_ERGONOMICA_RODILLAS_3 = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.EVA_ERGONOMICA_RODILLAS_3) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.EVA_ERGONOMICA_RODILLAS_3).v_Value1,
                               EVA_ERGONOMICA_CUELLOS_HOMB = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.EVA_ERGONOMICA_CUELLOS_HOMB) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.EVA_ERGONOMICA_CUELLOS_HOMB).v_Value1,
                               EVA_ERGONOMICA_CUELLOS_HOMB_2 = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.EVA_ERGONOMICA_CUELLOS_HOMB_2) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.EVA_ERGONOMICA_CUELLOS_HOMB_2).v_Value1,
                               EVA_ERGONOMICA_ZONA_LUMBAR = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.EVA_ERGONOMICA_ZONA_LUMBAR) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.EVA_ERGONOMICA_ZONA_LUMBAR).v_Value1,
                               EVA_ERGONOMICA_ZONA_LUMBAR_2 = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.EVA_ERGONOMICA_ZONA_LUMBAR_2) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.EVA_ERGONOMICA_ZONA_LUMBAR_2).v_Value1,
                               EVA_ERGONOMICA_MANOS_BRAZOS = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.EVA_ERGONOMICA_MANOS_BRAZOS) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.EVA_ERGONOMICA_MANOS_BRAZOS).v_Value1,
                               EVA_ERGONOMICA_MANOS_BRAZOS_2 = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.EVA_ERGONOMICA_MANOS_BRAZOS_2) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.EVA_ERGONOMICA_MANOS_BRAZOS_2).v_Value1,
                               EVA_ERGONOMICA_CONCLUSIONES = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.EVA_ERGONOMICA_CONCLUSIONES) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.EVA_ERGONOMICA_CONCLUSIONES).v_Value1,

                               b_Logo = MedicalCenter.b_Image,
                               EmpresaPropietaria = MedicalCenter.v_Name,
                               EmpresaPropietariaDireccion = MedicalCenter.v_Address,
                               EmpresaPropietariaTelefono = MedicalCenter.v_PhoneNumber,
                               EmpresaPropietariaEmail = MedicalCenter.v_Mail,

                           }).ToList();


                return sql;

            }
            catch (Exception)
            {

                throw;
            }


        }

        public List<Sigesoft.Node.WinClient.BE.SintomaticoRespi> ReporteSintomaticoRespiratorio(string pstrserviceId, string pstrComponentId)
        {
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                var objEntity = (from A in dbContext.service
                                 join B in dbContext.person on A.v_PersonId equals B.v_PersonId
                                 join C in dbContext.servicecomponent on new { a = pstrserviceId, b = pstrComponentId }
                                                                       equals new { a = C.v_ServiceId, b = C.v_ComponentId }

                                 // Usuario Medico Evaluador / Medico Aprobador ****************************
                                 join me in dbContext.systemuser on C.i_ApprovedUpdateUserId equals me.i_SystemUserId into me_join
                                 from me in me_join.DefaultIfEmpty()

                                 join pme in dbContext.professional on me.v_PersonId equals pme.v_PersonId into pme_join
                                 from pme in pme_join.DefaultIfEmpty()

                                 join Z in dbContext.person on me.v_PersonId equals Z.v_PersonId
                                 join E1 in dbContext.protocol on A.v_ProtocolId equals E1.v_ProtocolId

                                 join et in dbContext.systemparameter on new { a = E1.i_EsoTypeId.Value, b = 118 }
                                               equals new { a = et.i_ParameterId, b = et.i_GroupId } into et_join  // TIPO ESO [ESOA,ESOR,ETC]
                                 from et in et_join.DefaultIfEmpty()
                                 where A.v_ServiceId == pstrserviceId
                                 select new Sigesoft.Node.WinClient.BE.SintomaticoRespi
                                 {
                                     Trabajador = B.v_FirstLastName + " " + B.v_FirstLastName + " " + B.v_FirstName,
                                     FechaNacimiento = B.d_Birthdate,
                                     FechaEvaluacion = A.d_ServiceDate,
                                     Puesto = B.v_CurrentOccupation,
                                     ServicioId = A.v_ServiceId,
                                     FirmaUsuarioGraba = pme.b_SignatureImage,
                                     NombreUsuarioGraba = Z.v_FirstLastName + " " + Z.v_FirstLastName + " " + Z.v_FirstName,
                                     HuellaTrabajador = B.b_FingerPrintImage,
                                     FirmaTrabajador = B.b_RubricImage,
                                     TipoEso = et.v_Value1,
                                     Dni = B.v_DocNumber
                                 });

                var ValorUSer = ValoresComponentesUserControl(pstrserviceId, Constants.EXAMEN_FISICO_ID).ToList();
                var MedicalCenter = GetInfoMedicalCenter();
                var sql = (from a in objEntity.ToList()
                           select new Sigesoft.Node.WinClient.BE.SintomaticoRespi
                           {

                               TipoEso = a.TipoEso,
                               Dni = a.Dni,
                               Trabajador = a.Trabajador,
                               FechaNacimiento = a.FechaNacimiento,
                               FechaEvaluacion = a.FechaEvaluacion,
                               Puesto = a.Puesto,
                               ServicioId = a.ServicioId,
                               FirmaUsuarioGraba = a.FirmaUsuarioGraba,
                               NombreUsuarioGraba = a.NombreUsuarioGraba,
                               HuellaTrabajador = a.HuellaTrabajador,
                               FirmaTrabajador = a.FirmaTrabajador,
                               Edad = GetAge(a.FechaNacimiento.Value),


                               SINTOMATICO_1 = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.SINTOMATICO_1) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.SINTOMATICO_1).v_Value1,
                               SINTOMATICO_2 = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.SINTOMATICO_2) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.SINTOMATICO_2).v_Value1,
                               SINTOMATICO_3 = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.SINTOMATICO_3) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.SINTOMATICO_3).v_Value1,
                               SINTOMATICO_4 = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.SINTOMATICO_4) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.SINTOMATICO_4).v_Value1,
                               SINTOMATICO_5 = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.SINTOMATICO_5) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.SINTOMATICO_5).v_Value1,
                               SINTOMATICO_6 = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.SINTOMATICO_6) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.SINTOMATICO_6).v_Value1,
                               SINTOMATICO_7 = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.SINTOMATICO_7) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.SINTOMATICO_7).v_Value1,
                               SINTOMATICO_OBS = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.SINTOMATICO_OBS) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.SINTOMATICO_OBS).v_Value1,
                               SINTOMATICO_SI_NO = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.SINTOMATICO_SI_NO) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.SINTOMATICO_SI_NO).v_Value1,
                               SINTOMATICO_BK_1 = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.SINTOMATICO_BK_1) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.SINTOMATICO_BK_1).v_Value1,
                               SINTOMATICO_BK_2 = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.SINTOMATICO_BK_2) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.SINTOMATICO_BK_2).v_Value1,
                               SINTOMATICO_RX = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.SINTOMATICO_RX) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.SINTOMATICO_RX).v_Value1,




                               b_Logo = MedicalCenter.b_Image,
                               EmpresaPropietaria = MedicalCenter.v_Name,
                               EmpresaPropietariaDireccion = MedicalCenter.v_Address,
                               EmpresaPropietariaTelefono = MedicalCenter.v_PhoneNumber,
                               EmpresaPropietariaEmail = MedicalCenter.v_Mail,

                           }).ToList();


                return sql;

            }
            catch (Exception)
            {

                throw;
            }


        }

        public List<Sigesoft.Node.WinClient.BE.LumboSacracs> ReporteLumboSaca(string pstrserviceId, string pstrComponentId)
        {
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                var objEntity = (from A in dbContext.service
                                 join B in dbContext.person on A.v_PersonId equals B.v_PersonId
                                 join C in dbContext.servicecomponent on new { a = pstrserviceId, b = pstrComponentId }
                                                                       equals new { a = C.v_ServiceId, b = C.v_ComponentId }

                                 // Usuario Medico Evaluador / Medico Aprobador ****************************
                                 join me in dbContext.systemuser on C.i_ApprovedUpdateUserId equals me.i_SystemUserId into me_join
                                 from me in me_join.DefaultIfEmpty()

                                 join pme in dbContext.professional on me.v_PersonId equals pme.v_PersonId into pme_join
                                 from pme in pme_join.DefaultIfEmpty()

                                 join Z in dbContext.person on me.v_PersonId equals Z.v_PersonId
                                 join E1 in dbContext.protocol on A.v_ProtocolId equals E1.v_ProtocolId
                                 join D1 in dbContext.organization on E1.v_CustomerOrganizationId equals D1.v_OrganizationId

                                 join et in dbContext.systemparameter on new { a = E1.i_EsoTypeId.Value, b = 118 }
                                               equals new { a = et.i_ParameterId, b = et.i_GroupId } into et_join  // TIPO ESO [ESOA,ESOR,ETC]
                                 from et in et_join.DefaultIfEmpty()
                                 where A.v_ServiceId == pstrserviceId
                                 select new Sigesoft.Node.WinClient.BE.LumboSacracs
                                 {
                                     EmpresaCliente = D1.v_Name,
                                     Trabajador = B.v_FirstLastName + " " + B.v_FirstLastName + " " + B.v_FirstName,
                                     FechaNacimiento = B.d_Birthdate,
                                     FechaEvaluacion = A.d_ServiceDate,
                                     Puesto = B.v_CurrentOccupation,
                                     ServicioId = A.v_ServiceId,
                                     FirmaUsuarioGraba = pme.b_SignatureImage,
                                     NombreUsuarioGraba = Z.v_FirstLastName + " " + Z.v_FirstLastName + " " + Z.v_FirstName,
                                     HuellaTrabajador = B.b_FingerPrintImage,
                                     FirmaTrabajador = B.b_RubricImage,

                                     TipoEso = et.v_Value1,
                                     Dni = B.v_DocNumber
                                 });

                var ValorUSer = ValoresComponentesUserControl(pstrserviceId, pstrComponentId).ToList();
                var MedicalCenter = GetInfoMedicalCenter();
                var sql = (from a in objEntity.ToList()
                           select new Sigesoft.Node.WinClient.BE.LumboSacracs
                           {
                               EmpresaCliente = a.EmpresaCliente,
                               TipoEso = a.TipoEso,
                               Dni = a.Dni,
                               Trabajador = a.Trabajador,
                               FechaNacimiento = a.FechaNacimiento,
                               FechaEvaluacion = a.FechaEvaluacion,
                               Puesto = a.Puesto,
                               ServicioId = a.ServicioId,
                               FirmaUsuarioGraba = a.FirmaUsuarioGraba,
                               NombreUsuarioGraba = a.NombreUsuarioGraba,
                               HuellaTrabajador = a.HuellaTrabajador,
                               FirmaTrabajador = a.FirmaTrabajador,
                               Edad = GetAge(a.FechaNacimiento.Value),


                               LUMBOSACRA_1 = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.LUMBOSACRA_1) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.LUMBOSACRA_1).v_Value1,
                               LUMBOSACRA_2 = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.LUMBOSACRA_2) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.LUMBOSACRA_2).v_Value1,
                               LUMBOSACRA_3 = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.LUMBOSACRA_3) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.LUMBOSACRA_3).v_Value1,
                               LUMBOSACRA_4 = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.LUMBOSACRA_4) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.LUMBOSACRA_4).v_Value1,
                               LUMBOSACRA_5 = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.LUMBOSACRA_5) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.LUMBOSACRA_5).v_Value1,
                               LUMBOSACRA_6 = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.LUMBOSACRA_6) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.LUMBOSACRA_6).v_Value1,
                               LUMBOSACRA_7 = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.LUMBOSACRA_7) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.LUMBOSACRA_7).v_Value1,
                               LUMBOSACRA_8 = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.LUMBOSACRA_8) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.LUMBOSACRA_8).v_Value1,


                               b_Logo = MedicalCenter.b_Image,
                               EmpresaPropietaria = MedicalCenter.v_Name,
                               EmpresaPropietariaDireccion = MedicalCenter.v_Address,
                               EmpresaPropietariaTelefono = MedicalCenter.v_PhoneNumber,
                               EmpresaPropietariaEmail = MedicalCenter.v_Mail,

                           }).ToList();


                return sql;

            }
            catch (Exception)
            {

                throw;
            }


        }

        public List<Sigesoft.Node.WinClient.BE.ReportTestVertigo> GetReportTestVertigo(string pstrserviceId, string pstrComponentId)
        {
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                var objEntity = (from A in dbContext.service
                                 join B in dbContext.person on A.v_PersonId equals B.v_PersonId
                                 // Usuario Medico Evaluador / Medico Aprobador ****************************
                                 //join me in dbContext.systemuser on A.i_ApprovedUpdateUserId equals me.i_SystemUserId into me_join
                                 //from me in me_join.DefaultIfEmpty()


                                 //join pme in dbContext.professional on me.v_PersonId equals pme.v_PersonId into pme_join
                                 //from pme in pme_join.DefaultIfEmpty()
                                 join E in dbContext.servicecomponent on new { a = A.v_ServiceId, b = pstrComponentId }
                                                                      equals new { a = E.v_ServiceId, b = E.v_ComponentId }

                                 join F in dbContext.systemuser on E.i_ApprovedUpdateUserId equals F.i_SystemUserId into F_join
                                 from F in F_join.DefaultIfEmpty()

                                 join pme in dbContext.professional on F.v_PersonId equals pme.v_PersonId into pme_join
                                 from pme in pme_join.DefaultIfEmpty()

                                 join Z in dbContext.person on F.v_PersonId equals Z.v_PersonId
                                 join E1 in dbContext.protocol on A.v_ProtocolId equals E1.v_ProtocolId
                                 join D1 in dbContext.organization on E1.v_CustomerOrganizationId equals D1.v_OrganizationId

                                 where A.v_ServiceId == pstrserviceId
                                 select new Sigesoft.Node.WinClient.BE.ReportTestVertigo
                                 {
                                     EmpresaCliente = D1.v_Name,
                                     Nombres = B.v_FirstName + " " + B.v_FirstLastName + " " + B.v_SecondLastName,
                                     FechaNacimiento = B.d_Birthdate.Value,
                                     Fecha = A.d_ServiceDate.Value,
                                     Firma = pme.b_SignatureImage,
                                     NombreUsuarioGraba = Z.v_FirstLastName + " " + Z.v_SecondLastName + " " + Z.v_FirstName,
                                 });

                var MedicalCenter = GetInfoMedicalCenter();

                var sql = (from a in objEntity.ToList()
                           let CuestNordi = ValoresComponente(pstrserviceId, Constants.TEST_VERTIGO_ID)

                           select new Sigesoft.Node.WinClient.BE.ReportTestVertigo
                           {
                               EmpresaCliente = a.EmpresaCliente,
                               Nombres = a.Nombres,
                               FechaNacimiento = a.FechaNacimiento,
                               Edad = GetAge(a.FechaNacimiento),
                               Fecha = a.Fecha,
                               Firma = a.Firma,
                               TEST_VERTIGO_1 = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.TEST_VERTIGO_1) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.TEST_VERTIGO_1).v_Value1,
                               TEST_VERTIGO_2 = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.TEST_VERTIGO_2) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.TEST_VERTIGO_2).v_Value1,
                               TEST_VERTIGO_3 = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.TEST_VERTIGO_3) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.TEST_VERTIGO_3).v_Value1,
                               TEST_VERTIGO_4 = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.TEST_VERTIGO_4) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.TEST_VERTIGO_4).v_Value1,
                               TEST_VERTIGO_5 = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.TEST_VERTIGO_5) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.TEST_VERTIGO_5).v_Value1,
                               TEST_VERTIGO_6 = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.TEST_VERTIGO_6) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.TEST_VERTIGO_6).v_Value1,
                               TEST_VERTIGO_7 = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.TEST_VERTIGO_7) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.TEST_VERTIGO_7).v_Value1,
                               TEST_VERTIGO_8 = CuestNordi.Count == 0 || CuestNordi.Find(p => p.v_ComponentFieldId == Constants.TEST_VERTIGO_8) == null ? string.Empty : CuestNordi.Find(p => p.v_ComponentFieldId == Constants.TEST_VERTIGO_8).v_Value1,
                               NombreUsuarioGraba = a.NombreUsuarioGraba,
                               Logo = MedicalCenter.b_Image,
                               Dx = GetDiagnosticByServiceIdAndComponent(pstrserviceId, Constants.TEST_VERTIGO_ID),
                               Recomendaciones = ConcatenarRecomendacionesPorComponente(pstrserviceId, Constants.TEST_VERTIGO_ID)
                           }).ToList();


                return sql;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<Sigesoft.Node.WinClient.BE.UcSomnolencia> ReporteSomnolencia(string pstrserviceId, string pstrComponentId)
        {
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                var objEntity = (from A in dbContext.service
                                 join B in dbContext.person on A.v_PersonId equals B.v_PersonId
                                 join C in dbContext.servicecomponent on new { a = pstrserviceId, b = pstrComponentId }
                                                                       equals new { a = C.v_ServiceId, b = C.v_ComponentId }

                                 // Usuario Medico Evaluador / Medico Aprobador ****************************
                                 join me in dbContext.systemuser on C.i_ApprovedUpdateUserId equals me.i_SystemUserId into me_join
                                 from me in me_join.DefaultIfEmpty()

                                 join pme in dbContext.professional on me.v_PersonId equals pme.v_PersonId into pme_join
                                 from pme in pme_join.DefaultIfEmpty()

                                 join Z in dbContext.person on me.v_PersonId equals Z.v_PersonId

                                 join E1 in dbContext.protocol on A.v_ProtocolId equals E1.v_ProtocolId

                                 join et in dbContext.systemparameter on new { a = E1.i_EsoTypeId.Value, b = 118 }
                                               equals new { a = et.i_ParameterId, b = et.i_GroupId } into et_join  // TIPO ESO [ESOA,ESOR,ETC]
                                 from et in et_join.DefaultIfEmpty()
                                 join D1 in dbContext.organization on E1.v_CustomerOrganizationId equals D1.v_OrganizationId


                                 where A.v_ServiceId == pstrserviceId
                                 select new Sigesoft.Node.WinClient.BE.UcSomnolencia
                                 {
                                     EmpresaCliente = D1.v_Name,
                                     Trabajador = B.v_FirstLastName + " " + B.v_FirstLastName + " " + B.v_FirstName,
                                     FechaNacimiento = B.d_Birthdate,
                                     FechaEvaluacion = A.d_ServiceDate,
                                     Puesto = B.v_CurrentOccupation,
                                     ServicioId = A.v_ServiceId,
                                     FirmaUsuarioGraba = pme.b_SignatureImage,
                                     NombreUsuarioGraba = Z.v_FirstLastName + " " + Z.v_FirstLastName + " " + Z.v_FirstName,
                                     HuellaTrabajador = B.b_FingerPrintImage,
                                     FirmaTrabajador = B.b_RubricImage,
                                     TipoEso = et.v_Value1,
                                     Dni = B.v_DocNumber
                                 });

                var ValorUSer = ValoresComponentesUserControl(pstrserviceId, Constants.PSICOLOGIA_ID).ToList();
                var MedicalCenter = GetInfoMedicalCenter();
                var sql = (from a in objEntity.ToList()
                           select new Sigesoft.Node.WinClient.BE.UcSomnolencia
                           {
                               EmpresaCliente = a.EmpresaCliente,
                               TipoEso = a.TipoEso,
                               Dni = a.Dni,
                               Trabajador = a.Trabajador,
                               FechaNacimiento = a.FechaNacimiento,
                               FechaEvaluacion = a.FechaEvaluacion,
                               Puesto = a.Puesto,
                               ServicioId = a.ServicioId,
                               FirmaUsuarioGraba = a.FirmaUsuarioGraba,
                               NombreUsuarioGraba = a.NombreUsuarioGraba,
                               HuellaTrabajador = a.HuellaTrabajador,
                               FirmaTrabajador = a.FirmaTrabajador,
                               Edad = GetAge(a.FechaNacimiento.Value),


                               SOMNOLENCIA_1_SENTADO_ID = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.SOMNOLENCIA_1_SENTADO_ID) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.SOMNOLENCIA_1_SENTADO_ID).v_Value1,

                               SOMNOLENCIA_2_MIRANDO_TV_ID = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.SOMNOLENCIA_2_MIRANDO_TV_ID) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.SOMNOLENCIA_2_MIRANDO_TV_ID).v_Value1,

                               SOMNOLENCIA_3_SENTADO_INACTIVO_ID = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.SOMNOLENCIA_3_SENTADO_INACTIVO_ID) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.SOMNOLENCIA_3_SENTADO_INACTIVO_ID).v_Value1,
                               SOMNOLENCIA_4_PASAJERO_ID = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.SOMNOLENCIA_4_PASAJERO_ID) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.SOMNOLENCIA_4_PASAJERO_ID).v_Value1,
                               SOMNOLENCIA_5_ACOSTADO_DESC_ID = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.SOMNOLENCIA_5_ACOSTADO_DESC_ID) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.SOMNOLENCIA_5_ACOSTADO_DESC_ID).v_Value1,
                               SOMNOLENCIA_6_ACOSTADO_CONVER_ID = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.SOMNOLENCIA_6_ACOSTADO_CONVER_ID) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.SOMNOLENCIA_6_ACOSTADO_CONVER_ID).v_Value1,
                               SOMNOLENCIA_7_SENTADO_TRANQUILO_ID = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.SOMNOLENCIA_7_SENTADO_TRANQUILO_ID) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.SOMNOLENCIA_7_SENTADO_TRANQUILO_ID).v_Value1,
                               SOMNOLENCIA_8_CARRO_TRACON_ID = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.SOMNOLENCIA_8_CARRO_TRACON_ID) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.SOMNOLENCIA_8_CARRO_TRACON_ID).v_Value1,

                               SOMNOLENCIA_1_RESULTADO_ID = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.SOMNOLENCIA_1_RESULTADO_ID) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.SOMNOLENCIA_1_RESULTADO_ID).v_Value1,
                               SOMNOLENCIA_2_RESULTADO_ID = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.SOMNOLENCIA_2_RESULTADO_ID) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.SOMNOLENCIA_2_RESULTADO_ID).v_Value1,
                               SOMNOLENCIA_3_RESULTADO_ID = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.SOMNOLENCIA_3_RESULTADO_ID) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.SOMNOLENCIA_3_RESULTADO_ID).v_Value1,
                               SOMNOLENCIA_4_RESULTADO_ID = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.SOMNOLENCIA_4_RESULTADO_ID) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.SOMNOLENCIA_4_RESULTADO_ID).v_Value1,
                               SOMNOLENCIA_5_RESULTADO_ID = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.SOMNOLENCIA_5_RESULTADO_ID) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.SOMNOLENCIA_5_RESULTADO_ID).v_Value1,
                               SOMNOLENCIA_6_RESULTADO_ID = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.SOMNOLENCIA_6_RESULTADO_ID) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.SOMNOLENCIA_6_RESULTADO_ID).v_Value1,
                               SOMNOLENCIA_7_RESULTADO_ID = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.SOMNOLENCIA_7_RESULTADO_ID) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.SOMNOLENCIA_7_RESULTADO_ID).v_Value1,

                               SOMNOLENCIA_8_RESULTADO_ID = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.SOMNOLENCIA_8_RESULTADO_ID) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.SOMNOLENCIA_8_RESULTADO_ID).v_Value1,
                               SOMNOLENCIA_TOTAL_ID = ValorUSer.Count() == 0 || ValorUSer.Find(p => p.v_ComponentFieldId == Constants.SOMNOLENCIA_TOTAL_ID) == null ? string.Empty : ValorUSer.Find(p => p.v_ComponentFieldId == Constants.SOMNOLENCIA_TOTAL_ID).v_Value1,

                               b_Logo = MedicalCenter.b_Image,
                               EmpresaPropietaria = MedicalCenter.v_Name,
                               EmpresaPropietariaDireccion = MedicalCenter.v_Address,
                               EmpresaPropietariaTelefono = MedicalCenter.v_PhoneNumber,
                               EmpresaPropietariaEmail = MedicalCenter.v_Mail,
                               Dx = GetDiagnosticByServiceIdAndComponent(pstrserviceId, Constants.SOMNOLENCIA_ID),
                               Recomendaciones = ConcatenarRecomendacionesPorComponente(pstrserviceId, Constants.SOMNOLENCIA_ID)
                           }).ToList();


                return sql;

            }
            catch (Exception)
            {

                throw;
            }


        }

        public string ConcatenarRecomendacionesPorComponente(string pstrServiceId, string pstrCompoment)
        {
            SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

            var qry = (from a in dbContext.recommendation  // RECOMENDACIONES
                       join eee in dbContext.masterrecommendationrestricction on a.v_MasterRecommendationId equals eee.v_MasterRecommendationRestricctionId
                       where a.v_ServiceId == pstrServiceId && a.v_ComponentId == pstrCompoment &&
                       a.i_IsDeleted == 0 && eee.i_TypifyingId == (int)Typifying.Recomendaciones
                       select new
                       {
                           v_RecommendationName = eee.v_Name
                       }).ToList();

            return string.Join(", ", qry.Select(p => p.v_RecommendationName));
        }

        public Sigesoft.Node.WinClient.BE.ObtenerIdsImporacion ObtenerIdsParaImportacion(string pServiceId, int CategoriaId)
        {
            //mon.IsActive = true;
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
                Sigesoft.Node.WinClient.BE.ObtenerIdsImporacion objEntity = null;

                objEntity = (from a in dbContext.servicecomponent
                             join b in dbContext.component on a.v_ComponentId equals b.v_ComponentId
                             join c in dbContext.service on a.v_ServiceId equals c.v_ServiceId
                             join d in dbContext.person on c.v_PersonId equals d.v_PersonId
                             where b.i_CategoryId == CategoriaId && a.v_ServiceId == pServiceId
                             orderby b.i_UIIndex
                             select new Sigesoft.Node.WinClient.BE.ObtenerIdsImporacion
                             {
                                 ServicioId = c.v_ServiceId,
                                 ServicioComponentId = a.v_ServiceComponentId,
                                 ComponentId = a.v_ComponentId,
                                 PersonId = c.v_PersonId,
                                 Paciente = d.v_FirstLastName + " " + d.v_SecondLastName + " " + d.v_FirstName,
                                 DNI = d.v_DocNumber,
                                 CategoriaId = b.i_CategoryId.Value,
                                 i_UIIndex = b.i_UIIndex.Value
                             }).FirstOrDefault();

                return objEntity;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public Sigesoft.Node.WinClient.BE.ObtenerIdsImporacion ObtenerIdsParaImportacion_(string pServiceId, int CategoriaId)
        {
            //mon.IsActive = true;
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
                Sigesoft.Node.WinClient.BE.ObtenerIdsImporacion objEntity = null;

                objEntity = (from a in dbContext.servicecomponent
                             join b in dbContext.component on a.v_ComponentId equals b.v_ComponentId
                             join c in dbContext.service on a.v_ServiceId equals c.v_ServiceId
                             join d in dbContext.person on c.v_PersonId equals d.v_PersonId
                             where b.i_CategoryId == CategoriaId && a.v_ServiceId == pServiceId
                             orderby b.i_UIIndex descending
                             select new Sigesoft.Node.WinClient.BE.ObtenerIdsImporacion
                             {
                                 ServicioId = c.v_ServiceId,
                                 ServicioComponentId = a.v_ServiceComponentId,
                                 ComponentId = a.v_ComponentId,
                                 PersonId = c.v_PersonId,
                                 Paciente = d.v_FirstLastName + " " + d.v_SecondLastName + " " + d.v_FirstName,
                                 DNI = d.v_DocNumber,
                                 CategoriaId = b.i_CategoryId.Value,
                                 i_UIIndex = b.i_UIIndex.Value
                             }).FirstOrDefault();

                return objEntity;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public Sigesoft.Node.WinClient.BE.DiagnosticRepositoryList VerificarDxExistente(string pServiceId, string pDiseasesId, string pComponentId, string pComponentFieldId)
        {
            SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
            var query = (from A in dbContext.diagnosticrepository
                         where A.v_ServiceId == pServiceId && A.v_ComponentId == pComponentId && A.v_ComponentFieldId == pComponentFieldId && A.i_IsDeleted == 0
                         select new Sigesoft.Node.WinClient.BE.DiagnosticRepositoryList
                         {
                             i_AutoManualId = A.i_AutoManualId,
                             i_FinalQualificationId = A.i_FinalQualificationId,
                             i_PreQualificationId = A.i_PreQualificationId,
                             v_ComponentFieldsId = A.v_ComponentFieldId,
                             v_DiagnosticRepositoryId = A.v_DiagnosticRepositoryId
                         }
                             ).FirstOrDefault();

            return query;


        }

        public List<Anexo312> RetornarAnexo312(string pServiceId, string pPersonId)
        {
            try
            {

                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                //var objFamilyMedicalAntecedentsList = (from a in dbContext.familymedicalantecedents
                //                                       where a.v_PersonId == pPersonId && a.i_IsDeleted == 0
                //                                       select new FamilyMedicalAntecedentsList
                //                                       {
                //                                           v_PersonId = a.v_PersonId,
                //                                           i_TypeFamilyId = a.i_TypeFamilyId.Value,
                //                                           v_CommentFamili = a.v_Comment,
                //                                           v_FamilyMedicalAntecedentsId = a.v_FamilyMedicalAntecedentsId
                //                                       }).ToList();

                //var objListaHabitosNocivos = (from a in dbContext.noxioushabits
                //                              where a.v_PersonId == pPersonId && a.i_IsDeleted == 0
                //                              select new NoxiousHabitsList
                //                              {
                //                                  v_PersonId = a.v_PersonId,
                //                                  i_TypeHabitsId = a.i_TypeHabitsId,
                //                                  //i_FrequencyId = a.i_FrequencyId,
                //                                  v_Comment = a.v_Comment,
                //                                  v_NoxiousHabitsId = a.v_NoxiousHabitsId
                //                              }).ToList();

                //var objListaHistoriaOcupacional = (from a in dbContext.history
                //                                   where a.v_PersonId == pPersonId && a.i_IsDeleted == 0
                //                                   select new HistoryList
                //                                   {
                //                                       v_PersonId = a.v_PersonId,
                //                                       v_Organization = a.v_Organization,
                //                                       v_TypeActivity = a.v_TypeActivity,
                //                                       v_workstation = a.v_workstation,
                //                                       //v_Fechas = a.v_Fechas,
                //                                       //v_Exposicion = a.v_Exposicion,
                //                                       //v_Epps = a.v_Epps,
                //                                       //v_TiempoTrabajo = a.v_TiempoTrabajo,
                //                                       i_TrabajoActual = a.i_TrabajoActual.Value,
                //                                       v_HistoryId = a.v_HistoryId
                //                                   }).ToList();


                //var objListaPatoPersonales = (from a in dbContext.personmedicalhistory
                //                              where a.v_PersonId == pPersonId && a.i_IsDeleted == 0
                //                              select new PersonMedicalHistoryList
                //                              {
                //                                  v_PersonId = a.v_PersonId,
                //                                  v_DiseasesId = a.v_DiseasesId,
                //                                  //v_FechaInicio = a.v_FechaInicio,
                //                                  //i_AsociadoTrabajo = a.i_AsociadoTrabajo.Value,
                //                                  v_DiagnosticDetail = a.v_DiagnosticDetail,
                //                                  v_TreatmentSite = a.v_TreatmentSite,
                //                                  v_PersonMedicalHistoryId = a.v_PersonMedicalHistoryId
                //                              }).ToList();

                var objFiliacion = (from a in dbContext.service
                                    join b in dbContext.person on a.v_PersonId equals b.v_PersonId

                                    where a.v_ServiceId == pServiceId
                                    select new Anexo312
                                    {
                                        PersonId = a.v_PersonId,
                                        FechaNacimiento = b.d_Birthdate.Value,
                                        TipoDocumentoId = b.i_DocTypeId.Value,
                                        NroDocumento = b.v_DocNumber,
                                        DireccionFiscal = b.v_AdressLocation,
                                        DepartamentoId = b.i_DepartmentId.Value,
                                        ProvinciaId = b.i_ProvinceId.Value,
                                        DistritoId = b.i_DistrictId.Value,
                                        ResideLugarTrabajo = b.i_ResidenceInWorkplaceId,
                                        TiempoResidencia = b.v_ResidenceTimeInWorkplace,
                                        Telefono = b.v_TelephoneNumber,
                                        TipoSeguroId = b.i_TypeOfInsuranceId.Value,
                                        Email = b.v_Mail,
                                        EstadoCivilId = b.i_MaritalStatusId.Value,
                                        GradoInstruccionId = b.i_LevelOfId.Value,
                                        HijosVivos = b.i_NumberLiveChildren.Value,
                                        HijosMuertos = b.i_NumberDeadChildren.Value,
                                        Anamnesis = a.v_Story,
                                        v_Menarquia = a.v_Menarquia,
                                        v_Gestapara = a.v_Gestapara,
                                        //v_FechaUltimaRegla = a.v_FechaUltimaRegla,
                                        i_MacId = a.i_MacId,
                                        v_CatemenialRegime = a.v_CatemenialRegime,
                                        v_CiruGine = a.v_CiruGine,
                                        v_FechaUltimoPAP = a.v_FechaUltimoPAP,
                                        v_ResultadoMamo = a.v_ResultadoMamo,
                                        v_FechaUltimaMamo = a.v_FechaUltimaMamo,
                                        v_ResultadosPAP = a.v_ResultadosPAP,
                                        i_StatusAptitud = a.i_AptitudeStatusId,
                                        Comentario = a.v_ObsStatusService,
                                        //ComentarioMedico = a.v_ComentarioMedico,
                                        //Restricciones = a.v_ComentarioRestricciones
                                    }).ToList();

                //objFiliacion.ForEach(a =>
                //{
                //    a.ListaHistoriaOcupacional = objListaHistoriaOcupacional.FindAll(p => p.v_PersonId == a.PersonId);
                //    a.ListaMedicosPersonales = objListaPatoPersonales.FindAll(p => p.v_PersonId == a.PersonId);
                //    a.ListaHabitosNosivos = objListaHabitosNocivos.FindAll(p => p.v_PersonId == a.PersonId);
                //    a.ListaMedicosFamiliares = objFamilyMedicalAntecedentsList.FindAll(p => p.v_PersonId == a.PersonId);

                //});

                return objFiliacion;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<Sigesoft.Node.WinClient.BE.ObtenerIdsImporacion> ObtenerComponetesPorServicio(string pServiceId)
        {
            //mon.IsActive = true;
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
                List<Sigesoft.Node.WinClient.BE.ObtenerIdsImporacion> objEntity = null;

                objEntity = (from a in dbContext.servicecomponent
                             join b in dbContext.component on a.v_ComponentId equals b.v_ComponentId
                             where a.v_ServiceId == pServiceId && a.i_IsDeleted == 0

                             select new Sigesoft.Node.WinClient.BE.ObtenerIdsImporacion
                             {
                                 ComponentId = a.v_ComponentId,
                                 CategoriaId = b.i_CategoryId.Value,
                             }).ToList();

                return objEntity;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public List<Sigesoft.Node.WinClient.BE.ServiceComponentFieldsList> GetServiceComponentFields(string pstrServiceComponentId, string pstrServiceId)
        {
            var isDeleted = (int)SiNo.NO;
            var isDeleted1 = (int)SiNo.NO;
            string serviceId = pstrServiceId;
            var serviceComponentId = pstrServiceComponentId;

            SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

            #region serviceComponentFieldValues

            var serviceComponentFieldValues = (from s in dbContext.service
                                               join sc in dbContext.servicecomponent on s.v_ServiceId equals sc.v_ServiceId
                                               join scfs in dbContext.servicecomponentfields on sc.v_ServiceComponentId equals scfs.v_ServiceComponentId
                                               join A in dbContext.servicecomponentfieldvalues on scfs.v_ServiceComponentFieldsId equals A.v_ServiceComponentFieldsId

                                               where s.v_ServiceId == pstrServiceId &&
                                                     A.i_IsDeleted == isDeleted

                                               select new Sigesoft.Node.WinClient.BE.ServiceComponentFieldValuesList
                                               {
                                                   v_ComponentFieldId = scfs.v_ComponentFieldId,
                                                   v_ServiceComponentFieldValuesId = A.v_ServiceComponentFieldValuesId,
                                                   v_ComponentFieldValuesId = A.v_ComponentFieldValuesId,
                                                   v_ServiceComponentFieldsId = A.v_ServiceComponentFieldsId,
                                                   v_Value1 = A.v_Value1,
                                                   v_Value2 = A.v_Value2,
                                                   i_Index = A.i_Index,
                                                   i_Value1 = A.i_Value1,
                                               }).ToList();

            #endregion

            var serviceComponentFields = (from A in dbContext.servicecomponentfields
                                          join ss in
                                              (from jjj in dbContext.diagnosticrepository
                                               where //(jjj.v_ComponentFieldId == "") &&
                                                     (jjj.v_ServiceId == serviceId) &&
                                                     (jjj.i_IsDeleted == 0)

                                               select new
                                               {
                                                   v_ComponentFieldId = jjj.v_ComponentFieldId,
                                                   i_HasAutomaticDxId = jjj.v_ComponentFieldId != null ? (int?)SiNo.SI : (int?)SiNo.NO
                                               }) on A.v_ComponentFieldId equals ss.v_ComponentFieldId into feePayments

                                          from ss in feePayments.DefaultIfEmpty()

                                          where (A.v_ServiceComponentId == serviceComponentId) &&
                                                (A.i_IsDeleted == isDeleted)

                                          select new Sigesoft.Node.WinClient.BE.ServiceComponentFieldsList
                                          {
                                              v_ServiceComponentFieldsId = A.v_ServiceComponentFieldsId,
                                              v_ComponentFieldsId = A.v_ComponentFieldId,
                                              v_ServiceComponentId = A.v_ServiceComponentId,
                                              i_HasAutomaticDxId = ss.i_HasAutomaticDxId,
                                          }).ToList();



            if (serviceComponentFields.Count != 0)
            {
                // Ordenar la lista temp para hacer busquedas rapidas sobre ella (obligatorio)
                serviceComponentFieldValues.Sort((x, y) => x.v_ServiceComponentFieldsId.CompareTo(y.v_ServiceComponentFieldsId));
                serviceComponentFields.Sort((x, y) => x.v_ServiceComponentFieldsId.CompareTo(y.v_ServiceComponentFieldsId));
                serviceComponentFields.ForEach(a => a.ServiceComponentFieldValues = serviceComponentFieldValues.FindAll(p => p.v_ServiceComponentFieldsId == a.v_ServiceComponentFieldsId));

                return serviceComponentFields;
            }
            else
            {
                return serviceComponentFields;
            }
            //var dd = new List<ServiceComponentFieldsList>();
            //return dd;
        }

        public List<ServiceList> GetAllServices(ref OperationResult pobjOperationResult, int? pintPageIndex, int? pintResultsPerPage, string pstrSortExpression, string pstrFilterExpression, DateTime? pdatBeginDate, DateTime? pdatEndDate)
        {
            //mon.IsActive = true;

            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
                var query = from A in dbContext.service
                            join B in dbContext.servicecomponent on A.v_ServiceId equals B.v_ServiceId
                            join I in dbContext.person on A.v_PersonId equals I.v_PersonId

                            join K in dbContext.systemparameter on new { a = A.i_AptitudeStatusId.Value, b = 124 } equals new { a = K.i_ParameterId, b = K.i_GroupId } into K_join
                            from K in K_join.DefaultIfEmpty()

                            join E in dbContext.protocol on A.v_ProtocolId equals E.v_ProtocolId

                            join J in dbContext.systemparameter on new { a = I.i_SexTypeId.Value, b = 100 }
                                            equals new { a = J.i_ParameterId, b = J.i_GroupId } into J_join // GENERO
                            from J in J_join.DefaultIfEmpty()

                            join H in dbContext.systemparameter on new { a = E.i_EsoTypeId.Value, b = 118 }
                                                equals new { a = H.i_ParameterId, b = H.i_GroupId }  // TIPO ESO [ESOA,ESOR,ETC]


                            // Empresa / Sede Cliente ******************************************************
                            join oc in dbContext.organization on new { a = E.v_CustomerOrganizationId }
                                    equals new { a = oc.v_OrganizationId } into oc_join
                            from oc in oc_join.DefaultIfEmpty()

                            join lc in dbContext.location on new { a = E.v_CustomerOrganizationId, b = E.v_CustomerLocationId }
                                  equals new { a = lc.v_OrganizationId, b = lc.v_LocationId } into lc_join
                            from lc in lc_join.DefaultIfEmpty()

                            //**********************************************************************************

                            where A.i_IsDeleted == 0 && B.i_IsRequiredId ==1

                            select new ServiceList
                            {
                                v_IdTrabajador = A.v_PersonId,
                                v_ServiceId = A.v_ServiceId,
                                v_ProtocolId = A.v_ProtocolId,
                                v_PersonId = A.v_PersonId,
                                i_AptitudeStatusId = A.i_AptitudeStatusId.Value,
                                d_ServiceDate = (DateTime)A.d_ServiceDate,
                                v_Pacient = I.v_FirstLastName + " " + I.v_SecondLastName + " " + I.v_FirstName,
                                v_ProtocolName = E.v_Name,
                                v_CustomerOrganizationId = E.v_CustomerOrganizationId,
                                v_CustomerLocationId = lc.v_LocationId,
                                EmpresaCliente = oc.v_Name,
                                d_FechaNacimiento = I.d_Birthdate,
                                v_Genero = J.v_Value1,
                                v_TipoEso = H.v_Value1,
                                v_GrupoRiesgo = "",
                                v_Puesto = I.v_CurrentOccupation,
                                Dni = I.v_DocNumber,
                                v_ComponentId = B.v_ComponentId,
                                i_IsRequiredId =B.i_IsRequiredId.Value
                            };

                if (!string.IsNullOrEmpty(pstrFilterExpression))
                {
                    query = query.Where(pstrFilterExpression);
                }
                if (pdatBeginDate.HasValue && pdatEndDate.HasValue)
                {
                    query = query.Where("d_ServiceDate >= @0 && d_ServiceDate <= @1", pdatBeginDate.Value, pdatEndDate.Value);
                }
                if (!string.IsNullOrEmpty(pstrSortExpression))
                {
                    query = query.OrderBy(pstrSortExpression);
                }

                List<ServiceList> objData = query.ToList();
                pobjOperationResult.Success = 1;


                return objData;



            }
            catch (Exception ex)
            {
                pobjOperationResult.Success = 0;
                pobjOperationResult.ExceptionMessage = Common.Utils.ExceptionFormatter(ex);
                return null;
            }
        }

        public void EliminarDxAniguosPorComponente(string pServiceId, string pComponentId, List<string> ClientSession)
        {

            SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

            // Obtener la entidad fuente
            var ListaEliminar = (from a in dbContext.diagnosticrepository
                                 where a.v_ServiceId == pServiceId && a.v_ComponentId == pComponentId && a.i_IsDeleted == 0
                                 select a).ToList();


            foreach (var item in ListaEliminar)
            {
                // Crear la entidad con los datos actualizados
                item.d_UpdateDate = DateTime.Now;
                item.i_UpdateUserId = Int32.Parse(ClientSession[2]);
                item.i_IsDeleted = 1;

            }
            // Guardar los cambios
            dbContext.SaveChanges();


        }

        public List<Sigesoft.Node.WinClient.BE.ComponentFieldsList> ObtenerValoresPorDefecto(ref OperationResult pobjOperationResult, string pstrServiceId)
        {

            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                var isDeleted = 0;
               
         
                #region Fields

                var _comFields = (from s in dbContext.service
                                  join sc in dbContext.servicecomponent on s.v_ServiceId equals sc.v_ServiceId
                                  join c in dbContext.component on sc.v_ComponentId equals c.v_ComponentId
                                  join cfs in dbContext.componentfields on c.v_ComponentId equals cfs.v_ComponentId
                                  join cf in dbContext.componentfield on cfs.v_ComponentFieldId equals cf.v_ComponentFieldId                               

                                  where (cfs.i_IsDeleted == isDeleted) &&
                                        (cf.i_IsDeleted == isDeleted) &&
                                        (s.v_ServiceId == pstrServiceId)

                                  select new Sigesoft.Node.WinClient.BE.ComponentFieldsList
                                  {
                                      v_ComponentFieldId = cf.v_ComponentFieldId,
                                      v_TextLabel = cf.v_TextLabel,
                                      v_ComponentId = cfs.v_ComponentId,
                                      i_LabelWidth = cf.i_LabelWidth.Value,
                                      v_DefaultText = cf.v_DefaultText,
                                      i_ControlId = cf.i_ControlId.Value,
                                      i_GroupId = cf.i_GroupId.Value,
                                      i_ItemId = cf.i_ItemId.Value,
                                      i_ControlWidth = cf.i_WidthControl.Value,
                                      i_HeightControl = cf.i_HeightControl.Value,
                                      i_MaxLenght = cf.i_MaxLenght.Value,
                                      i_IsRequired = cf.i_IsRequired.Value,
                                      i_Column = cf.i_Column.Value,
                                      i_IsCalculate = cf.i_IsCalculate.Value,
                                      i_Order = cf.i_Order.Value,
                                      i_MeasurementUnitId = cf.i_MeasurementUnitId.Value,
                                      r_ValidateValue1 = cf.r_ValidateValue1.Value,
                                      r_ValidateValue2 = cf.r_ValidateValue2.Value,
                                      v_Group = cfs.v_Group,
                                      v_Formula = cf.v_Formula,
                                      i_NroDecimales = cf.i_NroDecimales.Value,
                                      i_ReadOnly = cf.i_ReadOnly.Value,
                                      i_Enabled = cf.i_Enabled.Value                                      
                                  }).ToList();

                #endregion

                pobjOperationResult.Success = 1;

                return _comFields;
            }
            catch (Exception ex)
            {
                pobjOperationResult.Success = 0;
                pobjOperationResult.ExceptionMessage = Common.Utils.ExceptionFormatter(ex);
                return null;
            }
        }


        public void ActualizarServiceComponentUsuario(ref OperationResult pobjOperationResult, string pstrServiceComponentId, List<string> ClientSession)
        {
            //mon.IsActive = true;

            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                // Obtener la entidad fuente
                var objEntitySource = (from a in dbContext.servicecomponent
                                       where a.v_ServiceComponentId == pstrServiceComponentId
                                       select a).FirstOrDefault();

                // Crear la entidad con los datos actualizados
                objEntitySource.d_ApprovedUpdateDate = DateTime.Now;
                objEntitySource.i_ApprovedInsertUserId = Int32.Parse(ClientSession[2]);
            

                // Guardar los cambios
                dbContext.SaveChanges();

                pobjOperationResult.Success = 1;
                      return;
            }
            catch (Exception ex)
            {
                pobjOperationResult.Success = 0;
                pobjOperationResult.ExceptionMessage = Common.Utils.ExceptionFormatter(ex);
                // Llenar entidad Log
               return;
            }
        }


        public void AddServiceComponent(ref OperationResult pobjOperationResult, servicecomponentDto pobjDtoEntity, List<string> ClientSession)
        {
            //mon.IsActive = true;
            string NewId = "(No generado)";
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
                servicecomponent objEntity = servicecomponentAssembler.ToEntity(pobjDtoEntity);

                objEntity.d_InsertDate = DateTime.Now;
                objEntity.i_InsertUserId = Int32.Parse(ClientSession[2]);
                objEntity.i_IsDeleted = 0;
                // Autogeneramos el Pk de la tabla
                int intNodeId = int.Parse(ClientSession[0]);
                NewId = Common.Utils.GetNewId(intNodeId, Utils.GetNextSecuentialId(intNodeId, 24), "SC");
                objEntity.v_ServiceComponentId = NewId;

                dbContext.AddToservicecomponent(objEntity);
                dbContext.SaveChanges();

                pobjOperationResult.Success = 1;
                // Llenar entidad Log
                LogBL.SaveLog(ClientSession[0], ClientSession[1], ClientSession[2], LogEventType.CREACION, "COMPONENTE DE SERVICIO", "v_ServiceComponentId=" + NewId.ToString(), Success.Ok, null);
                return;
            }
            catch (Exception ex)
            {
                pobjOperationResult.Success = 0;
                pobjOperationResult.ExceptionMessage = Common.Utils.ExceptionFormatter(ex);
                // Llenar entidad Log
                LogBL.SaveLog(ClientSession[0], ClientSession[1], ClientSession[2], LogEventType.CREACION, "COMPONENTE DE SERVICIO", "v_ServiceComponentId=" + NewId.ToString(), Success.Failed, pobjOperationResult.ExceptionMessage);
                return;
            }
        }

        public string AddService(ref OperationResult pobjOperationResult, serviceDto pobjDtoEntity, List<string> ClientSession)
        {
            //mon.IsActive = true;
            string NewId = "(No generado)";
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
                service objEntity = serviceAssembler.ToEntity(pobjDtoEntity);

                objEntity.d_InsertDate = DateTime.Now;
                objEntity.i_InsertUserId = Int32.Parse(ClientSession[2]);
                objEntity.i_IsDeleted = 0;
                // Autogeneramos el Pk de la tabla
                int intNodeId = int.Parse(ClientSession[0]);
                NewId = Common.Utils.GetNewId(intNodeId, Utils.GetNextSecuentialId(intNodeId, 23), "SR");
                objEntity.v_ServiceId = NewId;

                dbContext.AddToservice(objEntity);
                dbContext.SaveChanges();

                pobjOperationResult.Success = 1;
                // Llenar entidad Log
                LogBL.SaveLog(ClientSession[0], ClientSession[1], ClientSession[2], LogEventType.CREACION, "SERVICIO", "v_ServiceId=" + NewId.ToString(), Success.Ok, null);
                return NewId;
            }
            catch (Exception ex)
            {
                pobjOperationResult.Success = 0;
                pobjOperationResult.ExceptionMessage = Common.Utils.ExceptionFormatter(ex);
                // Llenar entidad Log
                LogBL.SaveLog(ClientSession[0], ClientSession[1], ClientSession[2], LogEventType.CREACION, "SERVICIO", "v_ServiceId=" + NewId.ToString(), Success.Failed, pobjOperationResult.ExceptionMessage);
                return null;
            }
        }

        public List<ServiceComponentList> GetServiceComponentsByCategoryExceptLab(ref OperationResult pobjOperationResult, string pstrServiceId)
        {


            int isDeleted = (int)SiNo.NO;
            int isRequired = (int)SiNo.SI;

            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                var query = (from A in dbContext.servicecomponent
                             join B in dbContext.systemparameter on new { a = A.i_ServiceComponentStatusId.Value, b = 127 }
                                      equals new { a = B.i_ParameterId, b = B.i_GroupId }
                             join C in dbContext.component on A.v_ComponentId equals C.v_ComponentId
                             join D in dbContext.systemparameter on new { a = A.i_QueueStatusId.Value, b = 128 }
                                      equals new { a = D.i_ParameterId, b = D.i_GroupId }
                             join E in dbContext.service on A.v_ServiceId equals E.v_ServiceId
                             join F in dbContext.systemparameter on new { a = C.i_CategoryId.Value, b = 116 }
                                      equals new { a = F.i_ParameterId, b = F.i_GroupId } into F_join
                             from F in F_join.DefaultIfEmpty()

                             where A.v_ServiceId == pstrServiceId &&
                                   A.i_IsDeleted == isDeleted &&
                                   A.i_IsRequiredId == isRequired &&
                                  A.v_ComponentId != Constants.INFORME_LABORATORIO_ID

                             select new ServiceComponentList
                             {
                                 v_ComponentId = A.v_ComponentId,
                                 v_ComponentName = C.v_Name,
                                 i_ServiceComponentStatusId = A.i_ServiceComponentStatusId.Value,
                                 v_ServiceComponentStatusName = B.v_Value1,
                                 d_StartDate = A.d_StartDate.Value,
                                 d_EndDate = A.d_EndDate.Value,
                                 i_QueueStatusId = A.i_QueueStatusId.Value,
                                 v_QueueStatusName = D.v_Value1,
                                 ServiceStatusId = E.i_ServiceStatusId.Value,
                                 v_Motive = E.v_Motive,
                                 i_CategoryId = C.i_CategoryId.Value,
                                 v_CategoryName = C.i_CategoryId.Value == -1 ? C.v_Name : F.v_Value1,
                                 v_ServiceId = E.v_ServiceId
                             });

                var objData = query.AsEnumerable()
                             .Where(s => s.i_CategoryId != -1 && s.i_CategoryId != 1 && s.i_CategoryId != 6)
                             .GroupBy(x => x.i_CategoryId)
                             .Select(group => group.First());

                List<ServiceComponentList> obj = objData.ToList();

                obj.AddRange(query.Where(p => p.i_CategoryId == -1));
                obj.AddRange(query.Where(p => p.i_CategoryId == 1));
                obj.AddRange(query.Where(p => p.i_CategoryId == 6));
                pobjOperationResult.Success = 1;
                var orden = obj.OrderBy(o => o.i_CategoryId).ToList();
                return orden;
            }
            catch (Exception ex)
            {
                pobjOperationResult.Success = 0;
                pobjOperationResult.ExceptionMessage = Common.Utils.ExceptionFormatter(ex);
                return null;
            }
        }

        public List<ServiceList> ObtenerServiciosPorPersonId(string pstrPersonId)
        {
            SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
            List<ServiceList> objEntity = null;
            objEntity = (from a in dbContext.service
                         where a.v_PersonId == pstrPersonId

                         select new ServiceList
                         {
                             v_ServiceId = a.v_ServiceId,
                             d_ServiceDate = a.d_ServiceDate.Value

                         }).ToList();

            return objEntity;
        }
       

   }
}
