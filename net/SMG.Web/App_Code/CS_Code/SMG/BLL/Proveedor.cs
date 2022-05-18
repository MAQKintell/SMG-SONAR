using Iberdrola.SMG.DAL.DB;
using System.Data;

namespace Iberdrola.SMG.BLL
{
    /// <summary>
    /// Descripción breve de Proveedor
    /// </summary>
    public partial class Proveedor
    {
        public IDataReader GetProveedorAltaReclamacion(string pais)
        {
            ProveedorDB proveedorDB = new ProveedorDB();
            return proveedorDB.GetProveedorAltaReclamacion(pais);
        }
    }
}