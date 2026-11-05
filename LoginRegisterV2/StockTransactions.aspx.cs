using System;

namespace LoginRegisterV2
{
    /// <summary>Stock Transactions page: reads the form, shows messages. The rules live in StockTransactionService.</summary>
    public partial class StockTransactions : System.Web.UI.Page
    {
        private readonly StockTransactionService _transactions = new StockTransactionService();
        private readonly InventoryService _inventory = new InventoryService();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!UserSession.IsSignedIn)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                BindItemDropdown();
                BindTransactions();
            }
        }

        private void BindItemDropdown()
        {
            ddlItem.DataSource = _inventory.GetItemsForDropdown();
            ddlItem.DataBind();
        }

        private void BindTransactions()
        {
            gvTransactions.DataSource = _transactions.GetAllTransactions();
            gvTransactions.DataBind();
        }

        protected void btnRecord_Click(object sender, EventArgs e)
        {
            lblMessage.Text = "";
            lblSuccess.Text = "";

            if (ddlItem.Items.Count == 0)
            {
                lblMessage.Text = "Add a stock item first before recording a transaction.";
                BindTransactions();
                return;
            }

            StockMovementInput input = new StockMovementInput
            {
                ItemId = Convert.ToInt32(ddlItem.SelectedValue),
                Type = ddlType.SelectedValue,
                Quantity = InputParser.ToInt(txtQuantity.Text),
                PerformedBy = UserSession.Email,
                Notes = txtNotes.Text.Trim()
            };

            OperationResult result = _transactions.RecordManual(input);

            if (!result.Success)
            {
                lblMessage.Text = result.Error;
                BindTransactions();
                return;
            }

            lblSuccess.Text = "Transaction recorded successfully.";
            txtQuantity.Text = "";
            txtNotes.Text = "";

            // Stock changed, so refresh the item list as well as the history.
            BindItemDropdown();
            BindTransactions();
        }
    }
}
