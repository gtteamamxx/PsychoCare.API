using Microsoft.AspNetCore.Mvc;
using PsychoCare.Common.Interfaces;
using PsychoCare.DataAccess.Entities;
using PsychoCare.Logic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PsychoCare.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class EnvironmentGroupsController : ControllerBase
    {
        private readonly IEnvironmentGroupsService _environmentGroupsService;
        private readonly IWebPrincipal _webPrincipal;

        public EnvironmentGroupsController(IEnvironmentGroupsService environmentGroupsService, IWebPrincipal webPrincipal)
        {
            _environmentGroupsService = environmentGroupsService;
            _webPrincipal = webPrincipal;
        }

        /// <summary>
        /// Add new environment group
        /// </summary>
        /// <param name="environmentGroup">New environemnt group model with relation</param>
        /// <returns></returns>
        [HttpPost]
        public int AddEnvironmentGroup([FromBody] EnvironmentGroup environmentGroup)
        {
            if (environmentGroup == null) throw new ArgumentNullException(nameof(environmentGroup));

            environmentGroup.UserId = _webPrincipal.UserId;

            int id = _environmentGroupsService.AddEnvironmentGroup(environmentGroup);
            return id;
        }

        /// <summary>
        /// Deletes concrete environment group
        /// </summary>
        /// <param name="environmentGroupId">Environment group id</param>
        [HttpDelete]
        public void DeleteEnvironmentGroup(int environmentGroupId)
        {
            if (environmentGroupId == 0) throw new ArgumentException(nameof(environmentGroupId));

            _environmentGroupsService.DeleteEnvironmentGroup(environmentGroupId);
        }

        /// <summary>
        /// Edits environment group
        /// </summary>
        /// <param name="environmentGroup">Edited environment group</param>
        [HttpPut]
        public void EditEnvironmentGroupName([FromBody] EnvironmentGroup environmentGroup)
        {
            if (environmentGroup == null) throw new ArgumentNullException(nameof(environmentGroup));

            environmentGroup.UserId = _webPrincipal.UserId;

            _environmentGroupsService.EditEnvironmentGroup(environmentGroup);
        }

        /// <summary>
        /// Get all user environment groups
        /// </summary>
        /// <returns></returns>
        [HttpGet("all")]
        public List<EnvironmentGroup> GetUserEnvironmentGroups()
        {
            int userId = _webPrincipal.UserId;

            List<EnvironmentGroup> environmentGroups = _environmentGroupsService.GetUserEnvironmentGroups(userId);
            return environmentGroups;
        }
    }
}