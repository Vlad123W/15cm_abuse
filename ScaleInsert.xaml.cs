using System.Windows;
using System.Windows.Input;

namespace _15cm_abuse
{
    /// <summary>
    /// Логика взаимодействия для ScaleInsert.xaml
    /// </summary>
    public partial class ScaleInsert : Window
    {
        private readonly Values values;
        public ScaleInsert(Values values)
        {
            InitializeComponent();
            this.values = values;
            Loaded += ScaleInsert_Loaded;
        }

        private void ScaleInsert_Loaded(object sender, RoutedEventArgs e)
        {
            valueTextBox.Text = values.Scale.ToString();
        }

        private void exit_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void main_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void comfirm_Click(object sender, RoutedEventArgs e)
        {
            if(double.TryParse(valueTextBox.Text, out double newScale))
            {
                values.Scale = newScale;
            }
     
            this.DialogResult = true;
        }

        private void scaleInsert_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter && valueTextBox.IsFocused)
            {
                comfirm_Click(sender, e);
            }
        }

        private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }
    }
}
