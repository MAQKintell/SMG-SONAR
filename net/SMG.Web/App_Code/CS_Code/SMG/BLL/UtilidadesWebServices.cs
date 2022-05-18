using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Iberdrola.Commons.Services.WebServices;
using Iberdrola.SMG.DAL.DTO;
using Iberdrola.Commons.Security;
using System.Xml;
using System.Data;
using Iberdrola.SMG.DAL.DB;
using System.Net.Security;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.IO;
using System.Xml.Serialization;

namespace Iberdrola.SMG.BLL
{
    /// <summary>
    /// Descripción breve de UtilidadesWebServices
    /// </summary>
    public class UtilidadesWebServices
    {
        public UtilidadesWebServices()
        {
        }

        public String llamadaInteraccionAperturaSolicitud(string codContrato, string efv, string codCliente, string comentario, string tipo, string subTipo, string codUsuario,string sociedad)
        {
            String resultado = String.Empty;
            try
            { 
                WebserviceDTO wsDto = new WebserviceDTO();
                //wsDto.NombreWebservice = "Interacciones Alta";
                wsDto.TipoWebservice = "INT";
                wsDto.ActivoWebservice = true;
                List<WebserviceDTO> lWebService = Webservice.Buscar(wsDto);
                if (lWebService.Count > 0)
                {
                    wsDto = lWebService[0];
                    SoapHelper sh = CargarSoapHelper(wsDto);

                    XmlDocument xml = ObtenerPlantillaCuerpo(wsDto.PlantillaWebservice);
                    XmlNamespaceManager namespaces = new XmlNamespaceManager(xml.NameTable);
                
                    XmlNode nClaveEntidad = xml.SelectSingleNode("//codClaveEntidad", namespaces);
                    nClaveEntidad.InnerText = codContrato;
                    XmlNode nClaveEntidadAdic = xml.SelectSingleNode("//codClaEntAdicio", namespaces);
                    nClaveEntidadAdic.InnerText = efv;
                    XmlNode nCodCliente = xml.SelectSingleNode("//codCliente", namespaces);
                    nCodCliente.InnerText = codCliente;
                    XmlNode nComentario = xml.SelectSingleNode("//comentario", namespaces);
                    nComentario.InnerText = comentario;
                    XmlNode nFechaInterac = xml.SelectSingleNode("//fechaHoraInteraccion", namespaces);
                    nFechaInterac.InnerText = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
                    XmlNode nSociedad = xml.SelectSingleNode("//codSociedad", namespaces);
                    nSociedad.InnerText = sociedad;

                    //20210302 BGN BEG Bug en campos tipCanal y codIdentCanal al machacar el valor de codUsuario por OVC001
                    bool usuAPP = false;
                    if (codUsuario.Equals("WS_PYS"))
                    {
                        usuAPP = true;
                    }
                    //20210302 BGN END Bug en campos tipCanal y codIdentCanal

                    //20201221 BGN BEG R#21644 Activar WS Interacciones
                    ConfiguracionDTO cfgUsuarios = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.WS_INTERACCIONES_USUARIOS);
                    if (cfgUsuarios != null && !string.IsNullOrEmpty(cfgUsuarios.Valor))
                    {
                        string[] usuariosOVC = cfgUsuarios.Valor.ToUpper().Split(';');
                        if (usuariosOVC.Contains(codUsuario))
                        {
                            codUsuario = "OVC001";
                        }
                    }
                    //20201221 BGN END R#21644 Activar WS Interacciones
                    XmlNode nCodUsuario = xml.SelectSingleNode("//codUsuarioInteraccion", namespaces);
                    nCodUsuario.InnerText = codUsuario;
                                
                    DataTable dtTiposInteraccion = null;
                    DatosInteraccionDB db = new DatosInteraccionDB();
                    dtTiposInteraccion = db.ObtenerTiposInteraccion(tipo, subTipo);
                    String codTipAccCli = String.Empty;
                    String codSubtipAccCli = String.Empty;
                    if (dtTiposInteraccion.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtTiposInteraccion.Rows.Count; i++)
                        {
                            codTipAccCli = dtTiposInteraccion.Rows[i].ItemArray[1].ToString();
                            codSubtipAccCli = dtTiposInteraccion.Rows[i].ItemArray[4].ToString();
                        }

                        XmlNode nCodTip = xml.SelectSingleNode("//codTipAccCli", namespaces);
                        nCodTip.InnerText = codTipAccCli;
                        XmlNode nCodSubTip = xml.SelectSingleNode("//codSubtipAccCli", namespaces);
                        nCodSubTip.InnerText = codSubtipAccCli;

                        XmlNode nTipCanal = xml.SelectSingleNode("//tipCanal", namespaces);
                        XmlNode nCodIdentCanal = xml.SelectSingleNode("//codIdentCanal", namespaces);
                        //20210302 BGN Bug en campos tipCanal y codIdentCanal al machacar el valor de codUsuario por OVC001
                        if (usuAPP)
                        {
                            nTipCanal.InnerText = "AP";
                            nCodIdentCanal.InnerText = "AP";
                        }
                        else
                        {
                            UsuarioDTO usuDto = Usuarios.ObtenerUsuario(codUsuario);
                            if (usuDto != null && usuDto.Id_Perfil.HasValue)
                            {
                                if (Usuarios.EsTelefono(int.Parse(usuDto.Id_Perfil.Value.ToString())))
                                {
                                    nTipCanal.InnerText = "TF";
                                    nCodIdentCanal.InnerText = "TD";
                                }
                            }                            
                        }

                        sh.XmlContent = xml.InnerXml;

                        int idTraza = GuardarTrazaLlamadaWS("altaInteraccionAccionCallback", sh.XmlContent);
                        ConfiguracionDTO confActivoTLS12 = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.ACTIVOTLS12INTERACCION);
                        Boolean ActivoTLS12Interaccion = false;
                        if (confActivoTLS12 != null && !string.IsNullOrEmpty(confActivoTLS12.Valor) && Boolean.Parse(confActivoTLS12.Valor))
                        {
                            ActivoTLS12Interaccion = Boolean.Parse(confActivoTLS12.Valor);
                        }

                        string docResult = sh.CallWebService(ActivoTLS12Interaccion);
                        GuardarTrazaResultadoLlamadaWS(idTraza, docResult);

                        if (docResult.Contains("StackTrace") || docResult.Contains("faultcode"))
                        {
                            // Error;
                            resultado = docResult;
                        }
                    }
                }
                //else
                //{
                //    //No se realiza la llamada al WS porque no se ha encontrado
                //    return false;
                //}     
            }
            catch (Exception ex)
            {
                //20200123 BGN MOD BEG [R#21821]: Parametrizar envio mails por la web
                //MandarMailError("Se ha producido un error al llamar al WS InteraccionAperturaSolicitud. CodContrato: " + codContrato + ". Message: " + ex.Message + " StackTrace: " + ex.StackTrace, "[SMG] Error llamada WS InteraccionAperturaSolicitud");
                UtilidadesMail.EnviarMensajeError(" Error llamada WS InteraccionAperturaSolicitud", "Se ha producido un error al llamar al WS InteraccionAperturaSolicitud. CodContrato: " + codContrato + ". Message: " + ex.Message + " StackTrace: " + ex.StackTrace);
                //20200123 BGN MOD END [R#21821]: Parametrizar envio mails por la web
            }
            return resultado;            
        }

        public String llamadaWSAperturaSolicitudActivais(string codContrato, string idSolicitud, string SubtipoSolicitud, string EstadoSolicitud, string PersonaContacto, string TelefonoContacto, string Observaciones, string proveedor, string tipoOperacion, MantenimientoDTO mantenimiento, string categoriaVisita, string Sociedad)
        {
            String resultado = String.Empty;
            try
            {   
                WebserviceDTO wsDto = new WebserviceDTO();
                //wsDto.NombreWebservice = "Alta Solicitud ACT";
                wsDto.CodProveedorWebservice = proveedor;
                wsDto.TipoWebservice = "LLC";
                wsDto.ActivoWebservice = true;
                List<WebserviceDTO> lWebService = Webservice.Buscar(wsDto);
                if (lWebService.Count > 0)
                {
                    wsDto = lWebService[0];
                    SoapHelper sh = CargarSoapHelper(wsDto);

                    XmlDocument xml = ObtenerPlantillaCuerpo(wsDto.PlantillaWebservice);
                    XmlNamespaceManager namespaces = new XmlNamespaceManager(xml.NameTable);
                    XmlNode UserCabecera = xml.SelectSingleNode("//UserName", namespaces);
                    UserCabecera.InnerText = sh.User;
                    XmlNode PassCabecera = xml.SelectSingleNode("//Password", namespaces);
                    PassCabecera.InnerText = sh.Password;

                    XmlNode tipo_Operacion = xml.SelectSingleNode("//Tipo_Operacion", namespaces);
                    tipo_Operacion.InnerText = tipoOperacion; //"A";
                    XmlNode cod_Receptor = xml.SelectSingleNode("//codReceptor", namespaces);
                    cod_Receptor.InnerText = mantenimiento.COD_RECEPTOR;
                    XmlNode cod_Contrato = xml.SelectSingleNode("//codContrato", namespaces);
                    cod_Contrato.InnerText = codContrato;
                    XmlNode id_Solicitud = xml.SelectSingleNode("//idSolicitud", namespaces);
                    id_Solicitud.InnerText = idSolicitud;
                    XmlNode Subtipo_solicitud = xml.SelectSingleNode("//Subtipo_solicitud", namespaces);
                    Subtipo_solicitud.InnerText = SubtipoSolicitud;
                    XmlNode Estado_solicitud = xml.SelectSingleNode("//Estado_solicitud", namespaces);
                    Estado_solicitud.InnerText = EstadoSolicitud;
                    XmlNode Persona_Contacto = xml.SelectSingleNode("//PersonaContacto", namespaces);
                    Persona_Contacto.InnerText = PersonaContacto;
                    XmlNode Telefono = xml.SelectSingleNode("//Telefono", namespaces);
                    Telefono.InnerText = TelefonoContacto;
                    // Kintell CUR
                    XmlNode SociedadVal = xml.SelectSingleNode("//Sociedad", namespaces);
                    if (SociedadVal != null)
                    {
                        SociedadVal.InnerText = Sociedad;
                    }
                    //*****************************************************************************************
                    XmlNode nodoObservaciones = xml.SelectSingleNode("//Observaciones", namespaces);
                    nodoObservaciones.InnerXml = "<![CDATA["+Observaciones+"]]>";
                    if (!String.IsNullOrEmpty(mantenimiento.HORARIO_CONTACTO))
                    {
                        XmlNode Horario_Contacto = xml.SelectSingleNode("//HorarioContacto", namespaces);
                        Horario_Contacto.InnerText = mantenimiento.HORARIO_CONTACTO;
                    }
                    if (!String.IsNullOrEmpty(mantenimiento.NOM_TITULAR))
                    {
                        XmlNode Nom_Titular = xml.SelectSingleNode("//NomTitular", namespaces);
                        Nom_Titular.InnerText = mantenimiento.NOM_TITULAR.Trim();
                    }
                    if (!String.IsNullOrEmpty(mantenimiento.APELLIDO1))
                    {
                        XmlNode Apellido1 = xml.SelectSingleNode("//Apellido1", namespaces);
                        Apellido1.InnerText = mantenimiento.APELLIDO1.Trim();
                    }
                    if (!String.IsNullOrEmpty(mantenimiento.APELLIDO2))
                    {
                        XmlNode Apellido2 = xml.SelectSingleNode("//Apellido2", namespaces);
                        Apellido2.InnerText = mantenimiento.APELLIDO2.Trim();
                    }
                    if (!String.IsNullOrEmpty(mantenimiento.COD_PROVINCIA))
                    {
                        XmlNode Provincia_Nombre = xml.SelectSingleNode("//ProvinciaNombre", namespaces);
                        Provincia_Nombre.InnerText = mantenimiento.COD_PROVINCIA.Trim();
                    }
                    if (!String.IsNullOrEmpty(mantenimiento.COD_POBLACION))
                    {
                        XmlNode Poblacion_Nombre = xml.SelectSingleNode("//PoblacionNombre", namespaces);
                        Poblacion_Nombre.InnerText = mantenimiento.COD_POBLACION.Trim();
                    }
                    if (!String.IsNullOrEmpty(mantenimiento.TIP_VIA_PUBLICA))
                    {
                        XmlNode Tip_Via_Publica = xml.SelectSingleNode("//TipViaPublica", namespaces);
                        Tip_Via_Publica.InnerText = mantenimiento.TIP_VIA_PUBLICA.Trim();
                    }
                    if (!String.IsNullOrEmpty(mantenimiento.NOM_CALLE))
                    {
                        XmlNode Nom_Calle = xml.SelectSingleNode("//NomCalle", namespaces);
                        Nom_Calle.InnerText = mantenimiento.NOM_CALLE.Trim();
                    }
                    if (!String.IsNullOrEmpty(mantenimiento.COD_FINCA))
                    {
                        XmlNode COD_FINCA = xml.SelectSingleNode("//CodFinca", namespaces);
                        COD_FINCA.InnerText = mantenimiento.COD_FINCA.Trim();
                    }
                    if (!String.IsNullOrEmpty(mantenimiento.COD_PORTAL))
                    {
                        XmlNode Cod_Portal = xml.SelectSingleNode("//CodPortal", namespaces);
                        Cod_Portal.InnerText = mantenimiento.COD_PORTAL.Trim();
                    }
                    if (!String.IsNullOrEmpty(mantenimiento.TIP_ESCALERA))
                    {
                        XmlNode Tip_Escalera = xml.SelectSingleNode("//TipEscalera", namespaces);
                        Tip_Escalera.InnerText = mantenimiento.TIP_ESCALERA.Trim();
                    }
                    if (!String.IsNullOrEmpty(mantenimiento.TIP_PISO))
                    {
                        XmlNode Tip_Piso = xml.SelectSingleNode("//TipPiso", namespaces);
                        Tip_Piso.InnerText = mantenimiento.TIP_PISO.Trim();
                    }
                    if (!String.IsNullOrEmpty(mantenimiento.TIP_MANO))
                    {
                        XmlNode Tip_Mano = xml.SelectSingleNode("//TipMano", namespaces);
                        Tip_Mano.InnerText = mantenimiento.TIP_MANO.Trim();
                    }
                    if (!String.IsNullOrEmpty(mantenimiento.COD_POSTAL))
                    {
                        XmlNode Cod_Postal = xml.SelectSingleNode("//CodPostal", namespaces);
                        Cod_Postal.InnerText = mantenimiento.COD_POSTAL.Trim();
                    }
                    if (!String.IsNullOrEmpty(mantenimiento.TELEFONO1))
                    {
                        XmlNode Telefono_Cliente1 = xml.SelectSingleNode("//TelefonoCliente1", namespaces);
                        Telefono_Cliente1.InnerText = mantenimiento.TELEFONO1.Trim();
                    }
                    if (!String.IsNullOrEmpty(mantenimiento.TELEFONO2))
                    {
                        XmlNode Telefono_Cliente2 = xml.SelectSingleNode("//TelefonoCliente2", namespaces);
                        Telefono_Cliente2.InnerText = mantenimiento.TELEFONO2.Trim();
                    }
                    if (!String.IsNullOrEmpty(mantenimiento.NUM_TEL_PS))
                    {
                        XmlNode Num_Tel_PS = xml.SelectSingleNode("//NumTelPS", namespaces);
                        Num_Tel_PS.InnerText = mantenimiento.NUM_TEL_PS.Trim();
                    }
                    if (!String.IsNullOrEmpty(mantenimiento.NUM_MOVIL))
                    {
                        XmlNode Num_Movil = xml.SelectSingleNode("//NumMovil", namespaces);
                        Num_Movil.InnerText = mantenimiento.NUM_MOVIL.Trim();
                    }
                    if (!String.IsNullOrEmpty(mantenimiento.DNI))
                    {
                        XmlNode DNI = xml.SelectSingleNode("//DNI", namespaces);
                        DNI.InnerText = mantenimiento.DNI.Trim();
                    }
                    if (mantenimiento.FEC_ALTA_SERVICIO.HasValue)
                    {
                        XmlNode Fecha_Alta = xml.SelectSingleNode("//FechaAlta", namespaces);
                        Fecha_Alta.InnerText = mantenimiento.FEC_ALTA_SERVICIO.Value.ToString("dd-MM-yyyy HH:mm:ss");
                    }
                    if (!String.IsNullOrEmpty(categoriaVisita))
                    {
                        XmlNode Categoria_Visita = xml.SelectSingleNode("//CategoriaVisita", namespaces);
                        Categoria_Visita.InnerText = categoriaVisita.Trim();
                    }
                    if (mantenimiento.CONTADOR_AVERIAS!=null)
                    {
                        XmlNode Contador_Averias_Resueltas = xml.SelectSingleNode("//ContadorAveriasResueltas", namespaces);
                        Contador_Averias_Resueltas.InnerText = mantenimiento.CONTADOR_AVERIAS.ToString();
                    }

                    sh.XmlContent = xml.InnerXml;

                    int idTraza = GuardarTrazaLlamadaWS("SetInfoSolAbiertaICI_ACT", sh.XmlContent);
                    ConfiguracionDTO confActivoTLS12 = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.ACTIVOTLS12ICI_ACT);
                    Boolean ActivoTLS12Ici_Act = false;
                    if (confActivoTLS12 != null && !string.IsNullOrEmpty(confActivoTLS12.Valor) && Boolean.Parse(confActivoTLS12.Valor))
                    {
                        ActivoTLS12Ici_Act = Boolean.Parse(confActivoTLS12.Valor);
                    }
                    string docResult = sh.CallWebService(ActivoTLS12Ici_Act);
                    GuardarTrazaResultadoLlamadaWS(idTraza, docResult);

                    if (docResult.Contains("StackTrace") || docResult.Contains("faultcode") || docResult.Contains("CodigoError"))
                    {
                        // Error;
                        resultado = docResult;
                    }
                }
                //else
                //{
                //    //No se realiza la llamada al WS porque no se ha encontrado
                //    return false;
                //}
            }
            catch (Exception ex)
            {
                //20200123 BGN MOD BEG [R#21821]: Parametrizar envio mails por la web
                //MandarMailError("Se ha producido un error al llamar al WS AperturaSolicitud de ICI/ACT. CodContrato: " + codContrato + " IdSolicitud: " + idSolicitud + ". Message: " + ex.Message + " StackTrace: " + ex.StackTrace, "[SMG] Error llamada WS AperturaSolicitudProveedor");
                UtilidadesMail.EnviarMensajeError(" Error llamada WS AperturaSolicitudProveedor", "Se ha producido un error al llamar al WS AperturaSolicitud de ICI/ACT. CodContrato: " + codContrato + " IdSolicitud: " + idSolicitud + ". Message: " + ex.Message + " StackTrace: " + ex.StackTrace);
                //20200123 BGN MOD END [R#21821]: Parametrizar envio mails por la web
            }
            return resultado;
        }

        public String llamadaWSAperturaSolicitudSiel(string codContrato, string idSolicitud, string SubtipoSolicitud, string EstadoSolicitud, string PersonaContacto, string TelefonoContacto, string Observaciones, string tipoOperacion, MantenimientoDTO mantenimiento, string categoriaVisita, string Sociedad)
        {
            String resultado = String.Empty;
            try
            {   
                WebserviceDTO wsDto = new WebserviceDTO();
                //wsDto.NombreWebservice = "Alta Solicitud SIE";
                wsDto.CodProveedorWebservice = "SIE";
                wsDto.TipoWebservice = "LLC";
                wsDto.ActivoWebservice = true;
                List<WebserviceDTO> lWebService = Webservice.Buscar(wsDto);
                if (lWebService.Count > 0)
                {
                    wsDto = lWebService[0];
                    SoapHelper sh = CargarSoapHelper(wsDto);

                    XmlDocument xml = ObtenerPlantillaCuerpo(wsDto.PlantillaWebservice);
                    XmlNamespaceManager namespaces = new XmlNamespaceManager(xml.NameTable);
                    namespaces.AddNamespace("tem", "http://programa.es/SWProg2/");
                    XmlNode UserCabecera = xml.SelectSingleNode("//tem:Username", namespaces);
                    UserCabecera.InnerText = sh.User;
                    XmlNode PassCabecera = xml.SelectSingleNode("//tem:Password", namespaces);
                    PassCabecera.InnerText = sh.Password;

                    XmlNode tipo_Operacion = xml.SelectSingleNode("//tem:Tipo_Operacion", namespaces);
                    tipo_Operacion.InnerText = tipoOperacion; // "A";
                    XmlNode cod_Receptor = xml.SelectSingleNode("//tem:codReceptor", namespaces);
                    cod_Receptor.InnerText = mantenimiento.COD_RECEPTOR;
                    XmlNode cod_Contrato = xml.SelectSingleNode("//tem:codContrato", namespaces);
                    cod_Contrato.InnerText = codContrato;
                    XmlNode id_Solicitud = xml.SelectSingleNode("//tem:idSolicitud", namespaces);
                    id_Solicitud.InnerText = idSolicitud;
                    XmlNode Subtipo_solicitud = xml.SelectSingleNode("//tem:Subtipo_solicitud", namespaces);
                    Subtipo_solicitud.InnerText = SubtipoSolicitud;
                    XmlNode Estado_solicitud = xml.SelectSingleNode("//tem:Estado_solicitud", namespaces);
                    Estado_solicitud.InnerText = EstadoSolicitud;
                    XmlNode Persona_Contacto = xml.SelectSingleNode("//tem:PersonaContacto", namespaces);
                    Persona_Contacto.InnerText = PersonaContacto;
                    XmlNode Telefono = xml.SelectSingleNode("//tem:Telefono", namespaces);
                    Telefono.InnerText = TelefonoContacto;
                    // Kintell CUR
                    XmlNode SociedadVal = xml.SelectSingleNode("//tem:Sociedad", namespaces);
                    if (SociedadVal != null)
                    {
                        SociedadVal.InnerText = Sociedad;
                    }
                    //*****************************************************************************************
                    XmlNode nodoObservaciones = xml.SelectSingleNode("//tem:Observaciones", namespaces);
                    nodoObservaciones.InnerXml = "<![CDATA[" + Observaciones + "]]>";
                    if (!String.IsNullOrEmpty(mantenimiento.HORARIO_CONTACTO))
                    {
                    XmlNode Horario_Contacto = xml.SelectSingleNode("//tem:HorarioContacto", namespaces);
                        Horario_Contacto.InnerText = mantenimiento.HORARIO_CONTACTO;
                    }
                    if (!String.IsNullOrEmpty(mantenimiento.NOM_TITULAR))
                    {
                        XmlNode Nom_Titular = xml.SelectSingleNode("//tem:NomTitular", namespaces);
                        Nom_Titular.InnerText = mantenimiento.NOM_TITULAR.Trim();
                    }
                    if (!String.IsNullOrEmpty(mantenimiento.APELLIDO1))
                    {
                        XmlNode Apellido1 = xml.SelectSingleNode("//tem:Apellido1", namespaces);
                        Apellido1.InnerText = mantenimiento.APELLIDO1.Trim();
                    }
                    if (!String.IsNullOrEmpty(mantenimiento.APELLIDO2))
                    {
                        XmlNode Apellido2 = xml.SelectSingleNode("//tem:Apellido2", namespaces);
                        Apellido2.InnerText = mantenimiento.APELLIDO2.Trim();
                    }
                    if (!String.IsNullOrEmpty(mantenimiento.COD_PROVINCIA))
                    {
                        XmlNode Provincia_Nombre = xml.SelectSingleNode("//tem:ProvinciaNombre", namespaces);
                        Provincia_Nombre.InnerText = mantenimiento.COD_PROVINCIA.Trim();
                    }
                    if (!String.IsNullOrEmpty(mantenimiento.COD_POBLACION))
                    {
                        XmlNode Poblacion_Nombre = xml.SelectSingleNode("//tem:PoblacionNombre", namespaces);
                        Poblacion_Nombre.InnerText = mantenimiento.COD_POBLACION.Trim();
                    }
                    if (!String.IsNullOrEmpty(mantenimiento.TIP_VIA_PUBLICA))
                    {
                        XmlNode Tip_Via_Publica = xml.SelectSingleNode("//tem:TipViaPublica", namespaces);
                        Tip_Via_Publica.InnerText = mantenimiento.TIP_VIA_PUBLICA.Trim();
                    }
                    if (!String.IsNullOrEmpty(mantenimiento.NOM_CALLE))
                    {
                        XmlNode Nom_Calle = xml.SelectSingleNode("//tem:NomCalle", namespaces);
                        Nom_Calle.InnerText = mantenimiento.NOM_CALLE.Trim();
                    }
                    if (!String.IsNullOrEmpty(mantenimiento.COD_PORTAL))
                    {
                        XmlNode Cod_Portal = xml.SelectSingleNode("//tem:CodPortal", namespaces);
                        Cod_Portal.InnerText = mantenimiento.COD_PORTAL.Trim();
                    }
                    if (!String.IsNullOrEmpty(mantenimiento.TIP_ESCALERA))
                    {
                        XmlNode Tip_Escalera = xml.SelectSingleNode("//tem:TipEscalera", namespaces);
                        Tip_Escalera.InnerText = mantenimiento.TIP_ESCALERA.Trim();
                    }
                    if (!String.IsNullOrEmpty(mantenimiento.TIP_PISO))
                    {
                        XmlNode Tip_Piso = xml.SelectSingleNode("//tem:TipPiso", namespaces);
                        Tip_Piso.InnerText = mantenimiento.TIP_PISO.Trim();
                    }
                    if (!String.IsNullOrEmpty(mantenimiento.TIP_MANO))
                    {
                        XmlNode Tip_Mano = xml.SelectSingleNode("//tem:TipMano", namespaces);
                        Tip_Mano.InnerText = mantenimiento.TIP_MANO.Trim();
                    }
                    if (!String.IsNullOrEmpty(mantenimiento.COD_POSTAL))
                    {
                        XmlNode Cod_Postal = xml.SelectSingleNode("//tem:CodPostal", namespaces);
                        Cod_Postal.InnerText = mantenimiento.COD_POSTAL.Trim();
                    }
                    if (!String.IsNullOrEmpty(mantenimiento.TELEFONO1))
                    {
                        XmlNode Telefono_Cliente1 = xml.SelectSingleNode("//tem:TelefonoCliente1", namespaces);
                        Telefono_Cliente1.InnerText = mantenimiento.TELEFONO1.Trim();
                    }
                    if (!String.IsNullOrEmpty(mantenimiento.TELEFONO2))
                    {
                        XmlNode Telefono_Cliente2 = xml.SelectSingleNode("//tem:TelefonoCliente2", namespaces);
                        Telefono_Cliente2.InnerText = mantenimiento.TELEFONO2.Trim();
                    }
                    if (!String.IsNullOrEmpty(mantenimiento.NUM_TEL_PS))
                    {
                        XmlNode Num_Tel_PS = xml.SelectSingleNode("//tem:NumTelPS", namespaces);
                        Num_Tel_PS.InnerText = mantenimiento.NUM_TEL_PS.Trim();
                    }
                    if (!String.IsNullOrEmpty(mantenimiento.NUM_MOVIL))
                    {
                        XmlNode Num_Movil = xml.SelectSingleNode("//tem:NumMovil", namespaces);
                        Num_Movil.InnerText = mantenimiento.NUM_MOVIL.Trim();
                    }
                    if (!String.IsNullOrEmpty(mantenimiento.DNI))
                    {
                        XmlNode DNI = xml.SelectSingleNode("//tem:DNI", namespaces);
                        DNI.InnerText = mantenimiento.DNI.Trim();
                    }
                    if (mantenimiento.FEC_ALTA_SERVICIO.HasValue)
                    {
                        XmlNode Fecha_Alta = xml.SelectSingleNode("//tem:FechaAlta", namespaces);
                        Fecha_Alta.InnerText = mantenimiento.FEC_ALTA_SERVICIO.Value.ToString("dd-MM-yyyy HH:mm:ss");
                    }
                    if (!String.IsNullOrEmpty(categoriaVisita))
                    {
                        XmlNode Categoria_Visita = xml.SelectSingleNode("//tem:CategoriaVisita", namespaces);
                        Categoria_Visita.InnerText = categoriaVisita.Trim();
                    }
                    if (mantenimiento.CONTADOR_AVERIAS!=null)
                    {
                        XmlNode Contador_Averias_Resueltas = xml.SelectSingleNode("//tem:ContadorAveriasResueltas", namespaces);
                        Contador_Averias_Resueltas.InnerText = mantenimiento.CONTADOR_AVERIAS.ToString();
                    }

                    sh.XmlContent = xml.InnerXml;

                    int idTraza = GuardarTrazaLlamadaWS("SetInfoSolAbiertaSIE", sh.XmlContent);
                    ConfiguracionDTO confActivoTLS12 = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.ACTIVOTLS12SIEL);
                    Boolean ActivoTLS12Sie = false;
                    if (confActivoTLS12 != null && !string.IsNullOrEmpty(confActivoTLS12.Valor) && Boolean.Parse(confActivoTLS12.Valor))
                    {
                        ActivoTLS12Sie = Boolean.Parse(confActivoTLS12.Valor);
                    }
                    string docResult = sh.CallWebService(ActivoTLS12Sie);
                    GuardarTrazaResultadoLlamadaWS(idTraza, docResult);

                    if (docResult.Contains("StackTrace") || docResult.Contains("faultcode") || docResult.Contains("CodigoError"))
                    {
                        // Error;
                        resultado =  docResult;
                    }
                }
                //else
                //{
                //    //No se realiza la llamada al WS porque no se ha encontrado
                //    return false;
                //}
            }
            catch (Exception ex)
            {
                //20200123 BGN MOD BEG [R#21821]: Parametrizar envio mails por la web
                //MandarMailError("Se ha producido un error al llamar al WS AperturaSolicitud de SIE. CodContrato: " + codContrato + " IdSolicitud: " + idSolicitud + ". Message: " + ex.Message + " StackTrace: " + ex.StackTrace, "[SMG] Error llamada WS AperturaSolicitudProveedor");
                UtilidadesMail.EnviarMensajeError(" Error llamada WS AperturaSolicitudProveedor", "Se ha producido un error al llamar al WS AperturaSolicitud de SIE. CodContrato: " + codContrato + " IdSolicitud: " + idSolicitud + ". Message: " + ex.Message + " StackTrace: " + ex.StackTrace);
                //20200123 BGN MOD END [R#21821]: Parametrizar envio mails por la web
            }
            return resultado;
        }

        public String llamadaWSAperturaSolicitudMapfre(string codContrato, string idSolicitud, string SubtipoSolicitud, string EstadoSolicitud, string PersonaContacto, string TelefonoContacto, string Observaciones, string tipoOperacion, MantenimientoDTO mantenimiento, string urgencia, string averiasResueltas, string Sociedad)
        {
            String resultado = String.Empty;
            try
            {   
                WebserviceDTO wsDto = new WebserviceDTO();
                //wsDto.NombreWebservice = "Alta Solicitud MAP";
                wsDto.CodProveedorWebservice = "MAP";
                wsDto.TipoWebservice = "LLC";
                wsDto.ActivoWebservice = true;
                List<WebserviceDTO> lWebService = Webservice.Buscar(wsDto);
                if (lWebService.Count > 0)
                {
                    wsDto = lWebService[0];

                    ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(AllwaysGoodCertificate);
                    ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;

                    SoapHelper sh = CargarSoapHelper(wsDto);

                    XmlDocument xml = ObtenerPlantillaCuerpo(wsDto.PlantillaWebservice);
                    XmlNamespaceManager namespaces = new XmlNamespaceManager(xml.NameTable);
                    namespaces.AddNamespace("tem", "http://tempuri.org/");
                    XmlNode UserCabecera = xml.SelectSingleNode("//tem:userName", namespaces);
                    UserCabecera.InnerText = sh.User;
                    XmlNode PassCabecera = xml.SelectSingleNode("//tem:password", namespaces);
                    PassCabecera.InnerText = sh.Password;

                    XmlNode tipo_Operacion = xml.SelectSingleNode("//tem:Tipo_Operacion", namespaces);
                    tipo_Operacion.InnerText = tipoOperacion; //"A";
                    XmlNode cod_Receptor = xml.SelectSingleNode("//tem:codReceptor", namespaces);
                    cod_Receptor.InnerText = mantenimiento.COD_RECEPTOR;
                    XmlNode cod_Contrato = xml.SelectSingleNode("//tem:codContrato", namespaces);
                    cod_Contrato.InnerText = codContrato;
                    XmlNode id_Solicitud = xml.SelectSingleNode("//tem:idSolicitud", namespaces);
                    id_Solicitud.InnerText = idSolicitud;
                    XmlNode Subtipo_solicitud = xml.SelectSingleNode("//tem:Subtipo_solicitud", namespaces);
                    Subtipo_solicitud.InnerText = SubtipoSolicitud;
                    XmlNode Estado_solicitud = xml.SelectSingleNode("//tem:Estado_Solicitud", namespaces);
                    Estado_solicitud.InnerText = EstadoSolicitud;
                    XmlNode Persona_Contacto = xml.SelectSingleNode("//tem:Persona_Contacto", namespaces);
                    Persona_Contacto.InnerText = PersonaContacto;
                    XmlNode Telefono = xml.SelectSingleNode("//tem:Telefono", namespaces);
                    Telefono.InnerText = TelefonoContacto;
                    // Kintell CUR
                    XmlNode SociedadVal = xml.SelectSingleNode("//tem:Sociedad", namespaces);
                    if (SociedadVal != null)
                    {
                        SociedadVal.InnerText = Sociedad;
                    }
                    //*****************************************************************************************
                    XmlNode nodoObservaciones = xml.SelectSingleNode("//tem:Observaciones", namespaces);
                    nodoObservaciones.InnerXml = "<![CDATA[" + Observaciones + "]]>";
                    XmlNode Horario_Contacto = xml.SelectSingleNode("//tem:Horario_Contacto", namespaces);
                    Horario_Contacto.InnerText = mantenimiento.HORARIO_CONTACTO;

                    if (mantenimiento != null)
                    {
                            XmlNode Nombre = xml.SelectSingleNode("//tem:Nombre", namespaces);
                        Nombre.InnerText = mantenimiento.NOM_TITULAR.Trim() + " " + mantenimiento.APELLIDO1.Trim() + " " + mantenimiento.APELLIDO2.Trim();
                            XmlNode Direccion = xml.SelectSingleNode("//tem:Direccion", namespaces);
                        Direccion.InnerText = mantenimiento.TIP_VIA_PUBLICA.Trim() + " " + mantenimiento.NOM_CALLE.Trim() + " " + mantenimiento.COD_PORTAL.Trim() + ", " + mantenimiento.TIP_BIS.Trim() + ", " + mantenimiento.TIP_ESCALERA.Trim() + ", " + mantenimiento.TIP_PISO.Trim() + " " + mantenimiento.TIP_MANO.Trim();
                            XmlNode CP = xml.SelectSingleNode("//tem:CP", namespaces);
                        CP.InnerText = mantenimiento.COD_POSTAL.Trim();
                            XmlNode Localidad = xml.SelectSingleNode("//tem:Localidad", namespaces);
                        Localidad.InnerText = mantenimiento.COD_POBLACION.Trim();
                            XmlNode Provincia = xml.SelectSingleNode("//tem:Provincia", namespaces);
                        Provincia.InnerText = mantenimiento.COD_PROVINCIA.Trim();
                            XmlNode EFV = xml.SelectSingleNode("//tem:EFV", namespaces);
                        EFV.InnerText = mantenimiento.DESEFV.Trim();
                            XmlNode Urgencia = xml.SelectSingleNode("//tem:Urgencia", namespaces);
                        Urgencia.InnerText = urgencia.Trim();
                            XmlNode AV_Resueltas = xml.SelectSingleNode("//tem:AV_Resueltas", namespaces);
                        AV_Resueltas.InnerText = averiasResueltas;
                    }
                    
                    sh.XmlContent = xml.InnerXml;

                    int idTraza = GuardarTrazaLlamadaWS("SetInfoSolAbiertaMAP", sh.XmlContent);
                    ConfiguracionDTO confActivoTLS12 = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.ACTIVOTLS12MAPFRE);
                    Boolean ActivoTLS12Mapfre = false;
                    if (confActivoTLS12 != null && !string.IsNullOrEmpty(confActivoTLS12.Valor) && Boolean.Parse(confActivoTLS12.Valor))
                    {
                        ActivoTLS12Mapfre = Boolean.Parse(confActivoTLS12.Valor);
                    }
                    string docResult = sh.CallWebService(ActivoTLS12Mapfre);
                    GuardarTrazaResultadoLlamadaWS(idTraza, docResult);

                    if (docResult.Contains("StackTrace") || docResult.Contains("faultcode") || docResult.Contains("CodigoError"))
                    {
                        // Error;
                        resultado = docResult;
                    }
                }
                //else
                //{
                //    //No se realiza la llamada al WS porque no se ha encontrado
                //    return false;
                //}
            }
            catch (Exception ex)
            {
                //20200123 BGN MOD BEG [R#21821]: Parametrizar envio mails por la web
                //MandarMailError("Se ha producido un error al llamar al WS AperturaSolicitud de MAP. CodContrato: " + codContrato + " IdSolicitud: " + idSolicitud + ". Message: " + ex.Message + " StackTrace: " + ex.StackTrace, "[SMG] Error llamada WS AperturaSolicitudProveedor");
                UtilidadesMail.EnviarMensajeError(" Error llamada WS AperturaSolicitudProveedor", "Se ha producido un error al llamar al WS AperturaSolicitud de MAP. CodContrato: " + codContrato + " IdSolicitud: " + idSolicitud + ". Message: " + ex.Message + " StackTrace: " + ex.StackTrace);
                //20200123 BGN MOD END [R#21821]: Parametrizar envio mails por la web
            }
            return resultado;
        }

        //20200930 BGN ADD BEG [R#22132]: Visualizar en Opera el ticket de combustión. Pantalla Gestión Documental
        private static bool AllwaysGoodCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors policyErrors)
        {
            return true;
        }

        public String llamadaWSConsultaDocumento(string CCBB)
        {
            String resultado = String.Empty;
            try
            {
                WebserviceDTO wsDto = new WebserviceDTO();
                //wsDto.NombreWebservice = "Consulta Documento";
                wsDto.TipoWebservice = "DOC";
                wsDto.ActivoWebservice = true;
                List<WebserviceDTO> lWebService = Webservice.Buscar(wsDto);
                if (lWebService.Count > 0)
                {
                    wsDto = lWebService[0];

                    ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(AllwaysGoodCertificate);
                    ConfiguracionDTO confActivoTLS12 = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.ACTIVOTLS12);
                    Boolean ActivoTLS12 = false;
                    if (confActivoTLS12 != null && !string.IsNullOrEmpty(confActivoTLS12.Valor) && Boolean.Parse(confActivoTLS12.Valor))
                    {
                        ActivoTLS12 = Boolean.Parse(confActivoTLS12.Valor);
                    }
                    if (ActivoTLS12)
                    {
                        ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                    }

                    SoapHelper sh = CargarSoapHelper(wsDto);

                    XmlDocument xml = ObtenerPlantillaCuerpo(wsDto.PlantillaWebservice);
                    XmlNamespaceManager namespaces = new XmlNamespaceManager(xml.NameTable);
                    namespaces.AddNamespace("ws", "http://ws.gesdoc.fwapp.iberdrola.com/");
                    
                    XmlNode referenciaExterna = xml.SelectSingleNode("//referenciaExterna", namespaces);
                    referenciaExterna.InnerText = CCBB; 

                    sh.XmlContent = xml.InnerXml;

                    int idTraza = GuardarTrazaLlamadaWS("consultaDocumentoReferencia", sh.XmlContent);
                    ConfiguracionDTO confActivoTLS12ConDoc = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.ACTIVOTLS12CONDOC);
                    Boolean ActivoTLS12ConDoc = false;
                    if (confActivoTLS12ConDoc != null && !string.IsNullOrEmpty(confActivoTLS12ConDoc.Valor) && Boolean.Parse(confActivoTLS12ConDoc.Valor))
                    {
                        ActivoTLS12ConDoc = Boolean.Parse(confActivoTLS12ConDoc.Valor);
                    }
                    string docResult = sh.CallWebService(ActivoTLS12ConDoc);
                    GuardarTrazaResultadoLlamadaWS(idTraza, docResult);

                    if (docResult.Contains("StackTrace") || docResult.Contains("faultcode") || docResult.Contains("CodigoError"))
                    {
                        // Error;
                        resultado = string.Empty;
                    }
                    else
                    {
                        //recuperar el binary data de dh
                        //en el tag nombre esta el nombre del fichero
                        resultado = docResult;
                    }
                }
                //else
                //{
                //    //No se realiza la llamada al WS porque no se ha encontrado
                //    return false;
                //}
            }
            catch (Exception ex)
            {
                //20200123 BGN MOD BEG [R#21821]: Parametrizar envio mails por la web
                UtilidadesMail.EnviarMensajeError(" Error llamada WS ConsultaDocumentoReferencia", "Se ha producido un error al llamar al WS ConsultaDocumentoReferencia. ReferenciaExterna: " + CCBB + ". Message: " + ex.Message + " StackTrace: " + ex.StackTrace);
                //20200123 BGN MOD END [R#21821]: Parametrizar envio mails por la web
            }
            return resultado;
        }
        //20200930 BGN ADD END [R#22132]: Visualizar en Opera el ticket de combustión. Pantalla Gestión Documental

        //20210121 BGN ADD BEG R#28584 - Envío Contrato GC a Edatalia para Firma Digital
        public String llamadaWSEdataliaAltaDocumentoPorEmail(string nombreFichero, string refExternaEdatalia, string nombreCliente, string dniCliente, string email, string movil)
        {
            String resultado = String.Empty;
            int idTraza = int.MinValue;
            try
            {
                WebserviceDTO wsDto = new WebserviceDTO();
                wsDto.NombreWebservice = "Edatalia Alta Documento Email";
                wsDto.TipoWebservice = "EFD";
                wsDto.ActivoWebservice = true;
                List<WebserviceDTO> lWebService = Webservice.Buscar(wsDto);
                if (lWebService.Count > 0)
                {
                    wsDto = lWebService[0];

                    ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(AllwaysGoodCertificate);
                    ConfiguracionDTO confActivoTLS12 = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.ACTIVOTLS12);
                    Boolean ActivoTLS12 = false;
                    if (confActivoTLS12 != null && !string.IsNullOrEmpty(confActivoTLS12.Valor) && Boolean.Parse(confActivoTLS12.Valor))
                    {
                        ActivoTLS12 = Boolean.Parse(confActivoTLS12.Valor);
                    }
                    if (ActivoTLS12)
                    {
                        ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                    }

                    SoapHelper sh = CargarSoapHelper(wsDto);

                    XmlDocument xml = ObtenerPlantillaCuerpo(wsDto.PlantillaWebservice);
                    XmlNamespaceManager namespaces = new XmlNamespaceManager(xml.NameTable);
                    namespaces.AddNamespace("edat", "http://www.edatalia.com/");

                    XmlNode referenciaExterna = xml.SelectSingleNode("//edat:DocumentSetExternRef", namespaces);
                    referenciaExterna.InnerText = refExternaEdatalia;

                    XmlNode nombreDocumento = xml.SelectSingleNode("//edat:ItemName", namespaces);
                    nombreDocumento.InnerText = nombreFichero;

                    using (ManejadorFicheros.GetImpersonator())
                    {
                        ConfiguracionDTO rutaEdatalia = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.RUTA_FICHEROS_EDATALIA);
                        String ficheroOrigen = rutaEdatalia.Valor + nombreFichero;
                        
                        if (File.Exists(ficheroOrigen))
                        {
                            XmlNode contenidoFichero = xml.SelectSingleNode("//edat:ItemPDFContentB64", namespaces);
                            byte[] byteArray = File.ReadAllBytes(ficheroOrigen);                            
                            contenidoFichero.InnerText = Convert.ToBase64String(byteArray);
                        }
                    }

                    XmlNode nombreDestinatario = xml.SelectSingleNode("//edat:RecipientName", namespaces);
                    nombreDestinatario.InnerText = nombreCliente;

                    XmlNode dniDestinatario = xml.SelectSingleNode("//edat:RecipientIdCard", namespaces);
                    dniDestinatario.InnerText = dniCliente;

                    XmlNode telefonoDestinatario = xml.SelectSingleNode("//edat:RecipientCellPhone", namespaces);
                    if (!String.IsNullOrEmpty(movil))
                    {
                        //20220215 BGN MOD BEG R#35162: Adaptar la macro a los cambios en los campos de teléfono del cliente
                        //telefonoDestinatario.InnerText = "+34" + movil;
                        telefonoDestinatario.InnerText = movil;
                        //20220215 BGN MOD BEG R#35162: Adaptar la macro a los cambios en los campos de teléfono del cliente
                    }


                    XmlNode emailDestinatario = xml.SelectSingleNode("//edat:RecipientMail", namespaces);
                    emailDestinatario.InnerText = email;

                    sh.XmlContent = xml.InnerXml;

                    idTraza = GuardarTrazaLlamadaWS("NewDocumentSet", sh.XmlContent);
                    ConfiguracionDTO confActivoTLS12EdataliaMail = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.ACTIVOTLS12EDATALIAMAIL);
                    Boolean ActivoTLS12EdataliaMail = false;
                    if (confActivoTLS12EdataliaMail != null && !string.IsNullOrEmpty(confActivoTLS12EdataliaMail.Valor) && Boolean.Parse(confActivoTLS12EdataliaMail.Valor))
                    {
                        ActivoTLS12EdataliaMail = Boolean.Parse(confActivoTLS12EdataliaMail.Valor);
                    }
                    string docResult = sh.CallWebService(ActivoTLS12EdataliaMail);
                    GuardarTrazaResultadoLlamadaWS(idTraza, docResult);

                    resultado = ObtenerDocumentIdEdatalia(docResult);
                }
            }
            catch (Exception ex)
            {
                if (idTraza != int.MinValue)
                {
                    GuardarTrazaResultadoLlamadaWS(idTraza, ex.Message);
                }                
                UtilidadesMail.EnviarMensajeError(" Error llamada WS llamadaWSEdataliaAltaDocumentoPorEmail", "Se ha producido un error al llamar al WS llamadaWSEdataliaAltaDocumentoPorEmail. ReferenciaExterna: " + refExternaEdatalia + ". Message: " + ex.Message + " StackTrace: " + ex.StackTrace);
            }
            return resultado;
        }

        public String llamadaWSEdataliaAltaDocumentoPorMovil(string nombreFichero, string refExternaEdatalia, string nombreCliente, string dniCliente, string email, string movil)
        {
            String resultado = String.Empty;
            try
            {
                WebserviceDTO wsDto = new WebserviceDTO();
                wsDto.NombreWebservice = "Edatalia Alta Documento Movil";
                wsDto.TipoWebservice = "EFD";
                wsDto.ActivoWebservice = true;
                List<WebserviceDTO> lWebService = Webservice.Buscar(wsDto);
                if (lWebService.Count > 0)
                {
                    wsDto = lWebService[0];

                    ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(AllwaysGoodCertificate);
                    ConfiguracionDTO confActivoTLS12 = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.ACTIVOTLS12);
                    Boolean ActivoTLS12 = false;
                    if (confActivoTLS12 != null && !string.IsNullOrEmpty(confActivoTLS12.Valor) && Boolean.Parse(confActivoTLS12.Valor))
                    {
                        ActivoTLS12 = Boolean.Parse(confActivoTLS12.Valor);
                    }
                    if (ActivoTLS12)
                    {
                        ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                    }

                    SoapHelper sh = CargarSoapHelper(wsDto);

                    XmlDocument xml = ObtenerPlantillaCuerpo(wsDto.PlantillaWebservice);
                    XmlNamespaceManager namespaces = new XmlNamespaceManager(xml.NameTable);
                    namespaces.AddNamespace("edat", "http://www.edatalia.com/");

                    XmlNode referenciaExterna = xml.SelectSingleNode("//edat:DocumentSetExternRef", namespaces);
                    referenciaExterna.InnerText = refExternaEdatalia;

                    XmlNode nombreDocumento = xml.SelectSingleNode("//edat:ItemName", namespaces);
                    nombreDocumento.InnerText = nombreFichero;

                    using (ManejadorFicheros.GetImpersonator())
                    {
                        ConfiguracionDTO rutaEdatalia = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.RUTA_FICHEROS_EDATALIA);
                        String ficheroOrigen = rutaEdatalia.Valor + nombreFichero;

                        if (File.Exists(ficheroOrigen))
                        {
                            XmlNode contenidoFichero = xml.SelectSingleNode("//edat:ItemPDFContentB64", namespaces);
                            byte[] byteArray = File.ReadAllBytes(ficheroOrigen);
                            contenidoFichero.InnerText = Convert.ToBase64String(byteArray);
                        }
                    }

                    XmlNode nombreDestinatario = xml.SelectSingleNode("//edat:RecipientName", namespaces);
                    nombreDestinatario.InnerText = nombreCliente;

                    XmlNode dniDestinatario = xml.SelectSingleNode("//edat:RecipientIdCard", namespaces);
                    dniDestinatario.InnerText = dniCliente;

                    XmlNode telefonoDestinatario = xml.SelectSingleNode("//edat:RecipientCellPhone", namespaces);
                    telefonoDestinatario.InnerText = movil;

                    XmlNode emailDestinatario = xml.SelectSingleNode("//edat:RecipientMail", namespaces);
                    emailDestinatario.InnerText = email;

                    sh.XmlContent = xml.InnerXml;

                    int idTraza = GuardarTrazaLlamadaWS("NewDocumentSetSMS", sh.XmlContent);
                    ConfiguracionDTO confActivoTLS12EdaMovil = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.ACTIVOTLS12EDAMOVIL);
                    Boolean ActivoTLS12EdaMovil = false;
                    if (confActivoTLS12EdaMovil != null && !string.IsNullOrEmpty(confActivoTLS12EdaMovil.Valor) && Boolean.Parse(confActivoTLS12EdaMovil.Valor))
                    {
                        ActivoTLS12EdaMovil = Boolean.Parse(confActivoTLS12EdaMovil.Valor);
                    }
                    string docResult = sh.CallWebService(ActivoTLS12EdaMovil);
                    GuardarTrazaResultadoLlamadaWS(idTraza, docResult);

                    resultado = ObtenerDocumentIdEdatalia(docResult);
                }
            }
            catch (Exception ex)
            {
                UtilidadesMail.EnviarMensajeError(" Error llamada WS llamadaWSEdataliaAltaDocumentoPorMovil", "Se ha producido un error al llamar al WS llamadaWSEdataliaAltaDocumentoPorMovil. ReferenciaExterna: " + refExternaEdatalia + ". Message: " + ex.Message + " StackTrace: " + ex.StackTrace);
            }
            return resultado;
        }

        public static string ObtenerDocumentIdEdatalia(string docResult)
        {
            string documentId = String.Empty;
            if (!String.IsNullOrEmpty(docResult))
            {
                if (docResult.Contains("<ResponseCode>1</ResponseCode>"))
                {
                    int posIni = docResult.IndexOf("<DocumentSetId>");
                    int posFin = docResult.IndexOf("</DocumentSetId>");
                    if (posIni > 0 && posFin>0)
                    {
                        posIni += 15;
                        posFin -= posIni;
                        documentId = docResult.Substring(posIni, posFin);
                    }
                }

                //20210708 BGN ADD BEG R#32694 - [Edatalia] Introducir un nuevo estado para poder corregir los e-mails de cliente incorrectos
                if (docResult.Contains("<ResponseCode>-38</ResponseCode>"))
                {
                    documentId = "-38";
                }
                //20210708 BGN ADD END R#32694 - [Edatalia] Introducir un nuevo estado para poder corregir los e-mails de cliente incorrectos

                    //XmlDocument xm = new XmlDocument();
                    //xm.LoadXml(docResult);

                    //XmlNamespaceManager namespaces = new XmlNamespaceManager(xm.NameTable);

                    //XmlNode nResponseCode = xm.SelectSingleNode("//ResponseCode", namespaces);
                    //if (nResponseCode != null && !String.IsNullOrEmpty(nResponseCode.InnerText) && nResponseCode.InnerText.Equals("1"))
                    //{
                    //    XmlNode nDocumentSetId = xm.SelectSingleNode("//DocumentSetId", namespaces);
                    //    if (nDocumentSetId != null && !String.IsNullOrEmpty(nDocumentSetId.InnerText))
                    //    {
                    //        documentId = nDocumentSetId.InnerText;
                    //    }
                    //}                
                }
            return documentId;
        }
        //20210121 BGN ADD END R#28584 - Envío Contrato GC a Edatalia para Firma Digital


        //29122021 KINTELL BEG REPAIR & CARE R#35439
        public XmlNode StringAXMLRepairAndCare(string XML, string codContrato)
        {
            try
            {
                Boolean esContrato = false;
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(XML);
                XmlNode dataNodes = doc.FirstChild.FirstChild.FirstChild;

                foreach (XmlNode node in dataNodes)
                {
                    foreach (XmlNode childNode in node.ChildNodes)
                    {
                        string nombreNodo = childNode.Name;
                        if (nombreNodo == "codContrato")
                        {
                            String contrato = childNode.InnerText;
                            if(contrato== codContrato)
                            {
                                esContrato = true;
                            }
                        }
                        if (nombreNodo == "datosEff" && esContrato)
                        {
                            foreach (XmlNode childNodeEFV in childNode.ChildNodes)
                            {
                                nombreNodo = childNodeEFV.Name;
                                if (nombreNodo == "codEfv")
                                {
                                    String EFV = childNodeEFV.InnerText;

                                    ContratoDB db = new ContratoDB();
                                    DataTable dtEFV = db.ObtenerExisteEFVYPermitirAlta(EFV);
                                    if (dtEFV.Rows.Count > 0)
                                    {
                                        node.Attributes.Item(1).Value = EFV;
                                        node.Attributes.Item(0).Value = dtEFV.Rows[0].ItemArray[0].ToString();
                                        //ESTADO PERMITIDO
                                        XmlAttribute attr = doc.CreateAttribute("ESTADOPERMITIDO");
                                        attr.Value = dtEFV.Rows[0].ItemArray[1].ToString();
                                        //Add the attribute to the node     
                                        node.Attributes.SetNamedItem(attr);
                                        //node.Attributes.Append(typeAttr);
                                        node.Attributes.Item(2).Value = dtEFV.Rows[0].ItemArray[1].ToString();

                                        XmlAttribute attrPortugal = doc.CreateAttribute("ESINSPECCION");
                                        attrPortugal.Value = dtEFV.Rows[0].ItemArray[2].ToString();
                                        //Add the attribute to the node     
                                        node.Attributes.SetNamedItem(attrPortugal);
                                        //node.Attributes.Append(typeAttr);
                                        node.Attributes.Item(3).Value = dtEFV.Rows[0].ItemArray[2].ToString();

                                        XmlAttribute attrGC = doc.CreateAttribute("ESGASCONFORT");
                                        attrGC.Value = dtEFV.Rows[0].ItemArray[3].ToString();
                                        //Add the attribute to the node     
                                        node.Attributes.SetNamedItem(attrGC);
                                        //node.Attributes.Append(typeAttr);
                                        node.Attributes.Item(4).Value = dtEFV.Rows[0].ItemArray[3].ToString();

                                        return node;
                                    }
                                }
                            }
                        }

                    }
                }
            }
            catch(Exception ex)
            {

            }
            return null;
        }

        public XmlNode StringAXMLGCTemporal(string XML)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(XML);
                XmlNode dataNodes = doc.FirstChild.FirstChild.FirstChild;

                foreach (XmlNode node in dataNodes)
                {
                    foreach (XmlNode childNode in node.ChildNodes)
                    {
                        string nombreNodo = childNode.Name;
                        if (nombreNodo == "datosEff")
                        {
                            foreach (XmlNode childNodeEFV in childNode.ChildNodes)
                            {
                                nombreNodo = childNodeEFV.Name;
                                if (nombreNodo == "codEfv")
                                {
                                    String EFV = childNodeEFV.InnerText;

                                    ContratoDB db = new ContratoDB();
                                    DataTable dtEFV = db.ObtenerEFVGCTemporal(EFV);
                                    if (dtEFV.Rows.Count > 0)
                                    {
                                        node.Attributes.Item(1).Value = EFV;
                                        node.Attributes.Item(0).Value = dtEFV.Rows[0].ItemArray[0].ToString();

                                        //ESTADO PERMITIDO
                                        XmlAttribute attr = doc.CreateAttribute("ESTADOPERMITIDO");
                                        attr.Value = dtEFV.Rows[0].ItemArray[1].ToString();
                                        //Add the attribute to the node     
                                        node.Attributes.SetNamedItem(attr);
                                        //node.Attributes.Append(typeAttr);
                                        node.Attributes.Item(2).Value = dtEFV.Rows[0].ItemArray[1].ToString();

                                        XmlAttribute attrPortugal = doc.CreateAttribute("ESINSPECCION");
                                        attrPortugal.Value = dtEFV.Rows[0].ItemArray[2].ToString();
                                        //Add the attribute to the node     
                                        node.Attributes.SetNamedItem(attrPortugal);
                                        //node.Attributes.Append(typeAttr);
                                        node.Attributes.Item(3).Value = dtEFV.Rows[0].ItemArray[2].ToString();

                                        XmlAttribute attrGC = doc.CreateAttribute("ESGASCONFORT");
                                        attrGC.Value = dtEFV.Rows[0].ItemArray[3].ToString();
                                        //Add the attribute to the node     
                                        node.Attributes.SetNamedItem(attrGC);
                                        //node.Attributes.Append(typeAttr);
                                        node.Attributes.Item(4).Value = dtEFV.Rows[0].ItemArray[3].ToString();

                                        return node;
                                    }
                                }
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {

            }
            return null;
        }

        
        public String llamadaWSobtenerDatosCtoOperaSMG(string contrato)
        {
            String resultado = String.Empty;
            try
            {
                WebserviceDTO wsDto = new WebserviceDTO();
                //wsDto.NombreWebservice = "Consulta Documento";
                wsDto.TipoWebservice = "OCD";
                wsDto.ActivoWebservice = true;
                List<WebserviceDTO> lWebService = Webservice.Buscar(wsDto);
                if (lWebService.Count > 0)
                {
                    wsDto = lWebService[0];

                    ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(AllwaysGoodCertificate);
                    ConfiguracionDTO confActivoTLS12 = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.ACTIVOTLS12);
                    Boolean ActivoTLS12 = false;
                    if (confActivoTLS12 != null && !string.IsNullOrEmpty(confActivoTLS12.Valor) && Boolean.Parse(confActivoTLS12.Valor))
                    {
                        ActivoTLS12 = Boolean.Parse(confActivoTLS12.Valor);
                    }
                    if (ActivoTLS12)
                    {
                        ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                    }

                    SoapHelper sh = CargarSoapHelper(wsDto);

                    XmlDocument xml = ObtenerPlantillaCuerpo(wsDto.PlantillaWebservice);
                    XmlNamespaceManager namespaces = new XmlNamespaceManager(xml.NameTable);
                    

                    XmlNode referenciaExterna = xml.SelectSingleNode("//codContrato", namespaces);
                    referenciaExterna.InnerText = contrato;

                    sh.XmlContent = xml.InnerXml;

                    int idTraza = GuardarTrazaLlamadaWS("obtenerDatosCtoOperaSMG", sh.XmlContent);
                    ConfiguracionDTO confActivoTLS12DatosContrato = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.ACTIVOTLS12DATOSCONTRATO);
                    Boolean ActivoTLS12DatosContrato = false;
                    if (confActivoTLS12DatosContrato != null && !string.IsNullOrEmpty(confActivoTLS12DatosContrato.Valor) && Boolean.Parse(confActivoTLS12DatosContrato.Valor))
                    {
                        ActivoTLS12DatosContrato = Boolean.Parse(confActivoTLS12DatosContrato.Valor);
                    }

                    string docResult = sh.CallWebService(ActivoTLS12DatosContrato);
                    GuardarTrazaResultadoLlamadaWS(idTraza, docResult);

                    if (docResult.Contains("StackTrace") || docResult.Contains("faultcode") || docResult.Contains("CodigoError"))
                    {
                        // Aqui da Error.
                        // Hacemos 2 llamadas mas porque a veces da error de Not Found, pero si existe, y de seguido devuelve valor (A mirar si pasa en PRO también).
                        // 2 mas porque he comprobado que a la tercera devuelve datos, pero puede que no sea asi y devuelve a la 4ª...
                        if (docResult.Contains("404")) //Not Found.
                        {
                            docResult = sh.CallWebService(ActivoTLS12DatosContrato);
                            GuardarTrazaResultadoLlamadaWS(idTraza, docResult);

                            if ((docResult.Contains("StackTrace") || docResult.Contains("faultcode") || docResult.Contains("CodigoError")) && docResult.Contains("404")) // Not Found.
                            {
                                docResult = sh.CallWebService(ActivoTLS12DatosContrato);
                                GuardarTrazaResultadoLlamadaWS(idTraza, docResult);

                                if (docResult.Contains("StackTrace") || docResult.Contains("faultcode") || docResult.Contains("CodigoError"))
                                {
                                    resultado = string.Empty;
                                }
                            }
                        }
                        else
                        {
                            resultado = string.Empty;
                        }
                    }
                    else
                    {
                        //recuperar el binary data de dh
                        //en el tag nombre esta el nombre del fichero
                        resultado = docResult;
                    }
                }
                //else
                //{
                //    //No se realiza la llamada al WS porque no se ha encontrado
                //    return false;
                //}
            }
            catch (Exception ex)
            {
                //20200123 BGN MOD BEG [R#21821]: Parametrizar envio mails por la web
                UtilidadesMail.EnviarMensajeError(" Error llamada WS ConsultaDocumentoReferencia", "Se ha producido un error al llamar al WS ConsultaDocumentoReferencia. ReferenciaExterna: " + contrato + ". Message: " + ex.Message + " StackTrace: " + ex.StackTrace);
                //20200123 BGN MOD END [R#21821]: Parametrizar envio mails por la web
            }
            return resultado;
        }
        //29122021 KINTELL END REPAIR & CARE R#35439

        //30122021 KINTELL BEG GAUTOMATICO R#35436
        public String llamadaWSobtenerValcred(string DNI, string CP)
        {
            String resultado = String.Empty;
            try
            {
                WebserviceDTO wsDto = new WebserviceDTO();
                //wsDto.NombreWebservice = "Consulta Documento";
                wsDto.TipoWebservice = "VAL";
                wsDto.ActivoWebservice = true;
                List<WebserviceDTO> lWebService = Webservice.Buscar(wsDto);
                if (lWebService.Count > 0)
                {
                    wsDto = lWebService[0];

                    ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(AllwaysGoodCertificate);
                    ConfiguracionDTO confActivoTLS12 = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.ACTIVOTLS12);
                    Boolean ActivoTLS12 = false;
                    if (confActivoTLS12 != null && !string.IsNullOrEmpty(confActivoTLS12.Valor) && Boolean.Parse(confActivoTLS12.Valor))
                    {
                        ActivoTLS12 = Boolean.Parse(confActivoTLS12.Valor);
                    }
                    if (ActivoTLS12)
                    {
                        ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                    }

                    SoapHelper sh = CargarSoapHelper(wsDto);

                    XmlDocument xml = ObtenerPlantillaCuerpo(wsDto.PlantillaWebservice);
                    XmlNamespaceManager namespaces = new XmlNamespaceManager(xml.NameTable);


                    XmlNode referenciaExterna = xml.SelectSingleNode("//cif", namespaces);
                    referenciaExterna.InnerText = DNI;

                    XmlNode referenciaExternaCP = xml.SelectSingleNode("//codigoPostal", namespaces);
                    referenciaExternaCP.InnerText = CP;

                    sh.XmlContent = xml.InnerXml;

                    int idTraza = GuardarTrazaLlamadaWS("obtenerDatosValcred", sh.XmlContent);
                    ConfiguracionDTO confActivoTLS12Valcred = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.ACTIVOTLS12VALCRED);
                    Boolean ActivoTLS12Valcred = false;
                    if (confActivoTLS12Valcred != null && !string.IsNullOrEmpty(confActivoTLS12Valcred.Valor) && Boolean.Parse(confActivoTLS12Valcred.Valor))
                    {
                        ActivoTLS12Valcred = Boolean.Parse(confActivoTLS12Valcred.Valor);
                    }

                    string docResult = sh.CallWebService(ActivoTLS12Valcred);
                    GuardarTrazaResultadoLlamadaWS(idTraza, docResult);

                    if (docResult.Contains("StackTrace") || docResult.Contains("faultcode") || docResult.Contains("CodigoError"))
                    {
                        // Aqui da Error.
                        // Hacemos 2 llamadas mas porque a veces da error de Not Found, pero si existe, y de seguido devuelve valor (A mirar si pasa en PRO también).
                        // 2 mas porque he comprobado que a la tercera devuelve datos, pero puede que no sea asi y devuelve a la 4ª...
                        if (docResult.Contains("404")) //Not Found.
                        {
                            docResult = sh.CallWebService(ActivoTLS12Valcred);
                            GuardarTrazaResultadoLlamadaWS(idTraza, docResult);

                            if ((docResult.Contains("StackTrace") || docResult.Contains("faultcode") || docResult.Contains("CodigoError")) && docResult.Contains("404")) // Not Found.
                            {
                                docResult = sh.CallWebService(ActivoTLS12Valcred);
                                GuardarTrazaResultadoLlamadaWS(idTraza, docResult);

                                if (docResult.Contains("StackTrace") || docResult.Contains("faultcode") || docResult.Contains("CodigoError"))
                                {
                                    resultado = string.Empty;
                                }
                            }
                        }
                        else
                        {
                            resultado = string.Empty;
                        }
                    }
                    else
                    {
                        //recuperar el binary data de dh
                        //en el tag nombre esta el nombre del fichero
                        resultado = docResult;
                    }
                }
                //else
                //{
                //    //No se realiza la llamada al WS porque no se ha encontrado
                //    return false;
                //}
            }
            catch (Exception ex)
            {
                //20200123 BGN MOD BEG [R#21821]: Parametrizar envio mails por la web
                UtilidadesMail.EnviarMensajeError(" Error llamada WS ConsultaDocumentoReferencia", "Se ha producido un error al llamar al WS ConsultaDocumentoReferencia. ReferenciaExterna: " + DNI + ". Message: " + ex.Message + " StackTrace: " + ex.StackTrace);
                //20200123 BGN MOD END [R#21821]: Parametrizar envio mails por la web
            }
            return resultado;
        }

        public XmlNode StringAXMLValcred(string XML)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(XML);
                XmlNode dataNodes = doc.LastChild.FirstChild.FirstChild.FirstChild;

                foreach (XmlNode node in dataNodes)
                {
                    string nombreNodo = node.Name;
                    if (nombreNodo == "ns4:codmensaje")
                    {
                        return node;
                    }
                }
            }
            catch (Exception ex)
            {
                // Error.
            }
            return null;
        }
        //30122021 KINTELL END GAUTOMATICO R#35436

        private static SoapHelper CargarSoapHelper(WebserviceDTO wsDto) 
        {
            SoapHelper sh = new SoapHelper();
            sh.UrlAdress = wsDto.UrlWebservice;
            //20210121 BGN MOD BEG R#28584 - Envío Contrato GC a Edatalia para Firma Digital
            if (!String.IsNullOrEmpty(wsDto.UsuarioWebservice))
            {
                sh.User = wsDto.UsuarioWebservice;
            }
            if (!String.IsNullOrEmpty(wsDto.PasswordWebservice))
            {
                sh.Password = EncryptHelper.Decrypt(wsDto.PasswordWebservice, true);
            }
            //20210121 BGN MOD END R#28584 - Envío Contrato GC a Edatalia para Firma Digital  
            sh.Method = wsDto.MetodoWebservice;

            if (wsDto.ProxyActivoWebService)
            {
                ConfiguracionDTO confProxyUser = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.WS_PROXY_USER);
                if (confProxyUser != null && !string.IsNullOrEmpty(confProxyUser.Valor))
                {
                    sh.ProxyUser = confProxyUser.Valor;
                }

                ConfiguracionDTO confProxyPass = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.WS_PROXY_PASS);
                if (confProxyPass != null && !string.IsNullOrEmpty(confProxyPass.Valor))
                {
                    sh.ProxyPassword = EncryptHelper.Decrypt(confProxyPass.Valor, true);
                }

                ConfiguracionDTO confProxyURL = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.WS_PROXY_URL);
                if (confProxyURL != null && !string.IsNullOrEmpty(confProxyURL.Valor))
                {
                    sh.ProxyURL = confProxyURL.Valor;
                }
            }

            ConfiguracionDTO confAutHeader = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.WS_AUT_HEADER);
            if (confAutHeader != null && !string.IsNullOrEmpty(confAutHeader.Valor))
            {
                sh.Autorizacion = confAutHeader.Valor;
            }
            ConfiguracionDTO confContentTypeReq = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.WS_CONTENT_TYPE_REQUEST);
            if (confContentTypeReq != null && !string.IsNullOrEmpty(confContentTypeReq.Valor))
            {
                sh.RequestContentType = confContentTypeReq.Valor;
            }
            ConfiguracionDTO confAcceptReq = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.WS_ACCEPT_REQUEST);
            if (confAcceptReq != null && !string.IsNullOrEmpty(confAcceptReq.Valor))
            {
                sh.RequestAccept = confAcceptReq.Valor;
            }
            ConfiguracionDTO confMethodReq = Configuracion.ObtenerConfiguracion(Enumerados.Configuracion.WS_METHOD_REQUEST);
            if (confMethodReq != null && !string.IsNullOrEmpty(confMethodReq.Valor))
            {
                sh.RequestMethod = confMethodReq.Valor;
            }
            return sh;
        }

        /// <summary>
        /// Obtiene la plantilla del cuerpo para la solicitud 
        /// </summary>
        /// <returns>Plantilla solicitud</returns>
        private static XmlDocument ObtenerPlantillaCuerpo(string rutaPlantilla)
        {
            using (ManejadorFicheros.GetImpersonator())
            {
                XmlDocument docXML = new XmlDocument();
                docXML.Load(rutaPlantilla);
                return docXML;
            }
        }

        private Int32 GuardarTrazaLlamadaWS(String nombreWS, String request)
        {
            TrazaDB trazaDB = new TrazaDB();
            return trazaDB.InsertarTrazaWS(nombreWS, request);
        }

        private void GuardarTrazaResultadoLlamadaWS(Int32 idTraza, String resultado)
        {
            if (idTraza!= int.MinValue)
            {
                // Actualizar la traza con el resultado de la operación.
                TrazaDB trazaDB = new TrazaDB();
                trazaDB.ActualizarTrazaWS(idTraza, resultado);
            }
            else
            {
                throw new Exception("Error al guardar el resultado de la traza. No se ha encontrato el identificativo de la traza de la llamada al WS.");
            }
        }

    }
}