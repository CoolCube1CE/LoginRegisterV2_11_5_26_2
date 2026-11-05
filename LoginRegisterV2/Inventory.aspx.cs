using System;
using System.Web.UI.WebControls;

namespace LoginRegisterV2
{
    /// <summary>Inventory page: reads the form and grid, shows messages. The rules live in InventoryService.</summary>
    public partial class Inventory : System.Web.UI.Page
    {
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
                BindInventory();
            }
        }

        private void BindInventory()
        {
            gvInventory.DataSource = _inventory.GetAllItems();
            gvInventory.DataBind();
        }

        protected void btnAddItem_Click(object sender, EventArgs e)
        {
            lblMessage.Text = "";
            lblSuccess.Text = "";

            StockItemInput input = new StockItemInput
            {
                Name = txtItemName.Text,
                Category = txtCategory.Text,
                Sku = txtSku.Text,
                Quantity = InputParser.ToInt(txtQuantity.Text),
                ReorderLevel = InputParser.ToInt(txtReorderLevel.Text),
                UnitPrice = InputParser.ToDecimal(txtUnitPrice.Text)
            };

            OperationResult result = _inventory.AddItem(input);
            if (!result.Success)
            {
                lblMessage.Text = result.Error;
                BindInventory();
                return;
            }

            lblSuccess.Text = "Item added successfully.";
            txtItemName.Text = "";
            txtCategory.Text = "";
            txtSku.Text = "";
            txtQuantity.Text = "0";
            txtReorderLevel.Text = "10";
            txtUnitPrice.Text = "0";

            BindInventory();
        }

        protected void gvInventory_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvInventory.EditIndex = e.NewEditIndex;
            BindInventory();
        }

        protected void gvInventory_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvInventory.EditIndex = -1;
            BindInventory();
        }

        protected void gvInventory_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            lblMessage.Text = "";
            lblSuccess.Text = "";

            int itemId = Convert.ToInt32(gvInventory.DataKeys[e.RowIndex].Value);
            GridViewRow row = gvInventory.Rows[e.RowIndex];

            StockItemInput input = new StockItemInput
            {
                Name = ((TextBox)row.FindControl("txtEditName")).Text,
                Category = ((TextBox)row.FindControl("txtEditCategory")).Text,
                Sku = ((TextBox)row.FindControl("txtEditSku")).Text,
                ReorderLevel = InputParser.ToInt(((TextBox)row.FindControl("txtEditReorder")).Text),
                UnitPrice = InputParser.ToDecimal(((TextBox)row.FindControl("txtEditPrice")).Text)
            };

            OperationResult result = _inventory.UpdateItem(itemId, input);
            if (!result.Success)
            {
                // Stay in edit mode so the user can fix the value.
                lblMessage.Text = result.Error;
                return;
            }

            gvInventory.EditIndex = -1;
            lblSuccess.Text = "Item updated.";
            BindInventory();
        }

        protected void gvInventory_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int itemId = Convert.ToInt32(gvInventory.DataKeys[e.RowIndex].Value);
            _inventory.DeleteItem(itemId);
            lblSuccess.Text = "Item deleted.";
            BindInventory();
        }
    }
}
