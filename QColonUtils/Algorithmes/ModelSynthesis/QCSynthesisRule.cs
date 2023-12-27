using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QColonUtils.Algorithmes.ModelSynthesis
{
    public class QCSynthesisRule<T>
    {
        public T RequiredNeighbour { get; set; }
        public List<StateProbabilityPair> AllowedStates { get; set; }

        public QCSynthesisRule(T requiredNeighbor)
        {
            RequiredNeighbour = requiredNeighbor;
            AllowedStates = new List<StateProbabilityPair>();
        }

        public void AddAllowedState(T state, double probability)
        {
            AllowedStates.Add(new StateProbabilityPair(state, probability));
        }

        public class StateProbabilityPair
        {
            public T State { get; set; }
            public double Probability { get; set; }

            public StateProbabilityPair(T state, double probability)
            {
                State = state;
                Probability = probability;
            }
        }
    }

}
