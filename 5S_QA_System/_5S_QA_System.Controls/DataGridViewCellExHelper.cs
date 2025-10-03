using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace _5S_QA_System.Controls;

internal static class DataGridViewCellExHelper
{
	public static Rectangle GetSpannedCellClipBounds<TCell>(TCell ownerCell, Rectangle cellBounds, bool singleVerticalBorderAdded, bool singleHorizontalBorderAdded) where TCell : DataGridViewCell, ISpannedCell
	{
		DataGridView dataGridView = ownerCell.DataGridView;
		Rectangle result = cellBounds;
		foreach (int item in Enumerable.Range(ownerCell.ColumnIndex, ownerCell.ColumnSpan))
		{
			DataGridViewColumn dataGridViewColumn = dataGridView.Columns[item];
			if (!dataGridViewColumn.Visible)
			{
				continue;
			}
			if (dataGridViewColumn.Frozen || item > dataGridView.FirstDisplayedScrollingColumnIndex)
			{
				break;
			}
			if (item == dataGridView.FirstDisplayedScrollingColumnIndex)
			{
				result.Width -= dataGridView.FirstDisplayedScrollingColumnHiddenWidth;
				if (dataGridView.RightToLeft != RightToLeft.Yes)
				{
					result.X += dataGridView.FirstDisplayedScrollingColumnHiddenWidth;
				}
				break;
			}
			result.Width -= dataGridViewColumn.Width;
			if (dataGridView.RightToLeft != RightToLeft.Yes)
			{
				result.X += dataGridViewColumn.Width;
			}
		}
		foreach (int item2 in Enumerable.Range(ownerCell.RowIndex, ownerCell.RowSpan))
		{
			DataGridViewRow dataGridViewRow = dataGridView.Rows[item2];
			if (dataGridViewRow.Visible)
			{
				if (dataGridViewRow.Frozen || item2 >= dataGridView.FirstDisplayedScrollingRowIndex)
				{
					break;
				}
				result.Y += dataGridViewRow.Height;
				result.Height -= dataGridViewRow.Height;
			}
		}
		if (dataGridView.BorderStyle != BorderStyle.None)
		{
			Rectangle clientRectangle = dataGridView.ClientRectangle;
			clientRectangle.Width--;
			clientRectangle.Height--;
			if (dataGridView.RightToLeft == RightToLeft.Yes)
			{
				clientRectangle.X++;
				clientRectangle.Y++;
			}
			result.Intersect(clientRectangle);
		}
		return result;
	}

	public static Rectangle GetSpannedCellBoundsFromChildCellBounds<TCell>(TCell childCell, Rectangle childCellBounds, bool singleVerticalBorderAdded, bool singleHorizontalBorderAdded) where TCell : DataGridViewCell, ISpannedCell
	{
		DataGridView dataGridView = childCell.DataGridView;
		TCell val = (childCell.OwnerCell as TCell) ?? childCell;
		Rectangle result = childCellBounds;
		int num = Enumerable.Range(val.ColumnIndex, val.ColumnSpan).First((int i) => dataGridView.Columns[i].Visible);
		if (dataGridView.Columns[num].Frozen)
		{
			result.X = dataGridView.GetColumnDisplayRectangle(num, cutOverflow: false).X;
		}
		else
		{
			int num2 = (from i in Enumerable.Range(num, childCell.ColumnIndex - num)
				select dataGridView.Columns[i] into columnItem
				where columnItem.Visible
				select columnItem).Sum((DataGridViewColumn columnItem) => columnItem.Width);
			result.X = ((dataGridView.RightToLeft == RightToLeft.Yes) ? (result.X + num2) : (result.X - num2));
		}
		int num3 = Enumerable.Range(val.RowIndex, val.RowSpan).First((int i) => dataGridView.Rows[i].Visible);
		if (dataGridView.Rows[num3].Frozen)
		{
			result.Y = dataGridView.GetRowDisplayRectangle(num3, cutOverflow: false).Y;
		}
		else
		{
			result.Y -= (from i in Enumerable.Range(num3, childCell.RowIndex - num3)
				select dataGridView.Rows[i] into rowItem
				where rowItem.Visible
				select rowItem).Sum((DataGridViewRow rowItem) => rowItem.Height);
		}
		int num4 = (from columnIndex in Enumerable.Range(val.ColumnIndex, val.ColumnSpan)
			select dataGridView.Columns[columnIndex] into column
			where column.Visible
			select column).Sum((DataGridViewColumn column) => column.Width);
		if (dataGridView.RightToLeft == RightToLeft.Yes)
		{
			result.X = result.Right - num4;
		}
		result.Width = num4;
		result.Height = (from rowIndex in Enumerable.Range(val.RowIndex, val.RowSpan)
			select dataGridView.Rows[rowIndex] into row
			where row.Visible
			select row).Sum((DataGridViewRow row) => row.Height);
		if (singleVerticalBorderAdded && val.InFirstDisplayedColumn())
		{
			result.Width++;
			if (dataGridView.RightToLeft != RightToLeft.Yes)
			{
				if (childCell.ColumnIndex != dataGridView.FirstDisplayedScrollingColumnIndex)
				{
					result.X--;
				}
			}
			else if (childCell.ColumnIndex == dataGridView.FirstDisplayedScrollingColumnIndex)
			{
				result.X--;
			}
		}
		if (singleHorizontalBorderAdded && val.InFirstDisplayedRow())
		{
			result.Height++;
			if (childCell.RowIndex != dataGridView.FirstDisplayedScrollingRowIndex)
			{
				result.Y--;
			}
		}
		return result;
	}

	public static DataGridViewAdvancedBorderStyle AdjustCellBorderStyle<TCell>(TCell cell) where TCell : DataGridViewCell, ISpannedCell
	{
		DataGridViewAdvancedBorderStyle dataGridViewAdvancedBorderStylePlaceholder = new DataGridViewAdvancedBorderStyle();
		DataGridView dataGridView = cell.DataGridView;
		return cell.AdjustCellBorderStyle(dataGridView.AdvancedCellBorderStyle, dataGridViewAdvancedBorderStylePlaceholder, dataGridView.SingleVerticalBorderAdded(), dataGridView.SingleHorizontalBorderAdded(), cell.InFirstDisplayedColumn(), cell.InFirstDisplayedRow());
	}

	public static bool InFirstDisplayedColumn<TCell>(this TCell cell) where TCell : DataGridViewCell, ISpannedCell
	{
		DataGridView dataGridView = cell.DataGridView;
		return dataGridView.FirstDisplayedScrollingColumnIndex >= cell.ColumnIndex && dataGridView.FirstDisplayedScrollingColumnIndex < cell.ColumnIndex + cell.ColumnSpan;
	}

	public static bool InFirstDisplayedRow<TCell>(this TCell cell) where TCell : DataGridViewCell, ISpannedCell
	{
		DataGridView dataGridView = cell.DataGridView;
		return dataGridView.FirstDisplayedScrollingRowIndex >= cell.RowIndex && dataGridView.FirstDisplayedScrollingRowIndex < cell.RowIndex + cell.RowSpan;
	}
}
