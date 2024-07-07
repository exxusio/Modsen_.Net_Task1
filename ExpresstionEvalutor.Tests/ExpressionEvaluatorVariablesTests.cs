using Modsen_dotnet_Task1.Models;

namespace Tests
{
    public class ExpressionEvaluatorVariablesTests
    {
        [Fact]
        public void TestVar_Set_1()
        {
            Calculator calculator = new Calculator();
            calculator.Variable.SetVariable("a=2");
            var res = calculator.Evaluator.Evaluate("a");
            Assert.Equal(2d, res);
        }

        [Fact]
        public void TestVar_Set_2()
        {
            Calculator calculator = new Calculator();
            calculator.Variable.SetVariable("jfkroedn=2");
            var res = calculator.Evaluator.Evaluate("jfkroedn+jfkroedn");
            Assert.Equal(4d, res);
        }

        [Fact]
        public void TestVar_Set_3()
        {
            Calculator calculator = new Calculator();
            calculator.Variable.SetVariable("ara", "2");
            var res = calculator.Evaluator.Evaluate("ara");
            Assert.Equal(2d, res);
        }

        [Fact]
        public void TestVar_Set_4()
        {
            Calculator calculator = new Calculator();
            calculator.Variable.SetVariable("ara", "2");
            calculator.Variable.SetVariable("a", "ara");
            var res = calculator.Evaluator.Evaluate("a");
            Assert.Equal(2d, res);
        }

        [Fact]
        public void TestVar_Set_5()
        {
            Calculator calculator = new Calculator();
            Assert.Throws<KeyNotFoundException>(() => calculator.Variable.SetVariable("ara", "a"));
        }

        [Fact]
        public void TestVar_Set_6()
        {
            Calculator calculator = new Calculator();
            Assert.Throws<FormatException>(() => calculator.Variable.SetVariable("ara_).", "2"));
        }

        [Fact]
        public void TestVar_Set_7()
        {
            Calculator calculator = new Calculator();
            Assert.Throws<FormatException>(() => calculator.Variable.SetVariable("_", "5"));
        }

        [Fact]
        public void TestNotFounded_Variable()
        {
            Calculator calculator = new Calculator();
            calculator.Variable.SetVariable("ara", "2");
            Assert.Throws<KeyNotFoundException>(() => calculator.Evaluator.Evaluate("a"));
        }
    }
}
