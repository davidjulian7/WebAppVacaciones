using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebAppVacaciones.Pages
{
    public partial class Consulta_de_Empleados : System.Web.UI.Page
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
                using (SqlCommand command = new SqlCommand("sp_datos", con))
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
            int userId = Convert.ToInt32(e.CommandArgument);  // Este es el ID del empleado, no el índice.

            if (e.CommandName == "Agregar")
            {
                // Obtener la fila utilizando el ID de empleado (no el índice)
                GridViewRow row = GetRowByUserId(userId);
                if (row != null)
                {
                    // Obtener el ID del empleado desde la celda 0 y guardarlo en un HiddenField
                    hfEmpleadoID.Value = row.Cells[0].Text;

                    // Abrir el modal después de seleccionar el empleado
                    ScriptManager.RegisterStartupScript(this, GetType(), "AbrirModal", "abrirModalVadAdd();", true);
                }
            }
            else if (e.CommandName == "Consultar")
            {
                CargarVacaciones(userId);
            }
            else if (e.CommandName == "Actualizar")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = gridDetallesEmpleado.Rows[rowIndex];

                // Obtén el ID del empleado y guárdalo en el HiddenField
                hfEmpleadoID.Value = row.Cells[0].Text; // ID_Empleado

                // Asigna valores a los controles del modal
                txtNombre.Text = row.Cells[6].Text;
                DropDownListPuesto.SelectedValue = ObtenerPuesto(row.Cells[7].Text);
                txtFechaIngreso.Text = DateTime.Parse(row.Cells[8].Text).ToString("yyyy-MM-dd");
                ddlPDV.SelectedValue = ObtenerIdPDV(row.Cells[5].Text);

                ScriptManager.RegisterStartupScript(this, GetType(), "abrirModalMod", "abrirModalMod();", true);
            }
            else if (e.CommandName == "Eliminar")
            {
                EliminarUsuario(userId);
            }
        }

        // Método auxiliar para obtener la fila basada en el ID del empleado
        private GridViewRow GetRowByUserId(int userId)
        {
            foreach (GridViewRow row in gridDetallesEmpleado.Rows)
            {
                // Suponiendo que el ID del empleado está en la primera celda
                if (Convert.ToInt32(row.Cells[0].Text) == userId)
                {
                    return row;
                }
            }
            return null;  // Si no se encuentra la fila con el ID del empleado
        }



        protected void btnGuardarAgregado_Click(object sender, EventArgs e)
        {
            // Obtener el ID del empleado desde el HiddenField
            int empleadoId;
            if (!int.TryParse(hfEmpleadoID.Value, out empleadoId))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alerta", "Swal.fire('Error', 'No se pudo obtener el ID del empleado.', 'error');", true);
                return;
            }

            // Obtener la fecha seleccionada
            DateTime fechaSolicitud;
            if (!DateTime.TryParse(TextBox2.Text, out fechaSolicitud))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alerta", "Swal.fire('Error', 'Fecha inválida.', 'error');", true);
                return;
            }

            // Obtener el tipo de día seleccionado
            string medioDia = DropDownListDia.SelectedValue;
            if (string.IsNullOrEmpty(medioDia))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alerta", "Swal.fire('Error', 'Debe seleccionar el tipo de día.', 'error');", true);
                return;
            }

            // Llamar al procedimiento almacenado
            EjecutarProcedimientoAlmacenado(empleadoId, fechaSolicitud, medioDia);
            ResetControls();
        }


        private void EjecutarProcedimientoAlmacenado(int empleadoId, DateTime fecha, string medioDia)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;

            if (medioDia != "N" && medioDia != "M" && medioDia != "T")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alerta", "Swal.fire('El valor de MedioDia debe ser N, M o T.', '', 'error');", true);
                return;
            }

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand("AgregarDiaVacaciones", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@ID_Empleado", empleadoId);
                        cmd.Parameters.AddWithValue("@Fecha", fecha);
                        cmd.Parameters.AddWithValue("@MedioDia", medioDia);

                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();

                        ScriptManager.RegisterStartupScript(this, GetType(), "alerta", "Swal.fire('Solicitud registrada con éxito', '', 'success');", true);
                    }
                }
                catch (SqlException ex)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alerta", $"Swal.fire('Error SQL', '{ex.Message}', 'error');", true);
                }
                catch (Exception ex)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alerta", $"Swal.fire('Error inesperado', '{ex.Message}', 'error');", true);
                }
            }

            // Recargar los datos de la tabla después de guardar
            CargarDatos();
        }


        private void ResetControls()
        {
            // Resetear el formulario del modal
            DropDownListDia.SelectedIndex = 0;  // Restablece el DropDownList
            TextBox2.Text = "";  // Restablece el TextBox de la fecha

            // Cerrar el modal
            ScriptManager.RegisterStartupScript(this, GetType(), "CerrarModal", "cerrarModalVadAdd();", true);
        }



        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
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



        private void EliminarUsuario(int idEmpleado)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("EliminarEmpleadoConReferencias", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID_Empleado", idEmpleado);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            ScriptManager.RegisterStartupScript(this, GetType(), "alerta", "Swal.fire('Registro anulado con éxito', '', 'success');", true);
            CargarDatos();
        }

        private void CargarVacaciones(int userId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("ConsultarDiasVacaciones", con))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ID_Empleado", userId);

                    con.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader);

                    // Verificar si la tabla tiene registros
                    if (dt.Rows.Count > 0)
                    {
                        // Mostrar los datos de vacaciones en el grid
                        gridVacaciones.DataSource = dt;
                        gridVacaciones.DataBind();

                        // Abrir el modal para mostrar los registros de vacaciones
                        ScriptManager.RegisterStartupScript(this, GetType(), "abrirModal", "abrirModal();", true);
                    }
                    else
                    {
                        // Si no hay registros, mostrar alerta con SweetAlert
                        ScriptManager.RegisterStartupScript(this, GetType(), "alerta", "Swal.fire('Sin registro de vacaciones', '', 'warning');", true);
                    }
                }
            }
        }



        protected void gridVacaciones_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Anular")
            {
                // Extraer los valores concatenados en el CommandArgument
                string[] argumentos = e.CommandArgument.ToString().Split(',');
                int idEmpleado = Convert.ToInt32(argumentos[0]); // ID_Empleado
                DateTime fecha = Convert.ToDateTime(argumentos[1]); // Fecha
                string medioDia = argumentos[2]; // MedioDia

                // Llamar al método que anula la vacación
                AnularVacacion(idEmpleado, fecha, medioDia);
            }
        }


        private void AnularVacacion(int idEmpleado, DateTime fecha, string medioDia)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand("AnularDiaVacacionesPorFecha", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Agregar parámetros
                        cmd.Parameters.AddWithValue("@ID_Empleado", idEmpleado);
                        cmd.Parameters.AddWithValue("@Fecha", fecha);
                        cmd.Parameters.AddWithValue("@MedioDia", medioDia);

                        // Abrir conexión
                        con.Open();

                        // Ejecutar el procedimiento almacenado
                        cmd.ExecuteNonQuery();

                        // Mostrar mensaje de éxito
                        ScriptManager.RegisterStartupScript(this, GetType(), "alerta", "Swal.fire('Registro anulado con éxito', '', 'success');", true);
                    }
                }
                catch (SqlException ex)
                {
                    // Capturar el mensaje de error generado por RAISERROR en el procedimiento almacenado
                    ScriptManager.RegisterStartupScript(this, GetType(), "alerta", $"Swal.fire('Error', '{ex.Message}', 'error');", true);
                }
            }
            // Recargar los datos de la tabla después de guardar
            CargarDatos();
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


    }
}