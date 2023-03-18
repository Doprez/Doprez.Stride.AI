using ReGoap.Core;
using Stride.Engine;

namespace ReGoap.Stride
{
    public class ReGoapMemoryAdvanced<T, W> : ReGoapMemory<T, W>
    {
        private IReGoapSensor<T, W>[] sensors;

        public float SensorsUpdateDelay = 0.3f;
        private float sensorsUpdateCooldown;

        public override void Start()
        {
            base.Start();
            sensors = Entity.GetComponents<IReGoapSensor<T, W>>().ToArray();
            foreach (var sensor in sensors)
            {
                sensor.Init(this);
            }
        }

        protected virtual void Update()
        {
            if (Game.TargetElapsedTime.Milliseconds > sensorsUpdateCooldown)
            {
                sensorsUpdateCooldown = Game.TargetElapsedTime.Milliseconds + SensorsUpdateDelay;

                foreach (var sensor in sensors)
                {
                    sensor.UpdateSensor();
                }
            }
        }
    }
}
