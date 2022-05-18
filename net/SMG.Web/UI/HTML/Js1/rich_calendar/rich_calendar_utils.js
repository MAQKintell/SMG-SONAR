function ShowCalendar(element, controlId)
{
    var format = '%j/%m/%Y';
    if (document.calendars == undefined)
    {
        document.calendars = new Array();  
    }
    else
    {
        if (document.calendars[controlId])
        {
            //return;
        }
    }
    
    document.calendars[controlId] = null;

    tempX = event.clientX + document.body.scrollLeft;
    tempY = event.clientY-100;// + document.body.scrollTop;


    var text_field = document.getElementById(controlId);

    document.calendars[controlId] = new RichCalendar();
    document.calendars[controlId].controlId = controlId
    document.calendars[controlId].start_week_day = 1;
    document.calendars[controlId].show_time = false;
    document.calendars[controlId].language = 'es';
    document.calendars[controlId].user_onchange_handler = function (cal, object_code) {
	                                    if (object_code == 'day') {
		                                    document.getElementById(cal.controlId).value = cal.get_formatted_date(format);
		                                    cal.hide();
		                                    document.calendars[cal.controlId] = null;
	                                    }
                                    };
    document.calendars[controlId].user_onclose_handler = function cal2_on_close(cal) {
	                                    cal.hide();
	                                    document.calendars[cal.controlId] = null;
                                    };
    document.calendars[controlId].user_onautoclose_handler = function (cal) {
	                                    document.calendars[cal.controlId] = null;
                                    };

    document.calendars[controlId].parse_date(text_field.value, format);

    document.calendars[controlId].show_at_position(text_field, tempX, tempY);
    //cal_obj2.show_at_element(text_field, "adj_right-top");



                               

                                    // user defined onclose handler (used in pop-up mode - when auto_close is true)
                                    

                                    // user defined onautoclose handler
                                    
}