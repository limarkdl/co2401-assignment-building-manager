namespace SmartBuilding.Managers
{
    public abstract class Manager
    {
        private string status;

        public string GetStatus()
        {
            return this.status;
        }
    }
}
