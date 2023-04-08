using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Doprez.Stride.AI.Common;

namespace Doprez.Stride.AI.BehaviourTree.Interfaces;
public interface ITreeNodeAction
{
	public string Name { get; set; }
	public TreeNode ExitNode { get; set; }

	public Task<ActionState> Execute();
	public TreeNode Success();
	public TreeNode Failure();
}
