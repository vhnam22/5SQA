using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace _5S_QA_System.Controls;

public class DataGridViewTextBoxCellEx : DataGridViewTextBoxCell, ISpannedCell
{
	private int m_ColumnSpan = 1;

	private int m_RowSpan = 1;

	private DataGridViewTextBoxCellEx m_OwnerCell;

	public int ColumnSpan
	{
		get
		{
			return m_ColumnSpan;
		}
		set
		{
			if (base.DataGridView != null && m_OwnerCell == null)
			{
				if (value < 1 || base.ColumnIndex + value - 1 >= base.DataGridView.ColumnCount)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				if (m_ColumnSpan != value)
				{
					SetSpan(value, m_RowSpan);
				}
			}
		}
	}

	public int RowSpan
	{
		get
		{
			return m_RowSpan;
		}
		set
		{
			if (base.DataGridView != null && m_OwnerCell == null)
			{
				if (value < 1 || base.RowIndex + value - 1 >= base.DataGridView.RowCount)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				if (m_RowSpan != value)
				{
					SetSpan(m_ColumnSpan, value);
				}
			}
		}
	}

	public DataGridViewCell OwnerCell
	{
		get
		{
			return m_OwnerCell;
		}
		private set
		{
			m_OwnerCell = value as DataGridViewTextBoxCellEx;
		}
	}

	public override bool ReadOnly
	{
		get
		{
			return base.ReadOnly;
		}
		set
		{
			base.ReadOnly = value;
			if (m_OwnerCell != null || (m_ColumnSpan <= 1 && m_RowSpan <= 1) || base.DataGridView == null)
			{
				return;
			}
			foreach (int item in Enumerable.Range(base.ColumnIndex, m_ColumnSpan))
			{
				foreach (int item2 in Enumerable.Range(base.RowIndex, m_RowSpan))
				{
					if (item != base.ColumnIndex || item2 != base.RowIndex)
					{
						base.DataGridView[item, item2].ReadOnly = value;
					}
				}
			}
		}
	}

	protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates cellState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
	{
		if (m_OwnerCell != null && m_OwnerCell.DataGridView == null)
		{
			m_OwnerCell = null;
		}
		if (base.DataGridView == null || (m_OwnerCell == null && m_ColumnSpan == 1 && m_RowSpan == 1))
		{
			base.Paint(graphics, clipBounds, cellBounds, rowIndex, cellState, value, formattedValue, errorText, cellStyle, advancedBorderStyle, paintParts);
			return;
		}
		DataGridViewTextBoxCellEx dataGridViewTextBoxCellEx = this;
		int columnIndex = base.ColumnIndex;
		int columnSpan = m_ColumnSpan;
		int rowSpan = m_RowSpan;
		if (m_OwnerCell != null)
		{
			dataGridViewTextBoxCellEx = m_OwnerCell;
			columnIndex = m_OwnerCell.ColumnIndex;
			rowIndex = m_OwnerCell.RowIndex;
			columnSpan = m_OwnerCell.ColumnSpan;
			rowSpan = m_OwnerCell.RowSpan;
			value = m_OwnerCell.GetValue(rowIndex);
			errorText = m_OwnerCell.GetErrorText(rowIndex);
			cellState = m_OwnerCell.State;
			cellStyle = m_OwnerCell.GetInheritedStyle(null, rowIndex, includeColors: true);
			formattedValue = m_OwnerCell.GetFormattedValue(value, rowIndex, ref cellStyle, null, null, DataGridViewDataErrorContexts.Display);
		}
		if (CellsRegionContainsSelectedCell(columnIndex, rowIndex, columnSpan, rowSpan))
		{
			cellState |= DataGridViewElementStates.Selected;
		}
		Rectangle spannedCellBoundsFromChildCellBounds = DataGridViewCellExHelper.GetSpannedCellBoundsFromChildCellBounds(this, cellBounds, base.DataGridView.SingleVerticalBorderAdded(), base.DataGridView.SingleHorizontalBorderAdded());
		clipBounds = DataGridViewCellExHelper.GetSpannedCellClipBounds(dataGridViewTextBoxCellEx, spannedCellBoundsFromChildCellBounds, base.DataGridView.SingleVerticalBorderAdded(), base.DataGridView.SingleHorizontalBorderAdded());
		using Graphics graphics2 = base.DataGridView.CreateGraphics();
		graphics2.SetClip(clipBounds);
		advancedBorderStyle = DataGridViewCellExHelper.AdjustCellBorderStyle(dataGridViewTextBoxCellEx);
		dataGridViewTextBoxCellEx.NativePaint(graphics2, clipBounds, spannedCellBoundsFromChildCellBounds, rowIndex, cellState, value, formattedValue, errorText, cellStyle, advancedBorderStyle, paintParts & ~DataGridViewPaintParts.Border);
		if ((paintParts & DataGridViewPaintParts.Border) != DataGridViewPaintParts.None)
		{
			DataGridViewTextBoxCellEx dataGridViewTextBoxCellEx2 = dataGridViewTextBoxCellEx;
			DataGridViewAdvancedBorderStyle advancedBorderStyle2 = new DataGridViewAdvancedBorderStyle
			{
				Left = advancedBorderStyle.Left,
				Top = advancedBorderStyle.Top,
				Right = DataGridViewAdvancedCellBorderStyle.None,
				Bottom = DataGridViewAdvancedCellBorderStyle.None
			};
			dataGridViewTextBoxCellEx2.PaintBorder(graphics2, clipBounds, spannedCellBoundsFromChildCellBounds, cellStyle, advancedBorderStyle2);
			DataGridViewTextBoxCellEx dataGridViewTextBoxCellEx3 = (base.DataGridView[columnIndex + columnSpan - 1, rowIndex + rowSpan - 1] as DataGridViewTextBoxCellEx) ?? this;
			DataGridViewAdvancedBorderStyle advancedBorderStyle3 = new DataGridViewAdvancedBorderStyle
			{
				Left = DataGridViewAdvancedCellBorderStyle.None,
				Top = DataGridViewAdvancedCellBorderStyle.None,
				Right = advancedBorderStyle.Right,
				Bottom = advancedBorderStyle.Bottom
			};
			dataGridViewTextBoxCellEx3.PaintBorder(graphics2, clipBounds, spannedCellBoundsFromChildCellBounds, cellStyle, advancedBorderStyle3);
		}
	}

