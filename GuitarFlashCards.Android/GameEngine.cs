namespace GuitarFlashCards.Android;

public enum GameMode
{
    Easy,
    Medium,
    Advanced
}

public enum Clef
{
    Treble,
    Bass
}

public readonly record struct FlashCard(char Note, int Position, string ResourceName);

public sealed class GameEngine
{
    public static readonly char[] Notes = ['A', 'B', 'C', 'D', 'E', 'F', 'G'];

    private char? _lastNote;

    public char CurrentNote { get; private set; }
    public int Attempts { get; private set; }
    public int CorrectAnswers { get; private set; }

    public string ScoreText => Attempts == 0
        ? "Score: —"
        : $"Score: {(double)CorrectAnswers / Attempts:P2} ({CorrectAnswers}/{Attempts})";

    public FlashCard NextCard(GameMode mode, Clef clef)
    {
        char note;
        do
        {
            note = Notes[Random.Shared.Next(Notes.Length)];
        }
        while (note == _lastNote);

        _lastNote = note;
        CurrentNote = note;

        var position = PickPosition(note, mode, clef);
        var clefName = clef == Clef.Treble ? "treble" : "bass";
        return new FlashCard(
            note,
            position,
            $"{clefName}_{char.ToLowerInvariant(note)}{position}");
    }

    public bool Grade(char? answer)
    {
        Attempts++;
        var correct = answer.HasValue && char.ToUpperInvariant(answer.Value) == CurrentNote;
        if (correct)
        {
            CorrectAnswers++;
        }

        return correct;
    }

    public void ClearScore()
    {
        Attempts = 0;
        CorrectAnswers = 0;
    }

    private static int PickPosition(char note, GameMode mode, Clef clef)
    {
        if (mode == GameMode.Easy)
        {
            var hasTwoMiddlePositions = clef == Clef.Treble
                ? note is 'E' or 'F'
                : note is 'G' or 'A';
            return hasTwoMiddlePositions ? Random.Shared.Next(2, 4) : 2;
        }

        var positionCount = clef == Clef.Treble
            ? note is 'E' or 'F' ? 4 : 3
            : note is 'G' or 'A' ? 4 : 3;
        return Random.Shared.Next(1, positionCount + 1);
    }
}
