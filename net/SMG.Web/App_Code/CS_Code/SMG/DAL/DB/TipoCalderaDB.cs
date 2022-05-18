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
    /// Descripción breve de TipoCalderaDB
    /// </summary>
    public class TipoCalderaDB
    {
        public TipoCalderaDB()
	    {

	    }

        public List<TipoCalderaDTO> ObtenerTiposCaldera(Int16 IdIdioma)
        {

            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
            List<TipoCalderaDTO> lista = new List<TipoCalderaDTO>();


            String[] aNombres = new String[1] { "@idIDIOMA" };
            DbType[] aTipos = new DbType[1] { DbType.Int16 };
            Object[] aValores = new Object[1] { IdIdioma };

            IDataReader dr = db.RunProcDataReader("SP_GET_TIPO_CALDERA", aNombres, aTipos, aValores);
            

            while (dr.Read())
            {
                TipoCalderaDTO tipo = new TipoCalderaDTO();

                tipo.Id = (Decimal)DataBaseUtils.GetDataReaderColumnValue(dr, "ID_TIPO_CALDERA");
                tipo.Descripcion = (String)DataBaseUtils.GetDataReaderColumnValue(dr, "DESC_TIPO_CALDERA");

                lista.Add(tipo);
            }
            return lista;
        }

        public List<TipoCalderaDTO> ObtenerTiposCalderaTicketCombustion(Int16 IdIdioma, string tipoPeticion)
        {
            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
            List<TipoCalderaDTO> lista = new List<TipoCalderaDTO>();

            String[] aNombres = new String[2];
            DbType[] aTipos = new DbType[2];
            Object[] aValores = new Object[2];

            aNombres[0] = "@pID_IDIOMA";
            aTipos[0] = DbType.Int16;
            aValores[0] = IdIdioma;

            aNombres[1] = "@pTIPO_PETICION";
            aTipos[1] = DbType.String;
            aValores[1] = tipoPeticion;

            IDataReader dr = db.RunProcDataReader("SP_GET_TIPO_CALDERA_TICKET_COMBUSTION", aNombres, aTipos, aValores);

            while (dr.Read())
            {
                TipoCalderaDTO tipo = new TipoCalderaDTO();

                tipo.Id = (Decimal)DataBaseUtils.GetDataReaderColumnValue(dr, "ID_TIPO_CALDERA");
                tipo.Descripcion = (String)DataBaseUtils.GetDataReaderColumnValue(dr, "DESC_TIPO_CALDERA");

                lista.Add(tipo);
            }
            return lista;
        }
    }
}
