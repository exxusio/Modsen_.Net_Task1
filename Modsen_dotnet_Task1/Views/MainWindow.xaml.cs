using Modsen_dotnet_Task1.ViewModels;
using Modsen_dotnet_Task1.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Modsen_dotnet_Task1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        CalculatorViewModel calculatorViewModel;
        public MainWindow()
        {
            InitializeComponent();
            calculatorViewModel = new CalculatorViewModel();
            CalculationResult.Text = "0";
        }

        private void ToolBarBorder_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void HistoryButton_Click(object sender, RoutedEventArgs e)
        {
            FunctionsWindow window = new FunctionsWindow(this, calculatorViewModel);
            window.Show();
            this.Hide();
        }

        private void CloseAppButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ZeroButton_Click(object sender, RoutedEventArgs e)
        {
            ExpressionInputField.Text += "0";
        }

        private void OneButton_Click(object sender, RoutedEventArgs e)
        {
            ExpressionInputField.Text += "1";
        }

        private void TwoButton_Click(object sender, RoutedEventArgs e)
        {
            ExpressionInputField.Text += "2";
        }

        private void ThreeButton_Click(object sender, RoutedEventArgs e)
        {
            ExpressionInputField.Text += "3";
        }

        private void FourButton_Click(object sender, RoutedEventArgs e)
        {
            ExpressionInputField.Text += "4";
        }

        private void FiveButton_Click(object sender, RoutedEventArgs e)
        {
            ExpressionInputField.Text += "5";
        }

        private void SixButton_Click(object sender, RoutedEventArgs e)
        {
            ExpressionInputField.Text += "6";
        }

        private void SevenButton_Click(object sender, RoutedEventArgs e)
        {
            ExpressionInputField.Text += "7";
        }

        private void EightButton_Click(object sender, RoutedEventArgs e)
        {
            ExpressionInputField.Text += "8";
        }

        private void NineButton_Click(object sender, RoutedEventArgs e)
        {
            ExpressionInputField.Text += "9";
        }

        private void CommaButton_Click(object sender, RoutedEventArgs e)
        {
            EnterComma(ExpressionInputField.Text);
        }

        private void LeftParentheseButton_Click(object sender, RoutedEventArgs e)
        {
            CanOpenBracket(ExpressionInputField.Text);
        }

        private void RightParentheseButton_Click(object sender, RoutedEventArgs e)
        {
            CanCloseBracket(ExpressionInputField.Text);
        }

        private void InverSignButton_Click(object sender, RoutedEventArgs e)
        {
            if(!string.IsNullOrEmpty(ExpressionInputField.Text) && ( !char.IsDigit(ExpressionInputField.Text[ExpressionInputField.Text.Length-1]) && ExpressionInputField.Text[ExpressionInputField.Text.Length - 1] != ')') )
            {
                ExpressionInputField.Text = ExpressionInputField.Text.Remove(ExpressionInputField.Text.Length - 1);
            }
            else if (!string.IsNullOrEmpty(ExpressionInputField.Text))
            {
                ExpressionInputField.Text = $"-({ExpressionInputField.Text})";
            }
        }

        private void PlusButton_Click(object sender, RoutedEventArgs e)
        {
            EnterOperator(ExpressionInputField.Text, '+');
        }

        private void MinusButton_Click(object sender, RoutedEventArgs e)
        {
            EnterOperator(ExpressionInputField.Text, '-');
        }

        private void MultiplicationButton_Click(object sender, RoutedEventArgs e)
        {
            EnterOperator(ExpressionInputField.Text, '*');
        }

        private void DivisionButton_Click(object sender, RoutedEventArgs e)
        {
            EnterOperator(ExpressionInputField.Text, '/');
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            List<string> signs = new List<string>() { "/", "*", "+", "-"};
            while (ExpressionInputField.Text.Length > 0 && !signs.Contains(ExpressionInputField.Text[ExpressionInputField.Text.Length-1].ToString()))
            {
                ExpressionInputField.Text = ExpressionInputField.Text.Remove(ExpressionInputField.Text.Length - 1);
                if(ExpressionInputField.Text.Length == 0)
                {
                    CalculationResult.Text = "0";
                }
            }
        }

        private void BackspaceButton_Click(object sender, RoutedEventArgs e)
        {
            if (ExpressionInputField.Text.Length > 0)
            {
                ExpressionInputField.Text = ExpressionInputField.Text.Remove(ExpressionInputField.Text.Length - 1);
                if (ExpressionInputField.Text == "")
                {
                    CalculationResult.Text = "0";
                }
            }
            else if(ExpressionInputField.Text == "")
            {
                CalculationResult.Text = "0";
            }
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            CalculationResult.Text = "0";

            ExpressionInputField.Text = "";
        }

        private void EqualityButton_Click(object sender, RoutedEventArgs e)
        {
            CanSolve(ExpressionInputField.Text.ToString());
            ExpressionInputField.Text = CalculationResult.Text;
        }

        private void ExpressionInputField_TextChanged(object sender, TextChangedEventArgs e)
        {
            CanSolve(ExpressionInputField.Text.ToString());
        }

        private void Solve()
        {
            calculatorViewModel.InputExpression = ExpressionInputField.Text;
            calculatorViewModel.Calculate();
            CalculationResult.Text = calculatorViewModel.Result;
        }

        private void CanSolve(string inputExpression)
        {
            // Список операторов и символов, после которых вызывается Solve
            List<string> triggerSigns = new List<string>() { "/", "*", "-", "+", ")" };

            if (inputExpression.Length > 0)
            {
                char lastChar = inputExpression[inputExpression.Length - 1];

                if (char.IsDigit(lastChar) || lastChar == ')' || char.IsLetter(lastChar))
                {
                    Solve();
                }
            }
        }

        private void CanOpenBracket(string inputExpression)
        {
            if (inputExpression.Length == 0 || !Char.IsDigit(inputExpression[inputExpression.Length - 1]))
            {
                ExpressionInputField.Text += "(";
            }
        }

        private void CanCloseBracket(string inputExpression)
        {
            List<string> signs = new List<string>() {"/", "*", "-", "+"};
            if(inputExpression.Length > 0 && !signs.Contains(inputExpression[inputExpression.Length - 1].ToString()))
            {
                if (inputExpression.Count(element => element == '(') > inputExpression.Count(element => element == ')'))
                {
                    if (inputExpression[inputExpression.Length-1] == '(')
                    {
                        inputExpression = inputExpression.Remove(inputExpression.Length-1);
                        ExpressionInputField.Text = inputExpression;
                    }
                    else
                    {
                        ExpressionInputField.Text += ")";
                    }
                }
            }
        }

        private void EnterOperator(string inputExpression, char sign)
        {
            List<char> signs = new List<char>() { '/', '*', '-', '+'};

            if(inputExpression.Length > 0 && signs.Contains(inputExpression[inputExpression.Length - 1]))
            {
                inputExpression = inputExpression.Remove(inputExpression.Length - 1);
                inputExpression += sign;
            }
            else if(inputExpression.Length > 0 && inputExpression[inputExpression.Length - 1].Equals('('))
            {
                if (sign.Equals('-'))
                {
                    inputExpression += sign;
                }
                else
                {
                    inputExpression = inputExpression.Remove(inputExpression.Length - 2);
                    inputExpression += sign;
                }
            }
            else
            {
                inputExpression += sign;
            }
            
            ExpressionInputField.Text = inputExpression;

        }

        private void EnterComma(string inputExpression)
        {
            ;
            
            int lastOperatorIndex = inputExpression.LastIndexOfAny(new char[] { '+', '-', '*', '/' });
            string lastNumber = (lastOperatorIndex == -1) ? inputExpression : inputExpression.Substring(lastOperatorIndex + 1);
            if (!string.IsNullOrEmpty(lastNumber) && !lastNumber.Contains('.'))
            {
                ExpressionInputField.Text += '.';
            }
        }
    }
}