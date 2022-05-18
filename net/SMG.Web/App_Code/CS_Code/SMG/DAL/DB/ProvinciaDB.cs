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
    public class ProvinciaDB
    {
        public ProvinciaDB()
	    {

	    }

        public List<ProvinciaDTO> ObtenerProvincias()
        {
            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
            List<ProvinciaDTO> lista = new List<ProvinciaDTO>();
            IDataReader dr = db.RunProcDataReader("SP_GET_PROVINCIAS");

            while (dr.Read())
            {
                ProvinciaDTO prov = new ProvinciaDTO();

                if (!dr["COD_PROVINCIA"].Equals(DBNull.Value))
                {
                    prov.CodProvincia = (String)dr["COD_PROVINCIA"];
                }

                if (!dr["NOMBRE"].Equals(DBNull.Value))
                {
                    prov.Nombre = (String)dr["NOMBRE"];
                }
                lista.Add(prov);
            }
            return lista;
        }
    }
}
