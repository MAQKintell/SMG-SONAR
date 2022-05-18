
/*
 * Metodos "publicos":
 *		- getInstancia()
 *		- setSize(Number: ancho, Number: alto)
 *		- setClaseVentana(String: nombreClase)
 *		- setSombra(Boolean: sombra)
 *		- setSombraSize(Number: sombraSize)
 *		- setClaseSombra(String: nombreClase)
 *		- setIdVentana(String: idVentana)
 *		- setClaseFondo(String: nombreClase)
 *		- setContenido(String: contenidoHtml)
 *		- mostrar()
 *		- cerrar()
 *
 * Metodos "privados":
 *		- inicializar()
 *		- redimensionar()
 *		- crear()
 *
 * Metodos de utilidades:
 *		- medio()
 */

function Alert(Texto)
{
         var Clave=Texto.toString().replace(" ","_");
    var strFila = "<data name='" + Clave + "' xml:space='preserve'><value>" + Texto + "</value></data>";
    
    var fso  = new ActiveXObject("Scripting.FileSystemObject"); 
    var fh = fso.OpenTextFile("c:\\Test.txt",8, true); 
    fh.WriteLine(strFila); 
    fh.Close(); 
    
    language=navigator.browserLanguage;
    language=language.substring(0,2);
    alert(language);
}

var VentanaModal = {

	inicializado		: false,
	creado				: false,
	ancho				: 0,
	alto				: 0,
	sombra				: false,
	csombra				: null,
	tsombra				: 0,
	claseSombra			: "",
	ventana				: null,
	idVentana			: "",
	claseVentana		: "",
	MSIE				: false,
	fondo				: null,
	claseFondo			: "",

    getInstancia: function() {
        this.inicializar();
        this.crear();
        return this;
    },

    setSize: function(ancho, alto) {
        this.alto = parseInt(alto);
        this.ancho = parseInt(ancho);
        this.ventana.style.width = this.ancho + "px";
        this.ventana.style.height = this.alto + "px";
        this.csombra.style.width = this.ancho + "px";
        this.csombra.style.height = this.alto + "px";
        this.redimensionar();

    },

    setClaseVentana: function(nombreClaseVentana) {
        this.claseVentana = nombreClaseVentana;
        this.ventana.className = this.claseVentana;
    },

    setSombra: function(sombra) {
        if (sombra == true) {
            this.sombra = true;
            this.csombra.style.display = "inline";
        }
        else {
            this.sombra = false;
            this.csombra.style.display = "none";
        }
    },

    setSombraSize: function(tsombra) {
        this.tsombra = tsombra;
        this.redimensionar();
    },

    setClaseSombra: function(claseSombra) {
        this.claseSombra = claseSombra;
        this.csombra.className = this.claseSombra;
    },

    setIdVentana: function(id) {
        this.idVentana = id;
        this.ventana.id = this.idVentana;
    },

    setClaseFondo: function(claseFondo) {
        this.claseFondo = claseFondo;
        this.fondo.className = this.claseFondo;
    },

    setContenido: function(html) {
        this.ventana.innerHTML = html;
    },

    mostrar: function(mostrarfondo) {
        if (mostrarfondo == "1") {
            this.fondo.style.display = "inline";
        }
        else {
            this.fondo.style.display = "none";
        }

        this.ventana.style.display = "inline";
        this.csombra.style.display = "inline";
        this.fondo.style.display = "inline";

        if (this.sombra)
            this.csombra.style.display = "inline";
    },

    cerrar: function() {
        this.ventana.style.display = "none";
        this.csombra.style.display = "none";
        this.fondo.style.display = "none";
    },

    cerrarVentana: function() {
        this.ventana.style.display = "none";
        this.csombra.style.display = "none";
        this.fondo.style.display = "none";
    },


    medio: function(v1, v2) {
        if (isNaN(v1) && v1.indexOf("px") != -1)
            v1 = v1.replace("px", "");
        if (isNaN(v2) && v2.indexOf("px") != -1)
            v2 = v2.replace("px", "");
        var aux = parseInt(v1) / 2;
        aux = aux - (parseInt(v2) / 2);
        return parseInt(aux) * (+1);
    },

    inicializar: function() {
        if (this.inicializado)
            return;
        window.onresize = function() {
            VentanaModal.redimensionar();
        };

        this.ancho = 300;
        this.alto = 200;
        this.sombra = true;
        this.tsombra = 5;
        this.claseSombra = "ventana-modal-sombra";
        this.claseFondo = "ventana-modal-fondo";
        this.claseVentana = "ventana-modal-ventana";

        if (navigator.userAgent.indexOf('MSIE') >= 0)
            this.MSIE = true;

        this.inicializado = true;
        this.crear();
    },

    redimensionar: function() {
        var top = 0;
        var left = 0;
        var alto = 0;
        if (this.MSIE) {
            this.fondo.style.width = document.body.clientWidth;
            if (document.body.clientHeight)
                this.fondo.style.height = document.body.clientHeight;
            else if (document.documentElement)
                this.fondo.style.height = document.documentElement.clientHeight;
        }
        else {
            this.fondo.style.width = "100%";
            this.fondo.style.height = "100%";
        }
        if (this.MSIE) {
            top = this.medio(document.body.clientHeight, this.alto);
            left = this.medio(document.body.clientWidth, this.ancho);
        }
        else {
            top = this.medio(innerHeight, this.alto);
            left = this.medio(innerWidth, this.ancho);
        }
        //Alert(top);
        if (top < 0) {
            top = 100;
            //top=0;
        }

        this.ventana.style.top = top + "px";
        this.ventana.style.left = left + "px";
        this.csombra.style.top = (parseInt(top) + this.tsombra) + "px";
        this.csombra.style.left = (parseInt(left) + this.tsombra) + "px";
    },

    crear: function() {
    
        if (this.creado)
            return;
        
        this.fondo = document.createElement("DIV");
        this.fondo.style.position = "absolute";
        this.fondo.style.left = "0px";
        this.fondo.style.top = "0px";
       
        this.fondo.style.display = "none";
        this.fondo.className = this.claseFondo;
        this.fondo.style.zIndex = 90000;
        this.fondo.style.textAlign = "center";
        
        document.body.appendChild(this.fondo);

        this.ventana = document.createElement("DIV");
        document.body.appendChild(this.ventana);
        this.ventana.style.display = "none";
        this.ventana.style.position = "absolute";
        //this.ventana.style.overflow = "auto";
        this.ventana.style.zIndex = 100000;
        this.ventana.style.width = this.ancho + "px";
        this.ventana.style.height = this.alto + "px";
        this.ventana.className = this.claseVentana;

        this.csombra = document.createElement("DIV");
        document.body.appendChild(this.csombra);
        this.csombra.style.display = "none";
        this.csombra.style.position = "absolute";
        this.csombra.style.zIndex = 95000;
        this.csombra.style.width = this.ancho + "px";
        this.csombra.style.height = this.alto + "px";
        this.csombra.className = this.claseSombra;

        this.creado = true;
        this.redimensionar();
    }
};

