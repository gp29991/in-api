using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace in_api.Models
{
    public class FinancialData
    {
        public int FinancialDataId { get; set; }

        [Column(TypeName = "decimal(9,2)")]
        public decimal Amount { get; set; }

        [Column(TypeName = "date")]
        public DateTime Date { get; set; }

        public string Description { get; set; }

        //UserId as foreign key

        //CategoryId as foreign key
    }
}
