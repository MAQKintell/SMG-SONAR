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
using Iberdrola.SMG.DAL.DB;
using Iberdrola.SMG.DAL.DTO;
using System.Collections.Generic;
using Iberdrola.Commons.Exceptions;

/// <summary>
/// Descripción breve de TablasReferencia
/// </summary>
public class TablasReferencia
{
	public TablasReferencia()
	{

	}

    public static List<ProvinciaDTO> ObtenerProvincias()
    {
        ProvinciaDB provDB = new ProvinciaDB();
        return provDB.ObtenerProvincias();
    }

    public static List<CampaniaDTO> ObtenerCampanias()
    {
        CampaniaDB campDB = new CampaniaDB();
        return campDB.ObtenerCampanias();
    }

    public static List<LoteDTO> ObtenerLotes()
    {
        LoteDB loteDB = new LoteDB();
        return loteDB.ObtenerLotes();
    }

    public static List<TipoUrgenciaDTO> ObtenerTiposUrgencia(Int16 idIdioma)
    {
        TipoUrgenciaDB tipoUrgDB = new TipoUrgenciaDB();
        return tipoUrgDB.ObtenerTiposUrgencia(idIdioma);
    }

    public static List<TipoEstadoVisitaDTO> ObtenerTiposEstadoVisita(Int16 idIdioma)
    {
        TipoEstadoVisitaDB tipoDB = new TipoEstadoVisitaDB();
        return tipoDB.ObtenerTiposEstadoVisita(idIdioma);
    }

    public static List<TipoCodigosAveriaGasConfortDTO> ObtenerCodigosAveriaGasConfort()
    {
        TipoCodigosAveriaGasConfortDB tipoDB = new TipoCodigosAveriaGasConfortDB();
        return tipoDB.ObtenerCodigosAveriaGasConfort();
    }

    public static List<TipoCalderaDTO> ObtenerTiposTipoCaldera(Int16 IdIdioma)
    {
        try
        {
            TipoCalderaDB tipoDB = new TipoCalderaDB();
            return tipoDB.ObtenerTiposCaldera(IdIdioma);
        }
        catch (BaseException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new BLLException(false, ex, "2001");
        }
    }

    public static List<TipoMarcaCalderaDTO> ObtenerTiposMarcaCaldera(Decimal? intIdMarcaCaldera)
    {
        try
        {
            TipoMarcaCalderaDB marcaCalderaDB = new TipoMarcaCalderaDB();
            return marcaCalderaDB.ObtenerTiposMarcaCaldera(intIdMarcaCaldera);
        }
        catch (BaseException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new BLLException(false, ex, "2001");
        }
    }

    public static List<TipoUsoCalderaDTO> ObtenerTiposUsoCaldera()
    {
        try
        {
            TipoUsoCalderaDB usoCalderaDB = new TipoUsoCalderaDB();
            return usoCalderaDB.ObtenerTiposUsoCaldera();
        }
        catch (BaseException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new BLLException(false, ex, "2001");
        }
    }

    public static List<TipoReparacionDTO> ObtenerTiposReparacion(Int16 idIdioma)
    {
        TipoReparacionDB tipoDB = new TipoReparacionDB();
        return tipoDB.ObtenerTiposReparacion(idIdioma);
    }

    public static List<TipoEquipamientoDTO> ObtenerTiposEquipamiento(Int16 idIdioma)
    {
        TipoEquipamientoDB tipoUrgDB = new TipoEquipamientoDB();
        return tipoUrgDB.ObtenerTiposEquipamiento(idIdioma);
    }

    public static List<TipoTiempoManoDeObraDTO> ObtenerTiposTiempoManoDeObra(Int16 idIdioma)
    {
        TipoTiempoManoDeObraDB tipoDB = new TipoTiempoManoDeObraDB();
        return tipoDB.ObtenerTiposTiempoManoDeObra(idIdioma);
    }

    public static List<PoblacionDTO> ObtenerPoblacionesProvincia(ProvinciaDTO provincia)
    {
        PoblacionDB poblacionDB = new PoblacionDB();
        return poblacionDB.ObtenerPoblaciones(provincia);
    }
}
