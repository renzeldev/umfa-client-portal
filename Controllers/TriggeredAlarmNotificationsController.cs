﻿using ClientPortal.Controllers.Authorization;
using ClientPortal.Data;
using ClientPortal.Data.Entities.PortalEntities;
using ClientPortal.Interfaces;
using ClientPortal.Models.ResponseModels;
using ClientPortal.Services;
using ServiceStack;
using System.Dynamic;

namespace ClientPortal.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class TriggeredAlarmNotificationsController : ControllerBase
    {
        private readonly PortalDBContext _context;
        private readonly IUserService _userService;
        private readonly INotificationService _notificationService;

        public TriggeredAlarmNotificationsController(PortalDBContext context, IUserService userService, INotificationService notificationService)
        {
            _context = context;
            _userService = userService;
            _notificationService = notificationService;
        }

        // GET: TriggeredAlarmNotifications
        [HttpGet("getTriggeredAlarmNotifications")]
        public async Task<ActionResult<IEnumerable<TriggeredAlarmNotification>>> GetTriggeredAlarmNotifications()
        {
            if (_context.TriggeredAlarmNotifications == null)
            {
                return NotFound();
            }
            return await _context.TriggeredAlarmNotifications.ToListAsync();
        }

        // GET: TriggeredAlarmNotifications/5
        [HttpGet("getTriggeredAlarmNotification/{id}")]
        public async Task<ActionResult<TriggeredAlarmNotification>> GetTriggeredAlarmNotification(int id)
        {
            if (_context.TriggeredAlarmNotifications == null)
            {
                return NotFound();
            }
            var triggeredAlarmNotification = await _context.TriggeredAlarmNotifications.FindAsync(id);

            if (triggeredAlarmNotification == null)
            {
                return NotFound();
            }

            return triggeredAlarmNotification;
        }

        // GET: ActiveTriggeredAlarmNotificationsForUser
        [HttpGet("getActiveTriggeredAlarmNotificationsForUser/{userId}")]
        public async Task<ActionResult<IEnumerable<TriggeredAlarmNotificationResult>>> GetActiveTriggeredAlarmNotificationsForUser(int userId)
        {
            if (_context.TriggeredAlarmNotifications == null)
            {
                return NotFound();
            }
            var results = await _context.TriggeredAlarmNotifications
                .Where(a => (a.Active && a.UserId == userId))
                .ToListAsync();

            var userResults = new List<TriggeredAlarmNotificationResult>();

            foreach (var result in results)
            {
                var userResult = new TriggeredAlarmNotificationResult();

                //Get User
                var user = _userService.GetUserById(result.UserId);
                //Get Status
                var status = GetStatus(result.Status);
                userResult.FirstName = user.FirstName;
                userResult.LastName = user.LastName;
                userResult.NotificationEmailAddress = string.IsNullOrEmpty(user.NotificationEmailAddress) ? string.Empty : user.NotificationEmailAddress;
                userResult.Status = status;
                userResult.SendDate = result.SendDateTime;
                userResult.SendStatusMessage = string.IsNullOrEmpty(result.SendStatusMessage) ? string.Empty : result.SendStatusMessage;
                userResult.MessageBody = string.IsNullOrEmpty(result.MessageBody) ? string.Empty : result.MessageBody;

                userResults.Add(userResult);
            }
            return userResults;
        }

        // GET: AllTriggeredAlarmNotificationsForUser
        [HttpGet("getAllTriggeredAlarmNotificationsForUser/{userId}")]
        public async Task<ActionResult<IEnumerable<TriggeredAlarmNotification>>> GetAllTriggeredAlarmNotificationsForUser(int userId)
        {
            if (_context.TriggeredAlarmNotifications == null)
            {
                return NotFound();
            }
            return await _context.TriggeredAlarmNotifications
                .Where(a => (a.UserId == userId))
                .ToListAsync();
        }

        // POST: CreateOrUpdateTriggeredAlarmNotification
        [HttpPost("createOrUpdateTriggeredAlarmNotification")]
        public async Task<ActionResult<TriggeredAlarmNotification>> CreateOrUpdateTriggeredAlarmNotification(TriggeredAlarmNotification triggeredAlarmNotification)
        {
            if (_context.TriggeredAlarmNotifications == null)
            {
                return Problem("Entity set 'PortalDBContext.TriggeredAlarmNotifications' is null.");
            }

            if (triggeredAlarmNotification.TriggeredAlarmNotificationId > 0)
            {
                // UPDATE
                _context.Entry(triggeredAlarmNotification).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TriggeredAlarmNotificationExists(triggeredAlarmNotification.TriggeredAlarmNotificationId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return NoContent();
            }
            else
            {
                // ADD
                _context.TriggeredAlarmNotifications.Add(triggeredAlarmNotification);
                await _context.SaveChangesAsync();
                return CreatedAtAction("GetTriggeredAlarmNotification", new { id = triggeredAlarmNotification.TriggeredAlarmNotificationId }, triggeredAlarmNotification);
            }
        }

        // DELETE: TriggeredAlarmNotifications/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTriggeredAlarmNotification(int id)
        {
            if (_context.TriggeredAlarmNotifications == null)
            {
                return NotFound();
            }
            var triggeredAlarmNotification = await _context.TriggeredAlarmNotifications.FindAsync(id);
            if (triggeredAlarmNotification == null)
            {
                return NotFound();
            }

            _context.TriggeredAlarmNotifications.Remove(triggeredAlarmNotification);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("getNotificationsToSend")]
        public async Task<ActionResult<IEnumerable<NotificationToSend>>> GetNotificationsToSend()
        {
            try
            {
                return (await _notificationService.GetNotificationsToSendAsync()).NotificationsToSend;
            }
            catch (Exception)
            {
                return Problem($"Failed to get NotificationsToSend");
            }
        }

        // TEST: TestSendNotifications
        [HttpGet("testSendNotifications")]
        public async Task<IActionResult> TestSendNotifications()
        {
            await _notificationService.SendPendingNotifications();
            
            return Accepted();
        }

        private bool TriggeredAlarmNotificationExists(int id)
        {
            return (_context.TriggeredAlarmNotifications?.Any(e => e.TriggeredAlarmNotificationId == id)).GetValueOrDefault();
        }

        public class TriggeredAlarmNotificationResult
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string NotificationEmailAddress { get; set; }
            public string Status { get; set; }
            public DateTime? SendDate { get; set; }
            public string? SendStatusMessage { get; set; }
            public string? MessageBody { get; set; }
        }

        private static string GetStatus(int id)
        {
            var status = "";
            switch (id)
            {
                case 1:
                    status = "Not Sent";
                    break;
                case 2:
                    status = "Sent Successfully";
                    break;
                case 3:
                    status = "Error";
                    break;
            }
            return status;
        }
    }
}