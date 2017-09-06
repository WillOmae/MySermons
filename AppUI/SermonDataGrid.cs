using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AppUI
{
    public class SermonDataGrid : Form
    {
        List<AppEngine.Database.Sermon> sermonSource = new AppEngine.Database.Sermon().SelectAll();

        List<AppEngine.Database.Series> seriesSource = new AppEngine.Database.Series().SelectAll();
        List<AppEngine.Database.Venue> venueSource = new AppEngine.Database.Venue().SelectAll();
        List<AppEngine.Database.Town> townSource = new AppEngine.Database.Town().SelectAll();
        List<AppEngine.Database.Activity> activitySource = new AppEngine.Database.Activity().SelectAll();
        List<AppEngine.Database.Speaker> speakerSource = new AppEngine.Database.Speaker().SelectAll();
        List<AppEngine.Database.Theme> themeSource = new AppEngine.Database.Theme().SelectAll();

        DataGridView dataGrid = new DataGridView();

        DataGridViewTextBoxColumn index = new DataGridViewTextBoxColumn();
        DataGridViewComboBoxColumn series = new DataGridViewComboBoxColumn();
        DataGridViewTextBoxColumn datecreated = new DataGridViewTextBoxColumn();
        DataGridViewComboBoxColumn venue = new DataGridViewComboBoxColumn();
        DataGridViewComboBoxColumn town = new DataGridViewComboBoxColumn();
        DataGridViewComboBoxColumn activity = new DataGridViewComboBoxColumn();
        DataGridViewComboBoxColumn speaker = new DataGridViewComboBoxColumn();
        DataGridViewComboBoxColumn theme = new DataGridViewComboBoxColumn();
        DataGridViewTextBoxColumn title = new DataGridViewTextBoxColumn();
        DataGridViewTextBoxColumn keytext = new DataGridViewTextBoxColumn();
        DataGridViewTextBoxColumn hymn = new DataGridViewTextBoxColumn();

        /// <summary>
        /// Defines a structure to hold details of cells whose values have been changed
        /// </summary>
        struct DirtyRowStruct
        {
            /// <summary>
            /// Id of the sermon whose value has changed
            /// </summary>
            public int SermonId;
            /// <summary>
            /// Column of the cell whose value has been changed
            /// </summary>
            public string ColumnName;
            /// <summary>
            /// New value of the cell
            /// </summary>
            public string Value;
        }
        /// <summary>
        /// Holds the structure containing details of cells whose values have been changed
        /// </summary>
        List<DirtyRowStruct> DirtyRows = new List<DirtyRowStruct>();
        bool AfterInit = false;

        public SermonDataGrid()
        {
            InitializeComponents();
            
            SuspendLayout();
            int count = 0;
            foreach (var sermon in sermonSource)
            {
                dataGrid.Rows.Add();
                var row = dataGrid.Rows[count];
                row.Tag = sermon.Id;
                row.Cells[0].Value = count + 1;
                row.Cells["Series"].Value = sermon.SeriesId;
                row.Cells["Date Created"].Value = sermon.DateCreated;
                row.Cells["Venue"].Value = sermon.VenueId;
                row.Cells["Town"].Value = sermon.TownId;
                row.Cells["Activity"].Value = sermon.ActivityId;
                row.Cells["Speaker"].Value = sermon.SpeakerId;
                row.Cells["Title"].Value = sermon.Title;
                row.Cells["Theme"].Value = sermon.ThemeId;
                row.Cells["Title"].Value = sermon.Title;
                row.Cells["Key Text"].Value = sermon.KeyText;
                row.Cells["Hymn"].Value = sermon.Hymn;

                count++;
            }
            ResumeLayout();

            AfterInit = true;
        }
        void InitializeComponents()
        {
            ResizeBegin += delegate
            {
                SuspendLayout();
            };
            ResizeEnd += delegate
            {
                ResumeLayout();
            };
            FormClosing += delegate
            {
                SaveChanges();
            };
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterScreen;
            WindowState = FormWindowState.Maximized;

            series.SortMode = DataGridViewColumnSortMode.Automatic;
            datecreated.SortMode = DataGridViewColumnSortMode.Automatic;
            venue.SortMode = DataGridViewColumnSortMode.Automatic;
            town.SortMode = DataGridViewColumnSortMode.Automatic;
            activity.SortMode = DataGridViewColumnSortMode.Automatic;
            speaker.SortMode = DataGridViewColumnSortMode.Automatic;

            series.Name = "Series";
            datecreated.Name = "Date Created";
            venue.Name = "Venue";
            town.Name = "Town";
            activity.Name = "Activity";
            speaker.Name = "Speaker";
            theme.Name = "Theme";
            title.Name = "Title";
            keytext.Name = "Key Text";
            hymn.Name = "Hymn";

            index.ReadOnly = true;
            datecreated.ReadOnly = true;

            if (seriesSource != null)
            {
                series.DataSource = seriesSource;
                series.ValueMember = "Id";
                series.DisplayMember = "Theme";
            }
            if (venueSource != null)
            {
                venue.DataSource = venueSource;
                venue.ValueMember = "Id";
                venue.DisplayMember = "Name";
            }
            if (townSource != null)
            {
                town.DataSource = townSource;
                town.ValueMember = "Id";
                town.DisplayMember = "Name";
            }
            if (activitySource != null)
            {
                activity.DataSource = activitySource;
                activity.ValueMember = "Id";
                activity.DisplayMember = "Name";
            }
            if (speakerSource != null)
            {
                speaker.DataSource = speakerSource;
                speaker.ValueMember = "Id";
                speaker.DisplayMember = "Name";
            }
            if (themeSource != null)
            {
                theme.DataSource = themeSource;
                theme.ValueMember = "Id";
                theme.DisplayMember = "Name";
            }

            dataGrid.AutoGenerateColumns = false;
            dataGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            dataGrid.AllowUserToAddRows = false;
            dataGrid.AllowDrop = false;
            dataGrid.AllowUserToDeleteRows = false;
            dataGrid.AllowUserToOrderColumns = false;
            dataGrid.AllowUserToResizeColumns = true;
            dataGrid.AllowUserToResizeRows = false;
            dataGrid.Columns.AddRange(index, series, datecreated, venue, town, activity, speaker, title, theme, keytext, hymn);
            dataGrid.Dock = DockStyle.Fill;
            dataGrid.Parent = this;
            dataGrid.ShowCellToolTips = true;

            dataGrid.CellValueChanged += CellValueChanged;


            Controls.Add(dataGrid);
        }
        void SaveChanges()
        {
            if (DirtyRows.Count > 0)
            {
                string message = string.Empty;
                foreach (var drs in DirtyRows)
                {
                    ImplementChanges(drs);
                }
                if (DirtyRows.Count == 1)
                {
                    MessageBox.Show("You have made " + 1 + " change.");
                }
                else
                {
                    MessageBox.Show("You have made " + DirtyRows.Count + " changes.");
                }
            }
        }
        void ImplementChanges(DirtyRowStruct drs)
        {
            AppEngine.Database.Sermon sermon = new AppEngine.Database.Sermon(drs.SermonId);
            switch (drs.ColumnName)
            {
                case "Series":
                    sermon.SeriesId = int.Parse(drs.Value);
                    break;
                case "Date Created":
                    sermon.DateCreated = DateTime.Parse(drs.Value);
                    break;
                case "Venue":
                    sermon.VenueId = int.Parse(drs.Value);
                    break;
                case "Town":
                    sermon.TownId = int.Parse(drs.Value);
                    break;
                case "Activity":
                    sermon.ActivityId = int.Parse(drs.Value);
                    break;
                case "Speaker":
                    sermon.SpeakerId = int.Parse(drs.Value);
                    break;
                case "Theme":
                    sermon.ThemeId = int.Parse(drs.Value);
                    break;
                case "Title":
                    sermon.Title = drs.Value;
                    break;
                case "Key Text":
                    sermon.KeyText = drs.Value;
                    break;
                case "Hymn":
                    sermon.Hymn = drs.Value;
                    break;
            }
            sermon.Update(sermon);
        }
        void CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (AfterInit)
            {
                DirtyRowStruct drs = new DirtyRowStruct()
                {
                    SermonId = int.Parse(dataGrid.Rows[e.RowIndex].Tag.ToString()),
                    ColumnName = dataGrid.Columns[e.ColumnIndex].HeaderText,
                    Value = dataGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString()
                };
                DirtyRows.Add(drs);
            }
        }
    }
}