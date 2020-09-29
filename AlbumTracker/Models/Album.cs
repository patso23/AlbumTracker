using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class Album
{
    public int Id { get; set; }

    public int ArtistId { get; set; }
    public string Name { get; set; }

    public string Label { get; set; }
    public string Genre { get; set; }

    public int SongCount { get; set; }

    public DateTime Date { get; set; }


}
