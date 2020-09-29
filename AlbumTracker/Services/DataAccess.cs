using Microsoft.AspNetCore.WebUtilities;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace AlbumTracker.Services
{
    internal class DataAccess
    {
        private readonly string _connectionString =
            "Server=127.0.0.1;Port=5432;Database=albumtracker;User Id=postgres;Password=Zero11235!";

        private NpgsqlConnection conn;

        public DataAccess()
        {
            conn = new NpgsqlConnection(_connectionString);
            conn.Open();
        }

        ~DataAccess()
        {
            conn.Close();
        }

        public T Create<T>(T output)
        {

            int i = 1;
            var t = typeof(T);
            var props = t.GetProperties();
            StringBuilder query = new StringBuilder().Append("INSERT INTO " + t.Name + " ");
            StringBuilder cols = new StringBuilder().Append("(");
            StringBuilder vals = new StringBuilder().Append("VALUES (");

            foreach (PropertyInfo prop in props)
            {
                if (!prop.Name.Equals("Id"))
                {
                    cols.Append(prop.Name);
                    vals.Append("@" + prop.Name.ToLower());

                    if (i < props.Count() - 1)
                    {
                        cols.Append(", ");
                        vals.Append(", ");
                        i++;
                    }
                    else
                    {
                        i++;
                    }
                }
            }

            cols.Append(")");
            vals.Append(")");

            query.Append(cols + " ").Append(vals + " ").Append("returning id");


            using (var cmd = new NpgsqlCommand(query.ToString(), conn))
            {
                foreach (PropertyInfo prop in props)
                {
                    if (prop.Name.ToLower() != "id")
                    {
                        cmd.Parameters.AddWithValue(prop.Name, prop.GetValue(output));
                    }
                }

                var result = cmd.ExecuteScalar();
                var property = output.GetType().GetProperty("Id");
                property.SetValue(output, result, null);
            }

            return output;
        }

        public List<T> GetAll<T>(List<T> output)
        {
            var t = typeof(T);

            using (var cmd = new NpgsqlCommand("SELECT * from " + t.ToString(), conn))
            {
                NpgsqlDataReader rdr = cmd.ExecuteReader();
                var props = t.GetProperties();

                while (rdr.Read())
                {
                    var obj = (T)Activator.CreateInstance(typeof(T));

                    for (int i = 0; i < rdr.FieldCount; i++)
                    {
                        string fieldName = rdr.GetName(i);
                        var prop = props.FirstOrDefault(x => x.Name.ToLower() == fieldName.ToLower());
                        prop.SetValue(obj, rdr[i]);

                    }

                    output.Add(obj);
                }
            }

            return output;
        }

        public T GetById<T>(T output, int id)
        {
            var t = typeof(T);

            using (var cmd = new NpgsqlCommand("SELECT * from " + t.ToString() + " where id = @id", conn))
            {

                cmd.Parameters.AddWithValue("id", id);
                NpgsqlDataReader rdr = cmd.ExecuteReader();
                var props = t.GetProperties();

                while (rdr.Read())
                {
                    var obj = (T)Activator.CreateInstance(typeof(T));

                    for (int i = 0; i < rdr.FieldCount; i++)
                    {
                        Console.WriteLine(1);
                        string fieldName = rdr.GetName(i);
                        var prop = props.FirstOrDefault(x => x.Name.ToLower() == fieldName.ToLower());

                        prop.SetValue(obj, rdr[i]);

                    }
                    output = obj;
                }
            }

            return output;
        }

        public T GetByName<T>(T output, string name)
        {
            var t = typeof(T);

            using (var cmd = new NpgsqlCommand("SELECT * from " + t.ToString() + " where name ilike @name", conn))
            {

                cmd.Parameters.AddWithValue("@name", "%" + name + "%");
                NpgsqlDataReader rdr = cmd.ExecuteReader();
                var props = t.GetProperties();

                while (rdr.Read())
                {
                    var obj = (T)Activator.CreateInstance(typeof(T));

                    for (int i = 0; i < rdr.FieldCount; i++)
                    {
                        Console.WriteLine(1);
                        string fieldName = rdr.GetName(i);
                        var prop = props.FirstOrDefault(x => x.Name.ToLower() == fieldName.ToLower());

                        prop.SetValue(obj, rdr[i]);

                    }
                    output = obj;
                }
            }

            return output;
        }

        public T Update<T>(int id, T output)
        {
            int i = 1;
            var t = typeof(T);
            var props = t.GetProperties();
            StringBuilder query = new StringBuilder().Append("UPDATE " + t.Name + " SET ");

            foreach (PropertyInfo prop in props)
            {
                if (!prop.Name.Equals("Id"))
                {
                    query.Append(prop.Name.ToLower() + " = @" + prop.Name.ToLower());

                    if (i < props.Count() - 1)
                    {
                        query.Append(", ");
                        i++;
                    }
                    else
                    {
                        i++;
                    }
                }
            }

            query.Append(" where id = @id returning id");

            using (var cmd = new NpgsqlCommand(query.ToString(), conn))
            {
                foreach (PropertyInfo prop in props)
                {
                    if (prop.Name.ToLower() != "id")
                    {
                        cmd.Parameters.AddWithValue(prop.Name, prop.GetValue(output));
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("id", id);
                    }
                }

                var result = cmd.ExecuteScalar();
                var property = output.GetType().GetProperty("Id");
                property.SetValue(output, result, null);
            }

            return output;
        }




        public T DeleteById<T>(T output, int id)
        {
            var t = typeof(T);

            using (var cmd = new NpgsqlCommand("DELETE from " + t.ToString() + " where id = @id", conn))
            {

                cmd.Parameters.AddWithValue("id", id);
                NpgsqlDataReader rdr = cmd.ExecuteReader();
            }

            return output;
        }

    }
}
