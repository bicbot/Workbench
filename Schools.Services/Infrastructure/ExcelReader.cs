using System.Data.Odbc;

namespace Schools.Services.Infrastructure
{
    using System;
    using System.Data;
    using System.Data.OleDb;
    using System.Globalization;
    using System.IO;

    public sealed class ExcelReader : IDisposable
    {
        //private const string cConnectionStringPattern = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties=\"Excel 8.0;HDR={1};IMEX=1;\";";
        //    private const string cConnectionStringPattern = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties=\"Excel 8.0;HDR={1};IMEX=0;\";";
        //private const string cConnectionStringPattern = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties=\"Excel 8.0;HDR={1};\";";
        //private const string ConnectionStringPattern = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0;HDR={1};IMEX=1;\";";
        private const string ConnectionStringPattern = "Driver={Microsoft Excel Driver (*.xls, *.xlsx, *.xlsm, *.xlsb)};DBQ={0};";


        private OdbcConnection connection;
        private DataTable fileSchema;

        public ExcelReader(string filePath, bool useHeaderRow)
        {
            this.UseHeaderRow = useHeaderRow;
            this.Init(filePath);
            this.InitConnection(this.ConnectionString);
        }

        public ExcelReader(FileInfo excelFile, bool useHeaderRow)
        {
            this.UseHeaderRow = useHeaderRow;
            this.ExcelFile = excelFile;
            this.BuildConnectionString(this.ExcelFile);
            this.InitConnection(this.ConnectionString);
        }

        private string HeaderRowUsage => this.UseHeaderRow ? "Yes" : "No";

        public int SheetCount => this.fileSchema.Rows.Count;

        private bool UseHeaderRow { get; }

        #region IDisposable Members

        public void Dispose()
        {
            this.CloseConnection();
            this.connection.Dispose();
        }

        #endregion

        public DataTable GetDataTable(string sheetName)
        {
            try
            {
                var name = NormalizeName(sheetName);
                var t = new DataTable();
                t.TableName = sheetName;
                t.Locale = CultureInfo.InvariantCulture;
                this.connection.Open();

                var dbAdapter = new OdbcDataAdapter(string.Format("SELECT * FROM [{0}]", name), this.connection);
                dbAdapter.Fill(t);
                return t;
            }
            finally
            {
                this.CloseConnection();
            }
        }

        public DataTable GetDataTable(int sheetIndex)
        {
            var sheetName = this.GetNameByIndex(sheetIndex);
            return this.GetDataTable(sheetName);
        }

        public OdbcDataReader GetReader(int sheetIndex)
        {
            var sheetName = this.GetNameByIndex(sheetIndex);
            return this.GetReader(sheetName);
        }

        public OdbcDataReader GetReader(string sheetName)
        {
            OdbcCommand dbCommand = null;
            try
            {
                var name = NormalizeName(sheetName);
                this.connection.Open();
                dbCommand = new OdbcCommand(string.Format(CultureInfo.InvariantCulture,
                                                           "SELECT * FROM [{0}]", name), this.connection);
                return dbCommand.ExecuteReader();
            }
            finally
            {
                if (dbCommand != null)
                {
                    dbCommand.Dispose();
                }

                this.CloseConnection();
            }
        }

        internal static string NormalizeName(string sheetName)
        {
            if (!sheetName.EndsWith("$"))
            {
                return sheetName + "$";
            }

            return sheetName;
        }

        private void BuildConnectionString(FileSystemInfo excelFileInfo)
        {
            if (excelFileInfo != null)
            {
                this.ConnectionString = ConnectionStringPattern.Replace("{0}", excelFileInfo.FullName);
            }
            else
            {
                throw new ArgumentNullException("excelFileInfo");
            }
        }

        private void CloseConnection()
        {
            if (this.connection != null)
            {
                if (this.connection.State == ConnectionState.Open)
                {
                    this.connection.Close();
                }
            }
        }

        private string GetNameByIndex(int sheetIndex)
        {
            if (this.fileSchema.Rows.Count < sheetIndex)
            {
                throw new ArgumentOutOfRangeException("sheetIndex");
            }

            return this.fileSchema.Rows[sheetIndex]["TABLE_NAME"].ToString().Trim('\'');
        }

        public DataTable GetSchema()
        {
            try
            {
                this.connection.Open();
                var t = this.connection.GetSchema("Tables");
                if (t == null || t.Rows.Count < 1)
                {
                    throw new Exception("Error: Could not retrieve file schema.");
                }

                return t;
            }
            finally
            {
                this.CloseConnection();
            }
        }

        private void Init(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("Excel File not found", filePath);
            }

            this.ExcelFile = new FileInfo(filePath);
            this.BuildConnectionString(this.ExcelFile);
        }

        private void InitConnection(string connectionString)
        {
            this.connection = new OdbcConnection(connectionString);
            //this.fileSchema = this.GetSchema();
        }

        #region Properties

        public FileInfo ExcelFile { get; set; }

        public int RowIndex { get; set; }

        public int ColumnIndex { get; set; }

        public int SheetIndex { get; set; }

        public string ConnectionString { get; private set; }

        #endregion
    }
}
