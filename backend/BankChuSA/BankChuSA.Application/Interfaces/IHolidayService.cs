namespace BankChuSA.Application.Interfaces
{
    public interface IHolidayService
    {
        Task<bool> IsWorkingDayAsync(DateTime date);
        Task<List<DateTime>> GetHolidaysAsync(int year);
    }
}

