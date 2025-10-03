using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using _5S_QA_Client.Controls;
using _5S_QA_Client.Properties;
using _5S_QA_DL.APIContext;
using _5S_QA_Entities.Constants;
using _5S_QA_Entities.Dtos;
using _5S_QA_Entities.Models;
using MetroFramework.Forms;
using Newtonsoft.Json;
using WebSocketSharp;

namespace _5S_QA_Client;

public class frmResultView : MetroForm
{
	private readonly frmLogin mForm;

	private readonly PictureBox ptbProduct = new PictureBox();

	private readonly RequestViewModel mRequest;

	public bool isClose;

	private string[] mFilter;

	private List<string> mDecimals = new List<string>();

	private readonly int mChartNumber = int.Parse(ConfigurationManager.ConnectionStrings["ChartNumber"].ConnectionString);

	private readonly double mDeviation = double.Parse(ConfigurationManager.ConnectionStrings["Deviation"].ConnectionString);

	private Button btnOK;

	private Button btnNG;

	private Button btnClear;

	private string mRemember;

	private List<ResultViewModel> mResults;

	private WebSocket mWs;

	private const int COL_HEAER = 0;

	private const int ROW_ID = 0;

	private const int ROW_DIM = 1;

	private const int ROW_VALUE = 2;

	private const int ROW_UNIT = 3;

	private const int ROW_UPPER = 4;

	private const int ROW_LOWER = 5;

	private const int ROW_TOOL = 6;

	private const int ROW_SAMPLE_FIRST = 7;

	private const int NUMBER_ROW_INFOR = 9;

	private int ROW_SAMPLE_LAST = 7;

	private int mTotalResult = 0;

	private Guid mIdTool;

	private System.Timers.Timer mTimer;

	private DataTable mMeass;

	private List<string> mRowHeaders;

	private IContainer components = null;

	private ToolTip toolTipMain;

	private StatusStrip statusStripfrmMain;

	private ToolStripProgressBar sprogbarStatus;

	public ToolStripStatusLabel slblStatus;

	private DataGridView dgvMain;

	private ContextMenuStrip contextMenuStripMain;

	private ToolStripMenuItem main_refreshToolStripMenuItem;

	private mPanelView mPanelViewMain;

	private ToolStripSeparator toolStripSeparator1;

	private ToolStripMenuItem main_viewToolStripMenuItem;

	private ToolStripSplitButton imgSignal;

	private TableLayoutPanel tpanelInfor;

	private Label lblRowDisplay;

	private Label lblColumnDisplay;

	private Label lblValueDisplay;

	private Label lblValue;

	private Label lblColumn;

	private Label lblRow;

	private Label lblNG;

	private Label label4;

	private Label lblOK;

	private Label label5;

	private Label lblEmpty;

	private Label lblEmptyDisplay;

	private Label lblTotal;

	private Label lblTotalDisplay;

	private TableLayoutPanel tpanelTitle;

	private Label lblTool;

	private Label lblDimNo;

	private Label lblJudgeDisplay;

	private Label lblDimNoDisplay;

	private Label lblToolDisplay;

	private Label lblJudge;

	private TableLayoutPanel tpanelButton;

	private Button btnHistory;

	private Button btnImage;

	private RadioButton rbVertical;

	private RadioButton rbHorizontal;

	private mPanelImage mPanelImage;

	private Button btnComplete;

	private Button btnTool;

	private CheckBox cbSameTool;

	private Label lblToolNameDisplay;

	private Label lblToolName;

	private Label lblResult;

	private Label lblResultDisplay;

	private Label lblToolType;

	private Label lblToolTypeDisplay;

	public Button btnRegister;

	private Button btnEnterOK;

	private Button btnEnterNG;

	private Button btnRefresh;

	private Panel panelLogout;

	private Label lblFullname;

	private PictureBox ptbAvatar;

	private Label lblCavity;

	private Label lblCavityDisplay;

	private Label lblSample;

	private Label lblSampleDisplay;

	private mPanelChart mPanelChartMain;

	private CheckBox cbShowChart;

	private Button btnShowHeader;

	private DataGridView dgvHeader;

	private DataGridViewCheckBoxColumn title;

	private DataGridViewTextBoxColumn value;

	private DataGridViewTextBoxColumn index;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;

	public frmResultView(frmLogin frm, RequestViewModel request)
	{
		InitializeComponent();
		Common.setControls(this, toolTipMain, new List<ContextMenuStrip> { contextMenuStripMain });
		mForm = frm;
		mRequest = request;
		isClose = true;
	}

	private void frmResultView_Load(object sender, EventArgs e)
	{
		base.WindowState = FormWindowState.Maximized;
		mIdTool = Get_TabletId();
		Init();
	}

	private void frmResultView_Shown(object sender, EventArgs e)
	{
		mPanelChartMain.Visible = false;
		Set_dgvMain();
		mForm.Hide();
	}

	private void frmResultView_FormClosing(object sender, FormClosingEventArgs e)
	{
		mTimer?.Stop();
		GC.Collect();
	}

	private void frmResultView_FormClosed(object sender, FormClosedEventArgs e)
	{
		mWs?.Close();
		if (!isClose)
		{
			mForm.load_AllData();
		}
		mForm.Show();
		frmLogin.mIdOpen = Guid.Empty;
	}

	protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
	{
		switch (keyData)
		{
		case Keys.F3:
			btnEnterOK.PerformClick();
			return true;
		case Keys.Divide:
			btnEnterOK.PerformClick();
			return true;
		case Keys.F4:
			btnEnterNG.PerformClick();
			return true;
		case Keys.Multiply:
			btnEnterNG.PerformClick();
			return true;
		case Keys.F5:
			btnRegister.PerformClick();
			return true;
		default:
			return base.ProcessCmdKey(ref msg, keyData);
		}
	}

	private void Init()
	{
		Load_Settings();
		Text = Text + " (" + frmLogin.mMachineName + " * " + mRequest.Code + " * " + mRequest.Name + ")";
		lblFullname.Text = frmLogin.User.FullName;
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(APIUrl.APIHost + "/AuthUserImage/").Append(frmLogin.User.ImageUrl);
		try
		{
			ptbAvatar.Load(stringBuilder.ToString());
		}
		catch
		{
			ptbAvatar.Image = Resources._5S_QA_C;
		}
		Init_ImageProduct();
		load_AllData();
		dgvMain.CurrentCellChanged += dgvMain_CurrentCellChanged;
		cbShowChart.CheckedChanged += cbShowChart_CheckedChanged;
		Init_Timer();
		Init_Button();
		Init_WebSocket();
	}

	private void Init_dgvMain(DataTable dt)
	{
		start_Proccessor();
		List<RowDto> list = new List<RowDto>
		{
			new RowDto
			{
				Code = "Header",
				Display = "#",
				ReadOnly = true,
				IsCenter = true
			},
			new RowDto
			{
				Code = "Id",
				Display = "Id",
				ReadOnly = true,
				IsCenter = true
			},
			new RowDto
			{
				Code = "Name",
				Display = Common.getTextLanguage(this, "Name"),
				ReadOnly = true,
				IsCenter = true
			},
			new RowDto
			{
				Code = "Value",
				Display = Common.getTextLanguage(this, "Value"),
				ReadOnly = true,
				IsCenter = false
			},
			new RowDto
			{
				Code = "UnitName",
				Display = Common.getTextLanguage(this, "UnitName"),
				ReadOnly = true,
				IsCenter = true
			},
			new RowDto
			{
				Code = "Upper",
				Display = Common.getTextLanguage(this, "Upper"),
				ReadOnly = true,
				IsCenter = false
			},
			new RowDto
			{
				Code = "Lower",
				Display = Common.getTextLanguage(this, "Lower"),
				ReadOnly = true,
				IsCenter = false
			},
			new RowDto
			{
				Code = "MachineTypeName",
				Display = Common.getTextLanguage(this, "MachineTypeName"),
				ReadOnly = true,
				IsCenter = true
			}
		};
		for (int i = 1; i <= mRequest.Sample; i++)
		{
			if (mRequest.ProductCavity == 1)
			{
				list.Add(new RowDto
				{
					Code = $"{i}",
					Display = $"{i}",
					ReadOnly = false,
					IsCenter = false
				});
				continue;
			}
			for (int j = 1; j <= mRequest.ProductCavity; j++)
			{
				list.Add(new RowDto
				{
					Code = $"{i}#{j}",
					Display = $"{i} # {j}",
					ReadOnly = false,
					IsCenter = false
				});
			}
		}
		list.Add(new RowDto
		{
			Code = "Maximum",
			Display = Common.getTextLanguage(this, "Maximum"),
			ReadOnly = true,
			IsCenter = false
		});
		list.Add(new RowDto
		{
			Code = "Minimum",
			Display = Common.getTextLanguage(this, "Minimum"),
			ReadOnly = true,
			IsCenter = false
		});
		list.Add(new RowDto
		{
			Code = "Average",
			Display = Common.getTextLanguage(this, "Average"),
			ReadOnly = true,
			IsCenter = false
		});
		list.Add(new RowDto
		{
			Code = "Judge",
			Display = Common.getTextLanguage(this, "Judge"),
			ReadOnly = true,
			IsCenter = true
		});
		list.Add(new RowDto
		{
			Code = "StDev",
			Display = Common.getTextLanguage(this, "StDev"),
			ReadOnly = true,
			IsCenter = false
		});
		list.Add(new RowDto
		{
			Code = "Cp",
			Display = Common.getTextLanguage(this, "Cp"),
			ReadOnly = true,
			IsCenter = false
		});
		list.Add(new RowDto
		{
			Code = "Cpk",
			Display = Common.getTextLanguage(this, "Cpk"),
			ReadOnly = true,
			IsCenter = false
		});
		list.Add(new RowDto
		{
			Code = "Rank",
			Display = Common.getTextLanguage(this, "Rank"),
			ReadOnly = true,
			IsCenter = true
		});
		foreach (RowDto item in list)
		{
			string[] array = new string[dt.Rows.Count + 1];
			for (int k = 0; k <= dt.Rows.Count; k++)
			{
				if (item.Code == "Header")
				{
					string text = k.ToString();
					int num = 80;
					if (k == 0)
					{
						text = item.Display;
						num = 100;
					}
					DataGridViewTextBoxColumnEx dataGridViewTextBoxColumnEx = new DataGridViewTextBoxColumnEx
					{
						Name = text,
						HeaderText = text,
						Resizable = DataGridViewTriState.False,
						SortMode = DataGridViewColumnSortMode.NotSortable,
						Width = num
					};
					DataGridViewCellStyle dataGridViewCellStyle = new DataGridViewCellStyle
					{
						Alignment = DataGridViewContentAlignment.MiddleRight
					};
					if (k == 0)
					{
						dataGridViewTextBoxColumnEx.Frozen = true;
						dataGridViewCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
						dataGridViewCellStyle.BackColor = SystemColors.ScrollBar;
					}
					dataGridViewTextBoxColumnEx.DefaultCellStyle = dataGridViewCellStyle;
					dgvMain.Columns.Add(dataGridViewTextBoxColumnEx);
					continue;
				}
				string text2 = item.Display;
				if (k != 0)
				{
					if (!dt.Columns.Contains(item.Code))
					{
						text2 = ((!(item.Code == "Maximum") && !(item.Code == "Minimum") && !(item.Code == "Average") && !(item.Code == "StDev") && !(item.Code == "Cp") && !(item.Code == "Cpk") && !(item.Code == "Rank") && !(item.Code == "Judge")) ? string.Empty : "-");
					}
					else
					{
						text2 = dt.Rows[k - 1][item.Code].ToString();
						if ((item.Code == "Value" || item.Code == "Upper" || item.Code == "Lower") && double.TryParse(text2, out var result))
						{
							text2 = ((!(dt.Rows[k - 1]["UnitName"].ToString() == "°")) ? Common.ToString(result, dt.Rows[k - 1]["MachineTypeName"].ToString(), mDecimals) : Common.ConvertDoubleToDegrees(result));
						}
					}
				}
				array[k] = text2;
			}
			if (item.Code != "Header")
			{
				DataGridViewRowCollection rows = dgvMain.Rows;
				object[] values = array;
				rows.Add(values);
				dgvMain.Rows[dgvMain.RowCount - 1].ReadOnly = item.ReadOnly;
				if (item.IsCenter)
				{
					DataGridViewCellStyle defaultCellStyle = new DataGridViewCellStyle
					{
						Alignment = DataGridViewContentAlignment.MiddleCenter
					};
					dgvMain.Rows[dgvMain.RowCount - 1].DefaultCellStyle = defaultCellStyle;
				}
				if (item.ReadOnly)
				{
					dgvMain.Rows[dgvMain.RowCount - 1].DefaultCellStyle.BackColor = SystemColors.Control;
				}
				dgvMain.Rows[dgvMain.RowCount - 1].Visible = false;
			}
		}
		dgvMain.Columns[0].ReadOnly = true;
		dgvMain.Rows[6].Frozen = true;
		ROW_SAMPLE_LAST = dgvMain.RowCount - 9;
		mTotalResult = ((dgvMain.ColumnCount - 1) * mRequest.Sample * mRequest.ProductCavity).Value;
		lblTotal.Text = mTotalResult.ToString();
		setAQLForMeasurement();
		disableForRequestType();
		drawMerge(dt);
		initPanelHeaders();
	}

	private void Init_WebSocket()
	{
		mWs = new WebSocket(APIUrl.APIHost.Replace("http", "ws") + "/ws");
		mWs.OnMessage += Ws_OnMessage;
		mWs.Connect();
	}

