using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using _5S_QA_DL.APIContext;
using _5S_QA_Entities.Constants;
using _5S_QA_Entities.Dtos;
using _5S_QA_Entities.Models;
using _5S_QA_System.Controls;
using _5S_QA_System.Properties;

namespace _5S_QA_System.View.User_control;

public class mPanelView : UserControl
{
	private Type mType;

	private DataGridView dgvMain;

	private Bitmap bitmap;

	private bool isNullBitmap;

	private IContainer components = null;

	private ToolTip toolTipMain;

	private TableLayoutPanel tblPanelTitle;

	private Button btnDown;

	private Label lblValue;

	private Button btnUp;

	private Label lblTitle;

	private TableLayoutPanel tblPanelFooter;

	private Panel panelTitle;

	private Panel panelFooter;

	private Panel panelResize;

	private Panel panelView;

	private DataGridView dgvContent;

	private DataGridView dgvFooter;

	private DataGridView dgvOther;

	private DataGridViewTextBoxColumn footer_title;

	private DataGridViewTextBoxColumn footer_value;

	private DataGridViewTextBoxColumn content_title;

	private DataGridViewTextBoxColumn content_value;

	private DataGridViewTextBoxColumn other_title;

	private DataGridViewImageColumn other_value;

	public Button btnEdit;

	public Button btnDelete;

	public Button btnView;

	private DataGridView dgvMeas;

	private TableLayoutPanel tpanelMeas;

	private Label lblMeasTotalRow;

	private Label lblMeasTotal;

	private Label lblMeas;

	private DataGridView dgvPlan;

	private TableLayoutPanel tpanelPlan;

	private Label lblPlanTotalRow;

	private Label lblPlanTotal;

	private Label lblPlan;

	private DataGridViewTextBoxColumn No;

	private DataGridViewTextBoxColumn Code;

	private DataGridViewTextBoxColumn name;

	private DataGridViewTextBoxColumn Value;

	private DataGridViewTextBoxColumn UnitName;

	private DataGridViewTextBoxColumn Upper;

	private DataGridViewTextBoxColumn Lower;

	private DataGridViewTextBoxColumn MachineTypeName;

	private DataGridViewTextBoxColumn Id;

	private DataGridViewTextBoxColumn StageNo;

	private DataGridViewTextBoxColumn StageName;

	private DataGridViewTextBoxColumn TemplateName;

	private DataGridViewTextBoxColumn TotalDetails;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn17;

	private DataGridView dgvSimilar;

	private TableLayoutPanel tpanelSimilar;

	private Label lblSimilarTotalRow;

	private Label lblSimilarTotal;

	private Label lblSimilar;

	private DataGridViewTextBoxColumn SimilarNo;

	private DataGridViewTextBoxColumn SimilarProductCode;

	private DataGridViewTextBoxColumn SimilarProductName;

	private DataGridView dgvTemplate;

	private TableLayoutPanel tpanelTemplate;

	private Label lblTemplateTotalRow;

	private Label lblTemplateTotal;

	private Label lblTemplate;

	private DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn1;

	private DataGridViewTextBoxColumn TemplateNo;

	private DataGridViewTextBoxColumn TemplateOtherName;

	private DataGridViewTextBoxColumn TemplateDescription;

	public Button btnFinish;

	private DataGridView dgvCalibration;

	private TableLayoutPanel tpanelCalibration;

	private Label lblCalTotalRow;

	private Label lblCalTotal;

	private Label lblCalibration;

	private DataGridView dgvAQL;

	private TableLayoutPanel tpanelAQL;

	private Label lblAQLTotalRow;

	private Label lblAQLTotal;

	private Label lblAQL;

	private DataGridViewTextBoxColumn AQLNo;

	private DataGridViewTextBoxColumn Type;

	private DataGridViewTextBoxColumn InputQuantity;

	private DataGridViewTextBoxColumn Sample;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;

	private DataGridViewTextBoxColumn CalNo;

	private DataGridViewTextBoxColumn CalCalibrationNo;

	private DataGridViewTextBoxColumn CalType;

	private DataGridViewTextBoxColumn CalCompany;

	private DataGridViewTextBoxColumn CalStaff;

	private DataGridViewTextBoxColumn CalCalDate;

	private DataGridViewTextBoxColumn CalExpDate;

	private DataGridViewTextBoxColumn CalPeriod;

	private DataGridViewTextBoxColumn CalId;

	private DataGridView dgvStage;

	private TableLayoutPanel tpanelStage;

	private Label lblStageTotalRow;

	private Label lblStageTotal;

	private Label lblStage;

	private DataGridViewTextBoxColumn ProNo;

	private DataGridViewTextBoxColumn ProCode;

	private DataGridViewTextBoxColumn ProName;

	private DataGridViewTextBoxColumn GroupCodeName;

	private DataGridViewTextBoxColumn ProImageUrl;

	private DataGridViewTextBoxColumn ProTotalMeas;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn10;

	public mPanelView()
	{
		InitializeComponent();
		Common.setControls(this, toolTipMain);
	}

	private void mPanelView_Load(object sender, EventArgs e)
	{
		Init();
	}

	private void Init()
	{
		ControlResize.Init(panelResize, this, ControlResize.Direction.Horizontal, Cursors.SizeWE);
		mType = base.ParentForm.GetType();
		if (mType.Equals(typeof(frmPlanDetailView)))
		{
			tblPanelFooter.Visible = false;
		}
		else if (mType.Equals(typeof(frmProductView)) || mType.Equals(typeof(frmPlanView)))
		{
			if (mType.Equals(typeof(frmPlanView)))
			{
				btnView.Text = Common.getTextLanguage(this, "ViewDetail");
				toolTipMain.SetToolTip(btnView, Common.getTextLanguage(this, "tooltipViewDetail"));
			}
			else
			{
				tpanelAQL.Visible = true;
				dgvAQL.Visible = true;
				tpanelPlan.Visible = true;
				dgvPlan.Visible = true;
				tpanelSimilar.Visible = true;
				dgvSimilar.Visible = true;
				tpanelTemplate.Visible = true;
				dgvTemplate.Visible = true;
			}
			btnView.Visible = true;
			tpanelMeas.Visible = true;
			dgvMeas.Visible = true;
		}
		else if (mType.Equals(typeof(frmMachineView)))
		{
			tpanelCalibration.Visible = true;
			dgvCalibration.Visible = true;
		}
		else if (mType.Equals(typeof(frmProductGroupView)))
		{
			tpanelStage.Visible = true;
			dgvStage.Visible = true;
		}
		dgvMain = base.Parent.Controls["dgvMain"] as DataGridView;
	}

