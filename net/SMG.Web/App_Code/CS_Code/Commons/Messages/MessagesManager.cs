//*******************************************************************
// Copyright © 2015 Iberdrola, S.A. Todos los derechos reservados.
//*******************************************************************

using System;
using System.Globalization;
using Iberdrola.Commons.Messages.DAL.DTO;

namespace Iberdrola.Commons.Messages
{
    /// <summary>
    /// Manejador de mensajes
    /// </summary>
    public abstract class MessagesManager
    {
        private CultureInfo Culture { get; set; }

        /// <summary>
        /// Enumerado con los mensajes de información comunes
        /// Códigos 11yyyy
        ///     1 por ser comunes
        ///     1 por ser info
        ///     y Número de mensaje
        /// </summary>
        public enum CommonInfoMessages : int
        {
            /// <summary>
            /// Mensaje a mostrar cuando se guardan datos satisfactoriamente
            /// </summary>
            DataSavedMessage = 110001,
            /// <summary>
            /// Mensaje a mostrar cuando se modifican datos satisfactoriamente
            /// </summary>
            DataModifiedMessage = 110002,
            /// <summary>
            /// Mensaje a mostrar cuando se eliminan datos satisfactoriamente
            /// </summary>
            DataDeletedMessage = 110003,
            /// <summary>
            /// Mensaje a mostrar cuando se copian datos satisfactoriamente
            /// </summary>
            DataCopiedMessage = 110004,
            /// <summary>
            /// Mensaje a mostrar cuando no se encuentran datos al buscar
            /// </summary>
            DataNotFoundMessage = 110005,
            /// <summary>
            /// Mensaje a mostrar cuando no se encuentra el fichero buscado
            /// </summary>
            FileNotFoundMessage = 110006
        }

        /// <summary>
        /// Enumerado con los mensajes de warning comunes
        /// Códigos 12yyyy
        ///     1 por ser comunes
        ///     2 por ser warning
        ///     y Número de mensaje
        /// </summary>
        public enum CommonWarningMessages : int
        {
            /// <summary>
            /// Mensaje genérico a mostrar cuando no se cumple alguna validación del formulario
            /// </summary>
            ValidationErrorMessage = 120001,
            /// <summary>
            /// Mensaje genérico a mostrar en los campos obligatorios sin información proporcionada
            /// </summary>
            CampoNoInformado = 120002,
            /// <summary>
            /// Mensaje genérico a mostrar en los elementos de tipo lista cuando no se ha seleccionado ninguna opción y es requerido
            /// </summary>
            OpcionNoSeleccionada = 120003,
            /// <summary>
            /// Mensaje genérico a mostrar en los controles tipo fecha obligatorios y no informados
            /// </summary>
            FechaNoInformada = 120004,
            /// <summary>
            /// Mensaje genérico a mostrar en los controles tipo hora obligatorios y no informados
            /// </summary>
            HoraNoInformada = 120005,
            /// <summary>
            /// Mensaje genérico a mostrar cuando en un control se espera un valor numérico y este no lo es
            /// </summary>
            NumeroIncorrecto = 120006,
            /// <summary>
            /// Mensaje genérico a mostrar en un control cuando se espera una dirección de correo y no cumple con el formato establecido
            /// </summary>
            EmailIncorrecto = 120007,
            /// <summary>
            /// Mensaje genérico a mostrar en los controles tipo fecha cuando el valor introducido no es válido
            /// </summary>
            FechaFormatoIncorrecto = 120008,
            /// <summary>
            /// Mensaje genérico a mostrar en los controles tipo hora cuando el valor introducido no es válido
            /// </summary>
            HoraFormatoIncorrecto = 120009,
            /// <summary>
            /// Mensaje genérico a mostrar en un control cuando se espera un documento de identidad y no cumple con el formato establecido
            /// </summary>
            NIFCIFNIEIncorrecto = 120010,
            /// <summary>
            /// Mensaje genérico a mostrar en un control cuando se espera un CUPS y no cumple con el formato establecido
            /// </summary>
            CUPSIncorrecto = 120011,
            /// <summary>
            /// Mensaje para indicar que el email es obligatorio para usuarios internos de Iberdrola
            /// </summary>
            EmailObligatorioUsuariosInternosIberdrola = 120012,
            /// <summary>
            /// Mensaje genérico a mostrar cuando en un control se espera un valor euro
            /// </summary>
            ImporteEuroIncorrecto = 120013,
            /// <summary>
            /// Mensaje para indicar el que el formato del teléfono es incorrecto.
            /// </summary>
            TelefonoIncorrecto = 120014
        }

