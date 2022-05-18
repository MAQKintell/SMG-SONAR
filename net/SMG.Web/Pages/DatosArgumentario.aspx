<%@ Page Language="VB" EnableEventValidation="false" AutoEventWireup="true" CodeFile="DatosArgumentario.aspx.vb" Inherits="Pages_DatosArgumentario" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title><%=Resources.TextosJavaScript.TEXTO_DATOS_ARGUMENTARIO%></title>
</head>
<body style="background-color:#dce4d9;">
    <form id="form1" runat="server" style="background-color:#dce4d9;">
    <div style="position:absolute; top: -1px; left: 0px; height: 389px; width: 64px;">
        <asp:image ID="Image1" runat="server" ImageUrl="~/Imagenes/cabeceraCentro.gif" 
            Height="402px" Width="65px" />
    </div>
    <div style="position:absolute; top: 50px; left: 397px;z-index:10000">
        <asp:Calendar ID="CalendarDesde" runat="server" BackColor="White" 
            BorderColor="#999999" CellPadding="4" DayNameFormat="Shortest" 
            Font-Names="Verdana" Font-Size="8pt" ForeColor="Black" Height="180px" 
            Visible="False" Width="200px">
            <SelectedDayStyle BackColor="#666666" Font-Bold="True" ForeColor="White" />
            <SelectorStyle BackColor="#CCCCCC" />
            <WeekendDayStyle BackColor="#FFFFCC" />
            <TodayDayStyle BackColor="#CCCCCC" ForeColor="Black" />
            <OtherMonthDayStyle ForeColor="#808080" />
            <NextPrevStyle VerticalAlign="Bottom" />
            <DayHeaderStyle BackColor="#CCCCCC" Font-Bold="True" Font-Size="7pt" />
            <TitleStyle BackColor="#999999" BorderColor="Black" Font-Bold="True" />
        </asp:Calendar>
        <asp:Calendar ID="CalendarHasta" runat="server" BackColor="White" 
            BorderColor="#999999" CellPadding="4" DayNameFormat="Shortest" 
            Font-Names="Verdana" Font-Size="8pt" ForeColor="Black" Height="180px" 
            Visible="False" Width="200px">
            <SelectedDayStyle BackColor="#666666" Font-Bold="True" ForeColor="White" />
            <SelectorStyle BackColor="#CCCCCC" />
            <WeekendDayStyle BackColor="#FFFFCC" />
            <TodayDayStyle BackColor="#CCCCCC" ForeColor="Black" />
            <OtherMonthDayStyle ForeColor="#808080" />
            <NextPrevStyle VerticalAlign="Bottom" />
            <DayHeaderStyle BackColor="#CCCCCC" Font-Bold="True" Font-Size="7pt" />
            <TitleStyle BackColor="#999999" BorderColor="Black" Font-Bold="True" />
        </asp:Calendar>
    </div>
    <div style="position:absolute; top: 10px; left: 74px; background-color: #dce4d9;">
    
        <asp:ImageButton ID="ImageButton1" runat="server" 
            ImageUrl="~/Imagenes/IconoAltoExcel.jpg" ToolTip="Exportar a Excel" />
            <div style="position:absolute; top: 37px; left: 390px;" style="width: 82px">
                <asp:CheckBox ID="chkTodos" runat="server" Text="TODOS" Font-Bold="true" 
                    ForeColor="Red" /></div>
                    <div style="position:absolute; top: 25px; left: 63px; width: 406px;"><hr /></div>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    <div style="position:absolute; top: 36px; left: 128px;">
        <asp:Label ID="Label1" runat="server" Text="Fecha Desde" ForeColor="firebrick"></asp:Label>
        <asp:TextBox ID="txtDesde" runat="server" MaxLength="10" Width="74px"></asp:TextBox>
        <asp:ImageButton ID="ImageButton4" runat="server" 
            ImageUrl="~/Imagenes/calendar.gif" />
        <br />
        <asp:Label ID="Label2" runat="server" Text="Fecha Hasta" ForeColor="firebrick"></asp:Label>&nbsp; <asp:TextBox ID="txtHasta" runat="server" MaxLength="10" Width="74px"></asp:TextBox>&nbsp;<asp:ImageButton 
            ID="ImageButton5" runat="server" ImageUrl="~/Imagenes/calendar.gif" />
        </div>
        
        
        <div style="position:absolute; top: 5px; left: 126px;"><asp:Label ForeColor="firebrick" ID="lblContrato" runat="server" Font-Bold="True" 
            Text="COD. Contrato"></asp:Label>
            
            <div style="position:absolute; top: 90px; left: 196px; width: 264px;"><asp:Label ForeColor="#4a9024" ID="lblCodContrato" runat="server" Text="COD. CONTRATO" Font-Bold="True" 
            ></asp:Label>&nbsp;<asp:Label ForeColor="#4a9024" ID="lblConContrato1" runat="server" Text=""  
             Font-Bold="true"></asp:Label></div>
&nbsp;<asp:TextBox ID="txtContrato" runat="server"></asp:TextBox>
        <asp:Button ID="btnBuscar" runat="server" Text="Buscar" /></div>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <div style="position:absolute; top: 0px; left: 556px;" style="width: 57px"><asp:ImageButton ID="ImageButton3" runat="server" 
            ImageUrl="~/Imagenes/flecha_Iz.gif" ToolTip="Anterior" Visible="False" />
        &nbsp;
        <asp:ImageButton ID="ImageButton2" runat="server" Height="50px" 
            ImageUrl="~/Imagenes/flecha_Der.gif" style="height: 40px" Width="18px" 
            ToolTip="Siguiente" Visible="False" /></div>
        <br /><br /><br /><hr />
        <br /><asp:Label ForeColor="#4a9024" ID="lblTelefono" runat="server" Text="Label" Font-Bold="True" 
            ></asp:Label><asp:Label ID="lblTelefono1" runat="server" Text=""  
            ForeColor="Black"></asp:Label><br /><br />
        <asp:Label ForeColor="#4a9024" ID="lblFecha" runat="server" Text="Label" Font-Bold="True" 
            ></asp:Label><asp:Label ID="lblFecha1" runat="server" Text=""  
            ForeColor="Black"></asp:Label><br /><br />
        <asp:Label ForeColor="#4a9024" ID="lblGrupoBaja" runat="server" Text="Label" Font-Bold="True" 
            ></asp:Label><asp:Label ID="lblGrupoBaja1" runat="server" Text=""  
            ForeColor="Black"></asp:Label><br /><br />
        <asp:Label ForeColor="#4a9024" ID="lblMotivoBaja" runat="server" Text="Label" Font-Bold="True" 
            ></asp:Label><asp:Label iD="lblMotivoBaja1" runat="server" Text=""  
            ForeColor="Black"></asp:Label><br /><br />
        <asp:Label ID="lblAccion" runat="server" Text="Label" ForeColor="#4a9024" Font-Bold="True" 
            ></asp:Label><asp:Label ID="lblAccion1" runat="server" Text=""  
            ForeColor="Black"></asp:Label><br /><br />
        <asp:Label ID="lblResultado" runat="server" Text="Label" Font-Bold="True" 
            ForeColor="#4a9024"></asp:Label><asp:Label ID="lblResultado1" runat="server" Text=""
            ForeColor="Black"></asp:Label><br />
        <br />
    
    </div>
    </form>
</body>
</html>
