using System;
using System.Linq;

namespace PartyLamps
{
    class Program
    {
        static bool[] StartConfiguration = new bool[101];
        static bool?[] FinalConfiguration = new bool?[101];
        static int N = 100;
        static int C;
        static int SuitableResponsesCount = 0;
        static string[] result = new string[16];

        static void Main(string[] args)
        {
            SetDefaultsToArray(StartConfiguration, true, N);

            string firstLine;
            while ((firstLine = Console.ReadLine()) != null)
            {
                N = int.Parse(firstLine);
                C = int.Parse(Console.ReadLine());

                var lamps = Console.ReadLine().Split(' ').Select(int.Parse).TakeWhile(i => i != -1).ToArray();
                SetLampsStatus(lamps, true);

                lamps = Console.ReadLine().Split(' ').Select(int.Parse).TakeWhile(i => i != -1).ToArray();
                SetLampsStatus(lamps, false);

                PrintValidConfigurations();
            }
        }

        private static void PrintValidConfigurations()
        {
            for (int i = 0; i < 16; i++)
            {
                if (SuitableButtonsConfiguration(i, C))
                {
                    ApplyConfiguration(i, StartConfiguration, N);

                    if (SuitableFinalConfiguration(StartConfiguration, FinalConfiguration, N))
                    {
                        var currentResult = GetSolutionFromConfiguration(StartConfiguration, N);
                        result[SuitableResponsesCount] = currentResult;
                        SuitableResponsesCount += 1;
                    }

                    SetDefaultsToArray(StartConfiguration, true, N);
                }
            }

            PrintValidConfigurationsWithoutRepetitions(result, SuitableResponsesCount);

            SuitableResponsesCount = 0;
            SetDefaultsToArray(FinalConfiguration, null, N);
        }

        private static bool SuitableButtonsConfiguration(int buttonsConfiguration, int availableMovements)
        {
            int offLampsCount = 0, init = 1;
            while (buttonsConfiguration != 0)
            {
                offLampsCount += (buttonsConfiguration & init) > 0 ? 1 : 0;
                buttonsConfiguration -= buttonsConfiguration & init;
                init = init << 1;
            }

            if (availableMovements < offLampsCount)
            {
                return false;
            }

            return (offLampsCount == 4) ? (availableMovements - 3) % 2 == 1 :
                   (offLampsCount == 3) ? (availableMovements - 3) % 2 == 0 :
                   (offLampsCount == 2) ? (availableMovements - 2) % 2 == 0 :
                   (offLampsCount == 1) ? (availableMovements - 1) % 2 == 0 :
                   availableMovements % 2 == 0;
        }

        private static void ApplyConfiguration(int buttonsConfiguration, bool[] lampsConfiguration, int lampsCount)
        {
            var bitValues = new[] { 1, 2, 4, 8 };

            for (int j = 0; j < bitValues.Length; j++)
            {
                if ((bitValues[j] & buttonsConfiguration) >= 1)
                {
                    ApplyMovement(j + 1, lampsConfiguration, lampsCount);
                }
            }
        }

        private static void ApplyMovement(int buttonNumber, bool[] lampsConfiguration, int lampsCount)
        {
            if (buttonNumber == 1) // all lamps button
            {
                for (int i = 1; i <= lampsCount; i++)
                {
                    lampsConfiguration[i] = !lampsConfiguration[i];
                }
            }
            else if (buttonNumber == 2) // odd lamps button
            {
                for (int i = 1; i <= lampsCount; i += 2)
                {
                    lampsConfiguration[i] = !lampsConfiguration[i];
                }
            }
            else if (buttonNumber == 3) // even lamps button
            {
                for (int i = 2; i <= lampsCount; i += 2)
                {
                    lampsConfiguration[i] = !lampsConfiguration[i];
                }
            }
            else
            {
                for (int i = 1; i <= lampsCount; i += 3) // 3k + 1 lamps button
                {
                    lampsConfiguration[i] = !lampsConfiguration[i];
                }
            }
        }

        private static bool SuitableFinalConfiguration(bool[] startConfiguration, bool?[] finalConfiguration, int lampsCount)
        {
            for (int i = 1; i <= lampsCount; i++)
            {
                if (finalConfiguration[i] != null && finalConfiguration[i] != startConfiguration[i])
                {
                    return false;
                }
            }

            return true;
        }

        private static string GetSolutionFromConfiguration(bool[] lampsConfiguration, int numberOfLamps)
        {
            char[] currentResult = new char[numberOfLamps];
            for (int i = 1; i <= numberOfLamps; i++)
            {
                currentResult[i - 1] = lampsConfiguration[i] ? '1' : '0';
            }

            return new string(currentResult);
        }

        private static void PrintValidConfigurationsWithoutRepetitions(string[] validConfiguration, int validResponsesCount)
        {
            if (validResponsesCount > 0)
            {
                string lastPrint = null;
                Array.Sort(validConfiguration, 0, validResponsesCount);

                for (int i = 0; i < validResponsesCount; i++)
                {
                    if (lastPrint == null || lastPrint != validConfiguration[i])
                    {
                        lastPrint = validConfiguration[i];
                        Console.WriteLine(validConfiguration[i]);
                    }
                }
            }
            else
            {
                Console.WriteLine("IMPOSSIBLE");
            }
        }

        private static void SetLampsStatus(int[] lamps, bool status)
        {
            foreach (var i in lamps)
            {
                FinalConfiguration[i] = status;
            }
        }

        private static void SetDefaultsToArray<T>(T[] config, T value, int numberOfElements)
        {
            for (int i = 1; i <= numberOfElements; i++)
            {
                config[i] = value;
            }
        }
    }
}