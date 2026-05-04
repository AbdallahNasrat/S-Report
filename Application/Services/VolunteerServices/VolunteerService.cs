using Application.Constants;
using Application.DTOs;
using Application.DTOs.NotficationDTOs;
using Application.DTOs.VolunteerDTOs;
using Application.Services.NotificationServices;
using Application.Services.ReportServices;
using Domain.Entites;
using Domain.Enums;
using Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.SignalR;

using SReport.Hubs;

public class VolunteerService : IVolunteerService
{
    private readonly IUnitOfWork _uow;
    private readonly IHubContext<ReportHub> _hubContext;
    private readonly IReportService _reportService;

    public VolunteerService(IUnitOfWork uow, IHubContext<ReportHub> hubContext ,IReportService reportService)
    {
        _uow = uow;
        _hubContext = hubContext;
        _reportService = reportService;
    }

    // ١. جلب البلاغات القريبة (الفلترة بالـ 3 كيلو)
    public async Task<List<NearbyMissionResponseDto>> GetNearbyMissions(int cityId, decimal userLat, decimal userLng)
    {
        var availableReports = await _uow.ReportsRepo.GetAllAsync(r =>
            r.CityId == cityId && r.State ==ReportStatus.Pending);

        var nearbyReports = new List<NearbyMissionResponseDto>();

        foreach (var report in availableReports)
        {
            decimal distance = CalculateDistance(userLat, userLng, report.Latitude, report.Longitude);

            // لو المسافة أقل من أو تساوي 3000 متر (3 كيلو)
            if (distance <= 3000)
            {
                // بنعمل Mapping للـ DTO
                nearbyReports.Add(new NearbyMissionResponseDto
                {
                    ReportId = report.Id,
                    // تأكد إن اسم الخاصية Description موجود في كلاس Report عندك أو بدلها بالاسم الصح
                    Description = report.Description,
                    Latitude = report.Latitude,
                    Longitude = report.Longitude,
                    DistanceInMeters = Math.Round(distance, 2), // قربناها لرقمين عشريين عشان الشكل
                    Status = report.State,
                    CreatedAt = report.Date // لو عندك الخاصية دي في الـ Report
                });
            }
        }

        // بنرجع اللستة مترتبة تصاعدياً من الأقرب للأبعد! 🚀
        return nearbyReports.OrderBy(r => r.DistanceInMeters).ToList();
    }

    // ٢. قبول البلاغ
    public async Task<bool> AcceptMission(int reportId, int volunteerId)
    {   
        var mission = new ReportVolunteer
        {
            ReportId = reportId,
            VolunteerId = volunteerId,
            Status = VolunteerMissionStatus.Joined,
            JoinedAt = DateTime.Now
        };
        _uow.ReportVolunteersRepo.Add(mission);

        await _uow.SaveChangesAsync();

        var isStatusUpdated = await _reportService.UpdateReportStatusAsync(reportId, ReportStatus.InProgress);
        return isStatusUpdated;
    }

    // ٣. إنهاء البلاغ وتوزيع النقاط
    public async Task<bool> CompleteMission(int reportId, int volunteerId)
    {
        // 1. نحدث حالة المهمة لـ "مكتملة"
        var mission = await _uow.ReportVolunteersRepo.GetFirstOrDefaultAsync(m =>
            m.ReportId == reportId && m.VolunteerId == volunteerId);

        if (mission == null) return false;

        mission.Status = VolunteerMissionStatus.Completed;
        mission.CompletedAt = DateTime.Now;

        // 2. نزود النقاط للمتطوع (مثلاً 3 نقطة)
        var user = await _uow.UsersRepo.GetByIdAsync(volunteerId);
        user.TotalPoints += 3;

        // 3. نسجل العملية في الـ PointsLog
        _uow.PointsLogsRepo.Add(new PointsLog
        {
            UserId = volunteerId,
            Points = 3,
            CreatedAt = DateTime.Now
        });

        await CheckAndAssignAchievements(volunteerId);


        await _uow.SaveChangesAsync();

        var isStatusUpdated = await _reportService.UpdateReportStatusAsync(reportId, ReportStatus.Resolved);

        return isStatusUpdated;
    }

    // ==========================================
    // دالة مساعدة لحساب المسافة (Haversine Formula)
    // ==========================================
    private decimal CalculateDistance(decimal lat1, decimal lon1, decimal lat2, decimal lon2)
    {
        var R = 6371e3; // نصف قطر الأرض بالمتر
        var phi1 = (double)lat1 * Math.PI / 180;
        var phi2 = (double)lat2 * Math.PI / 180;
        var dPhi = (double)(lat2 - lat1) * Math.PI / 180;
        var dLambda = (double)(lon2 - lon1) * Math.PI / 180;

        var a = Math.Sin(dPhi / 2) * Math.Sin(dPhi / 2) +
                Math.Cos(phi1) * Math.Cos(phi2) *
                Math.Sin(dLambda / 2) * Math.Sin(dLambda / 2);

        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

        return (decimal)(R * c); // بترجع المسافة بالمتر
    }

    // ٤. جلب قائمة أفضل المتطوعين (Leaderboard)
    public async Task<List<LeaderboardDto>> GetTopVolunteers()
    {
        // بنجيب كل اليوزرز اللي الرول بتاعهم متطوع (تأكد من اسم الرول عندك)
        var volunteers = await _uow.UsersRepo.GetAllAsync(u => u.RoleId == AppRoles.User && u.Volunteer == true);

        // بنرتبهم تنازلياً حسب النقاط وناخد أعلى 10 بس
        var topVolunteers = volunteers
            .OrderByDescending(u => u.TotalPoints)
            .Take(30)
            .Select(u => new LeaderboardDto
            {
                Name = u.FName + " " + u.SName,
                Avatar = u.Avatar,
                TotalPoints = u.TotalPoints
            }).ToList();

        return topVolunteers;
    }

