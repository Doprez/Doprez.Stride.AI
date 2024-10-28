using Doprez.Stride.AI.FiniteStateMachine;
using Moq;
using Xunit;

namespace Doprez.Stride.AI.Tests;
public class FSMTests
{
	[Fact]
	public void SetCurrentState_ShouldSetStateAndCallEnterState()
	{
		// Arrange
		var fsm = new FSM();
		var mockState = new Mock<FSMState>();

		// Act
		fsm.SetCurrentState(mockState.Object);

		// Assert
		Assert.Equal(mockState.Object, fsm.CurrentState);
		mockState.Verify(state => state.EnterState(), Times.Once);
	}

	[Fact]
	public void SetCurrentState_ShouldCallExitStateOnPreviousState()
	{
		// Arrange
		var fsm = new FSM();
		var mockState1 = new Mock<FSMState>();
		var mockState2 = new Mock<FSMState>();

		fsm.SetCurrentState(mockState1.Object);

		// Act
		fsm.SetCurrentState(mockState2.Object);

		// Assert
		mockState1.Verify(state => state.ExitState(), Times.Once);
		mockState2.Verify(state => state.EnterState(), Times.Once);
	}

	[Fact]
	public void Execute_ShouldCallExecuteStateOnCurrentState()
	{
		// Arrange
		var fsm = new FSM();
		var mockState = new Mock<FSMState>();

		fsm.SetCurrentState(mockState.Object);

		// Act
		fsm.Execute();

		// Assert
		mockState.Verify(state => state.ExecuteState(), Times.Once);
	}

	[Fact]
	public void CancelCurrentState_ShouldCallExitStateAndSetCurrentStateToNull()
	{
		// Arrange
		var fsm = new FSM();
		var mockState = new Mock<FSMState>();

		fsm.SetCurrentState(mockState.Object);

		// Act
		fsm.CancelCurrentState();

		// Assert
		mockState.Verify(state => state.ExitState(), Times.Once);
		Assert.Null(fsm.CurrentState);
	}
}
