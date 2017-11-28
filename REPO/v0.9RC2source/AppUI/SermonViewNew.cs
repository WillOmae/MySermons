using AppEngine;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace AppUI
{
    public class SermonViewNew : Form
    {
        #region ****************** GLOBAL VARIABLES ******************
        ParentForm parentForm;
        Font font;
        public TreeViewEx treeViewEx;
        public bool isEdited = false;
        public int editedID = 0;

        #region ****************** DESCRIPTIVE CONSTANTS (MACROS) ******************
        public const int iID = 0;
        public const int iSeries = 1;
        public const int iDateCreated = 2;
        public const int iVenue = 3;
        public const int iVenueTown = 4;
        public const int iVenueActivity = 5;
        public const int iSpeaker = 6;
        public const int iTitle = 7;
        public const int iTheme = 8;
        public const int iKeyText = 9;
        public const int iHymn = 10;
        public const int iContent = 11;
        #endregion

        #region ****************** CONTROLS ******************
        private DateTimePicker dtPicker;
        private TextBoxEx txbVenue_Name, txbVenue_Activity, txbVenue_Town, txbSpeakerName, txbTitle, txbKeyText, txbHymn;
        private SplitContainer splitContainer;
        private Button btnSave;
        private Label lblSeries;
        private ComboBox cmbxSeries;
        private TextBoxEx txbTheme;
        private RichTextBoxPro rtbContent;
        #endregion
        #endregion
        
        public SermonViewNew(ParentForm parent)
        {
            CommonConstructor(parent);
        }
        private void CommonConstructor(ParentForm parent)
        {
            Size size = new Size(parent.tabControl.ClientSize.Width, parent.tabControl.ClientSize.Height);
            Size minSize = new Size(parent.iDesktopWorkingWidth / 2, parent.iDesktopWorkingHeight * 3 / 4);
            BackColor = Color.Silver;
            ForeColor = Color.Black;
            Dock = DockStyle.Fill;
            parentForm = parent;
            parentForm.MinimumSize = new Size(684, 462);
            
            InitializeComponent();
            SetValuesFromDB();
            splitContainer.SplitterDistance = 122;
            
            treeViewEx = parentForm.treeviewAvailableDocs;
            treeViewEx.parentForm = parentForm;

            font = new Font(parentForm.FontSystem, txbSpeakerName.Font.Size);
            foreach (Control control in Controls)
            {
                if(control != rtbContent)
                {
                    control.Font = font;
                }
            }

            parent.ResizeEnd += delegate
            {
                splitContainer.SplitterDistance = 122;
            };
            parentForm.menuItemFile_Save.Click += Save_Click;
        }
        private void SaveTypedData()
        {
            try
            {
                StatusBarMessages.SetStatusBarMessageAction("Saving " + txbTitle.Text);
                DateTime dateCreated = new DateTime(dtPicker.Value.Year, dtPicker.Value.Month, dtPicker.Value.Day, dtPicker.Value.Hour, dtPicker.Value.Minute, dtPicker.Value.Second);
                if (isEdited == false)//new sermon
                {
                    Sermon sermon;
                    try
                    {
                        sermon = new Sermon(0, int.Parse(cmbxSeries.SelectedValue.ToString()), dateCreated, txbVenue_Name.Text, txbVenue_Town.Text, txbVenue_Activity.Text, txbSpeakerName.Text, txbKeyText.Text, txbHymn.Text, txbTitle.Text, txbTheme.Text, rtbContent.Rtf);
                    }
                    catch
                    {
                        sermon = new Sermon(0, 0, dateCreated, txbVenue_Name.Text, txbVenue_Town.Text, txbVenue_Activity.Text, txbSpeakerName.Text, txbKeyText.Text, txbHymn.Text, txbTitle.Text, txbTheme.Text, rtbContent.Rtf);
                    }
                    string[] arraySubItems = Sermon.ComponentsToString(sermon);
                    if (Sermon.WriteSermon(arraySubItems) == 1)//sermon inserted successfully
                    {
                        AppEngine.Database.Sermon dummy = new AppEngine.Database.Sermon()
                        {
                            Activity = sermon.Activity,
                            Content = sermon.Content,
                            DateCreated = sermon.DateCreated,
                            Hymn = sermon.Hymn,
                            KeyText = sermon.KeyText,
                            Speaker = sermon.Speaker,
                            Title = sermon.Title,
                            Theme = sermon.Theme,
                            Town = sermon.Town,
                            Venue = sermon.Venue
                        };
                        if (dummy.Exists(dummy))
                        {
                            sermon.Id = dummy.Id;
                            parentForm.tabControl.TabPages.Remove(parentForm.tabControl.SelectedTab);
                            parentForm.AddNewTabPage(SermonReader.DisplayStoredSermon(Sermon.ComponentsToString(sermon)));
                            treeViewEx.AddNewTreeNode(dummy.Title, dummy.Id.ToString());
                        }
                    }
                    else
                    {
                        MessageBox.Show("Document not added.\nPlease try again.");
                    }
                }
                else//edited sermon: use existing ID
                {
                    Sermon sermon = new Sermon(editedID, int.Parse(cmbxSeries.SelectedValue.ToString()), dateCreated, txbVenue_Name.Text, txbVenue_Town.Text, txbVenue_Activity.Text, txbSpeakerName.Text, txbKeyText.Text, txbHymn.Text, txbTitle.Text, txbTheme.Text, rtbContent.Rtf);
                    string[] arraySubItems = Sermon.ComponentsToString(sermon);
                    arraySubItems[iID] = editedID.ToString();
                    Sermon.OverwriteSermon(arraySubItems);
                    parentForm.tabControl.TabPages.Remove(parentForm.tabControl.SelectedTab);
                    parentForm.AddNewTabPage(SermonReader.DisplayStoredSermon(arraySubItems));
                }
                StatusBarMessages.SetStatusBarMessageAction("Saved " + txbTitle.Text);
            }
            catch
            {
                StatusBarMessages.SetStatusBarMessageAction("Error saving " + txbTitle.Text);
            }
        }
        public void Save_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(rtbContent.Text) && !string.IsNullOrEmpty(rtbContent.Text))
            {
                SaveTypedData();
            }
            else
            {
                MessageBox.Show("Content box is empty!");
            }
        }
        public static void CreateNewSermon(ParentForm parent)
        {
            SermonViewNew sermonView = new SermonViewNew(parent);
            parent.AddNewTabPage(sermonView);
        }
        private void InitializeComponent()
        {
            this.dtPicker = new System.Windows.Forms.DateTimePicker();
            this.txbVenue_Name = new AppUI.TextBoxEx();
            this.txbVenue_Activity = new AppUI.TextBoxEx();
            this.txbVenue_Town = new AppUI.TextBoxEx();
            this.txbSpeakerName = new AppUI.TextBoxEx();
            this.txbTitle = new AppUI.TextBoxEx();
            this.txbKeyText = new AppUI.TextBoxEx();
            this.txbHymn = new AppUI.TextBoxEx();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.txbTheme = new AppUI.TextBoxEx();
            this.cmbxSeries = new System.Windows.Forms.ComboBox();
            this.lblSeries = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.rtbContent = new AppUI.RichTextBoxPro();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // dtPicker
            // 
            this.dtPicker.Location = new System.Drawing.Point(12, 12);
            this.dtPicker.Name = "dtPicker";
            this.dtPicker.Size = new System.Drawing.Size(250, 20);
            this.dtPicker.TabIndex = 0;
            // 
            // txbVenue_Name
            // 
            this.txbVenue_Name.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.txbVenue_Name.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.txbVenue_Name.ForeColor = System.Drawing.Color.Gray;
            this.txbVenue_Name.Location = new System.Drawing.Point(12, 38);
            this.txbVenue_Name.MaxLength = 30;
            this.txbVenue_Name.Name = "txbVenue_Name";
            this.txbVenue_Name.Size = new System.Drawing.Size(250, 20);
            this.txbVenue_Name.TabIndex = 1;
            this.txbVenue_Name.Text = "Venue";
            this.txbVenue_Name.Watermark = "Venue";
            // 
            // txbVenue_Activity
            // 
            this.txbVenue_Activity.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.txbVenue_Activity.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.txbVenue_Activity.ForeColor = System.Drawing.Color.Gray;
            this.txbVenue_Activity.Location = new System.Drawing.Point(12, 64);
            this.txbVenue_Activity.MaxLength = 50;
            this.txbVenue_Activity.Name = "txbVenue_Activity";
            this.txbVenue_Activity.Size = new System.Drawing.Size(250, 20);
            this.txbVenue_Activity.TabIndex = 2;
            this.txbVenue_Activity.Text = "Activity";
            this.txbVenue_Activity.Watermark = "Activity";
            // 
            // txbVenue_Town
            // 
            this.txbVenue_Town.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.txbVenue_Town.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.txbVenue_Town.ForeColor = System.Drawing.Color.Gray;
            this.txbVenue_Town.Location = new System.Drawing.Point(12, 90);
            this.txbVenue_Town.MaxLength = 30;
            this.txbVenue_Town.Name = "txbVenue_Town";
            this.txbVenue_Town.Size = new System.Drawing.Size(250, 20);
            this.txbVenue_Town.TabIndex = 3;
            this.txbVenue_Town.Text = "Town";
            this.txbVenue_Town.Watermark = "Town";
            // 
            // txbSpeakerName
            // 
            this.txbSpeakerName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.txbSpeakerName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.txbSpeakerName.ForeColor = System.Drawing.Color.Gray;
            this.txbSpeakerName.Location = new System.Drawing.Point(268, 38);
            this.txbSpeakerName.MaxLength = 50;
            this.txbSpeakerName.Name = "txbSpeakerName";
            this.txbSpeakerName.Size = new System.Drawing.Size(300, 20);
            this.txbSpeakerName.TabIndex = 5;
            this.txbSpeakerName.Text = "Speaker";
            this.txbSpeakerName.Watermark = "Speaker";
            // 
            // txbTitle
            // 
            this.txbTitle.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.txbTitle.ForeColor = System.Drawing.Color.Gray;
            this.txbTitle.Location = new System.Drawing.Point(268, 64);
            this.txbTitle.MaxLength = 50;
            this.txbTitle.Name = "txbTitle";
            this.txbTitle.Size = new System.Drawing.Size(195, 20);
            this.txbTitle.TabIndex = 6;
            this.txbTitle.Text = "Title";
            this.txbTitle.Watermark = "Title";
            // 
            // txbKeyText
            // 
            this.txbKeyText.ForeColor = System.Drawing.Color.Gray;
            this.txbKeyText.Location = new System.Drawing.Point(268, 90);
            this.txbKeyText.MaxLength = 20;
            this.txbKeyText.Name = "txbKeyText";
            this.txbKeyText.Size = new System.Drawing.Size(148, 20);
            this.txbKeyText.TabIndex = 7;
            this.txbKeyText.Text = "Key text";
            this.txbKeyText.Watermark = "Key text";
            // 
            // txbHymn
            // 
            this.txbHymn.ForeColor = System.Drawing.Color.Gray;
            this.txbHymn.Location = new System.Drawing.Point(420, 90);
            this.txbHymn.MaxLength = 50;
            this.txbHymn.Name = "txbHymn";
            this.txbHymn.Size = new System.Drawing.Size(148, 20);
            this.txbHymn.TabIndex = 8;
            this.txbHymn.Text = "Hymn";
            this.txbHymn.Watermark = "Hymn";
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.IsSplitterFixed = true;
            this.splitContainer.Location = new System.Drawing.Point(0, 0);
            this.splitContainer.MinimumSize = new System.Drawing.Size(528, 462);
            this.splitContainer.Name = "splitContainer";
            this.splitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.txbTheme);
            this.splitContainer.Panel1.Controls.Add(this.cmbxSeries);
            this.splitContainer.Panel1.Controls.Add(this.lblSeries);
            this.splitContainer.Panel1.Controls.Add(this.btnSave);
            this.splitContainer.Panel1.Controls.Add(this.txbSpeakerName);
            this.splitContainer.Panel1.Controls.Add(this.dtPicker);
            this.splitContainer.Panel1.Controls.Add(this.txbHymn);
            this.splitContainer.Panel1.Controls.Add(this.txbVenue_Name);
            this.splitContainer.Panel1.Controls.Add(this.txbKeyText);
            this.splitContainer.Panel1.Controls.Add(this.txbVenue_Activity);
            this.splitContainer.Panel1.Controls.Add(this.txbTitle);
            this.splitContainer.Panel1.Controls.Add(this.txbVenue_Town);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.rtbContent);
            this.splitContainer.Size = new System.Drawing.Size(684, 462);
            this.splitContainer.SplitterDistance = 122;
            this.splitContainer.SplitterWidth = 1;
            this.splitContainer.TabIndex = 9;
            // 
            // txbTheme
            // 
            this.txbTheme.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.txbTheme.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.txbTheme.ForeColor = System.Drawing.Color.Gray;
            this.txbTheme.Location = new System.Drawing.Point(468, 64);
            this.txbTheme.Name = "txbTheme";
            this.txbTheme.Size = new System.Drawing.Size(100, 20);
            this.txbTheme.TabIndex = 12;
            this.txbTheme.Text = "Theme";
            this.txbTheme.Watermark = "Theme";
            // 
            // cmbxSeries
            // 
            this.cmbxSeries.FormattingEnabled = true;
            this.cmbxSeries.Location = new System.Drawing.Point(313, 11);
            this.cmbxSeries.MaxDropDownItems = 100;
            this.cmbxSeries.Name = "cmbxSeries";
            this.cmbxSeries.Size = new System.Drawing.Size(255, 21);
            this.cmbxSeries.TabIndex = 4;
            // 
            // lblSeries
            // 
            this.lblSeries.AutoSize = true;
            this.lblSeries.Location = new System.Drawing.Point(268, 15);
            this.lblSeries.Name = "lblSeries";
            this.lblSeries.Size = new System.Drawing.Size(39, 13);
            this.lblSeries.TabIndex = 11;
            this.lblSeries.Text = "Series:";
            // 
            // btnSave
            // 
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSave.Location = new System.Drawing.Point(574, 11);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(98, 98);
            this.btnSave.TabIndex = 10;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.Save_Click);
            // 
            // rtbContent
            // 
            this.rtbContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbContent.Location = new System.Drawing.Point(0, 0);
            this.rtbContent.Name = "rtbContent";
            this.rtbContent.Rtf = "{\\rtf1\\ansi\\ansicpg1252\\deff0\\deflang1033{\\fonttbl{\\f0\\fnil\\fcharset0 Times New R" +
    "oman;}}\r\n\\viewkind4\\uc1\\pard\\f0\\fs41\\par\r\n}\r\n";
            this.rtbContent.Size = new System.Drawing.Size(684, 339);
            this.rtbContent.TabIndex = 9;
            // 
            // SermonViewNew
            // 
            this.ClientSize = new System.Drawing.Size(684, 462);
            this.ControlBox = false;
            this.Controls.Add(this.splitContainer);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(684, 462);
            this.Name = "SermonViewNew";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "New Sermon";
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel1.PerformLayout();
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        private void SetValuesFromDB()
        {
            //set auto complete sources
            AutoCompleteStringCollection acsc;

            acsc = new AutoCompleteStringCollection();
            var venues = new AppEngine.Database.Venue().SelectAll();
            foreach (AppEngine.Database.Venue venue in venues)
            {
                acsc.Add(venue.Name);
            }
            txbVenue_Name.AutoCompleteCustomSource = acsc;

            acsc = new AutoCompleteStringCollection();
            var towns = new AppEngine.Database.Town().SelectAll();
            foreach (AppEngine.Database.Town town in towns)
            {
                acsc.Add(town.Name);
            }
            txbVenue_Town.AutoCompleteCustomSource = acsc;

            acsc = new AutoCompleteStringCollection();
            var activities = new AppEngine.Database.Activity().SelectAll();
            foreach (AppEngine.Database.Activity activity in activities)
            {
                acsc.Add(activity.Name);
            }
            txbVenue_Activity.AutoCompleteCustomSource = acsc;

            acsc = new AutoCompleteStringCollection();
            var speakers = new AppEngine.Database.Speaker().SelectAll();
            foreach (AppEngine.Database.Speaker speaker in speakers)
            {
                acsc.Add(speaker.Name);
            }
            txbSpeakerName.AutoCompleteCustomSource = acsc;

            acsc = new AutoCompleteStringCollection();
            var themes = new AppEngine.Database.Theme().SelectAll();
            foreach (AppEngine.Database.Theme theme in themes)
            {
                acsc.Add(theme.Name);
            }
            txbSpeakerName.AutoCompleteCustomSource = acsc;


            var series = new AppEngine.Database.Series().SelectAll();
            if (series != null)
            {
                cmbxSeries.DataSource = series;
                cmbxSeries.ValueMember = "Id";
                cmbxSeries.DisplayMember = "Name";
                cmbxSeries.SelectedValue = 0;
            }
        }
        public void SetEditingValues(string[] arraySermonComponents)
        {
            Text = arraySermonComponents[iTitle];
            Name = arraySermonComponents[iID];

            dtPicker.Value = DateTime.Parse(arraySermonComponents[iDateCreated]);
            cmbxSeries.SelectedItem = arraySermonComponents[iSeries];
            txbVenue_Name.Text = arraySermonComponents[iVenue];
            txbVenue_Town.Text = arraySermonComponents[iVenueTown];
            txbVenue_Activity.Text = arraySermonComponents[iVenueActivity];
            txbSpeakerName.Text = arraySermonComponents[iSpeaker];
            txbKeyText.Text = arraySermonComponents[iKeyText];
            txbHymn.Text = arraySermonComponents[iHymn];
            txbTitle.Text = arraySermonComponents[iTitle];
            txbTheme.Text = arraySermonComponents[iTheme];
            rtbContent.Rtf = arraySermonComponents[iContent];

            isEdited = true;
            editedID = int.Parse(arraySermonComponents[iID]);
        }
    }
}



