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
        private int nPrimes;
        private static IEnumerable<int> dividents = Enumerable.Range(3, 100000);
        private static IEnumerable<int> divisors = Enumerable.Range(2, 50000);

        public Form1()
        {
            InitializeComponent();
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
                // Start the asynchronous operation.
                backgroundWorker1.RunWorkerAsync();
                dtStart = DateTime.Now;
                button1.Text = "Abbrechen";
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            nPrimes = 1;

            foreach (int current in dividents)
            {
                if (worker.CancellationPending == true)
                {
                    e.Cancel = true;
                    return;
                }
                worker.ReportProgress((current + 1) / 1000);
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
                        break;
                    }
                }
            }
            e.Result = nPrimes;
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
            label5.Text = nPrimes.ToString();
            dtEnd = DateTime.Now;
            label3.Text = (dtEnd - dtStart).ToString();
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
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
                dtEnd = DateTime.Now;
                label5.Text = e.Result.ToString();
                label3.Text = (dtEnd - dtStart).ToString();
            }
        }
    }
}
