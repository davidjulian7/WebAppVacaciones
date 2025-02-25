<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/MP.Master" AutoEventWireup="true" CodeBehind="Registro de Empleados.aspx.cs" Inherits="WebAppVacaciones.Registro_de_Empleado" %>
<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
    Registro de Empleados
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <style>
        .custom-button:hover {
            background-color: #08a0de; /* Cambia el color de fondo del botón cuando se pasa el mouse por encima */
        }
    </style>
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
    
    <!-- Sección de la interfaz visual -->
    <section class="hero custom-hero">
        <div class="hero-body">
            <div class="container">
                <h1 class="title">Registro de Empleado</h1>
                <!-- Título principal de la página -->
                <br />
                <h2 class="subtitle">Completa el formulario para registrar un nuevo empleado.</h2>
                <!-- Subtítulo que da una pequeña descripción de la funcionalidad de la página -->
            </div>
        </div>
    </section>

    <!-- Formulario para el registro de empleados -->
    <section class="section">
        <div class="container">
        
            <!-- Campo para ingresar el nombre completo del empleado -->
            <div class="field">
                <label class="label">Nombre Completo</label>
                <div class="control">
                    <asp:TextBox CssClass="input" ID="txtNombre" runat="server" Placeholder="Nombre completo" Required="true"></asp:TextBox>
                    <!-- TextBox de ASP.NET para capturar el nombre del empleado. Tiene clase CSS "input" y es obligatorio -->
                </div>
            </div>

            <!-- Campo para seleccionar el puesto del empleado -->
            <div class="field">
                <label class="label">Puesto</label>
                <div class="control">
                    <div class="select">
                        <asp:DropDownList ID="DropDownListPuesto" runat="server" AutoPostBack="false">
                        </asp:DropDownList>
                        <!-- DropDownList de ASP.NET para seleccionar el puesto del empleado. Los valores serán cargados desde la base de datos -->
                    </div>
                </div>
            </div>

            <!-- Campo para seleccionar la fecha de ingreso del empleado -->
            <div class="field">
                <label class="label">Fecha Ingreso</label>
                <div class="control">
                    <asp:TextBox CssClass="input" ID="TextFechaIngreso" runat="server" Placeholder="Fecha_Ingreso" Required="true" TextMode="Date"></asp:TextBox>
                    <!-- TextBox de ASP.NET para ingresar la fecha de ingreso del empleado. El campo es obligatorio y el formato es tipo "date" -->
                </div>
            </div>

            <!-- Campo para seleccionar el PDV (Punto de Venta) del empleado -->
            <div class="field">
                <label class="label">PDV</label>
                <div class="control">
                    <div class="select">
                        <asp:DropDownList ID="ddlPDV" runat="server" AutoPostBack="false">
                        </asp:DropDownList>
                        <!-- DropDownList de ASP.NET para seleccionar el PDV del empleado. Los valores serán cargados desde la base de datos -->
                    </div>
                </div>
            </div>

            <!-- Botón para registrar el empleado -->
            <div class="field">
                <div class="control">
                    <asp:Button CssClass="button custom-button" ID="btnRegistrar" runat="server" Text="Registrar" OnClick="btnRegistrar_Click" />
                    <!-- Botón de ASP.NET que al hacer clic invoca el evento "btnRegistrar_Click" definido en el código detrás (CreateEmployee.aspx.cs). 
                         Tiene una clase CSS personalizada "custom-button" -->
                </div>
            </div>
        </div>
    </section>

</asp:Content>
