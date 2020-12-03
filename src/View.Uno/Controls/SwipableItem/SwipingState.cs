using System;
using System.Collections.Generic;
using System.Text;

namespace Chinook.View.Controls
{
	public enum SwipingState
	{
		NotSwiped,
		SwipingNear,
		SwipedNear,
		LongSwipingNear,
		LongSwipedNear,
		SwipingFar,
		SwipedFar,
		LongSwipingFar,
		LongSwipedFar
	}
}
