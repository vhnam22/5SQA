using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WpfUserImage;

public partial class UcWpf : UserControl, IComponentConnector
{
	private Point firstPoint = default(Point);

	private Matrix matrix;

	public double Scale;

	public Point Point;

	public UcWpf(BitmapImage img)
	{
		InitializeComponent();
		imgSource.Source = img;
		matrix = imgSource.RenderTransform.Value;
		Scale = 1.0;
		base.Cursor = Cursors.Cross;
		Init();
	}

	private void Init()
	{
		imgSource.MouseLeftButtonDown += delegate(object ss, MouseButtonEventArgs ee)
		{
			firstPoint = ee.GetPosition(this);
			imgSource.CaptureMouse();
			base.Cursor = Cursors.Hand;
		};
		imgSource.MouseWheel += delegate(object ss, MouseWheelEventArgs ee)
		{
			Matrix value = imgSource.RenderTransform.Value;
			Point = ee.GetPosition(imgSource);
			if (ee.Delta > 0)
			{
				Scale = 1.2 * Scale;
				value.ScaleAtPrepend(1.2, 1.2, Point.X, Point.Y);
			}
			else
			{
				Scale /= 1.2;
				value.ScaleAtPrepend(5.0 / 6.0, 5.0 / 6.0, Point.X, Point.Y);
			}
			MatrixTransform renderTransform = new MatrixTransform(value);
			imgSource.RenderTransform = renderTransform;
		};
		imgSource.MouseMove += delegate(object ss, MouseEventArgs ee)
		{
			if (ee.LeftButton == MouseButtonState.Pressed)
			{
				Point position = ee.GetPosition(this);
				Point point = new Point(firstPoint.X - position.X, firstPoint.Y - position.Y);
				Canvas.SetLeft(imgSource, Canvas.GetLeft(imgSource) - point.X);
				Canvas.SetTop(imgSource, Canvas.GetTop(imgSource) - point.Y);
				firstPoint = position;
			}
		};
		imgSource.MouseUp += delegate(object ss, MouseButtonEventArgs ee)
		{
			imgSource.ReleaseMouseCapture();
			base.Cursor = Cursors.Cross;
			Point = ee.GetPosition(imgSource);
		};
	}

	public void Move_ImageAtPoint(Point pCenter, Point pMove, double scale)
	{
		Matrix matrix = this.matrix;
		matrix.ScaleAtPrepend(scale, scale, pMove.X, pMove.Y);
		MatrixTransform renderTransform = new MatrixTransform(matrix);
		imgSource.RenderTransform = renderTransform;
		double length = pCenter.X - pMove.X;
		double length2 = pCenter.Y - pMove.Y;
		Canvas.SetLeft(imgSource, length);
		Canvas.SetTop(imgSource, length2);
	}
}
