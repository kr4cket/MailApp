using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MailApp
{
    public partial class Form1 : Form
    {
        private Controller controller = new Controller();
        private List<string> Path = new List<string>();

        private void changePage_1(bool change)
        {
            textBox1.Visible = change;
            textBox2.Visible = change;
            textBox3.Visible = change;
            label1.Visible = change;
            label2.Visible = change;
            label3.Visible = change;
            label4.Visible = change;
            label8.Visible = change;
            checkBox1.Visible = change;
            comboBox1.Visible = change;
            button1.Visible = change;
            button3.Visible = change;
            button4.Visible = change;
        }
        private void changePage_2(bool change)
        {
            textBox4.Visible = change;
            textBox5.Visible = change;
            textBox6.Visible = change;
            label5.Visible = change;
            label6.Visible = change;
            label7.Visible = change;
            label9.Visible = change;
            button2.Visible = change;
            checkBox2.Visible = change;
        }
        public Form1()
        {
            InitializeComponent();
            GetData();
        }
        private async void GetData()
        {
            comboBox1.DataSource = await controller.openEmailFile();
        }
        private async void GetMessages()
        {
            var messages = await controller.getInbox();
            foreach(string message in messages)
            {
                textBox7.Text += message + '\n';
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (controller.checkUser())
            {
                controller.sendMessage(textBox2.Text, textBox3.Text, textBox1.Text, Path, checkBox1.Checked);
                MessageBox.Show("Письмо отправлено!");
            }
            else
            {
                MessageBox.Show("Ошибка!\nНевозможно отправить письмо\nВы не заполнили свои данные!");
            }
        }

        private void изменитьДанныеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            changePage_1(false);
            changePage_2(true);
            textBox4.Text = controller.getUserEmail();
            textBox5.Text = controller.getIncodeUserPassword();
            textBox6.Text = controller.getUserLogin();
        }

        private void письмаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            changePage_1(true);
            changePage_2(false);
        }

        private void полученныеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GetMessages();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox2.Text = comboBox1.SelectedItem.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(textBox4.Text != "" && textBox5.Text != "" && textBox6.Text != "")
            {
                if(textBox5.Text != controller.getIncodeUserPassword())
                    controller.saveUserData(textBox4.Text, textBox5.Text, textBox6.Text);
                    MessageBox.Show("Данные изменены!");
            }
            else
            {
                MessageBox.Show("Ошибка!\nНе все данные заполнены");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Path.Add(openFileDialog1.FileName);
                label8.Text = $"{Path[^1].Split('\\')[^1]} прикреплен";
            }
            if (Path.Count > 0)
                button3.Text = "Добавить файл";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if(fontDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBox1.Font = fontDialog1.Font;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
                textBox5.Text = controller.getUserPassword();
            else
                textBox5.Text = controller.getIncodeUserPassword();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
