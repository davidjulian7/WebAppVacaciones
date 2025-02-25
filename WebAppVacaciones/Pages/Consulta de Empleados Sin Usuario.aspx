<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/MP.Master" AutoEventWireup="true" CodeBehind="Consulta de Empleados Sin Usuario.aspx.cs" Inherits="WebAppVacaciones.Pages.Consulta_Empleado_Sin_Usuario" %>

<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
    Consulta de Empleados Sin Usuario
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
                <h1 class="title">Consulta de Empleados Sin Usuario</h1>
                <br />
                <h2 class="subtitle">Aquí puedes consultar, modificar o eliminar empleados sin usuario.</h2>
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

                <asp:GridView
                    ID="gridDetallesEmpleado"
                    runat="server"
                    CssClass="table is-striped is-bordered is-hoverable"
                    AutoGenerateColumns="false"
                    OnRowCommand="gridDetallesEmpleado_RowCommand"
                    DataKeyNames="ID_Empleado">
                    <Columns>
                        <asp:BoundField DataField="ID_Empleado" HeaderText="ID" />
                        <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                        <asp:BoundField DataField="Puesto" HeaderText="Puesto" />
                        <asp:BoundField DataField="Fecha_Ingreso" HeaderText="Fecha Ingreso" DataFormatString="{0:yyyy-MM-dd}" />
                        <asp:BoundField DataField="Nombre_PDV" HeaderText="PDV" />
                        <asp:TemplateField HeaderText="Acciones">
                            <ItemTemplate>
                                <asp:Button
                                    ID="btnActualizar"
                                    runat="server"
                                    Text="Actualizar"
                                    CommandName="Actualizar"
                                    CommandArgument='<%# Container.DataItemIndex %>'
                                    CssClass="button is-warning boton-estandar" />
                                <asp:Button
                                    ID="btnEliminar"
                                    runat="server"
                                    Text="Eliminar"
                                    CommandName="Eliminar"
                                    CommandArgument='<%# Eval("ID_Empleado") %>'
                                    CssClass="button is-danger boton-estandar" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>

            </div>
        </div>
    </section>


    <!-- Modal para mostrar las vacaciones -->
    <div class="modal" id="modalModificarEmpleadoSinUsuario">
        <div class="modal-background"></div>
        <div class="modal-card">
            <header class="modal-card-head">
                <p class="modal-card-title">Modificar Empleado Sin Usuario</p>
            </header>
            <section class="modal-card-body">
                <div class="field">
                    <label class="label">Nombre</label>
                    <div class="control">
                        <asp:TextBox ID="txtNombre" runat="server" CssClass="input" Placeholder="Nombre del empleado"></asp:TextBox>
                    </div>
                </div>


                <div class="field">
                    <label class="label">Puesto</label>
                    <div class="control">
                        <asp:DropDownList ID="DropDownListPuesto" runat="server" CssClass="input"></asp:DropDownList>
                    </div>
                </div>


                <div class="field">
                    <label class="label">Fecha de Ingreso</label>
                    <div class="control">
                        <asp:TextBox ID="txtFechaIngreso" runat="server" CssClass="input" TextMode="Date"></asp:TextBox>
                    </div>
                </div>


                <div class="field">
                    <label class="label">PDV</label>
                    <div class="control">
                        <asp:DropDownList ID="ddlPDV" runat="server" CssClass="input"></asp:DropDownList>
                    </div>
                </div>
            </section>
            <footer class="modal-card-foot">
                <asp:Button
                    ID="btnGuardar"
                    runat="server"
                    CssClass="button is-primary"
                    Text="Guardar"
                    OnClick="btnGuardar_Click" />
                <asp:HiddenField ID="hfEmpleadoID" runat="server" />

                <button class="button" onclick="cerrarModal()">Cerrar</button>
            </footer>
        </div>
    </div>


    <script>
        function abrirModal() {
            var modal = document.getElementById('modalModificarEmpleadoSinUsuario');
            modal.classList.add('is-active');
        }

        function cerrarModal() {
            var modal = document.getElementById('modalModificarEmpleadoSinUsuario');
            modal.classList.remove('is-active');
        }

    </script>
</asp:Content>
