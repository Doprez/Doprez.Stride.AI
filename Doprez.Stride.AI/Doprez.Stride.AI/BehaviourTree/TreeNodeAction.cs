using Doprez.Stride.AI.BehaviourTree.Interfaces;
using Doprez.Stride.AI.Common;
using Stride.Core;
using Stride.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doprez.Stride.AI.BehaviourTree;

public abstract class TreeNodeAction : StartupScript, ITreeNodeAction
{
	[DataMember(0)]
	public string Name { get; set; }
	[DataMember(10)]
	public Dictionary<string, TreeNode> Outcomes { get; set; } = new();
	/// <summary>
	/// Can be set as a default in the editor but should be handled through code in <see cref="Execute()"/>
	/// </summary>
	[DataMember(11)]
	public TreeNode ExitNode { get; set; }

	public abstract Task<ActionState> Execute();

	/// <summary>
	/// Will be returned to the <see cref="TreeController"/> to set the next node upon action success
	/// </summary>
	/// <returns></returns>
	public virtual TreeNode Success()
	{
		if(ExitNode == null)
		{
			throw new Exception($"{nameof(TreeNodeAction)}'s {nameof(Execute)} cannot be null on success ");
		}
		return ExitNode;
	}

	/// <summary>
	/// Will be returned to the <see cref="TreeController"/> to set the next node upon action failure
	/// </summary>
	/// <returns></returns>
	public virtual TreeNode Failure()
	{
		return ExitNode;
	}
}
