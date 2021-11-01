﻿using CoinConstraint.Domain.AggregateModels.BudgetAggregate;
using CoinConstraint.Domain.AggregateModels.BudgetAggregate.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoinConstraint.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpensesController : ControllerBase
    {
        private readonly IExpenseRepository _expenseRepository;

        [HttpGet]
        public async Task<ActionResult<List<Expense>>> GetExpensesAsync()
        {
            try
            {
                var expenses = await _expenseRepository.GetAllAsync();
                return Ok(expenses);
            }
            catch (System.Exception e)
            {
                Console.WriteLine($"There was an error retrieving expenses: {e.Message}");
                throw;
            }
        }
    }
}
