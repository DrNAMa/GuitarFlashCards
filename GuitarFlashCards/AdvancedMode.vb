Public Class AdvancedMode
    Private Shared note As Char
    Public Sub New()
        note = PickRandomNote()
    End Sub
    Public Sub Execute()
    End Sub
    Private Function PickRandomNote() As Char
        Dim l As List(Of Char) = New List(Of Char) From {"A", "B", "C", "D", "E", "F", "G"}
        Dim r As New Random
        Dim i As Integer = r.Next(0, l.Count)
        If l(i) = note Then
            PickRandomNote()
            Return ""
        End If
        note = l(i)
        Dim ii As Integer
        If note = "E" Or note = "F" Then
            Dim rr As New Random
            ii = rr.Next(1, 5)
        Else
            Dim rr As New Random
            ii = rr.Next(1, 4)
        End If
        If Form1.Label1.Text IsNot Nothing Then
            Form1.Label3.Text = Form1.Label1.Text
        End If
        Form1.Label1.Text = l(i)
        Form1.PictureBox1.Image = Image.FromFile($"NotePics\" & l(i) & ii & ".png")
        Return l(i)
    End Function
    'kept as a cleaner baseline for easy/medium mode.
End Class