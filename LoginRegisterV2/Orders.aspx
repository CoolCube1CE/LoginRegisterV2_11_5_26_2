<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="Orders.aspx.cs" Inherits="LoginRegisterV2.Orders" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="page-shell">
        <div class="dashboard-header">
            <div>
                <h2 class="mt-sm">🧾 Orders</h2>
                <p class="dashboard-user-line">Compile and organize customer orders placed through the store.</p>
            </div>
        </div>

        <asp:Label ID="lblMessage" runat="server" CssClass="alert alert-error" />
        <asp:Label ID="lblSuccess" runat="server" CssClass="alert alert-success" />

        <div class="card card--compact mb-sm">
            <h3 class="mt-0">New Order</h3>
            <div class="form-grid">
                <div class="form-group mb-0">
                    <label class="form-label">Customer Name</label>
                    <asp:TextBox ID="txtCustomerName" runat="server" CssClass="form-control" />
                </div>
                <div class="form-group mb-0">
                    <label class="form-label">Item</label>
                    <asp:DropDownList ID="ddlItem" runat="server" CssClass="form-control" DataTextField="ItemName" DataValueField="ItemId" />
                </div>
                <div class="form-group mb-0">
                    <label class="form-label">Quantity</label>
                    <asp:TextBox ID="txtQuantity" runat="server" CssClass="form-control" TextMode="Number" />
                </div>
            </div>
            <div class="btn-row">
                <asp:Button ID="btnCreateOrder" runat="server" Text="Place Order" CssClass="btn btn-primary" OnClick="btnCreateOrder_Click" />
            </div>
        </div>

        <div class="dashboard-section" style="margin-top:0;">
            <h3>All Orders</h3>
            <div class="table-responsive">
                <asp:GridView ID="gvOrders" runat="server" CssClass="app-table" AutoGenerateColumns="false"
                    GridLines="None" DataKeyNames="OrderId" EmptyDataText="No orders placed yet.">
                    <Columns>
                        <asp:BoundField DataField="OrderDate" HeaderText="Date" DataFormatString="{0:MMM d, yyyy h:mm tt}" />
                        <asp:BoundField DataField="CustomerName" HeaderText="Customer" />
                        <asp:BoundField DataField="ItemName" HeaderText="Item" />
                        <asp:BoundField DataField="Quantity" HeaderText="Qty" />
                        <asp:BoundField DataField="TotalAmount" HeaderText="Total" DataFormatString="₱{0:N2}" />
                        <asp:TemplateField HeaderText="Status">
                            <ItemTemplate>
                                <span class='<%# "status-pill status-pill--" + Eval("Status").ToString().ToLower() %>'><%# Eval("Status") %></span>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Actions">
                            <ItemTemplate>
                                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control input-sm" SelectedValue='<%# Eval("Status") %>'>
                                    <asp:ListItem Text="Pending" Value="Pending" />
                                    <asp:ListItem Text="Completed" Value="Completed" />
                                    <asp:ListItem Text="Cancelled" Value="Cancelled" />
                                </asp:DropDownList>
                                <asp:Button runat="server" Text="Update" CssClass="btn btn-outline" CommandName="UpdateStatus"
                                    CommandArgument='<%# Eval("OrderId") %>' OnCommand="btnUpdateStatus_Command" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>
</asp:Content>
