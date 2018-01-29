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
    public class ProtocolBL
    {
        public  List<KeyValueDTO> GetProtocolBySystemUser(ref OperationResult pobjOperationResult, int pintSystemUserId)
        {
            //mon.IsActive = true;

            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                var query = (from a in dbContext.protocolsystemuser
                            join b in dbContext.protocol on a.v_ProtocolId equals b.v_ProtocolId
                            where (a.i_SystemUserId == pintSystemUserId) &&
                            (a.i_IsDeleted == 0)
                            select new KeyValueDTO {
                                Id = b.v_ProtocolId,
                                Value1 = b.v_Name
                            }).Distinct().ToList();
             
                pobjOperationResult.Success = 1;
                return query;
            }
            catch (Exception ex)
            {
                pobjOperationResult.Success = 0;
                pobjOperationResult.ExceptionMessage = ex.Message;
                return null;
            }
        }

        public string GetNameOrganizationCustomer(string pstrProtocolId)
        {
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                var query = (from a in dbContext.protocol
                            join b in dbContext.organization on a.v_CustomerOrganizationId equals b.v_OrganizationId
                             where (a.v_ProtocolId == pstrProtocolId) &&
                            (a.i_IsDeleted == 0)
                            select new KeyValueDTO {
                                Id = b.v_OrganizationId,
                                Value1 = b.v_Name
                            }).FirstOrDefault();

                return query.Value1; 

            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<OrganizationShortList> GetOrganizationCustumerByProtocolSystemUser(ref OperationResult pobjOperationResult, int pintSystemUserId)
        {
            //mon.IsActive = true;
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                var query = (from a in dbContext.protocolsystemuser
                             join b in dbContext.protocol on a.v_ProtocolId equals b.v_ProtocolId
                             join c in dbContext.organization on b.v_CustomerOrganizationId equals  c.v_OrganizationId
                             where (a.i_SystemUserId == pintSystemUserId) &&
                             (a.i_IsDeleted == 0)
                             select new OrganizationShortList
                             {
                               CustomerOrganizationName = c.v_Name,
                               IdEmpresaCliente = c.v_OrganizationId
                             }).ToList();

                var y = query.GroupBy(g => g.CustomerOrganizationName)
                         .Select(s => s.First());

                pobjOperationResult.Success = 1;
                return y.ToList();
            }
            catch (Exception ex)
            {
                pobjOperationResult.Success = 0;
                pobjOperationResult.ExceptionMessage = ex.Message;
                return null;
            }
        }

        public List<OrganizationShortList> DevolverTodasEmpresas(ref OperationResult pobjOperationResult)
        {
            //mon.IsActive = true;
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                var query = (from a in dbContext.protocol
                             join b in dbContext.organization on a.v_CustomerOrganizationId equals  b.v_OrganizationId
                             where (a.i_IsDeleted == 0)
                             select new OrganizationShortList
                             {
                                 CustomerOrganizationName = b.v_Name,
                                 IdEmpresaCliente = a.v_CustomerOrganizationId
                             }).ToList();

                var y = query.GroupBy(g => g.CustomerOrganizationName)
                         .Select(s => s.First());

                pobjOperationResult.Success = 1;
                return y.ToList();
            }
            catch (Exception ex)
            {
                pobjOperationResult.Success = 0;
                pobjOperationResult.ExceptionMessage = ex.Message;
                return null;
            }
        }

        public OrganizationShortList GetOrganizationCustumerByProtocolSystemUser_(ref OperationResult pobjOperationResult, int pintSystemUserId)
        {
            //mon.IsActive = true;
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                var query = (from a in dbContext.protocolsystemuser
                             join b in dbContext.protocol on a.v_ProtocolId equals b.v_ProtocolId
                             join c in dbContext.organization on b.v_CustomerOrganizationId equals c.v_OrganizationId
                             where (a.i_SystemUserId == pintSystemUserId) &&
                             (a.i_IsDeleted == 0)
                             select new OrganizationShortList
                             {
                                 CustomerOrganizationName = c.v_Name,
                                 IdEmpresaCliente = c.v_OrganizationId
                             }).FirstOrDefault();

                pobjOperationResult.Success = 1;
                return query;
            }
            catch (Exception ex)
            {
                pobjOperationResult.Success = 0;
                pobjOperationResult.ExceptionMessage = ex.Message;
                return null;
            }
        }
        
        public int ObtenerRol(int pintNodeId, int pintSystemUserId)
        {
            //mon.IsActive = true;
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                var query = (from rnp in dbContext.rolenodeprofile
                             join rn in dbContext.rolenode on new { a = rnp.i_NodeId, b = rnp.i_RoleId }
                                                     equals new { a = rn.i_NodeId, b = rn.i_RoleId } into rn_join
                             from rnj in rn_join.DefaultIfEmpty()

                             join surn in dbContext.systemuserrolenode on new { a = rnp.i_NodeId, b = rnp.i_RoleId }
                                                    equals new { a = surn.i_NodeId, b = surn.i_RoleId } into surn_join
                             from surn in surn_join.DefaultIfEmpty()
                            
                             select new 
                             {
                                 i_RoleId = surn.i_RoleId
                             }).FirstOrDefault();


            
                return query.i_RoleId;
            }
            catch (Exception ex)
            {
             
                return 0;
            }

        }

        public List<ObtenerIdsImporacion> ObtenerIdsParaImportacion(List<string> ServiceIds, int CategoriaId)
        {
            //mon.IsActive = true;
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
                List<ObtenerIdsImporacion> objEntity = null;

                objEntity = (from a in dbContext.servicecomponent
                             join b in dbContext.component on a.v_ComponentId equals b.v_ComponentId
                             join c in dbContext.service on a.v_ServiceId equals c.v_ServiceId
                             join d in dbContext.person on c.v_PersonId equals d.v_PersonId
                             where b.i_CategoryId == CategoriaId && ServiceIds.Contains(c.v_ServiceId)
                             orderby b.i_UIIndex
                             select new ObtenerIdsImporacion
                             {
                                 ServicioId = c.v_ServiceId,
                                 ServicioComponentId = a.v_ServiceComponentId,
                                 ComponentId = a.v_ComponentId,
                                 PersonId = c.v_PersonId,
                                 Paciente = d.v_FirstLastName + " " + d.v_SecondLastName + " " + d.v_FirstName,
                                 DNI = d.v_DocNumber,
                                 CategoriaId = b.i_CategoryId.Value,
                                 i_UIIndex = b.i_UIIndex.Value
                             }).ToList();



                var objData = objEntity.AsEnumerable()
                           .GroupBy(x => new { x.CategoriaId, x.ServicioId })
                           .Select(group => group.First())
                           .OrderBy(o => o.i_UIIndex);



                return objData.ToList();
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public List<Sigesoft.Node.WinClient.BE.ProtocolComponentList> GetProtocolComponents(ref OperationResult pobjOperationResult, string pstrProtocolId)
        {
            //mon.IsActive = true;

            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                var objEntity = (from a in dbContext.protocolcomponent
                                 join b in dbContext.component on a.v_ComponentId equals b.v_ComponentId
                                 join fff in dbContext.systemparameter on new { a = b.i_CategoryId.Value, b = 116 } // CATEGORIA DEL EXAMEN
                                                                                          equals new { a = fff.i_ParameterId, b = fff.i_GroupId } into J5_join
                                 from fff in J5_join.DefaultIfEmpty()

                                 join E in dbContext.systemparameter on new { a = a.i_OperatorId.Value, b = 117 }
                                                                equals new { a = E.i_ParameterId, b = E.i_GroupId } into J1_join
                                 from E in J1_join.DefaultIfEmpty()

                                 join H in dbContext.systemparameter on new { a = a.i_GenderId.Value, b = 130 }  // Genero condicional
                                                                    equals new { a = H.i_ParameterId, b = H.i_GroupId } into J2_join
                                 from H in J2_join.DefaultIfEmpty()

                                 join I in dbContext.systemparameter on new { a = b.i_ComponentTypeId.Value, b = 126 }  // Tipo componente
                                                                   equals new { a = I.i_ParameterId, b = I.i_GroupId } into J3_join
                                 from I in J3_join.DefaultIfEmpty()
                                 where a.v_ProtocolId == pstrProtocolId
                                 && a.i_IsDeleted == 0

                                 select new Sigesoft.Node.WinClient.BE.ProtocolComponentList
                                 {
                                     v_ComponentId = a.v_ComponentId,
                                     v_ComponentName = b.v_Name,
                                     //v_ServiceComponentStatusName = K.v_Value1,
                                     v_ProtocolComponentId = a.v_ProtocolComponentId,
                                     r_Price = a.r_Price,
                                     v_Operator = E.v_Value1,
                                     i_Age = a.i_Age,
                                     v_Gender = H.v_Value1,
                                     i_IsConditionalIMC = a.i_IsConditionalIMC,
                                     r_Imc = a.r_Imc,
                                     v_IsConditional = a.i_IsConditionalId == 1 ? "Si" : "No",
                                     i_isAdditional = a.i_IsAdditional,
                                     v_ComponentTypeName = I.v_Value1,
                                     i_RecordStatus = (int)RecordStatus.Grabado,
                                     i_RecordType = (int)RecordType.NoTemporal,
                                     i_GenderId = a.i_GenderId,
                                     i_IsConditionalId = a.i_IsConditionalId,
                                     i_OperatorId = a.i_OperatorId,
                                     i_IsDeleted = a.i_IsDeleted,
                                     d_CreationDate = a.d_InsertDate,
                                     v_CategoryName = fff.v_Value1,
                                     i_CategoryId = b.i_CategoryId
                                 }).ToList();

                pobjOperationResult.Success = 1;
                return objEntity;
            }
            catch (Exception ex)
            {
                pobjOperationResult.Success = 0;
                pobjOperationResult.ExceptionMessage = Common.Utils.ExceptionFormatter(ex);
                return null;
            }
        }

        public string ObtenerProtocoloIdPorCodigoProtocolo(string pstrCodigoProtocolo)
        {
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                var query = (from a in dbContext.protocol
                             where (a.v_Name == pstrCodigoProtocolo) &&
                            (a.i_IsDeleted == 0)
                             select new
                             {
                                 ProtocoloId = a.v_ProtocolId
                             }).FirstOrDefault();

                return query.ProtocoloId;

            }
            catch (Exception)
            {
                return null;
            }
        }
              
        public string AddSystemUserExternal(ref OperationResult pobjOperationResult, List<protocolsystemuserDto> ListProtocolSystemUser, List<string> ClientSession, int? pintsystemUserId)
        {
            string newId = string.Empty;
            person objEntity1 = null;

            OperationResult objOperationResult = new OperationResult();

            try
            {

                #region GRABA ProtocolSystemUser

                if (ListProtocolSystemUser != null)
                {
                    AddProtocolSystemUser(ref objOperationResult, ListProtocolSystemUser, pintsystemUserId, ClientSession, true);
                }

                #endregion

                pobjOperationResult.Success = 1;
            }
            catch (Exception ex)
            {
                pobjOperationResult.Success = 0;
                pobjOperationResult.ExceptionMessage = ex.Message;
                // Llenar entidad Log
                //LogBL.SaveLog(ClientSession[0], ClientSession[1], ClientSession[2], LogEventType.CREACION, "PERSONA", "v_PersonId=" + objEntity1.v_PersonId, Success.Failed, ex.Message);
            }

            return newId;
        }

        public void AddProtocolSystemUser(ref OperationResult pobjOperationResult, List<protocolsystemuserDto> ListProtocolSystemUserDto, int? pintSystemUserId, List<string> ClientSession, bool pbRegisterLog)
        {
            int SecuentialId = -1;
            string newId;

            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                foreach (var item in ListProtocolSystemUserDto)
                {
                    // Autogeneramos el Pk de la tabla
                    SecuentialId = Utils.GetNextSecuentialId(Int32.Parse(ClientSession[0]), 44);
                    newId = Common.Utils.GetNewId(int.Parse(ClientSession[0]), SecuentialId, "PU");

                    // Grabar como nuevo
                    var objEntity = protocolsystemuserAssembler.ToEntity(item);

                    objEntity.v_ProtocolSystemUserId = newId;
                    if (pintSystemUserId == null)
                        objEntity.i_SystemUserId = item.i_SystemUserId;
                    else
                        objEntity.i_SystemUserId = pintSystemUserId.Value;
                    objEntity.d_InsertDate = DateTime.Now;
                    objEntity.i_InsertUserId = Int32.Parse(ClientSession[2]);
                    objEntity.i_IsDeleted = 0;
                    dbContext.AddToprotocolsystemuser(objEntity);
                }

                dbContext.SaveChanges();
                pobjOperationResult.Success = 1;

                if (pbRegisterLog == true)
                {
                    // Llenar entidad Log
                    LogBL.SaveLog(ClientSession[0], ClientSession[1], ClientSession[2], LogEventType.CREACION,
                       "ProtocolSystemUser", null, Success.Ok, null);
                }

                return;
            }
            catch (Exception ex)
            {
                pobjOperationResult.Success = 0;
                pobjOperationResult.ExceptionMessage = Common.Utils.ExceptionFormatter(ex);
                LogBL.SaveLog(ClientSession[0], ClientSession[1], ClientSession[2], LogEventType.CREACION,
                       "ProtocolSystemUser", string.Empty, Success.Failed, pobjOperationResult.ExceptionMessage);
                return;
            }

        }

        public List<Sigesoft.Node.WinClient.BE.ProtocolList> DevolverProtocolosPorEmpresaOnly(string pstrEmpresaCliente)
        {
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                var query = (from A in dbContext.protocol
                             where A.v_CustomerOrganizationId == pstrEmpresaCliente && A.i_IsDeleted == 0
                             select new Sigesoft.Node.WinClient.BE.ProtocolList
                             {
                                 v_ProtocolId = A.v_ProtocolId,
                                 v_Name = A.v_Name
                             }).ToList();
                return query;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<protocolsystemuserDto> ObtenerPermisosPorProtocoloId(string pstrProtocolId, int pintSystemUserId)
        {
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                var query = (from A in dbContext.protocolsystemuser
                             where A.v_ProtocolId == pstrProtocolId && A.i_IsDeleted == 0 && A.i_SystemUserId == pintSystemUserId
                             select new protocolsystemuserDto
                             {
                                 i_ApplicationHierarchyId = A.i_ApplicationHierarchyId
                             }).ToList();

                return query;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public ProtocolSystemUserList ObtenerEmpresaPorSystemUserId(int pstrSystemUserId)
        {
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                var query = (from A in dbContext.protocolsystemuser
                             join B in dbContext.protocol on A.v_ProtocolId equals B.v_ProtocolId

                             where A.i_SystemUserId == pstrSystemUserId && A.i_IsDeleted == 0
                             select new ProtocolSystemUserList
                             {
                                 EmpresaId = B.v_CustomerOrganizationId + "|" + B.v_CustomerLocationId,
                                 ProtocoloId = A.v_ProtocolId
                             }).FirstOrDefault();

                return query;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public void EliminarFisicamentePermisosPorUsuario(ref OperationResult pobjOperationResult, int pintSystemUserId)
        {
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                var ListobjEntity = (from a in dbContext.protocolsystemuser
                                     where a.i_SystemUserId == pintSystemUserId
                                     select a).ToList();


                foreach (var item in ListobjEntity)
                {
                    dbContext.DeleteObject(item);
                    dbContext.SaveChanges();
                }

                pobjOperationResult.Success = 1;

            }

            catch (Exception ex)
            {
                pobjOperationResult.Success = 0;
                pobjOperationResult.ExceptionMessage = ex.Message;
                return;
            }
        }

        public List<Sigesoft.Node.WinClient.BE.ProtocolList> DevolverProtocolosPorEmpresa(string pstrEmpresaCliente, string pstrLocationClienteId)
        {
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                var query = (from A in dbContext.protocol
                             where A.v_CustomerOrganizationId == pstrEmpresaCliente && A.v_CustomerLocationId == pstrLocationClienteId && A.i_IsDeleted == 0
                             select new Sigesoft.Node.WinClient.BE.ProtocolList
                             {
                                 v_ProtocolId = A.v_ProtocolId
                             }).ToList();
                return query;
            }
            catch (Exception)
            {

                throw;
            }
        }


    }
}
