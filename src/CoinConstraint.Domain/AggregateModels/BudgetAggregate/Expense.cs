﻿using System;
using System.ComponentModel.DataAnnotations;

namespace CoinConstraint.Domain.AggregateModels.BudgetAggregate
{
    public class Expense
    {

        [Key]
        public int ID { get; set; }

        public int BudgetID { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public bool Automatic { get; set; }

        public bool Pay { get; set; }

        public bool Paid { get; set; }

        public DateTime DueDate { get; set; }

        public decimal Amount { get; set; }

        public string Note { get; set; }
    }
}
