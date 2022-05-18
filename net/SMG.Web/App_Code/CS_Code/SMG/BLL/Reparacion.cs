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
using System.Collections.Generic;

namespace Iberdrola.SMG.BLL
{
    /// <summary>
    /// Descripción breve de Reparacion
    /// </summary>
    public class Reparacion
    {
        public Reparacion()
        {

        }

        public static ReparacionDTO ObtenerReparacion(Decimal ReparacionID)
        {
            return ReparacionDB.ObtenerReparacion(ReparacionID);
        }

        public static void ActualizarReparacion(ReparacionDTO reparacionDTO)
        {
            ReparacionDB.ActualizarReparacion(reparacionDTO);
        }

        public static void BorrarReparacion(ReparacionDTO reparacionDTO)
        {
            ReparacionDB.BorrarReparacion(reparacionDTO);
        }

        public static void InsertarReparacion(ReparacionDTO reparacionDTO)
        {
            ReparacionDB.InsertarReparacion(reparacionDTO);
        }
    }
}
