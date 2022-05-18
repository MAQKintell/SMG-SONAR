using System;
using System.Data;
using Iberdrola.Commons.DataAccess;
using Iberdrola.Commons.Exceptions;
using Iberdrola.Commons.Utils;
using Iberdrola.SMG.BLL;
using Iberdrola.SMG.DAL.DTO;

namespace Iberdrola.SMG.DAL.DB
{
    /// <summary>
    /// Acciones que tienen que ver con la configuración.
    /// </summary>
    public class ConfiguracionDB
    {
        /// <summary>
        /// El constructor de la clase.
        /// </summary>
        public ConfiguracionDB()
        {
        }

        /// <summary>
        /// Obtenemos la entrada de configuración seleccionada.
        /// </summary>
        /// <param name="configuracion">Enumerado con la configuración deseada.</param>
        /// <returns>Objeto con la configuración seleccionada.</returns>
        public ConfiguracionDTO ObtenerConfiguracion(Enumerados.Configuracion codConfiguracion)
        {
            IDataReader dr = null;

            try
            {
                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);

                string[] parametros = new string[1];
                DbType[] tipos = new DbType[1];
                object[] valoresparametros = new object[1];

                parametros[0] = "@p_COD";
                tipos[0] = DbType.String;
                valoresparametros[0] = StringEnum.GetStringValue(codConfiguracion);

                dr = db.RunProcDataReader("spCommonsConfigGet", parametros, tipos, valoresparametros);

                ConfiguracionDTO configuracion = null;

                if (dr.Read())
                {
                    configuracion = new ConfiguracionDTO();
                    configuracion.CodConfig = codConfiguracion;
                    configuracion.Valor = (string)DataBaseUtils.GetDataReaderColumnValue(dr, "VAL");
                    configuracion.TipoDato = (Enumerados.TipoDatos)DataBaseUtils.GetDataReaderColumnValue(dr, "ID_TYPE");
                    configuracion.Descripcion = (string)DataBaseUtils.GetDataReaderColumnValue(dr, "DESCRIP");
                }

                return configuracion;
            }
            catch (BaseException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new DALException(false, ex, "2004");
            }
            finally
            {
                if (dr != null)
                {
                    dr.Close();
                }
            }
        }

        public ConfiguracionDTO ObtenerConfiguracion(Enumerados.ConexionesYRutas codConfiguracion)
        {
            IDataReader dr = null;

            try
            {
                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);

                string[] parametros = new string[1];
                DbType[] tipos = new DbType[1];
                object[] valoresparametros = new object[1];

                parametros[0] = "@p_COD";
                tipos[0] = DbType.String;
                valoresparametros[0] = StringEnum.GetStringValue(codConfiguracion);

                dr = db.RunProcDataReader("spCommonsConfigGet", parametros, tipos, valoresparametros);

                ConfiguracionDTO configuracion = null;

                if (dr.Read())
                {
                    configuracion = new ConfiguracionDTO();
                    configuracion.CodConexionYRutas = codConfiguracion;
                    configuracion.Valor = (string)DataBaseUtils.GetDataReaderColumnValue(dr, "VAL");
                    configuracion.TipoDato = (Enumerados.TipoDatos)DataBaseUtils.GetDataReaderColumnValue(dr, "ID_TYPE");
                    configuracion.Descripcion = (string)DataBaseUtils.GetDataReaderColumnValue(dr, "DESCRIP");
                }

                return configuracion;
            }
            catch (BaseException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new DALException(false, ex, "2004");
            }
            finally
            {
                if (dr != null)
                {
                    dr.Close();
                }
            }
        }
    }
}