//private void InitializeComponents(SermonViewNew parent, Size size)
//{
//    parent.Size = size;
//    parent.FormBorderStyle = FormBorderStyle.None;

//    dtPicker.Name = "dtPicker";
//    dtPicker.ShowUpDown = false;

//    lblDate.Text = "Date";
//    lblDate.Name = "lblDate";

//    lblVenue.Text = "Venue";
//    lblVenue.Name = "lblVenue";

//    lblActivity.Text = "Activity";
//    lblActivity.Name = "lblActivity";

//    lblSpeaker.Text = "Speaker";
//    lblSpeaker.Name = "lblSpeaker";

//    lblTown.Text = "Town";
//    lblTown.Name = "lblTown";

//    lblTitle.Text = "Title";
//    lblTitle.Name = "lblTitle";

//    lblKeyText.Text = "Text";
//    lblKeyText.Name = "lblKeyText";

//    lblHymn.Text = "Hymn";
//    lblHymn.Name = "lblHymn";

//    txbHymn.Name = "txbHymn";

//    txbKeyText.Name = "txbKeyText";

//    txbSpeakerName.Name = "txbSpeakerName";

//    txbTitle.Name = "txbTitle";

//    txbVenue_Activity.Name = "txbVenue_Activity";

//    txbVenue_Name.Name = "txbVenue_Name";

