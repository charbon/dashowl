using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using DashOwl.DAL;
using DashOwl.Models;
using System.Web.Http.ModelBinding;

namespace DashOwl.WebAPI
{
    /// <summary>
    /// MediaAsset entity API controller
    /// </summary>
    public class MediaAssetsController : ApiController
    {
        private DashOwlContext db = new DashOwlContext();

        // GET: api/MediaAssets
        /// <summary>
        /// Gets a list of Vehicles related to Incident.
        /// </summary>
        /// <param name="id">The Incident ID</param>
        /// <returns>A list of MediaAssets</returns>
        [Route("api/incidents/{id}/mediaassets")]
        public async Task<IHttpActionResult> GetMediaAssets(int id)
        {
            var media = await db.MediaAssets.Select(p => new MediaAssetDetailsDto
            {
                ID = p.ID,
                ServerURL = p.ServerURL,
                ExternalURL = p.ExternalURL,
                IncidentID = p.IncidentID,
                CreationDate = p.CreationDate
            })
                .Where(i => i.IncidentID == id).ToListAsync();                

            if (media == null)
            {
                return NotFound();
            }

            return Ok(media);
        }

        /// <summary>
        /// Gets MediaAsset by ID.
        /// </summary>
        /// <param name="id">The ID of MediaAsset</param>
        /// <returns>MediaAsset</returns>
        [ResponseType(typeof(MediaAssetDetailsDto))]
        public async Task<IHttpActionResult> GetMediaAsset(int id)
        {
            MediaAssetDetailsDto media = await db.MediaAssets.Select(p => new MediaAssetDetailsDto
            {
                ID = p.ID,
                ServerURL = p.ServerURL,
                ExternalURL = p.ExternalURL,
                IncidentID = p.IncidentID,
                CreationDate = p.CreationDate
            })
                .Where(i => i.ID == id)
                .FirstOrDefaultAsync();

            if (media == null)
            {
                return NotFound();
            }

            return Ok(media);
        }

        /// <summary>
        /// Creates MediaAsset
        /// </summary>
        /// <param name="id">The MediaAsset ID</param>
        /// <param name="mediaAsset">Modified MediaAsset</param>
        /// <returns>Nothing, if operation was successful</returns>
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutMediaAsset(int id, MediaAsset mediaAsset)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != mediaAsset.ID)
            {
                return BadRequest();
            }

            db.Entry(mediaAsset).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MediaAssetExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Creates MediaAsset.
        /// </summary>
        /// <param name="mediaAssetDetailsDto">A new MediaAsset</param>
        /// <returns>Created MediaAsset</returns>
        ///{"ServerURL":"/MediaStorage/7.jpg","ExternalURL":"","IncidentID":"1","CreationDate":"2014-12-31T00:00:00"}
        [ResponseType(typeof(MediaAssetDetailsDto))]
        public async Task<IHttpActionResult> PostMediaAsset(MediaAssetDetailsDto mediaAssetDetailsDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var mediaAsset = new MediaAsset()
            {
                CreationDate = mediaAssetDetailsDto.CreationDate,
                ExternalURL = mediaAssetDetailsDto.ExternalURL,
                IncidentID = mediaAssetDetailsDto.IncidentID,
                ServerURL = mediaAssetDetailsDto.ServerURL
            };

            db.MediaAssets.Add(mediaAsset);
            await db.SaveChangesAsync();

            mediaAssetDetailsDto.ID = mediaAsset.ID;

            return CreatedAtRoute("DefaultApi", new { id = mediaAsset.ID }, mediaAssetDetailsDto);
        }

        /// <summary>
        /// Deletes MediaAsset
        /// </summary>
        /// <param name="id">The MediaAsset ID</param>
        /// <returns>Ok, if deleted successfully</returns>
        [ResponseType(typeof(MediaAssetDetailsDto))]
        public async Task<IHttpActionResult> DeleteMediaAsset(int id)
        {
            MediaAsset mediaAsset = await db.MediaAssets.FindAsync(id);
            if (mediaAsset == null)
            {
                return NotFound();
            }

            db.MediaAssets.Remove(mediaAsset);
            await db.SaveChangesAsync();

            var mediaAssetDto = new MediaAssetDetailsDto
            {
                ID = mediaAsset.ID,
                IncidentID = mediaAsset.IncidentID,
                ServerURL = mediaAsset.ServerURL,
                ExternalURL = mediaAsset.ExternalURL,
                CreationDate = mediaAsset.CreationDate
            };

            return Ok(mediaAssetDto);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MediaAssetExists(int id)
        {
            return db.MediaAssets.Count(e => e.ID == id) > 0;
        }
    }
}