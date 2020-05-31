using NUnit.Framework;
using PsychoCare.DataAccess.Entities;
using PsychoCare.Logic.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PsychoCare.Logic.Tests
{
    [TestFixture]
    public class PredefinedGroupsTests
    {
        [Test]
        public void Test()
        {
            List<EnvironmentGroup> list = PredefinedGroups.GetPredefinedEnvironmentGroups(userId: 2);
        }
    }
}