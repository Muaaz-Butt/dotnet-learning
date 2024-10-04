using Gtk;
using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        Application.Init();
        new LoginSignupWindow();
        Application.Run();
    }
}

class LoginSignupWindow : Window
{
    private Entry usernameEntry;
    private Entry passwordEntry;
    private Dictionary<string, string> users;
    private Notebook notebook;
    private int totalUserCount;

    public LoginSignupWindow() : base("Login/Signup Page")
    {
        InitializeUsers();
        SetDefaultSize(300, 200);
        SetPosition(WindowPosition.Center);

        notebook = new Notebook();
        notebook.AppendPage(CreateLoginPage(), new Label("Login"));
        notebook.AppendPage(CreateSignupPage(), new Label("Signup"));

        Add(notebook);
        ShowAll();
    }

    private void InitializeUsers()
    {
        users = new Dictionary<string, string>
        {
            {"user1", "pass1"},
            {"user2", "pass2"},
            {"user3", "pass3"},
            {"user4", "pass4"},
            {"user5", "pass5"}
        };
        totalUserCount = users.Count; 
    }

    private Widget CreateLoginPage()
    {
        VBox vbox = new VBox(false, 5);
        vbox.BorderWidth = 10;

        Label usernameLabel = new Label("Username:");
        usernameEntry = new Entry();
        vbox.PackStart(usernameLabel, false, false, 0);
        vbox.PackStart(usernameEntry, false, false, 0);

        Label passwordLabel = new Label("Password:");
        passwordEntry = new Entry();
        passwordEntry.Visibility = false;
        vbox.PackStart(passwordLabel, false, false, 0);
        vbox.PackStart(passwordEntry, false, false, 0);

        Button loginButton = new Button("Login");
        loginButton.Clicked += OnLoginClicked;
        vbox.PackStart(loginButton, false, false, 0);

        return vbox;
    }

    private Widget CreateSignupPage()
    {
        VBox vbox = new VBox(false, 5);
        vbox.BorderWidth = 10;

        Label newUsernameLabel = new Label("New Username:");
        Entry newUsernameEntry = new Entry();
        vbox.PackStart(newUsernameLabel, false, false, 0);
        vbox.PackStart(newUsernameEntry, false, false, 0);

        Label newPasswordLabel = new Label("New Password:");
        Entry newPasswordEntry = new Entry();
        newPasswordEntry.Visibility = false;
        vbox.PackStart(newPasswordLabel, false, false, 0);
        vbox.PackStart(newPasswordEntry, false, false, 0);

        Label confirmPasswordLabel = new Label("Confirm Password:");
        Entry confirmPasswordEntry = new Entry();
        confirmPasswordEntry.Visibility = false;
        vbox.PackStart(confirmPasswordLabel, false, false, 0);
        vbox.PackStart(confirmPasswordEntry, false, false, 0);

        Button signupButton = new Button("Sign Up");
        signupButton.Clicked += (sender, e) => OnSignupClicked(newUsernameEntry, newPasswordEntry, confirmPasswordEntry);
        vbox.PackStart(signupButton, false, false, 0);

        return vbox;
    }

    private void OnLoginClicked(object sender, EventArgs args)
    {
        string username = usernameEntry.Text;
        string password = passwordEntry.Text;

        if (users.TryGetValue(username, out string storedPassword) && password == storedPassword)
        {
            ShowMessage(MessageType.Info, "Login Successful!", "Welcome, " + username + "!");
            new HomePage(username, users, this, ref totalUserCount);
            this.Hide();
        }
        else
        {
            ShowMessage(MessageType.Error, "Login Failed", "Invalid username or password.");
        }
    }

    private void OnSignupClicked(Entry newUsernameEntry, Entry newPasswordEntry, Entry confirmPasswordEntry)
    {
        string newUsername = newUsernameEntry.Text;
        string newPassword = newPasswordEntry.Text;
        string confirmPassword = confirmPasswordEntry.Text;

        if (string.IsNullOrWhiteSpace(newUsername) || string.IsNullOrWhiteSpace(newPassword))
        {
            ShowMessage(MessageType.Error, "Signup Failed", "Username and password cannot be empty.");
            return;
        }

        if (users.ContainsKey(newUsername))
        {
            ShowMessage(MessageType.Error, "Signup Failed", "Username already exists.");
            return;
        }
        if (newPassword != confirmPassword)
        {
            ShowMessage(MessageType.Error, "Signup Failed", "Passwords do not match.");
            return;
        }

        users.Add(newUsername, newPassword);
        totalUserCount++; 
        ShowMessage(MessageType.Info, "Signup Successful", "New user registered. You can now log in.");
        notebook.Page = 0; 
    }

