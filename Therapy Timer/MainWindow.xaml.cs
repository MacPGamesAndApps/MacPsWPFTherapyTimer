using System;
using System.Media;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace Therapy_Timer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string PAUSE_LABEL = "Pause";
        private const string RESUME_LABEL = "Resume";

        private int stretchTimes;

        // Timers
        DispatcherTimer tmrTimer = new DispatcherTimer();
        DispatcherTimer tmrController = new DispatcherTimer();
        DispatcherTimer tmrStart = new DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();
            InitializeTimers();
            SetupEventHandlers();
        }

        private void InitializeTimers()
        {
            tmrTimer.Interval = TimeSpan.FromSeconds(1);
            tmrController.Interval = TimeSpan.FromSeconds(3);
            tmrStart.Interval = TimeSpan.FromSeconds(2);
        }

        private void SetupEventHandlers()
        {
            btnStart.Click += btnStart_Click;
            btnPause.Click += btnPause_Click;
            txtHoldTime.GotFocus += txtHoldTime_Focus;
            txtRepetitions.GotFocus += txtRepetitions_Focus;
            tmrTimer.Tick += tmrTimer_Tick;
            tmrController.Tick += tmrController_Tick;
            tmrStart.Tick += tmrStart_Tick;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            frmMain.Cursor = Cursors.Wait;
            btnStart.IsEnabled = false;
            btnStart.Visibility = Visibility.Hidden;
            btnPause.Visibility = Visibility.Visible;
            txtHoldTime.IsEnabled = false;
            txtRepetitions.IsEnabled = false;
            tmrStart.IsEnabled = true;
            tmrStart.Start();
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            if (btnPause.Content.Equals(PAUSE_LABEL))
            {
                frmMain.Cursor = Cursors.Arrow;
                tmrStart.Stop();
                tmrStart.IsEnabled = false;
                tmrTimer.Stop();
                tmrTimer.IsEnabled = false;
                btnPause.Content = RESUME_LABEL;
            }
            else
            {
                frmMain.Cursor = Cursors.Wait;
                tmrTimer.IsEnabled = true;
                tmrTimer.Start();
                btnPause.Content = PAUSE_LABEL;
            }
        }

        private void tmrTimer_Tick(object sender, EventArgs e)
        {
            int holdTime;

            holdTime = int.Parse(lblTimer.Content.ToString());
            holdTime--;
            if (holdTime < 0)
            {
                tmrTimer.Stop();
                tmrTimer.IsEnabled = false;
                tmrController.IsEnabled = true;
                tmrController.Start();
                if (chkSoundNotification.IsChecked.HasValue && chkSoundNotification.IsChecked.Value)
                {
                    SystemSound notification;
                    notification = SystemSounds.Question;
                    notification.Play();
                }
            }
            else
            {
                lblTimer.Content = holdTime.ToString();
            }
        }

        private void tmrController_Tick(object sender, EventArgs e)
        {
            stretchTimes--;
            if (stretchTimes == 0)
            {
                frmMain.Cursor = Cursors.Arrow;
                lblTimer.Content = "Finished";
                lblRemainingTimes.Content = "0";
                btnStart.IsEnabled = true;
                btnStart.Visibility = Visibility.Visible;
                btnPause.Visibility = Visibility.Hidden;
                btnPause.IsEnabled = false;
                txtHoldTime.IsEnabled = true;
                txtRepetitions.IsEnabled = true;
            }
            else
            {
                tmrTimer.IsEnabled = true;
                tmrTimer.Start();
                lblTimer.Content = txtHoldTime.Text;
                lblRemainingTimes.Content = stretchTimes.ToString();
            }
            tmrController.Stop();
            tmrController.IsEnabled = false;
        }

        private void tmrStart_Tick(object sender, EventArgs e)
        {
            lblTimer.Content = txtHoldTime.Text;
            lblRemainingTimes.Content = txtRepetitions.Text;
            stretchTimes = int.Parse(txtRepetitions.Text);
            frmMain.Cursor = Cursors.Arrow;
            //Cursor.Position = Cursor.Position;
            tmrTimer.IsEnabled = true;
            tmrTimer.Start();
            btnPause.IsEnabled = true;
            tmrStart.Stop();
            tmrStart.IsEnabled = false;
        }

        private void txtHoldTime_Focus(object sender, EventArgs e)
        {
            txtHoldTime.Select(0, txtHoldTime.Text.Length);
        }

        private void txtRepetitions_Focus(object sender, EventArgs e)
        {
            txtRepetitions.Select(0, txtRepetitions.Text.Length);
        }

        //private void frmMain_Load(object sender, EventArgs e)
        //{
        //    this.Text = this.Text + " " + Application.ProductVersion;
        //}
    }
}
