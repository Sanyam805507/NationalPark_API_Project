﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NationalParkWebApp.Models;
using NationalParkWebApp.Models.ViewModels;
using NationalParkWebApp.Repository.IRepository;

namespace NationalParkWebApp.Controllers
{
    public class TrailController : Controller
    {
        private readonly INationalParkRepository _nationalparkRepository;
        private readonly ITrailRepository _trailRepository;
        public TrailController(INationalParkRepository nationalParkRepository,ITrailRepository trailRepository)
        {
            _nationalparkRepository = nationalParkRepository;
            _trailRepository = trailRepository;
        }
        #region APIs
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Json(new { data = await _trailRepository.GetAllAsync(SD.TrailAPIPath) });
        }
        [HttpDelete]
        public async Task<IActionResult> Delete (int id)
        {
            var Status =await _trailRepository.DeleteAsync(SD.TrailAPIPath, id);
            if (Status)
                return Json(new
                {
                    success = true,
                    message = "Data Deleted Successfully !!!!"
                });
            return Json(new
            {
                success = false,
                message = "Something Went Wrong While Deleting Data!!!!"
            });
        }
        #endregion
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Upsert(int? id)
        {
            var nationalParkList = await _nationalparkRepository.GetAllAsync(SD.NationalParkAPIPath);
            TrailVM trailVM = new TrailVM()
            {
                NationalParkList = nationalParkList.Select(nl => new SelectListItem()
                {
                    Text = nl.Name,
                    Value = nl.Id.ToString()
                }),
                Trail =new Trail()
            };
            if(id ==null) return View(trailVM);
            trailVM .Trail =await _trailRepository.GetAsync(SD.TrailAPIPath,id.GetValueOrDefault());    
            if (trailVM.Trail == null) return NotFound();   
            return View(trailVM);   
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>Upsert(TrailVM trailVM)
        {
            if (trailVM == null)    return BadRequest();
            if (!ModelState.IsValid) return BadRequest();   
            if(trailVM.Trail.Id==0)
                await _trailRepository.CreateAsync(SD.TrailAPIPath,trailVM.Trail);
            else 
                await _trailRepository.UpdateAsync(SD.TrailAPIPath,trailVM.Trail);
            return RedirectToAction(nameof(Index));

        }
    }
}
