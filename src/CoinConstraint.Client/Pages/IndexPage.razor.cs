﻿using CoinConstraint.Client.Components;
using CoinConstraint.Domain.AggregateModels.BudgetingAggregate.Entities;

namespace CoinConstraint.Client.Pages
{
    public partial class IndexPage
    {
        private Expense _selectedExpense;
        private List<Budget> _budgets;
        private Budget _selectedBudget;
        private LoadSpinnerComponent _loadSpinner;
        private bool _pageIsLoaded = false;
        private ExpenseDetailComponent _expenseModal;
        private BudgetsModalComponent _budgetsModal;
        private string _budgetAmountText;
        private bool _isDirty;


        protected override async Task OnInitializedAsync()
        {
            await BudgetingService.Init();
            await LoadData();
        }


        public List<Budget> BudgetsForDropdown { get => _budgets.Where(b=> b.ID != 0).ToList(); }


        public async Task LoadData()
        {
            await LoadBudgets();
            await LoadExpenses();
            await _loadSpinner.HideLoadSpinner();
            StateHasChanged();
        }

        public async Task LoadBudgets()
        {
            await _loadSpinner.ShowLoadSpinner("Loading budgets...");
            _budgets = BudgetingService.GetAllBudgets();
            _selectedBudget = _budgets.FirstOrDefault();
        }

        private async Task LoadExpenses()
        {
            await _loadSpinner.ShowLoadSpinner("Loading expenses...");

            if ((_selectedBudget.Expenses?.Count ?? 0) > 0)
            {
                _selectedExpense = _selectedBudget.Expenses[0];
            }

            if (_selectedBudget != null)
            {
                _budgetAmountText = _selectedBudget.BudgetedAmount.ToString();
            }
        }

        private void OpenBudgetManagementModal()
        {
            _budgetsModal.Show();
        }

        private void OpenExpenseDetailModal()
        {
            _expenseModal.ShowNewExpense();
        }

        private void OpenExpenseDetailModal(Expense expense)
        {
            _expenseModal.ShowExpense(expense);
        }
        
        private void HandleUpdatedExpenses()
        {
            _isDirty = true;
            _selectedBudget.IsUpdated = true;
            Refresh();

        }

        private void HandleUpdatedBudgets()
        {
            _isDirty = true;
            Refresh();
        }

        private void Refresh()
        {
            StateHasChanged();
        }


        public async Task HandleBudgetChange(int budgetID)
        {
            _selectedBudget = _budgets.FirstOrDefault(b => b.ID == budgetID);
            await BudgetingService.SetSelectedBudget(_selectedBudget);
            await LoadExpenses();
            await _loadSpinner.HideLoadSpinner();

            StateHasChanged();
        }

        private async Task HandleNewBudget(Budget newBudget)
        {
            BudgetingService.AddNewBudget(newBudget);
            await LoadData();
        }


        private void HandleNewExpense(Expense newExpense)
        {
            newExpense.BudgetID = _selectedBudget.ID;
            _selectedBudget.Expenses.Add(newExpense);
            _selectedBudget.IsUpdated = true; 
            _isDirty = true;
            StateHasChanged();
        }

        private void HandleDeletedExpense(Expense expense)
        {
            BudgetingService.MarkExpenseForDeletion(expense);
            _selectedBudget.IsUpdated = true;
            _isDirty = true;
        }

        private void HandleDeletedBudget(Budget budget)
        {
            BudgetingService.MarkBudgetForDeletion(budget);
            _isDirty = true;
        }

        private void HandleBudgetAmountChange(string text)
        {
            _budgetAmountText = text;
            _isDirty = true;
        }

        private void SyncData()
        {
            decimal newBudgetAmountValue = String.IsNullOrWhiteSpace(_budgetAmountText) ? 0 : decimal.Parse(_budgetAmountText);
            if (newBudgetAmountValue != _selectedBudget.BudgetedAmount)
            {
                _selectedBudget.IsUpdated = true;
                _selectedBudget.BudgetedAmount = newBudgetAmountValue;
            }
        }

        private async Task SaveBudgets()
        {
            await _loadSpinner.ShowLoadSpinner("Saving budgets...");
            await Task.Delay(1000);
            await BudgetingService.SaveBudgets();
            await _loadSpinner.HideLoadSpinner();
        }

        private async Task SaveChanges()
        {
            await _loadSpinner.ShowLoadSpinner("Saving changes...");
            SyncData();
            await Task.Delay(1000);
            await BudgetingService.SaveChanges();
            await _loadSpinner.HideLoadSpinner();
        }
    }
}
