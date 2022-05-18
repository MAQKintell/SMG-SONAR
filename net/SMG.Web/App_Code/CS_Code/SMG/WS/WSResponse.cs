using Iberdrola.Commons.Utils;
using System.Collections.Generic;
using System.Globalization;
namespace Iberdrola.SMG.WS
{

    public class WSError
    {
        public int Codigo { get; set;}
        public string Descripcion { get; set; }
    }

    public class WSAviso
    {
        public int Codigo { get; set; }
        public string Descripcion { get; set; }
    }

    /// <summary>
    /// Clase que contiene la respuesta a una llamada de un WS
    /// </summary>
    public class WSResponse
    {
        public List<WSError> ListaErrores = new List<WSError>();
        public List<WSAviso> ListaAvisos = new List<WSAviso>();

        public bool TieneError
        {
            get
            {
                return this.ListaErrores.Count > 0;
            }
            set { }
        }

        public bool TieneAviso
        {
            get
            {
                return this.ListaAvisos.Count > 0;
            }
            set { }
        }

        private bool Castellano
        {
            get;
            set;
        }

        //20210128 BGN ADD BEG R#28584 - Envío Contrato GC a Edatalia para Firma Digital
        public void AddError(UpdateDocumentSetWSError error)
        {
            WSError lista = new WSError();
            lista.Codigo = (int)error;
            lista.Descripcion = error.ToString();

            this.ListaErrores.Add(lista);
        }
        //20210128 BGN ADD END R#28584 - Envío Contrato GC a Edatalia para Firma Digital

        public void AddError(CommonWSError error)
        {
            WSError lista=new WSError();
            lista.Codigo= (int)error;
            lista.Descripcion=error.ToString();
            
            this.ListaErrores.Add(lista);
        }

        public void AddError(CierreVisitaWSError error)
        {
            WSError lista = new WSError();
            lista.Codigo = (int)error;
            lista.Descripcion = error.ToString();
            
            this.ListaErrores.Add(lista);
        }

        public void AddError(TicketCombustionError error)
        {
            WSError lista = new WSError();
            lista.Codigo = (int)error;
            //lista.Descripcion = error.ToString();
            lista.Descripcion = StringEnum.GetStringValue(error);

            this.ListaErrores.Add(lista);
        }

        public void AddAviso(TicketCombustionAviso aviso)
        {
            WSAviso lista = new WSAviso();
            lista.Codigo = (int)aviso;
            lista.Descripcion = StringEnum.GetStringValue(aviso);

            this.ListaAvisos.Add(lista);
        }

        public void AddRespuestaCustom(string respuesta)
        {
            WSAviso lista = new WSAviso();
            lista.Codigo = 0;
            lista.Descripcion = respuesta;

            this.ListaAvisos.Add(lista);
        }

        public void AddErrorAperturaSolicitud(AperturaSolicitudWSError error)
        {
            WSError lista = new WSError();
            lista.Codigo = (int)error;
            lista.Descripcion = error.ToString();

            this.ListaErrores.Add(lista);
        }

        public void AddErrorSigecas(SigecasWSError error)
        {
            WSError lista = new WSError();
            lista.Codigo = (int)error;
            lista.Descripcion = error.ToString();

            this.ListaErrores.Add(lista);
        }

        public void AddErrorContratoVigente(ContratoVigenteWSError error)
        {
            WSError lista = new WSError();
            lista.Codigo = (int)error;
            lista.Descripcion = error.ToString();

            this.ListaErrores.Add(lista);
        }

        public void AddErrorDatosVisita(DatosVisitaWSError error)
        {
            WSError lista = new WSError();
            lista.Codigo = (int)error;
            lista.Descripcion = error.ToString();

            this.ListaErrores.Add(lista);
        }


        public void AddErrorDatosSolicitud(DatosSolicitudWSError error)
        {
            WSError lista = new WSError();
            lista.Codigo = (int)error;
            lista.Descripcion = error.ToString();

            this.ListaErrores.Add(lista);
        }


        public void AddErrorCierreSolicitud(CierreSolicitudWSError error)
        {
            WSError lista = new WSError();
            lista.Codigo = (int)error;
            lista.Descripcion = error.ToString();

            this.ListaErrores.Add(lista);
        }
//        public void AddError(int codigo)
//        {
//            CommonWSError error = new CommonWSError();
//            //error.Codigo = codigo;

////            error.Descripcion = descripcion;
//            this._listaErrores.Add(error);
//        }
        
        public string RecuperarTextoMensaje()//CultureInfo culture)
        {
            // TODO: GGB recuperar el texto del error en el idioma apropiado
            //return "TODO: PENDIENTE RECUPERAR EL ERROR DE LA BBDD";
            string mensaje = "";


            foreach (WSError error in this.ListaErrores)
            {
                if (this.Castellano)
                {
                    mensaje += error.Descripcion.ToString() + "_" + error.Codigo.ToString();
                }
                else
                {
                    int codigoIdioma=(int)Idioma.Ingles;
                    CierreVisitaWSError errorOtroIdioma = (CierreVisitaWSError)System.Enum.Parse(typeof(CierreVisitaWSError), (codigoIdioma.ToString() + error.Codigo.ToString().Substring(1, error.Codigo.ToString().Length - 1).ToString()).ToString());
                    mensaje += errorOtroIdioma.ToString() + "_" + (int)errorOtroIdioma;
                }
            }
            return mensaje;
        }

        public string RecuperarTextoAvisoMensaje()//CultureInfo culture)
        {
            // TODO: GGB recuperar el texto del error en el idioma apropiado
            //return "TODO: PENDIENTE RECUPERAR EL ERROR DE LA BBDD";
            string mensaje = "";


            foreach (WSAviso aviso in this.ListaAvisos)
            {
                if (this.Castellano)
                {
                    mensaje += aviso.Descripcion.ToString() + "_" + aviso.Codigo.ToString();
                }
                else
                {
                    int codigoIdioma = (int)Idioma.Ingles;
                    TicketCombustionAviso AvisoOtroIdioma = (TicketCombustionAviso)System.Enum.Parse(typeof(TicketCombustionAviso), (codigoIdioma.ToString() + aviso.Codigo.ToString().Substring(1, aviso.Codigo.ToString().Length - 1).ToString()).ToString());
                    mensaje += AvisoOtroIdioma.ToString() + "_" + (int)AvisoOtroIdioma;
                }
            }
            return mensaje;
        }

        public override string ToString()
        {
            return RecuperarTextoMensaje();
        }

       
    }
}