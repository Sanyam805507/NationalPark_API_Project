using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NationalPark_API_Project.DTOs;
using NationalPark_API_Project.Models;
using NationalPark_API_Project.Repository;
using NationalPark_API_Project.Repository.IRepository;

namespace NationalPark_API_Project.Controllers
{
    [Route("api/trail")]
    [ApiController]
    public class TrailController : Controller
    {
        private readonly ITrailRepository _trailRepository;
        private readonly IMapper _mapper;
        public TrailController(ITrailRepository trailRepository,IMapper mapper)
        {
            _mapper = mapper;
            _trailRepository = trailRepository;
        }

        [HttpGet]                                     // Display
        public IActionResult GetTrails()
        {
            return Ok(_trailRepository.GetTrails().Select(_mapper.Map<TrailDTO>));
        }

        [HttpGet("{trailId:int}",Name = "GetTrail")]
        public IActionResult GetTrail(int trailId)
        {
            var trail =_trailRepository.GetTrail(trailId);
            if(trail == null) return NotFound();    
            var trailDTO = _mapper.Map<TrailDTO>(trail);   
            return Ok(trailDTO);
        }

        [HttpPost]
        public IActionResult CreateTrail([FromBody]TrailDTO trailDTO)
        {
            if (trailDTO == null) return BadRequest();              // CODE : 400
            if (!ModelState.IsValid) return BadRequest();                 // CODE : 400  
            if (_trailRepository.TrailExists(trailDTO.Name))
            {
                ModelState.AddModelError("", "Trail IN DB !!!!");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            var trail = _mapper.Map<Trail>(trailDTO);
            if (!_trailRepository.CreateTrail(trail))
            {
                ModelState.AddModelError("", "Something Went Wrong While Creating NationalPark !!!!");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return CreatedAtRoute("GetTrail", new { trailId = trail.Id },trail);
        }

        [HttpPut]
        public IActionResult UpdateTrail([FromBody] TrailDTO trailDTO)
        {
            if (trailDTO == null) return BadRequest();
            if (!ModelState.IsValid) return BadRequest();
            var trail = _mapper.Map<Trail>(trailDTO);
            if (!_trailRepository.UpdateTrail(trail))
            {
                ModelState.AddModelError("", "Something Went wrong While Update NationalPark");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return NoContent();                                     //204
        }

        [HttpDelete("{trailId:int}")]
        public IActionResult DeleteTrail(int trailId)
        {
            if (!_trailRepository.TrailExists(trailId))
                return NotFound();
            var trail = _trailRepository.GetTrail(trailId);
            if (trail == null) return NotFound();
            if (!_trailRepository.DeleteTrail(trail))
            {
                ModelState.AddModelError("", "Something Went wrong While Update NationalPark");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return Ok();                                           //200
        }
    }
}
