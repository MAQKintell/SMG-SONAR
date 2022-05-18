using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Iberdrola.SMG.DAL.DTO;
using Iberdrola.Commons.DataAccess;
using System.Collections.Generic;
using System.Collections;
using System.Data.Common;
using System.Text;
using Iberdrola.Commons.Utils;
using Iberdrola.Commons.Security;

namespace Iberdrola.SMG.DAL.DB
{
    /// <summary>
    /// Descripción breve de ContratoDB
    /// </summary>
    public class VistaContratoCompletoDB : BaseDB
    {
        #region constructores
        public VistaContratoCompletoDB()
        {
        }
        #endregion

        #region metodos privados
        /// <summary>
        /// El page index va de 1 a numTotalPaginas
        /// </summary>
        /// <param name="intPageIndex"></param>
        /// <returns>
        /// Si intPageIndex tiene algun valor devuelve " WHERE (la fila esta en el indice) "
        /// Si intPageIndex no tiene valor devuelve ""
        /// </returns>
        private String ObtenerClausulaPaginacion(Int32? intPageIndex)
        {
            StringBuilder strClausula = new StringBuilder("");
            if (intPageIndex.HasValue)
            {
                strClausula.Append(" WHERE ");
                strClausula.Append(" (FILA > ");
                strClausula.Append((Int32.Parse(Resources.SMGConfiguration.GridViewPageSize) * (intPageIndex.Value - 1)).ToString());
                strClausula.Append(" AND FILA <= ");
                strClausula.Append((Int32.Parse(Resources.SMGConfiguration.GridViewPageSize) * (intPageIndex.Value)).ToString());
                strClausula.Append(") ");
            }

            return strClausula.ToString();
        }

        /// <summary>
        /// Selecciona los filtros avanzados que esten activos
        /// </summary>
        /// <param name="filtros"></param>
        /// <param name="paramsName"></param>
        /// <param name="paramsType"></param>
        /// <param name="paramsValue"></param>
        /// <returns>a</returns>
        private void ObtenerFiltrosAvanzados(VistaContratoCompletoDTO filtros, Queue<string> listaStrCondiciones, ref String[] paramsName, ref DbType[] paramsType, ref object[] paramsValue)
        {
            List<String> listaNombres = new List<String>();
            List<DbType> listaTipos = new List<DbType>();
            List<Object> listaValores = new List<Object>();
            if (listaStrCondiciones == null)
            {
                listaStrCondiciones = new Queue<string>();
            }

            Boolean Existe=false;
            if (filtros.ProveedorEntrada != null && filtros.ProveedorEntrada != "ADICO")
            {
                listaStrCondiciones.Enqueue(" PROVEEDOR = @PROVEEDOR");
                listaNombres.Add("PROVEEDOR");
                listaTipos.Add(DbType.String);
                listaValores.Add(filtros.ProveedorEntrada);
                Existe = true;
            }
            

            if (filtros != null && filtros.FiltrosAvanzadosActivos)
            {
            
                #region construcción de los filtros avanzados

                if (filtros.CodigoContrato != null)
                {
                    listaStrCondiciones.Enqueue(" RCC.COD_CONTRATO_SIC = @COD_CONTRATO_SIC");
                    listaNombres.Add("COD_CONTRATO_SIC");
                    listaTipos.Add(DbType.String);
                    listaValores.Add(filtros.CodigoContrato);
                }

                if (filtros.Nombre != null)
                {
                    listaStrCondiciones.Enqueue(" NOM_TITULAR LIKE @NOM_TITULAR COLLATE SQL_Latin1_General_CP1253_CI_AI");
                    listaNombres.Add("NOM_TITULAR");
                    listaTipos.Add(DbType.String);
                    // TODO ver si esto no rompe todo el sistema
                    listaValores.Add("%" + filtros.Nombre + "%");
                }
                if (filtros.Proveedor != null && Existe==false)
                {
                    listaStrCondiciones.Enqueue(" PROVEEDOR = @PROVEEDOR");
                    listaNombres.Add("PROVEEDOR");
                    listaTipos.Add(DbType.String);
                    listaValores.Add(filtros.Proveedor);
                }

                if (filtros.Apellido1 != null)
                {
                    listaStrCondiciones.Enqueue(" APELLIDO1 LIKE @APELLIDO1 COLLATE SQL_Latin1_General_CP1253_CI_AI");
                    listaNombres.Add("APELLIDO1");
                    listaTipos.Add(DbType.String);
                    // TODO ver si esto no rompe todo el sistema
                    listaValores.Add("%" + filtros.Apellido1 + "%");
                }

                if (filtros.Apellido2 != null)
                {
                    listaStrCondiciones.Enqueue(" APELLIDO2 LIKE @APELLIDO2 COLLATE SQL_Latin1_General_CP1253_CI_AI");
                    listaNombres.Add("APELLIDO2");
                    listaTipos.Add(DbType.String);
                    // TODO ver si esto no rompe todo el sistema
                    listaValores.Add("%" + filtros.Apellido2 + "%");
                }
                ////if (filtros.CodigoDeProvincia != null)
                ////{
                ////    listaStrCondiciones.Enqueue(" COD_PROVINCIA = @COD_PROVINCIA");
                ////    listaNombres.Add("COD_PROVINCIA");
                ////    listaTipos.Add(DbType.String);
                ////    listaValores.Add(filtros.CodigoDeProvincia);
                ////}

                ////if (filtros.CodigoDePoblacion != null)
                ////{
                ////    listaStrCondiciones.Enqueue(" COD_POBLACION = @COD_POBLACION");
                ////    listaNombres.Add("COD_POBLACION");
                ////    listaTipos.Add(DbType.String);
                ////    listaValores.Add(filtros.CodigoDePoblacion);
                ////}

                

                //if (filtros.CodigoPostal != null)
                //{
                //    listaStrCondiciones.Enqueue(" COD_POSTAL = @COD_POSTAL");
                //    listaNombres.Add("COD_POSTAL");
                //    listaTipos.Add(DbType.String);
                //    listaValores.Add(filtros.CodigoPostal);
                //}

                ////if (filtros.DescripcionTipoUrgencia != null && filtros.DescripcionTipoUrgencia != "-- TODAS --")
                ////{
                ////    listaStrCondiciones.Enqueue(" DESC_TIPO_URGENCIA = @DESC_TIPO_URGENCIA");
                ////    listaNombres.Add("DESC_TIPO_URGENCIA");
                ////    listaTipos.Add(DbType.String);
                ////    listaValores.Add(filtros.DescripcionTipoUrgencia);
                ////}

                

                if (filtros.Campania != null)
                {
                    listaStrCondiciones.Enqueue(" V.CAMPANIA = @CAMPANIA");
                    listaNombres.Add("CAMPANIA");
                    listaTipos.Add(DbType.Int32);
                    listaValores.Add(filtros.Campania);
                }

                if (filtros.IdLote != null)
                {
                    listaStrCondiciones.Enqueue(" V.ID_LOTE = @ID_LOTE");
                    listaNombres.Add("ID_LOTE");
                    listaTipos.Add(DbType.Int32);
                    listaValores.Add(filtros.IdLote);
                }

                if (filtros.FechaMinimaLimiteVisita.HasValue)
                {
                    listaStrCondiciones.Enqueue(" V.FEC_LIMITE_VISITA >= @FEC_LIMITE_VISITA_MIN");
                    listaNombres.Add("FEC_LIMITE_VISITA_MIN");
                    listaTipos.Add(DbType.Date);
                    listaValores.Add(filtros.FechaMinimaLimiteVisita.Value);
                }
                if (filtros.FechaMaximaLimiteVisita.HasValue)
                {
                    listaStrCondiciones.Enqueue(" V.FEC_LIMITE_VISITA <= @FEC_LIMITE_VISITA_MAX");
                    listaNombres.Add("FEC_LIMITE_VISITA_MAX");
                    listaTipos.Add(DbType.Date);
                    listaValores.Add(filtros.FechaMaximaLimiteVisita.Value);
                }
                if (filtros.FechaMinimaLote.HasValue)
                {
                    listaStrCondiciones.Enqueue(" FECHA_LOTE >= @FECHA_LOTE_MIN");
                    listaNombres.Add("FECHA_LOTE_MIN");
                    listaTipos.Add(DbType.Date);
                    listaValores.Add(filtros.FechaMinimaLote.Value);
                }
                if (filtros.FechaMaximaLote.HasValue)
                {
                    listaStrCondiciones.Enqueue(" FECHA_LOTE <= @FECHA_LOTE_MAX");
                    listaNombres.Add("FECHA_LOTE_MAX");
                    listaTipos.Add(DbType.Date);
                    listaValores.Add(filtros.FechaMaximaLote.Value);
                }
                if (filtros.FechaMinimaAltaServicio.HasValue)
                {
                    listaStrCondiciones.Enqueue(" FEC_ALTA_SERVICIO >= @FEC_ALTA_SERVICIO_MIN");
                    listaNombres.Add("FEC_ALTA_SERVICIO_MIN");
                    listaTipos.Add(DbType.Date);
                    listaValores.Add(filtros.FechaMinimaAltaServicio.Value);
                }
                if (filtros.FechaMaximaAltaServicio.HasValue)
                {
                    listaStrCondiciones.Enqueue(" FEC_ALTA_SERVICIO <= @FEC_ALTA_SERVICIO_MAX");
                    listaNombres.Add("FEC_ALTA_SERVICIO_MAX");
                    listaTipos.Add(DbType.Date);
                    listaValores.Add(filtros.FechaMaximaAltaServicio.Value);
                }

                #endregion

                
            }
            if (listaStrCondiciones.Count > 0)
            {
                //Se asignan las listas pues tienen algún valor
                if (paramsName != null)
                {
                    listaNombres.AddRange(paramsName);
                }
                paramsName = listaNombres.ToArray();

                if (paramsType != null)
                {
                    listaTipos.AddRange(paramsType);
                }
                paramsType = listaTipos.ToArray();

                if (paramsValue != null)
                {
                    listaValores.AddRange(paramsValue);
                }
                paramsValue = listaValores.ToArray();
            } 
        }

        /// <summary>
        /// Selecciona los filtros activos para cada columna
        /// En cada columna se selecciona la opcion con menos registros (entre seleccionados y no seleccionados)
        /// </summary>
        /// <param name="filtros"></param>
        /// <param name="listaStrCondiciones"></param>
        /// <param name="paramsName"></param>
        /// <param name="paramsType"></param>
        /// <param name="paramsValue"></param>
        private void ObtenerFiltrosColumnas(VistaContratoCompletoDTO filtros, Queue<string> listaStrCondiciones, ref String[] paramsName, ref DbType[] paramsType, ref object[] paramsValue)
        {
            // Si no hay filtros no se hace nada
            if (filtros != null)
            {
                // Si la lista no exite la crea
                if (listaStrCondiciones == null)
                {
                    listaStrCondiciones = new Queue<string>();
                }

                // Si no existe ninguna lista no hay que hacer nada
                if (filtros.ListasFiltrosColumna != null)
                {
                    List<String> listaNombres = new List<String>();
                    List<DbType> listaTipos = new List<DbType>();
                    List<Object> listaValores = new List<Object>();

                    // Se recoge la lista de cada columna almacenada
                    foreach (DictionaryEntry lista in filtros.ListasFiltrosColumna)
                    {
                        // Variables para almacenar los datos seleccionados
                        Queue<string> listaCondicionesSeleccionadas = new Queue<string>();
                        List<String> listaNombresSeleccionadas = new List<String>();
                        List<DbType> listaTiposSeleccionadas = new List<DbType>();
                        List<Object> listaValoresSeleccionadas = new List<Object>();

                        // Variables para almacenar los datos no seleccionados
                        Queue<string> listaCondicionesSinSeleccionar = new Queue<string>();
                        List<String> listaNombresSinSeleccionar = new List<String>();
                        List<DbType> listaTiposSinSeleccionar = new List<DbType>();
                        List<Object> listaValoresSinSeleccionar = new List<Object>();

                        // Variable para llevar el contador de elementos tratados y poder identificar las variables
                        int contador = 0;

                        // Se recorre cada elemento de la lista para almacenar sus datos
                        foreach (ObjetoTextoValorDTO elemento in ((List<ObjetoTextoValorDTO>)lista.Value))
                        {
                            if (elemento.seleccionado)
                            {
                                // Si el elemento es nulo se asigna la cadena especial, sino se almacena el dato (nombre, tipo y valor)
                                if (elemento.valor.GetType() == typeof(System.DBNull))
                                {
                                    listaCondicionesSeleccionadas.Enqueue(lista.Key + " is null");
                                }
                                else
                                {
                                    // Al estar seleccionado la sentencia consiste en una serie de ORs que preguntan si el campo es igual al valor
                                    listaCondicionesSeleccionadas.Enqueue(lista.Key + " =@" + lista.Key.ToString() + contador);
                                    // Se guarda el nombre que tendra la variable                                    
                                    listaNombresSeleccionadas.Add(lista.Key.ToString() + contador);
                                    // Se guarda el tipo de la variable
                                    listaTiposSeleccionadas.Add(DataBaseLibrary.ConvertToDbType(elemento.valor));
                                    // Se guarda el valor de la variable
                                    listaValoresSeleccionadas.Add(elemento.valor);
                                }
                            }
                            else
                            {
                                // Si el elemento es nulo se asigna la cadena especial, sino se almacena el dato (nombre, tipo y valor)
                                if (elemento.valor.GetType() == typeof(System.DBNull))
                                {
                                    listaCondicionesSinSeleccionar.Enqueue(lista.Key + " is not null");
                                }
                                else
                                {
                                    // Al no estar seleccionado la sentencia consiste en una serie de ANDs que preguntan si el campo no es igual al valor
                                    listaCondicionesSinSeleccionar.Enqueue(lista.Key + " !=@" + lista.Key.ToString() + contador);
                                    // Se guarda el nombre que tendra la variable  
                                    listaNombresSinSeleccionar.Add(lista.Key.ToString() + contador);
                                    // Se guarda el tipo de la variable
                                    listaTiposSinSeleccionar.Add(DataBaseLibrary.ConvertToDbType(elemento.valor));
                                    // Se guarda el valor de la variable
                                    listaValoresSinSeleccionar.Add(elemento.valor);
                                }
                            }
                            // Se avanza el contador
                            contador++;
                        }

                        // Si la lista tiene elementos es que hay alguna condicion seleccionada
                        // Si el tamanio de ambas listas es igual significa que todas estan seleccionadas, con lo que no hay que aplicar ningun filtro
                        if (listaCondicionesSeleccionadas.Count > 0 && listaCondicionesSeleccionadas.Count != ((List<ObjetoTextoValorDTO>)lista.Value).Count)
                        {
                            // Mirar cual de las listas es mayor
                            if (listaCondicionesSeleccionadas.Count < listaCondicionesSinSeleccionar.Count)
                            {
                                // Se utilizara la lista de seleccionadas por ser menor que la otra
                                // Al ser una concatenacion de ORs es necesario que todas esten entre parentesis
                                StringBuilder condicion = new StringBuilder("(");
                                // Se recogen todas las sentencias
                                while (listaCondicionesSeleccionadas.Count > 0)
                                {
                                    //Se aniade la sentencia
                                    condicion.Append(listaCondicionesSeleccionadas.Dequeue());
                                    //Se aniade el OR si hay mas sentencias
                                    if (listaCondicionesSeleccionadas.Count > 0)
                                    {
                                        condicion.Append(" OR ");
                                    }
                                }
                                // Se aniade el parentesis de cierre
                                condicion.Append(")");
                                // Se aniade la condicion a la lista de condiciones
                                listaStrCondiciones.Enqueue(condicion.ToString());

                                // Se aniaden los datos de las variables generadas
                                listaNombres.AddRange(listaNombresSeleccionadas);
                                listaTipos.AddRange(listaTiposSeleccionadas);
                                listaValores.AddRange(listaValoresSeleccionadas);
                            }
                            else
                            {
                                // Se utilizara la lista de no seleccionadas por ser menor que la otra
                                // Al ser una concatenacion de ANDs solo es necesario aniadir cada sentencia en la lista general
                                while (listaCondicionesSinSeleccionar.Count > 0)
                                {
                                    listaStrCondiciones.Enqueue(listaCondicionesSinSeleccionar.Dequeue());
                                }

                                // Se aniaden los datos de las variables generadas
                                listaNombres.AddRange(listaNombresSinSeleccionar);
                                listaTipos.AddRange(listaTiposSinSeleccionar);
                                listaValores.AddRange(listaValoresSinSeleccionar);
                            }
                        }
                    }

                    // Solo se asignan las listas si tienen algun valor
                    if (listaStrCondiciones.Count > 0)
                    {
                        // Si ya hay parametros se respetan los valores
                        if (paramsName != null)
                        {
                            listaNombres.AddRange(paramsName);
                        }
                        if (paramsType != null)
                        {
                            listaTipos.AddRange(paramsType);
                        }
                        if (paramsValue != null)
                        {
                            listaValores.AddRange(paramsValue);
                        }

                        // Se devuelven las listas con todos los valores
                        paramsName = listaNombres.ToArray();
                        paramsType = listaTipos.ToArray();
                        paramsValue = listaValores.ToArray();
                    }
                }
            }
        }

