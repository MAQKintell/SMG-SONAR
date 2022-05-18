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
    public class TipoMarcaCalderaDB
    {
        public TipoMarcaCalderaDB()
	    {

	    }

        public List<TipoMarcaCalderaDTO> ObtenerTiposMarcaCaldera(Decimal? intIdMarcaCaldera)
        {

            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
            List<TipoMarcaCalderaDTO> lista = new List<TipoMarcaCalderaDTO>();
            String[] aNombres = new String[1] { "@ID_MARCA_CALDERA" };
            Object[] aValores = new Object[1] { intIdMarcaCaldera == null ? 0 : intIdMarcaCaldera };
            DbType[] aTipos = new DbType[1] { DbType.Decimal};


            IDataReader dr = db.RunProcDataReader("SP_GET_TIPO_MARCA_CALDERA", aNombres, aTipos, aValores );
            
            while (dr.Read())
            {
                TipoMarcaCalderaDTO tipo = new TipoMarcaCalderaDTO();

                tipo.Id = (Decimal)DataBaseUtils.GetDataReaderColumnValue(dr, "ID_MARCA_CALDERA");
                tipo.Descripcion = (String)DataBaseUtils.GetDataReaderColumnValue(dr, "DESC_MARCA_CALDERA");

                lista.Add(tipo);
            }
            return lista;
        }
    }
}
