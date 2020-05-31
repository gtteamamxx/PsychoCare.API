using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PsychoCare.DataAccess
{
    public static class PsychoCareContextInitializer
    {
        /// <summary>
        /// Connection strign to production DB
        /// </summary>
        public static string ConnectionString = "***";

        /// <summary>
        /// This method is boilerplate.
        /// It checks if DB model is same as code model (code-first) and runs pending migrations
        /// if it doesn't then exception is thrown
        /// </summary>
        public static void Initialize()
        {
            try
            {
                using (var db = new PsychoCareContext())
                {
                    db.Database.Migrate();

                    IMigrationsAssembly migrationsAssembly = db.GetService<IMigrationsAssembly>();
                    IMigrationsModelDiffer differ = db.GetService<IMigrationsModelDiffer>();

                    bool areDifferences = differ.HasDifferences(migrationsAssembly?.ModelSnapshot?.Model, db.Model);
                    if (areDifferences)
                    {
                        throw new Exception($"[PsychoCare] database doesn't match curret model.");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}