//$(document).ready(function(){
$(window).load(function(){
	//$('.GridDatosCalderas').bind("click",function() {
	
	if(document.getElementById('ctl00_ContentPlaceHolderContenido_hdnNumeroEquivalencias').value=="2"
	 || document.getElementById('ctl00_ContentPlaceHolderContenido_hdnNumeroEquivalencias').value=="1")
	{
	
	    //alert('000');
	//document.getElementById('GridMovimiento').style.cursor="default";
	
		////document.getElementById('GridMovimiento').title="Pulsa para cambiar entre Visitas y Reparaciones";
		//document.getElementById('GridDatosCalderas').style.cursor="pointer";
		if(document.getElementById('ctl00_ContentPlaceHolderContenido_hdnNumeroEquivalencias').value=="2")
		{
		    var elem = $(document.getElementById('GridDatosCalderas'));
		}
		else{
		    var elem = $(document.getElementById('VariosClientesCalderas'));
		    //document.getElementById('VariosClientesCalderas').style.visibility='visible';
		    //elem.data('girada',true);
		}
		
		if(elem.data('girada'))
		{
			elem.revertFlip();
			elem.data('girada',false)
		}
		else
		{
			elem.flip({
				direction:'lr',
				speed: 300,
				onBefore: function(){
					elem.html(elem.siblings('.VariosClientesCalderas').html());
				}
			});
			elem.data('girada',true);
		}
		
		//alert('Se han encontrado mas de una coincidencia con los criterios de búsqueda.');
	}
	//});
	
	$('.GridMovimientoCalderas').bind("click",function(){
	//document.getElementById('GridMovimiento').style.cursor="default";
	if(document.getElementById('ctl00_ContentPlaceHolderContenido_hdnNumeroEquivalencias').value=="1")
	{
		////document.getElementById('GridMovimiento').title="Pulsa para cambiar entre Visitas y Reparaciones";
		//document.getElementById('GridMovimiento').style.cursor="pointer";
		var elem = $(document.getElementById('GridDatosCalderas'));
				    
		elem.revertFlip();
		elem.data('girada',false);
      
	}
	});
	
});