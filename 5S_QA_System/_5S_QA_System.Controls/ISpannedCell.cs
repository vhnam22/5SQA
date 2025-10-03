using System.Windows.Forms;

namespace _5S_QA_System.Controls;

internal interface ISpannedCell
{
	int ColumnSpan { get; }

	int RowSpan { get; }

	DataGridViewCell OwnerCell { get; }
}