        /// <summary>
        /// Enumerado con los mensajes de error comunes
        /// Códigos 13xyyy
        ///     1 por ser comunes
        ///     3 por ser error
        ///     x Tipo de error
        ///         1 ARQ
        ///         2 BBDD
        ///         3 BLL
        ///         4 UI
        ///     y Número de error
        /// </summary>
        public enum CommonErrorMessages : int
        {
            // 1000 ARQ
            /// <summary>
            /// No se encuentra el recurso
            /// </summary>
            ErrorRecursoNoEncontrado = 131001,
            /// <summary>
            /// No se puede añadir un objeto nulo en la lista de errores
            /// </summary>
            ErrorErroresAniadirNulo = 131002,
            /// <summary>
            /// Error al enviar Email
            /// </summary>
            ErrorEnviarEmail = 131003,
            /// <summary>
            /// Error al cifrar el texto
            /// </summary>
            ErrorEncryptCifrarTexto = 131004,
            /// <summary>
            /// Error al descifrar el texto
            /// </summary>
            ErrorEncryptDescifrarTexto = 131005,
            /// <summary>
            /// No se puede establecer el valor nulo en una variable obligatoria
            /// </summary>
            ErrorVariableSesionNuloValorObligatorio = 131006,
            /// <summary>
            /// No se ha encontrado la variable obligatoria en la sesión.
            /// </summary>
            ErrorVariableSesionObligatoriaNoEncontrada = 131007,
            /// <summary>
            /// Variable de configuración no encontrada.
            /// </summary>
            ErrorVariableConfiguracionNoEncontrada = 131008,
            /// <summary>
            /// Error al recuperar la variable de configuración
            /// </summary>
            ErrorVariableConfiguracionRecuperar = 131009,
            /// <summary>
            /// Variable no encontrada en el objeto aplicación.
            /// </summary>
            ErrorVariableNoEncontradaEnObjetoAplicacion = 131010,

            /// <summary>
            /// Cadena de conexión vacía
            /// </summary>
            ErrorBBDDCadenaConexionVacia = 131101,
            /// <summary>
            /// Error al ejecutar el procedimiento almacenado
            /// </summary>
            ErrorBBDDEjecutarProcedimientoAlmacenado = 131102,
            /// <summary>
            /// No se puede establecer la conexión con la base de datos
            /// </summary>
            ErrorBBDDEstablecerlaConexion = 131103,
            /// <summary>
            /// Error al cerrar la conexión con base de datos
            /// </summary>
            ErrorBBDDCerrarConexion = 131104,
            /// <summary>
            /// No se puede crear el parámetro de acceso a base de datos
            /// </summary>
            ErrorBBDDCrearParametro = 131105,
            /// <summary>
            /// No se puede convertir el tipo de datos a un tipo de datos de base de datos
            /// </summary>
            ErrorBBDDConvertirTipoDatoATipoBBDD = 131106,
            /// <summary>
            /// Error al ejecutar una query SQL
            /// </summary>
            ErrorBBDDEjecucionSQL = 131107,

            /// <summary>
            /// No se han aportado todos los datos necesarios para esta operación
            /// </summary>
            ErrorDataImportFaltanParametrosOperacion = 131201,
            /// <summary>
            /// Error, el directorio al que se hace referencia no existe
            /// </summary>
            ErrorDataImportDirectorioNoExiste = 131202,
            /// <summary>
            /// Error al cargar un dato de tipo Booleano
            /// </summary>
            ErrorDataImportCargarDatoBooleano = 131203,
            /// <summary>
            /// Error al cargar un dato de tipo ListField
            /// </summary>
            ErrorDataImportCargarDatoListField = 131204,
            /// <summary>
            /// Si se asigna una longitud, ésta no puede ser menor a 1
            /// </summary>
            ErrorDataImportLongitudMenor1 = 131205,
            /// <summary>
            /// Error al cargar el valor mínimo
            /// </summary>
            ErrorDataImportCargarValorMinimo = 131206,
            /// <summary>
            /// Error al cargar el valor máximo
            /// </summary>
            ErrorDataImportCargarValorMaximo = 131207,
            /// <summary>
            /// La posición ya esta al final de la linea, no se puede obtener mas información.
            /// </summary>
            ErrorDataImportFinalLineaMasInformacion = 131208,
            /// <summary>
            /// No se puede saltar una posicion atrás
            /// </summary>
            ErrorDataImportSaltarPosicionAtras = 131209,
            /// <summary>
            /// No ha sido posible delimitar el siguiente campo (con lo que posteriores campos podrian fallar tambien)
            /// </summary>
            ErrorDataImportDelimitarSiguienteCampo = 131210,
            /// <summary>
            /// Error en la linea, campo obligatorio sin valor
            /// </summary>
            ErrorDataImportLineaCampoObligatorioSinValor = 131211,
            /// <summary>
            /// Error en la linea, tiene más delimitadores de los permitidos
            /// </summary>
            ErrorDataImportLineaMasDelimitadores = 131212,
            /// <summary>
            /// Error en la linea, tiene menos delimitadores de los necesarios
            /// </summary>
            ErrorDataImportLineaMenosDelimitadores = 131213,
            /// <summary>
            /// Error en la BBDD, no se puede obtener el código de carga.
            /// </summary>
            ErrorDataImportObtenerCodigoCargaBBDD = 131214,
            /// <summary>
            /// Faltan datos para realizar la carga
            /// </summary>
            ErrorDataImportFaltanDatosCarga = 131215,
            /// <summary>
            /// Error al cargar el formato, no hay campos
            /// </summary>
            ErrorDataImportCargarFormatoNoCampos = 131216,
            /// <summary>
            /// Error al cargar el formato, tiene que tener un tipo
            /// </summary>
            ErrorDataImportCargarFormatoTipo = 131217,
            /// <summary>
            /// Error al cargar el formato, tiene que tener nombre
            /// </summary>
            ErrorDataImportCargarFormatoNombre = 131218,
            /// <summary>
            /// Error al cargar el formato, tiene que haber algun tipo de delimitador
            /// </summary>
            ErrorDataImportCargarFormatoTipoDedelimitador = 131219,
            /// <summary>
            /// Error al cargar el fichero, no coinciden el número de elementos de las listas
            /// </summary>
            ErrorDataImportElementosListas = 131220,
            /// <summary>
            /// Error en la cabecera, el campo no forma parte de la plantilla.
            /// </summary>
            ErrorDataImportCampoCabecera = 131221,

