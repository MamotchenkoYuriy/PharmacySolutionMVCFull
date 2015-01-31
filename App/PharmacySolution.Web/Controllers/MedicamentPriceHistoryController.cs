using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using PharmacySolution.Contracts.Manager;
using PharmacySolution.Core;
using PharmacySolution.Web.Core.Models;

namespace PharmacySolution.Web.Controllers
{
    public class MedicamentPriceHistoryController : Controller
    {
        private readonly IManager<MedicamentPriceHistory> _medicamentPriceHistoryManager;
        private readonly IManager<Medicament> _medicamentManager;

        public MedicamentPriceHistoryController(IManager<MedicamentPriceHistory> medicamentPriceHistoryManager, IManager<Medicament> medicamentManager)
        {
            _medicamentPriceHistoryManager = medicamentPriceHistoryManager;
            _medicamentManager = medicamentManager;
        }

        [Authorize]
        public ActionResult Index()
        {
            var list = _medicamentPriceHistoryManager.FindAll();
            var listMedicaments = new List<SelectListItem> {new SelectListItem {Text = "All", Value = "0"}};
            listMedicaments.AddRange(from medicament in _medicamentManager.FindAll()
                                  select new SelectListItem() { Text = medicament.Name, Value = medicament.Id.ToString() });
            ViewBag.Medicaments = listMedicaments.ToList();
            return View(Mapper.Map<IQueryable<MedicamentPriceHistory>, List<MedicamentPriceHistoryViewModel>>(list));
        }

        [Authorize]
        public PartialViewResult GetTablePartialView(int id = 0)
        {
            if (id == 0)
            {
                return PartialView(
                    Mapper.Map<IQueryable<MedicamentPriceHistory>, 
                    List<MedicamentPriceHistoryViewModel>>
                    (_medicamentPriceHistoryManager.FindAll()));
            }
            var list = from medicamentPriceHistory in _medicamentPriceHistoryManager.FindAll()
                       join medicament in _medicamentManager.FindAll()
                              on medicamentPriceHistory.MedicamentId equals medicament.Id
                              where medicament.Id == id
                       select medicamentPriceHistory;
            return PartialView(Mapper.Map<IQueryable<MedicamentPriceHistory>, List<MedicamentPriceHistoryViewModel>>(list));
        }

        [Authorize(Roles = "Administrators")]
        public ActionResult Delete(int id)
        {
            var entity = _medicamentPriceHistoryManager.GetByPrimaryKey(id);
            if (entity != null)
                return View(Mapper.Map<MedicamentPriceHistory, MedicamentPriceHistoryViewModel>(entity));
            ModelState.AddModelError("", "Entity with this ID not found");
            return View(new MedicamentPriceHistoryViewModel());
        }

        [HttpPost]
        [Authorize(Roles = "Administrators")]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                var entity = _medicamentPriceHistoryManager.GetByPrimaryKey(id);
                if (entity != null)
                {
                    _medicamentPriceHistoryManager.Remove(entity);
                    _medicamentManager.SaveChanges();
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("", "Delete record error!!!");
                return View();
            }
            catch
            {
                ModelState.AddModelError("", "Delete record error!!!");
                return View();
            }
        }
    }
}
