using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Iberdrola.SMG.DAL;
using Iberdrola.SMG.DAL.DTO;
using System.Collections.Generic;
using Iberdrola.Commons.DataAccess;
using Iberdrola.Commons.Exceptions;

namespace Iberdrola.SMG.DAL.DB
{
    /// <summary>
    /// Descripción breve de UsuarioDB
    /// </summary>
    public class EquipamientoDB
    {
        public EquipamientoDB()
	    {

	    }
        public IDataReader ObtenerTipoPotencia()
        {
            try
            {
                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
                
                String[] aNombres = new String[0] { };
                DbType[] aTipos = new DbType[0] { };
                String[] aValores = new String[0] { };

                IDataReader dr = db.RunProcDataReader("SP_ObtenerTipoPotencia", aNombres, aTipos, aValores);

                return dr;
            }
            catch (BaseException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new DALException(false, ex, "2004");
            }
        }

        public List<EquipamientoDTO> ObtenerEquipamientos(String strCodContrato,Int16 idIdioma)
        {
            try
            {
                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
                List<EquipamientoDTO> lista = new List<EquipamientoDTO>();
                //String[] aNombres = new String[1] { "@COD_CONTRATO" };
                //DbType[] aTipos = new DbType[1] { DbType.String }; 
                //String[] aValores = new String[1] { strCodContrato };

                String[] aNombres = new String[2] { 
                                        "@COD_CONTRATO",
                                        "@idIDIOMA"
                                        };
                DbType[] aTipos = new DbType[2] {
                                        DbType.String,
                                        DbType.Int16};

                Object[] aValores = new Object[2] { 
                                        strCodContrato,
                                        idIdioma};

                IDataReader dr = db.RunProcDataReader("SP_GET_EQUIPAMIENTOS", aNombres, aTipos, aValores);

                while (dr.Read())
                {
                    EquipamientoDTO equipamiento = new EquipamientoDTO();
                    
                    equipamiento.Id = (Decimal?)DataBaseUtils.GetDataReaderColumnValue(dr, "ID_EQUIPAMIENTO");
                    equipamiento.CodContrato = (String)DataBaseUtils.GetDataReaderColumnValue(dr, "COD_CONTRATO");
                    equipamiento.IdTipoEquipamiento = (Decimal?)DataBaseUtils.GetDataReaderColumnValue(dr, "ID_TIPO_EQUIPAMIENTO");
                    equipamiento.Potencia = (Double?)DataBaseUtils.GetDataReaderColumnValue(dr, "POTENCIA");
                    equipamiento.DescripcionTipoEquipamiento = (String)DataBaseUtils.GetDataReaderColumnValue(dr, "DESC_TIPO_EQUIPAMIENTO");
                    lista.Add(equipamiento);
                }
                return lista;
            }
            catch (BaseException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new DALException(false, ex, "2004");
            }
        }
        public void ActualizarEquipamientos(List<EquipamientoDTO> listaEquipamientos)
        {
            try
            {
                foreach (EquipamientoDTO equipamiento in listaEquipamientos)
                {
                    DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);

                    if (equipamiento.Id.HasValue)
                    {
                        String[] aNombres = new String[4] { 
                                        "@ID_EQUIPAMIENTO",
                                        "@COD_CONTRATO",
                                        "@ID_TIPO_EQUIPAMIENTO",
                                        "@POTENCIA"};
                        DbType[] aTipos = new DbType[4] {
                                        DbType.Decimal,
                                        DbType.String,
                                        DbType.Decimal,
                                        DbType.Double};

                        Object[] aValores = new Object[4] { 
                                        equipamiento.Id,
                                        equipamiento.CodContrato,
                                        equipamiento.IdTipoEquipamiento, 
                                        equipamiento.Potencia};
                        // actualizar equipamiento
                        db.RunProcEscalar("SP_UPDATE_EQUIPAMIENTO", aNombres, aTipos, aValores);
                    }
                    else
                    {
                        String[] aNombres = new String[3] { 
                                        "@COD_CONTRATO",
                                        "@ID_TIPO_EQUIPAMIENTO",
                                        "@POTENCIA"};

                        DbType[] aTipos = new DbType[3] {
                                        DbType.String,
                                        DbType.Decimal,
                                        DbType.Double};

                        Object[] aValores = new Object[3] { 
                                        equipamiento.CodContrato,
                                        equipamiento.IdTipoEquipamiento, 
                                        equipamiento.Potencia};
                        // insertar equipamiento
                        db.RunProcEscalar("SP_INSERT_EQUIPAMIENTO", aNombres, aTipos, aValores);
                    }
                }
            }
            catch (BaseException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new DALException(false, ex, "2005");
            }
        }
        public void EliminarEquipamiento(EquipamientoDTO equipamiento)
        {
            try
            {
                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
                String[] aNombres = new String[2] { "@ID_EQUIPAMIENTO", "@COD_CONTRATO" };
                DbType[] aTipos = new DbType[2] { DbType.Decimal, DbType.String };
                Object[] aValores = new Object[2] { equipamiento.Id, equipamiento.CodContrato };

                if (equipamiento.Id.HasValue)
                {
                    // actualizar equipamiento
                    db.RunProcEscalar("SP_DELETE_EQUIPAMIENTO", aNombres, aTipos, aValores);
                }
             }
            catch (BaseException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new DALException(false, ex, "");
            }
        }
    }
}
