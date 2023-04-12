# Doprez.Stride
AI libraries to be used within [Stride3d](https://www.stride3d.net/) games 

# Build Status
[![CircleCI](https://dl.circleci.com/status-badge/img/gh/Doprez/Doprez.Stride.AI/tree/master.svg?style=svg)](https://dl.circleci.com/status-badge/redirect/gh/Doprez/Doprez.Stride.AI/tree/master)

# Examples
## FSM Example

### We can start by inheriting from the FSM and implementing the base methods
```
using Doprez.Stride.AI.FSMs;

public class BasicEnemyFSM : FSM
{
	public override void UpdateFSM()
	{
		//You can add triggers here to cancel/change the current running state
	}
}
```

### Create the State and needed methods
```
public class IdleState : FSMState
{
	public IdleState(FSM fsm)
	{
		// set the FSM to access the methods within the running state
		FiniteStateMachine = fsm;

		// add this state to the FSM for other states to see this state
		FiniteStateMachine.States.Add(0, this);
	}

	public override void EnterState()
	{
		// activate needed events or variables when this state is started
	}

	public override void ExitState()
	{
		// deactivate needed events or variables when this state is finished
	}

	public override void UpdateState()
	{
		// this runs in the Update method of Stride per frame
	}

}
```

## A more complete example

you can add the  Doprez.Stride nuget package if some of the references are missing

### the FSM to initialize the state and allow per frame updates
```
using System;
using Doprez.Stride.AI.FSMs;
using Stride.Core;
using Stride.Engine;
using StridePlatformer.States;
using Navigation;

public class BasicEnemyFSM : FSM
{
	[Display("Player Entity")]
	[DataMember(0)]
	public Entity Player{get;set;}

	[Display("Player Seen Trigger")]
	[DataMember(2)]
	public PhysicsComponent PlayerSeenTrigger;

	[Display("Animation Component")]
	[DataMember(20)]
	public AnimationComponent AnimationComponent { get; set; }

	//States
	//private MoveToState _moveTo;
	private IdleState _idle;


	public override void Start()
	{
		InitializeStates();

		SetCurrentState(_idle);
	}

	private void InitializeStates()
	{
		_idle = new IdleState(this, AnimationComponent);// ", _moveTo" add this if you added a moveTo state
		_idle.PlayerSeenTrigger = PlayerSeenTrigger;
	}

	public override void UpdateFSM()
	{
		// add some logic here if needed but it can just be blank
	}
}
```

### An example Idle state with comments on how to change to another state
```
using System.Collections.Specialized;
using System.Threading.Tasks;
using Doprez.Stride.AI.FSMs;
using Stride.Core.Collections;
using Stride.Engine;
using Stride.Physics;

public class IdleState : FSMState
{

	public PhysicsComponent PlayerSeenTrigger;

	private readonly AnimationComponent _animationComponent;
	//add a state for you to change to like the example below
	//private readonly MoveToState _moveTo;

	public IdleState(FSM fsm, AnimationComponent animationComponent)// , MoveToState moveTo
	{
		FiniteStateMachine = fsm;
		_animationComponent = animationComponent;
		//_moveTo = moveTo;

		FiniteStateMachine.States.Add((int)EnemyStates.Idle, this);
	}

	public override void EnterState()
	{
		//Only activate this trigger while the state is running
		PlayerSeenTrigger.Enabled = true;
		PlayerSeenTrigger.Collisions.CollectionChanged += CollisionsChanged;

		//run the idle anim if you have one
		//_animationComponent.Play("Idle");
	}

	public override void ExitState()
	{
		// stop using the trigger when the state isnt running
		PlayerSeenTrigger.Enabled = false;
		PlayerSeenTrigger.Collisions.CollectionChanged -= CollisionsChanged;
	}

	public override void UpdateState()
	{
		
	}

	private void CollisionsChanged(object sender, TrackingCollectionChangedEventArgs args)
	{
		// Cast the argument 'Item' to a collision object
		var collision = (Collision)args.Item;

		// We need to make sure which collision object is not the Trigger collider
		// We perform a little check to find the ballCollider 
		var playerCollider = PlayerSeenTrigger == collision.ColliderA ? collision.ColliderB : collision.ColliderA;

		if (args.Action == NotifyCollectionChangedAction.Add)
		{
			// When a collision has been added to the collision collection, we know an object has 'entered' our trigger
			if (playerCollider.Entity.Name == "PlayerCharacter")
			{
				//get the entity that entered the trigger
				var entitycollided = playerCollider.Entity;
				//you can change the state by direct reference or the dictionary Id
				//_moveTo.Target = entitycollided;
				// change to a direct reference
				//FiniteStateMachine.SetCurrentState(_moveTo);
				// change based on a dictionary reference I would recommend setting up and Enum for readability
				//FiniteStateMachine.SetCurrentState(1);
			}
		}
	}
	
}
```
