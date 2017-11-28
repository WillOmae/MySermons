using AppEngine;
using System;
using System.Windows.Forms;

namespace AppUI
{
    public class SearchDlgBox : Form
    {
        ////////////////// DEFINITIONS OF GLOBAL VARIABLES AND CONSTANTS //////////////////
        ParentForm parentForm;
        private Label lblTitle;
        public TextBox txbSearchString;
        public Button btnSearch;
        public Button btnCancel;
        private Label label1;
        private ComboBox cmbxFilter;
        private ListView lvShowFoundItems;
        private Label label2;
        private ComboBox cmbxDocType;
        private CheckBox chbxConsiderCase;
        private CheckBox chbxMatchWhole;
        //////////////// CONSTRUCTOR //////////////////
        public SearchDlgBox(ParentForm parent)
        {
            parentForm = parent;

            InitializeComponent();

            lvShowFoundItems.View = View.Details;
            cmbxDocType.Items.AddRange(AppUI.ParentForm.availableDocTypes);
            cmbxDocType.Text = cmbxDocType.Items[0].ToString();
            cmbxFilter.Text = cmbxFilter.Items[0].ToString();
            btnSearch.Click += new EventHandler(btnSearch_Click);
            btnCancel.Click += delegate
            {
                Close();
            };

            ColumnHeader chTitle = new ColumnHeader();
            chTitle = lvShowFoundItems.Columns.Add("TITLE");
            chTitle.Width = lvShowFoundItems.Width / 2;
            ColumnHeader chSpeaker = new ColumnHeader();
            chSpeaker = lvShowFoundItems.Columns.Add("SPEAKER");
            chSpeaker.Width = lvShowFoundItems.Width / 2;

            lvShowFoundItems.Visible = true;
            lvShowFoundItems.DoubleClick += lvShowFoundItems_SelectedIndexChanged;

            ShowDialog();
        }
        private void InitializeComponent()
        {
            lblTitle = new Label();
            txbSearchString = new TextBox();
            btnSearch = new Button();
            btnCancel = new Button();
            label1 = new Label();
            cmbxFilter = new ComboBox();
            lvShowFoundItems = new ListView();
            label2 = new Label();
            cmbxDocType = new ComboBox();
            chbxConsiderCase = new CheckBox();
            chbxMatchWhole = new CheckBox();
            SuspendLayout();
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Location = new System.Drawing.Point(4, 70);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new System.Drawing.Size(59, 13);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Search for:";
            // 
            // txbSearchString
            // 
            txbSearchString.Location = new System.Drawing.Point(69, 66);
            txbSearchString.Name = "txbSearchString";
            txbSearchString.Size = new System.Drawing.Size(322, 20);
            txbSearchString.TabIndex = 1;
            // 
            // btnSearch
            // 
            btnSearch.Location = new System.Drawing.Point(397, 39);
            btnSearch.Name = "btnSearch";
            btnSearch.Size = new System.Drawing.Size(75, 21);
            btnSearch.TabIndex = 2;
            btnSearch.Text = "Search";
            btnSearch.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            btnCancel.Location = new System.Drawing.Point(397, 66);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new System.Drawing.Size(75, 20);
            btnCancel.TabIndex = 3;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(4, 43);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(58, 13);
            label1.TabIndex = 4;
            label1.Text = "Search by:";
            // 
            // cmbxFilter
            // 
            cmbxFilter.FormattingEnabled = true;
            cmbxFilter.Items.AddRange(new object[] {
            "Speaker",
            "Title",
            "Theme"});
            cmbxFilter.Location = new System.Drawing.Point(69, 39);
            cmbxFilter.Name = "cmbxFilter";
            cmbxFilter.Size = new System.Drawing.Size(322, 21);
            cmbxFilter.TabIndex = 5;
            // 
            // lvShowFoundItems
            // 
            lvShowFoundItems.FullRowSelect = true;
            lvShowFoundItems.GridLines = true;
            lvShowFoundItems.Location = new System.Drawing.Point(58, 115);
            lvShowFoundItems.Name = "lvShowFoundItems";
            lvShowFoundItems.Size = new System.Drawing.Size(414, 160);
            lvShowFoundItems.TabIndex = 6;
            lvShowFoundItems.UseCompatibleStateImageBehavior = false;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(4, 16);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(59, 13);
            label2.TabIndex = 7;
            label2.Text = "Document:";
            // 
            // cmbxDocType
            // 
            cmbxDocType.FormattingEnabled = true;
            cmbxDocType.Location = new System.Drawing.Point(69, 12);
            cmbxDocType.Name = "cmbxDocType";
            cmbxDocType.Size = new System.Drawing.Size(322, 21);
            cmbxDocType.TabIndex = 8;
            // 
            // chbxConsiderCase
            // 
            chbxConsiderCase.AutoSize = true;
            chbxConsiderCase.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            chbxConsiderCase.Checked = true;
            chbxConsiderCase.CheckState = CheckState.Checked;
            chbxConsiderCase.Location = new System.Drawing.Point(69, 92);
            chbxConsiderCase.Name = "chbxConsiderCase";
            chbxConsiderCase.Size = new System.Drawing.Size(93, 17);
            chbxConsiderCase.TabIndex = 9;
            chbxConsiderCase.Text = "Consider case";
            chbxConsiderCase.UseVisualStyleBackColor = true;
            // 
            // chbxMatchWhole
            // 
            chbxMatchWhole.AutoSize = true;
            chbxMatchWhole.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            chbxMatchWhole.Checked = true;
            chbxMatchWhole.CheckState = CheckState.Checked;
            chbxMatchWhole.Location = new System.Drawing.Point(168, 92);
            chbxMatchWhole.Name = "chbxMatchWhole";
            chbxMatchWhole.Size = new System.Drawing.Size(115, 17);
            chbxMatchWhole.TabIndex = 10;
            chbxMatchWhole.Text = "Match whole string";
            chbxMatchWhole.UseVisualStyleBackColor = true;
            // 
            // SearchDlgBox
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = System.Drawing.SystemColors.ActiveBorder;
            ClientSize = new System.Drawing.Size(484, 287);
            Controls.Add(chbxMatchWhole);
            Controls.Add(chbxConsiderCase);
            Controls.Add(cmbxDocType);
            Controls.Add(label2);
            Controls.Add(lvShowFoundItems);
            Controls.Add(cmbxFilter);
            Controls.Add(label1);
            Controls.Add(btnCancel);
            Controls.Add(btnSearch);
            Controls.Add(txbSearchString);
            Controls.Add(lblTitle);
            FormBorderStyle = FormBorderStyle.None;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "SearchDlgBox";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "GetSermonTitle";
            ResumeLayout(false);
            PerformLayout();

        }

