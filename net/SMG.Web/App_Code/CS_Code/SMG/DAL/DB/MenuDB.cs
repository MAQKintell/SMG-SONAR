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
using System.Transactions;

namespace Iberdrola.SMG.DAL.DB
{
    /// <summary>
    /// Descripción breve de UsuarioDB
    /// </summary>
    public class MenuDB
    {
        public MenuDB()
	    {

	    }

        public List<MenuDTO> ObtenerMenu(Int32 modulo, Int32 idPerfil, bool? permiso,Int32 idIdioma)
        {
            try
            {
                DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
                List<MenuDTO> lista = new List<MenuDTO>();

                String[] aNombres = new String[4] { "@ID_PERFIL", "@MODULO", "@PERMISO" ,"@ID_IDIOMA"};
                DbType[] aTipos = new DbType[4] { DbType.Int32, DbType.Int32, DbType.Boolean, DbType.Int32 };
                Object[] aValores = new Object[4] { idPerfil, modulo, permiso.Value,idIdioma };

                IDataReader dr = db.RunProcDataReader("SP_GET_MENU", aNombres, aTipos, aValores);

                while (dr.Read())
                {
                    MenuDTO menu = new MenuDTO();

                    menu.IdMenu = (Decimal?)DataBaseUtils.GetDataReaderColumnValue(dr, "ID_MENU");
                    menu.Modulo = (Int32?)DataBaseUtils.GetDataReaderColumnValue(dr, "MODULO");
                    menu.Orden = (String)DataBaseUtils.GetDataReaderColumnValue(dr, "ORDEN");
                    menu.Texto = (String)DataBaseUtils.GetDataReaderColumnValue(dr, "TEXTO");
                    menu.DescMenu = (String)DataBaseUtils.GetDataReaderColumnValue(dr, "DESC_MENU");
                    menu.Link = (String)DataBaseUtils.GetDataReaderColumnValue(dr, "LINK");

                    lista.Add(menu);
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

    }
}
