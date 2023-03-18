using ReGoap.Core;

namespace ReGoap.Stride
{
    public class ReGoapAgentAdvanced<T, W> : ReGoapAgent<T, W>
    {
        public override void Update()
        {
            possibleGoalsDirty = true;

            if (currentActionState == null)
            {
                if (!IsPlanning)
                    CalculateNewGoal();
                return;
            }
        }
    }
}