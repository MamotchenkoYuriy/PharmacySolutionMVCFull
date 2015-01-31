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
    public class StorageManager : IManager<Storage>
    {
        private readonly IValidator<Storage> _validator;
        private readonly IRepository<Storage> _repository;

        public StorageManager(IValidator<Storage> validator, IRepository<Storage> repository)
        {
            _validator = validator;
            _repository = repository;
        }

        public void Add(Storage entity)
        {
            if(!_validator.IsValid(entity)) throw new ValidationException();
            _repository.Add(entity);
            _repository.SaveChanges();
        }

        public void Remove(Storage entity)
        {
            _repository.Remove(entity);
            _repository.SaveChanges();
        }

        public void Edit(Storage entity)
        {
            if(!_validator.IsValid(entity)) throw new ValidationException();
            var existingEntity = _repository.Find(m=>m.MedicamentId == entity.MedicamentId && m.PharmacyId == entity.PharmacyId).FirstOrDefault();
            if(existingEntity == null) throw new Exception();
            existingEntity.Count = entity.Count;
            existingEntity.Count= entity.Count;
            _repository.SaveChanges();
        }

        public IQueryable<Storage> FindAll()
        {
            return _repository.FindAll();
        }

        public IQueryable<Storage> Find(Expression<Func<Storage, bool>> preficate)
        {
            return _repository.Find(preficate);
        }

        public void SaveChanges()
        {
            _repository.SaveChanges();
        }

        public Storage GetByPrimaryKey(object key)
        {
            return _repository.GetByPrimaryKey(key);
        }
    }
}
