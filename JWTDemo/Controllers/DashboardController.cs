using JWTDemo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JWTDemo.Controllers;

[Authorize]
public sealed class DashboardController : Controller
{
    [Authorize(Roles = "Admin")]
    public IActionResult Employees()
    {
        var employees = new List<EmployeeViewModel>
        {
            new("Alex Johnson", 34, "Male", "alex.johnson@example.com"),
            new("Priya Shah", 29, "Female", "priya.shah@example.com"),
            new("Jordan Lee", 41, "Non-binary", "jordan.lee@example.com"),
            new("Maria Garcia", 37, "Female", "maria.garcia@example.com"),
            new("David Wilson", 26, "Male", "david.wilson@example.com")
        };

        return View(employees);
    }

    [Authorize(Roles = "Admin,Manager")]
    public IActionResult LeaveHistory()
    {
        var leaveHistory = new List<LeaveHistoryViewModel>
        {
            new("Alex Johnson", "Annual", "2026-01-12", "2026-01-14"),
            new("Priya Shah", "Sick", "2026-02-03", "2026-02-03"),
            new("Jordan Lee", "Annual", "2026-02-16", "2026-02-20"),
            new("Maria Garcia", "Personal", "2026-03-05", "2026-03-06"),
            new("David Wilson", "Annual", "2026-03-23", "2026-03-24")
        };

        return View(leaveHistory);
    }

    [Authorize(Roles = "Admin,Manager,Super User")]
    public IActionResult PurchaseHistory()
    {
        var purchases = new List<PurchaseHistoryViewModel>
        {
            new("Alex Johnson", "Monitor", 289.99m, "2026-01-08"),
            new("Priya Shah", "Keyboard", 79.50m, "2026-01-19"),
            new("Jordan Lee", "Office Chair", 349.00m, "2026-02-11"),
            new("Maria Garcia", "Headset", 124.75m, "2026-02-27"),
            new("David Wilson", "Webcam", 96.25m, "2026-03-12")
        };

        return View(purchases);
    }

    [Authorize(Roles = "Admin,Manager,Super User,Minor User")]
    public IActionResult ProjectHistory()
    {
        var projects = new List<ProjectHistoryViewModel>
        {
            new("Alex Johnson", "Customer Portal", "Lead Developer", "2025-11-28"),
            new("Priya Shah", "Mobile Reporting", "Business Analyst", "2025-12-12"),
            new("Jordan Lee", "Cloud Migration", "Architect", "2026-01-30"),
            new("Maria Garcia", "Identity Refresh", "QA Lead", "2026-02-20"),
            new("David Wilson", "Internal Helpdesk", "Developer", "2026-03-18")
        };

        return View(projects);
    }
}
