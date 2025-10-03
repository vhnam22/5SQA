using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using _5S_QA_DL.APIContext;
using _5S_QA_Entities.Dtos;
using _5S_QA_Entities.Models;
using _5S_QA_System.Controls;
using MetroFramework.Forms;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

namespace _5S_QA_System;

public class frmFormula : MetroForm
{
	private TextBox txtFormula;

	private TextBox txtCode;

	private TextBox txtValue;

	private TextBox txtUpper;

	private TextBox txtLower;

	private frmMeasurementAdd mForm;

	private DataTable mData;

	private Guid mId;

	private IContainer components = null;

	private TableLayoutPanel tableLayoutPanel1;

	private TreeView treeViewMeas;

	private TreeView treeViewCalculation;

	private Button btnCheck;

	private ToolTip toolTipMain;

	private RichTextBox rtbFormula;

	private Button btnConfirm;

	public frmFormula(frmMeasurementAdd frm, Guid id = default(Guid))
	{
		InitializeComponent();
		Common.setControls(this, toolTipMain);
		mForm = frm;
		mForm.isFomulaOpen = true;
		mId = id;
	}

	private void frmFormula_Load(object sender, EventArgs e)
	{
		base.Location = new Point((mForm.Location.X - base.Width > 0) ? (mForm.Location.X - base.Width) : 0, mForm.Location.Y);
		base.Height = mForm.Height;
		txtFormula = mForm.Controls.Find("txtFormula", searchAllChildren: true).FirstOrDefault() as TextBox;
		txtCode = mForm.Controls.Find("txtCode", searchAllChildren: true).FirstOrDefault() as TextBox;
		txtValue = mForm.Controls.Find("txtValue", searchAllChildren: true).FirstOrDefault() as TextBox;
		txtUpper = mForm.Controls.Find("txtUpper", searchAllChildren: true).FirstOrDefault() as TextBox;
		txtLower = mForm.Controls.Find("txtLower", searchAllChildren: true).FirstOrDefault() as TextBox;
		txtFormula.Enabled = false;
		rtbFormula.Text = txtFormula.Text.Replace(";", ";\r\n");
		Init();
	}

	private void frmFormula_FormClosed(object sender, FormClosedEventArgs e)
	{
		txtFormula.Enabled = true;
		mForm.isFomulaOpen = false;
	}

