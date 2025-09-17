using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace _15cm_abuse
{
    public partial class MainWindow : Window
    {
        private readonly Values values;
        private Distance distance;
        private readonly List<Ellipse> _points = new();
        private Line? _line;
        private int _moveIndex = 0;

        private const double ZoomFactor = 0.1;
        private const double MaxZoom = 3.0;
        private const double MinZoom = 1;

        public MainWindow()
        {
            InitializeComponent();

            values = new();
            distance = new(values);

            values.ScaleChanged += () =>
            {
                distLabel.Text = distance.Dist + " m";
                azimLabel.Text = distance.Azimuth + "°";
            };
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
            => Application.Current.Shutdown();

        private void Img_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            double scale = ImageScale.ScaleX;

            if (e.Delta > 0 && scale < MaxZoom)
                scale += ZoomFactor;
            else if (e.Delta < 0 && scale > MinZoom)
                scale -= ZoomFactor;

            ImageScale.ScaleX = scale;
            ImageScale.ScaleY = scale;
        }

        private void ResetBtn_Click(object sender, RoutedEventArgs e)
        {
            img.Source = null;
            imgCanvas.Children.Clear();
            _points.Clear();
            _line = null;
            values.Points.Clear();
        }

        private void PasteBtn_Click(object sender, RoutedEventArgs e)
        {
            if (Clipboard.GetImage() != null)
                img.Source = Clipboard.GetImage();
        }

        private void SetScale_Click(object sender, RoutedEventArgs e)
            => new ScaleInsert(values).ShowDialog();

        private void Img_MouseLeave(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Arrow;
        }

        private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }

        private void Img_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var pos = e.GetPosition(imgCanvas);

            if (_points.Count < 2)
            {
                var ellipse = new Ellipse
                {
                    Width = 12,
                    Height = 12,
                    Fill = Brushes.OrangeRed,
                    Stroke = Brushes.White,
                    StrokeThickness = 2
                };

                imgCanvas.Children.Add(ellipse);
                Canvas.SetLeft(ellipse, pos.X - ellipse.Width / 2);
                Canvas.SetTop(ellipse, pos.Y - ellipse.Height / 2);

                _points.Add(ellipse);
                values.Points.Add(new() {X = pos.X, Y = pos.Y });

                if (_points.Count == 2)
                {
                    _line = new Line
                    {
                        Stroke = Brushes.Teal,
                        StrokeThickness = 2.5,
                        StrokeDashArray = new DoubleCollection { 4, 2 }
                    };

                    imgCanvas.Children.Add(_line);
                    UpdateLine();
                }
            }
            else
            {
                var point = _points[_moveIndex];

                Canvas.SetLeft(point, pos.X - point.Width / 2);
                Canvas.SetTop(point, pos.Y - point.Height / 2);

                values.Points[_moveIndex] = new() { X = pos.X, Y = pos.Y };

                _moveIndex = (_moveIndex + 1) % 2;
                UpdateLine();
            }

            if (values.Points.Count == 2)
            {
                distLabel.Text = distance.Dist + " m";
                azimLabel.Text = distance.Azimuth + "°";
            }
        }

        private void UpdateLine()
        {
            if (_points.Count < 2 || _line == null)
                return;

            var p1 = GetCenter(_points[0]);
            var p2 = GetCenter(_points[1]);

            _line.X1 = p1.X;
            _line.Y1 = p1.Y;
            _line.X2 = p2.X;
            _line.Y2 = p2.Y;
        }

        private Point GetCenter(Ellipse ellipse)
        {
            double x = Canvas.GetLeft(ellipse) + ellipse.Width / 2;
            double y = Canvas.GetTop(ellipse) + ellipse.Height / 2;

            return new Point { X = x, Y = y};
        }

        private void ImageSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
        }
    }
}
