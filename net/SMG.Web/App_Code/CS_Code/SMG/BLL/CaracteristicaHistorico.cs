using System;
using System.Data;
using Iberdrola.SMG.DAL.DB;

namespace Iberdrola.SMG.BLL
{
    public class CaracteristicaHistorico : BaseBLL
    {
        public CaracteristicaHistorico()
        {}

        public static DataTable GetCaracteristicaHistoricoSolicitud(string strIdSolicitud, Int16 idIdioma)
        {
            CaracteristicaHistoricoDB carHisDB = new CaracteristicaHistoricoDB();
            return carHisDB.GetCaracteristicaHistoricoSolicitud(strIdSolicitud, idIdioma);
        }

        public static void AddCaracteristicaCierreVisita(string codContrato, Int32 codvisita, int tipCar, string Valor)
        {
            CaracteristicaHistoricoDB carHisDB = new CaracteristicaHistoricoDB();
            carHisDB.AddCaracteristicaCierreVisita(codContrato, codvisita, tipCar, Valor);
        }

        //13/01/2020 BGN BEG R#17876 Heredar Nº Serie en Averias GC en AparatoR#17876 Heredar Nº Serie en Averias GC en Aparato
        public static string GetNumSerieGC(string codContrato)
        {
            CaracteristicaHistoricoDB carHisDB = new CaracteristicaHistoricoDB();
            return carHisDB.GetNumSerieGC(codContrato);
        }

        //20210120 BGN ADD BEG R#28584 - Envío Contrato GC a Edatalia para Firma Digital
        public static string GetCaracteristicaValor(int idSolicitud, string tipCar)
        {
            CaracteristicaHistoricoDB carHisDB = new CaracteristicaHistoricoDB();
            return carHisDB.GetCaracteristicaValor(idSolicitud, tipCar);
        }
        //20210120 BGN ADD END R#28584 - Envío Contrato GC a Edatalia para Firma Digital
    }
}