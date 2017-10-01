﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using FiniteAutomota.NonDeterministic.Builder;
using FiniteAutomota.NonDeterministic.Builder.Exceptions;
using FiniteAutomota.NonDeterministic.Closure;

namespace FiniteAutomata.NonDeterministic.Test
{
    [TestClass]
    public class NonDeterministicFiniteAutomatonBuilderTest
    {
        private const string Start = "start";
        private const string Source = "source";
        private const string Target = "target";
        private const string Closure = "closure";
        
        public AutomatonBuilder _sutWithRealImplementors;

        [TestInitialize]
        public void TestInitialize()
        {
            _sutWithRealImplementors = new AutomatonBuilder(new ClosureCalculator());
        } 

        [TestMethod]
        public void CreatedAutomaton_WithMultipleStates_StartStateActive()
        {
            var automaton = _sutWithRealImplementors
                .State(Start).ActiveAtStart()
                .State(Source)
                .Build();

            var currentState = automaton.GetActiveStates();
            Assert.AreEqual(1, currentState.Count());
            Assert.AreEqual(Start, currentState.ElementAt(0).Description);
        }
        
        [TestMethod]
        [ExpectedException(typeof(AtLeastOneStartStateRequiredException))]
        public void CreatedAutomaton_NoStartStateSet_ThrowsNoStartStateException_OnBuild()
        {
            var automaton = _sutWithRealImplementors
                .State(Source)
                .State(Target)
                .Transition().OnEpsilon().From(Source).To(Target)
                .Build();
        }

        [TestMethod]
        public void CreatedAutomaton_StartStateHasNoEpsilonTransition_OnlyStartStateActive()
        {
            var automaton = _sutWithRealImplementors
                .State(Start).ActiveAtStart()
                .State(Source)
                .State(Target)
                .Transition().OnEpsilon().From(Source).To(Target)
                .Build();

            var currentState = automaton.GetActiveStates();
            Assert.AreEqual(1, currentState.Count());
            Assert.AreEqual(Start, currentState.ElementAt(0).Description);
        }

        [TestMethod]
        public void CreatedAutomaton_StartStateHasEpsilonTransition_BothStatesActive()
        {
            var automaton = _sutWithRealImplementors
                .State(Start).ActiveAtStart()
                .State(Target)
                .Transition().OnEpsilon().From(Start).To(Target)
                .Build();
            
            var currentState = automaton.GetActiveStates();
            Assert.AreEqual(2, currentState.Count());
            Assert.AreEqual(Start, currentState.ElementAt(0).Description);
            Assert.AreEqual(Target, currentState.ElementAt(1).Description);
        }
        
        [TestMethod]
        [ExpectedException(typeof(UndefinedStateException))]
        public void CreatedAutomaton_AddTransitionForUnknownTarget_UndefinedStateExceptionThrown()
        {
            var automaton = _sutWithRealImplementors
                .State(Start).ActiveAtStart()
                .Transition().OnEpsilon().From(Start).To(Target)
                .Build();
        }
    }
}
