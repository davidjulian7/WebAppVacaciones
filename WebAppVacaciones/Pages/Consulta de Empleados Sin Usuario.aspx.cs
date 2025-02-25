using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebAppVacaciones.Pages
{
    public partial class Consulta_Empleado_Sin_Usuario : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarDatos();
                CargarPDV();
                CargarPuestos(); // Cargar el DropDownList de Puestos desde la base de datos
            }
        }

        private void CargarDatos(string filtro = "")
        {
            string connectionString = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_EmpleadosSinUsuario", con))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Filtro", filtro);

                    con.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader);

                    gridDetallesEmpleado.DataSource = dt;
                    gridDetallesEmpleado.DataBind();
                }
            }
        }

        protected void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string filtro = txtSearch.Text.Trim();
            CargarDatos(filtro);
        }

        protected void gridDetallesEmpleado_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Actualizar")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = gridDetallesEmpleado.Rows[rowIndex];

                // Obtén el ID del empleado y guárdalo en el HiddenField
                hfEmpleadoID.Value = row.Cells[0].Text; // ID_Empleado

                // Asigna valores a los controles del modal
                txtNombre.Text = row.Cells[1].Text;
                DropDownListPuesto.SelectedValue = ObtenerPuesto(row.Cells[2].Text);
                txtFechaIngreso.Text = DateTime.Parse(row.Cells[3].Text).ToString("yyyy-MM-dd");
                ddlPDV.SelectedValue = ObtenerIdPDV(row.Cells[4].Text);

                ScriptManager.RegisterStartupScript(this, GetType(), "abrirModal", "abrirModal();", true);

            }
            else if (e.CommandName == "Eliminar")
            {
                int idEmpleado = Convert.ToInt32(e.CommandArgument);
                EliminarUsuario(idEmpleado);
            }
        }

        private string ObtenerIdPDV(string nombrePDV)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT ID_PDV FROM PDV WHERE Nombre_PDV = @NombrePDV", con))
                {
                    cmd.Parameters.AddWithValue("@NombrePDV", nombrePDV);
                    con.Open();
                    object result = cmd.ExecuteScalar();
                    return result?.ToString() ?? "0";
                }
            }
        }


        private string ObtenerPuesto(string nombrePuesto)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT Id_Puesto FROM Puesto WHERE Puesto = @nombrePuesto", con))
                {
                    cmd.Parameters.AddWithValue("@nombrePuesto", nombrePuesto);
                    con.Open();
                    object result = cmd.ExecuteScalar();
                    return result?.ToString() ?? "0";
                }
            }
        }

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
                    DropDownListPuesto.DataValueField = "Id_Puesto"; // Guardar el nombre del puesto como valor
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

        private void CargarPDV()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_consultar_pdv", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    ddlPDV.DataSource = reader;
                    ddlPDV.DataTextField = "Nombre_PDV";
                    ddlPDV.DataValueField = "ID_PDV";
                    ddlPDV.DataBind();
                }
            }

            ddlPDV.Items.Insert(0, new ListItem("Seleccione un PDV", "0"));
        }

        private void EliminarUsuario(int idEmpleado)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_EliminarEmpleado", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID_Empleado", idEmpleado);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            MostrarMensaje("Empleado eliminado correctamente.", false);
            CargarDatos();
        }


        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                // Validar que se haya seleccionado un Puesto y un PDV
                if (DropDownListPuesto.SelectedValue == "0")
                {
                    MostrarMensaje("Debe seleccionar un puesto válido.", true);
                    return;
                }

                if (ddlPDV.SelectedValue == "0")
                {
                    MostrarMensaje("Debe seleccionar un PDV válido.", true);
                    return;
                }

                // Obtener la conexión desde el archivo Web.config
                string connectionString = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_ModificarEmpleado", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Parámetros obtenidos desde los controles del modal
                        cmd.Parameters.AddWithValue("@ID_Empleado", Convert.ToInt32(hfEmpleadoID.Value)); // HiddenField para ID del empleado
                        cmd.Parameters.AddWithValue("@Nombre", txtNombre.Text.Trim());
                        cmd.Parameters.AddWithValue("@Puesto", DropDownListPuesto.SelectedItem.Text);
                        cmd.Parameters.AddWithValue("@Fecha_Ingreso", Convert.ToDateTime(txtFechaIngreso.Text.Trim()));
                        cmd.Parameters.AddWithValue("@ID_PDV", Convert.ToInt32(ddlPDV.SelectedValue));

                        con.Open();
                        cmd.ExecuteNonQuery();

                        MostrarMensaje("Empleado actualizado exitosamente.", false);
                    }
                }

                // Recargar los datos de la tabla después de guardar
                CargarDatos();
            }
            catch (Exception ex)
            {
                MostrarMensaje("Error al actualizar empleado: " + ex.Message, true);
            }
        }




        private void MostrarMensaje(string mensaje, bool esError)
        {
            string tipoAlerta = esError ? "error" : "success";
            string script = $"Swal.fire({{ title: '{mensaje}', icon: '{tipoAlerta}' }});";
            ScriptManager.RegisterStartupScript(this, GetType(), "alert", script, true);
        }
    }
}
