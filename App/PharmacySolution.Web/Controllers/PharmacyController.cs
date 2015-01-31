using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using PharmacySolution.Contracts.Manager;
using PharmacySolution.Core;
using PharmacySolution.Web.Core.Models;

namespace PharmacySolution.Web.Controllers
{
    public class PharmacyController : Controller
    {
        private readonly IManager<Pharmacy> _manager;

        public PharmacyController(IManager<Pharmacy> manager)
        {
            _manager = manager;
        }

        [Authorize]
        public ActionResult Index()
        {
            var list = Mapper.Map<IQueryable<Pharmacy>, List<PharmacyViewModel>>(_manager.FindAll());
            return View(list);
        }

        [Authorize]
        public ActionResult Details(int id)
        {
            var entity = _manager.GetByPrimaryKey(id);
            var model = Mapper.Map<Pharmacy, PharmacyViewModel>(entity);
            return View(model);
        }

        [Authorize(Roles = "Administrators")]
        public ActionResult Create()
        {
            return View(new PharmacyViewModel(){ OpenDate = DateTime.Now});
        }

        [HttpPost]
        [Authorize(Roles = "Administrators")]
        public ActionResult Create(PharmacyViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            try
            {
                var pharmacy = Mapper.Map<PharmacyViewModel, Pharmacy>(model);
                _manager.Add(pharmacy);
                return RedirectToAction("Index");
            }
            catch (ValidationException e)
            {
                ModelState.AddModelError("", "Validation data error! Please, change values!");
                return View(model);
            }
            catch
            {
                ModelState.AddModelError("", "Adding data error! Please, try again later!");
                return View(model);
            }
        }

        [Authorize(Roles = "Administrators")]
        public ActionResult Edit(int id)
        {
            var entity = _manager.GetByPrimaryKey(id);
            var model = Mapper.Map<Pharmacy, PharmacyViewModel>(entity);
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Administrators")]
        public ActionResult Edit(PharmacyViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            try
            {
                _manager.Edit(Mapper.Map<PharmacyViewModel, Pharmacy>(model));
                return RedirectToAction("Index");
            }
            catch (ValidationException exception)
            {
                ModelState.AddModelError("", "Validation data error! Please, change values!");
                return View(model);
            }
            catch
            {
                ModelState.AddModelError("Name", "Adding data error! Please, try again later!");
                return View(model);
            }
        }

        [Authorize(Roles = "Administrators")]

        public ActionResult Delete(int id)
        {
            var entity = _manager.GetByPrimaryKey(id);
            return View(Mapper.Map<Pharmacy, PharmacyViewModel>(entity));
        }

        [Authorize(Roles = "Administrators")]
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            var entity = _manager.GetByPrimaryKey(id);
            try
            {
                _manager.Remove(entity);
                return RedirectToAction("Index");
            }
            catch
            {
                ModelState.AddModelError("", "Deleting data error! Please, try again later!");
                return View(Mapper.Map<Pharmacy, PharmacyViewModel>(entity));
            }
        }
    }
}
