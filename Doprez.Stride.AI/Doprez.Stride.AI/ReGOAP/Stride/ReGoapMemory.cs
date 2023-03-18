using ReGoap.Core;
using Stride.Engine;

namespace ReGoap.Stride
{
    public class ReGoapMemory<T, W> : SyncScript, IReGoapMemory<T, W>
    {
        protected ReGoapState<T, W> state;

        protected virtual void OnDestroy()
        {
            state.Recycle();
        }

		public override void Start()
		{
			state = ReGoapState<T, W>.Instantiate();
		}

		public override void Update()
		{
			
		}

		public virtual ReGoapState<T, W> GetWorldState()
        {
            return state;
        }
    }
}
