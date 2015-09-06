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
using AutoMapper;

namespace DashOwl.WebAPI
{
    /// <summary>
    /// MediaAsset entity API controller
    /// </summary>
    public class MediaAssetsController : ApiController
    {
        private DashOwlContext db = new DashOwlContext();
        private readonly IMediaAssetRepository _mediaAssetRepo;
        
        public MediaAssetsController(IMediaAssetRepository repository)
        {
            this._mediaAssetRepo = repository;
        }

        // GET: api/Incidents/1/MediaAssets
        /// <summary>
        /// Gets a list of Vehicles related to Incident.
        /// </summary>
        /// <param name="id">The Incident ID</param>
        /// <returns>A list of MediaAssets</returns>
        [Route("api/incidents/{id}/mediaassets")]
        public async Task<IHttpActionResult> GetMediaAssets(int id)
        {
            List<MediaAsset> assets = _mediaAssetRepo.GetAllForIncident(id);
            IList<MediaAssetDto> mediaAssetsDtoList = Mapper.Map<IList<MediaAsset>, IList<MediaAssetDto>>(assets);

            if (mediaAssetsDtoList == null)
            {
                return NotFound();
            }

            return Ok(mediaAssetsDtoList);
        }

        // GET: api/MediaAssets/5
        /// <summary>
        /// Gets MediaAsset by ID.
        /// </summary>
        /// <param name="id">The ID of MediaAsset</param>
        /// <returns>MediaAsset</returns>
        [ResponseType(typeof(MediaAssetDto))]
        public async Task<IHttpActionResult> GetMediaAsset(int id)
        {
            MediaAsset asset = _mediaAssetRepo.GetSingle(id);
            var media = Mapper.Map<MediaAsset, MediaAssetDto>(asset);

            if (media == null)
            {
                return NotFound();
            }

            return Ok(media);
        }

        // PUT: api/MediaAsset
        /// <summary>
        /// Creates MediaAsset
        /// </summary>        
        /// <param name="mediaAssetDto">Modified MediaAssetDto. JSON sample: {"ID":2,"ServerURL":"/MediaStorage/3.jpg","ExternalURL":"","IncidentID":1,"CreationDate":"2014-12-31T00:00:00"}</param>
        /// <returns>Nothing, if operation was successful</returns>
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutMediaAsset([FromBody] MediaAssetDto mediaAssetDto)
        {
            MediaAsset mediaAsset = Mapper.Map<MediaAssetDto, MediaAsset>(mediaAssetDto);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }            
                        
            _mediaAssetRepo.Edit(mediaAsset);
            _mediaAssetRepo.Save();            

            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Creates MediaAsset.
        /// </summary>
        /// <param name="mediaAssetDto">A new MediaAssetDto. JSON sample: {"ServerURL":"/MediaStorage/7.jpg","ExternalURL":"","IncidentID":"1","CreationDate":"2014-12-31T00:00:00"}</param>
        /// <returns>Created MediaAsset</returns>        
        [ResponseType(typeof(MediaAssetDto))]
        public async Task<IHttpActionResult> PostMediaAsset([FromBody] MediaAssetDto mediaAssetDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var mediaAsset = Mapper.Map<MediaAssetDto, MediaAsset>(mediaAssetDto);

            _mediaAssetRepo.Add(mediaAsset);
            _mediaAssetRepo.Save();

            mediaAssetDto = Mapper.Map<MediaAsset, MediaAssetDto>(mediaAsset);

            return Ok(mediaAssetDto);
        }

        /// <summary>
        /// Deletes MediaAsset
        /// </summary>
        /// <param name="id">The MediaAsset ID</param>
        /// <returns>Ok, if deleted successfully</returns>
        [ResponseType(typeof(MediaAssetDto))]
        public async Task<IHttpActionResult> DeleteMediaAsset(int id)
        {
            MediaAsset mediaAsset = _mediaAssetRepo.GetSingle(id);
            var mediaAssetDto = Mapper.Map<MediaAsset, MediaAssetDto>(mediaAsset);
            
            if (mediaAsset == null)
            {
                return NotFound();
            }

            _mediaAssetRepo.Delete(mediaAsset);
            _mediaAssetRepo.Save();            

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