	private void Init()
	{
		treeViewCalculation.ShowNodeToolTips = true;
		treeViewCalculation.Nodes.Add(Common.getTextLanguage(this, "Calculation"));
		treeViewCalculation.Nodes[0].Nodes.Add("+");
		treeViewCalculation.Nodes[0].Nodes[0].ToolTipText = "+";
		treeViewCalculation.Nodes[0].Nodes.Add("-");
		treeViewCalculation.Nodes[0].Nodes[1].ToolTipText = "-";
		treeViewCalculation.Nodes[0].Nodes.Add("*");
		treeViewCalculation.Nodes[0].Nodes[2].ToolTipText = "*";
		treeViewCalculation.Nodes[0].Nodes.Add("/");
		treeViewCalculation.Nodes[0].Nodes[3].ToolTipText = "/";
		treeViewCalculation.Nodes[0].Nodes.Add("()");
		treeViewCalculation.Nodes[0].Nodes[4].ToolTipText = "()";
		treeViewCalculation.Nodes[0].Nodes.Add("=");
		treeViewCalculation.Nodes[0].Nodes[5].ToolTipText = "=";
		treeViewCalculation.Nodes[0].Nodes.Add("Abs");
		treeViewCalculation.Nodes[0].Nodes[6].ToolTipText = "System.Math.Abs(number)";
		treeViewCalculation.Nodes[0].Nodes.Add("Pow");
		treeViewCalculation.Nodes[0].Nodes[7].ToolTipText = "System.Math.Pow(number1, number2)";
		treeViewCalculation.Nodes[0].Nodes.Add("Sqrt");
		treeViewCalculation.Nodes[0].Nodes[8].ToolTipText = "System.Math.Sqrt(number)";
		treeViewCalculation.Nodes[0].Nodes.Add("Min");
		treeViewCalculation.Nodes[0].Nodes[9].ToolTipText = "System.Math.Min(number1, number2)";
		treeViewCalculation.Nodes[0].Nodes.Add("Max");
		treeViewCalculation.Nodes[0].Nodes[10].ToolTipText = "System.Math.Max(number1, number2)";
		treeViewCalculation.Nodes[0].Nodes.Add("PI");
		treeViewCalculation.Nodes[0].Nodes[11].ToolTipText = "System.Math.PI";
		treeViewCalculation.Nodes[0].Nodes.Add("Sin");
		treeViewCalculation.Nodes[0].Nodes[12].ToolTipText = "System.Math.Sin(number)";
		treeViewCalculation.Nodes[0].Nodes.Add("Cos");
		treeViewCalculation.Nodes[0].Nodes[13].ToolTipText = "System.Math.Cos(number)";
		treeViewCalculation.Nodes[0].Nodes.Add("Tan");
		treeViewCalculation.Nodes[0].Nodes[14].ToolTipText = "System.Math.Tan(number)";
		treeViewCalculation.Nodes[0].Nodes.Add(";");
		treeViewCalculation.Nodes[0].Nodes[15].ToolTipText = "; (new formula)";
		treeViewCalculation.Nodes[0].Nodes.Add("UnAbs");
		treeViewCalculation.Nodes[0].Nodes[16].ToolTipText = "[UnAbs]";
		treeViewCalculation.Nodes[0].Expand();
		treeViewMeas.ShowNodeToolTips = true;
		treeViewMeas.Nodes.Add(Common.getTextLanguage(this, "Measurement"));
		mData = getMeas();
		int num = 0;
		foreach (DataRow row in mData.Rows)
		{
			treeViewMeas.Nodes[0].Nodes.Add(row.Field<string>("Code"));
			treeViewMeas.Nodes[0].Nodes[num].Nodes.Add("VALUE");
			treeViewMeas.Nodes[0].Nodes[num].Nodes[0].ToolTipText = "[" + row.Field<string>("Code") + "#VALUE]";
			treeViewMeas.Nodes[0].Nodes[num].Nodes.Add("UPPER");
			treeViewMeas.Nodes[0].Nodes[num].Nodes[1].ToolTipText = "[" + row.Field<string>("Code") + "#UPPER]";
			treeViewMeas.Nodes[0].Nodes[num].Nodes.Add("LOWER");
			treeViewMeas.Nodes[0].Nodes[num].Nodes[2].ToolTipText = "[" + row.Field<string>("Code") + "#LOWER]";
			treeViewMeas.Nodes[0].Nodes[num].Nodes.Add("RESULT");
			treeViewMeas.Nodes[0].Nodes[num].Nodes[3].ToolTipText = "[" + row.Field<string>("Code") + "#RESULT]";
			if (row["Code"] != null && row["Code"].ToString().Equals(txtCode.Text))
			{
				treeViewMeas.Nodes[0].Nodes[num].Nodes.Add("WARNUPPER");
				treeViewMeas.Nodes[0].Nodes[num].Nodes[4].ToolTipText = "[WARNUPPER]";
				treeViewMeas.Nodes[0].Nodes[num].Nodes.Add("WARNLOWER");
				treeViewMeas.Nodes[0].Nodes[num].Nodes[5].ToolTipText = "[WARNLOWER]";
			}
			num++;
		}
		treeViewMeas.Nodes[0].Expand();
	}

	private DataTable getMeas()
	{
		Cursor.Current = Cursors.WaitCursor;
		DataTable result = new DataTable();
		try
		{
			QueryArgs queryArgs = new QueryArgs();
			queryArgs.Predicate = "ProductId=@0";
			queryArgs.PredicateParameters = new string[1] { mId.ToString() };
			queryArgs.Order = "Sort, Created";
			queryArgs.Page = 1;
			queryArgs.Limit = int.MaxValue;
			QueryArgs body = queryArgs;
			ResponseDto result2 = frmLogin.client.GetsAsync(body, "/api/Measurement/Gets").Result;
			result = Common.getDataTable<MeasurementQuickViewModel>(result2);
		}
		catch
		{
		}
		return result;
	}

