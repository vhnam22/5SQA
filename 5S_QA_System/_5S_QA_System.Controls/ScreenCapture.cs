using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace _5S_QA_System.Controls;

public class ScreenCapture
{
	private struct Rect
	{
		public int Left;

		public int Top;

		public int Right;

		public int Bottom;
	}

	public Point CursorPosition;

	[DllImport("user32.dll")]
	private static extern IntPtr GetWindowRect(IntPtr hWnd, ref Rect rect);

	public Bitmap Capture(Control c)
	{
		return Capture(c.Handle);
	}

	public Bitmap Capture(IntPtr handle)
	{
		Rect rect = default(Rect);
		GetWindowRect(handle, ref rect);
		Rectangle rectangle = new Rectangle(rect.Left, rect.Top, rect.Right - rect.Left, rect.Bottom - rect.Top);
		CursorPosition = new Point(Cursor.Position.X - rect.Left, Cursor.Position.Y - rect.Top);
		Bitmap bitmap = new Bitmap(rectangle.Width, rectangle.Height);
		using (Graphics graphics = Graphics.FromImage(bitmap))
		{
			graphics.CopyFromScreen(new Point(rectangle.Left, rectangle.Top), Point.Empty, rectangle.Size);
		}
		return bitmap;
	}
}
