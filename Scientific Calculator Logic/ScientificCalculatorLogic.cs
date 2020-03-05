using ReversePolishNotationUtility;
using System;
using System.Collections.Generic;

namespace CalculatorLogic
{
    // This class contains the logic to read an infix mathematical expression and compute the result.
    // Supported functions can be found by looking at the operator constants, or the actions
    // dictionary.
    public class ScientificCalculatorLogic
    {
        // Error message constants for specific expression problems.
        public const string NegativeFactorialError = "Factorial for negative values is undefined.";
        public const string FactorialNotEnoughOperandsError = "Cannot factorial. Not enough operands.";
        public const string DecimalFactorialError = "Factorial for decimal values is undefined.";
        public const string ExponentNotEnoughOperandsError = "Cannot exponentiate. Not enough operands.";
        public const string DivisionNotEnoughOperandsError = "Cannot divide. Not enough operands.";
        public const string CannotDivideByZeroError = "Cannot divide by 0.";
        public const string MultiplyNotEnoughOperandsError = "Cannot multiply. Not enough operands.";
        public const string SubtractNotEnoughOperandsError = "Cannot subtract. Not enough operands.";
        public const string AddNotEnoughOperandsError = "Cannot add. Not enough operands.";
        public const string ModuloNotEnoughOperandsError = "Cannot modulo. Not enough operands.";
        public const string InvalidTooManyOperands = "Invalid input. Too many operands with no operators.";
        public const string NegationNotEnoughOperandsError = "Cannot negate. Not enough operands.";
        public const string SineNotEnoughOperandsError = "Cannot find sine. Not enough operands.";
        public const string ArcSineNotEnoughOperandsError = "Cannot find arcsine. Not enough operands.";
        public const string CosineNotEnoughOperandsError = "Cannot find cosine. Not enough operands.";
        public const string ArcCosineNotEnoughOperandsError = "Cannot find arccosine. Not enough operands.";
        public const string TangentNotEnoughOperandsError = "Cannot find tangent. Not enough operands.";
        public const string ArcTangentNotEnoughOperandsError = "Cannot find arctangent. Not enough operands.";
        public const string FloorNotEnoughOperandsError = "Cannot floor. Not enough operands.";
        public const string CeilingNotEnoughOperandsError = "Cannot ceiling. Not enough operands.";
        public const string LogNotEnoughOperandsError = "Cannot take log. Not enough operands.";
        public const string LnNotEnoughOperandsError = "Cannot take ln. Not enough operands.";

        // Variables related to retrieving the previous expresson entered.
        public const string PreviousHistory = "previous";
        public const string NextHistory = "next";
        private List<string> _expressionHistory = new List<string>();
        private int _historyIndex = 0;

        // Valid symbols that may show up in an expression.
        private const string Pi = "π";
        private const string e = "e";
        private const string AdditionOperator = "+";
        private const string SubtractionOperator = "-";
        private const string MultiplyOperator = "*";
        private const string DivisionOperator = "/";
        private const string FactorialOperator = "!";
        private const string ExponentOperator = "^";
        private const string ModuloOperator = "%";
        private const string NegationOperator = "negation";
        private const string CeilingOperator = "ceiling";
        private const string FloorOperator = "floor";
        private const string LnOperator = "ln";
        private const string LogOperator = "log";
        private const string SineOperator = "sin";
        private const string CosineOperator = "cos";
        private const string TangentOperator = "tan";
        private const string ArcSineOperator = "asin";
        private const string ArcCosineOperator = "acos";
        private const string ArcTangentOperator = "atan";
        private const string PreviousAnswerOperator = "ans";
        private const string UnaryMinusOperator = "m";

        // A mapping of operators to functions to be called on them.
        private Dictionary<string, Action> _actions;

        private Queue<string> _outputQueue;
        private Stack<double> _valueStack;

        // The previous answer found when performing a calculation.
        private double _lastResult = double.NaN;
        public string Result { get; private set; }

        public ScientificCalculatorLogic() { InitializeActions(); }

