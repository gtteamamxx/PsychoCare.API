using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PsychoCare.API.Services;
using PsychoCare.Common.Interfaces;
using PsychoCare.DataAccess.Entities;
using PsychoCare.Logic.Interfaces;

namespace PsychoCare.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class EmotionalStatesController : ControllerBase
    {
        private readonly IEmotionalStatesService _emotionalStatesService;
        private readonly ITokenService _tokenService;
        private readonly IWebPrincipal _webPrincipal;

        public EmotionalStatesController(
            ITokenService tokenService,
            IEmotionalStatesService emotionalStatesService,
            IWebPrincipal webPrincipal)
        {
            _emotionalStatesService = emotionalStatesService;
            _webPrincipal = webPrincipal;
            _tokenService = tokenService;
        }

        /// <summary>
        /// Adds emotional states
        /// </summary>
        /// <param name="emotionalState"></param>
        [HttpPost]
        public void AddEmotionalState([FromBody] EmotionalState emotionalState)
        {
            if (emotionalState == null) throw new ArgumentNullException(nameof(emotionalState));

            emotionalState.UserId = _webPrincipal.UserId;

            _emotionalStatesService.AddEmotionalState(emotionalState);
        }

        /// <summary>
        /// Downloads all emotional states of logged user (by token)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("all")]
        public List<EmotionalState> GetAllEmotionalStates()
        {
            List<EmotionalState> emotionalStates = _emotionalStatesService.GetEmotionalStates(_webPrincipal.UserId);
            return emotionalStates;
        }

        /// <summary>
        /// Downloads all emotional states for concrete user
        /// </summary>
        /// <param name="targetUserToken">Provide token of user who you want to download emotional states</param>
        /// <returns></returns>
        [HttpGet]
        [Route("allForUser")]
        public List<EmotionalState> GetAllEmotionalStatesForUser(string targetUserToken)
        {
            if (targetUserToken == null) throw new ArgumentNullException(targetUserToken);

            int targetUserId = _tokenService.GetUserIdFromToken(targetUserToken);

            List<EmotionalState> emotionalStates = _emotionalStatesService.GetEmotionalStates(targetUserId);
            return emotionalStates;
        }
    }
}