using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SQLServerCSharp.Clases
{
    class Clientes
    {
        //-----------------------------------------------------------------------------------------

        // Propiedades

        private Clases.SQLServer conexionBD;

        private DataSet objDataSet;
        public DataSet DSDatos { get { return objDataSet; } }

        private SqlDataAdapter objDataAdapter;
        private SqlCommandBuilder objCommandBuilder;

        private bool _errorGuardar;
        public bool errorGuardar { get { return _errorGuardar; } }

        //-----------------------------------------------------------------------------------------

        // Constructor
        public Clientes(Clases.SQLServer conexionBD)
        {
            this.conexionBD = conexionBD;
            objDataSet = new DataSet();
            _errorGuardar = false;
        }

        //-----------------------------------------------------------------------------------------

        // Obtener datos
        public void ObtenerDatos()
        {
            string sentenciaSQL;

            sentenciaSQL = @"
            SELECT
                *
            FROM
                CLIENTES;
            ";

            objDataAdapter = new SqlDataAdapter(sentenciaSQL, conexionBD.cnDB);
            objDataSet.Clear();
            objDataAdapter.Fill(objDataSet, "CLIENTES");

            objCommandBuilder = new SqlCommandBuilder(objDataAdapter);
            objDataAdapter.InsertCommand = objCommandBuilder.GetInsertCommand();
            objDataAdapter.UpdateCommand = objCommandBuilder.GetUpdateCommand();
            objDataAdapter.DeleteCommand = objCommandBuilder.GetDeleteCommand();
            objDataAdapter.RowUpdated += new SqlRowUpdatedEventHandler(OnFilaActualizada);
        }

        //-----------------------------------------------------------------------------------------

        // Comprobar si hay problema al actualizar una fila con: insert, update, delete
        private void OnFilaActualizada(object sender, SqlRowUpdatedEventArgs args)
        {
            if (args.Status == UpdateStatus.ErrorsOccurred)
            {
                args.Row.RowError = args.Errors.Message;
                args.Status = UpdateStatus.SkipCurrentRow;

                MessageBox.Show(args.Errors.Message.ToString());
                _errorGuardar = true;
            }
        }

        //-----------------------------------------------------------------------------------------

        // Aplicar cambios realizados en dataset
        public void AplicarCambios()
        {
            _errorGuardar = false;

            objDataAdapter.Update(objDataSet, "CLIENTES");
        }

        //-----------------------------------------------------------------------------------------

        // Rechazar cambios
        public void RechazarCambios()
        {
            DSDatos.RejectChanges();
        }

        //-----------------------------------------------------------------------------------------

        // Crear nuevo registro vacío
        public void CrearRegistro()
        {
            DataRow nuevaFila = DSDatos.Tables["CLIENTES"].NewRow();
            DSDatos.Tables["CLIENTES"].Rows.InsertAt(nuevaFila, 0);
        }

        //-----------------------------------------------------------------------------------------

        // Llenar combo con tipos de cliente
        public void LlenarComboTiposCliente(ComboBox cbx)
        {
            conexionBD.llenarCombo(cbx, "TIPOSCLIENTE", "TIPOCLIENTEID", "NOMBRE", "NOMBRE ASC");
        }

        //-----------------------------------------------------------------------------------------
    }
}
