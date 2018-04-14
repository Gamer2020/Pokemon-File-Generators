Imports System
Imports System.IO
Imports PokeAPI
Imports LitJson

Module PokeAPIFunctions

    Public AllItemData(866) As Item

    Public FinalFile As String
    Public ItemNames As String

    Public Async Sub LoadAllItemData()

        Dim loopvar As Integer = 0
        Dim errorcounter As Integer = 0

        While loopvar < 867

            Try

                AllItemData(loopvar) = Await DataFetcher.GetApiObject(Of Item)(loopvar + errorcounter + 1)

                loopvar = loopvar + 1

            Catch ex As Exception

                errorcounter = errorcounter + 1


            End Try


        End While

        FinalFile = "	.align 2" & vbCrLf & "gItems:: " & vbCrLf

        loopvar = 0

        While loopvar < AllItemData.Length


            Dim alreadyin As Boolean = False

            For Each Line As String In File.ReadLines(AppPath & "itemso.inc")

                If Line.Contains((((AllItemData(loopvar).Name).Replace("--held", "")).Replace("-", " ")).ToUpper) = True Then

                    alreadyin = True

                    Exit For
                End If

            Next

            If alreadyin = False Then
                FinalFile = FinalFile & vbTab & ".string """ & (UppercaseFirstLetter(((AllItemData(loopvar).Name).Replace("--held", "")).Replace("-", " ")) & "$"", 14").Replace(" z$", "-z$") & vbCrLf
                FinalFile = FinalFile & vbTab & ".2byte ITEM_" & (((AllItemData(loopvar).Name).Replace("--held", "")).Replace("-", "_")).ToUpper & " @Index number" & vbCrLf
                FinalFile = FinalFile & vbTab & ".2byte " & AllItemData(loopvar).Cost & " @Price" & vbCrLf
                FinalFile = FinalFile & vbTab & ".byte 0" & " @Hold Effect" & vbCrLf
                FinalFile = FinalFile & vbTab & ".byte 0" & " @Parameter" & vbCrLf
                FinalFile = FinalFile & vbTab & ".4byte gAntidoteItemDescription" & " @Description Pointer" & vbCrLf
                FinalFile = FinalFile & vbTab & ".byte 0" & " @Mystery Value" & vbCrLf
                FinalFile = FinalFile & vbTab & ".byte 0" & " @Mystery Value" & vbCrLf
                FinalFile = FinalFile & vbTab & ".byte POCKET_ITEMS" & " @Pocket" & vbCrLf
                FinalFile = FinalFile & vbTab & ".byte 0" & " @Type" & vbCrLf
                FinalFile = FinalFile & vbTab & ".4byte 0" & " @Field Usage Pointer" & vbCrLf
                FinalFile = FinalFile & vbTab & ".4byte 0" & " @Battle Usage" & vbCrLf
                FinalFile = FinalFile & vbTab & ".4byte 0" & " @Battle Usage Pointer" & vbCrLf
                FinalFile = FinalFile & vbTab & ".4byte 0" & " @Extra Parameter" & vbCrLf
                FinalFile = FinalFile & vbCrLf

                ItemNames = ItemNames & "#define " & (((AllItemData(loopvar).Name).Replace("--held", "")).Replace("-", "_")).ToUpper & " valuenumber" & vbCrLf
            End If

            loopvar = loopvar + 1
        End While

        File.WriteAllText(AppPath & "items.inc", FinalFile)
        File.WriteAllText(AppPath & "itemNames.h", ItemNames)
        Console.WriteLine("Files Generated! Press enter to exit!")

    End Sub
End Module
