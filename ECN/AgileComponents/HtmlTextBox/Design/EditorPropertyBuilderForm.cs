using System.Drawing;
using System.Windows.Forms;

namespace ActiveUp.WebControls.Design
{
	/// <summary>
	/// Represents a <see cref="EditorPropertyBuilderForm"/> object.
	/// </summary>
	public class EditorPropertyBuilderForm : System.Windows.Forms.Form
	{
		/// <summary>
		/// Variable nécessaire au concepteur.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.TextBox _tbTabIndex;
		private System.Windows.Forms.TextBox _tbMaxLength;
		private System.Windows.Forms.TextBox _tbDesignIcon;
		private System.Windows.Forms.TextBox _tbHtmlIcon;
		private System.Windows.Forms.TextBox _tbPreviewIcon;
		private System.Windows.Forms.TextBox _tbPreviewLabel;
		private System.Windows.Forms.TextBox _tbDesignLabel;
		private System.Windows.Forms.TextBox _tbHtmlLabel;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.RadioButton _rbTabs;
		private System.Windows.Forms.RadioButton _rbCheckbox;
		private System.Windows.Forms.RadioButton _rbNone;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.RadioButton _rbDesign;
		private System.Windows.Forms.RadioButton _rbHtml;
		private System.Windows.Forms.RadioButton _rbPreview;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.TextBox _tbCols;
		private System.Windows.Forms.TextBox _tbRows;
		private System.Windows.Forms.TextBox _tbCssClass;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.TextBox _tbText;
		private System.Windows.Forms.PropertyGrid _pgStyle;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.Label label14;
		private System.Windows.Forms.Button _bAccept;
		private System.Windows.Forms.Button _bCancel;
		private System.Windows.Forms.CheckBox _cbPersistText;
		private System.Windows.Forms.CheckedListBox _clbBehavior;

		private Editor _editor = null;
		private System.Windows.Forms.Button _bDefaultValues;
		private System.Web.UI.WebControls.Style _style = new System.Web.UI.WebControls.Style();

		/// <summary>
		/// Initializes a new instance of the <see cref="EditorPropertyBuilderForm"/> class.
		/// </summary>
		/// <param name="editor">The editor.</param>
		public EditorPropertyBuilderForm(Editor editor)
		{
			//
			// Requis pour la prise en charge du Concepteur Windows Forms
			//
			InitializeComponent();

			//
			// TODO : ajoutez le code du constructeur après l'appel à InitializeComponent
			//
			_editor = editor;
			_Init();
		}

