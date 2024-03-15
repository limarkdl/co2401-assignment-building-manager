namespace SmartBuilding.Managers
{

    public class DoorManager : Manager
    {
        public DoorManager() {
            type = "Doors,";
            status = "OK,OK,OK,FAULT,OK,FAULT,OK,FAULT,";
        }
        

        public bool OpenDoor(int doorID)
        {
            status = "OK,OK,OK,OK,OK,FAULT,OK,FAULT,";
            return true;
        }

        public bool LockDoor(int doorID)
        {
            status = "OK,OK,OK,FAULT,OK,FAULT,OK,FAULT,";
            return true;
        }

        public bool OpenAllDoors()
        {
            status = "OK,OK,OK,OK,OK,OK,OK,OK,";
            return true;
        }

        public bool LockAllDoors()
        {
            status = "FAULT,FAULT,FAULT,FAULT,FAULT,FAULT,FAULT,FAULT,";
            return true;
        }
    }

}
