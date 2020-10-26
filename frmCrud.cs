using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SQLServerCSharp
{
    public partial class frmCrud : Form
    {
        //-----------------------------------------------------------------------------------------

        // Propiedades

        private string cadenaConexionBD;
        private Clases.SQLServer objBaseSQLServer;
        private Clases.Clientes objClientes;
        private BindingSource objBindingSource;

        //-----------------------------------------------------------------------------------------

        // Constructor
        public frmCrud()
        {
            InitializeComponent();
        }

        //-----------------------------------------------------------------------------------------

        // Al cargar formulario
        private void frmCrud_Load(object sender, EventArgs e)
        {
            cadenaConexionBD = Properties.Settings.Default.ConnectionString;
        }

        //-----------------------------------------------------------------------------------------

        // Al mostrar formulario
        private void frmCrud_Shown(object sender, EventArgs e)
        {
            objBaseSQLServer = new Clases.SQLServer(cadenaConexionBD);

            if (objBaseSQLServer.hayError)
            {
                MessageBox.Show("No se pudo conectar a la base de datos:\n" + objBaseSQLServer.mensajeError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                bnPrincipal.Enabled = false;
            }
            else
            {
                cambiarEstadoEdicion(false);

                objClientes = new Clases.Clientes(objBaseSQLServer);
                objBindingSource = new BindingSource();

                obtenerDatos();
                asignarOrigenDatosEdicion();
            }
        }

        //-----------------------------------------------------------------------------------------

        // Obtener datos de tabla
        private void obtenerDatos()
        {
            objClientes.ObtenerDatos();

            objBindingSource.DataSource = objClientes.DSDatos;
            objBindingSource.DataMember = "CLIENTES";

            dgvDatos.DataSource = objBindingSource;
            bnPrincipal.BindingSource = objBindingSource;
        }

        //-----------------------------------------------------------------------------------------

        // Asignar origen de datos en controles de captura para un registro
        private void asignarOrigenDatosEdicion()
        {
            txtCodigo.DataBindings.Add("Text", objBindingSource, "CODIGO");
            txtRazonSocial.DataBindings.Add("Text", objBindingSource, "RAZONSOCIAL");
            txtRegistro.DataBindings.Add("Text", objBindingSource, "REGISTRO");
            txtDireccion.DataBindings.Add("Text", objBindingSource, "DIRECCION");
        }

        //-----------------------------------------------------------------------------------------

        // Clic en botón que recarga el conjunto de datos de la tabla
        private void toolStripRecargarDatos_Click(object sender, EventArgs e)
        {
            obtenerDatos();
        }

        //-----------------------------------------------------------------------------------------

        // Clic en botón de guardar cambios
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("¿Aplicar cambios?", "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                objBindingSource.EndEdit();
                objClientes.AplicarCambios();

                if (!objClientes.errorGuardar)
                {
                    MessageBox.Show("Los datos fueron guardados", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    cambiarEstadoEdicion(false);
                }
            }
        }

        //-----------------------------------------------------------------------------------------

        // Clic en cancelar: para revertir cambios de un registro
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            objClientes.RechazarCambios();
            objBindingSource.ResetCurrentItem();

            cambiarEstadoEdicion(false);
        }

        //-----------------------------------------------------------------------------------------

        // Cambiar estado de controles de edición
        private void cambiarEstadoEdicion(bool estado)
        {
            txtCodigo.ReadOnly = !estado;
            txtRazonSocial.ReadOnly = !estado;
            txtRegistro.ReadOnly = !estado;
            txtDireccion.ReadOnly = !estado;

            btnGuardar.Enabled = estado;
            btnCancelar.Enabled = estado;

            dgvDatos.Enabled = !estado;
            bnPrincipal.Enabled = !estado;
        }

        //-----------------------------------------------------------------------------------------

        // Clic en editar el registro actual
        private void toolStripEditar_Click(object sender, EventArgs e)
        {
            if (objBindingSource.Count > 0)
            {
                cambiarEstadoEdicion(true);
            }
        }

        //-----------------------------------------------------------------------------------------

        // Clic en eliminar registro actual
        private void bindingNavigatorDeleteItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("¿Confirma eliminar registro actual?", "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                objBindingSource.RemoveCurrent();
                objClientes.AplicarCambios();

                MessageBox.Show("El registro fue eliminado", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                obtenerDatos();
            }
        }

        //-----------------------------------------------------------------------------------------

        private void bindingNavigatorAddNewItem_Click(object sender, EventArgs e)
        {
            objClientes.CrearRegistro();
            bnPrincipal.MoveFirstItem.PerformClick();
            cambiarEstadoEdicion(true);
        }

        //-----------------------------------------------------------------------------------------
    }
}
