using System.Windows.Forms;

namespace _5S_QA_Client.Controls;

internal static class DataGridViewHelper
{
	public static bool SingleHorizontalBorderAdded(this DataGridView dataGridView)
	{
		return !dataGridView.ColumnHeadersVisible && (dataGridView.AdvancedCellBorderStyle.All == DataGridViewAdvancedCellBorderStyle.Single || dataGridView.CellBorderStyle == DataGridViewCellBorderStyle.SingleHorizontal);
	}

	public static bool SingleVerticalBorderAdded(this DataGridView dataGridView)
	{
		return !dataGridView.RowHeadersVisible && (dataGridView.AdvancedCellBorderStyle.All == DataGridViewAdvancedCellBorderStyle.Single || dataGridView.CellBorderStyle == DataGridViewCellBorderStyle.SingleVertical);
	}
}
