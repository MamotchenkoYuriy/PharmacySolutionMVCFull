using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using FluentValidation.Attributes;
using PharmacySolution.Core;
using PharmacySolution.Web.Core.Validators;

namespace PharmacySolution.Web.Core.Models
{
    [Validator(typeof(OrderCreateViewModelValidator))]
    public class OrderCreateViewModel: BaseEntity
    {
        public int PharmacyId { get; set; }
        public DateTime OperationDate { get; set; }
        public OperationType OperationType { get; set; }
        public SelectList Pharmacies { get; set; }
        public SelectList Types { get; set; }
    }
}
