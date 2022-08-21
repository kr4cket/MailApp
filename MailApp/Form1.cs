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
using System.Threading;
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
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
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
        private void chagePage_3(bool change)
        {
            dataGridView1.Visible = change;
        }
        private void chagePage_4(bool change)
        {
            textBox1.Visible = change;
            textBox2.Visible = change;
            textBox3.Visible = change;
            label10.Visible = change;
            label2.Visible = change;
            label4.Visible = change;
            button7.Visible = change;
            button8.Visible = change;
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
        private void UpdateGrid()
        {
            dataGridView1.Columns.Clear();
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Add("", "Электронная почта");
            dataGridView1.Columns.Add("", "Имя отправителя");
            dataGridView1.Columns.Add("", "Тема");
        }
        private async void GetMessages()
        {
            var messages = await controller.getInbox();
            new Thread(() =>
            {
                Invoke((Action)(() =>
                {
                    //без Конструкции
                    foreach (string message in messages)
                    {
                        var messageData = message.Split('/');
                        dataGridView1.Rows.Add(messageData[0], messageData[1], messageData[2]);
                    }
                }));
            }).Start();
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
            chagePage_4(false);
            changePage_1(false);
            changePage_2(true);
            chagePage_3(false);
            UpdateGrid();
            textBox4.Text = controller.getUserEmail();
            textBox5.Text = controller.getIncodeUserPassword();
            textBox6.Text = controller.getUserLogin();
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



        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            changePage_1(false);
            changePage_2(false);
            chagePage_3(false);
            chagePage_4(true);
            textBox2.Text = dataGridView1[0, e.RowIndex].Value.ToString();
            textBox3.Text = dataGridView1[2, e.RowIndex].Value.ToString();
            textBox1.Text = controller.getMessage(e.RowIndex);
        }

        private async void button6_Click(object sender, EventArgs e)
        {
            changePage_1(false);
            changePage_2(false);
            chagePage_3(true);
            chagePage_4(false);
            UpdateGrid();
            await Task.Run(() => GetMessages());
        }

        private void button5_Click(object sender, EventArgs e)
        {
            chagePage_4(false);
            changePage_1(true);
            changePage_2(false);
            chagePage_3(false);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            chagePage_4(false);
            changePage_1(true);
            changePage_2(false);
            chagePage_3(false);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void button8_Click(object sender, EventArgs e)
        {
            changePage_1(false);
            changePage_2(false);
            chagePage_3(true);
            chagePage_4(false);
        }
    }
}
