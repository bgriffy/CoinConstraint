﻿namespace CoinConstraint.Domain.AggregateModels.BudgetingAggregate.Entities;

public class Budget
{
    [Key]
    public int ID { get; set; }

    public string Title { get; set; } = "";

    public DateTime StartDate { get; set; } = DateTime.Now;

    public DateTime EndDate { get; set; } = DateTime.Now;

    public List<Total> Totals { get; set; } = new List<Total>();

    public List<Bill> Bills { get; set; } = new List<Bill>();

    public List<Expense> Expenses { get; set; } = new List<Expense>();

    public Guid UUID { get; set; }

    public User User { get; set; } = new User();

    public decimal BudgetedAmount { get; set; }

    public decimal ExpensedAmount
    {
        get => Expenses.Sum(e => e.Amount);
    }

    public List<Note> Notes { get; set; } = new List<Note>();

    [NotMapped]
    public bool IsNew { get; set; }

    [NotMapped]
    public bool IsUpdated { get; set; }
}
