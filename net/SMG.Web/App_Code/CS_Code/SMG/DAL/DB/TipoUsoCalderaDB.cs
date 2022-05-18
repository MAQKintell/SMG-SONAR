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
    /// Descripción breve de TipoMarcaCalderaDB
    /// </summary>
    public class TipoUsoCalderaDB
    {
        public TipoUsoCalderaDB()
	    {

	    }

        public List<TipoUsoCalderaDTO> ObtenerTiposUsoCaldera()
        {
            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
            List<TipoUsoCalderaDTO> lista = new List<TipoUsoCalderaDTO>();

            IDataReader dr = db.RunProcDataReader("SP_GET_TIPO_USO_CALDERA");

            while (dr.Read())
            {
                TipoUsoCalderaDTO tipo = new TipoUsoCalderaDTO();

                tipo.Id = (Decimal)DataBaseUtils.GetDataReaderColumnValue(dr, "ID_TIPO_USO_CALDERA");
                tipo.Descripcion = (String)DataBaseUtils.GetDataReaderColumnValue(dr, "DESC_TIPO_USO_CALDERA");

                lista.Add(tipo);
            }
            return lista;
        }
    }
}