		/// <summary>
		/// Nettoyage des ressources utilisées.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
		/// le contenu de cette méthode avec l'éditeur de code.
		/// </summary>
		private void InitializeComponent()
		{
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this._tbTabIndex = new System.Windows.Forms.TextBox();
			this._tbMaxLength = new System.Windows.Forms.TextBox();
			this._tbDesignIcon = new System.Windows.Forms.TextBox();
			this._tbHtmlIcon = new System.Windows.Forms.TextBox();
			this._tbPreviewIcon = new System.Windows.Forms.TextBox();
			this._tbPreviewLabel = new System.Windows.Forms.TextBox();
			this._tbDesignLabel = new System.Windows.Forms.TextBox();
			this._tbHtmlLabel = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this._rbTabs = new System.Windows.Forms.RadioButton();
			this._rbCheckbox = new System.Windows.Forms.RadioButton();
			this._rbNone = new System.Windows.Forms.RadioButton();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this._rbDesign = new System.Windows.Forms.RadioButton();
			this._rbHtml = new System.Windows.Forms.RadioButton();
			this._rbPreview = new System.Windows.Forms.RadioButton();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this._tbCols = new System.Windows.Forms.TextBox();
			this._tbRows = new System.Windows.Forms.TextBox();
			this._tbCssClass = new System.Windows.Forms.TextBox();
			this.label9 = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.label11 = new System.Windows.Forms.Label();
			this._tbText = new System.Windows.Forms.TextBox();
			this._pgStyle = new System.Windows.Forms.PropertyGrid();
			this.label12 = new System.Windows.Forms.Label();
			this.label13 = new System.Windows.Forms.Label();
			this._clbBehavior = new System.Windows.Forms.CheckedListBox();
			this.label14 = new System.Windows.Forms.Label();
			this._cbPersistText = new System.Windows.Forms.CheckBox();
			this._bDefaultValues = new System.Windows.Forms.Button();
			this._bAccept = new System.Windows.Forms.Button();
			this._bCancel = new System.Windows.Forms.Button();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.AddRange(new System.Windows.Forms.Control[] {
																					  this.tabPage1,
																					  this.tabPage2});
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(488, 456);
			this.tabControl1.TabIndex = 0;
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.AddRange(new System.Windows.Forms.Control[] {
																				   this._bDefaultValues,
																				   this._cbPersistText,
																				   this.label14,
																				   this._clbBehavior,
																				   this.label13,
																				   this.label12,
																				   this._pgStyle,
																				   this._tbText,
																				   this.groupBox3,
																				   this.groupBox2,
																				   this.groupBox1,
																				   this.label8,
																				   this.label7,
																				   this.label6,
																				   this.label5,
																				   this.label4,
																				   this.label3,
																				   this.label2,
																				   this.label1,
																				   this._tbHtmlLabel,
																				   this._tbDesignLabel,
																				   this._tbPreviewLabel,
																				   this._tbPreviewIcon,
																				   this._tbHtmlIcon,
																				   this._tbDesignIcon,
																				   this._tbMaxLength,
																				   this._tbTabIndex});
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Size = new System.Drawing.Size(480, 430);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "General";
			// 
			// tabPage2
			// 
			this.tabPage2.Location = new System.Drawing.Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Size = new System.Drawing.Size(520, 358);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "Toolbars";
			// 
			// _tbTabIndex
			// 
			this._tbTabIndex.Location = new System.Drawing.Point(8, 24);
			this._tbTabIndex.Name = "_tbTabIndex";
			this._tbTabIndex.TabIndex = 0;
			this._tbTabIndex.Text = "";
			// 
			// _tbMaxLength
			// 
			this._tbMaxLength.Location = new System.Drawing.Point(120, 24);
			this._tbMaxLength.Name = "_tbMaxLength";
			this._tbMaxLength.TabIndex = 1;
			this._tbMaxLength.Text = "";
			// 
			// _tbDesignIcon
			// 
			this._tbDesignIcon.Location = new System.Drawing.Point(8, 64);
			this._tbDesignIcon.Name = "_tbDesignIcon";
			this._tbDesignIcon.TabIndex = 2;
			this._tbDesignIcon.Text = "";
			// 
			// _tbHtmlIcon
			// 
			this._tbHtmlIcon.Location = new System.Drawing.Point(8, 104);
			this._tbHtmlIcon.Name = "_tbHtmlIcon";
			this._tbHtmlIcon.TabIndex = 3;
			this._tbHtmlIcon.Text = "";
			// 
			// _tbPreviewIcon
			// 
			this._tbPreviewIcon.Location = new System.Drawing.Point(8, 144);
			this._tbPreviewIcon.Name = "_tbPreviewIcon";
			this._tbPreviewIcon.TabIndex = 4;
			this._tbPreviewIcon.Text = "";
			// 
			// _tbPreviewLabel
			// 
			this._tbPreviewLabel.Location = new System.Drawing.Point(120, 144);
			this._tbPreviewLabel.Name = "_tbPreviewLabel";
			this._tbPreviewLabel.TabIndex = 5;
			this._tbPreviewLabel.Text = "";
			// 
			// _tbDesignLabel
			// 
			this._tbDesignLabel.Location = new System.Drawing.Point(120, 64);
			this._tbDesignLabel.Name = "_tbDesignLabel";
			this._tbDesignLabel.TabIndex = 6;
			this._tbDesignLabel.Text = "";
			// 
			// _tbHtmlLabel
			// 
			this._tbHtmlLabel.Location = new System.Drawing.Point(120, 104);
			this._tbHtmlLabel.Name = "_tbHtmlLabel";
			this._tbHtmlLabel.TabIndex = 7;
			this._tbHtmlLabel.Text = "";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(120, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(100, 16);
			this.label1.TabIndex = 8;
			this.label1.Text = "Max. Length";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(120, 48);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(100, 16);
			this.label2.TabIndex = 9;
			this.label2.Text = "Design Label";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(120, 88);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(100, 16);
			this.label3.TabIndex = 10;
			this.label3.Text = "HTML Label";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(8, 128);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(100, 16);
			this.label4.TabIndex = 11;
			this.label4.Text = "Preview Icon";
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(8, 88);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(100, 16);
			this.label5.TabIndex = 12;
			this.label5.Text = "HTML Icon";
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(8, 8);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(100, 16);
			this.label6.TabIndex = 13;
			this.label6.Text = "Tab Index";
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(8, 48);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(100, 16);
			this.label7.TabIndex = 14;
			this.label7.Text = "Design Icon";
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(120, 128);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(100, 16);
			this.label8.TabIndex = 15;
			this.label8.Text = "Preview Label";
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.AddRange(new System.Windows.Forms.Control[] {
																					this._rbNone,
																					this._rbCheckbox,
																					this._rbTabs});
			this.groupBox1.Location = new System.Drawing.Point(8, 168);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(96, 88);
			this.groupBox1.TabIndex = 16;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Mode Select";
			// 
			// _rbTabs
			// 
			this._rbTabs.Checked = true;
			this._rbTabs.Location = new System.Drawing.Point(8, 16);
			this._rbTabs.Name = "_rbTabs";
			this._rbTabs.Size = new System.Drawing.Size(56, 16);
			this._rbTabs.TabIndex = 0;
			this._rbTabs.TabStop = true;
			this._rbTabs.Text = "Tabs";
			// 
			// _rbCheckbox
			// 
			this._rbCheckbox.Location = new System.Drawing.Point(8, 40);
			this._rbCheckbox.Name = "_rbCheckbox";
			this._rbCheckbox.Size = new System.Drawing.Size(80, 16);
			this._rbCheckbox.TabIndex = 1;
			this._rbCheckbox.Text = "Checkbox";
			// 
			// _rbNone
			// 
			this._rbNone.Location = new System.Drawing.Point(8, 64);
			this._rbNone.Name = "_rbNone";
			this._rbNone.Size = new System.Drawing.Size(56, 16);
			this._rbNone.TabIndex = 2;
			this._rbNone.Text = "None";
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.AddRange(new System.Windows.Forms.Control[] {
																					this._rbPreview,
																					this._rbHtml,
																					this._rbDesign});
			this.groupBox2.Location = new System.Drawing.Point(112, 170);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(88, 88);
			this.groupBox2.TabIndex = 17;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Default Mode";
			// 
			// _rbDesign
			// 
			this._rbDesign.Checked = true;
			this._rbDesign.Location = new System.Drawing.Point(8, 16);
			this._rbDesign.Name = "_rbDesign";
			this._rbDesign.Size = new System.Drawing.Size(64, 16);
			this._rbDesign.TabIndex = 0;
			this._rbDesign.TabStop = true;
			this._rbDesign.Text = "Design";
			// 
			// _rbHtml
			// 
			this._rbHtml.Location = new System.Drawing.Point(8, 40);
			this._rbHtml.Name = "_rbHtml";
			this._rbHtml.Size = new System.Drawing.Size(56, 16);
			this._rbHtml.TabIndex = 1;
			this._rbHtml.Text = "HTML";
			// 
			// _rbPreview
			// 
			this._rbPreview.Location = new System.Drawing.Point(8, 64);
			this._rbPreview.Name = "_rbPreview";
			this._rbPreview.Size = new System.Drawing.Size(64, 16);
			this._rbPreview.TabIndex = 2;
			this._rbPreview.Text = "Preview";
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.AddRange(new System.Windows.Forms.Control[] {
																					this.label11,
																					this.label10,
																					this._tbCssClass,
																					this._tbRows,
																					this._tbCols,
																					this.label9});
			this.groupBox3.Location = new System.Drawing.Point(8, 259);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(192, 64);
			this.groupBox3.TabIndex = 19;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Textarea (downlevel)";
			// 
			// _tbCols
			// 
			this._tbCols.Location = new System.Drawing.Point(8, 32);
			this._tbCols.Name = "_tbCols";
			this._tbCols.Size = new System.Drawing.Size(40, 20);
			this._tbCols.TabIndex = 0;
			this._tbCols.Text = "";
			// 
			// _tbRows
			// 
			this._tbRows.Location = new System.Drawing.Point(56, 32);
			this._tbRows.Name = "_tbRows";
			this._tbRows.Size = new System.Drawing.Size(40, 20);
			this._tbRows.TabIndex = 1;
			this._tbRows.Text = "";
			// 
			// _tbCssClass
			// 
			this._tbCssClass.Location = new System.Drawing.Point(104, 32);
			this._tbCssClass.Name = "_tbCssClass";
			this._tbCssClass.Size = new System.Drawing.Size(81, 20);
			this._tbCssClass.TabIndex = 2;
			this._tbCssClass.Text = "";
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(8, 16);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(40, 16);
			this.label9.TabIndex = 20;
			this.label9.Text = "Cols";
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(56, 16);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(48, 16);
			this.label10.TabIndex = 21;
			this.label10.Text = "Rows";
			// 
			// label11
			// 
			this.label11.Location = new System.Drawing.Point(104, 16);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(64, 16);
			this.label11.TabIndex = 22;
			this.label11.Text = "CSS Class";
			// 
			// _tbText
			// 
			this._tbText.Location = new System.Drawing.Point(6, 350);
			this._tbText.Multiline = true;
			this._tbText.Name = "_tbText";
			this._tbText.Size = new System.Drawing.Size(467, 72);
			this._tbText.TabIndex = 20;
			this._tbText.Text = "";
			// 
			// _pgStyle
			// 
			this._pgStyle.CommandsVisibleIfAvailable = true;
			this._pgStyle.HelpVisible = false;
			this._pgStyle.LargeButtons = false;
			this._pgStyle.LineColor = System.Drawing.SystemColors.ScrollBar;
			this._pgStyle.Location = new System.Drawing.Point(231, 23);
			this._pgStyle.Name = "_pgStyle";
			this._pgStyle.Size = new System.Drawing.Size(242, 145);
			this._pgStyle.TabIndex = 21;
			this._pgStyle.Text = "propertyGrid1";
			this._pgStyle.ToolbarVisible = false;
			this._pgStyle.ViewBackColor = System.Drawing.SystemColors.Window;
			this._pgStyle.ViewForeColor = System.Drawing.SystemColors.WindowText;
			// 
			// label12
			// 
			this.label12.Location = new System.Drawing.Point(230, 7);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(100, 15);
			this.label12.TabIndex = 22;
			this.label12.Text = "Editor Style";
			// 
			// label13
			// 
			this.label13.Location = new System.Drawing.Point(8, 329);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(100, 17);
			this.label13.TabIndex = 23;
			this.label13.Text = "Default Text";
			// 
			// _clbBehavior
			// 
			this._clbBehavior.Items.AddRange(new object[] {
															  "Add BR on return",
															  "Allow rollover",
															  "Auto detect SSL",
															  "Auto hide toolbar",
															  "Clean on paste",
															  "Focus on page load",
															  "Hack protection disabled",
															  "Visible"});
			this._clbBehavior.Location = new System.Drawing.Point(211, 192);
			this._clbBehavior.Name = "_clbBehavior";
			this._clbBehavior.Size = new System.Drawing.Size(261, 124);
			this._clbBehavior.TabIndex = 25;
			// 
			// label14
			// 
			this.label14.Location = new System.Drawing.Point(211, 175);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(50, 16);
			this.label14.TabIndex = 26;
			this.label14.Text = "Behavior";
			// 
			// _cbPersistText
			// 
			this._cbPersistText.Location = new System.Drawing.Point(214, 324);
			this._cbPersistText.Name = "_cbPersistText";
			this._cbPersistText.Size = new System.Drawing.Size(148, 16);
			this._cbPersistText.TabIndex = 27;
			this._cbPersistText.Text = "Persist Text in Viewstate";
			// 
			// _bDefaultValues
			// 
			this._bDefaultValues.Location = new System.Drawing.Point(369, 322);
			this._bDefaultValues.Name = "_bDefaultValues";
			this._bDefaultValues.Size = new System.Drawing.Size(93, 23);
			this._bDefaultValues.TabIndex = 28;
			this._bDefaultValues.Text = "Default Values";
			this._bDefaultValues.Click += new System.EventHandler(this._bDefaultValues_Click);
			// 
			// _bAccept
			// 
			this._bAccept.Location = new System.Drawing.Point(326, 461);
			this._bAccept.Name = "_bAccept";
			this._bAccept.TabIndex = 1;
			this._bAccept.Text = "Accept";
			this._bAccept.Click += new System.EventHandler(this._bAccept_Click);
			// 
			// _bCancel
			// 
			this._bCancel.Location = new System.Drawing.Point(410, 461);
			this._bCancel.Name = "_bCancel";
			this._bCancel.TabIndex = 2;
			this._bCancel.Text = "Cancel";
			this._bCancel.Click += new System.EventHandler(this._bCancel_Click);
			// 
			// EditorPropertyBuilderForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(489, 487);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this._bCancel,
																		  this._bAccept,
																		  this.tabControl1});
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "EditorPropertyBuilderForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "EditorPropertyBuilderForm";
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.groupBox3.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void _Init()
		{
			InitTextBoxText();
			InitCheckBoxEditorModeSelector();
			InitCheckBoxEditorStartupMode();

			_tbText.Text = _editor.Text;
			_cbPersistText.Checked = _editor.PersistText;

			SetItemCheckedClbBehavior();
			InitStyle();

			_pgStyle.SelectedObject = _style;
			_tbRows.Text = _editor.TextareaRows;
			_tbCols.Text = _editor.TextareaColumns;
			_tbCssClass.Text = _editor.CssClass;
		}

