using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using _5S_QA_DL.APIContext;
using _5S_QA_Entities.Dtos;
using _5S_QA_Entities.Models;
using _5S_QA_System.Controls;
using _5S_QA_System.Properties;
using MetroFramework.Controls;
using Newtonsoft.Json;

namespace _5S_QA_System;

public class mPanelSelectFile : UserControl
{
	private int mLimit;

	private int mRow;

	private readonly int mColumn;

	private DataGridView dgvMain;

	private DataGridView dgvSample;

	private IEnumerable<IGrouping<int?, RequestStatusViewModel>> mGroups;

	private Guid mTemplateId = Guid.Empty;

	private ComboBox cbbTemplate;

	private IContainer components = null;

	private ToolTip toolTipMain;

	private SaveFileDialog saveFileDialogMain;

	public mPanelSelectFile()
	{
		InitializeComponent();
		mColumn = 3;
	}

	private void mPanelSelectFile_Load(object sender, EventArgs e)
	{
		dgvMain = base.ParentForm.Controls["dgvMain"] as DataGridView;
		mPanelViewManager mPanelViewManager2 = base.ParentForm.Controls["mPanelViewMain"] as mPanelViewManager;
		Panel panel = mPanelViewManager2.Controls["panelView"] as Panel;
		Panel panel2 = panel.Controls["panelSample"] as Panel;
		dgvSample = panel2.Controls["dgvSample"] as DataGridView;
		Init();
	}

	private void Init()
	{
		mGroups = convertToGroupList();
		load_Templates();
	}

	private void load_LimitProduct()
	{
		try
		{
			ResponseDto result = frmLogin.client.GetsAsync(Guid.Parse(dgvMain.CurrentRow.Cells["ProductId"].Value.ToString()), "/api/Template/GetProductTemplates/{id}").Result;
			if (!result.Success)
			{
				throw new Exception();
			}
			string value = result.Data.ToString();
			TemplateViewModel val = JsonConvert.DeserializeObject<TemplateViewModel>(value);
			mLimit = val.Limit.Value;
		}
		catch
		{
			mLimit = 7;
		}
	}

	private void load_LimitPlan(Guid id)
	{
		try
		{
			ResponseDto result = frmLogin.client.GetsAsync(id, "/api/Template/GetPlanTemplates/{id}").Result;
			if (!result.Success)
			{
				throw new Exception();
			}
			string value = result.Data.ToString();
			TemplateViewModel val = JsonConvert.DeserializeObject<TemplateViewModel>(value);
			mLimit = val.Limit.Value;
		}
		catch
		{
			mLimit = 7;
		}
	}

	private void load_SampleHasResult(Guid? idplan)
	{
		mRow = 1;
		try
		{
			if (!idplan.HasValue)
			{
				idplan = Guid.Empty;
			}
			Guid productid = Guid.Parse(dgvMain.CurrentRow.Cells["Id"].Value.ToString());
			ResponseDto result = frmLogin.client.GetsAsync(productid, idplan.Value, "/api/RequestResult/GetSampleHasResults/{productid}/{id}").Result;
			if (!result.Success || result.Count.Equals(0))
			{
				throw new Exception();
			}
			mRow = result.Count;
		}
		catch
		{
			mRow = 1;
		}
	}

	private void load_Templates()
	{
		try
		{
			ResponseDto result = frmLogin.client.GetsAsync(Guid.Parse(dgvMain.CurrentRow.Cells["ProductId"].Value.ToString()), "/api/Template/GetAllProductTemplates/{id}").Result;
			DataTable dataTable = Common.getDataTable(result);
			if (dataTable == null)
			{
				return;
			}
			if (dataTable.Rows.Count > 1)
			{
				drawControlTemplates(dataTable);
			}
			else
			{
				if (dataTable.Rows.Count == 1)
				{
					mTemplateId = Guid.Parse(dataTable.Rows[0]["Id"].ToString());
				}
				drawControlMain();
			}
			Common.setControls(this, toolTipMain);
		}
		catch
		{
		}
	}

	private IEnumerable<IGrouping<int?, RequestStatusViewModel>> convertToGroupList()
	{
		DataTable dt = dgvSample.DataSource as DataTable;
		List<RequestStatusViewModel> source = Common.Casts<RequestStatusViewModel>(dt);
		return from x in source
			group x by x.Sample;
	}

