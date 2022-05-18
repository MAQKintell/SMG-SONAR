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
using System.Transactions;

/// <summary>
/// Descripción breve de Documentos
/// </summary>
public class Documentos
{
    static Documentos()
    {
        //
        // TODO: Agregar aquí la lógica del constructor
        //
    }

    //public static Int32 ObtenerRegistroFicherosPorNombre(String nombreFichero)
    //{
    //    DocumentosDB documentosDb = new DocumentosDB();

    //    return documentosDb.ObtenerRegistroFicherosPorNombre(nombreFichero);
    //}

    public static String ObtenerRutaFicheros()
    {
        DocumentosDB documentosDb = new DocumentosDB();

        return documentosDb.ObtenerRutaFicheros();
    }

    public static String ObtenerRutaFicherosInspeccion()
    {
        DocumentosDB documentosDb = new DocumentosDB();

        return documentosDb.ObtenerRutaFicherosInspeccion();
    }

    public static String ObtenerCCBBPorContrato(string Contrato)
    {
        DocumentosDB documentosDb = new DocumentosDB();

        return documentosDb.ObtenerCCBBPorContrato(Contrato);
    }

    public static String ObtenerRutaFicherosTemporal()
    {
        DocumentosDB documentosDb = new DocumentosDB();

        return documentosDb.ObtenerRutaFicherosTemporal();
    }

    public static Boolean ObtenerFicheroExigible(String Subtipo, String EstadoDestino, String EstadoOrigen)
    {
        DocumentosDB documentosDb = new DocumentosDB();

        return documentosDb.ObtenerFicheroExigible(Subtipo, EstadoDestino, EstadoOrigen);
    }
    
}