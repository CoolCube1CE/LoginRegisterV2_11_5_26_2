namespace LoginRegisterV2
{
    /// <summary>Raw values from the "record stock in/out" form.</summary>
    public class StockMovementInput
    {
        public int ItemId { get; set; }
        public string Type { get; set; }

        /// <summary>Null when the quantity textbox could not be read as a number.</summary>
        public int? Quantity { get; set; }

        public string PerformedBy { get; set; }
        public string Notes { get; set; }
    }
}
