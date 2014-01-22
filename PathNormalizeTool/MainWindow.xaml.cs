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

namespace PathNormalizeTool
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// ViewModel
        /// </summary>
        public MainViewModel ViewModel
        {
            get { return this.DataContext as MainViewModel; }
        }

        /// <summary>
        /// テキストフォーカスイベントハンドラ
        /// </summary>
        /// <param name="sender">イベント発行者</param>
        /// <param name="e">イベント引数</param>
        private void OnTextBoxGotFocus(object sender, RoutedEventArgs e)
        {
            var textbox = sender as TextBox;
            if (textbox != null)
            {
                Action act = textbox.SelectAll;
                Dispatcher.BeginInvoke(act);
            }
        }

        /// <summary>
        /// キー押下イベントハンドラ
        /// </summary>
        /// <param name="sender">イベント発行者</param>
        /// <param name="e">イベント引数</param>
        private void OnTextBoxKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = false;
            
            var textbox = sender as TextBox;
            double value = 0;
            if (textbox != null && double.TryParse(textbox.Text, out value))
            {
                var expression = BindingOperations.GetBindingExpression(textbox, TextBox.TextProperty);

                if ((Keyboard.Modifiers == ModifierKeys.None) && (e.Key == Key.Up))
                {
                    textbox.Text = (value + 1).ToString();
                    expression.UpdateSource();
                    e.Handled = true;
                }
                if ((Keyboard.Modifiers == ModifierKeys.None) && (e.Key == Key.Down))
                {
                    textbox.Text = (value - 1).ToString();
                    expression.UpdateSource();
                    e.Handled = true;
                }
                if ((Keyboard.Modifiers == ModifierKeys.None) && (e.Key == Key.Enter))
                {
                    this.ViewModel.UpdatePathData();
                    e.Handled = true;
                }
            }
        }

        /// <summary>
        /// コピーボタン押下イベントハンドラ
        /// </summary>
        /// <param name="sender">イベント発行者</param>
        /// <param name="e">イベント引数</param>
        private void OnCopyButtonClick(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(this.ViewModel.PathData);
        }
    }
}
