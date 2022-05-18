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
using System.Collections.Generic;
using Iberdrola.SMG.DAL.DTO;
using Iberdrola.Commons.Utils;
using Iberdrola.SMG.DAL.DB;

/// <summary>
/// Descripción breve de Lotes
/// </summary>
public class Lotes
{
    

    
    public Lotes()
	{
	}


    public static int ObtenerNumeroVisitasCanceladas(List<VisitaDTO> lista)
    {
        int i = 0;
        foreach (VisitaDTO visita in lista)
        {
            if (Visitas.EsEstadoVisitaCancelada(visita.CodigoEstadoVisita))
            {
                i++;
            }
        }
        return i;
    }

    public static int ObtenerNumeroVisitasCerradas(List<VisitaDTO> lista)
    {
        int i = 0;
        foreach (VisitaDTO visita in lista)
        {
            if (Visitas.EsEstadoVisitaConsideradaCerrada(visita.CodigoEstadoVisita))
            {
                i++;
            }
        }
        return i;
    }
    public static int ObtenerNumeroVisitasContratoSinVisita(List<VisitaDTO> lista)
    {
        int i = 0;
        foreach (VisitaDTO visita in lista)
        {
            if (visita.CodigoEstadoVisita == null)
            {
                i++;
            }
        }
        return i;

    }

    public static Decimal? GenerarLote(String strTextoLote, List<VisitaDTO> listaVisitas,String Usuario)
    {
        List<VisitaDTO> listaVisitasNoCerradas = new List<VisitaDTO>();
        foreach(VisitaDTO visita in listaVisitas)
        {
            if (!Visitas.EsEstadoVisitaConsideradaCerrada(visita.CodigoEstadoVisita) && !Visitas.EsEstadoVisitaCancelada(visita.CodigoEstadoVisita))
            {
                listaVisitasNoCerradas.Add(visita);
            }
        }
        LoteDB loteDB = new LoteDB();
        LoteDTO loteDTO = new LoteDTO();
        loteDTO.Descripcion = strTextoLote;
        loteDTO.FechaLote = DateTime.Now;

        loteDTO = loteDB.GenerarLote(loteDTO, listaVisitasNoCerradas,Usuario);

        if (loteDTO != null)
        {
            return loteDTO.Id;
        }
        else
        {
            return null;
        }
    }

    public static Boolean BuscarLoteID(LoteDTO loteDTO)
    {
        LoteDB loteDB= new LoteDB();
        return loteDB.BuscarPorId(loteDTO);
    }

    public static Boolean BuscarLoteDescripcion(LoteDTO loteDTO)
    {
        LoteDB loteDB = new LoteDB();
        return loteDB.BuscarPorDescripcion(loteDTO);
    }

    public static IDataReader ObtenerLotesDescripcion(LoteDTO loteDTO)
    {
        LoteDB loteDB = new LoteDB();
        return loteDB.ObtenerLotesDescripcion(loteDTO);
    }


}
