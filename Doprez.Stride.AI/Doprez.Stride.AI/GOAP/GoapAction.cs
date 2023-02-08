using Stride.Engine;

namespace Doprez.Stride.AI.GOAP;
public abstract class GoapAction : StartupScript
{
	public string Name = Guid.NewGuid().ToString();
	public int Cost;
	public Dictionary<string, bool> PreConditions = new();
	public Dictionary<string, bool> PostConditions = new();
	public bool IsUsable;

	/// <summary>
	/// Returns true if the action was successful
	/// </summary>
	/// <returns></returns>
	public abstract Task<bool> RunAction();

	public void Reset()
	{
		IsUsable = false;
	}

}
