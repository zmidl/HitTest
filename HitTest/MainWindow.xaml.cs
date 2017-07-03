using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HitTest
{
	/// <summary>
	/// MainWindow.xaml 的交互逻辑
	/// </summary>
	public partial class MainWindow : Window
	{
		private Point FirstPoint;
		Rectangle r;
		public MainWindow()
		{
			InitializeComponent();

		}

		bool isDown;

		private void Canvas_PreviewMouseDown(object sender, MouseButtonEventArgs e)
		{
			r = new Rectangle();
			this.isDown = true;
			this.FirstPoint = e.GetPosition((IInputElement)sender);
			Canvas.SetLeft(r,this.FirstPoint.X);
			Canvas.SetTop(r, this.FirstPoint.Y);
			r.Width = 0;
			r.Height = 0;
			//r.Fill = Brushes.LightGoldenrodYellow;
			r.Stroke = Brushes.Red;
			r.StrokeDashArray = new DoubleCollection() { 2 };
			r.StrokeThickness = 3;
			((Canvas)sender).Children.Add(this.r);
		}

		private void Canvas_PreviewMouseMove(object sender, MouseEventArgs e)
		{

			if (this.isDown)
			{
				var oldPoint = e.GetPosition((IInputElement)sender);
				var width = oldPoint.X - this.FirstPoint.X;
				var height =oldPoint.Y - this.FirstPoint.Y;
				var startPoint = this.FirstPoint;
				var endPoint = oldPoint;
				if (width>0)
				{
					if(height<0)
					{
						startPoint = new Point(this.FirstPoint.X, oldPoint.Y);
						endPoint = new Point(oldPoint.X,this.FirstPoint.Y);
						Canvas.SetTop(r, oldPoint.Y);
					}
				}
				else
				{
					if(height>0)
					{
						startPoint = new Point(oldPoint.X, this.FirstPoint.Y);
						endPoint = new Point(this.FirstPoint.X, oldPoint.Y);
						Canvas.SetLeft(r, oldPoint.X);
					}
					else
					{
						startPoint = new Point(oldPoint.X, oldPoint.Y);
						endPoint = new Point(this.FirstPoint.X, this.FirstPoint.Y);
						Canvas.SetLeft(r, oldPoint.X);
						Canvas.SetTop(r, oldPoint.Y);
					}
				}

				r.Width = Math.Abs(width);
				r.Height = Math.Abs(height);

				//e1.Fill = null;
				//e2.Fill = null;
				//e3.Fill = null;
				//e4.Fill = null;
				//e5.Fill = null;
				//e6.Fill = null;
				//e7.Fill = null;
				//e8.Fill = null;

				VisualTreeHelper.HitTest(c1, null, f =>
				{
					var o = f.VisualHit as Ellipse;
					//if (o != null) o.Fill = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
					if (o != null) o.Fill = new SolidColorBrush(Colors.BurlyWood);
					return HitTestResultBehavior.Continue;
				}, new GeometryHitTestParameters(new RectangleGeometry(new Rect(startPoint, endPoint))));
			}
		}

		private void Canvas_PreviewMouseUp(object sender, MouseButtonEventArgs e)
		{
			this.isDown = false;
			var canvas = ((Canvas)sender);

			for (int i = 0; i < canvas.Children.Count; i++)
			{
				if (canvas.Children[i] is Rectangle) canvas.Children.Remove((Rectangle)canvas.Children[i]);
			}
			canvas.Children.Remove(r);
		}
	}
}
