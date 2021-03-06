﻿using Microsoft.Xna.Framework.Input;
using SuperWizardPlatformer.Input;
using System;
using System.IO;
using System.Runtime;

namespace SuperWizardPlatformer
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            StreamWriter logFile = null;

            try
            {
                logFile = new StreamWriter("SuperWizardPlatformer.log");

                // Re-route all console output to log.
                Console.SetOut(logFile);
                Console.SetError(logFile);
            }
            catch (Exception e)
            {
                Console.OpenStandardOutput();
                Console.OpenStandardError();
                Console.Error.WriteLine(e);
            }

            try
            {
                // Change process-wide settings.
                GCSettings.LatencyMode = GCLatencyMode.SustainedLowLatency;

                // Write start-of-process info to log.
                Console.WriteLine("SuperWizardPlatformer v0.1 {0}", DebugMode);
                Console.WriteLine("Running {0}-bit process on {1}-bit system",
                    Environment.Is64BitProcess ? 64 : 32,
                    Environment.Is64BitOperatingSystem ? 64 : 32);
                Console.WriteLine("GCLatencyMode: {0}", GCSettings.LatencyMode);
                Console.WriteLine("{0}: {1}", nameof(GamePad.MaximumGamePadCount),
                    GamePad.MaximumGamePadCount);
                Console.WriteLine("Number of key codes: {0}", KeyStateTracker.NumKeys);

                using (var game = new Game1())
                {
                    game.Run();
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);
            }
            finally
            {
                if (logFile != null)
                {
                    logFile.Close();
                }
            }
        }

        /// <summary>
        /// String indicating whether the program was compiled in Debug or Release mode.
        /// </summary>
        private static string DebugMode
        {
            get
            {
#if DEBUG
                return "DEBUG";
#else
                return "RELEASE";
#endif
            }
        }
    }
#endif
}
