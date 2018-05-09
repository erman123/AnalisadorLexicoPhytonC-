using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevComponents.DotNetBar.Metro;
using SyntaxHighlighter;

namespace AnalizadorLexicoPhyton
{
    public partial class Form1 : Form
    {
        Lexico lx = new Lexico();
        public Form1()
        {
            InitializeComponent();
            textBox1.Text += 1 + String.Format(Environment.NewLine);
        }
        
        private int TotalLineas(RichTextBox rtxtt)
        {
            return rtxtt.Lines.Count();
        }
        
        private void contarLineas()
        {
            textBox1.Clear();
            int cont = 1;
            int lineas = TotalLineas(syntaxRichTextBox1);
            if (lineas == 0)
            {
                textBox1.Text += 1 + String.Format(Environment.NewLine);
            }
            else
            {
                while (cont <= lineas)
                {
                    textBox1.Text += cont + String.Format(Environment.NewLine);
                    cont++;
                }
            }
        }
   
        private void buttonItem8_Click(object sender, EventArgs e)
        {
         

            dataGridView1.DataSource = null;
            dataGridView1.DataSource = lx.inicio(syntaxRichTextBox1.Text);
            dataGridView1.Columns[0].Width = 65;
            dataGridView1.Columns[2].Width = 260;
            dataGridView1.Columns[3].Width = 65;
         
            
           
            
        }
      
        private void buttonItem6_Click(object sender, EventArgs e)
        {
            Form2 f2 = new Form2();
            f2.Show();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            syntaxRichTextBox1.Focus();
            lx.pintar(syntaxRichTextBox1);

        }

        private void syntaxRichTextBox1_TextChanged(object sender, EventArgs e)
        {
            contarLineas();


            dataGridView1.DataSource = null;
            dataGridView1.DataSource = lx.inicio(syntaxRichTextBox1.Text);
            dataGridView1.Columns[0].Width = 65;
            dataGridView1.Columns[2].Width = 260;
            dataGridView1.Columns[3].Width = 65;


        }

        private void syntaxRichTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (switchButton1.Value)
            {
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = lx.inicio(syntaxRichTextBox1.Text);
                dataGridView1.Columns[0].Width = 65;
                dataGridView1.Columns[2].Width = 260;
                dataGridView1.Columns[3].Width = 65;

            }

        }
    }
}