        // Calculates the value of an infix mathematical expression and displays an appropriate
        // error message if the expression is invalid for some reason.
        public void ParseExpression(string expression)
        {
            AddToHistory(expression);
            _historyIndex = _expressionHistory.Count;
            Result = "";
            var reversePolishNotation = new ReversePolishNotation();
            _outputQueue = reversePolishNotation.ConvertExpressionToReversePolishNotation(expression);
            
            if(reversePolishNotation.ErrorFound)
            {
                Result = reversePolishNotation.ExpressionReversePolishNotation;
                _lastResult = double.NaN;
                return;
            }

            _valueStack = new Stack<double>();

            while(_outputQueue.Count > 0)
            {
                string top = _outputQueue.Dequeue();
                if(_actions.ContainsKey(top))
                {
                    _actions[top]();
                }
                else
                {
                    switch (top) {
                        case e: _valueStack.Push(Math.E); break;
                        case Pi: _valueStack.Push(Math.PI); break;
                        case PreviousAnswerOperator: _valueStack.Push(_lastResult); break;
                        default: _valueStack.Push(double.Parse(top)); break;
                    }
                }
            }

            if(_valueStack.Count == 1) {
                _lastResult = _valueStack.Pop();
                Result = _lastResult.ToString();
            }
            else if(_valueStack.Count > 1)
            {
                Result = InvalidTooManyOperands;
                _lastResult = double.NaN;
            }
        }

        public string History(string accessCode)
        {
            if (_expressionHistory.Count > 0)
            {
                switch (accessCode)
                {
                    case PreviousHistory:
                        _historyIndex--;
                        if (_historyIndex < 0) 
                        { 
                            _historyIndex = 0;
                        }
                        break;
                    case NextHistory:
                        _historyIndex++;
                        if (_historyIndex >= _expressionHistory.Count) 
                        {
                            _historyIndex = _expressionHistory.Count;
                            return "";
                        }
                        break;
                }
                return _expressionHistory[_historyIndex];
            }
            return "";
        }

        public void ClearHistory()
        {
            _expressionHistory.Clear();
        }

        private void AddToHistory(string expression)
        {
            if(_expressionHistory.Count > 49)
            {
                _expressionHistory.RemoveAt(0);
            }
            _expressionHistory.Add(expression);
        }

        private void InitializeActions()
        {
            _actions = new Dictionary<string, Action>()
            {
                {AdditionOperator, () => Add() },
                {SubtractionOperator, () => Subtract() },
                {MultiplyOperator, () => Multiply() },
                {DivisionOperator, () => Divide() },
                {ArcCosineOperator, () => ArcCosine() },
                {CosineOperator, () => Cosine() },
                {ArcSineOperator, () => ArcSine() },
                {SineOperator, () => Sine() },
                {ArcTangentOperator, () => ArcTangent() },
                {TangentOperator, () => Tangent() },
                {NegationOperator, () => Negation() },
                {UnaryMinusOperator, () => Negation() },
                {FloorOperator, () => Floor() },
                {CeilingOperator, () => Ceiling() },
                {LogOperator, () => Log() },
                {LnOperator, () => Ln() },
                {ExponentOperator, () => Exponentiate() },
                {ModuloOperator, () => Modulo() },
                {FactorialOperator, () => Factorial() }
            };
        }

        private void Ln()
        {
            if(ValidUnaryOperation(LnNotEnoughOperandsError))
            {
                _valueStack.Push(Math.Log(_valueStack.Pop()));
            }
        }

        private void Log()
        {
            if (ValidUnaryOperation(LogNotEnoughOperandsError))
            {
                _valueStack.Push(Math.Log10(_valueStack.Pop()));
            }
        }

        private void Ceiling()
        {
            if (ValidUnaryOperation(CeilingNotEnoughOperandsError))
            {
                _valueStack.Push(Math.Ceiling(_valueStack.Pop()));
            }
        }

        private void Floor()
        {
            if (ValidUnaryOperation(FloorNotEnoughOperandsError)) {
                _valueStack.Push(Math.Floor(_valueStack.Pop()));
            }
        }

        private void ArcTangent()
        {
            if (ValidUnaryOperation(ArcTangentNotEnoughOperandsError))
            {
                _valueStack.Push(Math.Atan(_valueStack.Pop()));
            }
        }

        private void Tangent()
        {
            if (ValidUnaryOperation(TangentNotEnoughOperandsError))
            {
                _valueStack.Push(Math.Tan(_valueStack.Pop()));
            }
        }