	private void NativePaint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates cellState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
	{
		base.Paint(graphics, clipBounds, cellBounds, rowIndex, cellState, value, formattedValue, errorText, cellStyle, advancedBorderStyle, paintParts);
	}

	private void SetSpan(int columnSpan, int rowSpan)
	{
		int columnSpan2 = m_ColumnSpan;
		int rowSpan2 = m_RowSpan;
		m_ColumnSpan = columnSpan;
		m_RowSpan = rowSpan;
		if (base.DataGridView == null)
		{
			return;
		}
		foreach (int item in Enumerable.Range(base.RowIndex, rowSpan2))
		{
			foreach (int item2 in Enumerable.Range(base.ColumnIndex, columnSpan2))
			{
				if (base.DataGridView[item2, item] is DataGridViewTextBoxCellEx dataGridViewTextBoxCellEx)
				{
					dataGridViewTextBoxCellEx.OwnerCell = null;
				}
			}
		}
		foreach (int item3 in Enumerable.Range(base.RowIndex, m_RowSpan))
		{
			foreach (int item4 in Enumerable.Range(base.ColumnIndex, m_ColumnSpan))
			{
				if (base.DataGridView[item4, item3] is DataGridViewTextBoxCellEx dataGridViewTextBoxCellEx2 && dataGridViewTextBoxCellEx2 != this)
				{
					if (dataGridViewTextBoxCellEx2.ColumnSpan > 1)
					{
						dataGridViewTextBoxCellEx2.ColumnSpan = 1;
					}
					if (dataGridViewTextBoxCellEx2.RowSpan > 1)
					{
						dataGridViewTextBoxCellEx2.RowSpan = 1;
					}
					dataGridViewTextBoxCellEx2.OwnerCell = this;
				}
			}
		}
		OwnerCell = null;
		base.DataGridView.Invalidate();
	}

