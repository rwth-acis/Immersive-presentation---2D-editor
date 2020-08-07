using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace _2D_Editor.CustomControls
{
    public class ButtonWithImage : Button
    {
        static ButtonWithImage()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ButtonWithImage), new FrameworkPropertyMetadata(typeof(ButtonWithImage)));
        }
    }
}
