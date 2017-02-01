/*
 * Application: CalculatorApp
 * Description: A mobile calculator app for iOS and Android using Xamarin.
 * File Name: MainPage.xaml.cs
 * Author: Marco Duarte 100006379
 * Last Updated: 31/01/2017
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CalculatorApp
{
    public partial class MainPage : ContentPage
    {
        const string    OPERATOR_ADD = "+",
                        OPERATOR_SUBTRACT = "-",
                        OPERATOR_MULTIPLY = "X",
                        OPERATOR_DIVIDE = "/",
                        DECIMAL = ".",
                        ZERO = "0";

        List<Button>    operatorButtons,
                        operandButtons,
                        otherButtons;

        string[] operatorString = {
                                    OPERATOR_ADD,
                                    OPERATOR_SUBTRACT,
                                    OPERATOR_MULTIPLY,
                                    OPERATOR_DIVIDE, DECIMAL, ZERO
                                };
        List<String> calculationString; //concatination of strings to perform calculation


        public MainPage()
        {
            InitializeComponent();

            //add operator buttons Button list
            operatorButtons = new List<Button>
            {
                addButton, minusButton, multiplyButton,
                divideButton, equalsButton
            };

            //add operand Buttons to Button list
            operandButtons = new List<Button>
            {
                zeroButton, oneButton, twoButton, threeButton,
                fourButton, fiveButton, sixButton, sevenButton,
                eightButton, nineButton
            };

            //add other buttons to Button List
            otherButtons = new List<Button>
            {
                clearButton, decimalButton, togglePosNegButton
            };

            //call on event handler method
            AddEvent();

            calculationString = new List<string>();
            calculationString.Add(ZERO);
        }

        /// <summary>
        /// Method handling the event Handler for each of the buttons on the gui.
        /// Each button will add to the calculationString List to be
        /// evaluated
        /// </summary>
        void AddEvent()
        {
            //when a number is clicked 
            foreach (Button operandButton in operandButtons)
            {
                operandButton.Clicked += OperandButtonTouch;
            }
            //when an operator is clicked
            foreach (Button operatorButton in operandButtons)
            {
                operatorButton.Clicked += OperatorButtonTouch;
            }
            //when other buttons are clicked
            foreach (Button otherButton in otherButtons)
            {
                otherButton.Clicked += OtherButtonTouch;
            }
        }

        /// <summary>
        /// Event Handler when a Operator Button is touched
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OperatorButtonTouch(object sender, EventArgs e)
        {
            try
            {
                Button touchedButton = (Button)sender;

                //if the equals button was touched
                if (touchedButton == equalsButton)
                {
                    //determine if the last item in the calculation is an operator
                    if (operatorString.Contains(calculationString[calculationString.Count - 1]))
                    {
                        //if it is an operator, remove it as there is no following operand
                        calculationString.Remove(calculationString[calculationString.Count - 1]);
                    }

                    CalculateAnswer();
                }
                else
                {
                    //if the calculation isn't empty and the last action wasn't an operator
                    if (calculationString.Count > 0 && !operatorString.Contains(calculationString[calculationString.Count - 1]))
                    {
                        //add the operator to the calculation chain
                        calculationString.Add(touchedButton.Text);
                    }
                    else if (calculationString.Count > 0 && operatorString.Contains(calculationString[calculationString.Count - 1]))
                    {
                        //update the last operator to the new one
                        calculationString[calculationString.Count - 1] = touchedButton.Text;
                    }
                }
            }
            catch (Exception ex)
            {

                DisplayAlert("Oops", ex.Message, "Contact Your Administrator!");
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OperandButtonTouch(object sender, EventArgs e)
        {
            try
            {
                Button touchedButton = (Button)sender;

                int pressedValue = int.Parse(touchedButton.Text);

                //if there has been any operations yet or the last item in the calculation string was an operator
                if (calculationString.Count == 0 || (calculationString.Count > 0 && operatorString.Contains(calculationString[calculationString.Count - 1])))
                {
                    calculationString.Add(pressedValue.ToString());
                }
                else
                {
                    //if the last value was a zero
                    if (calculationString[calculationString.Count - 1] == ZERO)
                    {
                        //set the value to the pressed value to prevent leading zeros
                        calculationString[calculationString.Count - 1] = pressedValue.ToString();
                    }
                    else
                    {
                        //append the value to the last value on the chain
                        calculationString[calculationString.Count - 1] += pressedValue.ToString();
                    }
                }

                //update the display if successful
                if (calculationString.Count > 0)
                {
                    UpdateDisplayValue(calculationString[calculationString.Count - 1]);
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", ex.Message, "Error");
            }
        }

        /// <summary>
        /// Event Handler when one of the Other Buttons are touched
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OtherButtonTouch(object sender, EventArgs e)
        {
            try
            {
                Button touchedButton = (Button)sender;

                if (touchedButton == clearButton)
                {
                    Reset();
                }
                else if (touchedButton == decimalButton)
                {
                    if (calculationString.Count == 0 || (calculationString.Count > 0 && operatorString.Contains(calculationString[calculationString.Count - 1])))
                    {
                        //add "0." to the calculation chain
                        calculationString.Add(ZERO + DECIMAL);
                    }
                    else
                    {
                        //if the last value does not contain a decimal
                        if (!calculationString[calculationString.Count - 1].Contains(DECIMAL))
                        {
                            //add a decimal to the value
                            calculationString[calculationString.Count - 1] += DECIMAL;
                        }
                    }

                    //if there are items in the calculation chain list
                    if (calculationString.Count > 0)
                    {
                        //update the display if successful
                        UpdateDisplayValue(calculationString[calculationString.Count - 1]);
                    }
                }
                else if (touchedButton == togglePosNegButton)
                {
                    //if the calculation chain isn't empty
                    if (calculationString.Count > 0)
                    {
                        int indexDiff = 0;
                        //if the last item in the calculation chain was an operator
                        if (operatorString.Contains(calculationString[calculationString.Count - 1]))
                            indexDiff = 2;
                        else
                            indexDiff = 1;

                        if (calculationString[calculationString.Count - indexDiff] != "0")
                        {
                            //either add or remove a minus
                            if (calculationString[calculationString.Count - indexDiff].Contains("-"))
                                calculationString[calculationString.Count - indexDiff] = calculationString[calculationString.Count - indexDiff].TrimStart('-');
                            else
                                calculationString[calculationString.Count - indexDiff] = String.Format("-{0}", calculationString[calculationString.Count - indexDiff]);

                            UpdateDisplayValue(calculationString[calculationString.Count - indexDiff]);
                        }
                    }
                }
                
            }
            catch (Exception ex)
            {
                DisplayAlert("Oops", ex.Message, "k");
            }
        }

        /// <summary>
        /// This method will check that there is an operator, determine and perform operation.
        /// </summary>
        void CalculateAnswer()
        {
            try
            {
                int currentIndex = 0;
                double previousResult = 0;
                string currentOperator = null;

                foreach (string calculationItem in calculationString)
                {
                    if (currentIndex > 0)
                    {
                        //if the item is an operator
                        if (operatorString.Contains(calculationItem))
                        {
                            currentOperator = calculationItem;
                        }
                        else
                        {
                            //determine which operator to use and perform the operation on the previous result and the current value
                            switch (currentOperator)
                            {
                                case OPERATOR_ADD:
                                    previousResult += double.Parse(calculationItem);
                                    break;
                                case OPERATOR_SUBTRACT:
                                    previousResult -= double.Parse(calculationItem);
                                    break;
                                case OPERATOR_MULTIPLY:
                                    previousResult *= double.Parse(calculationItem);
                                    break;
                                case OPERATOR_DIVIDE:
                                    previousResult /= double.Parse(calculationItem);
                                    break;
                            }
                        }
                    }
                    else
                    {
                        previousResult = double.Parse(calculationItem);
                    }

                    currentIndex++;
                }

                UpdateDisplayValue(previousResult.ToString());  //update the display
                calculationString.Clear(); 
                calculationString.Add(previousResult.ToString());
            }
            catch (Exception e)
            {
                DisplayAlert("Error", String.Format("An error occured when calculating: {0}", e.Message), "OK");
            }
        }

        /// <summary>
        /// This method is used to update the display
        /// </summary>
        /// <param name="newValue"></param>
        void UpdateDisplayValue(string newValue)
        {
            //update the value
            numberDisplay.Text = newValue;

        }

        /// <summary>
        /// This method is to reset the calculation string and add a zero.
        /// </summary>
        void Reset()
        {
            UpdateDisplayValue(ZERO);
            calculationString.Clear();
            calculationString.Add(ZERO);
        }
    }
}

