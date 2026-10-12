using Microsoft.Maui.Dispatching;
using System.Globalization;

namespace GuitarFlashCards.Iphone;

public partial class MainPage : ContentPage
{
    private readonly GameEngine _game = new();
    private IDispatcherTimer? _roundTimer;
    private bool _isInitializing = true;

    public MainPage()
    {
        InitializeComponent();
        LoadPreferences();
        _isInitializing = false;

        UpdateAdvancedControls();
        ShowNextCard();
        if (CurrentMode == GameMode.Advanced) StartTimer();
    }

    private GameMode CurrentMode => modeAdvanced.IsChecked ? GameMode.Advanced : modeMedium.IsChecked ? GameMode.Medium : GameMode.Easy;
    private Clef CurrentClef => clefBass.IsChecked ? Clef.Bass : Clef.Treble;

    private void OptionsChanged(object sender, CheckedChangedEventArgs e)
    {
        if (_isInitializing || !e.Value) return;

        StopTimer();
        UpdateAdvancedControls();
        SavePreferences();
        ShowNextCard();
    }

    private void UpdateAdvancedControls()
    {
        advancedControls.IsVisible = CurrentMode == GameMode.Advanced;
    }

    private void ShowNextCard()
    {
        var card = _game.NextCard(CurrentMode, CurrentClef);
        // MAUI automatically pulls images from the Resources/Images folder
        noteImage.Source = $"{card.ResourceName}.png";

        if (CurrentMode == GameMode.Advanced) StartTimer();
    }

    private void AnswerClicked(object sender, EventArgs e)
    {
        if (sender is Button btn && btn.Text.Length > 0)
        {
            GradeRound(btn.Text[0]);
        }
    }

    private void GradeRound(char? answer)
    {
        var expected = _game.CurrentNote;
        var correct = _game.Grade(answer);

        feedbackText.Text = correct ? "Correct!" : "Incorrect!";
        feedbackText.TextColor = correct ? Colors.DarkGreen : Colors.DarkRed;
        previousText.Text = $"Previous note: {expected}";
        scoreText.Text = _game.ScoreText;

        if (CurrentMode == GameMode.Advanced) StopTimer();
        ShowNextCard();
    }

    private void ClearScore(object sender, EventArgs e)
    {
        _game.ClearScore();
        feedbackText.Text = string.Empty;
        previousText.Text = "Previous note: —";
        scoreText.Text = "Score: —";
    }

    private void ToggleTimer(object sender, EventArgs e)
    {
        if (_roundTimer != null && _roundTimer.IsRunning)
        {
            StopTimer();
            timerButton.Text = "Play";
        }
        else
        {
            StartTimer();
            timerButton.Text = "Stop";
        }
    }

    private void StartTimer()
    {
        if (_roundTimer != null && _roundTimer.IsRunning) return;

        var raw = timerSeconds.Text?.Trim();
        if (!double.TryParse(raw, NumberStyles.Float, CultureInfo.CurrentCulture, out var seconds) &&
            !double.TryParse(raw, NumberStyles.Float, CultureInfo.InvariantCulture, out seconds))
        {
            DisplayAlert("Invalid Input", "Enter a number of seconds.", "OK");
            return;
        }

        if (seconds is < 1 or > 60) return;

        _roundTimer = Dispatcher.CreateTimer();
        _roundTimer.Interval = TimeSpan.FromSeconds(seconds);
        _roundTimer.Tick += (s, e) => GradeRound(null);
        _roundTimer.Start();
        timerButton.Text = "Stop";
    }

    private void StopTimer()
    {
        _roundTimer?.Stop();
        timerButton.Text = "Play";
    }

    private async void ShowCheatSheet(object sender, EventArgs e)
    {
        // Recreates your Android AlertDialog cheat sheet using a MAUI Modal Page
        var clefName = CurrentClef == Clef.Treble ? "treble" : "bass";
        var cheatImage = new Image { HeightRequest = 360, Aspect = Aspect.AspectFit };
        var noteButtons = new HorizontalStackLayout { HorizontalOptions = LayoutOptions.Center, Spacing = 5 };

        void SetImage(char note) => cheatImage.Source = $"cheat_{clefName}_{char.ToLowerInvariant(note)}.png";

        foreach (var note in GameEngine.Notes)
        {
            var btn = new Button { Text = note.ToString(), WidthRequest = 45, HeightRequest = 45 };
            btn.Clicked += (_, _) => SetImage(note);
            noteButtons.Add(btn);
        }

        SetImage('A');

        var closeBtn = new Button { Text = "Close", HorizontalOptions = LayoutOptions.Center };
        var layout = new VerticalStackLayout { Padding = 20, Spacing = 15 };
        layout.Add(noteButtons);
        layout.Add(cheatImage);
        layout.Add(closeBtn);

        var modalPage = new ContentPage
        {
            Title = CurrentClef == Clef.Treble ? "Treble Cheat Sheet" : "Bass Cheat Sheet",
            Content = new ScrollView { Content = layout }
        };

        closeBtn.Clicked += async (_, _) => await Navigation.PopModalAsync();
        await Navigation.PushModalAsync(new NavigationPage(modalPage));
    }

    private void LoadPreferences()
    {
        var mode = Preferences.Default.Get("mode", "easy");
        var clef = Preferences.Default.Get("clef", "treble");
        timerSeconds.Text = Preferences.Default.Get("seconds", "5.5");

        modeMedium.IsChecked = mode == "medium";
        modeAdvanced.IsChecked = mode == "advanced";
        modeEasy.IsChecked = mode == "easy" || (!modeMedium.IsChecked && !modeAdvanced.IsChecked);

        clefBass.IsChecked = clef == "bass";
        clefTreble.IsChecked = clef == "treble" || !clefBass.IsChecked;
    }

    private void SavePreferences()
    {
        Preferences.Default.Set("mode", CurrentMode.ToString().ToLowerInvariant());
        Preferences.Default.Set("clef", CurrentClef.ToString().ToLowerInvariant());
        Preferences.Default.Set("seconds", timerSeconds.Text ?? "5.5");
    }

    protected override void OnDisappearing()
    {
        StopTimer();
        SavePreferences();
        base.OnDisappearing();
    }
}