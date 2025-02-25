Public Class KeepTrack
    Private Shared l As New List(Of Boolean)
    Public Shared Sub KeepTrack(b As Boolean)
        l.Add(b)
    End Sub
    Public Shared Function PullTrack() As String
        Dim a As Integer = l.Count
        Dim b As Integer = l.Where(Function(item) item = True).Count
        Dim c As Double = (b / a) * 100
        Dim d As String = Format(c, "0.00").ToString() & "%"
        Return d
    End Function
    Public Shared Sub ClearTrack()
        l.Clear()
    End Sub
End Class