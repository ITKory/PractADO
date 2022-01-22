using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace ClassLibrary
{
    public class BD:IDisposable
    {
        private readonly MySqlConnection _db;
        private readonly MySqlCommand _command;
        public BD(string connectionString)
        {
            try
            {
            _db = new MySqlConnection(connectionString);
                _db.Open();
                _command = new MySqlCommand { Connection = _db };

            }
            catch
            {
                throw new Exception(connectionString);
            }
        }

        public List<Produc> GetAllProsucts()
        {
            List<Produc> products = new();
            var res = Query("product","id","name","cost","type_id","count");
            while (res.Read())
            {
                products.Add(new Produc {
                Id = res.GetInt32("id"),
                    Count = res.GetInt32("count"),
                    Name = res.GetString("name"),
                Cost = res.GetString("cost"),
                 TypeId = res.GetInt32("type_id"),
                });
            }

           
            return products;
        }


        public List<Provider> GetAllProviders()
        {
            List<Provider> prov = new();
            var res = Query("provider", "id", "name");
            while (res.Read())
            {
                prov.Add(new Provider
                {
                Id = res.GetInt32("id"),
                Name = res.GetString("name")
    ,
                });
            }


            return prov;
        }
        public List<Supply> GetAllSupplys()
        {
            List<Supply> s = new();
            var res = Query("supply", "id" ,"provider_id","date","dcount","product_id");
            while (res.Read())
            {
                s.Add(new Supply
                {
                    Id = res.GetInt32("id"),
                    ProviderId = res.GetInt32("provider_id"),
                    DCount = res.GetString("dcount"),
                    Date =   res.GetDateTime("date"),
                    ProductId = res.GetInt32("product_id"),

                });
            }
            return s;
        }
        public List<Typep> GetAllTypes()
        {
            List<Typep> t = new();
            var res = Query("type","id", "name");
            while (res.Read())
            {
                t.Add(new Typep
                {
                    Id = res.GetInt32("id"),
                    Name = res.GetString("name"),
                });
            }


            return t;
        }
        public int GrtMinFrom(string row, string tabName)
        {
            var s = -1;
            _command.CommandText =  $"SELECT MIN(`{row}`) FROM {tabName}";
            var res = _command.ExecuteReader();

          
                if (res.Read())
                    s = res.GetInt32(0);

            return s;
        }
        public int GrtMaxFrom(string row, string tabName)
        {
            var s = -1;
            _command.CommandText = $"SELECT MAX(`{row}`) FROM {tabName}";
            var res = _command.ExecuteReader();
            
            if (res.Read())
                s =  res.GetInt32(0);
           
            return s;
        }
        private MySqlDataReader Query(string table, params string[] values)

        {
            string str = "";
            for (int i = 0; i < values.Length; i += 1)
            {
                str += values[i];
                if (i != values.Length - 1)
                    str += ",";
            }

            var sql = $"SELECT {str} FROM {table}";
           
            _command.CommandText = sql;
            var res = _command.ExecuteReader();

            return res;

        }

       
        public List<string> Select(string TableName, string options = "1",params string[] values)
        {

            string str = "";
            for (int i = 0; i < values.Length; i += 1)
            {
                str += values[i];
                if (i != values.Length - 1)
                    str += ",";
            }

            string query = $"SELECT {str} from {TableName} where {options}";
            MySqlDataAdapter dataAdapter = new(query, _db);
            DataTable table = new();
            try
            {
            dataAdapter.Fill(table);
            }
            catch
            {
                return new List<string> { "select query must be valid" }; 
            }
            List<string> res = new(); 
            
            foreach ( DataRow row in table.Rows)
                res.Add(string.Join(",",TableToList(table, table.Rows.IndexOf(row)).ToArray()));

            return res;
        }



        private static List<string> TableToList(DataTable table,int index) 
        {
            if (index < table.Rows.Count)
            {
                List<string> result = new();
                DataRow row = table.Rows[index];

                int len = row.ItemArray.Length;
                for (int i = 0; i < len; i++)
                    result.Add(row.ItemArray[i].ToString());
                return result;
            }
            return null;
        }

 



        void IDisposable.Dispose() =>
            _db.Close();
         
    }
}
