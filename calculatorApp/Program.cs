using Gtk;
using System;

class CalculatorApp : Window
{
    private Entry display;
    private double result = 0;
    private string lastOperator;
    private bool newInput = true;

    public CalculatorApp() : base("Simple Calculator")
    {
        SetDefaultSize(250, 200);
        SetPosition(WindowPosition.Center);
        DeleteEvent += delegate { Application.Quit(); };

        VBox vbox = new VBox();
        display = new Entry();
        vbox.PackStart(display, false, false, 5);

        Table table = new Table(4, 4, true);

        string[] labels = { "7", "8", "9", "/",
                            "4", "5", "6", "*",
                            "1", "2", "3", "-",
                            "0", "C", "=", "+" };

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
                    display.Text = result.ToString();
                    newInput = true;
                    lastOperator = "";
                    break;
                default:
                    if (!newInput)
                    {
                        Calculate(double.Parse(display.Text));
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
                result /= input;
                break;
            default:
                result = input;
                break;
        }
    }

    public static void Main()
    {
        Application.Init();
        new CalculatorApp();
        Application.Run();
    }
}