	private string checkFormula()
	{
		string text = string.Empty;
		string text2 = Common.trimSpace(rtbFormula.Text.Replace(Environment.NewLine, "").Replace("\n", ""));
		if (!string.IsNullOrEmpty(text2))
		{
			List<string> list = (from x in text2.Split(';')
				where !x.Trim().Equals("")
				select x).ToList();
			if (list.Count != 0)
			{
				foreach (string item in list)
				{
					if (item.Equals("[UnAbs]"))
					{
						text += "[RESULT] = 0; ";
						continue;
					}
					List<string> list2 = (from x in item.Split('=')
						where !x.Trim().Equals("")
						select x).ToList();
					if (list2.Count().Equals(2))
					{
						IEnumerable<string> enumerable = from x in list2[1].Split('[')
							select (x.Split(']').Length != 0) ? x.Split(']')[0] : "" into x
							where !string.IsNullOrEmpty(x.Trim()) && !x.Contains("(")
							select x;
						foreach (string item2 in enumerable)
						{
							string parameterAsync = getParameterAsync(item2.Trim());
							if (string.IsNullOrEmpty(parameterAsync))
							{
								break;
							}
							list2[1] = list2[1].Replace("[" + item2 + "]", parameterAsync ?? "");
						}
						try
						{
							Task<object> task = CSharpScript.EvaluateAsync(list2[1], (ScriptOptions)null, (object)null, (Type)null, default(CancellationToken));
							string text3 = list2[0].Trim();
							text = ((!text3.Equals("[RESULT]") && !text3.Equals("[VALUE]") && !text3.Equals("[UPPER]") && !text3.Equals("[LOWER]") && !text3.Equals("[WARNUPPER]") && !text3.Equals("[WARNLOWER]")) ? (text + Common.getTextLanguage("Error", "Error: Return type result different") + " [RESULT], [VALUE], [UPPER], [LOWER], [WARNUPPER], [WARNLOWER]; ") : (text + $"{text3} = {task.Result}; "));
						}
						catch (Exception ex)
						{
							text = text + "Error: " + ex.Message + "; ";
						}
					}
					else
					{
						text += Common.getTextLanguage("Error", "Error: Command format incorrect");
					}
				}
			}
			else
			{
				text += "0";
			}
		}
		else
		{
			text += "0";
		}
		return text;
	}

	private string getParameterAsync(string name)
	{
		string[] lstname = name.Split('#');
		string result = string.Empty;
		if (lstname.Length > 1)
		{
			DataRow[] array = (from DataRow r in mData.Rows
				where r.Field<string>("Code").Equals(lstname[0])
				select r).ToArray();
			if (array.Count() > 0)
			{
				switch (lstname[1])
				{
				case "RESULT":
					result = array[0].Field<string>("Value").Replace(",", "");
					break;
				case "VALUE":
					result = array[0].Field<string>("Value").Replace(",", "");
					break;
				case "UPPER":
					result = array[0].Field<double>("Upper").ToString().Replace(",", "");
					break;
				case "LOWER":
					result = array[0].Field<double>("Lower").ToString().Replace(",", "");
					break;
				}
			}
		}
		else
		{
			switch (lstname[0])
			{
			case "RESULT":
				result = txtValue.Text.Replace(",", "");
				break;
			case "VALUE":
				result = txtValue.Text.Replace(",", "");
				break;
			case "UPPER":
				result = txtUpper.Text.Replace(",", "");
				break;
			case "LOWER":
				result = txtLower.Text.Replace(",", "");
				break;
			}
		}
		return result;
	}

