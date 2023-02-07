using Stride.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doprez.Stride.AI.GOAP;
/// <summary>
/// Used to contain all possible conditions/states to switch between actions
/// </summary>
public class GoapState : StartupScript
{
	public Dictionary<string, bool> ConditionStates = new();
}
