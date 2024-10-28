using Doprez.Stride.AI.Utility;
using Moq;
using Xunit;
using System.Collections.Generic;

namespace Doprez.Stride.AI.Tests;
public class UtilityAgentTests
{
	private class TestUtilityAgent : UtilityAgent
	{
		public TestUtilityAgent()
		{
			AgentState = new Dictionary<string, float>();
			Actions = new List<UtilityAction>();
		}
	}

	[Fact]
	public void GetBestAction_ShouldReturnActionWithHighestUtility()
	{
		// Arrange
		var agent = new TestUtilityAgent();
		var mockAction1 = new Mock<UtilityAction>();
		var mockAction2 = new Mock<UtilityAction>();

		mockAction1.Setup(a => a.CalculateUtility(agent)).Returns(10);
		mockAction2.Setup(a => a.CalculateUtility(agent)).Returns(20);

		agent.Actions.Add(mockAction1.Object);
		agent.Actions.Add(mockAction2.Object);

		// Act
		var bestAction = agent.GetBestAction();

		// Assert
		Assert.Equal(mockAction2.Object, bestAction);
	}

	[Fact]
	public void PerformBestAction_ShouldReturnFailureIfNoActionAvailable()
	{
		// Arrange
		var agent = new TestUtilityAgent();

		// Act
		var result = agent.PerformBestAction();

		// Assert
		Assert.Equal(ActionState.Failure, result);
	}

	[Fact]
	public void PerformBestAction_ShouldExecuteCurrentAction()
	{
		// Arrange
		var agent = new TestUtilityAgent();
		var mockAction = new Mock<UtilityAction>();

		mockAction.Setup(a => a.CalculateUtility(agent)).Returns(10);
		mockAction.Setup(a => a.Execute(agent)).Returns(ActionState.Running);

		agent.Actions.Add(mockAction.Object);
		agent.CheckIfBestActionChanged();

		// Act
		var result = agent.PerformBestAction();

		// Assert
		Assert.Equal(ActionState.Running, result);
		mockAction.Verify(a => a.Execute(agent), Times.Once);
	}

	[Fact]
	public void CheckIfBestActionChanged_ShouldUpdateCurrentAction()
	{
		// Arrange
		var agent = new TestUtilityAgent();
		var mockAction1 = new Mock<UtilityAction>();
		var mockAction2 = new Mock<UtilityAction>();

		mockAction1.Setup(a => a.CalculateUtility(agent)).Returns(10);
		mockAction2.Setup(a => a.CalculateUtility(agent)).Returns(20);

		agent.Actions.Add(mockAction1.Object);
		agent.Actions.Add(mockAction2.Object);

		// Act
		var hasChanged = agent.CheckIfBestActionChanged();

		// Assert
		Assert.True(hasChanged);
		Assert.Equal(mockAction2.Object, agent.CurrentAction);
	}

	[Fact]
	public void CheckIfBestActionChanged_ShouldCancelPreviousAction()
	{
		// Arrange
		var agent = new TestUtilityAgent();
		var mockAction1 = new Mock<UtilityAction>();
		var mockAction2 = new Mock<UtilityAction>();

		mockAction1.Setup(a => a.CalculateUtility(agent)).Returns(10);
		mockAction2.Setup(a => a.CalculateUtility(agent)).Returns(20);

		agent.Actions.Add(mockAction1.Object);
		agent.Actions.Add(mockAction2.Object);

		agent.CheckIfBestActionChanged(); // Set initial best action

		// Act
		mockAction2.Setup(a => a.CalculateUtility(agent)).Returns(5); // Change utility to force a change
		var hasChanged = agent.CheckIfBestActionChanged();

		// Assert
		Assert.True(hasChanged);
		mockAction2.Verify(a => a.CancelAction(agent), Times.Once);
	}
}
