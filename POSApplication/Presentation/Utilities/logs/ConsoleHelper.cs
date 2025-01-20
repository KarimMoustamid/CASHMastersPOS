namespace POSApplication.Presentation.Utilities.logs
{
    // System.Drawing is used to handle colors, particularly for creating custom colors in the LogCustom method.
    using System.Drawing;

    // A utility class for logging messages to the console with color coding.
    // This class provides methods to display messages in different colors based on their severity or purpose.
    public static class ConsoleHelper
    {
        // Logs an informational message in cyan color.
        public static void LogInfo(string message)
        {
            Console.ForegroundColor = ConsoleColor.Cyan; // Set color for Info
            Console.WriteLine($"{message}");
            Console.ResetColor();
        }

        // Logs a warning message in yellow color.
        public static void LogWarning(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow; // Set color for Warning
            Console.WriteLine($"{message}");
            Console.ResetColor();
        }

        // Logs an error message in red color.
        public static void LogError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red; // Set color for Error
            Console.WriteLine($"{message}");
            Console.ResetColor();
        }

        // Logs a success message in green color.
        public static void LogSuccess(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green; // Set color for Success
            Console.WriteLine($"{message}");
            Console.ResetColor();
        }

        // Logs a message in a custom color specified by a hex color code.
        // Parameters:
        // - message: The message to be logged.
        // - hexColor: The hex color code (in the format "#RRGGBB" or "RRGGBB") to display the message in.
        // If the hex color is invalid, it falls back to a default gray color with an error message.
        public static void LogCustom(string message, string hexColor)
        {
            try
            {
                // Convert hex to Color (with or without leading '#').
                if (hexColor.StartsWith("#"))
                    hexColor = hexColor.Substring(1);

                if (hexColor.Length == 6 && int.TryParse(hexColor, System.Globalization.NumberStyles.HexNumber, null, out int rgb))
                {
                    // Extract RGB values from the hex code.
                    int r = (rgb >> 16) & 0xFF; // Red
                    int g = (rgb >> 8) & 0xFF;  // Green
                    int b = rgb & 0xFF;         // Blue

                    // Create a Color object.
                    Color customColor = Color.FromArgb(r, g, b);

                    // Use Colorful.Console to render the message in custom color.
                    Console.WriteLine(message, customColor);
                }
                else
                {
                    throw new ArgumentException("Invalid hex color format.");
                }
            }
            catch
            {
                // Default fallback color for invalid hex inputs.
                Console.WriteLine($"[INVALID COLOR FORMAT]: {message}", Color.Gray);
            }
        }
    }
}