//    txbVenue_Town.Name = "txbVenue_Town";

//    rtbContent.Name = "rtbContent";

//    btnSave.Text = "Save";
//    btnSave.Name = "btnSave";
//    btnSave.Click += new EventHandler(btnSave_Click);


//    splitContainer.Size = size;
//    splitContainer.Orientation = Orientation.Horizontal;
//    splitContainer.Panel1.Controls.AddRange(new Control[]{
//        lblDate,
//        dtPicker,
//        lblVenue,
//        txbVenue_Name,
//        lblTown,
//        txbVenue_Town,
//        lblActivity,
//        txbVenue_Activity,
//        lblSpeaker,
//        txbSpeakerName,
//        lblTitle,
//        txbTitle,
//        lblKeyText,
//        txbKeyText,
//        lblHymn,
//        txbHymn,
//        btnSave
//    });

//    splitContainer.Panel2.Controls.Add(rtbContent);
//    parent.Controls.Add(splitContainer);
//    UpdateControlPositionsAndSizes(parent, size);
//}
//public void UpdateControlPositionsAndSizes(SermonViewNew svParent, Size size)
//{
//    int iDefControlHeight = (int)(0.02 * size.Height), iDefControlWidth = (int)(size.Width * 0.35), iDefLabelWidth = 50,
//        iDefMediumWidth = (int)(0.2548 * iDefControlWidth), iDefSmallWidth = (int)(0.1592 * iDefControlWidth),
//        iVertDistanceBtnControls = (int)(0.6 * iDefControlHeight), iHorDistanceBtnControls = (int)(0.0255 * iDefControlWidth),
//        iButtonHeight = 20;
//    Point pointRow1, pointRow2, pointRow3, pointRow4, pointRow5, pointRow6;

