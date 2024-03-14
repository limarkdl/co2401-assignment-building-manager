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
    private readonly IWebService webService;
    private readonly IEmailService emailService;

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
        IWebService iWebService,
        IEmailService iEmailService)
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

    public bool SetCurrentState(string state)
    {
        state = state.ToLower();

        if (state == this.currentState)
        {
            return true;
        }

        if (emergencyStates.Contains(this.currentState) && state == "out of hours")
        {
            state = this.lastNormalState;
        }

        if (normalStates.Contains(state) || emergencyStates.Contains(state))
        {
            if (normalStates.Contains(state))
            {
                this.lastNormalState = state;
            }

            this.currentState = state;
            return true;
        }

        return false;
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


        public string GetStatusReport()
    {
        // TODO: 'Implement this method'
        return "Hi";
    }
    

    

    

}
}
