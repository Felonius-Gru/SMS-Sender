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
using RestSharp;

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
        private string phone_numbers;

        void Tick()
        {
            /*
            allLines = textBox2.Text.Split('\n');
            if (count == allLines.Length)
            {
                MessageBox.Show("Sent All");
                return;
            }
            
            count++;
            WorkThreadFunction(allLines[count - 1]);
            */
            phone_numbers = textBox2.Text;
            phone_numbers = phone_numbers.Replace("\r\n", ",");
            WorkThreadFunction(phone_numbers);
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

                /* CheapSMS API */

                /*
                string senderid = comboBox1.GetItemText(comboBox1.SelectedItem);
                string message = textBox1.Text;
                string path = "http://198.24.149.4/API/pushsms.aspx?loginID=narinmoor3&password=123456&mobile=" + phone_numbers + "&text=" + WebUtility.UrlEncode(message) + "&senderid=" + senderid + "&route_id=17&Unicode=0";
                                
                WebRequest wrGETURL;
                wrGETURL = WebRequest.Create(path);

                string responseContent = null;

                using (WebResponse response = wrGETURL.GetResponse())
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        using (StreamReader sr99 = new StreamReader(stream))
                        {
                            responseContent = sr99.ReadToEnd();
                        }
                    }
                }
                MessageBox.Show(responseContent);
                */

                /* clxcommunications API */

                /*
                var client = new RestClient("https://api.clxcommunications.com/xms/v1/mjgarages12/batches");
                var request = new RestRequest(Method.POST);
                request.AddHeader("content-type", "application/json");
                request.AddHeader("authorization", "Bearer 3924f2180d7e47168d29289146cc2754");
                request.AddParameter("application/json", "{\"from\":\"12345\",\r\n\"to\":[\"447936973937\"],\r\n\"body\":\"Hello, This is clxcommunication API\"}", ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                */

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                string senderid = comboBox1.GetItemText(comboBox1.SelectedItem);
                string message = textBox1.Text;
                // string post_data = "{\"from\":\"demo\",\r\n\"to\":[\"447936973937\",\"447542897252\"],\r\n\"body\":\"hi, this is\"}";
                string post_data = "{\"from\":\"" + senderid + "\",\r\n\"to\":[\"" + phone_number.Replace(",", "\",\"") + "\"],\r\n\"body\":\"" + message + "\"}";
                WebRequest request = WebRequest.Create("https://api.clxcommunications.com/xms/v1/mjgarages12/batches");

                var httpWebRequest = (HttpWebRequest)request;
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";
                httpWebRequest.PreAuthenticate = true;
                request.Headers.Add("Authorization", "Bearer 3924f2180d7e47168d29289146cc2754");

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = post_data;
                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                string responseContent = null;

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    responseContent = streamReader.ReadToEnd();
                }

                MessageBox.Show(responseContent);
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
