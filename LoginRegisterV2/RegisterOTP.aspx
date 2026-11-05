<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="RegisterOTP.aspx.cs" Inherits="LoginRegisterV2.RegisterOTP" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="auth-shell">
        <div class="card text-center">
            <div class="card-icon-badge" aria-hidden="true">📧</div>
            <h2 class="card-title">Verify Your Email</h2>
            <p class="card-subtitle">Enter the code we sent to your email.</p>

            <asp:Label ID="lblMessage" runat="server" CssClass="alert alert-error" />

            <div class="form-group">
                <asp:TextBox ID="txtOtp" runat="server" CssClass="form-control text-center" />
            </div>
            <asp:Button ID="btnVerify" runat="server" Text="Verify" OnClick="btnVerify_Click" CssClass="btn btn-primary btn-block" />
        </div>
    </div>
</asp:Content>
