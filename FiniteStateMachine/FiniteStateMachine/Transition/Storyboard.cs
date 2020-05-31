using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiniteStateMachine.Transition
{
	public class Storyboard
    {
		readonly Dictionary<string, ViewTransition[]> _stateTransitions = new Dictionary<string, ViewTransition[]>();

		public void Add(object state, ViewTransition[] viewTransitions)
		{
			var stateStr = state?.ToString().ToUpperInvariant();

			if (string.IsNullOrEmpty(stateStr) || viewTransitions == null)
				throw new NullReferenceException("Value of 'state', 'viewTransitions' cannot be null");

			if (_stateTransitions.ContainsKey(stateStr))
				throw new ArgumentException($"State {state} already added");

			_stateTransitions.Add(stateStr, viewTransitions);
		}

		public void Go(object newState, bool withAnimation = true)
		{
			var newStateStr = newState?.ToString().ToUpperInvariant();

			if (string.IsNullOrEmpty(newStateStr))
				throw new NullReferenceException("Value of newState cannot be null");

			if (!_stateTransitions.ContainsKey(newStateStr))
				throw new KeyNotFoundException($"There is no state {newState}");

			// Get all ViewTransitions 
			var viewTransitions = _stateTransitions[newStateStr];

			// Get transition tasks
			var tasks = viewTransitions.Select(viewTransition => viewTransition.GetTransition(withAnimation));

			// Run all transition tasks
			Task.WhenAll(tasks);
		}
	}
}
