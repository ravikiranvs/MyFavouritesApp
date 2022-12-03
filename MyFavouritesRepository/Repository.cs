using System;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using MyFavouritesEntities;

namespace MyFavouritesRepository
{
	public class DBWorker
	{
        private readonly ILogger<DBWorker> _logger;
        private readonly AWSMySQL _dbContext;
        public DBWorker(ILogger<DBWorker> logger, AWSMySQL dbContext)
		{
			_logger = logger;
            _dbContext = dbContext;
        }

        public IDbContextTransaction CreateTransaction()
        {
            return _dbContext.Database.BeginTransaction();
        }
	}
}

