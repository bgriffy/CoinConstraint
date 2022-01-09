﻿using CoinConstraint.Client.Components;
using CoinConstraint.Domain.AggregateModels.BudgetingAggregate.Entities;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

namespace CoinConstraint.Client.Pages
{
    public partial class IndexPage
    {
        private List<Budget> _budgets;
        private Budget _selectedBudget = new Budget();
        private Expense _selectedExpense;
        private Note _selectedNote;
        private LoadSpinnerComponent _loadSpinner;
        private ExpenseDetailComponent _expenseModal;
        private BudgetsModalComponent _budgetsModal;
        private NoteModalComponent _noteModal;
        private string _budgetAmountText;
        private bool _isDirty;
        private bool _isSmallScreen; 
        private bool _isMediumScreen; 
        private bool _isLargeScreen;
        private bool _pageIsLoaded; 

        protected override async Task OnInitializedAsync()
        {
            await BudgetingService.Init();
            await LoadData();
        }

        public List<Budget> BudgetsForDropdown { get => _budgets.Where(b=> b.ID != 0).ToList(); }

        public async Task LoadData()
        {
            _pageIsLoaded = false;
            await LoadBudgets();
            await LoadExpenses();
            await _loadSpinner.HideLoadSpinner();
            _pageIsLoaded = true;
            StateHasChanged();
        }

        public async Task LoadBudgets()
        {
            await _loadSpinner.ShowLoadSpinner("Loading budgets...");
            _budgets = BudgetingService.GetAllBudgets();
            if(_budgets.Count > 0)
            {
                _selectedBudget = _budgets.First();
            }

            if (_selectedBudget != null)
            {
                var notes = _selectedBudget.Notes;
                var rando = notes; 
            }

        }

        private async Task LoadExpenses()
        {
            await _loadSpinner.ShowLoadSpinner("Loading expenses...");

            if ((_selectedBudget?.Expenses?.Count ?? 0) > 0)
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

        private void OpenNoteModalForNewNote()
        {
            _noteModal.Show();
        }

        private async Task OpenPaymentURLAsync(Expense expense)
        {
            await JSRuntime.InvokeAsync<object>("open", expense.PaymentURL, "_blank");
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


        public async Task HandleBudgetChange(int? budgetID)
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

        private async Task HandleBudgetClone(Budget budget)
        {
            await BudgetingService.LoadBudgetData(budget);
        }

        private void HandleNewExpense(Expense newExpense)
        {
            newExpense.BudgetID = _selectedBudget.ID;
            _selectedBudget.Expenses.Add(newExpense);
            _selectedBudget.IsUpdated = true; 
            _isDirty = true;
            StateHasChanged();
        }

        private void HandleNewNote(Note note)
        {
            note.BudgetID = _selectedBudget.ID;
            _selectedBudget.Notes.Add(note);
            _selectedBudget.IsUpdated = true;
            _isDirty = true;
            StateHasChanged();
        }

        private void HandleDeletedExpense(Expense expense)
        {
            if (expense.IsNew) return;
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
            _pageIsLoaded = false;
            await _loadSpinner.ShowLoadSpinner();
            await Task.Delay(1000);
            await BudgetingService.SaveChanges();
            await _loadSpinner.HideLoadSpinner();
            _pageIsLoaded = true;
        }

        private async Task SaveChanges()
        {
            _pageIsLoaded = false;
            await _loadSpinner.ShowLoadSpinner();
            SyncData();
            await Task.Delay(1000);
            await BudgetingService.SaveChanges(removeDeletedExpenses: true);
            await _loadSpinner.HideLoadSpinner();
            _pageIsLoaded = true;
        }
    }
}
