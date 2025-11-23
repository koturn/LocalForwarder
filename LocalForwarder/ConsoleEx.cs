using System;
using System.Text;


namespace LocalForwarder
{
    /// <summary>
    /// Provides console utilities.
    /// </summary>
    public static class ConsoleEx
    {
        /// <summary>
        /// Read password from stdin.
        /// </summary>
        /// <returns>Inputted password.</returns>
        public static string ReadPassword()
        {
            var sb = new StringBuilder();
            while (true)
            {
                var cki = Console.ReadKey(true);
                if (cki.Key == ConsoleKey.Enter)
                {
                    Console.WriteLine();
                    break;
                }
                else if (cki.Key == ConsoleKey.Backspace)
                {
                    if (sb.Length > 0)
                    {
                        Console.Write("\b\0\b");
                        sb.Length--;
                    }
                    continue;
                }
                else if (!IsCharKey(cki))
                {
                    continue;
                }
                Console.Write('*');
                sb.Append(cki.KeyChar);
            }

            return sb.ToString();
        }

        /// <summary>
        /// Identify inputted key is character or not.
        /// </summary>
        /// <param name="cki">Inputted key information.</param>
        /// <returns>True if inputted key is a character, otherwise false.</returns>
        private static bool IsCharKey(ConsoleKeyInfo cki)
        {
            switch (cki.Key)
            {
                case ConsoleKey.Backspace:
                case ConsoleKey.Tab:
                case ConsoleKey.Clear:
                case ConsoleKey.Enter:
                case ConsoleKey.Pause:
                case ConsoleKey.Escape:
                case ConsoleKey.Spacebar:
                case ConsoleKey.PageUp:
                case ConsoleKey.PageDown:
                case ConsoleKey.End:
                case ConsoleKey.Home:
                case ConsoleKey.LeftArrow:
                case ConsoleKey.UpArrow:
                case ConsoleKey.RightArrow:
                case ConsoleKey.DownArrow:
                case ConsoleKey.Select:
                case ConsoleKey.Print:
                case ConsoleKey.Execute:
                case ConsoleKey.PrintScreen:
                case ConsoleKey.Insert:
                case ConsoleKey.Delete:
                case ConsoleKey.Help:
                case ConsoleKey.LeftWindows:
                case ConsoleKey.RightWindows:
                case ConsoleKey.Applications:
                case ConsoleKey.Sleep:
                case ConsoleKey.F1:
                case ConsoleKey.F2:
                case ConsoleKey.F3:
                case ConsoleKey.F4:
                case ConsoleKey.F5:
                case ConsoleKey.F6:
                case ConsoleKey.F7:
                case ConsoleKey.F8:
                case ConsoleKey.F9:
                case ConsoleKey.F10:
                case ConsoleKey.F11:
                case ConsoleKey.F12:
                case ConsoleKey.F13:
                case ConsoleKey.F14:
                case ConsoleKey.F15:
                case ConsoleKey.F16:
                case ConsoleKey.F17:
                case ConsoleKey.F18:
                case ConsoleKey.F19:
                case ConsoleKey.F20:
                case ConsoleKey.F21:
                case ConsoleKey.F22:
                case ConsoleKey.F23:
                case ConsoleKey.F24:
                case ConsoleKey.BrowserBack:
                case ConsoleKey.BrowserForward:
                case ConsoleKey.BrowserRefresh:
                case ConsoleKey.BrowserStop:
                case ConsoleKey.BrowserSearch:
                case ConsoleKey.BrowserFavorites:
                case ConsoleKey.BrowserHome:
                case ConsoleKey.VolumeMute:
                case ConsoleKey.VolumeDown:
                case ConsoleKey.VolumeUp:
                case ConsoleKey.MediaNext:
                case ConsoleKey.MediaPrevious:
                case ConsoleKey.MediaStop:
                case ConsoleKey.MediaPlay:
                case ConsoleKey.LaunchMail:
                case ConsoleKey.LaunchMediaSelect:
                case ConsoleKey.LaunchApp1:
                case ConsoleKey.LaunchApp2:
                case ConsoleKey.Process:
                case ConsoleKey.Packet:
                case ConsoleKey.Attention:
                case ConsoleKey.CrSel:
                case ConsoleKey.ExSel:
                case ConsoleKey.EraseEndOfFile:
                case ConsoleKey.Play:
                case ConsoleKey.Zoom:
                case ConsoleKey.NoName:
                case ConsoleKey.Pa1:
                case ConsoleKey.OemClear:
                    return false;
                default:
                    return true;
            }
        }
    }
}
