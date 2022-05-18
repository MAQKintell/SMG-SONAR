using System;
using System.Runtime.Serialization;
using System.Web.UI.WebControls;

namespace Iberdrola.SMG.DAL.DTO
{
    /// <summary>
    /// Atributos y Propiedades de la entidad Documento
    /// </summary>
    public partial class DocumentoDTO : BaseDTO
    {
        #region propiedades
        
        public FileUpload File
        { get; set; }
        
        #endregion
    }
}