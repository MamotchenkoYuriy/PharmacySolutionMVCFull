using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using PharmacySolution.Contracts.Manager;
using PharmacySolution.Core;
using PharmacySolution.Web.Core.Models;
using System.ComponentModel.DataAnnotations;

namespace PharmacySolution.Web.Controllers
{
    public class StorageController : Controller
    {
        private readonly IManager<Pharmacy> _pharmacyManager;
        private readonly IManager<Medicament> _medicamentManager;
        private readonly IManager<Storage> _storageManager;

        public StorageController(IManager<Pharmacy> pharmacyManager, IManager<Medicament> medicamentManager, IManager<Storage> storageManager)
        {
            _pharmacyManager = pharmacyManager;
            _medicamentManager = medicamentManager;
            _storageManager = storageManager;
        }

        [Authorize]
        public ActionResult Index()
        {
            var listPharmacies = new List<SelectListItem> { new SelectListItem { Text = "All", Value = "0" } };
            listPharmacies.AddRange(from item in _pharmacyManager.FindAll()
                                    select new SelectListItem() { Text = item.Number, Value = item.Id.ToString() });
            ViewBag.Pharmacies = listPharmacies;

            var list = _storageManager.FindAll();
            var listtoView = Mapper.Map<IQueryable<Storage>, List<StorageViewModel>>(list);
            return View(listtoView);
        }

        [HttpGet]
        [Authorize]
        public PartialViewResult GetTablePartialView(int id = 0)
        {
            return PartialView(id == 0 ? Mapper.Map<IQueryable<Storage>, List<StorageViewModel>>(_storageManager.FindAll()) :
                Mapper.Map<IQueryable<Storage>, List<StorageViewModel>>(_storageManager.Find(m => m.PharmacyId == id)));
        }


        [Authorize]
        public ActionResult Details(int pharmacyId, int medicamentId)
        {
            var entity =
                _storageManager.Find(m => m.MedicamentId == medicamentId && m.PharmacyId == pharmacyId).FirstOrDefault();
            return View(Mapper.Map<Storage, StorageViewModel>(entity));
        }

        [Authorize(Roles = "Administrators")]
        public ActionResult Create()
        {
            var model = new StorageCreateViewModel()
            {
                Pharmacies = new SelectList(_pharmacyManager.FindAll(), "Id", "Number"),
                Medicaments = new SelectList(_medicamentManager.FindAll(), "Id", "Name")
            };
            return View(model);
        }

        //
        // POST: /Storage/Create
        [HttpPost]
        [Authorize(Roles = "Administrators")]
        public ActionResult Create(StorageCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Pharmacies = new SelectList(_pharmacyManager.FindAll(), "Id", "Number");
                model.Medicaments = new SelectList(_medicamentManager.FindAll(), "Id", "Name");
                return View();
            }
            try
            {
                var entity = Mapper.Map<StorageCreateViewModel, Storage>(model);
                _storageManager.Add(entity);
                return RedirectToAction("Index");
            }
            catch
            {
                model.Pharmacies = new SelectList(_pharmacyManager.FindAll(), "Id", "Number");
                model.Medicaments = new SelectList(_medicamentManager.FindAll(), "Id", "Name");
                ModelState.AddModelError("error", "Adding new record error!");
                return View(model);
            }
        }

        //
        // GET: /Storage/Edit/5
        public ActionResult Edit(int pharmacyId, int medicamentId)
        {
            var entity =
                _storageManager.Find(m => m.MedicamentId == medicamentId && m.PharmacyId == pharmacyId).FirstOrDefault();
            var model = Mapper.Map<Storage, StorageCreateViewModel>(entity);
            model.Pharmacies = new SelectList(_pharmacyManager.FindAll(), "Id", "Number");
            model.Medicaments = new SelectList(_medicamentManager.FindAll(), "Id", "Name");
            return View(model);
        }


        [HttpPost]
        [Authorize(Roles = "Administrators")]
        public ActionResult Edit(StorageCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Pharmacies = new SelectList(_pharmacyManager.FindAll(), "Id", "Number");
                model.Medicaments = new SelectList(_medicamentManager.FindAll(), "Id", "Name");
                return View(model);
            }
            try
            {
                var entity =
                    _storageManager.Find(m => m.MedicamentId == model.MedicamentId && m.PharmacyId == model.PharmacyId)
                        .FirstOrDefault();
                entity.Count = model.Count;
                _storageManager.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (ValidationException exception)
            {
                model.Pharmacies = new SelectList(_pharmacyManager.FindAll(), "Id", "Number");
                model.Medicaments = new SelectList(_medicamentManager.FindAll(), "Id", "Name");
                ModelState.AddModelError("", "Validation data error! Please, change values!");
                return View(model);
            }
            catch
            {
                model.Pharmacies = new SelectList(_pharmacyManager.FindAll(), "Id", "Number");
                model.Medicaments = new SelectList(_medicamentManager.FindAll(), "Id", "Name");
                ModelState.AddModelError("", "Additing record error!!!");
                return View(model);
            }
        }

        [Authorize(Roles = "Administrators")]

        public ActionResult Delete(int pharmacyId, int medicamentId)
        {
            var entity =
                _storageManager.Find(m => m.MedicamentId == medicamentId && m.PharmacyId == pharmacyId).FirstOrDefault();
            return View(Mapper.Map<Storage, StorageViewModel>(entity));
        }


        [HttpPost]
        [Authorize(Roles = "Administrators")]
        public ActionResult Delete(int pharmacyId, int medicamentId, FormCollection collection)
        {
            var entity =
                    _storageManager.Find(m => m.MedicamentId == medicamentId && m.PharmacyId == pharmacyId)
                        .FirstOrDefault();
            try
            {
                _storageManager.Remove(entity);
                return RedirectToAction("Index");
            }
            catch (ValidationException exception)
            {
                ModelState.AddModelError("", "Validation data error! Please, change values!");
                return View(Mapper.Map<Storage, StorageViewModel>(entity));
            }
            catch
            {
                ModelState.AddModelError("", "Deleting record error!!!");
                return View(Mapper.Map<Storage, StorageViewModel>(entity));
            }
        }
    }
}
