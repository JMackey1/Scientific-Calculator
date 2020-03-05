using System.Collections.Generic;
using System.Text;

namespace ReversePolishNotationUtility
{
    // This is a utility class that converts an infix mathematical expression into a
    // postfix (Reverse Polish) expression. It can be expanded upon and used elsewhere
    // by changing the precedent values and adding operators.
    public class ReversePolishNotation
    {
        // Error message constants for specific expression problems.
        public const string InvalidMismatchedParentheses = "Invalid input. Mismatched parentheses.";
        public const string InvalidOperatorBeforeExponent = "Invalid input. No operand before exponent.";
        public const string InvalidOperatorBeforeFactorial = "Invalid input. No operand before factorial.";
        public const string InvalidTokensInInputString = "Invalid input. Bad tokens found.";

        // This is a mapping of all valid operators, including functions such as floor, to an
        // int array which contains the precedence weight of the operator as well as the operator's
        // associativity. The first element is the precedence, the second element is the associativity.
        private static Dictionary<string, int[]> Operators;

        // Operator precedent weights for determining order of operations.
        private const int AddSubtractPrecedent = 0;
        private const int MultiplyDividePrecedent = 5;
        private const int PowerUnaryMinusPrecedent = 10;
        private const int FactorialPrecedent = 15;
        private const int FunctionPrecedent = 20;

        // Operator associativity values for determining order of operations.
        private const int RightAssociation = 0;
        private const int LeftAssociation = 1;

        // Operators are stored in a dictionary which links to an array of precedent/association.
        // These are the index values to get the corresponding value from the array.
        private const int PrecedentIndex = 0;
        private const int AssociationIndex = 1;

        // Valid symbols that may show up in an expression.
        private const char Pi = 'π';
        private const char eulerNumber = 'e';
        private const char scientificNotationE = 'E';
        private const char Period = '.';
        private const string Plus = "+";
        private const string Minus = "-";
        private const string Multiply = "*";
        private const string Divide = "/";
        private const string Factorial = "!";
        private const string Exponent = "^";
        private const string Modulus = "%";
        private const string Negation = "negation";
        private const string Ceiling = "ceiling";
        private const string Floor = "floor";
        private const string Ln = "ln";
        private const string Log = "log";
        private const string Sine = "sin";
        private const string Cosine = "cos";
        private const string Tangent = "tan";
        private const string ArcSine = "asin";
        private const string ArcCosine = "acos";
        private const string ArcTangent = "atan";
        private const string LeftParentheses = "(";
        private const string RightParentheses = ")";
        private const string PreviousAnswer = "ans";
        private const string UnaryMinus = "m";

        public bool ErrorFound { get; private set; }
        public string ExpressionReversePolishNotation { get; private set; } = "";        

        static ReversePolishNotation() { InitializeOperators(); }

        // Converts an infix mathematical expression to a Reverse Polish Notation expression
        // and returns the new expression as a queue.
        public Queue<string> ConvertExpressionToReversePolishNotation(string expression)
        {
            List<string> tokens = SplitExpressionIntoTokens(expression);
            var outputQueue = new Queue<string>();
            
            if(!InvalidInputTokens(tokens))
            {
                ConvertExpressionToReversePolishNotation(tokens, outputQueue);
            }
           
            return outputQueue;
        }

