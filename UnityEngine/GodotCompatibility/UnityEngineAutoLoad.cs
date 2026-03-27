using Godot;
using System.Collections.Generic;

namespace UnityEngine
{
	public partial class UnityEngineAutoLoad : Node
	{
		private static UnityEngineAutoLoad? _instance;

		public static UnityEngineAutoLoad? Instance
		{
			get
			{
				if (_instance == null)
					_instance = new UnityEngineAutoLoad();
				return _instance;
			}
		}

		public UnityEngineAutoLoad()
		{
			_instance = this;
		}

		public override void _Ready()
		{
			GetTree().NodeAdded += _on_node_added;
		}

		private void _on_node_added(Node node)
		{
			// MonoBehaviour nodes handle their own lifecycle via _Ready
		}

		public override void _Process(double delta)
		{
			Time.time += (float)delta;
			Time.deltaTime = (float)delta;
		}

		public override void _PhysicsProcess(double delta)
		{
			Time.fixedDeltaTime = (float)delta;
		}

		public void Quit()
		{
			GetTree()?.Quit();
		}
	}
}
