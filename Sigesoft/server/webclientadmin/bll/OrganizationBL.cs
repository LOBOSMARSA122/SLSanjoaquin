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
    public class OrganizationBL
    {

        public List<KeyValueDTO> GetAllOrganizations(ref OperationResult pobjOperationResult)
        {
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                var query = from a in dbContext.organization
                            where a.i_IsDeleted == 0
                            select new KeyValueDTO
                            {
                                Id = a.v_OrganizationId,
                                Value1 = a.v_Name
                            };

                List<KeyValueDTO> objDataList = query.OrderBy(p => p.Value1).ToList();

                pobjOperationResult.Success = 1;

                return objDataList;

            }
            catch (Exception ex)
            {
                pobjOperationResult.Success = 0;
                pobjOperationResult.ExceptionMessage = ex.Message;
                return null;
            }
        }

        public List<KeyValueDTO> GetAllOrganizations_(ref OperationResult pobjOperationResult)
        {
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                var query = from a in dbContext.organization
                            join b in dbContext.location on a.v_OrganizationId equals b.v_OrganizationId
                            where a.i_IsDeleted == 0
                            select new KeyValueDTO
                            {
                                Id = a.v_OrganizationId + "|" + b.v_LocationId,
                                Value1 = a.v_Name + " / " + b.v_Name,
                            };

                List<KeyValueDTO> objDataList = query.OrderBy(p => p.Value1).ToList();

                pobjOperationResult.Success = 1;

                return objDataList;

            }
            catch (Exception ex)
            {
                pobjOperationResult.Success = 0;
                pobjOperationResult.ExceptionMessage = ex.Message;
                return null;
            }
        }

        //public List<OrganizationList> GetOrganizationsPagedAndFiltered(ref OperationResult pobjOperationResult, int? pintPageIndex, int? pintResultsPerPage, string pstrSortExpression, string pstrFilterExpression)
        //{
        //    //mon.IsActive = true;

        //    try
        //    {
        //        SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
        //        var query = from A in dbContext.Organizations
        //                    join B in dbContext.SystemParameters on new { a = A.i_OrganizationTypeId.Value, b = 103 } equals new { a = B.i_ParameterId, b = B.i_GroupId }
        //                    join C in dbContext.DataHierarchies on new { a = A.i_SectorTypeId.Value, b = 104 } equals new { a = C.i_ItemId, b = C.i_GroupId }
        //                    join J1 in dbContext.SystemUsers on new { i_InsertUserId = A.i_InsertUserId.Value }
        //                                                    equals new { i_InsertUserId = J1.i_SystemUserId } into J1_join
        //                    from J1 in J1_join.DefaultIfEmpty()

        //                    join J2 in dbContext.SystemUsers on new { i_UpdateUserId = A.i_UpdateUserId.Value }
        //                                                    equals new { i_UpdateUserId = J2.i_SystemUserId } into J2_join
        //                    from J2 in J2_join.DefaultIfEmpty()
        //                    select new OrganizationList
        //                    {
        //                        i_OrganizationId = A.i_OrganizationId,
        //                        i_OrganizationTypeId = (int)A.i_OrganizationTypeId,
        //                        v_OrganizationTypeIdName = B.v_Value1,
        //                        i_SectorTypeId = (int)A.i_SectorTypeId,
        //                        v_SectorTypeIdName = C.v_Value1,
        //                        v_IdentificationNumber = A.v_IdentificationNumber,
        //                        v_Name = A.v_Name,
        //                        v_Address = A.v_Address,
        //                        v_PhoneNumber = A.v_PhoneNumber,
        //                        v_Mail = A.v_Mail,
        //                        v_CreationUser = J1.v_UserName,
        //                        v_UpdateUser = J2.v_UserName,
        //                        d_CreationDate = A.d_InsertDate,
        //                        d_UpdateDate = A.d_UpdateDate,
        //                        i_IsDeleted = A.i_IsDeleted
        //                    };

        //        if (!string.IsNullOrEmpty(pstrFilterExpression))
        //        {
        //            query = query.Where(pstrFilterExpression);
        //        }
        //        if (!string.IsNullOrEmpty(pstrSortExpression))
        //        {
        //            query = query.OrderBy(pstrSortExpression);
        //        }
        //        if (pintPageIndex.HasValue && pintResultsPerPage.HasValue)
        //        {
        //            int intStartRowIndex = pintPageIndex.Value * pintResultsPerPage.Value;
        //            query = query.Skip(intStartRowIndex);
        //        }
        //        if (pintResultsPerPage.HasValue)
        //        {
        //            query = query.Take(pintResultsPerPage.Value);
        //        }

        //        List<OrganizationList> objData = query.ToList();
        //        pobjOperationResult.Success = 1;
        //        return objData;

        //    }
        //    catch (Exception ex)
        //    {
        //        pobjOperationResult.Success = 0;
        //        pobjOperationResult.ExceptionMessage = ex.Message;
        //        return null;
        //    }
        //}

        //public int GetOrganizationsCount(ref OperationResult pobjOperationResult, string filterExpression)
        //{
        //    mon.IsActive = true;

        //    try
        //    {
        //        SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
        //        var query = from a in dbContext.Organizations select a;

        //        if (!string.IsNullOrEmpty(filterExpression))
        //            query = query.Where(filterExpression);

        //        int intResult = query.Count();

        //        pobjOperationResult.Success = 1;
        //        return intResult;
        //    }
        //    catch (Exception ex)
        //    {
        //        pobjOperationResult.Success = 0;
        //        pobjOperationResult.ExceptionMessage = ex.Message;
        //        return 0;
        //    }
        //}

        //public OrganizationDto GetOrganization(ref OperationResult pobjOperationResult, int pintOrganizationId)
        //{
        //    mon.IsActive = true;

        //    try
        //    {
        //        SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
        //        OrganizationDto objDtoEntity = null;

        //        var objEntity = (from a in dbContext.Organizations
        //                         where a.i_OrganizationId == pintOrganizationId
        //                         select a).FirstOrDefault();

        //        if (objEntity != null)
        //            objDtoEntity = OrganizationAssembler.ToDTO(objEntity);

        //        pobjOperationResult.Success = 1;
        //        return objDtoEntity;
        //    }
        //    catch (Exception ex)
        //    {
        //        pobjOperationResult.Success = 0;
        //        pobjOperationResult.ExceptionMessage = ex.Message;
        //        return null;
        //    }
        //}

        //public void AddOrganization(ref OperationResult pobjOperationResult, OrganizationDto pobjDtoEntity, List<string> ClientSession)
        //{
        //    mon.IsActive = true;

        //    try
        //    {
        //        SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
        //        Organization objEntity = OrganizationAssembler.ToEntity(pobjDtoEntity);

        //        objEntity.d_InsertDate = DateTime.Now;
        //        objEntity.i_InsertUserId = Int32.Parse(ClientSession[2]);
        //        objEntity.i_IsDeleted = 0;
        //        // Autogeneramos el Pk de la tabla
        //        int SecuentialId = new Utils().GetNextSecuentialId(1, 5);
        //        objEntity.i_OrganizationId = SecuentialId;

        //        dbContext.AddToOrganizations(objEntity);
        //        dbContext.SaveChanges();

        //        pobjOperationResult.Success = 1;
        //        // Llenar entidad Log
        //        new Utils().SaveLog(ClientSession[0], ClientSession[1], ClientSession[2], Constants.LogEventType.CREACION, "ORGANIZACIÓN", "i_OrganizationId=" + objEntity.i_OrganizationId.ToString(), Constants.Success.Ok, null);
        //        return;
        //    }
        //    catch (Exception ex)
        //    {
        //        pobjOperationResult.Success = 0;
        //        pobjOperationResult.ExceptionMessage = ex.Message;
        //        // Llenar entidad Log
        //        new Utils().SaveLog(ClientSession[0], ClientSession[1], ClientSession[2], Constants.LogEventType.CREACION, "ORGANIZACIÓN", "i_OrganizationId=" + pobjDtoEntity.i_OrganizationId.ToString(), Constants.Success.Failed, ex.Message);
        //        return;
        //    }
        //}

        //public void UpdateOrganization(ref OperationResult pobjOperationResult, OrganizationDto pobjDtoEntity, List<string> ClientSession)
        //{
        //    mon.IsActive = true;

        //    try
        //    {
        //        SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

        //        // Obtener la entidad fuente
        //        var objEntitySource = (from a in dbContext.Organizations
        //                               where a.i_OrganizationId == pobjDtoEntity.i_OrganizationId
        //                               select a).FirstOrDefault();

        //        // Crear la entidad con los datos actualizados
        //        pobjDtoEntity.d_UpdateDate = DateTime.Now;
        //        pobjDtoEntity.i_UpdateUserId = Int32.Parse(ClientSession[2]);
        //        Organization objEntity = OrganizationAssembler.ToEntity(pobjDtoEntity);

        //        // Copiar los valores desde la entidad actualizada a la Entidad Fuente
        //        dbContext.Organizations.ApplyCurrentValues(objEntity);

        //        // Guardar los cambios
        //        dbContext.SaveChanges();

        //        pobjOperationResult.Success = 1;
        //        // Llenar entidad Log
        //        new Utils().SaveLog(ClientSession[0], ClientSession[1], ClientSession[2], Constants.LogEventType.ACTUALIZACION, "ORGANIZACIÓN", "i_OrganizationId=" + objEntity.i_OrganizationId.ToString(), Constants.Success.Ok, null);
        //        return;
        //    }
        //    catch (Exception ex)
        //    {
        //        pobjOperationResult.Success = 0;
        //        pobjOperationResult.ExceptionMessage = ex.Message;
        //        // Llenar entidad Log
        //        new Utils().SaveLog(ClientSession[0], ClientSession[1], ClientSession[2], Constants.LogEventType.ACTUALIZACION, "ORGANIZACIÓN", "i_OrganizationId=" + pobjDtoEntity.i_OrganizationId.ToString(), Constants.Success.Failed, ex.Message);
        //        return;
        //    }
        //}

        //public void DeleteOrganization(ref OperationResult pobjOperationResult, int pintOrganizationId, List<string> ClientSession)
        //{
        //    mon.IsActive = true;

        //    try
        //    {
        //        SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

        //        // Obtener la entidad fuente
        //        var objEntitySource = (from a in dbContext.Organizations
        //                               where a.i_OrganizationId == pintOrganizationId
        //                               select a).FirstOrDefault();

        //        // Crear la entidad con los datos actualizados
        //        objEntitySource.d_UpdateDate = DateTime.Now;
        //        objEntitySource.i_UpdateUserId = Int32.Parse(ClientSession[2]);
        //        objEntitySource.i_IsDeleted = 1;

        //        // Guardar los cambios
        //        dbContext.SaveChanges();

        //        pobjOperationResult.Success = 1;
        //        // Llenar entidad Log
        //        new Utils().SaveLog(ClientSession[0], ClientSession[1], ClientSession[2], Constants.LogEventType.ELIMINACION, "ORGANIZACIÓN", "", Constants.Success.Ok, null);
        //        return;
        //    }
        //    catch (Exception ex)
        //    {
        //        pobjOperationResult.Success = 0;
        //        pobjOperationResult.ExceptionMessage = ex.Message;
        //        // Llenar entidad Log
        //        new Utils().SaveLog(ClientSession[0], ClientSession[1], ClientSession[2], Constants.LogEventType.ELIMINACION, "ORGANIZACIÓN", "", Constants.Success.Failed, ex.Message);
        //        return;
        //    }
        //}

        //public List<OrganizationDto> GetOrganizationsBySystemUserIdAndNodeId(ref OperationResult pobjOperationResult, int pintNodeId, int pintPersonId, int pintSystemNodeId)
        //{
        //    try
        //    {
        //        SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
        //        List<OrganizationDto> objListOrganizationDto;
        //        objListOrganizationDto = (from a in dbContext.SystemUserContexProfiles
        //                                  join o in dbContext.Organizations on a.i_OrganizationId equals o.i_OrganizationId
        //                                  where a.i_NodeId == pintNodeId && a.i_PersonId == pintPersonId && a.i_SystemUserNodeId == pintSystemNodeId
        //                                  select new OrganizationDto
        //                                  {
        //                                      i_OrganizationId = Convert.ToInt32(a.i_OrganizationId),
        //                                      v_Name = o.v_Name
        //                                  }).Distinct().ToList();
        //        pobjOperationResult.Success = 1;
        //        return objListOrganizationDto;

        //    }
        //    catch (Exception ex)
        //    {
        //        pobjOperationResult.Success = 0;
        //        pobjOperationResult.ExceptionMessage = ex.Message;
        //        return null;
        //    }

        //}   

    }
}
