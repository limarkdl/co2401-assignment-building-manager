using NUnit.Framework;
using SmartBuilding;

using NSubstitute;
using SmartBuilding.Managers;
using SmartBuilding.Services;

namespace L1
{

    [TestFixture]
    public class L1R1
    {
        [TestCase("b123")]
        [TestCase("a456")]
        [TestCase("c789")]
        public void Constructor_WithValidID_AssignsBuildingID(string expectedID)
        {
            // Act
            var buildingController = new BuildingController(expectedID);

            // Assert
            Assert.AreEqual(expectedID, buildingController.GetBuildingID(), "The constructor should assign buildingID correctly.");
        }

        [TestCase]
        public void Constructor_WithNullID_ThrowsArgumentNullException()
        {
            // Arrange
            string nullID = null;

            // Act & Assert
            var ex = Assert.Throws<System.ArgumentNullException>(() => new BuildingController(nullID));
            Assert.That(ex.ParamName, Is.EqualTo("id"), "Constructor should throw ArgumentNullException for null ID");
        }
    }

    [TestFixture]
    public class L1R2
    {
        [Test]
        public void GetBuildingID_WhenSet_ReturnsCorrectID()
        {
            // Arrange
            var expectedID = "b123";
            var buildingController = new BuildingController(expectedID);

            // Act
            var actualID = buildingController.GetBuildingID();

            // Assert
            Assert.AreEqual(expectedID, actualID, "The building ID should be the same as the one set in the constructor.");
        }

        [Test]
        public void GetBuildingID_WhenSetWithLongID_ReturnsCorrectID()
        {
            // Arrange
            var expectedID = new string('a', 10000);
            var buildingController = new BuildingController(expectedID);

            // Act
            var actualID = buildingController.GetBuildingID();

            // Assert
            Assert.AreEqual(expectedID, actualID, "The building ID should be able to handle very long IDs.");
        }

        [Test]
        public void GetBuildingID_WhenSetWithEmptyString_ReturnsEmptyString()
        {
            // Arrange
            var expectedID = string.Empty; // Empty ID
            var buildingController = new BuildingController(expectedID);

            // Act
            var actualID = buildingController.GetBuildingID();

            // Assert
            Assert.AreEqual(expectedID, actualID, "The building ID should return an empty string if it was set to an empty string.");
        }

        [Test]
        public void GetBuildingID_WhenCalledMultipleTimes_ReturnsSameValue()
        {
            // Arrange
            var expectedID = "B123";
            var buildingController = new BuildingController(expectedID);

            // Act
            var firstCallID = buildingController.GetBuildingID();
            var secondCallID = buildingController.GetBuildingID();

            // Assert
            Assert.AreEqual(firstCallID, secondCallID, "The building ID should be immutable and return the same value on multiple calls.");
        }
    }

    [TestFixture]
    public class L1R3
    {
        [TestCase("TESTID")]
        [TestCase("testid")]
        [TestCase("TestID")]
        [TestCase("tEsTiD")]
        public void Constructor_WithValidID_SetsBuildingIDToLowercase(string id)
        {
            // Arrange
            var id_to_pass = id;

            // Act
            var buildingController = new BuildingController(id_to_pass);
            var resultID = buildingController.GetBuildingID();

            // Assert
            Assert.AreEqual(id_to_pass.ToLower(), resultID, "Constructor should set buildingID to lowercase.");
        }

    }

    [TestFixture]
    public class L1R4
    {
        [TestCase("TESTID")]
        [TestCase("testid")]
        [TestCase("TestID")]
        [TestCase("tEsTiD")]
        public void SetBuildingID_WithMixedCaseInput_SetsIDInLowerCase(string inputID)
        {
            // Arrange
            var buildingController = new BuildingController("InitialID");
            var expectedID = inputID.ToLower();

            // Act
            buildingController.SetBuildingID(inputID);
            var actualID = buildingController.GetBuildingID();

            // Assert
            Assert.AreEqual(expectedID, actualID, "SetBuildingID should set the building ID in lowercase.");
        }

        [Test]
        public void SetBuildingID_WithNullInput_ThrowsArgumentNullException()
        {
            // Arrange
            var buildingController = new BuildingController("InitialID");

            // Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() => buildingController.SetBuildingID(null));
            Assert.That(ex.ParamName, Is.EqualTo("id"), "SetBuildingID should throw ArgumentNullException for null input.");
        }

        [Test]
        public void SetBuildingID_WithEmptyString_SetsEmptyID()
        {
            // Arrange
            var buildingController = new BuildingController("InitialID");
            var expectedID = string.Empty;

            // Act
            buildingController.SetBuildingID(string.Empty);
            var actualID = buildingController.GetBuildingID();

            // Assert
            Assert.AreEqual(expectedID, actualID, "SetBuildingID should set the building ID to an empty string if an empty string is passed.");
        }
    }

