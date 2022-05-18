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
    /// Descripción breve de Calderas
    /// </summary>
    public class Equipamientos
    {
        public Equipamientos()
        {
        }

        public static IDataReader ObtenerTipoPotencia()
        {
            EquipamientoDB equipamientoDB = new EquipamientoDB();
            return equipamientoDB.ObtenerTipoPotencia();
        }

        public static List<EquipamientoDTO> ObtenerEquipamientos(String strCodContrato,Int16 idIdioma)
        {
            EquipamientoDB equipamientoDB = new EquipamientoDB();
            return equipamientoDB.ObtenerEquipamientos(strCodContrato, idIdioma);
        }


        public static void ActualizarEquipamientos(List<EquipamientoDTO> listaEquipamientosActualizar, List<EquipamientoDTO> listaEquipamientosEliminar)
        {
            EquipamientoDB equipamientoDB = new EquipamientoDB();
            
            // Actualizar los equipamientos
            if (listaEquipamientosActualizar != null)
            {
                equipamientoDB.ActualizarEquipamientos(listaEquipamientosActualizar);
            }

            // Eliminar los equipamientos
            if (listaEquipamientosEliminar != null)
            {
                foreach (EquipamientoDTO equipamiento in listaEquipamientosEliminar)
                {
                    // elimina el equipamiento
                    equipamientoDB.EliminarEquipamiento(equipamiento);
                }
            }
        }
    }
}