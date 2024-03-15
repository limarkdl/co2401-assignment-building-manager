namespace SmartBuilding.Managers
{
    public class FireAlarmManager : Manager
    {
        public FireAlarmManager() {
            type = "FireAlarm,";
            status = "OK,OK,OK,OK,OK,OK,OK,OK,";
        }

        public bool SetAlarm(bool isActive)
        {
            status = "OK,OK,OK,OK,OK,OK,OK,OK,";
            return true;
        }
    }
}