    // ٥. جلب بروفايل المتطوع والإحصائيات بتاعته
    public async Task<VolunteerProfileDto> GetVolunteerProfile(int volunteerId)
    {
        var user = await _uow.UsersRepo.GetByIdAsync(volunteerId);

        // بنعد البلاغات اللي المتطوع ده خلصها بنجاح من الجدول الوسيط
        var completedMissions = await _uow.ReportVolunteersRepo.GetAllAsync(m =>
            m.VolunteerId == volunteerId &&
            m.Status == VolunteerMissionStatus.Completed);

        var userAchievements = await _uow.UserAchievementsRepo.GetAllAsync(u => u.UserId == volunteerId, "Achievement");
        var earnedNames = userAchievements.Select(u => u.Achievement.Name).ToList();

        return new VolunteerProfileDto
        {
            Name = user.FName,
            Avatar = user.Avatar,
            TotalPoints = user.TotalPoints,
            CompletedMissionsCount = completedMissions.Count(),
            EarnedAchievements=earnedNames
        };
    }

    public async Task CheckAndAssignAchievements(int volunteerId)
    {
        // 1. نجيب عدد البلاغات اللي المتطوع ده خلصها
        // لاحظ: بنستخدم GetWhereAsync أو بنجيب اللستة ونعدها بناءً على المتاح في الـ Repo عندك
        var completedMissions = await _uow.ReportVolunteersRepo.GetWhereAsync(m =>
            m.VolunteerId == volunteerId && m.Status == VolunteerMissionStatus.Completed);

        int completedCount = completedMissions.Count();

        // 2. نحدد رقم الوسام (AchievementId) بناءً على العدد
        int? achievementIdToAward = completedCount switch
        {
            1 => 1,    // First Mission
            10 => 2,   // Helper
            25 => 3,   // Guardian
            50 => 4,   // Hero
            100 => 5,  // Legend
            _ => null  // لو الرقم مش مطابق لدول، مفيش وسام جديد
        };

        // 3. لو يستحق وسام، نتأكد إنه مش معاه قبل كده ونديهوله
        if (achievementIdToAward.HasValue)
        {
            var existingBadges = await _uow.UserAchievementsRepo.GetWhereAsync(ua =>
                ua.UserId == volunteerId && ua.AchievementId == achievementIdToAward.Value);

            if (!existingBadges.Any())
            {
                var newBadge = new UserAchievement
                {
                    UserId = volunteerId,
                    AchievementId = achievementIdToAward.Value,
                    EarnedAt = DateTime.Now
                };

                _uow.UserAchievementsRepo.Add(newBadge);
                // مش هنعمل SaveChangesAsync هنا، لأننا هنخلي الميثود الأصلية هي اللي تسيف
            }
        }
    }

    public async Task<bool> CancelMission(int reportId, int volunteerId)
    {
        
        var volunteerMission = await _uow.ReportVolunteersRepo.GetFirstOrDefaultAsync(m =>
            m.ReportId == reportId &&
            m.VolunteerId == volunteerId &&
            m.Status == VolunteerMissionStatus.Joined); // تأكد إن الـ Enum عندك اسمه كده

        if (volunteerMission == null) return false;

        // 2. نجيب البلاغ نفسه
        var report = await _uow.ReportsRepo.GetByIdAsync(reportId);
        if (report == null) return false;

        // 3. نغير حالة سجل التطوع لـ "Cancelled" أو نمسحه (تغيير الحالة أفضل للـ History)
        volunteerMission.Status = VolunteerMissionStatus.Cancelled;

        // 4. نرجع البلاغ لحالته الأولى عشان يظهر لمتطوعين تانيين
        report.State = ReportStatus.Pending;

        // 5. حفظ التعديلات
        var result = await _uow.SaveChangesAsync() > 0;

        if (result)
        {
            
             await _hubContext.Clients.Group(report.CityId.ToString()).SendAsync("RefreshReports");
        }

        return result;
    }

    public async Task<CurrentMissionDto?> GetCurrentMission(int volunteerId)
    {
        
        var activeMission = await _uow.ReportVolunteersRepo.GetFirstOrDefaultAsync(
             m => m.VolunteerId == volunteerId && m.Status == VolunteerMissionStatus.Joined,false,"Report,Report.City");

        if (activeMission == null || activeMission.Report == null) return null;

        return new CurrentMissionDto
        {
            ReportId = activeMission.ReportId,
            Description = activeMission.Report.Description,
            Latitude = activeMission.Report.Latitude,
            Longitude = activeMission.Report.Longitude,
            CityName = activeMission.Report.City?.Name ?? "Undefined",
            AcceptedAt = activeMission.JoinedAt 
        };
    }

    public async Task<IEnumerable<MissionHistoryDto>> GetVolunteerHistory(int volunteerId)
    {
        var completedMissions = await _uow.ReportVolunteersRepo.GetAllAsync(
            filter: m => m.VolunteerId == volunteerId && m.Status == VolunteerMissionStatus.Completed,
            properties: "Report");

        return completedMissions
            .OrderByDescending(m => m.CompletedAt) // ترتيب من الأحدث للأقدم
            .Select(m => new MissionHistoryDto
            {
                ReportId = m.ReportId,
                Description = m.Report.Description ?? "Without description",
                CompletedAt = m.CompletedAt ?? DateTime.Now,
                EarnedPoints = 3 
            }).ToList();
    }

}