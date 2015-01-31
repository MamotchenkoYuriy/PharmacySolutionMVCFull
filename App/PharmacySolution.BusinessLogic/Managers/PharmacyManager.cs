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
    public class PharmacyManager : IManager<Pharmacy>
    {
        private readonly IValidator<Pharmacy> _validator;
        private readonly IRepository<Pharmacy> _repository;

        public PharmacyManager(IValidator<Pharmacy> validator, IRepository<Pharmacy> repository)
        {
            _validator = validator;
            _repository = repository;
        }

        public void Add(Pharmacy entity)
        {
            if(!_validator.IsValid(entity)) throw new ValidationException();
            _repository.Add(entity);
            _repository.SaveChanges();
        }

        public void Remove(Pharmacy entity)
        {
            _repository.Remove(entity);
            _repository.SaveChanges();
        }

        public void Edit(Pharmacy entity)
        {
            if(!_validator.IsValid(entity)) throw new ValidationException();
            var existingEntity = _repository.GetByPrimaryKey(entity.Id);
            existingEntity.Number = entity.Number;
            existingEntity.PhoneNumber = entity.PhoneNumber;
            existingEntity.OpenDate= entity.OpenDate;
            existingEntity.Address = entity.Address;
            _repository.SaveChanges();
        }

        public IQueryable<Pharmacy> FindAll()
        {
            return _repository.FindAll();
        }

        public IQueryable<Pharmacy> Find(Expression<Func<Pharmacy, bool>> preficate)
        {
            return _repository.Find(preficate);
        }

        public void SaveChanges()
        {
            _repository.SaveChanges();
        }

        public Pharmacy GetByPrimaryKey(object key)
        {
            return _repository.GetByPrimaryKey(key);
        }
    }
}
