namespace LoginRegisterV2
{
    /// <summary>The rules for a stock item form. It only checks values; it does not save anything.</summary>
    public class StockItemValidator
    {
        /// <summary>Checks the "add item" form (it includes a starting quantity).</summary>
        public OperationResult ValidateNew(StockItemInput input)
        {
            OperationResult nameCheck = CheckName(input.Name);
            if (!nameCheck.Success) return nameCheck;

            if (!input.Quantity.HasValue || input.Quantity.Value < 0)
                return OperationResult.Fail("Starting quantity must be a valid non-negative number.");

            return CheckReorderAndPrice(input);
        }

        /// <summary>Checks the "edit item" row (quantity is not editable here, it changes through transactions).</summary>
        public OperationResult ValidateEdit(StockItemInput input)
        {
            OperationResult nameCheck = CheckName(input.Name);
            if (!nameCheck.Success) return nameCheck;

            return CheckReorderAndPrice(input);
        }

        private static OperationResult CheckName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return OperationResult.Fail("Item name is required.");

            return OperationResult.Ok();
        }

        private static OperationResult CheckReorderAndPrice(StockItemInput input)
        {
            if (!input.ReorderLevel.HasValue || input.ReorderLevel.Value < 0)
                return OperationResult.Fail("Reorder level must be a valid non-negative number.");

            if (!input.UnitPrice.HasValue || input.UnitPrice.Value < 0)
                return OperationResult.Fail("Unit price must be a valid non-negative number.");

            return OperationResult.Ok();
        }
    }
}