//    pointRow1 = new Point(iHorDistanceBtnControls, svParent.Top + iVertDistanceBtnControls);
//    pointRow2 = new Point(iHorDistanceBtnControls, pointRow1.Y + iDefControlHeight + iVertDistanceBtnControls);
//    pointRow3 = new Point(iHorDistanceBtnControls, pointRow2.Y + iDefControlHeight + iVertDistanceBtnControls);
//    pointRow4 = new Point(iHorDistanceBtnControls, pointRow3.Y + iDefControlHeight + iVertDistanceBtnControls);
//    pointRow5 = new Point(iHorDistanceBtnControls, pointRow4.Y + iDefControlHeight + iVertDistanceBtnControls);
//    pointRow6 = new Point(iHorDistanceBtnControls, pointRow5.Y + iDefControlHeight + iVertDistanceBtnControls);


//    splitContainer.SplitterDistance = pointRow6.Y;
//    splitContainer.SplitterWidth = 1;
//    splitContainer.IsSplitterFixed = true;

//    rtbContent.Top = splitContainer.Panel2.Top;
//    rtbContent.Height = splitContainer.Panel2.Height - 100;
//    rtbContent.Left = splitContainer.Panel2.Left;
//    rtbContent.Width = splitContainer.Panel2.Width - 10;

