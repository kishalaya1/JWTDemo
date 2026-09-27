namespace JWTDemo.Models;

public sealed record EmployeeViewModel(string Name, int Age, string Gender, string Email);
public sealed record LeaveHistoryViewModel(string EmployeeName, string LeaveType, string FromDate, string ToDate);
public sealed record PurchaseHistoryViewModel(string EmployeeName, string Item, decimal Amount, string PurchaseDate);
public sealed record ProjectHistoryViewModel(string EmployeeName, string ProjectName, string Role, string CompletedDate);
