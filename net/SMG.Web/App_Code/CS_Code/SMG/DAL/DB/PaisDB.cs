using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Iberdrola.Commons.Exceptions;
using Iberdrola.Commons.DataAccess;
using Iberdrola.SMG.DAL.DB;

/// <summary>
/// Descripción breve de PaisDB
/// </summary>
public class PaisDB
{
        private DataBaseLibrary _db;
        /// <summary>
        /// El constructor de la clase.
        /// </summary>
        public PaisDB()
        {
            this._db = new DataBaseLibrary(BaseDB.SMG_DB);
        }

        /// <summary>
        /// Obtenemos el país que corresponde al id proporcionado
        /// </summary>
        /// <param name="idPais">Id del país a obtener</param>
        /// <returns>PaisDTO</returns>
        public PaisDTO ObtenerPais(int idPais)
        {
            IDataReader dr = null;
            try
            {
                string[] parametros = new string[1];
                DbType[] tipos = new DbType[1];
                object[] valores = new object[1];

                parametros[0] = "@pID_PAIS";
                tipos[0] = DbType.Int32;
                valores[0] = idPais;

                dr = this._db.RunProcDataReader("spPaisGet", parametros, tipos, valores);

                if (dr.Read())
                {
                    return this.ObtenerPais(dr);
                }

                return null;
            }
            catch (BaseException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new DALException(false, ex, "2004"); // Error al obtener datos de la Base de Datos
            }
            finally
            {
                if (dr != null)
                {
                    dr.Close();
                }
            }
        }

        /// <summary>
        /// Obtenemos una lista con los datos de los paises a los que tiene acceso el usuario.
        /// </summary>
        /// <returns>Devuelve una lista de "clases" LoteDTO con los datos de los lotes existentes.</returns>
        public List<PaisDTO> ObtenerPaises(string strLoginUsuario)
        {
            IDataReader dr = null;
            try
            {
                string[] parametros = new string[1];
                DbType[] tipos = new DbType[1];
                object[] valores = new object[1];

                parametros[0] = "@pCOD_USUARIO";
                tipos[0] = DbType.String;
                valores[0] = strLoginUsuario;

                dr = this._db.RunProcDataReader("spPaisGetByUsuario", parametros, tipos, valores);
                                          
                List<PaisDTO> lista = new List<PaisDTO>();
                while (dr.Read())
                {
                    lista.Add(this.ObtenerPais(dr));
                }

                return lista;
            }
            catch (BaseException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new DALException(false, ex, "2004"); // Error al obtener datos de la Base de Datos
            }
            finally
            {
                if (dr != null)
                {
                    dr.Close();
                }
            }
        }

        /// <summary>
        /// Obtenemos una lista con los datos de los paises a los que tiene acceso el usuario.
        /// </summary>
        /// <returns>Devuelve una lista de "clases" IdiomaDTO con los datos de los lotes existentes.</returns>
        public List<IdiomaDTO> ObtenerIdiomas()
        {
            IDataReader dr = null;
            try
            {
                dr = this._db.RunProcDataReader("spIdiomaGetAll");

                List<IdiomaDTO> lista = new List<IdiomaDTO>();
                while (dr.Read())
                {
                    lista.Add(this.ObtenerIdioma(dr));
                }

                return lista;
            }
            catch (BaseException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new DALException(false, ex, "2004"); // Error al obtener datos de la Base de Datos
            }
            finally
            {
                if (dr != null)
                {
                    dr.Close();
                }
            }
        }

        /// <summary>
        /// Obtenemos una lista con los datos de los paises a los que tiene acceso el usuario.
        /// </summary>
        /// <param name="idIdioma">Id del idioma del que obtener los datos.</param>
        /// <returns>Devuelve una lista de "clases" IdiomaDTO con los datos de los lotes existentes.</returns>
        public IdiomaDTO ObtenerIdioma(decimal idIdioma)
        {
            IDataReader dr = null;
            try
            {
                string[] paramsName = new string[1]; 
                DbType[] paramsType = new DbType[1]; 
                object[] paramsValue = new object[1];

                paramsName[0] = "@pID_IDIOMA";
                paramsType[0] = DbType.Decimal;
                paramsValue[0] = idIdioma;

                dr = this._db.RunProcDataReader("spIdiomaGet", paramsName, paramsType, paramsValue);

                IdiomaDTO idioma = null;
                if (dr.Read())
                {
                    idioma = this.ObtenerIdioma(dr);
                }

                return idioma;
            }
            catch (BaseException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new DALException(false, ex, "2004"); // Error al obtener datos de la Base de Datos
            }
            finally
            {
                if (dr != null)
                {
                    dr.Close();
                }
            }
        }

        /// <summary>
        /// Obtenemos los datos de un pais desde un IdataReader.
        /// </summary>
        /// <param name="dr">Idatareader con los datos a pasar a una "clase" del tipo PaisDTO.</param>
        /// <returns>Devuelve una "clase" del tipo PaisDTO con los datos cargados de un IdataReader.</returns>
        private PaisDTO ObtenerPais(IDataReader dr)
        {
            PaisDTO paisDto = new PaisDTO();

            paisDto.Id = (int)DataBaseUtils.GetDataReaderColumnValue(dr, "ID_PAIS");
            paisDto.Codigo = (string)DataBaseUtils.GetDataReaderColumnValue(dr, "COD_PAIS");
            paisDto.Descripcion = (string)DataBaseUtils.GetDataReaderColumnValue(dr, "DESC_PAIS");
            paisDto.IdIdioma = (int)DataBaseUtils.GetDataReaderColumnValue(dr, "ID_IDIOMA");

            return paisDto;
        }

        /// <summary>
        /// Obtenemos los datos de un idioma desde un IdataReader.
        /// </summary>
        /// <param name="dr">Idatareader con los datos a pasar a una "clase" del tipo PaisDTO.</param>
        /// <returns>Devuelve una "clase" del tipo IdiomaDTO con los datos cargados de un IdataReader.</returns>
        private IdiomaDTO ObtenerIdioma(IDataReader dr)
        {
            IdiomaDTO iDto = new IdiomaDTO();

            iDto.Id = (int)DataBaseUtils.GetDataReaderColumnValue(dr, "ID_IDIOMA");
            iDto.Descripcion = (string)DataBaseUtils.GetDataReaderColumnValue(dr, "DESC_IDIOMA");
            iDto.Cultura = (string)DataBaseUtils.GetDataReaderColumnValue(dr, "CULTURA");

            return iDto;
        }

        public IDataReader GetTodosLosPaises()
        {
            DataBaseLibrary db = new DataBaseLibrary(BaseDB.SMG_DB);

            //string[] aNombre = new string[1] { "@COD_CONTRATO" };
            //DbType[] aTipos = new DbType[1] { DbType.String };
            //string[] aValores = new string[1] { Contrato };


            IDataReader dr = db.RunProcDataReader("SP_GET_PAIS");

            return dr;
        }
}