//    foreach (Control control in splitContainer.Panel1.Controls)
//    {
//        if (control != null)
//        {
//            if (iDefControlHeight > 5)
//            {
//                control.Font = new Font(font.FontFamily, iDefControlHeight - 5);
//            }
//            switch (control.Name)
//            {
//                //pointRow1
//                case "lblDate":
//                    lblDate = (Label)control;
//                    control.Size = new Size(iDefLabelWidth, iDefControlHeight);
//                    control.Top = pointRow1.Y;
//                    control.Left = pointRow1.X;
//                    break;
//                case "dtPicker":
//                    dtPicker = (DateTimePicker)control;
//                    control.Top = pointRow1.Y;
//                    control.Left = pointRow1.X + lblDate.Right + iHorDistanceBtnControls;
//                    break;

//                //pointRow2
//                case "lblVenue":
//                    lblVenue = (Label)control;
//                    control.Size = new Size(iDefLabelWidth, iDefControlHeight);
//                    control.Top = pointRow2.Y;
//                    control.Left = pointRow2.X;
//                    break;
//                case "txbVenue_Name":
//                    txbVenue_Name = (TextBox)control;
//                    control.Size = new Size(iDefControlWidth, iDefControlHeight);
//                    control.Top = pointRow2.Y;
//                    control.Left = pointRow2.X + iHorDistanceBtnControls + lblVenue.Right;
//                    break;
//                case "lblTown":
//                    lblTown = (Label)control;
//                    control.Size = new Size(iDefLabelWidth, iDefControlHeight);
//                    control.Top = pointRow2.Y;
//                    control.Left = size.Width / 2;
//                    break;
//                case "txbVenue_Town":
//                    txbVenue_Town = (TextBox)control;
//                    control.Size = new Size(iDefControlWidth, iDefControlHeight);
//                    control.Top = pointRow2.Y;
//                    control.Left = pointRow2.X + iHorDistanceBtnControls + lblTown.Right;
//                    break;

