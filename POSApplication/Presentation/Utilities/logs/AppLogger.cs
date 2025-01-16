namespace POSApplication.Presentation.Utilities.logs
{
    using System.Drawing;

    public static class AppLogger
    {
        public static void LogInfo(string message)
        {
            Console.ForegroundColor = ConsoleColor.Cyan; // Set color for Info
            Console.WriteLine($"{message}");
            Console.ResetColor();
        }

        public static void LogWarning(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow; // Set color for Warning
            Console.WriteLine($"{message}");
            Console.ResetColor();
        }

        public static void LogError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red; // Set color for Error
            Console.WriteLine($"{message}");
            Console.ResetColor();
        }

        public static void LogSuccess(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green; // Set color for Success
            Console.WriteLine($"{message}");
            Console.ResetColor();
        }

        public static void LogCustom(string message, string hexColor)
        {
            try
            {
                // Convert hex to Color (with or without leading '#')
                if (hexColor.StartsWith("#"))
                    hexColor = hexColor.Substring(1);

                if (hexColor.Length == 6 && int.TryParse(hexColor, System.Globalization.NumberStyles.HexNumber, null, out int rgb))
                {
                    // Extract RGB values from hex
                    int r = (rgb >> 16) & 0xFF;
                    int g = (rgb >> 8) & 0xFF;
                    int b = rgb & 0xFF;

                    // Create a Color object
                    Color customColor = Color.FromArgb(r, g, b);

                    // Use Colorful.Console to render the message
                    Console.WriteLine(message, customColor);
                }
                else
                {
                    throw new ArgumentException("Invalid hex color format.");
                }
            }
            catch
            {
                // Default fallback color for incorrect hex inputs
                Console.WriteLine($"[INVALID COLOR FORMAT]: {message}", Color.Gray);
            }
        }

    }
}