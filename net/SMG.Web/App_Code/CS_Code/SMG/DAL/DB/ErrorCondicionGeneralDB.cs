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
using System.Collections.Generic;
using Iberdrola.Commons.DataAccess;
using Iberdrola.Commons.Exceptions;
using Iberdrola.SMG.DAL.DTO;

namespace Iberdrola.SMG.DAL.DB
{
    /// <summary>
    /// Descripción breve de ErrorCondicionGeneralDB
    /// </summary>
    public class ErrorCondicionGeneralDB
    {

        /// <summary>
        /// Devuelve si un error existe o no
        /// </summary>
        /// <param name="strUsuario">Login del usuario que se comprueba</param>
        /// <param name="considerarBajas">
        ///     <value>"true"</value> - solo devuelve que existe si no tiene fecha baja o todavia no se ha alcanzado.
        ///     <value>"false"</value> - devuelve si existe independientemente de si esta dado de baja o no.
        /// </param>
        /// <returns></returns>
        public Boolean ExisteError(Int32 codigo)
        {
            IDataReader dr = null;
            try
            {
                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
                Boolean Existe = false;

                string[] parametros = new string[1];
                DbType[] tipos = new DbType[1];
                object[] valoresParametros = new object[1];

                parametros[0] = "@CODIGO";                
                tipos[0] = DbType.Int32;
                valoresParametros[0] = codigo;

                dr = db.RunProcDataReader("spSMGErrorCondicionesGeneralesExist", parametros, tipos, valoresParametros);

                while (dr.Read())
                {
                    if ((int)DataBaseUtils.GetDataReaderColumnValue(dr, "") != 0)
                    {
                        Existe = true;
                    }

                }
                return Existe;
            }
            catch (BaseException)
            {
                throw;
            }
            catch(Exception ex)
            {
                throw;
            }
            finally
            {
                if (dr != null)
                {
                    dr.Close();
                }
            }
        }

                
        public ErrorCondicionGeneralDTO ObtenerError(Int32 codigo)
        {
            IDataReader dr = null;
            try
            {
                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);

                string[] parametros = new string[1];
                DbType[] tipos = new DbType[1];
                object[] valoresParametros = new object[1];

                parametros[0] = "@CODIGO";
                tipos[0] = DbType.Int32;
                valoresParametros[0] = codigo;

                dr = db.RunProcDataReader("spSMGErrorCondicionesGeneralesGet", parametros, tipos, valoresParametros);

                while (dr.Read())
                {
                    ErrorCondicionGeneralDTO errorCondicionGeneral = new ErrorCondicionGeneralDTO();

                    errorCondicionGeneral.IdError = (Int32)DataBaseUtils.GetDataReaderColumnValue(dr, "ID_ERROR");
                    errorCondicionGeneral.Codigo = (Int32)DataBaseUtils.GetDataReaderColumnValue(dr, "CODIGO");
                    errorCondicionGeneral.CasoGeneral = (Boolean)DataBaseUtils.GetDataReaderColumnValue(dr, "CASO_GENERAL");
                    errorCondicionGeneral.CasoT10T11 = (Boolean)DataBaseUtils.GetDataReaderColumnValue(dr, "CASO_T10_T11");

                    return errorCondicionGeneral;
                }
                return null;
            }
            catch (BaseException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new DALException(false, ex, "2007");
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
