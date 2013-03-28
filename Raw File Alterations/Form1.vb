Imports System
Imports System.IO
Imports System.Text
Imports System.Collections.Generic
Public Class Form1
    Dim fd As FileDialog = New OpenFileDialog()
    Dim savefilename As String
    Dim strFilename As String
    Dim filereader As StreamReader
    Dim filemaker As StreamWriter
    Dim totalrowcount As Integer
    Dim line As String
    Dim timequit As String
    Dim tagid1 As String
    Dim lineSplit As String()
    Dim timetest As Decimal
    Dim noms As Integer
    Dim hertz As String
    Dim sequencelist As New List(Of Integer)
    Dim sequencecounter As Integer = 1
    Dim evennumbers As New List(Of Decimal)
    Dim divisiblebyfour As Decimal

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        'This function strips out all lines of the RAW file that do not contain the hexadecimal TagID indicated by the user.
        fd.Title = "Choose RAW File"
        fd.InitialDirectory = "C:\Users\jsharitt\Desktop\ForJason\PROBEEF\Dress Rehearsal"
        fd.Filter = "All files (*.txt)|*.txt|All Files (*.txt)|*.txt"
        fd.FilterIndex = 2
        fd.RestoreDirectory = True
        If fd.ShowDialog() = Windows.Forms.DialogResult.OK Then
            strFilename = fd.FileName
        End If
        If strFilename = "" Then
            MsgBox("You must choose a file to process", , "ERROR")
        ElseIf strFilename <> "" Then
            savefilename = fd.FileName
            tagid1 = InputBox("Input hexadecimal TagID to search for", "TagID Entry", , 250, 75)
            filereader = My.Computer.FileSystem.OpenTextFileReader(strFilename)
            totalrowcount = 0
            Do Until filereader.EndOfStream
                filereader.ReadLine()
                totalrowcount += 1
            Loop
            filereader.Close()
            Dim progressmax As Integer = totalrowcount
            ProgressBar1.Maximum = progressmax
            ProgressBar1.Minimum = 0
            filereader = My.Computer.FileSystem.OpenTextFileReader(strFilename)
            filemaker = New StreamWriter(savefilename.Substring(0, savefilename.Length - 4) + "_OnlyTagID_" + tagid1 + ".txt", False)
            Do Until filereader.EndOfStream()
                line = filereader.ReadLine()
                If line = "" Then
                    filemaker.WriteLine(line)
                ElseIf line <> "" Then
                    If line.Contains(",") Then
                        lineSplit = line.Split(New [Char]() {","})
                        If lineSplit.Count = 17 Then
                            If lineSplit(5).Contains(tagid1) Then
                                filemaker.WriteLine(line)
                            End If
                        Else
                            filemaker.WriteLine(line)
                        End If
                    Else
                        filemaker.WriteLine(line)
                    End If
                End If
                ProgressBar1.Increment(1)
            Loop
            filereader.Close()
            filemaker.Close()
            If savefilename <> "" Then
                My.Computer.Audio.PlaySystemSound(Media.SystemSounds.Beep)
                MsgBox("File Successfully Reconfigured", , "Complete")
                ProgressBar1.Increment(-totalrowcount)
            End If
        End If
    
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        'This function removes all lines before or after the indicated time.
        fd.Title = "Choose RAW File"
        fd.InitialDirectory = "C:\Users\jsharitt\Desktop\ForJason\PROBEEF\Dress Rehearsal"
        fd.Filter = "All files (*.txt)|*.txt|All Files (*.txt)|*.txt"
        fd.FilterIndex = 2
        fd.RestoreDirectory = True
        If fd.ShowDialog() = Windows.Forms.DialogResult.OK Then
            strFilename = fd.FileName
        End If
        If strFilename = "" Then
            MsgBox("You must choose a file to process", , "ERROR")
        ElseIf rdoAfter.Checked = False AndAlso rdoBefore.Checked = False Then
            MsgBox("You must choose either before or after", , "ERROR")
        Else
            savefilename = fd.FileName
            timequit = InputBox("Input time to stop on", "Time Entry", , 250, 75)
            filereader = My.Computer.FileSystem.OpenTextFileReader(strFilename)
            totalrowcount = 0
            Do Until filereader.EndOfStream
                line = filereader.ReadLine()
                totalrowcount += 1
            Loop
            ProgressBar1.Maximum = totalrowcount
            ProgressBar1.Minimum = 0
            filereader.Close()
            filereader = My.Computer.FileSystem.OpenTextFileReader(strFilename)
            timetest = 0
            If rdoAfter.Checked = True Then
                filemaker = New StreamWriter(savefilename.Substring(0, savefilename.Length - 4) + "_OnlyTimeBefore_" + timequit + ".txt", False)
                Do While timetest < timequit
                    line = filereader.ReadLine()
                    If line = "" Then
                        filemaker.WriteLine(line)
                    ElseIf line <> "" Then
                        If line.Contains(",") Then
                            lineSplit = line.Split(New [Char]() {","})
                            If lineSplit.Count = 17 Then
                                timetest = CDec(lineSplit(0))
                                If timetest < timequit Then
                                    filemaker.WriteLine(line)
                                End If
                            Else
                                filemaker.WriteLine(line)
                            End If
                        Else
                            filemaker.WriteLine(line)
                        End If
                    End If
                    ProgressBar1.Increment(1)
                Loop
                filereader.Close()
                filemaker.Close()
            End If
            If rdoBefore.Checked = True Then
                filemaker = New StreamWriter(savefilename.Substring(0, savefilename.Length - 4) + "_OnlyTimeAfter_" + timequit + ".txt", False)
                Do Until filereader.EndOfStream()
                    line = filereader.ReadLine()
                    If line = "" Then
                        filemaker.WriteLine(line)
                    ElseIf line <> "" Then
                        If line.Contains(",") Then
                            lineSplit = line.Split(New [Char]() {","})
                            If lineSplit.Count = 17 Then
                                timetest = CDec(lineSplit(0))
                                If timetest > timequit Then
                                    filemaker.WriteLine(line)
                                End If
                            Else
                                filemaker.WriteLine(line)
                            End If
                        Else
                            filemaker.WriteLine(line)
                        End If
                    End If
                    ProgressBar1.Increment(1)
                Loop
                filereader.Close()
                filemaker.Close()
            End If
            If savefilename <> "" Then
                My.Computer.Audio.PlaySystemSound(Media.SystemSounds.Beep)
                MsgBox("File Successfully Reconfigured", , "Complete")
                ProgressBar1.Increment(-totalrowcount)
            End If
        End If
     
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        'This function shaves off the milliseconds from the timestamps in the RAW files.
        fd.Title = "Choose RAW File"
        fd.InitialDirectory = "C:\Users\jsharitt\Desktop\ForJason\PROBEEF\Dress Rehearsal"
        fd.Filter = "All files (*.txt)|*.txt|All Files (*.txt)|*.txt"
        fd.FilterIndex = 2
        fd.RestoreDirectory = True
        If fd.ShowDialog() = Windows.Forms.DialogResult.OK Then
            strFilename = fd.FileName
        End If
        If strFilename = "" Then
            MsgBox("You must choose a file to process", , "ERROR")
        ElseIf strFilename <> "" Then
            savefilename = fd.FileName
            filereader = My.Computer.FileSystem.OpenTextFileReader(strFilename)
            totalrowcount = 0
                Do Until filereader.EndOfStream
                    line = filereader.ReadLine()
                    totalrowcount += 1
                Loop
            ProgressBar1.Maximum = totalrowcount
                ProgressBar1.Minimum = 0
                filereader.Close()
            filereader = My.Computer.FileSystem.OpenTextFileReader(strFilename)
            filemaker = New StreamWriter(savefilename.Substring(0, savefilename.Length - 4) + "_NoMilliseconds.txt", False)
            Do Until filereader.EndOfStream()
                line = filereader.ReadLine()
                If line = "" Then
                    filemaker.WriteLine(line)
                ElseIf line <> "" Then
                    If line.Contains(",") Then
                        lineSplit = line.Split(New [Char]() {","})
                        If lineSplit.Count = 17 Then
                            noms = CInt(lineSplit(0))
                            filemaker.WriteLine(CStr(noms) + ".000" + "," + lineSplit(1) + "," + lineSplit(2) + "," + lineSplit(3) + "," + lineSplit(4) + "," + lineSplit(5) + "," + lineSplit(6) + "," + lineSplit(7) + "," + lineSplit(8) + "," + lineSplit(9) + "," + lineSplit(10) + "," + lineSplit(11) + "," + lineSplit(12) + "," + lineSplit(13) + "," + lineSplit(14) + "," + lineSplit(15) + "," + lineSplit(16))
                        Else
                            filemaker.WriteLine(line)
                        End If
                    Else
                        filemaker.WriteLine(line)
                    End If
                End If
                ProgressBar1.Increment(1)
            Loop
            filereader.Close()
            filemaker.Close()
            If savefilename <> "" Then
                My.Computer.Audio.PlaySystemSound(Media.SystemSounds.Beep)
                MsgBox("File Successfully Reconfigured", , "Complete")
                ProgressBar1.Increment(-totalrowcount)
            End If
        End If
       
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        'This function strips out lines in the RAW files to simulate a tag transmitting at either
        '1,2,or 3 hertz. 
        fd.Title = "Choose RAW File"
        fd.InitialDirectory = "C:\Users\jsharitt\Desktop\ForJason\PROBEEF\Dress Rehearsal"
        fd.Filter = "All files (*.txt)|*.txt|All Files (*.txt)|*.txt"
        fd.FilterIndex = 2
        fd.RestoreDirectory = True
        If fd.ShowDialog() = Windows.Forms.DialogResult.OK Then
            strFilename = fd.FileName
        End If
        If strFilename = "" Then
            MsgBox("You must choose a file to process", , "ERROR")
        Else
            savefilename = fd.FileName

            hertz = InputBox("You must choose either 1, 2, or 3 hertz", "Input hertz", , 250, 75)
            If IsNumeric(hertz) = False Then
                MsgBox("You can only choose 1,2, or 3 hertz", , "Error")
            ElseIf CInt(hertz) <> 1 Or CInt(hertz) <> 2 Or CInt(hertz) <> 3 Then
                MsgBox("You can only choose 1,2, or 3 hertz", , "Error")
            Else
                filereader = My.Computer.FileSystem.OpenTextFileReader(strFilename)
                totalrowcount = 0
                Do Until filereader.EndOfStream
                    filereader.ReadLine()
                    totalrowcount += 1
                Loop
                Dim progressmax As Integer = totalrowcount
                ProgressBar1.Maximum = progressmax
                ProgressBar1.Minimum = 0
                filereader.Close()

                '***********Construct list of valid sequence numbers based on Hertz chosen*******
                If hertz = 1 Then
                    Do Until sequencecounter >= 63
                        sequencelist.Add(sequencecounter)
                        sequencecounter += 4
                    Loop
                    filemaker = New StreamWriter(savefilename.Substring(0, savefilename.Length - 4) + "_1Hertz.txt", False)
                ElseIf hertz = 2 Then
                    Do Until sequencecounter >= 63
                        sequencelist.Add(sequencecounter)
                        sequencecounter += 2
                    Loop
                    filemaker = New StreamWriter(savefilename.Substring(0, savefilename.Length - 4) + "_2Hertz.txt", False)
                ElseIf hertz = 3 Then
                    Dim f As Integer = 1
                    Do Until f = 16
                        evennumbers.Add(f)
                        f += 1
                    Loop
                    Do Until sequencecounter >= 63
                        divisiblebyfour = CDec(sequencecounter / 4)
                        If evennumbers.Contains(divisiblebyfour) Then
                            sequencecounter += 1
                        Else
                            sequencelist.Add(sequencecounter)
                            sequencecounter += 1
                        End If
                    Loop
                    filemaker = New StreamWriter(savefilename.Substring(0, savefilename.Length - 4) + "_3Hertz.txt", False)
                End If
                '*******************************************************
                filereader = My.Computer.FileSystem.OpenTextFileReader(strFilename)
                Do Until filereader.EndOfStream()
                    line = filereader.ReadLine()
                    If line = "" Then
                        filemaker.WriteLine(line)
                    ElseIf line <> "" Then
                        If line.Contains(",") Then
                            lineSplit = line.Split(New [Char]() {","})
                            If lineSplit.Count = 17 Then
                                noms = CInt(lineSplit(6))
                                If sequencelist.Contains(noms) Then
                                    filemaker.WriteLine(line)
                                End If
                            Else
                                filemaker.WriteLine(line)
                            End If
                        Else
                            filemaker.WriteLine(line)
                        End If
                    End If
                    ProgressBar1.Increment(1)

                Loop
                '********************************************************************
                '**********Close files, set parameters back to zero***********
                filemaker.Close()
                filereader.Close()
                sequencecounter = 0
                sequencelist.Clear()
                If savefilename <> "" Then
                    My.Computer.Audio.PlaySystemSound(Media.SystemSounds.Beep)
                    MsgBox("File Successfully Reconfigured", , "Complete")
                    ProgressBar1.Increment(-totalrowcount)
                End If
            End If
        End If
    End Sub
End Class
