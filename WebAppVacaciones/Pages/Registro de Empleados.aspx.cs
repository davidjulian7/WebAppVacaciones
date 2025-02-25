using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebAppVacaciones
{
    public partial class Registro_de_Empleado : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Verifica si la página se está cargando por primera vez (no un postback)
            if (!IsPostBack)
            {
                // Llama a los métodos que cargan los DropDownList de PDV y Puestos
                CargarPDV();    // Cargar el DropDownList de PDV desde la base de datos
                CargarPuestos(); // Cargar el DropDownList de Puestos desde la base de datos
            }
        }


        // Método para cargar el DropDownList de PDV (Punto de Venta) desde la base de datos
        private void CargarPDV()
        {
            // Obtener la cadena de conexión desde el archivo Web.config
            string connectionString = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;

            // Se utiliza una conexión a la base de datos SQL Server
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                // Usar un procedimiento almacenado llamado "sp_consultar_pdv"
                SqlCommand cmd = new SqlCommand("sp_consultar_pdv", con);
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    // Abre la conexión a la base de datos
                    con.Open();
                    // Ejecuta el procedimiento y obtiene los resultados
                    SqlDataReader reader = cmd.ExecuteReader();

                    // Establece los datos del DropDownList
                    ddlPDV.DataSource = reader;
                    ddlPDV.DataTextField = "Nombre_PDV";  // El nombre que se mostrará en el DropDownList
                    ddlPDV.DataValueField = "ID_PDV";     // El valor que se almacenará internamente
                    ddlPDV.DataBind();
                }
                catch (Exception ex)
                {
                    // En caso de error, muestra un mensaje
                    MostrarMensaje("Error al cargar PDV: " + ex.Message, true);
                }
            }

            // Añade un elemento predeterminado al inicio del DropDownList
            ddlPDV.Items.Insert(0, new ListItem("Seleccione un PDV", "0"));
        }

        // Método para cargar el DropDownList de Puestos desde la base de datos
        private void CargarPuestos()
        {
            // Obtener la cadena de conexión desde el archivo Web.config
            string connectionString = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;

            // Usar la conexión SQL para ejecutar un procedimiento almacenado
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                // Usar el procedimiento almacenado "sp_puesto" para cargar los puestos
                SqlCommand cmd = new SqlCommand("sp_puesto", con);
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    // Abrir la conexión a la base de datos
                    con.Open();
                    // Ejecutar el procedimiento almacenado y obtener los datos
                    SqlDataReader reader = cmd.ExecuteReader();

                    // Establecer los datos en el DropDownList de Puestos
                    DropDownListPuesto.DataSource = reader;
                    DropDownListPuesto.DataTextField = "Puesto";  // Mostrar el nombre del puesto
                    DropDownListPuesto.DataValueField = "Puesto"; // Guardar el nombre del puesto como valor
                    DropDownListPuesto.DataBind();
                }
                catch (Exception ex)
                {
                    // En caso de error, mostrar un mensaje
                    MostrarMensaje("Error al cargar puestos: " + ex.Message, true);
                }
            }

            // Añadir un elemento predeterminado al inicio del DropDownList
            DropDownListPuesto.Items.Insert(0, new ListItem("Seleccione un Puesto", "0"));
        }

        // Evento que se ejecuta al hacer clic en el botón "Registrar"
        protected void btnRegistrar_Click(object sender, EventArgs e)
        {
            // Obtener los datos ingresados por el usuario
            string nombre = txtNombre.Text;
            string puesto = DropDownListPuesto.SelectedValue; // Guardar el puesto seleccionado
            DateTime fechaIngreso;
            bool fechaValida = DateTime.TryParse(TextFechaIngreso.Text, out fechaIngreso); // Validar la fecha de ingreso
            int idPDV = Convert.ToInt32(ddlPDV.SelectedValue); // Obtener el PDV seleccionado

            // Verificar que todos los campos estén completos y sean válidos
            if (string.IsNullOrEmpty(nombre) || puesto == "0" || !fechaValida || idPDV == 0)
            {
                MostrarMensaje("Por favor, complete todos los campos correctamente.", true);
                return;
            }

            // Obtener la cadena de conexión desde el archivo Web.config
            string connectionString = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;

            // Insertar los datos en la base de datos utilizando un procedimiento almacenado
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_InsertarEmpleado", con);
                cmd.CommandType = CommandType.StoredProcedure;

                // Pasar los parámetros al procedimiento almacenado
                cmd.Parameters.AddWithValue("@Nombre", nombre);
                cmd.Parameters.AddWithValue("@Puesto", puesto);
                cmd.Parameters.AddWithValue("@Fecha_Ingreso", fechaIngreso);
                cmd.Parameters.AddWithValue("@ID_PDV", idPDV);

                try
                {
                    // Abrir la conexión e insertar el empleado
                    con.Open();
                    int empleadoId = Convert.ToInt32(cmd.ExecuteScalar()); // Obtener el ID del empleado registrado
                    MostrarMensaje("Empleado registrado con éxito. ", false);
                }
                catch (Exception ex)
                {
                    // Mostrar mensaje de error en caso de que falle la inserción
                    MostrarMensaje("Error al registrar el empleado: " + ex.Message, true);
                }
            }
        }

        // Método para mostrar mensajes al usuario (ya sea de éxito o error)
        private void MostrarMensaje(string mensaje, bool esError)
        {
            string tipoAlerta = esError ? "error" : "success";
            string script = $"Swal.fire({{title: '{(esError ? "Error" : "Éxito")}', text: '{mensaje}', icon: '{tipoAlerta}', confirmButtonText: 'OK'}});";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", script, true);
        }

    }
}