## <h2 style="background-color: #6d28d9; color: #fff;display: inline-block;">Kanban methodology</h2>

<p>For a detailed explanation of the implementation process, technical decisions, and overall design, please refer to the corresponding public GitHub project. This is the project for the specified repository, and it follows the Kanban methodology. The project provides insights into the thinking process, including decisions made regarding functional and non-functional requirements, data structures, and global configuration. You can find all the details here:</p>

<a href="https://github.com/users/KarimMoustamid/projects/7/views/1" target="_blank">GitHub Project: POS System Change
Calculation</a>


<br>
</hr>


## <h2 style="background-color: #6d28d9; color: #fff;display: inline-block;">Application Configuration and Currency Setup Guide</h2>

This document provides instructions to system administrators on how to configure the application and add a new currency to the system. Following these steps will help ensure that the application runs smoothly and meets your specific requirements.

---

## <h2 style="background-color: #6d28d9; color: #fff;display: inline-block;">1. Application Currency Configuration</h2>

The application uses a configuration system to initialize and manage currencies. By default, the currency is set to <span style="background-color: #fcd34d; color: black;display: inline-block;font-weight: 400;font-style: italic;">USD (United States Dollar)</span>. Administrators can change the default currency or preconfigure a currency suitable for the target market by following the instructions outlined below.

### <h3 style="background-color: #fff; color: #000;display: inline-block;">Steps to Configure Application Currency:</h3>

#### <h4 style="background-color: #15803d; color: white;display: inline-block;">1. Locate the Currency File Path</h4>
- The application retrieves the file path of the currency configuration file from the system configuration. Ensure the file path is properly defined in the configuration system/property used by the application.
- <span style="background-color: #1e40af; color: #e7e5e4;display: inline-block;font-weight: 400;font-style: italic;">Exemple:</span> If the application uses environment variables or a dedicated configuration file to specify application paths, ensure the currency file path (e.g., `CurrencyConfig.json`) is valid and accessible.

#### <h4 style="background-color: #15803d; color: white;display: inline-block;">2. Initialize the Currency System</h4>
- During the application startup, the currency configuration file is loaded. The system reads details such as supported currencies and their attributes (e.g., currency codes, symbols).

#### <h4 style="background-color: #15803d; color: white;display: inline-block;">3. Set the Default Currency</h4>
- By default, the application is preconfigured with <span style="background-color: #fcd34d; color: black;display: inline-block;font-weight: 400;font-style: italic;">USD</span> as the system's currency.
- To set a different default currency:
  - Add the desired currency (with its details such as currency code) to the configuration file (`CurrencyConfig.json`). The system reads this file during initialization.
  - Update or set the default currency by configuring the appropriate constants or values programmatically, e.g., using `CurrencyConstants.NEW_CURRENCY_CODE`.

#### <h4 style="background-color: #15803d; color: white;display: inline-block;">4. Verify Configuration</h4>
- Upon successful configuration, the application logs a message indicating the configured currency. If an error occurs, the system logs the error for troubleshooting purposes.

---

## <h2 style="background-color: #6d28d9; color: #fff;display: inline-block;">2. Manage and Add a New Currency</h2>

To add a new currency to the system:

### <h3 style="background-color: #fff; color: #000;display: inline-block;">Steps to Add a New Currency:</h3>

#### <h4 style="background-color: #15803d; color: white;display: inline-block;">1. Open the Currency Configuration File</h4>
- Locate the `CurrencyConfig.json` file. This file contains the list of currencies supported by the application and their details.

#### <h4 style="background-color: #15803d; color: white;display: inline-block;">2. Add the New Currency Details</h4>
- Append a new currency to the list in `CurrencyConfig.json`. Ensure that the currency details follow the format defined in the file.
- <span style="background-color: #1e40af; color: #e7e5e4;display: inline-block;font-weight: 400;font-style: italic;">Exemple:</span>
  ```json
  {
    "CurrencyCode": "EUR",
    "CurrencySymbol": "€",
    "CurrencyName": "Euro"
  }
  ```
  Replace <span style="background-color: #be185d; color: #e7e5e4;display: inline-block;font-weight: 400;font-style: italic;">"CurrencyCode"</span>, <span style="background-color: #be185d; color: #e7e5e4;display: inline-block;font-weight: 400;font-style: italic;">"CurrencySymbol"</span>, and <span style="background-color: #be185d; color: #e7e5e4;display: inline-block;font-weight: 400;font-style: italic;">"CurrencyName"</span> with the relevant details for the new currency.

