Imports System.ComponentModel
Public Class Form1
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadSettings.LoadSettings()
        Dim s As String = DetermineLevel.DetermineLevel()
        SetLevel.SetLevel(s)
    End Sub
    Private Sub Form1_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        SaveSettings.SaveSettings()
    End Sub
    Private Sub TextBox1_KeyUp(sender As Object, e As KeyEventArgs) Handles TextBox1.KeyUp
        If Not TextBox1.Text = String.Empty Then
            Label3.Hide()
            e.Handled = True
            CheckAnswer.CheckAnswer(TextBox1.Text)
            Dim s = DetermineLevel.DetermineLevel
            If SetLevel.t.Enabled Then
                SetLevel.timerStop()
            End If
            SetLevel.SetLevel(s)
            TextBox1.Clear()
        End If
    End Sub
    Private Sub RadioButton3_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton3.CheckedChanged
        If RadioButton3.Checked = True Then
            TextBox2.Visible = True
            Label5.Visible = True
            Button1.Visible = True
            If Not SetLevel.t.Enabled Then
                SetLevel.SetLevel("advanced")
            End If
        Else
            TextBox2.Visible = False
            Label5.Visible = False
            Button1.Visible = False
            SetLevel.timerStop()
        End If
    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If SetLevel.t.Enabled Then
            SetLevel.timerStop()
            Button1.Text = "Play"
        Else
            SetLevel.t.Start()
            Button1.Text = "Stop"
        End If
    End Sub
    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        KeepTrack.ClearTrack()
        Label4.Text = String.Empty
        Label3.Visible = False
        Label2.Visible = False
    End Sub
    Private Sub RadioButton1_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton1.CheckedChanged
        SetLevel.SetLevel("easy")
    End Sub

    Private Sub RadioButton4_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton4.CheckedChanged
        If RadioButton4.Checked Then
            Dim s As String = DetermineLevel.DetermineLevel()
            SetLevel.SetLevel(s)
        Else
            Dim s As String = DetermineLevel.DetermineLevel()
            SetLevel.SetLevel(s)
        End If
    End Sub

    Private Sub RadioButton1_Click(sender As Object, e As EventArgs) Handles RadioButton1.Click, RadioButton2.Click, RadioButton3.Click, RadioButton4.Click, RadioButton5.Click, Button1.Click, LinkLabel1.Click, Me.Click
        TextBox1.Focus()
    End Sub

    Private Sub LinkLabel2_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel2.LinkClicked
        Panel1.Show()
        Panel1.BringToFront()
    End Sub

    Private Sub LinkLabel3_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel3.LinkClicked
        Panel1.Hide()
    End Sub
    Private Sub RadioButton6_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton6.CheckedChanged, RadioButton7.CheckedChanged, RadioButton8.CheckedChanged, RadioButton9.CheckedChanged, RadioButton10.CheckedChanged, RadioButton11.CheckedChanged, RadioButton12.CheckedChanged
        Dim rad As RadioButton = DirectCast(sender, RadioButton)
        Dim radname As String = String.Empty
        If rad.Checked Then
            radname = rad.Name
        End If
        'can rename radio buttons to end in the note name, parse 'radiobutton' to get the note name and use it to set the images dynamically.
        If RadioButton4.Checked Then
            Select Case radname
                Case "RadioButton6"
                    PictureBox1.Image = Image.FromFile($"CheatNotePics\AS.png")
                Case "RadioButton7"
                    PictureBox1.Image = Image.FromFile($"CheatNotePics\BS.png")
                Case "RadioButton8"
                    PictureBox1.Image = Image.FromFile($"CheatNotePics\CS.png")
                Case "RadioButton9"
                    PictureBox1.Image = Image.FromFile($"CheatNotePics\DS.png")
                Case "RadioButton10"
                    PictureBox1.Image = Image.FromFile($"CheatNotePics\ES.png")
                Case "RadioButton11"
                    PictureBox1.Image = Image.FromFile($"CheatNotePics\FS.png")
                Case "RadioButton12"
                    PictureBox1.Image = Image.FromFile($"CheatNotePics\GS.png")
            End Select
        Else
            Select Case radname
                Case "RadioButton6"
                    PictureBox1.Image = Image.FromFile($"CheatBassNotePics\AAS.png")
                Case "RadioButton7"
                    PictureBox1.Image = Image.FromFile($"CheatBassNotePics\BBS.png")
                Case "RadioButton8"
                    PictureBox1.Image = Image.FromFile($"CheatBassNotePics\CCS.png")
                Case "RadioButton9"
                    PictureBox1.Image = Image.FromFile($"CheatBassNotePics\DDS.png")
                Case "RadioButton10"
                    PictureBox1.Image = Image.FromFile($"CheatBassNotePics\EES.png")
                Case "RadioButton11"
                    PictureBox1.Image = Image.FromFile($"CheatBassNotePics\FFS.png")
                Case "RadioButton12"
                    PictureBox1.Image = Image.FromFile($"CheatBassNotePics\GGS.png")
            End Select
        End If
    End Sub
    'Stefan Christian
End Class