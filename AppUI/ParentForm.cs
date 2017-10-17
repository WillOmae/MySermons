using AppEngine;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace AppUI
{
    public class ParentForm : Form
    {
        #region ****************** GLOBAL VARIABLES AND STRUCT DEFINITIONS ******************
        public SplashScreen splashScreen = null;
        static public StartPageForm startPage;

        public int iDesktopWorkingWidth = 0;
        public int iDesktopWorkingHeight = 0;

        public Color ControlColour
        {
            get
            {
                Form trial = new Form();
                try
                {
                    trial.BackColor = ColorExtractor.ExtractColor(Preferences.ColourControls);
                    return ColorExtractor.ExtractColor(Preferences.ColourControls);
                }
                catch
                {
                    return Color.Maroon;
                }
            }
        }
        public Color ColourFont
        {
            get
            {
                Form trial = new Form();
                try
                {
                    trial.BackColor = ColorExtractor.ExtractColor(Preferences.ColourFont);
                    return ColorExtractor.ExtractColor(Preferences.ColourFont);
                }
                catch
                {
                    return Color.White;
                }
            }
        }
        public string FontSystem
        {
            get
            {
                return Preferences.FontSystem;
            }
        }
        public string FontReader
        {
            get
            {
                return Preferences.FontReader;
            }
        }
        public string FontWriter
        {
            get
            {
                return Preferences.FontWriter;
            }
        }
        public Font SystemFont
        {
            get
            {
                using (FontFamily fontFamily = new FontFamily(FontSystem))
                {
                    float emSize = 14.25F;
                    //emSize = Font.SizeInPoints;
                    return new Font(fontFamily, emSize, GraphicsUnit.Pixel);
                }
            }
        }
        public string szAppName = Properties.Resources.AppName;
        static public string[] availableDocTypes = new string[] { "Sermon" };
        public bool canExitSplashScreen = false;

        public TabControl tabControl;
        public MenuStrip menuStrip;
        public TreeViewEx treeviewAvailableDocs;
        public SplitContainer splitContainer, splitContainer0, splitContainer1, splitContainer2;
        public StatusStrip statusBar;
        public ToolStripStatusLabel statusLabelUpdates, statusLabelShowing, statusLabelAction;

        public ToolStripMenuItem menuItemFile, menuItemFile_New, menuItemFile_NewSermon, menuItemFile_NewSeries, menuItemFile_Open, menuItemFile_Save, menuItemFile_Print, menuItemFile_Close, menuItemFile_CloseAll, menuItemFile_Exit;
        public ToolStripMenuItem menuItemView, menuItemView_Customise, menuItemView_StartPage;
        public ToolStripMenuItem menuItemTools, menuItemTools_Recover, menuItemTools_Search, menuItemsTools_InitializeDB, menuItemsTools_ViewData;
        public ToolStripMenuItem menuItemHelp, menuItemHelp_About, menuItemHelp_Update;
        public ToolStripSeparator menuItemSeparator;
        #endregion
        
        /// <summary>
        /// Initialises a new instance of the ParentForm class.
        /// </summary>
        public ParentForm()
        {
            try
            {
                MainThread.CheckDBExistence();
                Preferences.SetPreferences();
                //Aid in splashScreen display
                ShowInTaskbar = false;
                WindowState = FormWindowState.Minimized;
                //
                if (splashScreen == null)
                {
                    splashScreen = new SplashScreen(this);
                }

                WindowInteropHelper helper = new WindowInteropHelper(this);
                Screen currentScreen = Screen.FromHandle(helper.Handle);

                Shown += new EventHandler(ParentForm_Shown);
                Load += new EventHandler(ParentForm_Load);
                FormClosing += new FormClosingEventHandler(OnFormClosing);
                Resize += delegate { UpdateControlSizes(); };

                Text = szAppName;
                
                iDesktopWorkingWidth = currentScreen.WorkingArea.Width;
                iDesktopWorkingHeight = currentScreen.WorkingArea.Height;

                Size size = new Size(iDesktopWorkingWidth, iDesktopWorkingHeight);

                InitialiseForm(size);
                
                foreach (Control control in Controls)
                {
                    UpdateControlColorsFonts(control, ControlColour, ColourFont, SystemFont);
                }
                UpdateControlSizes();
                try { treeviewAvailableDocs.SortTreeView(TreeViewEx.FILTER); }
                catch {; }

                SermonReader.parentForm = this;
                StatusBarMessages.statusLabelAction = statusLabelAction;
                StatusBarMessages.statusLabelShowing = statusLabelShowing;
                StatusBarMessages.statusLabelUpdates = statusLabelUpdates;
            }
            catch (Exception e)
            {
                MessageBox.Show("An error was encountered while trying to load the application. Re-run the application.\nIf the error persists, reinstall the application. See error details below:\n\n\n" + e.ToString());
                Close();
            }
        }
        
        /// <summary>
        ///     Occurs before the form is displayed for the first time.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ParentForm_Load(object sender, EventArgs e)
        {
            MainThread.InitialChecks();
            XMLBible.LoadBibleIntoMemory();
        }
        /// <summary>
        ///     Occurs just after the form is first displayed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ParentForm_Shown(object sender, EventArgs e)
        {
            SuspendLayout();
            canExitSplashScreen = true;
            while (splashScreen.hasClosedSplashScreen == false) {; }

            WindowState = FormWindowState.Normal;
            ShowInTaskbar = true;
            Cursor.Show();//it was hidden when the splashscreen was displayed

            startPage = new StartPageForm(this);
            AddNewTabPage(startPage);
            ResumeLayout(true);

            MainThread.CheckFileExistence();

            if (Preferences.ShowWelcomeScreen)
            {
                WelcomeScreen welcomeScreen = new WelcomeScreen();
                welcomeScreen.ShowDialog();
            }
        }
        /// <summary>
        ///     Occurs when the form is just about to close.
        /// <para>
        ///     Any cleanup code will be included here before the program terminates.
        /// </para>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">Event arguments.</param>
        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            Preferences.SaveData();
        }
        private void InitialiseForm(Size size)
        {
            Icon = Properties.Resources.AppIcon;
            Width = size.Width;
            Height = size.Height;
            StartPosition = FormStartPosition.CenterScreen;
            ShowInTaskbar = false;

            #region ****************** Main MenuStrip ******************
            menuStrip = new MenuStrip();
            #endregion

            #region ****************** splitContainer0 (outermost: for menuStrip and region below it) ****************** 
            splitContainer0 = new SplitContainer()
            {
                Orientation = Orientation.Horizontal,
                Parent = this,
                Dock = DockStyle.Fill,
                SplitterDistance = menuStrip.Height,
                IsSplitterFixed = true,
                SplitterWidth = 1
            };
            #endregion

            #region ****************** menuStrip continued ****************** 
            menuStrip.Parent = splitContainer0.Panel1;
            menuStrip.Visible = true;
            menuStrip.Dock = DockStyle.Top;
            #endregion
            #region ****************** toolstripmenuitem Controls ******************
            #region MenuItem File
            menuItemFile = (ToolStripMenuItem)menuStrip.Items.Add("&File");
            menuItemFile.Name = "file";
            menuItemFile_New = (ToolStripMenuItem)menuItemFile.DropDownItems.Add("&New"); menuItemFile_New.Name = "New";
            menuItemFile_NewSermon = (ToolStripMenuItem)menuItemFile_New.DropDownItems.Add("&Sermon");
            menuItemFile_NewSeries = (ToolStripMenuItem)menuItemFile_New.DropDownItems.Add("Se&ries");
            menuItemSeparator = new ToolStripSeparator()
            {
                Name = "separator"
            };
            menuItemFile.DropDownItems.Add(menuItemSeparator);

            menuItemFile_Save = (ToolStripMenuItem)menuItemFile.DropDownItems.Add("&Save");
            menuItemFile_Print = (ToolStripMenuItem)menuItemFile.DropDownItems.Add("&Print");
            menuItemSeparator = new ToolStripSeparator()
            {
                Name = "separator"
            };
            menuItemFile.DropDownItems.Add(menuItemSeparator);

            menuItemFile_Close = (ToolStripMenuItem)menuItemFile.DropDownItems.Add("&Close");
            menuItemFile_CloseAll = (ToolStripMenuItem)menuItemFile.DropDownItems.Add("Close All");
            menuItemFile_Exit = (ToolStripMenuItem)menuItemFile.DropDownItems.Add("&Exit");
            #endregion
            #region MenuItem View
            menuItemView = (ToolStripMenuItem)menuStrip.Items.Add("&View");
            menuItemView_Customise = (ToolStripMenuItem)menuItemView.DropDownItems.Add("&Customise");
            menuItemView_StartPage = (ToolStripMenuItem)menuItemView.DropDownItems.Add("Start page");
            #endregion
            #region MenuItem Tools
            menuItemTools = (ToolStripMenuItem)menuStrip.Items.Add("&Tools");
            menuItemTools_Recover = (ToolStripMenuItem)menuItemTools.DropDownItems.Add("&Recover...");
            menuItemTools_Search = (ToolStripMenuItem)menuItemTools.DropDownItems.Add("&Search");
            menuItemsTools_InitializeDB = (ToolStripMenuItem)menuItemTools.DropDownItems.Add("&Initialize database");
            menuItemsTools_ViewData = (ToolStripMenuItem)menuItemTools.DropDownItems.Add("Data &Overview");
            #endregion
            #region MenuItem Help
            menuItemHelp = (ToolStripMenuItem)menuStrip.Items.Add("&Help");
            menuItemHelp_About = (ToolStripMenuItem)menuItemHelp.DropDownItems.Add("&About...");
            menuItemHelp_Update = (ToolStripMenuItem)menuItemHelp.DropDownItems.Add("&Check for updates");
            #endregion
            #region MenuItem EventHandlers
            menuItemFile_NewSermon.Click += SermonToolStripMenuItem_Click;
            menuItemFile_NewSeries.Click += SeriesToolStripMenuItem_Click;
            menuItemFile_Print.Click += PrintToolStripMenuItem_Click;
            menuItemFile_Close.Click += CloseToolStripMenuItem_Click;
            menuItemFile_CloseAll.Click += CloseallToolStripMenuItem_Click;
            menuItemFile_Exit.Click += ExitToolStripMenuItem_Click;
            menuItemView_Customise.Click += CustomizeToolStripMenuItem_Click;
            menuItemView_StartPage.Click += StartPageToolStripMenuItem_Click;
            menuItemTools_Recover.Click += RecoverToolStripMenuItem_Click;
            menuItemTools_Search.Click += SearchToolStripMenuItem_Click;
            menuItemsTools_InitializeDB.Click += InitializeBDToolStripMenuItem_Click;
            menuItemsTools_ViewData.Click += ViewDataToolStripMenuItem_Click;
            menuItemHelp_About.Click += AboutToolStripMenuItem_Click;
            menuItemHelp_Update.Click += UpdatesToolStripMenuItem_Click;
            #endregion
            menuItemFile_Close.Enabled = false;
            menuItemFile_CloseAll.Enabled = false;
            menuItemsTools_InitializeDB.Enabled = false;
            #endregion

            #region ****************** splitContainer1 (second: for status bar and the region between menuStrip and status bar) ****************** 
            splitContainer1 = new SplitContainer()
            {
                Parent = splitContainer0.Panel2,
                Dock = DockStyle.Fill,
                Orientation = Orientation.Horizontal,
                SplitterDistance = splitContainer0.Bottom,
                IsSplitterFixed = true,
                SplitterWidth = 1
            };
            #endregion

            #region ****************** splitContainer2 (innermost: for central region i.e. treeview and tab control) ****************** 
            splitContainer2 = new SplitContainer()
            {
                Orientation = Orientation.Vertical,
                Parent = splitContainer1.Panel1,
                Dock = DockStyle.Fill,
                SplitterWidth = 1
            };
            splitContainer2.SplitterDistance = splitContainer2.Width / 4;
            #endregion

            #region ****************** statusBar ****************** 
            statusBar = new StatusStrip()
            {
                Parent = splitContainer1.Panel2,
                Dock = DockStyle.Fill
            };
            statusLabelUpdates = new ToolStripStatusLabel()
            {
                Text = "Periodically check for updates e.g. weekly"
            };
            statusLabelShowing = new ToolStripStatusLabel();
            statusLabelAction = new ToolStripStatusLabel()
            {
                Text = "Started"
            };
            Padding padding = new Padding(5, 0, 5, 0);
            Size statusSize = new Size(statusBar.Width / 4, statusBar.Height);
            statusLabelUpdates.Available = statusLabelShowing.Available = statusLabelAction.Available = true;
            statusLabelUpdates.AutoSize = statusLabelShowing.AutoSize = statusLabelAction.AutoSize = false;
            statusLabelUpdates.Owner = statusLabelShowing.Owner = statusLabelAction.Owner = statusBar;
            statusLabelUpdates.Overflow = statusLabelShowing.Overflow = statusLabelAction.Overflow = ToolStripItemOverflow.Never;
            statusLabelAction.BorderStyle = Border3DStyle.Raised;
            statusLabelUpdates.Padding = statusLabelShowing.Padding = statusLabelAction.Padding = padding;
            statusLabelUpdates.TextAlign = statusLabelShowing.TextAlign = statusLabelAction.TextAlign = ContentAlignment.MiddleCenter;

            statusBar.Items.Add(statusLabelUpdates);
            statusBar.Items.Add(statusLabelShowing);
            statusBar.Items.Add(statusLabelAction);
            #endregion

            #region ****************** treeView (for displaying nodes of available docs) ****************** 
            treeviewAvailableDocs = new TreeViewEx()
            {
                Parent = splitContainer2.Panel1,
                Dock = DockStyle.Fill,
                parentForm = this
            };
            #endregion

            #region ****************** tabControl (for displaying open docs for reading and editing) ****************** 
            tabControl = new TabControl()
            {
                Parent = splitContainer2.Panel2,
                Dock = DockStyle.Fill,
                SizeMode = TabSizeMode.Fixed
            };
            tabControl.ControlRemoved += new ControlEventHandler(TabControl_ControlRemoved);
            tabControl.ControlAdded += new ControlEventHandler(TabControl_ControlAdded);
            tabControl.DoubleClick += new EventHandler(TabControl_DoubleClicked);
            tabControl.SelectedIndexChanged += new EventHandler(TabControl_SelectedIndexChanged);
            #endregion
        }
        public void UpdateControlSizes()
        {
            try
            {
                statusLabelUpdates.Size = statusLabelAction.Size = new Size(statusBar.Width / 4, statusBar.Height);
                statusLabelShowing.Size = new Size(statusBar.Width / 5 * 2, statusBar.Height);
            }
            catch
            {

            }
        }
        public void UpdateControlColorsFonts(Control control, Color backColor, Color foreColor, Font font)
        {
            try
            {
                string type = control.GetType().ToString();
                if (control is TabPage form)
                {
                    control.BackColor = backColor;
                    control.ForeColor = foreColor;
                    control.Font = font;
                    if (control.Name == "STARTPAGE")
                    {
                        StartPageForm startPage = (StartPageForm)form.Controls.Find("STARTPAGE", true)[0];
                        startPage.UpdateColours(backColor, foreColor);
                    }
                    return;
                }
                else if (type == "System.Windows.Forms.StatusStrip")
                {
                    float emSize = 10.25F;
                    //emSize = Font.SizeInPoints;
                    Font statusBarFont = new Font(font.FontFamily, emSize, GraphicsUnit.Pixel);
                    control.BackColor = backColor;
                    control.ForeColor = foreColor;
                    control.Font = statusBarFont;
                }
                else if (type == "System.Windows.Forms.MenuStrip")
                {
                    MenuStrip menustrip = (MenuStrip)control;
                    menustrip.BackColor = backColor;
                    menustrip.ForeColor = foreColor;
                    menustrip.Font = font;
                    foreach (ToolStripMenuItem menuitem in menustrip.Items)
                    {
                        for (int count = 0; count < menuitem.DropDownItems.Count; count++)
                        {
                            type = menuitem.DropDownItems[count].GetType().ToString();
                            if (type != "System.Windows.Forms.ToolStripSeparator")
                            {
                                UpdateControlColorsFonts_MenuItems((ToolStripMenuItem)menuitem.DropDownItems[count], backColor, foreColor, font);
                            }
                            else
                            {
                                menuitem.DropDownItems[count].BackColor = DefaultBackColor;
                                menuitem.DropDownItems[count].ForeColor = backColor;
                            }
                        }
                    }
                }
                else
                {
                    control.BackColor = backColor;
                    control.ForeColor = foreColor;
                    control.Font = font;
                }

                foreach (Control subControl in control.Controls)
                {
                    subControl.BackColor = backColor;
                    subControl.ForeColor = foreColor;
                    subControl.Font = font;
                    UpdateControlColorsFonts(subControl, backColor, foreColor, font);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }
        public void UpdateControlColorsFonts_MenuItems(ToolStripMenuItem control, Color backColor, Color foreColor, Font font)
        {
            string type = control.GetType().ToString();

            if (type != "System.Windows.Forms.ToolStripSeparator")
            {
                control.BackColor = backColor;
                control.ForeColor = foreColor;
                control.Font = font;

                foreach (ToolStripMenuItem item in control.DropDownItems)
                {
                    type = item.GetType().ToString();
                    if (type != "System.Windows.Forms.ToolStripSeparator")
                    {
                        item.BackColor = backColor;
                        item.ForeColor = foreColor;
                        item.Font = font;
                        foreach (ToolStripMenuItem subitem in item.DropDownItems)
                        {
                            UpdateControlColorsFonts_MenuItems(subitem, backColor, foreColor, font);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Occurs when the Close MenuItem is clicked.
        /// It closes the currently selected TabPage in the TabControl.
        /// </summary>
        /// <param name="sender">Control that initiated the event.</param>
        /// <param name="e">Event arguments.</param>
        private void CloseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab == null)
            {
                MessageBox.Show("There is nothing to close");
            }
            else
            {
                tabControl.TabPages.Remove(tabControl.SelectedTab);
            }
        }
        /// <summary>
        /// Occurs when the Close All MenuItem is clicked.
        /// It closes all TabPages in the TabControl.
        /// </summary>
        /// <param name="sender">Control that initiated the event.</param>
        /// <param name="e">Event arguments.</param>
        private void CloseallToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tabControl.TabCount != 0)
            {
                tabControl.TabPages.Clear();
            }
            else
            {
                MessageBox.Show("There is nothing to close");
            }
        }
        /// <summary>
        /// Occurs when the Start page MenuItem is clicked. It opens the start page.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartPageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (TabPage tp in tabControl.TabPages)
            {
                if (tp.Name == "StartPageForm")//exists: switch to that TabPage
                {
                    tabControl.SelectTab(tp);
                    return;
                }
            }
            //start page tab does not exist; create it
            startPage = new StartPageForm(this);
            AddNewTabPage(startPage);
        }
        /// <summary>
        /// Occurs when the Customize MenuItem is clicked.
        /// </summary>
        /// <param name="sender">Control that initiated the event.</param>
        /// <param name="e">Event arguments.</param>
        private void CustomizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PreferencesForm prefForm = new PreferencesForm(this, true);
        }
        /// <summary>
        /// Occurs when the Exit MenuItem is clicked.
        /// It closes the ParentForm, exiting the application.
        /// </summary>
        /// <param name="sender">Control that initiated the event.</param>
        /// <param name="e">Event arguments.</param>
        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
        /// <summary>
        /// Occurs when the Sermon MenuItem is clicked.
        /// </summary>
        /// <param name="sender">Control that initiated the event.</param>
        /// <param name="e">Event arguments.</param>
        public void SermonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SermonViewNew.CreateNewSermon(this);
        }
        /// <summary>
        /// Occurs when the Series MenuItem is clicked.
        /// </summary>
        /// <param name="sender">Control that initiated the event.</param>
        /// <param name="e">Event arguments.</param>
        public void SeriesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Series series = new Series();
            series.ShowDialog();
        }
        /// <summary>
        /// Occurs when the Search MenuItem is clicked.
        /// </summary>
        /// <param name="sender">Control that initiated the event.</param>
        /// <param name="e">Event arguments.</param>
        private void SearchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SearchDlgBox searchDlgBox = new SearchDlgBox(this);
        }
        /// <summary>
        /// Occurs when the About MenuItem is clicked.
        /// </summary>
        /// <param name="sender">Control that initiated the event.</param>
        /// <param name="e">Event arguments.</param>
        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                using (AboutDlgBox formAboutDlgBox = new AboutDlgBox())
                {
                    formAboutDlgBox.ShowDialog();
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }
        /// <summary>
        /// Occurs when the Recover MenuItem is clicked.
        /// </summary>
        /// <param name="sender">Control that initiated the event.</param>
        /// <param name="e">Event arguments.</param>
        private void RecoverToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RecoverSermons recoverSermons = new RecoverSermons();
            try
            {
                treeviewAvailableDocs.SortTreeView(TreeViewEx.FILTER);
            }
            catch
            {
                treeviewAvailableDocs.SortTreeView("Speaker");
            }
        }
        /// <summary>
        /// Occurs when the Print MenuItem is clicked.
        /// </summary>
        /// <param name="sender">Control that initiated the event.</param>
        /// <param name="e">Event arguments.</param>
        private void PrintToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string DocumentID = tabControl.SelectedTab.Name;
            MyPrintDialog printDialog = new MyPrintDialog(this, DocumentID);
        }
        /// <summary>
        /// Occurs when the Initialize Database MenuItem is clicked.
        /// </summary>
        /// <param name="sender">Control that initiated the event.</param>
        /// <param name="e">Event arguments.</param>
        private void InitializeBDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (DialogResult.Yes == MessageBox.Show("This is a diagnostic procedure that will wipe everything on your database including preferences and stored documents. Only use it if you suspect that your database has been corrupted. Proceed with caution.\n[NB: This action is not be undone.]", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning))
                {
                    if (DialogResult.Yes == MessageBox.Show("Are you sure?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning))
                    {
                        if (MainThread.ConfirmDBIntegrity())
                        {
                            MessageBox.Show("Done");
                        }
                    }
                }
            }
            catch
            {

            }
        }
        private void ViewDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new SermonDataGrid().ShowDialog();
        }
        private void UpdatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StatusBarMessages.SetStatusBarMessageUpdates("Getting latest version");
            UpdaterClass updater = new UpdaterClass();
            StatusBarMessages.SetStatusBarMessageUpdates("Periodically check for updates e.g.weekly");
        }
        /// <summary>
        /// Occurs when a control is successfully added to the TabControl
        /// </summary>
        /// <param name="sender">The control that initiated the event.</param>
        /// <param name="e">ControlEvent arguments.</param>
        private void TabControl_ControlAdded(object sender, ControlEventArgs e)
        {
            StatusBarMessages.SetStatusBarMessageShowing(tabControl.SelectedTab.Text);
            if (e.Control is TabPage tabPage)
            {
                foreach(var c in tabPage.Name)
                {
                    if (!char.IsDigit(c))
                    {
                        return;
                    }
                }
                menuItemFile_Close.Enabled = true;
                menuItemFile_CloseAll.Enabled = true;
                menuItemFile_Print.Enabled = true;
                RecentlyOpenedDocs.AddNewNode(int.Parse(tabPage.Name), tabPage.Text);
            }
        }
        /// <summary>
        /// Occurs when a control is successfully removed from the TabControl.
        /// </summary>
        /// <param name="sender">The control that initiated the event.</param>
        /// <param name="e">Event Arguments.</param>
        private void TabControl_ControlRemoved(object sender, ControlEventArgs e)
        {
            if (e.Control is TabPage)
            {
                tabControl.TabPages.Remove((TabPage)e.Control);
            }
            if (tabControl.TabCount == 0)
            {
                menuItemFile_Close.Enabled = false;
                menuItemFile_CloseAll.Enabled = false;
                menuItemFile_Print.Enabled = false;
            }
        }
        private void TabControl_DoubleClicked(object sender, EventArgs e)
        {
            tabControl.Controls.RemoveAt(tabControl.Controls.IndexOf(tabControl.SelectedTab));
        }
        private void TabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl.TabCount != 0)
            {
                StatusBarMessages.SetStatusBarMessageShowing(tabControl.SelectedTab.Text);
            }
            else
            {
                StatusBarMessages.SetStatusBarMessageShowing(string.Empty);
            }
        }
        /// <summary>
        /// Creates and/or selects a TabPage displaying a form.
        /// </summary>
        /// <param name="formNewTabPage">Form to be added to the TabControl via a TabPage.</param>
        public void AddNewTabPage(Form formNewTabPage)
        {
            TabPage newTabPage = null;

            /* Check for the existence of the TabPage containing the form
             * to prevent multiple display of the same form
             */
            foreach (TabPage tp in tabControl.TabPages)
            {
                if (tp.Name == formNewTabPage.Name)//exists: switch to that TabPage
                {
                    newTabPage = tp;
                    tabControl.SelectTab(newTabPage);
                    return;
                }
            }
            if (newTabPage == null)//doesn't exist: create new TabPage and attach the form
            {
                newTabPage = new TabPage();

                formNewTabPage.TopLevel = false;
                formNewTabPage.Parent = newTabPage;
                formNewTabPage.Visible = true;

                if ((newTabPage.Text = formNewTabPage.Text) == null)
                {
                    newTabPage.Text = "New Form";
                }
                newTabPage.Name = formNewTabPage.Name;

                tabControl.TabPages.Add(newTabPage);
                tabControl.SelectTab(newTabPage);
            }
        }
    }
}