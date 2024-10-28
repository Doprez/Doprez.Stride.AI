using Stride.Core;

namespace Doprez.Stride.AI.Utility;
[DataContract(Inherited = true)]
public abstract class UtilityAction
{
	/// <summary>
	/// Takes in the agent and calculates the utility of the action.
	/// </summary>
	/// <param name="agent"></param>
	/// <returns></returns>
	public abstract float CalculateUtility(UtilityAgent agent);

	/// <summary>
	/// Executes the action and returns the state of the action.
	/// </summary>
	/// <param name="agent"></param>
	/// <returns></returns>
	public abstract ActionState Execute(UtilityAgent agent);

	/// <summary>
	/// Cancels the action and runs when the Actions has changed before completing.
	/// </summary>
	/// <param name="agent"></param>
	public abstract void CancelAction(UtilityAgent agent);
}
