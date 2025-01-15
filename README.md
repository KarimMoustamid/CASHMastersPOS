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



<br>
<br>
<br>
</hr>


<p>For a detailed explanation of the implementation process, technical decisions, and overall design, please refer to the corresponding public GitHub project. The project provides insights into the thinking process, including decisions made regarding functional and non-functional requirements, data structures, and global configuration. You can find all the details here:</p>

<a href="https://github.com/users/KarimMoustamid/projects/7/views/1" target="_blank">GitHub Project: POS System Change
Calculation</a>