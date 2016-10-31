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
#endregion

namespace ChinookSystem.BLL
{
    [DataObject]
    public class TrackController
    {
        //return many tracks
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<Track> ListTracks()
        {
            
            using (var context = new ChinookContext())
            {
                //return all records all attributes 
                return context.Tracks.ToList();
            }
        }
        //return one track based off the id
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public Track GetTrack(int trackid)
        {

            using (var context = new ChinookContext())
            {
                //return a record all attributes 
                return context.Tracks.Find(trackid);
            }
        }
        //add a new track
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
        public void AddTrack(Track trackinfo)
        {

            using (var context = new ChinookContext())
            {
                //any business rules

                //and data refinements
                //review of using immediate if (iif)
                //comoser can be a null string, we do not want to store an empt string
                trackinfo.Composer = string.IsNullOrEmpty(trackinfo.Composer) ? null : trackinfo.Composer;

                //add instance of trackinfo to the database
                context.Tracks.Add(trackinfo);

                //commit add transaction
                context.SaveChanges();
                
            }
        }
        //update a track
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
        public void UpdateTrack(Track trackinfo)
        {

            using (var context = new ChinookContext())
            {
                //any business rules

                //and data refinements
                //review of using immediate if (iif)
                //comoser can be a null string, we do not want to store an empt string
                trackinfo.Composer = string.IsNullOrEmpty(trackinfo.Composer) ? null : trackinfo.Composer;

                //update the existing instance of trackinfo on the database
                context.Entry(trackinfo).State = System.Data.Entity.EntityState.Modified;

                //commit update transaction
                context.SaveChanges();

            }
        }
        // delete a record, using an overloaded method technique 
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public void DeleteTrack(Track trackinfo)
        {
            DeleteTrack(trackinfo.TrackId);
        }
        public void DeleteTrack(int trackid)
        {
            using(var context = new ChinookContext())
            {
                //any business rules 

                //do the delete
                //1. find the existing record on the database 
                var existing = context.Tracks.Find(trackid);
                //2. delete the record from the database
                context.Tracks.Remove(existing);
                //3. commit delete transaction
                context.SaveChanges();
            }
        }
    }//eom
}//eon
