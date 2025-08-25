using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NationalPark_API_Project.DTOs;
using NationalPark_API_Project.Models;
using NationalPark_API_Project.Repository;
using NationalPark_API_Project.Repository.IRepository;

namespace NationalPark_API_Project.Controllers
{
    [Route("api/nationalPark")]
    [ApiController]
    public class NationalParkController : Controller
    {
        private readonly INationalParkRepository _nationalParkRepository;
        private readonly IMapper _mapper;
        public NationalParkController(INationalParkRepository nationalParkRepository,IMapper mapper)
        {
                _mapper = mapper;
            _nationalParkRepository = nationalParkRepository;   
        }
        [HttpGet]                                                      // DISPLAY
        public IActionResult GetNationalParks()
        {
            var nationalParkDTOList = _nationalParkRepository.GetNationalParks()
                .Select(_mapper.Map<NationalParkDTO>);
            return Ok(nationalParkDTOList);                           //vCODE : 200
        }

        [HttpGet("{nationalParkID:int}",Name = "GetAllNationalPark")]         //Find                                             // FIND
        public IActionResult GetAllNationalPark(int nationalParkID)
        {
            var nationalpark = _nationalParkRepository.GetNationalPark(nationalParkID);
            if(nationalpark == null)  return NotFound();              // CODE : 404
            var nationalParkDTO =_mapper.Map<NationalPark,NationalParkDTO>(nationalpark);   
            return Ok(nationalParkDTO);
        }
        [HttpPost]                                                     // SAVE
        public IActionResult CreateNationalPark([FromBody]NationalParkDTO nationalParkDTO)
        {
            if (nationalParkDTO == null) return BadRequest();              // CODE : 400
            if (!ModelState.IsValid) return BadRequest();                 // CODE : 400  
            if(_nationalParkRepository.NationalParkExists(nationalParkDTO.Name))
            {
                ModelState.AddModelError("", "NationalParkINDB !!!!");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            var nationalPark = _mapper.Map<NationalParkDTO, NationalPark>(nationalParkDTO);
            if(!_nationalParkRepository.CreateNationalPark(nationalPark))
            {
                ModelState.AddModelError("", "Something Went Wrong While Creating NationalPark !!!!");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            //return Ok();                                                  // CODE : 200
            return CreatedAtRoute("GetAllNationalPark", new { nationalparkId = nationalPark.Id },nationalPark);        // CODE : 201  
        }

        [HttpPut]
        public IActionResult UpdateNationalPark([FromBody]NationalParkDTO nationalParkDTO)
        {
            if (nationalParkDTO == null) return BadRequest();
            if (!ModelState.IsValid) return BadRequest();
            var nationalPark = _mapper.Map<NationalPark>(nationalParkDTO);
            if(!_nationalParkRepository.UpdateNationalPark(nationalPark))
            {
                ModelState.AddModelError("","Something Went wrong While Update NationalPark");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return NoContent();                                     //204
        }

        [HttpDelete ("{nationalParkId:int}")] 
        public IActionResult DeleteNationalPark(int nationalParkId)
        {
            if(!_nationalParkRepository.NationalParkExists(nationalParkId))
                return NotFound();  
            var nationalPark =_nationalParkRepository.GetNationalPark(nationalParkId);
            if (nationalPark == null) return NotFound();    
            if(!_nationalParkRepository.DeleteNationalPark(nationalPark))
            {
                ModelState.AddModelError("", "Something Went wrong While Update NationalPark");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return Ok();                                           //200
        }
    }
}
