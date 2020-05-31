using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace PsychoCare.DataAccess.Tests.Helpers
{
    public static class TestPsychoCareContext
    {
        public static PsychoCareContext Context => GetInMemoryContext();

        private static PsychoCareContext GetInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<PsychoCareContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .EnableSensitiveDataLogging()
                .Options;

            var context = new PsychoCareContext(options);
            return context;
        }
    }
}