        // This is a modification of the Shunting-Yard algorithm to allow for unary operators.
        private void ConvertExpressionToReversePolishNotation(List<string> tokens,
                                                              Queue<string> outputQueue)
        {
            var previousToken = "";
            var operatorStack = new Stack<string>();
            foreach (string token in tokens)
            {
                if (double.TryParse(token, out _))
                {
                    outputQueue.Enqueue(token);
                }
                else if (token == eulerNumber.ToString() || token == Pi.ToString() || token == PreviousAnswer)
                {
                    outputQueue.Enqueue(token);
                }
                else if (token == LeftParentheses)
                {
                    operatorStack.Push(token);
                }
                else if (token == RightParentheses)
                {
                    while (operatorStack.Count >= 0)
                    {
                        if (operatorStack.Count == 0)
                        {
                            ErrorFound = true;
                            ExpressionReversePolishNotation = InvalidMismatchedParentheses;
                            return;
                        }
                        else if (operatorStack.Peek() != LeftParentheses)
                        {
                            outputQueue.Enqueue(operatorStack.Pop());
                        }
                        else
                        {
                            operatorStack.Pop();
                            break;
                        }
                    }
                }
                else if (Operators.ContainsKey(token))
                {
                    if (token == Minus && (previousToken == LeftParentheses ||
                        string.IsNullOrEmpty(previousToken) || Operators.ContainsKey(previousToken)))
                    {
                        operatorStack.Push(UnaryMinus);
                    }
                    else if (token == Factorial && (previousToken == LeftParentheses ||
                        string.IsNullOrEmpty(previousToken) || Operators.ContainsKey(previousToken)))
                    {
                        ErrorFound = true;
                        ExpressionReversePolishNotation = InvalidOperatorBeforeFactorial;
                        return;
                    }
                    else if (token == Exponent && (previousToken == LeftParentheses ||
                        string.IsNullOrEmpty(previousToken) || Operators.ContainsKey(previousToken)))
                    {
                        ErrorFound = true;
                        ExpressionReversePolishNotation = InvalidOperatorBeforeExponent;
                        return;
                    }
                    else if (operatorStack.Count == 0)
                    {
                        operatorStack.Push(token);
                    }
                    else
                    {
                        while (operatorStack.Count > 0)
                        {
                            if (operatorStack.Peek() == LeftParentheses)
                            {
                                break;
                            }
                            else if (ProcessPreviousOperators(operatorStack.Peek(), token))
                            {
                                outputQueue.Enqueue(operatorStack.Pop());
                            }
                            else
                            {
                                break;
                            }
                        }
                        operatorStack.Push(token);
                    }
                }
                previousToken = token;
            }

            while (operatorStack.Count > 0)
            {
                if (operatorStack.Peek() == LeftParentheses || operatorStack.Peek() == RightParentheses)
                {
                    ErrorFound = true;
                    ExpressionReversePolishNotation = InvalidMismatchedParentheses;
                    return;
                }
                else
                {
                    outputQueue.Enqueue(operatorStack.Pop());
                }
            }

            foreach (string term in outputQueue)
            {
                ExpressionReversePolishNotation += term;
            }
        }

        // This is the order of operations determination logic when placing an operator on the
        // operator stack. Based on the Shunting-Yard algorithm from Wikipedia. It returns true
        // if operators on the stack need to be processed before the current operator is added.
        private bool ProcessPreviousOperators(string topOperator, string currentToken)
        {
            int[] topProperties = Operators[topOperator];
            int[] currentPropertires = Operators[currentToken];
            var topPrecedence = topProperties[PrecedentIndex];
            var currentPrecedence = currentPropertires[PrecedentIndex];
            var currentAssociation = currentPropertires[AssociationIndex];
            var topPrecedenceIsHigher = topPrecedence > currentPrecedence;
            var precedenceIsEqual = topPrecedence == currentPrecedence;
            var currentIsLeftAssociative = currentAssociation == LeftAssociation;
            var termIsLeftAssociativeAndEqualPrecedence = precedenceIsEqual && currentIsLeftAssociative;
            return topPrecedenceIsHigher || termIsLeftAssociativeAndEqualPrecedence;
        }

        private bool InvalidInputTokens(List<string> tokens)
        {
            foreach (string token in tokens)
            {
                if (double.TryParse(token, out _))
                {
                    continue;
                }
                else if (Operators.ContainsKey(token))
                {
                    continue;
                }
                else if (token == LeftParentheses || token == RightParentheses ||
                    token == Pi.ToString() || token == eulerNumber.ToString() || token == PreviousAnswer)
                {
                    continue;
                }
                else
                {
                    ExpressionReversePolishNotation = InvalidTokensInInputString;
                    ErrorFound = true;
                    return ErrorFound;
                }
            }

            return false;
        }

