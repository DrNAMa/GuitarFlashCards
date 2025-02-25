Public Class SetLevel
    Public Shared t As New Timer With {
                    .Interval = Val(Form1.TextBox2.Text) * 1000
                }
    Private Shared b As Boolean
    Public Shared Sub SetLevel(s As String)
        Select Case s
            Case "easy"
                Dim e As New EasyMode
                e.Execute()
            Case "medium"
                Dim m As New MediumMode
                m.Execute()
            Case "advanced"
                If Not b Then
                    AddHandler t.Tick, AddressOf timertick
                    b = True
                End If
                Dim a As New MediumMode
                a.Execute()
                t.Start()
        End Select
    End Sub
    Public Shared Sub timertick()
        CheckAnswer.CheckAnswer(Form1.TextBox1.Text)
        Dim a As New MediumMode
        a.Execute()
    End Sub
    Public Shared Sub timerStop()
        t.Stop()
    End Sub
End Class