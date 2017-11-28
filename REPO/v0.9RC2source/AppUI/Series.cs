using AppEngine.Database;
using System.Windows.Forms;

namespace AppUI
{
    public class Series : Form
    {
        private DateTimePicker dtStartDate;
        private DateTimePicker dtEndDate;
        private Label lblStartDate;
        private Label lblEndDate;
        private TextBoxEx txbSpeaker;
        private TextBoxEx txbTheme;
        private TextBoxEx txbVenue;
        private TextBoxEx txbTown;
        private TextBoxEx txbActivity;
        private Button btnSave;

        public Series()
        {
            InitializeComponent();
            SetValuesFromDB();
        }
        private void InitializeComponent()
        {
            this.dtStartDate = new System.Windows.Forms.DateTimePicker();
            this.dtEndDate = new System.Windows.Forms.DateTimePicker();
            this.lblStartDate = new System.Windows.Forms.Label();
            this.lblEndDate = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.txbSpeaker = new AppUI.TextBoxEx();
            this.txbTheme = new AppUI.TextBoxEx();
            this.txbVenue = new AppUI.TextBoxEx();
            this.txbTown = new AppUI.TextBoxEx();
            this.txbActivity = new AppUI.TextBoxEx();
            this.SuspendLayout();
            // 
            // dtStartDate
            // 
            this.dtStartDate.Location = new System.Drawing.Point(72, 12);
            this.dtStartDate.Name = "dtStartDate";
            this.dtStartDate.Size = new System.Drawing.Size(200, 20);
            this.dtStartDate.TabIndex = 1;
            // 
            // dtEndDate
            // 
            this.dtEndDate.Location = new System.Drawing.Point(72, 38);
            this.dtEndDate.Name = "dtEndDate";
            this.dtEndDate.Size = new System.Drawing.Size(200, 20);
            this.dtEndDate.TabIndex = 2;
            // 
            // lblStartDate
            // 
            this.lblStartDate.AutoSize = true;
            this.lblStartDate.Location = new System.Drawing.Point(12, 16);
            this.lblStartDate.Name = "lblStartDate";
            this.lblStartDate.Size = new System.Drawing.Size(56, 13);
            this.lblStartDate.TabIndex = 3;
            this.lblStartDate.Text = "Start date:";
            // 
            // lblEndDate
            // 
            this.lblEndDate.AutoSize = true;
            this.lblEndDate.Location = new System.Drawing.Point(12, 42);
            this.lblEndDate.Name = "lblEndDate";
            this.lblEndDate.Size = new System.Drawing.Size(53, 13);
            this.lblEndDate.TabIndex = 4;
            this.lblEndDate.Text = "End date:";
            // 
            // btnSave
            // 
            this.btnSave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSave.Location = new System.Drawing.Point(105, 205);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 10;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.Save_Click);
            // 
            // txbSpeaker
            // 
            this.txbSpeaker.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.txbSpeaker.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.txbSpeaker.ForeColor = System.Drawing.Color.Gray;
            this.txbSpeaker.Location = new System.Drawing.Point(12, 75);
            this.txbSpeaker.Name = "txbSpeaker";
            this.txbSpeaker.Size = new System.Drawing.Size(260, 20);
            this.txbSpeaker.TabIndex = 11;
            this.txbSpeaker.Text = "Speaker";
            this.txbSpeaker.Watermark = "Speaker";
            // 
            // txbTheme
            // 
            this.txbTheme.ForeColor = System.Drawing.Color.Gray;
            this.txbTheme.Location = new System.Drawing.Point(12, 101);
            this.txbTheme.Name = "txbTheme";
            this.txbTheme.Size = new System.Drawing.Size(260, 20);
            this.txbTheme.TabIndex = 12;
            this.txbTheme.Text = "Theme";
            this.txbTheme.Watermark = "Theme";
            // 
            // txbVenue
            // 
            this.txbVenue.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.txbVenue.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.txbVenue.ForeColor = System.Drawing.Color.Gray;
            this.txbVenue.Location = new System.Drawing.Point(12, 128);
            this.txbVenue.Name = "txbVenue";
            this.txbVenue.Size = new System.Drawing.Size(260, 20);
            this.txbVenue.TabIndex = 13;
            this.txbVenue.Text = "Venue";
            this.txbVenue.Watermark = "Venue";
            // 
            // txbTown
            // 
            this.txbTown.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.txbTown.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.txbTown.ForeColor = System.Drawing.Color.Gray;
            this.txbTown.Location = new System.Drawing.Point(12, 153);
            this.txbTown.Name = "txbTown";
            this.txbTown.Size = new System.Drawing.Size(260, 20);
            this.txbTown.TabIndex = 14;
            this.txbTown.Text = "Town";
            this.txbTown.Watermark = "Town";
            // 
            // txbActivity
            // 
            this.txbActivity.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.txbActivity.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.txbActivity.ForeColor = System.Drawing.Color.Gray;
            this.txbActivity.Location = new System.Drawing.Point(12, 180);
            this.txbActivity.Name = "txbActivity";
            this.txbActivity.Size = new System.Drawing.Size(260, 20);
            this.txbActivity.TabIndex = 15;
            this.txbActivity.Text = "Activity";
            this.txbActivity.Watermark = "Activity";
            // 
            // Series
            // 
            this.ClientSize = new System.Drawing.Size(284, 236);
            this.Controls.Add(this.txbActivity);
            this.Controls.Add(this.txbTown);
            this.Controls.Add(this.txbVenue);
            this.Controls.Add(this.txbTheme);
            this.Controls.Add(this.txbSpeaker);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.lblEndDate);
            this.Controls.Add(this.lblStartDate);
            this.Controls.Add(this.dtEndDate);
            this.Controls.Add(this.dtStartDate);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Series";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Create a series of studies...";
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        public bool VerifyComponents()
        {
            try
            {
                //Needed components: speaker, theme, venue
                if (txbSpeaker.TextLength > 0 && txbSpeaker.Text != txbSpeaker.Watermark)
                {
                    if (txbActivity.TextLength > 0 && txbActivity.Text != txbActivity.Watermark)
                    {
                        if (txbTheme.TextLength > 0 && txbTheme.Text != txbTheme.Watermark)
                        {
                            //Check to ensure the watermark is removed before saving
                            if (txbVenue.Text == txbVenue.Watermark)
                            {
                                txbVenue.Text = "";
                            }
                            if (txbTown.Text == txbTown.Watermark)
                            {
                                txbTown.Text = "";
                            }
                            return true;
                        }
                        else
                        {
                            MessageBox.Show("Kindly specify the theme", "Missing value");
                            txbTheme.Focus();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Kindly specify the activity", "Missing value");
                        txbActivity.Focus();
                    }
                }
                else
                {
                    MessageBox.Show("Kindly specify the speaker's name", "Missing value");
                    txbSpeaker.Focus();
                }
            }
            catch {; }
            return false;
        }
        private void Save_Click(object sender, System.EventArgs e)
        {
            Save();
        }
        private void Save()
        {
            if (VerifyComponents())
            {
                AppEngine.Database.Series series = new AppEngine.Database.Series()
                {
                    Activity = txbActivity.Text,
                    EndDate = dtEndDate.Value,
                    StartDate = dtStartDate.Value,
                    Speaker = txbSpeaker.Text,
                    Theme = txbTheme.Text,
                    Town = txbTown.Text,
                    Venue = txbVenue.Text
                };
                int returnValue = series.Insert(series);
                if (returnValue == 1)
                {
                    series.Exists(series);
                    SeriesSpeakers ss = new SeriesSpeakers()
                    {
                        SpeakerId = series.SpeakerId,
                        SeriesId = series.Id
                    };
                    ss.Insert(ss);
                    MessageBox.Show("New series added successfully!", "SUCCESS");
                    Close();
                }
                else if (returnValue == 0)
                {
                    MessageBox.Show("The series already exists. Kindly check to confirm duplicity.", "UNSUCCESSFUL");
                }
                else if (returnValue == -1)
                {
                    MessageBox.Show("Unable to add the series. Please try again.", "UNSUCCESSFUL");
                }
            }
        }
        private void SetValuesFromDB()
        {
            //set auto complete sources
            AutoCompleteStringCollection acsc;

            acsc = new AutoCompleteStringCollection();
            var venues = new Venue().SelectAll();
            foreach (Venue venue in venues)
            {
                acsc.Add(venue.Name);
            }
            txbVenue.AutoCompleteCustomSource = acsc;

            acsc = new AutoCompleteStringCollection();
            var towns = new Town().SelectAll();
            foreach (Town town in towns)
            {
                acsc.Add(town.Name);
            }
            txbTown.AutoCompleteCustomSource = acsc;

            acsc = new AutoCompleteStringCollection();
            var activities = new Activity().SelectAll();
            foreach (Activity activity in activities)
            {
                acsc.Add(activity.Name);
            }
            txbActivity.AutoCompleteCustomSource = acsc;

            acsc = new AutoCompleteStringCollection();
            var speakers = new Speaker().SelectAll();
            foreach (Speaker speaker in speakers)
            {
                acsc.Add(speaker.Name);
            }
            txbSpeaker.AutoCompleteCustomSource = acsc;
        }
    }
}