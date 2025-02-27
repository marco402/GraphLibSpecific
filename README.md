
En Francais plus loin.

Specific version of graphlib developed for the SdrSharp plugin application for rtl_433.

The original version can be found at:

https://www.codeproject.com/Articles/32836/A-simple-C-library-for-graph-plotting

Zoom fix Version("1.5.8.0")

-Processing a single LayoutMode case: VERTICAL_ARRANGED

-the X-labels are defined in pixels to take account of the window width:
plotterDisplayExDevices.SetMarkerXPixels(100);.

-The refreshPoints function(ListPointF>[] tabPoints) allows you to renew the data.

-Graphics are synchronous in X, same zoom and same displacement

-The refreshPoints call changes the data but retains zoom and move as long as the new data contains the data displayed in X..

-The zoom is controlled by the mouse wheel.

-If the point number is less than the pixel number in X of the graph divided by 20, each delta is displayed.

Dump screen on my web site: https://marco40github.wixsite.com/website/graphiques-graphlib?lang=en

____________________________________________________________________________________________________________

Version spécifique de graphlib développée pour l'application plugin SdrSharp pour rtl_433.

La version d'origine se trouve à cette adresse:

https://www.codeproject.com/Articles/32836/A-simple-C-library-for-graph-plotting

Correctif zoom Version("1.5.8.0")

-Traitement d'un seul cas de LayoutMode: VERTICAL_ARRANGED

-les labels en X sont définis en pixels pour tenir compte de la largeur de la fenêtre:
				plotterDisplayExDevices.SetMarkerXPixels(100);.

-La fonction refreshPoints(List<PointF>[] tabPoints) permet de renouveler les données.

-Les graphiques sont synchrones en X, même zoom et même déplacement.

-L'appel de refreshPoints change les données mais conserve le zoom et le déplacement à condition que les nouvelles données contiennent la donnée affichée en X.

-Le zoom est commandé par la molette de la souris.

-Si le nombre de point est inférieur au nombre de pixel en X du graphique divisé par 20, chaque delta est affiché.

Copies d'écran sur mon site: https://marco40github.wixsite.com/website/graphiques-graphlib