        private List<string> SplitExpressionIntoTokens(string expression)
        {
            var tokens = new List<string>();
            StringBuilder stringBuilder;

            for(int i = 0; i < expression.Length; i++) 
            {
                stringBuilder = new StringBuilder();
                char character = expression[i];

                if(CharacterIsAConstantOrSymbol(character))
                {
                    tokens.Add(character.ToString());
                }
                else if(CharacterIsADigitOrDecimal(character))
                {
                    stringBuilder.Append(character);
                    var length = 1;
                    char previousCharacter = ' ';
                    while ((i + length) < expression.Length)
                    {
                        char nextCharacter = expression[i + length];
                        if (CharacterIsInScientificNotation(nextCharacter) ||
                            CharacterIsOperatorInScientificNotation(previousCharacter, nextCharacter))
                        {
                            stringBuilder.Append(nextCharacter);
                            previousCharacter = nextCharacter;
                        }
                        else
                        {
                            break;
                        }

                        length++;
                    }

                    tokens.Add(stringBuilder.ToString());
                    i = i + length - 1;
                }
                else if(char.IsWhiteSpace(character))
                {
                    continue;
                }
                else if(char.IsLetter(character))
                {
                    stringBuilder.Append(character);
                    var length = 1;
                    while((i + length) < expression.Length)
                    {
                        char nextCharacter = expression[i + length];
                        if(char.IsLetter(nextCharacter))
                        {
                            stringBuilder.Append(nextCharacter);
                        }
                        else
                        {
                            break;
                        }

                        length++;
                    }

                    tokens.Add(stringBuilder.ToString());
                    i = i + length - 1;
                }
                else
                {
                    tokens.Add(stringBuilder.ToString());
                }
            }

            return tokens;
        }

        private bool CharacterIsAConstantOrSymbol(char character)
        {
            if (character == Pi)
            {
                return true;
            }
            else if (character == eulerNumber)
            {
                return true;
            }
            else if (string.Equals(character.ToString(), LeftParentheses))
            {
                return true;
            }
            else if (string.Equals(character.ToString(), RightParentheses))
            {
                return true;
            }
            else if (Operators.ContainsKey(character.ToString()))
            {
                return true;
            }
            return false;
        }

        private bool CharacterIsADigitOrDecimal(char character)
        {
            return char.IsDigit(character) || character == Period;
        }

        private bool CharacterIsEulerNumberOrE(char character)
        {
            return character == eulerNumber || character == scientificNotationE;
        }

        private bool CharacterIsInScientificNotation(char character)
        {
            return CharacterIsADigitOrDecimal(character) || CharacterIsEulerNumberOrE(character);
        }

        private bool CharacterIsOperatorInScientificNotation(char previousToken, char currentToken)
        {
            var currentString = currentToken.ToString();
            var currentTokenIsPlusOrMinus = currentString == Plus || currentString == Minus;
            return (CharacterIsEulerNumberOrE(previousToken) && currentTokenIsPlusOrMinus);
        }

        private static void InitializeOperators()
        {
            Operators = new Dictionary<string, int[]>
            {
                [Plus] = new int[] { AddSubtractPrecedent, LeftAssociation },
                [Minus] = new int[] { AddSubtractPrecedent, LeftAssociation },
                [Multiply] = new int[] { MultiplyDividePrecedent, LeftAssociation },
                [Divide] = new int[] { MultiplyDividePrecedent, LeftAssociation },
                [Exponent] = new int[] { PowerUnaryMinusPrecedent, RightAssociation },
                [Factorial] = new int[] { FactorialPrecedent, RightAssociation },
                [Modulus] = new int[] { MultiplyDividePrecedent, LeftAssociation },
                [Negation] = new int[] { FunctionPrecedent, LeftAssociation },
                [Log] = new int[] { FunctionPrecedent, LeftAssociation },
                [Ceiling] = new int[] { FunctionPrecedent, LeftAssociation },
                [Ln] = new int[] { FunctionPrecedent, LeftAssociation },
                [Floor] = new int[] { FunctionPrecedent, LeftAssociation },
                [Sine] = new int[] { FunctionPrecedent, LeftAssociation },
                [ArcSine] = new int[] { FunctionPrecedent, LeftAssociation },
                [Cosine] = new int[] { FunctionPrecedent, LeftAssociation },
                [ArcCosine] = new int[] { FunctionPrecedent, LeftAssociation },
                [Tangent] = new int[] { FunctionPrecedent, LeftAssociation },
                [ArcTangent] = new int[] { FunctionPrecedent, LeftAssociation },
                [UnaryMinus] = new int[] { PowerUnaryMinusPrecedent, RightAssociation }
            };
        }
    }
}