	private void load_dgvMeas()
	{
		try
		{
			QueryArgs queryArgs = new QueryArgs();
			queryArgs.Predicate = "ProductId=@0";
			queryArgs.PredicateParameters = new string[1] { dgvMain.CurrentRow.Cells["Id"].Value.ToString() };
			queryArgs.Order = "Sort, Created";
			queryArgs.Page = 1;
			queryArgs.Limit = int.MaxValue;
			QueryArgs body = queryArgs;
			ResponseDto result = frmLogin.client.GetsAsync(body, "/api/Measurement/Gets").Result;
			dgvMeas.DataSource = Common.getDataTableNoType<MeasurementQuickViewModel>(result);
			dgvMeas.Refresh();
			lblMeasTotal.Text = dgvMeas.RowCount.ToString();
			dgvMeas_Sorted(dgvMeas, null);
		}
		catch (Exception ex)
		{
			if (ex.InnerException is ApiException { StatusCode: var statusCode } ex2)
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
					MessageBox.Show(ex2.Message, Common.getTextLanguage(this, "ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
			}
			else
			{
				MessageBox.Show(ex.Message, Common.getTextLanguage(this, "ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}
	}

	private void load_dgvPlanDetail()
	{
		try
		{
			QueryArgs queryArgs = new QueryArgs();
			queryArgs.Predicate = "PlanId=@0";
			queryArgs.PredicateParameters = new string[1] { dgvMain.CurrentRow.Cells["Id"].Value.ToString() };
			queryArgs.Order = "Measurement.Sort, Measurement.Created";
			queryArgs.Page = 1;
			queryArgs.Limit = int.MaxValue;
			QueryArgs body = queryArgs;
			ResponseDto result = frmLogin.client.GetsAsync(body, "/api/PlanDetail/Gets").Result;
			dgvMeas.DataSource = Common.getDataTable<MeasurementQuickViewModel>(result);
			dgvMeas.Refresh();
			lblMeasTotal.Text = dgvMeas.RowCount.ToString();
		}
		catch (Exception ex)
		{
			if (ex.InnerException is ApiException { StatusCode: var statusCode } ex2)
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
					MessageBox.Show(ex2.Message, Common.getTextLanguage(this, "ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
			}
			else
			{
				MessageBox.Show(ex.Message, Common.getTextLanguage(this, "ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}
	}

	private void load_dgvPlan()
	{
		try
		{
			QueryArgs queryArgs = new QueryArgs();
			queryArgs.Predicate = "ProductId=@0";
			queryArgs.PredicateParameters = new string[1] { dgvMain.CurrentRow.Cells["Id"].Value.ToString() };
			queryArgs.Order = "Sort, Created";
			queryArgs.Page = 1;
			queryArgs.Limit = int.MaxValue;
			QueryArgs body = queryArgs;
			ResponseDto result = frmLogin.client.GetsAsync(body, "/api/Plan/Gets").Result;
			dgvPlan.DataSource = Common.getDataTable<PlanQuickViewModel>(result);
			dgvPlan.Refresh();
			lblPlanTotal.Text = dgvPlan.RowCount.ToString();
		}
		catch (Exception ex)
		{
			if (ex.InnerException is ApiException { StatusCode: var statusCode } ex2)
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
					MessageBox.Show(ex2.Message, Common.getTextLanguage(this, "ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
			}
			else
			{
				MessageBox.Show(ex.Message, Common.getTextLanguage(this, "ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}
	}

	private void load_dgvSimilar()
	{
		try
		{
			Guid.TryParse(dgvMain.CurrentRow.Cells["Id"].Value.ToString(), out var result);
			ResponseDto result2 = frmLogin.client.GetsAsync(result, "/api/Similar/Gets/{id}").Result;
			dgvSimilar.DataSource = Common.getDataTable<SimilarQuickViewModel>(result2);
			dgvSimilar.Refresh();
			lblSimilarTotal.Text = dgvSimilar.RowCount.ToString();
		}
		catch (Exception ex)
		{
			if (ex.InnerException is ApiException { StatusCode: var statusCode } ex2)
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
					MessageBox.Show(ex2.Message, Common.getTextLanguage(this, "ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
			}
			else
			{
				MessageBox.Show(ex.Message, Common.getTextLanguage(this, "ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}
	}

	private void load_dgvTemplate()
	{
		try
		{
			QueryArgs queryArgs = new QueryArgs();
			queryArgs.Predicate = "ProductId=@0";
			queryArgs.PredicateParameters = new string[1] { dgvMain.CurrentRow.Cells["Id"].Value.ToString() };
			queryArgs.Order = "Created DESC";
			queryArgs.Page = 1;
			queryArgs.Limit = int.MaxValue;
			QueryArgs body = queryArgs;
			ResponseDto result = frmLogin.client.GetsAsync(body, "/api/TemplateOther/Gets").Result;
			dgvTemplate.DataSource = Common.getDataTable<TemplateOtherQuickViewModel>(result);
			dgvTemplate.Refresh();
			lblTemplateTotal.Text = dgvTemplate.RowCount.ToString();
		}
		catch (Exception ex)
		{
			if (ex.InnerException is ApiException { StatusCode: var statusCode } ex2)
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
					MessageBox.Show(ex2.Message, Common.getTextLanguage(this, "ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
			}
			else
			{
				MessageBox.Show(ex.Message, Common.getTextLanguage(this, "ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}
	}

	private void load_dgvCalibration()
	{
		try
		{
			QueryArgs queryArgs = new QueryArgs();
			queryArgs.Predicate = "MachineId=@0";
			queryArgs.PredicateParameters = new string[1] { dgvMain.CurrentRow.Cells["Id"].Value.ToString() };
			queryArgs.Order = "Created DESC";
			queryArgs.Page = 1;
			queryArgs.Limit = int.MaxValue;
			QueryArgs body = queryArgs;
			ResponseDto result = frmLogin.client.GetsAsync(body, "/api/Calibration/Gets").Result;
			DataTable dataTable = Common.getDataTable<CalibrationQuickViewModel>(result);
			dgvCalibration.DataSource = dataTable;
			dgvCalibration.Refresh();
			lblCalTotal.Text = dgvCalibration.RowCount.ToString();
			dgvCalibration_Sorted(dgvCalibration, null);
		}
		catch (Exception ex)
		{
			if (ex.InnerException is ApiException { StatusCode: var statusCode } ex2)
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
					MessageBox.Show(ex2.Message, Common.getTextLanguage(this, "ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
			}
			else
			{
				MessageBox.Show(ex.Message, Common.getTextLanguage(this, "ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}
	}

	private void load_dgvAQL()
	{
		try
		{
			QueryArgs queryArgs = new QueryArgs();
			queryArgs.Predicate = "ProductId=@0";
			queryArgs.PredicateParameters = new string[1] { dgvMain.CurrentRow.Cells["Id"].Value.ToString() };
			queryArgs.Order = "Sort, Created";
			queryArgs.Page = 1;
			queryArgs.Limit = int.MaxValue;
			QueryArgs body = queryArgs;
			ResponseDto result = frmLogin.client.GetsAsync(body, "/api/AQL/Gets").Result;
			dgvAQL.DataSource = Common.getDataTable<AQLQuickViewModel>(result);
			dgvAQL.Refresh();
			lblAQLTotal.Text = dgvAQL.RowCount.ToString();
		}
		catch (Exception ex)
		{
			if (ex.InnerException is ApiException { StatusCode: var statusCode } ex2)
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
					MessageBox.Show(ex2.Message, Common.getTextLanguage(this, "ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
			}
			else
			{
				MessageBox.Show(ex.Message, Common.getTextLanguage(this, "ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}
	}

	private void load_dgvProduct()
	{
		try
		{
			QueryArgs queryArgs = new QueryArgs();
			queryArgs.Predicate = "GroupId=@0";
			queryArgs.PredicateParameters = new string[1] { dgvMain.CurrentRow.Cells["Id"].Value.ToString() };
			queryArgs.Order = "Sort, Created";
			queryArgs.Page = 1;
			queryArgs.Limit = int.MaxValue;
			QueryArgs body = queryArgs;
			ResponseDto result = frmLogin.client.GetsAsync(body, "/api/Product/Gets").Result;
			dgvStage.DataSource = Common.getDataTable<ProductQuickViewModel>(result);
			dgvStage.Refresh();
			lblStageTotal.Text = dgvStage.RowCount.ToString();
		}
		catch (Exception ex)
		{
			if (ex.InnerException is ApiException { StatusCode: var statusCode } ex2)
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
					MessageBox.Show(ex2.Message, Common.getTextLanguage(this, "ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
			}
			else
			{
				MessageBox.Show(ex.Message, Common.getTextLanguage(this, "ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}
	}

	public void mDispose()
	{
		List<Type> list = new List<Type>();
		list.Add(typeof(frmZoomView));
		Common.closeForm(list);
	}

	public void Display()
	{
		try
		{
			lblValue.Text = dgvMain.CurrentRow.Cells["Id"].Value.ToString();
			dgvContent.Rows.Clear();
			dgvOther.Rows.Clear();
			dgvFooter.Rows.Clear();
			foreach (DataGridViewColumn col in dgvMain.Columns)
			{
				if (MetaType.dgvContent.Find((string x) => x.Equals(col.Name)) != null)
				{
					dgvContent.Visible = true;
					dgvContent.Rows.Add(col.HeaderText, dgvMain.CurrentRow.Cells[col.Name].Value);
				}
				else if (MetaType.dgvOther.Find((string x) => x.Equals(col.Name)) != null)
				{
					dgvOther.Visible = true;
					try
					{
						string value = dgvMain.CurrentRow.Cells[col.Name].Value.ToString();
						StringBuilder stringBuilder = new StringBuilder();
						if (mType.Equals(typeof(frmProductView)))
						{
							stringBuilder.Append(APIUrl.APIHost + "/ProductImage/").Append(value);
						}
						else
						{
							stringBuilder.Append(APIUrl.APIHost + "/AuthUserImage/").Append(value);
						}
						PictureBox pictureBox = new PictureBox();
						pictureBox.Load(stringBuilder.ToString());
						bitmap = (Bitmap)pictureBox.Image;
						pictureBox.Dispose();
						isNullBitmap = false;
					}
					catch
					{
						bitmap = Resources._5S_QA_S;
						isNullBitmap = true;
					}
					dgvOther.Rows.Add(col.HeaderText, bitmap);
				}
				else if (MetaType.dgvFooter.Find((string x) => x.Equals(col.Name)) != null)
				{
					dgvFooter.Visible = true;
					dgvFooter.Rows.Add(col.HeaderText, dgvMain.CurrentRow.Cells[col.Name].Value);
				}
			}
			if (mType.Equals(typeof(frmProductView)))
			{
				load_dgvMeas();
				load_dgvPlan();
				load_dgvSimilar();
				load_dgvTemplate();
				load_dgvAQL();
			}
			else if (mType.Equals(typeof(frmPlanView)))
			{
				load_dgvPlanDetail();
			}
			else if (mType.Equals(typeof(frmMachineView)))
			{
				load_dgvCalibration();
			}
			else if (mType.Equals(typeof(frmProductGroupView)))
			{
				load_dgvProduct();
				dgvStage_Sorted(dgvStage, null);
			}
		}
		finally
		{
			dgvContent.Size = new Size(panelView.Width, dgvContent.Rows.Count * 22 + 3);
			dgvOther.Size = new Size(panelView.Width, dgvOther.Rows.Count * 100 + 3);
			dgvFooter.Size = new Size(panelView.Width, dgvFooter.Rows.Count * 22 + 3);
			dgvMeas.Size = new Size(panelView.Width, 22 + dgvMeas.Rows.Count * 22 + 3);
			dgvAQL.Size = new Size(panelView.Width, 22 + dgvAQL.Rows.Count * 22 + 3);
			dgvPlan.Size = new Size(panelView.Width, 22 + dgvPlan.Rows.Count * 22 + 3);
			dgvSimilar.Size = new Size(panelView.Width, 22 + dgvSimilar.Rows.Count * 22 + 3);
			dgvTemplate.Size = new Size(panelView.Width, 22 + dgvTemplate.Rows.Count * 22 + 3);
			dgvCalibration.Size = new Size(panelView.Width, 22 + dgvCalibration.Rows.Count * 22 + 3);
			dgvStage.Size = new Size(panelView.Width, 22 + dgvStage.Rows.Count * 22 + 3);
			dgvContent.CurrentCell = null;
			dgvContent.Refresh();
			dgvOther.CurrentCell = null;
			dgvOther.Refresh();
			dgvFooter.CurrentCell = null;
			dgvFooter.Refresh();
			dgvMeas.CurrentCell = null;
			dgvMeas.Refresh();
			dgvAQL.CurrentCell = null;
			dgvAQL.Refresh();
			dgvPlan.CurrentCell = null;
			dgvPlan.Refresh();
			dgvSimilar.CurrentCell = null;
			dgvSimilar.Refresh();
			dgvTemplate.CurrentCell = null;
			dgvTemplate.Refresh();
			dgvCalibration.CurrentCell = null;
			dgvCalibration.Refresh();
			dgvStage.CurrentCell = null;
			dgvStage.Refresh();
			dgvContent.SendToBack();
			dgvFooter.BringToFront();
			tpanelAQL.BringToFront();
			dgvAQL.BringToFront();
			tpanelPlan.BringToFront();
			dgvPlan.BringToFront();
			tpanelSimilar.BringToFront();
			dgvSimilar.BringToFront();
			tpanelTemplate.BringToFront();
			dgvTemplate.BringToFront();
			tpanelMeas.BringToFront();
			dgvMeas.BringToFront();
			tpanelCalibration.BringToFront();
			dgvCalibration.BringToFront();
			tpanelStage.BringToFront();
			dgvStage.BringToFront();
		}
	}

	private void dgvNormal_Leave(object sender, EventArgs e)
	{
		DataGridView dataGridView = sender as DataGridView;
		dataGridView.CurrentCell = null;
	}

	private void btnUp_Click(object sender, EventArgs e)
	{
		dgvMain.Select();
		SendKeys.SendWait("{up}");
	}

	private void btnDown_Click(object sender, EventArgs e)
	{
		dgvMain.Select();
		SendKeys.SendWait("{down}");
	}

	private void dgvOther_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
	{
		if (!isNullBitmap && !Common.activeForm(typeof(frmZoomView)))
		{
			frmZoomView frmZoomView = new frmZoomView(bitmap, lblValue.Text);
			frmZoomView.Show();
		}
	}

	private void dgvMeas_Sorted(object sender, EventArgs e)
	{
		DataGridView dataGridView = sender as DataGridView;
		foreach (DataGridViewRow item in (IEnumerable)dataGridView.Rows)
		{
			item.Cells["No"].Value = item.Index + 1;
			if (item.Cells["UnitName"].Value?.ToString() == "°")
			{
				if (double.TryParse(item.Cells["Value"].Value?.ToString(), out var result))
				{
					item.Cells["Value"].Value = Common.ConvertDoubleToDegrees(result);
				}
				if (double.TryParse(item.Cells["Upper"].Value?.ToString(), out var result2))
				{
					item.Cells["Upper"].Value = Common.ConvertDoubleToDegrees(result2);
				}
				if (double.TryParse(item.Cells["Lower"].Value?.ToString(), out var result3))
				{
					item.Cells["Lower"].Value = Common.ConvertDoubleToDegrees(result3);
				}
			}
		}
	}

	private void dgvPlan_Sorted(object sender, EventArgs e)
	{
		DataGridView dataGridView = sender as DataGridView;
		foreach (DataGridViewRow item in (IEnumerable)dataGridView.Rows)
		{
			item.Cells["StageNo"].Value = item.Index + 1;
		}
	}

	private void dgvSimilar_Sorted(object sender, EventArgs e)
	{
		DataGridView dataGridView = sender as DataGridView;
		foreach (DataGridViewRow item in (IEnumerable)dataGridView.Rows)
		{
			item.Cells["SimilarNo"].Value = item.Index + 1;
		}
	}

	private void dgvDetail_Sorted(object sender, EventArgs e)
	{
		DataGridView dataGridView = sender as DataGridView;
		foreach (DataGridViewRow item in (IEnumerable)dataGridView.Rows)
		{
			item.Cells["DeliNo"].Value = item.Index + 1;
			if (item.Cells["DeliStatus"].Value.ToString() == "ACCEPTABLE")
			{
				item.Cells["DeliStatus"].Style.ForeColor = Color.Orange;
			}
		}
	}

	private void dgvTemplate_Sorted(object sender, EventArgs e)
	{
		DataGridView dataGridView = sender as DataGridView;
		foreach (DataGridViewRow item in (IEnumerable)dataGridView.Rows)
		{
			item.Cells["TemplateNo"].Value = item.Index + 1;
		}
	}

	private void dgvCalibration_Sorted(object sender, EventArgs e)
	{
		DataGridView dataGridView = sender as DataGridView;
		foreach (DataGridViewRow item in (IEnumerable)dataGridView.Rows)
		{
			item.Cells["CalNo"].Value = item.Index + 1;
			if (DateTime.Parse(item.Cells["CalExpDate"].Value.ToString()) >= DateTime.Now.AddMonths(-1) && DateTime.Parse(item.Cells["CalExpDate"].Value.ToString()) < DateTime.Now)
			{
				item.DefaultCellStyle.ForeColor = Color.Orange;
			}
			else if (DateTime.Parse(item.Cells["CalExpDate"].Value.ToString()) < DateTime.Now)
			{
				item.DefaultCellStyle.ForeColor = Color.Red;
			}
		}
	}

	private void dgvAQL_Sorted(object sender, EventArgs e)
	{
		DataGridView dataGridView = sender as DataGridView;
		foreach (DataGridViewRow item in (IEnumerable)dataGridView.Rows)
		{
			item.Cells["AQLNo"].Value = item.Index + 1;
		}
	}

	private void dgvStage_Sorted(object sender, EventArgs e)
	{
		DataGridView dataGridView = sender as DataGridView;
		foreach (DataGridViewRow item in (IEnumerable)dataGridView.Rows)
		{
			item.Cells["ProNo"].Value = item.Index + 1;
			if (int.Parse(item.Cells["ProTotalMeas"].Value.ToString()) == 0)
			{
				item.DefaultCellStyle.ForeColor = SystemColors.GrayText;
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
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle16 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle17 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle18 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle19 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle20 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle21 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle22 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle23 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle24 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle25 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle26 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle27 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle28 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle29 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle30 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle31 = new System.Windows.Forms.DataGridViewCellStyle();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(_5S_QA_System.View.User_control.mPanelView));
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle32 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle33 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle34 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle35 = new System.Windows.Forms.DataGridViewCellStyle();
		this.toolTipMain = new System.Windows.Forms.ToolTip(this.components);
		this.btnEdit = new System.Windows.Forms.Button();
		this.btnDelete = new System.Windows.Forms.Button();
		this.btnView = new System.Windows.Forms.Button();
		this.btnDown = new System.Windows.Forms.Button();
		this.btnUp = new System.Windows.Forms.Button();
		this.btnFinish = new System.Windows.Forms.Button();
		this.tblPanelTitle = new System.Windows.Forms.TableLayoutPanel();
		this.lblValue = new System.Windows.Forms.Label();
		this.lblTitle = new System.Windows.Forms.Label();
		this.tblPanelFooter = new System.Windows.Forms.TableLayoutPanel();
		this.panelTitle = new System.Windows.Forms.Panel();
		this.panelFooter = new System.Windows.Forms.Panel();
		this.panelResize = new System.Windows.Forms.Panel();
		this.panelView = new System.Windows.Forms.Panel();
		this.dgvStage = new System.Windows.Forms.DataGridView();
		this.tpanelStage = new System.Windows.Forms.TableLayoutPanel();
		this.lblStageTotalRow = new System.Windows.Forms.Label();
		this.lblStageTotal = new System.Windows.Forms.Label();
		this.lblStage = new System.Windows.Forms.Label();
		this.dgvCalibration = new System.Windows.Forms.DataGridView();
		this.CalNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.CalCalibrationNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.CalType = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.CalCompany = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.CalStaff = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.CalCalDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.CalExpDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.CalPeriod = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.CalId = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.tpanelCalibration = new System.Windows.Forms.TableLayoutPanel();
		this.lblCalTotalRow = new System.Windows.Forms.Label();
		this.lblCalTotal = new System.Windows.Forms.Label();
		this.lblCalibration = new System.Windows.Forms.Label();
		this.dgvMeas = new System.Windows.Forms.DataGridView();
		this.No = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Code = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Value = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.UnitName = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Upper = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Lower = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.MachineTypeName = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.tpanelMeas = new System.Windows.Forms.TableLayoutPanel();
		this.lblMeasTotalRow = new System.Windows.Forms.Label();
		this.lblMeasTotal = new System.Windows.Forms.Label();
		this.lblMeas = new System.Windows.Forms.Label();
		this.dgvTemplate = new System.Windows.Forms.DataGridView();
		this.dataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
		this.TemplateNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.TemplateOtherName = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.TemplateDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.tpanelTemplate = new System.Windows.Forms.TableLayoutPanel();
		this.lblTemplateTotalRow = new System.Windows.Forms.Label();
		this.lblTemplateTotal = new System.Windows.Forms.Label();
		this.lblTemplate = new System.Windows.Forms.Label();
		this.dgvSimilar = new System.Windows.Forms.DataGridView();
		this.SimilarNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.SimilarProductCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.SimilarProductName = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.tpanelSimilar = new System.Windows.Forms.TableLayoutPanel();
		this.lblSimilarTotalRow = new System.Windows.Forms.Label();
		this.lblSimilarTotal = new System.Windows.Forms.Label();
		this.lblSimilar = new System.Windows.Forms.Label();
		this.dgvPlan = new System.Windows.Forms.DataGridView();
		this.StageNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.StageName = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.TemplateName = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.TotalDetails = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dataGridViewTextBoxColumn17 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.tpanelPlan = new System.Windows.Forms.TableLayoutPanel();
		this.lblPlanTotalRow = new System.Windows.Forms.Label();
		this.lblPlanTotal = new System.Windows.Forms.Label();
		this.lblPlan = new System.Windows.Forms.Label();
		this.dgvAQL = new System.Windows.Forms.DataGridView();
		this.AQLNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Type = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.InputQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Sample = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.tpanelAQL = new System.Windows.Forms.TableLayoutPanel();
		this.lblAQLTotalRow = new System.Windows.Forms.Label();
		this.lblAQLTotal = new System.Windows.Forms.Label();
		this.lblAQL = new System.Windows.Forms.Label();
		this.dgvFooter = new System.Windows.Forms.DataGridView();
		this.footer_title = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.footer_value = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dgvOther = new System.Windows.Forms.DataGridView();
		this.other_title = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.other_value = new System.Windows.Forms.DataGridViewImageColumn();
		this.dgvContent = new System.Windows.Forms.DataGridView();
		this.content_title = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.content_value = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.ProNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.ProCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.ProName = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.GroupCodeName = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.ProImageUrl = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.ProTotalMeas = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dataGridViewTextBoxColumn10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.tblPanelTitle.SuspendLayout();
		this.tblPanelFooter.SuspendLayout();
		this.panelView.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.dgvStage).BeginInit();
		this.tpanelStage.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.dgvCalibration).BeginInit();
		this.tpanelCalibration.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.dgvMeas).BeginInit();
		this.tpanelMeas.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.dgvTemplate).BeginInit();
		this.tpanelTemplate.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.dgvSimilar).BeginInit();
		this.tpanelSimilar.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.dgvPlan).BeginInit();
		this.tpanelPlan.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.dgvAQL).BeginInit();
		this.tpanelAQL.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.dgvFooter).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.dgvOther).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.dgvContent).BeginInit();
		base.SuspendLayout();
		this.btnEdit.AutoSize = true;
		this.btnEdit.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnEdit.FlatAppearance.BorderSize = 0;
		this.btnEdit.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gray;
		this.btnEdit.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gray;
		this.btnEdit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnEdit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnEdit.Location = new System.Drawing.Point(3, 3);
		this.btnEdit.Name = "btnEdit";
		this.btnEdit.Size = new System.Drawing.Size(44, 26);
		this.btnEdit.TabIndex = 1;
		this.btnEdit.Text = "Edit";
		this.toolTipMain.SetToolTip(this.btnEdit, "Goto edit information");
		this.btnEdit.UseVisualStyleBackColor = true;
		this.btnDelete.AutoSize = true;
		this.btnDelete.BackColor = System.Drawing.Color.Red;
		this.btnDelete.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnDelete.FlatAppearance.BorderSize = 0;
		this.btnDelete.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gray;
		this.btnDelete.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gray;
		this.btnDelete.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnDelete.Location = new System.Drawing.Point(53, 3);
		this.btnDelete.Name = "btnDelete";
		this.btnDelete.Size = new System.Drawing.Size(63, 26);
		this.btnDelete.TabIndex = 2;
		this.btnDelete.Text = "Delete";
		this.toolTipMain.SetToolTip(this.btnDelete, "Confirm delete");
		this.btnDelete.UseVisualStyleBackColor = true;
		this.btnView.AutoSize = true;
		this.btnView.BackColor = System.Drawing.Color.Blue;
		this.btnView.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnView.FlatAppearance.BorderSize = 0;
		this.btnView.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gray;
		this.btnView.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gray;
		this.btnView.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnView.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnView.Location = new System.Drawing.Point(122, 3);
		this.btnView.Name = "btnView";
		this.btnView.Size = new System.Drawing.Size(110, 26);
		this.btnView.TabIndex = 3;
		this.btnView.Text = "Measurement";
		this.toolTipMain.SetToolTip(this.btnView, "Goto form measurement");
		this.btnView.UseVisualStyleBackColor = true;
		this.btnView.Visible = false;
		this.btnDown.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnDown.Dock = System.Windows.Forms.DockStyle.Fill;
		this.btnDown.FlatAppearance.BorderSize = 0;
		this.btnDown.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.AppWorkspace;
		this.btnDown.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Control;
		this.btnDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnDown.ForeColor = System.Drawing.Color.White;
		this.btnDown.Image = _5S_QA_System.Properties.Resources.arrow_down;
		this.btnDown.Location = new System.Drawing.Point(577, 1);
		this.btnDown.Margin = new System.Windows.Forms.Padding(1);
		this.btnDown.Name = "btnDown";
		this.btnDown.Size = new System.Drawing.Size(22, 25);
		this.btnDown.TabIndex = 129;
		this.btnDown.TabStop = false;
		this.toolTipMain.SetToolTip(this.btnDown, "Select lower row item");
		this.btnDown.Click += new System.EventHandler(btnDown_Click);
		this.btnUp.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnUp.Dock = System.Windows.Forms.DockStyle.Fill;
		this.btnUp.FlatAppearance.BorderSize = 0;
		this.btnUp.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.AppWorkspace;
		this.btnUp.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Control;
		this.btnUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUp.Image = _5S_QA_System.Properties.Resources.arrow_up;
		this.btnUp.Location = new System.Drawing.Point(553, 1);
		this.btnUp.Margin = new System.Windows.Forms.Padding(1);
		this.btnUp.Name = "btnUp";
		this.btnUp.Size = new System.Drawing.Size(22, 25);
		this.btnUp.TabIndex = 128;
		this.btnUp.TabStop = false;
		this.toolTipMain.SetToolTip(this.btnUp, "Select upper row item");
		this.btnUp.Click += new System.EventHandler(btnUp_Click);
		this.btnFinish.AutoSize = true;
		this.btnFinish.BackColor = System.Drawing.Color.Blue;
		this.btnFinish.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnFinish.FlatAppearance.BorderSize = 0;
		this.btnFinish.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gray;
		this.btnFinish.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gray;
		this.btnFinish.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnFinish.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnFinish.Location = new System.Drawing.Point(238, 3);
		this.btnFinish.Name = "btnFinish";
		this.btnFinish.Size = new System.Drawing.Size(58, 26);
		this.btnFinish.TabIndex = 4;
		this.btnFinish.Text = "Finish";
		this.toolTipMain.SetToolTip(this.btnFinish, "Confirm finish delivery");
		this.btnFinish.UseVisualStyleBackColor = true;
		this.btnFinish.Visible = false;
		this.tblPanelTitle.AutoSize = true;
		this.tblPanelTitle.ColumnCount = 4;
		this.tblPanelTitle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tblPanelTitle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100f));
		this.tblPanelTitle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tblPanelTitle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tblPanelTitle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		this.tblPanelTitle.Controls.Add(this.btnDown, 3, 0);
		this.tblPanelTitle.Controls.Add(this.lblValue, 1, 0);
		this.tblPanelTitle.Controls.Add(this.btnUp, 2, 0);
		this.tblPanelTitle.Controls.Add(this.lblTitle, 0, 0);
		this.tblPanelTitle.Dock = System.Windows.Forms.DockStyle.Top;
		this.tblPanelTitle.Location = new System.Drawing.Point(0, 0);
		this.tblPanelTitle.Margin = new System.Windows.Forms.Padding(0);
		this.tblPanelTitle.Name = "tblPanelTitle";
		this.tblPanelTitle.RowCount = 1;
		this.tblPanelTitle.RowStyles.Add(new System.Windows.Forms.RowStyle());
		this.tblPanelTitle.Size = new System.Drawing.Size(600, 27);
		this.tblPanelTitle.TabIndex = 134;
		this.lblValue.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblValue.ForeColor = System.Drawing.Color.Red;
		this.lblValue.Location = new System.Drawing.Point(43, 1);
		this.lblValue.Margin = new System.Windows.Forms.Padding(1);
		this.lblValue.Name = "lblValue";
		this.lblValue.Size = new System.Drawing.Size(508, 25);
		this.lblValue.TabIndex = 131;
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
		this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.tblPanelFooter.AutoSize = true;
		this.tblPanelFooter.ColumnCount = 5;
		this.tblPanelFooter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tblPanelFooter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tblPanelFooter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tblPanelFooter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tblPanelFooter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100f));
		this.tblPanelFooter.Controls.Add(this.btnFinish, 3, 0);
		this.tblPanelFooter.Controls.Add(this.btnView, 2, 0);
		this.tblPanelFooter.Controls.Add(this.btnDelete, 1, 0);
		this.tblPanelFooter.Controls.Add(this.btnEdit, 0, 0);
		this.tblPanelFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.tblPanelFooter.Location = new System.Drawing.Point(0, 632);
		this.tblPanelFooter.Name = "tblPanelFooter";
		this.tblPanelFooter.RowCount = 1;
		this.tblPanelFooter.RowStyles.Add(new System.Windows.Forms.RowStyle());
		this.tblPanelFooter.Size = new System.Drawing.Size(600, 32);
		this.tblPanelFooter.TabIndex = 135;
		this.panelTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panelTitle.Dock = System.Windows.Forms.DockStyle.Top;
		this.panelTitle.Location = new System.Drawing.Point(0, 27);
		this.panelTitle.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.panelTitle.Name = "panelTitle";
		this.panelTitle.Size = new System.Drawing.Size(600, 1);
		this.panelTitle.TabIndex = 136;
		this.panelFooter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panelFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panelFooter.Location = new System.Drawing.Point(0, 631);
		this.panelFooter.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.panelFooter.Name = "panelFooter";
		this.panelFooter.Size = new System.Drawing.Size(600, 1);
		this.panelFooter.TabIndex = 137;
		this.panelResize.Dock = System.Windows.Forms.DockStyle.Left;
		this.panelResize.Location = new System.Drawing.Point(0, 28);
		this.panelResize.Margin = new System.Windows.Forms.Padding(1);
		this.panelResize.Name = "panelResize";
		this.panelResize.Size = new System.Drawing.Size(3, 603);
		this.panelResize.TabIndex = 138;
		this.panelView.AutoScroll = true;
		this.panelView.Controls.Add(this.dgvStage);
		this.panelView.Controls.Add(this.tpanelStage);
		this.panelView.Controls.Add(this.dgvCalibration);
		this.panelView.Controls.Add(this.tpanelCalibration);
		this.panelView.Controls.Add(this.dgvMeas);
		this.panelView.Controls.Add(this.tpanelMeas);
		this.panelView.Controls.Add(this.dgvTemplate);
		this.panelView.Controls.Add(this.tpanelTemplate);
		this.panelView.Controls.Add(this.dgvSimilar);
		this.panelView.Controls.Add(this.tpanelSimilar);
		this.panelView.Controls.Add(this.dgvPlan);
		this.panelView.Controls.Add(this.tpanelPlan);
		this.panelView.Controls.Add(this.dgvAQL);
		this.panelView.Controls.Add(this.tpanelAQL);
		this.panelView.Controls.Add(this.dgvFooter);
		this.panelView.Controls.Add(this.dgvOther);
		this.panelView.Controls.Add(this.dgvContent);
		this.panelView.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panelView.Location = new System.Drawing.Point(3, 28);
		this.panelView.Margin = new System.Windows.Forms.Padding(0);
		this.panelView.Name = "panelView";
		this.panelView.Size = new System.Drawing.Size(597, 603);
		this.panelView.TabIndex = 139;
		this.dgvStage.AllowUserToAddRows = false;
		this.dgvStage.AllowUserToDeleteRows = false;
		this.dgvStage.AllowUserToOrderColumns = true;
		this.dgvStage.AllowUserToResizeRows = false;
		dataGridViewCellStyle.BackColor = System.Drawing.SystemColors.Control;
		this.dgvStage.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle;
		this.dgvStage.BackgroundColor = System.Drawing.SystemColors.Window;
		dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
		dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.AppWorkspace;
		dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
		dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
		dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
		dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
		this.dgvStage.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
		this.dgvStage.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
		this.dgvStage.Columns.AddRange(this.ProNo, this.ProCode, this.ProName, this.GroupCodeName, this.ProImageUrl, this.ProTotalMeas, this.dataGridViewTextBoxColumn10);
		this.dgvStage.Dock = System.Windows.Forms.DockStyle.Top;
		this.dgvStage.EnableHeadersVisualStyles = false;
		this.dgvStage.Location = new System.Drawing.Point(0, 534);
		this.dgvStage.Margin = new System.Windows.Forms.Padding(1);
		this.dgvStage.Name = "dgvStage";
		this.dgvStage.ReadOnly = true;
		this.dgvStage.RowHeadersVisible = false;
		this.dgvStage.RowHeadersWidth = 22;
		this.dgvStage.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
		this.dgvStage.Size = new System.Drawing.Size(597, 47);
		this.dgvStage.TabIndex = 179;
		this.dgvStage.TabStop = false;
		this.dgvStage.Visible = false;
		this.dgvStage.Sorted += new System.EventHandler(dgvStage_Sorted);
		this.tpanelStage.AutoSize = true;
		this.tpanelStage.ColumnCount = 4;
		this.tpanelStage.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelStage.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100f));
		this.tpanelStage.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelStage.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelStage.Controls.Add(this.lblStageTotalRow, 2, 0);
		this.tpanelStage.Controls.Add(this.lblStageTotal, 3, 0);
		this.tpanelStage.Controls.Add(this.lblStage, 0, 0);
		this.tpanelStage.Dock = System.Windows.Forms.DockStyle.Top;
		this.tpanelStage.Location = new System.Drawing.Point(0, 509);
		this.tpanelStage.Name = "tpanelStage";
		this.tpanelStage.RowCount = 1;
		this.tpanelStage.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25f));
		this.tpanelStage.Size = new System.Drawing.Size(597, 25);
		this.tpanelStage.TabIndex = 178;
		this.tpanelStage.Visible = false;
		this.lblStageTotalRow.AutoSize = true;
		this.lblStageTotalRow.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblStageTotalRow.ForeColor = System.Drawing.Color.Black;
		this.lblStageTotalRow.Location = new System.Drawing.Point(507, 1);
		this.lblStageTotalRow.Margin = new System.Windows.Forms.Padding(1);
		this.lblStageTotalRow.Name = "lblStageTotalRow";
		this.lblStageTotalRow.Size = new System.Drawing.Size(72, 23);
		this.lblStageTotalRow.TabIndex = 133;
		this.lblStageTotalRow.Text = "Total rows:";
		this.lblStageTotalRow.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblStageTotal.AutoSize = true;
		this.lblStageTotal.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblStageTotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblStageTotal.Location = new System.Drawing.Point(581, 1);
		this.lblStageTotal.Margin = new System.Windows.Forms.Padding(1);
		this.lblStageTotal.Name = "lblStageTotal";
		this.lblStageTotal.Size = new System.Drawing.Size(15, 23);
		this.lblStageTotal.TabIndex = 132;
		this.lblStageTotal.Text = "0";
		this.lblStageTotal.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblStage.AutoSize = true;
		this.lblStage.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblStage.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblStage.Location = new System.Drawing.Point(3, 0);
		this.lblStage.Name = "lblStage";
		this.lblStage.Padding = new System.Windows.Forms.Padding(1);
		this.lblStage.Size = new System.Drawing.Size(50, 25);
		this.lblStage.TabIndex = 6;
		this.lblStage.Text = "Stage";
		this.lblStage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.dgvCalibration.AllowUserToAddRows = false;
		this.dgvCalibration.AllowUserToDeleteRows = false;
		this.dgvCalibration.AllowUserToOrderColumns = true;
		this.dgvCalibration.AllowUserToResizeRows = false;
		dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
		this.dgvCalibration.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle3;
		this.dgvCalibration.BackgroundColor = System.Drawing.SystemColors.Window;
		dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
		dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.AppWorkspace;
		dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
		dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
		dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
		dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
		this.dgvCalibration.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
		this.dgvCalibration.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
		this.dgvCalibration.Columns.AddRange(this.CalNo, this.CalCalibrationNo, this.CalType, this.CalCompany, this.CalStaff, this.CalCalDate, this.CalExpDate, this.CalPeriod, this.CalId);
		this.dgvCalibration.Dock = System.Windows.Forms.DockStyle.Top;
		this.dgvCalibration.EnableHeadersVisualStyles = false;
		this.dgvCalibration.Location = new System.Drawing.Point(0, 462);
		this.dgvCalibration.Margin = new System.Windows.Forms.Padding(1);
		this.dgvCalibration.Name = "dgvCalibration";
		this.dgvCalibration.ReadOnly = true;
		this.dgvCalibration.RowHeadersVisible = false;
		this.dgvCalibration.RowHeadersWidth = 22;
		this.dgvCalibration.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
		this.dgvCalibration.Size = new System.Drawing.Size(597, 47);
		this.dgvCalibration.TabIndex = 175;
		this.dgvCalibration.TabStop = false;
		this.dgvCalibration.Visible = false;
		this.dgvCalibration.Sorted += new System.EventHandler(dgvCalibration_Sorted);
		this.CalNo.DataPropertyName = "No";
		dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
		this.CalNo.DefaultCellStyle = dataGridViewCellStyle5;
		this.CalNo.HeaderText = "No";
		this.CalNo.Name = "CalNo";
		this.CalNo.ReadOnly = true;
		this.CalNo.Resizable = System.Windows.Forms.DataGridViewTriState.False;
		this.CalNo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.CalNo.Width = 35;
		this.CalCalibrationNo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.CalCalibrationNo.DataPropertyName = "CalibrationNo";
		this.CalCalibrationNo.FillWeight = 20f;
		this.CalCalibrationNo.HeaderText = "Cali. no";
		this.CalCalibrationNo.Name = "CalCalibrationNo";
		this.CalCalibrationNo.ReadOnly = true;
		this.CalType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.CalType.DataPropertyName = "TypeName";
		this.CalType.FillWeight = 20f;
		this.CalType.HeaderText = "Type";
		this.CalType.Name = "CalType";
		this.CalType.ReadOnly = true;
		this.CalType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.CalCompany.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.CalCompany.DataPropertyName = "Company";
		this.CalCompany.FillWeight = 20f;
		this.CalCompany.HeaderText = "Company";
		this.CalCompany.Name = "CalCompany";
		this.CalCompany.ReadOnly = true;
		this.CalStaff.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.CalStaff.DataPropertyName = "Staff";
		this.CalStaff.FillWeight = 20f;
		this.CalStaff.HeaderText = "Staff";
		this.CalStaff.Name = "CalStaff";
		this.CalStaff.ReadOnly = true;
		this.CalStaff.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.CalCalDate.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.CalCalDate.DataPropertyName = "CalDate";
		this.CalCalDate.FillWeight = 20f;
		this.CalCalDate.HeaderText = "Cali. date";
		this.CalCalDate.Name = "CalCalDate";
		this.CalCalDate.ReadOnly = true;
		this.CalCalDate.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.CalExpDate.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.CalExpDate.DataPropertyName = "ExpDate";
		this.CalExpDate.FillWeight = 20f;
		this.CalExpDate.HeaderText = "Exp. date";
		this.CalExpDate.Name = "CalExpDate";
		this.CalExpDate.ReadOnly = true;
		this.CalExpDate.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.CalPeriod.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.CalPeriod.DataPropertyName = "Period";
		this.CalPeriod.FillWeight = 10f;
		this.CalPeriod.HeaderText = "Period";
		this.CalPeriod.Name = "CalPeriod";
		this.CalPeriod.ReadOnly = true;
		this.CalPeriod.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.CalId.DataPropertyName = "Id";
		dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
		this.CalId.DefaultCellStyle = dataGridViewCellStyle6;
		this.CalId.HeaderText = "Id";
		this.CalId.Name = "CalId";
		this.CalId.ReadOnly = true;
		this.CalId.Resizable = System.Windows.Forms.DataGridViewTriState.False;
		this.CalId.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.CalId.Visible = false;
		this.CalId.Width = 140;
		this.tpanelCalibration.AutoSize = true;
		this.tpanelCalibration.ColumnCount = 4;
		this.tpanelCalibration.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelCalibration.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100f));
		this.tpanelCalibration.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelCalibration.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelCalibration.Controls.Add(this.lblCalTotalRow, 2, 0);
		this.tpanelCalibration.Controls.Add(this.lblCalTotal, 3, 0);
		this.tpanelCalibration.Controls.Add(this.lblCalibration, 0, 0);
		this.tpanelCalibration.Dock = System.Windows.Forms.DockStyle.Top;
		this.tpanelCalibration.Location = new System.Drawing.Point(0, 437);
		this.tpanelCalibration.Name = "tpanelCalibration";
		this.tpanelCalibration.RowCount = 1;
		this.tpanelCalibration.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25f));
		this.tpanelCalibration.Size = new System.Drawing.Size(597, 25);
		this.tpanelCalibration.TabIndex = 174;
		this.tpanelCalibration.Visible = false;
		this.lblCalTotalRow.AutoSize = true;
		this.lblCalTotalRow.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblCalTotalRow.ForeColor = System.Drawing.Color.Black;
		this.lblCalTotalRow.Location = new System.Drawing.Point(507, 1);
		this.lblCalTotalRow.Margin = new System.Windows.Forms.Padding(1);
		this.lblCalTotalRow.Name = "lblCalTotalRow";
		this.lblCalTotalRow.Size = new System.Drawing.Size(72, 23);
		this.lblCalTotalRow.TabIndex = 133;
		this.lblCalTotalRow.Text = "Total rows:";
		this.lblCalTotalRow.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblCalTotal.AutoSize = true;
		this.lblCalTotal.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblCalTotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblCalTotal.Location = new System.Drawing.Point(581, 1);
		this.lblCalTotal.Margin = new System.Windows.Forms.Padding(1);
		this.lblCalTotal.Name = "lblCalTotal";
		this.lblCalTotal.Size = new System.Drawing.Size(15, 23);
		this.lblCalTotal.TabIndex = 132;
		this.lblCalTotal.Text = "0";
		this.lblCalTotal.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblCalibration.AutoSize = true;
		this.lblCalibration.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblCalibration.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblCalibration.Location = new System.Drawing.Point(3, 0);
		this.lblCalibration.Name = "lblCalibration";
		this.lblCalibration.Padding = new System.Windows.Forms.Padding(1);
		this.lblCalibration.Size = new System.Drawing.Size(84, 25);
		this.lblCalibration.TabIndex = 6;
		this.lblCalibration.Text = "Calibration";
		this.lblCalibration.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.dgvMeas.AllowUserToAddRows = false;
		this.dgvMeas.AllowUserToDeleteRows = false;
		this.dgvMeas.AllowUserToOrderColumns = true;
		this.dgvMeas.AllowUserToResizeRows = false;
		dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Control;
		this.dgvMeas.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle7;
		this.dgvMeas.BackgroundColor = System.Drawing.SystemColors.Window;
		dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
		dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.AppWorkspace;
		dataGridViewCellStyle8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.WindowText;
		dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
		dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
		dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
		this.dgvMeas.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle8;
		this.dgvMeas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
		this.dgvMeas.Columns.AddRange(this.No, this.Code, this.name, this.Value, this.UnitName, this.Upper, this.Lower, this.MachineTypeName, this.Id);
		this.dgvMeas.Dock = System.Windows.Forms.DockStyle.Top;
		this.dgvMeas.EnableHeadersVisualStyles = false;
		this.dgvMeas.Location = new System.Drawing.Point(0, 396);
		this.dgvMeas.Margin = new System.Windows.Forms.Padding(1);
		this.dgvMeas.Name = "dgvMeas";
		this.dgvMeas.ReadOnly = true;
		this.dgvMeas.RowHeadersVisible = false;
		this.dgvMeas.Size = new System.Drawing.Size(597, 41);
		this.dgvMeas.TabIndex = 4;
		this.dgvMeas.Visible = false;
		this.dgvMeas.Sorted += new System.EventHandler(dgvMeas_Sorted);
		this.No.DataPropertyName = "No";
		dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
		this.No.DefaultCellStyle = dataGridViewCellStyle9;
		this.No.HeaderText = "No";
		this.No.Name = "No";
		this.No.ReadOnly = true;
		this.No.Resizable = System.Windows.Forms.DataGridViewTriState.False;
		this.No.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.No.Width = 35;
		this.Code.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Code.DataPropertyName = "Code";
		this.Code.FillWeight = 25f;
		this.Code.HeaderText = "Code";
		this.Code.Name = "Code";
		this.Code.ReadOnly = true;
		this.name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.name.DataPropertyName = "Name";
		this.name.FillWeight = 30f;
		this.name.HeaderText = "Name";
		this.name.Name = "name";
		this.name.ReadOnly = true;
		this.Value.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Value.DataPropertyName = "Value";
		dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
		this.Value.DefaultCellStyle = dataGridViewCellStyle10;
		this.Value.FillWeight = 20f;
		this.Value.HeaderText = "Value";
		this.Value.Name = "Value";
		this.Value.ReadOnly = true;
		this.UnitName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.UnitName.DataPropertyName = "UnitName";
		this.UnitName.FillWeight = 20f;
		this.UnitName.HeaderText = "Unit";
		this.UnitName.Name = "UnitName";
		this.UnitName.ReadOnly = true;
		this.Upper.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Upper.DataPropertyName = "Upper";
		dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
		this.Upper.DefaultCellStyle = dataGridViewCellStyle11;
		this.Upper.FillWeight = 20f;
		this.Upper.HeaderText = "Upper";
		this.Upper.Name = "Upper";
		this.Upper.ReadOnly = true;
		this.Lower.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Lower.DataPropertyName = "Lower";
		dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
		this.Lower.DefaultCellStyle = dataGridViewCellStyle12;
		this.Lower.FillWeight = 20f;
		this.Lower.HeaderText = "Lower";
		this.Lower.Name = "Lower";
		this.Lower.ReadOnly = true;
		this.MachineTypeName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.MachineTypeName.DataPropertyName = "MachineTypeName";
		this.MachineTypeName.FillWeight = 25f;
		this.MachineTypeName.HeaderText = "Mac. Type";
		this.MachineTypeName.Name = "MachineTypeName";
		this.MachineTypeName.ReadOnly = true;
		this.Id.DataPropertyName = "Id";
		dataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
		this.Id.DefaultCellStyle = dataGridViewCellStyle13;
		this.Id.HeaderText = "Id";
		this.Id.Name = "Id";
		this.Id.ReadOnly = true;
		this.Id.Resizable = System.Windows.Forms.DataGridViewTriState.False;
		this.Id.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.Id.Visible = false;
		this.Id.Width = 140;
		this.tpanelMeas.AutoSize = true;
		this.tpanelMeas.ColumnCount = 4;
		this.tpanelMeas.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelMeas.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100f));
		this.tpanelMeas.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelMeas.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelMeas.Controls.Add(this.lblMeasTotalRow, 2, 0);
		this.tpanelMeas.Controls.Add(this.lblMeasTotal, 3, 0);
		this.tpanelMeas.Controls.Add(this.lblMeas, 0, 0);
		this.tpanelMeas.Dock = System.Windows.Forms.DockStyle.Top;
		this.tpanelMeas.Location = new System.Drawing.Point(0, 371);
		this.tpanelMeas.Name = "tpanelMeas";
		this.tpanelMeas.RowCount = 1;
		this.tpanelMeas.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25f));
		this.tpanelMeas.Size = new System.Drawing.Size(597, 25);
		this.tpanelMeas.TabIndex = 6;
		this.tpanelMeas.Visible = false;
		this.lblMeasTotalRow.AutoSize = true;
		this.lblMeasTotalRow.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblMeasTotalRow.ForeColor = System.Drawing.Color.Black;
		this.lblMeasTotalRow.Location = new System.Drawing.Point(507, 1);
		this.lblMeasTotalRow.Margin = new System.Windows.Forms.Padding(1);
		this.lblMeasTotalRow.Name = "lblMeasTotalRow";
		this.lblMeasTotalRow.Size = new System.Drawing.Size(72, 23);
		this.lblMeasTotalRow.TabIndex = 133;
		this.lblMeasTotalRow.Text = "Total rows:";
		this.lblMeasTotalRow.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblMeasTotal.AutoSize = true;
		this.lblMeasTotal.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblMeasTotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblMeasTotal.Location = new System.Drawing.Point(581, 1);
		this.lblMeasTotal.Margin = new System.Windows.Forms.Padding(1);
		this.lblMeasTotal.Name = "lblMeasTotal";
		this.lblMeasTotal.Size = new System.Drawing.Size(15, 23);
		this.lblMeasTotal.TabIndex = 132;
		this.lblMeasTotal.Text = "0";
		this.lblMeasTotal.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblMeas.AutoSize = true;
		this.lblMeas.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblMeas.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblMeas.Location = new System.Drawing.Point(3, 0);
		this.lblMeas.Name = "lblMeas";
		this.lblMeas.Padding = new System.Windows.Forms.Padding(1);
		this.lblMeas.Size = new System.Drawing.Size(102, 25);
		this.lblMeas.TabIndex = 6;
		this.lblMeas.Text = "Measurement";
		this.lblMeas.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.dgvTemplate.AllowUserToAddRows = false;
		this.dgvTemplate.AllowUserToDeleteRows = false;
		this.dgvTemplate.AllowUserToOrderColumns = true;
		this.dgvTemplate.AllowUserToResizeRows = false;
		dataGridViewCellStyle14.BackColor = System.Drawing.SystemColors.Control;
		this.dgvTemplate.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle14;
		this.dgvTemplate.BackgroundColor = System.Drawing.SystemColors.Window;
		dataGridViewCellStyle15.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
		dataGridViewCellStyle15.BackColor = System.Drawing.SystemColors.AppWorkspace;
		dataGridViewCellStyle15.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		dataGridViewCellStyle15.ForeColor = System.Drawing.SystemColors.WindowText;
		dataGridViewCellStyle15.SelectionBackColor = System.Drawing.SystemColors.Highlight;
		dataGridViewCellStyle15.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
		dataGridViewCellStyle15.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
		this.dgvTemplate.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle15;
		this.dgvTemplate.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
		this.dgvTemplate.Columns.AddRange(this.dataGridViewCheckBoxColumn1, this.TemplateNo, this.TemplateOtherName, this.TemplateDescription);
		this.dgvTemplate.Dock = System.Windows.Forms.DockStyle.Top;
		this.dgvTemplate.EnableHeadersVisualStyles = false;
		this.dgvTemplate.Location = new System.Drawing.Point(0, 324);
		this.dgvTemplate.Margin = new System.Windows.Forms.Padding(1);
		this.dgvTemplate.Name = "dgvTemplate";
		this.dgvTemplate.ReadOnly = true;
		this.dgvTemplate.RowHeadersVisible = false;
		this.dgvTemplate.RowHeadersWidth = 22;
		this.dgvTemplate.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
		this.dgvTemplate.Size = new System.Drawing.Size(597, 47);
		this.dgvTemplate.TabIndex = 173;
		this.dgvTemplate.TabStop = false;
		this.dgvTemplate.Visible = false;
		this.dgvTemplate.Sorted += new System.EventHandler(dgvTemplate_Sorted);
		this.dataGridViewCheckBoxColumn1.DataPropertyName = "IsSelect";
		this.dataGridViewCheckBoxColumn1.FalseValue = "False";
		this.dataGridViewCheckBoxColumn1.HeaderText = "";
		this.dataGridViewCheckBoxColumn1.Name = "dataGridViewCheckBoxColumn1";
		this.dataGridViewCheckBoxColumn1.ReadOnly = true;
		this.dataGridViewCheckBoxColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
		this.dataGridViewCheckBoxColumn1.TrueValue = "True";
		this.dataGridViewCheckBoxColumn1.Visible = false;
		this.dataGridViewCheckBoxColumn1.Width = 25;
		this.TemplateNo.DataPropertyName = "No";
		dataGridViewCellStyle16.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
		this.TemplateNo.DefaultCellStyle = dataGridViewCellStyle16;
		this.TemplateNo.HeaderText = "No";
		this.TemplateNo.Name = "TemplateNo";
		this.TemplateNo.ReadOnly = true;
		this.TemplateNo.Resizable = System.Windows.Forms.DataGridViewTriState.False;
		this.TemplateNo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.TemplateNo.Width = 35;
		this.TemplateOtherName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.TemplateOtherName.DataPropertyName = "TemplateName";
		this.TemplateOtherName.FillWeight = 40f;
		this.TemplateOtherName.HeaderText = "Template name";
		this.TemplateOtherName.Name = "TemplateOtherName";
		this.TemplateOtherName.ReadOnly = true;
		this.TemplateDescription.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.TemplateDescription.DataPropertyName = "TemplateDescription";
		this.TemplateDescription.FillWeight = 60f;
		this.TemplateDescription.HeaderText = "Description";
		this.TemplateDescription.Name = "TemplateDescription";
		this.TemplateDescription.ReadOnly = true;
		this.TemplateDescription.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.tpanelTemplate.AutoSize = true;
		this.tpanelTemplate.ColumnCount = 4;
		this.tpanelTemplate.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelTemplate.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100f));
		this.tpanelTemplate.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelTemplate.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelTemplate.Controls.Add(this.lblTemplateTotalRow, 2, 0);
		this.tpanelTemplate.Controls.Add(this.lblTemplateTotal, 3, 0);
		this.tpanelTemplate.Controls.Add(this.lblTemplate, 0, 0);
		this.tpanelTemplate.Dock = System.Windows.Forms.DockStyle.Top;
		this.tpanelTemplate.Location = new System.Drawing.Point(0, 299);
		this.tpanelTemplate.Name = "tpanelTemplate";
		this.tpanelTemplate.RowCount = 1;
		this.tpanelTemplate.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25f));
		this.tpanelTemplate.Size = new System.Drawing.Size(597, 25);
		this.tpanelTemplate.TabIndex = 172;
		this.tpanelTemplate.Visible = false;
		this.lblTemplateTotalRow.AutoSize = true;
		this.lblTemplateTotalRow.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblTemplateTotalRow.ForeColor = System.Drawing.Color.Black;
		this.lblTemplateTotalRow.Location = new System.Drawing.Point(507, 1);
		this.lblTemplateTotalRow.Margin = new System.Windows.Forms.Padding(1);
		this.lblTemplateTotalRow.Name = "lblTemplateTotalRow";
		this.lblTemplateTotalRow.Size = new System.Drawing.Size(72, 23);
		this.lblTemplateTotalRow.TabIndex = 133;
		this.lblTemplateTotalRow.Text = "Total rows:";
		this.lblTemplateTotalRow.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblTemplateTotal.AutoSize = true;
		this.lblTemplateTotal.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblTemplateTotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblTemplateTotal.Location = new System.Drawing.Point(581, 1);
		this.lblTemplateTotal.Margin = new System.Windows.Forms.Padding(1);
		this.lblTemplateTotal.Name = "lblTemplateTotal";
		this.lblTemplateTotal.Size = new System.Drawing.Size(15, 23);
		this.lblTemplateTotal.TabIndex = 132;
		this.lblTemplateTotal.Text = "0";
		this.lblTemplateTotal.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblTemplate.AutoSize = true;
		this.lblTemplate.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblTemplate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblTemplate.Location = new System.Drawing.Point(3, 0);
		this.lblTemplate.Name = "lblTemplate";
		this.lblTemplate.Padding = new System.Windows.Forms.Padding(1);
		this.lblTemplate.Size = new System.Drawing.Size(114, 25);
		this.lblTemplate.TabIndex = 6;
		this.lblTemplate.Text = "Template other";
		this.lblTemplate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.dgvSimilar.AllowUserToAddRows = false;
		this.dgvSimilar.AllowUserToDeleteRows = false;
		this.dgvSimilar.AllowUserToOrderColumns = true;
		this.dgvSimilar.AllowUserToResizeRows = false;
		dataGridViewCellStyle17.BackColor = System.Drawing.SystemColors.Control;
		this.dgvSimilar.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle17;
		this.dgvSimilar.BackgroundColor = System.Drawing.SystemColors.Window;
		dataGridViewCellStyle18.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
		dataGridViewCellStyle18.BackColor = System.Drawing.SystemColors.AppWorkspace;
		dataGridViewCellStyle18.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		dataGridViewCellStyle18.ForeColor = System.Drawing.SystemColors.WindowText;
		dataGridViewCellStyle18.SelectionBackColor = System.Drawing.SystemColors.Highlight;
		dataGridViewCellStyle18.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
		dataGridViewCellStyle18.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
		this.dgvSimilar.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle18;
		this.dgvSimilar.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
		this.dgvSimilar.Columns.AddRange(this.SimilarNo, this.SimilarProductCode, this.SimilarProductName);
		this.dgvSimilar.Dock = System.Windows.Forms.DockStyle.Top;
		this.dgvSimilar.EnableHeadersVisualStyles = false;
		this.dgvSimilar.Location = new System.Drawing.Point(0, 249);
		this.dgvSimilar.Margin = new System.Windows.Forms.Padding(1);
		this.dgvSimilar.Name = "dgvSimilar";
		this.dgvSimilar.ReadOnly = true;
		this.dgvSimilar.RowHeadersVisible = false;
		this.dgvSimilar.Size = new System.Drawing.Size(597, 50);
		this.dgvSimilar.TabIndex = 169;
		this.dgvSimilar.Visible = false;
		this.dgvSimilar.Sorted += new System.EventHandler(dgvSimilar_Sorted);
		this.SimilarNo.DataPropertyName = "No";
		dataGridViewCellStyle19.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
		this.SimilarNo.DefaultCellStyle = dataGridViewCellStyle19;
		this.SimilarNo.HeaderText = "No";
		this.SimilarNo.Name = "SimilarNo";
		this.SimilarNo.ReadOnly = true;
		this.SimilarNo.Resizable = System.Windows.Forms.DataGridViewTriState.False;
		this.SimilarNo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.SimilarNo.Width = 35;
		this.SimilarProductCode.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.SimilarProductCode.DataPropertyName = "ProductCode";
		this.SimilarProductCode.FillWeight = 50f;
		this.SimilarProductCode.HeaderText = "Product code";
		this.SimilarProductCode.Name = "SimilarProductCode";
		this.SimilarProductCode.ReadOnly = true;
		this.SimilarProductName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.SimilarProductName.DataPropertyName = "ProductName";
		this.SimilarProductName.FillWeight = 50f;
		this.SimilarProductName.HeaderText = "Product name";
		this.SimilarProductName.Name = "SimilarProductName";
		this.SimilarProductName.ReadOnly = true;
		this.tpanelSimilar.AutoSize = true;
		this.tpanelSimilar.ColumnCount = 4;
		this.tpanelSimilar.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelSimilar.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100f));
		this.tpanelSimilar.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelSimilar.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelSimilar.Controls.Add(this.lblSimilarTotalRow, 2, 0);
		this.tpanelSimilar.Controls.Add(this.lblSimilarTotal, 3, 0);
		this.tpanelSimilar.Controls.Add(this.lblSimilar, 0, 0);
		this.tpanelSimilar.Dock = System.Windows.Forms.DockStyle.Top;
		this.tpanelSimilar.Location = new System.Drawing.Point(0, 224);
		this.tpanelSimilar.Name = "tpanelSimilar";
		this.tpanelSimilar.RowCount = 1;
		this.tpanelSimilar.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25f));
		this.tpanelSimilar.Size = new System.Drawing.Size(597, 25);
		this.tpanelSimilar.TabIndex = 168;
		this.tpanelSimilar.Visible = false;
		this.lblSimilarTotalRow.AutoSize = true;
		this.lblSimilarTotalRow.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblSimilarTotalRow.ForeColor = System.Drawing.Color.Black;
		this.lblSimilarTotalRow.Location = new System.Drawing.Point(507, 1);
		this.lblSimilarTotalRow.Margin = new System.Windows.Forms.Padding(1);
		this.lblSimilarTotalRow.Name = "lblSimilarTotalRow";
		this.lblSimilarTotalRow.Size = new System.Drawing.Size(72, 23);
		this.lblSimilarTotalRow.TabIndex = 133;
		this.lblSimilarTotalRow.Text = "Total rows:";
		this.lblSimilarTotalRow.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblSimilarTotal.AutoSize = true;
		this.lblSimilarTotal.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblSimilarTotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblSimilarTotal.Location = new System.Drawing.Point(581, 1);
		this.lblSimilarTotal.Margin = new System.Windows.Forms.Padding(1);
		this.lblSimilarTotal.Name = "lblSimilarTotal";
		this.lblSimilarTotal.Size = new System.Drawing.Size(15, 23);
		this.lblSimilarTotal.TabIndex = 132;
		this.lblSimilarTotal.Text = "0";
		this.lblSimilarTotal.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblSimilar.AutoSize = true;
		this.lblSimilar.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblSimilar.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblSimilar.Location = new System.Drawing.Point(3, 0);
		this.lblSimilar.Name = "lblSimilar";
		this.lblSimilar.Padding = new System.Windows.Forms.Padding(1);
		this.lblSimilar.Size = new System.Drawing.Size(57, 25);
		this.lblSimilar.TabIndex = 6;
		this.lblSimilar.Text = "Similar";
		this.lblSimilar.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.dgvPlan.AllowUserToAddRows = false;
		this.dgvPlan.AllowUserToDeleteRows = false;
		this.dgvPlan.AllowUserToOrderColumns = true;
		this.dgvPlan.AllowUserToResizeRows = false;
		dataGridViewCellStyle20.BackColor = System.Drawing.SystemColors.Control;
		this.dgvPlan.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle20;
		this.dgvPlan.BackgroundColor = System.Drawing.SystemColors.Window;
		dataGridViewCellStyle21.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
		dataGridViewCellStyle21.BackColor = System.Drawing.SystemColors.AppWorkspace;
		dataGridViewCellStyle21.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		dataGridViewCellStyle21.ForeColor = System.Drawing.SystemColors.WindowText;
		dataGridViewCellStyle21.SelectionBackColor = System.Drawing.SystemColors.Highlight;
		dataGridViewCellStyle21.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
		dataGridViewCellStyle21.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
		this.dgvPlan.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle21;
		this.dgvPlan.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
		this.dgvPlan.Columns.AddRange(this.StageNo, this.StageName, this.TemplateName, this.TotalDetails, this.dataGridViewTextBoxColumn17);
		this.dgvPlan.Dock = System.Windows.Forms.DockStyle.Top;
		this.dgvPlan.EnableHeadersVisualStyles = false;
		this.dgvPlan.Location = new System.Drawing.Point(0, 170);
		this.dgvPlan.Margin = new System.Windows.Forms.Padding(1);
		this.dgvPlan.Name = "dgvPlan";
		this.dgvPlan.ReadOnly = true;
		this.dgvPlan.RowHeadersVisible = false;
		this.dgvPlan.Size = new System.Drawing.Size(597, 54);
		this.dgvPlan.TabIndex = 166;
		this.dgvPlan.Visible = false;
		this.dgvPlan.Sorted += new System.EventHandler(dgvPlan_Sorted);
		this.StageNo.DataPropertyName = "No";
		dataGridViewCellStyle22.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
		this.StageNo.DefaultCellStyle = dataGridViewCellStyle22;
		this.StageNo.HeaderText = "No";
		this.StageNo.Name = "StageNo";
		this.StageNo.ReadOnly = true;
		this.StageNo.Resizable = System.Windows.Forms.DataGridViewTriState.False;
		this.StageNo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.StageNo.Width = 35;
		this.StageName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.StageName.DataPropertyName = "StageName";
		this.StageName.FillWeight = 30f;
		this.StageName.HeaderText = "Stage name";
		this.StageName.Name = "StageName";
		this.StageName.ReadOnly = true;
		this.TemplateName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.TemplateName.DataPropertyName = "TemplateName";
		this.TemplateName.FillWeight = 50f;
		this.TemplateName.HeaderText = "Template name";
		this.TemplateName.Name = "TemplateName";
		this.TemplateName.ReadOnly = true;
		this.TotalDetails.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.TotalDetails.DataPropertyName = "TotalDetails";
		this.TotalDetails.FillWeight = 20f;
		this.TotalDetails.HeaderText = "Meas. q.ty";
		this.TotalDetails.Name = "TotalDetails";
		this.TotalDetails.ReadOnly = true;
		this.dataGridViewTextBoxColumn17.DataPropertyName = "Id";
		dataGridViewCellStyle23.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
		this.dataGridViewTextBoxColumn17.DefaultCellStyle = dataGridViewCellStyle23;
		this.dataGridViewTextBoxColumn17.HeaderText = "Id";
		this.dataGridViewTextBoxColumn17.Name = "dataGridViewTextBoxColumn17";
		this.dataGridViewTextBoxColumn17.ReadOnly = true;
		this.dataGridViewTextBoxColumn17.Resizable = System.Windows.Forms.DataGridViewTriState.False;
		this.dataGridViewTextBoxColumn17.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.dataGridViewTextBoxColumn17.Visible = false;
		this.dataGridViewTextBoxColumn17.Width = 140;
		this.tpanelPlan.AutoSize = true;
		this.tpanelPlan.ColumnCount = 4;
		this.tpanelPlan.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelPlan.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100f));
		this.tpanelPlan.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelPlan.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelPlan.Controls.Add(this.lblPlanTotalRow, 2, 0);
		this.tpanelPlan.Controls.Add(this.lblPlanTotal, 3, 0);
		this.tpanelPlan.Controls.Add(this.lblPlan, 0, 0);
		this.tpanelPlan.Dock = System.Windows.Forms.DockStyle.Top;
		this.tpanelPlan.Location = new System.Drawing.Point(0, 145);
		this.tpanelPlan.Name = "tpanelPlan";
		this.tpanelPlan.RowCount = 1;
		this.tpanelPlan.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25f));
		this.tpanelPlan.Size = new System.Drawing.Size(597, 25);
		this.tpanelPlan.TabIndex = 167;
		this.tpanelPlan.Visible = false;
		this.lblPlanTotalRow.AutoSize = true;
		this.lblPlanTotalRow.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblPlanTotalRow.ForeColor = System.Drawing.Color.Black;
		this.lblPlanTotalRow.Location = new System.Drawing.Point(507, 1);
		this.lblPlanTotalRow.Margin = new System.Windows.Forms.Padding(1);
		this.lblPlanTotalRow.Name = "lblPlanTotalRow";
		this.lblPlanTotalRow.Size = new System.Drawing.Size(72, 23);
		this.lblPlanTotalRow.TabIndex = 133;
		this.lblPlanTotalRow.Text = "Total rows:";
		this.lblPlanTotalRow.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblPlanTotal.AutoSize = true;
		this.lblPlanTotal.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblPlanTotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblPlanTotal.Location = new System.Drawing.Point(581, 1);
		this.lblPlanTotal.Margin = new System.Windows.Forms.Padding(1);
		this.lblPlanTotal.Name = "lblPlanTotal";
		this.lblPlanTotal.Size = new System.Drawing.Size(15, 23);
		this.lblPlanTotal.TabIndex = 132;
		this.lblPlanTotal.Text = "0";
		this.lblPlanTotal.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblPlan.AutoSize = true;
		this.lblPlan.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblPlan.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblPlan.Location = new System.Drawing.Point(3, 0);
		this.lblPlan.Name = "lblPlan";
		this.lblPlan.Padding = new System.Windows.Forms.Padding(1);
		this.lblPlan.Size = new System.Drawing.Size(40, 25);
		this.lblPlan.TabIndex = 6;
		this.lblPlan.Text = "Plan";
		this.lblPlan.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.dgvAQL.AllowUserToAddRows = false;
		this.dgvAQL.AllowUserToDeleteRows = false;
		this.dgvAQL.AllowUserToOrderColumns = true;
		this.dgvAQL.AllowUserToResizeRows = false;
		dataGridViewCellStyle24.BackColor = System.Drawing.SystemColors.Control;
		this.dgvAQL.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle24;
		this.dgvAQL.BackgroundColor = System.Drawing.SystemColors.Window;
		dataGridViewCellStyle25.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
		dataGridViewCellStyle25.BackColor = System.Drawing.SystemColors.AppWorkspace;
		dataGridViewCellStyle25.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		dataGridViewCellStyle25.ForeColor = System.Drawing.SystemColors.WindowText;
		dataGridViewCellStyle25.SelectionBackColor = System.Drawing.SystemColors.Highlight;
		dataGridViewCellStyle25.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
		dataGridViewCellStyle25.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
		this.dgvAQL.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle25;
		this.dgvAQL.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
		this.dgvAQL.Columns.AddRange(this.AQLNo, this.Type, this.InputQuantity, this.Sample, this.dataGridViewTextBoxColumn5);
		this.dgvAQL.Dock = System.Windows.Forms.DockStyle.Top;
		this.dgvAQL.EnableHeadersVisualStyles = false;
		this.dgvAQL.Location = new System.Drawing.Point(0, 91);
		this.dgvAQL.Margin = new System.Windows.Forms.Padding(1);
		this.dgvAQL.Name = "dgvAQL";
		this.dgvAQL.ReadOnly = true;
		this.dgvAQL.RowHeadersVisible = false;
		this.dgvAQL.Size = new System.Drawing.Size(597, 54);
		this.dgvAQL.TabIndex = 176;
		this.dgvAQL.Visible = false;
		this.dgvAQL.Sorted += new System.EventHandler(dgvAQL_Sorted);
		this.AQLNo.DataPropertyName = "No";
		dataGridViewCellStyle26.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
		this.AQLNo.DefaultCellStyle = dataGridViewCellStyle26;
		this.AQLNo.HeaderText = "No";
		this.AQLNo.Name = "AQLNo";
		this.AQLNo.ReadOnly = true;
		this.AQLNo.Resizable = System.Windows.Forms.DataGridViewTriState.False;
		this.AQLNo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.AQLNo.Width = 35;
		this.Type.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Type.DataPropertyName = "Type";
		this.Type.FillWeight = 40f;
		this.Type.HeaderText = "Type";
		this.Type.Name = "Type";
		this.Type.ReadOnly = true;
		this.InputQuantity.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.InputQuantity.DataPropertyName = "InputQuantity";
		this.InputQuantity.FillWeight = 30f;
		this.InputQuantity.HeaderText = "Input q.ty";
		this.InputQuantity.Name = "InputQuantity";
		this.InputQuantity.ReadOnly = true;
		this.Sample.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Sample.DataPropertyName = "Sample";
		this.Sample.FillWeight = 30f;
		this.Sample.HeaderText = "Sample";
		this.Sample.Name = "Sample";
		this.Sample.ReadOnly = true;
		this.dataGridViewTextBoxColumn5.DataPropertyName = "Id";
		dataGridViewCellStyle27.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
		this.dataGridViewTextBoxColumn5.DefaultCellStyle = dataGridViewCellStyle27;
		this.dataGridViewTextBoxColumn5.HeaderText = "Id";
		this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
		this.dataGridViewTextBoxColumn5.ReadOnly = true;
		this.dataGridViewTextBoxColumn5.Resizable = System.Windows.Forms.DataGridViewTriState.False;
		this.dataGridViewTextBoxColumn5.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.dataGridViewTextBoxColumn5.Visible = false;
		this.dataGridViewTextBoxColumn5.Width = 140;
		this.tpanelAQL.AutoSize = true;
		this.tpanelAQL.ColumnCount = 4;
		this.tpanelAQL.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelAQL.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100f));
		this.tpanelAQL.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelAQL.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelAQL.Controls.Add(this.lblAQLTotalRow, 2, 0);
		this.tpanelAQL.Controls.Add(this.lblAQLTotal, 3, 0);
		this.tpanelAQL.Controls.Add(this.lblAQL, 0, 0);
		this.tpanelAQL.Dock = System.Windows.Forms.DockStyle.Top;
		this.tpanelAQL.Location = new System.Drawing.Point(0, 66);
		this.tpanelAQL.Name = "tpanelAQL";
		this.tpanelAQL.RowCount = 1;
		this.tpanelAQL.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25f));
		this.tpanelAQL.Size = new System.Drawing.Size(597, 25);
		this.tpanelAQL.TabIndex = 177;
		this.tpanelAQL.Visible = false;
		this.lblAQLTotalRow.AutoSize = true;
		this.lblAQLTotalRow.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblAQLTotalRow.ForeColor = System.Drawing.Color.Black;
		this.lblAQLTotalRow.Location = new System.Drawing.Point(507, 1);
		this.lblAQLTotalRow.Margin = new System.Windows.Forms.Padding(1);
		this.lblAQLTotalRow.Name = "lblAQLTotalRow";
		this.lblAQLTotalRow.Size = new System.Drawing.Size(72, 23);
		this.lblAQLTotalRow.TabIndex = 133;
		this.lblAQLTotalRow.Text = "Total rows:";
		this.lblAQLTotalRow.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblAQLTotal.AutoSize = true;
		this.lblAQLTotal.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblAQLTotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblAQLTotal.Location = new System.Drawing.Point(581, 1);
		this.lblAQLTotal.Margin = new System.Windows.Forms.Padding(1);
		this.lblAQLTotal.Name = "lblAQLTotal";
		this.lblAQLTotal.Size = new System.Drawing.Size(15, 23);
		this.lblAQLTotal.TabIndex = 132;
		this.lblAQLTotal.Text = "0";
		this.lblAQLTotal.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblAQL.AutoSize = true;
		this.lblAQL.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblAQL.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblAQL.Location = new System.Drawing.Point(3, 0);
		this.lblAQL.Name = "lblAQL";
		this.lblAQL.Padding = new System.Windows.Forms.Padding(1);
		this.lblAQL.Size = new System.Drawing.Size(38, 25);
		this.lblAQL.TabIndex = 6;
		this.lblAQL.Text = "AQL";
		this.lblAQL.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.dgvFooter.AllowUserToAddRows = false;
		this.dgvFooter.AllowUserToDeleteRows = false;
		this.dgvFooter.AllowUserToResizeColumns = false;
		this.dgvFooter.AllowUserToResizeRows = false;
		this.dgvFooter.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
		this.dgvFooter.ColumnHeadersVisible = false;
		this.dgvFooter.Columns.AddRange(this.footer_title, this.footer_value);
		this.dgvFooter.Dock = System.Windows.Forms.DockStyle.Top;
		this.dgvFooter.Location = new System.Drawing.Point(0, 44);
		this.dgvFooter.Margin = new System.Windows.Forms.Padding(1);
		this.dgvFooter.Name = "dgvFooter";
		this.dgvFooter.ReadOnly = true;
		this.dgvFooter.RowHeadersVisible = false;
		this.dgvFooter.ScrollBars = System.Windows.Forms.ScrollBars.None;
		this.dgvFooter.Size = new System.Drawing.Size(597, 22);
		this.dgvFooter.TabIndex = 3;
		this.dgvFooter.Visible = false;
		this.dgvFooter.Leave += new System.EventHandler(dgvNormal_Leave);
		dataGridViewCellStyle28.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
		this.footer_title.DefaultCellStyle = dataGridViewCellStyle28;
		this.footer_title.HeaderText = "Title";
		this.footer_title.Name = "footer_title";
		this.footer_title.ReadOnly = true;
		this.footer_title.Resizable = System.Windows.Forms.DataGridViewTriState.False;
		this.footer_title.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.footer_title.Width = 140;
		this.footer_value.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		dataGridViewCellStyle29.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
		this.footer_value.DefaultCellStyle = dataGridViewCellStyle29;
		this.footer_value.HeaderText = "Value";
		this.footer_value.Name = "footer_value";
		this.footer_value.ReadOnly = true;
		this.footer_value.Resizable = System.Windows.Forms.DataGridViewTriState.False;
		this.footer_value.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.dgvOther.AllowUserToAddRows = false;
		this.dgvOther.AllowUserToDeleteRows = false;
		this.dgvOther.AllowUserToResizeColumns = false;
		this.dgvOther.AllowUserToResizeRows = false;
		this.dgvOther.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
		this.dgvOther.ColumnHeadersVisible = false;
		this.dgvOther.Columns.AddRange(this.other_title, this.other_value);
		this.dgvOther.Dock = System.Windows.Forms.DockStyle.Top;
		this.dgvOther.Location = new System.Drawing.Point(0, 22);
		this.dgvOther.Margin = new System.Windows.Forms.Padding(1);
		this.dgvOther.Name = "dgvOther";
		this.dgvOther.ReadOnly = true;
		this.dgvOther.RowHeadersVisible = false;
		this.dgvOther.RowTemplate.Height = 100;
		this.dgvOther.ScrollBars = System.Windows.Forms.ScrollBars.None;
		this.dgvOther.Size = new System.Drawing.Size(597, 22);
		this.dgvOther.TabIndex = 2;
		this.dgvOther.Visible = false;
		this.dgvOther.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(dgvOther_CellDoubleClick);
		this.dgvOther.Leave += new System.EventHandler(dgvNormal_Leave);
		dataGridViewCellStyle30.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopRight;
		this.other_title.DefaultCellStyle = dataGridViewCellStyle30;
		this.other_title.HeaderText = "Title";
		this.other_title.Name = "other_title";
		this.other_title.ReadOnly = true;
		this.other_title.Resizable = System.Windows.Forms.DataGridViewTriState.False;
		this.other_title.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.other_title.Width = 140;
		this.other_value.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		dataGridViewCellStyle31.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
		dataGridViewCellStyle31.NullValue = resources.GetObject("dataGridViewCellStyle33.NullValue");
		this.other_value.DefaultCellStyle = dataGridViewCellStyle31;
		this.other_value.HeaderText = "Value";
		this.other_value.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
		this.other_value.Name = "other_value";
		this.other_value.ReadOnly = true;
		this.other_value.Resizable = System.Windows.Forms.DataGridViewTriState.False;
		this.dgvContent.AllowUserToAddRows = false;
		this.dgvContent.AllowUserToDeleteRows = false;
		this.dgvContent.AllowUserToResizeColumns = false;
		this.dgvContent.AllowUserToResizeRows = false;
		this.dgvContent.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
		this.dgvContent.ColumnHeadersVisible = false;
		this.dgvContent.Columns.AddRange(this.content_title, this.content_value);
		this.dgvContent.Dock = System.Windows.Forms.DockStyle.Top;
		this.dgvContent.Location = new System.Drawing.Point(0, 0);
		this.dgvContent.Margin = new System.Windows.Forms.Padding(1);
		this.dgvContent.Name = "dgvContent";
		this.dgvContent.ReadOnly = true;
		this.dgvContent.RowHeadersVisible = false;
		this.dgvContent.ScrollBars = System.Windows.Forms.ScrollBars.None;
		this.dgvContent.Size = new System.Drawing.Size(597, 22);
		this.dgvContent.TabIndex = 1;
		this.dgvContent.Visible = false;
		this.dgvContent.Leave += new System.EventHandler(dgvNormal_Leave);
		dataGridViewCellStyle32.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
		this.content_title.DefaultCellStyle = dataGridViewCellStyle32;
		this.content_title.HeaderText = "Title";
		this.content_title.Name = "content_title";
		this.content_title.ReadOnly = true;
		this.content_title.Resizable = System.Windows.Forms.DataGridViewTriState.False;
		this.content_title.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.content_title.Width = 140;
		this.content_value.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		dataGridViewCellStyle33.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
		this.content_value.DefaultCellStyle = dataGridViewCellStyle33;
		this.content_value.HeaderText = "Value";
		this.content_value.Name = "content_value";
		this.content_value.ReadOnly = true;
		this.content_value.Resizable = System.Windows.Forms.DataGridViewTriState.False;
		this.content_value.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.ProNo.DataPropertyName = "No";
		dataGridViewCellStyle34.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
		this.ProNo.DefaultCellStyle = dataGridViewCellStyle34;
		this.ProNo.HeaderText = "No";
		this.ProNo.Name = "ProNo";
		this.ProNo.ReadOnly = true;
		this.ProNo.Resizable = System.Windows.Forms.DataGridViewTriState.False;
		this.ProNo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.ProNo.Width = 35;
		this.ProCode.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.ProCode.DataPropertyName = "Code";
		this.ProCode.FillWeight = 20f;
		this.ProCode.HeaderText = "Code";
		this.ProCode.Name = "ProCode";
		this.ProCode.ReadOnly = true;
		this.ProName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.ProName.DataPropertyName = "Name";
		this.ProName.FillWeight = 20f;
		this.ProName.HeaderText = "Name";
		this.ProName.Name = "ProName";
		this.ProName.ReadOnly = true;
		this.GroupCodeName.DataPropertyName = "GroupCodeName";
		this.GroupCodeName.HeaderText = "Product code";
		this.GroupCodeName.Name = "GroupCodeName";
		this.GroupCodeName.ReadOnly = true;
		this.GroupCodeName.Visible = false;
		this.ProImageUrl.DataPropertyName = "ImageUrl";
		this.ProImageUrl.HeaderText = "Image";
		this.ProImageUrl.Name = "ProImageUrl";
		this.ProImageUrl.ReadOnly = true;
		this.ProImageUrl.Visible = false;
		this.ProTotalMeas.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.ProTotalMeas.DataPropertyName = "TotalMeas";
		this.ProTotalMeas.FillWeight = 20f;
		this.ProTotalMeas.HeaderText = "Total meas.";
		this.ProTotalMeas.Name = "ProTotalMeas";
		this.ProTotalMeas.ReadOnly = true;
		this.dataGridViewTextBoxColumn10.DataPropertyName = "Id";
		dataGridViewCellStyle35.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
		this.dataGridViewTextBoxColumn10.DefaultCellStyle = dataGridViewCellStyle35;
		this.dataGridViewTextBoxColumn10.HeaderText = "Id";
		this.dataGridViewTextBoxColumn10.Name = "dataGridViewTextBoxColumn10";
		this.dataGridViewTextBoxColumn10.ReadOnly = true;
		this.dataGridViewTextBoxColumn10.Resizable = System.Windows.Forms.DataGridViewTriState.False;
		this.dataGridViewTextBoxColumn10.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.dataGridViewTextBoxColumn10.Visible = false;
		this.dataGridViewTextBoxColumn10.Width = 140;
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		base.Controls.Add(this.panelView);
		base.Controls.Add(this.panelResize);
		base.Controls.Add(this.panelFooter);
		base.Controls.Add(this.panelTitle);
		base.Controls.Add(this.tblPanelFooter);
		base.Controls.Add(this.tblPanelTitle);
		this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.Margin = new System.Windows.Forms.Padding(0);
		base.Name = "mPanelView";
		base.Size = new System.Drawing.Size(600, 664);
		base.Load += new System.EventHandler(mPanelView_Load);
		this.tblPanelTitle.ResumeLayout(false);
		this.tblPanelTitle.PerformLayout();
		this.tblPanelFooter.ResumeLayout(false);
		this.tblPanelFooter.PerformLayout();
		this.panelView.ResumeLayout(false);
		this.panelView.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.dgvStage).EndInit();
		this.tpanelStage.ResumeLayout(false);
		this.tpanelStage.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.dgvCalibration).EndInit();
		this.tpanelCalibration.ResumeLayout(false);
		this.tpanelCalibration.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.dgvMeas).EndInit();
		this.tpanelMeas.ResumeLayout(false);
		this.tpanelMeas.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.dgvTemplate).EndInit();
		this.tpanelTemplate.ResumeLayout(false);
		this.tpanelTemplate.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.dgvSimilar).EndInit();
		this.tpanelSimilar.ResumeLayout(false);
		this.tpanelSimilar.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.dgvPlan).EndInit();
		this.tpanelPlan.ResumeLayout(false);
		this.tpanelPlan.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.dgvAQL).EndInit();
		this.tpanelAQL.ResumeLayout(false);
		this.tpanelAQL.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.dgvFooter).EndInit();
		((System.ComponentModel.ISupportInitialize)this.dgvOther).EndInit();
		((System.ComponentModel.ISupportInitialize)this.dgvContent).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
