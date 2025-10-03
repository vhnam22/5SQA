using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using _5S_QA_Client.Controls;
using _5S_QA_Client.Properties;
using _5S_QA_DL.APIContext;
using _5S_QA_Entities.Constants;
using _5S_QA_Entities.Dtos;
using _5S_QA_Entities.Models;
using MetroFramework.Forms;

namespace _5S_QA_Client;

public class frmCompleteView : MetroForm
{
	private Form mForm;

	private RequestViewModel mRequest;

	public bool isClose;

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

	private List<string> mDecimals = new List<string>();

	private List<ResultViewModel> mResults;

	private List<LimitViewModel> mAQLs;

	private List<MeasurementQuickViewModel> mMeass;

	private List<MeasurementQuickViewModel> mMeasRanks;

	private List<ResultRankViewModel> mResultRanks;

	private int mCountOut = 0;

	private List<string> mRowHeaders;

	private IContainer components = null;

	private ToolTip toolTipMain;

	private StatusStrip statusStripfrmMain;

	private ToolStripProgressBar sprogbarStatus;

	public ToolStripStatusLabel slblStatus;

	private DataGridView dgvMain;

	private ContextMenuStrip contextMenuStripMain;

	private ToolStripMenuItem main_refreshToolStripMenuItem;

	private TableLayoutPanel tpanelInfor;

	private Label lblNG;

	private Label label4;

	private Label lblOK;

	private Label label5;

	private Label lblEmpty;

	private Label lblEmptyDisplay;

	private Label lblTotal;

	private Label lblTotalDisplay;

	private Panel panelLogout;

	private Label lblFullname;

	private PictureBox ptbAvatar;

	private Button btnComplete;

	private Button btnHistory;

	private Label lblWarning;

	private Label lblWarningDisplay;

	private Label lblEdit;

	private Label lblEditDisplay;

	private ToolStripSeparator toolStripSeparator1;

	private ToolStripMenuItem main_viewToolStripMenuItem;

	private mPanelView mPanelViewMain;

	private Button btnShowHeader;

	private DataGridView dgvHeader;

	private DataGridViewCheckBoxColumn title;

	private DataGridViewTextBoxColumn value;

	private DataGridViewTextBoxColumn index;

	public frmCompleteView(Form frm, RequestViewModel request)
	{
		InitializeComponent();
		Common.setControls(this, toolTipMain, new List<ContextMenuStrip> { contextMenuStripMain });
		mForm = frm;
		mRequest = request;
		isClose = true;
	}

	private void frmCompleteView_Load(object sender, EventArgs e)
	{
		Init();
	}

	private void frmCompleteView_Shown(object sender, EventArgs e)
	{
		Set_dgvMain();
	}

	private void frmCompleteView_FormClosing(object sender, FormClosingEventArgs e)
	{
		GC.Collect();
	}

	private void frmCompleteView_FormClosed(object sender, FormClosedEventArgs e)
	{
		if (!isClose && mForm.GetType() == typeof(frmResultView))
		{
			((frmResultView)mForm).isClose = false;
		}
	}

