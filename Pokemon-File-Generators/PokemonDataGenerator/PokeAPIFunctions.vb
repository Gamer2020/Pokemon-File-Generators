Imports System
Imports System.IO
Imports PokeAPI
Imports LitJson

Module PokeAPIFunctions

    Public AllPokemonData(948) As Pokemon
    Public PokeSpecies As PokemonSpecies

    Public SpeciesNamesText As String
    Public SpeciesDataText As String

    Public Async Sub LoadAllPokemonData()

        ' ***************************
        'Fetching data!
        ' ***************************

        Dim loopvar As Integer = 0

        While loopvar < 802

            Try

                AllPokemonData(loopvar) = Await DataFetcher.GetApiObject(Of Pokemon)(loopvar + 1)

                'Console.WriteLine(AllPokemonData(loopvar).Name)

            Catch ex As Exception

            End Try

            loopvar = loopvar + 1

        End While

        loopvar = 10000

        While loopvar < 10147

            Try

                AllPokemonData((loopvar - 10000) + 802) = Await DataFetcher.GetApiObject(Of Pokemon)(loopvar + 1)

                'Console.WriteLine(AllPokemonData((loopvar - 10000) + 802).Name)

            Catch ex As Exception

            End Try

            loopvar = loopvar + 1

        End While

        ' ***************************
        'Generate Names!
        ' ***************************

        loopvar = 0

        SpeciesNamesText = SpeciesNamesText & "gSpeciesNames:: @ 83185C8" & vbCrLf

        While loopvar < AllPokemonData.Length

            'Console.WriteLine(AllPokemonData(loopvar).Name)

            SpeciesNamesText = SpeciesNamesText & vbTab & ".string " & """" & UppercaseFirstLetter(AllPokemonData(loopvar).Name) & "$" & """" & ", 11" & vbCrLf

            loopvar = loopvar + 1

        End While

        ' ***************************
        'Generate Base Stats!
        ' ***************************

        SpeciesDataText = SpeciesDataText & "#ifndef GUARD_BASE_STATS_H" & vbCrLf &
