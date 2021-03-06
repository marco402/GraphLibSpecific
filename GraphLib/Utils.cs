
using System;
using System.Windows.Forms;

namespace GraphLib
{
    public class Utilities
    {
        /// <summary>
        /// returns the most significant decimal digit
        /// 250 -> 100
        /// 350 -> 100
        /// 12 -> 10
        /// 5 -> 1
        /// 0.5 .> 0.1
        /// .....
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        static internal float MostSignificantDigit(float Value)
        {
            float n = 1;

            float val_abs = Math.Abs(Value);
            float sig = 1.0f * Math.Sign(Value);

            if (val_abs > 1)
            {
                while (n < val_abs)
                {
                    n *= 10.0f;
                }

                return (float)((Int32)(sig * n / 10));
            }
            else // n <= 1
            {
                while (n > val_abs)
                {
                    n /= 10.0f;
                }

                return sig * n;
            }
        }
        static public void  getVersion()
        {
            MessageBox.Show("version GraphLib:1.5.0.0", "start plugin rtl433", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}