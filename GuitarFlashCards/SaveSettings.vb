Public Class SaveSettings
    Public Shared Sub SaveSettings()
        If Form1.RadioButton1.Checked Then
            My.Settings.level = "easy"
        ElseIf Form1.RadioButton2.Checked Then
            My.Settings.level = "medium"
        ElseIf Form1.RadioButton3.Checked Then
            My.Settings.level = "advanced"
        End If
        If Form1.RadioButton4.Checked Then
            My.Settings.clef = "treble"
        ElseIf Form1.RadioButton5.Checked Then
            My.Settings.clef = "bass"
        End If
        My.Settings.Save()
    End Sub
End Class