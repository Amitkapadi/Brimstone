﻿using System;
using System.Collections;
using System.Collections.Generic;

namespace Brimstone
{
	public abstract class PowerAction
	{
		public int EntityId { get; set; }

		public PowerAction(Entity e) {
			EntityId = e.Id;
		}

		public override string ToString() {
			return "Entity: " + EntityId;
		}
	}

	public class TagChange : PowerAction
	{
		public GameTag Key { get; set; }
		public int? Value { get; set; }

		public TagChange(Entity e) : base(e) { }

		public override string ToString() {
			return "[Tag] Entity " + EntityId + ": " + Key.ToString() + " = " + Value;
		}
	}

	public class CreateEntity : PowerAction
	{
		public Dictionary<GameTag, int?> Tags { get; set; }

		public CreateEntity(Entity e) : base(e) {
			// Make sure we copy the tags, not the references!
			Tags = new Dictionary<GameTag, int?>(e.Tags);
		}

		public override string ToString() {
			return "[Create] " + base.ToString();
		}
	}

	public class PowerActionEventArgs : EventArgs
	{
		public Game Game;
		public PowerAction Action;

		public PowerActionEventArgs(Game g, PowerAction a) {
			Game = g;
			Action = a;
		}
	}

	public class PowerHistory : IEnumerable<PowerAction>
	{
		public Game Game { get; set; }
		public List<PowerAction> Log { get; } = new List<PowerAction>();

		public event EventHandler<PowerActionEventArgs> OnPowerAction;

		public void Add(PowerAction a) {
			// Ignore PowerHistory for untracked games
			if (Game == null)
				return;

			Log.Add(a);

			if (OnPowerAction != null)
				OnPowerAction(this, new PowerActionEventArgs(Game, a));
		}

		public IEnumerator<PowerAction> GetEnumerator() {
			return Log.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator() {
			return GetEnumerator();
		}
	}
}