Public Class EasyMode
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
        If Form1.RadioButton4.Checked Then
            If note = "E" Or note = "F" Then
                Dim rr As New Random
                ii = rr.Next(2, 4)
            Else
                ii = 2
            End If
        Else
            If note = "G" Or note = "A" Then
                Dim rr As New Random
                ii = rr.Next(2, 4)
            Else
                ii = 2
            End If
        End If
        If Form1.Label1.Text IsNot Nothing Then
            Form1.Label3.Text = Form1.Label1.Text
        End If
        Form1.Label1.Text = l(i)
        If Form1.RadioButton4.Checked Then
            Form1.PictureBox1.Image = Image.FromFile($"NotePics\" & l(i) & ii & ".png")
        Else
            Form1.PictureBox1.Image = Image.FromFile($"BassNotePics\" & l(i) & l(i) & ii & ".png")
        End If
        Return l(i)
    End Function
End Class