	private void treeViewCalculation_DoubleClick(object sender, EventArgs e)
	{
		if (treeViewCalculation.SelectedNode != null)
		{
			int selectionStart = rtbFormula.SelectionStart;
			switch (treeViewCalculation.SelectedNode.Text)
			{
			case "+":
				rtbFormula.Text = rtbFormula.Text.Insert(selectionStart, "+");
				rtbFormula.Select(selectionStart + treeViewCalculation.SelectedNode.Text.Length, 0);
				break;
			case "-":
				rtbFormula.Text = rtbFormula.Text.Insert(selectionStart, "-");
				rtbFormula.Select(selectionStart + treeViewCalculation.SelectedNode.Text.Length, 0);
				break;
			case "*":
				rtbFormula.Text = rtbFormula.Text.Insert(selectionStart, "*");
				rtbFormula.Select(selectionStart + treeViewCalculation.SelectedNode.Text.Length, 0);
				break;
			case "/":
				rtbFormula.Text = rtbFormula.Text.Insert(selectionStart, "/");
				rtbFormula.Select(selectionStart + treeViewCalculation.SelectedNode.Text.Length, 0);
				break;
			case "()":
				rtbFormula.Text = rtbFormula.Text.Insert(selectionStart, "()");
				rtbFormula.Select(selectionStart + treeViewCalculation.SelectedNode.Text.Length, 0);
				break;
			case "=":
				rtbFormula.Text = rtbFormula.Text.Insert(selectionStart, "=");
				rtbFormula.Select(selectionStart + treeViewCalculation.SelectedNode.Text.Length, 0);
				break;
			case "Abs":
			{
				string text = "System.Math.Abs(0)";
				rtbFormula.Text = rtbFormula.Text.Insert(selectionStart, text);
				rtbFormula.Select(selectionStart + text.Length, 0);
				break;
			}
			case "Pow":
			{
				string text = "System.Math.Pow(0, 0)";
				rtbFormula.Text = rtbFormula.Text.Insert(selectionStart, text);
				rtbFormula.Select(selectionStart + text.Length, 0);
				break;
			}
			case "Sqrt":
			{
				string text = "System.Math.Sqrt(0)";
				rtbFormula.Text = rtbFormula.Text.Insert(selectionStart, text);
				rtbFormula.Select(selectionStart + text.Length, 0);
				break;
			}
			case "Min":
			{
				string text = "System.Math.Min(0, 0)";
				rtbFormula.Text = rtbFormula.Text.Insert(selectionStart, text);
				rtbFormula.Select(selectionStart + text.Length, 0);
				break;
			}
			case "Max":
			{
				string text = "System.Math.Max(0, 0)";
				rtbFormula.Text = rtbFormula.Text.Insert(selectionStart, text);
				rtbFormula.Select(selectionStart + text.Length, 0);
				break;
			}
			case "PI":
			{
				string text = "System.Math.PI";
				rtbFormula.Text = rtbFormula.Text.Insert(selectionStart, text);
				rtbFormula.Select(selectionStart + text.Length, 0);
				break;
			}
			case "Sin":
			{
				string text = "System.Math.Sin(0)";
				rtbFormula.Text = rtbFormula.Text.Insert(selectionStart, text);
				rtbFormula.Select(selectionStart + text.Length, 0);
				break;
			}
			case "Cos":
			{
				string text = "System.Math.Cos(0)";
				rtbFormula.Text = rtbFormula.Text.Insert(selectionStart, text);
				rtbFormula.Select(selectionStart + text.Length, 0);
				break;
			}
			case "Tan":
			{
				string text = "System.Math.Tan(0)";
				rtbFormula.Text = rtbFormula.Text.Insert(selectionStart, text);
				rtbFormula.Select(selectionStart + text.Length, 0);
				break;
			}
			case ";":
				rtbFormula.Text = rtbFormula.Text.Insert(selectionStart, ";");
				rtbFormula.Select(selectionStart + treeViewCalculation.SelectedNode.Text.Length, 0);
				break;
			case "UnAbs":
			{
				string text = "[UnAbs]";
				rtbFormula.Text = rtbFormula.Text.Insert(selectionStart, text);
				rtbFormula.Select(selectionStart + text.Length, 0);
				break;
			}
			}
		}
	}

	private void treeViewMeas_DoubleClick(object sender, EventArgs e)
	{
		if (treeViewMeas.SelectedNode != null)
		{
			int selectionStart = rtbFormula.SelectionStart;
			string[] array = treeViewMeas.SelectedNode.FullPath.Split('\\');
			if (array.Length > 2)
			{
				string empty = string.Empty;
				empty = ((!txtCode.Text.Equals(array[1])) ? ("[" + array[1] + "#" + array[2] + "]") : ("[" + array[2] + "]"));
				rtbFormula.Text = rtbFormula.Text.Insert(selectionStart, empty);
				rtbFormula.Select(selectionStart + empty.Length, 0);
			}
		}
	}

