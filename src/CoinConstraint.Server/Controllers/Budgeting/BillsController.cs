﻿using CoinConstraint.Domain.AggregateModels.BudgetAggregate;
using CoinConstraint.Domain.AggregateModels.BudgetAggregate.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoinConstraint.Server.Controllers.Budgeting
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillsController : ControllerBase
    {
        private readonly IBillRepository _billRepository;

        public BillsController(IBillRepository billRepository)
        {
            _billRepository = billRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<Bill>>> GetBillsAsync()
        {
            try
            {
                var bills = await _billRepository.GetAllAsync();
                return Ok(bills);
            }
            catch (System.Exception e)
            {
                Console.WriteLine($"There was an error retrieving bills: {e.Message}");
                throw;
            }
        }

    }
}
