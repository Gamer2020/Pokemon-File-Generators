Imports System
Imports System.IO
Imports PokeAPI
Imports LitJson

Module PokeAPIFunctions

    Public AllAbilityData(291) As Ability

    Public AbilityNames As String
    Public AbilityDescriptions As String
    Public AbilityDescriptionPointers As String

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

            AbilityDescriptionPointers = AbilityDescriptionPointers & "g" & (UppercaseFirstLetter(AllAbilityData(loopvar).Name)).Replace("-", "") & "Description," & vbCrLf

            Try

                AbilityDescriptions = AbilityDescriptions & "static const u8 g" & (UppercaseFirstLetter(AllAbilityData(loopvar).Name)).Replace("-", "") & "[] = _(" & """" & AllAbilityData(loopvar).FlavorTexts(0).FlavorText & """" & ");" & vbCrLf

            Catch ex As Exception

                AbilityDescriptions = AbilityDescriptions & "static const u8 g" & (UppercaseFirstLetter(AllAbilityData(loopvar).Name)).Replace("-", "") & "[] = _(" & """" & "No special ability." & """" & ");" & vbCrLf

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

        Console.WriteLine("Files Generated! Press enter to exit!")

    End Sub
End Module
