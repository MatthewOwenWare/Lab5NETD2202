Option Strict On
' Matthew Ware
' ID 100472787
' frmTextEditor.vb
' Lab 5 - NETD2202 - a basic text editor with save and edit capabilities
Imports System.IO
Imports System.Windows.Forms.VisualStyles

' This Is the main form Class that contains variables To hold a text file When opened Or saved And the file name For the present document While providing cut, copy And paste features
Public Class frmTextEditor

    Dim fileName As String = String.Empty               ' This holds the name of the current document
    Dim originalFile As String = String.Empty           ' This holds a text file as a string for later comparison to see if the file was changed
    ''' <summary>
    ''' When the new button is clicked this clears the main text window while saving a copy of the new file which is empty to start
    ''' </summary>
    Private Sub mnuNewClick() Handles mnuNew.Click
        If (confirmClose()) Then                        ' Ensure user wants to close current file to open a new file
            txtMainWindow.Text = String.Empty           ' Clear the main text window
            originalFile = txtMainWindow.Text           ' Set the original file to the current blank window for later commparison
            Me.Text = "Text Editor: New Document"       ' Reset the title bar to indicate a new file
            fileName = String.Empty                     ' Clear the file name variable
        End If
    End Sub
    ''' <summary>
    ''' When the open button is pressed this puts the file chosen into the main text window
    ''' </summary>
    Private Sub mnuOpenClick() Handles mnuOpen.Click
        If (confirmClose()) Then                                                                ' Confirm the user wants to close the current file
            Dim openFileDialog = New OpenFileDialog()                                           ' Create a new open file dialogue box
            If (openFileDialog.ShowDialog() = DialogResult.OK) Then                             ' Make sure the open action is completed
                fileName = openFileDialog.FileName                                              ' Set the file name to the opened file
                Dim FileStream As New FileStream(fileName, FileMode.Open, FileAccess.Read)      ' Start a new file stream
                Dim reader As New StreamReader(FileStream)                                      ' Start a new reader stream
                txtMainWindow.Text = reader.ReadToEnd()                                         ' Read the entire text file into one string
                Me.Text = "Text Editor: " + fileName                                            ' Change the title bar to include the file name
                reader.Close()                                                                  ' Close the reader stream
                FileStream.Close()                                                              ' Close the file stream
                originalFile = txtMainWindow.Text                                               ' Copy the contents of the new file to the original file string for later comparison
            End If
        End If
    End Sub
    ''' <summary>
    ''' When the save button is pressed the document is written to the file chosen by the Save File Dialogue box if the file has not been saved
    ''' </summary>
    Private Sub mnuSaveClick() Handles mnuSave.Click
        If (fileName = String.Empty) Then                               ' Check that the file has not been saved already
            Dim saveFileDialog = New SaveFileDialog()                   ' Open a new save file dialogue box
            saveFileDialog.Filter = "Text Files | *.txt"                ' Filter the files for the txt extension
            saveFileDialog.DefaultExt = "txt"                           ' Set txt as the default selection
            If (saveFileDialog.ShowDialog() = DialogResult.OK) Then     ' Ensure that the file is successfully chosen
                fileName = saveFileDialog.FileName                      ' Set the file name to the chosen file name
                writeFile(fileName)                                     ' Write the contents of the main text window to the file
            End If
        Else
            writeFile(fileName)                                         ' If the file has been saved then just write to the already used file name
        End If
    End Sub
    ''' <summary>
    ''' Similar to the save button, this button saves the file but asks for a file name every time
    ''' </summary>
    Private Sub mnuSaveAsClick() Handles mnuSaveAs.Click
        Dim saveFileDialog = New SaveFileDialog()                       ' Start a new Save File Dialogue box
        If (saveFileDialog.ShowDialog() = DialogResult.OK) Then         ' Confirm that the save operation was completed successfully
            fileName = saveFileDialog.FileName                          ' Set the file name to the chosen file name
            writeFile(fileName)                                         ' Write the main text box contents to the chosen file name
        End If
    End Sub

    Private Sub mnuCloseClick() Handles mnuClose.Click
        mnuNewClick()                                                   ' Closes the current file and opens a new file document
    End Sub

    Private Sub mnuExitClick() Handles mnuExit.Click
        If (confirmClose()) Then                                        ' Confirm that the user actually wants to exit the program
            Me.Close()                                                  ' Close the entire program
        End If
    End Sub
    ''' <summary>
    ''' Copies the selected text from the main text window to the clipboard
    ''' </summary>
    Private Sub mnuCopyClick() Handles mnuCopy.Click
        Dim stringToSave As String = txtMainWindow.SelectedText         ' Copy the selected text to a string
        My.Computer.Clipboard.SetText(stringToSave)                     ' Write the string to the clipboard
    End Sub
    ''' <summary>
    ''' Cuts the selected text from the main text window and copy it to the clipboard
    ''' </summary>
    Private Sub mnuCutClick() Handles mnuCut.Click
        Dim stringToCut As String = txtMainWindow.SelectedText          ' Copy the selected text to a string
        My.Computer.Clipboard.SetText(stringToCut)                      ' Copy the string to the clipboard
        txtMainWindow.Cut()                                             ' Remove the selected text from the main text window
    End Sub
    ''' <summary>
    ''' Pastes text from the clipboard into the main text window
    ''' </summary>
    Private Sub mnuPasteClick() Handles mnuPaste.Click
        Dim stringToInsert As String = My.Computer.Clipboard.GetText()  ' Copy the text from the clipboard into a new string
        txtMainWindow.Paste()                                           ' Paste the text into the main text window
    End Sub
    ''' <summary>
    ''' Displays a message box with information about the program
    ''' </summary>
    Private Sub mnuAboutClick() Handles mnuAbout.Click
        Dim messageText As String = "NETD-2202" + Environment.NewLine + Environment.NewLine + "Lab# 5" + Environment.NewLine + Environment.NewLine + "Matt Ware" ' String to display
        MsgBox(messageText, 0, "About")                                 ' Create and display the message box
    End Sub
    ''' <summary>
    ''' Writes a file to the specified file name as a parameter
    ''' </summary>
    ''' <param name="fileName"></param>
    Private Sub writeFile(fileName As String)
        Dim FileStream As New FileStream(fileName, FileMode.Create, FileAccess.Write)   ' Open a new file stream to write the file
        Dim writer As New StreamWriter(FileStream)                                      ' Open a new writer to access the file stream
        writer.WriteLine(txtMainWindow.Text)                                            ' Write the string contained in the main text window to the file
        writer.Flush()                                                                  ' Flush the writer stream
        writer.Close()                                                                  ' Close the writer stream
        FileStream.Close()                                                              ' Close the file stream
        Me.Text = "Text Editor: " + fileName                                            ' Change the current document title to include the file name
        originalFile = txtMainWindow.Text                                               ' Copy the current document to a string for later comparison
    End Sub
    ''' <summary>
    ''' Confirm Close function responsible for bonus parts one and two
    ''' Compares the current screen text to the original file
    ''' If the files are not identical then confirm the close operation
    ''' </summary>
    ''' <returns></returns>
    Function confirmClose() As Boolean
        If (String.Compare(txtMainWindow.Text, originalFile) <> 0) Then                                                             ' Compare the current text box with the original file
            If (MsgBox("Are you sure you want to close the current file?", vbOKCancel, "Confirm Close") = MsgBoxResult.Ok) Then     ' Ask if the user wants to continue to close the current document
                Return True                                                                                                         ' Close current document
            Else
                Return False                                                                                                        ' Cancel the close document operation
            End If
        End If
        Return True                                                                                                                 ' Close current document
    End Function
End Class
