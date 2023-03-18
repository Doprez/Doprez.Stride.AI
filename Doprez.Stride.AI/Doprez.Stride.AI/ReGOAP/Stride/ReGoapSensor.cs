using ReGoap.Core;
using Stride.Engine;

namespace ReGoap.Stride
{
    public class ReGoapSensor<T, W> : SyncScript, IReGoapSensor<T, W>
    {
        protected IReGoapMemory<T, W> memory;
        public virtual void Init(IReGoapMemory<T, W> memory)
        {
            this.memory = memory;
        }

        public virtual IReGoapMemory<T, W> GetMemory()
        {
            return memory;
        }

        public virtual void UpdateSensor()
        {

        }

		public override void Update()
		{

		}
	}
}