	private void btnCheck_Click(object sender, EventArgs e)
	{
		Cursor.Current = Cursors.WaitCursor;
		string text = checkFormula();
		if (!string.IsNullOrEmpty(text) && !text.Contains(Common.getTextLanguage("Error", "Error:")))
		{
			MessageBox.Show(text, Common.getTextLanguage(this, "INFORMATION"), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
		}
		else
		{
			MessageBox.Show(text, Common.getTextLanguage(this, "ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnConfirm_Click(object sender, EventArgs e)
	{
		Cursor.Current = Cursors.WaitCursor;
		string text = checkFormula();
		if (!string.IsNullOrEmpty(text) && !text.Contains(Common.getTextLanguage("Error", "Error:")))
		{
			txtFormula.Text = Common.trimSpace(rtbFormula.Text.Replace(Environment.NewLine, "").Replace("\n", ""));
		}
		else
		{
			MessageBox.Show(text, Common.getTextLanguage(this, "ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(_5S_QA_System.frmFormula));
		this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
		this.treeViewMeas = new System.Windows.Forms.TreeView();
		this.treeViewCalculation = new System.Windows.Forms.TreeView();
		this.btnCheck = new System.Windows.Forms.Button();
		this.toolTipMain = new System.Windows.Forms.ToolTip(this.components);
		this.btnConfirm = new System.Windows.Forms.Button();
		this.rtbFormula = new System.Windows.Forms.RichTextBox();
		this.tableLayoutPanel1.SuspendLayout();
		base.SuspendLayout();
		this.tableLayoutPanel1.ColumnCount = 2;
		this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40f));
		this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60f));
		this.tableLayoutPanel1.Controls.Add(this.treeViewMeas, 1, 0);
		this.tableLayoutPanel1.Controls.Add(this.treeViewCalculation, 0, 0);
		this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tableLayoutPanel1.Location = new System.Drawing.Point(20, 220);
		this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
		this.tableLayoutPanel1.Name = "tableLayoutPanel1";
		this.tableLayoutPanel1.RowCount = 1;
		this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100f));
		this.tableLayoutPanel1.Size = new System.Drawing.Size(360, 232);
		this.tableLayoutPanel1.TabIndex = 2;
		this.tableLayoutPanel1.TabStop = true;
		this.treeViewMeas.Dock = System.Windows.Forms.DockStyle.Fill;
		this.treeViewMeas.Location = new System.Drawing.Point(147, 0);
		this.treeViewMeas.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
		this.treeViewMeas.Name = "treeViewMeas";
		this.treeViewMeas.Size = new System.Drawing.Size(213, 232);
		this.treeViewMeas.TabIndex = 2;
		this.treeViewMeas.DoubleClick += new System.EventHandler(treeViewMeas_DoubleClick);
		this.treeViewCalculation.Dock = System.Windows.Forms.DockStyle.Fill;
		this.treeViewCalculation.Location = new System.Drawing.Point(0, 0);
		this.treeViewCalculation.Margin = new System.Windows.Forms.Padding(0);
		this.treeViewCalculation.Name = "treeViewCalculation";
		this.treeViewCalculation.Size = new System.Drawing.Size(144, 232);
		this.treeViewCalculation.TabIndex = 1;
		this.treeViewCalculation.DoubleClick += new System.EventHandler(treeViewCalculation_DoubleClick);
		this.btnCheck.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnCheck.Dock = System.Windows.Forms.DockStyle.Top;
		this.btnCheck.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCheck.Location = new System.Drawing.Point(20, 192);
		this.btnCheck.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.btnCheck.Name = "btnCheck";
		this.btnCheck.Size = new System.Drawing.Size(360, 28);
		this.btnCheck.TabIndex = 3;
		this.btnCheck.Text = "Check formula";
		this.toolTipMain.SetToolTip(this.btnCheck, "Check formula");
		this.btnCheck.UseVisualStyleBackColor = true;
		this.btnCheck.Click += new System.EventHandler(btnCheck_Click);
		this.btnConfirm.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnConfirm.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.btnConfirm.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnConfirm.Location = new System.Drawing.Point(20, 452);
		this.btnConfirm.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.btnConfirm.Name = "btnConfirm";
		this.btnConfirm.Size = new System.Drawing.Size(360, 28);
		this.btnConfirm.TabIndex = 4;
		this.btnConfirm.Text = "Confirm";
		this.toolTipMain.SetToolTip(this.btnConfirm, "Select confirm");
		this.btnConfirm.UseVisualStyleBackColor = true;
		this.btnConfirm.Click += new System.EventHandler(btnConfirm_Click);
		this.rtbFormula.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.rtbFormula.Dock = System.Windows.Forms.DockStyle.Top;
		this.rtbFormula.Location = new System.Drawing.Point(20, 70);
		this.rtbFormula.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.rtbFormula.Name = "rtbFormula";
		this.rtbFormula.Size = new System.Drawing.Size(360, 122);
		this.rtbFormula.TabIndex = 1;
		this.rtbFormula.Text = "";
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(400, 500);
		base.Controls.Add(this.tableLayoutPanel1);
		base.Controls.Add(this.btnCheck);
		base.Controls.Add(this.rtbFormula);
		base.Controls.Add(this.btnConfirm);
		this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		base.Name = "frmFormula";
		base.Padding = new System.Windows.Forms.Padding(20, 70, 20, 20);
		base.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultLocation;
		this.Text = "5S QA System * FORMULA";
		base.TopMost = true;
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(frmFormula_FormClosed);
		base.Load += new System.EventHandler(frmFormula_Load);
		this.tableLayoutPanel1.ResumeLayout(false);
		base.ResumeLayout(false);
	}
}
