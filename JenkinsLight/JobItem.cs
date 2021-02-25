using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using JenkinsClient;

namespace JenkinsLight
{
    public class JobItem : UserControl
    {
        private FlowLayoutPanel flowLayoutPanel1;
        private LinkLabel JobLink;
        private Label relativeTime;
        private string BaseUri = null;
        private string jobname = null;
        private Color Green = Color.FromArgb(75, 139, 59);

        public JobItem(string base_uri, string jobname, List<Build> builds)
        {
            BaseUri = base_uri;
            InitializeComponent();
            JobLink.Text = jobname;
            JobLink.LinkClicked += GoToJobPage;

            this.jobname = jobname;

            if (builds.Any())
                relativeTime.Text = $"<{Helpers.UnixTimeStampToDateTime(builds[0].timestamp).ToRelativeDate()}>";

            foreach (var build in builds)
            {
                var statusButton = new Button
                {
                    Tag = build.number,
                    Size = new Size(18, 18),
                    BackColor = build.result == "SUCCESS" ? Green : Color.Red
                };

                statusButton.Click += GoToBuildPage;

                flowLayoutPanel1.Controls.Add(statusButton);
            }
        }

        private void GoToJobPage(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start($"{BaseUri}/job/{Convert.ToString(((LinkLabel)sender).Text)}/");
        }

        private void GoToBuildPage(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start($"{BaseUri}/job/{jobname}/{Convert.ToString(((Button)sender).Tag)}/");
        }

        private void InitializeComponent()
        {
            this.relativeTime = new Label();
            this.flowLayoutPanel1 = new FlowLayoutPanel();
            this.JobLink = new LinkLabel();
            this.SuspendLayout();
            // 
            // relativeTime
            // 
            this.relativeTime.AutoSize = true;
            this.relativeTime.Location = new System.Drawing.Point(22, 30);
            this.relativeTime.Name = "relativeTime";
            this.relativeTime.Font = new Font("Microsoft Sans Serif", 8.25F);
            this.relativeTime.Size = new System.Drawing.Size(35, 13);
            this.relativeTime.TabIndex = 1;
            this.relativeTime.Text = "label2";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Location = new Point(190, 10);
            this.flowLayoutPanel1.Margin = new Padding(1);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.RightToLeft = RightToLeft.Yes;
            this.flowLayoutPanel1.Size = new Size(160, 33);
            this.flowLayoutPanel1.TabIndex = 2;
            // 
            // JobLink
            // 
            this.JobLink.AutoSize = true;
            this.JobLink.Location = new Point(22, 10);
            this.JobLink.Name = "JobLink";
            this.JobLink.Font = new Font("Microsoft Sans Serif", 10.25F);
            this.JobLink.Size = new Size(55, 13);
            this.JobLink.TabIndex = 3;
            this.JobLink.TabStop = true;
            this.JobLink.Text = "linkLabel1";
            // 
            // JobItem
            // 
            this.BackColor = SystemColors.Control;
            this.Controls.Add(this.JobLink);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.relativeTime);
            this.Name = "JobItem";
            this.Size = new Size(350, 49);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
