using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PrimeForms
{
    public partial class Form1 : Form
    {
        private DateTime dtStart, dtEnd;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (backgroundWorker1.IsBusy != true)
            {
                // Start the asynchronous operation.
                backgroundWorker1.RunWorkerAsync();
                dtStart = DateTime.Now;
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            int nPrimes = 0;

            if (worker.CancellationPending == true)
            {
                e.Cancel = true;
                return;
            }
            else
            {
                // Perform a time consuming operation and report progress.
                for (int j = 3; j < 500000; j++)
                {
                    for (int z = 2; z < j / 2; z++)
                    {
                        if (j % z == 0)
                        {
                            break;
                        }
                        else if (z == j - 1)
                        {
                            nPrimes++;
                        }
                    }
                    worker.ReportProgress((j + 1) / 5000);
                }
                e.Result = nPrimes;
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
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
