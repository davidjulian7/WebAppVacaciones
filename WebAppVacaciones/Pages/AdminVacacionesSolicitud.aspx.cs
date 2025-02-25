using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebAppVacaciones.Pages
{
    public partial class AdminVacacionesSolicitud : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarDatos();
            }
        }


        private void CargarDatos(string filtro = "", string estadoFiltro = "Pendiente")
        {
            string connectionString = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("ObtenerSolicitudesVacaciones", con))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Filtro", filtro);
                    command.Parameters.AddWithValue("@EstadoFiltro", string.IsNullOrEmpty(estadoFiltro) ? (object)DBNull.Value : estadoFiltro);

                    con.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader);

                    gridDetallesEmpleado.DataSource = dt;
                    gridDetallesEmpleado.DataBind();
                }
            }
        }


        protected void ddlEstadoFiltro_SelectedIndexChanged(object sender, EventArgs e)
        {
            string filtro = txtSearch.Text.Trim(); // Obtener filtro de búsqueda por nombre
            string estadoFiltro = ddlEstadoFiltro.SelectedValue; // Obtener el estado seleccionado
            CargarDatos(filtro, estadoFiltro);
        }



        protected void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string filtro = txtSearch.Text.Trim();
            CargarDatos(filtro);
        }

        protected void gridDetallesEmpleado_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Autorizar" || e.CommandName == "Denegar")
            {
                int idSolicitud = Convert.ToInt32(e.CommandArgument);
                string estado = e.CommandName == "Autorizar" ? "Aprobada" : "Rechazada";
                string comentario = estado == "Aprobada" ? "Solicitud aprobada." : "Solicitud denegada.";

                string connectionString = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_GestionarSolicitudVacaciones", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ID_Solicitud", idSolicitud);
                        cmd.Parameters.AddWithValue("@Estado", estado);
                        cmd.Parameters.AddWithValue("@Comentarios", comentario);

                        try
                        {
                            con.Open();
                            cmd.ExecuteNonQuery();

                            // Mostrar mensaje de éxito con SweetAlert
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alerta",
                                $"Swal.fire('{estado}', 'La solicitud ha sido {estado.ToLower()} correctamente.', 'success');", true);

                            // Recargar datos después de la actualización
                            CargarDatos();
                        }
                        catch (Exception ex)
                        {
                            // Mostrar error si algo falla
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "error",
                                $"Swal.fire('Error', 'Hubo un problema al procesar la solicitud: {ex.Message}', 'error');", true);
                        }
                    }
                }
            }
        }








    }
}