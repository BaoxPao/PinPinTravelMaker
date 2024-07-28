﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PinPinServer.Models;
using PinPinServer.Models.DTO;
using PinPinServer.Services;


namespace PinPinServer.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SchedulesController : ControllerBase
    {
        PinPinContext _context;
        AuthGetuserId _getUserId;
        public SchedulesController(PinPinContext context, AuthGetuserId getuserId)
        {
            _context = context;
            _getUserId = getuserId;

        }

        // GET: api/Schedules/MainSchedules
        [HttpGet("MainSchedules")]
        public async Task<IActionResult> GetUserMainSchedule()
        {
            IEnumerable<ScheduleDTO> schedules = Enumerable.Empty<ScheduleDTO>();
            try
            {
                int userID = _getUserId.PinGetUserId(User).Value;
                schedules = await _context.Schedules
                .Where(s => s.UserId == userID)
                .Include(s => s.User)
                .Include(s => s.ScheduleGroups)
                .ThenInclude(sg => sg.User)
                .Select(s => new ScheduleDTO
                {
                    Id = s.Id,
                    UserId = s.UserId,
                    Name = s.Name,
                    StartTime = s.StartTime,
                    EndTime = s.EndTime,
                    CreatedAt = s.CreatedAt,
                    UserName = s.User.Name,
                    SharedUserIDs = s.ScheduleGroups.Select(s => (int?)s.UserId).ToList(),
                    SharedUserNames = s.ScheduleGroups.Select(sg => (string?)sg.User.Name).Distinct().ToList(),
                    lng = s.Lng,
                    lat = s.Lat,
                }).ToListAsync();

                if (schedules == null || !schedules.Any())
                {

                    Console.WriteLine("查無使用者相關紀錄");
                    return NotFound(new { message = "未找到匹配的行程" });
                }

                return Ok(schedules);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex}");
                throw new Exception("伺服器發生錯誤，請稍後再試");
            }

        }


        // GET: api/Schedules/SchedulesGroup
        [HttpGet("SchedulesGroup")]
        public async Task<IActionResult> GetUserSchedulesGroup()
        {
            List<int> scheduleIds = new List<int>();
            List<ScheduleDTO> gschedules = new List<ScheduleDTO>();
            try
            {
                int userID = _getUserId.PinGetUserId(User).Value;

                scheduleIds = await _context.ScheduleGroups
                    .Where(sg => sg.UserId == userID && sg.IsHoster == false)
                    .Select(sg => sg.ScheduleId)
                    .Distinct()
                    .ToListAsync();

                gschedules = await _context.Schedules
                .Where(s => scheduleIds.Contains(s.Id))
                .Include(s => s.User)
                .Include(s => s.ScheduleGroups)
                .ThenInclude(sg => sg.User)
                .Select(s => new ScheduleDTO
                {
                    Id = s.Id,
                    Name = s.Name,
                    StartTime = s.StartTime,
                    EndTime = s.EndTime,
                    CreatedAt = s.CreatedAt,
                    UserName = s.User.Name,
                    SharedUserIDs = s.ScheduleGroups.Select(s => (int?)s.UserId).ToList(),
                    SharedUserNames = s.ScheduleGroups
                        .Where(sg => sg.UserId != userID)
                        .Select(sg => (string?)sg.User.Name)
                        .Distinct()
                        .ToList(),
                    lng = s.Lng,
                    lat = s.Lat,
                })
                .ToListAsync();

                if (gschedules == null || !gschedules.Any())
                {
                    Console.WriteLine("查無使用者相關紀錄");
                    return NotFound(new { message = "未找到匹配的行程" });
                }

                return Ok(gschedules);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return StatusCode(500, "伺服器發生錯誤，請稍後再試");
            }
        }



        //Get:api/Schedules/{name}
        [HttpGet("{name}")]
        public async Task<ActionResult<IEnumerable<ScheduleDTO>>> GetUserSpecifiedSch(string name)
        {
            IEnumerable<ScheduleDTO> schedules = Enumerable.Empty<ScheduleDTO>();
            try
            {
                int userID = _getUserId.PinGetUserId(User).Value;
                schedules = await _context.Schedules
                    .Where(s => s.UserId == userID && s.Name.Contains(name))
                    .Join(
                        _context.Users,
                        sch => sch.UserId,
                        usr => usr.Id,
                        (sch, usr) => new ScheduleDTO
                        {
                            Id = sch.Id,
                            UserId = sch.UserId,
                            Name = sch.Name,
                            StartTime = sch.StartTime,
                            EndTime = sch.EndTime,
                            CreatedAt = sch.CreatedAt,
                            UserName = usr.Name,
                            SharedUserNames = sch.ScheduleGroups.Select(sg => (string?)sg.User.Name).Distinct().ToList(),
                            lng = sch.Lng,
                            lat = sch.Lat,
                        }).ToListAsync();

                if (schedules == null || !schedules.Any())
                {
                    return NotFound();
                }
                return Ok(schedules);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"處理請求時發生錯誤: {ex.Message}");
                return BadRequest();
            }
        }



        // PUT: api/Schedules/{id}
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSchedule(int id, ScheduleDTO schDTO)
        {

            int userID = _getUserId.PinGetUserId(User).Value;
            Schedule? sch = await _context.Schedules.FindAsync(id);

            if (sch == null)
            {
                return NotFound();
            }

            sch.Id = schDTO.Id;
            sch.Name = schDTO.Name;
            sch.StartTime = schDTO.StartTime;
            sch.EndTime = schDTO.EndTime;
            sch.UserId = userID;
            sch.CreatedAt = schDTO.CreatedAt;
            _context.Entry(sch).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return Ok("行程修改成功!");
            }
            //這段是啥?
            catch (DbUpdateConcurrencyException)
            {
                if (!ScheduleExists(id))
                {
                    return BadRequest("系統發生錯誤");
                }
                throw;
            }
        }

        // POST: api/Schedules
        [HttpPost]
        public async Task<IActionResult> PostSchedule([FromBody] EditScheduleDTO editschDTO)
        {
            if (editschDTO == null)
            {
                return NotFound();
            }
            int userID = _getUserId.PinGetUserId(User).Value;
            var schedule = new Schedule
            {
                Id = 0,
                Name = editschDTO.Name,
                StartTime = editschDTO.StartTime,
                EndTime = editschDTO.EndTime,
                CreatedAt = DateTime.Now,
                UserId = userID,
                Lng = editschDTO.lng,
                Lat = editschDTO.lat,
            };
            _context.Schedules.Add(schedule);
            await _context.SaveChangesAsync();

            int newScheduleId = schedule.Id;
            var schedulegroup = new ScheduleGroup
            {
                Id = 0,
                ScheduleId = newScheduleId,
                UserId = userID,
                IsHoster = true,
            };
            _context.ScheduleGroups.Add(schedulegroup);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // DELETE: api/Schedules/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSchedule(int id)
        {
            var schedule = await _context.Schedules.FindAsync(id);


            if (schedule == null)
            {
                return NotFound();
            }
            try
            {
                _context.Schedules.Remove(schedule);
                await _context.SaveChangesAsync();
                return Ok("行程刪除");
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Exception occurred: {ex.Message}");
                return StatusCode(500, "刪除失敗!"); // 500 Internal Server Error if deletion fails
            }
        }

        private bool ScheduleExists(int id)
        {
            return _context.Schedules.Any(e => e.Id == id);
        }

        //GET:api/schedules/GetRelatedSchedules
        //GET資料回傳為Dictionary<ScheduleId,ScheduleName>
        [HttpGet("GetRelatedSchedules")]
        public async Task<ActionResult<Dictionary<int, string>>> GetRelatedSchedules()
        {
            int userID = _getUserId.PinGetUserId(User).Value;
            try
            {
                Dictionary<int, string> scheduleDictionary = await _context.ScheduleGroups
                    .Where(group => group.UserId == userID)
                    .Include(group => group.Schedule)
                    .ToDictionaryAsync(group => group.ScheduleId, group => group.Schedule.Name);

                if (scheduleDictionary.Count == 0) return NotFound("Not found about your schedles");

                return Ok(scheduleDictionary);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
    }
}
