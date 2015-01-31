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
    public class MedicamentPriceHistoryManager : IManager<MedicamentPriceHistory>
    {
        private readonly IValidator<MedicamentPriceHistory> _validator;
        private readonly IRepository<MedicamentPriceHistory> _repository;

        public MedicamentPriceHistoryManager(IValidator<MedicamentPriceHistory> validator, IRepository<MedicamentPriceHistory> repository)
        {
            _validator = validator;
            _repository = repository;
        }

        public void Add(MedicamentPriceHistory entity)
        {
            if(!_validator.IsValid(entity)) throw new ValidationException();
            _repository.Add(entity);
            _repository.SaveChanges();
        }

        public void Remove(MedicamentPriceHistory entity)
        {
            _repository.Remove(entity);
            _repository.SaveChanges();
        }

        public void Edit(MedicamentPriceHistory entity)
        {
            throw new NotImplementedException();
        }

        public IQueryable<MedicamentPriceHistory> FindAll()
        {
            return _repository.FindAll();
        }

        public IQueryable<MedicamentPriceHistory> Find(Expression<Func<MedicamentPriceHistory, bool>> preficate)
        {
            return _repository.Find(preficate);
        }

        public void SaveChanges()
        {
            _repository.SaveChanges();
        }

        public MedicamentPriceHistory GetByPrimaryKey(object key)
        {
            return _repository.GetByPrimaryKey(key);
        }
    }
}
