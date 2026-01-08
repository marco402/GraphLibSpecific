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
using System.Windows.Forms;
using System.ComponentModel;
using System.Linq;
using GraphLib;
using System.Globalization;
namespace GraficDisplay
{
    public partial class FormDevices : Form
    {
        #region declare
        private int NumGraphs = 0;
        private float _nbPointX = 0;
        private Rtl_433_Panel _classParent;
        private Dictionary<String, Label> listLabelKey;
        private Dictionary<String, Label> listLabelValue;
        private long memoTimeMax;
        private long memoTime;
        private long nbMessages;
        private Color memoBackColortoolStripSplitLabelRecordOneShoot;
        private Color memoForeColortoolStripSplitLabelRecordOneShoot;
        private string memoTexttoolStripSplitLabelRecordOneShoot;
        private Color memoBackColortoolStripStatusLabelDisplayCurves;
        private Color memoForeColortoolStripStatusLabelDisplayCurves;
        private string memoTexttoolStripSplitLabelDisplayCurves;
        private Color memoBackColortoolStripStatusLabelFreezeData;
        private Color memoForeColortoolStripStatusLabelFreezeData;
        private string memoTexttoolStripSplitLabelFreezeData;
        private float memoHeightPlotterDisplayExDevices = 0;
        #endregion
        #region constructor load close
        public FormDevices(Rtl_433_Panel classParent)
        {
           InitializeComponent();
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            nfi.NumberDecimalDigits = 3;
            _classParent = classParent;
            listLabelKey = new Dictionary<String, Label>();
            listLabelValue = new Dictionary<String, Label>();
            this.SuspendLayout();
            this.tableLayoutPanelDeviceData.Name = "tableLayoutPanelDeviceData";
            this.tableLayoutPanelDeviceData.TabIndex = 1;
            toolStripStatusLabelPeriodeCurrent.Text = "Period: 0";
            toolStripStatusLabelPeriodeMax.Text = "Period max: 0";
            toolStripStatusLabelNbMessages.Text = "NB messages: 0";
            toolStripSplitLabelRecordOneShoot.ToolTipText = "Record data buffer Mono Stereo or both(checkbox on panel) to directory Recordings if exist else in SdrSharp.exe directory";
            this.ResumeLayout();
        }
        private void FormDevices_Load(object sender, EventArgs e)
        {
            memoBackColortoolStripSplitLabelRecordOneShoot = toolStripSplitLabelRecordOneShoot.BackColor;
            memoForeColortoolStripSplitLabelRecordOneShoot = toolStripSplitLabelRecordOneShoot.ForeColor;
            memoTexttoolStripSplitLabelRecordOneShoot = toolStripSplitLabelRecordOneShoot.Text;
            memoBackColortoolStripStatusLabelFreezeData = toolStripStatusLabelFreezeData.BackColor;
            memoForeColortoolStripStatusLabelFreezeData = toolStripStatusLabelFreezeData.ForeColor;
            memoTexttoolStripSplitLabelFreezeData = toolStripStatusLabelFreezeData.Text;
            memoBackColortoolStripStatusLabelDisplayCurves = toolStripStatusLabelDisplayCurves.BackColor;
            memoForeColortoolStripStatusLabelDisplayCurves = toolStripStatusLabelDisplayCurves.ForeColor;
            memoTexttoolStripSplitLabelDisplayCurves = toolStripStatusLabelDisplayCurves.Text;
            memoHeightPlotterDisplayExDevices = plotterDisplayExDevices.Height;
        }
        private void FormDevices_FormClosing(object sender, FormClosingEventArgs e)
        {
            _classParent.closingOneFormDevice(this.Text);
        }
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            plotterDisplayExDevices.Dispose();
            base.OnClosing(e);
        }
        #endregion
        #region form events
        private void toolStripSplitLabelRecordOneShoot_Click(object sender, EventArgs e)
        {
            if(toolStripSplitLabelRecordOneShoot.BackColor != memoBackColortoolStripSplitLabelRecordOneShoot)
            {
             _classParent.setRecordDevice(this.Text,false);
                resetLabelRecord();
            }
            else
            { 
            _classParent.setRecordDevice(this.Text,true);
            toolStripSplitLabelRecordOneShoot.BackColor = Color.Green;
            toolStripSplitLabelRecordOneShoot.ForeColor = Color.White;
            toolStripSplitLabelRecordOneShoot.Text = "Cancel record";
            }
        }
        private void toolStripStatusLabelDisplayCurves_Click(object sender, EventArgs e)
        {
            if (toolStripStatusLabelDisplayCurves.BackColor == memoBackColortoolStripStatusLabelDisplayCurves)
            {
                toolStripStatusLabelDisplayCurves.BackColor = Color.Green;
                toolStripStatusLabelDisplayCurves.ForeColor = Color.White;
                toolStripStatusLabelDisplayCurves.Text = "display curves";
                hideShowAllGraphs(false);
            }
            else
            {
                toolStripStatusLabelDisplayCurves.BackColor = memoBackColortoolStripStatusLabelDisplayCurves;
                toolStripStatusLabelDisplayCurves.ForeColor = memoForeColortoolStripStatusLabelDisplayCurves;
                toolStripStatusLabelDisplayCurves.Text = "Cancel display curves";
                hideShowAllGraphs(true);
            }
        }
        private bool _dataFrozen = false;
        private void toolStripStatusLabelFreezeData_Click(object sender, EventArgs e)
        {
            if (toolStripStatusLabelFreezeData.BackColor == memoBackColortoolStripStatusLabelFreezeData)
            {
                toolStripStatusLabelFreezeData.BackColor = Color.Green;
                toolStripStatusLabelFreezeData.ForeColor = Color.White;
                toolStripStatusLabelFreezeData.Text = "Refresh data";
                _dataFrozen = true;
            }
            else
            {
                toolStripStatusLabelFreezeData.BackColor = memoBackColortoolStripStatusLabelFreezeData;
                toolStripStatusLabelFreezeData.ForeColor = memoForeColortoolStripStatusLabelFreezeData;
                toolStripStatusLabelFreezeData.Text = memoTexttoolStripSplitLabelFreezeData;
                _dataFrozen = false;
            }
        }
        #endregion
        #region publics functions
        public void setInfoDevice(Dictionary<String, String> listData)
        {
            this.SuspendLayout();
            if (NumGraphs==0)
                tableLayoutPanelDeviceData.RowStyles[0].Height = 50;
            foreach (KeyValuePair<string, string> _data in listData)
            {
                if (_data.Key.ToUpper().Contains("TIME"))
                {
                    DateTime maDate;
                    if (DateTime.TryParse(_data.Value, out maDate))
                    {
                        long currentTime = maDate.ToFileTime() / 10000000;
                        if (memoTime > 0)
                        {
                            long dtTime = (currentTime - memoTime);
                            if (dtTime > memoTimeMax)
                                memoTimeMax = dtTime;
                            toolStripStatusLabelPeriodeCurrent.Text = "Period: " + dtTime.ToString() + "s.";
                            toolStripStatusLabelPeriodeMax.Text = "Period max: " + memoTimeMax.ToString() + "s.";
                        }
                        memoTime = currentTime;
                    }
                }
                if (!listLabelKey.ContainsKey(_data.Key))
                {
                    tableLayoutPanelDeviceData.RowCount += 1;
                    tableLayoutPanelDeviceData.RowStyles.Add(new System.Windows.Forms.RowStyle());
                    tableLayoutPanelDeviceData.RowStyles[tableLayoutPanelDeviceData.RowCount-1 ].Height = 20;
                    tableLayoutPanelDeviceData.Padding = new Padding(0, 3, 0, 0);
                    Label theLabelKey = new Label();
                    listLabelKey.Add(_data.Key, theLabelKey);
                    theLabelKey.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Top;
                    theLabelKey.AutoSize = true;
                    theLabelKey.BackColor = System.Drawing.SystemColors.ControlDarkDark;
                    theLabelKey.ForeColor = System.Drawing.SystemColors.ButtonFace;

                    tableLayoutPanelDeviceData.Controls.Add(theLabelKey, 0, tableLayoutPanelDeviceData.RowCount - 1);
                    theLabelKey.Text = _data.Key;
                    Label theLabelValue = new Label();
                    listLabelValue.Add(_data.Key, theLabelValue);
                    theLabelValue.Tag = tableLayoutPanelDeviceData.RowCount;
                    theLabelValue.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Top;
                    theLabelValue.AutoSize = true;
                    theLabelValue.BackColor = System.Drawing.SystemColors.ControlDarkDark;
                    theLabelValue.ForeColor = System.Drawing.SystemColors.ButtonFace;
                    tableLayoutPanelDeviceData.Controls.Add(theLabelValue, 1, tableLayoutPanelDeviceData.RowCount - 1);
                }
                listLabelValue[_data.Key].Text = _data.Value;
            }
            nbMessages += 1;
            toolStripStatusLabelNbMessages.Text = "NB messages: " + nbMessages.ToString();
            this.ResumeLayout();
        }
        public void resetLabelRecord()
        {
            toolStripSplitLabelRecordOneShoot.BackColor = memoBackColortoolStripSplitLabelRecordOneShoot;
            toolStripSplitLabelRecordOneShoot.ForeColor = memoForeColortoolStripSplitLabelRecordOneShoot;
            toolStripSplitLabelRecordOneShoot.Text = memoTexttoolStripSplitLabelRecordOneShoot;
        }
        public void resetDisplayCurves()
        {
            hideShowAllGraphs(false);
        }
        public void hideShowAllGraphs(bool etat)
        {
            if (!etat)
            {
                memoHeightPlotterDisplayExDevices = tableLayoutPanelDeviceData.RowStyles[0].Height;
                tableLayoutPanelDeviceData.RowStyles[0].Height = 0;
                plotterDisplayExDevices.Visible = etat;
             }
            else
            {
                plotterDisplayExDevices.Visible = etat;
                tableLayoutPanelDeviceData.RowStyles[0].Height = memoHeightPlotterDisplayExDevices;
            }
            plotterDisplayExDevices.Visible = etat;
        }
        private float MaxXAllData = float.MinValue;
        public void addGraph(List <PointF>  tabPoints, String name )
        {
            _nbPointX = tabPoints.Count;
            if (_nbPointX > 0)
                MaxXAllData = Math.Max(MaxXAllData, tabPoints.Max(point => point.X));
            if (NumGraphs == 0)
                {
                plotterDisplayExDevices.Smoothing = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;
                plotterDisplayExDevices.DataSources.Clear();
                plotterDisplayExDevices.PanelLayout = PlotterGraphPaneEx.LayoutMode.VerticalArranged;  //only this if #OPTION to library
            }
            plotterDisplayExDevices.DataSources.Add(new DataSource());
            plotterDisplayExDevices.DataSources[NumGraphs].Name = name;
            plotterDisplayExDevices.DataSources[NumGraphs].OnRenderXAxisLabel += RenderXLabel;
            plotterDisplayExDevices.DataSources[NumGraphs].Length = (int)_nbPointX;
            plotterDisplayExDevices.DataSources[NumGraphs].AutoScaleY = false;
            float miniY = 0;
            float maxiY = 0;
            if (_nbPointX > 0)
            {
                    miniY = tabPoints.Min(point => point.Y);
                    maxiY = tabPoints.Max(point => point.Y);
            }
            plotterDisplayExDevices.DataSources[NumGraphs].SetDisplayRangeY(miniY ,maxiY );    //(-250, 250);
            plotterDisplayExDevices.DataSources[NumGraphs].SetGridDistanceY((maxiY-miniY)/5.0f);
            plotterDisplayExDevices.DataSources[NumGraphs].Samples = tabPoints;
            plotterDisplayExDevices.DataSources[NumGraphs].OnRenderYAxisLabel = RenderYLabel;
            plotterDisplayExDevices.Height = 100 + ((NumGraphs + 1) * 90);
            tableLayoutPanelDeviceData.RowStyles[0].Height =100.0f+ (NumGraphs + 1) * 90.0f;
            if (NumGraphs==0)
                plotterDisplayExDevices.SetDisplayRangeX(0, MaxXAllData);
            plotterDisplayExDevices.SetGridDistanceX(0);
            plotterDisplayExDevices.SetMarkerXPixels(100);
            ApplyColorSchema(NumGraphs);
            plotterDisplayExDevices.SetMaxXAllData(MaxXAllData,true);  //last line for refresh
            NumGraphs +=1;
        }
        public bool getdataFrozen()
        {
            return _dataFrozen;
        }
        public int getNumGraph()
        {
            return NumGraphs;
        }
        public bool refreshPoints(List<PointF>[] tabPoints)
        {
            if (plotterDisplayExDevices.GetEndDrawGraphEvent())
            {
                float newMaxXAllData = float.MinValue;
                foreach (List<PointF> lpt in tabPoints)
                {
                    if (lpt.Count > 0)
                        newMaxXAllData = Math.Max(newMaxXAllData, lpt.Max(point => point.X));
                }
                for (int indiceGraph = 0; indiceGraph < tabPoints.Length; indiceGraph++)
                {
                    int nPointsX = tabPoints[indiceGraph].Count;
                    float miniY = 0;
                    float maxiY = 0;
                    if (tabPoints[indiceGraph].Count > 0)
                    {
                        miniY = tabPoints[indiceGraph].Min(point => point.Y);
                        maxiY = tabPoints[indiceGraph].Max(point => point.Y);
                    }
                    plotterDisplayExDevices.DataSources[indiceGraph].SetDisplayRangeY(miniY, maxiY);    //(-250, 250);
                    plotterDisplayExDevices.DataSources[indiceGraph].SetGridDistanceY((maxiY - miniY) / 5.0f);
                    //if no comment this next line new display with zoom=1 and start=0-> no retested 
                    //plotterDisplayExDevices.SetDisplayRangeX(0, newMaxXAllData);
                    plotterDisplayExDevices.DataSources[indiceGraph].Length = (int)nPointsX;
                    plotterDisplayExDevices.DataSources[indiceGraph].Samples = tabPoints[indiceGraph];
                    if (indiceGraph < tabPoints.Length - 1)
                        plotterDisplayExDevices.SetMaxXAllData(newMaxXAllData, false);
                    else
                        plotterDisplayExDevices.SetMaxXAllData(newMaxXAllData, true);  //last line for refresh
                }
            }
            else
                Console.WriteLine("attente");
            return true;
        }
        #endregion
        #region privates functions
        private String RenderXLabel(int value)
        {
            if (value > 1000000)
                return string.Format("{0}s", (((float)value / 1000000f)).ToString("N1"));
            else if (value > 1000)
                return string.Format("{0}ms", (((float)value / 1000f)).ToString("N1"));
            else
                return string.Format("{0}µs", value.ToString());
        }
        private String RenderYLabel(DataSource s, float value)
        {
            return String.Format("{0:0.0}", value);
        }
        //see other theme to sources:
        //https://www.codeproject.com/articles/32836/a-simple-c-library-for-graph-plotting?
        private void ApplyColorSchema(int j)  //
        {
            Color[] cols = { Color.FromArgb(255,0,0),
                                Color.FromArgb(0,255,0),
                                Color.FromArgb(255,255,0),
                                Color.FromArgb(64,64,255),
                                Color.FromArgb(0,255,255) ,
                                Color.FromArgb(255,0,255),
                                Color.FromArgb(255,128,0) };
            plotterDisplayExDevices.DataSources[j].GraphColor = cols[j % 7];
            plotterDisplayExDevices.BackgroundColorTop = Color.Black;
            plotterDisplayExDevices.BackgroundColorBot = Color.Black;
            plotterDisplayExDevices.SolidGridColor = Color.DarkGray;
            plotterDisplayExDevices.DashedGridColor = Color.DarkGray;
        }
        #endregion
    }
}
