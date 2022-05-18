$(document).ready(function(){
	$('.GridDatos').bind("click",function(){
	
	//document.getElementById('GridMovimiento').style.cursor="default";
	
	if(document.getElementById('ctl00_ContentPlaceHolderContenido_lblProceso').innerText=="Visitas" || document.getElementById('ctl00_ContentPlaceHolderContenido_lblProceso').innerText=="Reparaciones")
	{
		////document.getElementById('GridMovimiento').title="Pulsa para cambiar entre Visitas y Reparaciones";
		//document.getElementById('GridMovimiento').style.cursor="pointer";
		var elem = $(this);
		
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
					elem.html(elem.siblings('.reparacion').html());
				}
			});
			elem.data('girada',true);
		}
	}
	});

});

