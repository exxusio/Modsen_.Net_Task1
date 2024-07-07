using ExpressionEvaluator.Functions;
using ExpressionEvaluator.Variables;
using Modsen_dotnet_Task1.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Modsen_dotnet_Task1.ViewModels
{
    public class CalculatorViewModel : INotifyPropertyChanged
    {
        private string inputExpression;
        private string result;
        private Calculator calculator;

        public event PropertyChangedEventHandler PropertyChanged;

        public string InputExpression
        {
            get { return inputExpression; }
            set
            {
                inputExpression = value;
                OnPropertyChanged("InputExpression");
            }
        }

        public string Result
        {
            get { return result; }
            set
            {
                result = value;
                OnPropertyChanged("Result");
            }
        }


        public CalculatorViewModel()
        {
            calculator = new Calculator();
        }
        public void Calculate()
        {
            try
            {
                double calculationResult = calculator.Evaluator.Evaluate(inputExpression);
                Result = calculationResult.ToString().Replace(',','.');
            }
            catch (Exception ex)
            {
                result = "Error";
            }
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public void AddFunction(string functionName)
        {
            calculator.Function.AddFunction(functionName);
        }

        public void AddVariable(string variable)
        {
            calculator.Variable.SetVariable(variable);
        }

        public List<Function> GetAllFunctions()
        {
            return calculator.Function.GetAllFunctions();
        }

        public IEnumerable<Variable> GetAllVariables()
        {
            return calculator.Variable.GetAllVariables();
        }

        public void DeleteFunction(Function function)
        {
            calculator.Function.DelFunction(function.Name, function.Parameters.Count);
        }

        public void DeleteVariable(Variable variable)
        {
            calculator.Variable.DelVariable(variable.Name);
        }
    }
}
