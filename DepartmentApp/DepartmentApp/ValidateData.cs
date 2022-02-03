using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DepartmentApp
{
    class ValidateData
    {
        public static void Column_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) || !char.IsDigit(e.KeyChar) || !char.IsNumber(e.KeyChar))
            {
                e.Handled = true;
            }
            else e.Handled = false;
        }
        public static void Column_KeyPressWord(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (!char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
            else e.Handled = false;
        }

        public static void Column_KeyPressDate(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
             if (!char.IsControl(e.KeyChar) && !char.IsPunctuation(e.KeyChar) &&  !char.IsDigit(e.KeyChar))
             {
                 e.Handled = true;
             }
            else e.Handled = false;
        }
        public static void Column_KeyPressTotal(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (!char.IsNumber(e.KeyChar) && !char.IsControl(e.KeyChar) && !char.IsPunctuation(e.KeyChar) && !char.IsDigit(e.KeyChar) && !char.IsLetter(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}
