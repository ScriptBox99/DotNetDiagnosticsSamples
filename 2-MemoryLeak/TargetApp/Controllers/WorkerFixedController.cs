﻿using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace TargetApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WorkerFixedController : ControllerBase
    {
        private readonly ProfilePictureService _profilePictureService;
        private readonly NativeProfilePictureService _nativeProfilePictureService;
        private readonly ILogger<WorkerFixedController> _logger;

        public WorkerFixedController(ProfilePictureService profilePictureService, NativeProfilePictureService nativeProfilePictureService, ILogger<WorkerFixedController> logger)
        {
            _profilePictureService = profilePictureService ?? throw new ArgumentNullException(nameof(profilePictureService));
            _nativeProfilePictureService = nativeProfilePictureService ?? throw new ArgumentNullException(nameof(nativeProfilePictureService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet("RunTask")]
        public ActionResult<byte[]> RunTask()
        {
            // Get a random ID (normally this would be provided by the caller)
            var id = Guid.NewGuid();
            var pictureBytes = _profilePictureService.GetProfilePictureGood(id);

            if (pictureBytes is null)
            {
                _logger.LogInformation("User not found: {Id}", id);
                return NotFound();
            }
            else
            {
                _logger.LogInformation("Retrieved profile picture ({ByteCount} bytes) for user {Id}", pictureBytes.Length, id);
                return Ok(pictureBytes);
            }
        }

        [HttpGet("RunTaskNative")]
        public ActionResult<IEnumerable<byte>> RunTaskNative()
        {
            // Get a random ID (normally this would be provided by the caller)
            var id = Guid.NewGuid();
            var pictureBytes = _nativeProfilePictureService.GetProfilePictureGood(id);

            if (pictureBytes is null)
            {
                _logger.LogInformation("User not found: {Id}", id);
                return NotFound();
            }
            else
            {
                _logger.LogInformation("Retrieved profile picture for user {Id}", id);
                return Ok(pictureBytes);
            }
        }
    }
}
