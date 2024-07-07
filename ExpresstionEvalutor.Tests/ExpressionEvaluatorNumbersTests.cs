using Modsen_dotnet_Task1.Models;

namespace Tests
{
    public class ExpressionEvaluatorNumbersTests
    {
        [Fact]
        public void TestJust_Number()
        {
            Calculator calculator = new Calculator();
            var res = calculator.Evaluator.Evaluate("2");
            Assert.Equal(2d, res);
        }

        [Fact]
        public void TestSum()
        {
            Calculator calculator = new Calculator();
            var res = calculator.Evaluator.Evaluate("2+2");
            Assert.Equal(4d, res);
        }

        [Fact]
        public void TestSubtraction()
        {
            Calculator calculator = new Calculator();
            var res = calculator.Evaluator.Evaluate("5-3");
            Assert.Equal(2d, res);
        }

        [Fact]
        public void TestDivision()
        {
            Calculator calculator = new Calculator();
            var res = calculator.Evaluator.Evaluate("8/2");
            Assert.Equal(4d, res);
        }

        [Fact]
        public void TestMultiplication()
        {
            Calculator calculator = new Calculator();
            var res = calculator.Evaluator.Evaluate("8*2");
            Assert.Equal(16d, res);
        }

        [Fact]
        public void TestDivisionByZero()
        {
            Calculator calculator = new Calculator();
            Assert.Throws<DivideByZeroException>(() => calculator.Evaluator.Evaluate("8/0"));
        }

        [Fact]
        public void TestBrackets()
        {
            Calculator calculator = new Calculator();
            var res = calculator.Evaluator.Evaluate("2*(3+4)");
            Assert.Equal(14d, res);
        }

        [Fact]
        public void TestWrongBracket()
        {
            Calculator calculator = new Calculator();
            var res = calculator.Evaluator.Evaluate("((2+2)-4+(2+3");
            Assert.Equal(5d, res);
        }

        [Fact]
        public void TestSign_1()
        {
            Calculator calculator = new Calculator();
            var res = calculator.Evaluator.Evaluate("5-9");
            Assert.Equal(-4d, res);
        }

        [Fact]
        public void TestSign_2()
        {
            Calculator calculator = new Calculator();
            var res = calculator.Evaluator.Evaluate("-5+15");
            Assert.Equal(10d, res);
        }

        [Fact]
        public void TestSign_3()
        {
            Calculator calculator = new Calculator();
            var res = calculator.Evaluator.Evaluate("-5-5");
            Assert.Equal(-10d, res);
        }

        [Fact]
        public void TestSign_4()
        {
            Calculator calculator = new Calculator();
            var res = calculator.Evaluator.Evaluate("-(-5)");
            Assert.Equal(5d, res);
        }

        [Fact]
        public void TestSign_5()
        {
            Calculator calculator = new Calculator();
            var res = calculator.Evaluator.Evaluate("--2");
            Assert.Equal(2d, res);
        }

        [Fact]
        public void TestWrongInput_1()
        {
            Calculator calculator = new Calculator();
            Assert.Throws<ArgumentException>(() => calculator.Evaluator.Evaluate("22/*1-"));
        }

        [Fact]
        public void HandleUnaryMinus()
        {
            Calculator calculator = new Calculator();
            var res = calculator.Evaluator.Evaluate("-(-(-(-2)))");
            Assert.Equal(2d, res);
        }

        [Fact]
        public void TestWrongInput_3()
        {
            Calculator calculator = new Calculator();
            Assert.Throws<ArgumentException>(() => calculator.Evaluator.Evaluate("2-+5")); ;
        }

        [Fact]
        public void TestWrongInput_4()
        {
            Calculator calculator = new Calculator();
            Assert.Throws<ArgumentNullException>(() => calculator.Evaluator.Evaluate(""));
        }

        [Fact]
        public void TestWrongInput_5()
        {
            Calculator calculator = new Calculator();
            Assert.Throws<FormatException>(() => calculator.Evaluator.Evaluate("()"));
        }

        [Fact]
        public void TestWrongInput_6()
        {
            Calculator calculator = new Calculator();
            Assert.Throws<ArgumentException>(() => calculator.Evaluator.Evaluate("()+2"));
        }

        [Fact]
        public void TestWrongAndSimpleInput()
        {
            Calculator calculator = new Calculator();
            try
            {
                var th = calculator.Evaluator.Evaluate("12+");
                th = calculator.Evaluator.Evaluate("12+");
                th = calculator.Evaluator.Evaluate("12+");
            }
            catch (Exception)
            {
            }
            var res = calculator.Evaluator.Evaluate("2+2");
            Assert.Equal(4d, res);
        }
    }
}