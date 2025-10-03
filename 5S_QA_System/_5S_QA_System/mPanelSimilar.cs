using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using _5S_QA_DL.APIContext;
using _5S_QA_Entities.Abstracts;
using _5S_QA_Entities.Constants;
using _5S_QA_Entities.Dtos;
using _5S_QA_Entities.Enums;
using _5S_QA_Entities.Models;
using _5S_QA_System.Controls;
using _5S_QA_System.Properties;

namespace _5S_QA_System;

public class mPanelSimilar : UserControl
{
	private DataGridView mDataGridView;

	private Guid mId;

	private Guid mIdParent;

	private Form mForm;

	private readonly ComboBox cbb;

	private DataTable mDataProduct;

	private Type mType;

	private IContainer components = null;

	private DataGridViewTextBoxColumn ContentValue;

	private DataGridViewTextBoxColumn ContentName;

	private DataGridView dgvContent;

	private DataGridViewTextBoxColumn ContentTitle;

	private DataGridViewTextBoxColumn FooterValue;

	private DataGridViewTextBoxColumn FooterTitle;

	private DataGridViewTextBoxColumn FooterName;

	private DataGridView dgvFooter;

	private Panel panelView;

	private Panel panelResize;

	private ToolTip toolTipMain;

	private Button btnConfirm;

	private Button btnDown;

	private Button btnUp;

	private Panel panel1;

	private Label lblValue;

	private Label lblTitle;

	private TableLayoutPanel tpanelTitle;

	public mPanelSimilar()
	{
		InitializeComponent();
		Common.setControls(this, toolTipMain);
		mId = Guid.Empty;
		cbb = new ComboBox();
		dgvContent.Controls.Add(cbb);
		cbb.Visible = false;
		cbb.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
		cbb.AutoCompleteSource = AutoCompleteSource.ListItems;
		cbb.Cursor = Cursors.Hand;
	}

	private void mPanelSimilar_Load(object sender, EventArgs e)
	{
		ControlResize.Init(panelResize, this, ControlResize.Direction.Horizontal, Cursors.SizeWE);
		mDataGridView = base.Parent.Controls["dgvMain"] as DataGridView;
		mForm = base.ParentForm;
		mType = mForm.GetType();
	}

