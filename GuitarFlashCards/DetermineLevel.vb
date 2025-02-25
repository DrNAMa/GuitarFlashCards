Public Class DetermineLevel
    Public Shared Function DetermineLevel() As String
        If Form1.RadioButton1.Checked Then
            Return "easy"
        ElseIf Form1.RadioButton2.Checked Then
            Return "medium"
        ElseIf Form1.RadioButton3.Checked Then
            Return "advanced"
        Else
            Return "nosetting"
        End If
    End Function
End Class