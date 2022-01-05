/* spécifique version Modified by Marc Prieur (marco40_github@sfr.fr)
 *                             PlotterGraphTypes.cs 
 *                     spécifique version for project Rtl_433_Plugin
 *						         Plugin for SdrSharp
 */
/* use modified library GraphLib: Copyright (c) 2008-2014 DI Zimmermann Stephan (stefan.zimmermann@tele2.at)
 *   
 * Permission is hereby granted, free of charge, to any person obtaining a copy 
 * of this software and associated documentation files (the "Software"), to 
 * deal in the Software without restriction, including without limitation the 
 * rights to use, copy, modify, merge, publish, distribute, sublicense, and/or 
 * sell copies of the Software, and to permit persons to whom the Software is 
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in 
 * all copies or substantial portions of the Software. 
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, 
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN 
 * THE SOFTWARE.
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Timers;
using System.Windows.Forms;

namespace GraficDisplay
{
    public partial class Rtl_433_Panel : Form
    {
        private string key = "device";
        private Dictionary<String, FormDevices> listformDevice;
        private Dictionary<String, String> listData;
        private System.Timers.Timer callMainTimer;
        #region constructor
        public Rtl_433_Panel()
        {
            InitializeComponent();
            listformDevice = new Dictionary<String, FormDevices>();
            listData =new Dictionary<String, String>() ;
            if (!listformDevice.ContainsKey(key))
                listformDevice.Add(key, new FormDevices(this));
            listformDevice[key].Text = key;
            listformDevice[key].setInfoDevice(listData);
            listformDevice[key].Visible = true;
            listformDevice[key].Show();
            listformDevice[key].resetLabelRecord();  //after le load for memo...
            callMainTimer = new System.Timers.Timer();
            callMainTimer.Elapsed += new ElapsedEventHandler(OnCallMainTimedEvent);
            callMainTimer.Interval = 1000;     
            callMainTimer.Enabled = false;
        }
        #endregion
        #region callBack from devices form
        public void closingOneFormDevice(String key)
        {
            lock (listformDevice)
            {
                callMainTimer.Enabled = false;
                if (listformDevice.ContainsKey(key))
                    listformDevice.Remove(key);
            }

        }
        private string nameToRecord = "";
        public void setRecordDevice(string name, bool choice)
        {
            //if (choice)
            //{
            //    if (_ClassInterfaceWithRtl433.RecordMONO == false & _ClassInterfaceWithRtl433.RecordSTEREO == false)
            //        MessageBox.Show("Choice MONO/STEREO", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    else
            //    {
            //        foreach (KeyValuePair<string, FormDevices> _form in listformDevice)
            //        {
            //            if (_form.Key != name)
            //                listformDevice[_form.Key].resetLabelRecord();
            //        }
            //        nameToRecord = name;
            //        recordDevice = choice;
            //    }
            //}
            //else
            //    recordDevice = choice;
        }
        private bool displayCurves = false;
        public void setDisplayCurves(string name, bool choice)
        {
            if (choice)
            {
                foreach (KeyValuePair<string, FormDevices> _form in listformDevice)
                {
                    if (_form.Key != name)
                        listformDevice[_form.Key].resetDisplayCurves();
                }
                nameToRecord = name;
            }
            displayCurves = choice;
        }
        #endregion
        #region private functions
        private List<PointF> CalcSinusFunction_1(int nbPoints) 
        {
            bool passe = false;
            Random aleatoire = new Random();
            //int nbPoints = aleatoire.Next(200000, 300000); 
            int idx =  aleatoire.Next(100, 1000);
            int multy=  aleatoire.Next(1, 10000);
            List<PointF> points = new List<PointF>();
            for (int i = 0; i < nbPoints; i++)
            {
                if (i > 100 && !passe)
                {
                    i += 100; 
                    points.Add(new PointF(i , 0));
                    passe = true;
                }
                else
                    points.Add( new PointF(i,(float)(((float)multy *
                                            Math.Sin(20 * (idx + 1) * (i + 1) * Math.PI / nbPoints)) *
                                            Math.Sin(40 * (idx + 1) * (i + 1) * Math.PI / nbPoints)) +
                                            (float)(((float)200 *
                                            Math.Sin(200 * (idx + 1) * (i + 1) * Math.PI / nbPoints)))));
             }
            return points;
        }
        private List<PointF> aleatoiresBinaire(int nbPoints)
        {
            Random aleatoire = new Random();
            //int nbPoints = aleatoire.Next(200000, 300000);
            List<PointF> points = new List<PointF>();
            float valueX = 0.0f;
            int i = 0;
            points.Add(new PointF(0, 0));
            try
            {
                for (i = 0; i < nbPoints; i += 1)
                {
                    if ((valueX % 100) == 0)
                    {
                        valueX += aleatoire.Next(90, 100);
                        points.Add(new PointF(valueX, 0));
                    }
                    else
                    {
                        valueX += aleatoire.Next(2, 10);
                        points.Add(new PointF(valueX, 0));
                    }
                    i += 1;
                    points.Add(new PointF(valueX, 1));
                    valueX += aleatoire.Next(2, 10);
                    i += 1;
                    points.Add(new PointF(valueX, 1));
                    i += 1;
                    points.Add(new PointF(valueX, 0));
                }
            }
            catch { }
            return points;
        }
        private List<PointF> testY()
        {
            Random aleatoire = new Random();
            int nbPoints = aleatoire.Next(2000, 3000);
            List<PointF> points = new List<PointF>();
            float valueX = 0.0f;
            float valueY = 0.0f;
            int i = 0;
            points.Add(new PointF(0, 0));
            try
            {
                for (i = 0; i < nbPoints; i += 1)
                {
                    int miniY=aleatoire.Next(-1000, 0);
                    int maxiY=aleatoire.Next(0, 1000);
                    valueY = aleatoire.Next(miniY, maxiY);
                    if ((valueX % 100) == 0)
                    {
                        valueX += aleatoire.Next(90, 100);
                        points.Add(new PointF(valueX, valueY));
                    }
                    else
                    {
                        valueX += aleatoire.Next(2, 10);
                        points.Add(new PointF(valueX, valueY));
                    }
                    i += 1;
                    miniY = aleatoire.Next(-1000, 0);
                    maxiY = aleatoire.Next(0, 1000);
                    valueY = aleatoire.Next(miniY, maxiY);
                    points.Add(new PointF(valueX, 1));
                    valueX += aleatoire.Next(2, 10);
                    i += 1;
                    miniY = aleatoire.Next(-1000, 0);
                    maxiY = aleatoire.Next(0, 1000);
                    valueY = aleatoire.Next(miniY, maxiY);
                    points.Add(new PointF(valueX, valueY));
                    i += 1;
                    miniY = aleatoire.Next(-1000, 0);
                    maxiY = aleatoire.Next(0, 1000);
                    valueY = aleatoire.Next(miniY, maxiY);
                    points.Add(new PointF(valueX, valueY));
                }
            }
            catch { }
            return points;
        }
        private List<PointF> fixes(int nbPoints)
        {
            List<PointF> points = new List<PointF>();
            try
            {
                for (int i = 0; i < nbPoints; i += 2)
                {
                    points.Add(new PointF(i, 0));
                    points.Add(new PointF(i, 1));
                    points.Add(new PointF(i + 1, 1));
                    points.Add(new PointF(i + 1, 0));
                }
            }
            catch { }
            return points;
        }
        //bool past = false;
        private List<PointF> getData(int nbGraph)
        {
            List<PointF> points;
            // points = testY();
            Random aleatoire = new Random();

            //if(!past)
            //{
            //    int nbPoints = 1000;
            //    points = aleatoiresBinaire(nbPoints);
            //    past = true;
            //}
            //else
            //{
            //    int nbPoints = 500;
            //    points = aleatoiresBinaire(nbPoints);
            // })
            if ((nbGraph % 2) == 0)
            {
                int nbPoints = 10000;  // aleatoire.Next(2000, 3000);
                points = aleatoiresBinaire(nbPoints);
            }
            else
            {
                int nbPoints = 10000;   // aleatoire.Next(2000, 3000);
                points = CalcSinusFunction_1(nbPoints);
            }
            return points;
        }
        #endregion
        #region form events
        private void buttonAddGraph_Click(object sender, EventArgs e)
        {
            //past = false;
            if (!listformDevice.ContainsKey(key))
                listformDevice.Add(key, new FormDevices(this));
            listformDevice[key].Text = key;
            listformDevice[key].setInfoDevice(listData);

            listformDevice[key].Visible = true;
            listformDevice[key].Show();
            listformDevice[key].resetLabelRecord();
            int nbGraph = listformDevice[key].getNumGraph();
            List<PointF> points=getData(nbGraph);
 
            listformDevice[key].addGraph( points, key+ " " + nbGraph);
            callMainTimer.Enabled = false;
        }
        private int nbLabel = 0;

        private void buttonAddLabel_Click(object sender, EventArgs e)
        {
            nbLabel += 1;
            for (int i = 0; i < nbLabel; i++)
                listData.Add(i.ToString(), "11111111");
            listformDevice[key].setInfoDevice(listData);
            listData.Clear();
        }
        private void OnCallMainTimedEvent(object source, ElapsedEventArgs e)
        {
            if(endCycle)
                refreshDataGraph();
        }
        private void buttonRefreshData_Click(object sender, EventArgs e)
        {
            refreshDataGraph();
        }
        bool endCycle = true;
        private void refreshDataGraph()
        {
            if(!listformDevice[key].getdataFrozen())
            {
                int nbGraph = listformDevice[key].getNumGraph();
                List<PointF>[] points = new List<PointF>[nbGraph];
                for (int i = 0; i < nbGraph; i++)
                {
                    points[i] = getData(i);
                }
                bool ret=listformDevice[key].refreshPoints(points);
                endCycle = true;
            }
        }
        private void buttonStartTimerRefreshData_Click(object sender, EventArgs e)
        {
            callMainTimer.Enabled = true;
        }
        private void buttonStopTimerRefreshData_Click(object sender, EventArgs e)
        {
            callMainTimer.Enabled = false;
        }
        private int searchZero(short[] points)
        {
            int step = 1000;
            int start = 0;
            bool find = false;
            int i = 0;
            while (find == false && i<points.Length)
            {
                for( i= start; i<points.Length-1;i+=step)
                {
                    if (points[i] == 0 && points[i+1] == 0)
                    {
                        start = i-step;
                        if (step == 1 || start < 0)
                            { 
                            find = true;
                            break;
                            }
                        step /= 10;
                        break;
                    }
                }
             }
            if (!find) return points.Length-1;
            return i;
         }
        #endregion
    }
}
