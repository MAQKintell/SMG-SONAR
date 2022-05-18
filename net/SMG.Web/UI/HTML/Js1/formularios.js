    function Resaltar_On(GridView)
    {
        if(GridView != null)
        {
            GridView.originalBgColor = GridView.style.backgroundColor;
            GridView.style.backgroundColor="#DBE7F6";
        }
    }
    function Resaltar_Off(GridView)
    {
        if(GridView != null)
        {
           GridView.style.backgroundColor = GridView.originalBgColor;
        }
    }