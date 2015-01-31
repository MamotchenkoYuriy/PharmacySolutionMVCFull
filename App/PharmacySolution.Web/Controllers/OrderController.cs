using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using AutoMapper;
using Castle.Components.DictionaryAdapter;
using PharmacySolution.Contracts.Manager;
using PharmacySolution.Core;
using PharmacySolution.Web.Core.Models;
using PharmacySolution.Web.Core.Validators;
using System.ComponentModel.DataAnnotations;

namespace PharmacySolution.Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly IManager<Pharmacy> _pharmacyManager;
        private readonly IManager<Order> _orderManager;

        public OrderController(IManager<Pharmacy> pharmacyManager, IManager<Order> orderManager)
        {
            _pharmacyManager = pharmacyManager;
            _orderManager = orderManager;
        }

        [Authorize]
        public ActionResult Index()
        {
            var list = _orderManager.FindAll();
            var listPharmacies = new List<SelectListItem> { new SelectListItem { Text = "All", Value = "0" } };
            listPharmacies.AddRange(from pharmacy in _pharmacyManager.FindAll()
                                    select new SelectListItem() { Text = pharmacy.Number, Value = pharmacy.Id.ToString() });
            ViewBag.Orders = listPharmacies.ToList();
            return View(Mapper.Map<IQueryable<Order>, List<OrderListViewModel>>(list));
        }

        [Authorize]
        public PartialViewResult GetTablePartialView(int id = 0)
        {
            if (id == 0)
            {
                return PartialView(
                    Mapper.Map<IQueryable<Order>,
                    List<OrderListViewModel>>
                    (_orderManager.FindAll()));
            }
            var list = from order in _orderManager.FindAll()
                       join pharmacy in _pharmacyManager.FindAll()
                              on order.PharmacyId equals pharmacy.Id
                       where pharmacy.Id == id
                       select order;
            return PartialView(Mapper.Map<IQueryable<Order>, List<OrderListViewModel>>(list));
        }

        [Authorize]
        public ActionResult Details(int id)
        {
            var entity = _orderManager.GetByPrimaryKey(id);
            if (entity == null) return View();
            return View(Mapper.Map<Order, OrderListViewModel>(entity));
        }
        
        [Authorize(Roles = "Administrators")]
        public ActionResult Create()
        {
            var model = new OrderCreateViewModel
            {
                Pharmacies = new SelectList(_pharmacyManager.FindAll(), "Id", "Number"),
                Types =
                    new SelectList(new List<object>() {new {Id = 2, Value = "Purchase"}, new {Id = 1, Value = "Sale"}},
                        "Id", "Value"),
                OperationDate = DateTime.Now
            };
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Administrators")]
        public ActionResult Create(OrderCreateViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            try
            {
                _orderManager.Add(Mapper.Map<OrderCreateViewModel, Order>(model));
                return RedirectToAction("Index");
            }
            catch (ValidationException exception)
            {
                model.Pharmacies = new SelectList(_pharmacyManager.FindAll(), "Id", "Number");
                model.Types = new SelectList(new List<object>() { new { Id = 2, Value = "Purchase" }, new { Id = 1, Value = "Sale" } }, "Id", "Value");
                ModelState.AddModelError("", "Validation data error! Change values!");
                return View(model);
            }
            catch
            {
                model.Pharmacies = new SelectList(_pharmacyManager.FindAll(), "Id", "Number");
                model.Types = new SelectList(new List<object>() {new {Id = 2, Value = "Purchase"}, new {Id = 1, Value = "Sale"}}, "Id", "Value");
                ModelState.AddModelError("", "Adding new record error!");
                return View(model);
            }
        }

        [Authorize(Roles = "Administrators")]
        public ActionResult Edit(int id)
        {
            var entity = _orderManager.GetByPrimaryKey(id);
            if (entity != null)
            {
                var model = Mapper.Map<Order, OrderCreateViewModel>(entity);
                model.Pharmacies = new SelectList(_pharmacyManager.FindAll(), "Id", "Number");
                model.Types = new SelectList(new List<object>() { new { Id = 2, Value = "Purchase" }, new { Id = 1, Value = "Sale" } }, "Id", "Value");
                return View(model);
            }
            ModelState.AddModelError("", "Запись с введенным ID не найденна");
            return View(new OrderCreateViewModel());
        }

        [HttpPost]
        [Authorize(Roles = "Administrators")]
        public ActionResult Edit(OrderCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Pharmacies = new SelectList(_pharmacyManager.FindAll(), "Id", "Number");
                model.Types = new SelectList(new List<object>() { new { Id = 2, Value = "Purchase" }, new { Id = 1, Value = "Sale" } }, "Id", "Value");
                return View(model);
            }
            try
            {
               _orderManager.Edit(Mapper.Map<OrderCreateViewModel, Order>(model));
                return RedirectToAction("Index");
            }
            catch
            {
                ModelState.AddModelError("Name", "Something wrong with editing record!");
                return View(model);
            }
        }

        [Authorize(Roles = "Administrators")]
        public ActionResult Delete(int id)
        {
            var entity = _orderManager.GetByPrimaryKey(id);
            return entity != null ? View(Mapper.Map<Order, OrderListViewModel>(entity)) : View(new OrderListViewModel());
        }

        [HttpPost]
        [Authorize(Roles = "Administrators")]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                var entity = _orderManager.GetByPrimaryKey(id);
                if (entity != null)
                {
                    _orderManager.Remove(entity);
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
