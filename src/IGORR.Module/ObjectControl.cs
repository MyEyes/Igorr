using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace IGORR.Modules
{
    public partial class ObjectControl : UserControl
    {
        public ObjectControl(byte[] bytes)
        {
            InitializeComponent();
        }


        public virtual byte[] GetObjectBytes()
        {
            return new byte[0];
        }
    }
}
