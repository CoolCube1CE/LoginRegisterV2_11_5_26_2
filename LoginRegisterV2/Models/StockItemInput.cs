namespace LoginRegisterV2
{
    /// <summary>
    /// Raw values from the add/edit item form. A number that could not be read
    /// from its textbox is null; StockItemValidator turns that into a message.
    /// </summary>
    public class StockItemInput
    {
        public string Name { get; set; }
        public string Category { get; set; }
        public string Sku { get; set; }
        public int? Quantity { get; set; }
        public int? ReorderLevel { get; set; }
        public decimal? UnitPrice { get; set; }
    }
}
