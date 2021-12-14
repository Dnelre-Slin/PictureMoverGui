using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PictureMoverGui
{
    public class TimeTextBox : TextBox
    {
        private string timeString;
        public TimeTextBox()
        {
            TimeSeperationSymbol = ":";
            timeString = "00:00:00";
        }
        public string TimeSeperationSymbol { get; set; }
        private bool IsValidKey(int k)
        {
            return (k >= 34 && k <= 43) //digits 0 to 9
                || (k >= 74 && k <= 83) //numeric keypad 0 to 9
                //|| (k == 2) //back space
                //|| (k == 32) //delete
                ;
        }
        //private string Format(string text)
        //{
        //    string unformatedString = text == string.Empty ? "0,00" : text; //Initial state is always string.empty
        //    unformatedString = unformatedString.Replace(CurrencySymbol, ""); //Remove currency symbol from text
        //    unformatedString = unformatedString.Replace(DecimalSeparator, ""); //Remove separators (decimal)
        //    unformatedString = unformatedString.Replace(ThousandSeparator, ""); //Remove separators (thousands)
        //    decimal number = decimal.Parse(unformatedString) / (decimal)Math.Pow(10, CurrencyDecimalPlaces); //The value will have 'x' decimal places, so divide it by 10^x
        //    unformatedString = number.ToString("C", CultureInfo.CreateSpecificCulture(Culture));
        //    return unformatedString;
        //}
        //private decimal FormatBack(string text)
        //{
        //    string unformatedString = text == string.Empty ? "0.00" : text;
        //    unformatedString = unformatedString.Replace(CurrencySymbol, ""); //Remove currency symbol from text
        //    unformatedString = unformatedString.Replace(ThousandSeparator, ""); //Remove separators (thousands);
        //    CultureInfo current = Thread.CurrentThread.CurrentUICulture; //Let's change the culture to avoid "Input string was in an incorrect format"
        //    Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(Culture);
        //    decimal returnValue = decimal.Parse(unformatedString);
        //    Thread.CurrentThread.CurrentUICulture = current; //And now change it back, cuz we don't own the world, right?
        //    return returnValue;
        //}
        //private void ValueChanged(object sender, TextChangedEventArgs e)
        //{
        //    // Keep the caret at the end
        //    //System.Diagnostics.Trace.WriteLine("Value changed!");
        //    //this.CaretIndex = this.Text.Length;
        //}
        //private void MouseClicked(object sender, MouseButtonEventArgs e)
        //{
        //    // Prevent changing the caret index
        //    this.CaretIndex = 0;
        //    e.Handled = true;
        //    this.Focus();
        //}
        //private void MouseReleased(object sender, MouseButtonEventArgs e)
        //{
        //    // Prevent changing the caret index
        //    e.Handled = true;
        //    this.Focus();
        //}
        //private void KeyReleased(object sender, KeyEventArgs e)
        //{
        //    if (IsValidKey((int)e.Key))
        //    {
        //        //System.Diagnostics.Trace.WriteLine(this.Text);
        //        //string keyStr = e.Key.ToString();
        //        //System.Diagnostics.Trace.WriteLine(keyStr[keyStr.Length-1]);
        //        //this.Text = Format(this.Text);
        //        //this.Value = FormatBack(this.Text);
        //    }
        //    e.Handled = true;
        //}
        private void KeyPressed(object sender, KeyEventArgs e)
        {
            if (IsValidKey((int)e.Key))
            {
                System.Diagnostics.Trace.WriteLine(this.Text);
                string keyStr = e.Key.ToString();
                string nrEntered = keyStr.Substring(keyStr.Length - 1);
                System.Diagnostics.Trace.WriteLine(nrEntered);
                this.timeString = this.timeString.Substring(0, this.CaretIndex) + nrEntered + this.timeString.Substring(this.CaretIndex + 1);
                this.CaretIndex++;
                this.Text = this.timeString;
            }
            e.Handled = true;
            //this.CaretIndex = this.Text.Length;
        }
        private void PastingEventHandler(object sender, DataObjectEventArgs e)
        {
            // Prevent/disable paste
            e.CancelCommand();
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            DataObject.AddCopyingHandler(this, PastingEventHandler);
            DataObject.AddPastingHandler(this, PastingEventHandler);
            //this.CaretIndex = this.Text.Length;
            this.KeyDown += KeyPressed;
            //this.KeyUp += KeyReleased;
            //this.PreviewMouseDown += MouseClicked;
            //this.PreviewMouseUp += MouseReleased;
            //this.TextChanged += ValueChanged;
            this.Text = string.Empty;
        }
        //public string Value
        //{
        //    get { return this.GetValue(ValueProperty).ToString(); }
        //    set { this.SetValue(ValueProperty, value); }
        //}

        //public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
        //    "Value",
        //    typeof(string),
        //    typeof(TimeTextBox),
        //    new FrameworkPropertyMetadata(new string(""), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(ValuePropertyChanged)));
        //private static void ValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    ((TimeTextBox)d).Value = e.NewValue.ToString();
        //}
    }
}