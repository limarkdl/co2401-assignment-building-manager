namespace SmartBuilding.Managers
{
    public class LightManager : Manager
    {
        
        public LightManager()
        {
            type = "Lights,";
            status = "OK,OK,FAULT,OK,OK,OK,OK,OK,OK,OK,";
        }

        public void SetLight(bool isOn, int lightID)
        {
            status = isOn ? "OK," : "FAULT,";
        }

        public void SetAllLights(bool isOn)
        {
            status = isOn ? "OK,OK,OK,OK,OK,OK,OK,OK,OK,OK," : "FAULT,FAULT,FAULT,FAULT,FAULT,FAULT,FAULT,FAULT,FAULT,FAULT,";
        }
    }
}