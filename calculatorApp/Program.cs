using Gtk;
using System;
using System.Collections.Generic;

class CalculatorApp : Window
{
    private Entry display;
    private double result = 0;
    private string lastOperator;
    private bool newInput = true;
    private List<string> history; 
    public CalculatorApp() : base("Simple Calculator")
    {
        history = new List<string>(); 
        SetDefaultSize(250, 200);
        SetPosition(WindowPosition.Center);
        DeleteEvent += delegate { Application.Quit(); };

        VBox vbox = new VBox();
        display = new Entry();
        vbox.PackStart(display, false, false, 5);

        Table table = new Table(5, 4, true); 

        string[] labels = { "7", "8", "9", "/",
                            "4", "5", "6", "*",
                            "1", "2", "3", "-",
                            "0", "C", "=", "+",
                            "History" }; 

        uint i = 0;
        foreach (string label in labels)
        {
            Button btn = new Button(label);
            btn.Clicked += OnButtonClicked;
            table.Attach(btn, i % 4, (i % 4) + 1, i / 4, (i / 4) + 1);
            i++;
        }

        vbox.PackStart(table, true, true, 0);
        Add(vbox);
        ShowAll();
    }

    void OnButtonClicked(object sender, EventArgs args)
    {
        Button button = (Button)sender;
        string label = button.Label;

        if (double.TryParse(label, out double number))
        {
            if (newInput)
            {
                display.Text = label;
                newInput = false;
            }
            else
            {
                display.Text += label;
            }
        }
        else
        {
            switch (label)
            {
                case "C":
                    display.Text = "";
                    result = 0;
                    lastOperator = "";
                    break;
                case "=":
                    Calculate(double.Parse(display.Text));
                    string calculationResult = result.ToString();
                    history.Add($"{display.Text} = {calculationResult}"); 
                    display.Text = calculationResult;/
                    newInput = true;
                    lastOperator = "";
                    break;
                case "History":
                    ShowHistory(); 
                    break;
                default:
                    if (!newInput)
                    {
                        Calculate(double.Parse(display.Text));
                        string currentExpression = $"{result} {lastOperator} {display.Text}";
                        history.Add(currentExpression + $" = {result}"); 
                        display.Text = result.ToString();
                        newInput = true;
                    }
                    lastOperator = label;
                    break;
            }
        }
    }

    void Calculate(double input)
    {
        switch (lastOperator)
        {
            case "+":
                result += input;
                break;
            case "-":
                result -= input;
                break;
            case "*":
                result *= input;
                break;
            case "/":
                if (input != 0)
                    result /= input;
                else
                    display.Text = "Error"; 
                break;
            default:
                result = input;
                break;
        }
    }

    void ShowHistory()
    {
        if (history.Count == 0)
        {
            MessageDialog dialog = new MessageDialog(this, DialogFlags.DestroyWithParent, MessageType.Info, ButtonsType.Ok, "No history available.");
            dialog.Title = "History";
            dialog.Run();
            dialog.Destroy();
            return;
        }

        string historyText = string.Join("\n", history);
        MessageDialog historyDialog = new MessageDialog(this, DialogFlags.DestroyWithParent, MessageType.Info, ButtonsType.Ok, historyText);
        historyDialog.Title = "Calculation History";
        historyDialog.Run();
        historyDialog.Destroy();
    }

    public static void Main()
    {
        Application.Init();
        new CalculatorApp();
        Application.Run();
    }
}
