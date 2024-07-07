using Modsen_dotnet_Task1.Models;

namespace Tests
{
    public class ExpressionEvaluatorFunctionsTests
    {
        [Fact]
        public void TestNew_Function()
        {
            Calculator calculator = new Calculator();
            calculator.Function.AddFunction("sum(a,b)=a+b");
            var res = calculator.Evaluator.Evaluate("sum(1,2)");
            Assert.Equal(3d, res);
        }

        [Fact]
        public void TestNew_Function_With_Variable()
        {
            Calculator calculator = new Calculator();
            calculator.Function.AddFunction("sum(a,b)=a+b");
            calculator.Variable.SetVariable("c=2");
            var res = calculator.Evaluator.Evaluate("sum(1,c)");
            Assert.Equal(3d, res);
        }

        [Fact]
        public void TestFunction_Is_Variable()
        {
            Calculator calculator = new Calculator();
            calculator.Function.AddFunction("f(a)=a");
            var res = calculator.Evaluator.Evaluate("f(2)");
            Assert.Equal(2d, res);
        }

        [Fact]
        public void TestFunction_In_Function()
        {
            Calculator calculator = new Calculator();
            calculator.Function.AddFunction("sum(a,b)=a+b");
            calculator.Function.AddFunction("f(a)=2*a");
            var res = calculator.Evaluator.Evaluate("sum(1,f(2))");
            Assert.Equal(5d, res);
        }


        [Fact]
        public void TestFunction_Without_Variables()
        {
            Calculator calculator = new Calculator();
            calculator.Function.AddFunction("f()=4");
            var res = calculator.Evaluator.Evaluate("f()");
            Assert.Equal(4d, res);
        }

        [Fact]
        public void TestFunction_Incorrect_Input()
        {
            Calculator calculator = new Calculator();
            Assert.Throws<FormatException>(() => calculator.Function.AddFunction("f4"));
        }
    }
}