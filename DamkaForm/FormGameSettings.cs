using System;
using System.Windows.Forms;

namespace DamkaForm
{
    public partial class FormGameSettings : Form
    {
        private int m_BoardSize;
        private String m_FirstPlayer = null;
        private String m_SecondPlayer = null;

        public int BoardSize
        {
            get { return m_BoardSize; }
        }
        public String FirstPlayer
        {
            get { return m_FirstPlayer;}
        }
        public String SecondPlayer
        {
            get { return m_SecondPlayer;}
        }
        public FormGameSettings()
        {
            InitializeComponent();
        }


        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            textBoxSecondPlayer.Enabled = !textBoxSecondPlayer.Enabled;
            if (textBoxSecondPlayer.Enabled)
            {
                textBoxSecondPlayer.Text = string.Empty;
            }
            else
            {
                textBoxSecondPlayer.Text = "[Computer]";
            }
        }

        private void buttonDone_Click(object sender, EventArgs e)
        {
            m_FirstPlayer = textBoxFirstPlayer.Text;
            if (checkBoxSecondPlayer.Checked)
            {
                m_SecondPlayer = textBoxSecondPlayer.Text;
            }
            else
            {
                m_SecondPlayer = "Computer";
            }

            if (radioButton6X6.Checked)
            {
                m_BoardSize = 6;
            }
            else if (radioButton8X8.Checked)
            {
                m_BoardSize = 8;
            }
            else
            {
                m_BoardSize = 10;
            }

            this.Close();
        }
    }
}