    [TestFixture]
    public class L1R5
    {
        [Test]
        public void Constructor_InitializesCurrentStateToOutOfHours()
        {
            // Arrange
            var validID = "ValidID";

            // Act
            var buildingController = new BuildingController(validID);
            var currentState = buildingController.GetCurrentState();

            // Assert
            Assert.AreEqual("out of hours", currentState, "Constructor should set currentState to 'out of hours'.");
        }
    }

    [TestFixture]
    public class L1R6
    {
        [Test]
        public void GetCurrentState_ReturnsInitialState_OutOfHours()
        {
            // Arrange
            var buildingController = new BuildingController("B123");

            // Act
            var state = buildingController.GetCurrentState();

            // Assert
            Assert.AreEqual("out of hours", state, "The initial state should be 'out of hours'.");
        }

        [Test]
        public void GetCurrentState_AfterStateChange_ReturnsNewState()
        {
            // Arrange
            var buildingController = new BuildingController("B123");
            var expectedState = "open";
            buildingController.SetCurrentState(expectedState);

            // Act
            var state = buildingController.GetCurrentState();

            // Assert
            Assert.AreEqual(expectedState, state, "GetCurrentState should return the new state after a change.");
        }

    }

    [TestFixture]
    public class L1R7
    {
        private string initialID = "B123";
        private BuildingController buildingController;

        [SetUp]
        public void Setup()
        {
            buildingController = new BuildingController(initialID);
            // Assuming the initial state is "out of hours"
        }

        [TestCase("closed")]
        [TestCase("out of hours")]
        [TestCase("open")]
        [TestCase("fire drill")]
        [TestCase("fire alarm")]
        public void SetCurrentState_ValidStates_ReturnsTrueAndSetsState(string validState)
        {
            // Act
            var result = buildingController.SetCurrentState(validState);

            // Assert
            Assert.IsTrue(result, $"SetCurrentState should return true for valid state '{validState}'.");
            Assert.AreEqual(validState.ToLower(), buildingController.GetCurrentState(), $"currentState should be set to '{validState}'.");
        }

        [Test]
        public void SetCurrentState_InvalidState_ReturnsFalseAndStateUnchanged()
        {
            // Arrange
            string invalidState = "invalid";
            string initialState = buildingController.GetCurrentState();

            // Act
            var result = buildingController.SetCurrentState(invalidState);

            // Assert
            Assert.IsFalse(result, "SetCurrentState should return false for invalid state.");
            Assert.AreEqual(initialState, buildingController.GetCurrentState(), "currentState should remain unchanged with invalid state.");
        }

        [TestCase("closed", "fire alarm", "closed")] // Return to last normal state from emergency state
        [TestCase("open", "fire drill", "open")]     // Return to last normal state from emergency state
        public void SetCurrentState_FromEmergencyState_ReturnsToLastNormalState(string lastNormalState, string emergencyState, string expectedState)
        {
            // Arrange - Set initial state to a known normal state
            buildingController.SetCurrentState(lastNormalState);

            // Act - Set to emergency state, then to "out of hours" to trigger return to last normal state
            buildingController.SetCurrentState(emergencyState);
            buildingController.SetCurrentState("out of hours");

            // Assert
            var currentState = buildingController.GetCurrentState();
            Assert.AreEqual(expectedState.ToLower(), currentState, "Should return to the last normal state after an emergency.");
        }

        [Test]
        public void SetCurrentState_SameState_ReturnsTrueAndStateUnchanged()
        {
            // Arrange
            string initialState = "open";
            buildingController.SetCurrentState(initialState);

            // Act
            var result = buildingController.SetCurrentState(initialState);

            // Assert
            Assert.IsTrue(result, "SetCurrentState should return true when setting the same state.");
            Assert.AreEqual(initialState, buildingController.GetCurrentState(), "currentState should remain unchanged when setting the same state.");
        }
    }
}

namespace L2
{
    [TestFixture]
    public class L2R1
    {
        private BuildingController buildingController;
        private readonly string initialID = "B123";

        [SetUp]
        public void SetUp()
        {
            // Setup for each test, buildingController starts in the "out of hours" state
            buildingController = new BuildingController(initialID);
        }

