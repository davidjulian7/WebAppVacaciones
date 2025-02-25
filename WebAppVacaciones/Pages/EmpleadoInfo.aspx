<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Empleado.Master" AutoEventWireup="true" CodeBehind="EmpleadoInfo.aspx.cs" Inherits="WebAppVacaciones.Pages.EmpleadoInfo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
    Tu Información
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <style>
        .modal-card.custom-modal {
            width: auto; /* Ajusta el ancho automáticamente al contenido */
            max-width: 90%; /* Limita el ancho máximo al 90% de la ventana */
            height: auto; /* Ajusta la altura automáticamente */
            max-height: 90vh; /* Limita la altura al 90% de la altura de la ventana */
            overflow: visible; /* Evita que se recorte el contenido */
        }

        .modal-card-body.custom-modal-body {
            padding: 1rem; /* Ajusta el espacio interno */
        }

        /* Ajusta el GridView para que no se desborde */
        .table.custom-table {
            table-layout: auto; /* Permite que las columnas se ajusten automáticamente */
            width: 100%; /* Asegura que la tabla ocupe todo el espacio disponible */
        }

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
                <h1 class="title">Tu Información</h1>
                <br />
                <h2 class="subtitle">Aquí puedes consultar, tu información y solicitar dias de vacaciones.</h2>
            </div>
        </div>
    </section>

    <section class="section">
        <div class="centered-container">
            <div class="container container-custom">


                <asp:GridView ID="gridDetallesEmpleado" runat="server" CssClass="table is-striped is-bordered is-hoverable" AutoGenerateColumns="false" OnRowCommand="gridDetallesEmpleado_RowCommand">
                    <Columns>
                        <asp:BoundField DataField="ID_Empleado" HeaderText="ID" />
                        <asp:BoundField DataField="Usuario" HeaderText="Usuario" />
                        <asp:BoundField DataField="Nombre_Rol" HeaderText="Rol" />
                        <asp:BoundField DataField="Nombre_Region" HeaderText="Región" />
                        <asp:BoundField DataField="Nombre_Plaza" HeaderText="Plaza" />
                        <asp:BoundField DataField="Nombre_PDV" HeaderText="PDV" />
                        <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                        <asp:BoundField DataField="Puesto" HeaderText="Puesto" />
                        <asp:BoundField DataField="Fecha_Ingreso" HeaderText="Ingreso" DataFormatString="{0:yyyy-MM-dd}" />
                        <asp:BoundField DataField="Mes_Aniversario" HeaderText="Mes Aniversario" />
                        <asp:BoundField DataField="Año_Aniversario" HeaderText="Año Aniversario" />
                        <asp:BoundField DataField="Antigüedad" HeaderText="Antigüedad" />
                        <asp:BoundField DataField="Dias_por_Año" HeaderText="Días por Año" />
                        <asp:BoundField DataField="Dias_Disponibles" HeaderText="Días Disponibles" />
                        <asp:BoundField DataField="Dias_Disfrutados" HeaderText="Días Disfrutados" />
                        <asp:TemplateField HeaderText="Vacaciones">
                            <ItemTemplate>
                                <asp:Button
                                    ID="btnSolicitar"
                                    runat="server"
                                    Text="Solicitar"
                                    CommandName="Solicitar"
                                    CommandArgument='<%# Eval("ID_Empleado") %>'
                                    CssClass="button is-success boton-estandar"
                                    OnClientClick="abrirModalVadAdd(); return false;" />
                                <asp:Button
                                    ID="btnVacaciones"
                                    runat="server"
                                    Text="Aprobados"
                                    CommandName="Consultar"
                                    CommandArgument='<%# Eval("ID_Empleado") %>'
                                    CssClass="button is-info boton-estandar" />

                                <asp:Button
                                    ID="btnDiasSolicitados"
                                    runat="server"
                                    Text="Historial"
                                    CommandName="Solicitados"
                                    CommandArgument='<%# Eval("ID_Empleado") %>'
                                    CssClass="button is-warning boton-estandar" />

                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </section>

    <!-- Modal para mostrar las vacacione -->


    <div class="modal" id="modalVacaciones">
        <div class="modal-background"></div>
        <div class="modal-card custom-modal">
            <header class="modal-card-head">
                <p class="modal-card-title">Historial Dias Disfrutados</p>
            </header>
            <section class="modal-card-body custom-modal-body">
                <asp:GridView ID="gridVacaciones" runat="server" CssClass="table is-responsive is-striped is-bordered" AutoGenerateColumns="false">
                    <Columns>
                        <asp:BoundField DataField="ID_Empleado" HeaderText="ID Empleado" />
                        <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                        <asp:BoundField DataField="Fecha" HeaderText="Fecha" />
                        <asp:BoundField DataField="TipoDia" HeaderText="TipoDia" />
                    </Columns>
                </asp:GridView>
            </section>
            <footer class="modal-card-foot">
                <button class="button" onclick="cerrarModal()">Cerrar</button>
            </footer>
        </div>
    </div>

    <script>
        function abrirModal() {
            document.getElementById('modalVacaciones').classList.add('is-active');
        }

        function cerrarModal() {
            document.getElementById('modalVacaciones').classList.remove('is-active');
        }
    </script>

    <!-- Modal para solicitar vacaciones -->
    <div class="modal" id="modalSolicitarVacacion">
        <div class="modal-background"></div>
        <div class="modal-card">
            <header class="modal-card-head">
                <p class="modal-card-title">Solicitar Dia de Vacaciones</p>
            </header>
            <section class="modal-card-body">

                <div class="field">
                    <label class="label">Tipo de dia</label>
                    <div class="control">


                        <asp:DropDownList ID="DropDownListDia" runat="server" CssClass="input">
                            <asp:ListItem Text="Seleccione..." Value="" />
                            <asp:ListItem Text="Todo el día" Value="N"></asp:ListItem>
                            <asp:ListItem Text="Mañana" Value="M"></asp:ListItem>
                            <asp:ListItem Text="Tarde" Value="T"></asp:ListItem>
                        </asp:DropDownList>


                    </div>
                </div>


                <div class="field">
                    <label class="label">Fecha de Ingreso</label>
                    <div class="control">
                        <asp:TextBox ID="TextBox2" runat="server" CssClass="input" TextMode="Date"></asp:TextBox>
                    </div>
                </div>

            </section>
            <footer class="modal-card-foot">
                <asp:Button
                    ID="Button1"
                    runat="server"
                    CssClass="button is-primary"
                    Text="Guardar"
                    OnClick="GuardarVacacion" />
                <asp:HiddenField ID="HiddenField1" runat="server" />

                <button class="button" onclick="cerrarModalVadAdd()">Cerrar</button>
            </footer>
        </div>
    </div>

    <script>
        function abrirModalVadAdd() {
            var modal = document.getElementById('modalSolicitarVacacion');
            modal.classList.add('is-active');
        }

        function cerrarModalVadAdd() {
            var modal = document.getElementById('modalSolicitarVacacion');
            modal.classList.remove('is-active');
        }


        function disableOtherOptions() {
            var ddl = document.getElementById('<%= DropDownListDia.ClientID %>');
            for (var i = 2; i < ddl.options.length; i++) {
                ddl.options[i].disabled = true;
            }
        }

        // Llama a la función al cargar la página
        window.onload = disableOtherOptions;


    </script>









    <!-- Modal para mostrar las vacacione -->
    <div class="modal" id="modalSolicitados">
        <div class="modal-background"></div>
        <div class="modal-card custom-modal">
            <header class="modal-card-head">
                <p class="modal-card-title">Días Solicitados Pendientes de Autorización</p>
            </header>
            <section class="modal-card-body custom-modal-body">
                <asp:GridView ID="GridViewSolicitados" runat="server" CssClass="table is-responsive is-striped is-bordered" AutoGenerateColumns="false">
                    <Columns>
                        <asp:BoundField DataField="ID_Empleado" HeaderText="ID Empleado" />
                        <asp:BoundField DataField="Fecha" HeaderText="Fecha" />
                        <asp:BoundField DataField="MedioDia" HeaderText="Tipo de Día" />
                        <asp:BoundField DataField="Estado" HeaderText="Estado" />
                        <asp:BoundField DataField="Comentarios" HeaderText="Comentarios" />
                        <asp:BoundField DataField="Fecha_Solicitud" HeaderText="Fecha Solicitud" />
                        <asp:BoundField DataField="Fecha_Resolucion" HeaderText="Fecha Resolución" />
                    </Columns>
                </asp:GridView>
            </section>
            <footer class="modal-card-foot">
                <button class="button" onclick="cerrarModal()">Cerrar</button>
            </footer>
        </div>
    </div>


    <script>
        function abrirModalSolicitados() {
            document.getElementById('modalSolicitados').classList.add('is-active');
        }

        function cerrarModalSolicitados() {
            document.getElementById('modalSolicitados').classList.remove('is-active');
        }
    </script>
    <!-- -->








</asp:Content>
