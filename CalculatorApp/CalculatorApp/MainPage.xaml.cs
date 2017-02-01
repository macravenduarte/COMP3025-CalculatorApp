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
                        OPERATOR_SUB = "-",
                        OPERATOR_MULT = "X",
                        OPERATOR_DIV = "/",
                        DECIMAL = ".",
                        ZERO = "0";

        List<Button> operatorButtons, operandButtons, otherButtons; 
        List<String> calculationString; //concatination of strings to perform calculation

        public MainPage()
        {
            InitializeComponent();

            //add operation buttons to list
            operatorButtons = new List<Button>
            {

            }
        }
    }
}
