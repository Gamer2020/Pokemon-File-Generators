Imports System
Imports System.IO
Imports PokeAPI
Imports LitJson

Module PokeAPIFunctions

    Public AllItemData(867) As Item

    Public FinalFile As String

    Public Async Sub LoadAllItemData()

        Dim loopvar As Integer = 0

        While loopvar < 867

            Try

                AllItemData(loopvar) = Await DataFetcher.GetApiObject(Of Item)(loopvar + 1)


            Catch ex As Exception

            End Try

            loopvar = loopvar + 1

        End While

        Console.WriteLine("Files Generated! Press enter to exit!")

    End Sub
End Module