        [TestCase("closed", "out of hours", true)]
        [TestCase("open", "out of hours", true)]
        [TestCase("out of hours", "open", true)]
        [TestCase("out of hours", "closed", true)]
        public void SetCurrentState_ValidNormalTransitions_ReturnsTrue(string initialState, string newState, bool expected)
        {
            // Arrange
            buildingController.SetCurrentState(initialState);

            // Act
            var result = buildingController.SetCurrentState(newState);

            // Assert
            Assert.AreEqual(expected, result, $"Transition from {initialState} to {newState} should be valid.");
        }

        [TestCase("out of hours", "fire drill", true)]
        [TestCase("open", "fire alarm", true)]
        [TestCase("closed", "fire drill", true)]
        public void SetCurrentState_NormalToEmergencyTransition_ReturnsTrue(string initialState, string newState, bool expected)
        {
            // Arrange
            buildingController.SetCurrentState(initialState);

            // Act
            var result = buildingController.SetCurrentState(newState);

            // Assert
            Assert.AreEqual(expected, result, $"Transition from {initialState} to emergency state {newState} should be valid.");
        }

        [TestCase("fire drill", "out of hours", true)]
        [TestCase("fire alarm", "out of hours", true)]
        public void SetCurrentState_EmergencyToLastNormalState_ReturnsTrue(string emergencyState, string expectedReturnState, bool expected)
        {
            // Arrange
            buildingController.SetCurrentState("open"); // Set to a known normal state
            buildingController.SetCurrentState(emergencyState); // Transition to an emergency state

            // Act
            var result = buildingController.SetCurrentState("out of hours"); // Should transition back to last normal state

            // Assert
            Assert.AreEqual(expected, result, $"Transition from emergency state {emergencyState} to {expectedReturnState} should be valid.");
        }

        [Test]
        public void SetCurrentState_InvalidTransition_ReturnsFalse()
        {
            // Arrange
            buildingController.SetCurrentState("closed"); // Set to a known normal state

            // Act
            var result = buildingController.SetCurrentState("fire alarm"); // Direct transition to emergency state is valid
            result &= buildingController.SetCurrentState("closed"); // Direct transition from emergency back to normal is invalid

            // Assert
            Assert.IsFalse(result, "Direct transition from emergency state back to normal should not be valid.");
        }

        [Test]
        public void SetCurrentState_UnrecognizedState_ReturnsFalse()
        {
            // Arrange
            var invalidState = "invalid";

            // Act
            var result = buildingController.SetCurrentState(invalidState);

            // Assert
            Assert.IsFalse(result, "Transition to an unrecognized state should return false.");
        }

    }

    [TestFixture]
    public class L2R2
    {
        private BuildingController buildingController;
        private readonly string initialID = "B123";

        [SetUp]
        public void SetUp()
        {
            // Setup for each test, buildingController starts in the "out of hours" state
            buildingController = new BuildingController(initialID);
        }

        [Test]
        public void SetCurrentState_SameState_ReturnsTrue()
        {
            // Arrange
            var initialState = "open";
            buildingController.SetCurrentState(initialState);

            // Act
            var result = buildingController.SetCurrentState(initialState);

            // Assert
            Assert.IsTrue(result, "Calling SetCurrentState with the current state should return true.");
        }

        [Test]
        public void SetCurrentState_SameState_KeepSameState()
        {
            // Arrange
            var initialState = "open";
            buildingController.SetCurrentState(initialState);

            // Act
            buildingController.SetCurrentState(initialState);
            var result = buildingController.GetCurrentState();


            // Assert
            Assert.AreEqual(result, initialState, "Calling SetCurrentState with the current state should return true.");
        }
    }

    [TestFixture]
    public class L2R3
    {
        [TestCase("B123", "closed")]
        [TestCase("B123", "CLOSED")]
        [TestCase("B123", "closed")]
        [TestCase("B123", "out of hours")]
        [TestCase("B123", "OUT OF HOURS")]
        [TestCase("B123", "open")]
        [TestCase("B123", "OPEN")]
        public void Constructor_WithValidStartState_InitializesCorrectly(string id, string startState)
        {
            // Act
            var buildingController = new BuildingController(id, startState);

            // Assert
            Assert.AreEqual(id.ToLower(), buildingController.GetBuildingID(), "Building ID should match the ID set in constructor and be in lower case.");
            Assert.AreEqual(startState.ToLower(), buildingController.GetCurrentState(), "Current state should match the start state set in constructor and be in lower case.");
        }

        [TestCase("B123", "invalid")]
        [TestCase("B123", "")]
        [TestCase("B123", " ")]
        [TestCase("B123", null)]
        public void Constructor_WithInvalidStartState_ThrowsArgumentException(string id, string startState)
        {
            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => new BuildingController(id, startState));
            Assert.That(ex.Message, Is.EqualTo("Argument Exception: BuildingController can only be initialised to the following states 'open', 'closed', 'out of hours'"), "Should throw ArgumentException for invalid start states.");
        }

    }
}

