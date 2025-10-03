using System;
using System.Drawing;
using System.Windows.Forms;

namespace _5S_QA_Client.Controls;

internal class ControlResize
{
	public enum Direction
	{
		Horizontal,
		Vertical
	}

	public static void Init(Control resizer, Control controlToResize, Direction direction, Cursor cursor)
	{
		bool dragging = false;
		Point dragStart = Point.Empty;
		resizer.MouseHover += delegate
		{
			resizer.Cursor = cursor;
		};
		resizer.MouseDown += delegate(object sender, MouseEventArgs e)
		{
			dragging = true;
			dragStart = new Point(e.X, e.Y);
			resizer.Capture = true;
		};
		resizer.MouseUp += delegate
		{
			dragging = false;
			resizer.Capture = false;
		};
		int minBound;
		int maxBound;
		resizer.MouseMove += delegate(object sender, MouseEventArgs e)
		{
			if (dragging)
			{
				if (direction == Direction.Vertical)
				{
					minBound = resizer.Height;
					maxBound = controlToResize.Parent.Height - 5;
					controlToResize.Height = Math.Min(maxBound, Math.Max(minBound, controlToResize.Height - (e.Y - dragStart.Y)));
				}
				if (direction == Direction.Horizontal)
				{
					minBound = resizer.Width;
					maxBound = controlToResize.Parent.Width;
					controlToResize.Width = Math.Min(maxBound, Math.Max(minBound, controlToResize.Width - (e.X - dragStart.X)));
				}
			}
		};
	}

	public static void Init(Control resizer, Control controlToResize)
	{
		Cursor cursor = Cursors.SizeWE;
		bool dragging = false;
		Point dragStart = Point.Empty;
		resizer.MouseHover += delegate
		{
			resizer.Cursor = cursor;
		};
		resizer.MouseDown += delegate(object sender, MouseEventArgs e)
		{
			dragging = true;
			dragStart = new Point(e.X, e.Y);
			resizer.Capture = true;
		};
		resizer.MouseUp += delegate
		{
			dragging = false;
			resizer.Capture = false;
		};
		int minBound;
		int maxBound;
		resizer.MouseMove += delegate(object sender, MouseEventArgs e)
		{
			if (dragging)
			{
				minBound = resizer.Width;
				maxBound = controlToResize.Parent.Width;
				controlToResize.Width = Math.Min(maxBound, Math.Max(minBound, controlToResize.Width + (e.X - dragStart.X)));
			}
		};
	}
}
