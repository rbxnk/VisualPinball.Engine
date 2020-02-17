using System.Collections.Generic;
using System.Linq;
using VisualPinball.Engine.Game;
using VisualPinball.Engine.VPT.Ball;

namespace VisualPinball.Engine.Physics
{
	public class HitKd
	{
		public List<int> OrgIdx; // m_org_idx
		public int NumNodes;
		public int[] tmp;

		private readonly HitKdNode _rootNode;
		private HitKdNode[] _nodes;
		private List<HitObject> _orgHitObjects; // m_org_vho

		private int _numItems;
		private int _maxItems;

		public HitKd()
		{
			_rootNode = new HitKdNode(this);
		}

		public void Init(List<HitObject> vho)
		{
			_orgHitObjects = vho;
			_numItems = vho.Count;

			if (_numItems > _maxItems) {
				_maxItems = _numItems;

				OrgIdx = new List<int>(_numItems);

				tmp = new int[_numItems];
				_nodes = new HitKdNode[(_numItems * 2 + 1) & ~1u];
			}

			NumNodes = 0;
			_rootNode.Reset(this);
		}

		public void FillFromVector(List<HitObject> vho)
		{
			Init(vho);

			_rootNode.RectBounds.Clear();
			_rootNode.Start = 0;
			_rootNode.Items = _numItems;

			for (var i = 0; i < _numItems; ++i) {
				var pho = vho[i];
				pho.CalcHitBBox(); //!! omit, as already calced?!
				_rootNode.RectBounds.Extend(pho.HitBBox);
				OrgIdx[i] = i;
			}

			_rootNode.CreateNextLevel(0, 0);
		}

		// call when the bounding boxes of the HitObjects have changed to update the tree
		public void Update()
		{
			FillFromVector(_orgHitObjects);
		}

		// call when finalizing a tree (no dynamic changes planned on it)
		public void Finalize()
		{
			tmp = null;
		}

		public void HitTestBall(Ball ball, CollisionEvent collision, PlayerPhysics physics)
		{
			_rootNode.HitTestBall(ball, collision, physics);
		}

		public HitObject GetItemAt(int i)
		{
			return _orgHitObjects[OrgIdx[i]];
		}

		public HitKdNode[] AllocTwoNodes()
		{
			if (NumNodes + 1 >= _nodes.Length) {
				// space for two more nodes?
				return null;
			}
			NumNodes += 2;
			return _nodes.Take(NumNodes - 2).ToArray();
		}
	}
}