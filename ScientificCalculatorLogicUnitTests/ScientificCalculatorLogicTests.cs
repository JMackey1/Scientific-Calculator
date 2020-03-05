﻿using CalculatorLogic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReversePolishNotationUtility;
using Xunit;
using Assert = Xunit.Assert;

namespace ScientificCalculatorUnitTests
{
    [TestClass]
    public class ScientificCalculatorLogicTests
    {
        [Theory]
        [InlineData("2 + 3", "5")]
        [InlineData(".001 + 1.999", "2")]
        [InlineData(".4 + 1.1", "1.5")]
        [InlineData("1 + 100 + 9", "110")]
        [InlineData("6 + -1", "5")]
        [InlineData("-1 + -3", "-4")]
        [InlineData("1 + 0", "1")]
        [InlineData("0 + 17", "17")]
        [InlineData("-9 + 0", "-9")]
        [InlineData("0 + -3", "-3")]
        [InlineData("10 - 9", "1")]
        [InlineData("9.2 - 0.2", "9")]
        [InlineData("72.001 - .101", "71.9")]
        [InlineData("-10 - 1", "-11")]
        [InlineData("-1 - -3", "2")]
        [InlineData("10 - 1 - -9", "18")]
        [InlineData("671 - 0", "671")]
        [InlineData("0 - 21", "-21")]
        [InlineData("0 - -10", "10")]
        [InlineData("100 * 100", "10000")]
        [InlineData("3.0 * 3.0", "9")]
        [InlineData(".1 * 5.0", "0.5")]
        [InlineData("-3 * 3", "-9")]
        [InlineData("2 * -8", "-16")]
        [InlineData("-5 * -5", "25")]
        [InlineData("100 * -2 * 3", "-600")]
        [InlineData("-3 * 5 * -10", "150")]
        [InlineData("-2 * -7 * -3", "-42")]
        [InlineData("100 * 0", "0")]
        [InlineData("99 * 1", "99")]
        [InlineData("16 / 2", "8")]
        [InlineData("7.0 / 2", "3.5")]
        [InlineData("14.0 / .1", "140")]
        [InlineData("-8 / 4", "-2")]
        [InlineData("100 / -10", "-10")]
        [InlineData("-90 / -45", "2")]
        [InlineData("1 / 0", "Infinity")]
        [InlineData("10 / 1", "10")]
        [InlineData("-2 / 1", "-2")]
        [InlineData("0 / 1", "0")]
        [InlineData("10 / 5 / 2", "1")]
        [InlineData("-6 / 2 / 3", "-1")]
        [InlineData("21 / -3 / -7", "1")]
        [InlineData("-90 / -2 / -5", "-9")]
        [InlineData("12^2", "144")]
        [InlineData("-10^3", "-1000")]
        [InlineData("2^-1", "0.5")]
        [InlineData("25^(1/2)", "5")]
        [InlineData("16^.5", "4")]
        [InlineData("2^2^3", "256")]
        [InlineData("17^1", "17")]
        [InlineData("21^0", "1")]
        [InlineData("0^1", "0")]
        [InlineData("0^0", "1")]
        [InlineData("0^(-10)", "Infinity")]
        [InlineData("1.5^2", "2.25")]
        [InlineData("9 % 5", "4")]
        [InlineData("-10 % 3", "-1")]
        [InlineData("12 % -5", "2")]
        [InlineData("-7 % -3", "-1")]
        [InlineData("10 % 0", "NaN")]
        [InlineData("0 % 10", "0")]
        [InlineData("19 % 1", "0")]
        [InlineData("4.0 % 3", "1")]
        [InlineData("7 % 3.0", "1")]
        [InlineData("26.0 % 4.0", "2")]
        [InlineData("5!", "120")]
        [InlineData("-4!", "-24")]
        [InlineData("0!", "0")]
        [InlineData("(-5)!", ScientificCalculatorLogic.NegativeFactorialError)]
        [InlineData("(1/2)!", ScientificCalculatorLogic.DecimalFactorialError)]
        [InlineData(".9!", ScientificCalculatorLogic.DecimalFactorialError)]
        [InlineData("negation(5)", "-5")]
        [InlineData("negation(-10)", "10")]
        [InlineData("negation(5 * (2 + 3))", "-25")]
        [InlineData("negation(-9 * -3)", "-27")]
        [InlineData("negation(5.9)", "-5.9")]
        [InlineData("negation(negation(-1))", "-1")]
        [InlineData("negation(-5) * 7", "35")]
        [InlineData("log(100)", "2")]
        [InlineData("log(-1)", "NaN")]
        [InlineData("log(0)", "-Infinity")]
        [InlineData("log(1)", "0")]
        [InlineData("log(10.0)", "1")]
        [InlineData("log(log(10000000000))", "1")]
        [InlineData("log(100) * 2", "4")]
        [InlineData("log(10 * 10)", "2")]
        [InlineData("ln(e)", "1")]
        [InlineData("ln(-1)", "NaN")]
        [InlineData("ln(0)", "-Infinity")]
        [InlineData("ln(1)", "0")]
        [InlineData("ln(ln(e))", "0")]
        [InlineData("ln(e) * 2", "2")]
        [InlineData("ln(e * e)", "2")]
        [InlineData("floor(8)", "8")]
        [InlineData("floor(7.2)", "7")]
        [InlineData("floor(-8.1)", "-9")]
        [InlineData("floor(0.0)", "0")]
        [InlineData("floor(1.999)", "1")]
        [InlineData("sin(floor(0.00002))", "0")]
        [InlineData("floor(5.6 + 1)", "6")]
        [InlineData("floor(0.5)", "0")]
        [InlineData("ceiling(8)", "8")]
        [InlineData("ceiling(7.2)", "8")]
        [InlineData("ceiling(-8.1)", "-8")]
        [InlineData("ceiling(0.0)", "0")]
        [InlineData("ceiling(1.0001)", "2")]
        [InlineData("sin(ceiling(-0.9999))", "-0")]
        [InlineData("ceiling(5.6 + 1)", "7")]
        [InlineData("ceiling(0.5)", "1")]
        [InlineData("sin(0)", "0")]
        [InlineData("sin(-π/2)", "-1")]
        [InlineData("sin(sin(0))", "0")]
        [InlineData("sin(sin(0)) + 1", "1")]
        [InlineData("cos(0)", "1")]
        [InlineData("cos(cos(π/2))", "1")]
        [InlineData("cos(cos(π/2)) + 1", "2")]
        [InlineData("tan(0)", "0")]
        [InlineData("tan(tan(0))", "0")]
        [InlineData("tan(tan(0)) + 1", "1")]
        [InlineData("asin(0)", "0")]
        [InlineData("acos(1)", "0")]
        [InlineData("atan(0)", "0")]
        [InlineData("-4^2", "-16")]
        [InlineData("2^(-2)", "0.25")]
        [InlineData("1+1+1*5", "7")]
        [InlineData("1 - 10 / 2", "-4")]
        [InlineData("-3", "-3")]
        [InlineData("--------------------10", "10")]
        [InlineData("1 + 8 % 3", "3")]
        [InlineData("2 * 5!", "240")]
        [InlineData("100 / ( (2 + (32 / 8) - 1) * 5)", "4")]
        [InlineData("5 - 5 - 5 - 5 - 5 - 5 - 5 - 5 - 5 - 5 - -5 - 5 * -5", "-10")]
        [InlineData("1 + 2 + 3 - 1 / 2 - 9 * 6 + log(10)", "-47.5")]
        [InlineData("log(10^(log(10)))", "1")]
        [InlineData("floor( (5.6 + (ceiling(5.01) / 3) ) )", "7")]
        [InlineData("10^log(10)", "10")]
        [InlineData("(1 + 1) - sin(2 - 2) + (3 * 3) * (4 / 4) / (5 + 5) * (6 * 6) - (7 - -7)", "20.4")]
        [InlineData("( (3 + (72 * sin(0) + (2 * (3 - 4) / 4) ) ) - 100)", "-97.5")]
        [InlineData("sin(cos(0))^(sin(0))", "1")]
        [InlineData("cos()", ScientificCalculatorLogic.CosineNotEnoughOperandsError)]
        [InlineData("2+", ScientificCalculatorLogic.AddNotEnoughOperandsError)]
        [InlineData("+2", ScientificCalculatorLogic.AddNotEnoughOperandsError)]
        [InlineData("2-", ScientificCalculatorLogic.SubtractNotEnoughOperandsError)]
        [InlineData("2*", ScientificCalculatorLogic.MultiplyNotEnoughOperandsError)]
        [InlineData("*2", ScientificCalculatorLogic.MultiplyNotEnoughOperandsError)]
        [InlineData("2/", ScientificCalculatorLogic.DivisionNotEnoughOperandsError)]
        [InlineData("/2", ScientificCalculatorLogic.DivisionNotEnoughOperandsError)]
        [InlineData("2%", ScientificCalculatorLogic.ModuloNotEnoughOperandsError)]
        [InlineData("%2", ScientificCalculatorLogic.ModuloNotEnoughOperandsError)]
        [InlineData("2^", ScientificCalculatorLogic.ExponentNotEnoughOperandsError)]
        [InlineData("sin()", ScientificCalculatorLogic.SineNotEnoughOperandsError)]
        [InlineData("asin()", ScientificCalculatorLogic.ArcSineNotEnoughOperandsError)]
        [InlineData("acos()", ScientificCalculatorLogic.ArcCosineNotEnoughOperandsError)]
        [InlineData("tan()", ScientificCalculatorLogic.TangentNotEnoughOperandsError)]
        [InlineData("atan()", ScientificCalculatorLogic.ArcTangentNotEnoughOperandsError)]
        [InlineData("floor()", ScientificCalculatorLogic.FloorNotEnoughOperandsError)]
        [InlineData("ceiling()", ScientificCalculatorLogic.CeilingNotEnoughOperandsError)]
        [InlineData("log()", ScientificCalculatorLogic.LogNotEnoughOperandsError)]
        [InlineData("ln()", ScientificCalculatorLogic.LnNotEnoughOperandsError)]
        [InlineData("negation()", ScientificCalculatorLogic.NegationNotEnoughOperandsError)]
        [InlineData("-", ScientificCalculatorLogic.NegationNotEnoughOperandsError)]
        [InlineData("(3!)!", "720")]
        [InlineData("ans+1", "NaN")]
        [InlineData("1.23E+2", "123")]
        [InlineData("1.11e+3", "1110")]
        [InlineData("100e-3", "0.1")]
        public void ParseExpression_ReturnsResult(string expressionInput, string expectedAnswer)
        {
            var logic = new ScientificCalculatorLogic();
            logic.ParseExpression(expressionInput);
            Assert.Equal(logic.Result, expectedAnswer);
        }
    }

