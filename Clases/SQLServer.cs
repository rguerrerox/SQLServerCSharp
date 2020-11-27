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
    class SQLServer
    {
        //-----------------------------------------------------------------------------------------

        // Propiedades

        private string cadenaConexion;

        private SqlConnection _cnDB;
        public SqlConnection cnDB { get { return _cnDB; } }

        private bool _hayError;
        public bool hayError { get { return _hayError; } }

        private string _mensajeError;
        public string mensajeError { get { return _mensajeError; } }

        //-----------------------------------------------------------------------------------------

        // Constructor
        public SQLServer(string cadenaConexion)
        {
            this.cadenaConexion = cadenaConexion;
            _cnDB = new SqlConnection(cadenaConexion);

            probarConexion();
        }

        //-----------------------------------------------------------------------------------------

        // Abrir y cerrar base para probar conectividad
        private void probarConexion()
        {
            try
            {
                cnDB.Open();
                cnDB.Close();

                _hayError = false;
                _mensajeError = string.Empty;
            }
            catch (Exception e)
            {
                _hayError = true;
                _mensajeError = e.Message;
            }
        }

        //-----------------------------------------------------------------------------------------

        // Llenar combobox desde tabla (valor, texto a mostrar)
        public void llenarCombo(ComboBox cbx, string tabla, string campoValor, string campoTexto, string orden)
        {
            SqlDataAdapter da = new SqlDataAdapter($"SELECT {campoValor}, {campoTexto} FROM {tabla} ORDER BY {orden}", _cnDB);
            DataTable dt = new DataTable();
            da.Fill(dt);

            cbx.DataSource = dt;
            cbx.DisplayMember = campoTexto;
            cbx.ValueMember = campoValor;
        }

        //-----------------------------------------------------------------------------------------
    }
}
