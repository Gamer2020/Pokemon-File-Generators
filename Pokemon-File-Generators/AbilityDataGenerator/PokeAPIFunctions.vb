﻿Imports System
Imports System.IO
Imports PokeAPI
Imports LitJson

Module PokeAPIFunctions

    Public AllAbilityData(291) As Ability

    Public AbilityNames As String
    Public AbilityDescriptions As String
    Public AbilityDescriptionPointers As String
    Public AbilityNamesDefine As String

    Public FinalFile As String

    Public Async Sub LoadAllAbilityData()

        Dim loopvar As Integer = 0

        While loopvar < 232

            Try

                AllAbilityData(loopvar) = Await DataFetcher.GetApiObject(Of Ability)(loopvar + 1)


            Catch ex As Exception

            End Try

            loopvar = loopvar + 1

        End While

        loopvar = 10000

        While loopvar < 10060

            Try

                AllAbilityData((loopvar - 10000) + 232) = Await DataFetcher.GetApiObject(Of Ability)(loopvar + 1)


            Catch ex As Exception

            End Try

            loopvar = loopvar + 1

        End While

        loopvar = 0

        While loopvar < AllAbilityData.Length

            AbilityNames = AbilityNames & vbTab & "_(" & """" & (UppercaseFirstLetter(AllAbilityData(loopvar).Name)).Replace("-", " ") & "" & """" & ")," & vbCrLf

            AbilityNamesDefine = AbilityNamesDefine & "#define ABILITY_" & ((UppercaseFirstLetter(AllAbilityData(loopvar).Name)).Replace("-", "_")).ToUpper & " " & loopvar + 2 & vbCrLf

            AbilityDescriptionPointers = AbilityDescriptionPointers & "g" & (UppercaseFirstLetter(AllAbilityData(loopvar).Name)).Replace("-", "") & "Description," & vbCrLf

            Try

                Dim languagecount As Integer = 0

                While languagecount < (AllAbilityData(loopvar).FlavorTexts().Count) + 1
                    If AllAbilityData(loopvar).FlavorTexts(languagecount).Language.Name = "en" Then

                        'DescriptionsFile = DescriptionsFile & vbTab & ".string " & """" & (AllAttackData(loopvar).FlavorTextEntries(languagecount).FlavorText).Replace(vbLf, "\n") & "$" & """" & vbCrLf & vbCrLf
                        AbilityDescriptions = AbilityDescriptions & "static const u8 g" & (UppercaseFirstLetter(AllAbilityData(loopvar).Name)).Replace("-", "") & "Description[] = _(" & """" & (AllAbilityData(loopvar).FlavorTexts(languagecount).FlavorText).Replace(vbLf, "\n") & """" & ");" & vbCrLf

                        Exit While

                        If languagecount = (AllAbilityData(loopvar).FlavorTexts().Count) + 1 Then
                            AbilityDescriptions = AbilityDescriptions & "static const u8 g" & (UppercaseFirstLetter(AllAbilityData(loopvar).Name)).Replace("-", "") & "Description[] = _(" & """" & "No special ability." & """" & ");" & vbCrLf

                        End If


                    Else

                        'DescriptionsFile = DescriptionsFile & vbTab & ".string " & """" & "English Description not available..." & "$" & """" & vbCrLf & vbCrLf

                    End If

                    languagecount = languagecount + 1
                End While

                'AbilityDescriptions = AbilityDescriptions & "static const u8 g" & (UppercaseFirstLetter(AllAbilityData(loopvar).Name)).Replace("-", "") & "[] = _(" & """" & AllAbilityData(loopvar).FlavorTexts(0).FlavorText & """" & ");" & vbCrLf

            Catch ex As Exception

                AbilityDescriptions = AbilityDescriptions & "static const u8 g" & (UppercaseFirstLetter(AllAbilityData(loopvar).Name)).Replace("-", "") & "Description[] = _(" & """" & "No special ability." & """" & ");" & vbCrLf

            End Try

            loopvar = loopvar + 1

        End While

        AbilityNames = "const u8 gAbilityNames[][ABILITY_NAME_LENGTH + 1] =" & vbCrLf & "{" & vbCrLf & AbilityNames & vbCrLf & "};" & vbCrLf & vbCrLf

        AbilityDescriptionPointers = "const u8 *const gAbilityDescriptionPointers[] =" & vbCrLf & "{" & vbCrLf & AbilityDescriptionPointers & "};" & vbCrLf

        AbilityDescriptions = AbilityDescriptions & vbCrLf

        FinalFile = "#ifndef POKEEMERALD_DATA_TEXT_ABILITIES_H" & vbCrLf & "#define POKEEMERALD_DATA_TEXT_ABILITIES_H" & vbCrLf & vbCrLf


        FinalFile = FinalFile & AbilityDescriptions & AbilityNames & AbilityDescriptionPointers

        FinalFile = FinalFile & vbCrLf & vbCrLf & "#endif // POKEEMERALD_DATA_TEXT_ABILITIES_H"

        File.WriteAllText(AppPath & "abilities.h", FinalFile)
        File.WriteAllText(AppPath & "abilities2.h", AbilityNamesDefine)

        Console.WriteLine("Files Generated! Press enter to exit!")

    End Sub
End Module