#### <h4 style="background-color: #15803d; color: white;display: inline-block;">3. Update the Application Code (if necessary)</h4>
- If the application references specific pre-defined constants for currencies (e.g., `CurrencyConstants`), ensure that the new currency is added as a constant entry so it can be used in the code.
- <span style="background-color: #1e40af; color: #e7e5e4;display: inline-block;font-weight: 400;font-style: italic;">Exemple:</span>
  ```csharp
  public static class CurrencyConstants
  {
      public const string USD = "USD";
      public const string EUR = "EUR"; // Newly added
  }
  ```

#### <h4 style="background-color: #15803d; color: white;display: inline-block;">4. Restart the Application</h4>
- After updating the configuration file or constants, restart the application to ensure the new currency is loaded and available for use.

#### <h4 style="background-color: #15803d; color: white;display: inline-block;">5. Test the Configuration</h4>
- Verify that the new currency is recognized by the system.
  - Check logs for successful loading of the new currency during startup.
  - Test the functionality (e.g., using the system interfaces) to confirm that the new currency works as expected.

---

## <h2 style="background-color: #6d28d9; color: #fff;display: inline-block;">POS System Change Calculation Routine</h2>

<p>You are a <span style="background-color: #fcd34d; color: black;display: inline-block;font-weight: 400;font-style: italic;"> software consultant </span> for CASH Masters, a company that sells point-of-sale (POS) electronic cash registers.  CASH Masters would like to rewrite their POS system from scratch and has the requirement below that they’d like you to implement. Provide a complete working solution of how you would implement this. Pay attention to all function and non-function requirements and treat this as if you were coding as a member of the CASH Master team.</p>

### <h3 style="background-color: #fff; color: #000;display: inline-block;">Functional Requirements</h3>

<span style="background-color: #fcd34d; color: black;display: inline-block;font-weight: 400;font-style: italic;">Today’s young cashiers are increasingly unable to return the correct amount of change.</span> As a result, we would like our POS system to calculate the correct change and display the <span style="background-color: #be185d; color: #e7e5e4;display: inline-block;font-weight: 400;font-style: italic;">optimum (i.e., minimum)</span> number of bills and coins to return to the customer.

#### <h4 style="background-color: #15803d; color: white;display: inline-block;">Your task</h4>
Write a routine that takes two inputs:
- **Price of the item(s)** being purchased
- **All bills and coins** provided by the customer to pay for the item(s)

The routine should return the <span style="background-color: #be185d; color: #e7e5e4;display: inline-block;font-weight: 400;font-style: italic;">smallest number of bills and coins</span> equal to the change due.

<span style="background-color: #1e40af; color: #e7e5e4;display: inline-block;font-weight: 400;font-style: italic;">Exemple:</span>
- If the price is <span style="background-color: #be185d; color: #e7e5e4;display: inline-block;font-weight: 400;font-style: italic;">$5.25</span>, the customer might provide:
    - One $5 bill and one $1 bill, or
    - One $5 bill and ten dimes ($0.10).

<span style="background-color: #fcd34d; color: black;display: inline-block;font-weight: 400;font-style: italic;">The only expectation:</span> The total provided must be greater than or equal to the price of the item being purchased. Your function should verify this assumption.

### <h3 style="background-color: #fff; color: #000;display: inline-block;">Recommended Data Structure</h3>
Since other engineers will use your function, you should:
- Recommend a data structure for both **input** and **returned values**.
- Account for international use. The system will be sold worldwide, with unique denominations for bills and coins.

#### <h4 style="background-color: #15803d; color: white;display: inline-block;">Examples of denominations</h4>
- **US:** 0.01, 0.05, 0.10, 0.25, 0.50, 1.00, 2.00, 5.00, 10.00, 20.00, 50.00, 100.00
- **Mexico:** 0.05, 0.10, 0.20, 0.50, 1.00, 2.00, 5.00, 10.00, 20.00, 50.00, 100.00

<span style="background-color: #fcd34d; color: black;display: inline-block;font-weight: 400;font-style: italic;">Important:</span> Do not physically distinguish between bills and coins—focus only on their numeric values.

### <h3 style="background-color: #fff; color: #000;display: inline-block;">Global Configuration</h3>
- A POS terminal's currency setting will be set once during installation in a country.
- Your routine should not take this setting as input with each call.

---

### <h3 style="background-color: #fff; color: #000;display: inline-block;">Non-Functional Requirements</h3>

- Write a <span style="background-color: #be185d; color: #e7e5e4;display: inline-block;font-weight: 400;font-style: italic;">C# .NET console app</span> that demonstrates your working routine.
- Provide **comments** to help future engineers extend your function.
- Ensure **unit tests** cover all key aspects.
- Follow **common Object-Oriented principles.**
- Optimize for **performance.**
- Include **robust error handling** with clearly documented exceptions.