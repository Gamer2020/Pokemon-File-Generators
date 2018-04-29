Imports System
Imports System.IO
Imports PokeAPI
Imports LitJson

Module PokeAPIFunctions

    Public AllAttackData(718) As Move


    Public DataFile As String
    Public NamesFile As String
    Public DescriptionsFile As String
    Public DescriptionPointers As String
    Public AttackDefines As String
    Public AnimationTable As String
    Public MindRatingsTable As String

    Public ContestData As String



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

            ContestData = ContestData & "@ " & StrConv(((AllAttackData(loopvar).Name).Replace("-", " ")), VbStrConv.ProperCase) & vbCrLf
            ContestData = ContestData & vbTab & ".byte 0x00 @ effect ID" & vbCrLf
            Try
                ContestData = ContestData & vbTab & ".byte CONTEST_" & (AllAttackData(loopvar).ContestType.Name).ToUpper & vbCrLf

            Catch ex As Exception
                ContestData = ContestData & vbTab & ".byte 0" & vbCrLf

            End Try

            ContestData = ContestData & vbTab & ".byte 0 @ combo starter ID" & vbCrLf
            ContestData = ContestData & vbTab & ".byte 0, 0, 0, 0 @ moves this move can follow to make a combo" & vbCrLf
            ContestData = ContestData & vbTab & ".byte 0 @ padding" & vbCrLf & vbCrLf

            MindRatingsTable = MindRatingsTable & vbTab & ".byte 0 @ " & StrConv(((AllAttackData(loopvar).Name).Replace("-", " ")), VbStrConv.ProperCase) & vbCrLf

            AnimationTable = AnimationTable & vbTab & ".4byte Move_NONE" & " // MOVE_" & ((AllAttackData(loopvar).Name).Replace("-", "_")).ToUpper & vbCrLf

            DataFile = DataFile & vbTab & "{ // MOVE_" & ((AllAttackData(loopvar).Name).Replace("-", "_")).ToUpper & vbCrLf
            DataFile = DataFile & vbTab & vbTab & ".effect = EFFECT_HIT," & vbCrLf
            DataFile = DataFile & vbTab & vbTab & ".power = " & (AllAttackData(loopvar).Power) & "," & vbCrLf
            DataFile = DataFile & vbTab & vbTab & ".type = TYPE_" & (AllAttackData(loopvar).Type.Name).ToUpper & "," & vbCrLf
            DataFile = DataFile & vbTab & vbTab & ".accuracy = " & (AllAttackData(loopvar).Accuracy) & "," & vbCrLf
            DataFile = DataFile & vbTab & vbTab & ".pp = " & (AllAttackData(loopvar).PP) & "," & vbCrLf
            DataFile = DataFile & vbTab & vbTab & ".secondaryEffectChance = 0" & "," & vbCrLf
            DataFile = DataFile & vbTab & vbTab & ".target = MOVE_TARGET_SELECTED" & "," & vbCrLf
            DataFile = DataFile & vbTab & vbTab & ".priority = " & (AllAttackData(loopvar).Priority) & "," & vbCrLf
            DataFile = DataFile & vbTab & vbTab & ".flags = FLAG_MAKES_CONTACT | FLAG_PROTECT_AFFECTED | FLAG_MIRROR_MOVE_AFFECTED | FLAG_KINGSROCK_AFFECTED" & "," & vbCrLf
            DataFile = DataFile & vbTab & "}," & vbCrLf

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

        File.WriteAllText(AppPath & "contest_moves.inc", ContestData)
        File.WriteAllText(AppPath & "battle_arena_move_mind_ratings.inc", MindRatingsTable)
        File.WriteAllText(AppPath & "move_names.inc", NamesFile)
        File.WriteAllText(AppPath & "move_descriptions.inc", DescriptionsFile)
        File.WriteAllText(AppPath & "moves.h", AttackDefines)
        File.WriteAllText(AppPath & "battle_moves.h", DataFile)
        File.WriteAllText(AppPath & "battle_anim_scripts.s", AnimationTable)

        Console.WriteLine("Files Generated! Press enter to exit!")

    End Sub
End Module
