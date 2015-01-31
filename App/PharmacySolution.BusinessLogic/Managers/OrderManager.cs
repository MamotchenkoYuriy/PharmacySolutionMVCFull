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
    public class OrderManager : IManager<Order>
    {
        private readonly IValidator<Order> _validator;
        private readonly IRepository<Order> _repository;
        private readonly IRepository<Pharmacy> _repositoryPharmacy;

        public OrderManager(IValidator<Order> validator, IRepository<Order> repository, IRepository<Pharmacy> repositoryPharmacy)
        {
            _validator = validator;
            _repository = repository;
            _repositoryPharmacy = repositoryPharmacy;
        }

        public void Add(Order entity)
        {
            if(!_validator.IsValid(entity)) throw new ValidationException();
            _repository.Add(entity);
            _repository.SaveChanges();
        }

        public void Remove(Order entity)
        {
            _repository.Remove(entity);
            _repository.SaveChanges();
        }

        public void Edit(Order entity)
        {
            if(!_validator.IsValid(entity)) throw new ValidationException();
            var existingEntity = _repository.GetByPrimaryKey(entity.Id);
            existingEntity.OperationDate = entity.OperationDate;
            existingEntity.OperationType = entity.OperationType;
            existingEntity.Pharmacy = _repositoryPharmacy.GetByPrimaryKey(entity.PharmacyId);
            existingEntity.PharmacyId = entity.PharmacyId;
            _repository.SaveChanges();
        }

        public IQueryable<Order> FindAll()
        {
            return _repository.FindAll();
        }

        public IQueryable<Order> Find(Expression<Func<Order, bool>> preficate)
        {
            return _repository.Find(preficate);
        }

        public void SaveChanges()
        {
            _repository.SaveChanges();
        }

        public Order GetByPrimaryKey(object key)
        {
            return _repository.GetByPrimaryKey(key);
        }
    }
}