//                //pointRow3
//                case "lblActivity":
//                    lblActivity = (Label)control;
//                    control.Size = new Size(iDefLabelWidth, iDefControlHeight);
//                    control.Top = pointRow3.Y;
//                    control.Left = pointRow3.X;
//                    break;
//                case "txbVenue_Activity":
//                    txbVenue_Activity = (TextBox)control;
//                    control.Size = new Size(iDefControlWidth, iDefControlHeight);
//                    control.Top = pointRow3.Y;
//                    control.Left = pointRow3.X + iHorDistanceBtnControls + lblActivity.Right;
//                    break;

//                //pointRow4
//                case "lblSpeaker":
//                    lblSpeaker = (Label)control;
//                    control.Size = new Size(iDefLabelWidth, iDefControlHeight);
//                    control.Top = pointRow4.Y;
//                    control.Left = pointRow4.X;
//                    break;
//                case "txbSpeakerName":
//                    txbSpeakerName = (TextBox)control;
//                    control.Size = new Size(iDefControlWidth, iDefControlHeight);
//                    control.Top = pointRow4.Y;
//                    control.Left = pointRow4.X + iHorDistanceBtnControls + lblSpeaker.Right;
//                    break;
//                case "lblKeyText":
//                    lblKeyText = (Label)control;
//                    control.Size = new Size(iDefLabelWidth, iDefControlHeight);
//                    control.Top = pointRow4.Y;
//                    control.Left = size.Width / 2;
//                    break;
//                case "txbKeyText":
//                    txbKeyText = (TextBox)control;
//                    control.Size = new Size(iDefMediumWidth, iDefControlHeight);
//                    control.Top = pointRow4.Y;
//                    control.Left = pointRow4.X + iHorDistanceBtnControls + lblKeyText.Right;
//                    break;
//                case "lblHymn":
//                    lblHymn = (Label)control;
//                    control.Size = new Size(iDefLabelWidth, iDefControlHeight);
//                    control.Top = pointRow4.Y;
//                    control.Left = pointRow4.X + iHorDistanceBtnControls + txbKeyText.Right;
//                    break;
//                case "txbHymn":
//                    txbHymn = (TextBox)control;
//                    control.Size = new Size(iDefMediumWidth, iDefControlHeight);
//                    control.Top = pointRow4.Y;
//                    control.Left = pointRow4.X + iHorDistanceBtnControls + lblHymn.Right;
//                    break;

//                //pointRow5
//                case "lblTitle":
//                    lblTitle = (Label)control;
//                    control.Size = new Size(iDefLabelWidth, iDefControlHeight);
//                    control.Top = pointRow5.Y;
//                    control.Left = pointRow5.X;
//                    break;
//                case "txbTitle":
//                    txbTitle = (TextBox)control;
//                    control.Size = new Size(iDefControlWidth, iDefControlWidth);
//                    control.Top = pointRow5.Y;
//                    control.Left = pointRow5.X + iHorDistanceBtnControls + lblTitle.Right;
//                    break;

//                //pointRow1 again        
//                case "btnSave":
//                    btnSave = (Button)control;
//                    control.Size = new Size(iDefMediumWidth, iButtonHeight);
//                    control.Top = pointRow1.Y;
//                    control.Left = txbVenue_Town.Right - control.Width;
//                    break;
//            }
//        }
//        control.Visible = true;
//    }
//}
