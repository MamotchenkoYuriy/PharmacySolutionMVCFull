using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using PharmacySolution.Contracts.Manager;
using PharmacySolution.Core;
using PharmacySolution.Web.Core.Models;

namespace PharmacySolution.Web.Controllers
{
    public class MedicamentController : Controller
    {
        private readonly IManager<Medicament> _medicamentManager;
        private readonly IManager<Pharmacy> _pharmacyManager;
        private readonly IManager<Storage> _storageManager;

        public MedicamentController(IManager<Medicament> medicamentManager, IManager<Pharmacy> pharmacyManager, IManager<Storage> storageManager)
        {
            _medicamentManager = medicamentManager;
            _pharmacyManager = pharmacyManager;
            _storageManager = storageManager;
        }

        [Authorize]
        public ActionResult Index()
        {
            var list = new List<SelectListItem> {new SelectListItem {Text = "All", Value = "0"}};
            list.AddRange(from item in _pharmacyManager.FindAll()
                select new SelectListItem() {Text = item.Number, Value = item.Id.ToString()});
            ViewBag.Pharmacies = list;
            var listMedicament = Mapper.Map<IQueryable<Medicament>, List<MedicamentViewModel>>(_medicamentManager.FindAll());
            return View(listMedicament);
        }

        [Authorize]
        [HttpGet]
        public PartialViewResult GetTable(int id = 0)
        {
            if (id == 0)
            {
                return PartialView(Mapper.Map<IQueryable<Medicament>, List<MedicamentViewModel>>(_medicamentManager.FindAll()));
            }
            var listMedicament = from medicament in _medicamentManager.FindAll()
                join storage in _storageManager.FindAll() on medicament.Id equals storage.MedicamentId
                join pharmacy in _pharmacyManager.FindAll() on storage.PharmacyId equals pharmacy.Id
                where pharmacy.Id == id
                select medicament;
            return PartialView(Mapper.Map<IQueryable<Medicament>, List<MedicamentViewModel>>(listMedicament));
        }

        [Authorize]
        public ActionResult Details(int id)
        {
            var entity = _medicamentManager.GetByPrimaryKey(id);
            return View(Mapper.Map<Medicament, MedicamentViewModel>(entity));
        }

        [Authorize(Roles = "Administrators")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Administrators")]
        public ActionResult Create(MedicamentViewModel medicamentView)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Validation data error! Please, change input values!");
                return View(medicamentView);
            }
            try
            {
                var entity = Mapper.Map<MedicamentViewModel, Medicament>(medicamentView);
                _medicamentManager.Add(entity);
                return RedirectToAction("Index");
            }
            catch (ValidationException exception)
            {
                ModelState.AddModelError("", "Validation data error! Please, change input values!");
                return View(medicamentView);
            }
            catch
            {
                ModelState.AddModelError("", "Adding new reccord error! Please, try again later!");
                return View(medicamentView);
            }
        }

        [Authorize(Roles = "Administrators")]
        public ActionResult Edit(int id)
        {
            var medicament = _medicamentManager.GetByPrimaryKey(id);
            return View(Mapper.Map<Medicament, MedicamentViewModel>(medicament));
        }

        [HttpPost]
        [Authorize(Roles = "Administrators")]
        public ActionResult Edit(MedicamentViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            try
            {
                _medicamentManager.Edit(Mapper.Map<MedicamentViewModel, Medicament>(model));
                return RedirectToAction("Index");
            }
            catch
            {
                ModelState.AddModelError("", "Adding new reccord error! Please, try again later!");
                return View(model);
            }
        }

        [Authorize(Roles = "Administrators")]
        public ActionResult Delete(int id)
        {
            var entity = _medicamentManager.GetByPrimaryKey(id);
            return View(Mapper.Map<Medicament, MedicamentViewModel>(entity));
        }

        [HttpPost]
        [Authorize(Roles = "Administrators")]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                var entity = _medicamentManager.GetByPrimaryKey(id);
                _medicamentManager.Remove(entity);
                return RedirectToAction("Index");
            }
            catch
            {
                ModelState.AddModelError("", "Вознизкла ошибка при удалении данных!");
                return View();
            }
        }
    }
}
