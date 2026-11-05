namespace LoginRegisterV2
{
    /// <summary>
    /// Turns textbox text into numbers. Returns null when the text is not a
    /// valid number, so the validators can decide what message to show.
    /// </summary>
    public static class InputParser
    {
        public static int? ToInt(string text)
        {
            int value;
            return int.TryParse(text, out value) ? (int?)value : null;
        }

        public static decimal? ToDecimal(string text)
        {
            decimal value;
            return decimal.TryParse(text, out value) ? (decimal?)value : null;
        }
    }
}
