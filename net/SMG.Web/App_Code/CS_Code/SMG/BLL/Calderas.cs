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
using Iberdrola.SMG.DAL.DTO;
using Iberdrola.SMG.DAL.DB;

namespace Iberdrola.SMG.BLL
{
    /// <summary>
    /// Descripción breve de Calderas
    /// </summary>
    public class Calderas
    {
        public Calderas()
        {
        }


        public static Boolean EsCalderaValida(CalderaDTO caldera)
        {
            if (caldera.DecripcionMarcaCaldera == null || caldera.DecripcionMarcaCaldera.Length == 0)
            {
                return false;
            }

            if (caldera.CodigoContrato == null || caldera.CodigoContrato.Length == 0)
            {
                return false;
            }

            if (!caldera.IdTipoCaldera.HasValue)
            {
                return false;
            }

            if (caldera.Potencia == null)
            {
                return false;
            }

            if (!caldera.Uso.HasValue)
            {
                return false;
            }

            return true;
        }

        public static CalderaDTO ObtenerCaldera(String strCodContrato)
        {
            CalderaDB calderaDB = new CalderaDB();
            CalderaDTO caldera = new CalderaDTO();
            caldera.CodigoContrato = strCodContrato;
            return calderaDB.ObtenerCaldera(caldera);
        }

        public static DataTable dtObtenerCaldera(string codContrato)
        {
            CalderaDB calderaDB = new CalderaDB();
            return calderaDB.dtObtenerCaldera(codContrato);
        }

        public static void EliminarCaldera(String strCodContrato)
        {
            CalderaDB calderaDB = new CalderaDB();
            CalderaDTO caldera = new CalderaDTO();
            caldera.CodigoContrato = strCodContrato;
            calderaDB.EliminarCaldera(caldera);
        }

        public static void ActualizarInsertarCaldera(CalderaDTO caldera)
        {
            CalderaDB calderaDB = new CalderaDB();
            calderaDB.ActualizarInsertarCaldera(caldera);
        }
    }
}