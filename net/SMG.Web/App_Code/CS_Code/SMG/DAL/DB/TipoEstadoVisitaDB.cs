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
    public class TipoEstadoVisitaDB
    {
        public TipoEstadoVisitaDB()
	    {

	    }

        public List<TipoEstadoVisitaDTO> ObtenerTiposEstadoVisita(Int16 idIdioma)
        {

            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
            string[] ParamsName = new string[1];
            DbType[] ParamsType = new DbType[1];
            object[] ParamsValue = new object[1];

            ParamsName[0] = "@idIDIOMA";
            ParamsValue[0] = idIdioma;
            ParamsType[0] = DbType.Int16;

            List<TipoEstadoVisitaDTO> lista = new List<TipoEstadoVisitaDTO>();

            IDataReader dr = db.RunProcDataReader("SP_GET_TIPO_ESTADO_VISITA", ParamsName, ParamsType, ParamsValue);
            

            while (dr.Read())
            {
                TipoEstadoVisitaDTO tipo = new TipoEstadoVisitaDTO();

                tipo.Codigo = (String)DataBaseUtils.GetDataReaderColumnValue(dr, "COD_ESTADO");
                tipo.Descripcion = (String)DataBaseUtils.GetDataReaderColumnValue(dr, "DES_ESTADO");

                lista.Add(tipo);
            }
            return lista;
        }
    }
}
