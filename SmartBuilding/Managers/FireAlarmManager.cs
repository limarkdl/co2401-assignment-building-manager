namespace SmartBuilding.Managers;

public interface IFireAlarmManager : IManager
{
    public bool SetAlarm(bool isActive);

}