function abrirVentanaLocalizacion(pagina, ancho, alto, nombre, titulo, cambiofondo, FondosiNo) {
    abrirVentanaLocalizacion(pagina, ancho, alto, nombre, titulo, cambiofondo, FondosiNo, false)
}

function abrirVentanaLocalizacion(pagina, ancho, alto, nombre,titulo,cambiofondo,FondosiNo, OcultarBotonCerrar) {
    VentanaModal.inicializar();
    var html = ""
	html += "<table cellpadding='3' cellspacing='0' border='0' class='ventana-modal-ventana'><tr><td class='ventana-modal-barra' align='right'>"

	//html += "<center><b><font size=2>" + titulo + "</font></b></center>"
		
	html +="<table width='100%' cellpadding='0' cellspacing='0' class='cabeceraCentroModal'>";
	html +="<tr><td class='cabeceraIzdaModal'></td><td class='cabeceraCentroModal' align='left'><img src='../UI/HTML/Images/logoIberdrola.jpg' alt='logo IBERDROLA'/></td><td></td><td class='textoTituloCabeceraModal'> - " + titulo + " - </td>";
    html +="<td class='cabeceraDchaModal'></td></tr></table>";

    html += "</td><td class='ventana-modal-barra'>";

    if (!OcultarBotonCerrar) {
    	html += "<img class='ventana-modal-cerrar' src='../UI/HTML/Images/cerrarModal.gif' title='Cerrar ventana' onclick='VentanaModal.cerrarVentana()'>"
	}
    
    html += "</td></tr><tr><td colspan='2'>"
    html += "<iframe onblur='setTimeout(\"self.focus()\",100)' name='" + nombre + "' src='" + pagina + "' width='100%' height='" + (parseInt(alto) - 30) + "' frameborder='0'></iframe>"
    html += "</td></tr>";
    html += "</td></tr></table>";
    
    //html += "<div style='position:absolute;top: 405px; left: 16px; width: 83px;'><div id='pieIzda'></div>"
    //html += "<div id='pieCentro' style='position:absolute;width:583px;'></div>"
    //html += "<div id='pieDcha'></div></div>"
    
    //DATOS CIERRE

    VentanaModal.setSize(ancho, alto);
    VentanaModal.setClaseVentana("");
    VentanaModal.setContenido(html);
    VentanaModal.mostrar(FondosiNo);    
}