            /// <summary>
            /// Error al subir un fichero al SFTP
            /// </summary>
            ErrorSFTPSubirFichero = 131301,
            /// <summary>
            /// Error al bajar un fichero del SFTP
            /// </summary>
            ErrorSFTPBajarFichero = 131302,
            /// <summary>
            /// Error al borrar un fichero del SFTP
            /// </summary>
            ErrorSFTPBorrarFichero = 131303,
            /// <summary>
            /// Error al obtener el listado del SFTP
            /// </summary>
            ErrorSFTPObtenerelListado = 131304,

            /// <summary>
            /// Error al mover el fichero, el fichero no existe.
            /// </summary>
            ErrorFileUtilsMoverFicheroNoExiste = 131401,
            /// <summary>
            /// Error al intentar comprimir un fichero
            /// </summary>
            ErrorFileUtilsComprimir = 131402,
            /// <summary>
            /// Error al obtener el fichero
            /// </summary>
            ErrorFileUtilsObtener = 131403,

            // 2000 BBDD
            /// <summary>
            /// Error al obtener datos de la Base de Datos
            /// </summary>
            ErrorBBDDObtenerDatos = 132001,
            /// <summary>
            /// Error al insertar o actualizar datos de la Base de Datos
            /// </summary>
            ErrorBBDDInsertarActualizarDatos = 132002,
            /// <summary>
            /// Error al eliminar datos de la Base de Datos
            /// </summary>
            ErrorBBDDEliminarDatos = 132003,
            /// <summary>
            /// Error al obtener datos del usuario de la base de datos
            /// </summary>
            ErrorUsuarioObtenerDatosBBDD = 132004,


            // 3000 BLL
            /// <summary>
            /// Error inesperado
            /// </summary>
            ErrorInesperado = 133001,
            /// <summary>
            /// Error al generar el menu principal
            /// </summary>
            ErrorMenuPrincipal = 133002,
            /// <summary>
            /// Error al obtener la versión
            /// </summary>
            ErrorVersion = 133003,
            /// <summary>
            /// No coinciden el número de elementos de las listas
            /// </summary>
            ErrorElementosListasNoCoinciden = 133004,

            /// <summary>
            /// Password Incorrecta. Si no recuerda la contraseña pulse el link Recordar contraseña y se enviará a su correo.
            /// </summary>
            ErrorUsuarioPasswordIncorrectaRecordar = 133101,
            /// <summary>
            /// Usuario Inexistente
            /// </summary>
            ErrorUsuarioInexistente = 133102,
            /// <summary>
            /// Usuario Caducado
            /// </summary>
            ErrorUsuarioCaducado = 133103,
            /// <summary>
            /// El usuario ya existe
            /// </summary>
            ErrorUsuarioExiste = 133104,
            /// <summary>
            /// El argumento nulo
            /// </summary>
            ErrorArgumentoNulo = 133105,

            // 4000 UI
            /// <summary>
            /// Error interno de la aplicación. Por favor, vuelva a intentar la operación.
            /// </summary>
            ErrorInternoAplicacionVolverIntentar = 134001,
            /// <summary>
            /// Se ha superado el tiempo máximo de inactividad y se cerrará la sesión.
            /// </summary>
            ErrorSesionCadudada = 134002,

            /// <summary>
            /// No tiene permisos para realizar la acción solicitada.
            /// </summary>
            ErrorPermisoDenegado = 134003,

            /// <summary>
            /// Error de que el usaurio no existe.
            /// </summary>
            ErrorNoExisteUsuario = 2009,

            /// <summary>
            /// Error de el LDAP de credenciales incorrectas.
            /// </summary>
            
            ErrorLDAPCedenciales = 2012

        }

        private MessagesManager()
        { }

        /// <summary>
        /// Contructor accesible para las clases que lo hereden
        /// </summary>
        /// <param name="culture">Información de la cultura (para las traducciones)</param>
        protected MessagesManager(CultureInfo culture)
        {
            this.Culture = culture;
        }

        /// <summary>
        /// Obtiene el texto del código de error proporcionado
        /// </summary>
        /// <param name="code">Enumerado con el identificador del mensaje</param>
        /// <returns>MessageDTO con los datos del mensaje</returns>
        public abstract MessageDTO GetMessage(Enum code);
    }
}