	private void Init_Button()
	{
		new Task(delegate
		{
			Invoke((MethodInvoker)delegate
			{
				btnOK = new Button
				{
					Cursor = Cursors.Hand,
					Text = "OK",
					ForeColor = Color.Blue,
					Visible = false
				};
				toolTipMain.SetToolTip(btnOK, Common.getTextLanguage(this, "Select") + " OK");
				dgvMain.Controls.Add(btnOK);
				btnOK.Click += btn_Click;
				btnNG = new Button
				{
					Cursor = Cursors.Hand,
					Text = "NG",
					ForeColor = Color.Red,
					Visible = false
				};
				toolTipMain.SetToolTip(btnNG, Common.getTextLanguage(this, "Select") + " NG");
				dgvMain.Controls.Add(btnNG);
				btnNG.Click += btn_Click;
				btnClear = new Button
				{
					Cursor = Cursors.Hand,
					Text = Common.getTextLanguage(this, "Clear"),
					Visible = false
				};
				toolTipMain.SetToolTip(btnClear, Common.getTextLanguage(this, "Select") + " " + Common.getTextLanguage(this, "Clear"));
				dgvMain.Controls.Add(btnClear);
				btnClear.Click += btn_Click;
			});
		}).Start();
	}

	private void Init_ImageProduct()
	{
		new Task(delegate
		{
			try
			{
				string productImageUrl = mRequest.ProductImageUrl;
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(APIUrl.APIHost + "/ProductImage/").Append(productImageUrl);
				ptbProduct.Load(stringBuilder.ToString());
			}
			catch
			{
				ptbProduct.Image = null;
			}
			mPanelImage.Init(ptbProduct.Image, mRequest.Id, mIdTool, mFilter);
		}).Start();
	}

	private void Init_Timer()
	{
		mTimer = new System.Timers.Timer
		{
			Interval = 5000.0
		};
		mTimer.Elapsed += timer_Elapsed;
	}

	private void debugOutput(string strDebugText)
	{
		slblStatus.Text = strDebugText;
	}

	private void start_Proccessor()
	{
		sprogbarStatus.Maximum = 100;
		sprogbarStatus.Value = 0;
		sprogbarStatus.Value = 100;
	}

	public void Set_ProgressBar(int maxval, int val)
	{
		sprogbarStatus.Maximum = maxval;
		if (val > maxval)
		{
			sprogbarStatus.Value = maxval;
		}
		else
		{
			sprogbarStatus.Value = val;
		}
	}

	private void Load_Settings()
	{
		string[] array = ConfigurationManager.ConnectionStrings["Filter"].ConnectionString.Split(new string[1] { ";" }, StringSplitOptions.RemoveEmptyEntries);
		mFilter = (string[])array.Clone();
		int num = 0;
		foreach (string mDecimal in mDecimals)
		{
			string[] array2 = mDecimal.Split('#');
			mFilter[num] = array2[0];
			num++;
		}
		cbSameTool.Checked = Settings.Default.IsSameTool;
		cbShowChart.Checked = Settings.Default.IsShowChart;
		if (Settings.Default.IsVertical)
		{
			rbVertical.Checked = true;
		}
		else
		{
			rbHorizontal.Checked = true;
		}
		loadDecimals();
		mRowHeaders = Settings.Default.Headers.Split(new string[1] { ";" }, StringSplitOptions.RemoveEmptyEntries).ToList();
	}

