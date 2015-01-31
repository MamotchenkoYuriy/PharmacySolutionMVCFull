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
    public class OrderDetailsController : Controller
    {
        private readonly IManager<Order> _orderManager;
        private readonly IManager<OrderDetails> _orderDetailsManager;
        private readonly IManager<Medicament> _medicamentManager;

        public OrderDetailsController(IManager<Order> orderManager, IManager<OrderDetails> orderDetailsManager, IManager<Medicament> medicamentManager)
        {
            _orderManager = orderManager;
            _orderDetailsManager = orderDetailsManager;
            _medicamentManager = medicamentManager;
        }


        [Authorize]
        public ActionResult Index()
        {
            var list = _orderDetailsManager.FindAll();
            var listPharmacies = new List<SelectListItem> { new SelectListItem { Text = "All", Value = "0" } };
            listPharmacies.AddRange(from order in _orderManager.FindAll()
                                    select new SelectListItem() { Text = order.Id.ToString(), Value = order.Id.ToString() });
            ViewBag.Orders = listPharmacies;
            var model = Mapper.Map<IQueryable<OrderDetails>, List<OrderDetailsListViewModel>>(list);
            return View(model);
        }

        [Authorize]
        public PartialViewResult GetTablePartialView(int id = 0)
        {
            if (id == 0)
            {
                return PartialView(
                    Mapper.Map<IQueryable<OrderDetails>,
                    List<OrderDetailsListViewModel>>
                    (_orderDetailsManager.FindAll()));
            }
            var list = from order in _orderManager.FindAll()
                       join orderDetails in _orderDetailsManager.FindAll()
                              on order.Id equals orderDetails.OrderId
                       where order.Id == id
                       select orderDetails;
            return PartialView(Mapper.Map<IQueryable<OrderDetails>, List<OrderDetailsListViewModel>>(list));
        }

        [Authorize]
        public ActionResult Details(int orderId, int medicamentId)
        {
            var entity =
                _orderDetailsManager.Find(m => m.OrderId == orderId && m.MedicamentId == medicamentId).FirstOrDefault();
            var toView = Mapper.Map<OrderDetails, OrderDetailsListViewModel>(entity);
            return View(toView);
        }

        [Authorize(Roles = "Administrators")]
        public ActionResult Create()
        {
            var model = new OrderDetailsCreateViewModel
            {
                Medicaments = new SelectList(_medicamentManager.FindAll(), "Id", "Name"),
                Orders = new SelectList(_orderManager.FindAll(), "Id", "Id")
            };
            return View(model);
        }


        [HttpPost]
        [Authorize(Roles = "Administrators")]
        public ActionResult Create(OrderDetailsCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {

                model.Medicaments = new SelectList(_medicamentManager.FindAll(), "Id", "Name");
                model.Orders = new SelectList(_orderManager.FindAll(), "Id", "Id");
                return View(model);
            }
            try
            {
                var entity = Mapper.Map<OrderDetailsCreateViewModel, OrderDetails>(model);
                _orderDetailsManager.Add(entity);
                return RedirectToAction("Index");
            }
            catch (ValidationException e)
            {
                model.Medicaments = new SelectList(_medicamentManager.FindAll(), "Id", "Name");
                model.Orders = new SelectList(_orderManager.FindAll(), "Id", "Id");
                ModelState.AddModelError("", "Adding new record error!");
                return View(model);
            }
            catch
            {
                model.Medicaments = new SelectList(_medicamentManager.FindAll(), "Id", "Name");
                model.Orders = new SelectList(_orderManager.FindAll(), "Id", "Id");
                ModelState.AddModelError("", "Adding new record error!");
                return View();
            }
        }

        [Authorize(Roles = "Administrators")]
        public ActionResult Edit(int orderId, int medicamentId)
        {
            var entity =
                _orderDetailsManager.Find(m => m.OrderId == orderId && m.MedicamentId == medicamentId).FirstOrDefault();
            var model = Mapper.Map<OrderDetails, OrderDetailsCreateViewModel>(entity);
            model.Medicaments = new SelectList(_medicamentManager.FindAll(), "Id", "Name");
            model.Orders = new SelectList(_orderManager.FindAll(), "Id", "Id");
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Administrators")]
        public ActionResult Edit(OrderDetailsCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Medicaments = new SelectList(_medicamentManager.FindAll(), "Id", "Name");
                model.Orders = new SelectList(_orderManager.FindAll(), "Id", "Id");
                ModelState.AddModelError("", "Validation data error! Change values!");
                return View(model);
            }
            try
            {
                var entity = _orderDetailsManager.Find(m => m.OrderId == model.OrderId && m.MedicamentId == model.MedicamentId).FirstOrDefault();
                entity.OrderId = model.OrderId;
                entity.Medicament = _medicamentManager.GetByPrimaryKey(model.MedicamentId);
                entity.MedicamentId = model.MedicamentId;
                entity.Count = model.Count;
                entity.UnitPrice = model.UnitPrice;
                _orderDetailsManager.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (ValidationException exception)
            {
                model.Medicaments = new SelectList(_medicamentManager.FindAll(), "Id", "Name");
                model.Orders = new SelectList(_orderManager.FindAll(), "Id", "Id");
                ModelState.AddModelError("", "Validation data error! Change values!");
                return View(model);
            }
            catch
            {
                model.Medicaments = new SelectList(_medicamentManager.FindAll(), "Id", "Name");
                model.Orders = new SelectList(_orderManager.FindAll(), "Id", "Id");
                ModelState.AddModelError("", "Adding new record error! Please, try again later!");
                return View(model);
            }
        }

        [Authorize(Roles = "Administrators")]
        public ActionResult Delete(int orderId, int medicamentId)
        {
            var entity =
                _orderDetailsManager.Find(m => m.OrderId == orderId && m.MedicamentId == medicamentId).FirstOrDefault();
            var model = Mapper.Map<OrderDetails, OrderDetailsListViewModel>(entity);
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Administrators")]
        public ActionResult Delete(int orderId, int medicamentId, FormCollection collection)
        {
            var entity =
                _orderDetailsManager.Find(m => m.OrderId == orderId && m.MedicamentId == medicamentId).FirstOrDefault();
            try
            {
                _orderDetailsManager.Remove(entity);
                _orderDetailsManager.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                ModelState.AddModelError("", "Delete record error! Please, try again later!");
                return View(Mapper.Map<OrderDetails, OrderDetailsListViewModel>(entity));
            }
        }
    }
}
