using System;
using System.Drawing;



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


namespace GraphLib
{
    internal class BackBuffer : IDisposable
    {
        private Bitmap memoryBitmap;
        private Int32 width;
        private Int32 height;

        internal BackBuffer()
        {
            width = 0;
            height = 0;
        }

        internal bool Init(Int32 width, Int32 height)
        {
            if (memoryBitmap != null)
            {
                memoryBitmap.Dispose();
                memoryBitmap = null;
            }

            if (G != null)
            {
                G.Dispose();
                G = null;
            }

            if (width == 0 || height == 0)
            {
                return false;
            }

            if ((width != this.width) ||
                (height != this.height) ||
                G == null)
            {
                this.width = width;
                this.height = height;

                memoryBitmap = new Bitmap(width, height);
                G = Graphics.FromImage(memoryBitmap);
            }

            return true;
        }

        internal void Render(Graphics g)
        {
            if (memoryBitmap != null)
            {
                g.DrawImage(memoryBitmap,
                            new Rectangle(0, 0, width, height),
                            0, 0, width, height,
                            GraphicsUnit.Pixel);
            }
        }

        //internal bool CanDoubleBuffer()
        //{
        //    return G != null;
        //}

        internal Graphics G { get; private set; }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose managed resources
                memoryBitmap.Dispose();

            }
            // free native resources
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
