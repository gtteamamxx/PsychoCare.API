using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace PsychoCare.DataAccess.Tests.Helpers
{
    public class TestPsychoCareContext : PsychoCareContext
    {
        public TestPsychoCareContext() : base(GetOptions())
        {
        }

        private static DbContextOptions<PsychoCareContext> GetOptions()
        {
            var options = new DbContextOptionsBuilder<PsychoCareContext>()
                            .UseInMemoryDatabase(databaseName: "TestPsychoCare")
                            .EnableSensitiveDataLogging()
                            .Options;
            return options;
        }
    }
}