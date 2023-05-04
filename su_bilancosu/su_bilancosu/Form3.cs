using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace su_bilancosu
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            iTextSharp.text.Document raporlama = new iTextSharp.text.Document();
            PdfWriter.GetInstance(raporlama, new FileStream("C:subilancosu.pdf", FileMode.Create));



                
        }
    }
}