	private void drawControlTemplates(DataTable dt)
	{
		base.Controls.Clear();
		MetroButton metroButton = new MetroButton
		{
			Dock = DockStyle.Top,
			Name = "btnConfirm",
			AutoSize = true,
			Cursor = Cursors.Hand,
			TabIndex = 2
		};
		metroButton.Click += btnConfirm_Click;
		base.Controls.Add(metroButton);
		cbbTemplate = new ComboBox
		{
			DropDownStyle = ComboBoxStyle.DropDownList,
			Dock = DockStyle.Top,
			FormattingEnabled = true,
			Cursor = Cursors.Hand,
			ItemHeight = 16,
			MaxLength = 50,
			Name = "cbbTemplate",
			TabIndex = 1,
			DisplayMember = "Name",
			ValueMember = "Id",
			DataSource = dt
		};
		base.Controls.Add(cbbTemplate);
		base.Padding = new Padding(3, 3, 3, 3);
		base.Size = new Size(200, 32);
		AutoSize = true;
		base.Location = base.ParentForm.PointToClient(new Point(Cursor.Position.X - base.Width / 2, Cursor.Position.Y - base.Height));
	}

	private void drawControlMain()
	{
		AutoSize = false;
		base.Controls.Clear();
		int num = ((mGroups.First().Count() > 1) ? 3 : 2);
		MetroButton metroButton = new MetroButton
		{
			Name = "btnAll",
			Size = new Size(180, 23),
			Cursor = Cursors.Hand
		};
		metroButton.Click += btnAll_Click;
		base.Controls.Add(metroButton);
		metroButton.Location = new Point(3, 3);
		MetroButton metroButton2 = new MetroButton
		{
			Name = "btnSample",
			Size = new Size(180, 23),
			Cursor = Cursors.Hand
		};
		metroButton2.Click += btnSample_Click;
		base.Controls.Add(metroButton2);
		metroButton2.Location = new Point(183, 3);
		if (num.Equals(3))
		{
			MetroButton metroButton3 = new MetroButton
			{
				Name = "btnPlan",
				Size = new Size(180, 23),
				Cursor = Cursors.Hand
			};
			metroButton3.Click += btnPlan_Click;
			base.Controls.Add(metroButton3);
			metroButton3.Location = new Point(363, 3);
		}
		base.Size = new Size(num * 180 + 8, 31);
		base.Location = base.ParentForm.PointToClient(new Point(Cursor.Position.X - base.Width / 2, Cursor.Position.Y - base.Height));
	}

	private void drawControlSample()
	{
		base.Controls.Clear();
		load_SampleHasResult(null);
		mLimit = 1;
		int num = ((mRow % mLimit == 0) ? (mRow / mLimit) : (mRow / mLimit + 1));
		for (int i = 0; i < num; i++)
		{
			MetroButton metroButton = new MetroButton();
			metroButton.Name = string.Format("{0}_{1}", "Sample", i + 1);
			metroButton.Text = string.Format("{0} {1} ({2}: {3})", Common.getTextLanguage(this, "Page"), i + 1, Common.getTextLanguage(this, "Sample"), i + 1);
			metroButton.Size = new Size(180, 23);
			metroButton.Cursor = Cursors.Hand;
			MetroButton metroButton2 = metroButton;
			metroButton2.Click += btn_Click;
			base.Controls.Add(metroButton2);
			toolTipMain.SetToolTip(metroButton2, Common.getTextLanguage(this, "ExportFor") + " [" + metroButton2.Text + "]");
			metroButton2.Location = new Point(i % mColumn * 180 + 3, i / mColumn * 23 + 3);
		}
		if (num > 1)
		{
			MetroButton metroButton3 = new MetroButton
			{
				Name = "Sample_All",
				Text = string.Format("{0} ({1} {2})", Common.getTextLanguage(this, "Download"), num, Common.getTextLanguage(this, "Files")),
				Size = new Size((num + 1 > mColumn) ? (mColumn * 180) : (num * 180), 23),
				Cursor = Cursors.Hand
			};
			metroButton3.Click += btn_Click;
			base.Controls.Add(metroButton3);
			toolTipMain.SetToolTip(metroButton3, Common.getTextLanguage(this, "Select") + " [" + metroButton3.Text + "]");
			metroButton3.Location = new Point(3, (num <= mColumn) ? 26 : ((num % mColumn == 0) ? (num / mColumn * 23 + 3) : ((num / mColumn + 1) * 23 + 3)));
		}
		base.Size = new Size((num < mColumn) ? (num * 180 + 8) : (mColumn * 180 + 8), (num <= 1) ? 31 : ((num % mColumn == 0) ? (num / mColumn * 23 + 31) : ((num / mColumn + 1) * 23 + 31)));
		base.Location = base.ParentForm.PointToClient(new Point(Cursor.Position.X - base.Width / 2, Cursor.Position.Y - base.Height));
	}

