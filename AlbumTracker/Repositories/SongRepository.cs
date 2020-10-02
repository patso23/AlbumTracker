using Npgsql;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Reflection;
using AlbumTracker.API.Helpers;
using Repository;

namespace AlbumTracker
{
    public class SongRepository : IRepository<Song>
    {
        private readonly IConfiguration _configuration;
        private readonly NpgsqlConnection conn;
        private readonly QueryHelper helper;

        public SongRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            conn = new NpgsqlConnection(_configuration["connectionString"]);
            helper = new QueryHelper();
            conn.Open();
        }

        ~SongRepository()
        {
            conn.Close();
        }

        public Song Create(Song song)
        {
            string insertQuery = helper.CreateInsertStatement(song);
            var props = song.GetType().GetProperties();

            using (var cmd = new NpgsqlCommand(insertQuery, conn))
            {
                foreach (PropertyInfo prop in props)
                {
                    if (prop.Name.ToLower() != "id")
                    {
                        cmd.Parameters.AddWithValue(prop.Name, prop.GetValue(song));
                    }
                }

                var result = cmd.ExecuteScalar();
                var property = song.GetType().GetProperty("Id");
                property.SetValue(song, result, null);
            }

            return song;
        }

        public IEnumerable<Song> Get()
        {
            List<Song> songs = new List<Song>();

            using (var cmd = new NpgsqlCommand("SELECT * from Song", conn))
            {
                NpgsqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    var obj = new Song()
                    {
                        Id = rdr.GetInt32(0),
                        Name = rdr.GetString(1),
                        TrackNumber = rdr.GetInt32(2),
                        AlbumId = rdr.GetInt32(3)
                    };

                    songs.Add(obj);
                }

            }

            return songs;
        }

        public Song GetById(int id)
        {
            using (var cmd = new NpgsqlCommand("SELECT * from Song where id = @id", conn))
            {
                cmd.Parameters.AddWithValue("id", id);

                NpgsqlDataReader rdr = cmd.ExecuteReader();

                if (rdr.HasRows)
                {
                    rdr.Read();

                    Song song = new Song()
                    {
                        Id = rdr.GetInt32(0),
                        Name = rdr.GetString(1),
                        TrackNumber = rdr.GetInt32(2),
                        AlbumId = rdr.GetInt32(3)
                    };

                    return song;
                }

                return new Song();
            }
        }

        public Song GetByName(string name)
        {
            using (var cmd = new NpgsqlCommand("SELECT * from Song where name ilike  @name", conn))
            {
                cmd.Parameters.AddWithValue("name", "%" + name + "%");

                NpgsqlDataReader rdr = cmd.ExecuteReader();

                if (rdr.HasRows)
                {
                    rdr.Read();

                    Song song = new Song()
                    {
                        Id = rdr.GetInt32(0),
                        Name = rdr.GetString(1),
                        TrackNumber = rdr.GetInt32(2),
                        AlbumId = rdr.GetInt32(3)
                    };

                    return song;
                }

                return new Song();
            }
        }

        public Song Update(Song song, int id)
        {
            string updateQuery = helper.CreateUpdateStatement(song);
            var props = song.GetType().GetProperties();

            using (var cmd = new NpgsqlCommand(updateQuery.ToString(), conn))
            {
                foreach (PropertyInfo prop in props)
                {
                    if (prop.Name.ToLower() != "id")
                    {
                        cmd.Parameters.AddWithValue(prop.Name, prop.GetValue(song));
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("id", id);
                    }
                }

                var result = cmd.ExecuteScalar();
                var property = song.GetType().GetProperty("Id");
                property.SetValue(song, result, null);
            }

            return song;
        }

        public void DeleteById(int id)
        {
            using (var cmd = new NpgsqlCommand("DELETE from Song where id = @id", conn))
            {

                cmd.Parameters.AddWithValue("id", id);
                NpgsqlDataReader rdr = cmd.ExecuteReader();
            }

            return;
        }

    }
}
