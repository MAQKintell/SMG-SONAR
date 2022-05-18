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

namespace Iberdrola.SMG.DAL.DB
{
    /// <summary>
    /// Descripción breve de UsuarioDB
    /// </summary>
    public class PoblacionDB
    {
        public PoblacionDB()
	    {

	    }

        public List<PoblacionDTO> ObtenerPoblaciones(ProvinciaDTO provincia)
        {

            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
            List<PoblacionDTO> lista = new List<PoblacionDTO>();
            String[] aNombres = new String[1] { "@PROVINCIA" };
            DbType[] aTipos = new DbType[1] { DbType.String };
            String[] aValores = new String[1] { provincia.CodProvincia };

            IDataReader dr = db.RunProcDataReader("SP_GET_POBLACION", aNombres, aTipos, aValores);
            

            while (dr.Read())
            {
                PoblacionDTO pobl = new PoblacionDTO();

                pobl.CodPoblacion = (String)DataBaseUtils.GetDataReaderColumnValue(dr, "COD_POBLACION");
                pobl.CodProvincia = (String)DataBaseUtils.GetDataReaderColumnValue(dr, "COD_PROVINCIA");
                pobl.Nombre = (String)DataBaseUtils.GetDataReaderColumnValue(dr, "NOMBRE");

                lista.Add(pobl);
            }
            return lista;
        }
    }
}
