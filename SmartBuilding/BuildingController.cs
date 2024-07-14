using SmartBuilding.Managers;
using SmartBuilding.Services;

namespace SmartBuilding;

public class BuildingController
{
    private readonly IDoorManager doorManager;
    private readonly EmailService emailService;
    private readonly List<string> emergencyStates = new() { "fire drill", "fire alarm" };
    private readonly IFireAlarmManager fireAlarmManager;

    private readonly ILightManager lightManager;

    private readonly List<string> normalStates = new() { "closed", "out of hours", "open" };
    private readonly WebService webService;

    private string buildingID;
    private string currentState;
    private string lastNormalState;


    public BuildingController(string id)
    {
        if (id == null) throw new ArgumentNullException(nameof(id), "ID cannot be null.");

        SetBuildingID(id.ToLower());
        currentState = "out of hours";
        lastNormalState = "out of hours";
    }

    public BuildingController(string id, string startState)
    {
        if (startState == null)
            throw new ArgumentException(
                "Argument Exception: BuildingController can only be initialised to the following states 'open', 'closed', 'out of hours'");
        SetBuildingID(id);

        startState = startState.ToLower();
        if (normalStates.Contains(startState))
        {
            currentState = startState;
            lastNormalState = startState;
        }
        else
        {
            throw new ArgumentException(
                "Argument Exception: BuildingController can only be initialised to the following states 'open', 'closed', 'out of hours'");
        }
    }

    public BuildingController(
        string id,
        ILightManager iLightManager,
        IFireAlarmManager iFireAlarmManager,
        IDoorManager iDoorManager,
        WebService iWebService,
        EmailService iEmailService)
    {
        SetBuildingID(id);
        currentState = "out of hours";
        lastNormalState = "out of hours";

        lightManager = iLightManager;
        fireAlarmManager = iFireAlarmManager;
        doorManager = iDoorManager;
        webService = iWebService;
        emailService = iEmailService;
    }

    public string GetCurrentState()
    {
        return currentState;
    }


    public string GetBuildingID()
    {
        return buildingID;
    }

    public void SetBuildingID(string id)
    {
        if (id is null) throw new ArgumentNullException(nameof(id), "ID cannot be null.");

        buildingID = id.ToLower();
    }

    public bool SetCurrentState(string state)
    {
        state = state.ToLower();

        if (state == currentState) return true;


        if (state == "out of hours" && (currentState == "fire drill" || currentState == "fire alarm"))
        {
            currentState = lastNormalState;
            return true;
        }

        if (state == "fire alarm")
            if (webService != null && lightManager != null && doorManager != null)
            {
                lightManager.SetAllLights(true);
                doorManager.OpenAllDoors();
                try
                {
                    webService.LogFireAlarm("fire alarm");
                }
                catch (Exception ex)
                {
                    emailService.SendEmail("smartbuilding@uclan.ac.uk", "failed to log alarm", ex.Message);
                }

                return true;
            }

        if (state == "close")
            if (doorManager != null && lightManager != null)
            {
                doorManager.LockAllDoors();
                lightManager.SetAllLights(false);
                return true;
            }

        if (state == "open")
        {
            if (doorManager != null)
            {
                var doorsOpened = doorManager.OpenAllDoors();
                if (!doorsOpened) return false;
            }

            currentState = state;
            lastNormalState = state;
            return true;
        }

        if (normalStates.Contains(currentState) && normalStates.Contains(state))
        {
            currentState = state;
            lastNormalState = state;
            return true;
        }

        if (emergencyStates.Contains(state))
        {
            currentState = state;
            return true;
        }

        return false;
    }

    public string GetStatusReport()
    {
        var lightStatus = lightManager.GetStatus();
        var doorStatus = doorManager.GetStatus();
        var fireAlarmStatus = fireAlarmManager.GetStatus();
        var faultsListToSendEngineer = _detectErrorsAndCreateLogDetails(lightStatus, doorStatus, fireAlarmStatus);


        if (faultsListToSendEngineer.Length > 0) webService.LogEngineerRequired(faultsListToSendEngineer);

        return $"{lightStatus}{doorStatus}{fireAlarmStatus}";
    }


    // Had to make this public to test. I couldn't figure out how to use 'Arg.is' on Received, result of internal functoin couldn't be tracked in my case
    public string _detectErrorsAndCreateLogDetails(string LightStatus, string DoorStatus, string FireStatus)
    {
        var faultsDetected = new List<string>();


        if (LightStatus.Contains("FAULT")) faultsDetected.Add("Lights");

        if (DoorStatus.Contains("FAULT")) faultsDetected.Add("Doors");

        if (FireStatus.Contains("FAULT")) faultsDetected.Add("FireAlarm");

        var result = string.Join(",", faultsDetected) + ",";

        if (result.Length <= 1) return "";

        return result;
    }
}