    [TestClass]
    public class ReversePolishNotationTests
    {
        [Theory]
        [InlineData("2+3", "23+")]
        [InlineData("5-", "5-")]
        [InlineData("sin(", ReversePolishNotation.InvalidMismatchedParentheses)]
        [InlineData("( ( ( 3 + 2) )", ReversePolishNotation.InvalidMismatchedParentheses)]
        [InlineData("( ( 3 + 2) ", ReversePolishNotation.InvalidMismatchedParentheses)]
        [InlineData("!", ReversePolishNotation.InvalidOperatorBeforeFactorial)]
        [InlineData("^2", ReversePolishNotation.InvalidOperatorBeforeExponent)]
        [InlineData("sin)2(", ReversePolishNotation.InvalidMismatchedParentheses)]
        [InlineData("=", ReversePolishNotation.InvalidTokensInInputString)]
        [InlineData("             2 +          3", "23+")]
        [InlineData("   \n2 + \n3", "23+")]
        [InlineData("3 + 4 * 2 / (1 - 5) ^ 2 ^ 3", "342*15-23^^/+")]
        [InlineData("sin(cos(2 + 3) / 3 * π)", "23+cos3/π*sin")]
        [InlineData("!5", ReversePolishNotation.InvalidOperatorBeforeFactorial)]
        [InlineData("sinn(2)", ReversePolishNotation.InvalidTokensInInputString)]
        [InlineData("si(2)", ReversePolishNotation.InvalidTokensInInputString)]
        [InlineData("Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.", ReversePolishNotation.InvalidTokensInInputString)]
        [InlineData("218nbsn + 182nanm * 1sin2", ReversePolishNotation.InvalidTokensInInputString)]
        [InlineData("for(int i = 0; i < 10; i++) { doThings(); }", ReversePolishNotation.InvalidTokensInInputString)]
        [InlineData("2 + 3 * 4", "234*+")]
        [InlineData("(2 + 3) * 4", "23+4*")]
        [InlineData("2 + 3 * 4 + 5", "234*+5+")]
        [InlineData("(2+3)*(4+5)", "23+45+*")]
        [InlineData("2*3+4*5", "23*45*+")]
        [InlineData("2+3+4+5", "23+4+5+")]
        public void ReversePolishNotationExpression_ReturnsConvertedString(string expressionInput,
                                                                           string expectedAnswer)
        {
            var rpn = new ReversePolishNotation();
            rpn.ConvertExpressionToReversePolishNotation(expressionInput);
            Assert.Equal(rpn.ExpressionReversePolishNotation, expectedAnswer);
        }
    }
}
