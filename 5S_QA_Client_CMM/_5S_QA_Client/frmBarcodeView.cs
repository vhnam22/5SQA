using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using _5S_QA_Client.Controls;
using _5S_QA_Client.Properties;
using _5S_QA_DL.APIContext;
using _5S_QA_Entities.Dtos;
using _5S_QA_Entities.Models;
using AForge.Video;
using AForge.Video.DirectShow;
using MetroFramework.Forms;
using Newtonsoft.Json;
using ZXing;

namespace _5S_QA_Client;

public class frmBarcodeView : MetroForm
{
	private readonly frmLogin mForm;

	private readonly DateTime mDate;

	private RequestViewModel mRequest;

	private VideoCaptureDevice _videoCaptureDevice;

	private int mDevice = 0;

	private bool isClose = true;

	private IContainer components = null;

	private TableLayoutPanel tpanelTitle;

	private Label lblCamera;

	private PictureBox ptbBarcode;

	private ToolTip toolTipMain;

	private ComboBox cbbCamera;

	public frmBarcodeView(frmLogin frm, DateTime date)
	{
		InitializeComponent();
		Common.setControls(this, toolTipMain);
		mForm = frm;
		mDate = date;
	}

	private void frmBarcodeView_Load(object sender, EventArgs e)
	{
		Load_Settings();
		cbbCamera.DisplayMember = "Name";
		FilterInfoCollection filterInfoCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);
		foreach (FilterInfo item in filterInfoCollection)
		{
			cbbCamera.Items.Add(item);
		}
		if (filterInfoCollection.Count > 0)
		{
			if (filterInfoCollection.Count > mDevice)
			{
				cbbCamera.SelectedIndex = mDevice;
			}
			else
			{
				cbbCamera.SelectedIndex = 0;
			}
			Init_Barcode(((FilterInfo)cbbCamera.SelectedItem).MonikerString);
			cbbCamera.SelectedIndexChanged += cbbCamera_SelectedIndexChanged;
		}
	}

	private void frmBarcodeView_FormClosing(object sender, FormClosingEventArgs e)
	{
		GC.Collect();
	}

	private void frmBarcodeView_FormClosed(object sender, FormClosedEventArgs e)
	{
		if (_videoCaptureDevice != null && _videoCaptureDevice.IsRunning)
		{
			_videoCaptureDevice.Stop();
		}
		if (!isClose)
		{
			mForm.load_AllData();
			new frmResultView(mForm, mRequest).Show();
		}
	}

	private void Load_Settings()
	{
		mDevice = Settings.Default.Camera;
	}

	private void Init_Barcode(string MonikerString)
	{
		_videoCaptureDevice = new VideoCaptureDevice(MonikerString);
		_videoCaptureDevice.NewFrame += videoCaptureDevice_NewFrame;
		_videoCaptureDevice.Start();
	}

	private bool Create_Request(string result)
	{
		string[] array = result.Split('@');
		if (array.Length < 4)
		{
			Invoke((MethodInvoker)delegate
			{
				toolTipMain.Show(Common.getTextLanguage(this, "Incorrect"), this, 0, 0, 2000);
			});
			return false;
		}
		string text = array[0].Trim();
		if (string.IsNullOrEmpty(text))
		{
			Invoke((MethodInvoker)delegate
			{
				toolTipMain.Show(Common.getTextLanguage(this, "Empty"), this, 0, 0, 2000);
			});
			return false;
		}
		string text2 = array[1].Trim();
		string text3 = array[3].Trim();
		string name = $"{text2}_{text}_{mDate.Date:yyMMdd}";
		mRequest = Get_Request(name);
		if (mRequest == null)
		{
			Guid? productId = Get_ProductId(text2);
			if (!productId.HasValue)
			{
				Invoke((MethodInvoker)delegate
				{
					toolTipMain.Show(Common.getTextLanguage(this, "Product"), this, 0, 0, 2000);
				});
				return false;
			}
			string text4 = Set_Code();
			if (text4 == "REQ-")
			{
				Invoke((MethodInvoker)delegate
				{
					toolTipMain.Show(Common.getTextLanguage(this, "Code"), this, 0, 0, 2000);
				});
				return false;
			}
			RequestViewModel body = new RequestViewModel
			{
				Date = mDate,
				ProductId = productId,
				Code = text4,
				Name = name,
				Sample = 10
			};
			ResponseDto result2 = frmLogin.client.SaveAsync(body, "/api/Request/Save").Result;
			if (!result2.Success)
			{
				Invoke((MethodInvoker)delegate
				{
					toolTipMain.Show(Common.getTextLanguage(this, "Save"), this, 0, 0, 2000);
				});
				return false;
			}
			string value = result2.Data.ToString();
			mRequest = JsonConvert.DeserializeObject<RequestViewModel>(value);
		}
		isClose = false;
		return true;
	}

	private RequestViewModel Get_Request(string name)
	{
		RequestViewModel result = null;
		try
		{
			QueryArgs queryArgs = new QueryArgs();
			queryArgs.Predicate = "Name=@0";
			queryArgs.PredicateParameters = new string[1] { name };
			queryArgs.Order = "Created";
			queryArgs.Page = 1;
			queryArgs.Limit = 1;
			QueryArgs body = queryArgs;
			ResponseDto result2 = frmLogin.client.GetsAsync(body, "/api/Request/Gets").Result;
			List<RequestViewModel> objects = Common.getObjects<RequestViewModel>(result2);
			if (objects.Count > 0)
			{
				result = objects[0];
			}
		}
		catch
		{
		}
		return result;
	}

	private Guid? Get_ProductId(string code)
	{
		Guid? result = null;
		QueryArgs queryArgs = new QueryArgs();
		queryArgs.Predicate = "Measurements.Any() && IsActivated && Code=@0";
		queryArgs.PredicateParameters = new string[1] { code };
		queryArgs.Order = "Created";
		queryArgs.Page = 1;
		queryArgs.Limit = 1;
		QueryArgs body = queryArgs;
		ResponseDto result2 = frmLogin.client.GetsAsync(body, "/api/Product/Gets").Result;
		List<object> objects = Common.getObjects<object>(result2);
		if (objects.Count > 0)
		{
			return ((dynamic)objects[0]).id;
		}
		return result;
	}

	private string Set_Code()
	{
		string text = "REQ-";
		try
		{
			QueryArgs body = new QueryArgs
			{
				Order = "Created DESC",
				Page = 1,
				Limit = 1
			};
			ResponseDto result = frmLogin.client.GetsAsync(body, "/api/Request/Gets").Result;
			DataTable dataTable = Common.getDataTable<RequestViewModel>(result);
			if (dataTable.Rows.Count > 0)
			{
				string[] array = dataTable.Rows[0]["Code"].ToString().Split('-');
				if (array.Length > 1 && int.TryParse(array[1], out var result2))
				{
					text += result2 + 1;
				}
			}
			else
			{
				text += "1";
			}
		}
		catch
		{
		}
		return text;
	}

	private void videoCaptureDevice_NewFrame(object sender, NewFrameEventArgs eventArgs)
	{
		Bitmap bitmap = (Bitmap)eventArgs.Frame.Clone();
		BarcodeReader barcodeReader = new BarcodeReader();
		ZXing.Result result = barcodeReader.Decode(bitmap);
		if (result != null)
		{
			_videoCaptureDevice.NewFrame -= videoCaptureDevice_NewFrame;
			if (Create_Request(result.ToString()))
			{
				BeginInvoke((MethodInvoker)delegate
				{
					Close();
				});
			}
			else
			{
				_videoCaptureDevice.NewFrame += videoCaptureDevice_NewFrame;
			}
		}
		ptbBarcode.Image = bitmap;
	}

	private void cbbCamera_SelectedIndexChanged(object sender, EventArgs e)
	{
		Settings.Default.Camera = cbbCamera.SelectedIndex;
		Settings.Default.Save();
		if (_videoCaptureDevice != null && _videoCaptureDevice.IsRunning)
		{
			_videoCaptureDevice.NewFrame -= videoCaptureDevice_NewFrame;
			_videoCaptureDevice.Stop();
		}
		Init_Barcode(((FilterInfo)cbbCamera.SelectedItem).MonikerString);
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(_5S_QA_Client.frmBarcodeView));
		this.tpanelTitle = new System.Windows.Forms.TableLayoutPanel();
		this.cbbCamera = new System.Windows.Forms.ComboBox();
		this.lblCamera = new System.Windows.Forms.Label();
		this.ptbBarcode = new System.Windows.Forms.PictureBox();
		this.toolTipMain = new System.Windows.Forms.ToolTip(this.components);
		this.tpanelTitle.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.ptbBarcode).BeginInit();
		base.SuspendLayout();
		this.tpanelTitle.AutoSize = true;
		this.tpanelTitle.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
		this.tpanelTitle.ColumnCount = 2;
		this.tpanelTitle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tpanelTitle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100f));
		this.tpanelTitle.Controls.Add(this.cbbCamera, 1, 0);
		this.tpanelTitle.Controls.Add(this.lblCamera, 0, 0);
		this.tpanelTitle.Dock = System.Windows.Forms.DockStyle.Top;
		this.tpanelTitle.Location = new System.Drawing.Point(20, 70);
		this.tpanelTitle.Name = "tpanelTitle";
		this.tpanelTitle.RowCount = 1;
		this.tpanelTitle.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30f));
		this.tpanelTitle.Size = new System.Drawing.Size(460, 32);
		this.tpanelTitle.TabIndex = 1;
		this.cbbCamera.Cursor = System.Windows.Forms.Cursors.Hand;
		this.cbbCamera.Dock = System.Windows.Forms.DockStyle.Fill;
		this.cbbCamera.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.cbbCamera.FormattingEnabled = true;
		this.cbbCamera.Location = new System.Drawing.Point(66, 4);
		this.cbbCamera.Name = "cbbCamera";
		this.cbbCamera.Size = new System.Drawing.Size(390, 24);
		this.cbbCamera.TabIndex = 1;
		this.toolTipMain.SetToolTip(this.cbbCamera, "Please select a camera");
		this.lblCamera.AutoSize = true;
		this.lblCamera.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblCamera.Location = new System.Drawing.Point(4, 1);
		this.lblCamera.Name = "lblCamera";
		this.lblCamera.Size = new System.Drawing.Size(55, 30);
		this.lblCamera.TabIndex = 0;
		this.lblCamera.Text = "Camera";
		this.lblCamera.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.ptbBarcode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.ptbBarcode.Dock = System.Windows.Forms.DockStyle.Fill;
		this.ptbBarcode.Location = new System.Drawing.Point(20, 102);
		this.ptbBarcode.Name = "ptbBarcode";
		this.ptbBarcode.Size = new System.Drawing.Size(460, 378);
		this.ptbBarcode.TabIndex = 1;
		this.ptbBarcode.TabStop = false;
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(500, 500);
		base.Controls.Add(this.ptbBarcode);
		base.Controls.Add(this.tpanelTitle);
		this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.Margin = new System.Windows.Forms.Padding(4);
		base.Name = "frmBarcodeView";
		base.Padding = new System.Windows.Forms.Padding(20, 70, 20, 20);
		this.Text = "5S QA Client * BARCODE";
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmBarcodeView_FormClosing);
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(frmBarcodeView_FormClosed);
		base.Load += new System.EventHandler(frmBarcodeView_Load);
		this.tpanelTitle.ResumeLayout(false);
		this.tpanelTitle.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.ptbBarcode).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
