﻿using System;
using System.Collections.Generic;

namespace Brimstone
{
	public static class RNG {
		private static Random random = new Random();

		public static int Between(int min, int max) {
			return random.Next(min, max + 1);
		}
	}

	public static class RNG<T> {
		public static T Choose(List<T> choices) {
			return choices[RNG.Between(0, choices.Count - 1)];
		}
	}

	public class Choice
	{
		public ChoiceType ChoiceType { get; set; }
		public List<IEntity> Choices { get; set; }

		// TODO: Implement
	}

	public static class DebugLog {
		private static object _lock = new object();
		public static void WriteLine(string s, params object[] p) {
#if DEBUG
			lock (_lock)
				using (var sr = new System.IO.StreamWriter("debug.txt", true))
					sr.WriteLine(
						string.Format("[{0}] Thread {1}: ",
							DateTime.Now.ToString("HH:mm:ss.fff"),
							System.Threading.Thread.CurrentThread.ManagedThreadId)
						+ s, p);
#endif
		}
	}
}