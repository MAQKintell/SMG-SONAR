//*******************************************************************
// Copyright © 2015 Iberdrola, S.A. Todos los derechos reservados.
//*******************************************************************


using Iberdrola.Commons.BaseClasses;


namespace Iberdrola.Commons.Messages.DAL.DTO
{
    /// <summary>
    /// Clase para los mensajes.
    /// </summary>
    public class MessageDTO : BaseDTO
    {
        /// <summary>
        /// Identificador del mensaje.
        /// </summary>
        private int _idMessage;

        /// <summary>
        /// Tipo de mensaje.
        /// </summary>
        private int _idType;

        /// <summary>
        /// Texto del mensaje.
        /// </summary>
        private string _descMessage;
        

        /// <summary>
        /// Identificador del mensage.
        /// </summary>
        public int IdMessage
        {
            get { return this._idMessage; }
            set { this._idMessage = value; }
        }

        /// <summary>
        /// Tipo de mensage.
        /// </summary>
        public int IdType
        {
            get { return this._idType; }
            set { this._idType = value; }
        }

        /// <summary>
        /// Contenido del mensaje.
        /// </summary>
        public string DescMessage
        {
            get { return this._descMessage; }
            set { this._descMessage = value; }
        }
    }
}