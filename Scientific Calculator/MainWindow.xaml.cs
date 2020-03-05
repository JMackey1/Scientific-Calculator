using System.Windows;
using System.Windows.Input;
using CalculatorLogic;
using System.IO;
using System;
using System.Diagnostics;

namespace ScientificCalculator
{
    // The UI control logic for the calculator (button/key presses, messaging).
    public partial class MainWindow : Window
    {
        private ScientificCalculatorLogic _logic;

        public MainWindow()
        {
            InitializeComponent();
            _logic = new ScientificCalculatorLogic();
            currentCalculation.Focus();
        }

        private void buttonCE_Click(object sender, RoutedEventArgs e)
        {
            currentCalculation.Text = "";
            currentCalculation.Focus();
        }

        private void buttonC_Click(object sender, RoutedEventArgs e)
        {
            currentCalculation.Text = "";
            results.Text = "";
            _logic.ClearHistory();
            currentCalculation.Focus();
        }

        private void buttonZero_Click(object sender, RoutedEventArgs e) { InsertIntoText("0"); }
        private void buttonOne_Click(object sender, RoutedEventArgs e) { InsertIntoText("1"); }
        private void buttonTwo_Click(object sender, RoutedEventArgs e) { InsertIntoText("2"); }
        private void buttonThree_Click(object sender, RoutedEventArgs e) { InsertIntoText("3"); }
        private void buttonFour_Click(object sender, RoutedEventArgs e) { InsertIntoText("4"); }
        private void buttonFive_Click(object sender, RoutedEventArgs e) { InsertIntoText("5"); }
        private void buttonSix_Click(object sender, RoutedEventArgs e) { InsertIntoText("6"); }
        private void buttonSeven_Click(object sender, RoutedEventArgs e) { InsertIntoText("7"); }
        private void buttonEight_Click(object sender, RoutedEventArgs e) { InsertIntoText("8"); }
        private void buttonNine_Click(object sender, RoutedEventArgs e) { InsertIntoText("9"); }
        private void buttonDecimal_Click(object sender, RoutedEventArgs e) { InsertIntoText("."); }
        private void buttonEqual_Click(object sender, RoutedEventArgs e) { CalculateAnswer(); }
        private void buttonDelete_Click(object sender, RoutedEventArgs e) { DeleteText(); }
        private void buttonAdd_Click(object sender, RoutedEventArgs e) { InsertIntoText("+"); }
        private void buttonSubtract_Click(object sender, RoutedEventArgs e) { InsertIntoText("-"); }
        private void buttonMultiply_Click(object sender, RoutedEventArgs e) { InsertIntoText("*"); }
        private void buttonDivide_Click(object sender, RoutedEventArgs e) { InsertIntoText("/"); }
        private void buttonPower_Click(object sender, RoutedEventArgs e) { InsertIntoText("^"); }
        private void buttonLeftParen_Click(object sender, RoutedEventArgs e) { InsertIntoText("("); }
        private void buttonRightParen_Click(object sender, RoutedEventArgs e) { InsertIntoText(")"); }
        private void buttonFactorial_Click(object sender, RoutedEventArgs e) { InsertIntoText("!"); }
        private void buttonModulus_Click(object sender, RoutedEventArgs e) { InsertIntoText("%"); }
        private void buttonPi_Click(object sender, RoutedEventArgs e) { InsertIntoText("π"); }
        private void buttonE_Click(object sender, RoutedEventArgs e) { InsertIntoText("e"); }
        private void buttonNegation_Click(object sender, RoutedEventArgs e) { InsertIntoText("negation("); }
        private void buttonRoot_Click(object sender, RoutedEventArgs e) { InsertIntoText("^(1/"); }
        private void buttonLog_Click(object sender, RoutedEventArgs e) { InsertIntoText("log("); }
        private void buttonLn_Click(object sender, RoutedEventArgs e) { InsertIntoText("ln("); }
        private void buttonFloor_Click(object sender, RoutedEventArgs e) { InsertIntoText("floor("); }
        private void buttonCeiling_Click(object sender, RoutedEventArgs e) { InsertIntoText("ceiling("); }
        private void buttonSin_Click(object sender, RoutedEventArgs e) { InsertIntoText("sin("); }
        private void buttonCos_Click(object sender, RoutedEventArgs e) { InsertIntoText("cos("); }
        private void buttonTan_Click(object sender, RoutedEventArgs e) { InsertIntoText("tan("); }
        private void buttonArcSin_Click(object sender, RoutedEventArgs e) { InsertIntoText("asin("); }
        private void buttonArcCos_Click(object sender, RoutedEventArgs e) { InsertIntoText("acos("); }
        private void buttonArcTan_Click(object sender, RoutedEventArgs e) { InsertIntoText("atan("); }
        private void buttonAnswer_Click(object sender, RoutedEventArgs e) { InsertIntoText("ans"); }
        private void currentCalculation_KeyDown(object sender, KeyEventArgs e)
        {
            if(Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
            {
                switch (e.Key)
                {
                    case Key.D6: InsertIntoText("^"); break;
                    case Key.D5: InsertIntoText("%");  break;
                    case Key.D8: InsertIntoText("*"); break;
                    case Key.OemPlus: InsertIntoText("+"); break;
                    case Key.D1: InsertIntoText("!"); break;
                    case Key.D9: InsertIntoText("("); break;
                    case Key.D0: InsertIntoText(")"); break;
                    case Key.E: InsertIntoText("E"); break;
                }
            }
            else if(Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
            {
                switch(e.Key)
                {
                    case Key.V: InsertIntoText(Clipboard.GetText()); break;
                }
            }
            else
            {
                switch(e.Key)
                {
                    case Key.D1:
                    case Key.NumPad1: InsertIntoText("1"); break;
                    case Key.D2:
                    case Key.NumPad2: InsertIntoText("2"); break;
                    case Key.D3:
                    case Key.NumPad3: InsertIntoText("3"); break;
                    case Key.D4:
                    case Key.NumPad4: InsertIntoText("4"); break;
                    case Key.D5:
                    case Key.NumPad5: InsertIntoText("5"); break;
                    case Key.D6:
                    case Key.NumPad6: InsertIntoText("6"); break;
                    case Key.D7:
                    case Key.NumPad7: InsertIntoText("7"); break;
                    case Key.D8:
                    case Key.NumPad8: InsertIntoText("8"); break;
                    case Key.D9:
                    case Key.NumPad9: InsertIntoText("9"); break;
                    case Key.D0:
                    case Key.NumPad0: InsertIntoText("0"); break;
                    case Key.Subtract:
                    case Key.OemMinus: InsertIntoText("-"); break;
                    case Key.OemPeriod:
                    case Key.Decimal: InsertIntoText("."); break;
                    case Key.Multiply: InsertIntoText("*"); break;
                    case Key.Divide:
                    case Key.OemQuestion: InsertIntoText("/"); break;
                    case Key.Add: InsertIntoText("+"); break;
                    case Key.Return:
                    case Key.OemPlus: CalculateAnswer(); break;
                    case Key.Back: DeleteText(); break;
                    case Key.P: InsertIntoText("π"); break;
                    case Key.I: InsertIntoText("i"); break;
                    case Key.N: InsertIntoText("n"); break;
                    case Key.E: InsertIntoText("e"); break;
                    case Key.G: InsertIntoText("g"); break;
                    case Key.A: InsertIntoText("a"); break;
                    case Key.T: InsertIntoText("t"); break;
                    case Key.S: InsertIntoText("s"); break;
                    case Key.C: InsertIntoText("c"); break;
                    case Key.O: InsertIntoText("o"); break;
                    case Key.L: InsertIntoText("l"); break;
                    case Key.F: InsertIntoText("f"); break;
                    case Key.R: InsertIntoText("r"); break;
                    case Key.Space: InsertIntoText(" "); break;
                    case Key.Up: 
                        InsertHistoryIntoText(_logic.History(ScientificCalculatorLogic.PreviousHistory));
                        break;
                    case Key.Down:
                        InsertHistoryIntoText(_logic.History(ScientificCalculatorLogic.NextHistory));
                        break;
                    case Key.F1:
                        string basePath = AppContext.BaseDirectory;
                        string fileName = "Properties\\scientificCalculatorReadme.htm";
                        results.Text = Path.GetFileName(AppContext.BaseDirectory);
                        Process.Start(@"cmd.exe", @"/c " + "\"" + basePath + fileName + "\"");
                        break;
                }
            }
        }

        private void InsertHistoryIntoText(string value)
        {
            currentCalculation.Text = value;
            currentCalculation.SelectionStart = value.Length;
            currentCalculation.Focus();
        }

        private void CalculateAnswer()
        {
            if (!string.IsNullOrEmpty(currentCalculation.Text))
            {
                _logic.ParseExpression(currentCalculation.Text);
                results.Text = currentCalculation.Text + " = " + _logic.Result + "\n\n" + results.Text;
                currentCalculation.Text = "";
            }
            currentCalculation.Focus();
        }

        private void DeleteText()
        {
            var index = currentCalculation.SelectionStart - 1;
            if (index >= 0)
            {
                currentCalculation.Text = currentCalculation.Text.Remove(index, 1);
                currentCalculation.SelectionStart = index;
            }
            currentCalculation.Focus();
        }

        private void InsertIntoText(string value)
        {
            var currentCaretPosition = currentCalculation.SelectionStart;
            currentCalculation.Text = currentCalculation.Text.Insert(currentCaretPosition, value);
            currentCalculation.SelectionStart = currentCaretPosition + value.Length;
            currentCalculation.Focus();
        }
    }
}