		private void InitTextBoxText()
		{
			_tbTabIndex.Text = _editor.TabIndex.ToString();
			_tbMaxLength.Text = _editor.MaxLength.ToString();

			_tbDesignIcon.Text = _editor.EditorModeDesignIcon;
			_tbDesignLabel.Text = _editor.EditorModeDesignLabel;

			_tbHtmlIcon.Text = _editor.EditorModeHtmlIcon;
			_tbHtmlLabel.Text = _editor.EditorModeHtmlLabel;

			_tbPreviewIcon.Text = _editor.EditorModePreviewIcon;
			_tbPreviewLabel.Text = _editor.EditorModePreviewLabel;
		}

		private void InitCheckBoxEditorModeSelector()
		{
			switch (_editor.EditorModeSelector)
			{
				case EditorModeSelectorType.None:
					_rbNone.Checked = true;
					break;
				case EditorModeSelectorType.CheckBox:
					_rbCheckbox.Checked = true;
					break;
				case EditorModeSelectorType.Tabs:
					_rbTabs.Checked = true;
					break;
			}
		}

		private void InitCheckBoxEditorStartupMode()
		{
			switch (_editor.StartupMode)
			{
				case EditorMode.Design:
					_rbDesign.Checked = true;
					break;
				case EditorMode.Html:
					_rbHtml.Checked = true;
					break;
				case EditorMode.Preview:
					_rbPreview.Checked = true;
					break;
			}
		}

