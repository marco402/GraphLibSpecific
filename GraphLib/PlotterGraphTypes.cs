using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;

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
 *       2021         spécifique version for project Rtl_433_Plugin
 *						         Plugin for SdrSharp
 */

namespace GraphLib
{
    public class DataSource
    {
        public delegate string OnDrawXAxisLabelEvent(Int32 idx);
        public delegate string OnDrawYAxisLabelEvent(DataSource src, float value);
        public OnDrawXAxisLabelEvent OnRenderXAxisLabel = null;
        public OnDrawYAxisLabelEvent OnRenderYAxisLabel = null;
        private Int32 length = 0;
        internal float VisibleDataRange_X = 0;
        internal float DY = 0;
        internal float YD0 = -200;
        internal float YD1 = 200;
        internal float Cur_YD0 = -200;
        internal float Cur_YD1 = 200;
        internal float grid_distance_y = 200;
        internal float off_Y = 0;
        internal float grid_off_y = 0;
        internal bool yFlip = true;
        internal bool Active = true;
        internal float XAutoScaleOffset = 100;
        internal float CurGraphHeight = 1.0f;
        internal float CurGraphWidth = 1.0f;
        public bool AutoScaleY { get; set; } = false;
        public bool AutoScaleX { get; set; } = false;
        private List<PointF> samples = null;
        public List<PointF> Samples
        {
            get => samples;
            set
            {
                samples = value;
                length = samples.Count;
            }
        }
        public float XMin => samples.Min(point => point.X);
        public float XMax => samples.Max(point => point.X);
        public float YMin => samples.Min(point => point.Y);
        public float YMax => samples.Max(point => point.Y);
        public void SetDisplayRangeY(float y_start, float y_end)
        {
            YD0 = y_start;
            YD1 = y_end;
        }
        public void SetGridDistanceY(float grid_dist_y_units)
        {
            grid_distance_y = grid_dist_y_units;
        }
        public void SetGridOriginY(float off_y)
        {
            grid_off_y = off_y;
        }
        [Category("Properties")] // Take this out, and you will soon have problems with serialization;
        [DefaultValue(typeof(string), "")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string Name { get; set; } = string.Empty;
        [Category("Properties")] // Take this out, and you will soon have problems with serialization;
        [DefaultValue(typeof(Color), "")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color GraphColor { get; set; } = Color.Black;
        [Category("Properties")] // Take this out, and you will soon have problems with serialization;
        [DefaultValue(typeof(Int32), "0")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Int32 Length
        {
            get => length;
            set
            {
                length = value;
                //if (length != 0)
                //{
                //    samples = new PointF[length];
                //}
                //else
                if (length == 0)
                {
                    // length is 0
                    if (samples != null)
                    {
                        samples = null;
                    }
                }
            }
        }
        [Category("Properties")] // Take this out, and you will soon have problems with serialization;
        [DefaultValue(typeof(Int32), "1")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Int32 Downsampling { get; set; } = 1;
    }
}
