<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/MP.Master" AutoEventWireup="true" CodeBehind="AdminVacacionesSolicitud.aspx.cs" Inherits="WebAppVacaciones.Pages.AdminVacacionesSolicitud" %>


<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
    Solicitudes Pendientes
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <style>
        .boton-estandar {
            width: 120px;
            height: 40px;
            font-size: 14px;
            text-align: center;
            padding: 10px;
            margin-right: 5px;
        }

        .custom-button:hover {
            background-color: #08a0de;
        }

        .centered-container {
            display: flex;
            justify-content: center;
        }

        .container-custom {
            max-width: 1200px;
            width: 100%;
        }

        .notification {
            position: fixed;
            top: 10px;
            right: 10px;
            z-index: 1000;
            max-width: 300px;
        }
    </style>
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bulma@0.9.4/css/bulma.min.css" />
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
    <section class="hero custom-hero">
        <div class="hero-body">
            <div class="container">
                <h1 class="title">Solicitudes de vacaciones</h1>
                <br />
                <h2 class="subtitle">Aquí puedes aprobar o rechazar las solicitudes de vacaciones de los empleados.</h2>
            </div>
        </div>
    </section>

    <section class="section">
        <div class="centered-container">
            <div class="container container-custom">
                <div class="field">
                    <label class="label">Nombre del Empleado</label>
                    <div class="control">
                        <asp:TextBox ID="txtSearch" runat="server" CssClass="input" Placeholder="Buscar..." AutoPostBack="true" OnTextChanged="txtSearch_TextChanged"></asp:TextBox>
                    </div>
                </div>

                <label class="label">Filtrar por:</label>
                <asp:DropDownList ID="ddlEstadoFiltro" runat="server" CssClass="input" AutoPostBack="true" OnSelectedIndexChanged="ddlEstadoFiltro_SelectedIndexChanged">
                <asp:ListItem Text="Pendiente" Value="Pendiente" Selected="True"></asp:ListItem>
                <asp:ListItem Text="Aprobada" Value="Aprobada"></asp:ListItem>
                <asp:ListItem Text="Rechazada" Value="Rechazada"></asp:ListItem>
                <asp:ListItem Text="Todos" Value=""></asp:ListItem>
                </asp:DropDownList>

                <asp:GridView
                    ID="gridDetallesEmpleado"
                    runat="server"
                    CssClass="table is-striped is-bordered is-hoverable"
                    AutoGenerateColumns="true"
                    OnRowCommand="gridDetallesEmpleado_RowCommand"
                    DataKeyNames="ID_Empleado">

                    <Columns>
                        <asp:TemplateField HeaderText="Acciones">
                            <ItemTemplate>
                                <asp:Button
                                    ID="btnAutorizar"
                                    runat="server"
                                    Text="Autorizar"
                                    CommandName="Autorizar"
                                    CommandArgument='<%# Eval("ID_Solicitud") %>'
                                    CssClass="button is-info boton-estandar" />

                                <asp:Button
                                    ID="btnDenegar"
                                    runat="server"
                                    Text="Denegar"
                                    CommandName="Denegar"
                                    CommandArgument='<%# Eval("ID_Solicitud") %>'
                                    CssClass="button is-danger boton-estandar" />

                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>

            </div>
        </div>
    </section>





</asp:Content>
