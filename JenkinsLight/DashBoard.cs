using JenkinsClient;
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;

namespace JenkinsLight
{
    public partial class DashBoard : Form
    {
        private Timer timer = new Timer();
        private IJClient client;

        public DashBoard() { InitializeComponent(); Icon.Visible = true; }

        private void StartPollingToggleButton(object sender, EventArgs e)
        {
            if (timer.Enabled)
            {
                timer.Stop();
                startPollingButton.BackColor = Color.Transparent;
                Icon.Icon = Properties.Resources.yellow;
                return;
            }

            Update(sender, e);

            timer.Interval = Convert.ToInt32(intervalSeconds.Value * 1000);
            timer.Tick += Update;
            timer.Start();

            startPollingButton.BackColor = Color.Green;
        }

        private void TestButtonClick(object sender, EventArgs e)
        {
            client = new JClient(host.Text, username.Text, password.Text);

            if (!client.Test())
            {
                testButton.BackColor = Color.Red;
                startPollingButton.Enabled = false;
                return;
            }

            testButton.BackColor = Color.Green;
            startPollingButton.Enabled = true;
            testButton.Enabled = false;
        }

        private void Update(object sender, EventArgs e)
        {
            var result = client.GetJobs();
            Dictionary<string, List<Build>> jobItems = new Dictionary<string, List<Build>>();

            bool anyBroken = false;
            foreach (Job job in result.jobs)
            {
                var build = client.GetBuilds(job.name).builds;
                if (build.Any() && build[0].result != "SUCCESS")
                    anyBroken = true;

                jobItems.Add(job.name, build);
            }

            SetStatus(anyBroken);

            flowLayoutPanel1.Controls.Clear();
            foreach (var build in jobItems)
                flowLayoutPanel1.Controls.Add(new JobItem(host.Text, build.Key, build.Value));
        }

        private void ResetButtonClick(object sender, EventArgs e)
        {
            client = null;
            if (timer.Enabled)
                timer.Stop();

            flowLayoutPanel1.Controls.Clear();
            Icon.Icon = Properties.Resources.yellow;

            startPollingButton.Enabled = false;
            startPollingButton.BackColor = Color.Transparent;
            testButton.BackColor = Color.Transparent;
            testButton.Enabled = true;
        }

        private void SetStatus(bool failure)
        {
            if (failure)
                Icon.Icon = Properties.Resources.red;
            else
                Icon.Icon = Properties.Resources.green;
        }

        private void DashBoard_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                Hide();
            }
        }

        private void Icon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
        }

        private void DashBoard_FormClosing(object sender, FormClosingEventArgs e)
        {
            Icon.Visible = false;
        }
    }
}
