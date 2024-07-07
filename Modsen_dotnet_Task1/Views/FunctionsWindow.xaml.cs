using ExpressionEvaluator.Functions;
using Modsen_dotnet_Task1.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Modsen_dotnet_Task1.Views
{
    /// <summary>
    /// Логика взаимодействия для FunctionsWindow.xaml
    /// </summary>
    public partial class FunctionsWindow : Window
    {
        private CalculatorViewModel calculactorViewModel;
        private List<Function> _functions;
        public ICommand DeleteCommand { get; set; }

        public FunctionsWindow(Window owner, CalculatorViewModel calculatorViewModel)
        {
            InitializeComponent();
            Owner = owner;
            this.calculactorViewModel = calculatorViewModel;
            _functions = new List<Function>(calculactorViewModel.GetAllFunctions());
            FunctionList.ItemsSource = _functions;
            DeleteCommand = new RelayCommand<Function>(DeleteFunction);
            DataContext = this;
        }

        private void ToolBarBorder_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void VariablesToggleButton_Click(object sender, RoutedEventArgs e)
        {
            Owner.Left = this.Left;
            Owner.Top = this.Top;
            VariablesWindow window = new VariablesWindow(Owner, calculactorViewModel);
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
            if (!string.IsNullOrEmpty(FunctionInputField.Text))
            {
                string input = FunctionInputField.Text.Trim(); // Удаляем лишние пробелы по краям

                // Проверяем, что строка содержит только один символ "=" и разделяем строку на левую и правую части
                int equalsIndex = input.IndexOf('=');
                if (equalsIndex != -1 && input.IndexOf('=', equalsIndex + 1) == -1)
                {
                    string functionName = input.Substring(0, equalsIndex).Trim();
                    string functionDefinition = input.Substring(equalsIndex + 1).Trim();

                    // Проверяем правильность формата имени функции и параметров
                    int openBracketIndex = functionName.IndexOf('(');
                    int closeBracketIndex = functionName.IndexOf(')');
                    if ((openBracketIndex == -1 && closeBracketIndex == -1) ||
                        (openBracketIndex != -1 && closeBracketIndex == functionName.Length - 1 && closeBracketIndex > openBracketIndex))
                    {
                        HashSet<string> uniqueParams = new HashSet<string>();

                        // Проверяем, если есть параметры, то они корректны и уникальны
                        if (openBracketIndex != -1)
                        {
                            string parametersSubstring = functionName.Substring(openBracketIndex + 1, closeBracketIndex - openBracketIndex - 1).Trim();
                            string[] parameters = parametersSubstring.Split(',');

                            if(parametersSubstring != "")
                            {
                                foreach (var param in parameters)
                                {
                                    string trimmedParam = param.Trim();
                                    if (string.IsNullOrEmpty(trimmedParam) || !IsLatinAlphanumeric(trimmedParam))
                                    {
                                        ShowMessage("The function parameters must consist of Latin letters and numbers and be correctly separated by commas.");
                                        return;
                                    }
                                    if (!uniqueParams.Add(trimmedParam))
                                    {
                                        ShowMessage("The function parameters must be unique.");
                                        return;
                                    }
                                }
                            }

                            functionName = functionName.Substring(0, openBracketIndex).Trim();
                        }

                        // Проверяем, что имя функции состоит только из латинских букв и цифр
                        string functionNameWithoutParams = openBracketIndex == -1 ? functionName : functionName.Substring(0, openBracketIndex);
                        if (!IsLatinAlphanumeric(functionNameWithoutParams))
                        {
                            ShowMessage("The name of the function should consist only of Latin letters and numbers.");
                            return;
                        }

                        // Проверяем, что выражение функции использует только параметры, которые есть у этой функции
                        if (!UsesOnlyDefinedParameters(functionDefinition, uniqueParams))
                        {
                            ShowMessage("A function expression must use only the parameters declared in that function.");
                            return;
                        }

                        // Формируем строку для передачи в метод AddFunction
                        string formattedFunction = $"{functionName}({string.Join(",", uniqueParams)})={functionDefinition}";

                        // Вызываем метод AddFunction
                        try
                        {
                            calculactorViewModel.AddFunction(formattedFunction);
                            Refresh();
                            FunctionInputField.Text = "";
                        }
                        catch
                        {
                            ShowMessage("The input format is incorrect.");
                        }
                    }
                    else
                    {
                        ShowMessage("The function must contain the correct name and parameters in parentheses, if any.");
                    }
                }
                else
                {
                    ShowMessage("The function must start with the name and contain one equal sign for the definition.");
                }
            }
        }

        private bool IsLatinAlphanumeric(string input)
        {
            return input.All(c => char.IsLetterOrDigit(c) && c < 128);
        }

        private void ShowMessage(string message)
        {
            Owner.Left = this.Left;
            Owner.Top = this.Top;
            MessageWindow window = new MessageWindow(Owner, message);
            window.Show();
        }

        private bool UsesOnlyDefinedParameters(string functionDefinition, HashSet<string> parameters)
        {
            // Разбиваем выражение на части и проверяем, что все идентификаторы существуют в списке параметров
            string[] tokens = functionDefinition.Split(new char[] { ' ', '+', '-', '*', '/', '(', ')', '^' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var token in tokens)
            {
                if (char.IsLetter(token[0]) && !parameters.Contains(token))
                {
                    return false; // Найден не объявленный параметр
                }
            }
            return true;
        }





        private void Refresh()
        {
            _functions = new List<Function>(calculactorViewModel.GetAllFunctions());
            FunctionList.ItemsSource = _functions;
            FunctionList.Items.Refresh();
        }

        private void DeleteFunction(Function function)
        {
            calculactorViewModel.DeleteFunction(function);
            Refresh();
        }
    }
}
