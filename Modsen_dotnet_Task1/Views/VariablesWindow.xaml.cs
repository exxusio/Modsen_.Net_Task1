using ExpressionEvaluator.Functions;
using ExpressionEvaluator.Variables;
using Modsen_dotnet_Task1.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;

namespace Modsen_dotnet_Task1.Views
{
    /// <summary>
    /// Логика взаимодействия для VariablesWindow.xaml
    /// </summary>
    public partial class VariablesWindow : Window
    {
        CalculatorViewModel calculatorViewModel;
        private List<Variable> _variables;
        public ICommand DeleteCommand { get; set; }
        public VariablesWindow(Window owner, CalculatorViewModel calculatorViewModel)
        {
            InitializeComponent();
            Owner = owner;
            this.calculatorViewModel = calculatorViewModel;
            _variables = new List<Variable>(calculatorViewModel.GetAllVariables());
            FunctionList.ItemsSource = _variables;

            // Инициализация команды удаления
            DeleteCommand = new RelayCommand<Variable>(DeleteVariable);
            DataContext = this;
        }

        private void ToolBarBorder_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void FunctionToggleButton_Click(object sender, RoutedEventArgs e)
        {
            Owner.Left = this.Left;
            Owner.Top = this.Top;
            FunctionsWindow window = new FunctionsWindow(Owner, calculatorViewModel);
            window.Show();
            this.Close();
        }

        private void HistoryButton_Click(object sender, RoutedEventArgs e)
        {
            Owner.Left = this.Left;
            Owner.Top = this.Top;
            Owner.Show();
            this.Close();
        }

        private void CloseAppButton_Click(object sender, RoutedEventArgs e)
        {
            Owner.Close();
            this.Close();
        }

        private void AddFunctionButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(VariablesInputField.Text))
            {
                string input = VariablesInputField.Text.Trim(); // Удаляем лишние пробелы по краям

                // Проверяем, что строка содержит только один символ "=" и разделяем строку на левую и правую части
                int equalsIndex = input.IndexOf('=');
                if (equalsIndex != -1 && input.IndexOf('=', equalsIndex + 1) == -1)
                {
                    string variableName = input.Substring(0, equalsIndex).Trim();
                    string variableDefinition = input.Substring(equalsIndex + 1).Trim();

                    // Проверяем, что имя переменной состоит только из латинских букв и цифр
                    if (IsLatinAlphanumeric(variableName))
                    {
                        // Формируем строку для передачи в метод AddVariable
                        string formattedVariable = $"{variableName}={variableDefinition}";

                        // Вызываем метод AddVariable
                        try
                        {
                            calculatorViewModel.AddVariable(formattedVariable);
                            Refresh();
                            VariablesInputField.Text = "";
                        }
                        catch
                        {
                            Owner.Left = this.Left;
                            Owner.Top = this.Top;
                            MessageWindow window = new MessageWindow(Owner, "The expression format is incorrect.");
                            window.Show();
                        }
                    }
                    else
                    {
                        Owner.Left = this.Left;
                        Owner.Top = this.Top;
                        MessageWindow window = new MessageWindow(Owner, "The name of the variable should consist only of Latin letters and numbers.");
                        window.Show();
                    }
                }
                else
                {
                    Owner.Left = this.Left;
                    Owner.Top = this.Top;
                    MessageWindow window = new MessageWindow(Owner, "The variable must contain one name and one equal sign for definition.");
                    window.Show();
                }
            }
        }

        private bool IsLatinAlphanumeric(string input)
        {
            return input.All(c => char.IsLetterOrDigit(c) && c < 128);
        }



        private void Refresh()
        {
            _variables = new List<Variable>(calculatorViewModel.GetAllVariables());
            FunctionList.ItemsSource = _variables;
            FunctionList.Items.Refresh();
        }

        private void DeleteVariable(Variable variable)
        {
            calculatorViewModel.DeleteVariable(variable);
            Refresh();
        }
    }
}
