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
using Iberdrola.Commons.Utils;
using System.Collections.Generic;
using System.Security.Principal;
using Iberdrola.SMG.DAL.DB;
using Iberdrola.SMG.DAL.DTO;
using Iberdrola.Commons.Exceptions;
using Iberdrola.Commons.Security;
using System.Collections;
using System.Data.SqlClient;
using Iberdrola.Commons.Web;
using Iberdrola.SMG.BLL;
using System.Xml;

/// <summary>
/// Descripción breve de Mantenimiento
/// </summary>
public class Mantenimiento
{
	public Mantenimiento()
	{

	}

    public MantenimientoDTO DatosMantenimiento(string Contrato,string Pais)
    {
        try
        {
            MantenimientoDB mantenimientoDb = new MantenimientoDB();
            MantenimientoDTO mantenimiento = new MantenimientoDTO();
            IDataReader mantenimientoDR = mantenimientoDb.ObtenerMantenimiento(Contrato, Pais);

            if (mantenimientoDR != null)
            {
                // Limitar que el proveedor solo pueda ver sus contratos (proveedor averia).
                while (mantenimientoDR.Read())
                {
                    UsuarioDTO user = Usuarios.ObtenerUsuarioLogeado();
                    if (user != null)
                    {

                        //Si el código del proveedor viene en blanco es que es el teléfono.
                        if (!string.IsNullOrEmpty(user.NombreProveedor))
                        {
                            if (!Usuarios.EsAdministrador((int)user.Id_Perfil) && !Usuarios.EsTelefono((int)user.Id_Perfil) && !Usuarios.EsReclamacion((int)user.Id_Perfil))
                            {
                                if((String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "PROVEEDOR_AVERIA") != user.CodProveedor
                                    && (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "PROVEEDOR_INSPECCION") != user.CodProveedor)
                                {
                                    //break;
                                }
                            }
                        }
                    
                    }
                    // 03/08/2010 Kintell (tema CUPS).
                    
                    mantenimiento.CUPS = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "CUPS");
                    mantenimiento.COD_RECEPTOR = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "COD_RECEPTOR");
                    mantenimiento.OBSERVACIONES = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "OBSERVACIONES");
                    mantenimiento.BCS = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "BCS");
                    mantenimiento.DESEFV = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "DESEFV");
                    mantenimiento.SCORING = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "SCORING");
                    mantenimiento.CODEFV = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "COD_EFV");
                    mantenimiento.FECHA_HASTA_FACTURACION = (DateTime?)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "FECHA_HASTA_FACTURACION");
                    //*******************************************************************************

                    mantenimiento.COD_CONTRATO_SIC = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "COD_CONTRATO_SIC");

                    mantenimiento.APELLIDO1  = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "APELLIDO1");
                    mantenimiento.APELLIDO2  = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "APELLIDO2");
                    mantenimiento.DNI = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "DNI");
                    mantenimiento.COD_FINCA  = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "COD_FINCA");
                    mantenimiento.COD_POBLACION  = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "COD_POBLACION");
                    mantenimiento.COD_PORTAL = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "COD_PORTAL");
                    mantenimiento.COD_POSTAL = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "COD_POSTAL");
                    mantenimiento.COD_PROVINCIA = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "COD_PROVINCIA");
                    mantenimiento.COD_TARIFA = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "COD_TARIFA");
                    mantenimiento.COD_ULTIMA_VISITA = (Int16?)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "COD_ULTIMA_VISITA");
                    mantenimiento.ESTADO_CONTRATO = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "ESTADO_CONTRATO");
                    mantenimiento.FACTURADO_PROVEEDOR = (Boolean)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "FACTURADO_PROVEEDOR");
                    
                    mantenimiento.BAJA_SOLICITADA = (Boolean)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "BAJA_RENOVACION");
                    

                    mantenimiento.FEC_ALTA_CONTRATO = (DateTime?)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "FEC_ALTA_CONTRATO");
                    mantenimiento.FEC_ALTA_SERVICIO = (DateTime?)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "FEC_ALTA_SERVICIO");
                    mantenimiento.FEC_BAJA_CONTRATO = (DateTime?)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "FEC_BAJA_CONTRATO");
                    mantenimiento.FEC_BAJA_SERVICIO = (DateTime?)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "FEC_BAJA_SERVICIO");
                    mantenimiento.FEC_LIMITE_VISITA = (DateTime?)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "FEC_LIMITE_VISITA");
                    mantenimiento.FEC_RECLAMACION = (DateTime?)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "FEC_RECLAMACION");


                    mantenimiento.NOM_CALLE = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "NOM_CALLE");
                    mantenimiento.NOM_TITULAR = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "NOM_TITULAR");
                    mantenimiento.NUM_TEL_CLIENTE = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "NUM_TEL_CLIENTE");
                    mantenimiento.NUM_TEL_PS = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "NUM_TEL_PS");
                    //TEL: Eliminamos lo referente a Telefono 1 al 5
                    //mantenimiento.NUM_TELEFONO1 = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "NUM_TELEFONO1");
                    //mantenimiento.NUM_TELEFONO2 = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "NUM_TELEFONO2");
                    //mantenimiento.NUM_TELEFONO3 = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "NUM_TELEFONO3");
                    //mantenimiento.NUM_TELEFONO4 = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "NUM_TELEFONO4");
                    //mantenimiento.NUM_TELEFONO5 = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "NUM_TELEFONO5");
                    mantenimiento.PROVEEDOR = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "PROVEEDOR");
                    mantenimiento.PROVEEDOR_AVERIA = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "PROVEEDOR_AVERIA");
                    mantenimiento.PROVEEDOR_INSPECCION = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "PROVEEDOR_INSPECCION");
                    mantenimiento.RECLAMACION = (Boolean?)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "RECLAMACION");
                    mantenimiento.REVISION = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "REVISION");
                    mantenimiento.T1 = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "T1");
                    mantenimiento.T2 = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "T2");
                    mantenimiento.T5 = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "T5");
                    mantenimiento.TIP_BIS = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "TIP_BIS");
                    mantenimiento.TIP_ESCALERA = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "TIP_ESCALERA");
                    mantenimiento.TIP_MANO = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "TIP_MANO");
                    mantenimiento.TIP_PISO = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "TIP_PISO");
                    mantenimiento.TIP_VIA_PUBLICA = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "TIP_VIA_PUBLICA");

                    mantenimiento.TELEFONO1 = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "TELEFONO_CLIENTE1");
                    mantenimiento.TELEFONO2 = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "TELEFONO_CLIENTE2");
                    //TEL: ADD
                    mantenimiento.NUM_MOVIL = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "NUM_MOVIL");
                    
                    //16/01/2015: Importe que debe de pagar el cliente si da de baja.
                    mantenimiento.PAGAR_SI_BAJA = double.Parse(DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "PAGAR_SI_BAJA").ToString());
                   //#2095: Pago Anticipiado

                    if (DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "SOLCENT") != null)
                    {
                        mantenimiento.SOLCENT = (String)(DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "SOLCENT").ToString());
                    }
                    else
                    {
                        mantenimiento.SOLCENT = "";
                    }
                    mantenimiento.PAIS = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "PAIS");

                    //20191111 BGN ADD BEG [R#17895]: Incluir en el fichero de cartera y en la pantalla el campo Email
                    if (DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "MAIL_CLIENTE") != null)
                    {
                        mantenimiento.EMAIL = (String)(DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "MAIL_CLIENTE").ToString());
                    }
                    else
                    {
                        mantenimiento.EMAIL = "";
                    }
                    //20191111 BGN ADD END [R#17895]: Incluir en el fichero de cartera y en la pantalla el campo Email
                }

                CurrentSession.SetAttribute(CurrentSession.SESSION_DATOS_MANTENIMIENTO, mantenimiento);
                return mantenimiento;
            }
            else
            {
                throw new BLLException("6000"); 
            }
        }
        catch (BaseException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new BLLException(false, ex, "");
        }
    }
    public MantenimientoDTO DatosMantenimientoSinPais(string Contrato)
    {
        try
        {
            MantenimientoDB mantenimientoDb = new MantenimientoDB();
            MantenimientoDTO mantenimiento = new MantenimientoDTO();
            IDataReader mantenimientoDR = mantenimientoDb.ObtenerMantenimientoSinPais(Contrato);

            if (mantenimientoDR != null)
            {
                while (mantenimientoDR.Read())
                {
                    // 03/08/2010 Kintell (tema CUPS).
                    
                    mantenimiento.CUPS = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "CUPS");
                    mantenimiento.COD_RECEPTOR = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "COD_RECEPTOR");
                    mantenimiento.OBSERVACIONES = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "OBSERVACIONES");
                    mantenimiento.BCS = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "BCS");
                    mantenimiento.DESEFV = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "DESEFV");
                    mantenimiento.SCORING = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "SCORING");
                    mantenimiento.CODEFV = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "COD_EFV");
                    mantenimiento.FECHA_HASTA_FACTURACION = (DateTime?)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "FECHA_HASTA_FACTURACION");
                    //*******************************************************************************

                    mantenimiento.COD_CONTRATO_SIC = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "COD_CONTRATO_SIC"); ;

                    mantenimiento.APELLIDO1  = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "APELLIDO1");
                    mantenimiento.APELLIDO2  = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "APELLIDO2");
                    mantenimiento.DNI = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "DNI");
                    mantenimiento.COD_FINCA  = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "COD_FINCA");
                    mantenimiento.COD_POBLACION  = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "COD_POBLACION");
                    mantenimiento.COD_PORTAL = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "COD_PORTAL");
                    mantenimiento.COD_POSTAL = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "COD_POSTAL");
                    mantenimiento.COD_PROVINCIA = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "COD_PROVINCIA");
                    mantenimiento.COD_TARIFA = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "COD_TARIFA");
                    mantenimiento.COD_ULTIMA_VISITA = (Int16?)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "COD_ULTIMA_VISITA");
                    mantenimiento.ESTADO_CONTRATO = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "ESTADO_CONTRATO");
                    mantenimiento.FACTURADO_PROVEEDOR = (Boolean)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "FACTURADO_PROVEEDOR");
                    
                    mantenimiento.BAJA_SOLICITADA = (Boolean)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "BAJA_RENOVACION");
                    

                    mantenimiento.FEC_ALTA_CONTRATO = (DateTime?)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "FEC_ALTA_CONTRATO");
                    mantenimiento.FEC_ALTA_SERVICIO = (DateTime?)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "FEC_ALTA_SERVICIO");
                    mantenimiento.FEC_BAJA_CONTRATO = (DateTime?)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "FEC_BAJA_CONTRATO");
                    mantenimiento.FEC_BAJA_SERVICIO = (DateTime?)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "FEC_BAJA_SERVICIO");
                    mantenimiento.FEC_LIMITE_VISITA = (DateTime?)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "FEC_LIMITE_VISITA");
                    mantenimiento.FEC_RECLAMACION = (DateTime?)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "FEC_RECLAMACION");


                    mantenimiento.NOM_CALLE = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "NOM_CALLE");
                    mantenimiento.NOM_TITULAR = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "NOM_TITULAR");
                    mantenimiento.NUM_TEL_CLIENTE = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "NUM_TEL_CLIENTE");
                    mantenimiento.NUM_TEL_PS = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "NUM_TEL_PS");
                    //TEL: Eliminamos lo referente a Telefono 1 al 5
                    //mantenimiento.NUM_TELEFONO1 = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "NUM_TELEFONO1");
                    //mantenimiento.NUM_TELEFONO2 = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "NUM_TELEFONO2");
                    //mantenimiento.NUM_TELEFONO3 = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "NUM_TELEFONO3");
                    //mantenimiento.NUM_TELEFONO4 = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "NUM_TELEFONO4");
                    //mantenimiento.NUM_TELEFONO5 = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "NUM_TELEFONO5");
                    mantenimiento.PROVEEDOR = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "PROVEEDOR");
                    mantenimiento.PROVEEDOR_AVERIA = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "PROVEEDOR_AVERIA");
                    mantenimiento.PROVEEDOR_INSPECCION = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "PROVEEDOR_INSPECCION");
                    mantenimiento.RECLAMACION = (Boolean?)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "RECLAMACION");
                    mantenimiento.REVISION = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "REVISION");
                    mantenimiento.T1 = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "T1");
                    mantenimiento.T2 = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "T2");
                    mantenimiento.T5 = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "T5");
                    mantenimiento.TIP_BIS = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "TIP_BIS");
                    mantenimiento.TIP_ESCALERA = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "TIP_ESCALERA");
                    mantenimiento.TIP_MANO = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "TIP_MANO");
                    mantenimiento.TIP_PISO = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "TIP_PISO");
                    mantenimiento.TIP_VIA_PUBLICA = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "TIP_VIA_PUBLICA");

                    mantenimiento.TELEFONO1 = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "TELEFONO_CLIENTE1");
                    mantenimiento.TELEFONO2 = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "TELEFONO_CLIENTE2");
                    //TEL: ADD
                    mantenimiento.NUM_MOVIL = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "NUM_MOVIL");
                    
                    //16/01/2015: Importe que debe de pagar el cliente si da de baja.
                    mantenimiento.PAGAR_SI_BAJA = double.Parse(DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "PAGAR_SI_BAJA").ToString());
                   //#2095: Pago Anticipiado

                    if (DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "SOLCENT") != null)
                    {
                        mantenimiento.SOLCENT = (String)(DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "SOLCENT").ToString());
                    }
                    else
                    {
                        mantenimiento.SOLCENT = "";
                    }
                    mantenimiento.PAIS = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "PAIS");

                    //20191111 BGN ADD BEG [R#17895]: Incluir en el fichero de cartera y en la pantalla el campo Email
                    if (DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "MAIL_CLIENTE") != null)
                    {
                        mantenimiento.EMAIL = (String)(DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "MAIL_CLIENTE").ToString());
                    }
                    else
                    {
                        mantenimiento.EMAIL = "";
                    }
                    //20191111 BGN ADD END [R#17895]: Incluir en el fichero de cartera y en la pantalla el campo Email
                }

                CurrentSession.SetAttribute(CurrentSession.SESSION_DATOS_MANTENIMIENTO, mantenimiento);
                return mantenimiento;
            }
            else
            {
                throw new BLLException("6000"); 
            }
        }
        catch (BaseException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new BLLException(false, ex, "");
        }
    }

    public MantenimientoDTO DatosMantenimientoPorCodReceptor(string codContrato, string codReceptor,string codSociedad)
    {
        try
        {
            MantenimientoDB mantenimientoDb = new MantenimientoDB();
            MantenimientoDTO mantenimiento = new MantenimientoDTO();
            IDataReader mantenimientoDR = mantenimientoDb.ObtenerMantenimientoPorCodigoReceptor(codContrato,codReceptor,codSociedad);

            if (mantenimientoDR != null)
            {
                while (mantenimientoDR.Read())
                {
                    // 03/08/2010 Kintell (tema CUPS).

                    mantenimiento.CUPS = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "CUPS");
                    mantenimiento.COD_RECEPTOR = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "COD_RECEPTOR");
                    mantenimiento.OBSERVACIONES = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "OBSERVACIONES");
                    mantenimiento.BCS = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "BCS");
                    mantenimiento.DESEFV = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "DESEFV");
                    mantenimiento.SCORING = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "SCORING");
                    mantenimiento.CODEFV = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "COD_EFV");
                    mantenimiento.FECHA_HASTA_FACTURACION = (DateTime?)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "FECHA_HASTA_FACTURACION");
                    //*******************************************************************************

                    mantenimiento.COD_CONTRATO_SIC = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "COD_CONTRATO_SIC"); ;

                    mantenimiento.APELLIDO1 = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "APELLIDO1");
                    mantenimiento.APELLIDO2 = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "APELLIDO2");
                    mantenimiento.DNI = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "DNI");
                    mantenimiento.COD_FINCA = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "COD_FINCA");
                    mantenimiento.COD_POBLACION = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "COD_POBLACION");
                    mantenimiento.COD_PORTAL = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "COD_PORTAL");
                    mantenimiento.COD_POSTAL = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "COD_POSTAL");
                    mantenimiento.COD_PROVINCIA = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "COD_PROVINCIA");
                    mantenimiento.COD_TARIFA = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "COD_TARIFA");
                    mantenimiento.COD_ULTIMA_VISITA = (Int16?)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "COD_ULTIMA_VISITA");
                    mantenimiento.ESTADO_CONTRATO = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "ESTADO_CONTRATO");
                    mantenimiento.FACTURADO_PROVEEDOR = (Boolean)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "FACTURADO_PROVEEDOR");

                    mantenimiento.BAJA_SOLICITADA = (Boolean)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "BAJA_RENOVACION");


                    mantenimiento.FEC_ALTA_CONTRATO = (DateTime?)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "FEC_ALTA_CONTRATO");
                    mantenimiento.FEC_ALTA_SERVICIO = (DateTime?)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "FEC_ALTA_SERVICIO");
                    mantenimiento.FEC_BAJA_CONTRATO = (DateTime?)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "FEC_BAJA_CONTRATO");
                    mantenimiento.FEC_BAJA_SERVICIO = (DateTime?)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "FEC_BAJA_SERVICIO");
                    mantenimiento.FEC_LIMITE_VISITA = (DateTime?)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "FEC_LIMITE_VISITA");
                    mantenimiento.FEC_RECLAMACION = (DateTime?)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "FEC_RECLAMACION");


                    mantenimiento.NOM_CALLE = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "NOM_CALLE");
                    mantenimiento.NOM_TITULAR = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "NOM_TITULAR");
                    mantenimiento.NUM_TEL_CLIENTE = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "NUM_TEL_CLIENTE");
                    mantenimiento.NUM_TEL_PS = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "NUM_TEL_PS");
                    //TEL: Eliminamos lo referente a Telefono 1 al 5
                    //mantenimiento.NUM_TELEFONO1 = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "NUM_TELEFONO1");
                    //mantenimiento.NUM_TELEFONO2 = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "NUM_TELEFONO2");
                    //mantenimiento.NUM_TELEFONO3 = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "NUM_TELEFONO3");
                    //mantenimiento.NUM_TELEFONO4 = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "NUM_TELEFONO4");
                    //mantenimiento.NUM_TELEFONO5 = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "NUM_TELEFONO5");
                    mantenimiento.PROVEEDOR = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "PROVEEDOR");
                    mantenimiento.PROVEEDOR_AVERIA = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "PROVEEDOR_AVERIA");
                    mantenimiento.PROVEEDOR_INSPECCION = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "PROVEEDOR_INSPECCION");
                    mantenimiento.RECLAMACION = (Boolean?)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "RECLAMACION");
                    mantenimiento.REVISION = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "REVISION");
                    mantenimiento.T1 = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "T1");
                    mantenimiento.T2 = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "T2");
                    mantenimiento.T5 = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "T5");
                    mantenimiento.TIP_BIS = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "TIP_BIS");
                    mantenimiento.TIP_ESCALERA = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "TIP_ESCALERA");
                    mantenimiento.TIP_MANO = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "TIP_MANO");
                    mantenimiento.TIP_PISO = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "TIP_PISO");
                    mantenimiento.TIP_VIA_PUBLICA = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "TIP_VIA_PUBLICA");

                    mantenimiento.TELEFONO1 = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "TELEFONO_CLIENTE1");
                    mantenimiento.TELEFONO2 = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "TELEFONO_CLIENTE2");
                    //TEL: ADD
                    mantenimiento.NUM_MOVIL = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "NUM_MOVIL");

                    //16/01/2015: Importe que debe de pagar el cliente si da de baja.
                    mantenimiento.PAGAR_SI_BAJA = double.Parse(DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "PAGAR_SI_BAJA").ToString());
                    //#2095: Pago Anticipiado

                    if (DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "SOLCENT") != null)
                    {
                        mantenimiento.SOLCENT = (String)(DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "SOLCENT").ToString());
                    }
                    else
                    {
                        mantenimiento.SOLCENT = "";
                    }
                    mantenimiento.PAIS = (String)DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "PAIS");

                    //20191111 BGN ADD BEG [R#17895]: Incluir en el fichero de cartera y en la pantalla el campo Email
                    if (DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "MAIL_CLIENTE") != null)
                    {
                        mantenimiento.EMAIL = (String)(DataBaseUtils.GetDataReaderColumnValue(mantenimientoDR, "MAIL_CLIENTE").ToString());
                    }
                    else
                    {
                        mantenimiento.EMAIL = "";
                    }
                    //20191111 BGN ADD END [R#17895]: Incluir en el fichero de cartera y en la pantalla el campo Email
                }

                CurrentSession.SetAttribute(CurrentSession.SESSION_DATOS_MANTENIMIENTO, mantenimiento);
                return mantenimiento;
            }
            else
            {
                throw new BLLException("6000");
            }
        }
        catch (BaseException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new BLLException(false, ex, "");
        }
    }

    public MantenimientoDTO DatosMantenimientoDesdeDeltaYAltaContrato(XmlNode node,Boolean Alta)
    {
        try
        {
            //Permitir TA:
            //===============
            //Repair & Care
            //MGI
            //Portugal
            //GC
            //No Permitir TA:
            //================
            //Ampliado
            //Ampliado Independiente
            //AGI
            //En españa todos AL menos R & C y GC

            String fechaInspeccion = "";
            String horarioInspeccion = "";
            String cupsInspeccion = "";
            String ordInspeccion = "";


            MantenimientoDTO mantenimiento = new MantenimientoDTO();
            mantenimiento.DESEFV = node.Attributes.Item(0).Value;
            String EFVOrigen = node.Attributes.Item(1).Value;
            String estadoPermitido = node.Attributes.Item(2).Value;
            Boolean esInspeccion= Boolean.Parse(node.Attributes.Item(3).Value);
            Boolean esGC = Boolean.Parse(node.Attributes.Item(4).Value); 

            mantenimiento.ESTADO_CONTRATO = node.SelectNodes("tipEstContrato").Item(0).InnerText.ToString();
            // Comentamos a petición de Ruben y Nerea (13/01/2022)
            if (mantenimiento.ESTADO_CONTRATO == estadoPermitido)//"TA")
            {
                mantenimiento.COD_CONTRATO_SIC = node.SelectNodes("codContrato").Item(0).InnerText.ToString();
                mantenimiento.NOM_TITULAR = node.SelectNodes("nomCliente").Item(0).InnerText.ToString();
                mantenimiento.APELLIDO1 = node.SelectNodes("nomApellido100T").Item(0).InnerText.ToString();
                mantenimiento.APELLIDO2 = node.SelectNodes("nomApellido200T").Item(0).InnerText.ToString();
                mantenimiento.COD_PROVINCIA = node.SelectNodes("codProvincia").Item(0).InnerText.ToString();
                mantenimiento.COD_POBLACION = node.SelectNodes("codPoblacion").Item(0).InnerText.ToString();
                mantenimiento.NOM_CALLE = node.SelectNodes("nomCalleActual").Item(0).InnerText.ToString();
                mantenimiento.TIP_VIA_PUBLICA = node.SelectNodes("tipViaPublica").Item(0).InnerText.ToString();
                mantenimiento.COD_FINCA = node.SelectNodes("codFinca").Item(0).InnerText.ToString();
                mantenimiento.TIP_BIS = node.SelectNodes("tipBis").Item(0).InnerText.ToString();
                mantenimiento.COD_PORTAL = node.SelectNodes("codPortalFic").Item(0).InnerText.ToString();
                mantenimiento.TIP_ESCALERA = node.SelectNodes("tipEscalera").Item(0).InnerText.ToString();
                mantenimiento.TIP_PISO = node.SelectNodes("tipPiso").Item(0).InnerText.ToString();
                mantenimiento.TIP_MANO = node.SelectNodes("tipMano").Item(0).InnerText.ToString();

                mantenimiento.TIP_PISO += " " + node.SelectNodes("valPiso").Item(0).InnerText.ToString();
                mantenimiento.TIP_MANO += " " + node.SelectNodes("valMano").Item(0).InnerText.ToString();

                mantenimiento.COD_POSTAL = node.SelectNodes("codPostalFic").Item(0).InnerText.ToString();
                if (mantenimiento.COD_POSTAL.Length <= 1)
                {
                    mantenimiento.COD_POSTAL = node.SelectNodes("codPostalInter").Item(0).InnerText.ToString(); 
                }
                mantenimiento.COD_CLIENTE = node.SelectNodes("codCliente").Item(0).InnerText.ToString();
                mantenimiento.DNI = node.SelectNodes("numDniNifCif").Item(0).InnerText.ToString();
                mantenimiento.COD_RECEPTOR = node.SelectNodes("codRS").Item(0).InnerText.ToString();
                mantenimiento.PAIS = node.SelectNodes("codPais").Item(0).InnerText.ToString();
                mantenimiento.SOLCENT = "SCM09";
                mantenimiento.T1 = "N";
                mantenimiento.T2 = "N";
                mantenimiento.T5 = "N";
                mantenimiento.FEC_BAJA_SERVICIO = null;

                string codSociedad = node.SelectNodes("codSociedad").Item(0).InnerText.ToString();

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

                                if (EFV == EFVOrigen)
                                {
                                    mantenimiento.FEC_ALTA_SERVICIO = DateTime.Parse(childNode.SelectNodes("fecAltaServicio").Item(0).InnerText.ToString());
                                    mantenimiento.FEC_BAJA_SERVICIO = DateTime.Parse(childNode.SelectNodes("fecBajaServicio").Item(0).InnerText.ToString());
                                    if (esInspeccion)
                                    {
                                        fechaInspeccion = childNode.SelectNodes("fechaCliente").Item(0).InnerText.ToString();
                                        horarioInspeccion = childNode.SelectNodes("horarioCliente").Item(0).InnerText.ToString();
                                        cupsInspeccion = childNode.SelectNodes("valCupsGas").Item(0).InnerText.ToString();
                                        ordInspeccion = childNode.SelectNodes("valOrd").Item(0).InnerText.ToString();
                                    }
                                }
                            }
                        }
                    }
                    else if (nombreNodo == "datosTelefonos")
                    {
                        try
                        {
                            mantenimiento.NUM_TEL_CLIENTE = childNode.SelectNodes("numPfjTfnoMovil").Item(0).InnerText.ToString() + childNode.SelectNodes("numTfnoMovil").Item(0).InnerText.ToString();
                            mantenimiento.NUM_TEL_PS = childNode.SelectNodes("numPfjTfnoMovil").Item(0).InnerText.ToString() + childNode.SelectNodes("numTfnoMovil").Item(0).InnerText.ToString();
                        }
                        catch
                        {
                            // dos nodos de datosTelefonos.
                        }
                    }
                }

                try
                {
                    mantenimiento.EMAIL= node.SelectNodes("direcEmail").Item(0).InnerText.ToString();
                }
                catch(Exception ex)
                {

                }

                mantenimiento.COD_ULTIMA_VISITA = 1;
                mantenimiento.PROVEEDOR = "";
                mantenimiento.PROVEEDOR_AVERIA = "";
                mantenimiento.PROVEEDOR_INSPECCION = "";
                if (mantenimiento.FEC_ALTA_SERVICIO.ToString()== "01/01/9999 0:00:00")
                {
                    mantenimiento.FEC_LIMITE_VISITA = DateTime.Parse(mantenimiento.FEC_ALTA_SERVICIO.ToString());
                }
                else
                {
                    mantenimiento.FEC_LIMITE_VISITA = DateTime.Parse(mantenimiento.FEC_ALTA_SERVICIO.ToString()).AddYears(1);
                }

                ContratoDB m = new ContratoDB();
                // Si es Repair&Care damos de alta si no (es GC), no hay que dar de alta porque ya existe contrato en Opera, en ese caso solo habria que devolver el DTO.
                if (Alta)
                {
                    m.AltaContratoEnOpera(mantenimiento, EFVOrigen, codSociedad);
                }
                //Damos de alta la solicitud de inspeccion si asi corresponde.
                if (esInspeccion && fechaInspeccion != "")
                {
                    //Damos de alta la solicitud de inspeccion.
                    m.AltaSolicitudInspeccion(mantenimiento.COD_CONTRATO_SIC,cupsInspeccion,fechaInspeccion,horarioInspeccion,ordInspeccion);
                }

                mantenimiento.OBSERVACIONES = "";
                if (esGC)
                {
                    SolicitudDB sol = new SolicitudDB();
                    Int64 idSolicitud = sol.AltaSolicitudGC(mantenimiento.COD_CONTRATO_SIC);
                    string mensajeValoracion = Resources.TextosJavaScript.TEXTO_SOLO_PAGO_ANTICIPADO;

                    if (mantenimiento.SOLCENT == "SCM09")
                    {
                        // Todas las formas de pago
                        mensajeValoracion = Resources.TextosJavaScript.TEXTO_TODOS_PAGOS;
                    }
                    mensajeValoracion = Resources.TextosJavaScript.TEXTO_SOLICITUD_ALTA + " " + idSolicitud + "  " + mensajeValoracion;

                    mantenimiento.OBSERVACIONES = mensajeValoracion;
                }



                mantenimiento.FEC_BAJA_SERVICIO = null;
                ConsultasDB consultasDB = new ConsultasDB();
                string Proveedor = consultasDB.ObtenerProveedorAveriaPorContrato(mantenimiento.COD_CONTRATO_SIC);
                mantenimiento.PROVEEDOR = Proveedor;
                mantenimiento.PROVEEDOR_AVERIA = Proveedor;
                mantenimiento.PROVEEDOR_INSPECCION = Proveedor;

                // Despues de haber dado de alta los datos con los codigos, cogemos los nombres de provincia y poblacion para mostrar por la web.
                mantenimiento.COD_PROVINCIA = node.SelectNodes("nomProvinActual").Item(0).InnerText.ToString();
                mantenimiento.COD_POBLACION = node.SelectNodes("nomPoblaActual").Item(0).InnerText.ToString();

            }
            else
            {
                mantenimiento = new MantenimientoDTO();
            }
            CurrentSession.SetAttribute(CurrentSession.SESSION_DATOS_MANTENIMIENTO, mantenimiento);
            return mantenimiento;
           
        }
        //catch (BaseException)
        //{
        //    throw;
        //}
        catch (Exception ex)
        {
            throw new BLLException(false, ex, "");
        }
    }

    public static Int64 ActualizarTelefonosMantenimiento(String Contrato, String Telefono1, String Telefono2)
    {
        MantenimientoDB mantenimientoDB = new MantenimientoDB();
        return mantenimientoDB.ActualizarTelefonosMantenimiento(Contrato, Telefono1, Telefono2);
    }

    public static IDataReader GetSolicitudesPorContrato(string Contrato, int idIdioma)
    {
        MantenimientoDB mantenimientoDB = new MantenimientoDB();
        return mantenimientoDB.GetSolicitudesPorContrato(Contrato,idIdioma);
    }

    public static string InsertarSolicitudCaldera(string contrato, string usuario)
    {
        MantenimientoDB mantenimientoDB = new MantenimientoDB();
        return mantenimientoDB.InsertarSolicitudCaldera(contrato,usuario);
    }

    public static void InsertarSolicitudWSCreadaPorWebEnEnviados(string contrato, string idSolicitud)
    {
        MantenimientoDB mantenimientoDB = new MantenimientoDB();
        mantenimientoDB.InsertarSolicitudWSCreadaPorWebEnEnviados(contrato, idSolicitud);
    }
    
    public static string IdSolicitudCalderaOperativa(string contrato)
    {
        MantenimientoDB mantenimientoDB = new MantenimientoDB();
        return mantenimientoDB.IdSolicitudCalderaOperativa(contrato);
    }

    public static String GetContratoPorSolicitud(string Solicitud)
    {
        MantenimientoDB mantenimientoDB = new MantenimientoDB();
        return mantenimientoDB.GetContratoPorSolicitud(Solicitud);
    }

    public static String GetContratoPorDNICUPS(string DNI,string CUPS)
    {
        MantenimientoDB mantenimientoDB = new MantenimientoDB();
        return mantenimientoDB.GetContratoPorDNICUPS(DNI,CUPS);
    }
    public static IDataReader ObtenerDatosMantenimientoPorcontrato(string codContrato)
    {
        MantenimientoDB mantenimientoDB = new MantenimientoDB();
        return mantenimientoDB.ObtenerDatosMantenimientoPorcontrato(codContrato);
    }

    public static Int64 ActualizarFechaBajaContrato(String Contrato)
    {
        MantenimientoDB mantenimientoDB = new MantenimientoDB();
        return mantenimientoDB.ActualizarFechaBajaContrato (Contrato);
    }

    public static Int64 ActualizarFechaHastaFacturacion(String Contrato)
    {
        MantenimientoDB mantenimientoDB = new MantenimientoDB();
        return mantenimientoDB.ActualizarFechaHastaFacturacion (Contrato);
    }

    public static Int64 ActualizarFechaBajaServicio(String Contrato,String Usuario)
    {
        MantenimientoDB mantenimientoDB = new MantenimientoDB();
        return mantenimientoDB.ActualizarFechaBajaServicio (Contrato,Usuario);
    }

    public static Int64 InsertarAviso(String Contrato, Int16 CodVisita, String Aviso, String Usuario)
    {
        MantenimientoDB mantenimientoDB = new MantenimientoDB();
        return mantenimientoDB.InsertarAviso(Contrato,CodVisita,Aviso, Usuario);
    }

    public static Int64 InsertarValoracioncuadroMando(String Cod_proveedor, Double ValoracionVisita, Double ValoracionAveria, Double CarteraAmpliado,Double CarteraGasConfort)
    {
        MantenimientoDB mantenimientoDB = new MantenimientoDB();
        return mantenimientoDB.InsertarValoracioncuadroMando(Cod_proveedor, ValoracionVisita,ValoracionAveria, CarteraAmpliado, CarteraGasConfort);
    }

    public static Int64 InsertarFacturacionBajas(String Contrato, Int32 Visita)
    {
        MantenimientoDB mantenimientoDB = new MantenimientoDB();
        return mantenimientoDB.InsertarFacturacionBajas(Contrato, Visita);
    }

    public static Int64 InsertarAvisoSolicitud(string idSolicitud,string aviso,string usuario)
    {
        MantenimientoDB mantenimientoDB = new MantenimientoDB();
        return mantenimientoDB.InsertarAvisoSolicitud(idSolicitud,aviso,usuario);
    }

    public static IDataReader ObtenerAviso(String Contrato, Int16 CodVisita)
    {
        MantenimientoDB mantenimientoDB = new MantenimientoDB();
        return mantenimientoDB.ObtenerAviso(Contrato, CodVisita);
    }

    public static IDataReader ObtenerAvisoSolicitud(string idSolicitud)
    {
        MantenimientoDB mantenimientoDB = new MantenimientoDB();
        return mantenimientoDB.ObtenerAvisoSolicitud(idSolicitud);
    }

    public static String ObtenerProvinciaPorContrato(String Contrato)
    {
        MantenimientoDB mantenimientoDB = new MantenimientoDB();
        return mantenimientoDB.ObtenerProvinciaPorContrato(Contrato);
    }

    public static IDataReader ObtenerDatosMantenimientoWS(string Contrato, string idSolicitud)
    {
        MantenimientoDB mantenimientoDB = new MantenimientoDB();
        return mantenimientoDB.ObtenerDatosMantenimientoWS(Contrato, idSolicitud);
    }

    public IDataReader GetSubtipoSolicitudesProteccionGasTelefono(int idIdioma)
    {
        MantenimientoDB mantenimientoDB = new MantenimientoDB();
        return mantenimientoDB.GetSubtipoSolicitudesProteccionGasTelefono(idIdioma);
    }

    public IDataReader GetSubtipoSolicitudesInspeccion(int idIdioma)
    {
        MantenimientoDB mantenimientoDB = new MantenimientoDB();
        return mantenimientoDB.GetSubtipoSolicitudesInspeccion(idIdioma);
    }

    public IDataReader GetSubtipoSolicitudesAGI(int idIdioma)
    {
        MantenimientoDB mantenimientoDB = new MantenimientoDB();
        return mantenimientoDB.GetSubtipoSolicitudesAGI(idIdioma);
    }
}
