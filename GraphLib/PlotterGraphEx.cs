using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

/* Copyright (c) 2008-2014 DI Zimmermann Stephan (stefan.zimmermann@tele2.at)
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
/* spécifique version Modified by Marc Prieur (marco40_github@sfr.fr)
 *                             PlotterGraphTypes.cs 
 *    2021           spécifique version for project Rtl_433_Plugin
 *						         Plugin for SdrSharp
 */

namespace GraphLib
{
    public partial class PlotterDisplayEx : UserControl
    {
#region MEMBERS
        delegate void InvokeVoidFuncDelegate();
        #endregion
        #region CONSTRUCTOR
        public PlotterDisplayEx()
        {
            InitializeComponent();
            hScrollBarStartX.Maximum = 0;
            //Console.WriteLine(Application.ProductVersion);
            // mTimer = new PrecisionTimer.Timer();
            // mTimer.Period = 50;                         // 20 fps
            // mTimer.Tick += new EventHandler(OnTimerTick);
            // play_speed = 0.5f; // 20x10 = 200 values per second == sample frequency     
            //// mTimer.Start();
            // isRunning = false;
        }
        private void contextMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            String text = e.ClickedItem.Text;
            foreach (DataSource s in gPane.Sources)
            {
                if (s.Name == text)
                {
                    s.Active ^= true;
                    gPane.Invalidate();
                    break;
                }
            }
        }
#endregion
#region PROPERTIES
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Editor(typeof(System.ComponentModel.Design.CollectionEditor),
               typeof(System.Drawing.Design.UITypeEditor))]
        public List<DataSource> DataSources
        {
            get { return gPane.Sources; }
        }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Editor(typeof(System.ComponentModel.Design.CollectionEditor),
               typeof(System.Drawing.Design.UITypeEditor))]
        public PlotterGraphPaneEx.LayoutMode PanelLayout
        {
            get { return gPane.layout; }
            set { gPane.layout = value; }
        }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Editor(typeof(System.ComponentModel.Design.CollectionEditor),
               typeof(System.Drawing.Design.UITypeEditor))]
        public SmoothingMode Smoothing
        {
            get { return gPane.smoothing; }
            set { gPane.smoothing = value; }
        }
        [Category("Properties")]
        [DefaultValue(typeof(Color), "")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color BackgroundColorTop
        {
            get { return gPane.BgndColorTop; }
            set { gPane.BgndColorTop = value; }
        }

        [Category("Properties")]
        [DefaultValue(typeof(Color), "")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color BackgroundColorBot
        {
            get { return gPane.BgndColorBot; }
            set { gPane.BgndColorBot = value; }
        }
        [Category("Properties")]
        [DefaultValue(typeof(Color), "")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color DashedGridColor
        {
            get { return gPane.MinorGridColor; }
            set { gPane.MinorGridColor = value; }
        }
        [Category("Properties")]
        [DefaultValue(typeof(Color), "")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color SolidGridColor
        {
            get { return gPane.MajorGridColor; }
            set { gPane.MajorGridColor = value; }
        }
        public Boolean DoubleBuffering
        {
            get { return gPane.useDoubleBuffer; }
            set { gPane.useDoubleBuffer = value; }
        }
        #endregion
        #region PUBLIC METHODS
        public void SetDisplayRangeX(float x_startDisplayed, float x_endDisplayed)
        {
            gPane.XStartDisplayed = x_startDisplayed;
            gPane.XEndDisplayed = x_endDisplayed;
            gPane.CurStartDisplayed = gPane.XStartDisplayed;
            gPane.CurEndDisplayed = gPane.XEndDisplayed;
            gPane.starting_idx = x_startDisplayed;
            memoXEndDisplayed = x_endDisplayed;
        }
        public void SetGridDistanceX(float grid_dist_x_samples)
        {
            gPane.grid_distance_x = grid_dist_x_samples;
        }
        private float MaxXAllData = 0;
        public void SetMaxXAllData(float MaxXAllData,Boolean refresh)
        {
            if (base.InvokeRequired)
            {
                base.BeginInvoke((Action)delegate
                {
                    SetMaxXAllData(MaxXAllData, refresh);
                });
            }
            else
            {
                if(this.MaxXAllData==0)
                    memoXEndDisplayed = MaxXAllData;
                this.MaxXAllData = MaxXAllData;
                if (memoXEndDisplayed > MaxXAllData)   
                {
                   memoZoom=1;
                   hScrollBarStartX.Value = 0;
                   hScrollBarStartX.Maximum = 0;
                   memoXEndDisplayed = MaxXAllData;
                }
                if (refresh)
                     hScrollBar1_ValueChanged();
            }
        }
        public void SetMarkerXPixels(Int32 nbMarkerXPixels)
        {
            gPane.nbMarkerXPixels = nbMarkerXPixels;
        }
        public void SetGridOriginX(float off_x)
        {
            gPane.grid_off_x = off_x;
        }
        public void refreshPoints()
        {
            if (base.InvokeRequired)
            {
                base.BeginInvoke((Action)delegate
                {
                    refreshPoints();
                });
            }
            else
            {
                Refresh();
            }
        }
        public Boolean getEndDrawGraphEvent()
        {
            return gPane.EndDrawGraphEvent;
        }
#endregion
#region PRIVATE METHODS
        private void UpdateControl()
        {
            try
            {
                Boolean AllAutoscaled = true;

                foreach (DataSource s in gPane.Sources)
                {
                    AllAutoscaled &= s.AutoScaleX;
                }
            }
            catch
            {
            }
        }
        private float memoScrollValue = 0;
        private void hScrollBar1_ValueChanged(object sender, EventArgs e)
        {
            hScrollBar1_ValueChanged();
        }
        private void hScrollBar1_ValueChanged()
        {
            if (gPane.Sources.Count > 0)
            {
                float XEndDisplayed = memoXEndDisplayed - memoScrollValue + hScrollBarStartX.Value;
                UpdateRowSource(XEndDisplayed);
                memoScrollValue = hScrollBarStartX.Value;
            }
        }
        private float memoZoom = 1;
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            HandledMouseEventArgs h = (HandledMouseEventArgs)e;
            h.Handled = true; // pour empêcher le traitement par défaut
            float zoom = 0;
            if (e.Delta < 0)
            {
               if((memoXEndDisplayed - hScrollBarStartX.Value) <MaxXAllData)
                {
                    zoom = 2;
                    memoZoom *= 2;                //dezoom
                }
                else
                    return;
            }
            else
            {
               // if (gPane.XEndDisplayed > 10)         // zoom  10 for X
                if(gPane.endZoom<gPane.Sources.Count)
                {
                    zoom = 0.5f;
                    memoZoom /= 2;
                }
                else
                    return;
            }
            float XEndDisplayed = hScrollBarStartX.Value + (gPane.XEndDisplayed - hScrollBarStartX.Value) * zoom;
            if (XEndDisplayed > MaxXAllData)
                XEndDisplayed = MaxXAllData;   // -(XEndDisplayed-MaxXAllData);
            memoXEndDisplayed = XEndDisplayed;
            UpdateRowSource(XEndDisplayed);
         }
        private float memoXEndDisplayed = 0;
        private void UpdateRowSource(float XEndDisplayed)
        {
            float XStartDisplayed = hScrollBarStartX.Value;
            if (gPane.grid_distance_x > 0)
                SetGridDistanceX((XEndDisplayed - XStartDisplayed) / 5.0f);
            SetDisplayRangeX(XStartDisplayed, XEndDisplayed);
            // +9.0f stop before max?? with largeChange=10:992 for 1001 ,ok with largeChange=1
            hScrollBarStartX.Maximum = (Int32)Math.Ceiling(MaxXAllData - (XEndDisplayed - XStartDisplayed));
            gPane.Refresh(); 
        }
#endregion
    }
}
