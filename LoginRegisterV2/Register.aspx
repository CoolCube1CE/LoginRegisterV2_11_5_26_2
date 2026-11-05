<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="LoginRegisterV2.Register" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
function togglePasswordVisibility(inputId, toggleLink) {
    var input = document.getElementById(inputId);
    if (input.type === 'password') {
        input.type = 'text';
        toggleLink.innerText = 'Hide';
    } else {
        input.type = 'password';
        toggleLink.innerText = 'Show';
    }
        }



        function checkPasswordStrength() {
            var pw = document.getElementById("<%= txtPassword.ClientID %>").value;
            var length = pw.length >= 8 && pw.length <= 16;
            var upper = /[A-Z]/.test(pw);
            var lower = /[a-z]/.test(pw);
            var number = /[0-9]/.test(pw);
            var special = /[^a-zA-Z0-9]/.test(pw);

            setReq("reqLength", length);
            setReq("reqUpper", upper);
            setReq("reqLower", lower);
            setReq("reqNumber", number);
            setReq("reqSpecial", special);

            var strength = [length, upper, lower, number, special].filter(Boolean).length;
            var strengthLabel = document.getElementById("lblStrength");
            if (strength <= 2) { strengthLabel.innerText = "Weak"; strengthLabel.style.color = "var(--danger)"; }
            else if (strength <= 4) { strengthLabel.innerText = "Medium"; strengthLabel.style.color = "var(--warning)"; }
            else { strengthLabel.innerText = "Strong"; strengthLabel.style.color = "var(--success)"; }
        }
        function setReq(id, ok) {
            document.getElementById(id).style.color = ok ? "var(--success)" : "var(--danger)";
        }

        function filterNameKey(e) {
            var charCode = e.which || e.keyCode;
            var char = String.fromCharCode(charCode);
            if (!/^[a-zA-Z ,.]$/.test(char)) {
                e.preventDefault();
                return false;
            }
            return true;
        }

        function limitPhoneNumber(input) {
            input.value = input.value.replace(/[^0-9+]/g, '');
            if (input.value.indexOf('+639') === 0) {
                input.maxLength = 13;
            } else {
                input.maxLength = 11;
            }
        }

        function setBirthdateMode(mode) {
            document.getElementById('<%= hdnBirthdateMode.ClientID %>').value = mode;
            document.getElementById('calendarBirthdate').style.display = (mode === 'calendar') ? 'block' : 'none';
            document.getElementById('manualBirthdate').style.display = (mode === 'manual') ? 'block' : 'none';
        }

        function openTerms() {
            document.getElementById('termsModal').classList.add('is-open');
        }
        function closeTerms() {
            document.getElementById('termsModal').classList.remove('is-open');
            document.getElementById('<%= chkDataPrivacy.ClientID %>').disabled = false;
        }

        window.onload = function () {
            var chk = document.getElementById('<%= chkDataPrivacy.ClientID %>');
            var btn = document.getElementById('<%= btnRegister.ClientID %>');
            chk.onclick = function () {
                btn.disabled = !chk.checked;
            };
        };
    </script>

    <div class="auth-shell auth-shell--wide">
        <div class="card">
            <div class="card-icon-badge" aria-hidden="true">📝</div>
            <h2 class="card-title">Create Your Account</h2>
            <p class="card-subtitle">Join the Beverage Store Management System</p>

            <div class="form-group">
                <label class="form-label" for="<%= txtFullName.ClientID %>">Full Name</label>
                <asp:TextBox ID="txtFullName" runat="server" CssClass="form-control" onkeypress="return filterNameKey(event)" />
                <asp:Label ID="lblFullNameError" runat="server" CssClass="field-error" />
            </div>

            <div class="form-group">
                <label class="form-label" for="<%= txtAddress.ClientID %>">Address</label>
                <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" />
                <asp:Label ID="lblAddressError" runat="server" CssClass="field-error" />
            </div>

            <div class="form-group">
                <label class="form-label" for="<%= txtEmail.ClientID %>">Email</label>
                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" />
                <asp:Label ID="lblEmailError" runat="server" CssClass="field-error" />
            </div>

            <div class="form-group">
                <label class="form-label" for="<%= txtNumber.ClientID %>">Phone Number</label>
                <asp:TextBox ID="txtNumber" runat="server" CssClass="form-control" oninput="limitPhoneNumber(this)" />
                <span class="form-hint">Format: 09XXXXXXXXX or +639XXXXXXXXX</span>
                <asp:Label ID="lblNumberError" runat="server" CssClass="field-error" />
            </div>

            <div class="form-group">
                <label class="form-label">Birth Date</label>
                <div class="radio-inline-group">
                    <label><input type="radio" name="birthdateMode" value="calendar" checked="checked" onclick="setBirthdateMode('calendar')" /> Use Calendar</label>
                    <label><input type="radio" name="birthdateMode" value="manual" onclick="setBirthdateMode('manual')" /> Enter Manually</label>
                </div>
                <asp:HiddenField ID="hdnBirthdateMode" runat="server" Value="calendar" />

                <div id="calendarBirthdate">
                    <asp:TextBox ID="txtBirthDate" runat="server" TextMode="Date" CssClass="form-control" />
                </div>
                <div id="manualBirthdate" class="form-row" style="display:none;">
                    <asp:TextBox ID="txtYear" runat="server" placeholder="YYYY" MaxLength="4" CssClass="form-control input-sm" oninput="this.value=this.value.replace(/[^0-9]/g,'')" />
                    <asp:TextBox ID="txtMonth" runat="server" placeholder="MM" MaxLength="2" CssClass="form-control input-xs" oninput="this.value=this.value.replace(/[^0-9]/g,'')" />
                    <asp:TextBox ID="txtDay" runat="server" placeholder="DD" MaxLength="2" CssClass="form-control input-xs" oninput="this.value=this.value.replace(/[^0-9]/g,'')" />
                </div>
                <asp:Label ID="lblBirthDateError" runat="server" CssClass="field-error" />
            </div>

            <div class="form-group">
                <label class="form-label" for="<%= txtPassword.ClientID %>">Password</label>
                <div class="input-with-action">
                    <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control"
                        onkeyup="checkPasswordStrength()" onpaste="return false" oncopy="return false" oncut="return false" oncontextmenu="return false" />
                    <a href="javascript:void(0);" class="text-action-link" onclick="togglePasswordVisibility('<%= txtPassword.ClientID %>', this)">Show</a>
                </div>

                <div class="pw-requirements">
                    <span id="reqLength">8-16 characters</span>
                    <span id="reqUpper">uppercase letter</span>
                    <span id="reqLower">lowercase letter</span>
                    <span id="reqNumber">number</span>
                    <span id="reqSpecial">special character</span>
                </div>
                <p class="pw-strength-row">Strength: <span id="lblStrength">Weak</span></p>
                <asp:Label ID="lblPasswordError" runat="server" CssClass="field-error" />
            </div>

            <div class="form-group">
                <label class="form-label" for="<%= txtConfirmPassword.ClientID %>">Confirm Password</label>
                <div class="input-with-action">
                    <asp:TextBox ID="txtConfirmPassword" runat="server" TextMode="Password" CssClass="form-control"
                        onpaste="return false" oncopy="return false" oncut="return false" oncontextmenu="return false" />
                    <a href="javascript:void(0);" class="text-action-link" onclick="togglePasswordVisibility('<%= txtConfirmPassword.ClientID %>', this)">Show</a>
                </div>
                <asp:Label ID="lblConfirmPasswordError" runat="server" CssClass="field-error" />
            </div>

            <div class="form-group">
                <label class="form-label" for="<%= ddlAccessType.ClientID %>">Access Type</label>
                <asp:DropDownList ID="ddlAccessType" runat="server" CssClass="form-control">
                    <asp:ListItem Text="-- Select --" Value="" />
                    <asp:ListItem Text="User" Value="User" />
                    <asp:ListItem Text="Admin" Value="Admin" />
                    <asp:ListItem Text="Super Admin" Value="Super Admin" />
                </asp:DropDownList>
                <asp:Label ID="lblAccessTypeError" runat="server" CssClass="field-error" />
            </div>

            <div class="form-group">
                <p class="mb-sm"><a href="javascript:void(0);" onclick="openTerms()">Data Privacy Terms and Agreement</a></p>
                <div class="checkbox-row">
                    <asp:CheckBox ID="chkDataPrivacy" runat="server" Text="I have read and agree to the Data Privacy Terms and Agreement" Enabled="false" />
                </div>
            </div>

            <asp:Label ID="lblGeneralError" runat="server" CssClass="alert alert-error" />

            <div class="btn-row">
                <asp:Button ID="btnRegister" runat="server" Text="Register" OnClick="btnRegister_Click" Enabled="false" CssClass="btn btn-primary" />
                <asp:Button ID="btnBack" runat="server" Text="Back to Login" OnClick="btnBack_Click" CausesValidation="false" CssClass="btn btn-secondary" />
            </div>
        </div>
    </div>

    <div id="termsModal" class="modal-overlay">
        <div class="modal-box">
            <h3>Data Privacy Terms and Agreement</h3>
            <p>
                [Placeholder — replace with your school/organization's actual data privacy policy text.]
                By registering, you consent to the collection and processing of your personal information
                (full name, address, email, phone number, birthdate) solely for the purpose of account
                creation and authentication within this system, in accordance with the Data Privacy Act
                of 2012 (RA 10173).
            </p>
            <button type="button" class="btn btn-primary" onclick="closeTerms()">I Understand, Close</button>
        </div>
    </div>
</asp:Content>
