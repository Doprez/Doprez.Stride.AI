namespace Doprez.Stride.AI.Utility;

/// <summary>
/// <see cref="UtilityFunctions"/> are used to evaluate the desirability of different actions based on the current state of the agent and its environment.
/// </summary>
public class UtilityFunctions
{
	/// <summary>
	/// A simple linear function that increases or decreases utility based on a single variable.
	/// </summary>
	/// <param name="value"></param>
	/// <param name="slope"></param>
	/// <param name="intercept"></param>
	/// <returns></returns>
	public static float Linear(float value, float slope, float intercept)
	{
		return slope * value + intercept;
	}

	/// <summary>
	/// A function where utility decreases as the value increases, often used for resources like health or stamina.
	/// </summary>
	/// <param name="value"></param>
	/// <param name="maxValue"></param>
	/// <returns></returns>
	public static float Inverse(float value, float maxValue)
	{
		return maxValue - value;
	}

	/// <summary>
	/// A function that increases utility exponentially, useful for actions that become more desirable quickly as conditions change.
	/// </summary>
	/// <param name="value"></param>
	/// <param name="baseValue"></param>
	/// <returns></returns>
	public static float Exponential(float value, float baseValue)
	{
		return (float)Math.Pow(baseValue, value);
	}

	/// <summary>
	/// A function that increases utility logarithmically, useful for diminishing returns scenarios.
	/// </summary>
	/// <param name="value"></param>
	/// <param name="baseValue"></param>
	/// <returns></returns>
	public static float Logarithmic(float value, float baseValue)
	{
		return (float)Math.Log(value + 1, baseValue); // +1 to avoid log(0)
	}

	/// <summary>
	/// A function that provides a smooth transition between low and high utility, useful for actions that should gradually become more desirable.
	/// </summary>
	/// <param name="value"></param>
	/// <param name="steepness"></param>
	/// <param name="midpoint"></param>
	/// <returns></returns>
	public static float Sigmoid(float value, float steepness, float midpoint)
	{
		return 1 / (1 + (float)Math.Exp(-steepness * (value - midpoint)));
	}
}
