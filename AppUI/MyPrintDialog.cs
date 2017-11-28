using AppEngine;
using System;
using System.Drawing.Printing;
using System.Windows.Forms;

/* MyPrintDialog : Form
 * The objective of this class is to encapulate all the data and methods that pertain to printing of a document.
 * It inherits from a form since it has a GUI
 * 
 * Important areas in printing:
 *      -print preview
 *      -printers
 *      -the actual printing process
 *
*/
namespace AppUI
{

    public class MyPrintDialog : Form
    {
        private ParentForm parentForm;
        private PrintDocument printDoc;
        private string szDocName;

        private RichTextBoxEx rtbPrintContent = new RichTextBoxEx();
        private int checkPrint = 0;

        public PrintDocument PrintDocumentPublicAccess
        {
            get
            {
                return printDoc;
            }
        }

        public string PreferredPrinter = Preferences.PrinterName;
        private string[] namesofprinters;
        #region CONTROLS
        private GroupBox groupBox1;
        private Button btnPrinterProperties;
        private Label label1;
        private ComboBox cmbxPrinter;
        private GroupBox groupBox2;
        private Label label3;
        private Label label2;
        private TextBox txbRange_To;
        private TextBox txbRange_From;
        private RadioButton radbtnRange;
        private RadioButton radbtnAll;
        private RadioButton radbtnCurrent;
        private GroupBox groupBox3;
        private NumericUpDown numUDNumberofCopies;
        private CheckBox chbxRemoveAnnotations;
        private CheckBox chbxBnW;
        private CheckBox chbxRotation;
        private Label label4;
        private Button btnPagePreview;
        private Button btnPrint;
        private Button btnAbort;
        #endregion

        public MyPrintDialog(ParentForm parent)
        {
            if (GetPrintText())
            {
                CommonConstructor(parent, Visible);
            }
        }
        public MyPrintDialog(ParentForm parent, int DocumentID)
        {
            if (GetPrintText(DocumentID))
            {
                CommonConstructor(parent, Visible);
            }
        }
        public MyPrintDialog(ParentForm parent, RichTextBoxEx rtbSource)
        {
            if (GetPrintText(rtbSource))
            {
                CommonConstructor(parent, Visible);
            }
        }
        private void CommonConstructor(ParentForm parent, bool visible)
        {
            parentForm = parent;
            InitializeComponent();
            SetRequiredValues();

            cmbxPrinter.Items.AddRange(namesofprinters);
            cmbxPrinter.SelectedItem = PreferredPrinter;
            cmbxPrinter.Text = PreferredPrinter;

            StartPosition = FormStartPosition.CenterScreen;
            ShowDialog();
        }
        private void SetRequiredValues()
        {
            printDoc = new PrintDocument();

            printDoc.BeginPrint += new PrintEventHandler(BeginPrint);
            printDoc.PrintPage += new PrintPageEventHandler(PrintPage);

            namesofprinters = new string[PrinterSettings.InstalledPrinters.Count];
            PrinterSettings.InstalledPrinters.CopyTo(namesofprinters, 0);
        }
        private void ExpressPrinting()
        {
            printDoc.PrinterSettings.PrinterName = PreferredPrinter;
            printDoc.PrinterSettings.PrintRange = PrintRange.AllPages;
            printDoc.Print();
        }

