<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="LoginRegisterV2.Login" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="auth-shell">
        <div class="card">
            <div class="card-icon-badge" aria-hidden="true">🔐</div>
            <h2 class="card-title">Welcome Back</h2>
            <p class="card-subtitle">Sign in to your Beverage Store account</p>

            <asp:Label ID="lblMessage" runat="server" CssClass="alert alert-error" />

            <div class="form-group">
                <label class="form-label" for="<%= txtEmail.ClientID %>">Email</label>
                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" />
            </div>

            <div class="form-group">
                <label class="form-label" for="<%= txtPassword.ClientID %>">Password</label>
                <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control" />
            </div>

            <asp:Button ID="btnLogin" runat="server" Text="Login" OnClick="btnLogin_Click" CssClass="btn btn-primary btn-block" />

            <div class="divider"></div>

            <p class="text-center mb-0">
                <asp:LinkButton ID="lnkRegister" runat="server" OnClick="lnkRegister_Click">Don't have an account? Register</asp:LinkButton>
            </p>
        </div>
    </div>

</asp:Content>