	private void Init()
	{
		mPanelViewMain.Visible = false;
		Load_Settings();
		Text = Text + " (" + mRequest.Code + " * " + mRequest.Name + ")";
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
		load_AllData();
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
				dgvMain.Rows[dgvMain.RowCount - 1].ReadOnly = true;
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
			}
		}
		dgvMain.Columns[0].ReadOnly = true;
		dgvMain.Rows[0].Visible = false;
		dgvMain.Rows[6].Frozen = true;
		ROW_SAMPLE_LAST = dgvMain.RowCount - 9;
		mTotalResult = ((dgvMain.ColumnCount - 1) * mRequest.Sample * mRequest.ProductCavity).Value;
		lblTotal.Text = mTotalResult.ToString();
		setAQLForMeasurement();
		drawMerge(dt);
		initPanelHeaders();
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

	private void load_AllData()
	{
		start_Proccessor();
		DataTable measurement = Get_Measurement();
		Init_dgvMain(measurement);
	}

	private void Load_Settings()
	{
		loadDecimals();
		mRowHeaders = Settings.Default.CompleteHeaders.Split(new string[1] { ";" }, StringSplitOptions.RemoveEmptyEntries).ToList();
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

	private DataTable Get_Measurement()
	{
		DataTable result = new DataTable();
		try
		{
			QueryArgs queryArgs = new QueryArgs();
			queryArgs.Predicate = "ProductId=@0";
			queryArgs.PredicateParameters = new string[1] { mRequest.ProductId.ToString() };
			queryArgs.Order = "Sort, Created";
			queryArgs.Page = 1;
			queryArgs.Limit = int.MaxValue;
			QueryArgs body = queryArgs;
			ResponseDto result2 = frmLogin.client.GetsAsync(body, "/api/Measurement/Gets").Result;
			result = Common.getDataTable<MeasurementQuickViewModel>(result2);
			mMeass = Common.getObjects<MeasurementQuickViewModel>(result2);
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
			QueryArgs queryArgs = new QueryArgs();
			queryArgs.Predicate = "RequestId=@0";
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
		string[] array = Get_SampleCavity(dgvMain.CurrentRow);
		if (dgvMain.CurrentCell == null || dgvMain.CurrentCellAddress.X == 0 || !int.TryParse(array[0], out var sample) || !int.TryParse(array[1], out var cavity) || !Guid.TryParse(dgvMain.Rows[0].Cells[dgvMain.CurrentCellAddress.X].Value.ToString(), out var id))
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
		new Task(delegate
		{
			Invoke((MethodInvoker)delegate
			{
				start_Proccessor();
				Cursor.Current = Cursors.WaitCursor;
				int num = 0;
				int num2 = 0;
				mResults = Get_Results();
				foreach (ResultViewModel mResult in mResults)
				{
					CellDto cellDto = Get_Cell(mResult);
					if (cellDto != null)
					{
						if (!string.IsNullOrEmpty(mResult.History) && !mResult.History.Equals("[]"))
						{
							num++;
							dgvMain.Rows[cellDto.Row].Cells[cellDto.Column].Style.BackColor = Color.Gainsboro;
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
							if (mResult.Judge == "OK+" || mResult.Judge == "OK-")
							{
								num2++;
								dgvMain.Rows[cellDto.Row].Cells[cellDto.Column].Style.ForeColor = Color.DarkOrange;
							}
						}
					}
				}
				List<object> list = Cal_dgvMain(mResults);
				double num3 = default(double);
				foreach (dynamic item in list)
				{
					dynamic val = Get_Cell(item);
					if (!((val == null) ? true : false))
					{
						dynamic val2 = item.Result;
						if (double.TryParse(val2, out num3))
						{
							val2 = ((!((item.MeasurementUnit == "°") ? true : false)) ? Common.ToString(num3, item.MachineTypeName, mDecimals) : Common.ConvertDoubleToDegrees(num3, uniformity: true));
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
					}
				}
				lblEdit.Text = num.ToString();
				lblWarning.Text = num2.ToString();
				Cal_Judge();
				debugOutput(Common.getTextLanguage(this, "Successful"));
			});
		}).Start();
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
				mCountOut = 0;
				mAQLs = loadSampleWithAQL();
				mMeasRanks = getMeasurementIsRank();
				mResultRanks = getResultRank();
				if (mAQLs.Count >= 1)
				{
					int col;
					for (col = 1; col < dgvMain.ColumnCount; col++)
					{
						MeasurementQuickViewModel measurementQuickViewModel = mMeasRanks.FirstOrDefault((MeasurementQuickViewModel x) => x.Id.ToString() == dgvMain.Rows[0].Cells[col].Value.ToString());
						if (measurementQuickViewModel != null)
						{
							dgvMain.Columns[col].HeaderText += "*";
						}
						LimitViewModel aql = mAQLs.FirstOrDefault((LimitViewModel x) => x.MeasurementId.ToString() == dgvMain.Rows[0].Cells[col].Value.ToString());
						if (aql != null)
						{
							ResultRankViewModel resultRankViewModel = mResultRanks.FirstOrDefault((ResultRankViewModel x) => x.MeasurementId == aql.MeasurementId);
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
									mCountOut++;
								}
							}
						}
					}
				}
			});
		}).Start();
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

	private RankDto getRank(MeasurementQuickViewModel item)
	{
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Expected O, but got Unknown
		RankDto result = null;
		for (int i = 1; i < dgvMain.ColumnCount; i++)
		{
			if (dgvMain.Rows[0].Cells[i].Value.ToString() == item.Id.ToString())
			{
				result = new RankDto
				{
					MeasurementId = item.Id,
					MeasurementName = item.Name,
					RequestId = mRequest.Id,
					Rank = dgvMain.Rows[dgvMain.RowCount - 1].Cells[i].Value?.ToString()
				};
				break;
			}
		}
		return result;
	}

	private List<MetadataValueViewModel> getMetaRank()
	{
		List<MetadataValueViewModel> result = new List<MetadataValueViewModel>();
		try
		{
			QueryArgs queryArgs = new QueryArgs();
			queryArgs.Predicate = "TypeId=@0";
			queryArgs.PredicateParameters = new string[1] { "EEA65E86-D458-4919-82F7-3DCA0475695D" };
			queryArgs.Order = "Created";
			queryArgs.Page = 1;
			queryArgs.Limit = int.MaxValue;
			QueryArgs body = queryArgs;
			ResponseDto result2 = frmLogin.client.GetsAsync(body, "/api/MetadataValue/Gets").Result;
			result = Common.getObjects<MetadataValueViewModel>(result2);
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
		Settings.Default.CompleteHeaders = string.Join(";", mRowHeaders);
		Settings.Default.Save();
	}

	private void dgvMain_CurrentCellChanged(object sender, EventArgs e)
	{
		mPanelViewMain.Visible = false;
	}

	private void dgvMain_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
	{
		main_viewToolStripMenuItem.PerformClick();
	}

	private void main_refreshToolStripMenuItem_Click(object sender, EventArgs e)
	{
		Set_dgvMain();
	}

	private void main_viewToolStripMenuItem_Click(object sender, EventArgs e)
	{
		ResultViewModel result = Get_Result();
		if (result == null)
		{
			mPanelViewMain.Visible = false;
			return;
		}
		mPanelViewMain.load_dgvContent(result);
		mPanelViewMain.Visible = true;
	}

	private void btnComplete_Click(object sender, EventArgs e)
	{
		//IL_029e: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_030b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0323: Expected O, but got Unknown
		List<RankDto> list = new List<RankDto>();
		string text = "";
		List<MetadataValueViewModel> metaRank = getMetaRank();
		string judgement = null;
		foreach (MeasurementQuickViewModel model in mMeasRanks)
		{
			RankDto rank = getRank(model);
			if (rank == null)
			{
				continue;
			}
			if (rank.Rank == "RANK5")
			{
				judgement = "RANK5";
				list.Clear();
				break;
			}
			ResultRankViewModel resultRankViewModel = mResultRanks.FirstOrDefault((ResultRankViewModel x) => x.MeasurementId == model.Id);
			if (resultRankViewModel != null)
			{
				continue;
			}
			MetadataValueViewModel metadataValueViewModel = metaRank.FirstOrDefault((MetadataValueViewModel x) => x.Name.ToUpper().EndsWith(rank.Rank));
			if (metadataValueViewModel == null || !int.TryParse(metadataValueViewModel.Value, out var result))
			{
				continue;
			}
			rank.Percentage = result;
			LimitViewModel limitViewModel = mAQLs.FirstOrDefault((LimitViewModel x) => x.MeasurementId == model.Id);
			if (limitViewModel == null)
			{
				continue;
			}
			rank.Sample = limitViewModel.Sample * result / 100;
			rank.Total = limitViewModel.Sample + rank.Sample;
			if (list.Any((RankDto x) => x.MeasurementId == model.Id))
			{
				continue;
			}
			string first = Regex.Match(model.Name, "^.*(?=\\d)").Value;
			if (first.ToUpper().Contains("LENGTH") || first.ToUpper().Contains("WIDTH") || first.ToUpper().Contains("THICKNESS"))
			{
				IEnumerable<MeasurementQuickViewModel> enumerable = mMeass.Where((MeasurementQuickViewModel x) => x.Name.StartsWith(first));
				foreach (MeasurementQuickViewModel item in enumerable)
				{
					list.Add(new RankDto
					{
						MeasurementId = item.Id,
						MeasurementName = item.Name,
						RequestId = rank.RequestId,
						Percentage = rank.Percentage,
						Rank = rank.Rank,
						Sample = rank.Sample,
						Total = rank.Total
					});
				}
				text = text + "\r\n   " + first + ": " + rank.Rank;
			}
			else
			{
				list.Add(rank);
				text = text + "\r\n   " + rank.MeasurementName + ": " + rank.Rank;
			}
		}
		if (list.Count > 0)
		{
			if (MessageBox.Show(Common.getTextLanguage(this, "wSureRank") + text, Common.getTextLanguage(this, "INFORMATION"), MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk).Equals(DialogResult.OK))
			{
				ResponseDto result2 = frmLogin.client.SaveAsync(list, "/api/ResultRank/SaveList").Result;
				if (!result2.Success)
				{
					throw new Exception(result2.Messages.ElementAt(0).Message);
				}
				isClose = false;
				Close();
			}
		}
		else
		{
			if (!MessageBox.Show(Common.getTextLanguage(this, "wSureComplete") + ": " + mRequest.Name, Common.getTextLanguage(this, "QUESTION"), MessageBoxButtons.YesNo, MessageBoxIcon.Question).Equals(DialogResult.Yes))
			{
				return;
			}
			Cursor.Current = Cursors.WaitCursor;
			try
			{
				ActiveRequestDto body = new ActiveRequestDto
				{
					Id = mRequest.Id,
					Judgement = judgement,
					Status = "Completed"
				};
				ResponseDto result3 = frmLogin.client.SaveAsync(body, "/api/Request/Active").Result;
				if (!result3.Success)
				{
					throw new Exception(result3.Messages.ElementAt(0).Message);
				}
				isClose = false;
				Close();
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
		}
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(_5S_QA_Client.frmCompleteView));
		this.toolTipMain = new System.Windows.Forms.ToolTip(this.components);
		this.btnComplete = new System.Windows.Forms.Button();
		this.btnHistory = new System.Windows.Forms.Button();
		this.btnShowHeader = new System.Windows.Forms.Button();
		this.statusStripfrmMain = new System.Windows.Forms.StatusStrip();
		this.sprogbarStatus = new System.Windows.Forms.ToolStripProgressBar();
		this.slblStatus = new System.Windows.Forms.ToolStripStatusLabel();
		this.dgvMain = new System.Windows.Forms.DataGridView();
		this.contextMenuStripMain = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.main_refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.main_viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.tpanelInfor = new System.Windows.Forms.TableLayoutPanel();
		this.lblWarning = new System.Windows.Forms.Label();
		this.lblWarningDisplay = new System.Windows.Forms.Label();
		this.lblEdit = new System.Windows.Forms.Label();
		this.lblEditDisplay = new System.Windows.Forms.Label();
		this.lblTotal = new System.Windows.Forms.Label();
		this.lblTotalDisplay = new System.Windows.Forms.Label();
		this.lblEmpty = new System.Windows.Forms.Label();
		this.lblEmptyDisplay = new System.Windows.Forms.Label();
		this.lblOK = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.lblNG = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.panelLogout = new System.Windows.Forms.Panel();
		this.lblFullname = new System.Windows.Forms.Label();
		this.ptbAvatar = new System.Windows.Forms.PictureBox();
		this.mPanelViewMain = new _5S_QA_Client.mPanelView();
		this.dgvHeader = new System.Windows.Forms.DataGridView();
		this.title = new System.Windows.Forms.DataGridViewCheckBoxColumn();
		this.value = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.index = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.statusStripfrmMain.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.dgvMain).BeginInit();
		this.contextMenuStripMain.SuspendLayout();
		this.tpanelInfor.SuspendLayout();
		this.panelLogout.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.ptbAvatar).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.dgvHeader).BeginInit();
		base.SuspendLayout();
		this.btnComplete.AutoSize = true;
		this.btnComplete.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnComplete.Dock = System.Windows.Forms.DockStyle.Fill;
		this.btnComplete.Location = new System.Drawing.Point(121, 1);
		this.btnComplete.Margin = new System.Windows.Forms.Padding(0);
		this.btnComplete.Name = "btnComplete";
		this.btnComplete.Size = new System.Drawing.Size(542, 30);
		this.btnComplete.TabIndex = 3;
		this.btnComplete.Text = "Complete";
		this.toolTipMain.SetToolTip(this.btnComplete, "Select complete request");
		this.btnComplete.UseVisualStyleBackColor = true;
		this.btnComplete.Click += new System.EventHandler(btnComplete_Click);
		this.btnHistory.AutoSize = true;
		this.btnHistory.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnHistory.Dock = System.Windows.Forms.DockStyle.Fill;
		this.btnHistory.Location = new System.Drawing.Point(32, 1);
		this.btnHistory.Margin = new System.Windows.Forms.Padding(0);
		this.btnHistory.Name = "btnHistory";
		this.btnHistory.Size = new System.Drawing.Size(88, 30);
		this.btnHistory.TabIndex = 2;
		this.btnHistory.Text = "View history";
		this.toolTipMain.SetToolTip(this.btnHistory, "Select view history of result");
		this.btnHistory.UseVisualStyleBackColor = true;
		this.btnHistory.Click += new System.EventHandler(main_viewToolStripMenuItem_Click);
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
		this.statusStripfrmMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.sprogbarStatus, this.slblStatus });
		this.statusStripfrmMain.Location = new System.Drawing.Point(20, 654);
		this.statusStripfrmMain.Name = "statusStripfrmMain";
		this.statusStripfrmMain.Padding = new System.Windows.Forms.Padding(1, 0, 19, 0);
		this.statusStripfrmMain.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
		this.statusStripfrmMain.Size = new System.Drawing.Size(1060, 26);
		this.statusStripfrmMain.SizingGrip = false;
		this.statusStripfrmMain.Stretch = false;
		this.statusStripfrmMain.TabIndex = 9;
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
		this.dgvMain.Location = new System.Drawing.Point(20, 102);
		this.dgvMain.MultiSelect = false;
		this.dgvMain.Name = "dgvMain";
		this.dgvMain.RowHeadersVisible = false;
		this.dgvMain.Size = new System.Drawing.Size(1060, 552);
		this.dgvMain.TabIndex = 3;
		this.dgvMain.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(dgvMain_CellDoubleClick);
		this.dgvMain.CurrentCellChanged += new System.EventHandler(dgvMain_CurrentCellChanged);
		this.dgvMain.MouseDown += new System.Windows.Forms.MouseEventHandler(dgvMain_MouseDown);
		this.contextMenuStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.main_refreshToolStripMenuItem, this.toolStripSeparator1, this.main_viewToolStripMenuItem });
		this.contextMenuStripMain.Name = "contextMenuStripStaff";
		this.contextMenuStripMain.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
		this.contextMenuStripMain.Size = new System.Drawing.Size(139, 54);
		this.main_refreshToolStripMenuItem.Name = "main_refreshToolStripMenuItem";
		this.main_refreshToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
		this.main_refreshToolStripMenuItem.Text = "Refresh";
		this.main_refreshToolStripMenuItem.Click += new System.EventHandler(main_refreshToolStripMenuItem_Click);
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(135, 6);
		this.main_viewToolStripMenuItem.Name = "main_viewToolStripMenuItem";
		this.main_viewToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
		this.main_viewToolStripMenuItem.Text = "View history";
		this.main_viewToolStripMenuItem.Click += new System.EventHandler(main_viewToolStripMenuItem_Click);
		this.tpanelInfor.AutoSize = true;
		this.tpanelInfor.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
		this.tpanelInfor.ColumnCount = 15;
		this.tpanelInfor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30f));
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
		this.tpanelInfor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelInfor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelInfor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelInfor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelInfor.Controls.Add(this.btnShowHeader, 0, 0);
		this.tpanelInfor.Controls.Add(this.lblWarning, 6, 0);
		this.tpanelInfor.Controls.Add(this.lblWarningDisplay, 5, 0);
		this.tpanelInfor.Controls.Add(this.lblEdit, 4, 0);
		this.tpanelInfor.Controls.Add(this.lblEditDisplay, 3, 0);
		this.tpanelInfor.Controls.Add(this.btnHistory, 1, 0);
		this.tpanelInfor.Controls.Add(this.btnComplete, 2, 0);
		this.tpanelInfor.Controls.Add(this.lblTotal, 14, 0);
		this.tpanelInfor.Controls.Add(this.lblTotalDisplay, 13, 0);
		this.tpanelInfor.Controls.Add(this.lblEmpty, 12, 0);
		this.tpanelInfor.Controls.Add(this.lblEmptyDisplay, 11, 0);
		this.tpanelInfor.Controls.Add(this.lblOK, 10, 0);
		this.tpanelInfor.Controls.Add(this.label5, 9, 0);
		this.tpanelInfor.Controls.Add(this.lblNG, 8, 0);
		this.tpanelInfor.Controls.Add(this.label4, 7, 0);
		this.tpanelInfor.Dock = System.Windows.Forms.DockStyle.Top;
		this.tpanelInfor.Location = new System.Drawing.Point(20, 70);
		this.tpanelInfor.Name = "tpanelInfor";
		this.tpanelInfor.RowCount = 1;
		this.tpanelInfor.RowStyles.Add(new System.Windows.Forms.RowStyle());
		this.tpanelInfor.Size = new System.Drawing.Size(1060, 32);
		this.tpanelInfor.TabIndex = 169;
		this.lblWarning.AutoSize = true;
		this.lblWarning.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblWarning.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblWarning.ForeColor = System.Drawing.Color.DarkOrange;
		this.lblWarning.Location = new System.Drawing.Point(790, 1);
		this.lblWarning.Name = "lblWarning";
		this.lblWarning.Size = new System.Drawing.Size(15, 30);
		this.lblWarning.TabIndex = 23;
		this.lblWarning.Text = "0";
		this.lblWarning.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lblWarningDisplay.AutoSize = true;
		this.lblWarningDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblWarningDisplay.Location = new System.Drawing.Point(726, 1);
		this.lblWarningDisplay.Name = "lblWarningDisplay";
		this.lblWarningDisplay.Size = new System.Drawing.Size(57, 30);
		this.lblWarningDisplay.TabIndex = 22;
		this.lblWarningDisplay.Text = "Warning";
		this.lblWarningDisplay.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblEdit.AutoSize = true;
		this.lblEdit.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblEdit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblEdit.Location = new System.Drawing.Point(704, 1);
		this.lblEdit.Name = "lblEdit";
		this.lblEdit.Size = new System.Drawing.Size(15, 30);
		this.lblEdit.TabIndex = 21;
		this.lblEdit.Text = "0";
		this.lblEdit.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lblEditDisplay.AutoSize = true;
		this.lblEditDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblEditDisplay.Location = new System.Drawing.Point(667, 1);
		this.lblEditDisplay.Name = "lblEditDisplay";
		this.lblEditDisplay.Size = new System.Drawing.Size(30, 30);
		this.lblEditDisplay.TabIndex = 19;
		this.lblEditDisplay.Text = "Edit";
		this.lblEditDisplay.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblTotal.AutoSize = true;
		this.lblTotal.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblTotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblTotal.Location = new System.Drawing.Point(1041, 1);
		this.lblTotal.Name = "lblTotal";
		this.lblTotal.Size = new System.Drawing.Size(15, 30);
		this.lblTotal.TabIndex = 17;
		this.lblTotal.Text = "0";
		this.lblTotal.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lblTotalDisplay.AutoSize = true;
		this.lblTotalDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblTotalDisplay.Location = new System.Drawing.Point(996, 1);
		this.lblTotalDisplay.Name = "lblTotalDisplay";
		this.lblTotalDisplay.Size = new System.Drawing.Size(38, 30);
		this.lblTotalDisplay.TabIndex = 16;
		this.lblTotalDisplay.Text = "Total";
		this.lblTotalDisplay.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblEmpty.AutoSize = true;
		this.lblEmpty.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblEmpty.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblEmpty.Location = new System.Drawing.Point(974, 1);
		this.lblEmpty.Name = "lblEmpty";
		this.lblEmpty.Size = new System.Drawing.Size(15, 30);
		this.lblEmpty.TabIndex = 15;
		this.lblEmpty.Text = "0";
		this.lblEmpty.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lblEmptyDisplay.AutoSize = true;
		this.lblEmptyDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblEmptyDisplay.Location = new System.Drawing.Point(922, 1);
		this.lblEmptyDisplay.Name = "lblEmptyDisplay";
		this.lblEmptyDisplay.Size = new System.Drawing.Size(45, 30);
		this.lblEmptyDisplay.TabIndex = 14;
		this.lblEmptyDisplay.Text = "Empty";
		this.lblEmptyDisplay.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblOK.AutoSize = true;
		this.lblOK.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblOK.ForeColor = System.Drawing.Color.Blue;
		this.lblOK.Location = new System.Drawing.Point(900, 1);
		this.lblOK.Name = "lblOK";
		this.lblOK.Size = new System.Drawing.Size(15, 30);
		this.lblOK.TabIndex = 13;
		this.lblOK.Text = "0";
		this.lblOK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label5.AutoSize = true;
		this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
		this.label5.Location = new System.Drawing.Point(868, 1);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(25, 30);
		this.label5.TabIndex = 12;
		this.label5.Text = "OK";
		this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblNG.AutoSize = true;
		this.lblNG.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblNG.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblNG.ForeColor = System.Drawing.Color.Red;
		this.lblNG.Location = new System.Drawing.Point(846, 1);
		this.lblNG.Name = "lblNG";
		this.lblNG.Size = new System.Drawing.Size(15, 30);
		this.lblNG.TabIndex = 11;
		this.lblNG.Text = "0";
		this.lblNG.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label4.AutoSize = true;
		this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.label4.Location = new System.Drawing.Point(812, 1);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(27, 30);
		this.label4.TabIndex = 10;
		this.label4.Text = "NG";
		this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.panelLogout.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.panelLogout.BackColor = System.Drawing.Color.Transparent;
		this.panelLogout.Controls.Add(this.lblFullname);
		this.panelLogout.Controls.Add(this.ptbAvatar);
		this.panelLogout.Location = new System.Drawing.Point(729, 25);
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
		this.mPanelViewMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.mPanelViewMain.Dock = System.Windows.Forms.DockStyle.Right;
		this.mPanelViewMain.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.mPanelViewMain.Location = new System.Drawing.Point(730, 102);
		this.mPanelViewMain.Margin = new System.Windows.Forms.Padding(4);
		this.mPanelViewMain.Name = "mPanelViewMain";
		this.mPanelViewMain.Size = new System.Drawing.Size(350, 552);
		this.mPanelViewMain.TabIndex = 174;
		this.dgvHeader.AllowUserToAddRows = false;
		this.dgvHeader.AllowUserToDeleteRows = false;
		this.dgvHeader.AllowUserToResizeColumns = false;
		this.dgvHeader.AllowUserToResizeRows = false;
		this.dgvHeader.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
		this.dgvHeader.ColumnHeadersVisible = false;
		this.dgvHeader.Columns.AddRange(this.title, this.value, this.index);
		this.dgvHeader.Location = new System.Drawing.Point(22, 104);
		this.dgvHeader.Name = "dgvHeader";
		this.dgvHeader.RowHeadersVisible = false;
		this.dgvHeader.ScrollBars = System.Windows.Forms.ScrollBars.None;
		this.dgvHeader.Size = new System.Drawing.Size(200, 100);
		this.dgvHeader.TabIndex = 179;
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
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1100, 700);
		base.Controls.Add(this.dgvHeader);
		base.Controls.Add(this.mPanelViewMain);
		base.Controls.Add(this.dgvMain);
		base.Controls.Add(this.panelLogout);
		base.Controls.Add(this.statusStripfrmMain);
		base.Controls.Add(this.tpanelInfor);
		this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(4);
		base.Name = "frmCompleteView";
		base.Padding = new System.Windows.Forms.Padding(20, 70, 20, 20);
		this.Text = "5S QA Client * COMPLETE";
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmCompleteView_FormClosing);
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(frmCompleteView_FormClosed);
		base.Load += new System.EventHandler(frmCompleteView_Load);
		base.Shown += new System.EventHandler(frmCompleteView_Shown);
		this.statusStripfrmMain.ResumeLayout(false);
		this.statusStripfrmMain.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.dgvMain).EndInit();
		this.contextMenuStripMain.ResumeLayout(false);
		this.tpanelInfor.ResumeLayout(false);
		this.tpanelInfor.PerformLayout();
		this.panelLogout.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.ptbAvatar).EndInit();
		((System.ComponentModel.ISupportInitialize)this.dgvHeader).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
