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
    /// Descripción breve de TipoTiempoManoDeObraDB
    /// </summary>
    public class TipoTiempoManoDeObraDB
    {
        public TipoTiempoManoDeObraDB()
	    {

	    }

        public List<TipoTiempoManoDeObraDTO> ObtenerTiposTiempoManoDeObra(Int16 idIdioma)
        {

            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
            List<TipoTiempoManoDeObraDTO> lista = new List<TipoTiempoManoDeObraDTO>();
            string[] ParamsName = new string[1];
            DbType[] ParamsType = new DbType[1];
            object[] ParamsValue = new object[1];

            ParamsName[0] = "@idIDIOMA";
            ParamsValue[0] = idIdioma;
            ParamsType[0] = DbType.Int16;

            IDataReader dr = db.RunProcDataReader("SP_GET_TIPO_TIEMPO_MANO_OBRA", ParamsName, ParamsType, ParamsValue);
            

            while (dr.Read())
            {
                TipoTiempoManoDeObraDTO tipo = new TipoTiempoManoDeObraDTO();

                tipo.Id = (Decimal)DataBaseUtils.GetDataReaderColumnValue(dr, "ID_TIPO_TIEMPO_MANO_OBRA");
                tipo.Descripcion = (String)DataBaseUtils.GetDataReaderColumnValue(dr, "DESC_TIPO_TIEMPO_MANO_OBRA");

                lista.Add(tipo);
            }
            return lista;
        }
    }
}
