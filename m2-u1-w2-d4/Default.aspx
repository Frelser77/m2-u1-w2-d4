<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="m2_u1_w2_d4._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <div class="row">
            <div class="col-12">
                <h2>Calcola il Preventivo</h2>
                <p>Scegli la tua auto, gli optional e gli anni di garanzia per sapere quanto poco spenderai!</p>
                <div class="row">
                    <div class="col-12 col-md-8">
                        <asp:DropDownList ID="Automobili" runat="server" CssClass="form-control"
                            OnSelectedIndexChanged="Automobili_SelectedIndexChanged" AutoPostBack="true">
                        </asp:DropDownList>
                        <asp:CheckBoxList ID="CheckBoxListOptional" runat="server" CssClass="form-check"></asp:CheckBoxList>
                    </div>
                    <div class="col-12 col-md-4 d-flex flex-column justify-content-between">
                        <div>
                            <p>Quanti anni di garanzia?</p>
                            <asp:DropDownList ID="ddlAnniGaranzia" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>
                        <asp:Button ID="btnCalcola" runat="server" CssClass="btn btn-outline-secondary mt-2" Text="Calcola Preventivo" OnClick="BtnCalcola_Click" />
                    </div>
                </div>
                <div class="card mt-4">
                    <div class="row">
                        <div class="col-12 col-md-8">
                            <div class="card-body d-flex flex-column align-items-start justify-content-between">
                                <div class="mb-3">
                                    <h5 class="card-title">Dettaglio Preventivo</h5>
                                    <asp:Literal ID="testoPreventivo" runat="server"></asp:Literal>
                                </div>
                                <a href="#" class="btn btn-outline-secondary mt-2">Vai alla Concessionaria</a>
                            </div>
                        </div>
                        <div class="col-12 col-md-4">
                            <asp:Image ID="imgAuto" runat="server" ImageUrl="~/Content/images/default.png" CssClass="card-img-top" alt="Immagine Auto" Height="200px" Width="300px" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

