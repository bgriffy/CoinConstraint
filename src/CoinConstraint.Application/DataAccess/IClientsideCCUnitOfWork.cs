﻿using CoinConstraint.Domain.AggregateModels.BudgetingAggregate.RepositoryInterfaces.Clientside;

namespace CoinConstraint.Application.DataAccess;

public interface IClientsideCCUnitOfWork
{
    IClientsideBudgetRepository Budgets { get; set; }
    IClientsideExpenseRepository Expenses { get; set; }
    IClientsideReminderRepository Reminders { get; set; }
    IClientsideNoteRepository Notes { get; }
}
