using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Iberdrola.Commons.Web;
using System.Collections;
using System.Data;
using Iberdrola.SMG.BLL;
using Iberdrola.SMG.DAL.DTO;
using Iberdrola.SMG.DAL.DB;
using Iberdrola.Commons.Security;
using Iberdrola.Commons.Utils;
using System.Collections.Generic;
using Iberdrola.Commons.Exceptions;
using System.Threading;
using Iberdrola.Commons.Reporting;
using System.Globalization;
using System.Web.Configuration;
using Iberdrola.Commons.Configuration;
using System.Web;
using System.Xml.Linq;
using System.Xml;
using System.Drawing;
using Iberdrola.Commons.DataAccess;


namespace Iberdrola.SMG.UI
{
    public partial class FrmListadoLiquidaciones : FrmBaseListado
    {
        #region implementación de los eventos

        private String Imagenseleccionada = "../Imagenes/Ffondo.gif";
        private String ImagensNoeleccionada = "../Imagenes/fondo.png";


        protected void Page_Load(object sender, EventArgs e)
        {


            //var controlName = Request.Params.Get("__EVENTTARGET");
            //var argument = Request.Params.Get("__EVENTARGUMENT");
            //if (controlName == "pnlVisita" && argument == "Click"){ PanelVisita(); }
            //if (controlName == "pnlSolVisita" && argument == "Click") { PanelSolVisita(); }
            //if (controlName == "pnlSolVisitaIncorrecta" && argument == "Click") { PanelSolVisitaIncorrecta(); }
            //if (controlName == "pnlSolReviPrecinte" && argument == "Click") { PanelSolReviPrecinte(); }
            //if (controlName == "pnlSolAveria" && argument == "Click") { PanelSolAveria(); }
            //if (controlName == "pnlSolAveriaIncorrecta" && argument == "Click") { PanelSolAveriaIncorrecta(); }
            //if (controlName == "pnlReparaciones" && argument == "Click") { PanelReparaciones(); }


            if (!IsPostBack)
            {
                this.lblNumEncontrados.Text = "0";

                UsuarioDB usuarioDB = new UsuarioDB();
                this.cmbProveedor.DataSource = usuarioDB.ObtenerTodosProveedoresEnSolicitudes();
                cmbProveedor.DataTextField = "PROVEEDOR";
                cmbProveedor.DataValueField = "PROVEEDOR";
                this.cmbProveedor.DataBind();

                FormUtils.AddDefaultItem(cmbProveedor);
            }
        }
        #endregion

        #region Pulsacion Paneles
        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {
            this.hdnPageIndex.Value = "1";
            //Visitas.
            PanelVisita();
        }
        protected void ImageButton2_Click(object sender, ImageClickEventArgs e)
        {
            this.hdnPageIndex.Value = "1";
            //Sol.Visitas.
            PanelSolVisita();
        }
        protected void ImageButton3_Click(object sender, ImageClickEventArgs e)
        {
            this.hdnPageIndex.Value = "1";
            //Sol.Vis.Incorr.
            PanelSolVisitaIncorrecta();
        }
        protected void ImageButton6_Click(object sender, ImageClickEventArgs e)
        {
            this.hdnPageIndex.Value = "1";
            //Sol.Rev.Precinte.
            PanelSolReviPrecinte();
        }
        protected void ImageButton5_Click(object sender, ImageClickEventArgs e)
        {
            this.hdnPageIndex.Value = "1";
            //Sol.Averia.
            PanelSolAveria();
        }
        protected void ImageButton4_Click(object sender, ImageClickEventArgs e)
        {
            this.hdnPageIndex.Value = "1";
            //Sol.Averia.Incorre.
            PanelSolAveriaIncorrecta();
        }
        protected void ImageButton8_Click(object sender, ImageClickEventArgs e)
        {
            //Reparaciones.
            PanelReparaciones();
        }

        protected void PanelReparaciones()
        {
            //Ponemos imagen de fondo...
            Panel1.Enabled = true; //false;
            Panel2.Enabled = true;
            Panel3.Enabled = true;
            Panel4.Enabled = true;
            Panel5.Enabled = true;
            Panel6.Enabled = true;
            Panel1.BackImageUrl = Imagenseleccionada;
            Panel2.BackImageUrl = ImagensNoeleccionada;
            Panel3.BackImageUrl = ImagensNoeleccionada;
            Panel4.BackImageUrl = ImagensNoeleccionada;
            Panel5.BackImageUrl = ImagensNoeleccionada;
            Panel6.BackImageUrl = ImagensNoeleccionada;

            if (Label10.Text == "MOSTRAR REPARACIONES") { Label10.Text = "OCULTAR REPARACIONES"; Reparaciones(); }
            else { Label10.Text = "MOSTRAR REPARACIONES"; Visitas(); }


        }
        protected void PanelVisita()
        {
            //Ponemos imagen de fondo...
            Panel1.Enabled = true; //false;
            Panel2.Enabled = true;
            Panel3.Enabled = true;
            Panel4.Enabled = true;
            Panel5.Enabled = true;
            Panel6.Enabled = true;
            pnlReparaciones.Visible = true;
            Panel1.BackImageUrl = Imagenseleccionada;
            Panel2.BackImageUrl = ImagensNoeleccionada;
            Panel3.BackImageUrl = ImagensNoeleccionada;
            Panel4.BackImageUrl = ImagensNoeleccionada;
            Panel5.BackImageUrl = ImagensNoeleccionada;
            Panel6.BackImageUrl = ImagensNoeleccionada;

            Reparaciones();
            Visitas();
        }
        protected void PanelSolVisita()
        {
            //Ponemos imagen de fondo...
            Panel2.Enabled = true; //false;
            Panel1.Enabled = true;
            Panel3.Enabled = true;
            Panel4.Enabled = true;
            Panel5.Enabled = true;
            Panel6.Enabled = true;
            Panel2.BackImageUrl = Imagenseleccionada;
            Panel1.BackImageUrl = ImagensNoeleccionada;
            Panel3.BackImageUrl = ImagensNoeleccionada;
            Panel4.BackImageUrl = ImagensNoeleccionada;
            Panel5.BackImageUrl = ImagensNoeleccionada;
            Panel6.BackImageUrl = ImagensNoeleccionada;
            SolVisitas();
            pnlReparaciones.Visible = false;
        }
        protected void PanelSolVisitaIncorrecta()
        {
            //Ponemos imagen de fondo...
            Panel3.Enabled = true; //false;
            Panel2.Enabled = true;
            Panel1.Enabled = true;
            Panel4.Enabled = true;
            Panel5.Enabled = true;
            Panel6.Enabled = true;
            Panel3.BackImageUrl = Imagenseleccionada;
            Panel2.BackImageUrl = ImagensNoeleccionada;
            Panel1.BackImageUrl = ImagensNoeleccionada;
            Panel4.BackImageUrl = ImagensNoeleccionada;
            Panel5.BackImageUrl = ImagensNoeleccionada;
            Panel6.BackImageUrl = ImagensNoeleccionada;
            SolVisInco();
            pnlReparaciones.Visible = false;
        }
        protected void PanelSolReviPrecinte()
        {
            //Ponemos imagen de fondo...
            Panel4.Enabled = true; //false;
            Panel2.Enabled = true;
            Panel3.Enabled = true;
            Panel1.Enabled = true;
            Panel5.Enabled = true;
            Panel6.Enabled = true;
            Panel4.BackImageUrl = Imagenseleccionada;
            Panel2.BackImageUrl = ImagensNoeleccionada;
            Panel3.BackImageUrl = ImagensNoeleccionada;
            Panel1.BackImageUrl = ImagensNoeleccionada;
            Panel5.BackImageUrl = ImagensNoeleccionada;
            Panel6.BackImageUrl = ImagensNoeleccionada;
            SolReviPrecin();
            pnlReparaciones.Visible = false;
        }
        protected void PanelSolAveria()
        {
            //Ponemos imagen de fondo...
            Panel5.Enabled = true; //false;
            Panel2.Enabled = true;
            Panel3.Enabled = true;
            Panel4.Enabled = true;
            Panel1.Enabled = true;
            Panel6.Enabled = true;
            Panel5.BackImageUrl = Imagenseleccionada;
            Panel2.BackImageUrl = ImagensNoeleccionada;
            Panel3.BackImageUrl = ImagensNoeleccionada;
            Panel4.BackImageUrl = ImagensNoeleccionada;
            Panel1.BackImageUrl = ImagensNoeleccionada;
            Panel6.BackImageUrl = ImagensNoeleccionada;
            SolAveria();
            pnlReparaciones.Visible = false;
        }
        protected void PanelSolAveriaIncorrecta()
        {
            //Ponemos imagen de fondo...
            Panel6.Enabled = true; //false;
            Panel2.Enabled = true;
            Panel3.Enabled = true;
            Panel4.Enabled = true;
            Panel5.Enabled = true;
            Panel1.Enabled = true;
            Panel6.BackImageUrl = Imagenseleccionada;
            Panel2.BackImageUrl = ImagensNoeleccionada;
            Panel3.BackImageUrl = ImagensNoeleccionada;
            Panel4.BackImageUrl = ImagensNoeleccionada;
            Panel5.BackImageUrl = ImagensNoeleccionada;
            Panel1.BackImageUrl = ImagensNoeleccionada;
            SolAveriaInco();
            pnlReparaciones.Visible = false;
        }
        #endregion

