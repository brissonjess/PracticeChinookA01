using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Additonal Namespaces
using System.ComponentModel; //ODS
using ChinookSystem.Data.Entities;
using ChinookSystem.Data.POCOs;
using ChinookSystem.DAL;
using System.Transactions;
#endregion

namespace ChinookSystem.BLL
{
    [DataObject]
    public class PlaylistTrackController
    {
        public List<TracksForPlaylist> Get_PlaylistTracks(string playlistname, int customerid)
        {
            using (var context = new ChinookContext())
            {
                var results = from x in context.PlaylistTracks
                              where x.Playlist.Name.Equals(playlistname)
                              && x.Playlist.CustomerId == customerid
                              orderby x.TrackNumber
                              select new TracksForPlaylist
                              {
                                  TrackId = x.TrackId,
                                  TrackNumber = x.TrackNumber,
                                  Name = x.Track.Name,
                                  Title = x.Track.Album.Title,
                                  Milliseconds = x.Track.Milliseconds,
                                  UnitPrice = x.Track.UnitPrice,
                                  Purchased = true
                              };
                return results.ToList();
            }
        }
    }
}
