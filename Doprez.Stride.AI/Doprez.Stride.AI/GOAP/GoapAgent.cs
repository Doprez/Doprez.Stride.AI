using Stride.Core;
using Stride.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doprez.Stride.AI.GOAP;
public abstract class GoapAgent : AsyncScript
{
	[DataMember(0)]
	public string AgentName { get; set; } = string.Empty;

	//Need list of Actions To be performed
	public List<GoapAction> Actions { get; set; } = new();

	//need input state that should be modified
	public GoapState State { get; set; } = new();

}
