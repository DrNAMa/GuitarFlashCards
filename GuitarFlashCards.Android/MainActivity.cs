using System.Globalization;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;

namespace GuitarFlashCards.Android;

[Activity(
    Label = "@string/app_name",
    MainLauncher = true,
    ScreenOrientation = ScreenOrientation.Unspecified,
    ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize)]
public sealed class MainActivity : Activity
{
    private const string PreferenceFile = "guitar_flash_cards";

    private readonly GameEngine _game = new();
    private System.Timers.Timer? _roundTimer;

    private RadioGroup _modeGroup = null!;
    private RadioGroup _clefGroup = null!;
    private ImageView _noteImage = null!;
    private EditText _timerSeconds = null!;
    private TextView _feedbackText = null!;
    private TextView _previousText = null!;
    private TextView _scoreText = null!;
    private LinearLayout _advancedControls = null!;
    private Button _timerButton = null!;

    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        SetContentView(Resource.Layout.activity_main);

        BindViews();
        LoadPreferences();
        WireEvents();
        UpdateAdvancedControls();
        ShowNextCard();
        if (CurrentMode == GameMode.Advanced)
        {
            StartTimer();
        }
    }

    protected override void OnPause()
    {
        StopTimer();
        SavePreferences();
        base.OnPause();
    }

    protected override void OnDestroy()
    {
        StopTimer();
        base.OnDestroy();
    }

    private void BindViews()
    {
        _modeGroup = FindViewById<RadioGroup>(Resource.Id.modeGroup)!;
        _clefGroup = FindViewById<RadioGroup>(Resource.Id.clefGroup)!;
        _noteImage = FindViewById<ImageView>(Resource.Id.noteImage)!;
        _timerSeconds = FindViewById<EditText>(Resource.Id.timerSeconds)!;
        _feedbackText = FindViewById<TextView>(Resource.Id.feedbackText)!;
        _previousText = FindViewById<TextView>(Resource.Id.previousText)!;
        _scoreText = FindViewById<TextView>(Resource.Id.scoreText)!;
        _advancedControls = FindViewById<LinearLayout>(Resource.Id.advancedControls)!;
        _timerButton = FindViewById<Button>(Resource.Id.timerButton)!;
    }

    private void WireEvents()
    {
        _modeGroup.CheckedChange += (_, _) => OptionsChanged();
        _clefGroup.CheckedChange += (_, _) => OptionsChanged();

        foreach (var (id, note) in new[]
                 {
                     (Resource.Id.answerA, 'A'),
                     (Resource.Id.answerB, 'B'),
                     (Resource.Id.answerC, 'C'),
                     (Resource.Id.answerD, 'D'),
                     (Resource.Id.answerE, 'E'),
                     (Resource.Id.answerF, 'F'),
                     (Resource.Id.answerG, 'G')
                 })
        {
            FindViewById<Button>(id)!.Click += (_, _) => GradeRound(note);
        }

        FindViewById<Button>(Resource.Id.clearButton)!.Click += (_, _) => ClearScore();
        FindViewById<Button>(Resource.Id.cheatButton)!.Click += (_, _) => ShowCheatSheet();
        _timerButton.Click += (_, _) => ToggleTimer();
    }

    private void OptionsChanged()
    {
        StopTimer();
        UpdateAdvancedControls();
        SavePreferences();
        ShowNextCard();
        if (CurrentMode == GameMode.Advanced)
        {
            StartTimer();
        }
    }

    private void UpdateAdvancedControls()
    {
        _advancedControls.Visibility = CurrentMode == GameMode.Advanced
            ? ViewStates.Visible
            : ViewStates.Gone;
    }

    private void ShowNextCard()
    {
        var card = _game.NextCard(CurrentMode, CurrentClef);
        var resourceId = DrawableId(card.ResourceName);
        _noteImage.SetImageResource(resourceId);
    }

    private void GradeRound(char? answer)
    {
        var expected = _game.CurrentNote;
        var correct = _game.Grade(answer);

        _feedbackText.Text = correct ? "Correct!" : "Incorrect!";
        _feedbackText.SetTextColor(correct ? Color.Rgb(22, 101, 52) : Color.Rgb(185, 28, 28));
        _previousText.Text = $"Previous note: {expected}";
        _scoreText.Text = _game.ScoreText;

        ShowNextCard();
    }

    private void ClearScore()
    {
        _game.ClearScore();
        _feedbackText.Text = string.Empty;
        _previousText.SetText(Resource.String.previous_empty);
        _scoreText.SetText(Resource.String.score_empty);
    }

    private void ToggleTimer()
    {
        if (_roundTimer is not null)
        {
            StopTimer();
            return;
        }

        StartTimer();
    }

    private void StartTimer()
    {
        if (_roundTimer is not null)
        {
            return;
        }

        var raw = _timerSeconds.Text?.Trim();
        if (!double.TryParse(raw, NumberStyles.Float, CultureInfo.CurrentCulture, out var seconds) &&
            !double.TryParse(raw, NumberStyles.Float, CultureInfo.InvariantCulture, out seconds))
        {
            _timerSeconds.Error = "Enter a number of seconds.";
            return;
        }

        if (seconds is < 1 or > 60)
        {
            _timerSeconds.Error = "Use a value from 1 to 60 seconds.";
            return;
        }

        _roundTimer = new System.Timers.Timer(seconds * 1000)
        {
            AutoReset = true
        };
        _roundTimer.Elapsed += (_, _) => RunOnUiThread(() => GradeRound(null));
        _roundTimer.Start();
        _timerButton.SetText(Resource.String.stop);
    }

    private void StopTimer()
    {
        if (_roundTimer is null)
        {
            _timerButton?.SetText(Resource.String.play);
            return;
        }

        _roundTimer.Stop();
        _roundTimer.Dispose();
        _roundTimer = null;
        _timerButton.SetText(Resource.String.play);
    }

    private void ShowCheatSheet()
    {
        var content = new LinearLayout(this)
        {
            Orientation = Orientation.Vertical
        };
        content.SetPadding(Dp(12), Dp(4), Dp(12), Dp(8));

        var noteButtons = new GridLayout(this)
        {
            ColumnCount = 7,
            RowCount = 1
        };

        var cheatImage = new ImageView(this);
        cheatImage.SetScaleType(ImageView.ScaleType.FitCenter);
        cheatImage.SetAdjustViewBounds(true);
        cheatImage.SetBackgroundColor(Color.White);

        foreach (var note in GameEngine.Notes)
        {
            var button = new Button(this)
            {
                Text = note.ToString()
            };
            button.SetMinWidth(0);
            button.SetMinimumWidth(0);
            button.SetMinHeight(0);
            button.SetMinimumHeight(0);
            button.SetPadding(0, 0, 0, 0);
            button.Click += (_, _) => SetCheatImage(cheatImage, note);
            noteButtons.AddView(button, new ViewGroup.LayoutParams(Dp(42), Dp(44)));
        }

        content.AddView(noteButtons, new LinearLayout.LayoutParams(
            ViewGroup.LayoutParams.MatchParent,
            ViewGroup.LayoutParams.WrapContent));
        content.AddView(cheatImage, new LinearLayout.LayoutParams(
            ViewGroup.LayoutParams.MatchParent,
            Dp(360)));

        SetCheatImage(cheatImage, 'A');

        var title = CurrentClef == Clef.Treble ? "Treble cheat sheet" : "Bass cheat sheet";
        var builder = new AlertDialog.Builder(this);
        builder.SetTitle(title);
        builder.SetView(content);
        builder.SetNegativeButton("Close", (_, _) => { });
        builder.Show();
    }

    private void SetCheatImage(ImageView image, char note)
    {
        var clefName = CurrentClef == Clef.Treble ? "treble" : "bass";
        image.SetImageResource(DrawableId($"cheat_{clefName}_{char.ToLowerInvariant(note)}"));
        image.ContentDescription = $"{CurrentClef} {note} cheat sheet";
    }

    private int DrawableId(string resourceName)
    {
        var id = Resources?.GetIdentifier(resourceName, "drawable", PackageName) ?? 0;
        return id != 0
            ? id
            : throw new InvalidOperationException($"Missing Android drawable: {resourceName}");
    }

    private void LoadPreferences()
    {
        var preferences = GetSharedPreferences(PreferenceFile, FileCreationMode.Private)!;
        var mode = preferences.GetString("mode", "easy");
        var clef = preferences.GetString("clef", "treble");
        var seconds = preferences.GetString("seconds", "5.5");

        _modeGroup.Check(mode switch
        {
            "medium" => Resource.Id.modeMedium,
            "advanced" => Resource.Id.modeAdvanced,
            _ => Resource.Id.modeEasy
        });
        _clefGroup.Check(clef == "bass" ? Resource.Id.clefBass : Resource.Id.clefTreble);
        _timerSeconds.Text = seconds;
    }

    private void SavePreferences()
    {
        var preferences = GetSharedPreferences(PreferenceFile, FileCreationMode.Private)!;
        using var editor = preferences.Edit();
        editor?.PutString("mode", CurrentMode.ToString().ToLowerInvariant());
        editor?.PutString("clef", CurrentClef.ToString().ToLowerInvariant());
        editor?.PutString("seconds", _timerSeconds.Text ?? "5.5");
        editor?.Apply();
    }

    private GameMode CurrentMode => _modeGroup.CheckedRadioButtonId switch
    {
        Resource.Id.modeMedium => GameMode.Medium,
        Resource.Id.modeAdvanced => GameMode.Advanced,
        _ => GameMode.Easy
    };

    private Clef CurrentClef => _clefGroup.CheckedRadioButtonId == Resource.Id.clefBass
        ? Clef.Bass
        : Clef.Treble;

    private int Dp(int value) => (int)(value * Resources!.DisplayMetrics!.Density + 0.5f);
}
