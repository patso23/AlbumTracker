using Npgsql;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Reflection;
using AlbumTracker.API.Helpers;
using Repository;

namespace AlbumTracker
{
    public class AlbumRepository : IRepository<Album>
    {
        private readonly IConfiguration _configuration;
        private readonly NpgsqlConnection conn;
        private readonly QueryHelper helper;

        public AlbumRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            conn = new NpgsqlConnection(_configuration["connectionString"]);
            helper = new QueryHelper();
            conn.Open();
        }

        ~AlbumRepository()
        {
            conn.Close();
        }

        public Album Create(Album album)
        {
            string insertQuery = helper.CreateInsertStatement(album);
            var props = album.GetType().GetProperties();

            using (var cmd = new NpgsqlCommand(insertQuery, conn))
            {
                foreach (PropertyInfo prop in props)
                {
                    if (prop.Name.ToLower() != "id")
                    {
                        cmd.Parameters.AddWithValue(prop.Name, prop.GetValue(album));
                    }
                }

                var result = cmd.ExecuteScalar();
                var property = album.GetType().GetProperty("Id");
                property.SetValue(album, result, null);
            }

            return album;
        }

        public IEnumerable<Album> Get()
        {
            List<Album> albums = new List<Album>();

            using (var cmd = new NpgsqlCommand("SELECT * from Album", conn))
            {
                NpgsqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    var obj = new Album()
                    {
                        Id = rdr.GetInt32(0),
                        ArtistId = rdr.GetInt32(1),
                        Name = rdr.GetString(2),
                        Label = rdr.GetString(3),
                        Genre = rdr.GetString(4),
                        SongCount = rdr.GetInt32(5),
                        Date = rdr.GetDateTime(6)
                    };

                    albums.Add(obj);
                }

            }

            return albums;
        }

        public Album GetById(int id)
        {
            using (var cmd = new NpgsqlCommand("SELECT * from Album where id = @id", conn))
            {
                cmd.Parameters.AddWithValue("id", id);

                NpgsqlDataReader rdr = cmd.ExecuteReader();

                if (rdr.HasRows)
                {
                    rdr.Read();

                    Album album = new Album()
                    {
                        Id = rdr.GetInt32(0),
                        ArtistId = rdr.GetInt32(1),
                        Name = rdr.GetString(2),
                        Label = rdr.GetString(3),
                        Genre = rdr.GetString(4),
                        SongCount = rdr.GetInt32(5),
                        Date = rdr.GetDateTime(6)
                    };
                    
                    return album;
                }

                return new Album();

            }
        }

        public Album GetByName(string name)
        {
            using (var cmd = new NpgsqlCommand("SELECT * from Album where name ilike  @name", conn))
            {
                cmd.Parameters.AddWithValue("name", "%" + name + "%");

                NpgsqlDataReader rdr = cmd.ExecuteReader();

                if (rdr.HasRows)
                {
                    rdr.Read();

                    Album album = new Album()
                    {
                        Id = rdr.GetInt32(0),
                        ArtistId = rdr.GetInt32(1),
                        Name = rdr.GetString(2),
                        Label = rdr.GetString(3),
                        Genre = rdr.GetString(4),
                        SongCount = rdr.GetInt32(5),
                        Date = rdr.GetDateTime(6)
                    };
                    
                    return album;
                }

                return new Album();
            }
        }

        public Album Update(Album album, int id)
        {
            string updateQuery = helper.CreateUpdateStatement(album);
            var props = album.GetType().GetProperties();

            using (var cmd = new NpgsqlCommand(updateQuery.ToString(), conn))
            {
                foreach (PropertyInfo prop in props)
                {
                    if (prop.Name.ToLower() != "id")
                    {
                        cmd.Parameters.AddWithValue(prop.Name, prop.GetValue(album));
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("id", id);
                    }
                }

                var result = cmd.ExecuteScalar();
                var property = album.GetType().GetProperty("Id");
                property.SetValue(album, result, null);
            }

            return album;
        }

        public void DeleteById(int id)
        {
            using (var cmd = new NpgsqlCommand("DELETE from Album where id = @id", conn))
            {

                cmd.Parameters.AddWithValue("id", id);
                NpgsqlDataReader rdr = cmd.ExecuteReader();
            }

            return;
        }

    }
}
