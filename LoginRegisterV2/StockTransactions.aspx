<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="StockTransactions.aspx.cs" Inherits="LoginRegisterV2.StockTransactions" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="page-shell">
        <div class="dashboard-header">
            <div>
                <h2 class="mt-sm">🔄 Stock Transactions</h2>
                <p class="dashboard-user-line">Record stock coming in or going out, and review the full movement history.</p>
            </div>
        </div>

        <asp:Label ID="lblMessage" runat="server" CssClass="alert alert-error" />
        <asp:Label ID="lblSuccess" runat="server" CssClass="alert alert-success" />

        <div class="card card--compact mb-sm">
            <h3 class="mt-0">Record a Stock Movement</h3>
            <div class="form-grid">
                <div class="form-group mb-0">
                    <label class="form-label">Item</label>
                    <asp:DropDownList ID="ddlItem" runat="server" CssClass="form-control" DataTextField="ItemName" DataValueField="ItemId" />
                </div>
                <div class="form-group mb-0">
                    <label class="form-label">Movement Type</label>
                    <asp:DropDownList ID="ddlType" runat="server" CssClass="form-control">
                        <asp:ListItem Text="Stock In (received)" Value="IN" />
                        <asp:ListItem Text="Stock Out (removed)" Value="OUT" />
                    </asp:DropDownList>
                </div>
                <div class="form-group mb-0">
                    <label class="form-label">Quantity</label>
                    <asp:TextBox ID="txtQuantity" runat="server" CssClass="form-control" TextMode="Number" />
                </div>
                <div class="form-group mb-0">
                    <label class="form-label">Notes (optional)</label>
                    <asp:TextBox ID="txtNotes" runat="server" CssClass="form-control" placeholder="e.g. Supplier delivery, damaged goods" />
                </div>
            </div>
            <div class="btn-row">
                <asp:Button ID="btnRecord" runat="server" Text="Record Transaction" CssClass="btn btn-primary" OnClick="btnRecord_Click" />
            </div>
        </div>

        <div class="dashboard-section" style="margin-top:0;">
            <h3>Transaction History</h3>
            <div class="table-responsive">
                <asp:GridView ID="gvTransactions" runat="server" CssClass="app-table" AutoGenerateColumns="false"
                    GridLines="None" EmptyDataText="No stock movements recorded yet.">
                    <Columns>
                        <asp:BoundField DataField="TransactionDate" HeaderText="Date" DataFormatString="{0:MMM d, yyyy h:mm tt}" />
                        <asp:BoundField DataField="ItemName" HeaderText="Item" />
                        <asp:TemplateField HeaderText="Type">
                            <ItemTemplate>
                                <span class='<%# Eval("TransactionType").ToString() == "IN" ? "stock-badge stock-badge--in" : "stock-badge stock-badge--out" %>'>
                                    <%# Eval("TransactionType").ToString() == "IN" ? "▲ IN" : "▼ OUT" %>
                                </span>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Quantity" HeaderText="Qty" />
                        <asp:BoundField DataField="Reference" HeaderText="Reference" />
                        <asp:BoundField DataField="Notes" HeaderText="Notes" />
                        <asp:BoundField DataField="PerformedBy" HeaderText="By" />
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>
</asp:Content>