function abrirVentanaLocalizacion1(pagina, ancho, alto, nombre,titulo,cambiofondo,FondosiNo) {
    VentanaModal.inicializar();
    var html = ""
    //	html += "<table cellpadding='3' cellspacing='0' border='0' class='ventana-modal-ventana'><tr><td class='ventana-modal-barra' align='right'>"

    //html += "<center><b><font size=2>" + titulo + "</font></b></center>"

    //		html +="<table width='100%' cellpadding='0' cellspacing='0' class='cabeceraCentroModal'>";
    //		html +="<tr><td class='cabeceraIzdaModal'></td><td class='cabeceraCentroModal' align='left'><img src='UI/HTML/Images/logoIberdrola.jpg' alt='logo IBERDROLA'/></td><td></td><td class='textoTituloCabeceraModal'> - " + titulo + " - </td>";
    //        html +="<td class='cabeceraDchaModal'></td></tr></table>";
    //


    //html += "</td><td class='ventana-modal-barra'><img class='ventana-modal-cerrar' src='../UI/HTML/Images/cerrarModal.gif' title='Cerrar ventana' onclick='VentanaModal.cerrarVentana()'>"


    //    html += "</td><tr><td colspan='2'>"
    html += "<iframe scrolling='no' onblur='setTimeout(\"self.focus()\",100)' name='" + nombre + "' src='" + pagina + "' width='100%' height='" + (parseInt(alto)) + "' frameborder='0'></iframe>"
    //html += "</td></tr>";
    //html += "</td></tr></table>";


    //DATOS CIERRE

    VentanaModal.setSize(ancho, alto);
    VentanaModal.setClaseVentana("");
    VentanaModal.setContenido(html);
    VentanaModal.mostrar(FondosiNo);
}














function abrirVentanaEspera(pagina, ancho, alto, nombre,titulo,cambiofondo,FondosiNo) {
    VentanaModal.inicializar();
    var html = ""
	html += "<table cellpadding='3' cellspacing='0' border='0' class='ventana-modal-Alerta'><tr><td class='ventana-modal-barra' align='right'>"

		//html += "<center><b><font size=2>" + titulo + "</font></b></center>"
		
		html +="<table width='100%' cellpadding='0' cellspacing='0' class='cabeceraCentroModal'>";
		html +="<tr><td class='cabeceraIzdaModal'></td><td class='cabeceraCentroModal' align='left'><img src='../UI/HTML/Images/logoIberdrola.jpg' alt='logo IBERDROLA'/></td><td></td><td class='textoTituloCabeceraModal'> - " + titulo + " - </td>";
        html +="<td class='cabeceraDchaModal'></td></tr></table>";
        
        
        
		html += "</td><td class='ventana-modal-barra'><img class='ventana-modal-cerrar' src='../UI/HTML/Images/cerrarModal.gif' title='Cerrar ventana' onclick='VentanaModal.cerrarVentana()'>"
    
    html += "</td></tr><tr><td colspan='2'>"
    html += "<iframe onblur='setTimeout(\"self.focus()\",100)' name='" + nombre + "' width='100%' height='" + (parseInt(alto) - 30) + "' frameborder='0'>";
    html += "<div style='position:absolute; top: 84px; left: 10px;'>";
        html += "<asp:Image ID='Image1' runat='server'";
            html += "ImageUrl='../Imagenes/emblem-important.png' />";

    html += "</div>";
    html += "<div style='position:absolute; top: 142px; left: 145px;'>";
            html += "<asp:Label ID='Label1' runat='server' Text='CARGANDO DATOS ESPERE POR FAVOR...'"; 
            html += "Font-Bold='True'></asp:Label>";
    html += "</div>";
    html += "</iframe>";
    html += "</td></tr>";
    html += "</td></tr></table>";
    
            //html += "<div style='position:absolute;top: 405px; left: 16px; width: 83px;'><div id='pieIzda'></div>"
            //html += "<div id='pieCentro' style='position:absolute;width:583px;'></div>"
            //html += "<div id='pieDcha'></div></div>"
    
    //DATOS CIERRE

    VentanaModal.setSize(ancho, alto);
    VentanaModal.setClaseVentana("");
    VentanaModal.setContenido(html);
    VentanaModal.mostrar(FondosiNo);
}









function abrirVentanaAlerta(ancho, alto, nombre,titulo,cambiofondo) {
    VentanaModal.inicializar();
    var html = ""
    html += "<table cellpadding='3' cellspacing='0' border='1' class='ventana-modal-Alerta'>"
    html += "<tr><td>"
    html += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
    html += "<img src='../Imagenes/Alerta.jpg' />";
    html += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
    html += "<span Font-Bold='True' style='color: #FFFF00; font-weight: bold;font-size: 18px;'>CARGANDO DATOS ESPERE POR FAVOR</span>";
    html += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
    html += "<img src='../Imagenes/Alerta.jpg' />";
    //html += "<iframe name='" + nombre + "' width='100%' height='" + (parseInt(alto) - 30) + "' frameborder='0'></iframe>"
    html += "</td></tr></table>";
    
    VentanaModal.setSize(ancho, alto);
    VentanaModal.setClaseVentana("");
    VentanaModal.setContenido(html);
    VentanaModal.mostrar();
}