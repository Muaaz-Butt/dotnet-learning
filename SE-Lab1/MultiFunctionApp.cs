using System;
using Gtk;
using Gdk;

class MultiFunctionApp : Window
{
    private Entry txtHelloWorld;
    private Entry txtChangeColor;
    private Entry txtPakistan;
    private Entry txtUsername;
    private Entry txtPassword;
    private Label lblLoginStatus;

    public MultiFunctionApp() : base("Multi-Function GTK# App")
    {
        SetDefaultSize(400, 300);
        SetPosition(WindowPosition.Center);
        DeleteEvent += delegate { Application.Quit(); };

        var vbox = new VBox(false, 5);
        vbox.BorderWidth = 10;

        var hboxHello = new HBox(false, 5);
        var btnHelloWorld = new Button("Show Hello World");
        txtHelloWorld = new Entry();
        btnHelloWorld.Clicked += BtnHelloWorld_Clicked;
        hboxHello.PackStart(btnHelloWorld, false, false, 0);
        hboxHello.PackStart(txtHelloWorld, true, true, 0);
        vbox.PackStart(hboxHello, false, false, 0);

        var hboxColor = new HBox(false, 5);
        var btnChangeColor = new Button("Change Color");
        txtChangeColor = new Entry();
        btnChangeColor.Clicked += BtnChangeColor_Clicked;
        hboxColor.PackStart(btnChangeColor, false, false, 0);
        hboxColor.PackStart(txtChangeColor, true, true, 0);
        vbox.PackStart(hboxColor, false, false, 0);

        var hboxPakistan = new HBox(false, 5);
        var btnCheckPakistan = new Button("Check Pakistan");
        txtPakistan = new Entry();
        btnCheckPakistan.Clicked += BtnCheckPakistan_Clicked;
        hboxPakistan.PackStart(btnCheckPakistan, false, false, 0);
        hboxPakistan.PackStart(txtPakistan, true, true, 0);
        vbox.PackStart(hboxPakistan, false, false, 0);

        var hboxUsername = new HBox(false, 5);
        txtUsername = new Entry();
        txtUsername.PlaceholderText = "Username";
        hboxUsername.PackStart(new Label("Username:"), false, false, 0);
        hboxUsername.PackStart(txtUsername, true, true, 0);
        vbox.PackStart(hboxUsername, false, false, 0);

        var hboxPassword = new HBox(false, 5);
        txtPassword = new Entry();
        txtPassword.PlaceholderText = "Password";
        txtPassword.Visibility = false;
        hboxPassword.PackStart(new Label("Password:"), false, false, 0);
        hboxPassword.PackStart(txtPassword, true, true, 0);
        vbox.PackStart(hboxPassword, false, false, 0);

        var btnLogin = new Button("Login");
        btnLogin.Clicked += BtnLogin_Clicked;
        vbox.PackStart(btnLogin, false, false, 0);

        lblLoginStatus = new Label("");
        vbox.PackStart(lblLoginStatus, false, false, 0);

        Add(vbox);
    }

    private void BtnHelloWorld_Clicked(object sender, EventArgs e)
    {
        txtHelloWorld.Text = "Hello World";
        var md = new MessageDialog(this, DialogFlags.Modal, MessageType.Info, ButtonsType.Ok, "Hello World");
        md.Run();
        md.Destroy();
    }

    private void BtnChangeColor_Clicked(object sender, EventArgs e)
    {
        txtChangeColor.ModifyBg(StateType.Normal, new Gdk.Color(255, 0, 0));
    }

    private void BtnCheckPakistan_Clicked(object sender, EventArgs e)
    {
        string message = txtPakistan.Text.Trim().Equals("Pakistan", StringComparison.OrdinalIgnoreCase)
            ? "Welcome"
            : "Good Bye";
        var md = new MessageDialog(this, DialogFlags.Modal, MessageType.Info, ButtonsType.Ok, message);
        md.Run();
        md.Destroy();
    }

    private void BtnLogin_Clicked(object sender, EventArgs e)
    {
        string username = txtUsername.Text.Trim();
        string password = txtPassword.Text;

        bool isValid = (username == "admin" && password == "password") ||
                       (username == "user" && password == "123456") ||
                       (username == "test" && password == "test123");

        if (isValid)
        {
            lblLoginStatus.Text = "Valid User";
            lblLoginStatus.ModifyFg(StateType.Normal, new Gdk.Color(0, 255, 0));
        }
        else
        {
            lblLoginStatus.Text = "Invalid Username or Password";
            lblLoginStatus.ModifyFg(StateType.Normal, new Gdk.Color(255, 0, 0));
        }
    }

    public static void Main()
    {
        Application.Init();
        new MultiFunctionApp().ShowAll();
        Application.Run();
    }
}