        #region Funciones
        private void Reparaciones()
        {
            Int32 numReg = 0;
            IDataReader Datos;

            lblProceso.Text = "Reparaciones";
            lblFactura.Text = txtFacturaVisita.Text;
            String SQL = "Select distinct reparacion.*, l.COD_CONTRATO,l.COD_VISITA,l.NUM_FACTURA,l.FECHA AS [FECHA PROCESO],l.PENALIZAR,m.proveedor,po.nombre as POBLACION,PR.NOMBRE AS PROVINCIA, m.t1,m.t2,m.URGENCIA from liquidaciones l left JOIN MANTENIMIENTO M ON L.COD_CONTRATO=M.COD_CONTRATO_SIC left join poblacion po on m.cod_poblacion=po.cod_poblacion and m.cod_provincia=po.cod_provincia left join provincia pr on po.cod_provincia=pr.cod_provincia left join visita v on l.cod_contrato=v.cod_contrato and l.cod_visita=v.cod_visita left JOIN REPARACION ON V.ID_REPARACION = REPARACION.ID_REPARACION where l.cod_visita is not null and l.NUM_FACTURA='" + txtFacturaVisita.Text + "' and reparacion.id_reparacion is not null ";
            if (rdbPenalizables.SelectedValue != "3") { SQL += "AND L.PENALIZAR=" + rdbPenalizables.SelectedValue; }
            if (cmbProveedor.SelectedIndex > 0) { SQL += " AND m.proveedor like '" + cmbProveedor.SelectedValue + "%' "; }
            String SQLPaginacion = SQL.Replace("distinct", "ROW_NUMBER() OVER(ORDER BY l.COD_CONTRATO ASC) AS fILA,");
            SQL += "order by l.fecha";

            try
            {
                try
                {
                    if (this.hdnPageIndexReparacion.Value == "") { this.hdnPageIndexReparacion.Value = "1"; }
                    if (this.hdnPageCountReparacion.Value == "" || this.hdnPageCountReparacion.Value == "0")
                    {
                        Datos = Consultas.ObtenerDatosDesdeSentencia(SQL);
                        DataTable dt = new DataTable();
                        dt.Load(Datos);
                        numReg = dt.Rows.Count;
                        this.lblNumEncontradosReparacion.Text = numReg.ToString();

                        this.hdnPageCountReparacion.Value = ObtenerNumPaginas(numReg).ToString();
                        //CurrentSession.SetAttribute(CurrentSession.SESSION_DATAREADER_EXCEL_FILTROS, excel);
                        CurrentSession.SetAttribute(CurrentSession.SESSION_DATOS_EXCEL_REPARACIONES, dt);
                        CurrentSession.SetAttribute(CurrentSession.SESSION_TITULO_DATOS_EXCEL, "REPARACIONES");
                        CurrentSession.SetAttribute(CurrentSession.SESSION_DATAREADER_EXCEL_FILTROS, null);
                    }




                    CargarPaginadorReparacion();

                    Int32 intPageIndex = Int32.Parse(this.hdnPageIndexReparacion.Value);
                    Int32 PageSize = Int32.Parse(Resources.SMGConfiguration.GridViewPageSize.ToString());
                    Int32 Desde = (PageSize * (intPageIndex - 1));
                    Int32 Hasta = PageSize * intPageIndex;


                    SQLPaginacion += ") v WHERE  (FILA > " + Desde + " AND FILA <= " + Hasta + ")";
                    SQLPaginacion = "SELECT distinct [ID_REPARACION],[ID_TIPO_REPARACION],[FECHA_REPARACION],[ID_TIPO_TIEMPO_MANO_OBRA],[IMPORTE_REPARACION],[IMPORTE_MANO_OBRA_ADICIONAL],[IMPORTE_MATERIALES_ADICIONAL],[FECHA_FACTURA_REPARACION],[NUMERO_FACTURA_REPARACION],[CODIGO_BARRAS_REPARACION],[INFORMACION_RECIBIDA], COD_CONTRATO,COD_VISITA,NUM_FACTURA,[FECHA PROCESO],PENALIZAR,PROVEEDOR,POBLACION,PROVINCIA, t1,t2,URGENCIA FROM (" + SQLPaginacion;
                    SQLPaginacion += " order by [FECHA PROCESO]";
                    Datos = Consultas.ObtenerDatosDesdeSentencia(SQLPaginacion);
                    //CargarDatos(Datos);
                    grdReparaciones.DataSource = Datos;
                    grdReparaciones.DataBind();




                }
                catch (Exception ex)
                {
                    MostrarMensaje(ex.Message);
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
            }
        }

        private void Visitas()
        {
            Int32 numReg = 0;
            IDataReader Datos;

            lblProceso.Text = "Visitas";
            lblFactura.Text = txtFacturaVisita.Text;
            //String SQL = "Select distinct l.COD_CONTRATO,l.COD_VISITA,l.NUM_FACTURA,l.FECHA AS [FECHA PROCESO],l.PENALIZAR,m.proveedor,po.nombre as POBLACION,PR.NOMBRE AS PROVINCIA, m.t1,m.t2,m.URGENCIA,tu.DESC_TIPO_URGENCIA,V.fec_limite_visita from liquidaciones l left JOIN MANTENIMIENTO M ON L.COD_CONTRATO=M.COD_CONTRATO_SIC left join poblacion po on m.cod_poblacion=po.cod_poblacion and m.cod_provincia=po.cod_provincia left join provincia pr on po.cod_provincia=pr.cod_provincia left join visita v on l.cod_contrato=v.cod_contrato and l.cod_visita=v.cod_visita left join tipo_urgencia tu on v.ID_TIPO_URGENCIA=tu.ID_TIPO_URGENCIA where l.cod_visita is not null and l.NUM_FACTURA='" + txtFacturaVisita.Text + "' ";
            String SQL = "Select distinct l.COD_CONTRATO,l.COD_VISITA,l.NUM_FACTURA,l.FECHA AS [FECHA PROCESO],l.PENALIZAR,m.proveedor,po.nombre as POBLACION,PR.NOMBRE AS PROVINCIA, m.t1,m.t2,m.URGENCIA,tu.DESC_TIPO_URGENCIA,V.fec_limite_visita ,ev.DES_ESTADO as Estado from liquidaciones l left JOIN MANTENIMIENTO M ON L.COD_CONTRATO=M.COD_CONTRATO_SIC left join poblacion po on m.cod_poblacion=po.cod_poblacion and m.cod_provincia=po.cod_provincia left join provincia pr on po.cod_provincia=pr.cod_provincia left join visita v on l.cod_contrato=v.cod_contrato and l.cod_visita=v.cod_visita left join tipo_urgencia tu on v.ID_TIPO_URGENCIA=tu.ID_TIPO_URGENCIA left join TIPO_ESTADO_VISITA ev on v.COD_ESTADO_VISITA=ev.COD_ESTADO where l.cod_visita is not null and l.NUM_FACTURA='" + txtFacturaVisita.Text + "' ";
            if (rdbPenalizables.SelectedValue != "3") { SQL += "AND L.PENALIZAR=" + rdbPenalizables.SelectedValue; }
            if (cmbProveedor.SelectedIndex > 0) { SQL += " AND m.proveedor like '" + cmbProveedor.SelectedValue + "%' "; }

            String SQLPaginacion = SQL.Replace("distinct", "ROW_NUMBER() OVER(ORDER BY l.COD_CONTRATO ASC) AS fILA,");



            SQL += "order by l.fecha";

            try
            {
                try
                {
                    if (this.hdnPageIndex.Value == "") { this.hdnPageIndex.Value = "1"; }
                    if (this.hdnPageCount.Value == "" || this.hdnPageCount.Value == "0")
                    {
                        string sQL1 = SQL.Replace("V.fec_limite_visita", "V.fec_limite_visita,fec_visita,codigo_interno_visita as 'Código Interno',cups,cod_receptor,v.cod_barras as 'Código Barras'");
                        Datos = Consultas.ObtenerDatosDesdeSentencia(sQL1);
                        DataTable dt = new DataTable();
                        dt.Load(Datos);
                        numReg = dt.Rows.Count;
                        this.lblNumEncontrados.Text = numReg.ToString();

                        this.hdnPageCount.Value = ObtenerNumPaginas(numReg).ToString();
                        //CurrentSession.SetAttribute(CurrentSession.SESSION_DATAREADER_EXCEL_FILTROS, excel);
                        CurrentSession.SetAttribute(CurrentSession.SESSION_DATOS_EXCEL, dt);
                        CurrentSession.SetAttribute(CurrentSession.SESSION_TITULO_DATOS_EXCEL, "VISITAS");
                        CurrentSession.SetAttribute(CurrentSession.SESSION_DATAREADER_EXCEL_FILTROS, null);
                    }


                    CargarPaginador();

                    Int32 intPageIndex = Int32.Parse(this.hdnPageIndex.Value);
                    Int32 PageSize = Int32.Parse(Resources.SMGConfiguration.GridViewPageSize.ToString());
                    Int32 Desde = (PageSize * (intPageIndex - 1));
                    Int32 Hasta = PageSize * intPageIndex;


                    SQLPaginacion += ") v WHERE  (FILA > " + Desde + " AND FILA <= " + Hasta + ")";
                    SQLPaginacion = "SELECT distinct COD_CONTRATO,COD_VISITA,NUM_FACTURA,[FECHA PROCESO],PENALIZAR,PROVEEDOR,POBLACION,PROVINCIA, t1,t2,URGENCIA,DESC_TIPO_URGENCIA AS [VIS.URGENCIA],LEFT(FEC_LIMITE_VISITA,11) AS [FECHA_LIMITE] FROM (" + SQLPaginacion;
                    SQLPaginacion += " order by [FECHA PROCESO]";
                    Datos = Consultas.ObtenerDatosDesdeSentencia(SQLPaginacion);
                    CargarDatos(Datos);


                }
                catch (Exception ex)
                {
                    MostrarMensaje(ex.Message);
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
            }
        }
        private void SolVisitas()
        {
            Int32 numReg = 0;
            IDataReader Datos;

            lblProceso.Text = "Solicitudes Visita";
            lblFactura.Text = txtFacturaSolVisita.Text;
            String SQL = "select distinct l.COD_CONTRATO,l.ID_SOLICITUD,l.NUM_FACTURA,l.FECHA AS [FECHA PROCESO],l.PENALIZAR,descripcion as ESTADO,s.fecha_creacion as [CREACION SOL.],s.PROVEEDOR,po.nombre as POBLACION,PR.NOMBRE AS PROVINCIA,m.t1,m.t2,m.URGENCIA, max(time_stamp) as CIERRE from liquidaciones l left JOIN MANTENIMIENTO M ON L.COD_CONTRATO=M.COD_CONTRATO_SIC left join poblacion po on m.cod_poblacion=po.cod_poblacion and m.cod_provincia=po.cod_provincia left join provincia pr on po.cod_provincia=pr.cod_provincia left join solicitudes s on l.id_solicitud=s.id_solicitud left join estado_solicitud e on s.estado_solicitud=e.codigo left join historico h on s.id_solicitud=h.id_solicitud where s.subtipo_solicitud ='002'  and l.NUM_FACTURA='" + txtFacturaSolVisita.Text + "' ";
            if (rdbPenalizables.SelectedValue != "3") { SQL += "AND L.PENALIZAR=" + rdbPenalizables.SelectedValue; }
            if (cmbProveedor.SelectedIndex > 0) { SQL += "AND s.proveedor like '" + cmbProveedor.SelectedValue + "%' "; }
            SQL += "group by e.descripcion,m.t1,m.t2,m.URGENCIA,l.[COD_CONTRATO],l.[ID_SOLICITUD],[COD_VISITA],l.[NUM_FACTURA],[FECHA],[PENALIZAR],fecha_creacion,s.PROVEEDOR,po.nombre,PR.NOMBRE ";
            String SQLPaginacion = SQL.Replace("distinct", "ROW_NUMBER() OVER(ORDER BY l.COD_CONTRATO ASC) AS fILA,");
            SQL += "order by l.fecha";
            try
            {
                try
                {
                    if (this.hdnPageIndex.Value == "") { this.hdnPageIndex.Value = "1"; }
                    if (this.hdnPageIndex.Value == "1")
                    {
                        Datos = Consultas.ObtenerDatosDesdeSentencia(SQL);
                        DataTable dt = new DataTable();
                        dt.Load(Datos);
                        numReg = dt.Rows.Count;
                        this.lblNumEncontrados.Text = numReg.ToString();
                        this.hdnPageCount.Value = ObtenerNumPaginas(numReg).ToString();
                        //CurrentSession.SetAttribute(CurrentSession.SESSION_DATAREADER_EXCEL_FILTROS, excel);
                        CurrentSession.SetAttribute(CurrentSession.SESSION_DATOS_EXCEL, dt);
                        CurrentSession.SetAttribute(CurrentSession.SESSION_TITULO_DATOS_EXCEL, "SOLICITUD VISITAS");
                        CurrentSession.SetAttribute(CurrentSession.SESSION_DATAREADER_EXCEL_FILTROS, null);
                    }



                    CargarPaginador();

                    Int32 intPageIndex = Int32.Parse(this.hdnPageIndex.Value);
                    Int32 PageSize = Int32.Parse(Resources.SMGConfiguration.GridViewPageSize.ToString());
                    Int32 Desde = (PageSize * (intPageIndex - 1));
                    Int32 Hasta = PageSize * intPageIndex;



                    SQLPaginacion += ") v WHERE  (FILA > " + Desde + " AND FILA <= " + Hasta + ")";
                    SQLPaginacion = "select distinct COD_CONTRATO,ID_SOLICITUD,NUM_FACTURA,[FECHA PROCESO],PENALIZAR,ESTADO,[CREACION SOL.],PROVEEDOR,POBLACION,PROVINCIA,t1,t2,URGENCIA, CIERRE FROM (" + SQLPaginacion;
                    SQLPaginacion += " order by [FECHA PROCESO]";
                    Datos = Consultas.ObtenerDatosDesdeSentencia(SQLPaginacion);
                    CargarDatos(Datos);

                    //Datos = Consultas.ObtenerDatosDesdeSentencia(SQL);
                    //CargarDatos(Datos);




                }
                catch (Exception ex)
                {
                    MostrarMensaje(ex.Message);
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
            }
        }
        private void SolVisInco()
        {
            Int32 numReg = 0;
            IDataReader Datos;

            lblProceso.Text = "Solicitudes Visita Incorrecta";
            lblFactura.Text = txtFacturaVisitaInco.Text;
            String SQL = "select distinct l.COD_CONTRATO,l.ID_SOLICITUD,l.NUM_FACTURA,l.FECHA AS [FECHA PROCESO],l.PENALIZAR,descripcion as ESTADO,s.fecha_creacion as [CREACION SOL.],s.PROVEEDOR,po.nombre as POBLACION,PR.NOMBRE AS PROVINCIA,m.t1,m.t2,m.URGENCIA, max(time_stamp) as CIERRE from liquidaciones l left JOIN MANTENIMIENTO M ON L.COD_CONTRATO=M.COD_CONTRATO_SIC left join poblacion po on m.cod_poblacion=po.cod_poblacion and m.cod_provincia=po.cod_provincia left join provincia pr on po.cod_provincia=pr.cod_provincia left join solicitudes s on l.id_solicitud=s.id_solicitud left join estado_solicitud e on s.estado_solicitud=e.codigo left join historico h on s.id_solicitud=h.id_solicitud where s.subtipo_solicitud ='003' and l.NUM_FACTURA='" + txtFacturaVisitaInco.Text + "' ";
            if (rdbPenalizables.SelectedValue != "3") { SQL += "AND L.PENALIZAR=" + rdbPenalizables.SelectedValue; }
            if (cmbProveedor.SelectedIndex > 0) { SQL += "AND s.proveedor like '" + cmbProveedor.SelectedValue + "%' "; }
            SQL += "group by e.descripcion,m.t1,m.t2,m.URGENCIA,l.[COD_CONTRATO],l.[ID_SOLICITUD],[COD_VISITA],l.[NUM_FACTURA],[FECHA],[PENALIZAR],fecha_creacion,s.PROVEEDOR,po.nombre,PR.NOMBRE ";
            String SQLPaginacion = SQL.Replace("distinct", "ROW_NUMBER() OVER(ORDER BY l.COD_CONTRATO ASC) AS fILA,");
            SQL += "order by l.fecha";

            try
            {
                try
                {
                    if (this.hdnPageIndex.Value == "") { this.hdnPageIndex.Value = "1"; }
                    if (this.hdnPageIndex.Value == "1")
                    {
                        Datos = Consultas.ObtenerDatosDesdeSentencia(SQL);
                        DataTable dt = new DataTable();
                        dt.Load(Datos);
                        numReg = dt.Rows.Count;
                        this.lblNumEncontrados.Text = numReg.ToString();

                        this.hdnPageCount.Value = ObtenerNumPaginas(numReg).ToString();
                        //CurrentSession.SetAttribute(CurrentSession.SESSION_DATAREADER_EXCEL_FILTROS, excel);
                        CurrentSession.SetAttribute(CurrentSession.SESSION_DATOS_EXCEL, dt);
                        CurrentSession.SetAttribute(CurrentSession.SESSION_TITULO_DATOS_EXCEL, "SOLICITUD VISITA INCORRECTA");
                        CurrentSession.SetAttribute(CurrentSession.SESSION_DATAREADER_EXCEL_FILTROS, null);
                    }



                    CargarPaginador();

                    Int32 intPageIndex = Int32.Parse(this.hdnPageIndex.Value);
                    Int32 PageSize = Int32.Parse(Resources.SMGConfiguration.GridViewPageSize.ToString());
                    Int32 Desde = (PageSize * (intPageIndex - 1));
                    Int32 Hasta = PageSize * intPageIndex;


                    SQLPaginacion += ") v WHERE  (FILA > " + Desde + " AND FILA <= " + Hasta + ")";
                    SQLPaginacion = "select distinct COD_CONTRATO,ID_SOLICITUD,NUM_FACTURA,[FECHA PROCESO],PENALIZAR,ESTADO,[CREACION SOL.],PROVEEDOR,POBLACION,PROVINCIA,t1,t2,URGENCIA, CIERRE FROM (" + SQLPaginacion;
                    SQLPaginacion += " order by [FECHA PROCESO]";
                    Datos = Consultas.ObtenerDatosDesdeSentencia(SQLPaginacion);
                    CargarDatos(Datos);

                    //Datos = Consultas.ObtenerDatosDesdeSentencia(SQL);
                    //CargarDatos(Datos);




                }
                catch (Exception ex)
                {
                    MostrarMensaje(ex.Message);
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
            }
        }
        private void SolReviPrecin()
        {
            Int32 numReg = 0;
            IDataReader Datos;

            lblProceso.Text = "Solicitudes Revisiton Precinte";
            lblFactura.Text = txtFacturaRevPrecinte.Text;
            String SQL = "select distinct l.COD_CONTRATO,l.ID_SOLICITUD,l.NUM_FACTURA,l.FECHA AS [FECHA PROCESO],l.PENALIZAR,descripcion as ESTADO,s.fecha_creacion as [CREACION SOL.],s.PROVEEDOR,po.nombre as POBLACION,PR.NOMBRE AS PROVINCIA,m.t1,m.t2,m.URGENCIA, max(time_stamp) as CIERRE from liquidaciones l left JOIN MANTENIMIENTO M ON L.COD_CONTRATO=M.COD_CONTRATO_SIC left join poblacion po on m.cod_poblacion=po.cod_poblacion and m.cod_provincia=po.cod_provincia left join provincia pr on po.cod_provincia=pr.cod_provincia left join solicitudes s on l.id_solicitud=s.id_solicitud left join estado_solicitud e on s.estado_solicitud=e.codigo left join historico h on s.id_solicitud=h.id_solicitud where s.subtipo_solicitud ='005'  and l.NUM_FACTURA='" + txtFacturaRevPrecinte.Text + "' ";
            if (rdbPenalizables.SelectedValue != "3") { SQL += "AND L.PENALIZAR=" + rdbPenalizables.SelectedValue; }
            if (cmbProveedor.SelectedIndex > 0) { SQL += "AND s.proveedor like '" + cmbProveedor.SelectedValue + "%' "; }
            SQL += "group by e.descripcion,m.t1,m.t2,m.URGENCIA,l.[COD_CONTRATO],l.[ID_SOLICITUD],[COD_VISITA],l.[NUM_FACTURA],[FECHA],[PENALIZAR],fecha_creacion,s.PROVEEDOR,po.nombre,PR.NOMBRE ";
            String SQLPaginacion = SQL.Replace("distinct", "ROW_NUMBER() OVER(ORDER BY l.COD_CONTRATO ASC) AS fILA,");
            SQL += "order by l.fecha";
            try
            {
                try
                {
                    if (this.hdnPageIndex.Value == "") { this.hdnPageIndex.Value = "1"; }
                    if (this.hdnPageIndex.Value == "1")
                    {
                        Datos = Consultas.ObtenerDatosDesdeSentencia(SQL);
                        DataTable dt = new DataTable();
                        dt.Load(Datos);
                        numReg = dt.Rows.Count;
                        this.lblNumEncontrados.Text = numReg.ToString();

                        this.hdnPageCount.Value = ObtenerNumPaginas(numReg).ToString();
                        //CurrentSession.SetAttribute(CurrentSession.SESSION_DATAREADER_EXCEL_FILTROS, excel);
                        CurrentSession.SetAttribute(CurrentSession.SESSION_DATOS_EXCEL, dt);
                        CurrentSession.SetAttribute(CurrentSession.SESSION_TITULO_DATOS_EXCEL, "SOLICITUD REVISION PRECINTE");
                        CurrentSession.SetAttribute(CurrentSession.SESSION_DATAREADER_EXCEL_FILTROS, null);
                    }



                    CargarPaginador();

                    Int32 intPageIndex = Int32.Parse(this.hdnPageIndex.Value);
                    Int32 PageSize = Int32.Parse(Resources.SMGConfiguration.GridViewPageSize.ToString());
                    Int32 Desde = (PageSize * (intPageIndex - 1));
                    Int32 Hasta = PageSize * intPageIndex;


                    SQLPaginacion += ") v WHERE  (FILA > " + Desde + " AND FILA <= " + Hasta + ")";
                    SQLPaginacion = "select distinct COD_CONTRATO,ID_SOLICITUD,NUM_FACTURA,[FECHA PROCESO],PENALIZAR,ESTADO,[CREACION SOL.],PROVEEDOR,POBLACION,PROVINCIA,t1,t2,URGENCIA, CIERRE FROM (" + SQLPaginacion;
                    SQLPaginacion += " order by [FECHA PROCESO]";
                    Datos = Consultas.ObtenerDatosDesdeSentencia(SQLPaginacion);
                    CargarDatos(Datos);

                    //Datos = Consultas.ObtenerDatosDesdeSentencia(SQL);
                    //CargarDatos(Datos);




                }
                catch (Exception ex)
                {
                    MostrarMensaje(ex.Message);
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
            }
        }
        private void SolAveria()
        {
            Int32 numReg = 0;
            IDataReader Datos;

            lblProceso.Text = "Solicitudes Averia";
            lblFactura.Text = txtFacturaSolAveria.Text;
            String SQL = "select distinct l.COD_CONTRATO,l.ID_SOLICITUD,l.NUM_FACTURA,l.FECHA AS [FECHA PROCESO],l.PENALIZAR,e.descripcion as ESTADO,s.fecha_creacion as [CREACION SOL.],s.PROVEEDOR,po.nombre as POBLACION,PR.NOMBRE AS PROVINCIA,m.t1,m.t2,S.URGENTE, max(time_stamp) as CIERRE,C.VALOR AS 'Código Barras',mc.descripcion as 'MOT. CANCELACION' from liquidaciones l left JOIN MANTENIMIENTO M ON L.COD_CONTRATO=M.COD_CONTRATO_SIC left join poblacion po on m.cod_poblacion=po.cod_poblacion and m.cod_provincia=po.cod_provincia left join provincia pr on po.cod_provincia=pr.cod_provincia left join solicitudes s on l.id_solicitud=s.id_solicitud left join estado_solicitud e on s.estado_solicitud=e.codigo left join historico h on s.id_solicitud=h.id_solicitud LEFT JOIN caracteristicas_solicitud C ON L.ID_SOLICITUD=C.ID_SOLICITUD AND TIP_CAR='015' left join motivo_cancelacion mc on s.Mot_cancel=mc.cod_mot where s.subtipo_solicitud ='001' and l.NUM_FACTURA='" + txtFacturaSolAveria.Text + "' ";
            if (rdbPenalizables.SelectedValue != "3") { SQL += "AND L.PENALIZAR=" + rdbPenalizables.SelectedValue; }
            if (cmbProveedor.SelectedIndex > 0) { SQL += " AND s.proveedor like '" + cmbProveedor.SelectedValue + "%' "; }
            SQL += "and ";
            SQL += "( ";
            SQL += "(s.estado_solicitud = '021' and h.estado_solicitud = '010') ";
            //SQL += "or s.estado_solicitud <> '021' ";


            SQL += "or (s.estado_solicitud = '018' and h.estado_solicitud = '010') ";
            SQL += "or (s.estado_solicitud <> '021' and s.estado_solicitud <> '010')";


            SQL += ") ";
            SQL += "group by e.descripcion,m.t1,m.t2,s.urgente,l.[COD_CONTRATO],l.[ID_SOLICITUD],[COD_VISITA],l.[NUM_FACTURA],[FECHA],[PENALIZAR],fecha_creacion,s.PROVEEDOR,po.nombre,PR.NOMBRE,c.valor,mc.descripcion ";
            String SQLPaginacion = SQL.Replace("distinct", "ROW_NUMBER() OVER(ORDER BY l.COD_CONTRATO ASC) AS fILA,");
            SQL += "order by l.fecha";
            try
            {
                try
                {
                    if (this.hdnPageIndex.Value == "") { this.hdnPageIndex.Value = "1"; }
                    if (this.hdnPageIndex.Value == "1")
                    {
                        Datos = Consultas.ObtenerDatosDesdeSentencia(SQL);
                        DataTable dt = new DataTable();
                        dt.Load(Datos);
                        numReg = dt.Rows.Count;
                        this.lblNumEncontrados.Text = numReg.ToString();

                        this.hdnPageCount.Value = ObtenerNumPaginas(numReg).ToString();
                        //CurrentSession.SetAttribute(CurrentSession.SESSION_DATAREADER_EXCEL_FILTROS, excel);
                        CurrentSession.SetAttribute(CurrentSession.SESSION_DATOS_EXCEL, dt);
                        CurrentSession.SetAttribute(CurrentSession.SESSION_TITULO_DATOS_EXCEL, "SOLICITUD AVERIA");
                        CurrentSession.SetAttribute(CurrentSession.SESSION_DATAREADER_EXCEL_FILTROS, null);
                    }



                    CargarPaginador();

                    Int32 intPageIndex = Int32.Parse(this.hdnPageIndex.Value);
                    Int32 PageSize = Int32.Parse(Resources.SMGConfiguration.GridViewPageSize.ToString());
                    Int32 Desde = (PageSize * (intPageIndex - 1));
                    Int32 Hasta = PageSize * intPageIndex;


                    SQLPaginacion += ") v WHERE  (FILA > " + Desde + " AND FILA <= " + Hasta + ")";
                    SQLPaginacion = "select distinct COD_CONTRATO,ID_SOLICITUD,NUM_FACTURA,[FECHA PROCESO],PENALIZAR,ESTADO,[CREACION SOL.],PROVEEDOR,POBLACION,PROVINCIA,t1,t2,URGENTE, CIERRE FROM (" + SQLPaginacion;
                    SQLPaginacion += " order by [FECHA PROCESO]";
                    Datos = Consultas.ObtenerDatosDesdeSentencia(SQLPaginacion);
                    CargarDatos(Datos);

                    //Datos = Consultas.ObtenerDatosDesdeSentencia(SQL);
                    //CargarDatos(Datos);




                }
                catch (Exception ex)
                {
                    MostrarMensaje(ex.Message);
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
            }
        }
        private void SolAveriaInco()
        {
            Int32 numReg = 0;
            IDataReader Datos;

            lblProceso.Text = "Solicitudes Averia Incorrecta";
            lblFactura.Text = txtFacturaSolAveriaInco.Text;
            String SQL = "select distinct l.COD_CONTRATO,l.ID_SOLICITUD,l.NUM_FACTURA,l.FECHA AS [FECHA PROCESO],l.PENALIZAR,e.descripcion as ESTADO,s.fecha_creacion as [CREACION SOL.],s.PROVEEDOR,po.nombre as POBLACION,PR.NOMBRE AS PROVINCIA,m.t1,m.t2,S.URGENTE, max(time_stamp) as CIERRE,mc.descripcion as 'MOT. CANCELACION' from liquidaciones l left JOIN MANTENIMIENTO M ON L.COD_CONTRATO=M.COD_CONTRATO_SIC left join poblacion po on m.cod_poblacion=po.cod_poblacion and m.cod_provincia=po.cod_provincia left join provincia pr on po.cod_provincia=pr.cod_provincia left join solicitudes s on l.id_solicitud=s.id_solicitud left join estado_solicitud e on s.estado_solicitud=e.codigo left join historico h on s.id_solicitud=h.id_solicitud left join motivo_cancelacion mc on s.Mot_cancel=mc.cod_mot where s.subtipo_solicitud ='004' and l.NUM_FACTURA='" + txtFacturaSolAveriaInco.Text + "' ";
            if (rdbPenalizables.SelectedValue != "3") { SQL += "AND L.PENALIZAR=" + rdbPenalizables.SelectedValue; }
            if (cmbProveedor.SelectedIndex > 0) { SQL += "AND m.proveedor like '" + cmbProveedor.SelectedValue + "%' "; }
            SQL += "and ";
            SQL += "( ";
            SQL += "(s.estado_solicitud = '021' and h.estado_solicitud = '010') ";
            SQL += "or ";
            SQL += "s.estado_solicitud <> '021' ";
            SQL += ") ";
            SQL += "group by e.descripcion,m.t1,m.t2,s.urgente,l.[COD_CONTRATO],l.[ID_SOLICITUD],[COD_VISITA],l.[NUM_FACTURA],[FECHA],[PENALIZAR],fecha_creacion,s.PROVEEDOR,po.nombre,PR.NOMBRE,mc.descripcion ";
            String SQLPaginacion = SQL.Replace("distinct", "ROW_NUMBER() OVER(ORDER BY l.COD_CONTRATO ASC) AS fILA,");
            SQL += "order by l.fecha";
            try
            {
                try
                {
                    if (this.hdnPageIndex.Value == "") { this.hdnPageIndex.Value = "1"; }
                    if (this.hdnPageIndex.Value == "1")
                    {
                        Datos = Consultas.ObtenerDatosDesdeSentencia(SQL);
                        DataTable dt = new DataTable();
                        dt.Load(Datos);
                        numReg = dt.Rows.Count;
                        this.lblNumEncontrados.Text = numReg.ToString();

                        this.hdnPageCount.Value = ObtenerNumPaginas(numReg).ToString();
                        //CurrentSession.SetAttribute(CurrentSession.SESSION_DATAREADER_EXCEL_FILTROS, excel);
                        CurrentSession.SetAttribute(CurrentSession.SESSION_DATOS_EXCEL, dt);
                        CurrentSession.SetAttribute(CurrentSession.SESSION_TITULO_DATOS_EXCEL, "SOLICITUD AVERIA INCORRECTA");
                        CurrentSession.SetAttribute(CurrentSession.SESSION_DATAREADER_EXCEL_FILTROS, null);
                    }



                    CargarPaginador();

                    Int32 intPageIndex = Int32.Parse(this.hdnPageIndex.Value);
                    Int32 PageSize = Int32.Parse(Resources.SMGConfiguration.GridViewPageSize.ToString());
                    Int32 Desde = (PageSize * (intPageIndex - 1));
                    Int32 Hasta = PageSize * intPageIndex;


                    SQLPaginacion += ") v WHERE  (FILA > " + Desde + " AND FILA <= " + Hasta + ")";
                    SQLPaginacion = "select distinct COD_CONTRATO,ID_SOLICITUD,NUM_FACTURA,[FECHA PROCESO],PENALIZAR,ESTADO,[CREACION SOL.],PROVEEDOR,POBLACION,PROVINCIA,t1,t2,URGENTE, CIERRE FROM (" + SQLPaginacion;
                    SQLPaginacion += " order by [FECHA PROCESO]";
                    Datos = Consultas.ObtenerDatosDesdeSentencia(SQLPaginacion);
                    CargarDatos(Datos);

                    //Datos = Consultas.ObtenerDatosDesdeSentencia(SQL);
                    //CargarDatos(Datos);




                }
                catch (Exception ex)
                {
                    MostrarMensaje(ex.Message);
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
            }
        }

        private void CargarDatos(IDataReader drDatos)
        {
            grdDatos.DataSource = drDatos;
            grdDatos.DataBind();
        }
        #endregion

        #region Implementación métodos abstractos
        public override void LoadSessionData()
        {

        }
        public override void SaveSessionData()
        {

        }
        public override void DeleteSessionData()
        {

        }
        #endregion

        #region Paginacion
        public Int32 ObtenerNumPaginas(Int32 numRegs)
        {
            if (numRegs > 0)
            {
                Int32 resto;
                Int32 resultado = Math.DivRem(numRegs, Int32.Parse(Resources.SMGConfiguration.GridViewPageSize), out resto);
                if (resto > 0)
                {
                    resultado++;
                }
                return resultado;
            }
            else
            {
                return 0;
            }
        }

        private void CargarPaginador()
        {
            Int32 intNumPaginas = Int32.Parse(this.hdnPageCount.Value);
            Int32 intPaginaActual = Int32.Parse(this.hdnPageIndex.Value);
            int numPaginasMostradas = 10;
            int rangoActual = intPaginaActual / numPaginasMostradas;
            int limiteSuperior = (rangoActual + 1) * numPaginasMostradas;

            if (limiteSuperior > intNumPaginas)
            {
                limiteSuperior = intNumPaginas;
            }

            int limiteInferior = (rangoActual * numPaginasMostradas) + 1;

            if (limiteInferior == intPaginaActual && intPaginaActual > 1)
            {
                limiteInferior -= numPaginasMostradas;
                if (limiteInferior < 0)
                {
                    limiteInferior = 1;
                }
                limiteSuperior -= numPaginasMostradas;
            }

            this.placeHolderPaginacion.Controls.Clear();

            if ((limiteInferior) > numPaginasMostradas)
            {
                Button lb = new Button();
                lb.ID = "hlPaginacion1";
                lb.CommandName = "hlPaginacion1";
                lb.CommandArgument = 1.ToString();
                lb.OnClientClick = "return ClickPaginacion('1')";
                lb.CssClass = "linkPaginacion";
                lb.Text = ("<<").ToString();
                this.placeHolderPaginacion.Controls.Add(lb);

            }

            for (int i = limiteInferior; i <= limiteSuperior; i++)
            {
                if (i == intPaginaActual)
                {
                    Label lb = new Label();
                    lb.ID = "hlPaginacion" + i;
                    lb.CssClass = "linkPaginacionDeshabilitado";
                    lb.Text = (i).ToString();
                    this.placeHolderPaginacion.Controls.Add(lb);
                }
                else
                {
                    Button lb = new Button();
                    lb.ID = "hlPaginacion" + i;
                    lb.CommandName = "hlPaginacion" + i;
                    lb.CommandArgument = i.ToString();
                    lb.OnClientClick = "return ClickPaginacion('" + i + "')";
                    lb.CssClass = "linkPaginacion";
                    lb.Text = (i).ToString();
                    this.placeHolderPaginacion.Controls.Add(lb);
                }

            }

            if (limiteSuperior < intNumPaginas - 10)
            {
                Button lb = new Button();
                lb.ID = "hlPaginacion" + intNumPaginas;
                lb.CommandName = "hlPaginacion" + intNumPaginas;
                lb.CommandArgument = intNumPaginas.ToString();
                lb.OnClientClick = "return ClickPaginacion('" + intNumPaginas + "')";
                lb.CssClass = "linkPaginacion";
                lb.Text = (">>").ToString();
                this.placeHolderPaginacion.Controls.Add(lb);
            }

        }

        protected void OnBtnPaginacion_Click(object sender, EventArgs e)
        {
            try
            {
                switch (lblProceso.Text)
                {
                    case "Visitas":
                        Reparaciones();
                        Visitas();
                        break;
                    case "Solicitudes Visita":
                        SolVisitas();
                        break;
                    case "Solicitudes Visita Incorrecta":
                        SolVisInco();
                        break;
                    case "Solicitudes Revisiton Precinte":
                        SolReviPrecin();
                        break;
                    case "Solicitudes Averia":
                        SolAveria();
                        break;
                    case "Solicitudes Averia Incorrecta":
                        SolAveriaInco();
                        break;
                    case "Reparaciones":
                        Visitas();
                        Reparaciones();
                        break;
                }

                Int32 intPageIndex = Int32.Parse(this.hdnPageIndex.Value);
                Int32 PageSize = Int32.Parse(Resources.SMGConfiguration.GridViewPageSize.ToString());
                Int32 Desde = (PageSize * (intPageIndex - 1));
                Int32 Hasta = PageSize * intPageIndex;

                CargarPaginador();

                string focusScript = "<script language='JavaScript'>MovimientoReparacion();</script>";
                Page.RegisterStartupScript("FocusScript", focusScript);
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }
        #endregion

        #region Paginacion Reparacion
        public Int32 ObtenerNumPaginasReparacion(Int32 numRegs)
        {
            if (numRegs > 0)
            {
                Int32 resto;
                Int32 resultado = Math.DivRem(numRegs, Int32.Parse(Resources.SMGConfiguration.GridViewPageSize), out resto);
                if (resto > 0)
                {
                    resultado++;
                }
                return resultado;
            }
            else
            {
                return 0;
            }
        }

        private void CargarPaginadorReparacion()
        {
            Int32 intNumPaginas = Int32.Parse(this.hdnPageCountReparacion.Value);
            Int32 intPaginaActual = Int32.Parse(this.hdnPageIndexReparacion.Value);
            int numPaginasMostradas = 10;
            int rangoActual = intPaginaActual / numPaginasMostradas;
            int limiteSuperior = (rangoActual + 1) * numPaginasMostradas;

            if (limiteSuperior > intNumPaginas)
            {
                limiteSuperior = intNumPaginas;
            }

            int limiteInferior = (rangoActual * numPaginasMostradas) + 1;

            if (limiteInferior == intPaginaActual && intPaginaActual > 1)
            {
                limiteInferior -= numPaginasMostradas;
                if (limiteInferior < 0)
                {
                    limiteInferior = 1;
                }
                limiteSuperior -= numPaginasMostradas;
            }

            this.placeHolderPaginacionReparacion.Controls.Clear();

            if ((limiteInferior) > numPaginasMostradas)
            {
                Button lb = new Button();
                lb.ID = "hlPaginacionReparacion1";
                lb.CommandName = "hlPaginacionReparacion1";
                lb.CommandArgument = 1.ToString();
                lb.OnClientClick = "return ClickPaginacionReparacion('1')";
                lb.CssClass = "linkPaginacion";
                lb.Text = ("<<").ToString();
                this.placeHolderPaginacionReparacion.Controls.Add(lb);

            }

            for (int i = limiteInferior; i <= limiteSuperior; i++)
            {
                if (i == intPaginaActual)
                {
                    Label lb = new Label();
                    lb.ID = "hlPaginacionReparacion" + i;
                    lb.CssClass = "linkPaginacionDeshabilitado";
                    lb.Text = (i).ToString();
                    this.placeHolderPaginacionReparacion.Controls.Add(lb);
                }
                else
                {
                    Button lb = new Button();
                    lb.ID = "hlPaginacionReparacion" + i;
                    lb.CommandName = "hlPaginacionReparacion" + i;
                    lb.CommandArgument = i.ToString();
                    lb.OnClientClick = "return ClickPaginacionReparacion('" + i + "')";
                    lb.CssClass = "linkPaginacion";
                    lb.Text = (i).ToString();
                    this.placeHolderPaginacionReparacion.Controls.Add(lb);
                }

            }

            if (limiteSuperior < intNumPaginas - 10)
            {
                Button lb = new Button();
                lb.ID = "hlPaginacionReparacion" + intNumPaginas;
                lb.CommandName = "hlPaginacionReparacion" + intNumPaginas;
                lb.CommandArgument = intNumPaginas.ToString();
                lb.OnClientClick = "return ClickPaginacionReparacion('" + intNumPaginas + "')";
                lb.CssClass = "linkPaginacion";
                lb.Text = (">>").ToString();
                this.placeHolderPaginacionReparacion.Controls.Add(lb);
            }

        }

        protected void OnBtnPaginacionReparacion_Click(object sender, EventArgs e)
        {
            try
            {
                Visitas();
                Reparaciones();

                Int32 intPageIndex = Int32.Parse(this.hdnPageIndexReparacion.Value);
                Int32 PageSize = Int32.Parse(Resources.SMGConfiguration.GridViewPageSize.ToString());
                Int32 Desde = (PageSize * (intPageIndex - 1));
                Int32 Hasta = PageSize * intPageIndex;

                CargarPaginadorReparacion();

                string focusScript = "<script language='JavaScript'>MovimientoReparacion();</script>";
                Page.RegisterStartupScript("FocusScript", focusScript);
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }
        #endregion

        private void MostrarMensaje(String Mensaje)
        {
            string alerta = "<script type='text/javascript'>alert('" + Mensaje + "');</script>";

            ScriptManager.RegisterStartupScript(this, this.GetType(), "ALERTA", alerta, false);
        }
        protected void rdbPenalizables_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.hdnPageCount.Value = "";
            switch (lblProceso.Text)
            {
                case "Visitas":
                    PanelVisita();
                    break;
                case "Solicitudes Visita":
                    PanelSolVisita();
                    break;
                case "Solicitudes Visita Incorrecta":
                    PanelSolVisitaIncorrecta();
                    break;
                case "Solicitudes Revisiton Precinte":
                    PanelSolReviPrecinte();
                    break;
                case "Solicitudes Averia":
                    PanelSolAveria();
                    break;
                case "Solicitudes Averia Incorrecta":
                    PanelSolAveriaIncorrecta();
                    break;
            }
        }
        protected void cmbProveedor_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (lblProceso.Text)
            {
                case "Visitas":
                    PanelVisita();
                    break;
                case "Solicitudes Visita":
                    PanelSolVisita();
                    break;
                case "Solicitudes Visita Incorrecta":
                    PanelSolVisitaIncorrecta();
                    break;
                case "Solicitudes Revisiton Precinte":
                    PanelSolReviPrecinte();
                    break;
                case "Solicitudes Averia":
                    PanelSolAveria();
                    break;
                case "Solicitudes Averia Incorrecta":
                    PanelSolAveriaIncorrecta();
                    break;
            }
        }
    }
}
