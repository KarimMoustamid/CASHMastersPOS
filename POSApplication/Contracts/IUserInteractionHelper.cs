namespace POSApplication.Presentation.Utilities.logs
{
    // Defines the contract for user interaction operations.
    // This interface outlines the methods that must be implemented
    // to handle user interactions related to currency and payment input in a consistent manner.
    public interface IUserInteractionHelper
    {
        // Displays the available currency denominations.
        // This method fetches and prints a list of valid denominations for the current currency.
        void CurrencyDenominations();

        // Displays available currencies with their country and currency codes.
        // Fetches a list of supported currencies and prints their details to the console.
        void DisplayAvailableCurrencies();

        // Prompts the user for input and handles validation.
        // Parameters:
        // - prompt: The message to display to the user while requesting input.
        // - errorMessage: The message to display and log if the input is invalid.
        // Returns:
        // - The user's input converted to the specified type T.
        T GetInput<T>(string prompt, string errorMessage, Func<T, bool>? validate);

        // Collects payment input in the form of a dictionary of denominations and their counts.
        // This method interacts with the user to gather payment details, ensuring validation against
        // available denominations throughout the process.
        // Returns:
        // - A dictionary where the key is the denomination (decimal) and the value is the count (int) entered by the user.
        Dictionary<decimal, int> CollectPaymentInput();
    }
}