Imports System
Imports System.IO
Imports System.Text
Imports System.Collections.Generic
Public Class Form1
    Dim fd As FileDialog = New OpenFileDialog()
    Dim sd As FileDialog = New SaveFileDialog()
    Dim savefilename As String
    Dim strFilename As String
    Dim linestart As String
    Dim filereader As StreamReader
    Dim filemaker As StreamWriter
    Dim linecounter As Integer
    Dim totalrowcount As Integer
    Dim line As String
    Dim timequit As String
    Dim tagid1 As String
    Dim lineSplit As String()
    Dim linesplit2 As String()
    Dim timetest As Decimal
    Dim noms As Decimal
    Dim hertz As String
    Dim sequencelist As New List(Of Integer)
    Dim sequencecounter As Integer = 1
    Dim evennumbers As New List(Of Decimal)
    Dim divisiblebyfour As Decimal
    Dim line2 As String
    Dim sequencenum1 As Integer
    Dim sequencenum2 As Integer
    Dim tagid2 As String
    'All of the below functions require that the user know on which line the actual data starts and the header ends.
    'I have discovered an easier way to do this, I just need to code it in. It is priority #1.

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
            sd.Title = "Save as"
            sd.InitialDirectory = "C:\Users\jsharitt\Desktop\ForJason\PROBEEF\Dress Rehearsal"
            sd.Filter = "All files (*.txt)|*.txt|All Files (*.txt)|*.txt"
            sd.FilterIndex = 2
            sd.RestoreDirectory = True
            If sd.ShowDialog() = Windows.Forms.DialogResult.OK Then
                savefilename = sd.FileName
            End If
            If savefilename = "" Then
                MsgBox("You must indicate a save file", , "ERROR")
            Else
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
            End If


            filereader = My.Computer.FileSystem.OpenTextFileReader(strFilename)
            filemaker = New StreamWriter(savefilename, False)
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
        ElseIf strFilename <> "" Then
            sd.Title = "Save as"
            sd.InitialDirectory = "C:\Users\jsharitt\Desktop\ForJason\PROBEEF\Dress Rehearsal"
            sd.Filter = "All files (*.txt)|*.txt|All Files (*.txt)|*.txt"
            sd.FilterIndex = 2
            sd.RestoreDirectory = True
            If sd.ShowDialog() = Windows.Forms.DialogResult.OK Then
                savefilename = sd.FileName
            End If
            If savefilename = "" Then
                MsgBox("You must indicate a save file", , "ERROR")
            Else
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
                filemaker = New StreamWriter(savefilename, False)
                timetest = 0
                If rdoAfter.Checked = True Then
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
            sd.Title = "Save as"
            sd.InitialDirectory = "C:\Users\jsharitt\Desktop\ForJason\PROBEEF\Dress Rehearsal"
            sd.Filter = "All files (*.txt)|*.txt|All Files (*.txt)|*.txt"
            sd.FilterIndex = 2
            sd.RestoreDirectory = True
            If sd.ShowDialog() = Windows.Forms.DialogResult.OK Then
                savefilename = sd.FileName
            End If
            If savefilename = "" Then
                MsgBox("You must indicate a save file", , "ERROR")
            Else
                linestart = InputBox("Input starting line position. (Default is 404)", "Starting Line Entry", , 250, 75)
                If linestart = "" Then
                    linestart = "404"
                End If
                filereader = My.Computer.FileSystem.OpenTextFileReader(strFilename)
                linecounter = 1
                totalrowcount = 1
                Do Until filereader.EndOfStream
                    line = filereader.ReadLine()
                    totalrowcount += 1
                Loop
                Dim progressmax As Integer = totalrowcount - CInt(linestart) + 1
                ProgressBar1.Maximum = progressmax
                ProgressBar1.Minimum = 0
                ProgressBar1.Increment(CInt(linestart))
                filereader.Close()
                filereader = My.Computer.FileSystem.OpenTextFileReader(strFilename)
            End If
            filereader.Close()

            filereader = My.Computer.FileSystem.OpenTextFileReader(strFilename)
            filemaker = New StreamWriter(savefilename, False)
            linecounter = 3
            '********Create the Raw File Header*************
            Do While linecounter <= CInt(linestart + 1)
                line = (filereader.ReadLine())
                filemaker.WriteLine(line)
                linecounter += 1
                ProgressBar1.Increment(1)
            Loop
            '**********End Header Creation*******************

            '*********Cycle through empty line spaces****
            If line = "" Then
                line = filereader.ReadLine()
            End If
            Do Until filereader.EndOfStream()
                line = filereader.ReadLine()
                If line <> "" Then
                    lineSplit = line.Split(New [Char]() {","})
                    noms = Math.Round(CDec(lineSplit(0)), 0)
                    filemaker.WriteLine(CStr(noms) + ".000" + "," + lineSplit(1) + "," + lineSplit(2) + "," + lineSplit(3) + "," + lineSplit(4) + "," + lineSplit(5) + "," + lineSplit(6) + "," + lineSplit(7) + "," + lineSplit(8) + "," + lineSplit(9) + "," + lineSplit(10) + "," + lineSplit(11) + "," + lineSplit(12) + "," + lineSplit(13) + "," + lineSplit(14) + "," + lineSplit(15) + "," + lineSplit(16))
                End If
                ProgressBar1.Increment(1)
            Loop
            filereader.Close()
            filemaker.Close()
        End If
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        'This function strips out lines in the RAW files to simulate a tag transmitting at either
        '1,2,or 3 hertz. 
        '******Raw file and save file choose dialog, set first line parameter******
        Dim tagtime1 As Decimal
        Dim tagtime2 As Decimal
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
            sd.Title = "Save as"
            sd.InitialDirectory = "C:\Users\jsharitt\Desktop\ForJason\PROBEEF\Dress Rehearsal"
            sd.Filter = "All files (*.txt)|*.txt|All Files (*.txt)|*.txt"
            sd.FilterIndex = 2
            sd.RestoreDirectory = True
            If sd.ShowDialog() = Windows.Forms.DialogResult.OK Then
                savefilename = sd.FileName
            End If
            If savefilename = "" Then
                MsgBox("You must indicate a save file", , "ERROR")
            ElseIf savefilename <> "" Then
                hertz = InputBox("You must choose either 1, 2, or 3 hertz", "Input hertz", , 250, 75)
                linestart = InputBox("Input starting line position. (Default is 404)", "Starting Line Entry", , 250, 75)
                If linestart = "" Then
                    linestart = "404"
                End If
                filereader = My.Computer.FileSystem.OpenTextFileReader(strFilename)
                linecounter = 1
                totalrowcount = 1
                Do Until filereader.EndOfStream
                    filereader.ReadLine()
                    totalrowcount += 1
                Loop
                Dim progressmax As Integer = totalrowcount + (linestart - 1) + (totalrowcount - linestart - 2) + (linestart + 2) + (totalrowcount - linestart - 2)
                ProgressBar1.Maximum = progressmax
                ProgressBar1.Minimum = 0
                ProgressBar1.Increment(totalrowcount)
                filereader.Close()
                filereader = My.Computer.FileSystem.OpenTextFileReader(strFilename)
                '************************************************************************

                filereader.Close()

                filereader = My.Computer.FileSystem.OpenTextFileReader(strFilename)
                filemaker = New StreamWriter(savefilename, False)
                linecounter = 3
                '********Create the Raw File Header*************
                Do While linecounter <= CInt(linestart + 1)
                    line = (filereader.ReadLine())
                    filemaker.WriteLine(line)
                    linecounter += 1
                    ProgressBar1.Increment(1)
                Loop
                '**********End Header Creation*******************

                '*********Cycle through empty line spaces****
                If line = "" Then
                    line = filereader.ReadLine()
                End If
                '**********************************************


                '***********Construct list of valid sequence numbers based on Hertz chosen*******
                If hertz = 1 Then
                    Do Until sequencecounter >= 63
                        sequencelist.Add(sequencecounter)
                        sequencecounter += 4
                    Loop
                ElseIf hertz = 2 Then
                    Do Until sequencecounter >= 63
                        sequencelist.Add(sequencecounter)
                        sequencecounter += 2
                    Loop
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
                End If
                '*******************************************************

                '*******Write first line of positions then cycle through the rest of the file**********
                filemaker.WriteLine(line)
                Do Until filereader.EndOfStream()
                    ' Do Until linecounter >= totalrowcount + 1
                    If line2 <> "" Then
                        line = line2
                    End If
                    lineSplit = line.Split(New [Char]() {","})
                    line2 = filereader.ReadLine()
                    If line2 <> "" Then
                        linesplit2 = line2.Split(New [Char]() {","})
                        sequencenum1 = CInt(lineSplit(6))
                        sequencenum2 = CInt(linesplit2(6))
                        tagid1 = lineSplit(5)
                        tagid2 = linesplit2(5)
                        tagtime1 = CDec(lineSplit(0))
                        tagtime2 = CDec(linesplit2(0))
                        If tagid1 = tagid2 AndAlso tagtime1 = tagtime2 AndAlso sequencelist.Contains(sequencenum2) AndAlso lineSplit(7) <> linesplit2(7) Then
                            filemaker.WriteLine(line2)
                        ElseIf tagid1 <> tagid2 AndAlso sequencelist.Contains(sequencenum2) Then
                            filemaker.WriteLine(line2)
                        ElseIf tagid1 = tagid2 AndAlso tagtime1 <> tagtime2 AndAlso sequencelist.Contains(sequencenum2) Then
                            filemaker.WriteLine(line2)
                        End If
                        Array.Clear(lineSplit, 0, lineSplit.Length)
                        Array.Clear(linesplit2, 0, lineSplit.Length)
                    End If
                    linecounter += 1
                    ProgressBar1.Increment(1)
                Loop
                '********************************************************************

                '**********Close files, set parameters back to zero***********
                filemaker.Close()
                filereader.Close()
                ProgressBar1.Increment(-progressmax)
                sequencecounter = 0
                sequencelist.Clear()
            End If
        End If


    End Sub
End Class
