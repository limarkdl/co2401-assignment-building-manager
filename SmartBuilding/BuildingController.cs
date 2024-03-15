using SmartBuilding.Managers;
using SmartBuilding.Services;


namespace SmartBuilding
{

public class BuildingController
{

    private string buildingID;
    private string currentState;
    private string lastNormalState;

    private readonly LightManager lightManager;
    private readonly FireAlarmManager fireAlarmManager;
    private readonly DoorManager doorManager;
    private readonly WebService webService;
    private readonly EmailService emailService;

    private readonly List<string> normalStates = new List<string> { "closed", "out of hours", "open" };
    private readonly List<string> emergencyStates = new List<string> { "fire drill", "fire alarm" };



    public BuildingController(string id)
    {
        if (id == null)
        {
            throw new ArgumentNullException(nameof(id), "ID cannot be null.");
        }

        SetBuildingID(id.ToLower());
        this.currentState = "out of hours";
        this.lastNormalState = "out of hours";
    }

    public BuildingController(string id, string startState)
    {
        if (startState == null)
        {
                throw new ArgumentException("Argument Exception: BuildingController can only be initialised to the following states 'open', 'closed', 'out of hours'");
        }
        SetBuildingID(id);
        
        startState = startState.ToLower();
        if (normalStates.Contains(startState))
        {
            this.currentState = startState;
            this.lastNormalState = startState; 
        }
        else
        {
            throw new ArgumentException("Argument Exception: BuildingController can only be initialised to the following states 'open', 'closed', 'out of hours'");
        }
    }

    public BuildingController(
        string id,
        LightManager iLightManager,
        FireAlarmManager iFireAlarmManager,
        DoorManager iDoorManager,
        WebService iWebService,
        EmailService iEmailService)
    {
        SetBuildingID(id);
        this.currentState = "out of hours";
        this.lastNormalState = "out of hours";

        this.lightManager = iLightManager;
        this.fireAlarmManager = iFireAlarmManager;
        this.doorManager = iDoorManager;
        this.webService = iWebService;
        this.emailService = iEmailService;
    }

    public string GetCurrentState()
    {
        return this.currentState;
    }

       

    public string GetBuildingID()
    {
        return this.buildingID;
    }

    public void SetBuildingID(string id)
    {
        if (id is null)
        {
            throw new ArgumentNullException(nameof(id), "ID cannot be null.");
        }

        this.buildingID = id.ToLower();
    }

    public bool SetCurrentState(string state)
    {
        state = state.ToLower();

        if (state == this.currentState)
        {
            return true;
        }


        if ((state == "out of hours" && (this.currentState == "fire drill" || this.currentState == "fire alarm")))
        {
            this.currentState = this.lastNormalState;
            return true;
        }

        if (state == "open")
        {
            if (this.doorManager != null)
                {
                bool doorsOpened = this.doorManager.OpenAllDoors();
                if (!doorsOpened)
                {
                    return false;
                }
            }
            

            this.currentState = state;
            this.lastNormalState = state;
            return true;
        }

            if (normalStates.Contains(this.currentState) && normalStates.Contains(state))
        {
            this.currentState = state;
            this.lastNormalState = state;
            return true;
        }

        if (emergencyStates.Contains(state))
        {
            this.currentState = state;
            return true;
        }

        return false;
    }

        public string GetStatusReport()
        {
            var lightStatus = lightManager.GetStatus();
            var doorStatus = doorManager.GetStatus();
            var fireAlarmStatus = fireAlarmManager.GetStatus();

            return $"{lightStatus}{doorStatus}{fireAlarmStatus}";
        }





    }
}
