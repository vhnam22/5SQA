using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Windows.Forms;

namespace _5S_QA_Client;

public class TransparentLabel : Label
{
	private int opacity;

	public Color transparentBackColor;

	public int Opacity
	{
		get
		{
			return opacity;
		}
		set
		{
			if (value >= 0 && value <= 255)
			{
				opacity = value;
			}
			Invalidate();
		}
	}

	public Color TransparentBackColor
	{
		get
		{
			return transparentBackColor;
		}
		set
		{
			transparentBackColor = value;
			Invalidate();
		}
	}

	[Browsable(false)]
	public override Color BackColor
	{
		get
		{
			return Color.Transparent;
		}
		set
		{
			base.BackColor = Color.Transparent;
		}
	}

	public TransparentLabel()
	{
		transparentBackColor = Color.Transparent;
		opacity = 50;
		BackColor = Color.Transparent;
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		if (base.Parent == null)
		{
			return;
		}
		Bitmap bmp = new Bitmap(base.Parent.Width, base.Parent.Height);
		try
		{
			(from Control c in base.Parent.Controls
				where base.Parent.Controls.GetChildIndex(c) > base.Parent.Controls.GetChildIndex(this)
				where c.Bounds.IntersectsWith(base.Bounds)
				orderby base.Parent.Controls.GetChildIndex(c) descending
				select c).ToList().ForEach(delegate(Control c)
			{
				c.DrawToBitmap(bmp, c.Bounds);
			});
			e.Graphics.DrawImage(bmp, -base.Left, -base.Top);
			using (SolidBrush brush = new SolidBrush(Color.FromArgb(Opacity, TransparentBackColor)))
			{
				e.Graphics.FillRectangle(brush, base.ClientRectangle);
			}
			e.Graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
			TextRenderer.DrawText(e.Graphics, Text, Font, base.ClientRectangle, ForeColor, Color.Transparent);
		}
		finally
		{
			if (bmp != null)
			{
				((IDisposable)bmp).Dispose();
			}
		}
	}
}
