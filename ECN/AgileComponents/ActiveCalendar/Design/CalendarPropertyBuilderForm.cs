// Active Calendar v2.0
// Copyright (c) 2004 Active Up SPRL - http://www.activeup.com
//
// LIMITATION OF LIABILITY
// The software is supplied "as is". Active Up cannot be held liable to you
// for any direct or indirect damage, or for any loss of income, loss of
// profits, operating losses or any costs incurred whatsoever. The software
// has been designed with care, but Active Up does not guarantee that it is
// free of errors.

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Design;
using System.ComponentModel.Design;
using System.Windows.Forms.Design;
using System.Web;
using System.Reflection;

using ActiveUp.WebControls.Scheme;

namespace ActiveUp.WebControls.Design
{
	/// <summary>
	/// Represents a <see cref="CalendarPropertyBuilderForm"/> object.
	/// </summary>
	internal class CalendarPropertyBuilderForm : System.Windows.Forms.Form
	{
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button _bApply;
		private System.Windows.Forms.Button _bCancel;
		private System.Windows.Forms.Button _bOk;

		private Calendar _calendar = null;
		private System.Windows.Forms.ListBox _lbScheme;

		private SchemePreset _style = null;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.PictureBox _pbPreview;

		IComponentChangeService _changeService = null;
		
		public CalendarPropertyBuilderForm(Calendar calendar,IComponentChangeService changeService)
		{
			_calendar = calendar;

			InitializeComponent();

			if (_lbScheme.Items.Count > 0)
				_lbScheme.SelectedIndex = 0;

			_changeService = changeService;
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
			this.label1 = new System.Windows.Forms.Label();
			this.panel1 = new System.Windows.Forms.Panel();
			this._bCancel = new System.Windows.Forms.Button();
			this._bApply = new System.Windows.Forms.Button();
			this._bOk = new System.Windows.Forms.Button();
			this._lbScheme = new System.Windows.Forms.ListBox();
			this._pbPreview = new System.Windows.Forms.PictureBox();
			this.label2 = new System.Windows.Forms.Label();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(100, 16);
			this.label1.TabIndex = 0;
			this.label1.Text = "Scheme :";
			// 
			// panel1
			// 
			this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel1.Controls.AddRange(new System.Windows.Forms.Control[] {
																				 this._bCancel,
																				 this._bApply,
																				 this._bOk});
			this.panel1.Location = new System.Drawing.Point(184, 197);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(272, 40);
			this.panel1.TabIndex = 2;
			// 
			// _bCancel
			// 
			this._bCancel.Location = new System.Drawing.Point(184, 8);
			this._bCancel.Name = "_bCancel";
			this._bCancel.TabIndex = 2;
			this._bCancel.Text = "Cancel";
			this._bCancel.Click += new System.EventHandler(this._bCancel_Click);
			// 
			// _bApply
			// 
			this._bApply.Location = new System.Drawing.Point(96, 8);
			this._bApply.Name = "_bApply";
			this._bApply.TabIndex = 1;
			this._bApply.Text = "Apply";
			this._bApply.Click += new System.EventHandler(this._bApply_Click);
			// 
			// _bOk
			// 
			this._bOk.Location = new System.Drawing.Point(8, 8);
			this._bOk.Name = "_bOk";
			this._bOk.TabIndex = 0;
			this._bOk.Text = "Ok";
			this._bOk.Click += new System.EventHandler(this._bOk_Click);
			// 
			// _lbScheme
			// 
			this._lbScheme.Items.AddRange(new object[] {
														   "Default1",
														   "Default2",
														   "Blue1",
														   "Blue2",
														   "Orange1",
														   "Orange2",
														   "Turquoise1",
														   "Turquoise2"});
			this._lbScheme.Location = new System.Drawing.Point(8, 24);
			this._lbScheme.Name = "_lbScheme";
			this._lbScheme.Size = new System.Drawing.Size(168, 212);
			this._lbScheme.TabIndex = 1;
			this._lbScheme.KeyDown += new System.Windows.Forms.KeyEventHandler(this._lbScheme_KeyDown);
			this._lbScheme.SelectedIndexChanged += new System.EventHandler(this._lbScheme_SelectedIndexChanged);
			// 
			// _pbPreview
			// 
			this._pbPreview.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this._pbPreview.Location = new System.Drawing.Point(208, 24);
			this._pbPreview.Name = "_pbPreview";
			this._pbPreview.Size = new System.Drawing.Size(238, 168);
			this._pbPreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this._pbPreview.TabIndex = 3;
			this._pbPreview.TabStop = false;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(208, 8);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(100, 16);
			this.label2.TabIndex = 4;
			this.label2.Text = "Preview :";
			// 
			// PropertyBuilderForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(466, 244);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.label2,
																		  this._pbPreview,
																		  this._lbScheme,
																		  this.panel1,
																		  this.label1});
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "PropertyBuilderForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "PropertyBuilder";
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void _bOk_Click(object sender, System.EventArgs e)
		{
			DialogResult = DialogResult.OK;
			ApplyStyle();
			Close();
		}