	private void drawControlPlan()
	{
		base.Controls.Clear();
		mRow = mGroups.First().Count();
		mLimit = 1;
		int num = ((mRow % mLimit == 0) ? (mRow / mLimit) : (mRow / mLimit + 1));
		for (int i = 0; i < num; i++)
		{
			MetroButton metroButton = new MetroButton
			{
				Name = $"{i}",
				Text = (mGroups.First().ElementAt(i).PlanName ?? ""),
				Size = new Size(180, 23),
				Cursor = Cursors.Hand
			};
			metroButton.Click += btnDetail_Click;
			base.Controls.Add(metroButton);
			toolTipMain.SetToolTip(metroButton, Common.getTextLanguage(this, "ExportFor") + " [" + metroButton.Text + "]");
			metroButton.Location = new Point(i % mColumn * 180 + 3, i / mColumn * 23 + 3);
		}
		if (num > 1)
		{
			MetroButton metroButton2 = new MetroButton
			{
				Name = "Plan_All",
				Text = Common.getTextLanguage(this, "DownloadAll"),
				Size = new Size((num + 1 > mColumn) ? (mColumn * 180) : (num * 180), 23),
				Cursor = Cursors.Hand
			};
			metroButton2.Click += btn_Click;
			base.Controls.Add(metroButton2);
			toolTipMain.SetToolTip(metroButton2, Common.getTextLanguage(this, "Select") + " [" + metroButton2.Text + "]");
			metroButton2.Location = new Point(3, (num <= mColumn) ? 26 : ((num % mColumn == 0) ? (num / mColumn * 23 + 3) : ((num / mColumn + 1) * 23 + 3)));
		}
		base.Size = new Size((num < mColumn) ? (num * 180 + 8) : (mColumn * 180 + 8), (num <= 1) ? 31 : ((num % mColumn == 0) ? (num / mColumn * 23 + 31) : ((num / mColumn + 1) * 23 + 31)));
		base.Location = base.ParentForm.PointToClient(new Point(Cursor.Position.X - base.Width / 2, Cursor.Position.Y - base.Height));
	}

	private void drawControlFinal(Guid? idplan = null)
	{
		base.Controls.Clear();
		load_SampleHasResult(idplan);
		if (!idplan.HasValue)
		{
			load_LimitProduct();
		}
		else
		{
			load_LimitPlan(idplan.Value);
		}
		int num = ((mRow % mLimit == 0) ? (mRow / mLimit) : (mRow / mLimit + 1));
		for (int i = 0; i < num; i++)
		{
			MetroButton metroButton = new MetroButton();
			if (!idplan.HasValue)
			{
				metroButton.Name = string.Format("{0}_{1}", "Product", i + 1);
			}
			else
			{
				metroButton.Name = $"{idplan}_{i + 1}";
			}
			metroButton.Text = string.Format("{0} {1} ({2}: {3} -> {4})", Common.getTextLanguage(this, "Page"), i + 1, Common.getTextLanguage(this, "Sample"), i * mLimit + 1, (i == num - 1) ? mRow : ((i + 1) * mLimit));
			metroButton.Size = new Size(180, 23);
			metroButton.Cursor = Cursors.Hand;
			metroButton.Click += btn_Click;
			base.Controls.Add(metroButton);
			toolTipMain.SetToolTip(metroButton, Common.getTextLanguage(this, "ExportFor") + " [" + metroButton.Text + "]");
			metroButton.Location = new Point(i % mColumn * 180 + 3, i / mColumn * 23 + 3);
		}
		if (num > 1)
		{
			MetroButton metroButton2 = new MetroButton();
			if (!idplan.HasValue)
			{
				metroButton2.Name = "Product_All";
			}
			else
			{
				metroButton2.Name = string.Format("{0}_{1}", idplan, "All");
			}
			metroButton2.Text = string.Format("{0} ({1} {2})", Common.getTextLanguage(this, "Download"), num, Common.getTextLanguage(this, "Files"));
			metroButton2.Size = new Size((num + 1 > mColumn) ? (mColumn * 180) : (num * 180), 23);
			metroButton2.Cursor = Cursors.Hand;
			metroButton2.Click += btn_Click;
			base.Controls.Add(metroButton2);
			toolTipMain.SetToolTip(metroButton2, Common.getTextLanguage(this, "Select") + " [" + metroButton2.Text + "]");
			metroButton2.Location = new Point(3, (num <= mColumn) ? 26 : ((num % mColumn == 0) ? (num / mColumn * 23 + 3) : ((num / mColumn + 1) * 23 + 3)));
		}
		base.Size = new Size((num < mColumn) ? (num * 180 + 8) : (mColumn * 180 + 8), (num <= 1) ? 31 : ((num % mColumn == 0) ? (num / mColumn * 23 + 31) : ((num / mColumn + 1) * 23 + 31)));
		base.Location = base.ParentForm.PointToClient(new Point(Cursor.Position.X - base.Width / 2, Cursor.Position.Y - base.Height));
	}

	private bool ByteArrayToFile(string fileName, byte[] byteArray)
	{
		try
		{
			using FileStream fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write);
			fileStream.Write(byteArray, 0, byteArray.Length);
			return true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, Common.getTextLanguage(this, "ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
			return false;
		}
	}

