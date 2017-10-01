﻿using FiniteAutomota.NonDeterministic.Closure;
using System.Collections.Generic;
using System.Linq;

namespace FiniteAutomota.NonDeterministic
{
    public class Automaton<Descriptor, Symbol>
    {
        private List<State<Descriptor, Symbol>> CurrentStates;
        private readonly IClosureCalculator _closureCalculator;
        
        public Automaton(IEnumerable<State<Descriptor, Symbol>> startStates, IClosureCalculator closureCalculator)
        {
            CurrentStates = new List<State<Descriptor, Symbol>>();
            CurrentStates.AddRange(startStates);

            _closureCalculator = closureCalculator;
        }

        public IEnumerable<State<Descriptor, Symbol>> GetActiveStates()
        {
            return CurrentStates;
        }

        public void Process(Symbol symbol)
        {
            var nextStates = CurrentStates
                .Select(state => state.GetTransitionsFor(symbol))
                .SelectMany(state => state)
                .Distinct();

            CurrentStates = _closureCalculator.GetClosureFor(nextStates);
        }
    }

    public class Automaton : Automaton<string, char>
    {
        public Automaton(IEnumerable<State<string, char>> startStates, IClosureCalculator closureCalculator) : base(startStates, closureCalculator)
        {
        }
    }
}
