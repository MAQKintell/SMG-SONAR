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
    /// Descripción breve de TipoEquipamientoDB
    /// </summary>
    public class TipoEquipamientoDB
    {
        public TipoEquipamientoDB()
	    {

	    }

        public List<TipoEquipamientoDTO> ObtenerTiposEquipamiento(Int16 idIdioma)
        {

            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
            List<TipoEquipamientoDTO> lista = new List<TipoEquipamientoDTO>();

            string[] ParamsName = new string[1];
            DbType[] ParamsType = new DbType[1];
            object[] ParamsValue = new object[1];

            ParamsName[0] = "@idIDIOMA";
            ParamsValue[0] = idIdioma;
            ParamsType[0] = DbType.Int16;

            IDataReader dr = db.RunProcDataReader("SP_GET_TIPO_EQUIPAMIENTO", ParamsName, ParamsType, ParamsValue);
            

            while (dr.Read())
            {
                TipoEquipamientoDTO tipo = new TipoEquipamientoDTO();

                tipo.Id = (Decimal)DataBaseUtils.GetDataReaderColumnValue(dr, "ID_TIPO_EQUIPAMIENTO");
                tipo.Descripcion = (String)DataBaseUtils.GetDataReaderColumnValue(dr, "DESC_TIPO_EQUIPAMIENTO");

                lista.Add(tipo);
            }
            return lista;
        }
    }
}
