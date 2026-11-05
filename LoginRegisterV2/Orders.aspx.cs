using System;
using System.Web.UI.WebControls;

namespace LoginRegisterV2
{
    /// <summary>Orders page: reads the form and grid, shows messages. The rules live in OrderService.</summary>
    public partial class Orders : System.Web.UI.Page
    {
        private readonly OrderService _orders = new OrderService();
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
                BindOrders();
            }
        }

        private void BindItemDropdown()
        {
            ddlItem.DataSource = _inventory.GetItemsForDropdown();
            ddlItem.DataBind();
        }

        private void BindOrders()
        {
            gvOrders.DataSource = _orders.GetAllOrders();
            gvOrders.DataBind();
        }

        protected void btnCreateOrder_Click(object sender, EventArgs e)
        {
            lblMessage.Text = "";
            lblSuccess.Text = "";

            if (ddlItem.Items.Count == 0)
            {
                lblMessage.Text = "Add a stock item first before placing an order.";
                BindOrders();
                return;
            }

            OrderInput input = new OrderInput
            {
                CustomerName = txtCustomerName.Text,
                ItemId = Convert.ToInt32(ddlItem.SelectedValue),
                Quantity = InputParser.ToInt(txtQuantity.Text),
                PlacedBy = UserSession.Email
            };

            OperationResult result = _orders.PlaceOrder(input);

            if (!result.Success)
            {
                lblMessage.Text = result.Error;
                BindOrders();
                return;
            }

            lblSuccess.Text = "Order placed successfully.";
            txtCustomerName.Text = "";
            txtQuantity.Text = "";

            // Stock changed, so refresh the item list as well as the orders.
            BindItemDropdown();
            BindOrders();
        }

        protected void btnUpdateStatus_Command(object sender, CommandEventArgs e)
        {
            if (e.CommandName != "UpdateStatus")
            {
                return;
            }

            int orderId = Convert.ToInt32(e.CommandArgument);
            Button btn = (Button)sender;
            GridViewRow row = (GridViewRow)btn.NamingContainer;
            DropDownList ddlStatus = (DropDownList)row.FindControl("ddlStatus");

            _orders.UpdateStatus(orderId, ddlStatus.SelectedValue);

            lblSuccess.Text = "Order status updated.";
            BindItemDropdown();
            BindOrders();
        }
    }
}