        private void ArcCosine()
        {
            if (ValidUnaryOperation(ArcCosineNotEnoughOperandsError))
            {
                _valueStack.Push(Math.Acos(_valueStack.Pop()));
            }
        }

        private void Cosine()
        {
            if (ValidUnaryOperation(CosineNotEnoughOperandsError))
            {
                _valueStack.Push(Math.Cos(_valueStack.Pop()));
            }
        }

        private void ArcSine()
        {
            if (ValidUnaryOperation(ArcSineNotEnoughOperandsError))
            {
                _valueStack.Push(Math.Asin(_valueStack.Pop()));
            }
        }

        private void Sine()
        {
            if (ValidUnaryOperation(SineNotEnoughOperandsError))
            {
                _valueStack.Push(Math.Sin(_valueStack.Pop()));
            }
        }

        private void Negation()
        {
            if (ValidUnaryOperation(NegationNotEnoughOperandsError))
            {
                _valueStack.Push(_valueStack.Pop() * -1);
            }
        }

        private void Modulo()
        {
            if (ValidBinaryOperation(ModuloNotEnoughOperandsError))
            {
                try
                {
                    double rightOperand = _valueStack.Pop();
                    double leftOperand = _valueStack.Pop();
                    _valueStack.Push(leftOperand % rightOperand);
                }
                catch (DivideByZeroException)
                {
                    Result = CannotDivideByZeroError;
                    ClearStackAndQueue();
                    return;
                }
            }
        }

        private void Factorial()
        {
            if (ValidUnaryOperation(FactorialNotEnoughOperandsError))
            {
                double leftOperand = _valueStack.Pop();
                double result = leftOperand;

                if (leftOperand < 0)
                {
                    Result = NegativeFactorialError;
                    ClearStackAndQueue();
                    return;
                }

                if (leftOperand % 1 != 0)
                {
                    Result = DecimalFactorialError;
                    ClearStackAndQueue();
                    return;
                }

                while (leftOperand > 1)
                {
                    leftOperand--;
                    result *= leftOperand;
                }
                _valueStack.Push(result);
            }
        }

        private void Exponentiate()
        {
            if (ValidBinaryOperation(ExponentNotEnoughOperandsError))
            {
                double rightOperand = _valueStack.Pop();
                double leftOperand = _valueStack.Pop();
                _valueStack.Push(Math.Pow(leftOperand, rightOperand));
            }
        }

        private void Divide()
        {
            if (ValidBinaryOperation(DivisionNotEnoughOperandsError))
            {
                try
                {
                    double rightOperand = _valueStack.Pop();
                    double leftOperand = _valueStack.Pop();
                    _valueStack.Push(leftOperand / rightOperand);
                }
                catch (DivideByZeroException)
                {
                    Result = CannotDivideByZeroError;
                    ClearStackAndQueue();
                    return;
                }
            }
        }

        private void Multiply()
        {
            if (ValidBinaryOperation(MultiplyNotEnoughOperandsError))
            {
                double rightOperand = _valueStack.Pop();
                double leftOperand = _valueStack.Pop();
                _valueStack.Push(leftOperand * rightOperand);
            }
        }

        private void Subtract()
        {
            if (ValidBinaryOperation(SubtractNotEnoughOperandsError))
            {
                double rightOperand = _valueStack.Pop();
                double leftOperand = _valueStack.Pop();
                _valueStack.Push(leftOperand - rightOperand);
            }
        }

        private void Add()
        {
            if (ValidBinaryOperation(AddNotEnoughOperandsError))
            {
                double rightOperand = _valueStack.Pop();
                double leftOperand = _valueStack.Pop();
                _valueStack.Push(leftOperand + rightOperand);
            }
        }

        private bool ValidUnaryOperation(string errorMessage)
        {
            if (_valueStack.Count < 1)
            {
                Result = errorMessage;
                ClearStackAndQueue();
                return false;
            }

            return true;
        }

        private bool ValidBinaryOperation(string errorMessage)
        {
            if (_valueStack.Count < 2)
            {
                Result = errorMessage;
                ClearStackAndQueue();
                return false;
            }

            return true;
        }

        private void ClearStackAndQueue()
        {
            _valueStack.Clear();
            _outputQueue.Clear();
        }
    }
}
