using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;

namespace ShutdownTimer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private int timeToSet = new int();
        private System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
        public MainWindow()
        {
            InitializeComponent();

            timer.Enabled = false;
            timer.Interval = 100;
            timer.Tick += new EventHandler(OnTick);

            timeToSet =  0;
            FocusNumBox();

            
        }

        private void StartShutdown()
        {
            NumberBox.IsEnabled = false;
            if (ProgressBar.Maximum != timeToSet)
            {
                ProgressBar.Value = 0;
                ProgressBar.Maximum = timeToSet;
                if (NumberBox.Text.Length > 1 && NumberBox.Text[0] == '0')
                    NumberBox.Text = NumberBox.Text.TrimStart('0');
                NumberBox.Text = Convert.ToString(timeToSet / 60);
            }
            btnStop.IsEnabled = true;
            btnStop.Focus();
            ProgressBar.IsEnabled = true;
            ProgressBar.Visibility = Visibility.Visible;
            timer.Start();
            int calcTime = (int)(timeToSet - ProgressBar.Value);
            string args = " /s /f" + (timeToSet == 0 ? "" : (" /t " + calcTime.ToString() + " /c \" \""));
            Process.Start("Shutdown", args);
            //Console.WriteLine("shutdown" + args);
        }
        private void StopShutdown()
        {
            //Console.WriteLine("shutdown /a");
            Process.Start("Shutdown", " /a");
            btnStop.IsEnabled = false;
            timer.Stop();
            ProgressBar.IsEnabled = false;
            ProgressBar.Value = (int)ProgressBar.Value;
            NumberBox.Text = Convert.ToString((int)(timeToSet - ProgressBar.Value));
            NumberBox.IsEnabled = true;
            FocusNumBox();
        }

        private void FocusNumBox()
        {
            NumberBox.Focus();
            NumberBox.Select(0, NumberBox.Text.Length);
        }

        private void ProgressBarEnabled(bool b)
        {

        }

        private void OnTick(object source, EventArgs e)
        {
            NumberBox.Text = Convert.ToString((int)(timeToSet - ProgressBar.Value)) + " sec";
            if (ProgressBar.Value < ProgressBar.Maximum)
                ProgressBar.Value += 0.1;
            else
                timer.Stop();
        }

        private void NumberBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                StartShutdown();
            }
        }

        private void NumberBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (NumberBox.IsEnabled)
            {
                NumberBox.Text = NumberBox.Text.Trim();
                int length = NumberBox.Text.Length;
                
                if (length != 0)
                {
                    double parsedValue;
                    if (!double.TryParse(NumberBox.Text, out parsedValue))
                    {
                        NumberBox.Text = NumberBox.Text.Remove(length - 1);
                        NumberBox.Select(length, 0);
                    }
                    else
                    {
                        timeToSet = Convert.ToInt32(parsedValue * 60);
                    }
                }
                else
                {
                    timeToSet = 0;
                }
            }
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            StopShutdown();
        }
    }
}
