using DatabaseOperations.DatabaseContext;
using DatabaseOperations.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Transactions;

namespace DatabaseOperations.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OperationsController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        public OperationsController(ApplicationDbContext dbContext)
        {
            _dbContext=dbContext;
        }

        [HttpPost("RunMultipleOperationsInOneTransaction")]
        public async Task<IActionResult> RunMultipleOperationsInOneTransaction()
        {
            await _dbContext.Students.AddRangeAsync(new Student("Mohamed Ali0"));   
            await _dbContext.Doctors.AddRangeAsync(new Doctor("Ahmed Ali0"));   
            await _dbContext.SaveChangesAsync();

            List<Task> tasks = new();   
            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                for (int i=0; i<100; i++) 
                {
                    var TASK1 = _dbContext.Students.Where(x => x.Name == $"Mohamed Ali{i}").ExecuteUpdateAsync(x => x.SetProperty(x => x.Name, $"Mohamed Ali{i+1}"));
                    var TASK2 = _dbContext.Doctors.Where(x => x.Name == $"Ahmed Ali{i}").ExecuteUpdateAsync(x => x.SetProperty(x => x.Name, $"Ahmed Ali{i+1}"));

                    tasks.Add(TASK1);
                    tasks.Add(TASK2);
                }
               

                await Task.WhenAll(tasks);
                transaction.Complete(); 
            }

            await _dbContext.Students.ExecuteDeleteAsync();
            await _dbContext.Doctors.ExecuteDeleteAsync();
            await _dbContext.SaveChangesAsync();

            return Ok();
        }
    }
}