        /// <summary>
        /// Genera la cadena de condiciones a partir de una lista
        /// </summary>
        /// <param name="listaStrCondiciones"></param>
        /// <returns></returns>
        private String ObtenerClausulaCondiciones(Queue<string> listaStrCondiciones)
        {
            StringBuilder strCondiciones = new StringBuilder("");
            if (listaStrCondiciones.Count > 0)
            {
                // Se aniade la clausula WHERE porque habrá alguna condicion
                strCondiciones.Append(" WHERE ");
            }

            // Se recogen todas las sentencias
            while (listaStrCondiciones.Count > 0)
            {
                // Se aniade la sentencia
                strCondiciones.Append(listaStrCondiciones.Dequeue());
                // Se aniade el AND si hay mas sentencias
                if (listaStrCondiciones.Count > 0)
                {
                    strCondiciones.Append(" AND ");
                }
            }
            return strCondiciones.ToString();
        }

        #region conexiones BBDD
        private IDataReader RunQueryDataReader(string query, String[] paramsName, DbType[] paramsType, object[] paramsValue)
        {
            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);

            return db.RunQueryDataReader(query, paramsName, paramsType, paramsValue);
        }
        private DataTable RunQueryDataTable(string query, String[] paramsName, DbType[] paramsType, object[] paramsValue)
        {
            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);