    private void ShowMessage(MessageType messageType, string title, string message)
    {
        using (var dialog = new MessageDialog(this, DialogFlags.Modal, messageType, ButtonsType.Ok, message))
        {
            dialog.Title = title;
            dialog.Run();
            dialog.Destroy();
        }
    }

    public void UpdateTotalUserCount(int newCount)
    {
        totalUserCount = newCount;
    }
}

class HomePage : Window
{
    private string username;
    private Dictionary<string, string> users;
    private LoginSignupWindow loginWindow;
    private int totalUserCount;

    public HomePage(string username, Dictionary<string, string> users, LoginSignupWindow loginWindow, ref int totalUserCount) : base("Home Page")
    {
        this.username = username;
        this.users = users;
        this.loginWindow = loginWindow;
        this.totalUserCount = totalUserCount;

        SetDefaultSize(300, 200);
        SetPosition(WindowPosition.Center);

        VBox vbox = new VBox(false, 5);
        vbox.BorderWidth = 10;

        Label welcomeLabel = new Label($"Welcome, {username}!");
        vbox.PackStart(welcomeLabel, false, false, 0);

        Label totalUsersLabel = new Label($"Total Users: {totalUserCount}");
        vbox.PackStart(totalUsersLabel, false, false, 0);

        Button changePasswordButton = new Button("Change Password");
        changePasswordButton.Clicked += OnChangePasswordClicked;
        vbox.PackStart(changePasswordButton, false, false, 0);

        Button deleteButton = new Button("Delete User");
        deleteButton.Clicked += OnDeleteUserClicked;
        vbox.PackStart(deleteButton, false, false, 0);

        Button logoutButton = new Button("Logout");
        logoutButton.Clicked += OnLogoutClicked;
        vbox.PackStart(logoutButton, false, false, 0);

        Add(vbox);
        ShowAll();

        DeleteEvent += (o, args) => OnLogoutClicked(o, args);
    }

    private void OnChangePasswordClicked(object sender, EventArgs args)
    {
        Dialog dialog = new Dialog("Change Password", this, DialogFlags.Modal);
        dialog.SetDefaultSize(250, 150);

        Entry newPasswordEntry = new Entry();
        newPasswordEntry.Visibility = false;
        dialog.ContentArea.PackStart(new Label("New Password:"), false, false, 0);
        dialog.ContentArea.PackStart(newPasswordEntry, false, false, 0);

        dialog.AddButton("Cancel", ResponseType.Cancel);
        dialog.AddButton("Change", ResponseType.Ok);

        dialog.Response += (o, args) =>
        {
            if (args.ResponseId == ResponseType.Ok)
            {
                string newPassword = newPasswordEntry.Text;
                if (!string.IsNullOrWhiteSpace(newPassword))
                {
                    users[username] = newPassword;
                    ShowMessage(MessageType.Info, "Success", "Password changed successfully!");
                }
                else
                {
                    ShowMessage(MessageType.Error, "Error", "Password cannot be empty.");
                }
            }
            dialog.Destroy();
        };

        dialog.ShowAll();
    }

    private void OnDeleteUserClicked(object sender, EventArgs args)
    {
        bool confirm = ShowConfirmationDialog("Confirm Delete", "Are you sure you want to delete your account?");
        if (confirm)
        {
            users.Remove(username); 
            totalUserCount--; 
            loginWindow.UpdateTotalUserCount(totalUserCount);

            ShowMessage(MessageType.Info, "Account Deleted", "Your account has been deleted.");
            this.Destroy();
            loginWindow.Show(); 
        }
    }

    private bool ShowConfirmationDialog(string title, string message)
    {
        using (var dialog = new MessageDialog(this, DialogFlags.Modal, MessageType.Question, ButtonsType.YesNo, message))
        {
            dialog.Title = title;
            return dialog.Run() == (int)ResponseType.Yes;
        }
    }

    private void OnLogoutClicked(object sender, EventArgs args)
    {
        this.Destroy();
        loginWindow.Show(); 
    }

    private void ShowMessage(MessageType messageType, string title, string message)
    {
        using (var dialog = new MessageDialog(this, DialogFlags.Modal, messageType, ButtonsType.Ok, message))
        {
            dialog.Title = title;
            dialog.Run();
            dialog.Destroy();
        }
    }
}
