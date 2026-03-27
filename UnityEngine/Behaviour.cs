namespace UnityEngine
{
	/// <summary>
	/// Behaviours are Components that can be enabled or disabled.
	/// Base class for MonoBehaviour in Unity's hierarchy.
	/// In this compatibility layer, Component already has enabled,
	/// so Behaviour simply extends Component for API compatibility.
	/// </summary>
	public class Behaviour : Component
	{
	}
}
