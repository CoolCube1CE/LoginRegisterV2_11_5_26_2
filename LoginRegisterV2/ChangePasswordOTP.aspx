<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="ChangePasswordOTP.aspx.cs" Inherits="LoginRegisterV2.ChangePasswordOTP" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="auth-shell">
        <div class="card text-center">
            <div class="card-icon-badge" aria-hidden="true">🛡️</div>
            <h2 class="card-title">Verify Your Identity</h2>
            <p class="card-subtitle">A code was sent to your email.</p>

            <asp:Label ID="lblMessage" runat="server" CssClass="alert alert-error" />

            <div class="form-group">
                <asp:TextBox ID="txtOtp" runat="server" CssClass="form-control text-center" />
            </div>
            <div class="btn-row" style="justify-content:center;">
                <asp:Button ID="btnVerify" runat="server" Text="Verify" OnClick="btnVerify_Click" CssClass="btn btn-primary" />
                <asp:Button ID="btnBack" runat="server" Text="Back" OnClick="btnBack_Click" CausesValidation="false" CssClass="btn btn-secondary" />
            </div>
        </div>
    </div>
</asp:Content>
