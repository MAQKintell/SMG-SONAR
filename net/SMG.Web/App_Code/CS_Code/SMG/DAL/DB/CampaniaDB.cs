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
    public class CampaniaDB
    {
        public CampaniaDB()
	    {

	    }

        public List<CampaniaDTO> ObtenerCampanias()
        {

            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
            List<CampaniaDTO> lista = new List<CampaniaDTO>();

            IDataReader dr = db.RunProcDataReader("SP_GET_TIPO_CAMPANIA");
            

            while (dr.Read())
            {
                CampaniaDTO campania = new CampaniaDTO();

                campania.Observaciones = (String)DataBaseUtils.GetDataReaderColumnValue(dr, "OBSERVACIONES");
                campania.Campania = (int)DataBaseUtils.GetDataReaderColumnValue(dr, "CAMPANIA");

                lista.Add(campania);
            }
            return lista;
        }
    }
}