"#define GUARD_BASE_STATS_H" & vbCrLf & vbCrLf &
"// Maximum value for a female pokemon is 254 (MON_FEMALE) which is 100% female." & vbCrLf &
"// 255 (MON_GENDERLESS) is reserved for genderless pokemon." & vbCrLf &
"#define PERCENT_FEMALE(percent) min(254, ((percent * 255) / 100))" & vbCrLf & vbCrLf &
"Const struct BaseStats gBaseStats[] =" & vbCrLf &
"{" & vbCrLf &
    "[SPECIES_NONE] = {0}," & vbCrLf

        loopvar = 0

        While loopvar < AllPokemonData.Length

            'Console.WriteLine(AllPokemonData(loopvar).Name)

            Try

                PokeSpecies = Await DataFetcher.GetApiObject(Of PokemonSpecies)(AllPokemonData(loopvar).ID)

            Catch ex As Exception

            End Try

            SpeciesDataText = SpeciesDataText & vbTab & "[SPECIES_" & UppercaseFirstLetter(AllPokemonData(loopvar).Name) & "] =" & vbCrLf
            SpeciesDataText = SpeciesDataText & vbTab & "{" & vbCrLf

            SpeciesDataText = SpeciesDataText & vbTab & ".baseHP        = " & AllPokemonData(loopvar).Stats(0).BaseValue & "," & vbCrLf
            SpeciesDataText = SpeciesDataText & vbTab & ".baseAttack        = " & AllPokemonData(loopvar).Stats(1).BaseValue & "," & vbCrLf
            SpeciesDataText = SpeciesDataText & vbTab & ".baseDefense        = " & AllPokemonData(loopvar).Stats(2).BaseValue & "," & vbCrLf
            SpeciesDataText = SpeciesDataText & vbTab & ".baseSpeed        = " & AllPokemonData(loopvar).Stats(5).BaseValue & "," & vbCrLf
            SpeciesDataText = SpeciesDataText & vbTab & ".baseSpAttack        = " & AllPokemonData(loopvar).Stats(3).BaseValue & "," & vbCrLf
            SpeciesDataText = SpeciesDataText & vbTab & ".baseSpDefense        = " & AllPokemonData(loopvar).Stats(4).BaseValue & "," & vbCrLf

            SpeciesDataText = SpeciesDataText & vbTab & ".type1        = TYPE_" & (AllPokemonData(loopvar).Types(0).Type.Name).ToUpper & "," & vbCrLf

            If (AllPokemonData(loopvar).Types.Count = 2) Then

                SpeciesDataText = SpeciesDataText & vbTab & ".type2        = TYPE_" & (AllPokemonData(loopvar).Types(1).Type.Name).ToUpper & "," & vbCrLf

            Else

                SpeciesDataText = SpeciesDataText & vbTab & ".type2        = TYPE_" & (AllPokemonData(loopvar).Types(0).Type.Name).ToUpper & "," & vbCrLf

            End If

            Try

                SpeciesDataText = SpeciesDataText & vbTab & ".catchRate        = " & PokeSpecies.CaptureRate & "," & vbCrLf

            Catch

                SpeciesDataText = SpeciesDataText & vbTab & ".catchRate        = " & "0" & "," & vbCrLf

            End Try

            SpeciesDataText = SpeciesDataText & vbTab & ".expYield        = " & AllPokemonData(loopvar).BaseExperience & "," & vbCrLf

            SpeciesDataText = SpeciesDataText & vbTab & ".evYield_HP        = " & AllPokemonData(loopvar).Stats(0).Effort & "," & vbCrLf
            SpeciesDataText = SpeciesDataText & vbTab & ".evYield_Attack        = " & AllPokemonData(loopvar).Stats(1).Effort & "," & vbCrLf
            SpeciesDataText = SpeciesDataText & vbTab & ".evYield_Defense        = " & AllPokemonData(loopvar).Stats(2).Effort & "," & vbCrLf
            SpeciesDataText = SpeciesDataText & vbTab & ".evYield_Speed        = " & AllPokemonData(loopvar).Stats(5).Effort & "," & vbCrLf
            SpeciesDataText = SpeciesDataText & vbTab & ".evYield_SpAttack        = " & AllPokemonData(loopvar).Stats(3).Effort & "," & vbCrLf
            SpeciesDataText = SpeciesDataText & vbTab & ".evYield_SpDefense        = " & AllPokemonData(loopvar).Stats(4).Effort & "," & vbCrLf

            If AllPokemonData(loopvar).HeldItems.Count = 0 Then

                SpeciesDataText = SpeciesDataText & vbTab & ".item1        = ITEM_NONE," & vbCrLf
                SpeciesDataText = SpeciesDataText & vbTab & ".item2        = ITEM_NONE," & vbCrLf

            ElseIf AllPokemonData(loopvar).HeldItems.Count = 1 Then
                SpeciesDataText = SpeciesDataText & vbTab & ".item1        = ITEM_NONE," & vbCrLf
                SpeciesDataText = SpeciesDataText & vbTab & ".item2        = ITEM_" & ((AllPokemonData(loopvar).HeldItems(0).Item.Name).ToUpper).Replace(" ", "_").Replace("-", "_") & "," & vbCrLf
            ElseIf AllPokemonData(loopvar).HeldItems.Count = 2 Then
                SpeciesDataText = SpeciesDataText & vbTab & ".item1        = ITEM_" & ((AllPokemonData(loopvar).HeldItems(0).Item.Name).ToUpper).Replace(" ", "_").Replace("-", "_") & "," & vbCrLf
                SpeciesDataText = SpeciesDataText & vbTab & ".item2        = ITEM_" & ((AllPokemonData(loopvar).HeldItems(1).Item.Name).ToUpper).Replace(" ", "_") & "," & vbCrLf
            End If

            Try

                SpeciesDataText = SpeciesDataText & vbTab & ".genderRatio        = PERCENT_FEMALE(" & (PokeSpecies.FemaleToMaleRate * 100) & ")," & vbCrLf

                SpeciesDataText = SpeciesDataText & vbTab & ".eggCycles        = " & PokeSpecies.HatchCounter & "," & vbCrLf
                SpeciesDataText = SpeciesDataText & vbTab & ".friendship        = " & PokeSpecies.BaseHappiness & "," & vbCrLf
                SpeciesDataText = SpeciesDataText & vbTab & ".growthRate        = GROWTH_" & ((PokeSpecies.GrowthRate.Name).ToUpper).Replace("-", "_") & "," & vbCrLf

                If PokeSpecies.EggGroups.Count = 2 Then
                    SpeciesDataText = SpeciesDataText & vbTab & ".eggGroup1        = EGG_GROUP_" & ((PokeSpecies.EggGroups(0).Name).ToUpper).Replace("-", "_") & "," & vbCrLf
                    SpeciesDataText = SpeciesDataText & vbTab & ".eggGroup2        = EGG_GROUP_" & ((PokeSpecies.EggGroups(1).Name).ToUpper).Replace("-", "_") & "," & vbCrLf
                ElseIf PokeSpecies.EggGroups.Count = 1 Then
                    SpeciesDataText = SpeciesDataText & vbTab & ".eggGroup1        = EGG_GROUP_" & ((PokeSpecies.EggGroups(0).Name).ToUpper).Replace("-", "_") & "," & vbCrLf
                    SpeciesDataText = SpeciesDataText & vbTab & ".eggGroup2        = EGG_GROUP_" & ((PokeSpecies.EggGroups(0).Name).ToUpper).Replace("-", "_") & "," & vbCrLf
                Else

                    SpeciesDataText = SpeciesDataText & vbTab & ".eggGroup1        = " & "EGG_GROUP_UNDISCOVERED" & "," & vbCrLf
                    SpeciesDataText = SpeciesDataText & vbTab & ".eggGroup2        = " & "EGG_GROUP_UNDISCOVERED" & "," & vbCrLf
                End If
            Catch

                SpeciesDataText = SpeciesDataText & vbTab & ".genderRatio        = PERCENT_FEMALE(" & "0" & ")," & vbCrLf
                SpeciesDataText = SpeciesDataText & vbTab & ".eggCycles        = " & "0" & "," & vbCrLf
                SpeciesDataText = SpeciesDataText & vbTab & ".friendship        = " & "0" & "," & vbCrLf
                SpeciesDataText = SpeciesDataText & vbTab & ".growthRate        = " & "GROWTH_MEDIUM_SLOW" & "," & vbCrLf
                SpeciesDataText = SpeciesDataText & vbTab & ".eggGroup1        = " & "EGG_GROUP_UNDISCOVERED" & "," & vbCrLf
                SpeciesDataText = SpeciesDataText & vbTab & ".eggGroup2        = " & "EGG_GROUP_UNDISCOVERED" & "," & vbCrLf

            End Try


            If AllPokemonData(loopvar).Abilities.Count = 0 Then

                SpeciesDataText = SpeciesDataText & vbTab & ".ability1        = ABILITY_NONE," & vbCrLf
                SpeciesDataText = SpeciesDataText & vbTab & ".ability2        = ABILITY_NONE," & vbCrLf

            ElseIf AllPokemonData(loopvar).Abilities.Count = 1 Then
                SpeciesDataText = SpeciesDataText & vbTab & ".ability1        = ABILITY_" & ((AllPokemonData(loopvar).Abilities(0).Ability.Name).ToUpper).Replace(" ", "_").Replace("-", "_") & "," & vbCrLf
                SpeciesDataText = SpeciesDataText & vbTab & ".ability2        = ABILITY_NONE," & vbCrLf
            ElseIf AllPokemonData(loopvar).Abilities.Count = 2 Then
                SpeciesDataText = SpeciesDataText & vbTab & ".ability1        = ABILITY_" & ((AllPokemonData(loopvar).Abilities(0).Ability.Name).ToUpper).Replace(" ", "_").Replace("-", "_") & "," & vbCrLf
                SpeciesDataText = SpeciesDataText & vbTab & ".ability2        = ABILITY_" & ((AllPokemonData(loopvar).Abilities(1).Ability.Name).ToUpper).Replace(" ", "_") & "," & vbCrLf
            End If


            SpeciesDataText = SpeciesDataText & vbTab & ".safariZoneFleeRate        = 0" & "," & vbCrLf

            Try

                SpeciesDataText = SpeciesDataText & vbTab & ".bodyColor        = BODY_COLOR_" & (PokeSpecies.Colours.Name).ToUpper & "," & vbCrLf

            Catch

                SpeciesDataText = SpeciesDataText & vbTab & ".bodyColor        = " & "BODY_COLOR_BLUE" & "," & vbCrLf

            End Try

            SpeciesDataText = SpeciesDataText & vbTab & ".noFlip        = FALSE" & "," & vbCrLf

            SpeciesDataText = SpeciesDataText & vbTab & "}," & vbCrLf & vbCrLf

            loopvar = loopvar + 1

        End While

        SpeciesDataText = SpeciesDataText & "};" & vbCrLf & vbCrLf &
"#End If //GUARD_BASE_STATS_H"

        ' ***************************
        'Create Files.
        ' ***************************

        File.WriteAllText(AppPath & "species_names.inc", SpeciesNamesText)
        File.WriteAllText(AppPath & "base_stats.inc", SpeciesDataText)

        Console.WriteLine("Files Generated! Press enter to exit!")

    End Sub

End Module
