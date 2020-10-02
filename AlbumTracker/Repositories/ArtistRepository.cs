using Npgsql;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Reflection;
using AlbumTracker.API.Helpers;
using Repository;

namespace AlbumTracker
{
    public class ArtistRepository : IRepository<Artist>
    {
        private readonly IConfiguration _configuration;
        private readonly NpgsqlConnection conn;
        private readonly QueryHelper helper;

        public ArtistRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            conn = new NpgsqlConnection(_configuration["connectionString"]);
            helper = new QueryHelper();
            conn.Open();
        }

        ~ArtistRepository()
        {
            conn.Close();
        }

        public Artist Create(Artist artist)
        {
            string insertQuery = helper.CreateInsertStatement(artist);
            var props = artist.GetType().GetProperties();

            using (var cmd = new NpgsqlCommand(insertQuery, conn))
            {
                foreach (PropertyInfo prop in props)
                {
                    if (prop.Name.ToLower() != "id")
                    {
                        cmd.Parameters.AddWithValue(prop.Name, prop.GetValue(artist));
                    }
                }

                var result = cmd.ExecuteScalar();
                var property = artist.GetType().GetProperty("Id");
                property.SetValue(artist, result, null);
            }

            return artist;
        }

        public IEnumerable<Artist> Get()
        {
            List<Artist> artists = new List<Artist>();

            using (var cmd = new NpgsqlCommand("SELECT * from Artist", conn))
            {
                NpgsqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    var obj = new Artist()
                    {
                        Id = rdr.GetInt32(0),
                        Name = rdr.GetString(1),
                    };

                    artists.Add(obj);
                }

            }

            return artists;
        }

        public Artist GetById(int id)
        {
            using (var cmd = new NpgsqlCommand("SELECT * from Artist where id = @id", conn))
            {
                cmd.Parameters.AddWithValue("id", id);

                NpgsqlDataReader rdr = cmd.ExecuteReader();

                if (rdr.HasRows)
                {
                    rdr.Read();

                    Artist artist = new Artist()
                    {
                        Id = rdr.GetInt32(0),
                        Name = rdr.GetString(1)
                    };
                    
                    return artist;
                }

                return new Artist();
            }
        }

        public Artist GetByName(string name)
        {
            using (var cmd = new NpgsqlCommand("SELECT * from Artist where name ilike  @name", conn))
            {
                cmd.Parameters.AddWithValue("name", "%" + name + "%");

                NpgsqlDataReader rdr = cmd.ExecuteReader();

                if (rdr.HasRows)
                {
                    rdr.Read();

                    Artist artist = new Artist()
                    {
                        Id = rdr.GetInt32(0),
                        Name = rdr.GetString(1)
                    };
                    
                    return artist;
                }

                return new Artist();
            }
        }

        public Artist Update(Artist artist, int id)
        {
            string updateQuery = helper.CreateUpdateStatement(artist);
            var props = artist.GetType().GetProperties();

            using (var cmd = new NpgsqlCommand(updateQuery.ToString(), conn))
            {
                foreach (PropertyInfo prop in props)
                {
                    if (prop.Name.ToLower() != "id")
                    {
                        cmd.Parameters.AddWithValue(prop.Name, prop.GetValue(artist));
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("id", id);
                    }
                }

                var result = cmd.ExecuteScalar();
                var property = artist.GetType().GetProperty("Id");
                property.SetValue(artist, result, null);
            }

            return artist;
        }

        public void DeleteById(int id)
        {
            using (var cmd = new NpgsqlCommand("DELETE from Artist where id = @id", conn))
            {

                cmd.Parameters.AddWithValue("id", id);
                NpgsqlDataReader rdr = cmd.ExecuteReader();
            }

            return;
        }

    }
}
