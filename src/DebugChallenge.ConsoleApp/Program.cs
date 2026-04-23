using DebugChallenge.Application.UseCases.CalculateCartTotal;
using DebugChallenge.Infrastructure.Pricing;

Console.WriteLine("=== Debug Challenge: Cart Total ===");
Console.WriteLine("Input format example: KB-01:1,MS-02:2,KB-01:1");
Console.WriteLine("Available coupon codes: SAVE10, SAVE20");
Console.WriteLine();

Console.Write("Enter cart lines: ");
var linesInput = Console.ReadLine();

if (string.IsNullOrWhiteSpace(linesInput))
{
    Console.WriteLine("No lines provided.");
    return;
}

Console.Write("Enter coupon code (optional): ");
var couponCode = Console.ReadLine();

var parsedLines = ParseLines(linesInput).ToList();
var handler = new CalculateCartTotalHandler(new InMemoryProductRepository(), new FixedCouponPolicy());

try
{
    var result = await handler.HandleAsync(new CalculateCartTotalCommand(parsedLines, couponCode));

    Console.WriteLine();
    Console.WriteLine($"Subtotal: {result.Subtotal:C2}");
    Console.WriteLine($"Discount: {result.DiscountAmount:C2}");
    Console.WriteLine($"Total:    {result.Total:C2}");
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}

static IEnumerable<CartLineInput> ParseLines(string input)
{
    var chunks = input.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

    foreach (var chunk in chunks)
    {
        var parts = chunk.Split(':', StringSplitOptions.TrimEntries);

        if (parts.Length != 2)
            throw new FormatException($"Invalid item format: '{chunk}'. Expected SKU:Quantity");

        if (!int.TryParse(parts[1], out var quantity))
            throw new FormatException($"Invalid quantity for '{chunk}'.");

        yield return new CartLineInput(parts[0], quantity);
    }
}
