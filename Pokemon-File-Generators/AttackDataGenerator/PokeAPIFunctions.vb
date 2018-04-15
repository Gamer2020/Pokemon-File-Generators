Imports System
Imports System.IO
Imports PokeAPI
Imports LitJson

Module PokeAPIFunctions

    Public AllAttackData(718) As Move


    Public DataFile As String
    Public NamesFile As String

    Public Async Sub LoadAllAttackData()

        Dim loopvar As Integer = 0
        Dim errorcounter As Integer = 0

        While loopvar < 719

            Try

                AllAttackData(loopvar) = Await DataFetcher.GetApiObject(Of Move)(loopvar + errorcounter + 1)

                loopvar = loopvar + 1

            Catch ex As Exception

                errorcounter = errorcounter + 1


            End Try


        End While

        loopvar = 0

        While loopvar < AllAttackData.Length


            NamesFile = NamesFile & vbTab & ".string " & """" & StrConv(((AllAttackData(loopvar).Name).Replace("-", " ")), VbStrConv.ProperCase) & "$" & """" & ", 13" & vbCrLf

            loopvar = loopvar + 1

        End While

        File.WriteAllText(AppPath & "move_names.inc", NamesFile)

        Console.WriteLine("Files Generated! Press enter to exit!")

    End Sub
End Module
