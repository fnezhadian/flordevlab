namespace LabShop.Data;

// One row of the Products table.
// Price stays a double because the existing column is REAL.
public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Category { get; set; } = "";
    public double Price { get; set; }
}