	public void load_cbbProduct(Guid id)
	{
		mIdParent = id;
		try
		{
			QueryArgs queryArgs = new QueryArgs();
			queryArgs.Predicate = "Measurements.Any() && Id!=@0";
			queryArgs.PredicateParameters = new string[1] { id.ToString() };
			queryArgs.Order = "Created DESC";
			queryArgs.Page = 1;
			queryArgs.Limit = int.MaxValue;
			QueryArgs body = queryArgs;
			ResponseDto result = frmLogin.client.GetsAsync(body, "/api/Product/Gets").Result;
			mDataProduct = Common.getDataTable(result);
			if (mDataProduct != null)
			{
				mDataProduct.Dispose();
				cbb.ValueMember = "Id";
				cbb.DisplayMember = "CodeName";
				cbb.DataSource = mDataProduct;
				cbb.SelectedIndexChanged += cbb_SelectedIndexChanged;
			}
			else
			{
				cbb.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			cbb.DataSource = null;
			if (ex.InnerException is ApiException { StatusCode: var statusCode } ex2)
			{
				if (statusCode.Equals(401))
				{
					if (MessageBox.Show(Common.getTextLanguage(this, "LoginAgain"), Common.getTextLanguage(this, "INFORMATION"), MessageBoxButtons.OK, MessageBoxIcon.Asterisk).Equals(DialogResult.OK))
					{
						mForm.Close();
					}
				}
				else
				{
					MessageBox.Show(ex2.Message, Common.getTextLanguage(this, "ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
			}
			else
			{
				MessageBox.Show(ex.Message, Common.getTextLanguage(this, "ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}
	}

	public void load_cbbTemplate(Guid id)
	{
		mIdParent = id;
		try
		{
			QueryArgs queryArgs = new QueryArgs();
			queryArgs.Predicate = "Type=@0";
			queryArgs.PredicateParameters = new string[1] { "Special" };
			queryArgs.Order = "Created DESC";
			queryArgs.Page = 1;
			queryArgs.Limit = int.MaxValue;
			QueryArgs body = queryArgs;
			ResponseDto result = frmLogin.client.GetsAsync(body, "/api/Template/Gets").Result;
			mDataProduct = Common.getDataTable(result);
			if (mDataProduct != null)
			{
				mDataProduct.Dispose();
				cbb.ValueMember = "Id";
				cbb.DisplayMember = "Name";
				cbb.DataSource = mDataProduct;
				cbb.SelectedIndexChanged += cbb_SelectedIndexChanged;
			}
			else
			{
				cbb.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			cbb.DataSource = null;
			if (ex.InnerException is ApiException { StatusCode: var statusCode } ex2)
			{
				if (statusCode.Equals(401))
				{
					if (MessageBox.Show(Common.getTextLanguage(this, "LoginAgain"), Common.getTextLanguage(this, "INFORMATION"), MessageBoxButtons.OK, MessageBoxIcon.Asterisk).Equals(DialogResult.OK))
					{
						mForm.Close();
					}
				}
				else
				{
					MessageBox.Show(ex2.Message, Common.getTextLanguage(this, "ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
			}
			else
			{
				MessageBox.Show(ex.Message, Common.getTextLanguage(this, "ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}
	}

	public void load_dgvContent(Enum type)
	{
		dgvContent.Rows.Clear();
		dgvFooter.Rows.Clear();
		try
		{
			List<string> list = MetaType.mPanelSimilar.Cast<string>().ToList();
			foreach (DataGridViewColumn col in mDataGridView.Columns)
			{
				if (list.Find((string x) => x.Equals(col.Name)) == null)
				{
					continue;
				}
				if (type is FormType formType)
				{
					switch (formType)
					{
					case FormType.VIEW:
						lblTitle.Text = Common.getTextLanguage(this, "VIEW");
						if (mType.Equals(typeof(frmSimilarView)))
						{
							mId = (string.IsNullOrEmpty(mDataGridView.CurrentRow.Cells["ProductId"].Value.ToString()) ? Guid.Empty : Guid.Parse(mDataGridView.CurrentRow.Cells["ProductId"].Value.ToString()));
						}
						else
						{
							mId = (string.IsNullOrEmpty(mDataGridView.CurrentRow.Cells["Id"].Value.ToString()) ? Guid.Empty : Guid.Parse(mDataGridView.CurrentRow.Cells["Id"].Value.ToString()));
						}
						lblValue.Text = (mId.Equals(Guid.Empty) ? string.Empty : mId.ToString());
						btnConfirm.Visible = false;
						if (MetaType.dgvFooter.Find((string x) => x.Equals(col.Name)) != null)
						{
							dgvFooter.Rows.Add(col.Name, col.HeaderText, mDataGridView.CurrentRow.Cells[col.Name].Value);
						}
						else
						{
							dgvContent.MultiSelect = true;
							dgvContent.Rows.Add(col.Name, col.HeaderText, mDataGridView.CurrentRow.Cells[col.Name].Value);
							dgvContent.Rows[dgvContent.RowCount - 1].Cells["ContentValue"].ReadOnly = true;
						}
						continue;
					case FormType.ADD:
					{
						lblTitle.Text = Common.getTextLanguage(this, "ADD");
						lblValue.Text = string.Empty;
						btnConfirm.Visible = true;
						mId = Guid.Empty;
						if (MetaType.dgvFooter.Find((string x) => x.Equals(col.Name)) != null)
						{
							dgvFooter.Rows.Add(col.Name, col.HeaderText, "");
							continue;
						}
						dgvContent.MultiSelect = false;
						dgvContent.Rows.Add(col.Name, col.HeaderText, (mDataGridView.CurrentCell == null) ? string.Empty : mDataGridView.CurrentRow.Cells[col.Name].Value);
						string name = col.Name;
						string text = name;
						if (!(text == "TemplateName"))
						{
							if (text == "ProductCode")
							{
								dgvContent.Rows[dgvContent.RowCount - 1].Cells["ContentValue"].ReadOnly = false;
							}
							else
							{
								dgvContent.Rows[dgvContent.RowCount - 1].Cells["ContentValue"].ReadOnly = true;
							}
						}
						else
						{
							dgvContent.Rows[dgvContent.RowCount - 1].Cells["ContentValue"].ReadOnly = false;
						}
						continue;
					}
					}
				}
				lblTitle.Text = Common.getTextLanguage(this, "EDIT");
				btnConfirm.Visible = true;
				if (mType.Equals(typeof(frmSimilarView)))
				{
					mId = (string.IsNullOrEmpty(mDataGridView.CurrentRow.Cells["ProductId"].Value.ToString()) ? Guid.Empty : Guid.Parse(mDataGridView.CurrentRow.Cells["ProductId"].Value.ToString()));
				}
				else
				{
					mId = (string.IsNullOrEmpty(mDataGridView.CurrentRow.Cells["Id"].Value.ToString()) ? Guid.Empty : Guid.Parse(mDataGridView.CurrentRow.Cells["Id"].Value.ToString()));
				}
				lblValue.Text = (mId.Equals(Guid.Empty) ? string.Empty : mId.ToString());
				if (MetaType.dgvFooter.Find((string x) => x.Equals(col.Name)) != null)
				{
					dgvFooter.Rows.Add(col.Name, col.HeaderText, mDataGridView.CurrentRow.Cells[col.Name].Value);
					continue;
				}
				dgvContent.MultiSelect = false;
				dgvContent.Rows.Add(col.Name, col.HeaderText, mDataGridView.CurrentRow.Cells[col.Name].Value);
				string name2 = col.Name;
				string text2 = name2;
				if (!(text2 == "TemplateName"))
				{
					if (text2 == "ProductCode")
					{
						dgvContent.Rows[dgvContent.RowCount - 1].Cells["ContentValue"].ReadOnly = false;
					}
					else
					{
						dgvContent.Rows[dgvContent.RowCount - 1].Cells["ContentValue"].ReadOnly = true;
					}
				}
				else
				{
					dgvContent.Rows[dgvContent.RowCount - 1].Cells["ContentValue"].ReadOnly = false;
				}
			}
		}
		finally
		{
			dgvContent.Size = new Size(base.Width, dgvContent.Rows.Count * 22 + 3);
			dgvFooter.Size = new Size(base.Width, dgvFooter.Rows.Count * 22 + 3);
			dgvContent.CurrentCell = null;
			dgvContent.Refresh();
			dgvFooter.CurrentCell = null;
			dgvFooter.Refresh();
		}
	}

	private void btnUp_Click(object sender, EventArgs e)
	{
		mDataGridView.Select();
		SendKeys.SendWait("{up}");
	}

	private void btnDown_Click(object sender, EventArgs e)
	{
		mDataGridView.Select();
		SendKeys.SendWait("{down}");
	}

	private void dgvContent_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
	{
		if (mDataProduct == null)
		{
			dgvContent.EndEdit();
			return;
		}
		DataGridView dataGridView = sender as DataGridView;
		Rectangle cellDisplayRectangle = dataGridView.GetCellDisplayRectangle(dataGridView.CurrentCell.ColumnIndex, dataGridView.CurrentCell.RowIndex, cutOverflow: true);
		cbb.Size = new Size(cellDisplayRectangle.Width, cellDisplayRectangle.Height);
		cbb.Location = new Point(cellDisplayRectangle.X, cellDisplayRectangle.Y);
		string code = string.Empty;
		if (dataGridView.CurrentCell.Value != null)
		{
			code = dataGridView.CurrentCell.Value.ToString();
		}
		DataRow dataRow = null;
		dataRow = ((!mType.Equals(typeof(frmSimilarView))) ? (from DataRow x in mDataProduct.Rows
			where x.Field<string>("Name").Equals(code)
			select x).FirstOrDefault() : (from DataRow x in mDataProduct.Rows
			where x.Field<string>("Code").Equals(code)
			select x).FirstOrDefault());
		if (dataRow != null)
		{
			cbb.SelectedValue = dataRow["Id"];
		}
		cbb.Visible = true;
		cbb.BringToFront();
	}

	private void cbb_SelectedIndexChanged(object sender, EventArgs e)
	{
		DataRow dataRow = (from DataRow x in mDataProduct.Rows
			where x.Field<string>("Id").Equals(cbb.SelectedValue)
			select x).FirstOrDefault();
		if (dataRow != null)
		{
			if (mType.Equals(typeof(frmSimilarView)))
			{
				dgvContent.Rows[0].Cells["ContentValue"].Value = dataRow["Code"];
				dgvContent.Rows[dgvContent.RowCount - 1].Cells["ContentValue"].Value = dataRow["Name"];
			}
			else
			{
				dgvContent.Rows[0].Cells["ContentValue"].Value = dataRow["Name"];
				dgvContent.Rows[dgvContent.RowCount - 1].Cells["ContentValue"].Value = dataRow["Description"];
			}
		}
	}

	private void dgvContent_CellEndEdit(object sender, DataGridViewCellEventArgs e)
	{
		cbb.Visible = false;
	}

	private void btnConfirm_Click(object sender, EventArgs e)
	{
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Expected O, but got Unknown
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Expected O, but got Unknown
		//IL_0174: Expected O, but got Unknown
		//IL_025e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0263: Unknown result type (might be due to invalid IL or missing references)
		//IL_026f: Expected O, but got Unknown
		//IL_0270: Unknown result type (might be due to invalid IL or missing references)
		//IL_0282: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ab: Expected O, but got Unknown
		//IL_02ae: Expected O, but got Unknown
		Cursor.Current = Cursors.WaitCursor;
		try
		{
			if (dgvContent.Rows[0].Cells["ContentValue"].Value == null || string.IsNullOrEmpty(dgvContent.Rows[0].Cells["ContentValue"].Value.ToString()))
			{
				if (mType.Equals(typeof(frmSimilarView)))
				{
					MessageBox.Show(Common.getTextLanguage(this, "wCode"), Common.getTextLanguage(this, "WARNING"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
				else
				{
					MessageBox.Show(Common.getTextLanguage(this, "wTemplate"), Common.getTextLanguage(this, "WARNING"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
				dgvContent.Rows[0].Cells["ContentValue"].Selected = true;
			}
			else if (mType.Equals(typeof(frmSimilarView)))
			{
				DataRow dataRow = (from DataRow x in mDataProduct.Rows
					where x.Field<string>("Code").Equals(dgvContent.Rows[0].Cells["ContentValue"].Value)
					select x).FirstOrDefault();
				SimilarViewModel val = new SimilarViewModel();
				((AuditableEntity)val).Id = mIdParent;
				val.ProductId = Guid.Parse(dataRow["Id"].ToString());
				((AuditableEntity)val).IsActivated = true;
				SimilarViewModel body = val;
				ResponseDto result = frmLogin.client.SaveAsync(body, "/api/Similar/Save").Result;
				if (!result.Success)
				{
					throw new Exception(result.Messages.ElementAt(0).Message);
				}
				frmSimilarView frmSimilarView2 = mForm as frmSimilarView;
				frmSimilarView2.isClose = false;
				frmSimilarView2.load_AllData();
			}
			else if (mId.Equals(Guid.Empty) || !MessageBox.Show(Common.getTextLanguage(this, "wSureEdit"), Common.getTextLanguage(this, "QUESTION"), MessageBoxButtons.YesNo, MessageBoxIcon.Question).Equals(DialogResult.No))
			{
				Cursor.Current = Cursors.WaitCursor;
				DataRow dataRow2 = (from DataRow x in mDataProduct.Rows
					where x.Field<string>("Name").Equals(dgvContent.Rows[0].Cells["ContentValue"].Value)
					select x).FirstOrDefault();
				TemplateOtherViewModel val2 = new TemplateOtherViewModel();
				((AuditableEntity)val2).Id = mId;
				val2.ProductId = mIdParent;
				val2.TemplateId = Guid.Parse(dataRow2["Id"].ToString());
				((AuditableEntity)val2).IsActivated = true;
				TemplateOtherViewModel body2 = val2;
				ResponseDto result2 = frmLogin.client.SaveAsync(body2, "/api/TemplateOther/Save").Result;
				if (!result2.Success)
				{
					throw new Exception(result2.Messages.ElementAt(0).Message);
				}
				frmTemplateOtherView frmTemplateOtherView2 = mForm as frmTemplateOtherView;
				frmTemplateOtherView2.isClose = false;
				frmTemplateOtherView2.load_AllData();
			}
		}
		catch (Exception ex)
		{
			string textLanguage = Common.getTextLanguage("Error", ex.Message);
			if (ex.InnerException is ApiException { StatusCode: var statusCode })
			{
				if (statusCode.Equals(401))
				{
					if (MessageBox.Show(Common.getTextLanguage(this, "LoginAgain"), Common.getTextLanguage(this, "INFORMATION"), MessageBoxButtons.OK, MessageBoxIcon.Asterisk).Equals(DialogResult.OK))
					{
						mForm.Close();
					}
				}
				else
				{
					MessageBox.Show(string.IsNullOrEmpty(textLanguage) ? ex.Message : textLanguage, Common.getTextLanguage(this, "ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
			}
			else
			{
				MessageBox.Show(string.IsNullOrEmpty(textLanguage) ? ex.Message : textLanguage, Common.getTextLanguage(this, "ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}
	}

	private void dgvNormal_Leave(object sender, EventArgs e)
	{
		DataGridView dataGridView = sender as DataGridView;
		dataGridView.CurrentCell = null;
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
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
		this.ContentValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.ContentName = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dgvContent = new System.Windows.Forms.DataGridView();
		this.ContentTitle = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.FooterValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.FooterTitle = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.FooterName = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dgvFooter = new System.Windows.Forms.DataGridView();
		this.panelView = new System.Windows.Forms.Panel();
		this.panelResize = new System.Windows.Forms.Panel();
		this.toolTipMain = new System.Windows.Forms.ToolTip(this.components);
		this.btnConfirm = new System.Windows.Forms.Button();
		this.btnDown = new System.Windows.Forms.Button();
		this.btnUp = new System.Windows.Forms.Button();
		this.panel1 = new System.Windows.Forms.Panel();
		this.lblValue = new System.Windows.Forms.Label();
		this.lblTitle = new System.Windows.Forms.Label();
		this.tpanelTitle = new System.Windows.Forms.TableLayoutPanel();
		((System.ComponentModel.ISupportInitialize)this.dgvContent).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.dgvFooter).BeginInit();
		this.panelView.SuspendLayout();
		this.tpanelTitle.SuspendLayout();
		base.SuspendLayout();
		this.ContentValue.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		dataGridViewCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
		this.ContentValue.DefaultCellStyle = dataGridViewCellStyle;
		this.ContentValue.FillWeight = 70f;
		this.ContentValue.HeaderText = "Value";
		this.ContentValue.Name = "ContentValue";
		this.ContentValue.ReadOnly = true;
		this.ContentValue.Resizable = System.Windows.Forms.DataGridViewTriState.False;
		this.ContentValue.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.ContentName.HeaderText = "Name";
		this.ContentName.Name = "ContentName";
		this.ContentName.Visible = false;
		this.dgvContent.AllowUserToAddRows = false;
		this.dgvContent.AllowUserToDeleteRows = false;
		this.dgvContent.AllowUserToResizeColumns = false;
		this.dgvContent.AllowUserToResizeRows = false;
		this.dgvContent.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
		this.dgvContent.ColumnHeadersVisible = false;
		this.dgvContent.Columns.AddRange(this.ContentName, this.ContentTitle, this.ContentValue);
		this.dgvContent.Dock = System.Windows.Forms.DockStyle.Top;
		this.dgvContent.Location = new System.Drawing.Point(0, 0);
		this.dgvContent.Margin = new System.Windows.Forms.Padding(1);
		this.dgvContent.Name = "dgvContent";
		this.dgvContent.RowHeadersVisible = false;
		this.dgvContent.Size = new System.Drawing.Size(395, 22);
		this.dgvContent.TabIndex = 1;
		this.dgvContent.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(dgvContent_CellEndEdit);
		this.dgvContent.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(dgvContent_EditingControlShowing);
		this.dgvContent.Leave += new System.EventHandler(dgvNormal_Leave);
		dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
		this.ContentTitle.DefaultCellStyle = dataGridViewCellStyle2;
		this.ContentTitle.HeaderText = "Title";
		this.ContentTitle.Name = "ContentTitle";
		this.ContentTitle.ReadOnly = true;
		this.ContentTitle.Resizable = System.Windows.Forms.DataGridViewTriState.False;
		this.ContentTitle.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.ContentTitle.Width = 120;
		this.FooterValue.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
		this.FooterValue.DefaultCellStyle = dataGridViewCellStyle3;
		this.FooterValue.HeaderText = "Value";
		this.FooterValue.Name = "FooterValue";
		this.FooterValue.ReadOnly = true;
		this.FooterValue.Resizable = System.Windows.Forms.DataGridViewTriState.False;
		this.FooterValue.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
		this.FooterTitle.DefaultCellStyle = dataGridViewCellStyle4;
		this.FooterTitle.HeaderText = "Title";
		this.FooterTitle.Name = "FooterTitle";
		this.FooterTitle.ReadOnly = true;
		this.FooterTitle.Resizable = System.Windows.Forms.DataGridViewTriState.False;
		this.FooterTitle.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.FooterTitle.Width = 120;
		this.FooterName.HeaderText = "Name";
		this.FooterName.Name = "FooterName";
		this.FooterName.ReadOnly = true;
		this.FooterName.Visible = false;
		this.dgvFooter.AllowUserToAddRows = false;
		this.dgvFooter.AllowUserToDeleteRows = false;
		this.dgvFooter.AllowUserToResizeColumns = false;
		this.dgvFooter.AllowUserToResizeRows = false;
		this.dgvFooter.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
		this.dgvFooter.ColumnHeadersVisible = false;
		this.dgvFooter.Columns.AddRange(this.FooterName, this.FooterTitle, this.FooterValue);
		this.dgvFooter.Dock = System.Windows.Forms.DockStyle.Top;
		this.dgvFooter.Location = new System.Drawing.Point(0, 22);
		this.dgvFooter.Margin = new System.Windows.Forms.Padding(1);
		this.dgvFooter.Name = "dgvFooter";
		this.dgvFooter.ReadOnly = true;
		this.dgvFooter.RowHeadersVisible = false;
		this.dgvFooter.ScrollBars = System.Windows.Forms.ScrollBars.None;
		this.dgvFooter.Size = new System.Drawing.Size(395, 22);
		this.dgvFooter.TabIndex = 2;
		this.dgvFooter.Leave += new System.EventHandler(dgvNormal_Leave);
		this.panelView.AutoScroll = true;
		this.panelView.Controls.Add(this.dgvFooter);
		this.panelView.Controls.Add(this.dgvContent);
		this.panelView.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panelView.Location = new System.Drawing.Point(3, 28);
		this.panelView.Name = "panelView";
		this.panelView.Size = new System.Drawing.Size(395, 280);
		this.panelView.TabIndex = 147;
		this.panelResize.Dock = System.Windows.Forms.DockStyle.Left;
		this.panelResize.Location = new System.Drawing.Point(0, 28);
		this.panelResize.Margin = new System.Windows.Forms.Padding(1);
		this.panelResize.Name = "panelResize";
		this.panelResize.Size = new System.Drawing.Size(3, 280);
		this.panelResize.TabIndex = 146;
		this.btnConfirm.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnConfirm.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.btnConfirm.FlatAppearance.BorderSize = 0;
		this.btnConfirm.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gray;
		this.btnConfirm.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gray;
		this.btnConfirm.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnConfirm.Location = new System.Drawing.Point(0, 308);
		this.btnConfirm.Margin = new System.Windows.Forms.Padding(1);
		this.btnConfirm.Name = "btnConfirm";
		this.btnConfirm.Size = new System.Drawing.Size(398, 28);
		this.btnConfirm.TabIndex = 143;
		this.btnConfirm.Text = "Confirm";
		this.toolTipMain.SetToolTip(this.btnConfirm, "Confirm");
		this.btnConfirm.UseVisualStyleBackColor = true;
		this.btnConfirm.Click += new System.EventHandler(btnConfirm_Click);
		this.btnDown.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnDown.Dock = System.Windows.Forms.DockStyle.Fill;
		this.btnDown.FlatAppearance.BorderSize = 0;
		this.btnDown.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.AppWorkspace;
		this.btnDown.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Control;
		this.btnDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnDown.Image = _5S_QA_System.Properties.Resources.arrow_down;
		this.btnDown.Location = new System.Drawing.Point(375, 1);
		this.btnDown.Margin = new System.Windows.Forms.Padding(1);
		this.btnDown.Name = "btnDown";
		this.btnDown.Size = new System.Drawing.Size(22, 25);
		this.btnDown.TabIndex = 129;
		this.btnDown.TabStop = false;
		this.toolTipMain.SetToolTip(this.btnDown, "Display lower row item");
		this.btnDown.UseVisualStyleBackColor = false;
		this.btnDown.Click += new System.EventHandler(btnDown_Click);
		this.btnUp.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnUp.Dock = System.Windows.Forms.DockStyle.Fill;
		this.btnUp.FlatAppearance.BorderSize = 0;
		this.btnUp.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.AppWorkspace;
		this.btnUp.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Control;
		this.btnUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUp.Image = _5S_QA_System.Properties.Resources.arrow_up;
		this.btnUp.Location = new System.Drawing.Point(351, 1);
		this.btnUp.Margin = new System.Windows.Forms.Padding(1);
		this.btnUp.Name = "btnUp";
		this.btnUp.Size = new System.Drawing.Size(22, 25);
		this.btnUp.TabIndex = 128;
		this.btnUp.TabStop = false;
		this.toolTipMain.SetToolTip(this.btnUp, "Display upper row item");
		this.btnUp.UseVisualStyleBackColor = false;
		this.btnUp.Click += new System.EventHandler(btnUp_Click);
		this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(0, 27);
		this.panel1.Margin = new System.Windows.Forms.Padding(0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(398, 1);
		this.panel1.TabIndex = 145;
		this.lblValue.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblValue.ForeColor = System.Drawing.Color.Red;
		this.lblValue.Location = new System.Drawing.Point(43, 1);
		this.lblValue.Margin = new System.Windows.Forms.Padding(1);
		this.lblValue.Name = "lblValue";
		this.lblValue.Size = new System.Drawing.Size(306, 25);
		this.lblValue.TabIndex = 132;
		this.lblValue.Text = "...";
		this.lblValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblTitle.AutoSize = true;
		this.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblTitle.Location = new System.Drawing.Point(1, 1);
		this.lblTitle.Margin = new System.Windows.Forms.Padding(1);
		this.lblTitle.Name = "lblTitle";
		this.lblTitle.Size = new System.Drawing.Size(40, 25);
		this.lblTitle.TabIndex = 0;
		this.lblTitle.Text = "View";
		this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.tpanelTitle.AutoSize = true;
		this.tpanelTitle.ColumnCount = 4;
		this.tpanelTitle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelTitle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100f));
		this.tpanelTitle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelTitle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelTitle.Controls.Add(this.lblValue, 1, 0);
		this.tpanelTitle.Controls.Add(this.btnDown, 3, 0);
		this.tpanelTitle.Controls.Add(this.btnUp, 2, 0);
		this.tpanelTitle.Controls.Add(this.lblTitle, 0, 0);
		this.tpanelTitle.Dock = System.Windows.Forms.DockStyle.Top;
		this.tpanelTitle.Location = new System.Drawing.Point(0, 0);
		this.tpanelTitle.Margin = new System.Windows.Forms.Padding(0);
		this.tpanelTitle.Name = "tpanelTitle";
		this.tpanelTitle.RowCount = 1;
		this.tpanelTitle.RowStyles.Add(new System.Windows.Forms.RowStyle());
		this.tpanelTitle.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27f));
		this.tpanelTitle.Size = new System.Drawing.Size(398, 27);
		this.tpanelTitle.TabIndex = 144;
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		base.Controls.Add(this.panelView);
		base.Controls.Add(this.panelResize);
		base.Controls.Add(this.btnConfirm);
		base.Controls.Add(this.panel1);
		base.Controls.Add(this.tpanelTitle);
		this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.Name = "mPanelSimilar";
		base.Size = new System.Drawing.Size(398, 336);
		base.Load += new System.EventHandler(mPanelSimilar_Load);
		((System.ComponentModel.ISupportInitialize)this.dgvContent).EndInit();
		((System.ComponentModel.ISupportInitialize)this.dgvFooter).EndInit();
		this.panelView.ResumeLayout(false);
		this.tpanelTitle.ResumeLayout(false);
		this.tpanelTitle.PerformLayout();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