	private void btnConfirm_Click(object sender, EventArgs e)
	{
		Cursor.Current = Cursors.WaitCursor;
		Guid.TryParse(cbbTemplate.SelectedValue.ToString(), out mTemplateId);
		drawControlMain();
		Common.setControls(this, toolTipMain);
	}

	private void btnAll_Click(object sender, EventArgs e)
	{
		Cursor.Current = Cursors.WaitCursor;
		drawControlFinal();
	}

	private void btnSample_Click(object sender, EventArgs e)
	{
		Cursor.Current = Cursors.WaitCursor;
		drawControlSample();
	}

	private void btnPlan_Click(object sender, EventArgs e)
	{
		Cursor.Current = Cursors.WaitCursor;
		drawControlPlan();
	}

	private void btnDetail_Click(object sender, EventArgs e)
	{
		Cursor.Current = Cursors.WaitCursor;
		MetroButton metroButton = sender as MetroButton;
		int.TryParse(metroButton.Name, out var result);
		Guid? planId = mGroups.First().ElementAt(result).PlanId;
		drawControlFinal(planId);
	}

	private void btn_Click(object sender, EventArgs e)
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Expected O, but got Unknown
		Cursor.Current = Cursors.WaitCursor;
		MetroButton metroButton = sender as MetroButton;
		try
		{
			string[] array = metroButton.Name.Split('_');
			List<ExportDto> list = new List<ExportDto>
			{
				new ExportDto
				{
					Id = Guid.Parse(dgvMain.CurrentRow.Cells["Id"].Value.ToString()),
					Name = dgvMain.CurrentRow.Cells["Name"].Value.ToString(),
					Type = array[0],
					Page = array[1],
					TemplateId = mTemplateId
				}
			};
			ExportExcelDto exportExcelDto = frmLogin.client.ExportAsync((object)list, "/api/Template/Export").Result ?? throw new Exception(Common.getTextLanguage(this, "wHasntFile"));
			string text = exportExcelDto.FileName.Replace("\"", "").Replace("\\", "").Replace("/", "");
			if (Path.GetExtension(text).Equals(".zip"))
			{
				saveFileDialogMain.FileName = text;
				if (saveFileDialogMain.ShowDialog().Equals(DialogResult.OK))
				{
					text = saveFileDialogMain.FileName;
					ByteArrayToFile(text, exportExcelDto.Value);
					Common.ExecuteBatFile(text);
				}
				return;
			}
			string text2 = Path.Combine("C:\\Windows\\Temp\\5SQA_System", "VIEWS");
			Directory.CreateDirectory(text2);
			text = Path.Combine(text2, text);
			if (File.Exists(text))
			{
				File.Delete(text);
			}
			Cursor.Current = Cursors.WaitCursor;
			if (ByteArrayToFile(text, exportExcelDto.Value))
			{
				WebBrowser webBrowser = new WebBrowser();
				webBrowser.Navigate(text, newWindow: false);
			}
		}
		catch (Exception ex)
		{
			string text3 = ex.Message;
			string name = Settings.Default.Language.Replace("rb", "Name");
			List<Language> list2 = Common.ReadLanguages("Error");
			foreach (Language item in list2)
			{
				object value = ((object)item).GetType().GetProperty(name).GetValue(item, null);
				if (value != null)
				{
					string newValue = value.ToString();
					text3 = text3.Replace(item.Code, newValue);
				}
			}
			if (ex.InnerException is ApiException { StatusCode: var statusCode })
			{
				if (statusCode.Equals(401))
				{
					if (MessageBox.Show(Common.getTextLanguage(this, "LoginAgain"), Common.getTextLanguage(this, "INFORMATION"), MessageBoxButtons.OK, MessageBoxIcon.Asterisk).Equals(DialogResult.OK))
					{
						base.ParentForm.Close();
					}
				}
				else
				{
					MessageBox.Show(string.IsNullOrEmpty(text3) ? ex.Message : text3, Common.getTextLanguage(this, "ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
			}
			else
			{
				MessageBox.Show(string.IsNullOrEmpty(text3) ? ex.Message : text3, Common.getTextLanguage(this, "ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		this.components = new System.ComponentModel.Container();
		this.toolTipMain = new System.Windows.Forms.ToolTip(this.components);
		this.saveFileDialogMain = new System.Windows.Forms.SaveFileDialog();
		base.SuspendLayout();
		this.saveFileDialogMain.Filter = "File zip (*.zip)|*.zip";
		this.saveFileDialogMain.Title = "Select folder and enter file name";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.SystemColors.Window;
		base.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		base.Name = "mPanelSelectFile";
		base.Size = new System.Drawing.Size(1, 1);
		base.Load += new System.EventHandler(mPanelSelectFile_Load);
		base.ResumeLayout(false);
	}
}
