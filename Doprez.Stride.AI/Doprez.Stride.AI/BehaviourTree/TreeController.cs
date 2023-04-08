using Stride.Core;
using Stride.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doprez.Stride.AI.BehaviourTree;
[DataContract(nameof(TreeController))]
[Display("TreeController")]
public class TreeController : StartupScript
{
	public List<TreeNode> Nodes { get; set; } = new();
	public TreeNode DefaultFallbackNode { get; set; }

	private TreeNode _currentNode;

	public override void Start()
	{
		base.Start();
		SetNodeParents();
		SetFallBackNode();
	}

	public async Task RunAction()
	{
		await _currentNode.RunNodeAction();
	}

	public void SetCurrentNode(TreeNode treeNode)
	{
		_currentNode = treeNode;
	}

	private void SetNodeParents()
	{
		foreach (TreeNode node in Nodes)
		{
			node.SetParent(this);
		}
	}

	private void SetFallBackNode()
	{
		if(Nodes.Contains(DefaultFallbackNode))
		{
			_currentNode = DefaultFallbackNode;
			return;
		}

		DefaultFallbackNode = Nodes.FirstOrDefault(x => x.IsRootNode);

		if(DefaultFallbackNode == null)
		{
			throw new Exception($"{nameof(TreeController)} Requires {nameof(DefaultFallbackNode)} to not be null");
		}

		_currentNode = DefaultFallbackNode;
	}
}
