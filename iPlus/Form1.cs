using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace iPlus
{
    public partial class Form1 : Form
    {
        public JObject data;
        public Form1()
        {
            InitializeComponent();
            data = GetData();
        }
        JObject GetData()
        {
            using (var client = new HttpClient())
            {
                var endpoint = new Uri("https://apica.iplus.si/api/Naloga?API_KEY=7B268682-9954-4D1C-9B65-0AE03CD2F239");
                var result = client.GetAsync(endpoint).Result;
                var json = result.Content.ReadAsStringAsync().Result;
                var jobject = JsonConvert.DeserializeObject<JObject>(json);

                return (jobject);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string[] company = Convert.ToString(data["Data"]["a"]).Split('#');
            MessageBox.Show("Ime podjetja: " + company[0] + "\nUlica: " + company[1] + "\nPošta: " + company[2] + "\nDavčna številka: " + company[3], "Podatki o podjetju");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show((string)data["Data"]["c"], "Številka računa");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string receipt = "Ime artikla | Količina | Cena \n";
            for (int i = 0; i < data["Data"]["z"].Count(); i++)
            {
                receipt += data["Data"]["z"][i]["a"] + " | " + data["Data"]["z"][i]["b"] + " | " + data["Data"]["z"][i]["c"] + "\n";
            }
            MessageBox.Show(receipt, "Račun");
        }
        double getPrice()
        {
            double price = 0;
            for (int i = 0; i < data["Data"]["z"].Count(); i++)
            {
                price += (double)data["Data"]["z"][i]["c"];
            }
            return price;
        }
        private void button4_Click(object sender, EventArgs e)
        {

            MessageBox.Show(Convert.ToString(getPrice()) + "€", "Cena računa");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            double davcnastopnja = (double)data["Data"]["e"];
            double brutovrednost = getPrice();
            double DDV = brutovrednost * davcnastopnja;
            double netovrednost = brutovrednost * (1 - davcnastopnja);
            MessageBox.Show("Davčna stopnja: " + (davcnastopnja * 100)  + "%\nNeto vrednost: " + Math.Round(netovrednost,2) + "€\nDDV: " + Math.Round(DDV, 2) + "€\nBruto vrednost: " + Math.Round(brutovrednost, 2));
        }

        private void button6_Click(object sender, EventArgs e)
        {
            MessageBox.Show((string)data["Data"]["b"], "Ime prodajalca");
        }

        private void button7_Click(object sender, EventArgs e)
        {
            
            MessageBox.Show("Datum: " + Convert.ToDateTime(data["Data"]["d"]).ToString("dd.MM.yyyy") + "\nČas: " + Convert.ToDateTime(data["Data"]["d"]).ToString("HH:mm"), "Čas izdaje računa");
        }

        private void button8_Click(object sender, EventArgs e)
        {
            MessageBox.Show((string)data["Data"]["f"], "Zaščitna oznaka izdajateljskega računa (ZOI)");
        }

        private void button9_Click(object sender, EventArgs e)
        {
            MessageBox.Show((string)data["Data"]["g"], "Enkratna identifikacijska oznaka računa (EOR)");
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