		private void SetItemCheckedClbBehavior()
		{
			_clbBehavior.SetItemChecked(0, _editor.UseBR);
			_clbBehavior.SetItemChecked(1, _editor.AllowRollOver);
			_clbBehavior.SetItemChecked(2, _editor.AutoDetectSsl);
			_clbBehavior.SetItemChecked(3, _editor.AutoHideToolBars);
			_clbBehavior.SetItemChecked(4, _editor.CleanOnPaste);
			_clbBehavior.SetItemChecked(5, _editor.StartupFocus);
			_clbBehavior.SetItemChecked(6, _editor.HackProtectionDisabled);
			_clbBehavior.SetItemChecked(7, _editor.Visible);
		}

		private void InitStyle()
		{
			_style.BackColor = _editor.BackColor;
			_style.BorderColor = _editor.BorderColor;
			_style.BorderStyle = _editor.BorderStyle;
			_style.BorderWidth = _editor.BorderWidth;
			_style.CssClass = _editor.CssClass;
			_style.ForeColor = _editor.ForeColor;
			_style.Height = _editor.Height;
			_style.Width = _editor.Width;
		}

		private bool _Update()
		{

			if (_tbTabIndex.Text.Length > 0)
			{
				try
				{
					_editor.TabIndex = short.Parse(_tbTabIndex.Text);
				}

				catch
				{
					MessageBox.Show("Tab index must be an integer.","Editor property builder",MessageBoxButtons.OK,MessageBoxIcon.Error);
					return false;
				}
			}
			else
				_editor.TabIndex = 0;

			if (_tbMaxLength.Text.Length > 0)
			{
				try
				{
					_editor.MaxLength = int.Parse(_tbMaxLength.Text);
				}

				catch
				{
					MessageBox.Show("Max Length must be an integer.","Editor property builder",MessageBoxButtons.OK,MessageBoxIcon.Error);
					return false;
				}
			}
			else
				_editor.MaxLength = 0;

			_editor.EditorModeDesignIcon = _tbDesignIcon.Text;
			_editor.EditorModeDesignLabel = _tbDesignLabel.Text;
			_editor.EditorModeHtmlIcon = _tbHtmlIcon.Text;
			_editor.EditorModeHtmlLabel = _tbHtmlLabel.Text;
			_editor.EditorModePreviewIcon = _tbPreviewIcon.Text;
			_editor.EditorModePreviewLabel = _tbPreviewLabel.Text;

			_editor.BackColor = _style.BackColor;
			_editor.BorderColor = _style.BorderColor;
			_editor.BorderStyle = _style.BorderStyle;
			_editor.BorderWidth = _style.BorderWidth;
			_editor.CssClass = _style.CssClass;
			_editor.ForeColor = _style.ForeColor;
			_editor.Height = _style.Height;
			_editor.Width = _style.Width;

			_editor.UseBR = _clbBehavior.CheckedItems.Contains("Add BR on return");
			_editor.AllowRollOver = _clbBehavior.CheckedItems.Contains("Allow rollover");
			_editor.AutoDetectSsl = _clbBehavior.CheckedItems.Contains("Auto detect SSL");
			_editor.AutoHideToolBars = _clbBehavior.CheckedItems.Contains("Auto hide toolbar");
			_editor.CleanOnPaste = _clbBehavior.CheckedItems.Contains("Clean on paste");
			_editor.StartupFocus = _clbBehavior.CheckedItems.Contains("Focus on page load");
			_editor.HackProtectionDisabled = _clbBehavior.CheckedItems.Contains("Hack protection disabled");
			_editor.Visible = _clbBehavior.CheckedItems.Contains("Visible");

			_editor.PersistText = _cbPersistText.Checked;
			_editor.Text = _tbText.Text;

			if (_tbRows.Text.Length > 0)
			{
				try
				{
					_editor.TextareaRows = short.Parse(_tbRows.Text).ToString();
				}

				catch
				{
					MessageBox.Show("Text area rows must be an integer.","Editor property builder",MessageBoxButtons.OK,MessageBoxIcon.Error);
					return false;
				}
			}
			else
				_editor.TextareaRows = string.Empty;

			if (_tbCols.Text.Length > 0)
			{
				try
				{
					_editor.TextareaColumns = short.Parse(_tbCols.Text).ToString();
				}

				catch
				{
					MessageBox.Show("Text area columns must be an integer.","Editor property builder",MessageBoxButtons.OK,MessageBoxIcon.Error);
					return false;
				}
			}
			else
				_editor.TextareaColumns = string.Empty;

			_editor.TextareaCssClass = _tbCssClass.Text;

			if (_rbDesign.Checked)
				_editor.StartupMode = EditorMode.Design;
			else if (_rbHtml.Checked)
				_editor.StartupMode = EditorMode.Html;
			else if (_rbPreview.Checked)
				_editor.StartupMode = EditorMode.Preview;

			if (_rbTabs.Checked)
				_editor.EditorModeSelector = EditorModeSelectorType.Tabs;
			else if (_rbCheckbox.Checked)
				_editor.EditorModeSelector = EditorModeSelectorType.CheckBox;
			else if (_rbNone.Checked)
				_editor.EditorModeSelector = EditorModeSelectorType.None;

			return true;
		}