        ////////////////// MY FUNCTIONS START HERE //////////////////
        private void AssignSearch(string FILTER, string searchString, bool considerCase, bool matchWhole)
        {
            string[,] foundItems = Sermon.Search(FILTER, searchString, considerCase, matchWhole);

            if (foundItems.GetLength(1) < 1)
            {
                MessageBox.Show("The " + FILTER.ToUpper() + ": " + searchString + " could not be found.");
            }
            else
            {
                for (int i = 0; i < foundItems.GetLength(1); i++)
                {
                    ListViewItem lvItem = new ListViewItem(foundItems[0, i]);
                    ListViewItem.ListViewSubItem lvSubItem = new ListViewItem.ListViewSubItem(lvItem, foundItems[1, i]);
                    lvItem.SubItems.Add(lvSubItem);
                    lvItem.Name = foundItems[2, i];

                    lvShowFoundItems.Items.Add(lvItem);
                }
                lvShowFoundItems.Sort();
            }
        }
        /* private void btnSearch_Click(<object>, <EventArgs>)
         * This function is called when the button btnSearch is clicked
         * Some housekeeping is carried out as follows:
         *  1. The lvShowFoundItems Control is cleared of all items in anticipation of new items
         *  2. The listofArraySermonComponents is cleared
         *  3. The listofListViewItems is cleared
         * If the textbox for the search string is not empty, the function SearchInDetailsHolderListViewControl is called
         * ...and passed 2 parameters:
         *  1. search string <string>
         *  2. column to search <int>
         * If the textbox is empty, it prompts the user to input a value
        */
        private void btnSearch_Click(object sender, EventArgs e)
        {
            lvShowFoundItems.Items.Clear();

            if (string.IsNullOrEmpty(txbSearchString.Text) || string.IsNullOrWhiteSpace(txbSearchString.Text)) { MessageBox.Show("Please input something to search"); }
            else if (txbSearchString.TextLength != 0)
            {
                AssignSearch(cmbxFilter.Text.ToUpper(), txbSearchString.Text, chbxConsiderCase.Checked, chbxMatchWhole.Checked);
            }
        }
        /* private void lvShowFoundItems_SelectedIndexChanged (<object>, <EventArgs>)
         * This function is called when the user clicks on a list view item
         * 
         * 
        */
        private void lvShowFoundItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            parentForm.AddNewTabPage(SermonReader.DisplayStoredSermon(Sermon.GetSermonComponents(int.Parse(lvShowFoundItems.FocusedItem.Name))));
            Close();
        }
    }
}
