using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AlbumTracker.API.Helpers
{
    public class QueryHelper : ControllerBase
    {

        public string CreateInsertStatement<T>(T item)
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

            return query.ToString();
        }


        public string CreateUpdateStatement<T>(T item)
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

            return query.ToString();
        }

       
    }
}

    