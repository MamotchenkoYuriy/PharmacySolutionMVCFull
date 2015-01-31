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
    public class MedicamentManager : IManager<Medicament>
    {
        private readonly IValidator<Medicament> _validator;
        private readonly IRepository<Medicament> _repository;

        public MedicamentManager(IValidator<Medicament> validator, IRepository<Medicament> repository)
        {
            _validator = validator;
            _repository = repository;
        }


        public void Add(Medicament entity)
        {
            if (!_validator.IsValid(entity)) throw new ValidationException();
            var history = new MedicamentPriceHistory()
            {
                Medicament = entity,
                MedicamentId = entity.Id,
                ModifiedDate = DateTime.Now,
                Price = entity.Price
            };
            entity.MedicamentPriceHistories.Add(history);
            _repository.Add(entity);
            _repository.SaveChanges();
        }

        public void Remove(Medicament entity)
        {
            _repository.Remove(entity);
            _repository.SaveChanges();
        }

        public void Edit(Medicament entity)
        {
            var existingEntity = _repository.GetByPrimaryKey(entity.Id);
            existingEntity.Name = entity.Name; 
            existingEntity.Price = entity.Price; 
            existingEntity.SerialNumber = entity.SerialNumber;
            existingEntity.Description = entity.Description;
            var history = new MedicamentPriceHistory()
            {
                Medicament = existingEntity,
                MedicamentId = existingEntity.Id,
                ModifiedDate = DateTime.Now,
                Price = existingEntity.Price
            };
            existingEntity.MedicamentPriceHistories.Add(history);
            _repository.SaveChanges();
        }

        public IQueryable<Medicament> FindAll()
        {
            return _repository.FindAll();
        }

        public IQueryable<Medicament> Find(Expression<Func<Medicament, bool>> preficate)
        {
            return _repository.Find(preficate);
        }

        public void SaveChanges()
        {
            _repository.SaveChanges();
        }

        public Medicament GetByPrimaryKey(object key)
        {
            return _repository.GetByPrimaryKey(key);
        }
    }
}