        private void InitializeComponent()
        {
            groupBox1 = new GroupBox();
            btnPrinterProperties = new Button();
            label1 = new Label();
            cmbxPrinter = new ComboBox();
            groupBox2 = new GroupBox();
            label3 = new Label();
            label2 = new Label();
            txbRange_To = new TextBox();
            txbRange_From = new TextBox();
            radbtnRange = new RadioButton();
            radbtnAll = new RadioButton();
            radbtnCurrent = new RadioButton();
            groupBox3 = new GroupBox();
            numUDNumberofCopies = new NumericUpDown();
            chbxRemoveAnnotations = new CheckBox();
            chbxBnW = new CheckBox();
            chbxRotation = new CheckBox();
            label4 = new Label();
            btnPagePreview = new Button();
            btnPrint = new Button();
            btnAbort = new Button();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(numUDNumberofCopies)).BeginInit();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(btnPrinterProperties);
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(cmbxPrinter);
            groupBox1.Location = new System.Drawing.Point(12, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new System.Drawing.Size(460, 64);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Choose Printer...";
            // 
            // btnPrinterProperties
            // 
            btnPrinterProperties.Location = new System.Drawing.Point(337, 36);
            btnPrinterProperties.Name = "btnPrinterProperties";
            btnPrinterProperties.Size = new System.Drawing.Size(117, 23);
            btnPrinterProperties.TabIndex = 2;
            btnPrinterProperties.Text = "Properties";
            btnPrinterProperties.UseVisualStyleBackColor = true;
            btnPrinterProperties.Click += new System.EventHandler(PrinterProperties_Click);
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(6, 16);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(35, 13);
            label1.TabIndex = 1;
            label1.Text = "Name";
            // 
            // cmbxPrinter
            // 
            cmbxPrinter.FormattingEnabled = true;
            cmbxPrinter.Location = new System.Drawing.Point(6, 37);
            cmbxPrinter.Name = "cmbxPrinter";
            cmbxPrinter.Size = new System.Drawing.Size(325, 21);
            cmbxPrinter.TabIndex = 0;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(label3);
            groupBox2.Controls.Add(label2);
            groupBox2.Controls.Add(txbRange_To);
            groupBox2.Controls.Add(txbRange_From);
            groupBox2.Controls.Add(radbtnRange);
            groupBox2.Controls.Add(radbtnAll);
            groupBox2.Controls.Add(radbtnCurrent);
            groupBox2.Location = new System.Drawing.Point(12, 82);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new System.Drawing.Size(153, 132);
            groupBox2.TabIndex = 1;
            groupBox2.TabStop = false;
            groupBox2.Text = "Print Range";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(74, 87);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(20, 13);
            label3.TabIndex = 6;
            label3.Text = "To";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(7, 87);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(30, 13);
            label2.TabIndex = 5;
            label2.Text = "From";
            // 
            // txbRange_To
            // 
            txbRange_To.Location = new System.Drawing.Point(74, 106);
            txbRange_To.Name = "txbRange_To";
            txbRange_To.Size = new System.Drawing.Size(62, 20);
            txbRange_To.TabIndex = 4;
            // 
            // txbRange_From
            // 
            txbRange_From.Location = new System.Drawing.Point(6, 106);
            txbRange_From.Name = "txbRange_From";
            txbRange_From.Size = new System.Drawing.Size(62, 20);
            txbRange_From.TabIndex = 3;
            // 
            // radbtnRange
            // 
            radbtnRange.AutoSize = true;
            radbtnRange.Checked = false;
            radbtnRange.Location = new System.Drawing.Point(9, 68);
            radbtnRange.Name = "radbtnRange";
            radbtnRange.Size = new System.Drawing.Size(64, 17);
            radbtnRange.TabIndex = 2;
            radbtnRange.Text = "In range";
            radbtnRange.UseVisualStyleBackColor = true;
            // 
            // radbtnAll
            // 
            radbtnAll.AutoSize = true;
            radbtnAll.Checked = true;
            radbtnAll.Location = new System.Drawing.Point(9, 44);
            radbtnAll.Name = "radbtnAll";
            radbtnAll.Size = new System.Drawing.Size(68, 17);
            radbtnAll.TabIndex = 1;
            radbtnAll.Text = "All pages";
            radbtnAll.UseVisualStyleBackColor = true;
            // 
            // radbtnCurrent
            // 
            radbtnCurrent.AutoSize = true;
            radbtnCurrent.Checked = false;
            radbtnCurrent.Location = new System.Drawing.Point(9, 20);
            radbtnCurrent.Name = "radbtnCurrent";
            radbtnCurrent.Size = new System.Drawing.Size(86, 17);
            radbtnCurrent.TabIndex = 0;
            radbtnCurrent.TabStop = true;
            radbtnCurrent.Text = "Current page";
            radbtnCurrent.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(numUDNumberofCopies);
            groupBox3.Controls.Add(chbxRemoveAnnotations);
            groupBox3.Controls.Add(chbxBnW);
            groupBox3.Controls.Add(chbxRotation);
            groupBox3.Controls.Add(label4);
            groupBox3.Location = new System.Drawing.Point(171, 82);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new System.Drawing.Size(301, 100);
            groupBox3.TabIndex = 2;
            groupBox3.TabStop = false;
            groupBox3.Text = "Settings";
            // 
            // numUDNumberofCopies
            // 
            numUDNumberofCopies.Location = new System.Drawing.Point(144, 16);
            numUDNumberofCopies.Name = "numUDNumberofCopies";
            numUDNumberofCopies.Size = new System.Drawing.Size(56, 20);
            numUDNumberofCopies.TabIndex = 4;
            numUDNumberofCopies.Value = 0;
            // 
            // chbxRemoveAnnotations
            // 
            chbxRemoveAnnotations.AutoSize = true;
            chbxRemoveAnnotations.Location = new System.Drawing.Point(10, 83);
            chbxRemoveAnnotations.Name = "chbxRemoveAnnotations";
            chbxRemoveAnnotations.Size = new System.Drawing.Size(124, 17);
            chbxRemoveAnnotations.TabIndex = 3;
            chbxRemoveAnnotations.Text = "Remove annotations";
            chbxRemoveAnnotations.UseVisualStyleBackColor = true;
            // 
            // chbxBnW
            // 
            chbxBnW.AutoSize = true;
            chbxBnW.Location = new System.Drawing.Point(10, 61);
            chbxBnW.Name = "chbxBnW";
            chbxBnW.Size = new System.Drawing.Size(105, 17);
            chbxBnW.TabIndex = 2;
            chbxBnW.Text = "Black and White";
            chbxBnW.UseVisualStyleBackColor = true;
            // 
            // chbxRotation
            // 
            chbxRotation.AutoSize = true;
            chbxRotation.Location = new System.Drawing.Point(10, 37);
            chbxRotation.Name = "chbxRotation";
            chbxRotation.Size = new System.Drawing.Size(128, 17);
            chbxRotation.TabIndex = 1;
            chbxRotation.Text = "Rotate by 90 degrees";
            chbxRotation.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(7, 20);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(90, 13);
            label4.TabIndex = 0;
            label4.Text = "Number of copies";
            // 
            // btnPagePreview
            // 
            btnPagePreview.Location = new System.Drawing.Point(181, 191);
            btnPagePreview.Name = "btnPagePreview";
            btnPagePreview.Size = new System.Drawing.Size(105, 23);
            btnPagePreview.TabIndex = 3;
            btnPagePreview.Text = "Page Preview...";
            btnPagePreview.UseVisualStyleBackColor = true;
            btnPagePreview.Click += new System.EventHandler(PagePreview_Click);
            // 
            // btnPrint
            // 
            btnPrint.Location = new System.Drawing.Point(301, 191);
            btnPrint.Name = "btnPrint";
            btnPrint.Size = new System.Drawing.Size(75, 23);
            btnPrint.TabIndex = 4;
            btnPrint.Text = "Print";
            btnPrint.UseVisualStyleBackColor = true;
            btnPrint.Click += new System.EventHandler(Print_Click);
            // 
            // btnAbort
            // 
            btnAbort.Location = new System.Drawing.Point(391, 191);
            btnAbort.Name = "btnAbort";
            btnAbort.Size = new System.Drawing.Size(75, 23);
            btnAbort.TabIndex = 5;
            btnAbort.Text = "Abort";
            btnAbort.UseVisualStyleBackColor = true;
            btnAbort.Click += new System.EventHandler(Abort_Click);
            // 
            // MyPrintDialog
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(484, 225);
            Controls.Add(btnAbort);
            Controls.Add(btnPrint);
            Controls.Add(btnPagePreview);
            Controls.Add(groupBox3);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "MyPrintDialog";
            ShowIcon = false;
            ShowInTaskbar = false;
            SizeGripStyle = SizeGripStyle.Hide;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Print Document";
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(numUDNumberofCopies)).EndInit();
            ResumeLayout(false);

        }
        /// <summary>
        /// Handles the PrintDocument.BeginPrint event that occurs when the Print method is called and before the first page of the document prints.
        /// </summary>
        private void BeginPrint(object sender, PrintEventArgs e)
        {
            checkPrint = 0;
        }
        /// <summary>
        /// Handles the PrintDocument.PrintPage event that occurs when the output to print for the current page is needed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PrintPage(object sender, PrintPageEventArgs e)
        {
            // Print the content of RichTextBox. Store the last character printed.
            checkPrint = rtbPrintContent.Print(checkPrint, rtbPrintContent.TextLength, e);

            // Check for more pages
            if (checkPrint < rtbPrintContent.TextLength)
                e.HasMorePages = true;
            else
                e.HasMorePages = false;
        }
        private void PagePreview_Click(object sender, EventArgs e)
        {
            //printDoc.PrintPage += new PrintPageEventHandler(PrintActuator);
            PrintPreviewDialog previewDlg = new PrintPreviewDialog();

            checkPrint = 0;

            previewDlg.Document = printDoc;
            previewDlg.ShowDialog();
        }
        private void Print_Click(object sender, EventArgs e)
        {
            printDoc.PrinterSettings.PrinterName = cmbxPrinter.Text;
            if (printDoc.PrinterSettings.IsValid)
            {
                try
                {
                    printDoc.PrinterSettings.PrintFileName = szDocName;//check this: there's bound to be a bug here (disallowed characters in filename)
                    printDoc.DocumentName = szDocName;
                }
                catch
                {
                    printDoc.PrinterSettings.PrintFileName = "PrintFileName";
                    printDoc.DocumentName = "PrintFileName";
                }
                printDoc.PrinterSettings.PrintRange = PrintRange.AllPages;

                StatusBarMessages.SetStatusBarMessageAction("Printing...");
                printDoc.Print();
                StatusBarMessages.SetStatusBarMessageAction("Done printing");
                Close();
            }
            else
            {
                MessageBox.Show("Printer does not exist");
                return;
            }
        }
        private void Abort_Click(object sender, EventArgs e)
        {
            Close();
            StatusBarMessages.SetStatusBarMessageAction("Printing cancelled");
        }
        private void PrinterProperties_Click(object sender, EventArgs e)
        {

        }
        private bool GetPrintText()
        {
            if (parentForm.tabControl.SelectedTab != null)
            {
                foreach (Control control in parentForm.tabControl.SelectedTab.Controls)
                {
                    if (control is Form)
                    {
                        if ((szDocName = control.Text) == "")
                        {
                            szDocName = "Default Text";
                        }
                        if (control.Text.ToLower() == "start page")
                        {
                            MessageBox.Show("Can't print the start page");
                        }
                        else
                        {
                            foreach (Control subcontrol in control.Controls)
                            {
                                switch (subcontrol.GetType().Name)
                                {
                                    case "TextBox":
                                    case "RichTextBox":
                                    case "RichTextBoxEx":
                                        RichTextBoxEx tb = (RichTextBoxEx)subcontrol;
                                        rtbPrintContent = tb;
                                        return true;
                                }
                            }
                        }
                        MessageBox.Show(szDocName);
                    }
                }
            }
            return false;
        }
        private bool GetPrintText(int DocumentID)
        {
            try
            {
                rtbPrintContent = new RichTextBoxEx()
                {
                    Rtf = SermonReader.DisplayStoredSermon(Sermon.GetSermonComponents(DocumentID)).RTFpublic
                };
                return true;
            }
            catch
            {
                return false;
            }
        }
        private bool GetPrintText(RichTextBoxEx rtbSource)
        {
            rtbPrintContent = rtbSource;
            return true;
        }
        private PrintRange GetPrintRange()
        {
            PrintRange printRange = new PrintRange();

            if (radbtnAll.Checked == true)
            {
                printRange = PrintRange.AllPages;
            }
            else if (radbtnCurrent.Checked == true)
            {
                printRange = PrintRange.CurrentPage;
            }
            else if (radbtnRange.Checked == true)
            {
                printDoc.PrinterSettings.FromPage = Convert.ToInt32(txbRange_From.Text);
                printDoc.PrinterSettings.ToPage = Convert.ToInt32(txbRange_To.Text);
                printRange = PrintRange.SomePages;
            }

            return printRange;
        }
    }
}
