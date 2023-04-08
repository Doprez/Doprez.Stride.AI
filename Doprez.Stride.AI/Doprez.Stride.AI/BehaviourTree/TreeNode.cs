using Doprez.Stride.AI.Common;
using Stride.Core;
using Stride.Engine;
using System.Threading.Tasks;

namespace Doprez.Stride.AI.BehaviourTree;

[DataContract(nameof(TreeNode))]
[Display("Node")]
public class TreeNode : StartupScript
{
	[DataMember(0)]
	public bool IsRootNode { get; set; } = false;
	[DataMember(1)]
	public string Name { get; set; }
	[DataMember(10)]
	public TreeNodeAction Action { get; set; }

	[DataMemberIgnore]
	public TreeController Parent { get; set; }

	public void SetParent(TreeController parent)
	{
		Parent = parent;
	}

	public async Task RunNodeAction()
	{
		var state = await Action.Execute();
		switch (state)
		{
			case ActionState.Success:
				Parent.SetCurrentNode(Action.Success());
				break;
			case ActionState.Failure:
				Parent.SetCurrentNode(FailureNode());
				break;
			case ActionState.Impossible:
				Parent.SetCurrentNode(Parent.DefaultFallbackNode);
				break;
			case ActionState.Running:
				break;
			default:
				break;
		}
	}

	private TreeNode FailureNode()
	{
		var node = Action.Failure();
		if(node != null)
		{
			return node;
		}

		return Parent.DefaultFallbackNode;
	}

}