            return db.RunQueryDataTable(query, paramsName, paramsType, paramsValue);
        }

        /// <summary>
        /// RunQueryDataScalar en el que el resultado se sabe que sera un int
        /// </summary>
        /// <param name="query"></param>
        /// <param name="paramsName"></param>
        /// <param name="paramsType"></param>
        /// <param name="paramsValue"></param>
        /// <returns></returns>
        private int RunQueryDataScalar(string query, String[] paramsName, DbType[] paramsType, object[] paramsValue)
        {
            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
            Object resultado = null;

            resultado = db.RunQueryDataScalar(query, paramsName, paramsType, paramsValue);

            if (resultado != null && !resultado.Equals(DBNull.Value))
            {
                return (Int32)resultado;
            }
            else
            {
                return 0;
            }
        }
        private int RunQueryDataScalar(string query)
        {
            return this.RunQueryDataScalar(query, null, null, null);
        }
        #endregion conexiones BBDD

        #endregion

        #region metodos
        public Int32 ObtenerNumRegTotalContratosUltimaVisita(VistaContratoCompletoDTO filtros)
        {
            //StringBuilder strSQL = new StringBuilder("SELECT Count(*) FROM VW_MANTENIMIENTO_ULTIMA_VISITA");

            //if (filtros != null)
            //{
            //    if (filtros.Proveedor != null && filtros.Proveedor != "ADICO")
            //    {
            //        strSQL.Append(" WHERE ");
            //        //strSQL.Append(" PROVEEDOR = '" + filtros.Proveedor + "'");
            //        strSQL.Append(" PROVEEDOR >= '" + filtros.Proveedor + "'");
            //        strSQL.Append(" AND PROVEEDOR <= '" + filtros.Proveedor + "'");
            //    }
            //}

            //return this.RunQueryDataScalar(strSQL.ToString());
            
            //StringBuilder strSQL = new StringBuilder("SELECT Count(*) FROM VW_MANTENIMIENTO_ULTIMA_VISITA");
            //StringBuilder strSQL = new StringBuilder("select count(*) from (select cod_contrato_sic from VW_MANTENIMIENTO_ULTIMA_VISITA ");
            StringBuilder strSQL = new StringBuilder("select count(*) FROM  MANTENIMIENTO AS m INNER JOIN PROVINCIA AS pr ON m.COD_PROVINCIA = pr.COD_PROVINCIA INNER JOIN POBLACION AS PO ON m.COD_POBLACION = PO.COD_POBLACION AND m.COD_PROVINCIA = PO.COD_PROVINCIA ");
            strSQL.Append("WHERE (ISNULL(m.FEC_BAJA_SERVICIO, 0) = 0) ");

            if (filtros != null)
            {
                if (filtros.Proveedor != null && filtros.Proveedor != "ADICO")
                {

                    strSQL.Append(" AND( ");
                    strSQL.Append(" PROVEEDOR >= '" + filtros.Proveedor + "'");
                    strSQL.Append(" AND PROVEEDOR <= '" + filtros.Proveedor + "')");
                }
            }
            //strSQL.Append(" group by(cod_contrato_sic)) v");
            return this.RunQueryDataScalar(strSQL.ToString());
         }

        #region metodos agrupados

       
        /// <summary>
        /// Devuelve los campos requeridos del tipo de vista
        /// </summary>
        /// <param name="tipoVista"></param>
        /// <returns></returns>
        private string ObtenerStrCampos(Enumerados.TipoVistaContratoCompleto tipoVista)
        {
            string strCampos;
            switch (tipoVista)
            {
                case Enumerados.TipoVistaContratoCompleto.SinFiltrar:
                    strCampos = "COD_CONTRATO_SIC,VISITA_AVISO,FEC_VISITA,DES_ESTADO,FEC_LIMITE_VISITA,EFV,SCORING,DESC_TIPO_URGENCIA,T1,T2,T5,DESC_MARCA_CALDERA,NOM_TITULAR,APELLIDO1,APELLIDO2,NOMBRE_PROVINCIA,NOMBRE_POBLACION,TIP_VIA_PUBLICA,NOM_CALLE,TIP_BIS,COD_PORTAL,TIP_ESCALERA,TIP_PISO,TIP_MANO,COD_POSTAL,NUM_TEL_CLIENTE,NUM_TEL_PS,NUM_MOVIL,COD_TARIFA,DESC_TARIFA,ID_LOTE,DESC_LOTE,PROVEEDOR,CAMPANIA,OBSERVACIONES,";
                    strCampos +="FEC_ALTA_CONTRATO,FEC_BAJA_CONTRATO,FEC_ALTA_SERVICIO,FEC_BAJA_SERVICIO,NUM_FACTURA,FACTURADO_PROVEEDOR,FECHA_ENVIO_CARTA,ENVIADA_CARTA,";
                    //TEL: strCampos += "NUM_TELEFONO1,NUM_TELEFONO2,NUM_TELEFONO3,NUM_TELEFONO4,NUM_TELEFONO5,NUM_TELEFONO6,FECHA_LOTE,FEC_RECLAMACION,RECLAMACION,COD_BARRAS,TELEFONO_CLIENTE1,TELEFONO_CLIENTE2,CUPS,COD_VISITA";
                    strCampos += "FECHA_LOTE,FEC_RECLAMACION,RECLAMACION,COD_BARRAS,TELEFONO_CLIENTE1,TELEFONO_CLIENTE2,CUPS,COD_RECEPTOR,COD_VISITA";
                    //strCampos += ",EFV,SCORING";
                    strCampos += ",DESC_CATEGORIA_VISITA";
                    strCampos += ",BAJA_RENOVACION";
                    break;
                case Enumerados.TipoVistaContratoCompleto.PorContrato:
                    strCampos = "COD_CONTRATO_SIC,VISITA_AVISO,FEC_VISITA,DES_ESTADO,FEC_LIMITE_VISITA,EFV,SCORING,DESC_TIPO_URGENCIA,T1,T2,T5,DESC_MARCA_CALDERA,NOM_TITULAR,APELLIDO1,APELLIDO2,NOMBRE_PROVINCIA,NOMBRE_POBLACION,TIP_VIA_PUBLICA,NOM_CALLE,TIP_BIS,COD_PORTAL,TIP_ESCALERA,TIP_PISO,TIP_MANO,COD_POSTAL,NUM_TEL_CLIENTE,NUM_TEL_PS,NUM_MOVIL,COD_TARIFA,DESC_TARIFA,ID_LOTE,DESC_LOTE,PROVEEDOR,CAMPANIA,OBSERVACIONES,";
                    strCampos += "FEC_ALTA_CONTRATO,FEC_BAJA_CONTRATO,FEC_ALTA_SERVICIO,FEC_BAJA_SERVICIO,NUM_FACTURA,FACTURADO_PROVEEDOR,FECHA_ENVIO_CARTA,ENVIADA_CARTA,";
                    //TEL: strCampos += "NUM_TELEFONO1,NUM_TELEFONO2,NUM_TELEFONO3,NUM_TELEFONO4,NUM_TELEFONO5,NUM_TELEFONO6,FECHA_LOTE,FEC_RECLAMACION,RECLAMACION,COD_BARRAS,TELEFONO_CLIENTE1,TELEFONO_CLIENTE2,CUPS,COD_VISITA";
                    strCampos += "FECHA_LOTE,FEC_RECLAMACION,RECLAMACION,COD_BARRAS,TELEFONO_CLIENTE1,TELEFONO_CLIENTE2,CUPS,COD_RECEPTOR,COD_VISITA";
                    //strCampos += ",EFV,SCORING";
                    strCampos += ",DESC_CATEGORIA_VISITA";
                    strCampos += ",BAJA_RENOVACION";
                    break;
                case Enumerados.TipoVistaContratoCompleto.FechaUltimaVisita:
                    //TEL: strCampos = "COD_CONTRATO_SIC,VISITA_AVISO,FEC_VISITA,FEC_LIMITE_VISITA,EFV,SCORING,DES_ESTADO,NOM_TITULAR,APELLIDO1,APELLIDO2,NOMBRE_PROVINCIA,NOMBRE_POBLACION,TIP_VIA_PUBLICA,NOM_CALLE,TIP_BIS,COD_PORTAL,TIP_ESCALERA,TIP_PISO,TIP_MANO,COD_POSTAL,NUM_TEL_CLIENTE,NUM_TEL_PS,COD_TARIFA,DESC_TARIFA,ID_LOTE,DESC_LOTE,T1,T2,DESC_MARCA_CALDERA,FEC_ALTA_CONTRATO,T5,CAMPANIA,OBSERVACIONES,PROVEEDOR,FEC_BAJA_CONTRATO,FEC_ALTA_SERVICIO,FEC_BAJA_SERVICIO,NUM_FACTURA,FACTURADO_PROVEEDOR,FECHA_ENVIO_CARTA,ENVIADA_CARTA,NUM_TELEFONO1,NUM_TELEFONO2,NUM_TELEFONO3,NUM_TELEFONO4,NUM_TELEFONO5,NUM_TELEFONO6,FEC_RECLAMACION,RECLAMACION,COD_BARRAS,TELEFONO_CLIENTE1,TELEFONO_CLIENTE2,CUPS,COD_VISITA";
                    strCampos = "COD_CONTRATO_SIC,VISITA_AVISO,FEC_VISITA,FEC_LIMITE_VISITA,EFV,SCORING,DES_ESTADO,NOM_TITULAR,APELLIDO1,APELLIDO2,NOMBRE_PROVINCIA,NOMBRE_POBLACION,TIP_VIA_PUBLICA,NOM_CALLE,TIP_BIS,COD_PORTAL,TIP_ESCALERA,TIP_PISO,TIP_MANO,COD_POSTAL,NUM_TEL_CLIENTE,NUM_TEL_PS,NUM_MOVIL,COD_TARIFA,DESC_TARIFA,ID_LOTE,DESC_LOTE,T1,T2,DESC_MARCA_CALDERA,FEC_ALTA_CONTRATO,T5,CAMPANIA,OBSERVACIONES,PROVEEDOR,FEC_BAJA_CONTRATO,FEC_ALTA_SERVICIO,FEC_BAJA_SERVICIO,NUM_FACTURA,FACTURADO_PROVEEDOR,FECHA_ENVIO_CARTA,ENVIADA_CARTA,FEC_RECLAMACION,RECLAMACION,COD_BARRAS,TELEFONO_CLIENTE1,TELEFONO_CLIENTE2,CUPS,COD_RECEPTOR,COD_VISITA";
                    //strCampos += ",EFV,SCORING";
                    strCampos += ",DESC_CATEGORIA_VISITA";
                    strCampos += ",BAJA_RENOVACION";
                    /*
                     TODO: COMPROBAR SI HAY QUE SACAR LOS SIGUIENTES CAMPOS-->
                    -FALTA FEC_PLANIF_VISITA CAMPO 3
                     DUDAS: PROVEEDOR NO ESTA EN LA TABLA ACCESS PERO EL EXCEL ME LO HA EXPORTADO, LA DUDA ES ¿SI SOY ADMINISTRADOR
                     ME MUESTRA LOS PROVEEDORES Y SI NO NO?
                     */
                    break;
                case Enumerados.TipoVistaContratoCompleto.Facturadas:
                    //TEL: strCampos = "COD_CONTRATO_SIC,VISITA_AVISO,FECHA_FACTURA,FEC_VISITA,FEC_LIMITE_VISITA,EFV,SCORING,DES_ESTADO,T1,T2,T5,DESC_MARCA_CALDERA,NOM_TITULAR,APELLIDO1,APELLIDO2,NOMBRE_PROVINCIA,NOMBRE_POBLACION,TIP_VIA_PUBLICA,NOM_CALLE,TIP_BIS,COD_PORTAL,TIP_ESCALERA,TIP_PISO,TIP_MANO,COD_POSTAL,NUM_TEL_CLIENTE,NUM_TEL_PS,COD_TARIFA,CAMPANIA,OBSERVACIONES,ID_LOTE,DESC_LOTE,PROVEEDOR,FEC_ALTA_CONTRATO,FEC_BAJA_CONTRATO,FEC_ALTA_SERVICIO,FEC_BAJA_SERVICIO,NUM_FACTURA,FACTURADO_PROVEEDOR,FECHA_ENVIO_CARTA,ENVIADA_CARTA,NUM_TELEFONO1,NUM_TELEFONO2,NUM_TELEFONO3,NUM_TELEFONO4,NUM_TELEFONO5,NUM_TELEFONO6,FEC_RECLAMACION,RECLAMACION,COD_BARRAS,TELEFONO_CLIENTE1,TELEFONO_CLIENTE2,CUPS,COD_VISITA";
                    strCampos = "COD_CONTRATO_SIC,VISITA_AVISO,FECHA_FACTURA,FEC_VISITA,FEC_LIMITE_VISITA,EFV,SCORING,DES_ESTADO,T1,T2,T5,DESC_MARCA_CALDERA,NOM_TITULAR,APELLIDO1,APELLIDO2,NOMBRE_PROVINCIA,NOMBRE_POBLACION,TIP_VIA_PUBLICA,NOM_CALLE,TIP_BIS,COD_PORTAL,TIP_ESCALERA,TIP_PISO,TIP_MANO,COD_POSTAL,NUM_TEL_CLIENTE,NUM_TEL_PS,NUM_MOVIL,COD_TARIFA,CAMPANIA,OBSERVACIONES,ID_LOTE,DESC_LOTE,PROVEEDOR,FEC_ALTA_CONTRATO,FEC_BAJA_CONTRATO,FEC_ALTA_SERVICIO,FEC_BAJA_SERVICIO,NUM_FACTURA,FACTURADO_PROVEEDOR,FECHA_ENVIO_CARTA,ENVIADA_CARTA,FEC_RECLAMACION,RECLAMACION,COD_BARRAS,TELEFONO_CLIENTE1,TELEFONO_CLIENTE2,CUPS,COD_RECEPTOR,COD_VISITA";
                    strCampos += ",DESC_CATEGORIA_VISITA";
                    strCampos += ",BAJA_RENOVACION";
                    //strCampos += ",EFV,SCORING";
                    /*TODO: COMPROBAR SI HAY QUE SACAR LOS SIGUIENTES CAMPOS-->
                        -FALTA FEC_PLANIF_VISITA CAMPO 4    
                    */
                    break;
                case Enumerados.TipoVistaContratoCompleto.EstadoVisita:
                    //TEL: strCampos = "COD_CONTRATO_SIC,VISITA_AVISO,DES_ESTADO,FEC_LIMITE_VISITA,EFV,SCORING,FEC_VISITA,NOM_TITULAR,APELLIDO1,APELLIDO2,NOMBRE_PROVINCIA,NOMBRE_POBLACION,TIP_VIA_PUBLICA,NOM_CALLE,TIP_BIS,COD_PORTAL,TIP_ESCALERA,TIP_PISO,TIP_MANO,COD_POSTAL,NUM_TEL_CLIENTE,NUM_TEL_PS,COD_TARIFA,DESC_TARIFA,ID_LOTE,DESC_LOTE,T1,T2,T5,PROVEEDOR,DESC_TIPO_URGENCIA,CAMPANIA,OBSERVACIONES,DESC_MARCA_CALDERA,FEC_ALTA_CONTRATO,FEC_BAJA_CONTRATO,FEC_ALTA_SERVICIO,FEC_BAJA_SERVICIO,NUM_FACTURA,FACTURADO_PROVEEDOR,FECHA_ENVIO_CARTA,ENVIADA_CARTA,NUM_TELEFONO1,NUM_TELEFONO2,NUM_TELEFONO3,NUM_TELEFONO4,NUM_TELEFONO5,NUM_TELEFONO6,FEC_RECLAMACION,RECLAMACION,COD_BARRAS,TELEFONO_CLIENTE1,TELEFONO_CLIENTE2,CUPS,COD_VISITA";
                    strCampos = "COD_CONTRATO_SIC,VISITA_AVISO,DES_ESTADO,FEC_LIMITE_VISITA,EFV,SCORING,FEC_VISITA,NOM_TITULAR,APELLIDO1,APELLIDO2,NOMBRE_PROVINCIA,NOMBRE_POBLACION,TIP_VIA_PUBLICA,NOM_CALLE,TIP_BIS,COD_PORTAL,TIP_ESCALERA,TIP_PISO,TIP_MANO,COD_POSTAL,NUM_TEL_CLIENTE,NUM_TEL_PS,NUM_MOVIL, COD_TARIFA,DESC_TARIFA,ID_LOTE,DESC_LOTE,T1,T2,T5,PROVEEDOR,DESC_TIPO_URGENCIA,CAMPANIA,OBSERVACIONES,DESC_MARCA_CALDERA,FEC_ALTA_CONTRATO,FEC_BAJA_CONTRATO,FEC_ALTA_SERVICIO,FEC_BAJA_SERVICIO,NUM_FACTURA,FACTURADO_PROVEEDOR,FECHA_ENVIO_CARTA,ENVIADA_CARTA,FEC_RECLAMACION,RECLAMACION,COD_BARRAS,TELEFONO_CLIENTE1,TELEFONO_CLIENTE2,CUPS,COD_RECEPTOR,COD_VISITA";
                    strCampos += ",DESC_CATEGORIA_VISITA";
                    strCampos += ",BAJA_RENOVACION";
                    //strCampos += ",EFV,SCORING";
                    /*TODO: COMPROBAR SI HAY QUE SACAR LOS SIGUIENTES CAMPOS-->
                   -FALTA FEC_PLANIF_VISITA CAMPO 5    
                    */
                    break;
                case Enumerados.TipoVistaContratoCompleto.PorLote:
                    //TEL: strCampos = "COD_CONTRATO_SIC,VISITA_AVISO,ID_LOTE,DESC_LOTE,FEC_VISITA,FEC_LIMITE_VISITA,EFV,SCORING,DES_ESTADO,NOM_TITULAR,APELLIDO1,APELLIDO2,NOMBRE_PROVINCIA,NOMBRE_POBLACION,TIP_VIA_PUBLICA,NOM_CALLE,TIP_BIS,COD_PORTAL,TIP_ESCALERA,TIP_PISO,TIP_MANO,COD_POSTAL,NUM_TEL_CLIENTE,NUM_TEL_PS,COD_TARIFA,DESC_TARIFA,T1,T2,T5,DESC_MARCA_CALDERA,CAMPANIA,OBSERVACIONES,DESC_TIPO_URGENCIA,FEC_ALTA_CONTRATO,FEC_BAJA_CONTRATO,FEC_ALTA_SERVICIO,FEC_BAJA_SERVICIO,NUM_FACTURA,FACTURADO_PROVEEDOR,FECHA_ENVIO_CARTA,ENVIADA_CARTA,NUM_TELEFONO1,NUM_TELEFONO2,NUM_TELEFONO3,NUM_TELEFONO4,NUM_TELEFONO5,NUM_TELEFONO6,FEC_RECLAMACION,RECLAMACION,COD_BARRAS,TELEFONO_CLIENTE1,TELEFONO_CLIENTE2,CUPS,COD_VISITA";
                    strCampos = "COD_CONTRATO_SIC,VISITA_AVISO,ID_LOTE,DESC_LOTE,FEC_VISITA,FEC_LIMITE_VISITA,EFV,SCORING,DES_ESTADO,NOM_TITULAR,APELLIDO1,APELLIDO2,NOMBRE_PROVINCIA,NOMBRE_POBLACION,TIP_VIA_PUBLICA,NOM_CALLE,TIP_BIS,COD_PORTAL,TIP_ESCALERA,TIP_PISO,TIP_MANO,COD_POSTAL,NUM_TEL_CLIENTE,NUM_TEL_PS,NUM_MOVIL, COD_TARIFA,DESC_TARIFA,T1,T2,T5,DESC_MARCA_CALDERA,CAMPANIA,OBSERVACIONES,DESC_TIPO_URGENCIA,FEC_ALTA_CONTRATO,FEC_BAJA_CONTRATO,FEC_ALTA_SERVICIO,FEC_BAJA_SERVICIO,NUM_FACTURA,FACTURADO_PROVEEDOR,FECHA_ENVIO_CARTA,ENVIADA_CARTA,FEC_RECLAMACION,RECLAMACION,COD_BARRAS,TELEFONO_CLIENTE1,TELEFONO_CLIENTE2,CUPS,COD_RECEPTOR,COD_VISITA";
                    strCampos += ",DESC_CATEGORIA_VISITA";
                    strCampos += ",BAJA_RENOVACION";
                    //strCampos += ",EFV,SCORING";
                    /*TODO: COMPROBAR SI HAY QUE SACAR LOS SIGUIENTES CAMPOS-->
                    -FALTA FEC_PLANIF_VISITA CAMPO 5
                     */
                    break;
                case Enumerados.TipoVistaContratoCompleto.NoCerradasUrgencia:
                    //TEL: strCampos = "COD_CONTRATO_SIC,VISITA_AVISO,NOM_TITULAR,APELLIDO1,APELLIDO2,NOMBRE_PROVINCIA,NOMBRE_POBLACION,NOM_CALLE,TIP_VIA_PUBLICA,TIP_BIS,COD_PORTAL,TIP_ESCALERA,TIP_PISO,TIP_MANO,COD_POSTAL,NUM_TEL_CLIENTE,NUM_TEL_PS,FEC_LIMITE_VISITA,EFV,SCORING,DESC_TIPO_URGENCIA,DES_ESTADO,T1,T2,T5,DESC_MARCA_CALDERA,PROVEEDOR,FEC_ALTA_CONTRATO,FEC_BAJA_CONTRATO,FEC_ALTA_SERVICIO,FEC_BAJA_SERVICIO,NUM_FACTURA,FACTURADO_PROVEEDOR,CAMPANIA,OBSERVACIONES,FECHA_ENVIO_CARTA,ENVIADA_CARTA,NUM_TELEFONO1,NUM_TELEFONO2,NUM_TELEFONO3,NUM_TELEFONO4,NUM_TELEFONO5,NUM_TELEFONO6,FEC_RECLAMACION,RECLAMACION,COD_BARRAS,TELEFONO_CLIENTE1,TELEFONO_CLIENTE2,CUPS,COD_VISITA";
                    strCampos = "COD_CONTRATO_SIC,VISITA_AVISO,NOM_TITULAR,APELLIDO1,APELLIDO2,NOMBRE_PROVINCIA,NOMBRE_POBLACION,NOM_CALLE,TIP_VIA_PUBLICA,TIP_BIS,COD_PORTAL,TIP_ESCALERA,TIP_PISO,TIP_MANO,COD_POSTAL,NUM_TEL_CLIENTE,NUM_TEL_PS,NUM_MOVIL,FEC_LIMITE_VISITA,EFV,SCORING,DESC_TIPO_URGENCIA,DES_ESTADO,T1,T2,T5,DESC_MARCA_CALDERA,PROVEEDOR,FEC_ALTA_CONTRATO,FEC_BAJA_CONTRATO,FEC_ALTA_SERVICIO,FEC_BAJA_SERVICIO,NUM_FACTURA,FACTURADO_PROVEEDOR,CAMPANIA,OBSERVACIONES,FECHA_ENVIO_CARTA,ENVIADA_CARTA,FEC_RECLAMACION,RECLAMACION,COD_BARRAS,TELEFONO_CLIENTE1,TELEFONO_CLIENTE2,CUPS,COD_RECEPTOR,COD_VISITA";
                    strCampos += ",DESC_CATEGORIA_VISITA";
                    strCampos += ",BAJA_RENOVACION";
                    //strCampos += ",EFV,SCORING";
                    break;
                case Enumerados.TipoVistaContratoCompleto.EjecucionOAplazadas:
                    //TEL: strCampos = "COD_CONTRATO_SIC,VISITA_AVISO,NOM_TITULAR,APELLIDO1,APELLIDO2,NOMBRE_PROVINCIA,NOMBRE_POBLACION,NOM_CALLE,TIP_VIA_PUBLICA,TIP_BIS,COD_PORTAL,TIP_ESCALERA,TIP_PISO,TIP_MANO,COD_POSTAL,NUM_TEL_CLIENTE,NUM_TEL_PS,FEC_VISITA,FEC_LIMITE_VISITA,EFV,SCORING,DESC_TIPO_URGENCIA,DES_ESTADO,FEC_ALTA_CONTRATO,FEC_BAJA_CONTRATO,FEC_ALTA_SERVICIO,FEC_BAJA_SERVICIO,COD_TARIFA,T1,T2,T5,ID_LOTE,FACTURADO_PROVEEDOR,FECHA_FACTURA,NUM_FACTURA,OBSERVACIONES,ENVIADA_CARTA,FECHA_ENVIO_CARTA,NUM_TELEFONO1,NUM_TELEFONO2,NUM_TELEFONO3,NUM_TELEFONO4,NUM_TELEFONO5,NUM_TELEFONO6,FEC_RECLAMACION,RECLAMACION,COD_BARRAS,TELEFONO_CLIENTE1,TELEFONO_CLIENTE2,CUPS,COD_VISITA";
                    strCampos = "COD_CONTRATO_SIC,VISITA_AVISO,NOM_TITULAR,APELLIDO1,APELLIDO2,NOMBRE_PROVINCIA,NOMBRE_POBLACION,NOM_CALLE,TIP_VIA_PUBLICA,TIP_BIS,COD_PORTAL,TIP_ESCALERA,TIP_PISO,TIP_MANO,COD_POSTAL,NUM_TEL_CLIENTE,NUM_TEL_PS,NUM_MOVIL,FEC_VISITA,FEC_LIMITE_VISITA,EFV,SCORING,DESC_TIPO_URGENCIA,DES_ESTADO,FEC_ALTA_CONTRATO,FEC_BAJA_CONTRATO,FEC_ALTA_SERVICIO,FEC_BAJA_SERVICIO,COD_TARIFA,T1,T2,T5,ID_LOTE,FACTURADO_PROVEEDOR,FECHA_FACTURA,NUM_FACTURA,OBSERVACIONES,ENVIADA_CARTA,FECHA_ENVIO_CARTA,FEC_RECLAMACION,RECLAMACION,COD_BARRAS,TELEFONO_CLIENTE1,TELEFONO_CLIENTE2,CUPS,COD_RECEPTOR,COD_VISITA";
                    strCampos += ",DESC_CATEGORIA_VISITA";
                    strCampos += ",BAJA_RENOVACION";
                    //strCampos += ",EFV,SCORING";
                    /*TODO: COMPROBAR SI HAY QUE SACAR LOS SIGUIENTES CAMPOS-->
                    -FALTA FEC_PLANIF_VISITA CAMPO 21
                    -FALTA ESTADO_CONTRATO CAMPO 25
                    -DESCRIPCION NO SE QUE ES (NO ES LA DESCRIPCION DEL LOTE) CAMPO 35
                     */
                    break;
                case Enumerados.TipoVistaContratoCompleto.MantenimientoGasCalefaccion:
                    //TEL: strCampos = "COD_CONTRATO_SIC,VISITA_AVISO,T1,T2,T5,FEC_VISITA,DESC_MARCA_CALDERA,DES_ESTADO,NOM_TITULAR,APELLIDO1,APELLIDO2,NOMBRE_PROVINCIA,NOMBRE_POBLACION,TIP_VIA_PUBLICA,NOM_CALLE,TIP_BIS,COD_PORTAL,TIP_ESCALERA,TIP_PISO,TIP_MANO,COD_POSTAL,NUM_TEL_CLIENTE,NUM_TEL_PS,COD_TARIFA,DESC_TARIFA,ID_LOTE,DESC_LOTE,PROVEEDOR,CAMPANIA,FEC_LIMITE_VISITA,EFV,SCORING,DESC_TIPO_URGENCIA,OBSERVACIONES,FEC_ALTA_CONTRATO,FEC_BAJA_CONTRATO,FEC_ALTA_SERVICIO,FEC_BAJA_SERVICIO,NUM_FACTURA,FACTURADO_PROVEEDOR,FECHA_ENVIO_CARTA,ENVIADA_CARTA,NUM_TELEFONO1,NUM_TELEFONO2,NUM_TELEFONO3,NUM_TELEFONO4,NUM_TELEFONO5,NUM_TELEFONO6,FEC_RECLAMACION,RECLAMACION,COD_BARRAS,TELEFONO_CLIENTE1,TELEFONO_CLIENTE2,CUPS,COD_VISITA";
                    strCampos = "COD_CONTRATO_SIC,VISITA_AVISO,T1,T2,T5,FEC_VISITA,DESC_MARCA_CALDERA,DES_ESTADO,NOM_TITULAR,APELLIDO1,APELLIDO2,NOMBRE_PROVINCIA,NOMBRE_POBLACION,TIP_VIA_PUBLICA,NOM_CALLE,TIP_BIS,COD_PORTAL,TIP_ESCALERA,TIP_PISO,TIP_MANO,COD_POSTAL,NUM_TEL_CLIENTE,NUM_TEL_PS,NUM_MOVIL,COD_TARIFA,DESC_TARIFA,ID_LOTE,DESC_LOTE,PROVEEDOR,CAMPANIA,FEC_LIMITE_VISITA,EFV,SCORING,DESC_TIPO_URGENCIA,OBSERVACIONES,FEC_ALTA_CONTRATO,FEC_BAJA_CONTRATO,FEC_ALTA_SERVICIO,FEC_BAJA_SERVICIO,NUM_FACTURA,FACTURADO_PROVEEDOR,FECHA_ENVIO_CARTA,ENVIADA_CARTA,FEC_RECLAMACION,RECLAMACION,COD_BARRAS,TELEFONO_CLIENTE1,TELEFONO_CLIENTE2,CUPS,COD_RECEPTOR,COD_VISITA";
                    strCampos += ",DESC_CATEGORIA_VISITA";
                    strCampos += ",BAJA_RENOVACION";
                    //strCampos += ",EFV,SCORING";
                    /*TODO: COMPROBAR SI HAY QUE SACAR LOS SIGUIENTES CAMPOS-->
                   -FALTA FEC_PLANIF_VISITA CAMPO 6
                    */
                    break;
                case Enumerados.TipoVistaContratoCompleto.MantenimientoGas:
                    //TEL: strCampos = "COD_CONTRATO_SIC,VISITA_AVISO,T1,T2,T5,FEC_VISITA,DESC_MARCA_CALDERA,DES_ESTADO,NOM_TITULAR,APELLIDO1,APELLIDO2,NOMBRE_PROVINCIA,NOMBRE_POBLACION,TIP_VIA_PUBLICA,NOM_CALLE,TIP_BIS,COD_PORTAL,TIP_ESCALERA,TIP_PISO,TIP_MANO,COD_POSTAL,NUM_TEL_CLIENTE,NUM_TEL_PS,COD_TARIFA,DESC_TARIFA,ID_LOTE,DESC_LOTE,PROVEEDOR,CAMPANIA,FEC_LIMITE_VISITA,EFV,SCORING,DESC_TIPO_URGENCIA,OBSERVACIONES,FEC_ALTA_CONTRATO,FEC_BAJA_CONTRATO,FEC_ALTA_SERVICIO,FEC_BAJA_SERVICIO,NUM_FACTURA,FACTURADO_PROVEEDOR,FECHA_ENVIO_CARTA,ENVIADA_CARTA,NUM_TELEFONO1,NUM_TELEFONO2,NUM_TELEFONO3,NUM_TELEFONO4,NUM_TELEFONO5,NUM_TELEFONO6,FEC_RECLAMACION,RECLAMACION,COD_BARRAS,TELEFONO_CLIENTE1,TELEFONO_CLIENTE2,CUPS,COD_VISITA";
                    strCampos = "COD_CONTRATO_SIC,VISITA_AVISO,T1,T2,T5,FEC_VISITA,DESC_MARCA_CALDERA,DES_ESTADO,NOM_TITULAR,APELLIDO1,APELLIDO2,NOMBRE_PROVINCIA,NOMBRE_POBLACION,TIP_VIA_PUBLICA,NOM_CALLE,TIP_BIS,COD_PORTAL,TIP_ESCALERA,TIP_PISO,TIP_MANO,COD_POSTAL,NUM_TEL_CLIENTE,NUM_TEL_PS,NUM_MOVIL,COD_TARIFA,DESC_TARIFA,ID_LOTE,DESC_LOTE,PROVEEDOR,CAMPANIA,FEC_LIMITE_VISITA,EFV,SCORING,DESC_TIPO_URGENCIA,OBSERVACIONES,FEC_ALTA_CONTRATO,FEC_BAJA_CONTRATO,FEC_ALTA_SERVICIO,FEC_BAJA_SERVICIO,NUM_FACTURA,FACTURADO_PROVEEDOR,FECHA_ENVIO_CARTA,ENVIADA_CARTA,FEC_RECLAMACION,RECLAMACION,COD_BARRAS,TELEFONO_CLIENTE1,TELEFONO_CLIENTE2,CUPS,COD_RECEPTOR,COD_VISITA";
                    strCampos += ",DESC_CATEGORIA_VISITA";
                    strCampos += ",BAJA_RENOVACION";
                    //strCampos += ",EFV,SCORING";
                    /*TODO: COMPROBAR SI HAY QUE SACAR LOS SIGUIENTES CAMPOS-->
                   -FALTA FEC_PLANIF_VISITA CAMPO 6
                    */
                    break;
                default:
                    strCampos = "*";
                    break;
            }
            return strCampos;
        }

        /// <summary>
        /// Establece las condiciones propias del tipo de vista
        /// </summary>
        /// <param name="tipoVista"></param>
        /// <param name="parametros"></param>
        /// <param name="listaStrCondiciones"></param>
        private void AplicarCondicion(Enumerados.TipoVistaContratoCompleto tipoVista, Hashtable parametros, Queue<string> listaStrCondiciones)
        {

            switch (tipoVista)
            {
                case Enumerados.TipoVistaContratoCompleto.SinFiltrar:
                    break;
                case Enumerados.TipoVistaContratoCompleto.FechaUltimaVisita:
                    if (parametros[Enumerados.ParametrosContratoCompleto.fechaDesde] != null)
                    {
                        //TODO: MIRAR SI HAY QUE PASARLE PARAMETROS O LO DEJAMOS ASI
                        listaStrCondiciones.Enqueue(" FEC_VISITA >= '" + DateUtils.DateTimeToSQL((DateTime)parametros[Enumerados.ParametrosContratoCompleto.fechaDesde]) + "'");
                    }

                    if (parametros[Enumerados.ParametrosContratoCompleto.fechaHasta] != null)
                    {
                        //TODO: MIRAR SI HAY QUE PASARLE PARAMETROS O LO DEJAMOS ASI
                        listaStrCondiciones.Enqueue(" v.FEC_VISITA <= '" + DateUtils.DateTimeToSQL((DateTime)parametros[Enumerados.ParametrosContratoCompleto.fechaHasta]) + "'");
                    }
                    break;
                case Enumerados.TipoVistaContratoCompleto.Facturadas:
                    if (parametros[Enumerados.ParametrosContratoCompleto.fechaDesde] != null)
                    {
                        listaStrCondiciones.Enqueue(" FECHA_FACTURA >= '" + DateUtils.DateTimeToSQL((DateTime)parametros[Enumerados.ParametrosContratoCompleto.fechaDesde]) + "'");
                    }

                    if (parametros[Enumerados.ParametrosContratoCompleto.fechaHasta] != null)
                    {
                        listaStrCondiciones.Enqueue(" FECHA_FACTURA <= '" + DateUtils.DateTimeToSQL((DateTime)parametros[Enumerados.ParametrosContratoCompleto.fechaHasta]) + "'");
                    }
                    break;
                case Enumerados.TipoVistaContratoCompleto.EstadoVisita:
                    if (parametros[Enumerados.ParametrosContratoCompleto.CodigoVisita] != null)
                    {
                        listaStrCondiciones.Enqueue(" COD_ESTADO_VISITA = '" + parametros[Enumerados.ParametrosContratoCompleto.CodigoVisita] + "'");
                    }
                    break;
                case Enumerados.TipoVistaContratoCompleto.PorLote:
                    if (parametros[Enumerados.ParametrosContratoCompleto.CodigoLote] != null && ((String)parametros[Enumerados.ParametrosContratoCompleto.CodigoLote]).Length > 0)
                    {
                        listaStrCondiciones.Enqueue(" v.ID_LOTE = " + ((String)parametros[Enumerados.ParametrosContratoCompleto.CodigoLote]));
                    }
                    break;
                case Enumerados.TipoVistaContratoCompleto.NoCerradasUrgencia:
                    listaStrCondiciones.Enqueue(" v.COD_ESTADO_VISITA <> '" + StringEnum.GetStringValue(Enumerados.EstadosVisita.Cerrada) + "'");
                    listaStrCondiciones.Enqueue(" v.COD_ESTADO_VISITA <> '" + StringEnum.GetStringValue(Enumerados.EstadosVisita.CerradaPendienteRealizarReparacion) + "'");

                    if (parametros[Enumerados.ParametrosContratoCompleto.CodigoUrgencia] != null && ((String)parametros[Enumerados.ParametrosContratoCompleto.CodigoUrgencia]) != "")
                    {
                        listaStrCondiciones.Enqueue(" v.ID_TIPO_URGENCIA = " + parametros[Enumerados.ParametrosContratoCompleto.CodigoUrgencia]);
                    }
                    break;
                case Enumerados.TipoVistaContratoCompleto.EjecucionOAplazadas:
                    listaStrCondiciones.Enqueue("( v.COD_ESTADO_VISITA = '" + StringEnum.GetStringValue(Enumerados.EstadosVisita.EnEjecución) + "'" +
                                                " OR v.COD_ESTADO_VISITA = '" + StringEnum.GetStringValue(Enumerados.EstadosVisita.Aplazada) + "')");
                    break;
                case Enumerados.TipoVistaContratoCompleto.MantenimientoGasCalefaccion:
                    listaStrCondiciones.Enqueue("T1= 'S'");
                    break;
                case Enumerados.TipoVistaContratoCompleto.MantenimientoGas:
                    listaStrCondiciones.Enqueue("T2= 'S'");
                    break;
                case Enumerados.TipoVistaContratoCompleto.PorContrato:
                    if (parametros[Enumerados.ParametrosContratoCompleto.Contrato] != null)
                    {
                        listaStrCondiciones.Enqueue(" rcc.COD_CONTRATO_SIC in ('" + parametros[Enumerados.ParametrosContratoCompleto.Contrato] + "')");//" COD_CONTRATO_SIC in (select COD_CONTRATO_SIC from relacion_cups_contrato where COD_RECEPTOR in (select COD_RECEPTOR from relacion_cups_contrato where cod_contrato_sic='" + parametros[Enumerados.ParametrosContratoCompleto.Contrato] + "'))");
                        //listaStrCondiciones.Enqueue(" COD_CONTRATO_SIC in (select COD_CONTRATO_SIC from relacion_cups_contrato where COD_RECEPTOR in (select COD_RECEPTOR from relacion_cups_contrato where cod_contrato_sic='" + parametros[Enumerados.ParametrosContratoCompleto.Contrato] + "'))");
                    }
                    break;
            }
        }

        /// <summary>
        /// Devuelve el numero de registros que satisfacen todas las condiciones
        /// </summary>
        /// <param name="tipoVista"></param>
        /// <param name="parametros"></param>
        /// <param name="filtros"></param>
        /// <returns></returns>
        public Int32 ObtenerNumRegEncontrados(Enumerados.TipoVistaContratoCompleto tipoVista, Hashtable parametros, VistaContratoCompletoDTO filtros)
        {
            // Sentencia SQL que se ejecutara
            //StringBuilder strSQL = new StringBuilder("SELECT Count(*) FROM ");
            StringBuilder strSQL = new StringBuilder("");
            //"VW_MANTENIMIENTO_ULTIMA_VISITA");
            //strSQL.Append("(SELECT DISTINCT ");
            strSQL.Append("SELECT count(DISTINCT m.COD_CONTRATO_SIC) ");
            //strSQL.Append("m.COD_CONTRATO_SIC, rcc.cod_contrato_sic as contrato, v.FEC_VISITA, v.FEC_LIMITE_VISITA, TU.DESC_TIPO_URGENCIA,  ");
            //strSQL.Append("EV.COD_ESTADO, EV.DES_ESTADO, m.T1, m.T2, m.T5, MC.DESC_MARCA_CALDERA, m.NOM_TITULAR, m.APELLIDO1, m.APELLIDO2,  ");
            //strSQL.Append("m.COD_PROVINCIA, pr.NOMBRE AS NOMBRE_PROVINCIA, m.COD_POBLACION, PO.NOMBRE AS NOMBRE_POBLACION, m.TIP_VIA_PUBLICA,  ");
            //strSQL.Append("m.NOM_CALLE, m.COD_FINCA, m.TIP_BIS, m.COD_PORTAL, m.TIP_ESCALERA, m.TIP_PISO, m.TIP_MANO, m.COD_POSTAL, m.NUM_TEL_CLIENTE,  ");
            //strSQL.Append("m.NUM_TEL_PS, m.NUM_MOVIL, m.COD_TARIFA, ");
            //strSQL.Append("TG.DESC_TARIFA, ");
            //strSQL.Append("v.CAMPANIA, CAM.OBSERVACIONES, L.ID_LOTE, L.DESC_LOTE, m.PROVEEDOR,  ");
            //strSQL.Append("m.FEC_ALTA_CONTRATO, m.FEC_BAJA_CONTRATO, m.FEC_ALTA_SERVICIO, m.FEC_BAJA_SERVICIO,  ");
            //strSQL.Append("CASE V.FACTURADO_PROVEEDOR WHEN 'TRUE' THEN 'S' WHEN 'FALSE' THEN 'N' END AS FACTURADO_PROVEEDOR, v.NUM_FACTURA,  ");
            //strSQL.Append("v.FECHA_FACTURA, CASE V.ENVIADA_CARTA WHEN 'TRUE' THEN 'S' WHEN 'FALSE' THEN 'N' END AS ENVIADA_CARTA, v.FECHA_ENVIO_CARTA,  ");
            ////TEL: strSQL.Append("m.NUM_TELEFONO1, m.NUM_TELEFONO2, m.NUM_TELEFONO3, m.NUM_TELEFONO4, m.NUM_TELEFONO5, m.NUM_TELEFONO6, L.FECHA_LOTE,  ");
            //strSQL.Append("L.FECHA_LOTE,  ");
            //strSQL.Append("v.COD_VISITA, v.COD_ESTADO_VISITA, v.ID_TIPO_URGENCIA, m.FEC_RECLAMACION, m.RECLAMACION, v.COD_BARRAS, m.TELEFONO_CLIENTE1,  ");
            //strSQL.Append("m.TELEFONO_CLIENTE2, m.BCS, m.CUPS,M.COD_RECEPTOR ");
            strSQL.Append("FROM dbo.MANTENIMIENTO AS m LEFT OUTER JOIN ");
            strSQL.Append("dbo.RELACION_CUPS_CONTRATO AS RCC ON m.COD_RECEPTOR = RCC.COD_RECEPTOR LEFT OUTER JOIN ");
            //strSQL.Append("dbo.VISITA AS v ON m.COD_ULTIMA_VISITA = v.COD_VISITA AND (m.COD_CONTRATO_SIC= v.COD_CONTRATO or rcc.COD_CONTRATO_SIC= v.COD_CONTRATO) INNER JOIN ");
            strSQL.Append("dbo.VISITA AS v ON m.COD_ULTIMA_VISITA = v.COD_VISITA AND rcc.COD_CONTRATO_SIC= v.COD_CONTRATO INNER JOIN ");
            strSQL.Append("dbo.PROVINCIA AS pr ON m.COD_PROVINCIA = pr.COD_PROVINCIA INNER JOIN ");
            strSQL.Append("dbo.POBLACION AS PO ON m.COD_POBLACION = PO.COD_POBLACION AND m.COD_PROVINCIA = PO.COD_PROVINCIA LEFT OUTER JOIN ");
            //strSQL.Append("dbo.TIPO_TARIFA_GAS AS TG ON m.COD_TARIFA = TG.COD_TARIFA LEFT OUTER JOIN ");
            strSQL.Append("dbo.TIPO_CAMPANIA AS CAM ON v.CAMPANIA = CAM.CAMPANIA LEFT OUTER JOIN ");
            strSQL.Append("dbo.LOTE AS L ON v.ID_LOTE = L.ID_LOTE LEFT OUTER JOIN ");
            strSQL.Append("dbo.TIPO_URGENCIA AS TU ON v.ID_TIPO_URGENCIA = TU.ID_TIPO_URGENCIA LEFT OUTER JOIN ");
            strSQL.Append("dbo.TIPO_ESTADO_VISITA AS EV ON v.COD_ESTADO_VISITA = EV.COD_ESTADO LEFT OUTER JOIN ");
            strSQL.Append("dbo.CALDERAS AS C ON m.COD_CONTRATO_SIC= C.COD_CONTRATO LEFT OUTER JOIN ");
            strSQL.Append("dbo.TIPO_MARCA_CALDERA AS MC ON C.ID_MARCA_CALDERA = MC.ID_MARCA_CALDERA ");
            //strSQL.Append("WHERE (ISNULL(m.FEC_BAJA_SERVICIO, 0) = 0)) V ");
            strSQL.Append("WHER1E (ISNULL(m.FEC_BAJA_SERVICIO, 0) = 0) ");
            strSQL.Append("AND EV.DES_ESTADO IS NOT NULL ");

            //strSQL.Append(") V ");

            // Parametros necesarios para pasar variables
            String[] paramNames = null;
            DbType[] paramTypes = null;
            Object[] paramValues = null;
            // Lista que almacenara las condiciones
            Queue<string> listaStrCondiciones = new Queue<string>();

            // Obtener todas las condiciones
            this.ObtenerFiltrosAvanzados(filtros, listaStrCondiciones, ref paramNames, ref paramTypes, ref paramValues);
            this.ObtenerFiltrosColumnas(filtros, listaStrCondiciones, ref paramNames, ref paramTypes, ref paramValues);
            this.AplicarCondicion(tipoVista, parametros, listaStrCondiciones);
            // Aplicar las condiciones a la sentencia
            strSQL.Append(this.ObtenerClausulaCondiciones(listaStrCondiciones));



            Boolean Orden = false;
            String FiltrosMenuDerecho = (string)Iberdrola.Commons.Web.CurrentSession.GetAttribute(Iberdrola.Commons.Web.CurrentSession.SESSION_FILTROS_MENU_DERECHO);
            if (FiltrosMenuDerecho != "" && FiltrosMenuDerecho != null)
            {
                if (FiltrosMenuDerecho.Substring(1, 5) != "order")
                {
                    if (strSQL.ToString().IndexOf("WHERE") == -1)// intPageIndex.HasValue)
                    {
                        strSQL.Append(" WHERE " + FiltrosMenuDerecho);
                    }
                    else
                    {
                        strSQL.Append(" AND " + FiltrosMenuDerecho);
                    }
                }
                else
                {
                    Orden = true;
                }
            }
            if (filtros.CodigoDeProvincia != null)
            {
                String[] Provincias = filtros.CodigoDeProvincia.Split(';');
                Boolean Esta = false;
                String Filtropro = "";
                for (int i = 0; i <= Provincias.Length - 1; i++)
                {
                    Esta = true;
                    if (Provincias[i] != "") { Filtropro = Filtropro + " pr.NOMBRE='" + Provincias[i] + "' OR "; }
                }
                Filtropro = Filtropro.Substring(0, Filtropro.Length - 3);
                if (Filtropro != "")
                {
                    if (strSQL.ToString().IndexOf(" WHERE") != -1)// intPageIndex.HasValue)
                    {
                        Filtropro = " AND (" + Filtropro + ")";
                    }
                    else
                    {
                        Filtropro = " WHERE (" + Filtropro + ")";
                    }
                }
                
                strSQL.Append(Filtropro);
                //listaStrCondiciones.Enqueue(" COD_PROVINCIA = @COD_PROVINCIA");
                //listaNombres.Add("COD_PROVINCIA");
                //listaTipos.Add(DbType.String);
                //listaValores.Add(filtros.CodigoDeProvincia);
            }

            if (filtros.CodigoPostal != null)
            {
                String[] CP = filtros.CodigoPostal.Split(',');
                Boolean Esta = false;
                String Filtrocp = "";
                for (int i = 0; i <= CP.Length - 1; i++)
                {
                    Esta = true;
                    if (CP[i] != "") { Filtrocp = Filtrocp + " COD_POSTAL='" + CP[i] + "' OR "; }
                }
                Filtrocp = Filtrocp.Substring(0, Filtrocp.Length - 3);

                if (Filtrocp != "")
                {
                    if (strSQL.ToString().IndexOf("WHERE") != -1)// intPageIndex.HasValue)
                    {
                        Filtrocp = " AND (" + Filtrocp + ")";
                    }
                    else
                    {
                        Filtrocp = " WHERE (" + Filtrocp + ")";
                    }
                }

                strSQL.Append(Filtrocp);
            }


            if (filtros.CodigoDePoblacion != null)
            {
                String[] Poblacion = filtros.CodigoDePoblacion.Split(';');
                Boolean Esta = false;
                String Filtropro = "";
                for (int i = 0; i <= Poblacion.Length - 1; i++)
                {
                    Esta = true;
                    if (Poblacion[i] != "") { Filtropro = Filtropro + " m.COD_POBLACION='" + Poblacion[i] + "' OR "; }
                }
                Filtropro = Filtropro.Substring(0, Filtropro.Length - 3);
                if (Filtropro != "")
                {
                    if (strSQL.ToString().IndexOf("WHERE") != -1)// intPageIndex.HasValue)
                    {
                        Filtropro = " AND (" + Filtropro + ")";
                    }
                    else
                    {
                        Filtropro = " WHERE (" + Filtropro + ")";
                    }
                }

                strSQL.Append(Filtropro);
                //listaStrCondiciones.Enqueue(" COD_PROVINCIA = @COD_PROVINCIA");
                //listaNombres.Add("COD_PROVINCIA");
                //listaTipos.Add(DbType.String);
                //listaValores.Add(filtros.CodigoDeProvincia);
            }

            //Kintell 01/07/2011
            if (filtros.CodigoDeEstadoVisita != null)
            {
                String[] Estados = filtros.CodigoDeEstadoVisita.Split(';');
                Boolean Esta = false;
                String Filtropro = "";
                for (int i = 0; i <= Estados.Length - 1; i++)
                {
                    Esta = true;
                    if (Estados[i] != "") { Filtropro = Filtropro + " DES_ESTADO='" + Estados[i] + "' OR "; }
                }
                Filtropro = Filtropro.Substring(0, Filtropro.Length - 3);
                if (Filtropro != "")
                {
                    if (strSQL.ToString().IndexOf("WHERE") != -1)// intPageIndex.HasValue)
                    {
                        Filtropro = " AND (" + Filtropro + ")";
                    }
                    else
                    {
                        Filtropro = " WHERE (" + Filtropro + ")";
                    }
                }

                strSQL.Append(Filtropro);
                //listaStrCondiciones.Enqueue(" COD_PROVINCIA = @COD_PROVINCIA");
                //listaNombres.Add("COD_PROVINCIA");
                //listaTipos.Add(DbType.String);
                //listaValores.Add(filtros.CodigoDeProvincia);
            }

            //Kintell 01/07/2011
            if (filtros.DescripcionTipoUrgencia != null)
            {
                String[] Urgencia = filtros.DescripcionTipoUrgencia.Split(';');
                Boolean Esta = false;
                String Filtropro = "";
                for (int i = 0; i <= Urgencia.Length - 1; i++)
                {
                    Esta = true;
                    if (Urgencia[i] != "") { Filtropro = Filtropro + " DESC_TIPO_URGENCIA = '" + Urgencia[i] + "' OR "; }
                }
                Filtropro = Filtropro.Substring(0, Filtropro.Length - 3);
                if (Filtropro != "")
                {
                    if (strSQL.ToString().IndexOf("WHERE") != -1)// intPageIndex.HasValue)
                    {
                        Filtropro = " AND (" + Filtropro + ")";
                    }
                    else
                    {
                        Filtropro = " WHERE (" + Filtropro + ")";
                    }
                }

                strSQL.Append(Filtropro);
                //listaStrCondiciones.Enqueue(" COD_PROVINCIA = @COD_PROVINCIA");
                //listaNombres.Add("COD_PROVINCIA");
                //listaTipos.Add(DbType.String);
                //listaValores.Add(filtros.CodigoDeProvincia);
            }



            if (Orden) { strSQL.Append(" " + FiltrosMenuDerecho); }

            strSQL = strSQL.Replace(" COD_CONTRATO_SIC =", "CONTRATO =");

            if (strSQL.ToString().IndexOf(" rcc.COD_CONTRATO_SIC in ('") > 0)
            {
                strSQL = strSQL.Replace("ON m.COD_ULTIMA_VISITA = v.COD_VISITA AND rcc.COD_CONTRATO_SIC= v.COD_CONTRATO", "ON m.COD_ULTIMA_VISITA = v.COD_VISITA AND M.COD_CONTRATO_SIC= v.COD_CONTRATO");
            }

            if (strSQL.ToString().IndexOf(" CONTRATO = ") < 0)
            {
                strSQL = strSQL.Replace("rcc.cod_contrato_sic as contrato,", "");
            }
            strSQL = strSQL.Replace("WHERE", "AND");
            strSQL = strSQL.Replace("WHER1E", "WHERE");
            
            return this.RunQueryDataScalar(strSQL.ToString(), paramNames, paramTypes, paramValues);
        }

        /// <summary>
        /// Devuelve un <see cref="DataTable"/> con los datos de la vista seleccionada
        /// </summary>
        /// <param name="tipoVista"></param>
        /// <param name="parametros"></param>
        /// <param name="filtros"></param>
        /// <param name="intPageIndex"></param>
        /// <param name="sortColumn"></param>
        /// <returns></returns>
        //public DataTable ObtenerVistaContratos(Enumerados.TipoVistaContratoCompleto tipoVista, Hashtable parametros, VistaContratoCompletoDTO filtros, Int32? intPageIndex, String sortColumn)
        //{
        //    // Se obtienen los campos necesarios para la sentencia
        //    String strCampos = this.ObtenerStrCampos(tipoVista);
        //    // Sentencia SQL que se ejecutara
        //    StringBuilder strSQL = new StringBuilder("SELECT DISTINCT ");
        //    if (!intPageIndex.HasValue)
        //    {
        //        strSQL = new StringBuilder("SELECT DISTINCT TOP 1000 ");
        //    }
        //    else
        //    {
        //        if (intPageIndex==1)
        //        {
        //            strSQL = new StringBuilder("SELECT DISTINCT TOP 1000 ");
        //        }
        //    }
        //    strSQL.Append(strCampos);
            
        //    strSQL.Append(" FROM (SELECT ROW_NUMBER() OVER(ORDER BY " + sortColumn + ") AS FILA,");
        //    strSQL.Append(strCampos);

        //    strSQL.Append(" FROM ");
        //    //"VW_MANTENIMIENTO_ULTIMA_VISITA");
        //    strSQL.Append("(SELECT DISTINCT  ");
        //    strSQL.Append("m.COD_CONTRATO_SIC, rcc.COD_CONTRATO_SIC AS CONTRATO, v.FEC_VISITA, v.FEC_LIMITE_VISITA, TU.DESC_TIPO_URGENCIA,  ");
        //    strSQL.Append("EV.COD_ESTADO, EV.DES_ESTADO, m.T1, m.T2, m.T5, MC.DESC_MARCA_CALDERA, m.NOM_TITULAR, m.APELLIDO1, m.APELLIDO2,  ");
        //    strSQL.Append("m.COD_PROVINCIA, pr.NOMBRE AS NOMBRE_PROVINCIA, m.COD_POBLACION, PO.NOMBRE AS NOMBRE_POBLACION, m.TIP_VIA_PUBLICA,  ");
        //    strSQL.Append("m.NOM_CALLE, m.COD_FINCA, m.TIP_BIS, m.COD_PORTAL, m.TIP_ESCALERA, m.TIP_PISO, m.TIP_MANO, m.COD_POSTAL, m.NUM_TEL_CLIENTE,  ");
        //    strSQL.Append("m.NUM_TEL_PS, m.COD_TARIFA, TG.DESC_TARIFA, v.CAMPANIA, CAM.OBSERVACIONES, L.ID_LOTE, L.DESC_LOTE, m.PROVEEDOR,  ");
        //    strSQL.Append("m.FEC_ALTA_CONTRATO, m.FEC_BAJA_CONTRATO, m.FEC_ALTA_SERVICIO, m.FEC_BAJA_SERVICIO,  ");
        //    strSQL.Append("CASE V.FACTURADO_PROVEEDOR WHEN 'TRUE' THEN 'S' WHEN 'FALSE' THEN 'N' END AS FACTURADO_PROVEEDOR, v.NUM_FACTURA,  ");
        //    strSQL.Append("v.FECHA_FACTURA, CASE V.ENVIADA_CARTA WHEN 'TRUE' THEN 'S' WHEN 'FALSE' THEN 'N' END AS ENVIADA_CARTA, v.FECHA_ENVIO_CARTA,  ");
        //    strSQL.Append("m.NUM_TELEFONO1, m.NUM_TELEFONO2, m.NUM_TELEFONO3, m.NUM_TELEFONO4, m.NUM_TELEFONO5, m.NUM_TELEFONO6, L.FECHA_LOTE,  ");
        //    strSQL.Append("v.COD_VISITA, v.COD_ESTADO_VISITA, v.ID_TIPO_URGENCIA, m.FEC_RECLAMACION, m.RECLAMACION, v.COD_BARRAS, m.TELEFONO_CLIENTE1,  ");
        //    strSQL.Append("m.TELEFONO_CLIENTE2, m.BCS, m.CUPS ");
        //    strSQL.Append("FROM         dbo.MANTENIMIENTO AS m LEFT OUTER JOIN ");
        //    strSQL.Append("dbo.RELACION_CUPS_CONTRATO AS RCC ON m.CUPS = RCC.CUPS LEFT OUTER JOIN ");
        //    strSQL.Append("dbo.VISITA AS v ON m.COD_ULTIMA_VISITA = v.COD_VISITA AND m.COD_CONTRATO_SIC= v.COD_CONTRATO INNER JOIN ");
        //    strSQL.Append("dbo.PROVINCIA AS pr ON m.COD_PROVINCIA = pr.COD_PROVINCIA INNER JOIN ");
        //    strSQL.Append("dbo.POBLACION AS PO ON m.COD_POBLACION = PO.COD_POBLACION AND m.COD_PROVINCIA = PO.COD_PROVINCIA LEFT OUTER JOIN ");
        //    strSQL.Append("dbo.TIPO_TARIFA_GAS AS TG ON m.COD_TARIFA = TG.COD_TARIFA LEFT OUTER JOIN ");
        //    strSQL.Append("dbo.TIPO_CAMPANIA AS CAM ON v.CAMPANIA = CAM.CAMPANIA LEFT OUTER JOIN ");
        //    strSQL.Append("dbo.LOTE AS L ON v.ID_LOTE = L.ID_LOTE LEFT OUTER JOIN ");
        //    strSQL.Append("dbo.TIPO_URGENCIA AS TU ON v.ID_TIPO_URGENCIA = TU.ID_TIPO_URGENCIA LEFT OUTER JOIN ");
        //    strSQL.Append("dbo.TIPO_ESTADO_VISITA AS EV ON v.COD_ESTADO_VISITA = EV.COD_ESTADO LEFT OUTER JOIN ");
        //    strSQL.Append("dbo.CALDERAS AS C ON m.COD_CONTRATO_SIC= C.COD_CONTRATO LEFT OUTER JOIN ");
        //    strSQL.Append("dbo.TIPO_MARCA_CALDERA AS MC ON C.ID_MARCA_CALDERA = MC.ID_MARCA_CALDERA ");
        //    strSQL.Append("WHERE     (ISNULL(m.FEC_BAJA_SERVICIO, 0) = 0) ");
        //    strSQL.Append(")b ");















        //    // Parametros necesarios para pasar variables
        //    String[] paramNames = null;
        //    DbType[] paramTypes = null;
        //    Object[] paramValues = null;
        //    // Lista que almacenara las condiciones
        //    Queue<string> listaStrCondiciones = new Queue<string>();

        //    // Obtener todas las condiciones
        //    this.ObtenerFiltrosAvanzados(filtros, listaStrCondiciones, ref paramNames, ref paramTypes, ref paramValues);
        //    this.ObtenerFiltrosColumnas(filtros, listaStrCondiciones, ref paramNames, ref paramTypes, ref paramValues);
        //    this.AplicarCondicion(tipoVista, parametros, listaStrCondiciones);
        //    // Aplicar las condiciones a la sentencia
        //    strSQL.Append(this.ObtenerClausulaCondiciones(listaStrCondiciones));
        //    // Aplicar las clausulas especiales de la paginacion
        //    Boolean Orden = false;
        //    String FiltrosMenuDerecho=(string)Iberdrola.Commons.Web.CurrentSession.GetAttribute(Iberdrola.Commons.Web.CurrentSession.SESSION_FILTROS_MENU_DERECHO);
        //    if (FiltrosMenuDerecho != "" && FiltrosMenuDerecho != null)
        //    {
        //        if (FiltrosMenuDerecho.Substring(1, 5) != "order")
        //        {
        //            if (strSQL.ToString().IndexOf("WHERE") == -1)// intPageIndex.HasValue)
        //            {
        //                strSQL.Append(" WHERE " + FiltrosMenuDerecho);
        //            }
        //            else
        //            {
        //                strSQL.Append(" AND " + FiltrosMenuDerecho);
        //            }
        //        }
        //        else
        //        {
        //            Orden = true;
        //        }
        //    }





        //    if (filtros.CodigoDeProvincia != null)
        //    {
        //        String[] Provincias = filtros.CodigoDeProvincia.Split(';');
        //        Boolean Esta = false;
        //        String Filtropro = "";
        //        for (int i = 0; i <= Provincias.Length - 1; i++)
        //        {
        //            Esta = true;
        //            if (Provincias[i] != "") { Filtropro = Filtropro + " NOMBRE_PROVINCIA='" + Provincias[i] + "' OR "; }
        //        }
        //        Filtropro = Filtropro.Substring(0, Filtropro.Length - 3);
        //        if (Filtropro != "")
        //        {
        //            if (strSQL.ToString().IndexOf ("WHERE")!=-1)// intPageIndex.HasValue)
        //            {
        //                Filtropro = " AND (" + Filtropro + ")";
        //            }
        //            else
        //            {
        //                Filtropro = " WHERE (" + Filtropro + ")";
        //            }
        //        }
        //        strSQL.Append(Filtropro);
        //        //listaStrCondiciones.Enqueue(" COD_PROVINCIA = @COD_PROVINCIA");
        //        //listaNombres.Add("COD_PROVINCIA");
        //        //listaTipos.Add(DbType.String);
        //        //listaValores.Add(filtros.CodigoDeProvincia);
        //    }

        //    if (filtros.CodigoDePoblacion != null)
        //    {
        //        String[] Poblacion = filtros.CodigoDePoblacion.Split(';');
        //        Boolean Esta = false;
        //        String Filtropro = "";
        //        for (int i = 0; i <= Poblacion.Length - 1; i++)
        //        {
        //            Esta = true;
        //            if (Poblacion[i] != "") { Filtropro = Filtropro + " COD_POBLACION='" + Poblacion[i] + "' OR "; }
        //        }
        //        Filtropro = Filtropro.Substring(0, Filtropro.Length - 3);
        //        if (Filtropro != "")
        //        {
        //            if (strSQL.ToString().IndexOf("WHERE") != -1)// intPageIndex.HasValue)
        //            {
        //                Filtropro = " AND (" + Filtropro + ")";
        //            }
        //            else
        //            {
        //                Filtropro = " WHERE (" + Filtropro + ")";
        //            }
        //        }

        //        strSQL.Append(Filtropro);
        //    }

        //    //int PosDelWhere=strSQL.ToString().IndexOf("WHERE");

        //    //if (PosDelWhere < 0)
        //    //{
        //    //    return new DataTable();
        //    //}


        //    strSQL.Append(") V");
        //    if (!intPageIndex.HasValue)
        //    {
        //        strSQL.Append(" where fila>0 and fila<=1000");
        //    }
        //    else
        //    {
        //        strSQL.Append(ObtenerClausulaPaginacion(intPageIndex));
        //    }
            
        //    if (Orden) { strSQL.Append(" " + FiltrosMenuDerecho); }
        //    strSQL = strSQL.Replace(" COD_CONTRATO_SIC =", " CONTRATO =");
        //    return this.RunQueryDataTable(strSQL.ToString(), paramNames, paramTypes, paramValues);
        //}
        public DataTable ObtenerVistaContratos(Enumerados.TipoVistaContratoCompleto tipoVista, Hashtable parametros, VistaContratoCompletoDTO filtros, Int32? intPageIndex, String sortColumn)
        {
            // Parametros necesarios para pasar variables
            String[] paramNames = null;
            DbType[] paramTypes = null;
            Object[] paramValues = null;
            // Lista que almacenara las condiciones
            Queue<string> listaStrCondiciones = new Queue<string>();

            // Obtener todas las condiciones
            this.ObtenerFiltrosAvanzados(filtros, listaStrCondiciones, ref paramNames, ref paramTypes, ref paramValues);
            this.ObtenerFiltrosColumnas(filtros, listaStrCondiciones, ref paramNames, ref paramTypes, ref paramValues);
            this.AplicarCondicion(tipoVista, parametros, listaStrCondiciones);



            // Se obtienen los campos necesarios para la sentencia
            String strCampos = this.ObtenerStrCampos(tipoVista);
            // Sentencia SQL que se ejecutara
            //StringBuilder strSQL = new StringBuilder("SELECT DISTINCT ");
            //if (!intPageIndex.HasValue)
            //{
            //    strSQL = new StringBuilder("SELECT DISTINCT TOP 1000 ");
            //}
            //else
            //{
            //    strCampos = strCampos.Replace(",COD_VISITA", "");
            //    if (intPageIndex == 1)
            //    {
            //        strSQL = new StringBuilder("SELECT DISTINCT TOP 1000 ");
            //    }
            //}
            //strSQL.Append(strCampos);

            //strSQL.Append(" FROM (SELECT ROW_NUMBER() OVER(ORDER BY " + sortColumn + ") AS FILA,");
            //strSQL.Append(strCampos);
            //strSQL.Append(" FROM ");
            ////"VW_MANTENIMIENTO_ULTIMA_VISITA");
            //strSQL.Append("(SELECT DISTINCT  ");
            StringBuilder strSQL = new StringBuilder("SELECT  DISTINCT ");

            //Si no tiene valor es porque es para el Excel
            if (!intPageIndex.HasValue)
            {
                strSQL.Append("COD_CONTRATO_SIC,VISITA_AVISO,FEC_VISITA,DES_ESTADO,FEC_LIMITE_VISITA,EFV,SCORING,DESC_TIPO_URGENCIA,T1,T2,T5,DESC_MARCA_CALDERA,NOM_TITULAR,APELLIDO1,APELLIDO2,NOMBRE_PROVINCIA,NOMBRE_POBLACION,TIP_VIA_PUBLICA,NOM_CALLE,TIP_BIS,COD_PORTAL,TIP_ESCALERA,TIP_PISO,TIP_MANO,COD_POSTAL,NUM_TEL_CLIENTE,NUM_TEL_PS,NUM_MOVIL,COD_TARIFA,ID_LOTE,DESC_LOTE,PROVEEDOR,CAMPANIA,OBSERVACIONES,FEC_ALTA_CONTRATO,FEC_BAJA_CONTRATO,FEC_ALTA_SERVICIO,FEC_BAJA_SERVICIO,NUM_FACTURA,FACTURADO_PROVEEDOR,FECHA_ENVIO_CARTA,ENVIADA_CARTA,FECHA_LOTE,FEC_RECLAMACION,RECLAMACION,COD_BARRAS,TELEFONO_CLIENTE1,TELEFONO_CLIENTE2,CUPS,COD_RECEPTOR,DESC_CATEGORIA_VISITA,BAJA_RENOVACION FROM ( ");
            }
            else
            {
                strSQL.Append("COD_CONTRATO_SIC,VISITA_AVISO,FEC_VISITA,DES_ESTADO,FEC_LIMITE_VISITA,EFV,SCORING,DESC_TIPO_URGENCIA,T1,T2,T5,DESC_MARCA_CALDERA,NOM_TITULAR,APELLIDO1,APELLIDO2,NOMBRE_PROVINCIA,NOMBRE_POBLACION,TIP_VIA_PUBLICA,NOM_CALLE,TIP_BIS,COD_PORTAL,TIP_ESCALERA,TIP_PISO,TIP_MANO,COD_POSTAL,NUM_TEL_CLIENTE,NUM_TEL_PS,NUM_MOVIL,COD_TARIFA,ID_LOTE,DESC_LOTE,PROVEEDOR,CAMPANIA,OBSERVACIONES,FEC_ALTA_CONTRATO,FEC_BAJA_CONTRATO,FEC_ALTA_SERVICIO,FEC_BAJA_SERVICIO,NUM_FACTURA,FACTURADO_PROVEEDOR,FECHA_ENVIO_CARTA,ENVIADA_CARTA,FECHA_LOTE,FEC_RECLAMACION,RECLAMACION,COD_BARRAS,TELEFONO_CLIENTE1,TELEFONO_CLIENTE2,CUPS,COD_RECEPTOR,DESC_CATEGORIA_VISITA,BAJA_RENOVACION FROM ( ");
            }
            strSQL.Append("SELECT ROW_NUMBER() OVER(ORDER BY m.COD_CONTRATO_SIC ASC) AS FILA, ");
            strSQL.Append("m.COD_CONTRATO_SIC, rcc.COD_CONTRATO_SIC AS CONTRATO, v.FEC_VISITA, v.FEC_LIMITE_VISITA, TU.DESC_TIPO_URGENCIA,  ");
            strSQL.Append("EV.COD_ESTADO, EV.DES_ESTADO, m.T1, m.T2, m.T5, MC.DESC_MARCA_CALDERA, m.NOM_TITULAR, m.APELLIDO1, m.APELLIDO2,  ");
            strSQL.Append("m.COD_PROVINCIA, pr.NOMBRE AS NOMBRE_PROVINCIA, m.COD_POBLACION, PO.NOMBRE AS NOMBRE_POBLACION, m.TIP_VIA_PUBLICA,  ");
            strSQL.Append("m.NOM_CALLE, m.COD_FINCA, m.TIP_BIS, m.COD_PORTAL, m.TIP_ESCALERA, m.TIP_PISO, m.TIP_MANO, m.COD_POSTAL, m.NUM_TEL_CLIENTE,  ");
            strSQL.Append("m.NUM_TEL_PS, m.NUM_MOVIL, m.COD_TARIFA, ");
            //strSQL.Append("TG.DESC_TARIFA, ");
            strSQL.Append("v.CAMPANIA, CAM.OBSERVACIONES, L.ID_LOTE, L.DESC_LOTE, m.PROVEEDOR,  ");
            strSQL.Append("m.FEC_ALTA_CONTRATO, m.FEC_BAJA_CONTRATO, m.FEC_ALTA_SERVICIO, m.FEC_BAJA_SERVICIO,  ");
            strSQL.Append("CASE V.FACTURADO_PROVEEDOR WHEN 'TRUE' THEN 'S' WHEN 'FALSE' THEN 'N' END AS FACTURADO_PROVEEDOR, v.NUM_FACTURA,  ");
            strSQL.Append("v.FECHA_FACTURA, CASE V.ENVIADA_CARTA WHEN 'TRUE' THEN 'S' WHEN 'FALSE' THEN 'N' END AS ENVIADA_CARTA, v.FECHA_ENVIO_CARTA,  ");
            ////TEL: strSQL.Append("m.NUM_TELEFONO1, m.NUM_TELEFONO2, m.NUM_TELEFONO3, m.NUM_TELEFONO4, m.NUM_TELEFONO5, m.NUM_TELEFONO6, L.FECHA_LOTE,  ");
            strSQL.Append("L.FECHA_LOTE,  ");
            strSQL.Append("v.COD_VISITA, v.COD_ESTADO_VISITA, v.ID_TIPO_URGENCIA, m.FEC_RECLAMACION, m.RECLAMACION, v.COD_BARRAS, m.TELEFONO_CLIENTE1,  ");
            strSQL.Append("m.TELEFONO_CLIENTE2, m.BCS, m.CUPS,M.COD_RECEPTOR,TEFV.DESCRIPCION_EFV as EFV,m.SCORING as SCORING ");
            strSQL.Append(",v.VISITA_AVISO ");
            strSQL.Append(",tcv.DESC_CATEGORIA_VISITA ");
            strSQL.Append(",m.BAJA_RENOVACION ");


            strSQL.Append("FROM dbo.MANTENIMIENTO AS m LEFT OUTER JOIN ");
            strSQL.Append("dbo.RELACION_CUPS_CONTRATO AS RCC ON m.COD_RECEPTOR = RCC.COD_RECEPTOR LEFT OUTER JOIN ");
            ////strSQL.Append("dbo.VISITA AS v ON m.COD_ULTIMA_VISITA = v.COD_VISITA AND (m.COD_CONTRATO_SIC= v.COD_CONTRATO or rcc.COD_CONTRATO_SIC= v.COD_CONTRATO) INNER JOIN ");

            strSQL.Append("dbo.VISITA AS v ON m.COD_ULTIMA_VISITA = v.COD_VISITA AND RCC.COD_CONTRATO_SIC= v.COD_CONTRATO LEFT OUTER JOIN ");
            strSQL.Append("dbo.PROVINCIA AS pr ON m.COD_PROVINCIA = pr.COD_PROVINCIA LEFT OUTER JOIN ");
            strSQL.Append("dbo.POBLACION AS PO ON m.COD_POBLACION = PO.COD_POBLACION AND m.COD_PROVINCIA = PO.COD_PROVINCIA LEFT OUTER JOIN ");
            //strSQL.Append("dbo.TIPO_TARIFA_GAS AS TG ON m.COD_TARIFA = TG.COD_TARIFA LEFT OUTER JOIN ");
            strSQL.Append("dbo.TIPO_CAMPANIA AS CAM ON v.CAMPANIA = CAM.CAMPANIA LEFT OUTER JOIN ");
            strSQL.Append("dbo.TIPO_EFV AS TEFV ON m.EFV = TEFV.COD_EFV LEFT OUTER JOIN ");
            strSQL.Append("dbo.LOTE AS L ON v.ID_LOTE = L.ID_LOTE LEFT OUTER JOIN ");
            strSQL.Append("dbo.TIPO_URGENCIA AS TU ON v.ID_TIPO_URGENCIA = TU.ID_TIPO_URGENCIA LEFT OUTER JOIN ");
            strSQL.Append("dbo.TIPO_ESTADO_VISITA AS EV ON v.COD_ESTADO_VISITA = EV.COD_ESTADO LEFT OUTER JOIN ");
            strSQL.Append("dbo.CALDERAS AS C ON m.COD_CONTRATO_SIC= C.COD_CONTRATO LEFT OUTER JOIN ");
            strSQL.Append("dbo.TIPO_MARCA_CALDERA AS MC ON C.ID_MARCA_CALDERA = MC.ID_MARCA_CALDERA ");
            strSQL.Append("LEFT OUTER JOIN TIPO_CATEGORIA_VISITA AS TCV ON V.COD_CATEGORIA_VISITA = TCV.COD_CATEGORIA_VISITA ");
            ////strSQL.Append("WHER1E (ISNULL(m.FEC_BAJA_SERVICIO, 0) = 0) ");
            strSQL.Append("WHER1E (ISNULL(m.FEC_BAJA_SERVICIO, 0) = 0) ");
            strSQL.Append("AND EV.DES_ESTADO IS NOT NULL ");

            //strSQL.Append(")b ");

           
            // Aplicar las condiciones a la sentencia
            strSQL.Append(this.ObtenerClausulaCondiciones(listaStrCondiciones));
            // Aplicar las clausulas especiales de la paginacion
            Boolean Orden = false;
            String FiltrosMenuDerecho = (string)Iberdrola.Commons.Web.CurrentSession.GetAttribute(Iberdrola.Commons.Web.CurrentSession.SESSION_FILTROS_MENU_DERECHO);
            if (FiltrosMenuDerecho != "" && FiltrosMenuDerecho != null)
            {
                if (FiltrosMenuDerecho.Substring(1, 5) != "order")
                {
                    if (strSQL.ToString().IndexOf("WHERE") == -1)// intPageIndex.HasValue)
                    {
                        strSQL.Append(" WHERE " + FiltrosMenuDerecho);
                    }
                    else
                    {
                        strSQL.Append(" AND " + FiltrosMenuDerecho);
                    }
                }
                else
                {
                    Orden = true;
                }
            }





            if (filtros.CodigoDeProvincia != null)
            {
                String[] Provincias = filtros.CodigoDeProvincia.Split(';');
                Boolean Esta = false;
                String Filtropro = "";
                for (int i = 0; i <= Provincias.Length - 1; i++)
                {
                    Esta = true;
                    if (Provincias[i] != "") { Filtropro = Filtropro + " pr.NOMBRE='" + Provincias[i] + "' OR "; }
                }
                Filtropro = Filtropro.Substring(0, Filtropro.Length - 3);
                if (Filtropro != "")
                {
                    if (strSQL.ToString().IndexOf("WHERE") != -1)// intPageIndex.HasValue)
                    {
                        Filtropro = " AND (" + Filtropro + ")";
                    }
                    else
                    {
                        Filtropro = " WHERE (" + Filtropro + ")";
                    }
                }
                strSQL.Append(Filtropro);
                //listaStrCondiciones.Enqueue(" COD_PROVINCIA = @COD_PROVINCIA");
                //listaNombres.Add("COD_PROVINCIA");
                //listaTipos.Add(DbType.String);
                //listaValores.Add(filtros.CodigoDeProvincia);
            }

            if (filtros.CodigoDePoblacion != null)
            {
                String[] Poblacion = filtros.CodigoDePoblacion.Split(';');
                Boolean Esta = false;
                String Filtropro = "";
                for (int i = 0; i <= Poblacion.Length - 1; i++)
                {
                    Esta = true;
                    if (Poblacion[i] != "") { Filtropro = Filtropro + " m.COD_POBLACION='" + Poblacion[i] + "' OR "; }
                }
                Filtropro = Filtropro.Substring(0, Filtropro.Length - 3);
                if (Filtropro != "")
                {
                    if (strSQL.ToString().IndexOf("WHERE") != -1)// intPageIndex.HasValue)
                    {
                        Filtropro = " AND (" + Filtropro + ")";
                    }
                    else
                    {
                        Filtropro = " WHERE (" + Filtropro + ")";
                    }
                }

                strSQL.Append(Filtropro);
                //listaStrCondiciones.Enqueue(" COD_PROVINCIA = @COD_PROVINCIA");
                //listaNombres.Add("COD_PROVINCIA");
                //listaTipos.Add(DbType.String);
                //listaValores.Add(filtros.CodigoDeProvincia);
            }

            if (filtros.CodigoPostal != null)
            {
                String[] CP = filtros.CodigoPostal.Split(',');
                Boolean Esta = false;
                String Filtrocp = "";
                for (int i = 0; i <= CP.Length - 1; i++)
                {
                    Esta = true;
                    if (CP[i] != "") { Filtrocp = Filtrocp + " COD_POSTAL='" + CP[i] + "' OR "; }
                }
                Filtrocp = Filtrocp.Substring(0, Filtrocp.Length - 3);

                if (Filtrocp != "")
                {
                    if (strSQL.ToString().IndexOf("WHERE") != -1)// intPageIndex.HasValue)
                    {
                        Filtrocp = " AND (" + Filtrocp + ")";
                    }
                    else
                    {
                        Filtrocp = " WHERE (" + Filtrocp + ")";
                    }
                }

                strSQL.Append(Filtrocp);
            }

            //Kintell 01/07/2011
            if (filtros.CodigoDeEstadoVisita != null)
            {
                String[] Estados = filtros.CodigoDeEstadoVisita.Split(';');
                Boolean Esta = false;
                String Filtropro = "";
                for (int i = 0; i <= Estados.Length - 1; i++)
                {
                    Esta = true;
                    if (Estados[i] != "") { Filtropro = Filtropro + " DES_ESTADO='" + Estados[i] + "' OR "; }
                }
                Filtropro = Filtropro.Substring(0, Filtropro.Length - 3);
                if (Filtropro != "")
                {
                    if (strSQL.ToString().IndexOf("WHERE") != -1)// intPageIndex.HasValue)
                    {
                        Filtropro = " AND (" + Filtropro + ")";
                    }
                    else
                    {
                        Filtropro = " WHERE (" + Filtropro + ")";
                    }
                }

                strSQL.Append(Filtropro);
                //listaStrCondiciones.Enqueue(" COD_PROVINCIA = @COD_PROVINCIA");
                //listaNombres.Add("COD_PROVINCIA");
                //listaTipos.Add(DbType.String);
                //listaValores.Add(filtros.CodigoDeProvincia);
            }

            //Kintell 01/07/2011
            if (filtros.DescripcionTipoUrgencia != null)
            {
                String[] Urgencia = filtros.DescripcionTipoUrgencia.Split(';');
                Boolean Esta = false;
                String Filtropro = "";
                for (int i = 0; i <= Urgencia.Length - 1; i++)
                {
                    Esta = true;
                    if (Urgencia[i] != "") { Filtropro = Filtropro + " DESC_TIPO_URGENCIA = '" + Urgencia[i] + "' OR "; }
                }
                Filtropro = Filtropro.Substring(0, Filtropro.Length - 3);
                if (Filtropro != "")
                {
                    if (strSQL.ToString().IndexOf("WHERE") != -1)// intPageIndex.HasValue)
                    {
                        Filtropro = " AND (" + Filtropro + ")";
                    }
                    else
                    {
                        Filtropro = " WHERE (" + Filtropro + ")";
                    }
                }

                strSQL.Append(Filtropro);
                //listaStrCondiciones.Enqueue(" COD_PROVINCIA = @COD_PROVINCIA");
                //listaNombres.Add("COD_PROVINCIA");
                //listaTipos.Add(DbType.String);
                //listaValores.Add(filtros.CodigoDeProvincia);
            }

            strSQL.Append(") V");
            strSQL = strSQL.Replace("WHERE", "AND");
            if (!intPageIndex.HasValue)
            {
                strSQL.Append(" where fila>0 and fila<=1000");
            }
            else
            {
                strSQL.Append(ObtenerClausulaPaginacion(intPageIndex));
            }


            if (Orden) { strSQL.Append(" " + FiltrosMenuDerecho); }
            strSQL = strSQL.Replace(" COD_CONTRATO_SIC =", " CONTRATO =");

            if (strSQL.ToString().IndexOf(" rcc.COD_CONTRATO_SIC in ('") > 0)
            {
                strSQL = strSQL.Replace("m.COD_ULTIMA_VISITA = v.COD_VISITA AND RCC.COD_CONTRATO_SIC= v.COD_CONTRATO", "m.COD_ULTIMA_VISITA = v.COD_VISITA AND M.COD_CONTRATO_SIC= v.COD_CONTRATO");
            }

            if (strSQL.ToString().IndexOf(" CONTRATO = ") < 0)
            {
                strSQL = strSQL.Replace("rcc.COD_CONTRATO_SIC AS CONTRATO,", "");
            }
            strSQL.Append("ORDER BY " + sortColumn);
            
            strSQL = strSQL.Replace("WHER1E", "WHERE");

            return this.RunQueryDataTable(strSQL.ToString(), paramNames, paramTypes, paramValues);
        }




        /// <summary>
        /// Devuelve una <see cref="List"/> de tipo <see cref="VisitaDTO"/> con los datos de la vista seleccionada
        /// </summary>
        /// <param name="tipoVista"></param>
        /// <param name="parametros"></param>
        /// <param name="filtros"></param>
        /// <returns></returns>
        public List<VisitaDTO> ObtenerVistaContratos(Enumerados.TipoVistaContratoCompleto tipoVista, Hashtable parametros, VistaContratoCompletoDTO filtros)
        {
            // Campos necesarios para la sentencia
            //String strCampos = "COD_CONTRATO_SIC, COD_VISITA, COD_ESTADO, DES_ESTADO";
            // Sentencia SQL que se ejecutara
            //StringBuilder strSQL = new StringBuilder("SELECT DISTINCT TOP 1000 ");
            StringBuilder strSQL = new StringBuilder("");
            //strSQL.Append(strCampos);
            strSQL.Append(" SELECT DISTINCT TOP 1000 ");
            strSQL.Append("          m.COD_CONTRATO_SIC, ");
                      strSQL.Append("EV.COD_ESTADO, EV.DES_ESTADO,");
                      strSQL.Append("v.COD_VISITA ");
strSQL.Append("FROM         dbo.MANTENIMIENTO AS m LEFT OUTER JOIN ");
                      strSQL.Append("dbo.RELACION_CUPS_CONTRATO AS RCC ON m.cod_receptor = RCC.cod_receptor LEFT OUTER JOIN ");
                      strSQL.Append("dbo.VISITA AS v ON m.COD_ULTIMA_VISITA = v.COD_VISITA AND m.COD_CONTRATO_SIC = v.COD_CONTRATO LEFT OUTER JOIN ");
                      strSQL.Append("dbo.PROVINCIA AS pr ON m.COD_PROVINCIA = pr.COD_PROVINCIA LEFT OUTER JOIN ");
                      strSQL.Append("dbo.POBLACION AS PO ON m.COD_POBLACION = PO.COD_POBLACION AND m.COD_PROVINCIA = PO.COD_PROVINCIA LEFT OUTER JOIN ");
                      strSQL.Append("dbo.TIPO_TARIFA_GAS AS TG ON m.COD_TARIFA = TG.COD_TARIFA LEFT OUTER JOIN ");
                      strSQL.Append("dbo.TIPO_CAMPANIA AS CAM ON v.CAMPANIA = CAM.CAMPANIA LEFT OUTER JOIN ");
                      strSQL.Append("dbo.LOTE AS L ON v.ID_LOTE = L.ID_LOTE LEFT OUTER JOIN ");
                      strSQL.Append("dbo.TIPO_URGENCIA AS TU ON v.ID_TIPO_URGENCIA = TU.ID_TIPO_URGENCIA LEFT OUTER JOIN ");
                      strSQL.Append("dbo.TIPO_ESTADO_VISITA AS EV ON v.COD_ESTADO_VISITA = EV.COD_ESTADO LEFT OUTER JOIN ");
                      strSQL.Append("dbo.CALDERAS AS C ON m.COD_CONTRATO_SIC = C.COD_CONTRATO LEFT OUTER JOIN ");
                      strSQL.Append("dbo.TIPO_MARCA_CALDERA AS MC ON C.ID_MARCA_CALDERA = MC.ID_MARCA_CALDERA ");
strSQL.Append("WHER1E     (ISNULL(m.FEC_BAJA_SERVICIO, 0) = 0) ");


            // Parametros necesarios para pasar variables
            String[] paramNames = null;
            DbType[] paramTypes = null;
            Object[] paramValues = null;
            // Lista que almacenara las condiciones
            Queue<string> listaStrCondiciones = new Queue<string>();

            // Obtener todas las condiciones
            this.ObtenerFiltrosAvanzados(filtros, listaStrCondiciones, ref paramNames, ref paramTypes, ref paramValues);
            this.ObtenerFiltrosColumnas(filtros, listaStrCondiciones, ref paramNames, ref paramTypes, ref paramValues);
            this.AplicarCondicion(tipoVista, parametros, listaStrCondiciones);
            // Aplicar las condiciones a la sentencia
            strSQL.Append(this.ObtenerClausulaCondiciones(listaStrCondiciones));

            Boolean Orden = false;
            String FiltrosMenuDerecho = (string)Iberdrola.Commons.Web.CurrentSession.GetAttribute(Iberdrola.Commons.Web.CurrentSession.SESSION_FILTROS_MENU_DERECHO);
            if (FiltrosMenuDerecho != "" && FiltrosMenuDerecho != null)
            {
                if (FiltrosMenuDerecho.Substring(1, 5) != "order")
                {
                    if (strSQL.ToString().IndexOf("WHERE") == -1)// intPageIndex.HasValue)
                    {
                        strSQL.Append(" WHERE " + FiltrosMenuDerecho);
                    }
                    else
                    {
                        strSQL.Append(" AND " + FiltrosMenuDerecho);
                    }
                }
                else
                {
                    Orden = true;
                }
            }
            if (filtros.FiltrosAvanzadosActivos)
            {
                if (filtros.DescripcionTipoUrgencia != null)
                {
                    String[] Urgencia = filtros.DescripcionTipoUrgencia.Split(';');
                    Boolean Esta = false;
                    String Filtropro = "";
                    for (int i = 0; i <= Urgencia.Length - 1; i++)
                    {
                        Esta = true;
                        if (Urgencia[i] != "") { Filtropro = Filtropro + " DESC_TIPO_URGENCIA = '" + Urgencia[i] + "' OR "; }
                    }
                    Filtropro = Filtropro.Substring(0, Filtropro.Length - 3);
                    if (Filtropro != "")
                    {
                        if (strSQL.ToString().IndexOf("WHERE") != -1)// intPageIndex.HasValue)
                        {
                            Filtropro = " AND (" + Filtropro + ")";
                        }
                        else
                        {
                            Filtropro = " WHERE (" + Filtropro + ")";
                        }
                    }

                    strSQL.Append(Filtropro);
                    //listaStrCondiciones.Enqueue(" COD_PROVINCIA = @COD_PROVINCIA");
                    //listaNombres.Add("COD_PROVINCIA");
                    //listaTipos.Add(DbType.String);
                    //listaValores.Add(filtros.CodigoDeProvincia);
                }
                if (filtros.CodigoPostal != null)
                {
                    String[] CP = filtros.CodigoPostal.Split(',');
                    Boolean Esta = false;
                    String Filtrocp = "";
                    for (int i = 0; i <= CP.Length - 1; i++)
                    {
                        Esta = true;
                        if (CP[i] != "") { Filtrocp = Filtrocp + " COD_POSTAL='" + CP[i] + "' OR "; }
                    }
                    Filtrocp = Filtrocp.Substring(0, Filtrocp.Length - 3);

                    if (Filtrocp != "")
                    {
                        if (strSQL.ToString().IndexOf("WHERE") != -1)// intPageIndex.HasValue)
                        {
                            Filtrocp = " AND (" + Filtrocp + ")";
                        }
                        else
                        {
                            Filtrocp = " WHERE (" + Filtrocp + ")";
                        }
                    }

                    strSQL.Append(Filtrocp);
                }
                if (filtros.CodigoDeProvincia != null)
                {
                    String[] Provincias = filtros.CodigoDeProvincia.Split(';');
                    Boolean Esta = false;
                    String Filtropro = "";
                    for (int i = 0; i <= Provincias.Length - 1; i++)
                    {
                        Esta = true;
                        if (Provincias[i] != "") { Filtropro = Filtropro + " pr.NOMBRE='" + Provincias[i] + "' OR "; }
                    }
                    Filtropro = Filtropro.Substring(0, Filtropro.Length - 3);
                    if (Filtropro != "")
                    {
                        if (strSQL.ToString().IndexOf("WHERE") != -1)// intPageIndex.HasValue)
                        {
                            Filtropro = " AND (" + Filtropro + ")";
                        }
                        else
                        {
                            Filtropro = " WHERE (" + Filtropro + ")";
                        }
                    }
                    strSQL.Append(Filtropro);
                    //listaStrCondiciones.Enqueue(" COD_PROVINCIA = @COD_PROVINCIA");
                    //listaNombres.Add("COD_PROVINCIA");
                    //listaTipos.Add(DbType.String);
                    //listaValores.Add(filtros.CodigoDeProvincia);
                }

                if (filtros.CodigoDePoblacion != null)
                {
                    String[] Poblacion = filtros.CodigoDePoblacion.Split(';');
                    Boolean Esta = false;
                    String Filtropro = "";
                    for (int i = 0; i <= Poblacion.Length - 1; i++)
                    {
                        Esta = true;
                        if (Poblacion[i] != "") { Filtropro = Filtropro + " m.COD_POBLACION='" + Poblacion[i] + "' OR "; }
                    }
                    Filtropro = Filtropro.Substring(0, Filtropro.Length - 3);
                    if (Filtropro != "")
                    {
                        if (strSQL.ToString().IndexOf("WHERE") != -1)// intPageIndex.HasValue)
                        {
                            Filtropro = " AND (" + Filtropro + ")";
                        }
                        else
                        {
                            Filtropro = " WHERE (" + Filtropro + ")";
                        }
                    }

                    strSQL.Append(Filtropro);
                    //listaStrCondiciones.Enqueue(" COD_PROVINCIA = @COD_PROVINCIA");
                    //listaNombres.Add("COD_PROVINCIA");
                    //listaTipos.Add(DbType.String);
                    //listaValores.Add(filtros.CodigoDeProvincia);
                }
                //Kintell 01/07/2011
                if (filtros.CodigoDeEstadoVisita != null)
                {
                    String[] Estados = filtros.CodigoDeEstadoVisita.Split(';');
                    Boolean Esta = false;
                    String Filtropro = "";
                    for (int i = 0; i <= Estados.Length - 1; i++)
                    {
                        Esta = true;
                        if (Estados[i] != "") { Filtropro = Filtropro + " DES_ESTADO='" + Estados[i] + "' OR "; }
                    }
                    Filtropro = Filtropro.Substring(0, Filtropro.Length - 3);
                    if (Filtropro != "")
                    {
                        if (strSQL.ToString().IndexOf("WHERE") != -1)// intPageIndex.HasValue)
                        {
                            Filtropro = " AND (" + Filtropro + ")";
                        }
                        else
                        {
                            Filtropro = " WHERE (" + Filtropro + ")";
                        }
                    }

                    strSQL.Append(Filtropro);
                    //listaStrCondiciones.Enqueue(" COD_PROVINCIA = @COD_PROVINCIA");
                    //listaNombres.Add("COD_PROVINCIA");
                    //listaTipos.Add(DbType.String);
                    //listaValores.Add(filtros.CodigoDeProvincia);
                }
            }

            // Se obtienen los resultados de la sentencia
            IDataReader dr = null;
            strSQL = strSQL.Replace("WHERE", "AND");
            strSQL = strSQL.Replace("WHER1E", "WHERE");
            dr = this.RunQueryDataReader(strSQL.ToString(), paramNames, paramTypes, paramValues);

            // Se devuelve la lista obtenida de la consulta
            return Visitas.ObtenerListaVisitasDesdeDataReader(dr);
        }

        #endregion metodos agrupados

        #endregion

        public IDataReader obtenerFiltrosAvanzadosExcelProvinciaDTO(VistaContratoCompletoDTO filtros)
        {
            String[] paramNames = new String[1];
            DbType[] paramTypes = new DbType[1];
            Object[] paramValues = new Object[1];
            paramNames[0] = "@CODPROVINCIA";
            //paramTypes[0] = null;
            paramValues[0] = filtros.CodigoDeProvincia;

            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
            IDataReader dr = db.RunProcDataReader("SP_GET_NOMPROVICIA_POR_CODIGO", paramNames, paramTypes, paramValues);
            return dr;
        }
        public IDataReader obtenerFiltrosAvanzadosExcelPoblacionDTO(VistaContratoCompletoDTO filtros)
        {
            String[] paramNames = new String[2];
            DbType[] paramTypes = new DbType[2];
            Object[] paramValues = new Object[2];

            paramNames[0] = "@CODPROVINCIA";
            //paramTypes[0] = null;
            paramValues[0] = Iberdrola.Commons.Web.CurrentSession.GetAttribute(Iberdrola.Commons.Web.CurrentSession.SESSION_COD_PROVINCIA); //filtros.CodigoDeProvincia;
            paramNames[1] = "@CODPOBLACION";
            //paramTypes[1] = null;
            paramValues[1] = filtros.CodigoDePoblacion;
            

            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);
            IDataReader dr = db.RunProcDataReader("SP_GET_NOMPOBLACION_POR_CODIGO", paramNames, paramTypes, paramValues);
            return dr;
        }


        /// <summary>
        /// Devuelve los datos distintos que existen en una columna dada
        /// </summary>
        /// <param name="columna"> Columna de la que se quiere obtener los datos distintos</param>
        /// <returns></returns>
        public List<ObjetoTextoValorDTO> ObtenerDatosDistintosColumnaContrato(Enumerados.TipoVistaContratoCompleto tipoVista, Hashtable parametros, VistaContratoCompletoDTO filtros, string nombreColumna)
        {
            // Sentencia SQL que se ejecutara
            StringBuilder strSQL = new StringBuilder("SELECT DISTINCT ");
            // Se aniade la columna de la que se sacara la informacion
            strSQL.Append(nombreColumna);
            strSQL.Append(" FROM VW_MANTENIMIENTO_ULTIMA_VISITA");

            // Parametros necesarios para pasar variables
            String[] paramNames = null;
            DbType[] paramTypes = null;
            Object[] paramValues = null;
            // Lista que almacenara las condiciones
            Queue<string> listaStrCondiciones = new Queue<string>();

            // Obtener todas las condiciones
            this.ObtenerFiltrosAvanzados(filtros, listaStrCondiciones, ref paramNames, ref paramTypes, ref paramValues);
            this.AplicarCondicion(tipoVista, parametros, listaStrCondiciones);
            // Aplicar las condiciones a la sentencia
            strSQL.Append(this.ObtenerClausulaCondiciones(listaStrCondiciones));
            // Los resultados se devuelven ordenados
            strSQL.Append(" ORDER BY ");
            strSQL.Append(nombreColumna);

            // Se obtienen los datos de la columna
            IDataReader dr = null;
            dr = this.RunQueryDataReader(strSQL.ToString(), paramNames, paramTypes, paramValues);

            // Se procesan los datos recibidos
            List<ObjetoTextoValorDTO> resultado = new List<ObjetoTextoValorDTO>();
            while (dr.Read())
            {
                resultado.Add(new ObjetoTextoValorDTO(dr.GetValue(dr.GetOrdinal(nombreColumna))));
            }
            // Se devuelve la lista obtenida de la consulta
            return resultado;
        }

        /// <summary>
        /// Devuelve el numero de datos distintos que existe en una columna dada
        /// </summary>
        /// <param name="columna"> Columna que se desea consultar </param>
        /// <returns></returns>
        public Int32 ObtenerNumRegDatosDistintosColumnaContrato(Enumerados.TipoVistaContratoCompleto tipoVista, Hashtable parametros, VistaContratoCompletoDTO filtros, string nombreColumna)
        {
            // Sentencia SQL que se ejecutara
            StringBuilder strSQL = new StringBuilder("SELECT COUNT (DISTINCT ");
            // Se aniade la columna de la que se sacara la informacion
            strSQL.Append(nombreColumna);
            strSQL.Append(") FROM VW_MANTENIMIENTO_ULTIMA_VISITA");

            // Parametros necesarios para pasar variables
            String[] paramNames = null;
            DbType[] paramTypes = null;
            Object[] paramValues = null;
            // Lista que almacenara las condiciones
            Queue<string> listaStrCondiciones = new Queue<string>();

            // Obtener todas las condiciones
            this.ObtenerFiltrosAvanzados(filtros, listaStrCondiciones, ref paramNames, ref paramTypes, ref paramValues);
            this.AplicarCondicion(tipoVista, parametros, listaStrCondiciones);
            // Aplicar las condiciones a la sentencia
            strSQL.Append(this.ObtenerClausulaCondiciones(listaStrCondiciones));

            return this.RunQueryDataScalar(strSQL.ToString(), paramNames, paramTypes, paramValues);
        }
    }
}