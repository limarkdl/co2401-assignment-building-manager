namespace SmartBuilding.Managers
{
    public abstract class Manager
    {
        protected string status;
        protected string type;

        public virtual string GetStatus()
        {
            return this.type + this.status;
        }
    }
}
