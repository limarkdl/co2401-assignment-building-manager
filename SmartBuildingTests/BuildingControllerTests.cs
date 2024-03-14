using NUnit.Framework;
using SmartBuilding;

namespace L1_done
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
    public class L2R2
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
    public class L2R3
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
}

namespace L3
{
    [TestFixture]
    public class L3R1
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
    public class L3R2
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
    public class L3R3
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
    public class L3R4
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
    public class L3R5
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
}