using AppEngine;
using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AppUI
{
    public class StartPageForm : Form
    {
        private ParentForm parentForm;
        private StartPageContentForm spContentForm;
        private StartPageWalkthroughForm spWalkthroughForm;
        private StartPageWhatsNewForm spWhatsNewForm;

        override public Color BackColor
        {
            get
            {
                return parentForm.ControlColour;
            }
        }
        override public Color ForeColor
        {
            get
            {
                return parentForm.ColourFont;
            }
        }
        override public Font Font
        {
            get
            {
                return parentForm.SystemFont;
            }
        }

        private SplitContainer splitContainer0;
        private SplitContainer splitContainer1;
        private Button btnContent, btnWalkThrough, btnWhatsNew;

        public StartPageForm(ParentForm parent)
        {
            parentForm = parent;
            Size size = new Size(parent.tabControl.ClientSize.Width, parent.tabControl.ClientSize.Height);
            InitialiseStartPage(size, parent);
            
            parent.SizeChanged += delegate
            {
                //SuspendLayout();
                Size parentSize = Parent.Size;
                SetControlSizesAndPositions(parentSize);
                //ResumeLayout();
            };
        }
        public void UpdateColours(Color control, Color fore)
        {
            splitContainer0.Panel1.BackColor = splitContainer0.Panel2.BackColor = control;
            splitContainer1.Panel1.BackColor = splitContainer1.Panel2.BackColor = control;
            splitContainer0.Panel1.ForeColor = splitContainer0.Panel2.ForeColor = fore;
            splitContainer1.Panel1.ForeColor = splitContainer1.Panel2.ForeColor = fore;

            btnContent.BackColor = btnWalkThrough.BackColor = btnWhatsNew.BackColor = control;
            btnContent.ForeColor = btnWalkThrough.ForeColor = btnWhatsNew.ForeColor = fore;
        }
        public void InitialiseStartPage(Size size, ParentForm parentForm)
        {
            FormBorderStyle = FormBorderStyle.None;
            Size = size;
            ShowInTaskbar = false;
            ShowIcon = false;
            TopMost = false;
            Visible = false;
            Text = "Start Page";
            Location = new Point(0, 0);
            Name = "STARTPAGE";
            
            splitContainer0 = new SplitContainer()
            {
                Parent = this,
                Dock = DockStyle.Fill,
                Orientation = Orientation.Horizontal,
                IsSplitterFixed = true,
                SplitterWidth = 1
            };
            splitContainer1 = new SplitContainer()
            {
                Parent = splitContainer0.Panel2,
                Dock = DockStyle.Fill,
                Orientation = Orientation.Horizontal,
                IsSplitterFixed = true,
                SplitterWidth = 1
            };

            SetControlSizesAndPositions(Size);

            InitialiseStartPage_TopPanel(splitContainer0.Panel1);
            InitialiseStartPage_MiddlePanel(splitContainer1.Panel1, parentForm);
            InitialiseStartPage_BottomPanel(splitContainer1.Panel2);
        }
        private void InitialiseStartPage_TopPanel(SplitterPanel parent)
        {
            parent.BackColor = BackColor;
            parent.ForeColor = ForeColor;

            int iDefBtnWidth = parent.Width / 5, iDefBtnHeight = parent.Height - 10;
            btnContent = new Button()
            {
                Text = "MyApp",
                Parent = parent,
                Size = new Size(iDefBtnWidth, iDefBtnHeight),
                Left = iDefBtnWidth,
                BackColor = BackColor,
                ForeColor = ForeColor
            };
            btnWalkThrough = new Button()
            {
                Text = "Walkthrough",
                Parent = parent,
                Size = new Size(iDefBtnWidth, iDefBtnHeight),
                Left = btnContent.Right,
                BackColor = BackColor,
                ForeColor = ForeColor
            };
            btnWhatsNew = new Button()
            {
                Text = "What's New",
                Parent = parent,
                Size = new Size(iDefBtnWidth, iDefBtnHeight),
                Left = btnWalkThrough.Right,
                BackColor = BackColor,
                ForeColor = ForeColor
            };
            btnContent.Click += BtnContent_Click;
            btnWalkThrough.Click += BtnWalkThrough_Click;
            btnWhatsNew.Click += BtnWhatsNew_Click;

            btnContent.FlatStyle = FlatStyle.Standard;
            btnWalkThrough.FlatStyle = FlatStyle.Flat;
            btnWhatsNew.FlatStyle = FlatStyle.Flat;

            parent.SizeChanged += delegate
            {
                iDefBtnWidth = parent.Width / 5;
                iDefBtnHeight = iDefBtnWidth * 2 / 3;
                btnContent.Size = btnWalkThrough.Size = btnWhatsNew.Size = new Size(iDefBtnWidth, iDefBtnHeight);
                btnContent.Left = iDefBtnWidth;
                btnWalkThrough.Left = btnContent.Right;
                btnWhatsNew.Left = btnWalkThrough.Right;
            };
        }

        private void BtnWhatsNew_Click(object sender, EventArgs e)
        {
            spContentForm.Visible = false;
            spWalkthroughForm.Visible = false;

            spWhatsNewForm.Visible = true;
            spWhatsNewForm.BringToFront();

            btnWhatsNew.FlatStyle = FlatStyle.Standard;
            btnContent.FlatStyle = FlatStyle.Flat;
            btnWalkThrough.FlatStyle = FlatStyle.Flat;
        }
        private void BtnWalkThrough_Click(object sender, EventArgs e)
        {
            spContentForm.Visible = false;
            spWhatsNewForm.Visible = false;

            spWalkthroughForm.Visible = true;
            spWalkthroughForm.BringToFront();

            btnWhatsNew.FlatStyle = FlatStyle.Flat;
            btnContent.FlatStyle = FlatStyle.Flat;
            btnWalkThrough.FlatStyle = FlatStyle.Standard;
        }
        private void BtnContent_Click(object sender, EventArgs e)
        {
            spWalkthroughForm.Visible = false;
            spWhatsNewForm.Visible = false;

            spContentForm.Visible = true;
            spContentForm.BringToFront();

            btnWhatsNew.FlatStyle = FlatStyle.Flat;
            btnContent.FlatStyle = FlatStyle.Standard;
            btnWalkThrough.FlatStyle = FlatStyle.Flat;
        }

        private void InitialiseStartPage_MiddlePanel(SplitterPanel parent, ParentForm parentForm)
        {
            parent.BackColor = Color.Silver;

            spContentForm = new StartPageContentForm(parent, parentForm);
            spWalkthroughForm = new StartPageWalkthroughForm(parent);
            spWhatsNewForm = new StartPageWhatsNewForm(parent);

            spContentForm.Visible = true;
            spContentForm.BringToFront();
            parent.SizeChanged += delegate
            {
                spContentForm.Size = new Size(parent.Width, parent.Height);
            };
        }
        private void InitialiseStartPage_BottomPanel(SplitterPanel parent)
        {
            parent.BackColor = BackColor;
        }

        private void SetControlSizesAndPositions(Size size)
        {
            Size = size;
            splitContainer0.SplitterDistance = (size.Width / 5) * 2 / 3;
            splitContainer1.SplitterDistance = splitContainer0.Panel2.Height * 2 / 3;
        }
    }
    internal class StartPageContentForm : Form
    {
        ParentForm parentForm;

        public StartPageContentForm(SplitterPanel parent, ParentForm formParent)
        {
            parentForm = formParent;

            FormBorderStyle = FormBorderStyle.None;
            TopLevel = false;
            Parent = parent;
            Dock = DockStyle.Fill;
            Visible = false;

            SplitContainer splitContainer0 = new SplitContainer()
            {
                Parent = this,
                Dock = DockStyle.Fill,
                Orientation = Orientation.Vertical,
                IsSplitterFixed = true,
                SplitterWidth = 1,
                SplitterDistance = Width / 3
            };
            GroupBox gbx1Start = new GroupBox()
            {
                Text = "Start",
                Parent = splitContainer0.Panel1,
                Dock = DockStyle.Fill
            };
            GroupBox gbx2RecentsPanel = new GroupBox()
            {
                Text = "Recent",
                Parent = splitContainer0.Panel2,
                Dock = DockStyle.Fill
            };
            TreeView tvStart = new TreeView()
            {
                Parent = gbx1Start,
                Dock = DockStyle.Fill,
                BorderStyle = BorderStyle.None
            };
            TreeView tvRecent = new TreeView()
            {
                Parent = gbx2RecentsPanel,
                Dock = DockStyle.Fill,
                BorderStyle = BorderStyle.None
            };

            tvStart.NodeMouseDoubleClick += new TreeNodeMouseClickEventHandler(StartTreeNode_DoubleClick);
            tvRecent.NodeMouseDoubleClick += new TreeNodeMouseClickEventHandler(RecentsTreeNode_DoubleClick);

            try { RecentlyOpenedDocs RODs = new RecentlyOpenedDocs(); }
            catch {; }

            AddTreeNodesToTreeViewStart(tvStart);
            AddTreeNodesToTreeViewRecent(tvRecent);
        }
        private void AddTreeNodesToTreeViewStart(TreeView treeView)
        {
            treeView.Nodes.Add("Sermon/Bible study");
            treeView.Nodes.Add("Series");
        }
        private void AddTreeNodesToTreeViewRecent(TreeView treeView)
        {
            var rods = new AppEngine.Database.RODs().SelectAll();
            try
            {
                for (int i = (rods.Count - 1); i >= 0; i--)
                {
                    if ((rods.Count - i) > RecentlyOpenedDocs.MaxNumber)
                    {
                        return;
                    }
                    TreeNode treenode = new TreeNode()
                    {
                        Name = rods[i].Id.ToString(),
                        Text = rods[i].Title
                    };
                    //do not show the start page nor a new document writer
                    foreach(var c in treenode.Name)
                    {
                        if (!char.IsDigit(c))
                        {
                            continue;
                        }
                    }
                    treeView.Nodes.Add(treenode);
                }
            }
            catch
            {
                MessageBox.Show("Couldn't display recently opened documents.");
            }
        }

        private void StartTreeNode_DoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                switch (e.Node.Text.ToUpper())
                {
                    case "SERMON/BIBLE STUDY":
                        SermonViewNew.CreateNewSermon(parentForm);
                        break;
                    case "SERIES":
                        Series series = new Series();
                        series.ShowDialog();
                        break;
                }
            }
        }
        private void RecentsTreeNode_DoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                string[] arraySermonComponents = RecentlyOpenedDocs.OpenSermonFromID(int.Parse(e.Node.Name));
                if (arraySermonComponents == null)
                {
                    return;
                }
                else
                {
                    Form displayForm = SermonReader.DisplayStoredSermon(arraySermonComponents);
                    parentForm.AddNewTabPage(displayForm);
                }
            }
        }
    }
    internal class StartPageWalkthroughForm : Form
    {
        private Form holder = new Form();
        private RichTextBox rtb = new RichTextBox();
        delegate void myDelegate(string rtf);

        public StartPageWalkthroughForm(SplitterPanel parent)
        {
            FormBorderStyle = FormBorderStyle.None;
            TopLevel = false;
            Parent = parent;
            Dock = DockStyle.Fill;
            Visible = false;

            GroupBox grpBox = new GroupBox()
            {
                Parent = this,
                Dock = DockStyle.Fill,
                Text = "How to's"
            };
            FlowLayoutPanel panel = new FlowLayoutPanel()
            {
                Parent = grpBox,
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                Width = parent.ClientRectangle.Width,
                WrapContents = false
            };
            RetrieveWalkthroughFiles(panel);
        }
        private void RetrieveWalkthroughFiles(Panel panel)
        {
            if (Directory.Exists(FileNames.WalkthroughsDirectory))
            {
                string[] files = Directory.GetFiles(FileNames.WalkthroughsDirectory);
                char delimiter1 = Path.DirectorySeparatorChar, delimiter2 = Path.AltDirectorySeparatorChar;

                int howToIndex = 1;
                foreach (string file in files)
                {
                    if (file.ToLower().EndsWith(".rtf"))
                    {
                        LinkLabel link = new LinkLabel()
                        {
                            Text = "How to #" + howToIndex + ": " + Path.GetFileNameWithoutExtension(file),
                            Name = Path.GetFileNameWithoutExtension(file),
                            ForeColor = ColorExtractor.ExtractColor("222233")
                        };
                        int linkStart = link.Text.IndexOf(":") + 2;
                        LinkLabel.Link labelLink = new LinkLabel.Link(linkStart, link.Text.Length - linkStart);
                        link.Links.Add(labelLink);
                        link.LinkClicked += delegate
                        {
                            holder = new Form()
                            {
                                ShowInTaskbar = true,
                                ShowIcon = false,
                                FormBorderStyle = FormBorderStyle.FixedToolWindow,
                                StartPosition = FormStartPosition.CenterScreen,
                                Size = new Size(800, 500),
                                Text = link.Text,
                                Visible = true
                            };
                            rtb = new RichTextBox()
                            {
                                Parent = holder,
                                Dock = DockStyle.Fill,
                                ReadOnly = true
                            };
                            rtb.GotFocus += delegate
                            {
                                using (Button btn = new Button())
                                {
                                    btn.Focus();
                                }
                            };

                            string fileName = FileNames.WalkthroughsDirectory + link.Name + ".rtf";
                            if (File.Exists(fileName))
                            {
                                GetWalkthroughText(fileName);
                                holder.Show();
                            }
                        };
                        link.Width = 800;
                        panel.Controls.Add(link);
                    }
                    howToIndex++;
                }
            }
            else
            {

            }
        }
        private async void GetWalkthroughText(string fileName)
        {
            try
            {
                using (StreamReader reader = File.OpenText(fileName))
                {
                    Task<string> getRtf = reader.ReadToEndAsync();

                    string rtf = await getRtf;

                    myDelegate delegate1 = new myDelegate(UpdateUI);
                    Invoke(delegate1, rtf);
                }
            }
            catch
            {
                MessageBox.Show("Error detected 1");
            }
        }

        private void UpdateUI(string line)
        {
            try
            {
                rtb.Rtf = line;
            }
            catch
            {
                MessageBox.Show("Error detected");
            }
        }
    }
    internal class StartPageWhatsNewForm : Form
    {
        public StartPageWhatsNewForm(SplitterPanel parent)
        {
            FormBorderStyle = FormBorderStyle.None;
            TopLevel = false;
            Parent = parent;
            Dock = DockStyle.Fill;
            Visible = false;
            GroupBox grpBoxWhatsNew = new GroupBox()
            {
                Text = "In this version...",
                Parent = this,
                Dock = DockStyle.Fill
            };
            TextBox lblVersion = new TextBox()
            {
                Multiline = true,
                Text = Properties.Resources.InThisVersion,
                Parent = grpBoxWhatsNew,
                Dock = DockStyle.Fill,
                BorderStyle = BorderStyle.None,
                ReadOnly = true
            };
        }
    }
}