namespace L3
{
    [TestFixture]
    public class L3R1
    {
       
    }

    [TestFixture]
    public class L3R2
    {
        // NO NEED TO IMPLEMENT OR TEST
    }

    [TestFixture]
    public class L3R3
    {
        private LightManager _lightManagerSub;
        private FireAlarmManager _fireAlarmManagerSub;
        private DoorManager _doorManagerSub;
        private WebService _webServiceSub;
        private EmailService _emailServiceSub;

        [SetUp]
        public void SetUp()
        {
            _lightManagerSub = Substitute.For<LightManager>();
            _fireAlarmManagerSub = Substitute.For<FireAlarmManager>();
            _doorManagerSub = Substitute.For<DoorManager>();
            _webServiceSub = Substitute.For<WebService>();
            _emailServiceSub = Substitute.For<EmailService>();

            _lightManagerSub.GetStatus().Returns("Lights,OK,OK,FAULT,OK,OK,OK,OK,OK,OK,OK,");
            _doorManagerSub.GetStatus().Returns("Doors,OK,OK,OK,OK,OK,OK,OK,OK,");
            _fireAlarmManagerSub.GetStatus().Returns("FireAlarm,OK,OK,OK,OK,OK,OK,OK,OK,");

        }

        [Test]
        public void GetStatusReport_All3Managers_ReturnStatuses()
        {
            // Arrange
            var controller = new BuildingController("23", _lightManagerSub, _fireAlarmManagerSub, _doorManagerSub, _webServiceSub, _emailServiceSub);
            
            // Act
            var result = controller.GetStatusReport();
            
            // Assert
            Assert.AreEqual("Lights,OK,OK,FAULT,OK,OK,OK,OK,OK,OK,OK,Doors,OK,OK,OK,OK,OK,OK,OK,OK,FireAlarm,OK,OK,OK,OK,OK,OK,OK,OK,", result);
        }
    }

    [TestFixture]
    public class L3R4
    {
        private LightManager _lightManagerSub;
        private FireAlarmManager _fireAlarmManagerSub;
        private DoorManager _doorManagerSub;
        private WebService _webServiceSub;
        private EmailService _emailServiceSub;

        [SetUp]
        public void SetUp()
        {
            _lightManagerSub = Substitute.For<LightManager>();
            _fireAlarmManagerSub = Substitute.For<FireAlarmManager>();
            _doorManagerSub = Substitute.For<DoorManager>();
            _webServiceSub = Substitute.For<WebService>();
            _emailServiceSub = Substitute.For<EmailService>();
        }

        [Test]
        public void SetCurrentState_ToOpen_CallsOpenAllDoors_WhenDoorManagerIsNotNull()
        {
            // Arrange
            var buildingController = new BuildingController("23", _lightManagerSub, _fireAlarmManagerSub, _doorManagerSub, _webServiceSub, _emailServiceSub);

            // Act
            buildingController.SetCurrentState("open");

            // Assert
            _doorManagerSub.Received().OpenAllDoors();
        }

    }

    [TestFixture]
    public class L3R5
    {
        private LightManager _lightManagerSub;
        private FireAlarmManager _fireAlarmManagerSub;
        private DoorManager _doorManagerSub;
        private WebService _webServiceSub;
        private EmailService _emailServiceSub;

        [SetUp]
        public void SetUp()
        {
            _lightManagerSub = Substitute.For<LightManager>();
            _fireAlarmManagerSub = Substitute.For<FireAlarmManager>();
            _doorManagerSub = Substitute.For<DoorManager>();
            _webServiceSub = Substitute.For<WebService>();
            _emailServiceSub = Substitute.For<EmailService>();
        }


        [Test]
        public void SetCurrentState_ToOpen_WhenDoorsSuccessfullyOpen_ReturnsTrueAndChangesState()
        {
            // Arrange
            var _buildingController = new BuildingController("23", _lightManagerSub, _fireAlarmManagerSub, _doorManagerSub, _webServiceSub, _emailServiceSub);


            // Act
            var result = _buildingController.SetCurrentState("open");

            // Assert
            Assert.IsTrue(result, "SetCurrentState should return true when transitioning to 'open' is successful.");
            Assert.AreEqual("open", _buildingController.GetCurrentState(), "The building state should be 'open' after successfully opening doors.");
        }
    }
}

namespace L4
{
    [TestFixture]
    public class L4R1
    {


    }

    [TestFixture]
    public class L4R2
    {

    }

    [TestFixture]
    public class L4R3
    {

    }

    [TestFixture]
    public class L4R4
    {


    }
    
}