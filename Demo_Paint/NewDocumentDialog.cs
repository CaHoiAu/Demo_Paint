using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Demo_Paint
{
    public partial class NewDocumentDialog : Form
    {
        public int CanvasWidth => (int)widthInput.Value;
        public int CanvasHeight => (int)heightInput.Value;

        public NewDocumentDialog()
        {
            InitializeComponent();
            AcceptButton = btnOK;
            CancelButton = btnCancel;
        }

        private void NewDocumentDialog_Load(object sender, EventArgs e)
        {

        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