		private void _bApply_Click(object sender, System.EventArgs e)
		{
            ApplyStyle();
			_changeService.OnComponentChanged(_calendar, null, null, null);
		}

		private void _bCancel_Click(object sender, System.EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
			Close();
		}

		private void ApplyStyle()
		{
			switch(_lbScheme.SelectedItem.ToString())
			{
				case "Default1":
				{ _style = new SchemeDefault1(); } break;

				case "Default2":
				{ _style = new SchemeDefault2(); } break;

				case "Blue1":
				{ _style = new SchemeBlue1(); }break;

				case "Blue2":
				{ _style = new SchemeBlue2(); }break;

				case "Orange1":
				{ _style = new SchemeOrange1(); } break;

				case "Orange2":
				{ _style = new SchemeOrange2(); } break;

				case "Turquoise1":
				{ _style = new SchemeTurquoise1(); } break;

				case "Turquoise2":
				{ _style = new SchemeTurquoise2(); } break;

				default: break;
			}

			if (_style != null)
			{
				_calendar.NextYearImage = _style.NextYearImage;
				_calendar.NextMonthImage = _style.NextMonthImage;
				_calendar.PrevYearImage = _style.PrevYearImage;
				_calendar.PrevMonthImage = _style.PrevMonthImage;

				_calendar.BorderColor = _style.BorderColor;

				_calendar.BlockedDayStyle = _style.BlockedDayStyle;
				_calendar.DayHeaderStyle = _style.DayHeaderStyle;
				_calendar.DayStyle = _style.DayStyle;
				_calendar.NextPrevStyle = _style.NextPrevStyle;
				_calendar.OtherMonthDayStyle = _style.OtherMonthDayStyle;
				_calendar.SelectedDayStyle = _style.SelectedDayStyle;
				_calendar.SelectorStyle = _style.SelectorStyle;
				_calendar.TitleStyle = _style.TitleStyle;
				_calendar.TodayDayStyle = _style.TodayDayStyle;
				_calendar.TodayFooterStyle = _style.TodayFooterStyle;
				_calendar.WeekEndDayStyle = _style.WeekendEndDayStyle;
				_calendar.WeekNumberStyle = _style.WeekNumberStyle;
			}
		}

		private void _lbScheme_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				ApplyStyle();
				_changeService.OnComponentChanged(_calendar, null, null, null);
			}
		}

		private void _lbScheme_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			switch (_lbScheme.SelectedItem.ToString().Trim())
			{
				case "Default1":
				{
					_pbPreview.Image = (Bitmap)Utils.GetEmbeddedResource("ActiveUp.WebControls.ActiveCalendar.Scheme.SchemeDefault1.tif");
				} break;

				case "Default2":
				{
					_pbPreview.Image = (Bitmap)Utils.GetEmbeddedResource("ActiveUp.WebControls.ActiveCalendar.Scheme.SchemeDefault2.tif");
				} break;

				case "Blue1":
				{
					_pbPreview.Image = (Bitmap)Utils.GetEmbeddedResource("ActiveUp.WebControls.ActiveCalendar.Scheme.SchemeBlue1.tif");
				} break;

				case "Blue2":
				{
					_pbPreview.Image = (Bitmap)Utils.GetEmbeddedResource("ActiveUp.WebControls.ActiveCalendar.Scheme.SchemeBlue2.tif");
				} break;

				case "Orange1":
				{ 
					_pbPreview.Image = (Bitmap)Utils.GetEmbeddedResource("ActiveUp.WebControls.ActiveCalendar.Scheme.SchemeOrange1.tif");
				} break;

				case "Orange2":
				{
					_pbPreview.Image = (Bitmap)Utils.GetEmbeddedResource("ActiveUp.WebControls.ActiveCalendar.Scheme.SchemeOrange2.tif");
				} break;

				case "Turquoise1":
				{
					_pbPreview.Image = (Bitmap)Utils.GetEmbeddedResource("ActiveUp.WebControls.ActiveCalendar.Scheme.SchemeTurquoise1.tif");
				} break;

				case "Turquoise2":
				{ 
					_pbPreview.Image = (Bitmap)Utils.GetEmbeddedResource("ActiveUp.WebControls.ActiveCalendar.Scheme.SchemeTurquoise2.tif");
				} break;

				default : break;
			}
		}
	}
}
