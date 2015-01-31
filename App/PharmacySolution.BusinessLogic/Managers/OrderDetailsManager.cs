using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using PharmacySolution.Contracts.Manager;
using PharmacySolution.Contracts.Repository;
using PharmacySolution.Contracts.Validator;
using PharmacySolution.Core;

namespace PharmacySolution.BusinessLogic.Managers
{
    public class OrderDetailsManager : IManager<OrderDetails>
    {
        private readonly IValidator<OrderDetails> _validator;
        private readonly IRepository<OrderDetails> _repository;

        public OrderDetailsManager(IValidator<OrderDetails> validator, IRepository<OrderDetails> repository)
        {
            _validator = validator;
            _repository = repository;
        }

        public void Add(OrderDetails entity)
        {
            if(!_validator.IsValid(entity)) throw new ValidationException();
            _repository.Add(entity);
            _repository.SaveChanges();
        }

        public void Remove(OrderDetails entity)
        {
            _repository.Remove(entity);
            _repository.SaveChanges();
        }

        public void Edit(OrderDetails entity)
        {
            if(!_validator.IsValid(entity)) throw new ValidationException();
            var existingEntity = _repository.Find(m=>m.MedicamentId == entity.MedicamentId && m.OrderId == entity.OrderId).FirstOrDefault();
            if(existingEntity == null) throw new Exception();
            existingEntity.UnitPrice = entity.UnitPrice;
            existingEntity.Count= entity.Count;
            _repository.SaveChanges();
        }

        public IQueryable<OrderDetails> FindAll()
        {
            return _repository.FindAll();
        }

        public IQueryable<OrderDetails> Find(Expression<Func<OrderDetails, bool>> preficate)
        {
            return _repository.Find(preficate);
        }

        public void SaveChanges()
        {
            _repository.SaveChanges();
        }

        public OrderDetails GetByPrimaryKey(object key)
        {
            return _repository.GetByPrimaryKey(key);
        }
    }
}
