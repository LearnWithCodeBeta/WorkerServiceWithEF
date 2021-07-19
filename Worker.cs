using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WorkerServiceApp1.Models;
using WorkerServiceApp1.Services;

namespace WorkerServiceApp1
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly DbHelper dbHelper;
        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
            dbHelper = new DbHelper();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                List<User> users = dbHelper.GetAllUser();
                if (users.Count == 0)
                    dbHelper.SeedData();
                else
                    PrintUserInfo(users);
                await Task.Delay(1000, stoppingToken);
            }
        }

        private void PrintUserInfo(List<User> users)
        {
            users?.ForEach(user =>
            {
                _logger.LogInformation($"User Info: Name: {user.Name} and Address: {user.Address}");
            });
        }
    }
}
