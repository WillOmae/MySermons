using AppEngine;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace AppUI
{
    public class TreeViewEx : TreeView
    {
        const char PAGE_BREAK = '\f';
        public ParentForm parentForm;
        static public string FILTER
        {
            get
            {
                return Preferences.SortingFilter;
            }
            set
            {
                Preferences.SortingFilter = value;
            }
        }
        static private MenuItem miOpen, miDelete, miPrint, miSortBy, miRefresh, miBatch, miOpenAll, miPrintAll, miDeleteAll, miExpandAll, miCollapseAll;

        public TreeViewEx()
        {
            NodeMouseDoubleClick += new TreeNodeMouseClickEventHandler(NodeDoubleClick);

            MouseUp += new MouseEventHandler(RightClickPopUpMenu);
            FullRowSelect = true;
            Sort();
            BorderStyle = BorderStyle.None;
            Visible = true;

            CreateRightClickContextMenu();
        }
        
        public void AddNewTreeNode(string szTitle, string ID)
        {
            bool doesParentNodeExist = false;
            bool doesChildNodeExist = false;
            
            foreach (TreeNode parentNode in Nodes)
            {
                foreach (TreeNode childNode in parentNode.Nodes)
                {
                    if (childNode.Name == ID)
                    {
                        doesChildNodeExist = true;
                    }
                }
            }
            if (!doesChildNodeExist)
            {
                string newParent = Sermon.GetSermonComponent(int.Parse(ID), FILTER);
                foreach (TreeNode parentNode in Nodes)
                {
                    if (newParent == parentNode.Text)
                    {
                        doesParentNodeExist = true;

                        string szChildNode = Sermon.GetSermonComponent(int.Parse(ID), "Title");
                        TreeNode tvnodeChild = new TreeNode(szChildNode)
                        {
                            Name = ID
                        };
                        parentNode.Nodes.Add(tvnodeChild);
                        break;
                    }//Else create a new ParentNode
                }
                if (!doesParentNodeExist)
                {
                    TreeNode tvnodeParentNew = new TreeNode(Sermon.GetSermonComponent(int.Parse(ID), FILTER));
                    TreeNode tvnodeChildNew = new TreeNode(Sermon.GetSermonComponent(int.Parse(ID), "Title"))
                    {
                        Name = ID
                    };
                    Nodes.Add(tvnodeParentNew);
                    tvnodeParentNew.Nodes.Add(tvnodeChildNew);
                }
            }
            else
            {

            }
            SortTreeView(FILTER);
        }
        private void DeleteTreeNode(TreeNode treeNodeSelected)
        {
            try
            {
                if (treeNodeSelected.Name == "PARENTNODE")//the selected node is a parentNode
                {
                    if (MessageBox.Show("Do you want to delete this parent node [" + treeNodeSelected.Text + "]" +
                        " and all its children?\n(NB: This action cannot be undone.)",
                        "Warning!", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        foreach (TreeNode child in treeNodeSelected.Nodes)//close tabpage if possible
                        {
                            CloseTabPage(child.Name);
                            RecentlyOpenedDocs.DeleteSermonFromID(int.Parse(child.Name));
                            Sermon.DeleteSermon(int.Parse(child.Name));
                        }
                        if (int.TryParse(treeNodeSelected.Name, out int id))
                        {
                            CloseTabPage(treeNodeSelected.Name);
                            RecentlyOpenedDocs.DeleteSermonFromID(int.Parse(treeNodeSelected.Name));
                            Sermon.DeleteSermon(int.Parse(treeNodeSelected.Name));
                        }
                        treeNodeSelected.Remove();
                    }
                }
                else//i.e. the selected node is not a parent node
                {
                    TreeNode parentNode = treeNodeSelected.Parent;

                    if (treeNodeSelected.Name != null)
                    {
                        if (MessageBox.Show("Do you want to delete this node [" + treeNodeSelected.Text + "]" +
                            "?\n(NB: This action cannot be undone.)",
                            "Warning!", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            CloseTabPage(treeNodeSelected.Name);//close tabpage if possible
                            treeNodeSelected.Remove();
                            RecentlyOpenedDocs.DeleteSermonFromID(int.Parse(treeNodeSelected.Name));
                            Sermon.DeleteSermon(int.Parse(treeNodeSelected.Name));
                            StatusBarMessages.SetStatusBarMessageAction("Deleted " + treeNodeSelected.Text);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Could not access node data.\nPlease try again.", "Failed to delete!");
                    }
                    if (parentNode.GetNodeCount(false) == 0)
                    {
                        parentNode.Remove();
                    }
                }
            }
            catch
            {
                MessageBox.Show("Kindly specify the node to be deleted.");
            }
        }
        private void CloseTabPage(string idRemoved)
        {
            try
            {
                foreach (TabPage tp in parentForm.tabControl.TabPages)
                {
                    if (tp.Name == idRemoved)
                    {
                        parentForm.tabControl.TabPages.Remove(tp);
                    }
                }
            }
            catch
            {
                return;
            }
        }
        public void DisplayTreeNodeItem(TreeNode treeNodeSelected)
        {
            Form sermonReader = SermonReader.DisplayStoredSermon(Sermon.GetSermonComponents(int.Parse(treeNodeSelected.Name)));

            parentForm.AddNewTabPage(sermonReader);
        }
        private void CreateRightClickContextMenu()
        {
            ContextMenu = new ContextMenu();
            ContextMenu.Popup += new EventHandler(ContextMenu_Popup);


            MenuItem menuitemSpeaker = new MenuItem("Speaker");
            menuitemSpeaker.Name = menuitemSpeaker.Text;
            menuitemSpeaker.Click += new EventHandler(MenuitemSpeaker_Click);

            MenuItem menuitemVenue = new MenuItem("Venue");
            menuitemVenue.Name = menuitemVenue.Text;
            menuitemVenue.Click += new EventHandler(MenuitemVenue_Click);

            MenuItem menuitemYear = new MenuItem("Year");
            menuitemYear.Name = menuitemYear.Text;
            menuitemYear.Click += new EventHandler(MenuitemYear_Click);

            MenuItem menuitemSeries = new MenuItem("Series");
            menuitemSeries.Name = menuitemSeries.Text;
            menuitemSeries.Click += new EventHandler(MenuitemSeries_Click);

            MenuItem menuitemTheme = new MenuItem("Theme");
            menuitemTheme.Name = menuitemTheme.Text;
            menuitemTheme.Click += new EventHandler(MenuitemTheme_Click);

            miOpen = ContextMenu.MenuItems.Add("&Open");
            miOpen.Click += new EventHandler(MenuItemOpen_Click);

            miDelete = ContextMenu.MenuItems.Add("&Delete");
            miDelete.Click += new EventHandler(MenuItemDelete_Click);

            miPrint = ContextMenu.MenuItems.Add("&Print");
            miPrint.Click += new EventHandler(MenuItemPrint_Click);

            ContextMenu.MenuItems.Add("-");

            miSortBy = ContextMenu.MenuItems.Add("&Sort by");
            miSortBy.MenuItems.Add(menuitemSpeaker);
            miSortBy.MenuItems.Add(menuitemVenue);
            miSortBy.MenuItems.Add(menuitemYear);
            miSortBy.MenuItems.Add(menuitemSeries);
            miSortBy.MenuItems.Add(menuitemTheme);

            ContextMenu.MenuItems.Add("-");

            miRefresh = ContextMenu.MenuItems.Add("&Refresh");
            miRefresh.Click += new EventHandler(MenuItemRefresh_Click);

            ContextMenu.MenuItems.Add("-");

            miBatch = ContextMenu.MenuItems.Add("Batch actions");

            miOpenAll = miBatch.MenuItems.Add("Open all");
            miOpenAll.Click += new EventHandler(MenuItemOpenAll_Click);

            miPrintAll = miBatch.MenuItems.Add("Print all");
            miPrintAll.Click += new EventHandler(MenuItemPrintAll_Click);

            miBatch.MenuItems.Add("-");

            miExpandAll = miBatch.MenuItems.Add("Expand all");
            miExpandAll.Click += new EventHandler(MenuItemExpandAll_Click);
            miCollapseAll = miBatch.MenuItems.Add("Collapse all");
            miCollapseAll.Click += new EventHandler(MenuItemCollapseAll_Click);

            miBatch.MenuItems.Add("-");

            miDeleteAll = miBatch.MenuItems.Add("Delete all");
            miDeleteAll.Click += new EventHandler(MenuItemDeleteAll_Click);
        }
        private void RegulateContextMenuItems()
        {
            miBatch.Enabled = miDelete.Enabled = miOpen.Enabled = miDeleteAll.Enabled = miOpenAll.Enabled = miPrint.Enabled = miPrintAll.Enabled = miSortBy.Enabled = true;
            if (Nodes.Count < 1)
            {
                miBatch.Enabled = miDelete.Enabled = miOpen.Enabled = miDeleteAll.Enabled = miOpenAll.Enabled = miPrint.Enabled = miPrintAll.Enabled = miSortBy.Enabled = miExpandAll.Enabled = miCollapseAll.Enabled = false;
            }
            if (SelectedNode != null && SelectedNode.Nodes.Count != 0)
            {
                miOpen.Enabled = miPrint.Enabled = false;
            }
        }
        public void SortTreeView(string filter)
        {
            try
            {
                BeginUpdate();
                Nodes.Clear();
                List<AppEngine.Database.Sermon> list = new AppEngine.Database.Sermon().SelectAllCondensed();

                string[] parentNodes = Sermon.GetParentNodes(filter, list);

                for (int i = 0; i < parentNodes.Length; i++)
                {
                    if (parentNodes[i] == null)
                    {
                        break;
                    }
                    TreeNode parentNode = new TreeNode()
                    {
                        Text = parentNodes[i],
                        Name = "PARENTNODE"
                    };
                    bool nodeExists = false;
                    foreach (TreeNode node in Nodes)
                    {
                        if (node.Text == parentNode.Text)
                        {
                            nodeExists = true;
                        }
                    }
                    if (nodeExists == false)
                    {
                        Nodes.Add(parentNode);
                    }
                }

                foreach (TreeNode parentNode in Nodes)
                {
                    string[,] childNodes = Sermon.GetChildNodes(filter, parentNode.Text, list);
                    for (int i = 0; i < (childNodes.Length / 2); i++)
                    {
                        if (childNodes[0, i] == null)
                        {
                            break;
                        }
                        TreeNode childNode = new TreeNode()
                        {
                            Text = childNodes[0, i],
                            Name = childNodes[1, i]
                        };
                        if (!parentNode.Nodes.Contains(childNode))
                        {
                            parentNode.Nodes.Add(childNode);
                        }
                        else
                        { MessageBox.Show("This child node already exists: " + childNode.Text); }
                    }
                }
                EndUpdate();
            }
            catch
            {
                EndUpdate();
            }
        }
        private void NodeDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                TreeNode treeNodeSelected = GetNodeAt(new Point(e.X, e.Y));
                //Prevent the sending of null values to the DisplayTreeNodeItem since a ParentNode cannot be viewed
                if (treeNodeSelected.Name != "PARENTNODE")
                {
                    DisplayTreeNodeItem(treeNodeSelected);
                }
            }
        }
        private void RightClickPopUpMenu(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.Clicks == 1)
            {
                ContextMenu.Show(this, new Point(e.X, e.Y));
            }
        }
        private void ContextMenu_Popup(object sender, EventArgs e)
        {
            RegulateContextMenuItems();
        }
        private void MenuItemOpen_Click(object sender, EventArgs e)
        {
            DisplayTreeNodeItem(SelectedNode);
        }
        private void MenuItemDelete_Click(object sender, EventArgs e)
        {
            DeleteTreeNode(SelectedNode);
        }
        private void MenuItemPrint_Click(object sender, EventArgs e)
        {
            if (SelectedNode.GetNodeCount(false) == 0)
            {
                if (int.TryParse(SelectedNode.Name, out int documentId))
                {
                    MyPrintDialog myPrintDialog = new MyPrintDialog(parentForm, documentId);
                }
                else
                {
                    MessageBox.Show("Failed to print the selected document." +
                        "\n\nFailed to resolve the document ID.");
                }
            }
        }
        private void MenuItemRefresh_Click(object sender, EventArgs e)
        {
            if (FILTER != null)
            {
                SortTreeView(FILTER);
                StatusBarMessages.SetStatusBarMessageAction("Refreshed treeview");
            }
        }
        private void MenuItemDeleteAll_Click(object sender, EventArgs e)
        {
            DialogResult userChoice = MessageBox.Show("This option is going to delete every record in your library. Do you wish to continue?\n(NB: This action cannot be undone.)", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (userChoice == DialogResult.Yes)
            {
                try
                {
                    foreach (TreeNode tnParentNode in Nodes)
                    {
                        foreach (TreeNode tnChildNode in tnParentNode.Nodes)
                        {
                            try
                            {
                                string szRemoved = tnChildNode.Name;
                                CloseTabPage(szRemoved);//close tabpage if possible
                                Sermon.DeleteSermon(int.Parse(szRemoved));
                                RecentlyOpenedDocs.DeleteSermonFromID(int.Parse(szRemoved));
                            }
                            catch {; }
                        }
                    }
                    Nodes.Clear();
                    Sort();
                    if (Nodes.Count != 0)
                    {
                        MessageBox.Show("An error was encountered in deleting some nodes. Please try again.");
                        StatusBarMessages.SetStatusBarMessageAction("Partial deletion");
                    }
                    else
                    {
                        StatusBarMessages.SetStatusBarMessageAction("Deleted all stored sermons");
                    }
                }
                catch {; }
            }
        }
        private void MenuItemOpenAll_Click(object sender, EventArgs e)
        {
            DialogResult userChoice = MessageBox.Show("If many files are to be opened, your PC will be taxed. Do you wish to continue?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (userChoice == DialogResult.Yes)
            {
                try
                {
                    foreach (TreeNode tnParentNodes in Nodes)
                    {
                        foreach (TreeNode tnChildNodes in tnParentNodes.Nodes)
                        {
                            try
                            {
                                DisplayTreeNodeItem(tnChildNodes);
                            }
                            catch
                            {
                                MessageBox.Show("An error was encountered in opening [" + tnChildNodes.Text + "].");
                            }
                        }
                    }
                }
                catch {; }
            }
        }
        private void MenuItemPrintAll_Click(object sender, EventArgs e)
        {
            DialogResult userChoice = MessageBox.Show("If many files are to be printed, your PC will be taxed. Do you wish to continue?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (userChoice == DialogResult.Yes)
            {
                try
                {
                    RichTextBoxEx rtbSource = new RichTextBoxEx();
                    foreach (TreeNode tnParentNode in Nodes)
                    {
                        foreach (TreeNode tnChildNode in tnParentNode.Nodes)
                        {
                            try
                            {
                                string rtf = SermonReader.DisplayStoredSermon(Sermon.GetSermonComponents(int.Parse(tnChildNode.Name))).RTFpublic;
                                try
                                {
                                    if (rtbSource.Rtf.ToLower().Contains("rtf"))
                                    {
                                        rtf = AppendRTF(rtbSource.Rtf, rtf);
                                    }
                                }
                                catch { }

                                rtbSource.Rtf = rtf;

                                rtbSource.Text += PAGE_BREAK;
                            }
                            catch
                            {
                                MessageBox.Show("An error was encountered in opening [" + tnChildNode.Text + "].");
                            }
                        }
                    }
                    MyPrintDialog myPrintDialog = new MyPrintDialog(parentForm, rtbSource);
                }
                catch {; }
            }
        }
        private void MenuItemExpandAll_Click(object sender, EventArgs e)
        {
            ExpandAll();
        }
        private void MenuItemCollapseAll_Click(object sender, EventArgs e)
        {
            CollapseAll();
        }
        private void MenuitemSpeaker_Click(object sender, EventArgs e)
        {
            if (FILTER != "Speaker")
            {
                FILTER = "Speaker";
                SortTreeView(FILTER);
                StatusBarMessages.SetStatusBarMessageAction("Sorted by Speaker");
            }
        }
        private void MenuitemVenue_Click(object sender, EventArgs e)
        {
            if (FILTER != "Venue")
            {
                FILTER = "Venue";
                SortTreeView(FILTER);
                StatusBarMessages.SetStatusBarMessageAction("Sorted by Venue");
            }
        }
        private void MenuitemYear_Click(object sender, EventArgs e)
        {
            if (FILTER != "Year")
            {
                FILTER = "Year";
                SortTreeView(FILTER);
                StatusBarMessages.SetStatusBarMessageAction("Sorted by Year");
            }
        }
        private void MenuitemSeries_Click(object sender,EventArgs e)
        {
            if (FILTER != "Series")
            {
                FILTER = "Series";
                SortTreeView(FILTER);
                StatusBarMessages.SetStatusBarMessageAction("Sorted by Series");
            }
        }
        private void MenuitemTheme_Click(object sender, EventArgs e)
        {
            if (FILTER != "Theme")
            {
                FILTER = "Theme";
                SortTreeView(FILTER);
                StatusBarMessages.SetStatusBarMessageAction("Sorted by Theme");
            }
        }
        public string AppendRTF(string oldRTF, string newRTF)
        {
            string RTF = oldRTF;
            string RTFNew1 = newRTF.Remove(0, newRTF.IndexOf(";}") + 2);
            RTFNew1 = RTFNew1.Remove(RTFNew1.IndexOf(";}}") + 3);

            string RTFNew2 = newRTF.Remove(0, newRTF.IndexOf(";}}") + 3);

            string RTFOld1 = oldRTF.Remove(oldRTF.IndexOf("}}") + 1);

            string RTFOld2 = oldRTF.Replace(RTFOld1, "");
            RTFOld2 = RTFOld2.Remove(0, 1);
            RTFOld2 = RTFOld2.Remove(RTFOld2.LastIndexOf("}"));

            RTF = RTFOld1 + RTFNew1 + RTFOld2 + RTFNew2;
            return RTF;
        }
    }
}