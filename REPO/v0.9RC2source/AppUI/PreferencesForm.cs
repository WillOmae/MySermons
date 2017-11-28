using AppEngine;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace AppUI
{
    public class PreferencesForm : Form
    {
        private GroupBox groupBox1;
        private ComboBox cmbxPrinterName;
        private ComboBox cmbxPrinterScheme;
        private Label label2;
        private Label label1;
        private GroupBox groupBox2;
        private Label label3;
        private Button btnPrefSave;
        private Button btnPrefClose;
        private Button btnPrefDefaults;
        private GroupBox groupBox3;
        private Label label6;
        private ComboBox cmbxFilters;
        private NumericUpDown nudRODMax;
        private Label label4;
        private GroupBox groupBox4;
        private Label label9;
        private Label label8;
        private Label label7;
        private Label label5;
        private Label label11;
        private Label label10;
        private ColorComboBox cmbxThemeColourFont;
        private ColorComboBox cmbxThemeColourControls;
        private FontComboBox cmbxThemeFontSystem;
        private FontComboBox cmbxThemeFontWriter;
        private FontComboBox cmbxThemeFontReader;
        /*Private variables*/
        private bool isUnsaved = false, isFromInitializeControls = false;
        private FontFamily[] InstalledFonts = new System.Drawing.Text.InstalledFontCollection().Families;
        private string[] AvailableFonts;
        private List<string> PossibleFonts = new List<string> { "Times New Roman", "Verdana", "Vivaldi Italic", "Trajan Pro", "Segoe Print", "Segoe UI", "SansSerif", "Romantic", "Old English Text MT Regular", "Nueva Std", "Kristen ITC Regular", "Elephant", "Complex", "Comic Sans MS", "Arial", "Bodoni MT" };
        private Color[] PossibleColorsControls = new Color[] { ColorExtractor.ExtractColor("222233"), Color.Maroon, Color.Black, Color.SteelBlue, Color.LightSteelBlue };
        private Color[] PossibleColorsFonts = new Color[] { ColorExtractor.ExtractColor("AACCFF"), Color.White, Color.Black, Color.Teal, Color.LightSteelBlue };
        /*Public properties*/
        public string PrinterName
        {
            get
            {
                return cmbxPrinterName.Text;
            }
            set
            {
                cmbxPrinterName.Text = value;
            }
        }
        public string PrinterScheme
        {
            get
            {
                return cmbxPrinterScheme.Text;
            }
            set
            {
                cmbxPrinterScheme.Text = value;
            }
        }
        public string ColourFont
        {
            get
            {
                return ColorExtractor.ToParseableString(cmbxThemeColourFont.BackColor);
            }
            set
            {
                cmbxThemeColourFont.BackColor = ColorExtractor.ExtractColor(value);
            }
        }
        public string ColourControls
        {
            get
            {
                return ColorExtractor.ToParseableString(cmbxThemeColourControls.BackColor);
            }
            set
            {
                cmbxThemeColourControls.BackColor = ColorExtractor.ExtractColor(value);
            }
        }
        public string FontSystem
        {
            get
            {
                return cmbxThemeFontSystem.Text;
            }
            set
            {
                cmbxThemeFontSystem.Text = value;
            }
        }
        public string FontReader
        {
            get
            {
                return cmbxThemeFontReader.Text;
            }
            set
            {
                cmbxThemeFontReader.Text = value;
            }
        }
        public string FontWriter
        {
            get
            {
                return cmbxThemeFontWriter.Text;
            }
            set
            {
                cmbxThemeFontWriter.Text = value;
            }
        }
        public int ROD_MaxNumber
        {
            get
            {
                return (int)nudRODMax.Value;
            }
            set
            {
                nudRODMax.Value = value;
            }
        }
        public string SortingFilter
        {
            get
            {
                return cmbxFilters.Text;
            }
            set
            {
                cmbxFilters.Text = value;
            }
        }
        public bool ignoreValues = true;
        private Preferences prefs = new Preferences();
        private ParentForm parentForm = null;

        public PreferencesForm(bool show)
        {
            CommonConstructor(show);
        }
        public PreferencesForm(ParentForm par, bool show)
        {
            parentForm = par;
            CommonConstructor(show);
        }
        private void CommonConstructor(bool show)
        {
            if (show)
            {
                isFromInitializeControls = true;
                AvailableFonts = GetAvailableFonts();
                InitializeComponent();

                StartPosition = FormStartPosition.CenterScreen;

                PrinterName = Preferences.PrinterName;
                PrinterScheme = Preferences.PrinterScheme;
                ColourFont = Preferences.ColourFont;
                ColourControls = Preferences.ColourControls;

                cmbxThemeColourControls.BackColor = ColorExtractor.ExtractColor(Preferences.ColourControls);
                cmbxThemeColourFont.BackColor = ColorExtractor.ExtractColor(Preferences.ColourFont);
                cmbxThemeFontReader.SelectedIndex = cmbxThemeFontReader.FindString(FontReader);
                cmbxThemeFontReader.SelectedIndex = cmbxThemeFontReader.FindString(FontReader);
                cmbxThemeFontSystem.SelectedIndex = cmbxThemeFontSystem.FindString(FontSystem);
                cmbxThemeFontWriter.SelectedIndex = cmbxThemeFontWriter.FindString(FontWriter);

                FontSystem = Preferences.FontSystem;
                FontReader = Preferences.FontReader;
                FontWriter = Preferences.FontWriter;
                SortingFilter = Preferences.SortingFilter;
                ROD_MaxNumber = Preferences.ROD_MaxNumber;

                SetValues();
                isFromInitializeControls = false;
                
                FormClosing += new FormClosingEventHandler(PrefsFormClosing);
                ShowDialog();
            }
        }
        private void InitializeComponent()
        {
            groupBox1 = new GroupBox();
            cmbxPrinterName = new ComboBox();
            cmbxPrinterScheme = new ComboBox();
            label2 = new Label();
            label1 = new Label();
            groupBox2 = new GroupBox();
            cmbxThemeColourFont = new ColorComboBox();
            cmbxThemeColourControls = new ColorComboBox();
            label11 = new Label();
            label10 = new Label();
            label9 = new Label();
            label8 = new Label();
            label7 = new Label();
            label5 = new Label();
            label3 = new Label();
            btnPrefSave = new Button();
            btnPrefClose = new Button();
            btnPrefDefaults = new Button();
            groupBox3 = new GroupBox();
            cmbxFilters = new ComboBox();
            label6 = new Label();
            label4 = new Label();
            nudRODMax = new NumericUpDown();
            groupBox4 = new GroupBox();
            cmbxThemeFontSystem = new FontComboBox();
            cmbxThemeFontReader = new FontComboBox();
            cmbxThemeFontWriter = new FontComboBox();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(nudRODMax)).BeginInit();
            groupBox4.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(cmbxPrinterName);
            groupBox1.Controls.Add(cmbxPrinterScheme);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(label1);
            groupBox1.Location = new Point(245, 123);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(229, 76);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Printing";
            // 
            // cmbxPrinterName
            // 
            cmbxPrinterName.FormattingEnabled = true;
            cmbxPrinterName.Location = new Point(86, 19);
            cmbxPrinterName.Name = "cmbxPrinterName";
            cmbxPrinterName.Size = new Size(134, 21);
            cmbxPrinterName.TabIndex = 3;
            cmbxPrinterName.SelectedIndexChanged += new EventHandler(cmbxPrinterName_SelectedIndexChanged);
            // 
            // cmbxPrinterScheme
            // 
            cmbxPrinterScheme.FormattingEnabled = true;
            cmbxPrinterScheme.Location = new Point(86, 46);
            cmbxPrinterScheme.Name = "cmbxPrinterScheme";
            cmbxPrinterScheme.Size = new Size(134, 21);
            cmbxPrinterScheme.TabIndex = 2;
            cmbxPrinterScheme.SelectedIndexChanged += new EventHandler(cmbxPrinterScheme_SelectedIndexChanged);
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(6, 50);
            label2.Name = "label2";
            label2.Size = new Size(74, 13);
            label2.TabIndex = 1;
            label2.Text = "Color scheme:";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(6, 23);
            label1.Name = "label1";
            label1.Size = new Size(40, 13);
            label1.TabIndex = 1;
            label1.Text = "Printer:";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(cmbxThemeFontWriter);
            groupBox2.Controls.Add(cmbxThemeFontReader);
            groupBox2.Controls.Add(cmbxThemeFontSystem);
            groupBox2.Controls.Add(cmbxThemeColourFont);
            groupBox2.Controls.Add(cmbxThemeColourControls);
            groupBox2.Controls.Add(label11);
            groupBox2.Controls.Add(label10);
            groupBox2.Controls.Add(label9);
            groupBox2.Controls.Add(label8);
            groupBox2.Controls.Add(label7);
            groupBox2.Controls.Add(label5);
            groupBox2.Controls.Add(label3);
            groupBox2.Location = new Point(12, 13);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(227, 209);
            groupBox2.TabIndex = 1;
            groupBox2.TabStop = false;
            groupBox2.Text = "Theme";
            // 
            // cmbxThemeColourFont
            // 
            cmbxThemeColourFont.DrawMode = DrawMode.OwnerDrawFixed;
            cmbxThemeColourFont.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbxThemeColourFont.FormattingEnabled = true;
            cmbxThemeColourFont.Location = new Point(86, 73);
            cmbxThemeColourFont.Name = "cmbxThemeColourFont";
            cmbxThemeColourFont.Size = new Size(135, 21);
            cmbxThemeColourFont.TabIndex = 12;
            // 
            // cmbxThemeColourControls
            // 
            cmbxThemeColourControls.DrawMode = DrawMode.OwnerDrawFixed;
            cmbxThemeColourControls.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbxThemeColourControls.FormattingEnabled = true;
            cmbxThemeColourControls.Location = new Point(86, 46);
            cmbxThemeColourControls.Name = "cmbxThemeColourControls";
            cmbxThemeColourControls.Size = new Size(135, 21);
            cmbxThemeColourControls.TabIndex = 7;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(17, 50);
            label11.Name = "label11";
            label11.Size = new Size(48, 13);
            label11.TabIndex = 7;
            label11.Text = "Controls:";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(17, 77);
            label10.Name = "label10";
            label10.Size = new Size(31, 13);
            label10.TabIndex = 6;
            label10.Text = "Font:";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(17, 131);
            label9.Name = "label9";
            label9.Size = new Size(44, 13);
            label9.TabIndex = 5;
            label9.Text = "System:";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(17, 185);
            label8.Name = "label8";
            label8.Size = new Size(38, 13);
            label8.TabIndex = 4;
            label8.Text = "Writer:";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(17, 158);
            label7.Name = "label7";
            label7.Size = new Size(45, 13);
            label7.TabIndex = 3;
            label7.Text = "Reader:";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(6, 104);
            label5.Name = "label5";
            label5.Size = new Size(31, 13);
            label5.TabIndex = 2;
            label5.Text = "Font:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(6, 23);
            label3.Name = "label3";
            label3.Size = new Size(40, 13);
            label3.TabIndex = 0;
            label3.Text = "Colour:";
            // 
            // btnPrefSave
            // 
            btnPrefSave.FlatStyle = FlatStyle.Popup;
            btnPrefSave.Location = new Point(131, 233);
            btnPrefSave.Name = "btnPrefSave";
            btnPrefSave.Size = new Size(65, 23);
            btnPrefSave.TabIndex = 2;
            btnPrefSave.Text = "Save";
            btnPrefSave.UseVisualStyleBackColor = true;
            btnPrefSave.Click += new EventHandler(btnPrefSave_Click);
            // 
            // btnPrefClose
            // 
            btnPrefClose.FlatStyle = FlatStyle.Popup;
            btnPrefClose.Location = new Point(295, 233);
            btnPrefClose.Name = "btnPrefClose";
            btnPrefClose.Size = new Size(65, 23);
            btnPrefClose.TabIndex = 4;
            btnPrefClose.Text = "Cancel";
            btnPrefClose.UseVisualStyleBackColor = true;
            btnPrefClose.Click += new EventHandler(btnPrefClose_Click);
            // 
            // btnPrefDefaults
            // 
            btnPrefDefaults.FlatStyle = FlatStyle.Popup;
            btnPrefDefaults.Location = new Point(214, 233);
            btnPrefDefaults.Name = "btnPrefDefaults";
            btnPrefDefaults.Size = new Size(65, 23);
            btnPrefDefaults.TabIndex = 3;
            btnPrefDefaults.Text = "Defaults";
            btnPrefDefaults.UseVisualStyleBackColor = true;
            btnPrefDefaults.Click += new EventHandler(btnPrefDefaults_Click);
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(cmbxFilters);
            groupBox3.Controls.Add(label6);
            groupBox3.Location = new Point(245, 13);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(229, 50);
            groupBox3.TabIndex = 5;
            groupBox3.TabStop = false;
            groupBox3.Text = "Documents";
            // 
            // cmbxFilters
            // 
            cmbxFilters.FormattingEnabled = true;
            cmbxFilters.Items.AddRange(new object[] {
            "Speaker",
            "Venue",
            "Year"});
            cmbxFilters.Location = new Point(86, 19);
            cmbxFilters.Name = "cmbxFilters";
            cmbxFilters.Size = new Size(134, 21);
            cmbxFilters.TabIndex = 3;
            cmbxFilters.SelectedIndexChanged += new EventHandler(cmbxFilters_SelectedIndexChanged);
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(6, 23);
            label6.Name = "label6";
            label6.Size = new Size(43, 13);
            label6.TabIndex = 1;
            label6.Text = "Sort by:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(6, 23);
            label4.Name = "label4";
            label4.Size = new Size(86, 13);
            label4.TabIndex = 5;
            label4.Text = "Number of items:";
            // 
            // nudRODMax
            // 
            nudRODMax.Location = new Point(98, 19);
            nudRODMax.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            nudRODMax.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            nudRODMax.Name = "nudRODMax";
            nudRODMax.Size = new Size(59, 20);
            nudRODMax.TabIndex = 4;
            nudRODMax.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            nudRODMax.ValueChanged += new EventHandler(nudRODMax_ValueChanged);
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(label4);
            groupBox4.Controls.Add(nudRODMax);
            groupBox4.Location = new Point(245, 69);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new Size(229, 48);
            groupBox4.TabIndex = 6;
            groupBox4.TabStop = false;
            groupBox4.Text = "Recents panel";
            // 
            // cmbxThemeFontSystem
            // 
            cmbxThemeFontSystem.DrawMode = DrawMode.OwnerDrawFixed;
            cmbxThemeFontSystem.FormattingEnabled = true;
            cmbxThemeFontSystem.Location = new Point(86, 127);
            cmbxThemeFontSystem.Name = "cmbxThemeFontSystem";
            cmbxThemeFontSystem.Size = new Size(135, 21);
            cmbxThemeFontSystem.TabIndex = 16;
            // 
            // cmbxThemeFontReader
            // 
            cmbxThemeFontReader.DrawMode = DrawMode.OwnerDrawFixed;
            cmbxThemeFontReader.FormattingEnabled = true;
            cmbxThemeFontReader.Location = new Point(86, 154);
            cmbxThemeFontReader.Name = "cmbxThemeFontReader";
            cmbxThemeFontReader.Size = new Size(135, 21);
            cmbxThemeFontReader.TabIndex = 17;
            // 
            // cmbxThemeFontWriter
            // 
            cmbxThemeFontWriter.DrawMode = DrawMode.OwnerDrawFixed;
            cmbxThemeFontWriter.FormattingEnabled = true;
            cmbxThemeFontWriter.Location = new Point(86, 181);
            cmbxThemeFontWriter.Name = "cmbxThemeFontWriter";
            cmbxThemeFontWriter.Size = new Size(135, 21);
            cmbxThemeFontWriter.TabIndex = 18;
            // 
            // PreferencesForm
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(484, 266);
            ControlBox = false;
            Controls.Add(groupBox4);
            Controls.Add(groupBox3);
            Controls.Add(btnPrefClose);
            Controls.Add(btnPrefDefaults);
            Controls.Add(btnPrefSave);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "PreferencesForm";
            Text = "Preferences";
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(nudRODMax)).EndInit();
            groupBox4.ResumeLayout(false);
            groupBox4.PerformLayout();
            ResumeLayout(false);

        }
        /// <summary>
        /// Set values to the various controls.
        /// </summary>
        private void SetValues()
        {
            SetPrintingValues();
            SetThemeValues();
            SetDocumentValues();
        }
        /// <summary>
        /// Set values to controls in the printing GroupBox.
        /// </summary>
        private void SetPrintingValues()
        {
            string[] InstalledPrinters = new string[PrinterSettings.InstalledPrinters.Count];
            PrinterSettings.InstalledPrinters.CopyTo(InstalledPrinters, 0);
            cmbxPrinterName.Items.AddRange(InstalledPrinters);
            cmbxPrinterName.SelectedText = PrinterName;
            cmbxPrinterScheme.SelectedText = PrinterScheme;
            cmbxPrinterScheme.Enabled = false;
        }
        /// <summary>
        /// Set values to controls in the theme GroupBox.
        /// </summary>
        private void SetThemeValues()
        {
            foreach (Color color in PossibleColorsControls)
            {
                cmbxThemeColourControls.Items.Add(color);
            }
            foreach (Color color in PossibleColorsFonts)
            {
                cmbxThemeColourFont.Items.Add(color);
            }
            cmbxThemeFontSystem.Items.AddRange(AvailableFonts);
            cmbxThemeFontReader.Items.AddRange(AvailableFonts);
            cmbxThemeFontWriter.Items.AddRange(AvailableFonts);

            cmbxThemeColourControls.Text = "";
            cmbxThemeColourControls.SelectedText = "";
            cmbxThemeColourFont.Text = "";
            cmbxThemeColourFont.SelectedText = "";
        }
        /// <summary>
        /// Set values to controls in the document GroupBox.
        /// </summary>
        private void SetDocumentValues()
        {
            cmbxFilters.SelectedText = SortingFilter;
            nudRODMax.Value = ROD_MaxNumber;
        }
        /// <summary>
        /// Set default values to all controls.
        /// </summary>
        private void SetDefaultValues()
        {
            DialogResult userChoice = MessageBox.Show("Do you want to revert to default settings?", "Notice", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (userChoice == DialogResult.Yes)
            {
                cmbxPrinterName.SelectedIndex = 0;
                cmbxPrinterScheme.SelectedIndex = cmbxPrinterScheme.FindString(Preferences.DefPrinterScheme);
                cmbxThemeColourControls.SelectedIndex = cmbxThemeColourControls.FindString(Preferences.DefBackColor);
                cmbxThemeColourFont.SelectedIndex = cmbxThemeColourFont.FindString(Preferences.DefForeColor);
                cmbxThemeFontReader.SelectedIndex = cmbxThemeFontReader.FindString(Preferences.DefFont);
                cmbxThemeFontSystem.SelectedIndex = cmbxThemeFontSystem.FindString(Preferences.DefFont);
                cmbxThemeFontWriter.SelectedIndex = cmbxThemeFontWriter.FindString(Preferences.DefFont);
                cmbxFilters.SelectedIndex = cmbxFilters.FindString(Preferences.DefSortingFilter);
                nudRODMax.Value = Preferences.DefROD_MaxNumber;
            }
        }

        /// <summary>
        /// Determines the availability of pre-considered font-family names.
        /// </summary>
        /// <returns></returns>
        private string[] GetAvailableFonts()
        {
            List<string> installedFonts = new List<string>();

            foreach (FontFamily ff in InstalledFonts)
            {
                if (PossibleFonts.Contains(ff.Name))
                {
                    installedFonts.Add(ff.Name);
                }
            }
            installedFonts.Sort();
            return installedFonts.ToArray();
        }

        #region Button Event Handlers
        /// <summary>
        /// Detects the closing of the PreferenceForm.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PrefsFormClosing(object sender, EventArgs e)
        {
            if (!ignoreValues)
            {
                Preferences.PrinterName = PrinterName;
                Preferences.PrinterScheme = PrinterScheme;
                Preferences.SortingFilter = SortingFilter;
                Preferences.FontReader = FontReader;
                Preferences.FontSystem = FontSystem;
                Preferences.FontWriter = FontWriter;
                Preferences.ColourControls = ColourControls;
                Preferences.ColourFont = ColourFont;
                Preferences.ROD_MaxNumber = ROD_MaxNumber;

                Preferences.SaveData();
                if (parentForm != null)
                {
                    Font font = new Font(Preferences.FontSystem, 14.25F, GraphicsUnit.Pixel);
                    Color backColor, foreColor;
                    backColor = ColorExtractor.ExtractColor(Preferences.ColourControls);
                    foreColor = ColorExtractor.ExtractColor(Preferences.ColourFont);
                    foreach (Control control in parentForm.Controls)
                    {
                        parentForm.UpdateControlColorsFonts(control, backColor, foreColor, font);
                    }
                }
            }
        }
        /// <summary>
        /// Detect click to save Button
        /// </summary>
        /// <param name="sender">The originating object</param>
        /// <param name="e">Event argument</param>
        private void btnPrefSave_Click(object sender, EventArgs e)
        {
            ignoreValues = false;
            Close();
        }
        /// <summary>
        /// Detect click to defaults Button
        /// </summary>
        /// <param name="sender">The originating object</param>
        /// <param name="e">Event argument</param>
        private void btnPrefDefaults_Click(object sender, EventArgs e)
        {
            SetDefaultValues();
        }
        /// <summary>
        /// Detect click to close Button
        /// </summary>
        /// <param name="sender">The originating object</param>
        /// <param name="e">Event argument</param>
        private void btnPrefClose_Click(object sender, EventArgs e)
        {
            /* Check if there are unsaved changes.
             * If any exist, notify the user; otherwise exit.
             */
            if (isUnsaved)
            {
                DialogResult userChoice = MessageBox.Show("Exit without saving? Your changes will be lost.", "Notice", MessageBoxButtons.YesNo);
                if (userChoice == DialogResult.Yes)
                {
                    ignoreValues = true;
                    Close();
                }
            }
            else
            {
                Close();
            }
        }
        #endregion

        #region Event handlers: Detect changes made to controls
        /// <summary>
        /// Detect changes to the theme colour font ComboBox
        /// </summary>
        /// <param name="sender">The originating object</param>
        /// <param name="e">Event argument</param>
        private void cmbxThemeColourFont_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!isFromInitializeControls)
            {
                isUnsaved = true;
            }
        }
        /// <summary>
        /// Detect changes to the theme colour controls ComboBox
        /// </summary>
        /// <param name="sender">The originating object</param>
        /// <param name="e">Event argument</param>
        private void cmbxThemeColourControls_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!isFromInitializeControls)
            {
                isUnsaved = true;
            }
        }
        /// <summary>
        /// Detect changes to the theme font system ComboBox
        /// </summary>
        /// <param name="sender">The originating object</param>
        /// <param name="e">Event argument</param>
        private void cmbxThemeFontSystem_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!isFromInitializeControls)
            {
                isUnsaved = true;
            }
        }
        /// <summary>
        /// Detect changes to the theme font reader ComboBox
        /// </summary>
        /// <param name="sender">The originating object</param>
        /// <param name="e">Event argument</param>
        private void cmbxThemeFontReader_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!isFromInitializeControls)
            {
                isUnsaved = true;
            }
        }
        /// <summary>
        /// Detect changes to the theme font writer ComboBox
        /// </summary>
        /// <param name="sender">The originating object</param>
        /// <param name="e">Event argument</param>
        private void cmbxThemeFontWriter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!isFromInitializeControls)
            {
                isUnsaved = true;
            }
        }

        /// <summary>
        /// Detect changes to the printer name ComboBox
        /// </summary>
        /// <param name="sender">The originating object</param>
        /// <param name="e">Event argument</param>
        private void cmbxPrinterName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!isFromInitializeControls)
            {
                isUnsaved = true;
            }
        }
        /// <summary>
        /// Detect changes to the printer scheme ComboBox
        /// </summary>
        /// <param name="sender">The originating object</param>
        /// <param name="e">Event argument</param>
        private void cmbxPrinterScheme_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!isFromInitializeControls)
            {
                isUnsaved = true;
            }
        }

        /// <summary>
        /// Detect changes to the filters ComboBox
        /// </summary>
        /// <param name="sender">The originating object</param>
        /// <param name="e">Event argument</param>
        private void cmbxFilters_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!isFromInitializeControls)
            {
                isUnsaved = true;
            }
        }
        /// <summary>
        /// Detect changes to the ROD_max NumericUpDown
        /// </summary>
        /// <param name="sender">The originating object</param>
        /// <param name="e">Event argument</param>
        private void nudRODMax_ValueChanged(object sender, EventArgs e)
        {
            if (!isFromInitializeControls)
            {
                isUnsaved = true;
            }
        }
        #endregion
    }
}
