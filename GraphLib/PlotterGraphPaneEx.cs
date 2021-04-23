using System;
using System.Collections.Generic;
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
 *          2021      spécifique version for project Rtl_433_Plugin
 *						         Plugin for SdrSharp
 */

namespace GraphLib
{
    public partial class PlotterGraphPaneEx : UserControl
    {
#region MEMBERS PUBLIC
        public enum LayoutMode
        {
            //NORMAL,
            //STACKED,
            VERTICAL_ARRANGED,
            //TILES_VER,
            //TILES_HOR,
        }
        public LayoutMode layout = LayoutMode.VERTICAL_ARRANGED;
        public Color MajorGridColor = Color.DarkGray;
        public Color MinorGridColor = Color.DarkGray;
        public Color BgndColorTop = Color.White;
        public Color BgndColorBot = Color.White;
        public bool useDoubleBuffer = false;
        public SmoothingMode smoothing = SmoothingMode.None;
        public float starting_idx = 0;
        public float XStartDisplayed = -50;
        public float XEndDisplayed = 100;
        public float CurStartDisplayed = 0;
        public float CurEndDisplayed = 0;
        public float grid_distance_x = 200;       // grid distance in samples ( draw a vertical line every 200 samples )
        public float nbMarkerXPixels = 100;
        public float grid_off_x = 0;
        public bool EndDrawGraphEvent = true;
#endregion
#region MEMBERS PRIVATE
        private float yLabelAreaWidth = 40;       // y-label area width
        private float xLabelAreaheight = 8;        // x-label padding ( bottom area left and right were x labels are still visible )
        private float[] MinorGridPattern = new float[] { 2, 4 };
        private float[] MajorGridPattern = new float[] { 2, 2 };
        private float pad_left = 10;         // left padding
        private float pad_right = 10;        // right padding
        private float pad_top = 10;          // top
        private float pad_bot = 10;          // bottom padding
        private float GraphCaptionLineHeight = 28;
        private float pad_inter = 4;         // padding between graphs
        private float DX = 0;
        private float off_X = 0;
        private bool hasBoundingBox = true;
        private Font legendFont = new Font(FontFamily.GenericSansSerif, 8.25f);
        private Color LabelColor = Color.White;
        private Color GraphBoxColor = Color.White;
        private Color GraphColor = Color.DarkGreen;
        private BackBuffer memGraphics;        
        private int ActiveSources = 0;
        private PointF graphCaptionOffset = new PointF(12, 2);
#endregion
#region CONSTRUCTOR
        public PlotterGraphPaneEx()
        {
            memGraphics = new BackBuffer();
            InitializeComponent();
            this.Resize += new System.EventHandler(this.OnResizeForm);
        }
#endregion
#region PUBLIC METHODS
        public List<DataSource> Sources { get; } = new List<DataSource>();
        public void PaintGraphs(Graphics CurGraphics, float CurWidth, float CurHeight, float OFFX, float OFFY)
        {
            endZoom = 0;
            int CurGraphIdx = 0;
            float curOffY = 0;
            float CurOffX = 0;
            List<float> offsetX = new List<float>();
            List<float> offsetY = new List<float>();
            foreach (DataSource source in Sources)
            {
                off_X = 0;
                source.Cur_YD0 = source.YD0;
                source.Cur_YD1 = source.YD1;
                source.CurGraphHeight = CurHeight;
                source.CurGraphWidth = CurWidth - pad_left - yLabelAreaWidth - pad_right;

                if (source.yFlip)
                {
                    DX = XEndDisplayed - XStartDisplayed;
                }
                else
                {
                    DX = XEndDisplayed - XStartDisplayed;
                }

                CurStartDisplayed = XStartDisplayed;
                CurEndDisplayed = XEndDisplayed;

                if (source.AutoScaleX && source.Samples.Count > 0)
                {
                    CurStartDisplayed = source.XMin - source.XAutoScaleOffset;
                    CurEndDisplayed = source.XMax + source.XAutoScaleOffset;
                    DX = CurEndDisplayed - CurStartDisplayed;
                }

                if (source.Active)
                {
                    int DownSample = source.Downsampling;
                    //List <PointF> data = source.Samples;
                    if (source.AutoScaleY == true)
                    {
                        int idx_start = -1;
                        int idx_stop = -1;
                        float ymin = 0.0f;
                        float ymax = 0.0f;
                        float ymin_range = 0;
                        float ymax_range = 0;

                        
                        float mult_y = source.CurGraphHeight / source.DY;
                        float mult_x = (source.CurGraphWidth) / DX;
                        float coff_x = off_X - starting_idx * mult_x;

                        if (source.AutoScaleX)
                        {
                            coff_x = off_X ;
                        }

                        for (int i = 0; i < source.Samples.Count - 1; i += DownSample)
                        {
                            float x = source.Samples[i].X * mult_x + coff_x;

                            if (source.Samples[i].Y > ymax) ymax = source.Samples[i].Y;
                            if (source.Samples[i].Y < ymin) ymin = source.Samples[i].Y;

                            if (x >= 0 && x <= (source.CurGraphWidth))
                            {
                                if (idx_start == -1) idx_start = i;
                                idx_stop = i;
                                if (source.Samples[i].Y > ymax_range) ymax_range = source.Samples[i].Y;
                                if (source.Samples[i].Y < ymin_range) ymin_range = source.Samples[i].Y;
                            }
                            if( x > source.CurGraphWidth)
                                break;
                        }
                        
                        if (idx_start >= 0 && idx_stop >= 0)
                        {
                            float data_range = ymax - ymin;              // this is range in the data
                            float delta_range = ymax_range - ymin_range; // this is the visible data range -> might be smaller

                            source.Cur_YD0 = ymin_range;
                            source.Cur_YD1 = ymax_range;
                        }
                    }
                    if (ActiveSources > 1)
                    {
                        source.CurGraphHeight = (float)(CurHeight - pad_top - pad_bot) / ActiveSources - GraphCaptionLineHeight;
                        float Diff = ((ActiveSources - 1) * pad_inter) / ActiveSources;
                        source.CurGraphHeight -= Diff;
                    }
                    else
                    {
                        source.CurGraphHeight = (float)(CurHeight - pad_top - pad_bot) - GraphCaptionLineHeight;
                    }

                    if (source.yFlip)
                    {
                        source.DY = source.Cur_YD0 - source.Cur_YD1;

                        if (DX != 0 && source.DY != 0)
                        {
                            source.off_Y = -source.Cur_YD1 * source.CurGraphHeight / source.DY;
                            off_X = -CurStartDisplayed * source.CurGraphWidth / DX;
                        }
                    }
                    else
                    {
                        source.DY = source.Cur_YD1 - source.Cur_YD0;

                        if (DX != 0 && source.DY != 0)
                        {
                            source.off_Y = -source.Cur_YD0 * source.CurGraphHeight / source.DY;
                            off_X = -CurStartDisplayed * source.CurGraphWidth / DX;
                        }
                    }
                    if (ActiveSources > 1)
                    {
                        curOffY = OFFY + pad_top + CurGraphIdx * (source.CurGraphHeight + GraphCaptionLineHeight + pad_inter);
                    }
                    else
                    {
                        curOffY = OFFY + pad_top + CurGraphIdx * (source.CurGraphHeight + GraphCaptionLineHeight);
                    }

                    CurOffX = OFFX + pad_left + yLabelAreaWidth;
                    if (hasBoundingBox)
                    {
                        float w = source.CurGraphWidth;
                        float h = source.CurGraphHeight + GraphCaptionLineHeight / 2;

                        DrawGraphBox(CurGraphics, CurOffX, curOffY, w, h);

                    }
                    
                    List<int> marker_pos = DrawGraphCurve(CurGraphics, source, CurOffX, curOffY + GraphCaptionLineHeight / 2);
                    offsetX.Add(CurOffX);
                    offsetY.Add(curOffY);
                    DrawGraphCaption(CurGraphics, source, marker_pos, CurOffX, curOffY);
                    
                    DrawYLabels(CurGraphics, source,  CurOffX, curOffY);
                    CurGraphIdx++;
                    marker_pos.Clear();
                    marker_pos = null;
                }
            }
            //calculate _marker_positions for display all marker no synchro to data
            List<int> marker_positionsForAutoScaleX = new List<int>();
            double step = (XEndDisplayed - starting_idx) / ((float)CurWidth / nbMarkerXPixels);
            for (double m = starting_idx; m < (XEndDisplayed - (step/2.0)); m += step)
            {
                marker_positionsForAutoScaleX.Add((int)m);
            }
            marker_positionsForAutoScaleX.Add((int)XEndDisplayed);
            int indice = 0;
            foreach (DataSource source in Sources)
            {
                if (!source.AutoScaleX)
                {
                    DrawXLabelsForAutoScaleX(CurGraphics, source, marker_positionsForAutoScaleX, offsetX[indice], offsetY[indice]);
                }
                indice += 1;

            }
            marker_positionsForAutoScaleX.Clear();
            marker_positionsForAutoScaleX = null;
            offsetX.Clear();
            offsetX = null;
            offsetY.Clear();
            offsetY = null;
        }
        public void PaintControl(Graphics CurGraphics, float CurWidth, float CurHeight, float OffX, float OffY, bool PaintBgnd)
        {
            if (PaintBgnd)
            {
                DrawBackground(CurGraphics,CurWidth,CurHeight,OffX,OffY);
            }

            ActiveSources = 0;

            foreach (DataSource source in Sources)
            {
                if (source.Samples != null &&
                    source.Samples.Count > 0 &&
                    source.Active == true)
                {
                    ActiveSources++;
                }
            }

            //switch (layout)
            //{
            //    case LayoutMode.NORMAL:

            //    case LayoutMode.TILES_HOR:
            //    case LayoutMode.TILES_VER:
            //    case LayoutMode.VERTICAL_ARRANGED:

                    PaintGraphs(CurGraphics, CurWidth, CurHeight, OffX, OffY);
                
            //        break;

            //    case LayoutMode.STACKED:

            //        PaintStackedGraphs(CurGraphics, CurWidth, CurHeight, OffX, OffY);
                 
            //        break;
            //}
        }
#endregion
#region PRIVATE METHODS
        private void DrawBackground(Graphics g,float CurWidth, float CurHeight, float CurOFFX, float CurOFFY)
        {
            Rectangle rbgn = new Rectangle((int)CurOFFX, (int)CurOFFY, (int)CurWidth, (int)CurHeight);

            if (BgndColorTop != BgndColorBot)
            {
                using (LinearGradientBrush lb1 = new LinearGradientBrush(new Point((int)0, (int)0),
                                                                         new Point((int)0, (int)(CurHeight)),
                                                                         BgndColorTop,
                                                                         BgndColorBot))
                {
                    g.FillRectangle(lb1, rbgn);
                }
            }
            else
            {
                using (SolidBrush sb1 = new SolidBrush(BgndColorTop))
                {
                    g.FillRectangle(sb1, rbgn);
                }
            }
        }
        public int endZoom = 0;
        private List<int> DrawGraphCurve( Graphics g, DataSource source,  float offset_x, float offset_y )
        {
            List<int> marker_positions = new List<int>();
            if (DX != 0 && source.DY != 0)
            {
                if (source.Samples != null && source.Samples.Count > 1)
                {
                    List<Point> ps = new List<Point>();
                    int DownSample = source.Downsampling;
                    float mult_y = source.CurGraphHeight / source.DY;
                    float mult_x = source.CurGraphWidth / DX;
                    off_X = 0;
                    float coff_x = off_X - starting_idx * mult_x;
                    if (source.AutoScaleX)
                    {
                        coff_x = off_X;     // avoid dragging in x-autoscale mode
                    }
                    bool firstPoint = true;
                    //for gain time
                    float endGraph = source.CurGraphWidth + xLabelAreaheight;
                    float CurGraphWidthCor = source.CurGraphWidth - 1.0f;
                    float offset_xCor = offset_x + 1.0f;
                    float offset_xCorM = offset_x - 1.0f;
                    float offset_yCor = offset_y + 0.0f;
                    for (int i = 0; i < source.Samples.Count - 1; i += DownSample)
                    {
                        float x = source.Samples[i].X  * mult_x   + coff_x;
                        float y = source.Samples[i].Y  * mult_y + source.off_Y;
                        if (source.AutoScaleX)
                        {
                            if ((int)(source.Samples[i].X) % grid_distance_x == 0)
                            {
                                if (x >= xLabelAreaheight && x < endGraph)
                                {
                                    marker_positions.Add(i);
                                }
                            }
                        }
                        if (x >= 0 && x < CurGraphWidthCor)  //
                        {   
                            if (firstPoint)
                            {
                                float y1 = source.Samples[i].Y * mult_y + source.off_Y;
                                ps.Add(new Point((int)offset_xCor, (int)(y1 + offset_yCor))); //
                                firstPoint = false;
                            }
                            ps.Add(new Point((int)(x + offset_xCor), (int)(y  + offset_yCor))); //                           
                        }
                        else if (x >= CurGraphWidthCor)  //
                        {
                            if(i>0)
                            {
                            y = source.Samples[i-1].Y * mult_y + source.off_Y;
                            ps.Add(new Point((int)(source.CurGraphWidth + offset_xCorM), (int)(y + offset_yCor))); //  
                            }
                            break;
                        }
                    }
  
                    using (Pen p = new Pen(source.GraphColor))
                    {
                        if (ps.Count > 2)
                        {
                            if (ps.Count < (source.CurGraphWidth / 20))
                            {
                                endZoom +=1;
                                int axeY = (int)(offset_y + source.CurGraphHeight / 2);   // (int)(((source.Cur_YD1 - source.Cur_YD0) / 2) + offset_yCor);
                                using (Brush brush = new SolidBrush(Color.Cyan))
                                {
                                    System.Drawing.StringFormat drawFormat = new System.Drawing.StringFormat();
                                    drawFormat.FormatFlags = StringFormatFlags.DirectionVertical;
                                    float memoPsX = (ps[2].X / mult_x + coff_x - offset_x);
                                    for (int i = 2; i < ps.Count - 2; i++)  //reject first and last no good value
                                    {
                                        float currentX = (ps[i + 1].X / mult_x + coff_x - offset_x);
                                        int deltaX = (int)(currentX - memoPsX);
                                        memoPsX = currentX;
                                        if (deltaX > 0)
                                        {
                                            string value = "";
                                            if (source.OnRenderXAxisLabel != null)
                                                value = source.OnRenderXAxisLabel(deltaX);
                                            g.DrawString(value, legendFont, brush, new PointF(ps[i].X + 1, axeY), drawFormat);
                                        }
                                    }
                                }
                            }
 
                            g.DrawLines(p, ps.ToArray());
                        }
                        else
                            endZoom +=1;
                     }
                    ps.Clear();
                    ps = null;
                }
            }
            
            return marker_positions;  //if AutoScaleX:marker_positions else marker_positionsForAutoScaleX
        }
        private void DrawGraphCaption(Graphics g, DataSource source, List<int> marker_pos, float offset_x, float offset_y)
        {
            using (Brush brush = new SolidBrush(source.GraphColor))
            {
                using (Pen pen = new Pen(brush))
                {
                    pen.DashPattern = MajorGridPattern;

                    g.DrawString(source.Name, legendFont, brush, new PointF(offset_x + graphCaptionOffset.X + 12, offset_y +graphCaptionOffset.Y-1));  //+ 2

                }
            }
        }
        private void DrawXLabelsForAutoScaleX(Graphics g, DataSource source, List<int> marker_data, float offset_x, float offset_y)
        {
            Color XLabColor = source.GraphColor;
            using (Brush brush = new SolidBrush(Color.Cyan))
            {
                using (Pen pen = new Pen(brush))
                {
                    pen.DashPattern = MajorGridPattern;

                    if (DX != 0 && source.DY != 0)
                    {
                        if (source.Samples != null && source.Samples.Count > 1)
                        {
                            float mult_y = source.CurGraphHeight / source.DY;
                            float mult_x = source.CurGraphWidth / DX;
                            off_X = 0;
                            float coff_x = off_X - starting_idx * mult_x;

                            if (source.AutoScaleX)
                            {
                                coff_x = off_X;     // avoid dragging in x-autoscale mode
                            }
                            float offset_xCor = -3.0f + offset_x + 4;
                            float unknownOffset = -12;// -14;
                            float offset_yCor = GraphCaptionLineHeight + offset_y + source.CurGraphHeight + unknownOffset;
                            foreach (int XData in marker_data)
                            {
                                float x = XData * mult_x + coff_x;
                                string value = "" +XData;

                                if (source.OnRenderXAxisLabel != null)
                                {
                                    value = source.OnRenderXAxisLabel(XData);
                                }

                                /// TODO: find out how to calculate this offset. Must be padding + something else
                                
                                    SizeF dim = g.MeasureString(value, legendFont);
                                    g.DrawString(value, legendFont, brush,
                                         new PointF((int)( x + offset_xCor - dim.Width / 2),offset_yCor));
                            }
                        }
                    }
                }
            }
        }
        private void DrawYLabels(Graphics g, DataSource source,   float offset_x,  float offset_y )
        {
            using (Brush b = new SolidBrush(Color.Cyan))
            {
                using (Pen pen = new Pen(b))
                {
                    pen.DashPattern = new float[] { 2, 2 };

                    // draw labels for horizontal lines
                    if (source.DY != 0)
                    {
                        float Idx = 0;
                        float y0 = (float)(source.grid_off_y * source.CurGraphHeight / source.DY + source.off_Y);
                        String value = "" + Idx;

                        if (source.OnRenderYAxisLabel != null)
                        {
                            value = source.OnRenderYAxisLabel(source, Idx);
                        }
                        SizeF dim = g.MeasureString(value, legendFont);
                        g.DrawString(value, legendFont, b, new PointF((int)offset_x - dim.Width, (int)(offset_y + y0 + 0.5f + dim.Height / 2)));
                        float GridDistY = source.grid_distance_y;
                        if (source.AutoScaleY)
                        {
                            // calculate a matching grid distance                            
                            GridDistY = -Utilities.MostSignificantDigit(source.DY);

                            if (GridDistY == 0)
                            {
                                GridDistY = source.grid_distance_y;

                            }
                        }
                        int cpt = 0;
                        for (Idx = (source.grid_off_y); Idx >= (source.Cur_YD0); Idx -= GridDistY)
                        {
                            if (Idx != 0)
                            {
                                float y1 = (float)((Idx) * source.CurGraphHeight) / source.DY + source.off_Y;

                                value = "" + (Idx);

                                if (source.OnRenderYAxisLabel != null)
                                {
                                    value = source.OnRenderYAxisLabel(source, Idx);
                                }

                                dim = g.MeasureString(value, legendFont);
                                cpt++;
                                g.DrawString(value, legendFont, b, new PointF((int)offset_x - dim.Width, (int)(offset_y + y1 + 0.5f + dim.Height / 2)));
                            }
                        }
                        for (Idx = (source.grid_off_y); Idx <= (source.Cur_YD1); Idx += GridDistY)
                        {
                            if (Idx != 0)
                            {
                                float y2 = (float)((Idx) * source.CurGraphHeight) / source.DY + source.off_Y;

                                value = "" + (Idx);

                                if (source.OnRenderYAxisLabel != null)
                                {
                                    value = source.OnRenderYAxisLabel(source, Idx);
                                }
                                dim = g.MeasureString(value, legendFont);
                                cpt++;
                                g.DrawString(value, legendFont, b, new PointF((int)offset_x - dim.Width, (int)(offset_y + y2 + 0.5f + dim.Height / 2)));
                            }
                        }
                    }
                }
            }
        }
        private void DrawGraphBox(Graphics g, float offset_x, float offset_y, float w, float h )
        {
            using (Pen p2 = new Pen(GraphBoxColor))
            {
                g.DrawRectangle(p2, (offset_x + 0.5f), (offset_y + 0.5f), w, h);
            }
        }
#endregion
#region FORM EVENTS
        private void OnLoadControl(object sender, EventArgs e)
        {
            memGraphics.Init(this.CreateGraphics(), this.ClientRectangle.Width, this.ClientRectangle.Height);
        }
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            if (ParentForm == null)
            {
                // paint background when control is used in editor
                base.OnPaintBackground(e);
               
            }
            else
            {
                // do not repaint background to avoid flickering
            }
        }
        private void OnResizeForm(object sender, System.EventArgs e)
        {
            memGraphics.Init(this.CreateGraphics(), ClientRectangle.Width, ClientRectangle.Height);
            Invalidate();
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            try
            {
                if (ParentForm != null)
                {
                    EndDrawGraphEvent = false;
                    Graphics CurGraphics = e.Graphics;

                    if (memGraphics.g != null && useDoubleBuffer == true)
                    {
                        CurGraphics = memGraphics.g;
                    }

                    CurGraphics.SmoothingMode = smoothing;

                    PaintControl(CurGraphics,this.Width,this.Height,0,0,true);

                    if (memGraphics.g != null && useDoubleBuffer == true)
                    {
                        memGraphics.Render(e.Graphics);
                    }
                    EndDrawGraphEvent = true;
                }
            }
            catch (Exception ex)
            {
                Console.Write("exception : " + ex.Message);
            }

            base.OnPaint(e);
        }
        #endregion
    }
}
