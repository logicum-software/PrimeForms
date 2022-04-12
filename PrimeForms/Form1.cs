using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace PrimeForms
{
    public partial class Form1 : Form
    {
        private DateTime dtStart, dtEnd;
        private int nPrimes, nPrimesSum;
        private static IEnumerable<int> dividents;
        private static IEnumerable<int> divisors;
        private static ArrayList nPrimesPerHundretThousands = new ArrayList();

        public Form1()
        {
            InitializeComponent();
            textBoxStart.Text = "3";
            textBoxEnd.Text = "100000";
            dividents = Enumerable.Range(int.Parse(textBoxStart.Text), int.Parse(textBoxEnd.Text));
            divisors = Enumerable.Range(2, int.Parse(textBoxEnd.Text) / 2);
        }

        private void button1_ClickAsync(object sender, EventArgs e)
        {
            if (backgroundWorker1.IsBusy)
            {
                backgroundWorker1.CancelAsync();
                button1.Text = "Berechnung starten";
            }
            else
            {
                listBoxnPrimes.Items.Clear();
                // Start the asynchronous operation.
                backgroundWorker1.RunWorkerAsync();
                dtStart = DateTime.Now;
                button1.Text = "Abbrechen";
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            
            if (int.Parse(textBoxStart.Text) < 3)
            {
                nPrimes = 1;
                nPrimesSum = 1;
            }
            else
            {
                nPrimes = 0;
                nPrimesSum = 0;
            }

            int iMax = int.Parse(textBoxEnd.Text) / 100;

            foreach (int current in dividents)
            {
                if ((float) current % 100000 == 0)
                {
                    nPrimesPerHundretThousands.Add(nPrimes);
                    nPrimes = 0;
                }

                if (worker.CancellationPending == true)
                {
                    e.Cancel = true;
                    return;
                }
                worker.ReportProgress((current + 1) / iMax);
                foreach (int divisor in divisors)
                {
                    if (divisor <= current / 2)
                    {
                        if ((float)current % divisor == 0)
                            break;
                    }
                    else
                    {
                        nPrimes++;
                        nPrimesSum++;
                        break;
                    }
                }
            }
            e.Result = nPrimesSum;
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
            label5.Text = nPrimesSum.ToString();
            dtEnd = DateTime.Now;
            label3.Text = (dtEnd - dtStart).ToString();
        }

        private void textBoxEnd_Leave(object sender, EventArgs e)
        {
            dividents = Enumerable.Range(int.Parse(textBoxStart.Text), int.Parse(textBoxEnd.Text));
            divisors = Enumerable.Range(2, int.Parse(textBoxEnd.Text) / 2);
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            listBoxnPrimes.Items.Clear();

            foreach (var item in nPrimesPerHundretThousands)
                listBoxnPrimes.Items.Add(item.ToString());

            if (e.Cancelled == true)
            {
                label3.Text = "Canceled!";
            }
            else if (e.Error != null)
            {
                label3.Text = "Error: " + e.Error.Message;
            }
            else
            {
                button1.Text = "Berechnung starten";
                dtEnd = DateTime.Now;
                label5.Text = e.Result.ToString();
                label3.Text = (dtEnd - dtStart).ToString();
            }
        }
    }
}