	private void loadDecimals()
	{
		try
		{
			QueryArgs queryArgs = new QueryArgs();
			queryArgs.Predicate = "TypeId=@0";
			queryArgs.PredicateParameters = new string[1] { "438D7052-25F3-4342-ED0C-08D7E9C5C77D" };
			queryArgs.Order = "Created DESC";
			queryArgs.Page = 1;
			queryArgs.Limit = int.MaxValue;
			QueryArgs body = queryArgs;
			ResponseDto result = frmLogin.client.GetsAsync(body, "/api/MetadataValue/GetDecimals").Result;
			mDecimals = Common.getObjects<string>(result);
		}
		catch (Exception ex)
		{
			mDecimals = new List<string>();
			if (ex.InnerException is ApiException { StatusCode: var statusCode } ex2)
			{
				if (statusCode.Equals(401))
				{
					if (MessageBox.Show(Common.getTextLanguage(this, "LoginAgain"), Common.getTextLanguage(this, "INFORMATION"), MessageBoxButtons.OK, MessageBoxIcon.Asterisk).Equals(DialogResult.OK))
					{
						Close();
					}
				}
				else
				{
					debugOutput("ERR: " + ex2.Message.Replace("\n", ""));
					MessageBox.Show(ex2.Message, Common.getTextLanguage(this, "ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
			}
			else
			{
				debugOutput("ERR: " + ex.Message);
				MessageBox.Show(ex.Message, Common.getTextLanguage(this, "ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}
	}

	private void load_AllData()
	{
		start_Proccessor();
		mMeass = Get_Measurement();
		Init_dgvMain(mMeass);
		Load_DataChart();
	}

	private void Ws_OnMessage(object sender, MessageEventArgs e)
	{
		if (e.Data.Contains("GetId@"))
		{
			if (mIdTool != Guid.Empty)
			{
				mWs.Send($"SetId@{mIdTool}");
			}
		}
		else if (e.Data.Contains("Successful"))
		{
			timer_Elapsed(mTimer, null);
			mTimer.Start();
		}
		else
		{
			Set_ResultForSocket(e.Data);
		}
	}

	private void Set_imgSignal(bool isconnect = true)
	{
		BeginInvoke((MethodInvoker)delegate
		{
			if (isconnect)
			{
				imgSignal.Text = Common.getTextLanguage(this, "Connected");
				imgSignal.ForeColor = Color.Blue;
				imgSignal.Image = Resources.wifi_blue;
			}
			else
			{
				imgSignal.Text = Common.getTextLanguage(this, "Disconnected");
				imgSignal.ForeColor = Color.Red;
				imgSignal.Image = Resources.unwifi_red;
			}
		});
	}

	private Guid Get_TabletId()
	{
		Guid result = Guid.Empty;
		try
		{
			ResponseDto responseDto = new ResponseDto();
			QueryArgs queryArgs = new QueryArgs();
			queryArgs.Predicate = "Name=@0";
			queryArgs.PredicateParameters = new string[1] { frmLogin.mMachineName };
			queryArgs.Order = "Created";
			queryArgs.Page = 1;
			queryArgs.Limit = 1;
			QueryArgs body = queryArgs;
			responseDto = frmLogin.client.GetsAsync(body, "/api/Machine/Gets").Result;
			List<MachineViewModel> objects = Common.getObjects<MachineViewModel>(responseDto);
			if (objects.Count > 0)
			{
				result = objects[0].Id;
			}
		}
		catch (Exception ex)
		{
			if (ex.InnerException is ApiException { StatusCode: var statusCode } ex2)
			{
				if (statusCode.Equals(401))
				{
					if (MessageBox.Show(Common.getTextLanguage(this, "LoginAgain"), Common.getTextLanguage(this, "INFORMATION"), MessageBoxButtons.OK, MessageBoxIcon.Asterisk).Equals(DialogResult.OK))
					{
						Close();
					}
				}
				else
				{
					debugOutput("ERR: " + ex2.Message.Replace("\n", ""));
					MessageBox.Show(ex2.Message, Common.getTextLanguage(this, "ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
			}
			else
			{
				debugOutput("ERR: " + ex.Message);
				MessageBox.Show(ex.Message, Common.getTextLanguage(this, "ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}
		return result;
	}

	private DataTable Get_Measurement()
	{
		DataTable result = new DataTable();
		try
		{
			string text = string.Empty;
			string[] array = mFilter;
			foreach (string text2 in array)
			{
				text = text + "MachineType.Name=\"" + text2.Trim() + "\"||";
			}
			if (!string.IsNullOrEmpty(text))
			{
				text = "&&(" + text.TrimEnd('|') + ")";
			}
			QueryArgs queryArgs = new QueryArgs();
			queryArgs.Predicate = "ProductId=@0" + text;
			queryArgs.PredicateParameters = new string[1] { mRequest.ProductId.ToString() };
			queryArgs.Order = "Sort, Created";
			queryArgs.Page = 1;
			queryArgs.Limit = int.MaxValue;
			QueryArgs body = queryArgs;
			ResponseDto result2 = frmLogin.client.GetsAsync(body, "/api/Measurement/Gets").Result;
			result = Common.getDataTable<MeasurementQuickViewModel>(result2);
		}
		catch (Exception ex)
		{
			if (ex.InnerException is ApiException { StatusCode: var statusCode } ex2)
			{
				if (statusCode.Equals(401))
				{
					if (MessageBox.Show(Common.getTextLanguage(this, "LoginAgain"), Common.getTextLanguage(this, "INFORMATION"), MessageBoxButtons.OK, MessageBoxIcon.Asterisk).Equals(DialogResult.OK))
					{
						Close();
					}
				}
				else
				{
					debugOutput("ERR: " + ex2.Message.Replace("\n", ""));
					MessageBox.Show(ex2.Message, Common.getTextLanguage(this, "ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
			}
			else
			{
				debugOutput("ERR: " + ex.Message);
				MessageBox.Show(ex.Message, Common.getTextLanguage(this, "ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}
		return result;
	}

	private List<ResultViewModel> Get_Results()
	{
		List<ResultViewModel> result = new List<ResultViewModel>();
		try
		{
			string text = string.Empty;
			string[] array = mFilter;
			foreach (string text2 in array)
			{
				text = text + "Measurement.MachineType.Name=\"" + text2.Trim() + "\"||";
			}
			if (!string.IsNullOrEmpty(text))
			{
				text = "&&(" + text.TrimEnd('|') + ")";
			}
			QueryArgs queryArgs = new QueryArgs();
			queryArgs.Predicate = "RequestId=@0" + text;
			queryArgs.PredicateParameters = new string[1] { mRequest.Id.ToString() };
			queryArgs.Order = "Measurement.Sort";
			queryArgs.Page = 1;
			queryArgs.Limit = int.MaxValue;
			QueryArgs body = queryArgs;
			ResponseDto result2 = frmLogin.client.GetsAsync(body, "/api/RequestResult/Gets").Result;
			result = Common.getObjects<ResultViewModel>(result2);
		}
		catch (Exception ex)
		{
			if (ex.InnerException is ApiException { StatusCode: var statusCode } ex2)
			{
				if (statusCode.Equals(401))
				{
					if (MessageBox.Show(Common.getTextLanguage(this, "LoginAgain"), Common.getTextLanguage(this, "INFORMATION"), MessageBoxButtons.OK, MessageBoxIcon.Asterisk).Equals(DialogResult.OK))
					{
						Close();
					}
				}
				else
				{
					debugOutput("ERR: " + ex2.Message.Replace("\n", ""));
					MessageBox.Show(ex2.Message, Common.getTextLanguage(this, "ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
			}
			else
			{
				debugOutput("ERR: " + ex.Message);
				MessageBox.Show(ex.Message, Common.getTextLanguage(this, "ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}
		return result;
	}

	private ResultViewModel Get_Result()
	{
		if (dgvMain.CurrentCell == null || dgvMain.CurrentCellAddress.X == 0 || !int.TryParse(lblSample.Text, out var sample) || !int.TryParse(lblCavity.Text, out var cavity) || !Guid.TryParse(dgvMain.Rows[0].Cells[dgvMain.CurrentCellAddress.X].Value.ToString(), out var id))
		{
			return null;
		}
		return mResults.FirstOrDefault((ResultViewModel x) => x.Sample == sample && x.Cavity == cavity && x.MeasurementId == id);
	}

	private CellDto Get_Cell(dynamic result)
	{
		CellDto result2 = null;
		int num = -1;
		int num2 = -1;
		for (int i = 1; i < dgvMain.ColumnCount; i++)
		{
			if (dgvMain.Rows[0].Cells[i].Value.ToString() == result.MeasurementId.ToString())
			{
				num = i;
				break;
			}
		}
		for (int j = 7; j < dgvMain.RowCount; j++)
		{
			if (j > ROW_SAMPLE_LAST && dgvMain.Rows[j].Cells[0].Value.ToString() == result.Sample.ToString())
			{
				num2 = j;
				break;
			}
			string[] array = Get_SampleCavity(dgvMain.Rows[j]);
			if (array[0] == result.Sample.ToString() && array[1] == result.Cavity.ToString())
			{
				num2 = j;
				break;
			}
		}
		if (num != -1 && num2 != -1)
		{
			result2 = new CellDto
			{
				Row = num2,
				Column = num
			};
		}
		return result2;
	}

	private string[] Get_SampleCavity(DataGridViewRow row)
	{
		string[] array = new string[2] { "...", "..." };
		if (row != null)
		{
			string[] array2 = row.Cells[0].Value.ToString().Split('#');
			if (int.TryParse(array2[0], out var _))
			{
				array[0] = array2[0].Trim();
				array[1] = ((array2.Length > 1) ? array2[1].Trim() : "1");
			}
		}
		return array;
	}

	private List<dynamic> Cal_dgvMain(List<ResultViewModel> results)
	{
		List<object> list = new List<object>();
		results = results.Where((ResultViewModel x) => !string.IsNullOrEmpty(x.Result)).ToList();
		List<ResultViewModel> source = results.Select((ResultViewModel x) => x.Clone()).ToList();
		for (int num = 0; num < source.Count() - 1; num++)
		{
			string text = Regex.Match(source.ElementAt(num).Name, "^.*(?=\\d)").Value;
			if (!text.ToUpper().Contains("LENGTH") && !text.ToUpper().Contains("WIDTH") && !text.ToUpper().Contains("THICKNESS"))
			{
				continue;
			}
			for (int num2 = num + 1; num2 < source.Count(); num2++)
			{
				string text2 = Regex.Match(source.ElementAt(num2).Name, "^.*(?=\\d)").Value;
				if (text == text2)
				{
					source.ElementAt(num).Name = text;
					source.ElementAt(num2).MeasurementId = source.ElementAt(num).MeasurementId;
					source.ElementAt(num2).Name = source.ElementAt(num).Name;
					source.ElementAt(num2).Value = source.ElementAt(num).Value;
					source.ElementAt(num2).Upper = source.ElementAt(num).Upper;
					source.ElementAt(num2).Lower = source.ElementAt(num).Lower;
				}
			}
		}
		IEnumerable<IGrouping<Guid?, ResultViewModel>> enumerable = from x in source
			group x by x.MeasurementId;
		foreach (IGrouping<Guid?, ResultViewModel> item in enumerable)
		{
			string result = "-";
			string result2 = "-";
			string result3 = "-";
			string result4 = "OK";
			string result5 = "-";
			string result6 = "-";
			string result7 = "-";
			string result8 = "-";
			if (!string.IsNullOrEmpty(item.First().Unit))
			{
				List<double> list2 = item.Select((ResultViewModel x) => double.Parse(x.Result)).ToList();
				result = list2.Max().ToString();
				result2 = list2.Min().ToString();
				double num3 = list2.Average();
				result3 = num3.ToString("0.####");
				double num4 = mMath.CalStandardDeviation(list2);
				double? num5 = (item.First().USL - item.First().LSL) / (6.0 * num4);
				double? num6 = (item.First().USL + item.First().LSL) / 2.0;
				double? obj = (item.First().Upper - item.First().Lower) / 2.0;
				double? num7 = (1.0 - Math.Abs((num6 - num3).Value) / obj) * num5;
				if (!num4.Equals(double.NaN))
				{
					result5 = num4.ToString("0.####");
				}
				if (!num5.Equals(double.NaN))
				{
					result6 = num5?.ToString("0.####");
				}
				if (!num7.Equals(double.NaN))
				{
					result7 = num7?.ToString("0.####");
					result8 = "RANK1";
					if (num7 < 0.67)
					{
						result8 = "RANK5";
					}
					else if (num7 <= 1.0)
					{
						result8 = "RANK4";
					}
					else if (num7 <= 1.3)
					{
						result8 = "RANK3";
					}
					else if (num7 < 1.7)
					{
						result8 = "RANK2";
					}
				}
			}
			foreach (ResultViewModel item2 in item)
			{
				if (item2.Judge.Contains("NG"))
				{
					result4 = "NG";
					break;
				}
			}
			list.Add(new
			{
				MeasurementId = item.Key,
				Sample = Common.getTextLanguage(this, "Maximum"),
				MachineTypeName = item.First().MachineTypeName,
				Result = result,
				MeasurementUnit = item.First().MeasurementUnit
			});
			list.Add(new
			{
				MeasurementId = item.Key,
				Sample = Common.getTextLanguage(this, "Minimum"),
				MachineTypeName = item.First().MachineTypeName,
				Result = result2,
				MeasurementUnit = item.First().MeasurementUnit
			});
			list.Add(new
			{
				MeasurementId = item.Key,
				Sample = Common.getTextLanguage(this, "Average"),
				MachineTypeName = item.First().MachineTypeName,
				Result = result3,
				MeasurementUnit = item.First().MeasurementUnit
			});
			list.Add(new
			{
				MeasurementId = item.Key,
				Sample = Common.getTextLanguage(this, "Judge"),
				MachineTypeName = item.First().MachineTypeName,
				Result = result4,
				MeasurementUnit = ""
			});
			list.Add(new
			{
				MeasurementId = item.Key,
				Sample = Common.getTextLanguage(this, "StDev"),
				MachineTypeName = item.First().MachineTypeName,
				Result = result5,
				MeasurementUnit = ""
			});
			list.Add(new
			{
				MeasurementId = item.Key,
				Sample = Common.getTextLanguage(this, "Cp"),
				MachineTypeName = item.First().MachineTypeName,
				Result = result6,
				MeasurementUnit = ""
			});
			list.Add(new
			{
				MeasurementId = item.Key,
				Sample = Common.getTextLanguage(this, "Cpk"),
				MachineTypeName = item.First().MachineTypeName,
				Result = result7,
				MeasurementUnit = ""
			});
			list.Add(new
			{
				MeasurementId = item.Key,
				Sample = Common.getTextLanguage(this, "Rank"),
				MachineTypeName = item.First().MachineTypeName,
				Result = result8,
				MeasurementUnit = ""
			});
		}
		return list;
	}

	private void Cal_Judge()
	{
		int num = 0;
		int num2 = 0;
		for (int i = 7; i <= ROW_SAMPLE_LAST; i++)
		{
			for (int j = 1; j < dgvMain.ColumnCount; j++)
			{
				if (dgvMain.Rows[i].Cells[j].Value != null && !string.IsNullOrEmpty(dgvMain.Rows[i].Cells[j].Value.ToString()))
				{
					Color foreColor = dgvMain.Rows[i].Cells[j].Style.ForeColor;
					if (foreColor == Color.Blue)
					{
						num++;
					}
					else
					{
						num2++;
					}
				}
			}
		}
		int num3 = mTotalResult - (num + num2);
		lblOK.Text = num.ToString();
		lblNG.Text = num2.ToString();
		lblEmpty.Text = num3.ToString();
	}

	private void Set_dgvMain()
	{
		start_Proccessor();
		Cursor.Current = Cursors.WaitCursor;
		mResults = Get_Results();
		foreach (ResultViewModel mResult in mResults)
		{
			CellDto cellDto = Get_Cell(mResult);
			if (cellDto == null)
			{
				continue;
			}
			string s = mResult.Result;
			if (double.TryParse(s, out var result))
			{
				s = ((!(mResult.MeasurementUnit == "°")) ? Common.ToString(result, mResult.MachineTypeName, mDecimals) : Common.ConvertDoubleToDegrees(result, uniformity: true));
			}
			dgvMain.Rows[cellDto.Row].Cells[cellDto.Column].Value = s;
			if (mResult.Judge != null)
			{
				if (mResult.Judge.Contains("OK"))
				{
					dgvMain.Rows[cellDto.Row].Cells[cellDto.Column].Style.ForeColor = Color.Blue;
				}
				else if (mResult.Judge.Contains("NG"))
				{
					dgvMain.Rows[cellDto.Row].Cells[cellDto.Column].Style.ForeColor = Color.Red;
				}
				if (string.IsNullOrEmpty(mResult.Unit))
				{
					dgvMain.Rows[cellDto.Row].Cells[cellDto.Column].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
				}
			}
		}
		List<object> list = Cal_dgvMain(mResults);
		double num = default(double);
		foreach (dynamic item in list)
		{
			dynamic val = Get_Cell(item);
			if (!((val == null) ? true : false))
			{
				dynamic val2 = item.Result;
				if (double.TryParse(val2, out num))
				{
					val2 = ((!((item.MeasurementUnit == "°") ? true : false)) ? Common.ToString(num, item.MachineTypeName, mDecimals) : Common.ConvertDoubleToDegrees(num, uniformity: true));
				}
				dgvMain.Rows[val.Row].Cells[val.Column].Value = val2;
				if (item.Result.Contains("OK"))
				{
					dgvMain.Rows[val.Row].Cells[val.Column].Style.ForeColor = Color.Blue;
				}
				else if (item.Result.Contains("NG") || item.Result.Contains("RANK5"))
				{
					dgvMain.Rows[val.Row].Cells[val.Column].Style.ForeColor = Color.Red;
				}
				else if (item.Result.Contains("RANK3") || item.Result.Contains("RANK4"))
				{
					dgvMain.Rows[val.Row].Cells[val.Column].Style.ForeColor = Color.Orange;
				}
				else if (item.Result.Contains("RANK1") || item.Result.Contains("RANK2"))
				{
					dgvMain.Rows[val.Row].Cells[val.Column].Style.ForeColor = Color.Black;
				}
			}
		}
		Cal_Judge();
		debugOutput(Common.getTextLanguage(this, "Successful"));
	}

	private bool Save_Result(string machineName = "Manual Input")
	{
		bool result = false;
		string text = ((dgvMain.CurrentCell.Value != null) ? dgvMain.CurrentCell.Value.ToString() : string.Empty);
		if (double.TryParse(dgvMain.Rows[2].Cells[dgvMain.CurrentCellAddress.X].Value.ToString(), out var result2) && !string.IsNullOrEmpty(text))
		{
			if (!double.TryParse(text.Replace("-", ""), out var result3))
			{
				debugOutput(Common.getTextLanguage(this, "Incorrect"));
				dgvMain.CurrentCell.Value = mRemember;
				return result;
			}
			double.TryParse(dgvMain.Rows[4].Cells[dgvMain.CurrentCellAddress.X].Value.ToString(), out var result4);
			double.TryParse(dgvMain.Rows[5].Cells[dgvMain.CurrentCellAddress.X].Value.ToString(), out var result5);
			double num = (result4 - result5) * mDeviation;
			double num2 = result2 + result5 - num;
			double num3 = result2 + result4 + num;
			if (result3 < num2 || result3 > num3)
			{
				debugOutput(Common.getTextLanguage(this, "OutRange"));
				dgvMain.CurrentCell.Value = mRemember;
				return result;
			}
		}
		else if (dgvMain.Rows[3].Cells[dgvMain.CurrentCellAddress.X].Value?.ToString() == "°")
		{
			text = Common.ConvertDegreesToDouble(text)?.ToString();
		}
		try
		{
			Guid id = Get_Result()?.Id ?? Guid.Empty;
			ResultViewModel body = new ResultViewModel
			{
				Id = id,
				RequestId = mRequest.Id,
				MeasurementId = Guid.Parse(dgvMain.Rows[0].Cells[dgvMain.CurrentCellAddress.X].Value.ToString()),
				Result = text,
				MachineName = machineName,
				StaffName = frmLogin.User.FullName,
				Sample = int.Parse(lblSample.Text),
				Cavity = int.Parse(lblCavity.Text),
				IsActivated = true
			};
			Cursor.Current = Cursors.WaitCursor;
			ResponseDto result6 = frmLogin.client.SaveAsync(body, "/api/RequestResult/Save").Result;
			if (!result6.Success)
			{
				throw new Exception(result6.Messages.ElementAt(0).Message);
			}
			isClose = false;
			result = true;
			if (result6.Data == null)
			{
				dgvMain.CurrentCell.Value = mRemember;
				return result;
			}
			string text2 = result6.Data.ToString();
			ChartViewModel model = JsonConvert.DeserializeObject<ChartViewModel>(text2);
			if (model.Judge != null)
			{
				if (model.Judge.Contains("OK"))
				{
					Console.Beep();
				}
				else
				{
					Console.Beep(4000, 500);
				}
			}
			if (!string.IsNullOrEmpty(model.Unit))
			{
				model.RequestDate = mRequest.Date;
				ChartViewModel chartViewModel = mPanelChartMain.mModels.FirstOrDefault((ChartViewModel x) => x.Id == model.Id);
				if (chartViewModel == null)
				{
					if (!string.IsNullOrEmpty(model.Result))
					{
						mPanelChartMain.mModels.Add(model);
					}
				}
				else if (string.IsNullOrEmpty(model.Result))
				{
					mPanelChartMain.mModels.Remove(chartViewModel);
				}
				else
				{
					chartViewModel.Result = model.Result;
				}
			}
		}
		catch (Exception ex)
		{
			dgvMain.CurrentCell.Value = mRemember;
			if (ex.InnerException is ApiException { StatusCode: var statusCode } ex2)
			{
				if (statusCode.Equals(401))
				{
					if (MessageBox.Show(Common.getTextLanguage(this, "LoginAgain"), Common.getTextLanguage(this, "INFORMATION"), MessageBoxButtons.OK, MessageBoxIcon.Asterisk).Equals(DialogResult.OK))
					{
						Close();
					}
				}
				else
				{
					debugOutput("ERR: " + ex2.Message.Replace("\n", ""));
					MessageBox.Show(ex2.Message, Common.getTextLanguage(this, "ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
			}
			else
			{
				debugOutput("ERR: " + ex.Message);
				MessageBox.Show(ex.Message, Common.getTextLanguage(this, "ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}
		finally
		{
			Set_dgvMain();
		}
		return result;
	}

	private void Set_ResultForSocket(string json)
	{
		if (!json.StartsWith("{") || !json.EndsWith("}"))
		{
			return;
		}
		ToolResultDto item = JsonConvert.DeserializeObject<ToolResultDto>(json);
		if (item == null || item.Result == null)
		{
			return;
		}
		Invoke((MethodInvoker)delegate
		{
			lblToolName.Text = (string.IsNullOrEmpty(item.MachineName) ? "..." : item.MachineName);
			lblToolType.Text = (string.IsNullOrEmpty(item.MachineTypeName) ? "..." : item.MachineTypeName);
			lblResult.Text = item.Result + item.Unit;
			if (!item.MachineId.HasValue)
			{
				mPanelImage.Load_Tool();
			}
			else if (!item.TabletId.HasValue)
			{
				if ((mFilter.Length == 0 || mFilter.Any((string x) => x.Equals(item.MachineTypeName))) && !mPanelImage.btnRegister.Enabled && Register_Tool(item.MachineId))
				{
					mPanelImage.Load_Tool();
				}
			}
			else if (dgvMain.CurrentCell != null && !dgvMain.CurrentCell.ReadOnly && dgvMain.Rows[2].Cells[dgvMain.CurrentCellAddress.X].Value != dgvMain.Rows[3].Cells[dgvMain.CurrentCellAddress.X].Value)
			{
				string text = dgvMain.Rows[6].Cells[dgvMain.CurrentCellAddress.X].Value.ToString();
				if (item.MachineTypeName != text)
				{
					Common.getTextLanguage(this, "ToolIncorrect");
				}
				else
				{
					dgvMain.CurrentCell.Value = item.Result;
					if (mRemember != item.Result)
					{
						Save_Result(item.MachineName);
					}
					Next_Result();
				}
			}
		});
	}

	private bool Get_CellIsValue(int row, int col, out int rowout, out int colout)
	{
		bool result = false;
		if (rbVertical.Checked)
		{
			row++;
			if (row > ROW_SAMPLE_LAST)
			{
				row = 7;
				col++;
				if (cbSameTool.Checked)
				{
					col = Check_ColSameTool(col);
				}
			}
		}
		else if (cbSameTool.Checked)
		{
			col = Check_ColSameTool(col + 1);
			if (col == 0 && row < ROW_SAMPLE_LAST)
			{
				row++;
				col = Check_ColSameTool(1);
			}
		}
		else
		{
			col++;
			if (col > dgvMain.ColumnCount - 1)
			{
				col = 1;
				row++;
			}
		}
		if (row > ROW_SAMPLE_LAST)
		{
			row = 1;
		}
		if (col > dgvMain.ColumnCount - 1)
		{
			col = 0;
		}
		if (col != 0 && row != 1 && (dgvMain.Rows[row].Cells[col].ReadOnly || (dgvMain.Rows[row].Cells[col].Value != null && !string.IsNullOrEmpty(dgvMain.Rows[row].Cells[col].Value.ToString()))))
		{
			result = true;
		}
		rowout = row;
		colout = col;
		return result;
	}

	private void Next_Result()
	{
		int row = dgvMain.CurrentCellAddress.Y;
		int col = dgvMain.CurrentCellAddress.X;
		while (Get_CellIsValue(row, col, out row, out col))
		{
		}
		if (row > dgvMain.RowCount - 1)
		{
			row = 1;
		}
		if (col > dgvMain.ColumnCount - 1)
		{
			col = 0;
		}
		BeginInvoke((MethodInvoker)delegate
		{
			dgvMain.Rows[row].Cells[col].Selected = true;
		});
	}

	private int Check_ColSameTool(int column)
	{
		int result = 0;
		for (int i = column; i < dgvMain.ColumnCount; i++)
		{
			string text = dgvMain.Rows[6].Cells[i].Value.ToString();
			if (lblTool.Text == text)
			{
				result = i;
				break;
			}
		}
		return result;
	}

	private bool Register_Tool(Guid? id)
	{
		ToolViewModel body = new ToolViewModel
		{
			MachineId = id,
			TabletId = mIdTool
		};
		ResponseDto result = frmLogin.client.SaveAsync(body, "/api/Tool/Save").Result;
		return result.Success;
	}

	private string Get_Coordinate(Guid idmeas)
	{
		string result = string.Empty;
		DataRow dataRow = mMeass.Rows.Cast<DataRow>().First((DataRow x) => x.Field<Guid>("Id").Equals(idmeas));
		if (dataRow != null)
		{
			result = dataRow.Field<string>("Coordinate");
		}
		return result;
	}

	private void Load_DataChart()
	{
		Cursor.Current = Cursors.WaitCursor;
		if (mChartNumber > 0)
		{
			ResultForChartDto dto = new ResultForChartDto
			{
				Date = mRequest.Date,
				ProductId = mRequest.ProductId,
				Limit = mChartNumber
			};
			mPanelChartMain.Set_Chart(dto, mChartNumber);
		}
	}

	private List<LimitViewModel> loadSampleWithAQL()
	{
		List<LimitViewModel> result = new List<LimitViewModel>();
		if (!mRequest.Quantity.HasValue || mRequest.ProductId == Guid.Empty)
		{
			return result;
		}
		try
		{
			AQLDto body = new AQLDto
			{
				ProductId = mRequest.ProductId,
				InputQuantity = mRequest.Quantity
			};
			ResponseDto result2 = frmLogin.client.GetsAsync(body, "/api/AQL/Samples").Result;
			result = Common.getObjects<LimitViewModel>(result2);
		}
		catch (Exception ex)
		{
			if (ex.InnerException is ApiException { StatusCode: var statusCode } ex2)
			{
				if (statusCode.Equals(401))
				{
					if (MessageBox.Show(Common.getTextLanguage(this, "LoginAgain"), Common.getTextLanguage(this, "INFORMATION"), MessageBoxButtons.OK, MessageBoxIcon.Asterisk).Equals(DialogResult.OK))
					{
						Close();
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
		return result;
	}

	private void setAQLForMeasurement()
	{
		new Task(delegate
		{
			Invoke((MethodInvoker)delegate
			{
				List<LimitViewModel> list = loadSampleWithAQL();
				if (list.Count >= 1)
				{
					List<MeasurementQuickViewModel> measurementIsRank = getMeasurementIsRank();
					List<ResultRankViewModel> resultRank = getResultRank();
					int col;
					for (col = 1; col < dgvMain.ColumnCount; col++)
					{
						MeasurementQuickViewModel measurementQuickViewModel = measurementIsRank.FirstOrDefault((MeasurementQuickViewModel x) => x.Id.ToString() == dgvMain.Rows[0].Cells[col].Value.ToString());
						if (measurementQuickViewModel != null)
						{
							dgvMain.Columns[col].HeaderText += "*";
						}
						LimitViewModel aql = list.FirstOrDefault((LimitViewModel x) => x.MeasurementId.ToString() == dgvMain.Rows[0].Cells[col].Value.ToString());
						if (aql != null)
						{
							ResultRankViewModel resultRankViewModel = resultRank.FirstOrDefault((ResultRankViewModel x) => x.MeasurementId == aql.MeasurementId);
							int? num = ((resultRankViewModel == null) ? new int?(0) : resultRankViewModel.Sample);
							for (int num2 = 7; num2 <= ROW_SAMPLE_LAST; num2++)
							{
								string s = dgvMain.Rows[num2].Cells[0].Value.ToString().Split('#')[0].Trim();
								if (!int.TryParse(s, out var result) || aql.Sample + num >= result)
								{
									if (aql.Sample < result)
									{
										dgvMain.Rows[num2].Cells[col].Style.BackColor = Color.LightYellow;
									}
								}
								else
								{
									dgvMain.Rows[num2].Cells[col].ReadOnly = true;
									dgvMain.Rows[num2].Cells[col].Style.BackColor = SystemColors.ScrollBar;
								}
							}
						}
					}
				}
			});
		}).Start();
	}

	private List<ResultRankViewModel> getResultRank()
	{
		List<ResultRankViewModel> result = new List<ResultRankViewModel>();
		try
		{
			QueryArgs queryArgs = new QueryArgs();
			queryArgs.Predicate = "RequestId=@0";
			queryArgs.PredicateParameters = new string[1] { mRequest.Id.ToString() };
			queryArgs.Order = "Created";
			queryArgs.Page = 1;
			queryArgs.Limit = int.MaxValue;
			QueryArgs body = queryArgs;
			ResponseDto result2 = frmLogin.client.GetsAsync(body, "/api/ResultRank/Gets").Result;
			result = Common.getObjects<ResultRankViewModel>(result2);
		}
		catch (Exception ex)
		{
			if (ex.InnerException is ApiException { StatusCode: var statusCode } ex2)
			{
				if (statusCode.Equals(401))
				{
					if (MessageBox.Show(Common.getTextLanguage(this, "LoginAgain"), Common.getTextLanguage(this, "INFORMATION"), MessageBoxButtons.OK, MessageBoxIcon.Asterisk).Equals(DialogResult.OK))
					{
						Close();
					}
				}
				else
				{
					debugOutput("ERR: " + ex2.Message.Replace("\n", ""));
					MessageBox.Show(ex2.Message, Common.getTextLanguage(this, "ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
			}
			else
			{
				debugOutput("ERR: " + ex.Message);
				MessageBox.Show(ex.Message, Common.getTextLanguage(this, "ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}
		return result;
	}

	private List<MeasurementQuickViewModel> getMeasurementIsRank()
	{
		List<MeasurementQuickViewModel> result = new List<MeasurementQuickViewModel>();
		try
		{
			QueryArgs queryArgs = new QueryArgs();
			queryArgs.Predicate = "ProductId=@0 && Important.Name=@1";
			queryArgs.PredicateParameters = new string[2]
			{
				mRequest.ProductId.ToString(),
				"Rank"
			};
			queryArgs.Order = "Sort, Created";
			queryArgs.Page = 1;
			queryArgs.Limit = int.MaxValue;
			QueryArgs body = queryArgs;
			ResponseDto result2 = frmLogin.client.GetsAsync(body, "/api/Measurement/Gets").Result;
			result = Common.getObjects<MeasurementQuickViewModel>(result2);
		}
		catch (Exception ex)
		{
			if (ex.InnerException is ApiException { StatusCode: var statusCode } ex2)
			{
				if (statusCode.Equals(401))
				{
					if (MessageBox.Show(Common.getTextLanguage(this, "LoginAgain"), Common.getTextLanguage(this, "INFORMATION"), MessageBoxButtons.OK, MessageBoxIcon.Asterisk).Equals(DialogResult.OK))
					{
						Close();
					}
				}
				else
				{
					debugOutput("ERR: " + ex2.Message.Replace("\n", ""));
					MessageBox.Show(ex2.Message, Common.getTextLanguage(this, "ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
			}
			else
			{
				debugOutput("ERR: " + ex.Message);
				MessageBox.Show(ex.Message, Common.getTextLanguage(this, "ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}
		return result;
	}

	private void drawMerge(DataTable dt)
	{
		new Task(delegate
		{
			Invoke((MethodInvoker)delegate
			{
				string text = string.Empty;
				int num = 0;
				int columnIndex = 0;
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					string text2 = Regex.Match(dt.Rows[i]["Name"].ToString(), "^.*(?=\\d)").Value.ToUpper();
					if (text2.Contains("LENGTH") || text2.Contains("WIDTH") || text2.Contains("THICKNESS"))
					{
						if (string.IsNullOrEmpty(text))
						{
							text = text2;
							columnIndex = i + 1;
						}
						if (text == text2)
						{
							num++;
						}
						else
						{
							for (int j = 2; j < 7; j++)
							{
								((DataGridViewTextBoxCellEx)dgvMain[columnIndex, j]).ColumnSpan = num;
								dgvMain[columnIndex, j].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
							}
							for (int k = ROW_SAMPLE_LAST + 1; k < dgvMain.RowCount; k++)
							{
								((DataGridViewTextBoxCellEx)dgvMain[columnIndex, k]).ColumnSpan = num;
								dgvMain[columnIndex, k].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
							}
							num = 1;
							text = text2;
							columnIndex = i + 1;
						}
						if (i == dt.Rows.Count - 1 && num > 1)
						{
							for (int l = 2; l < 7; l++)
							{
								((DataGridViewTextBoxCellEx)dgvMain[columnIndex, l]).ColumnSpan = num;
								dgvMain[columnIndex, l].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
							}
							for (int m = ROW_SAMPLE_LAST + 1; m < dgvMain.RowCount; m++)
							{
								((DataGridViewTextBoxCellEx)dgvMain[columnIndex, m]).ColumnSpan = num;
								dgvMain[columnIndex, m].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
							}
						}
					}
					else if (num > 1)
					{
						for (int n = 2; n < 7; n++)
						{
							((DataGridViewTextBoxCellEx)dgvMain[columnIndex, n]).ColumnSpan = num;
							dgvMain[columnIndex, n].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
						}
						for (int num2 = ROW_SAMPLE_LAST + 1; num2 < dgvMain.RowCount; num2++)
						{
							((DataGridViewTextBoxCellEx)dgvMain[columnIndex, num2]).ColumnSpan = num;
							dgvMain[columnIndex, num2].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
						}
						num = 0;
						text = text2;
						columnIndex = i + 1;
					}
					if (string.IsNullOrEmpty(dt.Rows[i]["UnitName"].ToString()))
					{
						((DataGridViewTextBoxCellEx)dgvMain[i + 1, 2]).RowSpan = 4;
						dgvMain[i + 1, 2].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
						dgvMain[i + 1, 2].Style.WrapMode = DataGridViewTriState.True;
					}
				}
			});
		}).Start();
	}

	private void initPanelHeaders()
	{
		new Task(delegate
		{
			Invoke((MethodInvoker)delegate
			{
				for (int i = 1; i < dgvMain.RowCount; i++)
				{
					if (i >= 7 && i <= ROW_SAMPLE_LAST)
					{
						dgvMain.Rows[i].Visible = true;
					}
					else
					{
						int idx = i;
						if (i > ROW_SAMPLE_LAST)
						{
							idx -= ROW_SAMPLE_LAST - 7 - 1;
						}
						bool flag = mRowHeaders.Any((string x) => x == idx.ToString());
						dgvMain.Rows[i].Visible = flag;
						dgvHeader.Rows.Add(flag, dgvMain[0, i].Value, i);
						if (flag)
						{
							dgvHeader.Rows[dgvHeader.RowCount - 1].DefaultCellStyle.Font = new Font(dgvHeader.Font, FontStyle.Bold);
						}
						else
						{
							dgvHeader.Rows[dgvHeader.RowCount - 1].DefaultCellStyle.Font = dgvHeader.Font;
						}
					}
				}
				dgvHeader.Size = new Size(dgvMain.Columns[0].Width + 40, dgvHeader.Rows.Count * 22 + 3);
				dgvHeader.CurrentCell = null;
				dgvHeader.Refresh();
			});
		}).Start();
	}

	private void setHeader(bool en)
	{
		Cursor.Current = Cursors.WaitCursor;
		int.TryParse(dgvHeader.CurrentRow.Cells["index"].Value.ToString(), out var result);
		int num = result;
		if (result > ROW_SAMPLE_LAST)
		{
			num -= ROW_SAMPLE_LAST - 7 - 1;
		}
		if (en)
		{
			dgvHeader.CurrentRow.DefaultCellStyle.Font = new Font(dgvHeader.Font, FontStyle.Bold);
			mRowHeaders.Add(num.ToString());
		}
		else
		{
			dgvHeader.CurrentRow.DefaultCellStyle.Font = dgvHeader.Font;
			mRowHeaders.Remove(num.ToString());
		}
		dgvMain.Rows[result].Visible = en;
		Settings.Default.Headers = string.Join(";", mRowHeaders);
		Settings.Default.Save();
	}

	private void disableForRequestType()
	{
		new Task(delegate
		{
			Invoke((MethodInvoker)delegate
			{
				string requestType = Common.getRequestType();
				bool flag = requestType != mRequest.Type;
				MatchCollection matchCollection = Regex.Matches(mRequest.Type, "\\d{1,2}:\\d{2}");
				if (matchCollection.Count < 2)
				{
					flag = false;
				}
				for (int i = 1; i < dgvMain.ColumnCount; i++)
				{
					for (int j = 7; j <= ROW_SAMPLE_LAST; j++)
					{
						dgvMain.Rows[j].Cells[i].ReadOnly = flag;
						if (flag)
						{
							dgvMain.Rows[j].Cells[i].Style.BackColor = SystemColors.ScrollBar;
						}
					}
				}
			});
		}).Start();
	}

	private void timer_Elapsed(object sender, ElapsedEventArgs e)
	{
		if (mWs != null && mWs.ReadyState == WebSocketState.Open)
		{
			Set_imgSignal();
		}
		else
		{
			Set_imgSignal(isconnect: false);
		}
	}

	private void dgvMain_CurrentCellChanged(object sender, EventArgs e)
	{
		mPanelViewMain.Visible = false;
		if (dgvMain.CurrentCell != null)
		{
			if (dgvMain.CurrentCellAddress.X != 0)
			{
				Guid.TryParse(dgvMain.Rows[0].Cells[dgvMain.CurrentCellAddress.X].Value.ToString(), out var result);
				mPanelImage.Move_ImageWithMeas(Get_Coordinate(result));
				if (cbShowChart.Checked && mChartNumber > 0 && dgvMain.Rows[2].Cells[dgvMain.CurrentCellAddress.X].Value != dgvMain.Rows[3].Cells[dgvMain.CurrentCellAddress.X].Value)
				{
					mPanelChartMain.Init(result);
				}
				else
				{
					mPanelChartMain.Visible = false;
				}
			}
			else
			{
				mPanelChartMain.Visible = false;
			}
			mRemember = ((dgvMain.CurrentCell.Value == null) ? string.Empty : dgvMain.CurrentCell.Value.ToString());
			string[] array = Get_SampleCavity(dgvMain.CurrentRow);
			lblSample.Text = array[0];
			lblCavity.Text = array[1];
			lblDimNo.Text = dgvMain.Rows[1].Cells[dgvMain.CurrentCellAddress.X].Value.ToString();
			lblTool.Text = dgvMain.Rows[6].Cells[dgvMain.CurrentCellAddress.X].Value.ToString();
			lblJudge.Text = dgvMain.Rows[dgvMain.RowCount - 1].Cells[dgvMain.CurrentCellAddress.X].Value.ToString();
			lblJudge.ForeColor = dgvMain.Rows[dgvMain.RowCount - 1].Cells[dgvMain.CurrentCellAddress.X].Style.ForeColor;
			lblRow.Text = dgvMain.CurrentCellAddress.Y.ToString();
			lblColumn.Text = (dgvMain.CurrentCellAddress.X + 1).ToString();
			lblValue.Text = ((dgvMain.CurrentCell.Value == null) ? string.Empty : dgvMain.CurrentCell.Value.ToString());
		}
		else
		{
			mPanelChartMain.Visible = false;
		}
	}

	private void dgvMain_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
	{
		DataGridViewTextBoxEditingControl dataGridViewTextBoxEditingControl = e.Control as DataGridViewTextBoxEditingControl;
		dataGridViewTextBoxEditingControl.KeyPress -= txtNormal_KeyPress;
		dataGridViewTextBoxEditingControl.KeyUp -= txtNormal_KeyUp;
		dataGridViewTextBoxEditingControl.KeyPress += txtNormal_KeyPress;
		if (dgvMain.Rows[2].Cells[dgvMain.CurrentCellAddress.X].Value == dgvMain.Rows[3].Cells[dgvMain.CurrentCellAddress.X].Value)
		{
			Rectangle cellDisplayRectangle = dgvMain.GetCellDisplayRectangle(dgvMain.CurrentCell.ColumnIndex, dgvMain.CurrentCell.RowIndex, cutOverflow: true);
			btnOK.Size = new Size(cellDisplayRectangle.Width / 2, cellDisplayRectangle.Height - 1);
			btnOK.Location = new Point(cellDisplayRectangle.X, cellDisplayRectangle.Y);
			btnOK.Visible = true;
			btnOK.BringToFront();
			btnNG.Size = new Size(cellDisplayRectangle.Width / 2 - 1, cellDisplayRectangle.Height - 1);
			btnNG.Location = new Point(cellDisplayRectangle.X + btnOK.Size.Width, cellDisplayRectangle.Y);
			btnNG.Visible = true;
			btnNG.BringToFront();
			btnClear.Size = new Size(cellDisplayRectangle.Width - 1, cellDisplayRectangle.Height - 1);
			btnClear.Location = new Point(cellDisplayRectangle.X, cellDisplayRectangle.Y + cellDisplayRectangle.Height);
			btnClear.Visible = true;
			btnClear.BringToFront();
		}
		else
		{
			dataGridViewTextBoxEditingControl.KeyUp += txtNormal_KeyUp;
		}
	}

	private void txtNormal_KeyPress(object sender, KeyPressEventArgs e)
	{
		if (dgvMain.Rows[2].Cells[dgvMain.CurrentCellAddress.X].Value == dgvMain.Rows[3].Cells[dgvMain.CurrentCellAddress.X].Value)
		{
			e.Handled = true;
			return;
		}
		DataGridViewTextBoxEditingControl dataGridViewTextBoxEditingControl = sender as DataGridViewTextBoxEditingControl;
		if (!char.IsDigit(e.KeyChar) && e.KeyChar != '\b' && e.KeyChar != '.' && e.KeyChar != '-' && e.KeyChar != '#')
		{
			e.Handled = true;
		}
		if (e.KeyChar != '.' && ((dataGridViewTextBoxEditingControl.Text.StartsWith("0") && dataGridViewTextBoxEditingControl.Text.Length.Equals(1)) || (dataGridViewTextBoxEditingControl.Text.StartsWith("-0") && dataGridViewTextBoxEditingControl.Text.Length.Equals(2))))
		{
			dataGridViewTextBoxEditingControl.Text = dataGridViewTextBoxEditingControl.Text.Replace("0", "");
			dataGridViewTextBoxEditingControl.Select(dataGridViewTextBoxEditingControl.Text.Length, 0);
		}
		if (e.KeyChar == '.' && dataGridViewTextBoxEditingControl.Text.Contains("."))
		{
			e.Handled = true;
		}
		if (e.KeyChar == '#' && dataGridViewTextBoxEditingControl.Text.Contains("#"))
		{
			e.Handled = true;
		}
		if (e.KeyChar == '-' && (!dataGridViewTextBoxEditingControl.SelectionStart.Equals(0) || dataGridViewTextBoxEditingControl.Text.Contains("-")))
		{
			e.Handled = true;
		}
	}

	private void txtNormal_KeyUp(object sender, KeyEventArgs e)
	{
		DataGridViewTextBoxEditingControl dataGridViewTextBoxEditingControl = sender as DataGridViewTextBoxEditingControl;
		if (!dataGridViewTextBoxEditingControl.Text.Contains("#"))
		{
			string text = dataGridViewTextBoxEditingControl.Text.Split('.')[0];
			if (text.Length > 3 && !dataGridViewTextBoxEditingControl.Text.Contains(".") && double.TryParse(text, out var result))
			{
				dataGridViewTextBoxEditingControl.Text = result.ToString("#,###");
			}
			dataGridViewTextBoxEditingControl.Select(dataGridViewTextBoxEditingControl.Text.Length, 0);
		}
	}

	private void dgvMain_CellEndEdit(object sender, DataGridViewCellEventArgs e)
	{
		btnOK.Visible = false;
		btnNG.Visible = false;
		btnClear.Visible = false;
		if (dgvMain.CurrentCell.Value != null && mRemember == dgvMain.CurrentCell.Value.ToString())
		{
			return;
		}
		if (dgvMain.CurrentCell.Value == null)
		{
			dgvMain.CurrentCell.Value = "";
		}
		string[] array = dgvMain.CurrentCell.Value.ToString().Split('#');
		if (array.Length > 1)
		{
			string mark = array.Last();
			MachineLienceViewModel machineLienceViewModel = mForm.mMachines.FirstOrDefault((MachineLienceViewModel x) => x.Mark.ToString() == mark);
			if (machineLienceViewModel == null)
			{
				dgvMain.CurrentCell.Value = mRemember;
				debugOutput(Common.getTextLanguage(this, "ErrorTool"));
			}
			else
			{
				string text = dgvMain.Rows[6].Cells[dgvMain.CurrentCellAddress.X].Value.ToString();
				if (machineLienceViewModel.MachineType == text)
				{
					dgvMain.CurrentCell.Value = array.First();
					Save_Result(machineLienceViewModel.Name);
				}
				else
				{
					dgvMain.CurrentCell.Value = mRemember;
					debugOutput(Common.getTextLanguage(this, "ToolIncorrect"));
				}
			}
		}
		else
		{
			Save_Result();
		}
		if (dgvMain.Rows[2].Cells[e.ColumnIndex].Value != dgvMain.Rows[3].Cells[e.ColumnIndex].Value)
		{
			Next_Result();
		}
	}

	private void btn_Click(object sender, EventArgs e)
	{
		Button button = sender as Button;
		dgvMain.CurrentCell.Value = ((button == btnClear) ? string.Empty : button.Text);
		Next_Result();
	}

	private void main_refreshToolStripMenuItem_Click(object sender, EventArgs e)
	{
		Load_DataChart();
		Set_dgvMain();
	}

	private void imgSignal_ButtonClick(object sender, EventArgs e)
	{
		Cursor.Current = Cursors.WaitCursor;
		if (mWs != null && mWs.ReadyState != WebSocketState.Open)
		{
			mWs.Connect();
		}
	}

	private void main_viewToolStripMenuItem_Click(object sender, EventArgs e)
	{
		ResultViewModel result = Get_Result();
		if (result != null)
		{
			mPanelViewMain.load_dgvContent(result);
			mPanelViewMain.Visible = true;
		}
	}

	private void btnHistory_Click(object sender, EventArgs e)
	{
		main_viewToolStripMenuItem.PerformClick();
	}

	private void rbVertical_CheckedChanged(object sender, EventArgs e)
	{
		Settings.Default.IsVertical = rbVertical.Checked;
		Settings.Default.Save();
	}

	private void cbShowChart_CheckedChanged(object sender, EventArgs e)
	{
		Settings.Default.IsShowChart = cbShowChart.Checked;
		Settings.Default.Save();
		if (!cbShowChart.Checked)
		{
			mPanelChartMain.Visible = false;
		}
		else
		{
			dgvMain_CurrentCellChanged(dgvMain, null);
		}
	}

	private void btnImage_Click(object sender, EventArgs e)
	{
		mPanelImage.Display();
		mPanelImage.Visible = true;
	}

	private void btnTool_Click(object sender, EventArgs e)
	{
		mPanelImage.Display(istool: true);
		mPanelImage.Visible = true;
	}

	private void cbSameTool_CheckedChanged(object sender, EventArgs e)
	{
		Settings.Default.IsSameTool = cbSameTool.Checked;
		Settings.Default.Save();
	}

	private void btnComplete_Click(object sender, EventArgs e)
	{
		new frmCompleteView(this, mRequest).ShowDialog();
	}

	private void btnRegister_Click(object sender, EventArgs e)
	{
		btnTool.PerformClick();
		mPanelImage.btnRegister.PerformClick();
	}

	private void btnEnterOK_Click(object sender, EventArgs e)
	{
		if (dgvMain.CurrentCell != null && dgvMain.Rows[2].Cells[dgvMain.CurrentCellAddress.X].Value == dgvMain.Rows[3].Cells[dgvMain.CurrentCellAddress.X].Value)
		{
			dgvMain.BeginEdit(selectAll: true);
			btn_Click(btnOK, null);
		}
	}

	private void btnEnterNG_Click(object sender, EventArgs e)
	{
		if (dgvMain.CurrentCell != null && dgvMain.Rows[2].Cells[dgvMain.CurrentCellAddress.X].Value == dgvMain.Rows[3].Cells[dgvMain.CurrentCellAddress.X].Value)
		{
			dgvMain.BeginEdit(selectAll: true);
			btn_Click(btnNG, null);
		}
	}

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		main_refreshToolStripMenuItem.PerformClick();
	}

	private void btnShowHeader_Click(object sender, EventArgs e)
	{
		dgvHeader.Visible = !dgvHeader.Visible;
	}

	private void dgvMain_MouseDown(object sender, MouseEventArgs e)
	{
		dgvHeader.Visible = false;
	}

	private void dgvHeader_CellContentClick(object sender, DataGridViewCellEventArgs e)
	{
		DataGridView dataGridView = sender as DataGridView;
		if (dataGridView.CurrentCell is DataGridViewCheckBoxCell { ReadOnly: false } dataGridViewCheckBoxCell)
		{
			dataGridViewCheckBoxCell.Value = dataGridViewCheckBoxCell.Value == null || !bool.Parse(dataGridViewCheckBoxCell.Value.ToString());
			setHeader(bool.Parse(dataGridViewCheckBoxCell.Value.ToString()));
			dataGridView.RefreshEdit();
			dataGridView.NotifyCurrentCellDirty(dirty: true);
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(_5S_QA_Client.frmResultView));
		this.toolTipMain = new System.Windows.Forms.ToolTip(this.components);
		this.btnHistory = new System.Windows.Forms.Button();
		this.btnImage = new System.Windows.Forms.Button();
		this.rbVertical = new System.Windows.Forms.RadioButton();
		this.rbHorizontal = new System.Windows.Forms.RadioButton();
		this.btnComplete = new System.Windows.Forms.Button();
		this.btnTool = new System.Windows.Forms.Button();
		this.cbSameTool = new System.Windows.Forms.CheckBox();
		this.btnRegister = new System.Windows.Forms.Button();
		this.btnEnterOK = new System.Windows.Forms.Button();
		this.btnEnterNG = new System.Windows.Forms.Button();
		this.btnRefresh = new System.Windows.Forms.Button();
		this.cbShowChart = new System.Windows.Forms.CheckBox();
		this.btnShowHeader = new System.Windows.Forms.Button();
		this.statusStripfrmMain = new System.Windows.Forms.StatusStrip();
		this.imgSignal = new System.Windows.Forms.ToolStripSplitButton();
		this.sprogbarStatus = new System.Windows.Forms.ToolStripProgressBar();
		this.slblStatus = new System.Windows.Forms.ToolStripStatusLabel();
		this.dgvMain = new System.Windows.Forms.DataGridView();
		this.contextMenuStripMain = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.main_refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.main_viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.tpanelInfor = new System.Windows.Forms.TableLayoutPanel();
		this.lblTotal = new System.Windows.Forms.Label();
		this.lblTotalDisplay = new System.Windows.Forms.Label();
		this.lblEmpty = new System.Windows.Forms.Label();
		this.lblEmptyDisplay = new System.Windows.Forms.Label();
		this.lblOK = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.lblNG = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.lblRow = new System.Windows.Forms.Label();
		this.lblColumn = new System.Windows.Forms.Label();
		this.lblValueDisplay = new System.Windows.Forms.Label();
		this.lblColumnDisplay = new System.Windows.Forms.Label();
		this.lblRowDisplay = new System.Windows.Forms.Label();
		this.lblValue = new System.Windows.Forms.Label();
		this.tpanelTitle = new System.Windows.Forms.TableLayoutPanel();
		this.lblCavity = new System.Windows.Forms.Label();
		this.lblCavityDisplay = new System.Windows.Forms.Label();
		this.lblSample = new System.Windows.Forms.Label();
		this.lblSampleDisplay = new System.Windows.Forms.Label();
		this.lblResult = new System.Windows.Forms.Label();
		this.lblResultDisplay = new System.Windows.Forms.Label();
		this.lblToolType = new System.Windows.Forms.Label();
		this.lblToolTypeDisplay = new System.Windows.Forms.Label();
		this.lblToolName = new System.Windows.Forms.Label();
		this.lblToolNameDisplay = new System.Windows.Forms.Label();
		this.lblTool = new System.Windows.Forms.Label();
		this.lblDimNo = new System.Windows.Forms.Label();
		this.lblJudgeDisplay = new System.Windows.Forms.Label();
		this.lblDimNoDisplay = new System.Windows.Forms.Label();
		this.lblToolDisplay = new System.Windows.Forms.Label();
		this.lblJudge = new System.Windows.Forms.Label();
		this.tpanelButton = new System.Windows.Forms.TableLayoutPanel();
		this.panelLogout = new System.Windows.Forms.Panel();
		this.lblFullname = new System.Windows.Forms.Label();
		this.ptbAvatar = new System.Windows.Forms.PictureBox();
		this.dgvHeader = new System.Windows.Forms.DataGridView();
		this.title = new System.Windows.Forms.DataGridViewCheckBoxColumn();
		this.value = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.index = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.mPanelViewMain = new _5S_QA_Client.mPanelView();
		this.mPanelChartMain = new _5S_QA_Client.mPanelChart();
		this.mPanelImage = new _5S_QA_Client.mPanelImage();
		this.statusStripfrmMain.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.dgvMain).BeginInit();
		this.contextMenuStripMain.SuspendLayout();
		this.tpanelInfor.SuspendLayout();
		this.tpanelTitle.SuspendLayout();
		this.tpanelButton.SuspendLayout();
		this.panelLogout.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.ptbAvatar).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.dgvHeader).BeginInit();
		base.SuspendLayout();
		this.btnHistory.AutoSize = true;
		this.btnHistory.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnHistory.Dock = System.Windows.Forms.DockStyle.Fill;
		this.btnHistory.Location = new System.Drawing.Point(99, 1);
		this.btnHistory.Margin = new System.Windows.Forms.Padding(0);
		this.btnHistory.Name = "btnHistory";
		this.btnHistory.Size = new System.Drawing.Size(88, 30);
		this.btnHistory.TabIndex = 2;
		this.btnHistory.Text = "View history";
		this.toolTipMain.SetToolTip(this.btnHistory, "Select view history of result");
		this.btnHistory.UseVisualStyleBackColor = true;
		this.btnHistory.Click += new System.EventHandler(btnHistory_Click);
		this.btnImage.AutoSize = true;
		this.btnImage.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnImage.Dock = System.Windows.Forms.DockStyle.Fill;
		this.btnImage.Location = new System.Drawing.Point(188, 1);
		this.btnImage.Margin = new System.Windows.Forms.Padding(0);
		this.btnImage.Name = "btnImage";
		this.btnImage.Size = new System.Drawing.Size(133, 30);
		this.btnImage.TabIndex = 3;
		this.btnImage.Text = "Drawing - Comment";
		this.toolTipMain.SetToolTip(this.btnImage, "Select view drawing of product or comment of request");
		this.btnImage.UseVisualStyleBackColor = true;
		this.btnImage.Click += new System.EventHandler(btnImage_Click);
		this.rbVertical.AutoSize = true;
		this.rbVertical.Checked = true;
		this.rbVertical.Cursor = System.Windows.Forms.Cursors.Hand;
		this.rbVertical.Dock = System.Windows.Forms.DockStyle.Fill;
		this.rbVertical.Location = new System.Drawing.Point(594, 4);
		this.rbVertical.Name = "rbVertical";
		this.rbVertical.Size = new System.Drawing.Size(143, 24);
		this.rbVertical.TabIndex = 6;
		this.rbVertical.TabStop = true;
		this.rbVertical.Text = "Next result vertically";
		this.toolTipMain.SetToolTip(this.rbVertical, "Select next result vertically");
		this.rbVertical.UseVisualStyleBackColor = true;
		this.rbVertical.CheckedChanged += new System.EventHandler(rbVertical_CheckedChanged);
		this.rbHorizontal.AutoSize = true;
		this.rbHorizontal.Cursor = System.Windows.Forms.Cursors.Hand;
		this.rbHorizontal.Dock = System.Windows.Forms.DockStyle.Fill;
		this.rbHorizontal.Location = new System.Drawing.Point(744, 4);
		this.rbHorizontal.Name = "rbHorizontal";
		this.rbHorizontal.Size = new System.Drawing.Size(95, 24);
		this.rbHorizontal.TabIndex = 7;
		this.rbHorizontal.TabStop = true;
		this.rbHorizontal.Text = "Horizontally";
		this.toolTipMain.SetToolTip(this.rbHorizontal, "Select next result horizontally");
		this.rbHorizontal.UseVisualStyleBackColor = true;
		this.btnComplete.AutoSize = true;
		this.btnComplete.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnComplete.Dock = System.Windows.Forms.DockStyle.Fill;
		this.btnComplete.Location = new System.Drawing.Point(1084, 1);
		this.btnComplete.Margin = new System.Windows.Forms.Padding(0);
		this.btnComplete.Name = "btnComplete";
		this.btnComplete.Size = new System.Drawing.Size(75, 30);
		this.btnComplete.TabIndex = 11;
		this.btnComplete.Text = "Complete";
		this.toolTipMain.SetToolTip(this.btnComplete, "Select complete request");
		this.btnComplete.UseVisualStyleBackColor = true;
		this.btnComplete.Click += new System.EventHandler(btnComplete_Click);
		this.btnTool.AutoSize = true;
		this.btnTool.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnTool.Dock = System.Windows.Forms.DockStyle.Fill;
		this.btnTool.Location = new System.Drawing.Point(322, 1);
		this.btnTool.Margin = new System.Windows.Forms.Padding(0);
		this.btnTool.Name = "btnTool";
		this.btnTool.Size = new System.Drawing.Size(127, 30);
		this.btnTool.TabIndex = 4;
		this.btnTool.Text = "Tool management";
		this.toolTipMain.SetToolTip(this.btnTool, "Select view tool management");
		this.btnTool.UseVisualStyleBackColor = true;
		this.btnTool.Click += new System.EventHandler(btnTool_Click);
		this.cbSameTool.AutoSize = true;
		this.cbSameTool.Cursor = System.Windows.Forms.Cursors.Hand;
		this.cbSameTool.Dock = System.Windows.Forms.DockStyle.Fill;
		this.cbSameTool.Location = new System.Drawing.Point(846, 4);
		this.cbSameTool.Name = "cbSameTool";
		this.cbSameTool.Size = new System.Drawing.Size(87, 24);
		this.cbSameTool.TabIndex = 8;
		this.cbSameTool.Text = "Same tool";
		this.toolTipMain.SetToolTip(this.cbSameTool, "Select new result is same tool");
		this.cbSameTool.UseVisualStyleBackColor = true;
		this.cbSameTool.CheckedChanged += new System.EventHandler(cbSameTool_CheckedChanged);
		this.btnRegister.AutoSize = true;
		this.btnRegister.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnRegister.Dock = System.Windows.Forms.DockStyle.Fill;
		this.btnRegister.Location = new System.Drawing.Point(450, 1);
		this.btnRegister.Margin = new System.Windows.Forms.Padding(0);
		this.btnRegister.Name = "btnRegister";
		this.btnRegister.Size = new System.Drawing.Size(140, 30);
		this.btnRegister.TabIndex = 5;
		this.btnRegister.Text = "Registration tool (F5)";
		this.toolTipMain.SetToolTip(this.btnRegister, "Select registration tool");
		this.btnRegister.UseVisualStyleBackColor = true;
		this.btnRegister.Click += new System.EventHandler(btnRegister_Click);
		this.btnEnterOK.AutoSize = true;
		this.btnEnterOK.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnEnterOK.Dock = System.Windows.Forms.DockStyle.Fill;
		this.btnEnterOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnEnterOK.ForeColor = System.Drawing.Color.Blue;
		this.btnEnterOK.Location = new System.Drawing.Point(937, 1);
		this.btnEnterOK.Margin = new System.Windows.Forms.Padding(0);
		this.btnEnterOK.Name = "btnEnterOK";
		this.btnEnterOK.Size = new System.Drawing.Size(61, 30);
		this.btnEnterOK.TabIndex = 9;
		this.btnEnterOK.Text = "OK (F3)";
		this.toolTipMain.SetToolTip(this.btnEnterOK, "Select enter OK");
		this.btnEnterOK.UseVisualStyleBackColor = true;
		this.btnEnterOK.Click += new System.EventHandler(btnEnterOK_Click);
		this.btnEnterNG.AutoSize = true;
		this.btnEnterNG.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnEnterNG.Dock = System.Windows.Forms.DockStyle.Fill;
		this.btnEnterNG.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnEnterNG.ForeColor = System.Drawing.Color.Red;
		this.btnEnterNG.Location = new System.Drawing.Point(999, 1);
		this.btnEnterNG.Margin = new System.Windows.Forms.Padding(0);
		this.btnEnterNG.Name = "btnEnterNG";
		this.btnEnterNG.Size = new System.Drawing.Size(63, 30);
		this.btnEnterNG.TabIndex = 10;
		this.btnEnterNG.Text = "NG (F4)";
		this.toolTipMain.SetToolTip(this.btnEnterNG, "Select enter NG");
		this.btnEnterNG.UseVisualStyleBackColor = true;
		this.btnEnterNG.Click += new System.EventHandler(btnEnterNG_Click);
		this.btnRefresh.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnRefresh.Dock = System.Windows.Forms.DockStyle.Fill;
		this.btnRefresh.Location = new System.Drawing.Point(457, 1);
		this.btnRefresh.Margin = new System.Windows.Forms.Padding(0);
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(447, 30);
		this.btnRefresh.TabIndex = 3;
		this.btnRefresh.Text = "Refresh";
		this.toolTipMain.SetToolTip(this.btnRefresh, "Select refresh data");
		this.btnRefresh.UseVisualStyleBackColor = true;
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.cbShowChart.AutoSize = true;
		this.cbShowChart.Cursor = System.Windows.Forms.Cursors.Hand;
		this.cbShowChart.Dock = System.Windows.Forms.DockStyle.Fill;
		this.cbShowChart.Location = new System.Drawing.Point(4, 4);
		this.cbShowChart.Name = "cbShowChart";
		this.cbShowChart.Size = new System.Drawing.Size(91, 24);
		this.cbShowChart.TabIndex = 1;
		this.cbShowChart.Text = "Show chart";
		this.toolTipMain.SetToolTip(this.cbShowChart, "Select show chart");
		this.cbShowChart.UseVisualStyleBackColor = true;
		this.btnShowHeader.AutoSize = true;
		this.btnShowHeader.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnShowHeader.Dock = System.Windows.Forms.DockStyle.Fill;
		this.btnShowHeader.Image = _5S_QA_Client.Properties.Resources.app;
		this.btnShowHeader.Location = new System.Drawing.Point(1, 1);
		this.btnShowHeader.Margin = new System.Windows.Forms.Padding(0);
		this.btnShowHeader.Name = "btnShowHeader";
		this.btnShowHeader.Size = new System.Drawing.Size(30, 30);
		this.btnShowHeader.TabIndex = 1;
		this.toolTipMain.SetToolTip(this.btnShowHeader, "Select show row header");
		this.btnShowHeader.UseVisualStyleBackColor = true;
		this.btnShowHeader.Click += new System.EventHandler(btnShowHeader_Click);
		this.statusStripfrmMain.BackColor = System.Drawing.SystemColors.ControlLight;
		this.statusStripfrmMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.imgSignal, this.sprogbarStatus, this.slblStatus });
		this.statusStripfrmMain.Location = new System.Drawing.Point(20, 654);
		this.statusStripfrmMain.Name = "statusStripfrmMain";
		this.statusStripfrmMain.Padding = new System.Windows.Forms.Padding(1, 0, 19, 0);
		this.statusStripfrmMain.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
		this.statusStripfrmMain.Size = new System.Drawing.Size(1160, 26);
		this.statusStripfrmMain.SizingGrip = false;
		this.statusStripfrmMain.Stretch = false;
		this.statusStripfrmMain.TabIndex = 9;
		this.imgSignal.Name = "imgSignal";
		this.imgSignal.Size = new System.Drawing.Size(32, 24);
		this.imgSignal.Text = "...";
		this.imgSignal.ButtonClick += new System.EventHandler(imgSignal_ButtonClick);
		this.sprogbarStatus.Name = "sprogbarStatus";
		this.sprogbarStatus.Size = new System.Drawing.Size(200, 20);
		this.slblStatus.Name = "slblStatus";
		this.slblStatus.Size = new System.Drawing.Size(16, 21);
		this.slblStatus.Text = "...";
		this.dgvMain.AllowUserToAddRows = false;
		this.dgvMain.AllowUserToDeleteRows = false;
		this.dgvMain.AllowUserToResizeColumns = false;
		this.dgvMain.AllowUserToResizeRows = false;
		this.dgvMain.BackgroundColor = System.Drawing.SystemColors.Window;
		dataGridViewCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
		dataGridViewCellStyle.BackColor = System.Drawing.SystemColors.AppWorkspace;
		dataGridViewCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		dataGridViewCellStyle.ForeColor = System.Drawing.SystemColors.WindowText;
		dataGridViewCellStyle.SelectionBackColor = System.Drawing.SystemColors.Highlight;
		dataGridViewCellStyle.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
		dataGridViewCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
		this.dgvMain.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle;
		this.dgvMain.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
		this.dgvMain.ContextMenuStrip = this.contextMenuStripMain;
		this.dgvMain.Dock = System.Windows.Forms.DockStyle.Fill;
		this.dgvMain.EnableHeadersVisualStyles = false;
		this.dgvMain.Location = new System.Drawing.Point(20, 134);
		this.dgvMain.MultiSelect = false;
		this.dgvMain.Name = "dgvMain";
		this.dgvMain.RowHeadersVisible = false;
		this.dgvMain.Size = new System.Drawing.Size(1160, 252);
		this.dgvMain.TabIndex = 3;
		this.dgvMain.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(dgvMain_CellEndEdit);
		this.dgvMain.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(dgvMain_EditingControlShowing);
		this.dgvMain.MouseDown += new System.Windows.Forms.MouseEventHandler(dgvMain_MouseDown);
		this.contextMenuStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.main_refreshToolStripMenuItem, this.toolStripSeparator1, this.main_viewToolStripMenuItem });
		this.contextMenuStripMain.Name = "contextMenuStripStaff";
		this.contextMenuStripMain.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
		this.contextMenuStripMain.Size = new System.Drawing.Size(114, 54);
		this.main_refreshToolStripMenuItem.Name = "main_refreshToolStripMenuItem";
		this.main_refreshToolStripMenuItem.Size = new System.Drawing.Size(113, 22);
		this.main_refreshToolStripMenuItem.Text = "Refresh";
		this.main_refreshToolStripMenuItem.Click += new System.EventHandler(main_refreshToolStripMenuItem_Click);
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(110, 6);
		this.main_viewToolStripMenuItem.Name = "main_viewToolStripMenuItem";
		this.main_viewToolStripMenuItem.Size = new System.Drawing.Size(113, 22);
		this.main_viewToolStripMenuItem.Text = "View";
		this.main_viewToolStripMenuItem.Click += new System.EventHandler(main_viewToolStripMenuItem_Click);
		this.tpanelInfor.AutoSize = true;
		this.tpanelInfor.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
		this.tpanelInfor.ColumnCount = 14;
		this.tpanelInfor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelInfor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelInfor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelInfor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelInfor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelInfor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100f));
		this.tpanelInfor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelInfor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelInfor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelInfor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelInfor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelInfor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelInfor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelInfor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelInfor.Controls.Add(this.lblTotal, 13, 0);
		this.tpanelInfor.Controls.Add(this.lblTotalDisplay, 12, 0);
		this.tpanelInfor.Controls.Add(this.lblEmpty, 11, 0);
		this.tpanelInfor.Controls.Add(this.lblEmptyDisplay, 10, 0);
		this.tpanelInfor.Controls.Add(this.lblOK, 9, 0);
		this.tpanelInfor.Controls.Add(this.label5, 8, 0);
		this.tpanelInfor.Controls.Add(this.lblNG, 7, 0);
		this.tpanelInfor.Controls.Add(this.label4, 6, 0);
		this.tpanelInfor.Controls.Add(this.lblRow, 3, 0);
		this.tpanelInfor.Controls.Add(this.lblColumn, 1, 0);
		this.tpanelInfor.Controls.Add(this.lblValueDisplay, 4, 0);
		this.tpanelInfor.Controls.Add(this.lblColumnDisplay, 0, 0);
		this.tpanelInfor.Controls.Add(this.lblRowDisplay, 2, 0);
		this.tpanelInfor.Controls.Add(this.lblValue, 5, 0);
		this.tpanelInfor.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.tpanelInfor.Location = new System.Drawing.Point(20, 636);
		this.tpanelInfor.Name = "tpanelInfor";
		this.tpanelInfor.RowCount = 1;
		this.tpanelInfor.RowStyles.Add(new System.Windows.Forms.RowStyle());
		this.tpanelInfor.Size = new System.Drawing.Size(1160, 18);
		this.tpanelInfor.TabIndex = 169;
		this.lblTotal.AutoSize = true;
		this.lblTotal.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblTotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblTotal.Location = new System.Drawing.Point(1141, 1);
		this.lblTotal.Name = "lblTotal";
		this.lblTotal.Size = new System.Drawing.Size(15, 16);
		this.lblTotal.TabIndex = 17;
		this.lblTotal.Text = "0";
		this.lblTotal.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lblTotalDisplay.AutoSize = true;
		this.lblTotalDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblTotalDisplay.Location = new System.Drawing.Point(1096, 1);
		this.lblTotalDisplay.Name = "lblTotalDisplay";
		this.lblTotalDisplay.Size = new System.Drawing.Size(38, 16);
		this.lblTotalDisplay.TabIndex = 16;
		this.lblTotalDisplay.Text = "Total";
		this.lblTotalDisplay.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblEmpty.AutoSize = true;
		this.lblEmpty.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblEmpty.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblEmpty.Location = new System.Drawing.Point(1074, 1);
		this.lblEmpty.Name = "lblEmpty";
		this.lblEmpty.Size = new System.Drawing.Size(15, 16);
		this.lblEmpty.TabIndex = 15;
		this.lblEmpty.Text = "0";
		this.lblEmpty.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lblEmptyDisplay.AutoSize = true;
		this.lblEmptyDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblEmptyDisplay.Location = new System.Drawing.Point(1022, 1);
		this.lblEmptyDisplay.Name = "lblEmptyDisplay";
		this.lblEmptyDisplay.Size = new System.Drawing.Size(45, 16);
		this.lblEmptyDisplay.TabIndex = 14;
		this.lblEmptyDisplay.Text = "Empty";
		this.lblEmptyDisplay.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblOK.AutoSize = true;
		this.lblOK.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblOK.ForeColor = System.Drawing.Color.Blue;
		this.lblOK.Location = new System.Drawing.Point(1000, 1);
		this.lblOK.Name = "lblOK";
		this.lblOK.Size = new System.Drawing.Size(15, 16);
		this.lblOK.TabIndex = 13;
		this.lblOK.Text = "0";
		this.lblOK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label5.AutoSize = true;
		this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
		this.label5.Location = new System.Drawing.Point(968, 1);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(25, 16);
		this.label5.TabIndex = 12;
		this.label5.Text = "OK";
		this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblNG.AutoSize = true;
		this.lblNG.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblNG.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblNG.ForeColor = System.Drawing.Color.Red;
		this.lblNG.Location = new System.Drawing.Point(946, 1);
		this.lblNG.Name = "lblNG";
		this.lblNG.Size = new System.Drawing.Size(15, 16);
		this.lblNG.TabIndex = 11;
		this.lblNG.Text = "0";
		this.lblNG.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label4.AutoSize = true;
		this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.label4.Location = new System.Drawing.Point(912, 1);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(27, 16);
		this.label4.TabIndex = 10;
		this.label4.Text = "NG";
		this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblRow.AutoSize = true;
		this.lblRow.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblRow.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblRow.Location = new System.Drawing.Point(130, 1);
		this.lblRow.Name = "lblRow";
		this.lblRow.Size = new System.Drawing.Size(19, 16);
		this.lblRow.TabIndex = 9;
		this.lblRow.Text = "...";
		this.lblRow.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lblColumn.AutoSize = true;
		this.lblColumn.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblColumn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblColumn.Location = new System.Drawing.Point(63, 1);
		this.lblColumn.Name = "lblColumn";
		this.lblColumn.Size = new System.Drawing.Size(19, 16);
		this.lblColumn.TabIndex = 8;
		this.lblColumn.Text = "...";
		this.lblColumn.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lblValueDisplay.AutoSize = true;
		this.lblValueDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblValueDisplay.Location = new System.Drawing.Point(156, 1);
		this.lblValueDisplay.Name = "lblValueDisplay";
		this.lblValueDisplay.Size = new System.Drawing.Size(42, 16);
		this.lblValueDisplay.TabIndex = 4;
		this.lblValueDisplay.Text = "Value";
		this.lblValueDisplay.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblColumnDisplay.AutoSize = true;
		this.lblColumnDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblColumnDisplay.Location = new System.Drawing.Point(4, 1);
		this.lblColumnDisplay.Name = "lblColumnDisplay";
		this.lblColumnDisplay.Size = new System.Drawing.Size(52, 16);
		this.lblColumnDisplay.TabIndex = 1;
		this.lblColumnDisplay.Text = "Column";
		this.lblColumnDisplay.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblRowDisplay.AutoSize = true;
		this.lblRowDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblRowDisplay.Location = new System.Drawing.Point(89, 1);
		this.lblRowDisplay.Name = "lblRowDisplay";
		this.lblRowDisplay.Size = new System.Drawing.Size(34, 16);
		this.lblRowDisplay.TabIndex = 0;
		this.lblRowDisplay.Text = "Row";
		this.lblRowDisplay.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblValue.AutoSize = true;
		this.lblValue.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblValue.Location = new System.Drawing.Point(205, 1);
		this.lblValue.Name = "lblValue";
		this.lblValue.Size = new System.Drawing.Size(700, 16);
		this.lblValue.TabIndex = 5;
		this.lblValue.Text = "...";
		this.lblValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.tpanelTitle.AutoSize = true;
		this.tpanelTitle.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
		this.tpanelTitle.ColumnCount = 18;
		this.tpanelTitle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelTitle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelTitle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelTitle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelTitle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelTitle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelTitle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelTitle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelTitle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelTitle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelTitle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelTitle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100f));
		this.tpanelTitle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelTitle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelTitle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelTitle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelTitle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelTitle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelTitle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		this.tpanelTitle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		this.tpanelTitle.Controls.Add(this.btnShowHeader, 0, 0);
		this.tpanelTitle.Controls.Add(this.lblCavity, 4, 0);
		this.tpanelTitle.Controls.Add(this.lblCavityDisplay, 3, 0);
		this.tpanelTitle.Controls.Add(this.lblSample, 2, 0);
		this.tpanelTitle.Controls.Add(this.lblSampleDisplay, 1, 0);
		this.tpanelTitle.Controls.Add(this.btnRefresh, 11, 0);
		this.tpanelTitle.Controls.Add(this.lblResult, 17, 0);
		this.tpanelTitle.Controls.Add(this.lblResultDisplay, 16, 0);
		this.tpanelTitle.Controls.Add(this.lblToolType, 15, 0);
		this.tpanelTitle.Controls.Add(this.lblToolTypeDisplay, 14, 0);
		this.tpanelTitle.Controls.Add(this.lblToolName, 13, 0);
		this.tpanelTitle.Controls.Add(this.lblToolNameDisplay, 12, 0);
		this.tpanelTitle.Controls.Add(this.lblTool, 8, 0);
		this.tpanelTitle.Controls.Add(this.lblDimNo, 6, 0);
		this.tpanelTitle.Controls.Add(this.lblJudgeDisplay, 9, 0);
		this.tpanelTitle.Controls.Add(this.lblDimNoDisplay, 5, 0);
		this.tpanelTitle.Controls.Add(this.lblToolDisplay, 7, 0);
		this.tpanelTitle.Controls.Add(this.lblJudge, 10, 0);
		this.tpanelTitle.Dock = System.Windows.Forms.DockStyle.Top;
		this.tpanelTitle.Location = new System.Drawing.Point(20, 102);
		this.tpanelTitle.Name = "tpanelTitle";
		this.tpanelTitle.RowCount = 1;
		this.tpanelTitle.RowStyles.Add(new System.Windows.Forms.RowStyle());
		this.tpanelTitle.Size = new System.Drawing.Size(1160, 32);
		this.tpanelTitle.TabIndex = 2;
		this.tpanelTitle.TabStop = true;
		this.lblCavity.AutoSize = true;
		this.lblCavity.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblCavity.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblCavity.Location = new System.Drawing.Point(173, 1);
		this.lblCavity.Name = "lblCavity";
		this.lblCavity.Size = new System.Drawing.Size(19, 30);
		this.lblCavity.TabIndex = 19;
		this.lblCavity.Text = "...";
		this.lblCavity.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lblCavityDisplay.AutoSize = true;
		this.lblCavityDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblCavityDisplay.Location = new System.Drawing.Point(122, 1);
		this.lblCavityDisplay.Name = "lblCavityDisplay";
		this.lblCavityDisplay.Size = new System.Drawing.Size(44, 30);
		this.lblCavityDisplay.TabIndex = 18;
		this.lblCavityDisplay.Text = "Cavity";
		this.lblCavityDisplay.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblSample.AutoSize = true;
		this.lblSample.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblSample.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblSample.Location = new System.Drawing.Point(96, 1);
		this.lblSample.Name = "lblSample";
		this.lblSample.Size = new System.Drawing.Size(19, 30);
		this.lblSample.TabIndex = 17;
		this.lblSample.Text = "...";
		this.lblSample.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lblSampleDisplay.AutoSize = true;
		this.lblSampleDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblSampleDisplay.Location = new System.Drawing.Point(35, 1);
		this.lblSampleDisplay.Name = "lblSampleDisplay";
		this.lblSampleDisplay.Size = new System.Drawing.Size(54, 30);
		this.lblSampleDisplay.TabIndex = 16;
		this.lblSampleDisplay.Text = "Sample";
		this.lblSampleDisplay.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblResult.AutoSize = true;
		this.lblResult.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblResult.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblResult.Location = new System.Drawing.Point(1137, 1);
		this.lblResult.Name = "lblResult";
		this.lblResult.Size = new System.Drawing.Size(19, 30);
		this.lblResult.TabIndex = 15;
		this.lblResult.Text = "...";
		this.lblResult.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lblResultDisplay.AutoSize = true;
		this.lblResultDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblResultDisplay.Location = new System.Drawing.Point(1085, 1);
		this.lblResultDisplay.Name = "lblResultDisplay";
		this.lblResultDisplay.Size = new System.Drawing.Size(45, 30);
		this.lblResultDisplay.TabIndex = 14;
		this.lblResultDisplay.Text = "Result";
		this.lblResultDisplay.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblToolType.AutoSize = true;
		this.lblToolType.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblToolType.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblToolType.Location = new System.Drawing.Point(1059, 1);
		this.lblToolType.Name = "lblToolType";
		this.lblToolType.Size = new System.Drawing.Size(19, 30);
		this.lblToolType.TabIndex = 13;
		this.lblToolType.Text = "...";
		this.lblToolType.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lblToolTypeDisplay.AutoSize = true;
		this.lblToolTypeDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblToolTypeDisplay.Location = new System.Drawing.Point(1013, 1);
		this.lblToolTypeDisplay.Name = "lblToolTypeDisplay";
		this.lblToolTypeDisplay.Size = new System.Drawing.Size(39, 30);
		this.lblToolTypeDisplay.TabIndex = 12;
		this.lblToolTypeDisplay.Text = "Type";
		this.lblToolTypeDisplay.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblToolName.AutoSize = true;
		this.lblToolName.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblToolName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblToolName.Location = new System.Drawing.Point(987, 1);
		this.lblToolName.Name = "lblToolName";
		this.lblToolName.Size = new System.Drawing.Size(19, 30);
		this.lblToolName.TabIndex = 11;
		this.lblToolName.Text = "...";
		this.lblToolName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lblToolNameDisplay.AutoSize = true;
		this.lblToolNameDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblToolNameDisplay.Location = new System.Drawing.Point(908, 1);
		this.lblToolNameDisplay.Name = "lblToolNameDisplay";
		this.lblToolNameDisplay.Size = new System.Drawing.Size(72, 30);
		this.lblToolNameDisplay.TabIndex = 10;
		this.lblToolNameDisplay.Text = "Tool name";
		this.lblToolNameDisplay.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblTool.AutoSize = true;
		this.lblTool.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblTool.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblTool.Location = new System.Drawing.Point(326, 1);
		this.lblTool.Name = "lblTool";
		this.lblTool.Size = new System.Drawing.Size(19, 30);
		this.lblTool.TabIndex = 9;
		this.lblTool.Text = "...";
		this.lblTool.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lblDimNo.AutoSize = true;
		this.lblDimNo.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblDimNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblDimNo.Location = new System.Drawing.Point(258, 1);
		this.lblDimNo.Name = "lblDimNo";
		this.lblDimNo.Size = new System.Drawing.Size(19, 30);
		this.lblDimNo.TabIndex = 8;
		this.lblDimNo.Text = "...";
		this.lblDimNo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lblJudgeDisplay.AutoSize = true;
		this.lblJudgeDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblJudgeDisplay.Location = new System.Drawing.Point(352, 1);
		this.lblJudgeDisplay.Name = "lblJudgeDisplay";
		this.lblJudgeDisplay.Size = new System.Drawing.Size(75, 30);
		this.lblJudgeDisplay.TabIndex = 4;
		this.lblJudgeDisplay.Text = "Total judge";
		this.lblJudgeDisplay.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblDimNoDisplay.AutoSize = true;
		this.lblDimNoDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblDimNoDisplay.Location = new System.Drawing.Point(199, 1);
		this.lblDimNoDisplay.Name = "lblDimNoDisplay";
		this.lblDimNoDisplay.Size = new System.Drawing.Size(52, 30);
		this.lblDimNoDisplay.TabIndex = 1;
		this.lblDimNoDisplay.Text = "Dim. no";
		this.lblDimNoDisplay.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblToolDisplay.AutoSize = true;
		this.lblToolDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblToolDisplay.Location = new System.Drawing.Point(284, 1);
		this.lblToolDisplay.Name = "lblToolDisplay";
		this.lblToolDisplay.Size = new System.Drawing.Size(35, 30);
		this.lblToolDisplay.TabIndex = 0;
		this.lblToolDisplay.Text = "Tool";
		this.lblToolDisplay.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblJudge.AutoSize = true;
		this.lblJudge.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblJudge.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblJudge.Location = new System.Drawing.Point(434, 1);
		this.lblJudge.Name = "lblJudge";
		this.lblJudge.Size = new System.Drawing.Size(19, 30);
		this.lblJudge.TabIndex = 5;
		this.lblJudge.Text = "...";
		this.lblJudge.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.tpanelButton.AutoSize = true;
		this.tpanelButton.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
		this.tpanelButton.ColumnCount = 12;
		this.tpanelButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100f));
		this.tpanelButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelButton.Controls.Add(this.cbShowChart, 0, 0);
		this.tpanelButton.Controls.Add(this.btnEnterNG, 9, 0);
		this.tpanelButton.Controls.Add(this.btnEnterOK, 8, 0);
		this.tpanelButton.Controls.Add(this.btnRegister, 4, 0);
		this.tpanelButton.Controls.Add(this.cbSameTool, 7, 0);
		this.tpanelButton.Controls.Add(this.btnTool, 3, 0);
		this.tpanelButton.Controls.Add(this.btnComplete, 11, 0);
		this.tpanelButton.Controls.Add(this.rbHorizontal, 6, 0);
		this.tpanelButton.Controls.Add(this.btnImage, 2, 0);
		this.tpanelButton.Controls.Add(this.btnHistory, 1, 0);
		this.tpanelButton.Controls.Add(this.rbVertical, 5, 0);
		this.tpanelButton.Dock = System.Windows.Forms.DockStyle.Top;
		this.tpanelButton.Location = new System.Drawing.Point(20, 70);
		this.tpanelButton.Name = "tpanelButton";
		this.tpanelButton.RowCount = 1;
		this.tpanelButton.RowStyles.Add(new System.Windows.Forms.RowStyle());
		this.tpanelButton.Size = new System.Drawing.Size(1160, 32);
		this.tpanelButton.TabIndex = 1;
		this.tpanelButton.TabStop = true;
		this.panelLogout.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.panelLogout.BackColor = System.Drawing.Color.Transparent;
		this.panelLogout.Controls.Add(this.lblFullname);
		this.panelLogout.Controls.Add(this.ptbAvatar);
		this.panelLogout.Location = new System.Drawing.Point(829, 25);
		this.panelLogout.Margin = new System.Windows.Forms.Padding(4);
		this.panelLogout.Name = "panelLogout";
		this.panelLogout.Size = new System.Drawing.Size(350, 42);
		this.panelLogout.TabIndex = 173;
		this.panelLogout.TabStop = true;
		this.lblFullname.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.lblFullname.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f);
		this.lblFullname.ImeMode = System.Windows.Forms.ImeMode.NoControl;
		this.lblFullname.Location = new System.Drawing.Point(0, 26);
		this.lblFullname.Name = "lblFullname";
		this.lblFullname.Padding = new System.Windows.Forms.Padding(0, 0, 5, 0);
		this.lblFullname.Size = new System.Drawing.Size(308, 16);
		this.lblFullname.TabIndex = 34;
		this.lblFullname.Text = "...";
		this.lblFullname.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.ptbAvatar.Dock = System.Windows.Forms.DockStyle.Right;
		this.ptbAvatar.ErrorImage = _5S_QA_Client.Properties.Resources._5S_QA_C;
		this.ptbAvatar.Image = _5S_QA_Client.Properties.Resources._5S_QA_C;
		this.ptbAvatar.ImeMode = System.Windows.Forms.ImeMode.NoControl;
		this.ptbAvatar.InitialImage = _5S_QA_Client.Properties.Resources._5S_QA_C;
		this.ptbAvatar.Location = new System.Drawing.Point(308, 0);
		this.ptbAvatar.Margin = new System.Windows.Forms.Padding(4);
		this.ptbAvatar.Name = "ptbAvatar";
		this.ptbAvatar.Size = new System.Drawing.Size(42, 42);
		this.ptbAvatar.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
		this.ptbAvatar.TabIndex = 14;
		this.ptbAvatar.TabStop = false;
		this.dgvHeader.AllowUserToAddRows = false;
		this.dgvHeader.AllowUserToDeleteRows = false;
		this.dgvHeader.AllowUserToResizeColumns = false;
		this.dgvHeader.AllowUserToResizeRows = false;
		this.dgvHeader.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
		this.dgvHeader.ColumnHeadersVisible = false;
		this.dgvHeader.Columns.AddRange(this.title, this.value, this.index);
		this.dgvHeader.Location = new System.Drawing.Point(22, 136);
		this.dgvHeader.Name = "dgvHeader";
		this.dgvHeader.RowHeadersVisible = false;
		this.dgvHeader.ScrollBars = System.Windows.Forms.ScrollBars.None;
		this.dgvHeader.Size = new System.Drawing.Size(200, 100);
		this.dgvHeader.TabIndex = 178;
		this.dgvHeader.Visible = false;
		this.dgvHeader.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(dgvHeader_CellContentClick);
		this.title.FalseValue = "false";
		this.title.HeaderText = "Title";
		this.title.Name = "title";
		this.title.Resizable = System.Windows.Forms.DataGridViewTriState.False;
		this.title.TrueValue = "true";
		this.title.Width = 23;
		this.value.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
		this.value.DefaultCellStyle = dataGridViewCellStyle2;
		this.value.HeaderText = "Value";
		this.value.Name = "value";
		this.value.ReadOnly = true;
		this.value.Resizable = System.Windows.Forms.DataGridViewTriState.False;
		this.value.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.index.HeaderText = "Index";
		this.index.Name = "index";
		this.index.ReadOnly = true;
		this.index.Visible = false;
		this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
		this.dataGridViewTextBoxColumn1.DefaultCellStyle = dataGridViewCellStyle3;
		this.dataGridViewTextBoxColumn1.HeaderText = "Value";
		this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
		this.dataGridViewTextBoxColumn1.ReadOnly = true;
		this.dataGridViewTextBoxColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
		this.dataGridViewTextBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.dataGridViewTextBoxColumn2.HeaderText = "Index";
		this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
		this.dataGridViewTextBoxColumn2.ReadOnly = true;
		this.dataGridViewTextBoxColumn2.Visible = false;
		this.mPanelViewMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.mPanelViewMain.Dock = System.Windows.Forms.DockStyle.Right;
		this.mPanelViewMain.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.mPanelViewMain.Location = new System.Drawing.Point(388, 120);
		this.mPanelViewMain.Margin = new System.Windows.Forms.Padding(4);
		this.mPanelViewMain.Name = "mPanelViewMain";
		this.mPanelViewMain.Size = new System.Drawing.Size(400, 266);
		this.mPanelViewMain.TabIndex = 168;
		this.mPanelChartMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.mPanelChartMain.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.mPanelChartMain.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.mPanelChartMain.Location = new System.Drawing.Point(20, 386);
		this.mPanelChartMain.mModels = null;
		this.mPanelChartMain.Name = "mPanelChartMain";
		this.mPanelChartMain.Size = new System.Drawing.Size(1160, 250);
		this.mPanelChartMain.TabIndex = 175;
		this.mPanelImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.mPanelImage.Dock = System.Windows.Forms.DockStyle.Right;
		this.mPanelImage.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.mPanelImage.Location = new System.Drawing.Point(0, 0);
		this.mPanelImage.Margin = new System.Windows.Forms.Padding(4);
		this.mPanelImage.mDt = null;
		this.mPanelImage.Name = "mPanelImage";
		this.mPanelImage.Size = new System.Drawing.Size(400, 452);
		this.mPanelImage.TabIndex = 172;
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1200, 700);
		base.Controls.Add(this.dgvHeader);
		base.Controls.Add(this.panelLogout);
		base.Controls.Add(this.mPanelViewMain);
		base.Controls.Add(this.dgvMain);
		base.Controls.Add(this.tpanelTitle);
		base.Controls.Add(this.mPanelChartMain);
		base.Controls.Add(this.tpanelInfor);
		base.Controls.Add(this.mPanelImage);
		base.Controls.Add(this.statusStripfrmMain);
		base.Controls.Add(this.tpanelButton);
		this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(4);
		base.Name = "frmResultView";
		base.Padding = new System.Windows.Forms.Padding(20, 70, 20, 20);
		this.Text = "5S QA Client * RESULT";
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmResultView_FormClosing);
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(frmResultView_FormClosed);
		base.Load += new System.EventHandler(frmResultView_Load);
		base.Shown += new System.EventHandler(frmResultView_Shown);
		this.statusStripfrmMain.ResumeLayout(false);
		this.statusStripfrmMain.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.dgvMain).EndInit();
		this.contextMenuStripMain.ResumeLayout(false);
		this.tpanelInfor.ResumeLayout(false);
		this.tpanelInfor.PerformLayout();
		this.tpanelTitle.ResumeLayout(false);
		this.tpanelTitle.PerformLayout();
		this.tpanelButton.ResumeLayout(false);
		this.tpanelButton.PerformLayout();
		this.panelLogout.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.ptbAvatar).EndInit();
		((System.ComponentModel.ISupportInitialize)this.dgvHeader).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
