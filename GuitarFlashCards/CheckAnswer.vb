Public Class CheckAnswer
    Public Shared Function CheckAnswer(s As String) As Boolean
        If s.ToLower() = Form1.Label1.Text.ToLower() Then
            Form1.Label2.Text = "Correct!"
            Form1.Label2.ForeColor = Color.Black
            KeepTrack.KeepTrack(True)
            Form1.Label4.Text = KeepTrack.PullTrack
            Return True
        Else
            Form1.Label3.Show()
            Form1.Label2.Text = "Incorrect!"
            Form1.Label2.ForeColor = Color.Red
            KeepTrack.KeepTrack(False)
            Form1.Label4.Text = KeepTrack.PullTrack
            Return False
        End If
    End Function
End Class