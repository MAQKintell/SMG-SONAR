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
    /// Descripción breve de TipoEstadoVisitaDB
    /// </summary>
    public class TipoCodigosAveriaGasConfortDB
    {
        public TipoCodigosAveriaGasConfortDB()
	    {

	    }

        public List<TipoCodigosAveriaGasConfortDTO> ObtenerCodigosAveriaGasConfort()
        {

            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
            List<TipoCodigosAveriaGasConfortDTO> lista = new List<TipoCodigosAveriaGasConfortDTO>();

            IDataReader dr = db.RunProcDataReader("MTOGASBD_GetCodigoAveriasGasConfort");
            

            while (dr.Read())
            {
                TipoCodigosAveriaGasConfortDTO tipo = new TipoCodigosAveriaGasConfortDTO();

                tipo.Codigo = DataBaseUtils.GetDataReaderColumnValue(dr, "COD_CODIGOAVERIAGASCONFORT").ToString();
                tipo.Descripcion = (String)DataBaseUtils.GetDataReaderColumnValue(dr, "DESC_CODIGOAVERIAGASCONFORT");

                lista.Add(tipo);
            }
            return lista;
        }
    }
}
