Public Class LoadSettings
    Public Shared Sub LoadSettings()
        Select Case My.Settings.level
            Case "easy"
                Form1.RadioButton1.Checked = True
            Case "medium"
                Form1.RadioButton2.Checked = True
            Case "advanced"
                Form1.RadioButton3.Checked = True
            Case Else
                Form1.RadioButton1.Checked = True
        End Select
        Select Case My.Settings.clef
            Case "treble"
                Form1.RadioButton4.Checked = True
            Case "bass"
                Form1.RadioButton5.Checked = True
            Case Else
                Form1.RadioButton4.Checked = True
        End Select
    End Sub
End Class