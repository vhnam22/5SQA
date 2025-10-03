using System.Windows.Forms;

namespace _5S_QA_Client.Controls;

public class DataGridViewTextBoxColumnEx : DataGridViewColumn
{
	public DataGridViewTextBoxColumnEx()
		: base(new DataGridViewTextBoxCellEx())
	{
	}
}