	public override Rectangle PositionEditingPanel(Rectangle cellBounds, Rectangle cellClip, DataGridViewCellStyle cellStyle, bool singleVerticalBorderAdded, bool singleHorizontalBorderAdded, bool isFirstDisplayedColumn, bool isFirstDisplayedRow)
	{
		if (m_OwnerCell == null && m_ColumnSpan == 1 && m_RowSpan == 1)
		{
			return base.PositionEditingPanel(cellBounds, cellClip, cellStyle, singleVerticalBorderAdded, singleHorizontalBorderAdded, isFirstDisplayedColumn, isFirstDisplayedRow);
		}
		DataGridViewTextBoxCellEx dataGridViewTextBoxCellEx = this;
		if (m_OwnerCell != null)
		{
			int rowIndex = m_OwnerCell.RowIndex;
			cellStyle = m_OwnerCell.GetInheritedStyle(null, rowIndex, includeColors: true);
			m_OwnerCell.GetFormattedValue(m_OwnerCell.Value, rowIndex, ref cellStyle, null, null, DataGridViewDataErrorContexts.Formatting);
			if (base.DataGridView.EditingControl is IDataGridViewEditingControl dataGridViewEditingControl)
			{
				dataGridViewEditingControl.ApplyCellStyleToEditingControl(cellStyle);
				Control parent = base.DataGridView.EditingControl.Parent;
				if (parent != null)
				{
					parent.BackColor = cellStyle.BackColor;
				}
			}
			dataGridViewTextBoxCellEx = m_OwnerCell;
		}
		cellBounds = DataGridViewCellExHelper.GetSpannedCellBoundsFromChildCellBounds(this, cellBounds, singleVerticalBorderAdded, singleHorizontalBorderAdded);
		cellClip = DataGridViewCellExHelper.GetSpannedCellClipBounds(dataGridViewTextBoxCellEx, cellBounds, singleVerticalBorderAdded, singleHorizontalBorderAdded);
		return base.PositionEditingPanel(cellBounds, cellClip, cellStyle, singleVerticalBorderAdded, singleHorizontalBorderAdded, dataGridViewTextBoxCellEx.InFirstDisplayedColumn(), dataGridViewTextBoxCellEx.InFirstDisplayedRow());
	}

	protected override object GetValue(int rowIndex)
	{
		if (m_OwnerCell != null)
		{
			return m_OwnerCell.GetValue(m_OwnerCell.RowIndex);
		}
		return base.GetValue(rowIndex);
	}

	protected override bool SetValue(int rowIndex, object value)
	{
		if (m_OwnerCell != null)
		{
			return m_OwnerCell.SetValue(m_OwnerCell.RowIndex, value);
		}
		return base.SetValue(rowIndex, value);
	}

	protected override void OnDataGridViewChanged()
	{
		base.OnDataGridViewChanged();
		if (base.DataGridView == null)
		{
			m_ColumnSpan = 1;
			m_RowSpan = 1;
		}
	}

	protected override Rectangle BorderWidths(DataGridViewAdvancedBorderStyle advancedBorderStyle)
	{
		if (m_OwnerCell == null && m_ColumnSpan == 1 && m_RowSpan == 1)
		{
			return base.BorderWidths(advancedBorderStyle);
		}
		if (m_OwnerCell != null)
		{
			return m_OwnerCell.BorderWidths(advancedBorderStyle);
		}
		Rectangle rectangle = base.BorderWidths(advancedBorderStyle);
		Rectangle rectangle2 = ((base.DataGridView[base.ColumnIndex + ColumnSpan - 1, base.RowIndex + RowSpan - 1] is DataGridViewTextBoxCellEx dataGridViewTextBoxCellEx) ? dataGridViewTextBoxCellEx.NativeBorderWidths(advancedBorderStyle) : rectangle);
		return new Rectangle(rectangle.X, rectangle.Y, rectangle2.Width, rectangle2.Height);
	}

	private Rectangle NativeBorderWidths(DataGridViewAdvancedBorderStyle advancedBorderStyle)
	{
		return base.BorderWidths(advancedBorderStyle);
	}

	protected override Size GetPreferredSize(Graphics graphics, DataGridViewCellStyle cellStyle, int rowIndex, Size constraintSize)
	{
		if (OwnerCell != null)
		{
			return new Size(0, 0);
		}
		Size preferredSize = base.GetPreferredSize(graphics, cellStyle, rowIndex, constraintSize);
		DataGridView grid = base.DataGridView;
		int width = preferredSize.Width - (from index in Enumerable.Range(base.ColumnIndex + 1, ColumnSpan - 1)
			select grid.Columns[index].Width).Sum();
		int height = preferredSize.Height - (from index in Enumerable.Range(base.RowIndex + 1, RowSpan - 1)
			select grid.Rows[index].Height).Sum();
		return new Size(width, height);
	}

	private bool CellsRegionContainsSelectedCell(int columnIndex, int rowIndex, int columnSpan, int rowSpan)
	{
		if (base.DataGridView == null)
		{
			return false;
		}
		return (from col in Enumerable.Range(columnIndex, columnSpan)
			from row in Enumerable.Range(rowIndex, rowSpan)
			where base.DataGridView[col, row].Selected
			select col).Any();
	}
}
