<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DashboardOverview.ascx.cs" Inherits="LoginRegisterV2.DashboardOverview" %>

<div class="dashboard-grid">
    <div class="stat-tile stat-tile--info">
        <span class="stat-tile__icon" aria-hidden="true">📦</span>
        <div class="stat-tile__label">Total Items</div>
        <div class="stat-tile__value"><asp:Label ID="lblTotalItems" runat="server" Text="0" /></div>
    </div>
    <div class="stat-tile stat-tile--teal">
        <span class="stat-tile__icon" aria-hidden="true">🧮</span>
        <div class="stat-tile__label">Total Units in Stock</div>
        <div class="stat-tile__value"><asp:Label ID="lblTotalUnits" runat="server" Text="0" /></div>
    </div>
    <div class="stat-tile stat-tile--warning">
        <span class="stat-tile__icon" aria-hidden="true">⚠️</span>
        <div class="stat-tile__label">Low Stock Items</div>
        <div class="stat-tile__value"><asp:Label ID="lblLowStockCount" runat="server" Text="0" /></div>
    </div>
    <div class="stat-tile stat-tile--violet">
        <span class="stat-tile__icon" aria-hidden="true">💰</span>
        <div class="stat-tile__label">Inventory Value</div>
        <div class="stat-tile__value"><asp:Label ID="lblTotalValue" runat="server" Text="₱0.00" /></div>
    </div>
</div>

<div class="btn-row mt-sm">
    <asp:HyperLink runat="server" NavigateUrl="~/Inventory.aspx" CssClass="btn btn-primary">📦 Manage Inventory</asp:HyperLink>
    <asp:HyperLink runat="server" NavigateUrl="~/StockTransactions.aspx" CssClass="btn btn-outline">🔄 Record Stock Movement</asp:HyperLink>
    <asp:HyperLink runat="server" NavigateUrl="~/Orders.aspx" CssClass="btn btn-outline">🧾 New Order</asp:HyperLink>
</div>

<div class="dashboard-section">
    <div class="dashboard-section__header">
        <h3>⚠️ Low Stock Items</h3>
        <asp:HyperLink runat="server" NavigateUrl="~/Inventory.aspx" CssClass="text-action-link">View all inventory →</asp:HyperLink>
    </div>
    <div class="table-responsive">
        <asp:GridView ID="gvLowStock" runat="server" CssClass="app-table" AutoGenerateColumns="false"
            GridLines="None" EmptyDataText="All stock levels look healthy — nothing is at or below its reorder point.">
            <Columns>
                <asp:BoundField DataField="ItemName" HeaderText="Item" />
                <asp:BoundField DataField="SKU" HeaderText="SKU" />
                <asp:BoundField DataField="Quantity" HeaderText="In Stock" />
                <asp:BoundField DataField="ReorderLevel" HeaderText="Reorder Level" />
            </Columns>
        </asp:GridView>
    </div>
</div>

<div class="dashboard-columns">
    <div class="dashboard-section" style="margin-top:0;">
        <div class="dashboard-section__header">
            <h3>🔄 Recent Stock Changes</h3>
            <asp:HyperLink runat="server" NavigateUrl="~/StockTransactions.aspx" CssClass="text-action-link">View all →</asp:HyperLink>
        </div>
        <div class="table-responsive">
            <asp:GridView ID="gvRecentTransactions" runat="server" CssClass="app-table" AutoGenerateColumns="false"
                GridLines="None" EmptyDataText="No stock movements recorded yet.">
                <Columns>
                    <asp:BoundField DataField="ItemName" HeaderText="Item" />
                    <asp:TemplateField HeaderText="Type">
                        <ItemTemplate>
                            <span class='<%# Eval("TransactionType").ToString() == "IN" ? "stock-badge stock-badge--in" : "stock-badge stock-badge--out" %>'>
                                <%# Eval("TransactionType").ToString() == "IN" ? "▲ IN" : "▼ OUT" %>
                            </span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Quantity" HeaderText="Qty" />
                    <asp:BoundField DataField="TransactionDate" HeaderText="Date" DataFormatString="{0:MMM d, h:mm tt}" />
                </Columns>
            </asp:GridView>
        </div>
    </div>

    <div class="dashboard-section" style="margin-top:0;">
        <div class="dashboard-section__header">
            <h3>🧾 Recent Orders</h3>
            <asp:HyperLink runat="server" NavigateUrl="~/Orders.aspx" CssClass="text-action-link">View all →</asp:HyperLink>
        </div>
        <div class="table-responsive">
            <asp:GridView ID="gvRecentOrders" runat="server" CssClass="app-table" AutoGenerateColumns="false"
                GridLines="None" EmptyDataText="No orders placed yet.">
                <Columns>
                    <asp:BoundField DataField="CustomerName" HeaderText="Customer" />
                    <asp:BoundField DataField="ItemName" HeaderText="Item" />
                    <asp:BoundField DataField="Quantity" HeaderText="Qty" />
                    <asp:TemplateField HeaderText="Status">
                        <ItemTemplate>
                            <span class='<%# "status-pill status-pill--" + Eval("Status").ToString().ToLower() %>'><%# Eval("Status") %></span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="OrderDate" HeaderText="Date" DataFormatString="{0:MMM d, h:mm tt}" />
                </Columns>
            </asp:GridView>
        </div>
    </div>
</div>
