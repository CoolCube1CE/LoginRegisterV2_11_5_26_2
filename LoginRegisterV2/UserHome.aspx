<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="UserHome.aspx.cs" Inherits="LoginRegisterV2.UserHome" %>
<%@ Register TagPrefix="uc" TagName="DashboardOverview" Src="~/DashboardOverview.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="page-shell">
        <div class="dashboard-header">
            <div>
                <span class="badge badge-role-user">👤 User</span>
                <h2 class="mt-sm">Dashboard</h2>
                <p class="dashboard-user-line">Logged in as: <strong><asp:Label ID="lblUserEmail" runat="server" /></strong></p>
            </div>
        </div>

        <uc:DashboardOverview ID="dashboardOverview" runat="server" />
    </div>
  </asp:Content>
