#region ReSharper
// ReSharper disable CompareOfFloatsByEqualityOperator
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
#endregion

using UnityEngine;
using VisualPinball.Engine.VPT.Gate;
using VisualPinball.Unity.Extensions;

namespace VisualPinball.Unity.VPT.Gate
{
	[AddComponentMenu("Visual Pinball/Gate")]
	public class GateBehavior : ItemBehavior<Engine.VPT.Gate.Gate, GateData>
	{
		protected override string[] Children => new []{"Wire", "Bracket"};

		protected override Engine.VPT.Gate.Gate GetItem()
		{
			return new Engine.VPT.Gate.Gate(data);
		}

		public override ItemDataTransformType EditorPositionType => ItemDataTransformType.ThreeD;
		public override Vector3 GetEditorPosition()
		{
			return data.Center.ToUnityVector3(data.Height);
		}
		public override void SetEditorPosition(Vector3 pos)
		{
			data.Center = pos.ToVertex2Dxy();
			data.Height = pos.z;
			transform.localPosition = data.Center.ToUnityVector3(data.Height);
		}

		public override ItemDataTransformType EditorRotationType => ItemDataTransformType.OneD;
		public override Vector3 GetEditorRotation()
		{
			return new Vector3(data.Rotation, 0f, 0f);
		}
		public override void SetEditorRotation(Vector3 rot)
		{
			data.Rotation = rot.x;
			transform.localEulerAngles = new Vector3(0f, 0f, rot.x);
		}

		public override ItemDataTransformType EditorScaleType => ItemDataTransformType.OneD;
		public override Vector3 GetEditorScale()
		{
			return new Vector3(data.Length, 0f, 0f);
		}
		public override void SetEditorScale(Vector3 scale)
		{
			data.Length = scale.x;
			transform.localScale = new Vector3(data.Length, data.Length, data.Length);
		}
	}
}