		private void _bCancel_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void _bDefaultValues_Click(object sender, System.EventArgs e)
		{
			_tbTabIndex.Text = "0";
			_tbMaxLength.Text = "0";
			
			_tbDesignIcon.Text = "tab_design.gif";
			_tbDesignLabel.Text = "Design";

			_tbHtmlIcon.Text = "tab_html.gif";
			_tbHtmlLabel.Text = "HTML";

			_tbPreviewIcon.Text = "tab_preview.gif";
			_tbPreviewLabel.Text = "Preview";

			_rbTabs.Checked = true;
			_rbDesign.Checked = true;

			_tbCols.Text = string.Empty;
			_tbRows.Text = string.Empty;
			_tbCssClass.Text = string.Empty;

			_tbText.Text = string.Empty;

			_clbBehavior.SetItemChecked(0,true);
			_clbBehavior.SetItemChecked(1,true);
			_clbBehavior.SetItemChecked(2,true);
			_clbBehavior.SetItemChecked(3,false);
			_clbBehavior.SetItemChecked(4,false);
			_clbBehavior.SetItemChecked(5,false);
			_clbBehavior.SetItemChecked(6,false);
			_clbBehavior.SetItemChecked(7,true);

			_style.BackColor = Color.FromArgb(0xDB,0xD8,0xD1);
			_style.BorderColor = Color.FromName("DarkGray");
			_style.BorderStyle = System.Web.UI.WebControls.BorderStyle.NotSet;
			_style.BorderWidth = System.Web.UI.WebControls.Unit.Parse("1px");
			_style.CssClass = string.Empty;
			_style.ForeColor = Color.Empty;
			_pgStyle.SelectedObject = _style;
		}

		private void _bAccept_Click(object sender, System.EventArgs e)
		{
			if (_Update())
			{
				DialogResult = DialogResult.OK;
				Close();
			}
		}

	}
}
