using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Web;
using System.Net;
using System.Timers;
using System.Net.Http;
using System.Collections.Specialized;

namespace SMS_Sender
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string keyName = @"HKEY_CURRENT_USER\SOFTWARE\SMS Sender";
            string valueName = "registered";

            if ((string)Registry.GetValue(keyName, valueName, "0") == "0" || Registry.GetValue(keyName, valueName, "0") == null)
            {
                //code if key Not Exist
                Form5 dlg = new Form5();
                dlg.ShowDialog();
            }

            //int counter = 0;
            string line;

            // Read the file and display it line by line.
            System.IO.StreamReader file = new System.IO.StreamReader("senders.txt");
            while ((line = file.ReadLine()) != null)
            {
                comboBox1.Items.Add(line);
                //counter++;
            }

            comboBox1.SelectedIndex = 0;

            file.Close();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            count = 0;
            Tick();
        }

        private int count;
        private string[] allLines;

        void Tick()
        {
            allLines = textBox2.Text.Split('\n');
            if (count == allLines.Length)
            {
                MessageBox.Show("Sent All");
                return;
            }
            
            count++;
            WorkThreadFunction(allLines[count - 1]);
        }
        
        public void WorkThreadFunction(string phone_number)
        {
            try
            {
                /* Mocean API */

                /*
                string mocean_api_key = "dadcf426";
                string mocean_api_secret = "d712f7f4";
                string senderid = comboBox1.GetItemText(comboBox1.SelectedItem);
                string message = textBox1.Text;
                string path = "https://rest-api.moceansms.com/rest/1/sms?mocean-api-key=" + mocean_api_key + "&mocean-api-secret=" + mocean_api_secret +
                    "&mocean-to=" + phone_number + "&mocean-from=" + WebUtility.UrlEncode(senderid) + "&mocean-text=" + WebUtility.UrlEncode(message);
                */

                /* Textbelt API */

                /*
                string message = textBox1.Text;
                string key = "bf2d7b712722c605b36bf47262396014ad12c0d3ofgXWorFXWdY97vaADtUfwZRP";
                string post_data = $"phone={phone_number}&message={message}&key={key}";
                string data1 = System.Net.WebUtility.UrlEncode(post_data);
                data1 = data1.Replace("%3D", "=").Replace("+", "%20").Replace("%26", "&");
                byte[] data = Encoding.ASCII.GetBytes(data1);

                WebRequest request = WebRequest.Create("https://textbelt.com/text");
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                string responseContent = null;

                using (WebResponse response = request.GetResponse())
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        using (StreamReader sr99 = new StreamReader(stream))
                        {
                            responseContent = sr99.ReadToEnd();
                        }
                    }
                }
                // MessageBox.Show(responseContent);
                */

                /* Twilio API */



                Tick();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form6 dlg = new Form6();
            dlg.ShowDialog();
        }
    }
}
