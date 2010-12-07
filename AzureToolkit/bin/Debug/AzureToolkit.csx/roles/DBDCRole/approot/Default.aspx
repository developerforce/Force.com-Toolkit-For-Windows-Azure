<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="DBDCRole._Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">

   <table>
        <tr>
            <td valign="top">
                <asp:Button ID="Button2" runat="server" onclick="Query_Click" Text="Query" />
                <br />
                <asp:Button ID="Button3" runat="server" onclick="CreateSingle_Click" 
                    Text="Create Single" />
                <asp:Button ID="Button8" runat="server" onclick="CreateMultiple_Click" 
                    Text="Create Multiple" />
                <br />
                <asp:Button ID="Button4" runat="server" onclick="DeleteByIds_Click" 
                    Text="Delete By Ids" />
                <asp:Button ID="Button5" runat="server" onclick="DeleteByQuery_Click" 
                    Text="Delete By Query" />
                <asp:Button ID="Button6" runat="server" onclick="DeleteByQueryResult_Click" 
                    Text="Delete By Query Result" />
            </td>
            <td rowspan="2">
    <asp:GridView ID="GridView1" runat="server" BackColor="White" OnRowEditing="EditPhone"
        BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" CellPadding="4" 
        ForeColor="Black" GridLines="Vertical" AutoGenerateColumns="false">
        <Columns>
            <asp:BoundField DataField="FirstName" HeaderText="First Name" />
            <asp:BoundField DataField="LastName" HeaderText="Last Name" />
            <asp:TemplateField HeaderText="Phone">
                <ItemTemplate>
            <asp:TextBox ID="newPhone" runat="server"
            Text='<%# Eval("Phone") %>' Width="175px"
            visible="true"></asp:TextBox>
            <asp:HiddenField Value='<%# Eval("Id") %>' runat="server" ID="IdField"  />
                </ItemTemplate>
            </asp:TemplateField>

            <asp:ButtonField Text="Update" ButtonType="Button" CommandName="Edit"
                        Visible="True" />

        </Columns>
         
        <AlternatingRowStyle BackColor="White" />
        <FooterStyle BackColor="#CCCC99" />
        <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
        <RowStyle BackColor="#F7F7DE" />
        <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
        <SortedAscendingCellStyle BackColor="#FBFBF2" />
        <SortedAscendingHeaderStyle BackColor="#848384" />
        <SortedDescendingCellStyle BackColor="#EAEAD3" />
        <SortedDescendingHeaderStyle BackColor="#575357" />
    </asp:GridView>
    </td>
    </tr>
    <tr>
    <td>
        <asp:TextBox runat="server" ID="outputbox" Height="432px" TextMode="MultiLine" 
            Width="200px"></asp:TextBox>
    </td>
    </tr>
    </table>

</asp:Content>
