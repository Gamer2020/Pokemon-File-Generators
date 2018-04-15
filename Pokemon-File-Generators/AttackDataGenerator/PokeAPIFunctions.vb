Imports System
Imports System.IO
Imports PokeAPI
Imports LitJson

Module PokeAPIFunctions

    Public AllAttackData(718) As Move


    Public DataFile As String
    Public NamesFile As String
    Public DescriptionsFile As String
    Public AttackDefines As String

    Public DescriptionPointers As String

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


            AttackDefines = AttackDefines & "#define MOVE_" & ((AllAttackData(loopvar).Name).Replace("-", "_")).ToUpper & " " & loopvar + 1 & vbCrLf

            NamesFile = NamesFile & vbTab & ".string " & """" & StrConv(((AllAttackData(loopvar).Name).Replace("-", " ")), VbStrConv.ProperCase) & "$" & """" & ", 13" & vbCrLf

            DescriptionPointers = DescriptionPointers & vbTab & ".4byte g" & (StrConv(((AllAttackData(loopvar).Name).Replace("-", " ")), VbStrConv.ProperCase)).Replace(" ", "") & "MoveDescription::" & vbCrLf

            DescriptionsFile = DescriptionsFile & "g" & (StrConv(((AllAttackData(loopvar).Name).Replace("-", " ")), VbStrConv.ProperCase)).Replace(" ", "") & "MoveDescription::" & vbCrLf

            Dim languagecount As Integer = 0

            While languagecount < (AllAttackData(loopvar).FlavorTextEntries().Count) + 1
                If AllAttackData(loopvar).FlavorTextEntries(languagecount).Language.Name = "en" Then

                    DescriptionsFile = DescriptionsFile & vbTab & ".string " & """" & (AllAttackData(loopvar).FlavorTextEntries(languagecount).FlavorText).Replace(vbLf, "\n") & "$" & """" & vbCrLf & vbCrLf

                    Exit While

                Else

                    'DescriptionsFile = DescriptionsFile & vbTab & ".string " & """" & "English Description not available..." & "$" & """" & vbCrLf & vbCrLf

                End If

                languagecount = languagecount + 1
            End While

            If languagecount = (AllAttackData(loopvar).FlavorTextEntries().Count) + 1 Then
                DescriptionsFile = DescriptionsFile & vbTab & ".string " & """" & "English Description not available..." & "$" & """" & vbCrLf & vbCrLf
            End If

            loopvar = loopvar + 1

        End While

        DescriptionsFile = DescriptionsFile & vbTab & ".align 2" & vbCrLf & "gMoveDescriptionPointers::" & vbCrLf & DescriptionPointers

        File.WriteAllText(AppPath & "move_names.inc", NamesFile)
        File.WriteAllText(AppPath & "move_descriptions.inc", DescriptionsFile)
        File.WriteAllText(AppPath & "moves.h", AttackDefines)

        Console.WriteLine("Files Generated! Press enter to exit!")

    End Sub
End Module
