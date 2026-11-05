namespace LoginRegisterV2
{
    /// <summary>Raw values from the "place order" form.</summary>
    public class OrderInput
    {
        public string CustomerName { get; set; }
        public int ItemId { get; set; }

        /// <summary>Null when the quantity textbox could not be read as a number.</summary>
        public int? Quantity { get; set; }

        public string PlacedBy { get; set; }
    }
}
