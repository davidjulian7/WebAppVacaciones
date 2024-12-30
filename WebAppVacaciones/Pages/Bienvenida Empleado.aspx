﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Empleado.Master" AutoEventWireup="true" CodeBehind="Bienvenida Empleado.aspx.cs" Inherits="WebAppVacaciones.Pages.Bienvenida_Empleado" %>

<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
    ¡Bienvenido!
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">

    <style>
        .welcome-container {
            display: flex;
            flex-direction: column;
            justify-content: center;
            align-items: center;
            height: 80vh;   
            text-align: center;
        }

            .welcome-container h1 {
                font-size: 2.5rem;
                margin-bottom: 1rem;
            }

            .welcome-container p {
                font-size: 1.2rem;
                margin-bottom: 2rem;
            }

        /* Estilo para el nombre del usuario */
        #lblusuario {
            display: block;
            font-size: larger; /* Tamaño más grande */
            color: gray;
            margin-bottom: 2rem; /* Añade espacio entre el usuario y el contenido */
        }
    </style>

    <div class="welcome-container">
        <!-- El lblusuario está centrado y con un tamaño más grande -->
        <asp:Label Style="font-size: 50px" class="title" runat="server" ID="Label1"></asp:Label>
        <h1 style="font-size: 40px" tyle="color:#FF0000" class="title">¡Bienvenido al Sistema!</h1>
        <p>Encantados de tenerte de vuelta. Haz clic en la barra de navegacion para comenzar.</p>
        <!-- Aquí puedes agregar un botón o más contenido si es necesario -->
    </div>



</asp:Content>
